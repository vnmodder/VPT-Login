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
        public static List<Thread> ThreadList = new List<Thread>();

        public static void WriteStatus(ReactiveProperty<string> textBox, string id, string statusText)
        {
            try
            {
                System.Windows.Application.Current?.Dispatcher?.Invoke(() =>
                {
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

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowText(IntPtr hWnd, String strNewWindowName);
    }
}
