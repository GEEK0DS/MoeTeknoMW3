using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Fullrank
{
    public class doRank
    {



        internal sealed class ReadWritingMemory
        {
            private const int PROCESS_ALL_ACCESS = 0x1f0ff;

            [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            private static extern int OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);
            public static float ReadDMAFloat(string Process, int Address, int[] Offsets, int Level, int nsize = 4)
            {
                float num = 0;
                try
                {
                    int address = Address;
                    int num5 = Level;
                    for (int i = 1; i <= num5; i++)
                    {
                        address = (int)Math.Round((double)(ReadFloat(Process, address, nsize) + Offsets[i - 1]));
                    }
                    num = ReadFloat(Process, address, nsize);
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    ProjectData.ClearProjectError();
                }
                return num;
            }

            public static int ReadDMAInteger(string Process, int Address, int[] Offsets, int Level, int nsize = 4)
            {
                int num = 0;
                try
                {
                    int address = Address;
                    int num5 = Level;
                    for (int i = 1; i <= num5; i++)
                    {
                        address = ReadInteger(Process, address, nsize) + Offsets[i - 1];
                    }
                    num = ReadInteger(Process, address, nsize);
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    ProjectData.ClearProjectError();
                }
                return num;
            }

            public static long ReadDMALong(string Process, int Address, int[] Offsets, int Level, int nsize = 4)
            {
                long num = 0;
                try
                {
                    int address = Address;
                    int num5 = Level;
                    for (int i = 1; i <= num5; i++)
                    {
                        address = ((int)ReadLong(Process, address, nsize)) + Offsets[i - 1];
                    }
                    num = ReadLong(Process, address, nsize);
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    ProjectData.ClearProjectError();
                }
                return num;
            }

            public static float ReadFloat(string ProcessName, int Address, int nsize = 4)
            {
                float num2 = 0;
                float num3 = 0;
                if (ProcessName.EndsWith(".exe"))
                {
                    ProcessName = ProcessName.Replace(".exe", "");
                }
                Process[] processesByName = Process.GetProcessesByName(ProcessName);
                if (processesByName.Length == 0)
                {
                    MessageBox.Show(ProcessName + " isn't open!");
                    return num2;
                }
                IntPtr ptr = (IntPtr)OpenProcess(0x1f0ff, 0, processesByName[0].Id);
                if (ptr == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to open " + ProcessName + "!");
                    return num2;
                }
                int lpBaseAddress = Address;
                int lpNumberOfBytesWritten = 0;
                ReadProcessMemory2((int)ptr, lpBaseAddress, ref num3, nsize, ref lpNumberOfBytesWritten);
                return num3;
            }

            public static int ReadInteger(string ProcessName, int Address, int nsize = 4)
            {
                int num2 = 0;
                int num3 = 0;
                if (ProcessName.EndsWith(".exe"))
                {
                    ProcessName = ProcessName.Replace(".exe", "");
                }
                Process[] processesByName = Process.GetProcessesByName(ProcessName);
                if (processesByName.Length == 0)
                {
                    MessageBox.Show(ProcessName + " isn't open!");
                    return num2;
                }
                IntPtr ptr = (IntPtr)OpenProcess(0x1f0ff, 0, processesByName[0].Id);
                if (ptr == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to open " + ProcessName + "!");
                    return num2;
                }
                int lpBaseAddress = Address;
                int lpNumberOfBytesWritten = 0;
                ReadProcessMemory1((int)ptr, lpBaseAddress, ref num3, nsize, ref lpNumberOfBytesWritten);
                return num3;
            }

            public static long ReadLong(string ProcessName, int Address, int nsize = 4)
            {
                long num2 = 0;
                long num3 = 0;
                if (ProcessName.EndsWith(".exe"))
                {
                    ProcessName = ProcessName.Replace(".exe", "");
                }
                Process[] processesByName = Process.GetProcessesByName(ProcessName);
                if (processesByName.Length == 0)
                {
                    MessageBox.Show(ProcessName + " isn't open!");
                    return num2;
                }
                IntPtr ptr = (IntPtr)OpenProcess(0x1f0ff, 0, processesByName[0].Id);
                if (ptr == IntPtr.Zero)
                {
                    MessageBox.Show("Failed to open " + ProcessName + "!");
                    return num2;
                }
                int lpBaseAddress = Address;
                int lpNumberOfBytesWritten = 0;
                ReadProcessMemory3((int)ptr, lpBaseAddress, ref num3, nsize, ref lpNumberOfBytesWritten);
                return num3;
            }

            [DllImport("kernel32", EntryPoint = "ReadProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            private static extern int ReadProcessMemory1(int hProcess, int lpBaseAddress, ref int lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
            [DllImport("kernel32", EntryPoint = "ReadProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            private static extern float ReadProcessMemory2(int hProcess, int lpBaseAddress, ref float lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
            [DllImport("kernel32", EntryPoint = "ReadProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            private static extern long ReadProcessMemory3(int hProcess, int lpBaseAddress, ref long lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
            public static bool WriteDMAFloat(string Process, int Address, int[] Offsets, float Value, int Level, int nsize = 4)
            {
                bool flag;
                try
                {
                    int address = Address;
                    int num3 = Level;
                    for (int i = 1; i <= num3; i++)
                    {
                        address = (int)Math.Round((double)(ReadFloat(Process, address, nsize) + Offsets[i - 1]));
                    }
                    WriteFloat(Process, address, Value, nsize);
                    flag = true;
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    flag = false;
                    ProjectData.ClearProjectError();
                }
                return flag;
            }

            public static bool WriteDMAInteger(string Process, int Address, int[] Offsets, int Value, int Level, int nsize = 4)
            {
                bool flag;
                try
                {
                    int address = Address;
                    int num3 = Level;
                    for (int i = 1; i <= num3; i++)
                    {
                        address = ReadInteger(Process, address, nsize) + Offsets[i - 1];
                    }
                    WriteInteger(Process, address, Value, nsize);
                    flag = true;
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    flag = false;
                    ProjectData.ClearProjectError();
                }
                return flag;
            }

            public static bool WriteDMALong(string Process, int Address, int[] Offsets, long Value, int Level, int nsize = 4)
            {
                bool flag;
                try
                {
                    int address = Address;
                    int num3 = Level;
                    for (int i = 1; i <= num3; i++)
                    {
                        address = ((int)ReadLong(Process, address, nsize)) + Offsets[i - 1];
                    }
                    WriteLong(Process, address, Value, nsize);
                    flag = true;
                }
                catch (Exception exception1)
                {
                    ProjectData.SetProjectError(exception1);
                    Exception exception = exception1;
                    flag = false;
                    ProjectData.ClearProjectError();
                }
                return flag;
            }

            public static void WriteFloat(string ProcessName, int Address, float Value, int nsize = 4)
            {
                if (ProcessName.EndsWith(".exe"))
                {
                    ProcessName = ProcessName.Replace(".exe", "");
                }
                Process[] processesByName = Process.GetProcessesByName(ProcessName);
                if (processesByName.Length == 0)
                {
                    MessageBox.Show(ProcessName + " isn't open!");
                }
                else
                {
                    IntPtr ptr = (IntPtr)OpenProcess(0x1f0ff, 0, processesByName[0].Id);
                    if (ptr == IntPtr.Zero)
                    {
                        MessageBox.Show("Failed to open " + ProcessName + "!");
                    }
                    else
                    {
                        int lpBaseAddress = Address;
                        float lpBuffer = Value;
                        int lpNumberOfBytesWritten = 0;
                        WriteProcessMemory2((int)ptr, lpBaseAddress, ref lpBuffer, nsize, ref lpNumberOfBytesWritten);
                    }
                }
            }

            public static void WriteInteger(string ProcessName, int Address, int Value, int nsize = 4)
            {
                if (ProcessName.EndsWith(".exe"))
                {
                    ProcessName = ProcessName.Replace(".exe", "");
                }
                Process[] processesByName = Process.GetProcessesByName(ProcessName);
                if (processesByName.Length == 0)
                {
                    MessageBox.Show(ProcessName + " isn't open!");
                }
                else
                {
                    IntPtr ptr = (IntPtr)OpenProcess(0x1f0ff, 0, processesByName[0].Id);
                    if (ptr == IntPtr.Zero)
                    {
                        MessageBox.Show("Failed to open " + ProcessName + "!");
                    }
                    else
                    {
                        int lpBaseAddress = Address;
                        int lpBuffer = Value;
                        int lpNumberOfBytesWritten = 0;
                        WriteProcessMemory1((int)ptr, lpBaseAddress, ref lpBuffer, nsize, ref lpNumberOfBytesWritten);
                    }
                }
            }

            public static void WriteLong(string ProcessName, int Address, long Value, int nsize = 4)
            {
                if (ProcessName.EndsWith(".exe"))
                {
                    ProcessName = ProcessName.Replace(".exe", "");
                }
                Process[] processesByName = Process.GetProcessesByName(ProcessName);
                if (processesByName.Length == 0)
                {
                    MessageBox.Show(ProcessName + " isn't open!");
                }
                else
                {
                    IntPtr ptr = (IntPtr)OpenProcess(0x1f0ff, 0, processesByName[0].Id);
                    if (ptr == IntPtr.Zero)
                    {
                        MessageBox.Show("Failed to open " + ProcessName + "!");
                    }
                    else
                    {
                        int lpBaseAddress = Address;
                        long lpBuffer = Value;
                        int lpNumberOfBytesWritten = 0;
                        WriteProcessMemory3((int)ptr, lpBaseAddress, ref lpBuffer, nsize, ref lpNumberOfBytesWritten);
                    }
                }
            }

            public static void WriteNOPs(string ProcessName, long Address, int NOPNum)
            {
                if (ProcessName.EndsWith(".exe"))
                {
                    ProcessName = ProcessName.Replace(".exe", "");
                }
                Process[] processesByName = Process.GetProcessesByName(ProcessName);
                if (processesByName.Length == 0)
                {
                    MessageBox.Show(ProcessName + " isn't open!");
                }
                else
                {
                    IntPtr ptr = (IntPtr)OpenProcess(0x1f0ff, 0, processesByName[0].Id);
                    if (ptr == IntPtr.Zero)
                    {
                        MessageBox.Show("Failed to open " + ProcessName + "!");
                    }
                    else
                    {
                        int num = 0;
                        int num3 = NOPNum;
                        for (int i = 1; i <= num3; i++)
                        {
                            int lpBuffer = 0x90;
                            int lpNumberOfBytesWritten = 0;
                            WriteProcessMemory1((int)ptr, ((int)Address) + num, ref lpBuffer, 1, ref lpNumberOfBytesWritten);
                            num++;
                        }
                    }
                }
            }

            [DllImport("kernel32", EntryPoint = "WriteProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            private static extern int WriteProcessMemory1(int hProcess, int lpBaseAddress, ref int lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
            [DllImport("kernel32", EntryPoint = "WriteProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            private static extern float WriteProcessMemory2(int hProcess, int lpBaseAddress, ref float lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
            [DllImport("kernel32", EntryPoint = "WriteProcessMemory", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            private static extern long WriteProcessMemory3(int hProcess, int lpBaseAddress, ref long lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
            public static void WriteXBytes(string ProcessName, long Address, string Value)
            {
                if (ProcessName.EndsWith(".exe"))
                {
                    ProcessName = ProcessName.Replace(".exe", "");
                }
                Process[] processesByName = Process.GetProcessesByName(ProcessName);
                if (processesByName.Length == 0)
                {
                    MessageBox.Show(ProcessName + " isn't open!");
                }
                else
                {
                    IntPtr ptr = (IntPtr)OpenProcess(0x1f0ff, 0, processesByName[0].Id);
                    if (ptr == IntPtr.Zero)
                    {
                        MessageBox.Show("Failed to open " + ProcessName + "!");
                    }
                    else
                    {
                        int num = 0;
                        int start = 1;
                        int num5 = (int)Math.Round(Math.Round((double)(((double)Strings.Len(Value)) / 2.0)));
                        for (int i = 1; i <= num5; i++)
                        {
                            byte num4 = (byte)Math.Round(Conversion.Val("&H" + Strings.Mid(Value, start, 2)));
                            int lpBuffer = num4;
                            int lpNumberOfBytesWritten = 0;
                            WriteProcessMemory1((int)ptr, ((int)Address) + num, ref lpBuffer, 1, ref lpNumberOfBytesWritten);
                            num4 = (byte)lpBuffer;
                            num++;
                            start += 2;
                        }
                    }
                }
            }
        }
    }
}
		