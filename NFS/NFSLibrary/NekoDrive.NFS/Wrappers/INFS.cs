using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NekoDrive.NFS.Wrappers
{
    public interface INFS: IDisposable
    {
        event NFSDataEventHandler DataEvent;

        NFSResult Connect();

        NFSResult Connect(Int32 UserId, Int32 GroupId);

        NFSResult Disconnect();

        List<String> GetExportedDevices();

        NFSResult MountDevice(String DeviceName);

        NFSResult UnMountDevice();

        List<String> GetItemList();

        NFSAttributes GetItemAttributes(String ItemName);

        NFSResult ChangeCurrentDirectory(String DirectoryName);

        NFSResult CreateDirectory(String DirectoryName);

        NFSResult DeleteDirectory(String DirectoryName);

        NFSResult DeleteFile(String FileName);

        NFSResult CreateFile(String FileName);

        NFSResult Read(String FileName, String OutputFileName);

        NFSResult Read(String FileName, ref FileStream OutputStream);

        int Read(UInt64 Offset, UInt32 Count, ref Byte[] Buffer);

        NFSResult Write(String FileName, String InputFileName);

        NFSResult Write(String FileName, FileStream InputStream);

        int Write(UInt64 Offset, UInt32 Count, Byte[] Buffer);
    }
}
