using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPT_Login.Libs
{
    public class MemoryScanner
    {

        private const uint MEM_COMMIT = 0x1000;
        private const uint PAGE_READWRITE = 0x04;
        private const uint PAGE_WRITECOPY = 0x08;


        private IntPtr _processHandle;
        public List<IntPtr> ScanResults = new List<IntPtr>();

        public MemoryScanner(IntPtr processHandle)
        {
            _processHandle = processHandle;
        }

        public List<IntPtr> FirstScan(int value)
        {
            ScanResults.Clear();
            ScanResults = scanForValue(value);
            return ScanResults;
        }

        public List<IntPtr> NextScan(int newValue, bool isNot = false)
        {
            List<IntPtr> filtered = new List<IntPtr>();

            foreach (var addr in ScanResults)
            {
                byte[] buffer = new byte[4];

                Helper.ReadProcessMemory(_processHandle, addr, buffer, 4, out int read);
                int val = BitConverter.ToInt32(buffer, 0);
                if (isNot)
                {
                    if (val != newValue)
                        filtered.Add(addr);
                }
                else
                {
                    if (val == newValue)
                        filtered.Add(addr);
                }
            }

            ScanResults = filtered;
            return ScanResults;
        }

        private List<IntPtr> scanForValue(int target)
        {
            List<IntPtr> results = new List<IntPtr>();
            byte[] targetBytes = BitConverter.GetBytes(target);

            IntPtr addr = IntPtr.Zero;

            while (true)
            {
                int result = Helper.VirtualQueryEx(
                    _processHandle, addr, out MEMORY_BASIC_INFORMATION mbi,
                    (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))
                );

                if (result == 0) break;

                bool readable =
                    mbi.State == MEM_COMMIT &&
                   (mbi.Protect == PAGE_READWRITE ||
                    mbi.Protect == PAGE_WRITECOPY);

                if (readable)
                {
                    long size = mbi.RegionSize.ToInt64();
                    byte[] buffer = new byte[size];

                    if (Helper.ReadProcessMemory(_processHandle, mbi.BaseAddress, buffer, buffer.Length, out int bytesRead))
                    {
                        for (int i = 0; i < bytesRead - 4; i++)
                        {
                            if (buffer[i] == targetBytes[0] &&
                                buffer[i + 1] == targetBytes[1] &&
                                buffer[i + 2] == targetBytes[2] &&
                                buffer[i + 3] == targetBytes[3])
                            {
                                results.Add(mbi.BaseAddress + i);
                            }
                        }
                    }
                }

                addr = IntPtr.Add(mbi.BaseAddress, (int)mbi.RegionSize);
            }

            return results;
        }            

        public int ReadInt32(IntPtr addr)
        {
            byte[] buffer = new byte[4];
            Helper.ReadProcessMemory(_processHandle, addr, buffer, 4, out int bytesRead);
            return BitConverter.ToInt32(buffer, 0);
        }

        public void WriteInt32(IntPtr addr, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Helper.WriteProcessMemory(_processHandle, addr, bytes, 4, out int written);
        }
    }
}
