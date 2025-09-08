using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Input;
using VPT_Login.ViewModels;

namespace VPT_Login
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void dgAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.VaoGameCommand.CanExecute())
            {
                vm.VaoGameCommand.Execute();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.ScrollToEnd(); // Cuộn xuống cuối mỗi khi Text thay đổi
            }
        }
    }
}
