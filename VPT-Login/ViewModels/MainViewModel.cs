using KAutoHelper;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI.WebControls;
using System.Windows;
using System.Xml.Serialization;
using VPT_Login.Libs;
using VPT_Login.Models;
using static Emgu.CV.OCR.Tesseract;


namespace VPT_Login.ViewModels
{
    public class MainViewModel
    {
        private string ipInterface;

        public ReactiveCommand ThemCommand { get; } = new ReactiveCommand();
        public ReactiveCommand XoaCommand { get; } = new ReactiveCommand();
        public ReactiveCommand VaoGameCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ChayAllCommand { get; } = new ReactiveCommand();
        public ReactiveCommand CapNhatVerCommand { get; } = new ReactiveCommand();
        public ReactiveCommand CapNhatCommand { get; } = new ReactiveCommand();
        public ReactiveCommand VPNCommand { get; } = new ReactiveCommand();
        public ReactiveCommand BatPetCommand { get; } = new ReactiveCommand();
        public ReactiveCommand StopAutoCommand { get; } = new ReactiveCommand();
        public ReactiveCommand StopAllAutoCommand { get; } = new ReactiveCommand();
        public ReactiveCommand StatusCommand { get; } = new ReactiveCommand();
        public ReactiveCommand TrainMapCommand { get; } = new ReactiveCommand();
        public ReactiveCommand XuQueCommand { get; } = new ReactiveCommand();
        public ReactiveCommand LatTheCommand { get; } = new ReactiveCommand();
        public ReactiveCommand KhoiPhucCommand { get; } = new ReactiveCommand();
        public ReactiveCommand NuoiTLCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ThaiCoCommand { get; } = new ReactiveCommand();

        public ReactiveCommand LuuCaiDatCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RutboCommand { get; } = new ReactiveCommand();
        public ReactiveCommand DieuKhacCommand { get; } = new ReactiveCommand();
        public ReactiveCommand MatBaoCommand { get; } = new ReactiveCommand();
        public ReactiveCommand TrongNLCommand { get; } = new ReactiveCommand();
        public ReactiveCommand HanhLangCommand { get; } = new ReactiveCommand();
        public ReactiveCommand TuHanhCommand { get; } = new ReactiveCommand();
        public ReactiveCommand PhuBanCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RunCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ResetAutoCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ExitingCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<string> Ten { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Version { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Link { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> Status { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> IsBlocked { get; } = new ReactiveProperty<bool>(false);
        //public ReactiveProperty<string> LogText { get; } = new ReactiveProperty<string>("");

        public Dictionary<string, string> PetList => Constant.PetList;
        public Dictionary<string, string> PetOptions => Constant.PetOptions;
        public Dictionary<string, string> NLOptions => Constant.NLOptions;
        public Dictionary<string, string> DoiNNOptions => Constant.DoiNNOptions;
        public Dictionary<string, string> DSCapMB => Constant.DicCapMB;
        public Dictionary<string, string> DSLoaiMB => Constant.DicLoaiMB;
        public Dictionary<string, string> ResetAutoOptionsList => Constant.ResetAutoOptions;

        public ReactiveCollection<DataModel> Characters { get; set; } = new ReactiveCollection<DataModel>();
        public ReactiveProperty<DataModel> SelectedItem { get; } = new ReactiveProperty<DataModel>();

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
            VPNCommand.Subscribe(() => ChayVPN());
            StatusCommand.Subscribe(() => statusUpdate());
            TrainMapCommand.Subscribe(() => trainMap());

            SelectedItem.Subscribe((i) => Itemchaged(i));

            BatPetCommand.Subscribe(() => buttonBatPet());
            StopAutoCommand.Subscribe(() => buttonStopAuto());
            StopAllAutoCommand.Subscribe(() => dungTatCa());
            ChayAllCommand.Subscribe(() => runAll());

            RutboCommand.Subscribe(() => rutBo());
            DieuKhacCommand.Subscribe(() => dieuKhac());
            MatBaoCommand.Subscribe(() => matBao());
            TrongNLCommand.Subscribe(() => nguyenLieu());
            HanhLangCommand.Subscribe(() => nhanThuongHanhLang());
            TuHanhCommand.Subscribe(() => runAutoTuHanh());
            PhuBanCommand.Subscribe(() => runNhanAutoPB());
            RunCommand.Subscribe(() => ChayHet());
            LuuCaiDatCommand.Subscribe(() => SaveToXml());
            XuQueCommand.Subscribe(() => xuQue());
            LatTheCommand.Subscribe(() => latThe());
            KhoiPhucCommand.Subscribe(() => khoiPhuc());
            NuoiTLCommand.Subscribe(() => nuoiTL());
            ThaiCoCommand.Subscribe(() => thaiCo());

            IsBlocked.Subscribe((x) => dongBangAuto(x));
            ExitingCommand.Subscribe((x) => dongCuaSo());
        }

        private void dongCuaSo()
        {
            foreach (var thread in Helper.ThreadList)
            {
                thread.Abort();
            }
        }

        private void dongBangAuto(bool x)
        {
            if (x == false)
            {
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.runResetAuto, "runResetAuto");
        }

        private void thaiCo()
        {
            if (SelectedItem.Value == null) { return; }
            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.thaiCo, "thaiCo");
        }

        private void nuoiTL()
        {
            if (SelectedItem.Value == null) { return; }
            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.nuoiTL, "nuoiTL");
        }

        private void khoiPhuc()
        {
            if (SelectedItem.Value == null) { return; }

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.hoiPhuc, "hoiPhuc");
        }

        private void latThe()
        {
            if (SelectedItem.Value == null) { return; }

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.AutoLatThe, "AutoLatThe");
        }

        private void xuQue()
        {
            if (SelectedItem.Value == null) { return; }

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.xuQue, "xuQue");
        }

        private void trainMap()
        {
            if (SelectedItem.Value == null) { return; }

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.trainMap, "trainMap");
        }

        private void statusUpdate()
        {
            foreach (var item in Characters)
            {
                string windowName = item?.Id.Value + "-" + item?.Server.Value + "-" + item?.Name.Value + "\t" + "Liên hệ auto: https://facebook.com/groups/VPT.TQ.S120";
                item.HWnd.Value = FindWindow(null, windowName);
            }
        }

        private void runAll()
        {

            var selectedAccs = Characters.Where(x => x.IsChecked.Value).ToList();

            foreach (var character in selectedAccs)
            {
                if (character.HWnd.Value == IntPtr.Zero || !Helper.IsWindow(character.HWnd.Value))
                {
                    MessageBox.Show($"Không tìm thấy cửa sổ cho nhân vật: {character.Name.Value}");
                    continue;
                }

                var mainAuto = new MainAuto(character, Characters, IsBlocked);
                Thread autoThread = new Thread(mainAuto.ChayHet);
                autoThread.Name = $"{character.Server.Value}-{character.Name.Value}:ChayHet";
                autoThread.IsBackground = true;
                autoThread.Start();

                Helper.ThreadList.Add(autoThread);
            }

        }

        private void ChayHet(DataModel model = null)
        {
            if (model == null)
            {
                model = SelectedItem.Value;
            }

            if (model == null) { return; }

            IntPtr hWnd = model.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.ChayHet, "ChayHet");
        }

        private void nguyenLieu()
        {
            if (SelectedItem.Value == null) { return; }

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.trongNL, "trongNL");
        }

        private void runNhanAutoPB()
        {
            if (SelectedItem.Value == null) { return; }

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.runNhanAutoPB, "runNhanAutoPB");
        }

        private void matBao()
        {
            if (SelectedItem.Value == null) { return; }

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.runCheMatBao, "runCheMatBao");
        }

        private void nhanThuongHanhLang()
        {
            if (SelectedItem.Value == null) { return; }

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.nhanThuongHanhLang, "nhanThuongHanhLang");
        }

        private void runAutoTuHanh()
        {
            if (SelectedItem.Value == null) { return; }

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.runAutoTuHanh, "runAutoTuHanh");
        }

        private void dieuKhac()
        {
            if (SelectedItem.Value == null) { return; }

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.khongGianDieuKhac, "khongGianDieuKhac");
        }

        private void rutBo()
        {
            if (SelectedItem.Value == null) { return; }

            IntPtr hWnd = SelectedItem.Value.HWnd.Value;
            if (hWnd == IntPtr.Zero)
            {
                MessageBox.Show("Không tìm thấy nhân vật này đang được chạy.");
                return;
            }
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
            runTaskInThread(mainAuto.rutBo, "rutBo");
        }

        private void dungTatCa()
        {
            foreach (var item in Characters.Where(x => x.IsChecked.Value))
            {
                string matchKey = item.Name.Value;

                foreach (var thread in Helper.ThreadList.ToList())
                {
                    if (thread.IsAlive && thread.Name != null && thread.Name.Contains(matchKey))
                    {
                        Helper.WriteStatus(item.LogText, matchKey, "Đã ngừng auto");
                        try { thread.Abort(); } catch { }
                    }
                }
            }
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

            Ten.Value = i.Name.Value;
            Version.Value = i.Version.Value;
            Link.Value = i.Link.Value;
            Status.Value = i.Status.Value == 1;

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
                item.Version.Value = Version.Value;
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

            Characters.Remove(SelectedItem.Value);
            SaveToXml(); // Không load lại
            SelectedItem.Value = null;
        }

        private void LuuData()
        {

            string link = Link.Value?.Trim() ?? string.Empty;
            string server = "Unknown";
            var match = Regex.Match(link, @"\/(s\d{2,3})\/", RegexOptions.IgnoreCase);
            if (match.Success) server = match.Groups[1].Value.ToUpper();

            // Tạo Id mới chưa trùng
            int newId = Helper.GenerateNextId(Characters.ToList());

            var newItem = new DataModel(newId, Ten.Value?.Trim() ?? "", server, Version.Value?.Trim() ?? "", link, Status.Value ? 1 : 0);

            Characters.Add(newItem);
            SaveToXml();

        }

        private void SaveToXml()
        {
            try
            {
                var xmlList = Characters.Select(c => c.ToXmlModel()).ToList();
                var serializer = new XmlSerializer(typeof(List<XMLDataModel>), new XmlRootAttribute("ArrayOfDataModel"));
                using (var writer = new StreamWriter(Constant.FilePath.XML_PATH, false, Encoding.UTF8))
                {
                    serializer.Serialize(writer, xmlList);
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
                    var serializer = new XmlSerializer(typeof(List<XMLDataModel>), new XmlRootAttribute("ArrayOfDataModel"));
                    using (var reader = new StreamReader(Constant.FilePath.XML_PATH))
                    {
                        var data = serializer.Deserialize(reader) as List<XMLDataModel>;
                        if (data != null)
                        {
                            foreach (var xmlItem in data)
                            {
                                var existing = Characters.FirstOrDefault(c => c.Id.Value == xmlItem.Id);
                                if (existing == null)
                                {
                                    // Thêm mới
                                    Characters.Add(new DataModel(xmlItem));
                                }
                                else
                                {

                                    existing.Name.Value = xmlItem.Name;
                                    existing.Server.Value = xmlItem.Server;
                                    existing.Version.Value = xmlItem.Version;
                                    existing.Link.Value = xmlItem.Link;
                                    existing.Status.Value = xmlItem.Status;

                                    // Cập nhật reactive property (giữ nguyên vùng nhớ)
                                    existing.IsChecked.Value = xmlItem.IsChecked;
                                    existing.PetKey.Value = xmlItem.PetKey;
                                    existing.PetOption.Value = xmlItem.PetOption;
                                    existing.ChiEp.Value = xmlItem.ChiEp;
                                    existing.NLKey.Value = xmlItem.NLKey;
                                    existing.NLDoiNNKey.Value = xmlItem.NLDoiNNKey;
                                    existing.LoaiMB.Value = xmlItem.LoaiMB;
                                    existing.CapMB.Value = xmlItem.CapMB;

                                    existing.RutOutfit.Value = xmlItem.RutOutfit;
                                    existing.CheMatBao.Value = xmlItem.CheMatBao;
                                    existing.DieuKhac.Value = xmlItem.DieuKhac;
                                    existing.HanhLang.Value = xmlItem.HanhLang;
                                    existing.TuHanh.Value = xmlItem.TuHanh;
                                    existing.KhoiPhuc.Value = xmlItem.KhoiPhuc;
                                    existing.LatBai.Value = xmlItem.LatBai;
                                    existing.PhuBan.Value = xmlItem.PhuBan;
                                    existing.NangNo.Value = xmlItem.NangNo;
                                    existing.TrongNL.Value = xmlItem.TrongNL;

                                    existing.MHD.Value = xmlItem.MHD;
                                    existing.MC.Value = xmlItem.MC;
                                    existing.LTC.Value = xmlItem.LTC;
                                    existing.LD.Value = xmlItem.LD;
                                    existing.VLD.Value = xmlItem.VLD;
                                    existing.Muoi.Value = xmlItem.Muoi;
                                    existing.TGS.Value = xmlItem.TGS;
                                    existing.Tham.Value = xmlItem.Tham;

                                }
                            }
                        }
                    }
                }
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

            item.Name.Value = Ten.Value ?? "";
            item.Version.Value = Version.Value ?? "";
            item.Link.Value = Link.Value ?? "";
            item.Status.Value = Status.Value ? 1 : 0;

            var match = Regex.Match(item.Link.Value ?? "", @"\/(s\d{2,3})\/", RegexOptions.IgnoreCase);
            if (match.Success) item.Server.Value = match.Groups[1].Value.ToUpper();

            SaveToXml(); // Không reload
        }

        private void VaoGame(DataModel model = null)
        {
            if (model == null)
            {
                model = SelectedItem.Value;
            }

            if (model == null || string.IsNullOrEmpty(model?.Link.Value))
            {
                MessageBox.Show("Chưa chọn nhân vật.");
                return;
            }
            if (model.HWnd.Value != IntPtr.Zero &&
                Helper.IsWindow(model.HWnd.Value) && !model.Relog.Value)
            {
                MessageBox.Show("Nhân vật hiện đang được mở!");
                return;
            }

            string link = model?.Link.Value;
            if (!string.IsNullOrEmpty(model?.Version.Value))
            {
                link += ("&version=" + model?.Version.Value);
            }
            try
            {
                if (model?.Status.Value == 1)
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
                    Process.Start(Constant.FilePath.FLASH_PLAYER, model?.Link.Value + "&version=" + model?.Version.Value);
                }



                IntPtr defaultHWnd = IntPtr.Zero;
                string defaultWindowName = Constant.FLASH_NAME;

                // Thử tối đa 2 giây để tìm cửa sổ
                for (int i = 0; i < 40; i++)
                {
                    defaultHWnd = AutoControl.FindWindowHandle(null, defaultWindowName);
                    if (defaultHWnd != IntPtr.Zero)
                    {
                        model.HWnd.Value = defaultHWnd;
                        Helper.SetWindowText(defaultHWnd, model?.Id.Value + "-" + model?.Server.Value + "-" + model?.Name.Value + "\t" + "Liên hệ auto: https://facebook.com/groups/VPT.TQ.S120");
                        break;
                    }

                    Thread.Sleep(300);
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
            MainAuto mainAuto = new MainAuto(SelectedItem.Value, Characters, IsBlocked);
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
            var selected = SelectedItem.Value;
            if (selected == null)
            {
                MessageBox.Show("Hiện không chọn nhân vật nào.");
                return;
            }

            string matchKey = selected.Name.Value;

            foreach (var thread in Helper.ThreadList.ToList())
            {
                if (thread.IsAlive && thread.Name != null && thread.Name.Contains(matchKey))
                {
                    Helper.WriteStatus(selected.LogText, matchKey, "Đã ngừng auto");
                    try { thread.Abort(); } catch { }
                }
            }
        }
    }
}
