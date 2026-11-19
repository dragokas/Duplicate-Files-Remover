using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Hashing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesFinder
{
    internal class FileCompare
    {
        private FileCompareMethod _fileCompareMethod;
        private GroupCompareMethod _groupCompareMethod;

        internal FileCompare(FileCompareMethod fileCompareMethod, GroupCompareMethod groupCompareMethod)
        {
            _fileCompareMethod = fileCompareMethod;
            _groupCompareMethod = groupCompareMethod;
        }

        // returns Hash + Metadata
        // may returns several groups per same file size:
        // e.g. file1 + file2 are same; file3 + file4 are same; but file1 and file 3 are not content same.
        //
        // for FileCompareAlgorithm.ByName -> returns data as is, with Metadata appended
        internal List<FileCompareGroup>? GetIdenticalFileGroups(List<string> searchedGroups)
        {
            List<FileCompareGroup>? groupList;
            if (_fileCompareMethod.Algorithm == FileCompareAlgorithm.ByName)
            {
                groupList = GetIdenticalFileGroups_ByName(searchedGroups);
            }
            else
            {
                groupList = GetIdenticalFileGroups_Internal(searchedGroups);
            }
            if (groupList == null)
            {
                return null;
            }
            if (_groupCompareMethod.DropGroupWithSameRootDir)
            {
                RemoveGroupsHavingSameRootDir(ref groupList);
            }
            return groupList;
        }

        private void RemoveGroupsHavingSameRootDir(ref List<FileCompareGroup> groupList)
        {
            for (int i = groupList.Count - 1; i >= 0; i--)
            {
                if (GroupBelongsToSameRootDir(groupList[i]))
                {
                    groupList.Remove(groupList[i]);
                }
            }
        }

        private bool GroupBelongsToSameRootDir(FileCompareGroup group)
        {
            if (_groupCompareMethod.RootDirs == null)
            {
                return false;
            }
            var matchedRootDirs = new HashSet<string>(
                _groupCompareMethod.RootDirs.Where(root => group.MetadataList.Any(meta =>
                    meta.FilePath.StartsWith(@"\\?\" + root, StringComparison.OrdinalIgnoreCase))), StringComparer.OrdinalIgnoreCase);
            return (matchedRootDirs.Count == 1);
        }

        private List<FileCompareGroup>? GetIdenticalFileGroups_ByName(List<string> fileList_WithIdentacalName)
        {
            if (fileList_WithIdentacalName.Count < 2)
            {
                return null;
            }

            List<FileCompareMetadata> filesMetadata = new();
            FileCompareGroup group = new();

            foreach (var path in fileList_WithIdentacalName)
            {
                group.MetadataList.Add(new FileCompareMetadata(path, _fileCompareMethod, false));
            }

            return new() { group };
        }

        private List<FileCompareGroup>? GetIdenticalFileGroups_Internal(List<string> fileList_WithIdenticalSize)
        {
            if (fileList_WithIdenticalSize.Count() <= 1)
            {
                return null;
            }

            List<FileCompareMetadata> filesMetadata = new();

            Parallel.ForEach(fileList_WithIdenticalSize, file =>
            {
                try
                {
                    FileCompareMetadata meta = new(file, _fileCompareMethod);

                    lock (filesMetadata)
                    {
                        filesMetadata.Add(meta);
                    }
                }
                catch (Exception)
                {
                    Log.Error("Failed to read file: " + file);
                }
            });

            if (filesMetadata.Count < 2)
            {
                return null;
            }

            // FIRST -> make fast partial compare (of last 1024 bytes)
            // and create group of partially identical files
            Dictionary<ulong, FileCompareGroup> partiallyIdenticalGroup = new();
            
            foreach (var meta in filesMetadata)
            {
                if (partiallyIdenticalGroup.TryGetValue(meta.PartialHash, out var compareGroup)
                    && IsPartiallySameFile(compareGroup.MetadataList[0], meta))
                {
                    compareGroup.MetadataList.Add(meta);
                }
                else
                {
                    partiallyIdenticalGroup.Add(meta.PartialHash, new FileCompareGroup(meta));
                }
            }

            if (_fileCompareMethod.Algorithm == FileCompareAlgorithm.ByLastBytes)
            {
                return partiallyIdenticalGroup
                    .Where(pair => pair.Value.MetadataList.Count > 1)
                    .Select(pair => pair.Value)
                    .ToList();
            }

            // SECOND -> Ensure the partially identical files are really fully identical
            // by hashing the entire file per individual groups
            Dictionary<ulong, FileCompareGroup> fullyIdenticalGroup = new();

            foreach (var rootCompareGroup in partiallyIdenticalGroup.Values)
            {
                if (rootCompareGroup.MetadataList.Count > 1)
                {
                    Parallel.ForEach(rootCompareGroup.MetadataList, meta =>
                    {
                        if (meta.Hash == null)
                        {
                            meta.Hash = Hasher.GetHash(meta.FilePath);
                        }
                    });
                    foreach (var meta in rootCompareGroup.MetadataList)
                    {
                        if (meta.Hash != null)
                        {
                            if (fullyIdenticalGroup.TryGetValue((ulong)meta.Hash, out var compareGroup)
                                && IsFullySameFile(compareGroup.MetadataList[0], meta))
                            {
                                compareGroup.MetadataList.Add(meta);
                            }
                            else
                            {
                                fullyIdenticalGroup.Add((ulong)meta.Hash, new FileCompareGroup(meta));
                            }
                        }
                    }
                }
            }

            return fullyIdenticalGroup
                    .Where(pair => pair.Value.MetadataList.Count > 1)
                    .Select(pair => pair.Value)
                    .ToList();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsFullySameFile(FileCompareMetadata fileMetadata1, FileCompareMetadata fileMetadata2)
        {
            return (fileMetadata1.Size == fileMetadata2.Size)
                && (fileMetadata1?.Hash == fileMetadata2?.Hash);
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsPartiallySameFile(FileCompareMetadata fileMetadata1, FileCompareMetadata fileMetadata2)
        {
            return (fileMetadata1.Size == fileMetadata2.Size)
                && (fileMetadata1.PartialHash == fileMetadata2.PartialHash);
        }
    }
}
