using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;

namespace NekoDrive.NFS.Wrappers
{
    public class NFSv3 : INFS
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        private struct NFSv3Data
        {
            public UInt32 DateTime;
            public UInt32 Type;
            public UInt64 Size;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] Handle;
        }

        private IntPtr _nfsv3;

        [DllImport("NFSv3.dll", EntryPoint = "??0CNFSv3@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern void __NFSv3_Constructor(IntPtr pThis);

        [DllImport("NFSv3.dll", EntryPoint = "??1CNFSv3@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern void __NFSv3_Destructor(IntPtr pThis);

        [DllImport("NFSv3.dll", EntryPoint = "?CreateCNFSv3@@YAPAVCNFSv3@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr __NFSv3_CreateCNFSv3();

        [DllImport("NFSv3.dll", EntryPoint = "?DisposeCNFSv3@@YAXPAVCNFSv3@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void __NFSv3_DisposeCNFSv3(IntPtr pThis);

        [DllImport("NFSv3.dll", EntryPoint = "?ChangeCurrentDirectory@CNFSv3@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_ChangeCurrentDirectory(IntPtr pThis, String pName);

        [DllImport("NFSv3.dll", EntryPoint = "?ChangeMode@CNFSv3@@QAEHPADH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_ChangeMode(IntPtr pThis, String pName, Int32 Mode);

        [DllImport("NFSv3.dll", EntryPoint = "?ChangeOwner@CNFSv3@@QAEHPADHH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_ChangeOwner(IntPtr pThis, String pName, Int32 UID, Int32 GID);

        [DllImport("NFSv3.dll", EntryPoint = "?CloseFile@CNFSv3@@QAEXXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_CloseFile(IntPtr pThis);

        [DllImport("NFSv3.dll", EntryPoint = "?Connect@CNFSv3@@QAEHPBDHHJ@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_Connect(IntPtr pThis, String ServerAddress, Int32 UID, Int32 GID, Int32 CommandTimeout);

        [DllImport("NFSv3.dll", EntryPoint = "?CreateDirectoryW@CNFSv3@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_CreateDirectory(IntPtr pThis, String pName);

        [DllImport("NFSv3.dll", EntryPoint = "?CreateFileW@CNFSv3@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_CreateFile(IntPtr pThis, String pName);

        [DllImport("NFSv3.dll", EntryPoint = "?DeleteFileW@CNFSv3@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_DeleteFile(IntPtr pThis, String pName);

        [DllImport("NFSv3.dll", EntryPoint = "?DeleteDirectory@CNFSv3@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_DeleteDirectory(IntPtr pThis, String pName);

        [DllImport("NFSv3.dll", EntryPoint = "?Disconnect@CNFSv3@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_Disconnect(IntPtr pThis);

        [DllImport("NFSv3.dll", EntryPoint = "?GetExportedDevices@CNFSv3@@QAEPAPADPAH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr __NFSv3_GetExportedDevices(IntPtr pThis, out Int32 pnSize);

        [DllImport("NFSv3.dll", EntryPoint = "?GetItemAttributes@CNFSv3@@QAEPAXPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr __NFSv3_GetItemAttributes(IntPtr pThis, String pItem);

        [DllImport("NFSv3.dll", EntryPoint = "?GetItemsList@CNFSv3@@QAEPAPADPAH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr __NFSv3_GetItemsList(IntPtr pThis, out Int32 pnSize);

        [DllImport("NFSv3.dll", EntryPoint = "?GetLastNfsError@CNFSv3@@QAEPBDXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern String __NFSv3_GetLastNfsError(IntPtr pThis);

        [DllImport("NFSv3.dll", EntryPoint = "?MountDevice@CNFSv3@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_MountDevice(IntPtr pThis, String pDevice);

        [DllImport("NFSv3.dll", EntryPoint = "?Open@CNFSv3@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_Open(IntPtr pThis, String pName);

        [DllImport("NFSv3.dll", EntryPoint = "?Read@CNFSv3@@QAEH_KKPADPAI@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_Read(IntPtr pThis, UInt64 Offset, UInt32 Count, IntPtr pBuffer, out Int32 pSize);

        [DllImport("NFSv3.dll", EntryPoint = "?ReleaseBuffer@CNFSv3@@QAEXPAX@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void __NFSv3_ReleaseBuffer(IntPtr pThis, IntPtr pBuffer);

        [DllImport("NFSv3.dll", EntryPoint = "?ReleaseBuffers@CNFSv3@@QAEXPAPAX@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void __NFSv3_ReleaseBuffers(IntPtr pThis, IntPtr pBuffers);

        [DllImport("NFSv3.dll", EntryPoint = "?Rename@CNFSv3@@QAEHPAD0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_Rename(IntPtr pThis, String pOldName, String pNewName);

        [DllImport("NFSv3.dll", EntryPoint = "?UnMountDevice@CNFSv3@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_UnMountDevice(IntPtr pThis);

        [DllImport("NFSv3.dll", EntryPoint = "?Write@CNFSv3@@QAEH_KKPADPAI@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_Write(IntPtr pThis, UInt64 Offset, UInt32 Count, IntPtr pBuffer, out Int32 pSize);

        public void Create()
        {
            _nfsv3 = __NFSv3_CreateCNFSv3();
        }

        public void Destroy()
        {
            __NFSv3_Destructor(_nfsv3);
        }

        public NFSResult Connect(IPAddress Address)
        {
            return Connect(Address, 0, 0, 60);
        }

        public NFSResult Connect(IPAddress Address, Int32 UserId, Int32 GroupId, Int32 CommandTimeout)
        {
            return (NFSResult)__NFSv3_Connect(_nfsv3, Address.ToString(), UserId, GroupId, CommandTimeout);
        }

        public NFSResult Disconnect()
        {
            return (NFSResult)__NFSv3_Disconnect(_nfsv3);
        }

        public IntPtr GetExportedDevices(out Int32 Size)
        {
            IntPtr pDevices;
            pDevices = __NFSv3_GetExportedDevices(_nfsv3, out Size);
            return pDevices;
        }

        public NFSResult MountDevice(String DeviceName)
        {
            return (NFSResult)__NFSv3_MountDevice(_nfsv3, DeviceName);
        }

        public NFSResult UnMountDevice()
        {
            return (NFSResult)__NFSv3_UnMountDevice(_nfsv3);
        }

        public IntPtr GetItemList(out Int32 Size)
        {
            IntPtr pItems;
            pItems = __NFSv3_GetItemsList(_nfsv3, out Size);
            return pItems;
        }

        public IntPtr GetItemAttributes(String ItemName)
        {
            IntPtr pAttributes;
            pAttributes = __NFSv3_GetItemAttributes(_nfsv3, ItemName);
            return pAttributes;
        }

        public NFSResult ChangeCurrentDirectory(String DirectoryName)
        {
            return (NFSResult)__NFSv3_ChangeCurrentDirectory(_nfsv3, DirectoryName);
        }

        public NFSResult CreateDirectory(String DirectoryName)
        {
            return (NFSResult)__NFSv3_CreateDirectory(_nfsv3, DirectoryName);
        }

        public NFSResult DeleteDirectory(String DirectoryName)
        {
            return (NFSResult)__NFSv3_DeleteDirectory(_nfsv3, DirectoryName);
        }

        public NFSResult DeleteFile(String FileName)
        {
            return (NFSResult)__NFSv3_DeleteFile(_nfsv3, FileName);
        }

        public NFSResult CreateFile(String FileName)
        {
            return (NFSResult)__NFSv3_CreateFile(_nfsv3, FileName);
        }

        public NFSResult Read(UInt64 Offset, UInt32 Count, IntPtr pBuffer, out Int32 Size)
        {
            return (NFSResult)__NFSv3_Read(_nfsv3, Offset, Count, pBuffer, out Size);
        }

        public NFSResult Write(UInt64 Offset, UInt32 Count, IntPtr pBuffer, out Int32 Size)
        {
            return (NFSResult)__NFSv3_Write(_nfsv3, Offset, Count, pBuffer, out Size);
        }

        public NFSResult Open(String FileName)
        {
            return (NFSResult)__NFSv3_Open(_nfsv3, FileName);
        }

        public void CloseFile()
        {
            __NFSv3_CloseFile(_nfsv3);
        }

        public NFSResult Rename(String OldName, String NewName)
        {
            return (NFSResult)__NFSv3_Rename(_nfsv3, OldName, NewName);
        }

        public NFSAttributes GetNfsAttribute(IntPtr pAttributes)
        {
            if (pAttributes != IntPtr.Zero)
            {
                NFSv3Data nfsData = (NFSv3Data)Marshal.PtrToStructure(pAttributes, typeof(NFSv3Data));
                NFSAttributes nfsAttributes = new NFSAttributes(nfsData.DateTime, nfsData.Type, nfsData.Size, nfsData.Handle);
                return nfsAttributes;
            }
            return null;
        }

        public void ReleaseBuffer(IntPtr pBuffer)
        {
            if (pBuffer != IntPtr.Zero)
            {
                __NFSv3_ReleaseBuffer(_nfsv3, pBuffer);
            }
        }

        public void ReleaseBuffers(IntPtr pBuffers)
        {
            if (pBuffers != IntPtr.Zero)
            {
                __NFSv3_ReleaseBuffers(_nfsv3, pBuffers);
            }
        }

        public String GetLastNfsError()
        {
            return __NFSv3_GetLastNfsError(_nfsv3);
        }
    }
}
