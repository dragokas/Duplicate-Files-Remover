using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicatesFinder
{
    public class SearchTask
    {
        public string DirectoryToSearch = "";
        public int Timeout = 60000;

        public SearchTask(string directory)
        {
            this.DirectoryToSearch = directory;
        }
    }
}
