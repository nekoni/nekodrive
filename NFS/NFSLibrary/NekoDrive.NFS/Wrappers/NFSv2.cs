using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;

namespace NekoDrive.NFS.Wrappers
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
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

    public unsafe class NFSv2: INFS
    {
        private IntPtr _nfsv2;

        private IPAddress _Address;

        [DllImport("NFSv2.dll", EntryPoint = "??0CNFSv2@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern void __NFSv2_Constructor(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "??1CNFSv2@@QAE@XZ", CallingConvention = CallingConvention.ThisCall)]
        private static extern void __NFSv2_Destructor(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?CreateCNFSv2@@YAPAVCNFSv2@@XZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr __NFSv2_CreateCNFSv2();

        [DllImport("NFSv2.dll", EntryPoint = "?DisposeCNFSv2@@YAXPAVCNFSv2@@@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void __NFSv2_DisposeCNFSv2(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?ChangeCurrentDirectory@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_ChangeCurrentDirectory(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?ChangeMode@CNFSv2@@QAEHPADH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_ChangeMode(IntPtr pThis, String pName, Int32 Mode);

        [DllImport("NFSv2.dll", EntryPoint = "?ChangeOwner@CNFSv2@@QAEHPADHH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_ChangeOwner(IntPtr pThis, String pName, Int32 UID, Int32 GID);

        [DllImport("NFSv2.dll", EntryPoint = "?CloseFile@CNFSv2@@QAEXXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_CloseFile(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?Connect@CNFSv2@@QAEHIHH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_Connect(IntPtr pThis, UInt32 ServerAddress, Int32 UID, Int32 GID);

        [DllImport("NFSv2.dll", EntryPoint = "?CreateDirectoryW@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_CreateDirectory(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?CreateFileW@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_CreateFile(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?DeleteFileW@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_DeleteFile(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?DeleteDirectory@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_DeleteDirectory(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?Disconnect@CNFSv2@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_Disconnect(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?GetExportedDevices@CNFSv2@@QAEPAPADPAH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr __NFSv2_GetExportedDevices(IntPtr pThis, out Int32 pnSize);

        [DllImport("NFSv2.dll", EntryPoint = "?GetItemAttributes@CNFSv2@@QAEPAXPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr __NFSv2_GetItemAttributes(IntPtr pThis, String pItem);

        [DllImport("NFSv2.dll", EntryPoint = "?GetItemsList@CNFSv2@@QAEPAPADPAH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern IntPtr __NFSv2_GetItemsList(IntPtr pThis, out Int32 pnSize);

        [DllImport("NFSv2.dll", EntryPoint = "?GetLastNfsError@CNFSv2@@QAEPBDXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern String __NFSv2_GetLastNfsError(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?MountDevice@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_MountDevice(IntPtr pThis, String pDevice);

        [DllImport("NFSv2.dll", EntryPoint = "?Open@CNFSv2@@QAEHPAD@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_Open(IntPtr pThis, String pName);

        [DllImport("NFSv2.dll", EntryPoint = "?Read@CNFSv2@@QAEHIIPADPAK@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_Read(IntPtr pThis, UInt32 Offset, UInt32 Count, IntPtr pBuffer, out Int32 pSize);

        [DllImport("NFSv2.dll", EntryPoint = "?ReleaseBuffer@CNFSv2@@QAEXPAX@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void __NFSv2_ReleaseBuffer(IntPtr pThis, IntPtr pBuffer);

        [DllImport("NFSv2.dll", EntryPoint = "?ReleaseBuffers@CNFSv2@@QAEXPAPAX@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void __NFSv2_ReleaseBuffers(IntPtr pThis, IntPtr pBuffers);

        [DllImport("NFSv2.dll", EntryPoint = "?Rename@CNFSv2@@QAEHPAD0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_Rename(IntPtr pThis, String pOldName, String pNewName);

        [DllImport("NFSv2.dll", EntryPoint = "?UnMountDevice@CNFSv2@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_UnMountDevice(IntPtr pThis);

        [DllImport("NFSv2.dll", EntryPoint = "?Write@CNFSv2@@QAEHIIPADPAK@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv2_Write(IntPtr pThis, UInt32 Offset, UInt32 Count, IntPtr pBuffer, out Int32 pSize);

        public event NFSDataEventHandler DataEvent;

        public NFSv2(IPAddress ServerAddrss)
        {
            _Address = ServerAddrss;
            _nfsv2 = __NFSv2_CreateCNFSv2();
        }

        public void Dispose()
        {
            __NFSv2_Destructor(_nfsv2);
        }

        public NFSResult Connect()
        {
            return Connect(0, 0);
        }

        public NFSResult Connect(Int32 UserId, Int32 GroupId)
        {
            return (NFSResult)__NFSv2_Connect(_nfsv2, (UInt32)_Address.Address, UserId, GroupId);
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
            if (pAttributes != IntPtr.Zero)
            {
                NFSv2Data nfsData = (NFSv2Data)Marshal.PtrToStructure(pAttributes, typeof(NFSv2Data));
                NFSAttributes nfsAttributes = new NFSAttributes(nfsData.DateTime, nfsData.Type, nfsData.Size, nfsData.Handle);
                __NFSv2_ReleaseBuffer(_nfsv2, pAttributes);

                return nfsAttributes;
            }
            return null;
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

        public NFSResult Read(String FileName, String OutputFileName)
        {
            FileStream fs = null;
            try
            {
                NFSResult Result = NFSResult.NFS_ERROR;
                if (File.Exists(OutputFileName))
                    File.Delete(OutputFileName);
                fs = new FileStream(OutputFileName, FileMode.CreateNew);
                Result = Read(FileName, ref fs);
                return Result;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        public NFSResult Read(String FileName, ref FileStream OutputStream)
        {
            NFSResult Result = NFSResult.NFS_ERROR;
            if (OutputStream != null)
            {
                NFSAttributes nfsAttributes = GetItemAttributes(FileName);
                if (NFSResult.NFS_SUCCESS == Open(FileName))
                {
                    UInt32 TotalLenght = (UInt32) nfsAttributes.size;
                    UInt32 BlockSize = 4096;
                    UInt32 CuttentPosition = 0;
                    do
                    {
                        UInt32 Count = BlockSize;
                        if ((TotalLenght - CuttentPosition) < BlockSize)
                            Count = (UInt32)TotalLenght - CuttentPosition;

                        Byte[] Data = null;
                        int pSize = -1;
                        if ((pSize = Read(CuttentPosition, Count, ref Data)) != -1)
                        {
                            OutputStream.Write(Data, 0, pSize);
                            OutputStream.Flush();
                            CuttentPosition += (UInt32)pSize;
                            Result = NFSResult.NFS_SUCCESS;
                        }
                        else
                        {
                            Result = NFSResult.NFS_ERROR;
                            break;
                        }
                    } while (CuttentPosition != TotalLenght);
                    CloseFile();
                }
            }
            return Result;
        }

        public int Read(UInt64 Offset, UInt32 Count, ref Byte[] Buffer)
        {
            Int32 Size = -1;
            IntPtr pBuffer = Marshal.AllocHGlobal((Int32)Count);
            NFSResult Result = (NFSResult)__NFSv2_Read(_nfsv2, (UInt32) Offset, Count, pBuffer, out Size);
            if (Result == NFSResult.NFS_ERROR)
                Size = -1;
            else
            {
                Buffer = new Byte[Size];
                Marshal.Copy(pBuffer, Buffer, 0, Size);
                if (DataEvent != null)
                {
                    NFSEventArgs e = new NFSEventArgs();
                    e.Bytes = (UInt32)Size;
                    DataEvent(this, e);
                }
            }
            Marshal.FreeHGlobal(pBuffer);
            return Size;
        }

        public NFSResult Write(String FileName, String InputFileName)
        {
            NFSResult Result = NFSResult.NFS_ERROR;
            if (File.Exists(InputFileName))
            {
                FileStream wfs = new FileStream(InputFileName, FileMode.Open, FileAccess.Read);
                Result = Write(FileName, wfs);
                wfs.Close();
            }
            return Result;
        }

        public NFSResult Write(String FileName, FileStream InputStream)
        {
            NFSResult Result = NFSResult.NFS_ERROR;
            if (InputStream != null)
            {
                if (NFSResult.NFS_SUCCESS == CreateFile(FileName))
                {
                    if (NFSResult.NFS_SUCCESS == Open(FileName))
                    {
                        NFSAttributes nfsAttributes = GetItemAttributes(FileName);
                        UInt32 Offset = 0;
                        UInt32 Count = 4096;
                        Int32 Bytes = 0;
                        Byte[] Buffer = new Byte[Count];
                        while ((Bytes = InputStream.Read(Buffer, 0, (Int32)Count)) > 0)
                        {
                            Int32 Res = Write(Offset, (UInt32) Bytes, Buffer);
                            if (Res != -1)
                            {
                                Offset += (UInt32)Bytes;
                                Result = NFSResult.NFS_SUCCESS;
                            }
                            else
                            {
                                Result = NFSResult.NFS_ERROR;
                                break;
                            }
                        }
                        CloseFile();
                    }
                }
            }

            return Result;
        }

        public int Write(UInt64 Offset, UInt32 Count, Byte[] Buffer)
        {
            Int32 Size = -1;
            if (Buffer != null)
            {
                IntPtr pBuffer = Marshal.AllocHGlobal((Int32)Count);
                Marshal.Copy(Buffer, 0, pBuffer, (Int32) Count);
                NFSResult Result = (NFSResult)__NFSv2_Write(_nfsv2, (UInt32) Offset, Count, pBuffer, out Size);
                Marshal.FreeHGlobal(pBuffer);
                if (Result == NFSResult.NFS_ERROR)
                    Size = -1;
                else
                {
                    if (DataEvent != null)
                    {
                        NFSEventArgs e = new NFSEventArgs();
                        e.Bytes = (UInt32) Count;
                        DataEvent(this, e);
                    }
                }
            }
            return Size;
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

        public bool FileExists(String FileName)
        {
            return (GetItemAttributes(FileName) != null);
        }
    }
}
