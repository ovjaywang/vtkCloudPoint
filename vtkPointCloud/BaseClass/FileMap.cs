using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace vtkPointCloud
{
    public class FileMap
    {
        /// <summary>
        /// 非文件映射方式读取数据
        /// </summary>
        /// <param name="fileaName"></param>
        /// <returns></returns>
        public List<string> ReadFile(string fileaName)
        {
            List<string> rtn = new List<string>();
            string content = string.Empty;
            using (StreamReader sr = new StreamReader(fileaName))
            {
                content = sr.ReadToEnd();//一次性读入内存
            }
            MemoryStream ms = new MemoryStream(Encoding.GetEncoding("GB2312").GetBytes(content));//放入内存流，以便逐行读取
            using (StreamReader sr = new StreamReader(ms))
            {
                while (sr.Peek() > -1)
                {
                    rtn.Add(sr.ReadLine());
                }
            }
            return rtn;
        }

        [StructLayout(LayoutKind.Sequential)]

        internal struct SYSTEM_INFO
        {
            public uint dwOemId;
            public uint dwPageSize;
            public uint lpMinimumApplicationAddress;
            public uint lpMaximumApplicationAddress;
            public uint dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public uint dwProcessorLevel;
            public uint dwProcessorRevision;
        }

        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const int OPEN_EXISTING = 3;
        private const int INVALID_HANDLE_VALUE = -1;
        private const int FILE_ATTRIBUTE_NORMAL = 0x80;
        private const uint FILE_FLAG_SEQUENTIAL_SCAN = 0x08000000;
        private const uint PAGE_READWRITE = 0x04;

        private const int FILE_MAP_COPY = 1;
        private const int FILE_MAP_WRITE = 2;
        private const int FILE_MAP_READ = 4;

        /// <summary>
        /// 内存映射文件句柄
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="lpFileMappingAttributes"></param>
        /// <param name="flProtect"></param>
        /// <param name="dwMaximumSizeHigh"></param>
        /// <param name="dwMaximumSizeLow"></param>
        /// <param name="lpName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpFileMappingAttributes, uint flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);
        /// <summary>
        /// 内存映射文件
        /// </summary>
        /// <param name="hFileMappingObject"></param>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="dwFileOffsetHigh"></param>
        /// <param name="dwFileOffsetLow"></param>
        /// <param name="dwNumberOfBytesToMap"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

        /// <summary>
        /// 撤消文件映像
        /// </summary>
        /// <param name="lpBaseAddress"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

        /// <summary>
        /// 关闭内核对象句柄
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// 打开要映射的文件
        /// </summary>
        /// <param name="lpFileName"></param>
        /// <param name="dwDesiredAccess"></param>
        /// <param name="dwShareMode"></param>
        /// <param name="securityAttrs"></param>
        /// <param name="dwCreationDisposition"></param>
        /// <param name="dwFlagsAndAttributes"></param>
        /// <param name="hTemplateFile"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, FileShare dwShareMode, IntPtr securityAttrs, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        /// <summary>
        /// 得到文件大小
        /// </summary>
        /// <param name="hFile"></param>
        /// <param name="highSize"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern uint GetFileSize(IntPtr hFile, out uint highSize);

        /// <summary>
        /// 得到系统信息
        /// </summary>
        /// <param name="lpSystemInfo"></param>
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        /// <summary>
        /// 使用内存文件映射得到文件内容(读取大文件)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public StringBuilder ReadFileMap(string path)
        {
            StringBuilder sb = new StringBuilder();
            IntPtr fileHandle = CreateFile(path, GENERIC_READ | GENERIC_WRITE, FileShare.Read | FileShare.Write, IntPtr.Zero, FileMode.Open, FILE_ATTRIBUTE_NORMAL | FILE_FLAG_SEQUENTIAL_SCAN, IntPtr.Zero);
            if (INVALID_HANDLE_VALUE != (int)fileHandle)
            {
                IntPtr mappingFileHandle = CreateFileMapping(
                    fileHandle, IntPtr.Zero, PAGE_READWRITE, 0, 0, "~MappingTemp");
                if (mappingFileHandle != IntPtr.Zero)
                {
                    SYSTEM_INFO systemInfo = new SYSTEM_INFO(); ;
                    GetSystemInfo(ref systemInfo);
                    //得到系统页分配粒度
                    uint allocationGranularity = systemInfo.dwAllocationGranularity;
                    uint fileSizeHigh = 0;
                    //get file size
                    uint fileSize = GetFileSize(fileHandle, out fileSizeHigh);
                    fileSize |= (((uint)fileSizeHigh) << 32);
                    //关闭文件句柄 
                    CloseHandle(fileHandle);
                    uint fileOffset = 0;
                    uint blockBytes = 1000 * allocationGranularity;
                    if (fileSize < 1000 * allocationGranularity)
                        blockBytes = fileSize;
                    //分块读取内存，适用于几G的文件
                    while (fileSize > 0)
                    {
                        // 映射视图，得到地址 
                        IntPtr lpbMapAddress = MapViewOfFile(mappingFileHandle, FILE_MAP_COPY | FILE_MAP_READ | FILE_MAP_WRITE,
                           (uint)(fileOffset >> 32), (uint)(fileOffset & 0xFFFFFFFF),
                           blockBytes);
                        if (lpbMapAddress == IntPtr.Zero)
                        {
                            return sb;
                        }
                        // 对映射的视图进行访问
                        byte[] temp = new byte[blockBytes];
                        //从非托管的内存中复制内容到托管的内存中
                        Marshal.Copy(lpbMapAddress, temp, 0, (int)blockBytes);

                        //用循环太慢了，文件有几M的时候就慢的要死，还是用上面的方法直接
                        //for (uint i = 0; i < dwBlockBytes; i++)
                        //{
                        //    byte vTemp = Marshal.ReadByte((IntPtr)((int)lpbMapAddress + i));
                        //    temp[i] = vTemp;
                        //}
                        //此时用ASCII解码比较快，但有中文会有乱码，用gb2312即ANSI编码也比较快，16M的文件大概4秒就读出来了
                        //但用unicode解码，文件大的时候会非常慢，会现卡死的状态，不知道为什么？
                        //ASCIIEncoding encoding = new ASCIIEncoding();
                        //System.Text.UnicodeEncoding encoding = new UnicodeEncoding();
                        //sb.Append(encoding.GetString(temp));
                        sb.Append(System.Text.Encoding.GetEncoding("gb2312").GetString(temp));
                        // 撤消文件映像
                        UnmapViewOfFile(lpbMapAddress);
                        // 修正参数
                        fileOffset += blockBytes;
                        fileSize -= blockBytes;
                    }
                    //close file mapping handle
                    CloseHandle(mappingFileHandle);
                }
            }
            return sb;
        }
    }
}
