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
