using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DuplicatesFinder
{
    internal static class SearchEngineSimple
    {
        internal static async Task SearchDirectory(List<string> directoryList)
        {
            Globals.SearchThreadMan = new(Environment.ProcessorCount);
            Globals.DB.RemoveAll();

            foreach (var directory in directoryList)
            {
                try
                {
                    if (File.Exists(directory))
                    {
                        AddFile(@"\\?\" + directory);
                    }
                    else if (Directory.Exists(directory))
                    {
                        Globals.SearchThreadMan.AddTask(new SearchTask(@"\\?\" + directory));
                    }
                }
                catch (Exception)
                {
                    Log.Error("Failed to check Exists() of object: " + directory);
                }
            }

            await Globals.SearchThreadMan.WaitForTaskCompletion();
        }

        internal static void SearchDirectory(string directory)
        {
            try
            {
                foreach (var entry in Directory.EnumerateFileSystemEntries(directory))
                {
                    if (Directory.Exists(entry))
                    {
                        if (IsSymbolicLink(entry))
                        {
                            continue;
                        }
                        Globals.SearchThreadMan.AddTask(new SearchTask(entry));
                    }
                    else
                    {
                        AddFile(entry);
                    }
                }
            }
            catch (Exception)
            {
                Log.Error("Failed to enumerate object: " + directory);
            }
        }

        private static void AddFile(string entry)
        {
            if (FileSizeFormatter.GetFileSize(entry, out long size))
            {
                Globals.DB.InsertPath(entry, size);
            }
        }

        static bool IsSymbolicLink(string entryPath)
        {
            return (new FileInfo(entryPath).Attributes & FileAttributes.ReparsePoint) != 0;
        }
    }
}
