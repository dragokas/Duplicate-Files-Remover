using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Hashing;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatesFinder
{
    internal static class Hasher
    {
        internal static ulong? GetHash(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    var hashStream = new XxHash64();
                    var buffer = new byte[4096];
                    int bytesRead = 0;

                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        hashStream.Append(buffer.AsSpan(0, bytesRead));
                    }
                    return hashStream.GetCurrentHashAsUInt64();
                }
            }
            catch (Exception ex)
            {
                Globals.ReportError(ex, false, "Error hashing file: " + path);
            }
            return null;
        }

        internal static ulong GetHash(byte[] buffer)
        {
            return XxHash64.HashToUInt64(buffer);
        }
    }
}
