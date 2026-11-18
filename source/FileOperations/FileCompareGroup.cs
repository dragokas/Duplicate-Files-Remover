using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicatesFinder
{
    public class FileCompareGroup
    {
        internal List<FileCompareMetadata> MetadataList = new();

        internal FileCompareGroup() { }
        internal FileCompareGroup(FileCompareMetadata metadata)
        {
            MetadataList.Add(metadata);
        }
    }
}
