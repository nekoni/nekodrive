using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NekoDrive.NFS.Utility;
using System.Net;
using System.IO;

namespace NekoDrive.NFS.Wrappers
{
    public enum NFSResult
    {
        NFS_SUCCESS,
        NFS_ERROR
    }

    public enum NFSType
    {
        NFNON = 0,
        NFREG = 1,
        NFDIR = 2,
        NFBLK = 3,
        NFCHR = 4,
        NFLNK = 5
    }

    public class NFSAttributes
    {
        public NFSAttributes(UInt32 dateTime, UInt32 type, UInt32 size, UInt32 blocks, UInt32 blockSize, byte[] handle)
        {
            this.dateTime = new System.DateTime(1970,1,1).AddSeconds(dateTime);
            this.type = (NFSType) type;
            this.size = size;
            this.blocks = blocks;
            this.blockSize = blockSize;
            this.handle = (byte[]) handle.Clone();
        }

        public DateTime dateTime;
        public NFSType type;
        public UInt32 size;
        public UInt32 blocks;
        public UInt32 blockSize;
        public byte[] handle;

        public override string ToString()
        {
            string Handle = string.Empty;
            foreach(byte b in handle)
                Handle += b.ToString("X");

            return "DateTime: " + dateTime.ToString() + " " +
                "Type: " + type.ToString() + " " +
                "Size: " + size + " " +
                "Blocks: " + blocks + " " +
                "BlockSize: " + blockSize + " " +
                "Handle: " + Handle;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public unsafe struct __NFSv2
    {
        public IntPtr* _vtable;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct NFSv2Data
    {
        public UInt32 DateTime;
        public UInt32 Type;
        public UInt32 Size;
        public UInt32 Blocks;
        public UInt32 BlockSize;
        public UInt64 Dummy;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] Handle;
    }

    public unsafe class NFSv2: IDisposable
    {
        private __NFSv2* _nfsv2;

        private IPAddress _Address;

        [DllImport("NFSv2.dll", EntryPoint = "??0CNFSv2@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern void __NFSv2_Constructor(__NFSv2* pThis);

        [DllImport("NFSv2.dll", EntryPoint = "??1CNFSv2@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern void __NFSv2_Destructor(__NFSv2* pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?Connect@CNFSv2@@QAEHI@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_Connect(__NFSv2* pThis, UInt32 ServerAddress);

        [DllImport("NFSv2.dll", EntryPoint = "?Disconnect@CNFSv2@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_Disconnect(__NFSv2* pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?GetExportedDevices@CNFSv2@@QAEPAPADPAH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr __NFSv2_GetExportedDevices(__NFSv2* pThis, out Int32 pnSize);

        [DllImport("NFSv2.dll", EntryPoint = "?GetItemsList@CNFSv2@@QAEPAPADPAH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr __NFSv2_GetItemsList(__NFSv2* pThis, out Int32 pnSize);

        [DllImport("NFSv2.dll", EntryPoint = "?ReleaseBuffers@CNFSv2@@QAEXPAPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void __NFSv2_ReleaseBuffers(__NFSv2* pThis, IntPtr pBuffers);

        [DllImport("NFSv2.dll", EntryPoint = "?ReleaseBuffer@CNFSv2@@QAEXPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void __NFSv2_ReleaseBuffer(__NFSv2* pThis, IntPtr pBuffer);

        [DllImport("NFSv2.dll", EntryPoint = "?MountDevice@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_MountDevice(__NFSv2* pThis, String pDevice);

        [DllImport("NFSv2.dll", EntryPoint = "?UnMountDevice@CNFSv2@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_UnMountDevice(__NFSv2* pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?GetItemAttributes@CNFSv2@@QAEPAXPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr __NFSv2_GetItemAttributes(__NFSv2* pThis, String pItem);

        [DllImport("NFSv2.dll", EntryPoint = "?ChangeCurrentDirectory@CNFSv2@@QAEXPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void __NFSv2_ChangeCurrentDirectory(__NFSv2* pThis, IntPtr pHandle);

        [DllImport("NFSv2.dll", EntryPoint = "?CreateDirectoryW@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_CreateDirectory(__NFSv2* pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?DeleteDirectory@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_DeleteDirectory(__NFSv2* pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?DeleteFileW@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_DeleteFile(__NFSv2* pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?CreateFileW@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_CreateFile(__NFSv2* pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?Read@CNFSv2@@QAEHPADII0PAK@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_Read(__NFSv2* pThis, IntPtr pHandle, UInt32 Offset, UInt32 Count, IntPtr pBuffer, out Int32 pSize);

        [DllImport("NFSv2.dll", EntryPoint = "?Write@CNFSv2@@QAEHPADII0PAK@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_Write(__NFSv2* pThis, IntPtr pHandle, UInt32 Offset, UInt32 Count, IntPtr pBuffer, out Int32 pSize);

        [DllImport("NFSv2.dll", EntryPoint = "?Rename@CNFSv2@@QAEHPAD0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_Rename(__NFSv2* pThis, String pOldName, String pNewName);

        public NFSv2(String ServerAddrss)
        {
            _Address = IPAddress.Parse(ServerAddrss);
            _nfsv2 = (__NFSv2*)Memory.Alloc(sizeof(__NFSv2));
            __NFSv2_Constructor(_nfsv2);
        }

        public void Dispose()
        {
            __NFSv2_Destructor(_nfsv2);
            Memory.Free(_nfsv2);
            _nfsv2 = null;
        }

        public NFSResult Connect()
        {
            return (NFSResult)__NFSv2_Connect(_nfsv2, (UInt32)_Address.Address);
        }

        public NFSResult Disconnect()
        {
            return (NFSResult)__NFSv2_Disconnect(_nfsv2);
        }

        public List<String> GetExportedDevices()
        {
            Int32 Size;
            IntPtr pDevices;
            IntPtr pCurrentDevice;
            List<String> DevicesList = new List<String>();

            pDevices = __NFSv2_GetExportedDevices(_nfsv2, out Size);
            for (Int32 i = 0; i < Size; i++)
            {
                pCurrentDevice = Marshal.ReadIntPtr(new IntPtr(pDevices.ToInt32() + IntPtr.Size * i));
                DevicesList.Add(Marshal.PtrToStringAnsi(pCurrentDevice));
            }
            __NFSv2_ReleaseBuffers(_nfsv2, pDevices);
            return DevicesList;
        }

        public NFSResult MountDevice(String DeviceName)
        {
            return (NFSResult)__NFSv2_MountDevice(_nfsv2, DeviceName);
        }

        public NFSResult UnMountDevice()
        {
            return (NFSResult)__NFSv2_UnMountDevice(_nfsv2);
        }

        public List<String> GetItemList()
        {
            Int32 Size;
            IntPtr pItems;
            IntPtr pCurrentItem;
            List<String> ItemsList = new List<String>();

            pItems = __NFSv2_GetItemsList(_nfsv2, out Size);
            for (Int32 i = 0; i < Size; i++)
            {
                pCurrentItem = Marshal.ReadIntPtr(new IntPtr(pItems.ToInt32() + IntPtr.Size * i));
                ItemsList.Add(Marshal.PtrToStringAnsi(pCurrentItem));
            }
            __NFSv2_ReleaseBuffers(_nfsv2, pItems);
            return ItemsList;
        }

        public NFSAttributes GetItemAttributes(String ItemName)
        {
            IntPtr pAttributes;

            pAttributes = __NFSv2_GetItemAttributes(_nfsv2, ItemName);
            NFSv2Data nfsData =(NFSv2Data) Marshal.PtrToStructure(pAttributes, typeof(NFSv2Data));

            NFSAttributes nfsAttributes = new NFSAttributes(nfsData.DateTime, nfsData.Type, nfsData.Size, nfsData.Blocks,
                nfsData.BlockSize, nfsData.Handle);

            __NFSv2_ReleaseBuffer(_nfsv2, pAttributes);

            return nfsAttributes;
        }

        public void ChangeCurrentDirectory(NFSAttributes DirectoryAttribures)
        {
            IntPtr pHandle = Marshal.AllocCoTaskMem(Marshal.SizeOf(DirectoryAttribures.handle));
            __NFSv2_ChangeCurrentDirectory(_nfsv2, pHandle);
            Marshal.FreeCoTaskMem(pHandle);
        }

        public NFSResult CreateDirectory(String DirectoryName)
        {
            return (NFSResult)__NFSv2_CreateDirectory(_nfsv2, DirectoryName);
        }

        public NFSResult DeleteDirectory(String DirectoryName)
        {
            return (NFSResult)__NFSv2_DeleteDirectory(_nfsv2, DirectoryName);
        }

        public NFSResult DeleteFile(String FileName)
        {
            return (NFSResult)__NFSv2_DeleteFile(_nfsv2, FileName);
        }

        public NFSResult CreateFile(String FileName)
        {
            return (NFSResult)__NFSv2_CreateFile(_nfsv2, FileName);
        }

        public NFSResult Read(NFSAttributes FileAttributes, out Stream OutputStream)
        {
            OutputStream = null;
            return NFSResult.NFS_SUCCESS;
        }
        
        public NFSResult Write(NFSAttributes FileAttributes, Stream InputStream)
        {
            return NFSResult.NFS_SUCCESS;
        }

        public NFSResult Rename(String OldName, String NewName)
        {
            return (NFSResult)__NFSv2_Rename(_nfsv2, OldName, NewName);
        }
    }
}
