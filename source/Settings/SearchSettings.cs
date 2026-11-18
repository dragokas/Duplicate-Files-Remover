using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicatesFinder
{
    internal class SearchSettings
    {
        internal List<SourceEntry> ListToCompare { get; set; } = new();
        internal FileCompareMethod FileCompareMethod { get; set; } = new();
        internal GroupCompareMethod GroupCompareMethod { get; set; } = new();
        internal bool MarkFirstItem { get; set; } = false;
        internal bool MatchExtensions { get; set; } = false;
    }
}
