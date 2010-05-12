using System;
using System.Collections.Generic;
using System.Text;
using NekoDrive.NFS.Wrappers;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;

namespace NekoDrive.NFS
{
    /// <summary>
    /// NFS Client Library
    /// </summary>
    public class NFS: IDisposable
    {
        #region Enum

        /// <summary>
        /// The NFS version to use
        /// </summary>
        public enum NFSVersion
        {
            /// <summary>
            /// NFS Version 2
            /// </summary>
            v2 = 2,
            /// <summary>
            /// NFS Version 3
            /// </summary>
            v3 = 3,
            /// <summary>
            /// NFS Version 4.1
            /// </summary>
            v4 = 4
        }

        #endregion

        #region Events

        /// <summary>
        /// This event is fired when data is transferred from/to the server
        /// </summary>
        public event NFSDataEventHandler DataEvent;

        #endregion

        #region Fields

        private INFS nfsInterface = null;
        private const int blockSize = 4096 + 2048 + 1024 + 512 + 256;

        #endregion

        #region Properties

        /// <summary>
        /// This property tells if the current export is mounted
        /// </summary>
        public bool IsMounted = false;

        /// <summary>
        /// This property tells if the connection is active
        /// </summary>
        public bool IsConnected = false;

        /// <summary>
        /// This property contains the current server directory
        /// </summary>
        public string CurrentDirectory = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// NFS Client Constructor
        /// </summary>
        /// <param name="Version">The required NFS version</param>
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

        #endregion

        #region Methods

        /// <summary>
        /// Destroy the interface
        /// </summary>
        public void Dispose()
        {
            nfsInterface.Destroy();
        }

        /// <summary>
        /// Create a connection to a NFS Server
        /// </summary>
        /// <param name="Address">The server address</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Connect(IPAddress Address)
        {
            return nfsInterface.Connect(Address, 0, 0, 60);
        }

        /// <summary>
        /// Create a connection to a NFS Server
        /// </summary>
        /// <param name="Address">The server address</param>
        /// <param name="UserId">The unix user id</param>
        /// <param name="GroupId">The unix group id</param>
        /// <param name="CommandTimeout">The command timeout in seconds</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Connect(IPAddress Address, Int32 UserId, Int32 GroupId, Int32 CommandTimeout)
        {
            NFSResult res = nfsInterface.Connect(Address, UserId, GroupId, CommandTimeout);
            if (res == NFSResult.NFS_SUCCESS)
                IsConnected = true;
            return res;
        }

        /// <summary>
        /// Close the current connection
        /// </summary>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Disconnect()
        {
            NFSResult res = nfsInterface.Disconnect();
            if (res == NFSResult.NFS_SUCCESS)
                IsConnected = false;
            return res;
        }

        /// <summary>
        /// Get the list of the exported NFS devices
        /// </summary>
        /// <returns>A list of the exported NFS devices</returns>
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

        /// <summary>
        /// Mount device
        /// </summary>
        /// <param name="DeviceName">The device name</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult MountDevice(String DeviceName)
        {
            NFSResult res = nfsInterface.MountDevice(DeviceName);
            if(res == NFSResult.NFS_SUCCESS)
                IsMounted = true;
            return res;
        }

        /// <summary>
        /// Unmount the current device
        /// </summary>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult UnMountDevice()
        {
            NFSResult res = nfsInterface.UnMountDevice();
            if (res == NFSResult.NFS_SUCCESS)
                IsMounted = false;
            return res;
        }

        /// <summary>
        /// Get the items in a directory
        /// </summary>
        /// <param name="Directory">Directory name (e.g. "directory/subdirectory" or "." for the root)</param>
        /// <returns>A list of the items name</returns>
        public List<String> GetItemList(String Directory)
        {
            Int32 Size;
            IntPtr pItems;
            IntPtr pCurrentItem;
            List<String> ItemsList = new List<String>();
            pItems = nfsInterface.GetItemList(Directory, out Size);
            for (Int32 i = 0; i < Size; i++)
            {
                pCurrentItem = Marshal.ReadIntPtr(new IntPtr(pItems.ToInt32() + IntPtr.Size * i));
                ItemsList.Add(Marshal.PtrToStringAnsi(pCurrentItem));
            }
            nfsInterface.ReleaseBuffers(pItems);
            return ItemsList;
        }
        
        /// <summary>
        /// Get the items in the current directory
        /// </summary>
        /// <returns>A list of the items name</returns>
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

        /// <summary>
        /// Get an item attribures
        /// </summary>
        /// <param name="ItemName">The item name</param>
        /// <param name="Directory">The item directory (e.g. "directory/subdirecoty" or "." for the root)</param>
        /// <returns>A NFSAttributes class</returns>
        public NFSAttributes GetItemAttributes(String ItemName, String Directory)
        {
            IntPtr pAttributes;
            pAttributes = nfsInterface.GetItemAttributes(ItemName, Directory);
            if (pAttributes != IntPtr.Zero)
            {
                NFSAttributes nfsAttributes = nfsInterface.GetNfsAttribute(pAttributes);
                nfsInterface.ReleaseBuffer(pAttributes);
                return nfsAttributes;
            }
            return null;
        }

        /// <summary>
        /// Get an item attributes in the current directory
        /// </summary>
        /// <param name="ItemName">The item name</param>
        /// <returns>A NFSAttributes class</returns>
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

        /// <summary>
        /// Open a remote directory
        /// </summary>
        /// <param name="Path">The full path of the directory (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult OpenDirectory(String Path)
        {
            NFSResult res = NFSResult.NFS_SUCCESS;

            if (nfsInterface.ChangeCurrentDirectory(Path) != NFSResult.NFS_SUCCESS)
                res = NFSResult.NFS_ERROR;
                
            return res;
        }

        /// <summary>
        /// Change the current directory
        /// </summary>
        /// <param name="DirectoryName">The full path of the directory (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult ChangeCurrentDirectory(String DirectoryName)
        {
            return nfsInterface.ChangeCurrentDirectory(DirectoryName);
        }

        /// <summary>
        /// Create a new directory
        /// </summary>
        /// <param name="DirectoryName">Directory name</param>
        /// <param name="Directory">Directory path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult CreateDirectory(String DirectoryName, String Directory)
        {
            return nfsInterface.CreateDirectory(DirectoryName, Directory);
        }

        /// <summary>
        /// Create a new directory in the current directory
        /// </summary>
        /// <param name="DirectoryName">Directory name</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult CreateDirectory(String DirectoryName)
        {
            return nfsInterface.CreateDirectory(DirectoryName);
        }

        /// <summary>
        /// Delete a directory
        /// </summary>
        /// <param name="DirectoryName">Directory Name</param>
        /// <param name="Directory">Directory path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult DeleteDirectory(String DirectoryName, String Directory)
        {
            return nfsInterface.DeleteDirectory(DirectoryName, Directory);
        }

        /// <summary>
        /// Delete a directory from the current directory
        /// </summary>
        /// <param name="DirectoryName">Directory name</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult DeleteDirectory(String DirectoryName)
        {
            return nfsInterface.DeleteDirectory(DirectoryName);
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="FileName">File name</param>
        /// <param name="Directory">Directory path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult DeleteFile(String FileName, String Directory)
        {
            return nfsInterface.DeleteFile(FileName, Directory);
        }

        /// <summary>
        /// Delete a file from the current directory
        /// </summary>
        /// <param name="FileName">File name</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult DeleteFile(String FileName)
        {
            return nfsInterface.DeleteFile(FileName);
        }

        /// <summary>
        /// Create a new file
        /// </summary>
        /// <param name="FileName">File name</param>
        /// <param name="Directory">Directory path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult CreateFile(String FileName, String Directory)
        {
            return nfsInterface.CreateFile(FileName, Directory);
        }

        /// <summary>
        /// Create a new file in the current directory
        /// </summary>
        /// <param name="FileName">File name</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult CreateFile(String FileName)
        {
            return nfsInterface.CreateFile(FileName);
        }

        /// <summary>
        /// Copy a set of files from a remote directory to a local directory
        /// </summary>
        /// <param name="SourceFileNames">A list of the remote files name</param>
        /// <param name="SourceFolderPath">The remote directory path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <param name="DestinationFolderPath">The destination local directory</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Read(List<String> SourceFileNames, String SourceFolderPath, String DestinationFolderPath)
        {
            NFSResult result = NFSResult.NFS_ERROR;
            if (Directory.Exists(DestinationFolderPath))
            {
                foreach (String FileName in SourceFileNames)
                {
                    if ((result = Read(FileName, SourceFolderPath, Path.Combine(DestinationFolderPath, FileName))) != NFSResult.NFS_SUCCESS) 
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// Copy a file from a remote directory to a local directory
        /// </summary>
        /// <param name="SourceFileName">The remote file name</param>
        /// <param name="SourceFolderPath">The remote directory path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <param name="DestinationFullFilePath">The destination local directory</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Read(String SourceFileName, String SourceFolderPath, String DestinationFullFilePath)
        {
            FileStream fs = null;
            try
            {
                NFSResult Result = NFSResult.NFS_ERROR;
                if (File.Exists(DestinationFullFilePath))
                    File.Delete(DestinationFullFilePath);
                fs = new FileStream(DestinationFullFilePath, FileMode.CreateNew);
                Result = Read(SourceFileName, SourceFolderPath, fs);
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

        /// <summary>
        /// Copy a file from a remote directory to a stream
        /// </summary>
        /// <param name="SourceFileName">The remote file name</param>
        /// <param name="SourceFolderPath">Directory path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <param name="OutputStream"></param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Read(String SourceFileName, String SourceFolderPath, Stream OutputStream)
        {
            NFSResult Result = NFSResult.NFS_ERROR;
            if (OutputStream != null)
            {
                if (!FileExists(SourceFileName, SourceFolderPath))
                    throw new FileNotFoundException();
                NFSAttributes nfsAttributes = GetItemAttributes(SourceFileName, SourceFolderPath);
                if (NFSResult.NFS_SUCCESS == Open(Combine(SourceFileName, SourceFolderPath)))
                {
                    UInt64 TotalLenght = nfsAttributes.size;
                    UInt32 BlockSize = blockSize;
                    UInt32 CuttentPosition = 0;
                    do
                    {
                        UInt32 Count = BlockSize;
                        if ((TotalLenght - CuttentPosition) < BlockSize)
                            Count = (UInt32)TotalLenght - CuttentPosition;

                        Byte[] Data = null;
                        int pSize = -1;
                        if ((pSize = Read(Combine(SourceFileName, SourceFolderPath), CuttentPosition, Count, ref Data)) != -1)
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

        /// <summary>
        /// Copy a remote file to a buffer
        /// </summary>
        /// <param name="FullSourceFilePath">The remote file full path</param>
        /// <param name="Offset">Start offset</param>
        /// <param name="Count">Number of bytes</param>
        /// <param name="Buffer">Output buffer</param>
        /// <returns>The number of copied bytes</returns>
        public Int32 Read(String FullSourceFilePath, UInt64 Offset, UInt32 Count, ref Byte[] Buffer)
        {
            UInt64 TotalLenght = Count;
            UInt32 BlockSize = blockSize;
            UInt32 CurrentPosition = 0;
            do
            {
                UInt32 ChunkCount = BlockSize;
                if ((TotalLenght - CurrentPosition) < BlockSize)
                    ChunkCount = (UInt32)TotalLenght - CurrentPosition;

                Int32 Size = -1;
                IntPtr pBuffer = Marshal.AllocHGlobal((Int32)ChunkCount);
                NFSResult Result = nfsInterface.Read(FullSourceFilePath, Offset + CurrentPosition, ChunkCount, pBuffer, out Size);
                if (Result == NFSResult.NFS_ERROR)
                    return (Size = -1);
                else
                {
                    if (Size == 0)
                        return (int)CurrentPosition;

                    Byte[] ChunkBuffer = new Byte[Size];
                    Marshal.Copy(pBuffer, ChunkBuffer, 0, Size);
                    Array.Copy(ChunkBuffer, 0, Buffer, CurrentPosition, Size);
                    CurrentPosition += (UInt32)Size;
                }
                Marshal.FreeHGlobal(pBuffer);
            } while (CurrentPosition != TotalLenght);
            return (int)TotalLenght;
        }

        /// <summary>
        /// Copy a local file to a remote directory
        /// </summary>
        /// <param name="DestinationFileName">The destination file name</param>
        /// <param name="DestinationFolderPath">The destination directory path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <param name="SourceFileFullPath">The local full file path</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Write(String DestinationFileName, String DestinationFolderPath, String SourceFileFullPath)
        {
            NFSResult Result = NFSResult.NFS_ERROR;
            if (File.Exists(SourceFileFullPath))
            {
                FileStream wfs = new FileStream(SourceFileFullPath, FileMode.Open, FileAccess.Read);
                Result = Write(DestinationFileName, DestinationFolderPath, wfs);
                wfs.Close();
            }
            return Result;
        }

        /// <summary>
        /// Copy a local file to a remote file
        /// </summary>
        /// <param name="DestinationFileName">The destination file name</param>
        /// <param name="DestinationFolderPath">The destination directory path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <param name="InputStream">The input file stream</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Write(String DestinationFileName, String DestinationFolderPath, Stream InputStream)
        {
            return Write(DestinationFileName, DestinationFolderPath, 0, InputStream);
        }

        /// <summary>
        /// Copy a local file stream to a remote file
        /// </summary>
        /// <param name="DestinationFileName">The destination file name</param>
        /// <param name="DestinationFolderPath">The destination directory path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <param name="InputOffset">The input offset in bytes</param>
        /// <param name="InputStream">The input stream</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Write(String DestinationFileName, String DestinationFolderPath, long InputOffset, Stream InputStream)
        {
            NFSResult Result = NFSResult.NFS_ERROR;
            if (InputStream != null)
            {
                if (!FileExists(DestinationFileName, DestinationFolderPath))
                {
                    if (CreateFile(DestinationFileName, DestinationFolderPath) != NFSResult.NFS_SUCCESS)
                        return NFSResult.NFS_ERROR;
                }
                
                if (NFSResult.NFS_SUCCESS == Open(Combine(DestinationFileName, DestinationFolderPath)))
                {
                    UInt64 Offset = (UInt64) InputOffset;
                    UInt32 Count = blockSize;
                    Int32 Bytes = 0;
                    Byte[] Buffer = new Byte[Count];
                    while ((Bytes = InputStream.Read(Buffer, 0, (Int32)Count)) > 0)
                    {
                        Int32 Res = Write(Combine(DestinationFileName, DestinationFolderPath), Offset, (UInt32)Bytes, Buffer);
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

        /// <summary>
        /// Copy a local file  to a remote directory
        /// </summary>
        /// <param name="FullFilePath">The full local file path</param>
        /// <param name="Offset">The start offset in bytes</param>
        /// <param name="Count">The number of bytes</param>
        /// <param name="Buffer">The input buffer</param>
        /// <returns>Returns the total written bytes</returns>
        public Int32 Write(String FullFilePath, UInt64 Offset, UInt32 Count, Byte[] Buffer)
        {
            UInt64 TotalLenght = Count;
            UInt32 BlockSize = blockSize;
            UInt32 CurrentPosition = 0;
            if (Buffer != null)
            {
                do
                {
                    Int32 Size = -1;
                    UInt32 ChunkCount = BlockSize;
                    if ((TotalLenght - CurrentPosition) < BlockSize)
                        ChunkCount = (UInt32)TotalLenght - CurrentPosition;

                    IntPtr pBuffer = Marshal.AllocHGlobal((Int32)ChunkCount);
                    Marshal.Copy(Buffer, (int) CurrentPosition, pBuffer, (Int32)ChunkCount);
                    NFSResult Result = nfsInterface.Write(FullFilePath, Offset + CurrentPosition, ChunkCount, pBuffer, out Size);
                    Marshal.FreeHGlobal(pBuffer);
                    if (Result == NFSResult.NFS_ERROR)
                        return (Size = -1);
                    else
                    {
                        if (Size == 0)
                            return (int)CurrentPosition;
                        CurrentPosition += (UInt32)ChunkCount;
                    }
                } while (CurrentPosition != TotalLenght);
            }
            return (int)TotalLenght; ;
        }

        /// <summary>
        /// Open a remote file
        /// </summary>
        /// <param name="FullFilePath">The remote file full path (e.g "dir/test/test.txt")</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Open(String FullFilePath)
        {
            return nfsInterface.Open(FullFilePath);
        }

        /// <summary>
        /// Close the current file
        /// </summary>
        public void CloseFile()
        {
            nfsInterface.CloseFile();
        }

        /// <summary>
        /// Rename a file in the current directory
        /// </summary>
        /// <param name="OldName">Old file name</param>
        /// <param name="NewName">New file name</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Rename(String OldName, String NewName)
        {
            return nfsInterface.Rename(OldName, NewName);
        }

        /// <summary>
        /// Move a file from/to a directory
        /// </summary>
        /// <param name="OldDirectoryName">The old directory name (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <param name="OldFileName">The old filename</param>
        /// <param name="NewDirectoryName">The new directory name (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <param name="NewFileName">The new file name</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult Move(String OldDirectoryName, String OldFileName, String NewDirectoryName, String NewFileName)
        {
            return nfsInterface.Move(OldDirectoryName, OldFileName, NewDirectoryName, NewFileName);
        }

        /// <summary>
        /// Check if the passed path refers to a directory
        /// </summary>
        /// <param name="Path">The full path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>NFS_SUCCESS if is a directory</returns>
        public NFSResult IsDirectory(String Path)
        {
            return nfsInterface.IsDirectory(Path);
        }

        /// <summary>
        /// Check if a file/directory exists
        /// </summary>
        /// <param name="FileName">The file/directory name</param>
        /// <param name="Directory">The parent directory name (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>True exists</returns>
        public Boolean FileExists(String FileName, String Directory)
        {
            return (GetItemAttributes(FileName, Directory) != null);
        }

        /// <summary>
        /// Check if a file/directory exists in the current directory
        /// </summary>
        /// <param name="FileName">The file/directory name</param>
        /// <returns>True exists</returns>
        public Boolean FileExists(String FileName)
        {
            return (GetItemAttributes(FileName) != null);
        }

        /// <summary>
        /// Get the last happened error
        /// </summary>
        /// <returns>The error string</returns>
        public String GetLastError()
        {
            return nfsInterface.GetLastNfsError();
        }

        /// <summary>
        /// Get the file/directory name from a standard windwos path (eg. "\\test\text.txt" --> "text.txt" or "\\" --> ".")
        /// </summary>
        /// <param name="path">The source path</param>
        /// <returns>The file/direcotry name</returns>
        public string GetFileName(String path)
        {
            String str = Path.GetFileName(path);
            if(str == string.Empty)
                str = ".";
            return str;
        }

        /// <summary>
        /// Get the directory name from a standard windwos path (eg. "\\test\test1\text.txt" --> "test/test1" or "\\" --> ".")
        /// </summary>
        /// <param name="path">The full path(e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>The directory name</returns>
        public string GetDirectoryName(String path)
        {
            String str = Path.GetDirectoryName(path);
            if (str == null)
                return ".";
            str = str.Replace("\\", "/");
            if (str == "/")
                str = ".";
            if (str.Length > 1)
                str = str.Remove(0, 1);
            return str;
        }

        /// <summary>
        /// Combine a file name to a directory (eg. FileName "test.txt", Directory "test" --> "test/test.txt" or FileName "test.txt", Directory "." --> "test.txt")
        /// </summary>
        /// <param name="FileName">The file name</param>
        /// <param name="DirectoryName">The directory name (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>The combined path</returns>
        public string Combine(String FileName, String DirectoryName)
        {
            if (DirectoryName == ".")
                return FileName;
            return DirectoryName + "/" + FileName;
        }

        /// <summary>
        /// Set the file size
        /// </summary>
        /// <param name="FileName">The file name</param>
        /// <param name="DirectoryName">The directory path (e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <param name="Size">the size in bytes</param>
        /// <returns>NFS_ERROR in case of error</returns>
        public NFSResult SetFileSize(String FileName, String DirectoryName, UInt64 Size)
        {
            return nfsInterface.SetFileSize(FileName, DirectoryName, Size);
        }

        #endregion
    }
}
