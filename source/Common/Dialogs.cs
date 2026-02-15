using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicatesFinder.Common
{
    internal static class Dialogs
    {
        internal static string? SelectDirectory(string? startDirectory = "", string prompt = "")
        {
            FolderBrowserDialog dlg = new();
            dlg.RootFolder = Environment.SpecialFolder.Desktop;
            dlg.UseDescriptionForTitle = true;
            dlg.Description = prompt;
            dlg.Multiselect = false;

            if (!String.IsNullOrEmpty(startDirectory) && Directory.Exists(startDirectory))
            {
                dlg.InitialDirectory = startDirectory;
            }
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.SelectedPath;
            }
            return null;
        }

        internal static List<string>? SelectDirectories(string? startDirectory = "", string prompt = "")
        {
            FolderBrowserDialog dlg = new();
            dlg.RootFolder = Environment.SpecialFolder.Desktop;
            dlg.UseDescriptionForTitle = true;
            dlg.Description = prompt;
            dlg.Multiselect = true;

            if (!String.IsNullOrEmpty(startDirectory) && Directory.Exists(startDirectory))
            {
                dlg.InitialDirectory = startDirectory;
            }
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.SelectedPaths.ToList();
            }
            return null;
        }
    }
}
