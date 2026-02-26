using Reactive.Bindings;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using VPT_Login.Libs;

namespace VPT_Login.Views
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public ReactiveProperty<string> LogText { get; } = new ReactiveProperty<string>("");
        public ReactiveCommand TestCommand { get; } = new ReactiveCommand();

        public AutoFeatures mAuto;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        const int SW_RESTORE = 9;

        public Test()
        {
            InitializeComponent();
            DataContext = this;


            TestCommand.Subscribe(() => TestAAA());
        }

        private void TestAAA()
        {
            //var img = Constant.ImagePathBatPetFolder + "char_dy.png";
            //var a = mAuto.FindImages(img);
            ////{X = 537 Y = 461}
            ////{ X = 537 Y = 493}
            //LogText.Value += a.FirstOrDefault().ToString();

            ResizeWindow(Constant.hwnd, 100, 100, 400, 285);

            int i = 0;
        }


        public static bool ResizeWindow(IntPtr hWnd, int x, int y, int width, int height)
        {
            if (hWnd == IntPtr.Zero)
                return false;

            // Đưa cửa sổ về trạng thái bình thường (tránh lỗi nếu đang maximize)
            ShowWindow(hWnd, SW_RESTORE);

            // Resize + di chuyển
            return MoveWindow(hWnd, x, y, width, height, true);
        }
    }
}
