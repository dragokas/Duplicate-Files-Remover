using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuplicatesFinder
{
    internal class ListViewSourceManager
    {
        const string CONTENT_TYPE_FILE = "File";
        const string CONTENT_TYPE_DIR = "<Dir>";

        enum LvColumnIndex
        {
            Path = 0,
            Type = 1,
            Protected = 2
        }

        private ListView _lv;
        private HashSet<SourceEntry> _cachedList = new();

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

            bool isDirectory = false;
            bool isFile = File.Exists(path);

            if (!isFile)
            {
                isDirectory = Directory.Exists(path);
                if (!isDirectory)
                {
                    return false;
                }
            }

            ListViewItem lvi = new ListViewItem(new string[]{ path, "", "" });
            if (isChecked)
            {
                lvi.Checked = true;
            }
            lvi.SubItems[(int)LvColumnIndex.Type].Text = isFile ? CONTENT_TYPE_FILE : CONTENT_TYPE_DIR;
            UpdateProtected(lvi, false);
            _lv.Items.Add(lvi);

            SourceEntry sourceEntry = new(path);
            sourceEntry.IsDirectory = isDirectory;
            _cachedList.Add(sourceEntry);

            lvi.Tag = sourceEntry;

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
            if (_cachedList.TryGetValue(path, out var entry))
            {
                _cachedList.Remove(entry);
            }
        }

        public void UpdateProtected(ListViewItem item, bool isProtected)
        {
            item.SubItems[(int)LvColumnIndex.Protected].Text = isProtected ? "Yes" : "No";
            if (TryGetSourceEntry(item, out var entry))
            { 
                entry.IsProtected = isProtected;
            }
        }

        public bool TryGetSourceEntry(ListViewItem item, [MaybeNullWhen(false)] out SourceEntry entry)
        {
            return _cachedList.TryGetValue(item.Text, out entry);
        }

        public bool TryGetSourceEntryListChecked([MaybeNullWhen(false)] out List<SourceEntry> entryList)
        {
            entryList = _lv.CheckedItems.Cast<ListViewItem>()
                .Select(item => item.Tag)
                .OfType<SourceEntry>()
                .ToList();
            if (entryList.Count == 0)
            {
                entryList = null;
                return false;
            }
            return true;
        }

        public int Count
        {
            get
            {
                return _cachedList.Count;
            }
        }

    }
}
