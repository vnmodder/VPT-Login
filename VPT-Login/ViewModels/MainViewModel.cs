using Reactive.Bindings;
using System;
using KAutoHelper;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using VPT_Login.Models;
using System.IO;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using VPT_Login.Libs;
using System.Threading;
using System.Windows;
using AutoItX3Lib;


namespace VPT_Login.ViewModels
{
    public class MainViewModel
    {
        private string ipInterface;

        public ReactiveCommand ThemCommand { get; } = new ReactiveCommand();
        public ReactiveCommand XoaCommand { get; } = new ReactiveCommand();
        public ReactiveCommand VaoGameCommand { get; } = new ReactiveCommand();
        public ReactiveCommand CapNhatVerCommand { get; } = new ReactiveCommand();
        public ReactiveCommand CapNhatCommand { get; } = new ReactiveCommand();
        public ReactiveCommand VPNCommand { get; } = new ReactiveCommand();
        public ReactiveCommand BatPetCommand { get; } = new ReactiveCommand();
        public ReactiveCommand StopAutoCommand { get; } = new ReactiveCommand();
        public ReactiveCommand TatGameCommand { get; } = new ReactiveCommand();

        public ReactiveCommand RutboCommand { get; } = new ReactiveCommand();
        public ReactiveCommand DieuKhacCommand { get; } = new ReactiveCommand();
        public ReactiveCommand MatBaoCommand { get; } = new ReactiveCommand();
        public ReactiveCommand TrongNLCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<string> Ten { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Version { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Link { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> Status { get; } = new ReactiveProperty<bool>(true);
        //public ReactiveProperty<string> LogText { get; } = new ReactiveProperty<string>("");

        public Dictionary<string, string> PetList => Constant.PetList;
        public Dictionary<string, string> PetOptions => Constant.PetOptions;
        public Dictionary<string, string> NLOptions => Constant.NLOptions;
        public Dictionary<string, string> DoiNNOptions => Constant.DoiNNOptions;
        public Dictionary<string, string> DSCapMB => Constant.DicCapMB;
        public Dictionary<string, string> DSLoaiMB => Constant.DicLoaiMB;

        public ReactiveCollection<DataModel> Characters { get; set; } = new ReactiveCollection<DataModel>();
        public ReactiveProperty<DataModel> SelectedItem { get; } = new ReactiveProperty<DataModel>();

        public MainViewModel()
        {
            LoadFromXml();
            VaoGameCommand.Subscribe(() => VaoGame());
            ThemCommand.Subscribe(() => LuuData());
            XoaCommand.Subscribe(() => XoaData());
            CapNhatVerCommand.Subscribe(() => CapNhatVersion());
            CapNhatCommand.Subscribe(() => CapNhatThongTin());
            VPNCommand.Subscribe(() => ChayVPN());

            SelectedItem.Subscribe((i) => Itemchaged(i));

            BatPetCommand.Subscribe(() => buttonBatPet());
            StopAutoCommand.Subscribe(() => buttonStopAuto());
            TatGameCommand.Subscribe(() => buttonTatGame());

            RutboCommand.Subscribe(() => rutBo());
            DieuKhacCommand.Subscribe(() => dieuKhac());
            MatBaoCommand.Subscribe(() => matBao());
            TrongNLCommand.Subscribe(() => nguyenLieu());
        }

        private void nguyenLieu()
        {
            if (SelectedItem.Value == null) { return; }

            //SelectedItem.Value.HWnd.Value = (IntPtr)0x000506dc;

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, SelectedItem.Value.LogText);
            runTaskInThread(mainAuto.trongNL, "trongNL");
        }

        private void matBao()
        {
            if (SelectedItem.Value == null) { return; }

             //SelectedItem.Value.HWnd.Value = (IntPtr)0x000506dc;

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, SelectedItem.Value.LogText);
            runTaskInThread(mainAuto.runCheMatBao, "runCheMatBao");
        }

        private void dieuKhac()
        {
            if (SelectedItem.Value == null) { return; }

           // SelectedItem.Value.HWnd.Value = (IntPtr)0x0015086e;

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, SelectedItem.Value.LogText);
            runTaskInThread(mainAuto.khongGianDieuKhac, "khongGianDieuKhac");
        }

        private void rutBo()
        {
            if (SelectedItem.Value == null) { return; }

            //SelectedItem.Value.HWnd.Value = (IntPtr)0x00130bb4;

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, SelectedItem.Value.LogText);
            runTaskInThread(mainAuto.rutBo, "rutBo");
        }

        private void buttonTatGame()
        {
            if (SelectedItem.Value == null)
            {
                MessageBox.Show("Chưa chọn nhân vật để tắt.");
                return;
            }
            if (SelectedItem.Value.HWnd.Value == IntPtr.Zero || !Helper.IsWindow(SelectedItem.Value.HWnd.Value))
            {
                MessageBox.Show("Nhân vật đang chọn chưa vào game");
                return;
            }

            AutoItX3 au3 = new AutoItX3();
            au3.WinClose($"{SelectedItem.Value.Server}-{SelectedItem.Value.Name}");
            SelectedItem.Value.HWnd.Value = IntPtr.Zero;
        }

        private void ChayVPN()
        {
            try
            {
                string result = RunCmd("route print");
                int? vpnInterface = GetInterfaceIndexByName(result, "VPN Client Adapter");
                result = RunCmd("ipconfig");
                var (iface, gateway) = GetIPv4AndGatewayByAdapterKeyword(result, "VPN Client");
                ipInterface = iface;
                RunCmdAsAdmin($"route delete 0.0.0.0 mask 0.0.0.0 {gateway}");
                RunCmdAsAdmin($"route add 0.0.0.0 mask 0.0.0.0 {gateway} metric 60 if {vpnInterface}");
                MessageBox.Show("Đã thiết lập VPN");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi VPN" + ex.Message);
            }
        }

        static void RunCmdAsAdmin(string command)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c " + command,
                UseShellExecute = true,
                Verb = "runas", // yêu cầu quyền admin
                CreateNoWindow = true
            };

            try
            {
                using (var process = Process.Start(psi))
                {
                    process?.WaitForExit();
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1223)
                {
                    throw new Exception("Người dùng đã từ chối quyền admin (UAC).");
                }
                else
                {
                    throw;
                }
            }
        }

        static string RunCmd(string command, int timeoutMs = 10000)
        {
            var psi = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8
            };

            using (var process = new Process { StartInfo = psi })
            {
                StringBuilder outputBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();

                process.OutputDataReceived += (s, e) => { if (e.Data != null) outputBuilder.AppendLine(e.Data); };
                process.ErrorDataReceived += (s, e) => { if (e.Data != null) errorBuilder.AppendLine(e.Data); };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                if (!process.WaitForExit(timeoutMs))
                {
                    try { process.Kill(); } catch { }
                    throw new TimeoutException($"Lệnh '{command}' bị timeout sau {timeoutMs}ms.");
                }

                if (process.ExitCode != 0)
                {
                    throw new Exception($"Lệnh '{command}' thất bại:\n{errorBuilder}");
                }

                return outputBuilder.ToString();
            }
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

        private int? GetInterfaceIndexByName(string routePrintOutput, string keyword)
        {
            var lines = routePrintOutput.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            bool inInterfaceList = false;

            foreach (var line in lines)
            {
                // Tìm phần bắt đầu Interface List
                if (line.Contains("Interface List"))
                {
                    inInterfaceList = true;
                    continue;
                }

                // Kết thúc phần Interface List nếu đến dòng === hoặc IPv4 Route Table
                if (inInterfaceList && (line.Contains("====") || line.Contains("IPv4 Route Table")))
                    break;

                if (inInterfaceList && line.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var match = Regex.Match(line.Trim(), @"^(\d+)");
                    if (match.Success)
                    {
                        return int.Parse(match.Groups[1].Value);
                    }
                }
            }

            return null; // không tìm thấy
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
                MessageBox.Show("Vui lòng nhập Version cần cập nhật.");
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
                MessageBox.Show("Chưa chọn nhân vật để xóa.");
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

            var match = Regex.Match(link, @"\/(s\d{2,3})\/", RegexOptions.IgnoreCase);
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
                using (var writer = new StreamWriter(Constant.FilePath.XML_PATH, false, Encoding.UTF8))
                {
                    serializer.Serialize(writer, list);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }
        private void LoadFromXml()
        {
            try
            {
                if (File.Exists(Constant.FilePath.XML_PATH))
                {
                    var serializer = new XmlSerializer(typeof(List<DataModel>), new XmlRootAttribute("ArrayOfDataModel"));
                    using (var reader = new StreamReader(Constant.FilePath.XML_PATH))
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
                MessageBox.Show("Lỗi khi đọc: " + ex.Message);
            }
        }

        private void CapNhatThongTin()
        {
            var item = SelectedItem.Value;
            if (item == null)
            {
                MessageBox.Show("Chưa chọn dữ liệu để cập nhật.");
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
                MessageBox.Show("Chưa chọn nhân vật.");
                return;
            }
            if (SelectedItem.Value.HWnd.Value != IntPtr.Zero &&
                Helper.IsWindow(SelectedItem.Value.HWnd.Value))
            {
                MessageBox.Show("Nhân vật hiện đang được mở!");
                return;
            }

            string link = SelectedItem.Value?.Link;
            if (!string.IsNullOrEmpty(SelectedItem.Value?.Version))
            {
                link += ("&version=" + SelectedItem.Value?.Version);
            }
            try
            {
                if (SelectedItem.Value?.Status == 1)
                {
                    if (string.IsNullOrEmpty(ipInterface))
                    {
                        var result = RunCmd("ipconfig");
                        var (iface, gateway) = GetIPv4AndGatewayByAdapterKeyword(result, "VPN Client");
                        ipInterface = iface;
                    }

                    Process.Start(Constant.FilePath.FORCE_BIND_IP, $"{ipInterface} \"{Constant.FilePath.FLASH_PLAYER}\" {link}");
                }
                else
                {
                    Process.Start(Constant.FilePath.FLASH_PLAYER, SelectedItem.Value?.Link + "&version=" + SelectedItem.Value?.Version);
                }

                IntPtr defaultHWnd = IntPtr.Zero;
                string defaultWindowName = Constant.FLASH_NAME;

                // Thử tối đa 2 giây để tìm cửa sổ
                for (int i = 0; i < 40; i++)
                {
                    defaultHWnd = AutoControl.FindWindowHandle(null, defaultWindowName);
                    if (defaultHWnd != IntPtr.Zero)
                    {
                        SelectedItem.Value.HWnd.Value = defaultHWnd;
                        Helper.SetWindowText(defaultHWnd, SelectedItem.Value?.Server + "-" + SelectedItem.Value?.Name);
                        break;
                    }

                    Thread.Sleep(100);
                }

                if (defaultHWnd == IntPtr.Zero)
                {
                    MessageBox.Show("Không tìm thấy cửa sổ Flash.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void buttonBatPet()
        {
            if (SelectedItem.Value == null) { return; }

            //SelectedItem.Value.HWnd.Value = (IntPtr)0x00020922;

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, SelectedItem.Value.LogText);
            runTaskInThread(mainAuto.batPet, "batPet");
        }

        private void runTaskInThread(ThreadStart action, String actionName)
        {
            Helper.ThreadList.Add(new Thread(action));
            int index = Helper.ThreadList.Count() - 1;
            Helper.ThreadList[index].Name = $"{SelectedItem.Value?.Server}-{SelectedItem.Value?.Name}:" + actionName;
            Helper.ThreadList[index].Start();
        }

        private void buttonStopAuto()
        {

            foreach (var thread in Helper.ThreadList)
            {
                if (thread.Name.Contains($"{SelectedItem.Value?.Name}"))
                {
                    Helper.WriteStatus(SelectedItem.Value.LogText, $"{SelectedItem.Value?.Server}-{SelectedItem.Value?.Name}", "Đã ngừng auto");
                    thread.Abort();
                }
            }
        }
    }
}
