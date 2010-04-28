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

        private void Debug(string format, params object[] args)
        {
            if (MainForm.In.DebugMode)
            {
                Console.Error.WriteLine("NFS: " + format, args);
                System.Diagnostics.Debug.WriteLine(string.Format("NFS: " + format, args));
            }
        }

        public int CreateFile(string filename, System.IO.FileAccess access, System.IO.FileShare share, System.IO.FileMode mode, System.IO.FileOptions options, DokanFileInfo info)
        {
            Debug("CreateFile {0}", filename);

            int ret = -DokanNet.ERROR_FILE_NOT_FOUND;
            filename = MainForm.In.mNFS.ConvertPath(filename);
            string Directory = Path.GetDirectoryName(filename);
            string FileName = Path.GetFileName(filename);

            switch (mode)
            {
                case FileMode.Open:
                    {
                        Debug("Open");
                        if (MainForm.In.mNFS.FileExists(FileName, Directory))
                            ret = (int)MainForm.In.mNFS.Open(Path.Combine(Directory, FileName));
                        else
                            ret = -DokanNet.ERROR_FILE_NOT_FOUND;
                        break;
                    }
                case FileMode.CreateNew:
                    {
                        Debug("CreateNew");
                        if (MainForm.In.mNFS.FileExists(FileName, Directory))
                            ret = -DokanNet.ERROR_ALREADY_EXISTS;
                        else
                            ret = (int) MainForm.In.mNFS.CreateFile(FileName, Directory);
                        break;
                    }
                case FileMode.Create:
                    {
                        Debug("Create");
                        ret = (int) MainForm.In.mNFS.CreateFile(FileName, Directory);
                        break;
                    }
                case FileMode.OpenOrCreate:
                    {
                        Debug("OpenOrCreate");
                        if (MainForm.In.mNFS.FileExists(FileName, Directory))
                            ret = (int)MainForm.In.mNFS.Open(Path.Combine(Directory, FileName));
                        else
                            ret = (int)MainForm.In.mNFS.CreateFile(FileName, Directory);
                        break;
                    }
                case FileMode.Truncate:
                    {
                        Debug("Truncate");
                        if (!MainForm.In.mNFS.FileExists(FileName, Directory))
                            ret = -DokanNet.ERROR_FILE_NOT_FOUND;
                        else
                            ret = (int)MainForm.In.mNFS.CreateFile(FileName, Directory);
                        break;
                    }
                case FileMode.Append:
                    {
                        Debug("Appen");
                        if (MainForm.In.mNFS.FileExists(FileName, Directory))
                            ret = 0;
                        else
                            ret = (int)MainForm.In.mNFS.CreateFile(FileName, Directory);
                        break;
                    }
                default:
                    {
                        Debug("Error unknown FileMode {0}", mode);
                        break;
                    }
            }
            return ret;
        }

        public int OpenDirectory(string filename, DokanFileInfo info)
        {
            Debug("OpenDirectory {0}", filename);
            filename = MainForm.In.mNFS.ConvertPath(filename);
            if (MainForm.In.mNFS.CurrentDirectory != filename)
            {
                MainForm.In.mNFS.CurrentDirectory = filename;
                return (int)MainForm.In.mNFS.OpenDirectory(MainForm.In.mNFS.CurrentDirectory);
            }
            else
                return 0;
        }

        public int CreateDirectory(string filename, DokanFileInfo info)
        {
            Debug("CreateDirectory {0}", filename);
            filename = MainForm.In.mNFS.ConvertPath(filename);
            string Directory = Path.GetDirectoryName(filename);
            string DirectoryName = Path.GetFileName(filename);
            return (int)MainForm.In.mNFS.CreateDirectory(DirectoryName, Directory);
        }

        public int Cleanup(string filename, DokanFileInfo info)
        {
            Debug("Cleanup {0}", filename);
            return 0; //???
        }

        public int CloseFile(string filename, DokanFileInfo info)
        {
            Debug("CloseFile {0}", filename);
            MainForm.In.mNFS.CloseFile();
            return 0;
        }

        public int ReadFile(string filename, byte[] buffer, ref uint readBytes, long offset, DokanFileInfo info)
        {
            int ret = 0;
            Debug("ReadFile {0}", filename);
            filename = MainForm.In.mNFS.ConvertPath(filename);
            string Directory = Path.GetDirectoryName(filename);
            string FileName = Path.GetFileName(filename);
            try
            {
                Debug("ReadFile {0} {1} {2} {3}", Directory, FileName, offset, buffer.Length);
                ret = MainForm.In.mNFS.Read(Path.Combine(Directory, FileName), (ulong)offset, (uint)buffer.Length, ref buffer);
                if (ret != -1)
                {
                    readBytes = (uint)ret;
                    Debug("ReadFile bytes {0}", readBytes);
                    ret = 0;
                }
                else
                    ret = -1;
            }
            catch(Exception ex)
            {
                string message = ex.Message;
            }
            return ret;
        }

        public int WriteFile(string filename, byte[] buffer, ref uint writtenBytes, long offset, DokanFileInfo info)
        {
            Debug("WriteFile {0}", filename);
            filename = MainForm.In.mNFS.ConvertPath(filename);
            string Directory = Path.GetDirectoryName(filename);
            string FileName = Path.GetFileName(filename);
            if (OpenDirectory(Directory, null) != 0)
                return -1;
            int ret = -1;
            try
            {
                Debug("WriteFile {0} {1} {2} {3}", Directory, FileName, offset, buffer.Length);
                ret = MainForm.In.mNFS.Write(Path.Combine(Directory, FileName), (ulong) offset, (uint) buffer.Length, buffer);
                if (ret != -1)
                {
                    writtenBytes = (uint)ret;
                    Debug("WriteFile bytes {0}", writtenBytes);
                    ret = 0;
                }
                else
                    ret = -1;
            }
            catch
            {
            }
            return ret;
        }

        public int FlushFileBuffers(string filename, DokanFileInfo info)
        {
            Debug("FlushFileBuffers {0}", filename);
            return 0;
        }

        public int GetFileInformation(string filename, FileInformation fileinfo, DokanFileInfo info)
        {
            Debug("GetFileInformation {0}", filename);
            filename = MainForm.In.mNFS.ConvertPath(filename);
            string Directory = Path.GetDirectoryName(filename);
            string FileName = Path.GetFileName(filename);

            NFSAttributes nfsAttributes = MainForm.In.mNFS.GetItemAttributes(FileName, Directory);
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
            Debug("FindFiles {0}", filename);

            filename = MainForm.In.mNFS.ConvertPath(filename);
            string Directory = Path.GetDirectoryName(filename);
            string FileName = Path.GetFileName(filename);

            foreach (string strItem in MainForm.In.mNFS.GetItemList(Path.Combine(Directory, FileName)))
            {
                NFSAttributes nfsAttributes = MainForm.In.mNFS.GetItemAttributes(strItem);
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

        public int SetFileAttributes(string filename, System.IO.FileAttributes attr, DokanFileInfo info)
        {
            Debug("SetFileAttributes {0}", filename);
            return 0;
        }

        public int SetFileTime(string filename, DateTime ctime, DateTime atime, DateTime mtime, DokanFileInfo info)
        {
            Debug("SetFileTime {0}", filename);
            return 0;
        }

        public int DeleteFile(string filename, DokanFileInfo info)
        {
            Debug("DeleteFile {0}", filename);
            filename = MainForm.In.mNFS.ConvertPath(filename);
            string Directory = Path.GetDirectoryName(filename);
            string FileName = Path.GetFileName(filename);
            if (MainForm.In.mNFS.FileExists(FileName, Directory))
                return (int) MainForm.In.mNFS.DeleteFile(FileName, Directory);

            return -DokanNet.ERROR_FILE_NOT_FOUND;
        }

        public int DeleteDirectory(string filename, DokanFileInfo info)
        {
            Debug("DeleteDirectory {0}", filename);
            filename = MainForm.In.mNFS.ConvertPath(filename);
            string Directory = Path.GetDirectoryName(filename);
            string FileName = Path.GetFileName(filename);
            return (int)MainForm.In.mNFS.DeleteDirectory(FileName, Directory);
        }

        public int MoveFile(string filename, string newname, bool replace, DokanFileInfo info)
        {
            Debug("MoveFile {0}", filename);
            filename = MainForm.In.mNFS.ConvertPath(filename);
            string OldDirName = Path.GetDirectoryName(filename);
            string OldFileName = Path.GetFileName(filename);
            newname = MainForm.In.mNFS.ConvertPath(newname);
            string NewDirName = Path.GetDirectoryName(newname);
            string NewFileName = Path.GetFileName(newname);
            string TestDirectory = newname.Replace("/", "\\");
            if (MainForm.In.mNFS.IsDirectory(TestDirectory) == NFSResult.NFS_SUCCESS)
            {
                NewDirName = Path.Combine(NewDirName, NewFileName);
                NewFileName = OldFileName;
            }
            return (int)MainForm.In.mNFS.Move(OldDirName, OldFileName, NewDirName, NewFileName);
        }

        public int SetEndOfFile(string filename, long length, DokanFileInfo info)
        {
            Debug("SetEndOfFile {0}", filename);
            return 0;
        }

        public int SetAllocationSize(string filename, long length, DokanFileInfo info)
        {
            Debug("SetAllocationSize {0}", filename);
            return 0;
        }

        public int LockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            Debug("LockFile {0}", filename);
            return 0;
        }

        public int UnlockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            Debug("UnlockFile {0}", filename);
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
            return (int) MainForm.In.mNFS.UnMountDevice();
        }

        #endregion
    }
}
