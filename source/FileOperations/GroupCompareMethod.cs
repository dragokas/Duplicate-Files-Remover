using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicatesFinder
{
    internal class GroupCompareMethod
    {
        internal bool DropGroupWithSameRootDir = false;
        internal List<string>? RootDirs = null;

        internal GroupCompareMethod() { }
        internal GroupCompareMethod(bool dropGroupWithSameRootDir, List<string>? rootDirs)
        {
            DropGroupWithSameRootDir = dropGroupWithSameRootDir;
            RootDirs = rootDirs;
        }
    }
}
