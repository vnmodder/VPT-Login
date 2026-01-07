using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Drawing;

namespace VPT_Login.Models
{
    public static class Helper
    {
        private const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        public static List<Thread> ThreadList = new List<Thread>();



        public static void ShowAlert(string id, string message)
        {
            System.Windows.MessageBox.Show(id + ": " + message);
        }

        public static float GetCurrentScale()
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                return g.DpiX / 96.0f; // mặc định 96dpi = 100%
            }
        }

        public static IntPtr GetProcessHandle(IntPtr hwnd)
        {
            Helper.GetWindowThreadProcessId(hwnd, out uint pid);
            return Helper.OpenProcess(PROCESS_ALL_ACCESS, false, pid);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowText(IntPtr hWnd, String strNewWindowName);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, int dwSize, out int lpNumberOfBytesWritten);
    }
}
