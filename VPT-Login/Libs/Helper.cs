using MahApps.Metro.Controls;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

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
    }
}
