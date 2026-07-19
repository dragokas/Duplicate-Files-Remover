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
        private bool _markFirstItemForDeletion;
        private List<string> _protectedPaths;

        public ListViewResultManager(
            ListView listview, 
            SortedGroupList resultList, 
            bool markFirstItemForDeletion,
            List<string> protectedPaths)
        {
            _lv = listview;
            _lv.ColumnClick += _lv_ColumnClick;
            _resultList = resultList;
            _protectedPaths = protectedPaths;
            _markFirstItemForDeletion = markFirstItemForDeletion;
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
            int numItem = 0;
            ListViewGroup lvGroup = new(CreateGroupName(fileGroup));

            List<ListViewItem> itemsToAdd = new();
            bool hasProtectedItems = false;
            bool isAnyOneChecked = false;

            foreach (var meta in fileGroup.MetadataList.OrderByDescending(x => x.FilePath))
            {
                ListViewItem lvi = new(meta.FilePath.Substring(4), lvGroup); // remove "\\?\"
                lvi.SubItems.Add(fileGroup.MetadataList.Count.ToString());
                lvi.SubItems.Add(meta.Size.ToString());

                if (IsPathProtectedFromDeletion(meta.FilePath))
                {
                    lvi.Checked = false;
                    hasProtectedItems = true;
                }
                else
                {
                    if (_markFirstItemForDeletion)
                    {
                        if (numItem != 1) // mark all, except the 2nd item
                        {
                            lvi.Checked = true;
                            isAnyOneChecked = true;
                        }
                    }
                    else
                    {
                        if (numItem != 0) // mark all, except the 1st item
                        {
                            lvi.Checked = true;
                            isAnyOneChecked = true;
                        }
                    }
                }
                itemsToAdd.Add(lvi);
                ++numItem;
            }

            if (!isAnyOneChecked && hasProtectedItems)
            {
                foreach (var item in itemsToAdd)
                {
                    if (!IsPathProtectedFromDeletion(item.Text))
                    {
                        item.Checked = true;
                    }
                }
            }

            _lv.Items.AddRange(itemsToAdd.ToArray());
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

        internal bool IsPathProtectedFromDeletion(string path)
        {
            foreach (var entry in _protectedPaths)
            {
                if (path.StartsWith(@"\\?\"))
                {
                    if (path.Length > 4 && path.AsSpan(4).StartsWith(entry))
                    {
                        return true;
                    }
                }
                else
                {
                    if (path.StartsWith(entry))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
