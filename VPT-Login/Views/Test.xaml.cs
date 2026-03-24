using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
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
        private DataModel auto;
        public AutoFeatures mAuto;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        private static extern bool GetWindowThreadProcessId(IntPtr hWnd, out uint pid);


        const int SW_RESTORE = 9;

        public Test(DataModel auto)
        {
            InitializeComponent();
            DataContext = this;
            mAuto = new AutoFeatures(auto);
            this.auto = auto;

            TestCommand.Subscribe(() => TestAAA());
            TestCommand2.Subscribe(() => TestAAA2());
        }

        private async Task TestAAA2()
        {

            await DebugStruct("叛军大将");
            return;

            string id = await ScanBossId("叛军大将");
            if (!string.IsNullOrEmpty(id)) {
                LogText.Value += "ID: "+ id;
            }
            else
            {
                LogText.Value += "Không tìm thấy ID\n";
            }
        }

        private async Task TestAAA()
        {
            (string map, int x, int y)? bossLocation = await FindBossLocation("叛军大将");
            if (bossLocation != null)
            {
                var info = bossLocation.Value;
                string result = $"{info.map}; {info.x},{info.y}";

                LogText.Value += result + "\n";
            }
            else
            {
                LogText.Value += "Không tìm thấy boss\n";
            }
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

        private async Task<(string map, int x, int y)?> FindBossLocation(string bossName)
        {
            bossName = "[系统]" + bossName;
            uint pid;
            GetWindowThreadProcessId(auto.HWnd.Value, out pid);

            ScanAddress scan = new ScanAddress(pid.ToString(), bossName, "string");

            var addresses = await scan.GetListAddressArrayByte();

            byte[] textBytes = Encoding.UTF8.GetBytes(bossName);

            foreach (var addr in addresses)
            {
                byte[] data = scan.ReadMemoryBytes(addr, 8000);

                // check byte nhanh
                bool match = true;
                for (int i = 0; i < textBytes.Length; i++)
                {
                    if (data[i] != textBytes[i])
                    {
                        match = false;
                        break;
                    }
                }
                if (!match) continue;

                string text = Encoding.UTF8.GetString(data);

                int idxAt = text.IndexOf('在');
                int idxDe = text.IndexOf('的', idxAt);
                int idxColon = text.IndexOf('：', idxDe);

                if (idxAt < 0 || idxDe < 0 || idxColon < 0)
                    continue;

                string map = text.Substring(idxAt + 1, idxDe - idxAt - 1);

                string coordPart = text.Substring(idxColon + 1);

                int comma = coordPart.IndexOf(',');
                if (comma < 0) continue;

                int x = int.Parse(coordPart.Substring(0, comma));

                int yStart = comma + 1;
                int yEnd = yStart;

                while (yEnd < coordPart.Length && char.IsDigit(coordPart[yEnd]))
                    yEnd++;

                int y = int.Parse(coordPart.Substring(yStart, yEnd - yStart));

                return (map, x, y);
            }

            return null;
        }

        private async Task<string> ScanBossId(string bossName)
        {
            uint pid;
            GetWindowThreadProcessId(auto.HWnd.Value, out pid);

            ScanAddress scan = new ScanAddress(pid.ToString(), bossName, "string", "hard");

            var addresses = await scan.GetListAddressArrayByte();

            byte[] keyBytes = Encoding.UTF8.GetBytes(bossName);
            byte[] prefixBytes = Encoding.UTF8.GetBytes("event:L_N|");

            foreach (var addr in addresses)
            {
                byte[] data = scan.ReadMemoryBytes(addr - 64, 8000); // đọc rộng hơn

                for (int i = 32; i < data.Length - keyBytes.Length; i++)
                {
                    bool match = true;
                    for (int j = 0; j < keyBytes.Length; j++)
                    {
                        if (data[i + j] != keyBytes[j])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (!match) continue;

                    // tìm ID
                    int idEnd = i - 1;
                    int idStart = idEnd;

                    while (idStart >= 0 && data[idStart] >= '0' && data[idStart] <= '9')
                        idStart--;

                    idStart++;

                    if (idStart > idEnd) continue;

                    // check prefix
                    int prefixStart = idStart - prefixBytes.Length;
                    if (prefixStart < 0) continue;

                    bool ok = true;
                    for (int j = 0; j < prefixBytes.Length; j++)
                    {
                        if (data[prefixStart + j] != prefixBytes[j])
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (!ok) continue;

                    return Encoding.UTF8.GetString(data, idStart, idEnd - idStart + 1);
                }
            }

            return null;
        }

        private async Task DebugStruct(string bossName)
        {
            uint pid;
            GetWindowThreadProcessId(auto.HWnd.Value, out pid);

            ScanAddress scan = new ScanAddress(pid.ToString(), bossName, "string", "hard");

            var addresses = await scan.GetListAddressArrayByte();

            int count = 0;

            foreach (var addr in addresses)
            {
                LogText.Value += $"==== DEBUG ADDR 0x{addr:X} ====\n";

                DumpAround(addr, scan);

                count++;
                if (count >= 5) break;
            }
        }

        private void DumpAround(uint addr, ScanAddress scan)
        {
            // đọc rộng hơn để thấy struct
            byte[] data = scan.ReadMemoryBytes(addr - 256, 512);

            // in hex
            string hex = BitConverter.ToString(data);
            LogText.Value += $"HEX:\n{hex}\n\n";

            // in dạng int (quan trọng)
            for (int i = 0; i < data.Length - 4; i += 4)
            {
                int val = BitConverter.ToInt32(data, i);
                LogText.Value += $"[{i - 256}] = {val}\n";
            }

            LogText.Value += "\n====================\n";
        }

        private async Task DebugBossText(string bossName)
        {
            uint pid;
            GetWindowThreadProcessId(auto.HWnd.Value, out pid);

            ScanAddress scan = new ScanAddress(pid.ToString(), bossName, "string", "hard");

            var addresses = await scan.GetListAddressArrayByte();

            int count = 0;
            LogText.Value = "";
            foreach (var addr in addresses)
            {
                // đọc rộng hơn để thấy full context
                byte[] data = scan.ReadMemoryBytes(addr - 64, 8000);

                string text = Encoding.UTF8.GetString(data);
                if (text.Contains(bossName))
                {
                LogText.Value += $"===== [{count}] Addr: 0x{addr:X} =====\n";
                LogText.Value += text + "\n\n";

                }

                count++;

                // chỉ log 30 cái đầu để tránh spam
                if (count >= 30) break;
            }
        }
    }
}
