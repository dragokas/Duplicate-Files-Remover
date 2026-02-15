using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace System
{
    internal static class Shell
    {
        internal static bool Open(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                return false;
            }
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
            return (process != null && process.Id != 0);
        }

        internal static void OpenFolder(string? folder)
        {
            if (!Directory.Exists(folder))
            {
                return;
            }
            var process = Process.Start(new ProcessStartInfo()
            {
                FileName = "explorer.exe",
                Arguments = folder,
                UseShellExecute = false
            });
            process?.Dispose();
        }
    }
}
