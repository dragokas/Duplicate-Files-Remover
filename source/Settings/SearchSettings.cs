using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicatesFinder
{
    internal class SearchSettings
    {
        internal List<SourceEntry> ListToCompare { get; set; } = new();
        internal FileCompareMethod CompareMethod { get; set; } = new();
        internal bool MarkFirstItem { get; set; } = false;

        // Not implemented
        internal bool MatchExtensions { get; set; } = false;
    }
}
