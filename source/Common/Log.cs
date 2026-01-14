using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;

namespace System
{
    internal static class Log
    {
        internal enum LOG_NOISE_LEVEL : int
        {
            Minimum = 1,
            Average = 2,
            Noisy = 3
        }

        [Flags]
        internal enum LOG_LOCATION : int
        {
            LOG_LOCATION_NONE = 0,
            LOG_LOCATION_CONSOLE = 1,
            LOG_LOCATION_FILE = 2,
            LOG_LOCATION_DEBUG_OUTPUT_STRING = 4,
            LOG_LOCATION_ALL = -1
        }

        internal enum LogTag
        {
            Info,
            Warning,
            Error
        }

        private const int m_SizeFlushCache = 16 * 1024;
        private const int m_SizeMaxCache = 32 * 1024;

        private static LOG_LOCATION LogLocation =
            LOG_LOCATION.LOG_LOCATION_ALL;
        private static LOG_NOISE_LEVEL LogNoiseLevel =
            LOG_NOISE_LEVEL.Noisy;

        internal static string LogPath { get; set; } = "";

        private static readonly ReaderWriterLock locker = new();
        private static FileStream? fs;
        private static readonly UTF8Encoding utf8 = new();
        private static readonly string TagInfo = "|INFO|";
        private static readonly string TagError = "|ERROR|";
        private static readonly string TagWarning = "|WARNING|";
        private static StringBuilder sbCache = new StringBuilder(m_SizeFlushCache, m_SizeMaxCache);
        private static readonly object m_lockCache = new();
        private static bool m_DisableCache = false;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern void OutputDebugString(string lpOutputString);

        internal static void Initialize(bool deleteOld)
        {
            string? appdir = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            LogPath = Path.Combine((appdir ?? "").Replace("file:\\", ""), "main.log");

            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);

                if (deleteOld)
                {
                    if (File.Exists(LogPath))
                    {
                        File.Delete(LogPath);
                    }
                }
                fs = File.Open(LogPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                fs?.Seek(0, SeekOrigin.End);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        internal static void SetLogLocation(LOG_LOCATION logLocation)
        {
            LogLocation = logLocation;
        }

        internal static void SetLogNoiseLevel(LOG_NOISE_LEVEL level)
        {
            LogNoiseLevel = level;
        }

        internal static void DisableCache(bool disable)
        {
            m_DisableCache = disable;
        }

        internal static void Flush()
        {
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);

                if (fs != null)
                {
                    fs.Write(utf8.GetBytes(sbCache.ToString()));
                    fs.Flush();
                    sbCache.Clear();
                }
            }
            catch { }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        private static void WriteCache(string text)
        {
            try
            {
                lock (m_lockCache)
                {
                    sbCache.AppendLine(text);

                    if (sbCache.Length > m_SizeFlushCache || m_DisableCache)
                    {
                        Flush();
                    }
                }
            }
            catch { }
        }

        internal static void WriteLog(string data)
        {
            if ((LogLocation & LOG_LOCATION.LOG_LOCATION_FILE) != 0)
            {
                WriteCache(data);
            }
            if ((LogLocation & LOG_LOCATION.LOG_LOCATION_CONSOLE) != 0)
            {
                Console.WriteLine(data);
            }
            if ((LogLocation & LOG_LOCATION.LOG_LOCATION_DEBUG_OUTPUT_STRING) != 0)
            {
                System.Diagnostics.Debug.WriteLine(data);
            }
        }

        internal static void Info(string text)
        {
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                string data = String.Format("{0}/{1}/{2}.{3:D3} {4} {5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond, TagInfo, text);
                WriteLog(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        internal static void Info(LOG_NOISE_LEVEL noiseLevel, string text)
        {
            if (LogNoiseLevel < noiseLevel)
            {
                return;
            }
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                string data = String.Format("{0}/{1}/{2}.{3:D3} {4} {5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond, TagInfo, text);
                WriteLog(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        internal static void Warning(string text)
        {
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                string data = String.Format("{0}/{1}/{2}.{3:D3} {4} {5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond, TagWarning, text);
                WriteLog(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        internal static void Warning(LOG_NOISE_LEVEL noiseLevel, string text)
        {
            if (LogNoiseLevel < noiseLevel)
            {
                return;
            }
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                string data = String.Format("{0}/{1}/{2}.{3:D3} {4} {5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond, TagWarning, text);
                WriteLog(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        internal static void Error(string text)
        {
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                string data = String.Format("{0}/{1}/{2}.{3:D3} {4} {5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond, TagError, text);
                WriteLog(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        internal static void Error(LOG_NOISE_LEVEL noiseLevel, string text)
        {
            if (LogNoiseLevel < noiseLevel)
            {
                return;
            }
            try
            {
                locker.AcquireWriterLock(Timeout.Infinite);
                string data = String.Format("{0}/{1}/{2}.{3:D3} {4} {5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Millisecond, TagError, text);
                WriteLog(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (locker.IsWriterLockHeld)
                {
                    locker.ReleaseWriterLock();
                }
            }
        }

        internal static string GetTag(LogTag logTag)
        {
            if (logTag == LogTag.Info)
            {
                return TagInfo;
            }
            else if (logTag == LogTag.Warning)
            {
                return TagWarning;
            }
            else if (logTag == LogTag.Error)
            {
                return TagError;
            }
            return String.Empty;
        }

        internal static void Clear()
        {
            Release();
            Initialize(true);
        }

        internal static void Release()
        {
            try
            {
                if (fs != null)
                {
                    Flush();
                    fs.Close();
                    fs.Dispose();
                    fs = null;

                    FileInfo fi = new FileInfo(LogPath);
                    if (fi.Length == 0)
                    {
                        File.Delete(LogPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }
        }
    }
}
