using System;
using System.Collections.Generic;
using System.Text;
using Dokan;
using NekoDrive.NFS.Wrappers;

namespace NekoDrive.NFS
{
    class Operations: DokanOperations
    {
        #region DokanOperations Members

        public int CreateFile(string filename, System.IO.FileAccess access, System.IO.FileShare share, System.IO.FileMode mode, System.IO.FileOptions options, DokanFileInfo info)
        {
            return (int) MainForm.Instance.mNFS.CreateFile(filename);
        }

        public int OpenDirectory(string filename, DokanFileInfo info)
        {
            return (int)MainForm.Instance.mNFS.ChangeCurrentDirectory(filename);
        }

        public int CreateDirectory(string filename, DokanFileInfo info)
        {
            return (int)MainForm.Instance.mNFS.CreateDirectory(filename);
        }

        public int Cleanup(string filename, DokanFileInfo info)
        {
            return 0; //???
        }

        public int CloseFile(string filename, DokanFileInfo info)
        {
            MainForm.Instance.mNFS.CloseFile();
            return 0;
        }

        public int ReadFile(string filename, byte[] buffer, ref uint readBytes, long offset, DokanFileInfo info)
        {
            int ret = -1;
            try
            {
                if (MainForm.Instance.mNFS.Open(filename) == NekoDrive.NFS.Wrappers.NFSResult.NFS_SUCCESS)
                {
                    if (buffer != null)
                    {
                        int mReadBytes = MainForm.Instance.mNFS.Read((ulong)offset, (uint) buffer.Length, ref buffer);
                        if (mReadBytes != -1)
                        {
                            readBytes = (uint) mReadBytes;
                            ret = 0;
                        }
                    }
                    MainForm.Instance.mNFS.CloseFile();
                }
            }
            catch
            {
            }
            return ret;
        }

        public int WriteFile(string filename, byte[] buffer, ref uint writtenBytes, long offset, DokanFileInfo info)
        {
            int ret = -1;
            try
            {
                if (MainForm.Instance.mNFS.Open(filename) == NekoDrive.NFS.Wrappers.NFSResult.NFS_SUCCESS)
                {
                    if (buffer != null)
                    {
                        int mWrittenBytes = MainForm.Instance.mNFS.Write((ulong)offset, (uint) buffer.Length, buffer);
                        if (mWrittenBytes != -1)
                        {
                            writtenBytes = (uint) mWrittenBytes;
                            ret = 0;
                        }
                    }
                    MainForm.Instance.mNFS.CloseFile();
                }
            }
            catch
            {
            }
            return ret;
        }

        public int FlushFileBuffers(string filename, DokanFileInfo info)
        {
            return 0;
        }

        public int GetFileInformation(string filename, FileInformation fileinfo, DokanFileInfo info)
        {
            NFSAttributes nfsAttributes =  MainForm.Instance.mNFS.GetItemAttributes(filename);
            if (nfsAttributes == null)
                return -1;

            if (nfsAttributes.type == NFSType.NFDIR)
                fileinfo.Attributes = System.IO.FileAttributes.Directory;
            else
                fileinfo.Attributes = System.IO.FileAttributes.Archive;

            fileinfo.LastAccessTime = nfsAttributes.dateTime;
            fileinfo.LastWriteTime = nfsAttributes.dateTime;
            fileinfo.CreationTime = nfsAttributes.dateTime;
            fileinfo.Length = (long) nfsAttributes.size;

            return 0;
        }

        public int FindFiles(string filename, System.Collections.ArrayList files, DokanFileInfo info)
        {
            throw new NotImplementedException();
        }

        public int SetFileAttributes(string filename, System.IO.FileAttributes attr, DokanFileInfo info)
        {
            return 0;
        }

        public int SetFileTime(string filename, DateTime ctime, DateTime atime, DateTime mtime, DokanFileInfo info)
        {
            return 0;
        }

        public int DeleteFile(string filename, DokanFileInfo info)
        {
            return (int) MainForm.Instance.mNFS.DeleteFile(filename);
        }

        public int DeleteDirectory(string filename, DokanFileInfo info)
        {
            return (int)MainForm.Instance.mNFS.DeleteDirectory(filename);
        }

        public int MoveFile(string filename, string newname, bool replace, DokanFileInfo info)
        {
            return (int)MainForm.Instance.mNFS.Rename(filename, newname);
        }

        public int SetEndOfFile(string filename, long length, DokanFileInfo info)
        {
            return 0;
        }

        public int SetAllocationSize(string filename, long length, DokanFileInfo info)
        {
            return 0;
        }

        public int LockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            return 0;
        }

        public int UnlockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            return 0;
        }

        public int GetDiskFreeSpace(ref ulong freeBytesAvailable, ref ulong totalBytes, ref ulong totalFreeBytes, DokanFileInfo info)
        {
            return 0;
        }

        public int Unmount(DokanFileInfo info)
        {
            return 0;
        }

        #endregion
    }
}
