using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DuplicatesFinder
{
    public partial class FormResults : Form
    {
        private ListViewResultManager _lvManager;
        private SortedGroupList _groupList;
        private SearchSettings _searchSettings;
        private bool _isCancelled = false;
        private bool _isFinishedScanning = false;
        private bool _isFinishedUpdate = false;
        private readonly ConcurrentQueue<List<FileCompareGroup>> _updateQueue = new();
        private ProgressBarData _progressBarData;
        private ElapsedTimeWatcher _elapsedTimeWatcher;

        internal FormResults(SearchSettings searchSettings)
        {
            InitializeComponent();
            ResizeSplitter();
            ResizePathColumnWidth();
            _searchSettings = searchSettings;
            _groupList = new();
            List<string> protectedPaths = _searchSettings.ListToCompare
                .Where(x => x.IsProtected).Select(x => x.Path).ToList();
            _lvManager = new(listViewResult, _groupList, _searchSettings.MarkFirstItem, protectedPaths);
            _progressBarData = new(labelProgressBar);
            _elapsedTimeWatcher = new(labelSearchTime);
            timerUpdate.Enabled = true;
            Control.CheckForIllegalCrossThreadCalls = true;
        }

        private void FormResults_Shown(object sender, EventArgs e)
        {
            ReScan();
        }

        private void ReScan()
        {
            _groupList.Clear();
            UpdateSelectedFilesReport();
            buttonCancelSearch.Enabled = true;
            buttonRescan.Enabled = false;
            buttonRefresh.Enabled = false;
            buttonRemoveFiles.Enabled = false;
            Task.Run(() => StartSearch());
        }

        private async Task StartSearch()
        {
            _isCancelled = false;
            _isFinishedScanning = false;
            _isFinishedUpdate = false;

            _elapsedTimeWatcher.Reset();
            _elapsedTimeWatcher.Start();

            List<string> pathListToCompare = _searchSettings.ListToCompare.Select(x => x.Path).ToList();
            await SearchEngineManager.Search(pathListToCompare);

            if (_searchSettings.FileCompareMethod.Algorithm == FileCompareAlgorithm.ByName)
            {
                var searchedGroups = Globals.DB.GetFilePathsByName();

                _progressBarData.CurrentValue = 0;
                _progressBarData.Total = searchedGroups.Count();

                Parallel.ForEach(searchedGroups, (searchedGroupPair, state) =>
                {
                    if (_isCancelled)
                    {
                        state.Stop();
                    }
                    var searchedGroup = searchedGroupPair.Value;
                    var fileCompare = new FileCompare(
                            _searchSettings.FileCompareMethod, _searchSettings.GroupCompareMethod);
                    var groupList = fileCompare.GetIdenticalFileGroups(searchedGroup);
                    if (groupList != null)
                    {
                        _updateQueue.Enqueue(groupList);
                    }
                    _progressBarData.Increment();
                });
            }
            else
            {
                List<long> uniqueSizesList = Globals.DB.GetUniqueSizes();

                _progressBarData.CurrentValue = 0;
                _progressBarData.Total = uniqueSizesList.Count();

                Parallel.ForEach(uniqueSizesList, (uniqueSize, state) =>
                {
                    if (_isCancelled)
                    {
                        state.Stop();
                    }
                    Dictionary<string, List<string>> searchedGroups = new();
                    if (_searchSettings.MatchExtensions)
                    {
                        searchedGroups = Globals.DB.GetFilePathsBySizeAndExtension(uniqueSize);
                    }
                    else
                    {
                        searchedGroups.Add(".ext", Globals.DB.GetFilePathsBySize(uniqueSize));
                    }
                    foreach (var searchedGroup in searchedGroups.Values)
                    {
                        var fileCompare = new FileCompare(
                            _searchSettings.FileCompareMethod, _searchSettings.GroupCompareMethod);
                        var groupList = fileCompare.GetIdenticalFileGroups(searchedGroup);
                        if (groupList != null)
                        {
                            _updateQueue.Enqueue(groupList);
                        }
                    }
                    _progressBarData.Increment();
                });
            }

            _elapsedTimeWatcher.Stop();
            _elapsedTimeWatcher.Print();
            _isFinishedScanning = true;
            this.BeginInvoke(() => ConsumeWorkResults());
        }

        private void buttonCancelSearch_Click(object sender, EventArgs e)
        {
            Globals.SearchThreadMan.CancelTasks();
            buttonCancelSearch.Enabled = false;
            _isCancelled = true;
        }

        private void buttonRemoveFiles_Click(object sender, EventArgs e)
        {
            AskRemoveFiles(listViewResult.CheckedItems.Cast<ListViewItem>().Select(x => x.Text).ToList());
        }

        private void AskRemoveFiles(List<string> paths)
        {
            bool toRecycleBin = checkBoxRecycleBin.Checked;

            if (MessageBox.Show($"Remove {paths.Count} file(s) " + (toRecycleBin ? "to Recycle bin?" : "permanently?"),
                "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int countSuccess = 0;
            int countFailed = 0;

            _groupList.BeginRemove();
            foreach (var filePath in paths)
            {
                if (FileRemover.RemoveFile(filePath, toRecycleBin))
                {
                    countSuccess++;
                    _groupList.RemoveItemByPath(filePath);
                }
            }
            _groupList.EndRemove();
            countFailed = paths.Count - countSuccess;

            MessageBox.Show($"Success removed: {countSuccess}\nFailures: {countFailed}", Globals.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                try
                {
                    listViewResult.BeginUpdate();

                    while (_updateQueue.TryDequeue(out var groupList))
                    {
                        foreach (var group in groupList)
                        {
                            _groupList.Add(group);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Globals.ReportError(ex);
                }
                finally
                {
                    listViewResult.EndUpdate();
                }
            }
            if (_isFinishedScanning)
            {
                timerUpdate.Enabled = false;
                buttonCancelSearch.Enabled = false;
                buttonRescan.Enabled = true;
                buttonRefresh.Enabled = true;
                buttonRemoveFiles.Enabled = true;
                _isFinishedUpdate = true;
                UpdateSelectedFilesReport();
                if (listViewResult.Groups.Count == 0)
                {
                    MessageBox.Show("No duplicates found!", Globals.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            _elapsedTimeWatcher.Print();
            _progressBarData.Print();
        }

        private void FormResults_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerUpdate.Enabled = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewResult.SelectedItems.Count != 0)
            {
                string path = listViewResult.SelectedItems[0].Text;
                Shell.Open(path);
            }
        }
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewResult.SelectedItems.Count != 0)
            {
                string path = listViewResult.SelectedItems[0].Text;
                Shell.OpenFolder(Directory.GetParent(path)?.FullName);
            }
        }
        private void copyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyPath();
        }
        private void CopyPath()
        {
            if (listViewResult.SelectedItems.Count != 0)
                Clipboard.SetText(String.Join('\n', listViewResult.SelectedItems.Cast<ListViewItem>().Select(x => x.Text).ToList()));
        }
        private void removeFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AskRemoveFiles(listViewResult.SelectedItems.Cast<ListViewItem>().Select(x => x.Text).ToList());
        }
        private void compareFullHashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompareFullHash();
        }
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewResult.SelectedItems.Count != 0)
                Shell.OpenProperties(listViewResult.SelectedItems[0].Text);
        }

        private void listViewResult_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_isFinishedUpdate)
                return;

            if (e.Button == MouseButtons.Right)
            {
                if (listViewResult.SelectedItems.Count == 0)
                {
                    ListViewItem? item = listViewResult.GetItemAt(e.X, e.Y);
                    if (item != null)
                    {
                        listViewResult.SelectedItems.Clear();
                        item.Selected = true;
                        Point screenPoint = listViewResult.PointToScreen(e.Location); // consider DPI
                        contextMenuStrip1.Show(screenPoint);
                    }
                }
                else
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ResizeSplitter();
            ResizePathColumnWidth();
        }

        private void ResizeSplitter()
        {
            Geometry.LimitContainerPanel2MaxSize(splitContainer1, splitContainer1.Panel2MinSize);
        }

        private void ResizePathColumnWidth()
        {
            int allColumnsWidth = 0;
            foreach (ColumnHeader column in listViewResult.Columns)
            {
                allColumnsWidth += column.Width;
            }
            listViewResult.Columns[0].Width =
                listViewResult.Width - (allColumnsWidth - listViewResult.Columns[0].Width) - SystemInformation.VerticalScrollBarWidth;
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            _groupList.BeginRemove();
            foreach (var filePath in listViewResult.Items.Cast<ListViewItem>().Select(x => x.Text).ToList())
            {
                if (!File.Exists(filePath))
                {
                    _groupList.RemoveItemByPath(filePath);
                }
            }
            _groupList.EndRemove();
            UpdateSelectedFilesReport();
        }

        private void buttonRescan_Click(object sender, EventArgs e)
        {
            ReScan();
        }

        private void FormResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                if (e.KeyCode == Keys.C || e.KeyCode == Keys.Insert)
                {
                    CopyPath();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.A)
                {
                    _lvManager.SelectAll();
                    e.Handled = true;
                }
            }
        }

        private void CompareFullHash()
        {
            _isFinishedUpdate = false;
            int numTotalMismatches = 0;
            int numTotalMatches = 0;
            int numTotalInitialMatches = 0;
            int numSeparateGroups = 0;
            FileCompareMethod fileCompareMethod = new(FileCompareAlgorithm.ByHash);
            var selectedGroups = _lvManager.GetSelectedGroups();
            foreach (var group in selectedGroups)
            {
                var items = _lvManager.GetGroupItems(group);
                var paths = items.Select(x => x.Text).ToList();
                var fileCompare = new FileCompare(fileCompareMethod, _searchSettings.GroupCompareMethod);
                var newGroupList = fileCompare.GetIdenticalFileGroups(paths);
                numTotalInitialMatches += items.Count;

                if (newGroupList == null || newGroupList.Count != 1 || newGroupList[0].MetadataList.Count != items.Count)
                {
                    _groupList.RemoveAt(_lvManager.GetGroupIndex(group));

                    if (newGroupList != null)
                    {
                        int numMatches = 0;
                        newGroupList.ForEach(g => numMatches += g.MetadataList.Count);
                        numTotalMismatches += (items.Count - numMatches);
                        numTotalMatches += numMatches;

                        if (newGroupList.Count > 1)
                        {
                            numSeparateGroups += newGroupList.Count - 1;
                        }
                        newGroupList?.ForEach(g => _groupList.Add(g));
                    }
                }
            }
            _isFinishedUpdate = true;
            UpdateSelectedFilesReport();

            if (numTotalMismatches > 0)
            {
                MessageBox.Show($"{numTotalMismatches} of {numTotalInitialMatches} file hashes mismatched and dropped from the list", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (numSeparateGroups > 0)
            {
                MessageBox.Show($"Full match of {numTotalMatches} files confirmed, " +
                    $"however few files splitted into {numSeparateGroups} new group(s)", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                MessageBox.Show($"Full match of {numTotalMatches} files confirmed.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
