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

        private ListView _lv;
        private HashSet<string> _cachedList = new();

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
        
        public bool Add(string path, bool isChecked = true)
        {
            if (_cachedList.Contains(path))
                return false;

            bool isFile = File.Exists(path);
            bool isDirectory = Directory.Exists(path);

            if (!isFile && !isDirectory)
            {
                return false;
            }

            ListViewItem lvi = new ListViewItem(new string[]{ path, isFile ? CONTENT_TYPE_FILE : CONTENT_TYPE_DIR, "", "" });
            if (isChecked)
            {
                lvi.Checked = true;
            }
            _lv.Items.Add(lvi);
            _cachedList.Add(path);
            return true;
        }

        public void Add_AsInvoke(string path, bool isChecked = true)
        {
            _lv.BeginInvoke(() => Add(path, isChecked));
        }

        public void Clear()
        {
            _lv.Items.Clear();
            _cachedList.Clear();
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

        public void Remove(int index)
        {
            Cached_TryRemove(_lv.Items[index].Text);
            _lv.Items.RemoveAt(index);
        }

        public void Remove(ListViewItem item)
        {
            Cached_TryRemove(item.Text);
            _lv.Items.Remove(item);
        }

        private void Cached_TryRemove(string path)
        {
            if (_cachedList.Contains(path))
            {
                _cachedList.Remove(path);
            }
        }

        public int Count
        {
            get
            {
                return _lv.Items.Count;
            }
        }

    }
}
