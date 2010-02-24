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
        public NFSAttributes(UInt32 dateTime, UInt32 type, UInt64 size, byte[] handle)
        {
            this.dateTime = new System.DateTime(1970, 1, 1).AddSeconds(dateTime);
            this.type = (NFSType)type;
            this.size = size;
            this.handle = (byte[])handle.Clone();
        }

        public DateTime dateTime;
        public NFSType type;
        public UInt64 size;
        public byte[] handle;

        public override string ToString()
        {
            string Handle = string.Empty;
            foreach (byte b in handle)
                Handle += b.ToString("X");

            return "DateTime: " + dateTime.ToString() + " " +
                "Type: " + type.ToString() + " " +
                "Size: " + size + " " +
                "Handle: " + Handle;
        }
    }

    public delegate void NFSDataEventHandler(object sender, NFSEventArgs e);

    public class NFSEventArgs : EventArgs
    {
        public UInt32 Bytes;
    }
}
