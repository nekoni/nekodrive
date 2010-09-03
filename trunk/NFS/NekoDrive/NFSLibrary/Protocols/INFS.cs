using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace NFSLibrary.Protocols
{
    public interface INFS
    {
        void Connect(IPAddress Address);

        void Connect(IPAddress Address, Int32 UserId, Int32 GroupId, Int32 CommandTimeout);

        void Disconnect();

        List<String> GetExportedDevices();

        void MountDevice(String DeviceName);

        void UnMountDevice();

        List<String> GetItemList(String DirectoryFullName);

        NFSAttributes GetItemAttributes(String ItemFullName);

        void CreateDirectory(String DirectoryFullName);

        void DeleteDirectory(String DirectoryFullName);

        void DeleteFile(String FileFullName);

        void CreateFile(String FileFullName);

        void Read(String FileFullName, Int64 Offset, UInt32 Count, ref Byte[] Buffer, out Int32 Size);

        void SetFileSize(String FileFullName, UInt64 Size);

        void Write(String FileFullName, Int64 Offset, UInt32 Count, Byte[] Buffer, out Int32 Size);

        void Move(String OldDirectoryFullName, String OldFileName, String NewDirectoryFullName, String NewFileName);

        bool IsDirectory(String DirectoryFullName);
    }

}
