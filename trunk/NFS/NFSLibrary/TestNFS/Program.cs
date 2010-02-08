using System;
using System.Collections.Generic;
using System.Text;
using NekoDrive.NFS.Wrappers;
using System.Net;
using System.IO;
using System.Reflection;

namespace TestNFS
{
    class Program
    {
        static void Main(string[] args)
        {
            string OutFolder = "E:\\projects\\nekodrive\\trunk\\NFS\\Build\\x86\\Debug\\rd0";
            using (NFSv2 nfsv2 = new NFSv2("192.168.56.102"))
            {
                if (nfsv2.Connect() == NFSResult.NFS_SUCCESS)
                {
                    List<String> DevicesList = nfsv2.GetExportedDevices();
                    if (DevicesList.Count > 0)
                    {
                        nfsv2.MountDevice(DevicesList[0]);
                        List<String> ItemsList = nfsv2.GetItemList();
                        //foreach (String Item in ItemsList)
                        //{
                        //    NFSAttributes nfsAttribues = nfsv2.GetItemAttributes(Item);
                        //    //Console.WriteLine(Item + " " + nfsAttribues.ToString());
                        //    if (nfsAttribues.type == NFSType.NFREG)
                        //    {
                        //        string FileName = Path.Combine(OutFolder, Item);
                        //        if (File.Exists(FileName))
                        //            File.Delete(FileName);

                        //        FileStream fs = new FileStream(FileName, FileMode.CreateNew);
                        //        if (nfsv2.Read(Item, ref fs) != NFSResult.NFS_SUCCESS)
                        //            Console.WriteLine("Read error");
                        //        fs.Close();
                        //    }
                        //}
                        string FileName = Path.Combine(OutFolder, "new file");
                        if (File.Exists(FileName))
                            File.Delete(FileName);

                        FileStream fs = new FileStream(FileName, FileMode.CreateNew);
                        if (nfsv2.Read("new file", ref fs) != NFSResult.NFS_SUCCESS)
                            Console.WriteLine("Read error");
                        fs.Close();
                        string WriteFileName = Path.Combine(OutFolder, "prova.txt");
                        FileStream wfs = new FileStream(WriteFileName, FileMode.Open, FileAccess.Read);
                        if (nfsv2.Write(Path.GetFileName(WriteFileName), wfs) != NFSResult.NFS_SUCCESS)
                            Console.WriteLine("Write error");
                        wfs.Close();
                        nfsv2.UnMountDevice();
                    }
                    nfsv2.Disconnect();
                }
            }
        }
    }
}
