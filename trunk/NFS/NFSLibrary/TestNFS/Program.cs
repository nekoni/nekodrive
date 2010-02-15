using System;
using System.Collections.Generic;
using System.Text;
using NekoDrive.NFS.Wrappers;
using System.Net;
using System.IO;
using System.Reflection;
using System.Threading;

namespace TestNFS
{
    class Program
    {
        static void Main(string[] args)
        {
            String IpAddress = "161.55.180.150";
            string OutFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            OutFolder = Path.Combine(OutFolder, "Test");
            if (Directory.Exists(OutFolder))
                Directory.Delete(OutFolder, true);
            Directory.CreateDirectory(OutFolder);
            //using (NFSv2 nfsv2 = new NFSv2(IpAddress))
            //{
            //    if (nfsv2.Connect(0, 0) == NFSResult.NFS_SUCCESS)
            //    {
            //        List<String> DevicesList = nfsv2.GetExportedDevices();
            //        if (DevicesList.Count > 0)
            //        {
            //            nfsv2.MountDevice(DevicesList[0]);
            //            foreach (string fi in Directory.GetFiles(OutFolder))
            //            {
            //                nfsv2.Write(Path.GetFileName(fi), fi);
            //            }
            //        }
            //        nfsv2.UnMountDevice();
            //        nfsv2.Disconnect();
            //    }
            //}
            for (int z = 0; z < 1000; z++)
            {
                using (NFSv2 nfsv2 = new NFSv2(IpAddress))
                {
                    nfsv2.DataEvent += new NFSDataEventHandler(nfsv2_DataEvent);
                    if (nfsv2.Connect(0, 0) == NFSResult.NFS_SUCCESS)
                    {
                        List<String> DevicesList = nfsv2.GetExportedDevices();
                        if (DevicesList.Count > 0)
                        {
                            nfsv2.MountDevice(DevicesList[0]);
                            List<String> ItemsList = nfsv2.GetItemList();
                            for (int x = 0; x < 100; x++)
                            {
                                Console.WriteLine("Sleep...");
                                foreach (String Item in ItemsList)
                                {
                                    NFSAttributes nfsAttribues = nfsv2.GetItemAttributes(Item);
                                    Console.WriteLine("");
                                    Console.WriteLine(Item);
                                    Console.WriteLine(nfsAttribues.ToString());
                                    if (nfsAttribues.type == NFSType.NFREG)
                                    {
                                        string FileName = Path.Combine(OutFolder, Item);
                                        if (File.Exists(FileName))
                                            File.Delete(FileName);

                                        if (nfsv2.Read(Item, FileName) != NFSResult.NFS_SUCCESS)
                                            Console.WriteLine("Read error");

                                        //if (nfsv2.Write(Item, FileName) != NFSResult.NFS_SUCCESS)
                                        //    Console.WriteLine("Write error");
                                    }
                                }
                            }
                            nfsv2.UnMountDevice();
                        }
                        nfsv2.Disconnect();
                    }
                    nfsv2.DataEvent -= new NFSDataEventHandler(nfsv2_DataEvent);
                }
            }
        }

        static void nfsv2_DataEvent(object sender, NFSEventArgs e)
        {
            Console.Write("#");
        }
    }
}
