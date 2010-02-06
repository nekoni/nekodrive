using System;
using System.Collections.Generic;
using System.Text;
using NekoDrive.NFS.Wrappers;
using System.Net;

namespace TestNFS
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress Address = IPAddress.Parse("192.168.56.102");
            using (NFSv2 nfsv2 = new NFSv2())
            {
                if (nfsv2.Connect((UInt32) Address.Address) == NFSResult.NFS_SUCCESS)
                {
                    List<String> DevicesList = nfsv2.GetExportedDevices();
                    if (DevicesList.Count > 0)
                    {
                        nfsv2.MountDevice(DevicesList[0]);
                        List<String> ItemsList = nfsv2.GetItemList();
                        foreach (String Item in ItemsList)
                        {
                            Console.WriteLine(Item);
                        }
                        nfsv2.UnMountDevice();
                    }
                    nfsv2.Disconnect();
                }
            }
        }
    }
}
