
﻿/*
This file is part of LazyBot - Copyright (C) 2011 Arutha

    LazyBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LazyBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with LazyBot.  If not, see <http://www.gnu.org/licenses/>.
*/
#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;

#endregion

namespace LazyLib.Helpers
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public static class Memory
    {
// ReSharper disable InconsistentNaming
        private const int DEFAULT_MEMORY_SIZE = 0x1000;

        #region Imports

        [DllImport("kernel32.dll", SetLastError = true), SuppressUnmanagedCodeSecurity]
        internal static extern bool ReadProcessMemory(IntPtr hProcess, uint lpBaseAddress, [Out] byte[] lpBuffer,
                                                      int dwSize, out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true), SuppressUnmanagedCodeSecurity]
        internal static extern bool WriteProcessMemory(IntPtr hProcess, uint lpBaseAddress, byte[] lpBuffer, uint nSize,
                                                       out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll"), SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true), SuppressUnmanagedCodeSecurity]
        internal static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("kernel32", EntryPoint = "VirtualAllocEx")]
        public static extern uint VirtualAllocEx(IntPtr hProcess, uint dwAddress, int nSize, uint dwAllocationType,
                                                 uint dwProtect);

        [DllImport("kernel32", EntryPoint = "VirtualFreeEx")]
        public static extern bool VirtualFreeEx(IntPtr hProcess, uint dwAddress, int nSize, uint dwFreeType);

        #endregion

        #region Properties

        private static Process ProcessObject { get; set; }

        public static int ProcessId
        {
            get { return ProcessObject.Id; }
        }

        public static IntPtr WindowHandle
        {
            get { return ProcessObject.MainWindowHandle; }
        }

        public static ProcessModule MainModule
        {
            get
            {
                try
                {
                    return ProcessObject.MainModule;
                }
                catch
                {
                }
                return null;
            }
        }

        public static uint BaseAddress
        {
            get { return (uint) MainModule.BaseAddress; }
        }

        public static IntPtr ProcessHandle { get; private set; }

        public static bool Initialized
        {
            get { return ProcessHandle != IntPtr.Zero; }
        }

        #endregion

        #region ProcessHandle

        public static bool OpenProcess(int processId)
        {
// ReSharper disable AssignNullToNotNullAttribute
            var principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
// ReSharper restore AssignNullToNotNullAttribute
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
                Process.EnterDebugMode();

            ProcessObject = Process.GetProcessById(processId);
            ProcessHandle = OpenProcess(0x000F0000 | 0x00100000 | 0xFFF, false, processId);
            if (ProcessHandle != IntPtr.Zero)
                return true;
            return false;
        }

        public static void CloseProcess()
        {
            CloseHandle(ProcessHandle);
            Process.LeaveDebugMode();
        }

        #endregion

        #region WriteBytes

        public static int WriteBytes(uint address, byte[] val)
        {
            int written;
            if (WriteProcessMemory(ProcessHandle, address, val, (uint) val.Length, out written))
            {
                return written;
            }

            throw new Exception(string.Format("Could not write the specified bytes! {0:X8} [{1}]", address,
                                              Marshal.GetLastWin32Error()));
        }

        public static void Write<T>(uint address, T value)
        {
            if (value is string)
            {
                WriteBytes(address, Encoding.ASCII.GetBytes(value as string));
            }
            else
            {
                int numBytes = Marshal.SizeOf(value);
                unsafe
                {
                    byte* bytes = stackalloc byte[numBytes];
                    Marshal.StructureToPtr(value, (IntPtr) bytes, true);
                    var writeBytes = new byte[numBytes];
                    Marshal.Copy((IntPtr) bytes, writeBytes, 0, numBytes);
                    WriteBytes(address, writeBytes);
                }
            }
        }

        #endregion

        #region ReadBytes

// ReSharper disable InconsistentNaming
        public const byte ASCII_CHAR_LENGTH = 1;
        public const byte UNICODE_CHAR_LENGTH = 2;
        public const int DEFAULT_MemORY_SIZE = 0x1000;
        private static readonly UTF8Encoding UTF8 = new UTF8Encoding();

        public static T ReadRelative<T>(params uint[] addresses)
        {
            if (addresses.Length >= 1)
            {
                addresses[0] = (addresses[0] + BaseAddress);
            }
            return Read<T>(addresses);
        }

// ReSharper restore InconsistentNaming
        public static byte[] ReadBytes(uint address, int count)
        {
            var ret = new byte[count];
            int numRead;
            if (ReadProcessMemory(ProcessHandle, address, ret, count, out numRead) && numRead == count)
            {
                return ret;
            }
            return null;
        }

        private static T ReadInternal<T>(uint address)
        {
            object ret;
            try
            {
                if (typeof (T) == typeof (string))
                {
                    var chars = new List<char>();
                    uint offset = 0;
                    char lastChar;
                    while ((lastChar = (char) Read<byte>(address + offset)) != '\0')
                    {
                        offset++;
                        chars.Add(lastChar);
                    }
                    ret = new string(chars.ToArray());

                    return (T) ret;
                }

                int numBytes = Marshal.SizeOf(typeof (T));
                if (typeof (T) == typeof (IntPtr))
                {
                    ret = (IntPtr) BitConverter.ToInt64(ReadBytes(address, numBytes), 0);
                    return (T) ret;
                }

                switch (Type.GetTypeCode(typeof (T)))
                {
                    case TypeCode.Boolean:
                        ret = ReadBytes(address, 1)[0] != 0;
                        break;
                    case TypeCode.Char:
                        ret = (char) ReadBytes(address, 1)[0];
                        break;
                    case TypeCode.SByte:
                        ret = (sbyte) ReadBytes(address, numBytes)[0];
                        break;
                    case TypeCode.Byte:
                        ret = ReadBytes(address, numBytes)[0];
                        break;
                    case TypeCode.Int16:
                        ret = BitConverter.ToInt16(ReadBytes(address, numBytes), 0);
                        break;
                    case TypeCode.UInt16:
                        ret = BitConverter.ToUInt16(ReadBytes(address, numBytes), 0);
                        break;
                    case TypeCode.Int32:
                        ret = BitConverter.ToInt32(ReadBytes(address, numBytes), 0);
                        break;
                    case TypeCode.UInt32:
                        ret = BitConverter.ToUInt32(ReadBytes(address, numBytes), 0);
                        break;
                    case TypeCode.Int64:
                        ret = BitConverter.ToInt64(ReadBytes(address, numBytes), 0);
                        break;
                    case TypeCode.UInt64:
                        ret = BitConverter.ToUInt64(ReadBytes(address, numBytes), 0);
                        break;
                    case TypeCode.Single:
                        ret = BitConverter.ToSingle(ReadBytes(address, numBytes), 0);
                        break;
                    case TypeCode.Double:
                        ret = BitConverter.ToDouble(ReadBytes(address, numBytes), 0);
                        break;
                    default:
                        IntPtr dataStore = Marshal.AllocHGlobal(numBytes);
                        byte[] data = ReadBytes(address, numBytes);
                        Marshal.Copy(data, 0, dataStore, numBytes);
                        ret = Marshal.PtrToStructure(dataStore, typeof (T));
                        Marshal.FreeHGlobal(dataStore);
                        break;
                }
                return (T) ret;
            }
            catch
            {
                return default(T);
            }
        }

        public static T Read<T>(params uint[] addresses)
        {
            if (addresses.Length == 0)
            {
                return default(T);
            }
            if (addresses.Length == 1)
            {
                return ReadInternal<T>(addresses[0]);
            }

            uint last = 0;
            for (int i = 0; i < addresses.Length; i++)
            {
                if (i == addresses.Length - 1)
                {
                    return ReadInternal<T>(addresses[i] + last);
                }
                last = ReadInternal<uint>(last + addresses[i]);
            }

            return default(T);
        }


        public static T Read<T>(params IntPtr[] addresses)
        {
            if (addresses.Length == 0)
            {
                return default(T);
            }
            if (addresses.Length == 1)
            {
                return ReadInternal<T>((uint)addresses[0]);
            }
            uint last = 0;
            for (int i = 0; i < addresses.Length; i++)
            {
                if (i == addresses.Length - 1)
                {
                    return ReadInternal<T>((uint)addresses[i] + last);
                }
                last = ReadInternal<uint>((uint)last + (uint)addresses[i]);
            }
            return default(T);
        }

        public static T ReadStruct<T>(uint address) where T : struct
        {
            IntPtr localStore = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (T)));
            Marshal.Copy(ReadBytes(address, Marshal.SizeOf(typeof (T))), 0, localStore, Marshal.SizeOf(typeof (T)));
            var ret = (T) Marshal.PtrToStructure(localStore, typeof (T));
            Marshal.FreeHGlobal(localStore);
            return ret;
        }

        public static T ReadStruct<T>(IntPtr address) where T : struct
        {
            IntPtr localStore = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
            Marshal.Copy(ReadBytes((uint)address, Marshal.SizeOf(typeof(T))), 0, localStore, Marshal.SizeOf(typeof(T)));
            var ret = (T)Marshal.PtrToStructure(localStore, typeof(T));
            Marshal.FreeHGlobal(localStore);
            return ret;
        }

        /// <summary>
        ///   Reads Memory from an external process into a buffer of allocated Memory in the local process.
        /// </summary>
        /// <param name = "hProcess">Handle to the external process from which Memory will be read.</param>
        /// <param name = "dwAddress">Address in external process from which Memory will be read.</param>
        /// <param name = "lpBuffer">Pointer to a buffer of allocated Memory of at least nSize bytes.</param>
        /// <param name = "nSize">Number of bytes to be read.</param>
        /// <returns>Returns the number of bytes actually read.</returns>
        public static int ReadRawMemory(IntPtr hProcess, uint dwAddress, IntPtr lpBuffer, int nSize)
        {
            try
            {
                int lpBytesRead;
                if (!ReadProcessMemory(hProcess, dwAddress, lpBuffer, nSize, out lpBytesRead))
                    throw new Exception("ReadProcessMemory failed");

                return lpBytesRead;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///   Reads Utf8 string from Memory
        /// </summary>
        public static string ReadUtf8StringRelative(uint address, int length)
        {
            address = address + BaseAddress;
            byte[] buffer = ReadBytes(ProcessHandle, address, length);

            if (buffer != null)
            {
                string ret = Encoding.UTF8.GetString(buffer);

                if (ret.IndexOf('\0') != -1)
                {
                    ret = ret.Remove(ret.IndexOf('\0'));
                }
                return ret;
            }
            return "";
        }

        /// <summary>
        ///   Reads Utf8 string from Memory
        /// </summary>
        public static string ReadUtf8(uint dwAddress, int Size)
        {
            byte[] l = ReadBytes(ProcessHandle, dwAddress, Size);

            if (l == null)
                return String.Empty;

            return UTF8Decoder(l);
        }

        private static string UTF8Decoder(byte[] bytes)
        {
            String _s = UTF8.GetString(bytes, 0, bytes.Length);
            if (_s.IndexOf("\0") != -1)
                _s = _s.Remove(_s.IndexOf("\0"), _s.Length - _s.IndexOf("\0"));
            return _s;
        }

        public static byte[] ReadBytes(IntPtr hProcess, uint dwAddress, int nSize)
        {
            IntPtr lpBuffer = IntPtr.Zero;
            int iBytesRead;
            byte[] baRet;

            try
            {
                lpBuffer = Marshal.AllocHGlobal(nSize);

                iBytesRead = ReadRawMemory(hProcess, dwAddress, lpBuffer, nSize);
                if (iBytesRead != nSize)
                    throw new Exception("ReadProcessMemory error in ReadBytes");

                baRet = new byte[iBytesRead];
                Marshal.Copy(lpBuffer, baRet, 0, iBytesRead);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (lpBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(lpBuffer);
            }

            return baRet;
        }

        public static string ReadAsciiString(uint dwAddress, int nLength)
        {
            IntPtr lpBuffer = IntPtr.Zero;
            int iBytesRead, nSize;
            string sRet;

            try
            {
                nSize = nLength*ASCII_CHAR_LENGTH;
                lpBuffer = Marshal.AllocHGlobal(nSize + ASCII_CHAR_LENGTH);
                Marshal.WriteByte(lpBuffer, nLength, 0);

                iBytesRead = ReadRawMemory(ProcessHandle, dwAddress, lpBuffer, nSize);
                if (iBytesRead != nSize)
                    throw new Exception();

                sRet = Marshal.PtrToStringAnsi(lpBuffer);
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (lpBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(lpBuffer);
            }

            return sRet;
        }

        public static object ReadObject(uint dwAddress, Type objType)
        {
            IntPtr lpBuffer = IntPtr.Zero;
            int iBytesRead;
            int iObjectSize;
            object objRet;

            try
            {
                iObjectSize = Marshal.SizeOf(objType);
                lpBuffer = Marshal.AllocHGlobal(iObjectSize);

                iBytesRead = ReadRawMemory(ProcessHandle, dwAddress, lpBuffer, iObjectSize);
                if (iBytesRead != iObjectSize)
                    throw new Exception("ReadProcessMemory error in ReadObject.");

                objRet = Marshal.PtrToStructure(lpBuffer, objType);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (lpBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(lpBuffer);
            }

            return objRet;
        }

        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            uint dwAddress,
            IntPtr lpBuffer,
            int nSize,
            out int lpBytesRead);

        #endregion

        #region AllocateMemory

        public static uint AllocateMemory(int nSize, uint dwAddress, uint dwAllocationType, uint dwProtect)
        {
            return VirtualAllocEx(ProcessHandle, dwAddress, nSize, dwAllocationType, dwProtect);
        }

        public static uint AllocateMemory(int nSize, uint dwAllocationType, uint dwProtect)
        {
            return AllocateMemory(nSize, 0, dwAllocationType, dwProtect);
        }

        public static uint AllocateMemory(int nSize)
        {
            return AllocateMemory(nSize, MemoryAllocType.MEM_COMMIT, MemoryProtectType.PAGE_EXECUTE_READWRITE);
        }

        public static uint AllocateMemory()
        {
            return AllocateMemory(DEFAULT_MEMORY_SIZE);
        }

        #endregion

        #region FreeMemory

        public static bool FreeMemory(uint dwAddress, int nSize, uint dwFreeType)
        {
            if (dwFreeType == MemoryFreeType.MEM_RELEASE)
                nSize = 0;

            return VirtualFreeEx(ProcessHandle, dwAddress, nSize, dwFreeType);
        }

        public static bool FreeMemory(uint dwAddress)
        {
            return FreeMemory(dwAddress, 0, MemoryFreeType.MEM_RELEASE);
        }

        #endregion

        #region MemoryAllocType, MemoryProtectType, MemoryFreeType

        #region Nested type: MemoryAllocType

        public static class MemoryAllocType
        {
// ReSharper disable InconsistentNaming
            public const uint MEM_COMMIT = 0x00001000;
            public const uint MEM_RESERVE = 0x00002000;
            public const uint MEM_RESET = 0x00080000;
            public const uint MEM_PHYSICAL = 0x00400000;
            public const uint MEM_TOP_DOWN = 0x00100000;
// ReSharper restore InconsistentNaming
        }

        #endregion

        #region Nested type: MemoryFreeType

        public static class MemoryFreeType
        {
// ReSharper disable InconsistentNaming
            public const uint MEM_DECOMMIT = 0x4000;
            public const uint MEM_RELEASE = 0x8000;
            // ReSharper restore InconsistentNaming
        }

        #endregion

        #region Nested type: MemoryProtectType

        public static class MemoryProtectType
        {
            // ReSharper disable InconsistentNaming
            public const uint PAGE_EXECUTE = 0x10;
            public const uint PAGE_EXECUTE_READ = 0x20;
            public const uint PAGE_EXECUTE_READWRITE = 0x40;
            public const uint PAGE_EXECUTE_WRITECOPY = 0x80;
            public const uint PAGE_NOACCESS = 0x01;
            public const uint PAGE_READONLY = 0x02;
            public const uint PAGE_READWRITE = 0x04;
            public const uint PAGE_WRITECOPY = 0x08;
            public const uint PAGE_GUARD = 0x100;
            public const uint PAGE_NOCACHE = 0x200;
            public const uint PAGE_WRITECOMBINE = 0x400;
            // ReSharper restore InconsistentNaming
        }

        #endregion

        #endregion

// ReSharper restore InconsistentNaming
    }
}