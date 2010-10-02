using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using org.acplt.oncrpc;
using System.IO;

namespace NFSLibrary.Protocols.V3
{
    public class NFSv3 : INFS
    {
        #region Fields

        NFSv3MountProtocolClient _MountProtocolV3 = null;
        NFSv3ProtocolClient _ProtocolV3 = null;
        string _MountedDevice = string.Empty;
        byte[] _RootDirectoryHandle = null;
        int _GId = -1;
        int _UId = -1;
        string _CurrentFile = string.Empty;
        byte[] _CurrentFileHandle = null;

        #endregion

        #region Constants

        const int MODE_FMT = 0170000;
        const int MODE_DIR = 0040000;
        const int MODE_CHR = 0020000;
        const int MODE_BLK = 0060000;
        const int MODE_REG = 0100000;
        const int MODE_LNK = 0120000;
        const int MODE_SOCK = 0140000;
        const int MODE_FIFO = 0010000;

        #endregion

        #region Constructur

        public void Connect(IPAddress Address)
        {
            Connect(Address, 0, 0, 60000, System.Text.Encoding.ASCII);
        }

        public void Connect(IPAddress Address, int UserId, int GroupId, int Timeout)
        {
            Connect(Address, UserId, GroupId, Timeout, System.Text.Encoding.ASCII);
        }

        public void Connect(IPAddress Address, int UserId, int GroupId, int Timeout, System.Text.Encoding characterEncoding)
        {
            if (characterEncoding == null)
            { characterEncoding = System.Text.Encoding.ASCII; }

            _GId = GroupId;
            _UId = UserId;

            _MountProtocolV3 = new NFSv3MountProtocolClient(Address, OncRpcProtocols.ONCRPC_UDP);
            _ProtocolV3 = new NFSv3ProtocolClient(Address, OncRpcProtocols.ONCRPC_UDP);

            OncRpcClientAuthUnix authUnix = new OncRpcClientAuthUnix(Address.ToString(), UserId, GroupId);

            _MountProtocolV3.GetClient().setAuth(authUnix);
            _MountProtocolV3.GetClient().setTimeout(Timeout);
            _MountProtocolV3.GetClient().setCharacterEncoding(characterEncoding.WebName);

            _ProtocolV3.GetClient().setAuth(authUnix);
            _ProtocolV3.GetClient().setTimeout(Timeout);
            _ProtocolV3.GetClient().setCharacterEncoding(characterEncoding.WebName);
        }

        #endregion

        #region Public Methods

        public void Disconnect()
        {
            if (_MountProtocolV3 != null)
                _MountProtocolV3.close();

            if (_ProtocolV3 != null)
                _ProtocolV3.close();
        }

        public List<string> GetExportedDevices()
        {
            List<string> nfsDevices = new List<string>();
            if (_MountProtocolV3 != null)
            {
                exports3 exp = _MountProtocolV3.MOUNTPROC3_EXPORT_3();
                bool Exit = false;
                while (!Exit)
                {
                    if (exp.value.ex_next.value == null)
                        Exit = true;
                    nfsDevices.Add(exp.value.ex_dir.value);
                    exp = exp.value.ex_next;
                }
            }
            else
                throw new ApplicationException("NFS Client not connected!");
            return nfsDevices;
        }

        public void MountDevice(string DeviceName)
        {
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                mountres3 mnt = null;
                //mount
                mnt = _MountProtocolV3.MOUNTPROC3_MNT_3(new dirpath3(DeviceName));
                if (mnt.fhs_status == 0)
                {
                    _MountedDevice = DeviceName;
                    _RootDirectoryHandle = new Byte[NFSv3Protocol.NFS3_FHSIZE];
                    Array.Copy(mnt.mountinfo.fhandle.data, _RootDirectoryHandle, mnt.mountinfo.fhandle.data.Length);
                }
                else
                    throw new ApplicationException("MOUNTPROC3_MNT_3: errorcode " + mnt.fhs_status);
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void UnMountDevice()
        {
            if (_MountedDevice != null)
            {
                _MountProtocolV3.MOUNTPROC3_UMNT_3(new dirpath3(_MountedDevice));
                _MountedDevice = string.Empty;
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public List<string> GetItemList(string DirectoryFullName)
        {
            List<string> ItemsList = new List<string>();
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                READDIR3args dpRdArgs = new READDIR3args();
                READDIR3res pReadDirRes = new READDIR3res();
                dpRdArgs.cookie  = new cookie3(new uint64(0));
                dpRdArgs.count = new count3(new uint32(4096));
                dpRdArgs.cookieverf = new cookieverf3(new byte[NFSv3Protocol.NFS3_COOKIEVERFSIZE]);
                byte[] itemHandle = null;
                entry3 pEntry = null;
                if ((itemHandle = GetItemAttributes(DirectoryFullName).handle) != null)
                {
                    dpRdArgs.dir = new nfs_fh3();
                    dpRdArgs.dir.data = new byte[NFSv3Protocol.NFS3_FHSIZE];
                    Array.Copy(itemHandle, dpRdArgs.dir.data, NFSv3Protocol.NFS3_FHSIZE);
                    while (true)
                    {
                        if ((pReadDirRes = _ProtocolV3.NFSPROC3_READDIR_3(dpRdArgs)) == null)
                        {
                            throw new ApplicationException("NFSPROC3_READDIR_3: failure");
                        }
                        else
                        {
                            if (pReadDirRes.status == nfsstat3.NFS3_OK)
                            {
                                pEntry = pReadDirRes.resok.reply.entries;
                                Array.Copy(pReadDirRes.resok.cookieverf.value, dpRdArgs.cookieverf.value, NFSv3Protocol.NFS3_COOKIEVERFSIZE);
                                while (pEntry != null)
                                {
                                    ItemsList.Add(pEntry.name.value);
                                    dpRdArgs.cookie = pEntry.cookie;
                                    pEntry = pEntry.nextentry;
                                }
                            }
                            else
                            {
                                throw new ApplicationException("NFSPROC3_READDIR_3: errorcode " + pReadDirRes.status);
                            }
                            if (pReadDirRes.resok.reply.eof)
                                break;
                        }
                    }
                }
            }
            else
                throw new ApplicationException("NFS Client not connected!");

            return ItemsList;
        }

        public NFSAttributes GetItemAttributes(string ItemFullName)
        {
            NFSAttributes attributes = null;
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                if (String.IsNullOrEmpty(ItemFullName))
                    ItemFullName = ".";

                byte[] currentItem = new byte[NFSv3Protocol.NFS3_FHSIZE]; ;
                Array.Copy(_RootDirectoryHandle, currentItem, NFSv3Protocol.NFS3_FHSIZE);
                foreach (string Item in ItemFullName.Split(@"\".ToCharArray()))
                {
                    LOOKUP3args dpLookUpArgs = new LOOKUP3args();
                    LOOKUP3res pLookUpRes;
                    dpLookUpArgs.what = new diropargs3();
                    dpLookUpArgs.what.dir = new nfs_fh3();
                    dpLookUpArgs.what.dir.data = new byte[NFSv3Protocol.NFS3_FHSIZE];
                    Array.Copy(currentItem, dpLookUpArgs.what.dir.data, NFSv3Protocol.NFS3_FHSIZE);
                    dpLookUpArgs.what.name = new filename3(Item);

                    if ((pLookUpRes = _ProtocolV3.NFSPROC3_LOOKUP_3(dpLookUpArgs)) != null)
                    {
                        if (pLookUpRes.status == nfsstat3.NFS3_OK)
                        {
                            Array.Copy(pLookUpRes.resok.obj.data, currentItem, pLookUpRes.resok.obj.data.Length);
                            attributes = new NFSAttributes(pLookUpRes.resok.obj_attributes.attributes.ctime.seconds.value, pLookUpRes.resok.obj_attributes.attributes.atime.seconds.value,
                                pLookUpRes.resok.obj_attributes.attributes.mtime.seconds.value, pLookUpRes.resok.obj_attributes.attributes.type, pLookUpRes.resok.obj_attributes.attributes.size.value.value, currentItem);
                        }
                        else
                        {
                            if (pLookUpRes.status == nfsstat3.NFS3ERR_NOENT)
                                return null;

                            throw new ApplicationException("NFSPROC3_LOOKUP_3: errorcode " + pLookUpRes.status);
                        }
                    }
                }
            }
            else
                throw new ApplicationException("NFS Client not connected!");

            if (attributes == null)
                throw new ApplicationException("GetItemAttributes: failure");

            return attributes;
        }

        public void CreateDirectory(string DirectoryFullName)
        {
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                string ParentDirectory = Path.GetDirectoryName(DirectoryFullName);
                string DirectoryName = Path.GetFileName(DirectoryFullName);
                NFSAttributes ParentAttributes = GetItemAttributes(ParentDirectory);

                MKDIR3args dpMkDirArgs = new MKDIR3args();
                MKDIR3res pMkDirRes;
                dpMkDirArgs.attributes = new sattr3();
                dpMkDirArgs.attributes.atime = new set_atime();
                dpMkDirArgs.attributes.atime.set_it = time_how.DONT_CHANGE;
                dpMkDirArgs.attributes.mtime = new set_mtime();
                dpMkDirArgs.attributes.mtime.set_it = time_how.DONT_CHANGE;
                dpMkDirArgs.attributes.size = new set_size3();
                dpMkDirArgs.attributes.size.set_it = false;
                dpMkDirArgs.attributes.mode = new set_mode3();
                /* Calculate Permission */
                byte userP = 7; byte groupP = 7; byte otherP = 7;
                int permission = 0;
                permission = (((int)userP) << 6) | (((int)groupP) << 3) | ((int)otherP);
                /*  ---  */
                dpMkDirArgs.attributes.mode.mode = new mode3(new uint32(permission));
                dpMkDirArgs.attributes.mode.set_it = true;
                dpMkDirArgs.attributes.gid = new set_gid3();
                dpMkDirArgs.attributes.gid.gid = new gid3(new uint32(_GId));
                dpMkDirArgs.attributes.gid.set_it = true;
                dpMkDirArgs.attributes.uid = new set_uid3();
                dpMkDirArgs.attributes.uid.uid = new uid3(new uint32(_UId));
                dpMkDirArgs.attributes.uid.set_it = true;
                dpMkDirArgs.where = new diropargs3();
                dpMkDirArgs.where.dir = new nfs_fh3();
                dpMkDirArgs.where.dir.data = ParentAttributes.handle;
                dpMkDirArgs.where.name = new filename3(DirectoryName);

                if ((pMkDirRes = _ProtocolV3.NFSPROC3_MKDIR_3(dpMkDirArgs)) != null)
                {
                    if (pMkDirRes.status != nfsstat3.NFS3_OK)
                        throw new ApplicationException("NFSPROC3_MKDIR_3: errorcode " + pMkDirRes.status);
                }
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void DeleteDirectory(string DirectoryFullName)
        {
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                string ParentDirectory = Path.GetDirectoryName(DirectoryFullName);
                string DirectoryName = Path.GetFileName(DirectoryFullName);
                NFSAttributes ParentAttributes = GetItemAttributes(ParentDirectory);

                RMDIR3args dpRmDirArgs = new RMDIR3args();
                RMDIR3res pRmDirRes;

                dpRmDirArgs.obj = new diropargs3();
                dpRmDirArgs.obj.dir = new nfs_fh3();
                dpRmDirArgs.obj.dir.data = ParentAttributes.handle;
                dpRmDirArgs.obj.name = new filename3(DirectoryName);

                if ((pRmDirRes = _ProtocolV3.NFSPROC3_RMDIR_3(dpRmDirArgs)) != null)
                {
                    if (pRmDirRes.status != nfsstat3.NFS3_OK)
                        throw new ApplicationException("NFSPROC3_RMDIR_3: errorcode " + pRmDirRes.status);
                }
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void DeleteFile(string FileFullName)
        {
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                string ParentDirectory = Path.GetDirectoryName(FileFullName);
                string FileName = Path.GetFileName(FileFullName);
                NFSAttributes ParentAttributes = GetItemAttributes(ParentDirectory);

                REMOVE3args dpRemoveArgs = new REMOVE3args();
                REMOVE3res pRemoveRes;

                dpRemoveArgs.obj = new diropargs3();
                dpRemoveArgs.obj.dir = new nfs_fh3();
                dpRemoveArgs.obj.dir.data = ParentAttributes.handle;
                dpRemoveArgs.obj.name = new filename3(FileName);

                if ((pRemoveRes = _ProtocolV3.NFSPROC3_REMOVE_3(dpRemoveArgs)) != null)
                {
                    if (pRemoveRes.status != nfsstat3.NFS3_OK)
                        throw new ApplicationException("NFSPROC3_REMOVE_3: errorcode " + pRemoveRes.status);
                }
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void CreateFile(string FileFullName)
        {
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                string ParentDirectory = Path.GetDirectoryName(FileFullName);
                string FileName = Path.GetFileName(FileFullName);
                NFSAttributes ParentAttributes = GetItemAttributes(ParentDirectory);

                CREATE3args dpCreateArgs = new CREATE3args();
                CREATE3res pCreateRes;

                /* Calculate Permission */
                byte userP = 7; byte groupP = 7; byte otherP = 7;
                int permission = 0;
                permission = (((int)userP) << 6) | (((int)groupP) << 3) | ((int)otherP);
                /*  ---  */

                dpCreateArgs.how = new createhow3();
                dpCreateArgs.how.mode = createmode3.UNCHECKED;
                dpCreateArgs.how.obj_attributes_gu = new sattr3();
                dpCreateArgs.how.obj_attributes_gu.atime = new set_atime();
                dpCreateArgs.how.obj_attributes_gu.atime.set_it = time_how.DONT_CHANGE;
                dpCreateArgs.how.obj_attributes_gu.mtime = new set_mtime();
                dpCreateArgs.how.obj_attributes_gu.mtime.set_it = time_how.DONT_CHANGE;
                dpCreateArgs.how.obj_attributes_gu.mode = new set_mode3();
                dpCreateArgs.how.obj_attributes_gu.mode.mode = new mode3(new uint32(permission));
                dpCreateArgs.how.obj_attributes_gu.mode.set_it = true;
                dpCreateArgs.how.obj_attributes_gu.gid = new set_gid3();
                dpCreateArgs.how.obj_attributes_gu.gid.gid = new gid3(new uint32(_GId));
                dpCreateArgs.how.obj_attributes_gu.gid.set_it = true;
                dpCreateArgs.how.obj_attributes_gu.uid = new set_uid3();
                dpCreateArgs.how.obj_attributes_gu.uid.set_it = true;
                dpCreateArgs.how.obj_attributes_gu.uid.uid = new uid3(new uint32(_UId));
                dpCreateArgs.how.obj_attributes_gu.size = new set_size3();
                dpCreateArgs.how.obj_attributes_gu.size.size = new size3(new uint64(0));
                dpCreateArgs.how.obj_attributes_gu.size.set_it = true;

                dpCreateArgs.how.obj_attributes_un = new sattr3();
                dpCreateArgs.how.obj_attributes_un.atime = new set_atime();
                dpCreateArgs.how.obj_attributes_un.atime.set_it = time_how.DONT_CHANGE;
                dpCreateArgs.how.obj_attributes_un.mtime = new set_mtime();
                dpCreateArgs.how.obj_attributes_un.mtime.set_it = time_how.DONT_CHANGE;
                dpCreateArgs.how.obj_attributes_un.mode = new set_mode3();
                dpCreateArgs.how.obj_attributes_un.mode.mode = new mode3(new uint32(permission));
                dpCreateArgs.how.obj_attributes_un.mode.set_it = true;
                dpCreateArgs.how.obj_attributes_un.gid = new set_gid3();
                dpCreateArgs.how.obj_attributes_un.gid.gid = new gid3(new uint32(_GId));
                dpCreateArgs.how.obj_attributes_un.gid.set_it = true;
                dpCreateArgs.how.obj_attributes_un.uid = new set_uid3();
                dpCreateArgs.how.obj_attributes_un.uid.set_it = true;
                dpCreateArgs.how.obj_attributes_un.uid.uid = new uid3(new uint32(_UId));
                dpCreateArgs.how.obj_attributes_un.size = new set_size3();
                dpCreateArgs.how.obj_attributes_un.size.size = new size3(new uint64(0));
                dpCreateArgs.how.obj_attributes_un.size.set_it = true;
                dpCreateArgs.how.verf = new createverf3(new byte[NFSv3Protocol.NFS3_CREATEVERFSIZE]);
                
                dpCreateArgs.where = new diropargs3();
                dpCreateArgs.where.dir = new nfs_fh3();
                dpCreateArgs.where.dir.data = new byte[NFSv3Protocol.NFS3_FHSIZE];
                Array.Copy(ParentAttributes.handle, dpCreateArgs.where.dir.data, NFSv3Protocol.NFS3_FHSIZE);
                dpCreateArgs.where.name = new filename3(FileName);

                if ((pCreateRes = _ProtocolV3.NFSPROC3_CREATE_3(dpCreateArgs)) != null)
                {
                    if (pCreateRes.status != nfsstat3.NFS3_OK)
                        throw new ApplicationException("NFSPROC3_CREATE_3: errorcode " + pCreateRes.status);
                }
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void Read(string FileFullName, long Offset, uint Count, ref byte[] Buffer, out int Size)
        {
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                if (_CurrentFile != FileFullName)
                {
                    NFSAttributes Attributes = GetItemAttributes(FileFullName);
                    _CurrentFileHandle = Attributes.handle;
                    _CurrentFile = FileFullName;
                }

                READ3args dpArgRead = new READ3args();
                READ3res pReadRes;
                dpArgRead.file = new nfs_fh3();
                dpArgRead.file.data = _CurrentFileHandle;
                dpArgRead.offset = new offset3(new uint64(Offset));
                dpArgRead.count = new count3(new uint32((int)Count));
                if ((pReadRes = _ProtocolV3.NFSPROC3_READ_3(dpArgRead)) != null)
                {
                    if (pReadRes.status != nfsstat3.NFS3_OK)
                        throw new ApplicationException("NFSPROC3_READ_3: errorcode " + pReadRes.status);

                    Size = pReadRes.resok.data.Length;
                    Array.Copy(pReadRes.resok.data, Buffer, Size);
                }
                else
                    Size = -1;
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void SetFileSize(string FileFullName, ulong Size)
        {
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                NFSAttributes Attributes = GetItemAttributes(FileFullName);

                SETATTR3args dpArgSAttr = new SETATTR3args();
                SETATTR3res pAttrStat;

                dpArgSAttr.new_attributes = new sattr3();
                dpArgSAttr.new_attributes.atime = new set_atime();
                dpArgSAttr.new_attributes.atime.set_it = time_how.DONT_CHANGE;
                dpArgSAttr.new_attributes.mtime = new set_mtime();
                dpArgSAttr.new_attributes.mtime.set_it = time_how.DONT_CHANGE;
                dpArgSAttr.new_attributes.gid = new set_gid3();
                dpArgSAttr.new_attributes.gid.set_it = false;
                dpArgSAttr.new_attributes.uid = new set_uid3();
                dpArgSAttr.new_attributes.uid.set_it = false;
                dpArgSAttr.new_attributes.mode = new set_mode3();
                dpArgSAttr.new_attributes.mode.set_it = false;
                dpArgSAttr.new_attributes.size = new set_size3();
                dpArgSAttr.new_attributes.size.set_it = true;
                dpArgSAttr.new_attributes.size.size = new size3(new uint64((long)Size));
                dpArgSAttr.guard = new sattrguard3();
                dpArgSAttr.guard.check = false;
                if ((pAttrStat = _ProtocolV3.NFSPROC3_SETATTR_3(dpArgSAttr)) != null)
                    if (pAttrStat.status != nfsstat3.NFS3_OK)
                        throw new ApplicationException("NFSPROC3_SETATTR_3: errorcode " + pAttrStat.status);
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void Write(string FileFullName, long Offset, uint Count, byte[] Buffer, out int Size)
        {
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                if (_CurrentFile != FileFullName)
                {
                    NFSAttributes Attributes = GetItemAttributes(FileFullName);
                    _CurrentFileHandle = Attributes.handle;
                    _CurrentFile = FileFullName;
                }

                WRITE3args dpArgWrite = new WRITE3args();
                WRITE3res pWriteRes;
                dpArgWrite.file = new nfs_fh3();
                dpArgWrite.file.data = new byte[NFSv3Protocol.NFS3_FHSIZE];
                Array.Copy(_CurrentFileHandle, dpArgWrite.file.data, NFSv3Protocol.NFS3_FHSIZE);
                dpArgWrite.offset = new offset3(new uint64(Offset));
                dpArgWrite.count = new count3(new uint32((int)Count));
                dpArgWrite.data = Buffer;
                if ((pWriteRes = _ProtocolV3.NFSPROC3_WRITE_3(dpArgWrite)) != null)
                {
                    if (pWriteRes.status != nfsstat3.NFS3_OK)
                        throw new ApplicationException("NFSPROC3_WRITE_3: errorcode " + pWriteRes.status);

                    Size = pWriteRes.resok.count.value.value;
                }
                else
                    Size = -1;
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void Move(string OldDirectoryFullName, string OldFileName, string NewDirectoryFullName, string NewFileName)
        {
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                RENAME3args dpArgRename = new RENAME3args();
                RENAME3res pRenameRes;

                NFSAttributes OldDirectory = GetItemAttributes(OldDirectoryFullName);
                NFSAttributes NewDirectory = GetItemAttributes(NewDirectoryFullName);

                dpArgRename.from = new diropargs3();
                dpArgRename.from.dir = new nfs_fh3();
                dpArgRename.from.dir.data = OldDirectory.handle;
                dpArgRename.from.name = new filename3(OldFileName);
                dpArgRename.to = new diropargs3();
                dpArgRename.to.dir = new nfs_fh3();
                dpArgRename.to.dir.data = NewDirectory.handle;
                dpArgRename.to.name = new filename3(NewFileName);

                if ((pRenameRes = _ProtocolV3.NFSPROC3_RENAME_3(dpArgRename)) != null)
                {
                    if (pRenameRes.status != nfsstat3.NFS3_OK)
                        throw new ApplicationException("NFSPROC3_RENAME_3: errorcode " + pRenameRes.status);
                }
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public bool IsDirectory(string DirectoryFullName)
        {
            if (_ProtocolV3 != null && _MountProtocolV3 != null)
            {
                NFSAttributes Attributes = GetItemAttributes(DirectoryFullName);
                if (Attributes.type != NFSType.NFDIR)
                    return false;
                else
                    return true;
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        #endregion
    }

}

