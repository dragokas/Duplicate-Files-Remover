namespace DuplicatesFinder
{
    public partial class FormAccept : Form
    {
        ListViewSourceManager _sourceList;

        public FormAccept()
        {
            InitializeComponent();
            _sourceList = new(listViewSource);
        }

        private async void buttonSearch_Click(object sender, EventArgs e)
        {
            if (listViewSource.Items.Count == 0)
            {
                MessageBox.Show("You must Drag & Drop at least one folder to compare!");
                return;
            }
            new FormResults(GetFilePathList(), GetCompareMethod()).ShowDialog();
        }

        private List<string> GetFilePathList()
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
    }
}
