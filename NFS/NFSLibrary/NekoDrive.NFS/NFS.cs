using System;
using System.Collections.Generic;
using System.Text;
using NekoDrive.NFS.Wrappers;
using System.Net;

namespace NekoDrive.NFS
{
    public static class NFS
    {
        public enum NFSVersion
        {
            v2 = 2,
            v3 = 3
        }

        public static INFS GetNFS(IPAddress Address, NFSVersion Ver)
        {
            if(Ver == NFSVersion.v2)
                return new NFSv2(Address);
            else
                return new NFSv3(Address);
        }
    }
}
