using Reactive.Bindings;
using System;
using System.Linq;
using System.Windows;
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
