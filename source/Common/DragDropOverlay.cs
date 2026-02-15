using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace DuplicatesFinder.Common
{
    // UIPI Bypass Drag & Drop Overlay

    internal class DragDropOverlay
    {
        private IntPtr _handleOverlay;
        private int _width;
        private int _height;
        private string _labelTipText;

        #region declares
        private const int WM_DROPFILES = 0x0233;
        private const int WM_COPYDATA = 0x004A;
        private const int WM_COPYGLOBALDATA = 0x0049;
        private const int WS_CHILD = 0x40000000;
        private const int WS_POPUP = unchecked((int)0x80000000);
        private const int WS_VISIBLE = 0x10000000;
        private const int WM_PAINT = 0x000F;
        private const int WS_OVERLAPPEDWINDOW = 0x00CF0000;
        private const int HWND_TOP = 0;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint SWP_NOSIZE = 1;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_MOUSELEAVE = 0x02A3;
        private const int WM_DRAGENTER = 0x0283;
        private const int MAX_PATH = 260;

        private enum ChangeWindowMessageFilterExAction : uint
        {
            MSGFLT_RESET = 0,
            MSGFLT_ALLOW = 1,
            MSGFLT_DISALLOW = 2
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ChangeWindowMessageFilterEx(
            IntPtr hWnd,
            uint message,
            ChangeWindowMessageFilterExAction action,
            ref CHANGEFILTERSTRUCT pChangeFilterStruct);

        [StructLayout(LayoutKind.Sequential)]
        private struct CHANGEFILTERSTRUCT
        {
            public uint cbSize;
            public ChangeWindowMessageFilterExAction ExtStatus;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct TRACKMOUSEEVENT
        {
            public uint cbSize;
            public uint dwFlags;
            public IntPtr hwndTrack;
            public uint dwHoverTime;
        }

        private const uint TME_LEAVE = 0x00000002;

        [DllImport("user32.dll")]
        private static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

        [DllImport("shell32.dll")]
        private static extern void DragAcceptFiles(IntPtr hWnd, bool fAccept);

        [DllImport("shell32.dll")]
        private static extern uint DragQueryFile(IntPtr hDrop, uint iFile, StringBuilder? lpszFile, int cch);

        [DllImport("shell32.dll")]
        private static extern void DragFinish(IntPtr hDrop);

        private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CreateWindowEx(
            int dwExStyle,
            string lpClassName,
            string lpWindowName,
            int dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern ushort RegisterClassEx(ref WNDCLASSEX lpwcx);

        [DllImport("user32.dll")]
        private static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct WNDCLASSEX
        {
            public uint cbSize;
            public uint style;
            public WndProcDelegate lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern bool RedrawWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        #endregion

        public event Action<string>? FileDropped;

        internal DragDropOverlay(Form form, string labelTipText)
        {
            if (IsProcessElevated())
            {
                form.AllowDrop = false; // AllowDrop option conflicts with DragAcceptFiles on Win32 window!
            }
            _labelTipText = labelTipText;
        }

        internal IntPtr CreateOverlayWindow(IntPtr parentHandle, int x, int y, int width, int height)
        {
            WNDCLASSEX wndClass = new WNDCLASSEX
            {
                cbSize = (uint)Marshal.SizeOf(typeof(WNDCLASSEX)),
                lpfnWndProc = OverlayWndProc,
                lpszClassName = "OverlayWin32Window",
                hbrBackground = IntPtr.Zero, // manual re-draw
                hCursor = IntPtr.Zero,
                hInstance = IntPtr.Zero
            };

            RegisterClassEx(ref wndClass);

            int style = WS_VISIBLE | WS_POPUP; // or WS_OVERLAPPEDWINDOW

            _handleOverlay = CreateWindowEx(
                0,
                wndClass.lpszClassName,
                "Overlay",
                style,
                x, y, width, height,
                parentHandle,
                IntPtr.Zero,
                IntPtr.Zero,
                IntPtr.Zero);

            if (_handleOverlay != IntPtr.Zero)
            {
                DragDropBypassUIPI(_handleOverlay);
                DragAcceptFiles(_handleOverlay, true);
                SetWindowPos(_handleOverlay, HWND_TOP, x, y, width, height, SWP_NOACTIVATE | SWP_SHOWWINDOW);
            }

            _width = width;
            _height = height;

            return _handleOverlay;
        }

        private IntPtr OverlayWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WM_PAINT:
                    DrawOverlayWindow(hWnd);
                    break;

                case WM_DROPFILES:
                    HandleDropFiles(wParam);
                    break;
            }

            return DefWindowProc(hWnd, msg, wParam, lParam);
        }

        private void DrawOverlayWindow(IntPtr hWnd)
        {
            using (Graphics g = Graphics.FromHwnd(hWnd))
            {
                g.Clear(Color.LightGreen);

                using (Font font = new Font("Segoe UI", 12, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.Black))
                {
                    string text = _labelTipText;

                    SizeF textSize = g.MeasureString(text, font);
                    float x = (_width - textSize.Width) / 2;
                    float y = (_height - textSize.Height) / 2;

                    g.DrawString(text, font, brush, x, y);
                }
            }
        }

        private void HandleDropFiles(IntPtr hDrop)
        {
            uint count = DragQueryFile(hDrop, 0xFFFFFFFF, null, 0);

            for (uint i = 0; i < count; i++)
            {
                StringBuilder fileName = new StringBuilder(MAX_PATH);
                DragQueryFile(hDrop, i, fileName, fileName.Capacity);
                FileDropped?.Invoke(fileName.ToString());
            }

            DragFinish(hDrop);
        }

        internal void Move(int x, int y)
        {
            if (_handleOverlay != IntPtr.Zero)
            {
                SetWindowPos(_handleOverlay, HWND_TOP, x, y, 0, 0, SWP_NOACTIVATE | SWP_SHOWWINDOW | SWP_NOSIZE);
            }
        }

        internal static void DragDropBypassUIPI(IntPtr handle)
        {
            CHANGEFILTERSTRUCT filterStruct = new CHANGEFILTERSTRUCT();
            filterStruct.cbSize = (uint)Marshal.SizeOf(typeof(CHANGEFILTERSTRUCT));

            ChangeWindowMessageFilterEx(handle, WM_DROPFILES,
                ChangeWindowMessageFilterExAction.MSGFLT_ALLOW, ref filterStruct);

            ChangeWindowMessageFilterEx(handle, WM_COPYDATA,
                ChangeWindowMessageFilterExAction.MSGFLT_ALLOW, ref filterStruct);

            ChangeWindowMessageFilterEx(handle, WM_COPYGLOBALDATA,
                ChangeWindowMessageFilterExAction.MSGFLT_ALLOW, ref filterStruct);
        }

        internal static bool IsProcessElevated()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}
