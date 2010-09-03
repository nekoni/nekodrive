using System;
using System.Collections.Generic;
using System.Text;
using NFSLibrary.Protocols;
using NFSLibrary.Protocols.V2;
using NFSLibrary.Protocols.V3;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;

namespace NFSLibrary
{
    public enum NFSType
    {
        NFNON = 0,
        NFREG = 1,
        NFDIR = 2,
        NFBLK = 3,
        NFCHR = 4,
        NFLNK = 5
    }

    /// <summary>
    /// NFS Client Library
    /// </summary>
    public class NFSClient
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
        public NFSClient(NFSVersion Version)
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
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a connection to a NFS Server
        /// </summary>
        /// <param name="Address">The server address</param>
        public void Connect(IPAddress Address)
        {
            nfsInterface.Connect(Address, 0, 0, 60);
        }

        /// <summary>
        /// Create a connection to a NFS Server
        /// </summary>
        /// <param name="Address">The server address</param>
        /// <param name="UserId">The unix user id</param>
        /// <param name="GroupId">The unix group id</param>
        /// <param name="CommandTimeout">The command timeout in seconds</param>
        public void Connect(IPAddress Address, Int32 UserId, Int32 GroupId, Int32 CommandTimeout)
        {
            nfsInterface.Connect(Address, UserId, GroupId, CommandTimeout);
            IsConnected = true;
        }

        /// <summary>
        /// Close the current connection
        /// </summary>
        public void Disconnect()
        {
            nfsInterface.Disconnect();
            IsConnected = false;
        }

        /// <summary>
        /// Get the list of the exported NFS devices
        /// </summary>
        /// <returns>A list of the exported NFS devices</returns>
        public List<String> GetExportedDevices()
        {
            return nfsInterface.GetExportedDevices();
        }

        /// <summary>
        /// Mount device
        /// </summary>
        /// <param name="DeviceName">The device name</param>
        public void MountDevice(String DeviceName)
        {
            nfsInterface.MountDevice(DeviceName);
            IsMounted = true;
        }

        /// <summary>
        /// Unmount the current device
        /// </summary>
        public void UnMountDevice()
        {
            nfsInterface.UnMountDevice();
            IsMounted = false;
        }

        /// <summary>
        /// Get the items in a directory
        /// </summary>
        /// <param name="DirectoryFullName">Directory name (e.g. "directory\subdirectory" or "." for the root)</param>
        /// <returns>A list of the items name</returns>
        public List<String> GetItemList(String DirectoryFullName)
        {
            return nfsInterface.GetItemList(DirectoryFullName);
        }

        /// <summary>
        /// Get an item attribures
        /// </summary>
        /// <param name="ItemFullName">The item full path name</param>
        /// <returns>A NFSAttributes class</returns>
        public NFSAttributes GetItemAttributes(String ItemFullName)
        {
            return nfsInterface.GetItemAttributes(ItemFullName);
        }


        /// <summary>
        /// Create a new directory
        /// </summary>
        /// <param name="DirectoryFullName">Directory full name</param>
        public void CreateDirectory(String DirectoryFullName)
        {
            nfsInterface.CreateDirectory(DirectoryFullName);
        }

        
        /// <summary>
        /// Delete a directory
        /// </summary>
        /// <param name="DirectoryFullName">Directory full name</param>
        public void DeleteDirectory(String DirectoryFullName)
        {
            nfsInterface.DeleteDirectory(DirectoryFullName);
        }


        /// <summary>
        /// Delete a file 
        /// </summary>
        /// <param name="FileFullName">File full name</param>
        public void DeleteFile(String FileFullName)
        {
            nfsInterface.DeleteFile(FileFullName);
        }

        /// <summary>
        /// Create a new file
        /// </summary>
        /// <param name="FileFullName">File full name</param>
        public void CreateFile(String FileFullName)
        {
            nfsInterface.CreateFile(FileFullName);
        }

        /// <summary>
        /// Copy a set of files from a remote directory to a local directory
        /// </summary>
        /// <param name="SourceFileNames">A list of the remote files name</param>
        /// <param name="SourceDirectoryFullName">The remote directory path (e.g. "directory\sub1\sub2" or "." for the root)</param>
        /// <param name="DestinationDirectoryFullName">The destination local directory</param>
        public void Read(List<String> SourceFileNames, String SourceDirectoryFullName, String DestinationDirectoryFullName)
        {
            if (Directory.Exists(DestinationDirectoryFullName))
            {
                foreach (String FileName in SourceFileNames)
                {
                    Read(Combine(FileName, SourceDirectoryFullName), Path.Combine(DestinationDirectoryFullName, FileName));
                }
            }
        }

        /// <summary>
        /// Copy a file from a remote directory to a local directory
        /// </summary>
        /// <param name="SourceFilefullName">The remote file name</param>
        /// <param name="DestinationFileFullName">The destination local directory</param>
        public void Read(String SourceFilefullName, String DestinationFileFullName)
        {
            FileStream fs = null;
            try
            {
                if (File.Exists(DestinationFileFullName))
                    File.Delete(DestinationFileFullName);
                fs = new FileStream(DestinationFileFullName, FileMode.CreateNew);
                Read(SourceFilefullName, fs);
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
        /// <param name="SourceFileFullName">The remote file name</param>
        /// <param name="OutputStream"></param>
        public void Read(String SourceFileFullName, Stream OutputStream)
        {
            if (OutputStream != null)
            {
                if (!FileExists(SourceFileFullName))
                    throw new FileNotFoundException();
                NFSAttributes nfsAttributes = GetItemAttributes(SourceFileFullName);
                
                Int64 TotalLenght = nfsAttributes.size;
                Byte[] Data = new byte[TotalLenght];
                int pSize = -1;

                pSize = Read(SourceFileFullName, 0, TotalLenght, ref Data);
                OutputStream.Write(Data, 0, pSize);
                OutputStream.Flush();
            }
        }

        /// <summary>
        /// Copy a remote file to a buffer
        /// </summary>
        /// <param name="SourceFileFullName">The remote file full path</param>
        /// <param name="Offset">Start offset</param>
        /// <param name="Count">Number of bytes</param>
        /// <param name="Buffer">Output buffer</param>
        /// <returns>The number of copied bytes</returns>
        public Int32 Read(String SourceFileFullName, Int64 Offset, Int64 TotalLenght, ref Byte[] Buffer)
        {
            UInt32 BlockSize = blockSize;
            UInt32 CurrentPosition = 0;
            do
            {
                UInt32 ChunkCount = BlockSize;
                if ((TotalLenght - CurrentPosition) < BlockSize)
                    ChunkCount = (UInt32)TotalLenght - CurrentPosition;

                Byte[] ChunkBuffer = new Byte[ChunkCount];
                int Size = 0;
                nfsInterface.Read(SourceFileFullName, Offset + CurrentPosition, ChunkCount, ref ChunkBuffer, out Size);
                
                if (DataEvent != null)
                    DataEvent(this, new NFSEventArgs(ChunkCount));

                if (Size == 0)
                    return (int)CurrentPosition;

                Array.Copy(ChunkBuffer, 0, Buffer, CurrentPosition, Size);
                CurrentPosition += (UInt32)Size;

            } while (CurrentPosition != TotalLenght);
            return (int)TotalLenght;
        }

        /// <summary>
        /// Copy a local file to a remote directory
        /// </summary>
        /// <param name="DestinationFileFullName">The destination file full name</param>
        /// <param name="SourceFileFullName">The local full file path</param>
        public void Write(String DestinationFileFullName, String SourceFileFullName)
        {
            if (File.Exists(SourceFileFullName))
            {
                FileStream wfs = new FileStream(SourceFileFullName, FileMode.Open, FileAccess.Read);
                Write(DestinationFileFullName, wfs);
                wfs.Close();
            }
        }

        /// <summary>
        /// Copy a local file to a remote file
        /// </summary>
        /// <param name="DestinationFileFullName">The destination full file name</param>
        /// <param name="InputStream">The input file stream</param>
        public void Write(String DestinationFileFullName, Stream InputStream)
        {
            Write(DestinationFileFullName, 0, InputStream);
        }

        /// <summary>
        /// Copy a local file stream to a remote file
        /// </summary>
        /// <param name="DestinationFileFullName">The destination full file name</param>
        /// <param name="InputOffset">The input offset in bytes</param>
        /// <param name="InputStream">The input stream</param>
        public void Write(String DestinationFileFullName, long InputOffset, Stream InputStream)
        {
            if (InputStream != null)
            {
                if (!FileExists(DestinationFileFullName))
                    CreateFile(DestinationFileFullName);
                
                Int64 Offset = (Int64)InputOffset;
                UInt32 Count = blockSize;
                Int32 Bytes = 0;
                Byte[] Buffer = new Byte[Count];
                while ((Bytes = InputStream.Read(Buffer, 0, (Int32)Count)) > 0)
                {
                    Int32 Res = Write(DestinationFileFullName, Offset, (UInt32)Bytes, Buffer);
                    Offset += (UInt32)Bytes;
                }
            }
        }

        /// <summary>
        /// Copy a local file  to a remote directory
        /// </summary>
        /// <param name="DestinationFileFullName">The full local file path</param>
        /// <param name="Offset">The start offset in bytes</param>
        /// <param name="Count">The number of bytes</param>
        /// <param name="Buffer">The input buffer</param>
        /// <returns>Returns the total written bytes</returns>
        public Int32 Write(String DestinationFileFullName, Int64 Offset, UInt32 Count, Byte[] Buffer)
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

                    Byte[] ChunkBuffer = new Byte[ChunkCount];
                    Array.Copy(Buffer, (int)CurrentPosition, ChunkBuffer, 0, (Int32)ChunkCount);
                    nfsInterface.Write(DestinationFileFullName, Offset + CurrentPosition, ChunkCount, ChunkBuffer, out Size);
                    if (DataEvent != null)
                        DataEvent(this, new NFSEventArgs(ChunkCount));
                    if (Size == 0)
                        return (int)CurrentPosition;
                    CurrentPosition += (UInt32)ChunkCount;
                   
                } while (CurrentPosition != TotalLenght);
            }
            return (int)TotalLenght; ;
        }


        /// <summary>
        /// Move a file from/to a directory
        /// </summary>
        /// <param name="OldDirectoryName">The old directory name (e.g. "directory\sub1\sub2" or "." for the root)</param>
        /// <param name="OldFileName">The old filename</param>
        /// <param name="NewDirectoryName">The new directory name (e.g. "directory\sub1\sub2" or "." for the root)</param>
        /// <param name="NewFileName">The new file name</param>
        public void Move(String OldDirectoryFullName, String OldFileName, String NewDirectoryFullName, String NewFileName)
        {
            nfsInterface.Move(OldDirectoryFullName, OldFileName, NewDirectoryFullName, NewFileName);
        }

        /// <summary>
        /// Check if the passed path refers to a directory
        /// </summary>
        /// <param name="DirectoryFullName">The full path (e.g. "directory\sub1\sub2" or "." for the root)</param>
        /// <returns>True if is a directory</returns>
        public bool IsDirectory(String DirectoryFullName)
        {
            return nfsInterface.IsDirectory(DirectoryFullName);
        }

        /// <summary>
        /// Check if a file/directory exists
        /// </summary>
        /// <param name="FileFullName">The item full name</param>
        /// <returns>True exists</returns>
        public Boolean FileExists(String FileFullName)
        {
            return GetItemAttributes(FileFullName) != null;
        }


        /// <summary>
        /// Get the file/directory name from a standard windwos path (eg. "\\test\text.txt" --> "text.txt" or "\\" --> ".")
        /// </summary>
        /// <param name="FullFilePath">The source path</param>
        /// <returns>The file/direcotry name</returns>
        public string GetFileName(String FileFullName)
        {
            String str = Path.GetFileName(FileFullName);
            if (str == string.Empty)
                str = ".";
            return str;
        }

        /// <summary>
        /// Get the directory name from a standard windwos path (eg. "\\test\test1\text.txt" --> "test\\test1" or "\\" --> ".")
        /// </summary>
        /// <param name="FullDirectoryName">The full path(e.g. "directory/sub1/sub2" or "." for the root)</param>
        /// <returns>The directory name</returns>
        public string GetDirectoryName(String FullDirectoryName)
        {
            String str = Path.GetDirectoryName(FullDirectoryName);
            if (str == null)
                return ".";
            if (str == @"\")
                str = ".";
            if (str.Length > 1)
                str = str.Remove(0, 1);
            return str;
        }

        /// <summary>
        /// Combine a file name to a directory (eg. FileName "test.txt", Directory "test" --> "test\test.txt" or FileName "test.txt", Directory "." --> "test.txt")
        /// </summary>
        /// <param name="FileName">The file name</param>
        /// <param name="DirectoryName">The directory name (e.g. "directory\sub1\sub2" or "." for the root)</param>
        /// <returns>The combined path</returns>
        public string Combine(String FileName, String DirectoryFullName)
        {
            if (DirectoryFullName == ".")
                return FileName;
            return DirectoryFullName + @"\" + FileName;
        }

        /// <summary>
        /// Set the file size
        /// </summary>
        /// <param name="FileFullName">The file full path</param>
        /// <param name="Size">the size in bytes</param>
        public void SetFileSize(String FileFullName, UInt64 Size)
        {
            nfsInterface.SetFileSize(FileFullName, Size);
        }

        #endregion
    }

    public class NFSAttributes
    {
        public NFSAttributes(Int32 cdateTime, Int32 adateTime, Int32 mdateTime, Int32 type, Int64 size, byte[] handle)
        {
            this.cdateTime = new System.DateTime(1970, 1, 1).AddSeconds(cdateTime);
            this.adateTime = new System.DateTime(1970, 1, 1).AddSeconds(adateTime);
            this.mdateTime = new System.DateTime(1970, 1, 1).AddSeconds(mdateTime);
            this.type = (NFSType)type;
            this.size = size;
            this.handle = (byte[])handle.Clone();
        }

        public DateTime cdateTime;
        public DateTime adateTime;
        public DateTime mdateTime;
        public NFSType type;
        public Int64 size;
        public byte[] handle;

        public override string ToString()
        {
            string Handle = string.Empty;
            foreach (byte b in handle)
                Handle += b.ToString("X");

            return "CDateTime: " + cdateTime.ToString() + " " +
                "ADateTime: " + adateTime.ToString() + " " +
                "MDateTime: " + mdateTime.ToString() + " " +
                "Type: " + type.ToString() + " " +
                "Size: " + size + " " +
                "Handle: " + Handle;
        }
    }

    public delegate void NFSDataEventHandler(object sender, NFSEventArgs e);

    public class NFSEventArgs : EventArgs
    {
        public NFSEventArgs(UInt32 Bytes)
        {
            this.Bytes = Bytes;
        }

        public UInt32 Bytes;
    }
}
