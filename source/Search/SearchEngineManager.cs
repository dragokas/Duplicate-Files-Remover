using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicatesFinder
{
    internal static class SearchEngineManager
    {
        internal static async Task Search(List<string> _pathList)
        {
            await SearchEngineSimple.SearchDirectory(_pathList);
        }
    }
}
