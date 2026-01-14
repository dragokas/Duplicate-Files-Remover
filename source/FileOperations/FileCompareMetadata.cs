using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DuplicatesFinder
{
    internal class FileCompareMetadata
    {
        internal string FilePath = "";
        internal ulong? Hash = null;
        internal ulong PartialHash = 0;
        internal long Size = 0;

        internal string FileName
        {
            get
            {
                return Path.GetFileName(FilePath);
            }
        }

        internal FileCompareMetadata(string path, FileCompareMethod fileCompareMethod)
        {
            var fi = new FileInfo(path);
            this.FilePath = path;
            this.Size = fi.Length;
            this.PartialHash = Hasher.GetHash(ReadFileLastBytes(fileCompareMethod.NumLastBytes));
        }

        private byte[] ReadFileLastBytes(int numBytesToRead)
        {
            long len_to_read = numBytesToRead;
            long ptr = this.Size - len_to_read;
            if (ptr < 0)
            {
                ptr = 0;
                len_to_read = this.Size;
            }

            var buffer = new byte[len_to_read];

            using (FileStream fs = File.OpenRead(FilePath))
            {
                fs.Position = ptr;
                fs.ReadExactly(buffer);
            }
            return buffer;
        }

    };    
}
