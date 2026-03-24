using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Reactive.Bindings;
using VPT_Login.Libs;
using VPT_Login.Models;

namespace VPT_Login.Views
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public ReactiveProperty<string> LogText { get; } = new ReactiveProperty<string>("");
        public ReactiveProperty<int> Value1 { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> Value2 { get; } = new ReactiveProperty<int>(0);

        public ReactiveCommand TestCommand { get; } = new ReactiveCommand();
        public ReactiveCommand TestCommand2 { get; } = new ReactiveCommand();

        public AutoFeatures mAuto;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);


        const int SW_RESTORE = 9;

        public Test(DataModel auto)
        {
            InitializeComponent();
            DataContext = this;
            mAuto = new AutoFeatures(auto);

            TestCommand.Subscribe(() => TestAAA());
            TestCommand2.Subscribe(() => TestAAA2());
        }

        private void TestAAA2()
        {
            mAuto.ClickPoint(Value1.Value, Value2.Value, isRightClick: true);
        }

        private void TestAAA()
        {
            //var img = Constant.ImagePathBatPetFolder + "char_dy.png";
            //var a = mAuto.FindImages(img);
            ////{X = 537 Y = 461}
            ////{ X = 537 Y = 493}
            //LogText.Value += a.FirstOrDefault().ToString();

            ResizeWindow(Constant.hwnd, 400, 285);

            int i = 0;
        }


        public static bool ResizeWindow(IntPtr hWnd, int width, int height)
        {
            if (hWnd == IntPtr.Zero)
                return false;
            ShowWindow(hWnd, SW_RESTORE);
            if (!GetWindowRect(hWnd, out RECT rect))
                return false;
            return MoveWindow(hWnd, rect.Left, rect.Top, width, height, true);
        }
    }
}
