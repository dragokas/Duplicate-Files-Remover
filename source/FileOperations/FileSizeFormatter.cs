using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicatesFinder
{
    internal static class FileSizeFormatter
    {
        internal static string Format(long size)
        {
            if (size <= 1024)
            {
                return size.ToString() + " b.";
            }
            if (size <= 1024 * 1024)
            {
                return (size / 1024).ToString() + " Kb.";
            }
            if (size <= 1024 * 1024 * 1024)
            {
                return (size / 1024 / 1024).ToString() + " Mb.";
            }
            return (size / 1024 / 1024 / 1024).ToString() + " Gb.";
        }

        internal static bool GetFileSize(string path, out long size)
        {
            try
            {
                FileInfo fi = new(path);
                size = fi.Length;
                return true;
            }
            catch (Exception ex)
            {
                size = 0;
                Globals.ReportError(ex, false);
                return false;
            }
        }
    }
}
