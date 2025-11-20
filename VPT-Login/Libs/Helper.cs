using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using VPT_Login.Models;

namespace VPT_Login.Libs
{
    public static class Helper
    {
        private const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
        public static List<Thread> ThreadList = new List<Thread>();


        public static void WriteStatus(ReactiveProperty<string> textBox, string id, string statusText)
        {
            try
            {
                System.Windows.Application.Current?.Dispatcher?.Invoke(() =>
                {
                    if(textBox == null) { return; }
                    textBox.Value += (id + ": " + statusText + Environment.NewLine);
                });
            }
            catch
            {

            }

        }

        public static int GenerateNextId(List<DataModel> list)
        {
            int id = 1;
            var usedIds = new HashSet<int>(list.Select(x => x.Id.Value));
            while (usedIds.Contains(id))
            {
                id++;
            }
            return id;
        }

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

        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress,
            out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public uint AllocationProtect;
        public IntPtr RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }
}
