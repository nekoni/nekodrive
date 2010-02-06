using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NekoDrive.NFS.Utility;

namespace NekoDrive.NFS.Wrappers
{
    public enum NFSResult
    {
        NFS_SUCCESS,
        NFS_ERROR
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public unsafe struct __NFSv2
    {
        public IntPtr* _vtable;
    }

    public unsafe class NFSv2: IDisposable
    {
        private __NFSv2* _nfsv2;

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

        public NFSv2()
        {
            _nfsv2 = (__NFSv2*)Memory.Alloc(sizeof(__NFSv2));
            __NFSv2_Constructor(_nfsv2);
        }

        public void Dispose()
        {
            __NFSv2_Destructor(_nfsv2);
            Memory.Free(_nfsv2);
            _nfsv2 = null;
        }

        public NFSResult Connect(UInt32 ServerAddrss)
        {
            return (NFSResult)__NFSv2_Connect(_nfsv2, ServerAddrss);
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
    }
}
