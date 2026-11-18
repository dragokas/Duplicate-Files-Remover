using DuplicatesFinder.Common;

namespace DuplicatesFinder
{
    public partial class FormAccept : Form
    {
        ListViewSourceManager _sourceList;
        ListView _lv;
        DragDropOverlay _dragDropOverlay;
        bool _isFormLoaded = false;

        public FormAccept()
        {
            InitializeComponent();

            _sourceList = new(listViewSource);
            _lv = _sourceList.GetListView();
            _dragDropOverlay = new(this, "⇨ Drop files here ⇦");

            this.Text = Globals.AppTitle;
            this.MinimumSize = new Size(this.Width, this.Height);
        }

        private void FormAccept_Load(object sender, EventArgs e)
        {
            _dragDropOverlay.CreateOverlayWindow(this.Handle, 0, 0, pictureBoxDropFiles.Width, pictureBoxDropFiles.Height);
            _dragDropOverlay.FileDropped += DragDropOverlay_FileDropped;
            _isFormLoaded = true;
            MoveOverlay();
        }

        private void DragDropOverlay_FileDropped(string path)
        {
            _sourceList.Add_AsInvoke(path);
        }

        private void FormAccept_LocationChanged(object sender, EventArgs e)
        {
            MoveOverlay();
        }

        private void FormAccept_Resize(object sender, EventArgs e)
        {
            MoveOverlay();
        }

        private void MoveOverlay()
        {
            if (_isFormLoaded)
            {
                var location = pictureBoxDropFiles.PointToScreen(Point.Empty);
                _dragDropOverlay?.Move(location.X, location.Y);
            }
        }

        private async void buttonSearch_Click(object sender, EventArgs e)
        {
            if (_sourceList.TryGetSourceEntryListChecked(out var entryList))
            {
                SearchSettings searchSettings = new()
                {
                    ListToCompare = entryList,
                    FileCompareMethod = GetCompareMethod(),
                    GroupCompareMethod = new GroupCompareMethod(
                        checkBoxSkipSameRootDirMatches.Checked,
                        entryList.Select(x => x.Path).ToList()),
                    MarkFirstItem = checkBoxMarkFirstItem.Checked,
                    MatchExtensions = checkBoxMatchExtensions.Checked
                };
                new FormResults(searchSettings).ShowDialog();
            }
            else
            {
                MessageBox.Show("You must Drag & Drop at least one folder to compare and mark a checkbox!");
            }
        }

        private List<string> GetFilePathListChecked()
        {
            return listViewSource.CheckedItems.Cast<ListViewItem>()
                .Select(item => item.Text)
                .ToList();
        }

        private FileCompareMethod GetCompareMethod()
        {
            return new FileCompareMethod(
                radioButtonCompareMethodFast.Checked ? FileCompareAlgorithm.ByLastBytes : FileCompareAlgorithm.ByHash,
                (int)numericUpDownNumBytesToCompare.Value);
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        _sourceList.Add(file);
                    }
                }
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Geometry.LimitContainerPanel2MaxSize(splitContainer1, splitContainer1.Panel2MinSize);
            ResizePathColumnWidth();
        }
        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            Geometry.LimitContainerPanel2MaxSize(splitContainer2, splitContainer2.Panel2MinSize);
        }

        private void ResizePathColumnWidth()
        {
            int typeColumnMinWidth = listViewSource.Columns[1].Width;
            listViewSource.Columns[0].Width = listViewSource.Width - typeColumnMinWidth - SystemInformation.VerticalScrollBarWidth;
        }

        private void buttonAddFolder_Click(object sender, EventArgs e)
        {
            foreach (var dir in Dialogs.SelectDirectories("", "Select directories") ?? [])
            {
                _sourceList.Add(dir);
            }
        }

        private void buttonClearList_Click(object sender, EventArgs e)
        {
            _sourceList.Clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in _lv.SelectedItems)
            {
                Shell.OpenFolder(item.Text);
            }
        }

        private void deleteFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in _lv.SelectedItems)
            {
                _sourceList.Remove(item);
            }
        }

        private void listViewSource_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = _lv.HitTest(e.X, e.Y);
                if (hit.Item != null)
                {
                    if (!_lv.Items[hit.Item.Index].Selected)
                    {
                        _lv.SelectedItems.Clear();
                        _lv.Items[hit.Item.Index].Selected = true;
                    }
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void protectFromDeletionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in _lv.SelectedItems)
            {
                if (_sourceList.TryGetSourceEntry(item, out var entry))
                {
                    _sourceList.UpdateProtected(item, !entry.IsProtected);
                }
            }
        }
    }
}
