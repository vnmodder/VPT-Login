using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VPT_Login.Models;

namespace VPT_Login.Libs
{
    public class ScanAddress
    {
        private int lpNumberOfBytesRead;

        public Process process;

        public string signature;

        public byte[] buffer;

        public byte[] Bytes;

        public ModeScan mode;

        public ScanAddress(uint pID, string signature, ModeScan modeScan)
        {
            this.signature = signature;
            process = Process.GetProcessById((int)pID);
            mode = modeScan;
        }

        public ScanAddress(uint pID, byte[] bytes, ModeScan modeScan)
        {
            process = Process.GetProcessById((int)pID);
            Bytes = bytes;
            mode = modeScan;
        }

        public ScanAddress(string pID, string value, string valueType, string scanmode = "hard")
        {
            process = Process.GetProcessById(int.Parse(pID));
            switch (valueType)
            {
                case "double":
                    Bytes = BitConverter.GetBytes(double.Parse(value));
                    break;
                case "int":
                    Bytes = BitConverter.GetBytes(int.Parse(value));
                    break;
                case "string":
                    Bytes = Encoding.UTF8.GetBytes(value);
                    break;
                default:
                    Bytes = BitConverter.GetBytes(double.Parse(value));
                    break;
            }

            if (scanmode == "hard")
            {
                mode = ModeScan.Hard;
            }
            else
            {
                mode = ModeScan.Normal;
            }
        }

        private List<uint> FindAddrByteInData(int j, byte[] data)
        {
            List<uint> list = new List<uint>();
            List<uint> list2 = FindIndexInArray(Bytes, data);
            if (list2.Count > 0)
            {
                foreach (uint item2 in list2)
                {
                    uint item = item2 + (uint)(int)(process.MainModule.BaseAddress + 14131200 + 16000 * j);
                    list.Add(item);
                }
            }

            return list;
        }

        private List<uint> FindArrayByteInBuffer(int j, byte[] buff)
        {
            List<uint> list = new List<uint>();
            List<int> list2 = TimIndexMangCon(buff, Bytes);
            if (list2.Count > 0)
            {
                foreach (int item2 in list2)
                {
                    uint item = (uint)(item2 + (int)(process.MainModule.BaseAddress + 8000 * j));
                    list.Add(item);
                }
            }

            return list;
        }

        private List<uint> FindIndexInArray(byte[] value, byte[] data)
        {
            List<uint> list = new List<uint>();
            for (int i = 0; i < data.Length; i += value.Length)
            {
                for (int j = 0; j < value.Length && data[i + j] == value[j]; j++)
                {
                    if (j == value.Length - 1)
                    {
                        list.Add((uint)i);
                    }
                }
            }

            return list;
        }

        public List<IntPtr> GetListAddress()
        {
            List<IntPtr> list = new List<IntPtr>();
            for (int i = 0; i <= 100000; i++)
            {
                IntPtr intPtr = (IntPtr)((int)process.MainModule.BaseAddress + 4096 + 10000 * i);
                buffer = ReadMemoryBytes((uint)(int)intPtr, 10000u);
                foreach (IntPtr item in sigscan(signature, i))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public async Task<List<uint>> GetListAddressArrayByte()
        {
            List<uint> nums = new List<uint>();
            List<Task<List<uint>>> tasks = new List<Task<List<uint>>>();
            long virtualMemorySize64 = 0L;
            switch (mode)
            {
                case ModeScan.Normal:
                    virtualMemorySize64 = process.VirtualMemorySize64;
                    break;
                case ModeScan.Hard:
                    virtualMemorySize64 = process.PeakVirtualMemorySize;
                    break;
            }

            long num = (virtualMemorySize64 / 8000 - 7999) / 8000 + 1;
            for (int i = 0; i < num; i++)
            {
                int num2 = 8000 * i;
                int num3 = 8000 * i + 7999;
                Task<List<uint>> task = Task.Run(() => GetListAddressArrayByteFromTo(num2, num3));
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            foreach (Task<List<uint>> task2 in tasks)
            {
                List<uint> list = nums;
                list.AddRange(await task2);
            }

            return nums;
        }

        public List<uint> GetListAddressArrayByteFromTo(int startindex, int lastindex)
        {
            List<uint> list = new List<uint>();
            for (int i = startindex; i <= lastindex; i++)
            {
                uint memoryAddress = (uint)((int)process.MainModule.BaseAddress + 8000 * i);
                foreach (uint item in FindArrayByteInBuffer(i, ReadMemoryBytes(memoryAddress, 8000u)))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public List<IntPtr> GetListAddressFromTo(int startindex, int lastindex)
        {
            List<IntPtr> list = new List<IntPtr>();
            for (int i = startindex; i <= lastindex; i++)
            {
                IntPtr intPtr = (IntPtr)((int)process.MainModule.BaseAddress + 4096 + 10000 * i);
                foreach (IntPtr item in SigScan(signature, i, ReadMemoryBytes((uint)(int)intPtr, 10000u)))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public async Task<List<uint>> GetListAddressValue()
        {
            List<uint> nums = new List<uint>();
            List<Task<List<uint>>> tasks = new List<Task<List<uint>>>();
            long virtualMemorySize64 = 0L;
            switch (mode)
            {
                case ModeScan.Normal:
                    virtualMemorySize64 = process.VirtualMemorySize64;
                    break;
                case ModeScan.Hard:
                    virtualMemorySize64 = process.PeakVirtualMemorySize;
                    break;
            }

            long num = (virtualMemorySize64 / 16000 - 15999) / 16000 + 1;
            for (int i = 0; i <= num; i++)
            {
                int num2 = 16000 * i;
                int num3 = 16000 * i + 15999;
                Task<List<uint>> task = Task.Run(() => GetListAddressValueFromTo(num2, num3));
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            foreach (Task<List<uint>> task2 in tasks)
            {
                List<uint> list = nums;
                list.AddRange(await task2);
            }

            return nums;
        }

        public List<uint> GetListAddressValueFromTo(int startindex, int lastindex)
        {
            List<uint> list = new List<uint>();
            for (int i = startindex; i <= lastindex; i++)
            {
                uint memoryAddress = (uint)(int)(process.MainModule.BaseAddress + 14131200 + 16000 * i);
                foreach (uint item in FindAddrByteInData(i, ReadMemoryBytes(memoryAddress, 16000u)))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public async Task<List<IntPtr>> GetListAddressVer2()
        {
            List<IntPtr> intPtrs = new List<IntPtr>();
            List<Task<List<IntPtr>>> tasks = new List<Task<List<IntPtr>>>();
            for (int i = 0; i < 10; i++)
            {
                int num = 10000 * i;
                int num2 = 10000 * i + 9999;
                Task<List<IntPtr>> task = Task.Run(() => GetListAddressFromTo(num, num2));
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            foreach (Task<List<IntPtr>> task2 in tasks)
            {
                List<IntPtr> list = intPtrs;
                list.AddRange(await task2);
            }

            return intPtrs;
        }

        public int[] KMP_Preprocess(byte[] b)
        {
            int[] array = new int[b.Length];
            int num = 0;
            int num2 = 1;
            while (num2 < b.Length)
            {
                if (b[num2] == b[num])
                {
                    num = (array[num2] = num + 1);
                    num2++;
                }
                else if (num == 0)
                {
                    array[num2] = 0;
                    num2++;
                }
                else
                {
                    num = array[num - 1];
                }
            }

            return array;
        }

        public T ReadMemory<T>(uint MemoryAddress)
        {
            return (T)Marshal.PtrToStructure(GCHandle.Alloc(ReadMemoryBytes(MemoryAddress, (uint)Marshal.SizeOf(typeof(T))), GCHandleType.Pinned).AddrOfPinnedObject(), typeof(T));
        }

        public byte[] ReadMemoryBytes(uint MemoryAddress, uint Bytes)
        {
            byte[] array = new byte[Bytes];
            ReadProcessMemory(process.Handle, MemoryAddress, array, array.Length, out lpNumberOfBytesRead);
            return array;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, uint lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);

        private List<IntPtr> sigscan(string sig, int j)
        {
            int[] array = transformarray(sig);
            List<IntPtr> list = new List<IntPtr>();
            for (int i = 0; i < buffer.Length; i++)
            {
                for (int k = 0; k < array.Length && i + k != buffer.Length && ((array[k] == -1) | (array[k] == buffer[i + k])); k++)
                {
                    if (k + 1 == array.Length)
                    {
                        IntPtr item = new IntPtr(i + (int)process.MainModule.BaseAddress + 4096 + 10000 * j);
                        list.Add(item);
                    }
                }
            }

            return list;
        }

        public List<IntPtr> SigScan(string sig, int j, byte[] buff)
        {
            int[] array = transformarray(sig);
            List<IntPtr> list = new List<IntPtr>();
            for (int i = 0; i < buff.Length; i++)
            {
                for (int k = 0; k < array.Length && i + k != buff.Length && ((array[k] == -1) | (array[k] == buff[i + k])); k++)
                {
                    if (k + 1 == array.Length)
                    {
                        IntPtr item = new IntPtr(i + (int)process.MainModule.BaseAddress + 4096 + 10000 * j);
                        list.Add(item);
                    }
                }
            }

            return list;
        }

        public List<int> TimIndexMangCon(byte[] a, byte[] b)
        {
            List<int> list = new List<int>();
            int[] array = KMP_Preprocess(b);
            int num = 0;
            int num2 = 0;
            while (num < a.Length)
            {
                if (b[num2] == a[num])
                {
                    num2++;
                    num++;
                }

                if (num2 != b.Length)
                {
                    if (num < a.Length && b[num2] != a[num])
                    {
                        if (num2 == 0)
                        {
                            num++;
                        }
                        else
                        {
                            num2 = array[num2 - 1];
                        }
                    }
                }
                else
                {
                    list.Add(num - num2);
                    num2 = array[num2 - 1];
                }
            }

            return list;
        }

        private int[] transformarray(string sig)
        {
            string[] array = sig.Split(' ');
            int[] array2 = new int[array.Length];
            for (int i = 0; i < array2.Length; i++)
            {
                if (array[i] != "??")
                {
                    array2[i] = int.Parse(array[i], NumberStyles.HexNumber);
                }
                else
                {
                    array2[i] = -1;
                }
            }

            return array2;
        }

        private byte[] transformbytearray(string sig)
        {
            string[] array = sig.Split(' ');
            byte[] array2 = new byte[array.Length];
            for (int i = 0; i < array2.Length; i++)
            {
                if (array[i] != "??")
                {
                    array2[i] = byte.Parse(array[i], NumberStyles.HexNumber);
                }
                else
                {
                    array2[i] = 0;
                }
            }

            return array2;
        }
    }
}
