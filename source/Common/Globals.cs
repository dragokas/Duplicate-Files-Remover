using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicatesFinder
{
    internal class Globals
    {
        internal static readonly string AppName = "Duplicate Files Remover";

        internal static readonly DatabaseFS DB = new(eraseDatabase: true);
        internal static SearchThreadManager SearchThreadMan = new(Environment.ProcessorCount);

        public static string FormatException(Exception ex)
        {
            string s;
            s = "Error: " + ex.ToString() + "\n";
            if (ex.InnerException != null)
            {
                s += "Inner error: " + ex.InnerException.Message.ToString() + "\n";
            }
            if (ex.StackTrace != null)
            {
                s += "Stack: " + ex.StackTrace.ToString();
            }
            return s;
        }

        public static void ReportError(Exception ex, bool interactive = true, string additionalContext = "")
        {
            string s = String.Format("AppVersion: {0}\n{1}", "1.0", FormatException(ex));
            if (!string.IsNullOrEmpty(additionalContext))
            {
                s = additionalContext + ", " + s;
            }
            Log.Error(s);
            if (interactive)
            {
                MessageBox.Show(s, Globals.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
