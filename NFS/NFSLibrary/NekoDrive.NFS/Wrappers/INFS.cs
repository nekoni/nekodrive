using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace NekoDrive.NFS.Wrappers
{
    public interface INFS
    {
        void Create();

        void Destroy();

        NFSResult Connect(IPAddress Address);

        NFSResult Connect(IPAddress Address, Int32 UserId, Int32 GroupId, Int32 CommandTimeout);

        NFSResult Disconnect();

        IntPtr GetExportedDevices(out Int32 Size);

        NFSResult MountDevice(String DeviceName);

        NFSResult UnMountDevice();

        IntPtr GetItemList(out Int32 Size);

        IntPtr GetItemAttributes(String ItemName);

        NFSResult ChangeCurrentDirectory(String DirectoryName);

        NFSResult CreateDirectory(String DirectoryName);

        NFSResult DeleteDirectory(String DirectoryName);

        NFSResult DeleteFile(String FileName);

        NFSResult CreateFile(String FileName);

        NFSResult Read(UInt64 Offset, UInt32 Count, IntPtr pBuffer, out Int32 Size);

        NFSResult Write(UInt64 Offset, UInt32 Count, IntPtr pBuffer, out Int32 Size);

        NFSResult Open(String FileName);

        void CloseFile();

        NFSResult Rename(String OldName, String NewName);

        NFSAttributes GetNfsAttribute(IntPtr pAttributes);

        void ReleaseBuffer(IntPtr pBuffer);

        void ReleaseBuffers(IntPtr pBuffers);

        String GetLastNfsError();
    }
}
