using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using org.acplt.oncrpc;
using System.IO;

namespace NFSLibrary.Protocols.V2
{
    public class NFSv2: INFS
    {
        #region Fields

        NFSv2MountProtocolClient _MountProtocolV2 = null;
        NFSv2ProtocolClient _ProtocolV2 = null;
        string _MountedDevice = string.Empty;
        nfshandle _RootDirectoryHandle = null;
        int _GId = -1;
        int _UId = -1;
        string _CurrentFile = string.Empty;
        nfshandle _CurrentFileHandle = null;

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

            _MountProtocolV2 = new NFSv2MountProtocolClient(Address, OncRpcProtocols.ONCRPC_UDP);
            _ProtocolV2 = new NFSv2ProtocolClient(Address, OncRpcProtocols.ONCRPC_UDP);

            OncRpcClientAuthUnix authUnix = new OncRpcClientAuthUnix(Address.ToString(), UserId, GroupId);
            

            _MountProtocolV2.GetClient().setAuth(authUnix);
            _MountProtocolV2.GetClient().setTimeout(Timeout);
            _MountProtocolV2.GetClient().setCharacterEncoding(characterEncoding.EncodingName);

            _ProtocolV2.GetClient().setAuth(authUnix);
            _ProtocolV2.GetClient().setTimeout(Timeout);
            _ProtocolV2.GetClient().setCharacterEncoding(characterEncoding.EncodingName);
        }

        public void Disconnect()
        {
            if (_MountProtocolV2 != null)
                _MountProtocolV2.close();
            
            if (_ProtocolV2 != null)
                _ProtocolV2.close();
        }

        public List<string> GetExportedDevices()
        {
            List<string> nfsDevices = new List<string>();
            if (_MountProtocolV2 != null)
            {
                exports exp = _MountProtocolV2.MOUNTPROC_EXPORT_1();
                bool Exit = false;
                while (!Exit)
                {
                    if (exp.value.next.value == null)
                        Exit = true;
                    nfsDevices.Add(exp.value.filesys.value);
                    exp = exp.value.next;
                }
            }
            else
                throw new ApplicationException("NFS Client not connected!");
            return nfsDevices;
        }

        public void MountDevice(string DeviceName)
        {
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
            {
                fhstatus mnt = null;
                //mount
                mnt = _MountProtocolV2.MOUNTPROC_MNT_1(new dirpath(DeviceName));
                if (mnt.status == 0)
                {
                    _MountedDevice = DeviceName;
                    _RootDirectoryHandle = new nfshandle(new Byte[NFSv2Protocol.FHSIZE]);
                    Array.Copy(mnt.directory.value, _RootDirectoryHandle.value, NFSv2Protocol.FHSIZE);
                }
                else
                    throw new ApplicationException("MOUNTPROC_MNT_1: errorcode " + mnt.status);
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void UnMountDevice()
        {
            if (_MountedDevice != null)
            {
                _MountProtocolV2.MOUNTPROC_UMNT_1(new dirpath(_MountedDevice));
                _MountedDevice = string.Empty;
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public List<string> GetItemList(string DirectoryFullName)
        {
            List<string> ItemsList = new List<string>();
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
            {
                readdirargs dpRdArgs = new readdirargs();
                readdirres pReadDirRes = new readdirres();
                dpRdArgs.cookie = new nfscookie(0);
                dpRdArgs.count = 4096;
                nfshandle itemHandle = null;
                entry pEntry = null;
                if ((itemHandle = new nfshandle(GetItemAttributes(DirectoryFullName).handle)) != null)
                {
                    dpRdArgs.dir = new nfshandle(new Byte[NFSv2Protocol.FHSIZE]);
                    Array.Copy(itemHandle.value, dpRdArgs.dir.value, NFSv2Protocol.FHSIZE);
                    while(true)
		            {
			            if( (pReadDirRes = _ProtocolV2.NFSPROC_READDIR_2(dpRdArgs)) == null ) 
			            {
                            throw new ApplicationException("NFSPROC_READDIR_2: failure");
			            }
			            else
			            {
				            if(pReadDirRes.status == nfsstat.NFS_OK)
				            {
					            pEntry = pReadDirRes.ok.entries;
					            while(pEntry != null)
					            {
						            ItemsList.Add(pEntry.name.value);
						            dpRdArgs.cookie = pEntry.cookie;
						            pEntry = pEntry.nextentry;
					            }
				            }
				            else
				            {
                                throw new ApplicationException("NFSPROC_READDIR_2: errorcode " + pReadDirRes.status);
				            }
				            if(pReadDirRes.ok.eof)
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
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
            {
                if (String.IsNullOrEmpty(ItemFullName))
                    ItemFullName = ".";

                nfshandle currentItem = new nfshandle(new Byte[NFSv2Protocol.FHSIZE]);
                Array.Copy(_RootDirectoryHandle.value, currentItem.value, NFSv2Protocol.FHSIZE);
                foreach (string Item in ItemFullName.Split(@"\".ToCharArray()))
                {
                    diropargs dpDrArgs = new diropargs();
                    diropres pDirOpRes;
                    dpDrArgs.dir = new nfshandle(new Byte[NFSv2Protocol.FHSIZE]);
                    Array.Copy(currentItem.value, dpDrArgs.dir.value, NFSv2Protocol.FHSIZE);
                    dpDrArgs.name = new filename(string.Empty);
                    dpDrArgs.name.value = Item;
                    if ((pDirOpRes = _ProtocolV2.NFSPROC_LOOKUP_2(dpDrArgs)) != null)
                    {
                        if (pDirOpRes.status == nfsstat.NFS_OK)
                        {
                            Array.Copy(pDirOpRes.ok.file.value, currentItem.value, NFSv2Protocol.FHSIZE);
                            attributes = new NFSAttributes(pDirOpRes.ok.attributes.ctime.seconds, pDirOpRes.ok.attributes.atime.seconds,
                                pDirOpRes.ok.attributes.mtime.seconds, pDirOpRes.ok.attributes.type, pDirOpRes.ok.attributes.size, pDirOpRes.ok.file.value);
                        }
                        else
                        {
                            if (pDirOpRes.status == nfsstat.NFSERR_NOENT)
                                return null;

                            throw new ApplicationException("NFSPROC_LOOKUP_2: errorcode " + pDirOpRes.status);
                        }
                    }
                }
            }
            else
                throw new ApplicationException("NFS Client not connected!");

            if(attributes == null)
                throw new ApplicationException("GetItemAttributes: failure");

            return attributes;
        }

        public void CreateDirectory(string DirectoryFullName)
        {
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
            {
                string ParentDirectory = Path.GetDirectoryName(DirectoryFullName);
                string DirectoryName = Path.GetFileName(DirectoryFullName);
                NFSAttributes ParentAttributes = GetItemAttributes(ParentDirectory);

                createargs dpArgCreate = new createargs();
                diropres pDirOpRes;
                dpArgCreate.attributes = new sattr();
                dpArgCreate.attributes.atime = new nfstimeval();
		        dpArgCreate.attributes.atime.seconds = -1;
		        dpArgCreate.attributes.atime.useconds = -1;
                dpArgCreate.attributes.mtime = new nfstimeval();
		        dpArgCreate.attributes.mtime.seconds = -1;
		        dpArgCreate.attributes.mtime.useconds = -1;
                /* Calculate Permission */
                byte userP = 7; byte groupP = 7; byte otherP = 7;
                int permission = 0;
                permission = (((int)userP) << 6) | (((int)groupP) << 3) | ((int)otherP);
                /*  ---  */
                dpArgCreate.attributes.mode = permission;
		        dpArgCreate.attributes.gid = _GId;
		        dpArgCreate.attributes.uid = _UId;
                dpArgCreate.where = new diropargs();
                dpArgCreate.where.dir = new nfshandle(ParentAttributes.handle);
                dpArgCreate.where.name = new filename(DirectoryName);
                if( (pDirOpRes = _ProtocolV2.NFSPROC_MKDIR_2(dpArgCreate)) != null ) 
                {
			        if (pDirOpRes.status != nfsstat.NFS_OK)
                        throw new ApplicationException("NFSPROC_MKDIR_2: errorcode " + pDirOpRes.status);
		        }
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void DeleteDirectory(string DirectoryFullName)
        {
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
            {
                string ParentDirectory = Path.GetDirectoryName(DirectoryFullName);
                string DirectoryName = Path.GetFileName(DirectoryFullName);
                NFSAttributes ParentAttributes = GetItemAttributes(ParentDirectory);

                diropargs dpArgDelete = new diropargs();
                int status = 0;

		        dpArgDelete.dir = new nfshandle(ParentAttributes.handle);
                dpArgDelete.name = new filename(DirectoryName);
                if( (status = _ProtocolV2.NFSPROC_RMDIR_2(dpArgDelete)) !=  nfsstat.NFS_OK) 
			        throw new ApplicationException("NFSPROC_RMDIR_2: errorcode " + status);
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void DeleteFile(string FileFullName)
        {
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
            {
                string ParentDirectory = Path.GetDirectoryName(FileFullName);
                string FileName = Path.GetFileName(FileFullName);
                NFSAttributes ParentAttributes = GetItemAttributes(ParentDirectory);

                diropargs dpArgDelete = new diropargs();
                int status = 0;

                dpArgDelete.dir = new nfshandle(ParentAttributes.handle);
                dpArgDelete.name = new filename(FileName);
                if ((status = _ProtocolV2.NFSPROC_REMOVE_2(dpArgDelete)) != nfsstat.NFS_OK)
                    throw new ApplicationException("NFSPROC_REMOVE_2: errorcode " + status);
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void CreateFile(string FileFullName)
        {
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
            {
                string ParentDirectory = Path.GetDirectoryName(FileFullName);
                string FileName = Path.GetFileName(FileFullName);
                NFSAttributes ParentAttributes = GetItemAttributes(ParentDirectory);

                createargs dpArgCreate = new createargs();
                diropres pDirOpRes;
                dpArgCreate.attributes = new sattr();
                dpArgCreate.attributes.atime = new nfstimeval();
                dpArgCreate.attributes.atime.seconds = -1;
                dpArgCreate.attributes.atime.useconds = -1;
                dpArgCreate.attributes.mtime = new nfstimeval();
                dpArgCreate.attributes.mtime.seconds = -1;
                dpArgCreate.attributes.mtime.useconds = -1;
                /* Calculate Permission */
                byte userP = 7; byte groupP = 7; byte otherP = 7;
                int permission = 0;
                permission = (((int)userP) << 6) | (((int)groupP) << 3) | ((int)otherP);
                /*  ---  */
                dpArgCreate.attributes.mode = permission;
                dpArgCreate.attributes.gid = _GId;
                dpArgCreate.attributes.uid = _UId;
                dpArgCreate.attributes.size = -1;
                dpArgCreate.where = new diropargs();
                dpArgCreate.where.dir = new nfshandle(ParentAttributes.handle);
                dpArgCreate.where.name = new filename(FileName);
                if ((pDirOpRes = _ProtocolV2.NFSPROC_CREATE_2(dpArgCreate)) != null)
                {
                    if (pDirOpRes.status != nfsstat.NFS_OK)
                        throw new ApplicationException("NFSPROC_CREATE_2: errorcode " + pDirOpRes.status);
                }
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void Read(string FileFullName, long Offset, uint Count, ref byte[] Buffer, out int Size)
        {
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
            {
                if (_CurrentFile != FileFullName)
                {
                    NFSAttributes Attributes = GetItemAttributes(FileFullName);
                    _CurrentFileHandle = new nfshandle(Attributes.handle);
                    _CurrentFile = FileFullName;
                }

                readargs dpArgRead = new readargs();
		        readres pReadRes;
		        dpArgRead.file = _CurrentFileHandle;
		        dpArgRead.offset = (int) Offset;
		        dpArgRead.count = (int) Count;
		        if( (pReadRes = _ProtocolV2.NFSPROC_READ_2(dpArgRead)) != null )
                {
                    if (pReadRes.status != nfsstat.NFS_OK)
			            throw new ApplicationException("NFSPROC_READ_2: errorcode " + pReadRes.status);

                    Size = pReadRes.ok.data.Length;
                    Array.Copy(pReadRes.ok.data, Buffer, Size);
                }
                else
                    Size = -1;
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void SetFileSize(string FileFullName, ulong Size)
        {
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
            {
                NFSAttributes Attributes = GetItemAttributes(FileFullName);

                sattrargs dpArgSAttr = new sattrargs();
                attrstat pAttrStat;
                dpArgSAttr.attributes = new sattr();
                dpArgSAttr.attributes.atime = new nfstimeval();
                dpArgSAttr.attributes.atime.seconds = -1;
                dpArgSAttr.attributes.atime.useconds = -1;
                dpArgSAttr.attributes.gid = -1;
                dpArgSAttr.attributes.mode = -1;
                dpArgSAttr.attributes.mtime = new nfstimeval();
                dpArgSAttr.attributes.mtime.seconds = -1;
                dpArgSAttr.attributes.mtime.useconds = -1;
                dpArgSAttr.attributes.size = (int)Size;
                dpArgSAttr.attributes.uid = -1;

                if ((pAttrStat = _ProtocolV2.NFSPROC_SETATTR_2(dpArgSAttr)) != null)
                    if(pAttrStat.status != nfsstat.NFS_OK)
                        throw new ApplicationException("NFSPROC_SETATTR_2: errorcode " + pAttrStat.status);
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void Write(string FileFullName, long Offset, uint Count, byte[] Buffer, out int Size)
        {
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
            {
                if (_CurrentFile != FileFullName)
                {
                    NFSAttributes Attributes = GetItemAttributes(FileFullName);
                    _CurrentFileHandle = new nfshandle(Attributes.handle);
                    _CurrentFile = FileFullName;
                }

                writeargs dpArgWrite = new writeargs();
		        attrstat pAttrStat;
		        dpArgWrite.file = _CurrentFileHandle;
		        dpArgWrite.offset = (int) Offset;
		        dpArgWrite.data = Buffer;
                if ((pAttrStat = _ProtocolV2.NFSPROC_WRITE_2(dpArgWrite)) != null)
                {
                    if (pAttrStat.status != nfsstat.NFS_OK)
                        throw new ApplicationException("NFSPROC_WRITE_2: errorcode " + pAttrStat.status);

                    Size = pAttrStat.attributes.size;
                }
                else
                    Size = -1;
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public void Move(string OldDirectoryFullName, string OldFileName, string NewDirectoryFullName, string NewFileName)
        {
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
            {
                renameargs dpArgRename = new renameargs();
                int status = -1;

                NFSAttributes OldDirectory = GetItemAttributes(OldDirectoryFullName);
                NFSAttributes NewDirectory = GetItemAttributes(NewDirectoryFullName);

                dpArgRename.from = new diropargs();
                dpArgRename.from.dir = new nfshandle(OldDirectory.handle);
                dpArgRename.from.name = new filename(OldFileName);
                dpArgRename.to = new diropargs();
                dpArgRename.to.dir = new nfshandle(NewDirectory.handle);
                dpArgRename.to.name = new filename(NewFileName);

                if ((status = _ProtocolV2.NFSPROC_RENAME_2(dpArgRename)) != nfsstat.NFS_OK)
                        throw new ApplicationException("NFSPROC_WRITE_2: errorcode " + status);
            }
            else
                throw new ApplicationException("NFS Client not connected!");
        }

        public bool IsDirectory(string DirectoryFullName)
        {
            if (_ProtocolV2 != null && _MountProtocolV2 != null)
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
    }
}
