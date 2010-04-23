using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;

namespace NekoDrive.NFS.Wrappers
{
    public class NFSv2: INFS
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        private struct NFSv2Data
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

        private IntPtr _nfsv2;

        [DllImport("NFSv2.dll", EntryPoint = "??0CNFSv2@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern void __NFSv2_Constructor(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "??1CNFSv2@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern void __NFSv2_Destructor(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?CreateCNFSv2@@YAPAVCNFSv2@@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern IntPtr __NFSv2_CreateCNFSv2();

        [DllImport("NFSv2.dll", EntryPoint = "?DisposeCNFSv2@@YAXPAVCNFSv2@@@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern void __NFSv2_DisposeCNFSv2(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?ChangeCurrentDirectory@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_ChangeCurrentDirectory(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?ChangeMode@CNFSv2@@QAEHPADH@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_ChangeMode(IntPtr pThis, String pName, Int32 Mode);

        [DllImport("NFSv2.dll", EntryPoint = "?ChangeOwner@CNFSv2@@QAEHPADHH@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_ChangeOwner(IntPtr pThis, String pName, Int32 UID, Int32 GID);

        [DllImport("NFSv2.dll", EntryPoint = "?CloseFile@CNFSv2@@QAEXXZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_CloseFile(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?Connect@CNFSv2@@QAEHPBDHHJ@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_Connect(IntPtr pThis, String ServerAddress, Int32 UID, Int32 GID, Int32 CommandTimeout);

        [DllImport("NFSv2.dll", EntryPoint = "?CreateDirectoryW@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_CreateDirectory(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?CreateFileW@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_CreateFile(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?DeleteFileW@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_DeleteFile(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?DeleteDirectory@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_DeleteDirectory(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?Disconnect@CNFSv2@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_Disconnect(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?GetExportedDevices@CNFSv2@@QAEPAPADPAH@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern IntPtr __NFSv2_GetExportedDevices(IntPtr pThis, out Int32 pnSize);

        [DllImport("NFSv2.dll", EntryPoint = "?GetItemAttributes@CNFSv2@@QAEPAXPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern IntPtr __NFSv2_GetItemAttributes(IntPtr pThis, String pItem);

        [DllImport("NFSv2.dll", EntryPoint = "?GetItemsList@CNFSv2@@QAEPAPADPAH@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern IntPtr __NFSv2_GetItemsList(IntPtr pThis, out Int32 pnSize);

        [DllImport("NFSv2.dll", EntryPoint = "?GetLastNfsError@CNFSv2@@QAEPBDXZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern String __NFSv2_GetLastNfsError(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?MountDevice@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_MountDevice(IntPtr pThis, String pDevice);

        [DllImport("NFSv2.dll", EntryPoint = "?Open@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_Open(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?Read@CNFSv2@@QAEHIIPADPAK@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_Read(IntPtr pThis, UInt32 Offset, UInt32 Count, IntPtr pBuffer, out Int32 pSize);

        [DllImport("NFSv2.dll", EntryPoint = "?ReleaseBuffer@CNFSv2@@QAEXPAX@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern void __NFSv2_ReleaseBuffer(IntPtr pThis, IntPtr pBuffer);

        [DllImport("NFSv2.dll", EntryPoint = "?ReleaseBuffers@CNFSv2@@QAEXPAPAX@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern void __NFSv2_ReleaseBuffers(IntPtr pThis, IntPtr pBuffers);

        [DllImport("NFSv2.dll", EntryPoint = "?Rename@CNFSv2@@QAEHPAD0@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_Rename(IntPtr pThis, String pOldName, String pNewName);

        [DllImport("NFSv2.dll", EntryPoint = "?Move@CNFSv2@@QAEHPAD000@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_Move(IntPtr pThis, String pOldDirectoryName, String pOldName, String pNewDirectoryName, String pNewName);

        [DllImport("NFSv2.dll", EntryPoint = "?UnMountDevice@CNFSv2@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_UnMountDevice(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?Write@CNFSv2@@QAEHIIPADPAK@Z", CallingConvention = CallingConvention.ThisCall)]
        private static extern int __NFSv2_Write(IntPtr pThis, UInt32 Offset, UInt32 Count, IntPtr pBuffer, out Int32 pSize);

        public void Create()
        {
            _nfsv2 = __NFSv2_CreateCNFSv2();
        }

        public void Destroy()
        {
            __NFSv2_Destructor(_nfsv2);
        }

        public NFSResult Connect(IPAddress Address)
        {
            return Connect(Address, 0, 0, 60);
        }

        public NFSResult Connect(IPAddress Address, Int32 UserId, Int32 GroupId, Int32 CommandTimeout)
        {
            return (NFSResult)__NFSv2_Connect(_nfsv2, Address.ToString(), UserId, GroupId, CommandTimeout);
        }

        public NFSResult Disconnect()
        {
            return (NFSResult)__NFSv2_Disconnect(_nfsv2);
        }

        public IntPtr GetExportedDevices(out Int32 Size)
        {
            IntPtr pDevices;
            pDevices = __NFSv2_GetExportedDevices(_nfsv2, out Size);
            return pDevices;
        }

        public NFSResult MountDevice(String DeviceName)
        {
            return (NFSResult)__NFSv2_MountDevice(_nfsv2, DeviceName);
        }

        public NFSResult UnMountDevice()
        {
            return (NFSResult)__NFSv2_UnMountDevice(_nfsv2);
        }

        public IntPtr GetItemList(out Int32 Size)
        {
            IntPtr pItems;
            pItems = __NFSv2_GetItemsList(_nfsv2, out Size);
            return pItems;
        }

        public IntPtr GetItemAttributes(String ItemName)
        {
            IntPtr pAttributes;
            pAttributes = __NFSv2_GetItemAttributes(_nfsv2, ItemName);
            return pAttributes;
        }

        public NFSResult ChangeCurrentDirectory(String DirectoryName)
        {
            return (NFSResult)__NFSv2_ChangeCurrentDirectory(_nfsv2, DirectoryName);
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

        public NFSResult Read(UInt64 Offset, UInt32 Count, IntPtr pBuffer, out Int32 Size)
        {
            return (NFSResult)__NFSv2_Read(_nfsv2, (UInt32) Offset, Count, pBuffer, out Size);
        }

        public NFSResult Write(UInt64 Offset, UInt32 Count, IntPtr pBuffer, out Int32 Size)
        {
            return (NFSResult)__NFSv2_Write(_nfsv2, (UInt32) Offset, Count, pBuffer, out Size);
        }

        public NFSResult Open(String FileName)
        {
            return (NFSResult) __NFSv2_Open(_nfsv2, FileName);
        }

        public void CloseFile()
        {
            __NFSv2_CloseFile(_nfsv2);
        }

        public NFSResult Rename(String OldName, String NewName)
        {
            return (NFSResult)__NFSv2_Rename(_nfsv2, OldName, NewName);
        }

        public NFSResult Move(String OldDirectoryName, String OldName, String NewDirectoryName, String NewName)
        {
            return (NFSResult)__NFSv2_Move(_nfsv2, OldDirectoryName, OldName, NewDirectoryName, NewName);
        }

        public NFSAttributes GetNfsAttribute(IntPtr pAttributes)
        {
            if (pAttributes != IntPtr.Zero)
            {
                NFSv2Data nfsData = (NFSv2Data)Marshal.PtrToStructure(pAttributes, typeof(NFSv2Data));
                NFSAttributes nfsAttributes = new NFSAttributes(nfsData.DateTime, nfsData.Type, nfsData.Size, nfsData.Handle);
                return nfsAttributes;
            }
            return null;
        }

        public void ReleaseBuffer(IntPtr pBuffer)
        {
            if (pBuffer != IntPtr.Zero)
            {
                __NFSv2_ReleaseBuffer(_nfsv2, pBuffer);
            }
        }

        public void ReleaseBuffers(IntPtr pBuffers)
        {
            if (pBuffers != IntPtr.Zero)
            {
                __NFSv2_ReleaseBuffers(_nfsv2, pBuffers);
            }
        }

        public String GetLastNfsError()
        {
            return __NFSv2_GetLastNfsError(_nfsv2);
        }
    }
}
