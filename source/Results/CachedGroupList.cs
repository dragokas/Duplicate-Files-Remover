using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace DuplicatesFinder
{
    internal class OptimizedGroupList
    {
        private readonly Dictionary<string, int> _groupIndex = new();
        private readonly List<FileCompareGroup> _groupList = new();

        internal void AddGroup(FileCompareGroup group)
        {
            int index = _groupList.Count();

            foreach (var meta in group.MetadataList)
            {
                _groupIndex.TryAdd(meta.FilePath, index);
            }
            
            _groupList.Add(group);
        }

        internal void RemoveItemFromGroup(string filePath)
        {
            if (!filePath.StartsWith(@"\\?\"))
            {
                filePath = @"\\?\" + filePath;
            }
            if (_groupIndex.TryGetValue(filePath, out int index))
            {
                _groupList[index].MetadataList.RemoveAll(meta => meta.FilePath == filePath);
                _groupIndex.Remove(filePath);
            }
        }

        internal List<FileCompareGroup> GetPairedGroupList()
        {
            return _groupList.Where(x => x.MetadataList.Count > 1).ToList();
        }

        internal void Clear()
        {
            _groupList.Clear();
            _groupIndex.Clear();
        }
    }
}
