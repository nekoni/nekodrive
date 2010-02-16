using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;

namespace NekoDrive.NFS.Wrappers
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct NFSv3Data
    {
        public UInt32 DateTime;
        public UInt32 Type;
        public UInt64 Size;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] Handle;
    }

    public unsafe class NFSv3 : INFS, IDisposable
    {
        private IntPtr _nfsv3;

        private IPAddress _Address;

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

        [DllImport("NFSv3.dll", EntryPoint = "?Connect@CNFSv3@@QAEHIHH@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_Connect(IntPtr pThis, UInt32 ServerAddress, Int32 UID, Int32 GID);

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

        [DllImport("NFSv3.dll", EntryPoint = "?Read@CNFSv3@@QAEHIIPADPAK@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_Read(IntPtr pThis, UInt64 Offset, UInt32 Count, IntPtr pBuffer, out Int32 pSize);

        [DllImport("NFSv3.dll", EntryPoint = "?ReleaseBuffer@CNFSv3@@QAEXPAX@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void __NFSv3_ReleaseBuffer(IntPtr pThis, IntPtr pBuffer);

        [DllImport("NFSv3.dll", EntryPoint = "?ReleaseBuffers@CNFSv3@@QAEXPAPAX@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern void __NFSv3_ReleaseBuffers(IntPtr pThis, IntPtr pBuffers);

        [DllImport("NFSv3.dll", EntryPoint = "?Rename@CNFSv3@@QAEHPAD0@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_Rename(IntPtr pThis, String pOldName, String pNewName);

        [DllImport("NFSv3.dll", EntryPoint = "?UnMountDevice@CNFSv3@@QAEHXZ", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_UnMountDevice(IntPtr pThis);

        [DllImport("NFSv3.dll", EntryPoint = "?Write@CNFSv3@@QAEHIIPADPAK@Z", CallingConvention = CallingConvention.ThisCall)]
        public static extern int __NFSv3_Write(IntPtr pThis, UInt64 Offset, UInt32 Count, IntPtr pBuffer, out Int32 pSize);

        public event NFSDataEventHandler DataEvent;

        public NFSv3(IPAddress ServerAddrss)
        {
            _Address = ServerAddrss;
            _nfsv3 = __NFSv3_CreateCNFSv3();
        }

        public void Dispose()
        {
            __NFSv3_Destructor(_nfsv3);
        }

        public NFSResult Connect()
        {
            return Connect(0, 0);
        }

        public NFSResult Connect(Int32 UserId, Int32 GroupId)
        {
            return (NFSResult)__NFSv3_Connect(_nfsv3, (UInt32)_Address.Address, UserId, GroupId);
        }

        public NFSResult Disconnect()
        {
            return (NFSResult)__NFSv3_Disconnect(_nfsv3);
        }

        public List<String> GetExportedDevices()
        {
            Int32 Size;
            IntPtr pDevices;
            IntPtr pCurrentDevice;
            List<String> DevicesList = new List<String>();

            pDevices = __NFSv3_GetExportedDevices(_nfsv3, out Size);
            for (Int32 i = 0; i < Size; i++)
            {
                pCurrentDevice = Marshal.ReadIntPtr(new IntPtr(pDevices.ToInt32() + IntPtr.Size * i));
                DevicesList.Add(Marshal.PtrToStringAnsi(pCurrentDevice));
            }
            __NFSv3_ReleaseBuffers(_nfsv3, pDevices);
            return DevicesList;
        }

        public NFSResult MountDevice(String DeviceName)
        {
            return (NFSResult)__NFSv3_MountDevice(_nfsv3, DeviceName);
        }

        public NFSResult UnMountDevice()
        {
            return (NFSResult)__NFSv3_UnMountDevice(_nfsv3);
        }

        public List<String> GetItemList()
        {
            Int32 Size;
            IntPtr pItems;
            IntPtr pCurrentItem;
            List<String> ItemsList = new List<String>();

            pItems = __NFSv3_GetItemsList(_nfsv3, out Size);
            for (Int32 i = 0; i < Size; i++)
            {
                pCurrentItem = Marshal.ReadIntPtr(new IntPtr(pItems.ToInt32() + IntPtr.Size * i));
                ItemsList.Add(Marshal.PtrToStringAnsi(pCurrentItem));
            }
            __NFSv3_ReleaseBuffers(_nfsv3, pItems);
            return ItemsList;
        }

        public NFSAttributes GetItemAttributes(String ItemName)
        {
            IntPtr pAttributes;

            pAttributes = __NFSv3_GetItemAttributes(_nfsv3, ItemName);
            NFSv3Data nfsData = (NFSv3Data)Marshal.PtrToStructure(pAttributes, typeof(NFSv3Data));

            NFSAttributes nfsAttributes = new NFSAttributes(nfsData.DateTime, nfsData.Type, nfsData.Size, nfsData.Handle);

            __NFSv3_ReleaseBuffer(_nfsv3, pAttributes);

            return nfsAttributes;
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
                    UInt64 TotalLenght = nfsAttributes.size;
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
                            CuttentPosition += (UInt32) pSize;
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
            NFSResult Result = (NFSResult)__NFSv3_Read(_nfsv3, Offset, Count, pBuffer, out Size);
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
                        UInt64 Offset = 0;
                        UInt32 Count = 4096;
                        Int32 Bytes = 0;
                        Byte[] Buffer = new Byte[Count];
                        while ((Bytes = InputStream.Read(Buffer, 0, (Int32)Count)) > 0)
                        {
                            Int32 Res = Write(Offset, (UInt32)Bytes, Buffer);
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
                Marshal.Copy(Buffer, 0, pBuffer, (Int32)Count);
                NFSResult Result = (NFSResult)__NFSv3_Write(_nfsv3, Offset, Count, pBuffer, out Size);
                Marshal.FreeHGlobal(pBuffer);
                if (Result == NFSResult.NFS_ERROR)
                    Size = -1;
                else
                {
                    if (DataEvent != null)
                    {
                        NFSEventArgs e = new NFSEventArgs();
                        e.Bytes = (UInt32)Size;
                        DataEvent(this, e);
                    }
                }
            }
            return Size;
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
    }
}
