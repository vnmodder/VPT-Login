using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
        public ReactiveCommand TestCommand { get; } = new ReactiveCommand();

        public AutoFeatures mAuto;


        public Test()
        {
            InitializeComponent();
            DataContext = this;

            var aaa = new DataModel();
            aaa.Name = "Test";
            aaa.Server = "120";
            aaa.HWnd.Value = (IntPtr)0x00020922;
            mAuto = new AutoFeatures(aaa, "S120-Danh Y 120", LogText);

            TestCommand.Subscribe(() => TestAAA());
        }

        private void TestAAA()
        {
            var img = Constant.ImagePathBatPetFolder + "char_dy.png";
            var a = mAuto.FindImages(img);
            //{X = 537 Y = 461}
            //{ X = 537 Y = 493}
            LogText.Value += a.FirstOrDefault().ToString();

            int i = 0;
        }
    }
}
