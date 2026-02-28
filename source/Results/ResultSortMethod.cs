using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicatesFinder
{
    public enum SortField
    {
        ByPath,
        ByCount,
        BySize
    }

    public class SortMethod
    {
        public SortField SortField { get; set; } = SortField.BySize;
        public SortOrder SortDirection { get; set; } = SortOrder.Descending;
    }
}
