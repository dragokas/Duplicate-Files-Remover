using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuplicatesFinder
{
    internal class ListViewSourceManager
    {
        const string CONTENT_TYPE_FILE = "file";
        const string CONTENT_TYPE_DIR = "<Dir>";
        ListView _lv;

        public ListViewSourceManager(ListView listView)
        {
            _lv = listView;
            _lv.GridLines = true;
            _lv.View = View.Details;
        }

        public ListView GetListView()
        {
            return _lv;
        }
        
        public void Add(string path, bool isChecked = true)
        {
            bool isFile = File.Exists(path);
            bool isDirectory = Directory.Exists(path);

            if (!isFile && !isDirectory)
            {
                return;
            }

            ListViewItem lvi = new ListViewItem(new string[]{ path, isFile ? CONTENT_TYPE_FILE : CONTENT_TYPE_DIR, "", "" });
            if (isChecked)
            {
                lvi.Checked = true;
            }
            _lv.Items.Add(lvi);
        }

        public void Add_AsInvoke(string path, bool isChecked = true)
        {
            _lv.BeginInvoke(() => Add(path, isChecked));
        }

        public void Clear()
        {
            _lv.Items.Clear();
        }

        public string? GetSelectedPath()
        {
            if (_lv.SelectedItems.Count == 0)
                return null;

            return _lv.SelectedItems[0].ToString();
        }

        public int? GetSelectedIndex()
        {
            if (_lv.SelectedItems.Count == 0)
                return null;

            return _lv.SelectedItems[0].Index;
        }

        public void RemoveItem(int index)
        {
            _lv.Items.RemoveAt(index);
        }

        private void UpdateDuplicateCount(int index, int count)
        {
            _lv.Items[index].SubItems[1].Text = count.ToString();
        }

        private void UpdateDuplicateSize(int index, int count)
        {
            _lv.Items[index].SubItems[2].Text = count.ToString();
        }

        public int Count
        {
            get
            {
                return _lv.Items.Count;
            }
        }

        public void RemoveAll()
        {
            _lv.Items.Clear();
        }

    }
}
