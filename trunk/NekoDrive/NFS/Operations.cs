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
        #region Methods

        string CleanFileName(string filename)
        {
            int columnIndex = filename.IndexOf(":");
            if (columnIndex == -1)
                return filename;

            return filename.Substring(0, columnIndex);
        }

        #endregion

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

            filename = CleanFileName(filename);

            try
            {
                Debug("CreateFile {0}", filename);
                
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                if (MainForm.In.mNFS.IsDirectory(FullPath))
                    return ret;

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
                            if (MainForm.In.mNFS.FileExists(FullPath))
                                ret = -DokanNet.ERROR_ALREADY_EXISTS;
                            else
                                MainForm.In.mNFS.CreateFile(FullPath);
                            break;
                        }
                    case FileMode.Create:
                        {
                            Debug("Create");
                            if (MainForm.In.mNFS.FileExists(FullPath))
                                MainForm.In.mNFS.DeleteFile(FullPath);
                            
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
                ret = -DokanNet.DOKAN_ERROR;
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

            filename = CleanFileName(filename);

            Debug("CreateDirectory {0}", filename);
            try
            {
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                if (MainForm.In.mNFS.FileExists(FullPath))
                    return DokanNet.ERROR_FILE_EXISTS;

                byte UserP, GroupP, OtherP;

                switch (MainForm.In.UserIndex)
                {
                    case 0: UserP = 4; break;
                    case 1: UserP = 6; break;
                    case 2: UserP = 7; break;
                    default: UserP = 7; break;
                }

                switch (MainForm.In.GroupIndex)
                {
                    case 0: GroupP = 4; break;
                    case 1: GroupP = 6; break;
                    case 2: GroupP = 7; break;
                    default: GroupP = 7; break;
                }

                switch (MainForm.In.OtherIndex)
                {
                    case 0: OtherP = 4; break;
                    case 1: OtherP = 6; break;
                    case 2: OtherP = 7; break;
                    default: OtherP = 7; break;
                }

                MainForm.In.mNFS.CreateDirectory(FullPath, new NFSLibrary.Protocols.Commons.NFSPermission(UserP, GroupP, OtherP));
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
            MainForm.In.mNFS.CompleteIO();
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

            filename = CleanFileName(filename);

            if (MainForm.In.mNFS.IsDirectory(filename))
                return DokanNet.DOKAN_SUCCESS;

            try
            {
                Debug("ReadFile {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                Debug("ReadFile {0} {1} {2} {3}", Directory, FileName, offset, buffer.Length);
                long Bytes = (long) buffer.Length;
                MainForm.In.mNFS.Read(FullPath, (long)offset, ref Bytes, ref buffer);
                if (Bytes != -1)
                {
                    readBytes = (uint)Bytes;
                    Debug("ReadFile bytes {0}", readBytes);
                }
                else
                    ret = DokanNet.DOKAN_ERROR;
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

            filename = CleanFileName(filename);

            try
            {
                Debug("WriteFile {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);
            
                Debug("WriteFile {0} {1} {2} {3}", Directory, FileName, offset, buffer.Length);
                UInt32 Bytes = 0;
                MainForm.In.mNFS.Write(FullPath, (long)offset, (uint)buffer.Length, buffer, out Bytes);
                if (Bytes != 0)
                {
                    writtenBytes = (uint)Bytes;
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

            filename = CleanFileName(filename);

            try
            {
                Debug("GetFileInformation {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                NFSLibrary.Protocols.Commons.NFSAttributes nfsAttributes = MainForm.In.mNFS.GetItemAttributes(FullPath);
                if (nfsAttributes == null)
                    return DokanNet.DOKAN_ERROR;

                if (nfsAttributes.NFSType == NFSLibrary.Protocols.Commons.NFSItemTypes.NFDIR)
                    fileinfo.Attributes = System.IO.FileAttributes.Directory;
                else
                    fileinfo.Attributes = System.IO.FileAttributes.Archive;

                fileinfo.LastAccessTime = nfsAttributes.LastAccessedDateTime;
                fileinfo.LastWriteTime = nfsAttributes.ModifiedDateTime;
                fileinfo.CreationTime = nfsAttributes.CreateDateTime;
                fileinfo.Length = (long)nfsAttributes.Size;
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

            filename = CleanFileName(filename);

            try
            {
                Debug("FindFiles {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullPath = MainForm.In.mNFS.Combine(FileName, Directory);

                foreach (string strItem in MainForm.In.mNFS.GetItemList(FullPath))
                {
                    NFSLibrary.Protocols.Commons.NFSAttributes nfsAttributes = MainForm.In.mNFS.GetItemAttributes(MainForm.In.mNFS.Combine(strItem, FullPath));
                    if (nfsAttributes != null)
                    {
                        FileInformation fi = new FileInformation();
                        fi.Attributes = nfsAttributes.NFSType == NFSLibrary.Protocols.Commons.NFSItemTypes.NFDIR ? System.IO.FileAttributes.Directory : System.IO.FileAttributes.Normal;
                        fi.CreationTime = nfsAttributes.CreateDateTime;
                        fi.LastAccessTime = nfsAttributes.LastAccessedDateTime;
                        fi.LastWriteTime = nfsAttributes.ModifiedDateTime;
                        fi.Length = (long)nfsAttributes.Size;
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

            filename = CleanFileName(filename);

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

            filename = CleanFileName(filename);

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

            filename = CleanFileName(filename);

            try
            {
                Debug("MoveFile {0}", filename);

                if (MainForm.In.mNFS.IsDirectory(newname))
                {
                    newname = MainForm.In.mNFS.Combine(
                                    MainForm.In.mNFS.GetFileName(filename),
                                    newname
                                );
                }

                MainForm.In.mNFS.Move(filename, newname);
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

            //filename = CleanFileName(filename);

            //try
            //{
            //    Debug("SetEndOfFile {0}", filename);
            //    string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
            //    string FileName = MainForm.In.mNFS.GetFileName(filename);
            //    string FullName = MainForm.In.mNFS.Combine(FileName, Directory);

            //    MainForm.In.mNFS.SetFileSize(FullName, length);
            //}
            //catch (Exception ex)
            //{
            //    ret = DokanNet.DOKAN_ERROR;
            //    Debug("SetEndOfFile file {0} newfile {1} exception {2}", filename, ex.Message);
            //}

            return ret;
        }

        public int SetAllocationSize(string filename, long length, DokanFileInfo info)
        {
            int ret = DokanNet.DOKAN_SUCCESS;

            filename = CleanFileName(filename);

            try
            {
                Debug("SetEndOfFile {0}", filename);
                string Directory = MainForm.In.mNFS.GetDirectoryName(filename);
                string FileName = MainForm.In.mNFS.GetFileName(filename);
                string FullName = MainForm.In.mNFS.Combine(FileName, Directory);

                NFSLibrary.Protocols.Commons.NFSAttributes attr = MainForm.In.mNFS.GetItemAttributes(FullName);
                if (attr.Size < length)
                    MainForm.In.mNFS.SetFileSize(FullName, length);
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
