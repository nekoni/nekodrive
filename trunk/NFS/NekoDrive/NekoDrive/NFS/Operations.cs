using System;
using System.Collections.Generic;
using System.Text;
using Dokan;
using System.Collections;
using System.IO;
using NFSLibrary;

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
            int ret = DokanNet.DOKAN_SUCCESS;

            try
            {
                Debug("CreateFile {0}", filename);
                
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                switch (mode)
                {
                    case FileMode.Open:
                        {
                            Debug("Open");
                            if (!MainForm.In.mNFS.FileExists(FullPath))
                                ret = -DokanNet.ERROR_FILE_NOT_FOUND;
                            break;
                        }
                    case FileMode.CreateNew:
                        {
                            Debug("CreateNew");
                            if (!MainForm.In.mNFS.FileExists(FullPath))
                                ret = -DokanNet.ERROR_ALREADY_EXISTS;
                            else
                                MainForm.In.mNFS.CreateFile(FullPath);
                            break;
                        }
                    case FileMode.Create:
                        {
                            Debug("Create");
                            if (MainForm.In.mNFS.FileExists(FullPath))
                                ret = -DokanNet.ERROR_ALREADY_EXISTS;
                            else
                                MainForm.In.mNFS.CreateFile(FullPath);
                            break;
                        }
                    case FileMode.OpenOrCreate:
                        {
                            Debug("OpenOrCreate");
                            if (!MainForm.In.mNFS.FileExists(FullPath))
                                MainForm.In.mNFS.CreateFile(FullPath);
                            break;
                        }
                    case FileMode.Truncate:
                        {
                            Debug("Truncate");
                            if (!MainForm.In.mNFS.FileExists(FullPath))
                                ret = -DokanNet.ERROR_FILE_NOT_FOUND;
                            else
                                MainForm.In.mNFS.CreateFile(FullPath);
                            break;
                        }
                    case FileMode.Append:
                        {
                            Debug("Appen");
                            if (!MainForm.In.mNFS.FileExists(FullPath))
                                ret = -DokanNet.ERROR_FILE_NOT_FOUND;
                            break;
                        }
                    default:
                        {
                            Debug("Error unknown FileMode {0}", mode);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("CreateFile file {0} exception {1}", filename, ex.Message);
            }
            return ret;
        }

        public int OpenDirectory(string filename, DokanFileInfo info)
        {
            Debug("OpenDirectory {0}", filename);
            return DokanNet.DOKAN_SUCCESS;
        }

        public int CreateDirectory(string filename, DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;

            Debug("CreateDirectory {0}", filename);
            try
            {
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                MainForm.In.mNFS.CreateDirectory(FullPath);
            }
            catch (Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("CreateFile file {0} exception {1}", filename, ex.Message);
            }

            return ret;
        }

        public int Cleanup(string filename, DokanFileInfo info)
        {
            Debug("Cleanup {0}", filename);
            return DokanNet.DOKAN_SUCCESS; //???
        }

        public int CloseFile(string filename, DokanFileInfo info)
        {
            Debug("CloseFile {0}", filename);
            return DokanNet.DOKAN_SUCCESS;
        }

        public int ReadFile(string filename, byte[] buffer, ref uint readBytes, long offset, DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;

            try
            {
                Debug("ReadFile {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                Debug("ReadFile {0} {1} {2} {3}", Directory, FileName, offset, buffer.Length);
                ret = MainForm.In.mNFS.Read(FullPath, offset, (uint)buffer.Length, ref buffer);
                if (ret != -1)
                {
                    readBytes = (uint)ret;
                    Debug("ReadFile bytes {0}", readBytes);
                }
            }
            catch (Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("ReadFile file {0} exception {1}", filename, ex.Message);
            }
            return ret;
        }

        public int WriteFile(string filename, byte[] buffer, ref uint writtenBytes, long offset, DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;

            try
            {
                Debug("WriteFile {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);
            
                Debug("WriteFile {0} {1} {2} {3}", Directory, FileName, offset, buffer.Length);
                ret = MainForm.In.mNFS.Write(FullPath, offset, (uint)buffer.Length, buffer);
                if (ret != -1)
                {
                    writtenBytes = (uint)ret;
                    Debug("WriteFile bytes {0}", writtenBytes);
                }
            }
            catch(Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("WriteFile file {0} exception {1}", filename, ex.Message);
            }
            return ret;
        }

        public int FlushFileBuffers(string filename, DokanFileInfo info)
        {
            Debug("FlushFileBuffers {0}", filename);
            return DokanNet.DOKAN_SUCCESS;
        }

        public int GetFileInformation(string filename, FileInformation fileinfo, DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;

            try
            {
                Debug("GetFileInformation {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                NFSAttributes nfsAttributes = MainForm.In.mNFS.GetItemAttributes(FullPath);
                if (nfsAttributes == null)
                    return DokanNet.DOKAN_ERROR;

                if (nfsAttributes.type == NFSType.NFDIR)
                    fileinfo.Attributes = System.IO.FileAttributes.Directory;
                else
                    fileinfo.Attributes = System.IO.FileAttributes.Archive;

                fileinfo.LastAccessTime = nfsAttributes.adateTime;
                fileinfo.LastWriteTime = nfsAttributes.adateTime;
                fileinfo.CreationTime = nfsAttributes.cdateTime;
                fileinfo.Length = (long)nfsAttributes.size;
            }
            catch (Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("GetFileInformation file {0} exception {1}", filename, ex.Message);
            }

            return ret;
        }

        public int FindFiles(string filename, System.Collections.ArrayList files, DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;

            try
            {
                Debug("FindFiles {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                foreach (string strItem in MainForm.In.mNFS.GetItemList(FullPath))
                {
                    NFSAttributes nfsAttributes = MainForm.In.mNFS.GetItemAttributes(strItem);
                    if (nfsAttributes != null)
                    {
                        FileInformation fi = new FileInformation();
                        fi.Attributes = nfsAttributes.type == NFSType.NFDIR ? FileAttributes.Directory : FileAttributes.Normal;
                        fi.CreationTime = nfsAttributes.cdateTime;
                        fi.LastAccessTime = nfsAttributes.adateTime;
                        fi.LastWriteTime = nfsAttributes.adateTime;
                        fi.Length = (long)nfsAttributes.size;
                        fi.FileName = strItem;
                        files.Add(fi);
                    }
                }
            }
            catch (Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("FindFiles file {0} exception {1}", filename, ex.Message);
            }

            return ret;
        }

        public int SetFileAttributes(string filename, System.IO.FileAttributes attr, DokanFileInfo info)
        {
            Debug("SetFileAttributes {0}", filename);
            return DokanNet.DOKAN_SUCCESS;
        }

        public int SetFileTime(string filename, DateTime ctime, DateTime atime, DateTime mtime, DokanFileInfo info)
        {
            Debug("SetFileTime {0}", filename);
            return DokanNet.DOKAN_SUCCESS;
        }

        public int DeleteFile(string filename, DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;
            try
            {
                Debug("DeleteFile {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                if (MainForm.In.mNFS.FileExists(FullPath))
                {
                    MainForm.In.mNFS.DeleteFile(FullPath);
                }
                else
                    ret = -DokanNet.ERROR_FILE_NOT_FOUND;
            }
            catch (Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("DeleteFile file {0} exception {1}", filename, ex.Message);
            }

            return ret;
        }

        public int DeleteDirectory(string filename, DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;

            try
            {
                Debug("DeleteDirectory {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                MainForm.In.mNFS.DeleteDirectory(FullPath);
            }
            catch (Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("DeleteDirectory file {0} exception {1}", filename, ex.Message);
            }

            return ret;
        }

        public int MoveFile(string filename, string newname, bool replace, DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;

            try
            {
                Debug("MoveFile {0}", filename);
                string DirectoryOld = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileNameOld = MainForm.In.mNFS.GetFileName(filename);
                string FullPathOld = MainForm.In.mNFS.Combine(FileNameOld, DirectoryOld);

                string DirectoryNew = MainForm.In.mNFS.GetDirectoryName(newname);
                string FileNameNew = MainForm.In.mNFS.GetFileName(newname);
                string FullPathNew = MainForm.In.mNFS.Combine(FileNameNew, DirectoryNew);

                if (MainForm.In.mNFS.IsDirectory(FullPathNew))
                {
                    FileNameNew = FileNameOld;
                    DirectoryNew = FullPathNew;
                }

                MainForm.In.mNFS.Move(DirectoryOld, FileNameOld, DirectoryNew, FileNameNew);
            }
            catch (Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("MoveFile file {0} newfile {1} exception {2}", filename, newname, ex.Message);
            }

            return ret;
        }

        public int SetEndOfFile(string filename, long length, DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;

            try
            {
                Debug("SetEndOfFile {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullName = MainForm.In.mNFS.Combine(FileName, Directory);

                MainForm.In.mNFS.SetFileSize(FullName, (UInt64)length);
            }
            catch (Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("SetEndOfFile file {0} newfile {1} exception {2}", filename, ex.Message);
            }

            return ret;
        }

        public int SetAllocationSize(string filename, long length, DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;

            try
            {
                Debug("SetEndOfFile {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullName = MainForm.In.mNFS.Combine(FileName, Directory);

                NFSAttributes attr = MainForm.In.mNFS.GetItemAttributes(FullName);
                if (attr.size < length)
                    MainForm.In.mNFS.SetFileSize(FullName, (UInt64)length);
            }
            catch (Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("SetEndOfFile file {0} newfile {1} exception {2}", filename, ex.Message);
            }

            return ret;
        }

        public int LockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            Debug("LockFile {0}", filename);
            return DokanNet.DOKAN_SUCCESS;
        }

        public int UnlockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            Debug("UnlockFile {0}", filename);
            return DokanNet.DOKAN_SUCCESS;
        }

        public int GetDiskFreeSpace(ref ulong freeBytesAvailable, ref ulong totalBytes, ref ulong totalFreeBytes, DokanFileInfo info)
        {
            freeBytesAvailable = 1024ul * 1024 * 1024 * 10;
            totalBytes = 1024ul * 1024 * 1024 * 20;
            totalFreeBytes = 1024ul * 1024 * 1024 * 10;
            return DokanNet.DOKAN_SUCCESS;
        }

        public int Unmount(DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;

            try
            {
                Debug("Unmount");
                MainForm.In.mNFS.UnMountDevice();
            }
            catch (Exception ex)
            {
                ret = DokanNet.DOKAN_ERROR;
                Debug("Unmount exception {0}", ex.Message);
            }

            return ret;
        }

        #endregion
    }
}
