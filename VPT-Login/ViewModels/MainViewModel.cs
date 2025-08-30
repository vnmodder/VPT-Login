using Reactive.Bindings;
using System;
using KAutoHelper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using VPT_Login.Models;
using System.IO;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace VPT_Login.ViewModels
{
    public class MainViewModel
    {
        private string xmlPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "data.vpt");
        private string forceBindIP = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "forceBindIP.exe");
        private string exePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "flash.exe");


        public ReactiveCommand ThemCommand { get; } = new ReactiveCommand();
        public ReactiveCommand XoaCommand { get; } = new ReactiveCommand();
        public ReactiveCommand VaoGameCommand { get; } = new ReactiveCommand();
        public ReactiveCommand CapNhatVerCommand { get; } = new ReactiveCommand();
        public ReactiveCommand CapNhatCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<string> Ten { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Version { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Link { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> Status { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<DataModel> SelectedItem { get;  } = new ReactiveProperty<DataModel>();


        public ObservableCollection<DataModel> Characters { get; set; } = new ObservableCollection<DataModel>();

      
        [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowText(IntPtr hWnd, String strNewWindowName);

        public MainViewModel()
        {
            LoadFromXml();
            VaoGameCommand.Subscribe(() => VaoGame());
            ThemCommand.Subscribe(() => LuuData());
            XoaCommand.Subscribe(() => XoaData());
            CapNhatVerCommand.Subscribe(() => CapNhatVersion());
            CapNhatCommand.Subscribe(() => CapNhatThongTin());
            SelectedItem.Subscribe((i) => Itemchaged(i));
        }

        private void Itemchaged(DataModel i)
        {
            if(i == null) return;

            Ten.Value = i.Name;
            Version.Value = i.Version;
            Link.Value = i.Link;
            Status.Value = i.Status == 1;

        }

        private void CapNhatVersion()
        {
            if (string.IsNullOrWhiteSpace(Version.Value))
            {
                System.Windows.MessageBox.Show("Vui lòng nhập Version cần cập nhật.");
                return;
            }

            foreach (var item in Characters)
            {
                item.Version = Version.Value;
            }

            SaveToXml();
        }

        private void XoaData()
        {
            if (SelectedItem.Value == null)
            {
                System.Windows.MessageBox.Show("Chưa chọn nhân vật để xóa.");
                return;
            }

            var itemToRemove = SelectedItem.Value;

            if (Characters.Contains(itemToRemove))
            {
                Characters.Remove(itemToRemove);

                // Cập nhật lại No cho đẹp
                for (int i = 0; i < Characters.Count; i++)
                {
                    Characters[i].No = i + 1;
                }

                SaveToXml();
                SelectedItem.Value = null;
            }
        }
        private void LuuData()
        {
            try
            {
                // Bước 1: Lấy server từ Link
                string link = Link.Value?.Trim() ?? string.Empty;
                string server = "Unknown";

                var match = System.Text.RegularExpressions.Regex.Match(link, @"\/(s\d{2,3})\/", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    server = match.Groups[1].Value.ToUpper(); 
                }

                // Bước 2: Tạo đối tượng mới
                var newItem = new DataModel
                {
                    No = Characters.Count + 1,
                    Name = Ten.Value?.Trim() ?? string.Empty,
                    Server = server,
                    Version = Version.Value?.Trim() ?? string.Empty,
                    Link = link,
                    Status = Status.Value ? 1 : 0
                };

                // Bước 3: Thêm vào danh sách và lưu
                Characters.Add(newItem);
                SaveToXml();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }

        private void SaveToXml()
        {
            var list = Characters.ToList();
            var serializer = new XmlSerializer(typeof(List<DataModel>), new XmlRootAttribute("ArrayOfDataModel"));
            using (var writer = new StreamWriter(xmlPath, false, Encoding.UTF8))
            {
                serializer.Serialize(writer, list);
            }

            LoadFromXml();
        }
        private void LoadFromXml()
        {
            if (File.Exists(xmlPath))
            {
                var serializer = new XmlSerializer(typeof(List<DataModel>), new XmlRootAttribute("ArrayOfDataModel"));
                using (var reader = new StreamReader(xmlPath))
                {
                    var data = serializer.Deserialize(reader) as List<DataModel>;
                    if (data != null)
                    {
                        Characters.Clear();
                        foreach (var item in data)
                            Characters.Add(item);
                    }
                }
            }

            int idx = Characters.IndexOf(SelectedItem.Value);
            if (idx > 0)
                SelectedItem.Value = Characters[idx];
        }

        private void CapNhatThongTin()
        {
            var item = SelectedItem.Value;
            if (item == null)
            {
                System.Windows.MessageBox.Show("Chưa chọn dữ liệu để cập nhật.");
                return;
            }

            item.Name = (Ten.Value ?? "").Trim();
            item.Version = (Version.Value ?? "").Trim();
            item.Link = (Link.Value ?? "").Trim();
            item.Status = Status.Value ? 1 : 0;

            var match = Regex.Match(item.Link ?? "", @"\/(s\d{2,3})\/", RegexOptions.IgnoreCase);
            if (match.Success) item.Server = match.Groups[1].Value.ToUpper();

            SaveToXml();
        }

        private void VaoGame()
        {
            if (SelectedItem.Value == null || string.IsNullOrEmpty(SelectedItem.Value?.Link))
            {
                System.Windows.MessageBox.Show("Chưa chọn nhân vật để xóa.");
                return;
            }

            try
            {
                Process.Start(exePath, SelectedItem.Value?.Link + "&version=" + SelectedItem.Value?.Version);

                IntPtr defaultHWnd = IntPtr.Zero;
                string defaultWindowName = "Adobe Flash Player 32";

                // Thử tối đa 2 giây để tìm cửa sổ
                for (int i = 0; i < 20; i++)
                {
                    defaultHWnd = AutoControl.FindWindowHandle(null, defaultWindowName);
                    if (defaultHWnd != IntPtr.Zero)
                    {
                        SetWindowText(defaultHWnd, SelectedItem.Value?.Server +"-"+ SelectedItem.Value?.Name);
                        break;
                    }

                    System.Threading.Thread.Sleep(100);
                }

                if (defaultHWnd == IntPtr.Zero)
                {
                    System.Windows.MessageBox.Show("Không tìm thấy cửa sổ Flash.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}
