using System;
using System.Collections.Generic;
using System.Text;

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

        internal FileCompareMethod() { }
        internal FileCompareMethod(FileCompareAlgorithm algorithm, int numLastBytes = 1024)
        {
            Algorithm = algorithm;
            NumLastBytes = numLastBytes;
        }
    }
}
