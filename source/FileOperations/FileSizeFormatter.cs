using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DuplicatesFinder
{
    internal static class FileSizeFormatter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool GetFileSize(string path, out long size)
        {
            try
            {
                FileInfo fi = new(path);
                size = fi.Length;
                return true;
            }
            catch (Exception)
            {
                size = 0;
                Log.Error("Failed to get size of object: " + path);
                return false;
            }
        }
    }
}
