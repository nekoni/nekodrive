using System;
using System.Collections.Generic;
using System.Text;

namespace NekoDrive.NFS.Wrappers
{
    public enum NFSResult
    {
        NFS_SUCCESS = 0,
        NFS_ERROR = -1
    }

    public enum NFSType
    {
        NFNON = 0,
        NFREG = 1,
        NFDIR = 2,
        NFBLK = 3,
        NFCHR = 4,
        NFLNK = 5
    }

    public class NFSAttributes
    {
        public NFSAttributes(UInt32 cdateTime, UInt32 adateTime, UInt32 mdateTime, UInt32 type, UInt64 size, byte[] handle)
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
        public UInt64 size;
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
