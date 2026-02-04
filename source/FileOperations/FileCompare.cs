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
    internal enum FileCompareAlgorithm
    {
        ByHash,     // XXH3 hash of entire file
        ByLastBytes // XXH3 hash of the last few bytes of the file
    }

    internal class FileCompareMethod
    {
        internal FileCompareAlgorithm Algorithm;
        internal int NumLastBytes;

        internal FileCompareMethod(FileCompareAlgorithm algorithm, int numLastBytes = 0)
        {
            Algorithm = algorithm;
            NumLastBytes = numLastBytes;
        }
    }

    internal class FileCompareGroup
    {
        internal List<FileCompareMetadata> MetadataList = new();

        internal FileCompareGroup() { }
        internal FileCompareGroup(FileCompareMetadata metadata)
        {
            MetadataList.Add(metadata);
        }
    }

    internal class FileCompare
    {
        private FileCompareMethod _fileCompareMethod;

        internal FileCompare(FileCompareMethod fileCompareMethod)
        {
            _fileCompareMethod = fileCompareMethod;
        }
        
        // returns Hash + Metadata
        // may returns several groups per same file size:
        // e.g. file1 + file2 are same; file3 + file4 are same; but file1 and file 3 are not.
        internal List<FileCompareGroup>? GetIdenticalFileGroups(List<string> fileList_WithIdenticalSize)
        {
            if (fileList_WithIdenticalSize.Count() <= 1)
            {
                return null;
            }

            List<FileCompareMetadata> filesMetadata = new();

            foreach (var file in fileList_WithIdenticalSize)
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
            }

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
                    foreach (var meta in rootCompareGroup.MetadataList)
                    {
                        if (meta.Hash == null)
                        {
                            meta.Hash = Hasher.GetHash(meta.FilePath);
                        }
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
