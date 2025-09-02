using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace VPT_Login.Libs
{
    public static class Helper
    {
        public static List<Thread> ThreadList = new List<Thread>();

        public static void WriteStatus(RichTextBox textBox, string id, string statusText)
        {
            try
            {
                textBox.BeginInvoke(new Action(() => textBox.AppendText(id + ": " + statusText + Environment.NewLine)));
            }
            catch
            {

            }

        }

        public static void ShowAlert(string id, string message)
        {
            MessageBox.Show(id + ": " + message);
        }


    }
}
