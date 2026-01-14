using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DuplicatesFinder
{
    public partial class FormResults : Form
    {
        private List<string> _pathList;
        private FileCompareMethod _fileCompareMethod;
        private bool _isCancelled = false;
        private bool _isFinishedScanning = false;
        private bool _isFinishedUpdate = false;
        private readonly ConcurrentQueue<List<FileCompareGroup>> _updateQueue = new();
        private readonly OptimizedGroupList _cachedGroupList = new();
        private ProgressBarData _progressBarData;
        private ElapsedTimeWatcher _elapsedTimeWatcher;

        internal FormResults(List<string> pathList, FileCompareMethod fileCompareMethod)
        {
            InitializeComponent();
            _pathList = pathList;
            _fileCompareMethod = fileCompareMethod;
            _progressBarData = new(labelProgressBar);
            _elapsedTimeWatcher = new(labelSearchTime);
            timerUpdate.Enabled = true;
        }

        private void FormResults_Shown(object sender, EventArgs e)
        {
            Task.Run(() => StartSearch());
        }

        private async Task StartSearch()
        {
            _isCancelled = false;
            _isFinishedScanning = false;
            _isFinishedUpdate = false;

            _elapsedTimeWatcher.Reset();
            _elapsedTimeWatcher.Start();

            await SearchEngineManager.Search(_pathList);

            List<long> uniqueSizesList = Globals.DB.GetUniqueSizes();

            _progressBarData.CurrentValue = 0;
            _progressBarData.Total = uniqueSizesList.Count();

            Parallel.ForEach(uniqueSizesList, (uniqueSize, state) =>
            {
                if (_isCancelled)
                {
                    state.Stop();
                }
                List<string> paths = Globals.DB.GetFilePathsBySize(uniqueSize);
                var groupList = new FileCompare(_fileCompareMethod).GetIdenticalFileGroups(paths);
                if (groupList != null)
                {
                    _updateQueue.Enqueue(groupList);
                }
                _progressBarData.Increment();
            });

            _elapsedTimeWatcher.Stop();
            _elapsedTimeWatcher.Print();
            _isFinishedScanning = true;
            this.BeginInvoke(() => ConsumeWorkResults());
        }

        private void AppendGroupToResult(FileCompareGroup fileGroup)
        {
            bool isFirstItem = true;
            ListViewGroup lvGroup = new(CreateGroupName(fileGroup));
            listViewResult.Groups.Add(lvGroup);

            for (int i = fileGroup.MetadataList.Count - 1; i >= 0; i--)
            {
                FileCompareMetadata meta = fileGroup.MetadataList[i];

                ListViewItem lvi = new(meta.FilePath.Substring(4), lvGroup); // remove "\\?\"
                lvi.SubItems.Add(fileGroup.MetadataList.Count.ToString());
                lvi.SubItems.Add(meta.Size.ToString());
                lvi.Checked = !isFirstItem;

                listViewResult.Items.Add(lvi);
                isFirstItem = false;
            }
        }

        private string CreateGroupName(FileCompareGroup fileGroup)
        {
            var firstFileMetadata = fileGroup.MetadataList[0];
            return $"({FileSizeFormatter.Format(firstFileMetadata.Size)}) {firstFileMetadata.FileName}";
        }

        private void buttonCancelSearch_Click(object sender, EventArgs e)
        {
            Globals.SearchThreadMan.CancelTasks();
            buttonCancelSearch.Enabled = false;
            _isCancelled = true;
        }

        private void buttonRemoveFiles_Click(object sender, EventArgs e)
        {
            List<string> selectedPathList = listViewResult.CheckedItems.Cast<ListViewItem>()
                .Select(item => item.Text)
                .ToList();

            int countSuccess = 0;
            int countFailed = 0;
            bool toRecycleBin = checkBoxRecycleBin.Checked;

            foreach (var filePath in selectedPathList)
            {
                if (FileRemover.RemoveFile(filePath, toRecycleBin))
                {
                    countSuccess++;
                    _cachedGroupList.RemoveItemFromGroup(filePath);
                }
            }
            countFailed = selectedPathList.Count - countSuccess;

            MessageBox.Show($"Success removed: {countSuccess}\nFailures: {countFailed}", Globals.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshListViewFromCache();
        }

        private void RefreshListViewFromCache()
        {
            var pairedGroups = _cachedGroupList.GetPairedGroupList();
            _cachedGroupList.Clear();
            
            listViewResult.BeginUpdate();
            listViewResult.Items.Clear();

            foreach (var group in pairedGroups)
            {
                AppendGroupToResult(group);
                _cachedGroupList.AddGroup(group);
            }

            listViewResult.EndUpdate();
            UpdateSelectedFilesReport();
        }

        private void listViewResult_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_isFinishedUpdate)
            {
                UpdateSelectedFilesReport();
            }
        }

        private void UpdateSelectedFilesReport()
        {
            long allSizes = listViewResult.CheckedItems.Cast<ListViewItem>()
                .Sum(item => long.Parse(item.SubItems[2].Text));

            labelFileSizeSelected.Text = FileSizeFormatter.Format(allSizes);
            labelFileCountSelected.Text = listViewResult.CheckedItems.Count.ToString();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            ConsumeWorkResults();
        }

        private void ConsumeWorkResults()
        {
            if (_updateQueue.Count() > 0)
            {
                listViewResult.BeginUpdate();

                while (_updateQueue.TryDequeue(out var groupList))
                {
                    foreach (var group in groupList)
                    {
                        AppendGroupToResult(group);
                        _cachedGroupList.AddGroup(group);
                    }
                }

                listViewResult.EndUpdate();
            }
            else
            {
                if (_isFinishedScanning)
                {
                    timerUpdate.Enabled = false;
                    buttonCancelSearch.Enabled = false;
                    _isFinishedUpdate = true;
                    UpdateSelectedFilesReport();

                    if (listViewResult.Groups.Count == 0)
                    {
                        MessageBox.Show("No duplicates found!", Globals.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            _elapsedTimeWatcher.Print();
            _progressBarData.Print();
        }

        private void FormResults_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerUpdate.Enabled = false;
        }
    }
       

    
}
