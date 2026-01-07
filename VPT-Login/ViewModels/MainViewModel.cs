using KAutoHelper;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Serialization;
using VPT_Login.Models;

namespace VPT_Login.ViewModels
{
    public class MainViewModel
    {
        private string xmlPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "data.vpt");
        private string exePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "flash.exe");

        public ReactiveCommand ThemCommand { get; } = new ReactiveCommand();
        public ReactiveCommand XoaCommand { get; } = new ReactiveCommand();
        public ReactiveCommand VaoGameCommand { get; } = new ReactiveCommand();
        public ReactiveCommand CapNhatVerCommand { get; } = new ReactiveCommand();
        public ReactiveCommand CapNhatCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ThaiCoCommand { get; } = new ReactiveCommand();
        public ReactiveCommand HuyThaiCoCommand { get; } = new ReactiveCommand();
        public ReactiveCommand StatusCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<string> Ten { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Version { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Link { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> Status { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<DataModel> SelectedItem { get; } = new ReactiveProperty<DataModel>();


        public ObservableCollection<DataModel> Characters { get; set; } = new ObservableCollection<DataModel>();


        [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Unicode)]
        public static extern bool SetWindowText(IntPtr hWnd, String strNewWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public MainViewModel()
        {
            LoadFromXml();
            VaoGameCommand.Subscribe(() => VaoGame());
            ThemCommand.Subscribe(() => LuuData());
            XoaCommand.Subscribe(() => XoaData());
            CapNhatVerCommand.Subscribe(() => CapNhatVersion());
            CapNhatCommand.Subscribe(() => CapNhatThongTin());
            SelectedItem.Subscribe((i) => Itemchaged(i));


            ThaiCoCommand.Subscribe(() => ThaiCo());
            HuyThaiCoCommand.Subscribe(() => HuyThaiCo());
            StatusCommand.Subscribe(() => statusUpdate());
        }

        private void HuyThaiCo()
        {
            foreach (var thread in Helper.ThreadList)
            {
                if (thread.Name.Contains($"{SelectedItem.Value.Name}"))
                {
                    SelectedItem.Value.Active.Value = false;
                    thread.Abort();
                }
            }
        }

        private void ThaiCo()
        {
            if (SelectedItem.Value == null)
                return;

            Helper.ThreadList.Add(new Thread(AutoThaiCo));
            int index = Helper.ThreadList.Count() - 1;
            Helper.ThreadList[index].Name = $"{SelectedItem.Value?.Server}-{SelectedItem.Value?.Name}:" + "thaico";
            Helper.ThreadList[index].Start();
            SelectedItem.Value.Active.Value = true;
        }

        public static (string ip, string gateway) GetIPv4AndGatewayByAdapterKeyword(string ipconfigOutput, string keyword)
        {
            var lines = ipconfigOutput.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            bool inTargetBlock = false;
            string ip = null;
            string gateway = null;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                // Nếu là bắt đầu 1 adapter mới
                if (line.IndexOf("adapter", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    inTargetBlock = line.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
                    ip = null;
                    gateway = null;
                }

                if (inTargetBlock)
                {
                    // Dòng chứa IPv4
                    if (line.IndexOf("IPv4", StringComparison.OrdinalIgnoreCase) >= 0 && ip == null)
                    {
                        var match = Regex.Match(line, @"(\d{1,3}(?:\.\d{1,3}){3})");
                        if (match.Success)
                            ip = match.Groups[1].Value;
                    }

                    // Dòng chứa Gateway
                    if (line.IndexOf("Default Gateway", StringComparison.OrdinalIgnoreCase) >= 0 && gateway == null)
                    {
                        // Trường hợp gateway nằm ngay trên dòng
                        var match = Regex.Match(line, @"(\d{1,3}(?:\.\d{1,3}){3})");
                        if (match.Success)
                        {
                            gateway = match.Groups[1].Value;
                        }
                        else
                        {
                            // Có thể gateway nằm dòng kế tiếp
                            if (i + 1 < lines.Length)
                            {
                                var matchNext = Regex.Match(lines[i + 1], @"(\d{1,3}(?:\.\d{1,3}){3})");
                                if (matchNext.Success)
                                    gateway = matchNext.Groups[1].Value;
                            }
                        }
                    }

                    if (ip != null && gateway != null)
                        return (ip, gateway);
                }
            }

            return (null, null); // không tìm thấy
        }

        private void statusUpdate()
        {
            foreach (var item in Characters)
            {
                string windowName = item.Server + "-" + item.Name + "- Liên hệ mua auto: https://discord.gg/ExTvNzCnkA";
                item.HWnd.Value = FindWindow(null, windowName);
            }
        }

        private void Itemchaged(DataModel i)
        {
            if (i == null) return;

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
            LoadFromXml();
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
                LoadFromXml();
                SelectedItem.Value = null;
            }
        }
        private void LuuData()
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
            LoadFromXml();

        }

        private void SaveToXml()
        {
            try
            {
                var list = Characters.ToList();
                var serializer = new XmlSerializer(typeof(List<DataModel>), new XmlRootAttribute("ArrayOfDataModel"));
                using (var writer = new StreamWriter(xmlPath, false, Encoding.UTF8))
                {
                    serializer.Serialize(writer, list);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }
        private void LoadFromXml()
        {
            try
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
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Lỗi khi đọc: " + ex.Message);
            }
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
            LoadFromXml();
        }

        private void VaoGame()
        {
            if (SelectedItem.Value == null || string.IsNullOrEmpty(SelectedItem.Value?.Link))
            {
                System.Windows.MessageBox.Show("Chưa chọn nhân vật.");
                return;
            }
            string link = SelectedItem.Value?.Link;
            if (!string.IsNullOrEmpty(SelectedItem.Value?.Version))
            {
                link += ("&version=" + SelectedItem.Value?.Version);
            }
            try
            {
                Process.Start(exePath, SelectedItem.Value?.Link + "&version=" + SelectedItem.Value?.Version);
                //if (SelectedItem.Value?.Status == 1)
                //{
                //    Process.Start(forceBindIP, $"{ipInterface} \"{exePath}\" {link}");
                //}
                //else
                //{
                //    Process.Start(exePath, SelectedItem.Value?.Link + "&version=" + SelectedItem.Value?.Version);
                //}
                IntPtr defaultHWnd = IntPtr.Zero;
                string defaultWindowName = "Adobe Flash Player 10";

                // Thử tối đa 2 giây để tìm cửa sổ
                for (int i = 0; i < 20; i++)
                {
                    defaultHWnd = AutoControl.FindWindowHandle(null, defaultWindowName);
                    if (defaultHWnd != IntPtr.Zero)
                    {
                        SelectedItem.Value.HWnd.Value = defaultHWnd;
                        SetWindowText(defaultHWnd, SelectedItem.Value?.Server + "-" + SelectedItem.Value?.Name + "- Liên hệ mua auto: https://discord.gg/ExTvNzCnkA");
                        break;
                    }

                    System.Threading.Thread.Sleep(500);
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

        private void AutoThaiCo()
        {
            if (SelectedItem.Value.HWnd.Value == IntPtr.Zero)
            {
                return;
            }

            AutoFeatures mAuto = new AutoFeatures(SelectedItem.Value);

            mAuto.ClickImageByGroup("global", "conghoi", false, true);

            int count = 0;
            while (true)
            {
                if (!mAuto.FindImageByGroup("global", "khongtrongtrandau", false, true))
                {
                    Thread.Sleep(Constant.TimeMedium);
                    continue;
                }

                mAuto.CloseAllDialog();
                mAuto.ClickImageByGroup("global", "outbattletatauto", percent: .9);

                Thread.Sleep(Constant.VeryTimeShort);
                mAuto.ClickImageByGroup("global", "outbattletatauto_not", true, true);
                Thread.Sleep(Constant.VeryTimeShort);

                mAuto.ClickImageByGroup("global", "outbattletatauto_not", true, true);

                mAuto.ClickImageByGroup("global", "thugon_kynang");
                mAuto.ClickImageByGroup("global", "inbattleauto");

            huynv:
                int i = 0;
                while (!mAuto.FindImageByGroup("thai_co", "nhiemvu_check") && i < 5)
                {
                    mAuto.CloseAllDialog();
                    if (i % 2 == 0)
                        mAuto.ClickImageByGroup("thai_co", "nhiemvu");
                    else
                        mAuto.SendKey(Keys.Q);
                    i++;
                    Thread.Sleep(Constant.VeryTimeShort);
                }

                Thread.Sleep(Constant.TimeShort);
                if (mAuto.FindImageByGroup("thai_co", "nv_phu", false, true, percent: .9) &&
                    !mAuto.FindImageByGroup("thai_co", "chonhuy", false, true, percent: .9))
                    mAuto.ClickImageByGroup("thai_co", "nv_phu", percent: .9);

                if (mAuto.FindImageByGroup("thai_co", "chonhuy", false, true))
                {
                    Thread.Sleep(Constant.VeryTimeShort);
                    mAuto.ClickImageByGroup("thai_co", "chonhuy", false, true);

                    Thread.Sleep(Constant.VeryTimeShort);
                    mAuto.ClickImageByGroup("thai_co", "huy_nv", false, true);

                    Thread.Sleep(Constant.VeryTimeShort);

                    if (mAuto.FindImageByGroup("global", "xacnhanco2", false, true))
                        mAuto.ClickImageByGroup("global", "xacnhanco2", false, true);
                    else if (mAuto.FindImageByGroup("global", "xacnhanco", false, true))
                        mAuto.ClickImageByGroup("global", "xacnhanco", false, true);
                }
                Thread.Sleep(Constant.VeryTimeShort);

                i = 0;
                while (!mAuto.FindImageByGroup("thai_co", "npc_thaico") && i < 10)
                {
                    mAuto.CloseAllDialog();
                    if (mAuto.FindImageByGroup("thai_co", "thaico_1"))
                        mAuto.ClickImageByGroup("thai_co", "thaico_1");
                    else if (mAuto.FindImageByGroup("thai_co", "thaico_2"))
                        mAuto.ClickImageByGroup("thai_co", "thaico_2");
                    else if (mAuto.FindImageByGroup("thai_co", "thaico_3"))
                        mAuto.ClickImageByGroup("thai_co", "thaico_3");
                    else if (mAuto.FindImageByGroup("thai_co", "thaico_4"))
                        mAuto.ClickImageByGroup("thai_co", "thaico_4");
                    else if (mAuto.FindImageByGroup("thai_co", "thaico_5"))
                        mAuto.ClickImageByGroup("thai_co", "thaico_5");
                    i++;
                    Thread.Sleep(Constant.VeryTimeShort);
                }

                Thread.Sleep(Constant.VeryTimeShort);
                if (!mAuto.FindImageByGroup("thai_co", "nv_thaico", false, true))
                {
                    goto huynv;
                }
                i = 0;

                while (!mAuto.FindImageByGroup("thai_co", "nhan_nv", false, true) && i < 5)
                {
                    Thread.Sleep(Constant.TimeShort);
                    mAuto.ClickImageByGroup("thai_co", "nv_thaico", false, true);
                    i++;
                }
                Thread.Sleep(Constant.VeryTimeShort);
                mAuto.ClickImageByGroup("thai_co", "nhan_nv", false, true);
                Thread.Sleep(Constant.TimeShort);

                i = 0;
                while (mAuto.FindImageByGroup("global", "khongtrongtrandau", false, true) && i < 10)
                {
                    if (mAuto.FindImageByGroup("thai_co", "thaico_1"))
                        mAuto.ClickImageByGroup("thai_co", "thaico_1");
                    else if (mAuto.FindImageByGroup("thai_co", "thaico_2"))
                        mAuto.ClickImageByGroup("thai_co", "thaico_2");
                    else if (mAuto.FindImageByGroup("thai_co", "thaico_3"))
                        mAuto.ClickImageByGroup("thai_co", "thaico_3");
                    else if (mAuto.FindImageByGroup("thai_co", "thaico_4"))
                        mAuto.ClickImageByGroup("thai_co", "thaico_4");
                    else if (mAuto.FindImageByGroup("thai_co", "thaico_5"))
                        mAuto.ClickImageByGroup("thai_co", "thaico_5");
                    i++;

                    Thread.Sleep(Constant.VeryTimeShort);
                }

                count++;
            }

        }
    }
}
