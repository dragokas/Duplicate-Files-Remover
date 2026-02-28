using DuplicatesFinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
    internal static class Shell
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern void OpenAs_RunDLL(IntPtr hwnd, IntPtr hinst, string file, int nShow);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern bool SHObjectProperties(
            IntPtr hwnd,
            uint shopObjectType,
            string pszObjectName,
            string? pszPropertyPage);

        private const uint SHOP_FILEPATH = 0x00000002;
        private const uint ERROR_NO_ASSOCIATION = 1155;

        internal static bool Open(string path, string verb = "")
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                return false;
            }
            Process? process = null;
            try
            {
                process = Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    Verb = verb,
                    UseShellExecute = true
                });
                return (process != null && process.Id != 0);
            }
            catch (System.ComponentModel.Win32Exception ex) when (ex.NativeErrorCode == ERROR_NO_ASSOCIATION && verb == "")
            {
                ShowOpenWithDialog(path);
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex);
            }
            finally
            {
                process?.Dispose();
            }
            return false;
        }

        internal static void ShowOpenWithDialog(string path)
        {
            OpenAs_RunDLL(IntPtr.Zero, IntPtr.Zero, path, 1);
        }

        internal static void OpenProperties(string path)
        {
            SHObjectProperties(IntPtr.Zero, SHOP_FILEPATH, path, null);
        }

        internal static void OpenFolder(string? folder)
        {
            if (!Directory.Exists(folder))
            {
                return;
            }
            Process? process = null;
            try
            {
                process = Process.Start(new ProcessStartInfo()
                {
                    FileName = "explorer.exe",
                    Arguments = folder,
                    UseShellExecute = false
                });
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex);
            }
            finally
            {
                process?.Dispose();
            }
        }
    }
}
