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
            using (NFSv2 nfsv2 = new NFSv2("192.168.56.102"))
            {
                if (nfsv2.Connect() == NFSResult.NFS_SUCCESS)
                {
                    List<String> DevicesList = nfsv2.GetExportedDevices();
                    if (DevicesList.Count > 0)
                    {
                        nfsv2.MountDevice(DevicesList[0]);
                        List<String> ItemsList = nfsv2.GetItemList();
                        foreach (String Item in ItemsList)
                        {
                            NFSAttributes nfsAttribues = nfsv2.GetItemAttributes(Item);
                            Console.WriteLine(Item + " " + nfsAttribues.ToString());
                        }
                        nfsv2.UnMountDevice();
                    }
                    nfsv2.Disconnect();
                }
            }
        }
    }
}
