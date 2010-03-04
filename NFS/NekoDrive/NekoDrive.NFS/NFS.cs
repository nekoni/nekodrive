using System;
using System.Collections.Generic;
using System.Text;
using NekoDrive.NFS.Wrappers;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;

namespace NekoDrive.NFS
{
    public class NFS: IDisposable
    {
        public enum NFSVersion
        {
            v2 = 2,
            v3 = 3,
            v4 = 4
        }

        public event NFSDataEventHandler DataEvent;
        
        private INFS nfsInterface = null;

        public bool IsMounted = false;
        public bool IsConnected = false;

        public NFS(NFSVersion Version)
        {
            switch (Version)
            {
                case NFSVersion.v2:
                    nfsInterface = new NFSv2();
                    break;

                case NFSVersion.v3:
                    nfsInterface = new NFSv3();
                    break;

                default:
                    throw new NotImplementedException();
            }
            nfsInterface.Create();
        }

        public void Dispose()
        {
            nfsInterface.Destroy();
        }

        public NFSResult Connect(IPAddress Address)
        {
            return nfsInterface.Connect(Address, 0, 0, 60);
        }

        public NFSResult Connect(IPAddress Address, Int32 UserId, Int32 GroupId, Int32 CommandTimeout)
        {
            NFSResult res = nfsInterface.Connect(Address, UserId, GroupId, CommandTimeout);
            if (res == NFSResult.NFS_SUCCESS)
                IsConnected = true;
            return res;
        }

        public NFSResult Disconnect()
        {
            NFSResult res = nfsInterface.Disconnect();
            if (res == NFSResult.NFS_SUCCESS)
                IsConnected = false;
            return res;
        }

        public List<String> GetExportedDevices()
        {
            Int32 Size;
            IntPtr pDevices;
            IntPtr pCurrentDevice;
            List<String> DevicesList = new List<String>();
            pDevices = nfsInterface.GetExportedDevices(out Size);
            for (Int32 i = 0; i < Size; i++)
            {
                pCurrentDevice = Marshal.ReadIntPtr(new IntPtr(pDevices.ToInt32() + IntPtr.Size * i));
                DevicesList.Add(Marshal.PtrToStringAnsi(pCurrentDevice));
            }
            nfsInterface.ReleaseBuffers(pDevices);
            return DevicesList;
        }

        public NFSResult MountDevice(String DeviceName)
        {
            NFSResult res = nfsInterface.MountDevice(DeviceName);
            if(res == NFSResult.NFS_SUCCESS)
                IsMounted = true;
            return res;
        }

        public NFSResult UnMountDevice()
        {
            NFSResult res = nfsInterface.UnMountDevice();
            if (res == NFSResult.NFS_SUCCESS)
                IsMounted = false;
            return res;
        }

        public List<String> GetItemList()
        {
            Int32 Size;
            IntPtr pItems;
            IntPtr pCurrentItem;
            List<String> ItemsList = new List<String>();
            pItems = nfsInterface.GetItemList(out Size);
            for (Int32 i = 0; i < Size; i++)
            {
                pCurrentItem = Marshal.ReadIntPtr(new IntPtr(pItems.ToInt32() + IntPtr.Size * i));
                ItemsList.Add(Marshal.PtrToStringAnsi(pCurrentItem));
            }
            nfsInterface.ReleaseBuffers(pItems);
            return ItemsList;
        }

        public NFSAttributes GetItemAttributes(String ItemName)
        {
            IntPtr pAttributes;
            pAttributes = nfsInterface.GetItemAttributes(ItemName);
            if (pAttributes != IntPtr.Zero)
            {
                NFSAttributes nfsAttributes = nfsInterface.GetNfsAttribute(pAttributes);
                nfsInterface.ReleaseBuffer(pAttributes);
                return nfsAttributes;
            }
            return null;
        }

        public NFSResult ChangeCurrentDirectory(String DirectoryName)
        {
            return nfsInterface.ChangeCurrentDirectory(DirectoryName);
        }

        public NFSResult CreateDirectory(String DirectoryName)
        {
            return nfsInterface.CreateDirectory(DirectoryName);
        }

        public NFSResult DeleteDirectory(String DirectoryName)
        {
            return nfsInterface.DeleteDirectory(DirectoryName);
        }

        public NFSResult DeleteFile(String FileName)
        {
            return nfsInterface.DeleteFile(FileName);
        }

        public NFSResult CreateFile(String FileName)
        {
            return nfsInterface.CreateFile(FileName);
        }

        public NFSResult Read(List<String> FileNames, String OutputFolder)
        {
            NFSResult result = NFSResult.NFS_ERROR;
            if (Directory.Exists(OutputFolder))
            {
                foreach (String FileName in FileNames)
                {
                    if((result = Read(FileName, Path.Combine(OutputFolder, FileName))) != NFSResult.NFS_SUCCESS) 
                        break;
                }
            }
            return result;
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
                Result = Read(FileName, fs);
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

        public NFSResult Read(String FileName, Stream OutputStream)
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

        public Int32 Read(UInt64 Offset, UInt32 Count, ref Byte[] Buffer)
        {
            Int32 Size = -1;
            IntPtr pBuffer = Marshal.AllocHGlobal((Int32)Count);
            NFSResult Result = nfsInterface.Read(Offset, Count, pBuffer, out Size);
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

        public NFSResult Write(String FileName, long InputOffset, Stream InputStream)
        {
            NFSResult Result = NFSResult.NFS_ERROR;
            if (InputStream != null)
            {
                if (!FileExists(FileName))
                {
                    if (CreateFile(FileName) != NFSResult.NFS_SUCCESS)
                        return NFSResult.NFS_ERROR;
                }
                
                if (NFSResult.NFS_SUCCESS == Open(FileName))
                {
                    NFSAttributes nfsAttributes = GetItemAttributes(FileName);
                    UInt64 Offset = (UInt64) InputOffset;
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

            return Result;

        }

        public NFSResult Write(String FileName, Stream InputStream)
        {
            return Write(FileName, 0, InputStream);   
        }

        public Int32 Write(UInt64 Offset, UInt32 Count, Byte[] Buffer)
        {
            Int32 Size = -1;
            if (Buffer != null)
            {
                IntPtr pBuffer = Marshal.AllocHGlobal((Int32)Count);
                Marshal.Copy(Buffer, 0, pBuffer, (Int32)Count);
                NFSResult Result = nfsInterface.Write(Offset, Count, pBuffer, out Size);
                Marshal.FreeHGlobal(pBuffer);
                if (Result == NFSResult.NFS_ERROR)
                    Size = -1;
                else
                {
                    if (DataEvent != null)
                    {
                        NFSEventArgs e = new NFSEventArgs();
                        e.Bytes = (UInt32)Count;
                        DataEvent(this, e);
                    }
                }
            }
            return Size;
        }

        public NFSResult Open(String FileName)
        {
            return nfsInterface.Open(FileName);
        }

        public void CloseFile()
        {
            nfsInterface.CloseFile();
        }

        public NFSResult Rename(String OldName, String NewName)
        {
            return nfsInterface.Rename(OldName, NewName);
        }

        public Boolean FileExists(String FileName)
        {
            return (GetItemAttributes(FileName) != null);
        }

        public String GetLastError()
        {
            return nfsInterface.GetLastNfsError();
        }
    }
}
