using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DuplicatesFinder
{
    public class SortedGroupList : BindingList<FileCompareGroup>
    {
        public event Action? SortDirectionChanged;
        public event Action? SortFieldChanged;

        private SortMethod _sortMethod = new();
        private bool _isBeginRemove = false;
        private OptimizedGroupList? _optimizedGroupList = null;

        public SortField SortField
        {
            get => _sortMethod.SortField;
            set
            {
                if (_sortMethod.SortField != value)
                {
                    _sortMethod.SortField = value;
                    Resort();
                    SortFieldChanged?.Invoke();
                }
            }
        }

        public SortOrder SortDirection
        {
            get => _sortMethod.SortDirection;
            set
            {
                if (_sortMethod.SortDirection != value)
                {
                    _sortMethod.SortDirection = value;
                    Resort();
                    SortDirectionChanged?.Invoke();
                }
            }
        }

        public void ReverseSortDirection()
        {
            SortDirection =
                _sortMethod.SortDirection == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
        }

        protected override void InsertItem(int index, FileCompareGroup group)
        {
            int insertIndex = FindInsertIndex(group);
            base.InsertItem(insertIndex, group);
        }
        
        public void BeginRemove()
        {
            _optimizedGroupList = new();
            ((List<FileCompareGroup>)Items).ForEach(group => _optimizedGroupList.AddGroup(group));
            _isBeginRemove = true;
        }

        public void RemoveItemByPath(string path)
        {
            if (!_isBeginRemove)
                throw new Exception("BeginRemove() must be called first");

            _optimizedGroupList?.RemoveItemFromGroup(path);
        }

        public void EndRemove()
        {
            this.RaiseListChangedEvents = false;
            this.Items.Clear();
            _optimizedGroupList?.GetPairedGroupList().ForEach(group => this.Items.Add(group));
            this.RaiseListChangedEvents = true;
            this.ResetBindings();
            _optimizedGroupList = null;
            _isBeginRemove = false;
        }

        private int FindInsertIndex(FileCompareGroup item)
        {
            var list = (List<FileCompareGroup>)Items;

            int low = 0;
            int high = list.Count - 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;
                int comparison = Compare(item, list[mid]);

                if (comparison == 0)
                    return mid;

                if (comparison < 0)
                    high = mid - 1;
                else
                    low = mid + 1;
            }

            return low;
        }

        private int Compare(FileCompareGroup x, FileCompareGroup y)
        {
            int result = _sortMethod.SortField switch
            {
                SortField.ByPath => x.MetadataList[0].FilePath.CompareTo(y.MetadataList[0].FilePath),
                SortField.BySize => x.MetadataList[0].Size.CompareTo(y.MetadataList[0].Size),
                SortField.ByCount => x.MetadataList.Count.CompareTo(y.MetadataList.Count),
                _ => 0
            };

            return _sortMethod.SortDirection == SortOrder.Ascending
                ? result
                : -result;
        }

        private void Resort()
        {
            var list = (List<FileCompareGroup>)Items;
            list.Sort(Compare);
            ResetBindings();
        }
    }
}
