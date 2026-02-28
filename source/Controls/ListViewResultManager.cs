using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesFinder
{
    internal class ListViewResultManager
    {
        private ListView _lv;
        private SortedGroupList _resultList = new();

        public ListViewResultManager(ListView listview, SortedGroupList resultList)
        {
            _lv = listview;
            _lv.ColumnClick += _lv_ColumnClick;
            _resultList = resultList;
            _resultList.ListChanged += SourceList_ListChanged;
            _resultList.SortDirectionChanged += _resultList_SortDirectionChanged;
            _resultList.SortFieldChanged += _resultList_SortFieldChanged;
            SetSortIcon();
        }

        private void _resultList_SortFieldChanged()
        {
            SetSortIcon();
        }

        private void _resultList_SortDirectionChanged()
        {
            SetSortIcon();
        }

        private void SetSortIcon()
        {
            _lv.SetSortIcon((int)_resultList.SortField, _resultList.SortDirection);
        }

        private void _lv_ColumnClick(object? sender, ColumnClickEventArgs e)
        {
            if ((SortField)e.Column == _resultList.SortField)
            {
                _resultList.ReverseSortDirection();
            }
            else
            {
                _resultList.SortField = (SortField)e.Column;
            }
        }

        private void SourceList_ListChanged(object? sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    InsertGroupAt(e.NewIndex);
                    break;

                case ListChangedType.ItemDeleted:
                    RemoveGroup(e.NewIndex);
                    break;

                case ListChangedType.Reset:
                    RefreshAll();
                    break;
            }
        }

        private void InsertGroupAt(int index)
        {
            var item = _resultList[index];
            var lvgroup = CreateListViewGroup(item);
            _lv.Groups.Insert(index, lvgroup);
        }

        private void RemoveGroup(int index)
        {
            var items = GetGroupItems(_lv.Groups[index]);
            for (int i = items.Count - 1; i >= 0; i--)
            {
                _lv.Items.RemoveAt(items[i].Index);
            }
            _lv.Groups.RemoveAt(index);
        }

        private void RefreshAll()
        {
            _lv.BeginUpdate();
            _lv.Items.Clear();
            _lv.Groups.Clear();

            for (int i = 0; i < _resultList.Count; i++)
            {
                _lv.Groups.Add(CreateListViewGroup(_resultList[i]));
            }

            _lv.EndUpdate();
        }

        internal ListViewGroup CreateListViewGroup(FileCompareGroup fileGroup)
        {
            bool isFirstItem = true;
            ListViewGroup lvGroup = new(CreateGroupName(fileGroup));

            foreach (var meta in fileGroup.MetadataList.OrderByDescending(x => x.FilePath))
            {
                ListViewItem lvi = new(meta.FilePath.Substring(4), lvGroup); // remove "\\?\"
                lvi.SubItems.Add(fileGroup.MetadataList.Count.ToString());
                lvi.SubItems.Add(meta.Size.ToString());
                lvi.Checked = !isFirstItem;

                _lv.Items.Add(lvi);
                isFirstItem = false;
            }
            return lvGroup;
        }

        private string CreateGroupName(FileCompareGroup fileGroup)
        {
            var firstFileMetadata = fileGroup.MetadataList[0];
            return $"({FileSizeFormatter.Format(firstFileMetadata.Size)}) {firstFileMetadata.FileName}";
        }

        internal void SelectAll()
        {
            _lv.BeginUpdate();
            foreach (ListViewItem item in _lv.Items)
            {
                item.Selected = true;
            }
            _lv.EndUpdate();
        }

        internal HashSet<ListViewGroup> GetSelectedGroups()
        {
            var selectedGroups = new HashSet<ListViewGroup>();
            foreach (ListViewItem item in _lv.SelectedItems)
            {
                if (item.Group != null)
                    selectedGroups.Add(item.Group);
            }
            return selectedGroups;
        }

        internal List<ListViewItem> GetGroupItems(ListViewGroup group)
        {
            return group.Items.Cast<ListViewItem>().ToList();
        }

        internal List<ListViewItem> GetGroupItems(List<ListViewGroup> groups)
        {
            List<ListViewItem> items = new();
            groups.ForEach(g => { items.AddRange(GetGroupItems(g)); });
            return items;
        }

        internal int GetGroupIndex(ListViewGroup group)
        {
            return _lv.Groups.IndexOf(group);
        }

    }
}
