using System;
using System.Collections.Generic;
using System.Text;
using Dokan;
using NekoDrive.NFS.Wrappers;
using System.Collections;
using System.IO;

namespace NekoDrive.NFS
{
    class Operations: DokanOperations
    {
        #region DokanOperations Members

        public int CreateFile(string filename, System.IO.FileAccess access, System.IO.FileShare share, System.IO.FileMode mode, System.IO.FileOptions options, DokanFileInfo info)
        {
            filename = filename.Replace("\\", "");
            if (filename == string.Empty)
                filename = ".";
            if (access == FileAccess.Read)
                return 0;
            return (int) MainForm.Instance.mNFS.CreateFile(filename);
        }

        public int OpenDirectory(string filename, DokanFileInfo info)
        {
            return 0;
            filename = filename.Replace("\\", "");
            if (filename == string.Empty)
                filename = ".";
            return (int)MainForm.Instance.mNFS.ChangeCurrentDirectory(filename);
        }

        public int CreateDirectory(string filename, DokanFileInfo info)
        {
            filename = filename.Replace("\\", "");
            if (filename == string.Empty)
                filename = ".";
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
            filename = filename.Replace("\\", "");
            if (filename == string.Empty)
                filename = ".";
            int ret = -1;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    if (MainForm.Instance.mNFS.Read(filename, ms) == NekoDrive.NFS.Wrappers.NFSResult.NFS_SUCCESS)
                    {
                        if (buffer != null)
                        {
                            byte[] iBuffer = ms.GetBuffer();
                            int len = buffer.Length;
                            if (iBuffer.Length < len)
                                len = iBuffer.Length;
                            Array.Copy(iBuffer, offset, buffer, 0, len);
                            readBytes = (uint)len;
                            ret = 0;
                        }
                    }
                }
            }
            catch
            {
            }
            return ret;
        }

        public int WriteFile(string filename, byte[] buffer, ref uint writtenBytes, long offset, DokanFileInfo info)
        {
            filename = filename.Replace("\\", "");
            if (filename == string.Empty)
                filename = ".";
            int ret = -1;
            try
            {

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    ret = (int)MainForm.Instance.mNFS.Write(filename, offset, ms);
                    writtenBytes = (uint)ms.Length;
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
            filename = filename.Replace("\\", "");
            if (filename == string.Empty)
                filename = ".";
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
            filename = filename.Replace("\\", "");
            if (filename == string.Empty)
                filename = ".";
            if (MainForm.Instance.mNFS.ChangeCurrentDirectory(filename) == NFSResult.NFS_SUCCESS)
            {
                foreach (string strItem in MainForm.Instance.mNFS.GetItemList())
                {
                    NFSAttributes nfsAttributes = MainForm.Instance.mNFS.GetItemAttributes(strItem);
                    if (nfsAttributes != null)
                    {
                        FileInformation fi = new FileInformation();
                        fi.Attributes = nfsAttributes.type == NFSType.NFDIR ? FileAttributes.Directory : FileAttributes.Normal;
                        fi.CreationTime = nfsAttributes.dateTime;
                        fi.LastAccessTime = nfsAttributes.dateTime;
                        fi.LastWriteTime = nfsAttributes.dateTime;
                        fi.Length = (long) nfsAttributes.size;
                        fi.FileName = strItem;
                        files.Add(fi);
                    }
                }
                return 0;
            }
            return -1;
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
            filename = filename.Replace("\\", "");
            if (filename == string.Empty)
                filename = ".";
            return (int) MainForm.Instance.mNFS.DeleteFile(filename);
        }

        public int DeleteDirectory(string filename, DokanFileInfo info)
        {
            filename = filename.Replace("\\", "");
            if (filename == string.Empty)
                filename = ".";
            return (int)MainForm.Instance.mNFS.DeleteDirectory(filename);
        }

        public int MoveFile(string filename, string newname, bool replace, DokanFileInfo info)
        {
            filename = filename.Replace("\\", "");
            if (filename == string.Empty)
                filename = ".";
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
            freeBytesAvailable = 1024ul * 1024 * 1024 * 10;
            totalBytes = 1024ul * 1024 * 1024 * 20;
            totalFreeBytes = 1024ul * 1024 * 1024 * 10;
            return 0;
        }

        public int Unmount(DokanFileInfo info)
        {
            return (int) MainForm.Instance.mNFS.UnMountDevice();
        }

        #endregion
    }
}
