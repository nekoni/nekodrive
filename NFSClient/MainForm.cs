using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Net.NetworkInformation;
using NFSLibrary;

namespace NFSClient
{
    public partial class MainForm : Form
    {
        #region Enum

        enum Columns
        {
            NAME,
            SIZE,
            DATE
        }

        #endregion

        #region Properties

        NFSLibrary.NFSClient nfsClient;
        List<string> nfsDevs = null;
        DragDropEffects CurrentEffect;
        List<ListViewItem> lvDragItem = new List<ListViewItem>();
        string CurrentList;
        string CurrentItem;
        long CurrentSize;
        delegate void ShowProgressDelegate(bool ShowHide);
        ShowProgressDelegate show;
        delegate void UpdateProgressDelegate(string name, long total, int current);
        UpdateProgressDelegate update;
        Thread downloadThread;
        Thread uploadThread;
        string LocalFolder = string.Empty;
        string RemoteFolder = ".";

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();
            cboxVer.SelectedIndex = 0;
            ipAddressControl1.Text = "192.168.56.3";
            show = new ShowProgressDelegate(ShowProgress);
            update = new UpdateProgressDelegate(UpdateProgress);
            btnNewFolder.Enabled = false;
        
            ShowProgress(false);
        }

        #endregion

        #region Methods

        void ShowProgress(bool Show)
        {
            if (pb.InvokeRequired)
            {
                pb.Invoke(show, new object[] { Show });
            }
            else
            {
                if (Show)
                {
                    pb.Show();
                    lblCurrentFile.Text = CurrentItem;
                    btnCancel.Show();
                    pnlMain.Enabled = false;
                    cboxVer.Enabled = false;
                    btnConnect.Enabled = false;
                    btnNewFolder.Enabled = false;
                    ipAddressControl1.Enabled = false;
                    nupTimeOut.Enabled = false;
                }
                else
                {
                    pb.Hide();
                    lblCurrentFile.Text = CurrentItem = string.Empty;
                    pb.Value = 0;
                    btnCancel.Hide();
                    pnlMain.Enabled = true;
                    cboxVer.Enabled = true;
                    btnConnect.Enabled = true;
                    btnNewFolder.Enabled = true;
                    ipAddressControl1.Enabled = true;
                    nupTimeOut.Enabled = true;
                    if (CurrentList != "Local")
                        RefreshLocal(LocalFolder);
                    else
                        RefreshRemote();
                }
            }
        }

        long _lTotal = 0;
        void UpdateProgress(string name, long total, int current)
        {
            if (pb.InvokeRequired)
            {
                pb.Invoke(update, new object[] { name, total, current });
            }
            else
            {
                if (current > 0)
                {
                    if (_lTotal == 0)
                    { _lTotal = total; }
                    _lTotal -= current;

                    lblCurrentFile.Text = CurrentItem;
                    pb.Maximum = (int)(total / current);
                    int Value = pb.Value + 1;
                    if (Value < pb.Maximum)
                        pb.Value = Value;
                    else
                        pb.Value = pb.Maximum;
                }
            }
        }

        void RefreshLocal(string Dir)
        {
            if (Dir == string.Empty)
                return;

            Environment.CurrentDirectory = tbLocalPath.Text = Dir;
            System.IO.DirectoryInfo CurrentDirecotry = new System.IO.DirectoryInfo(tbLocalPath.Text);
            listViewLocal.Items.Clear();
            foreach (System.IO.FileInfo file in CurrentDirecotry.GetFiles())
            {
                ListViewItem lvi = new ListViewItem(new string[] { file.Name, file.Length.ToString(), file.LastWriteTime.ToString() });
                lvi.ImageIndex = 0;
                listViewLocal.Items.Add(lvi);
            }
        }

        void RefreshRemote()
        {
            listViewRemote.Items.Clear();
            List<string> Items = nfsClient.GetItemList(RemoteFolder);
            Items.Remove(".");
            Items.Remove("..");
            Items.Insert(0, "..");
            List<ListViewItem> ItemsList = new List<ListViewItem>();
            foreach (string Item in Items)
            {
                NFSLibrary.Protocols.Commons.NFSAttributes nfsAttribute = nfsClient.GetItemAttributes(nfsClient.Combine(Item, RemoteFolder));
                if (nfsAttribute != null)
                {
                    if (nfsAttribute.NFSType == NFSLibrary.Protocols.Commons.NFSItemTypes.NFDIR)
                    {
                        ListViewItem lvi = new ListViewItem(new string[] { Item, nfsAttribute.Size.ToString(), nfsAttribute.CreateDateTime.ToString() });
                        lvi.ImageIndex = 1;
                        ItemsList.Add(lvi);
                    }
                    else
                        if (nfsAttribute.NFSType == NFSLibrary.Protocols.Commons.NFSItemTypes.NFREG)
                        {
                            ListViewItem lvi = new ListViewItem(new string[] { Item, nfsAttribute.Size.ToString(), nfsAttribute.CreateDateTime.ToString(), nfsAttribute.ModifiedDateTime.ToString(), nfsAttribute.LastAccessedDateTime.ToString() });
                            lvi.ImageIndex = 0;
                            ItemsList.Add(lvi);
                        }
                }
                else
                {
                    ListViewItem lvi = new ListViewItem(new string[] { Item, "", "" });
                    lvi.ImageIndex = 0;
                    ItemsList.Add(lvi);
                }
            }

            List<ListViewItem> OrderedList = new List<ListViewItem>();
            foreach (ListViewItem lvi in ItemsList)
            {
                if (lvi.Text == "..")
                {
                    OrderedList.Add(lvi);
                    break;
                }
            }

            foreach (ListViewItem lvi in ItemsList)
            {
                if(lvi.ImageIndex == 1 && lvi.Text != "..")
                    OrderedList.Add(lvi);
            }

            foreach (ListViewItem lvi in ItemsList)
            {
                if (lvi.ImageIndex == 0)
                    OrderedList.Add(lvi);
            }

            listViewRemote.Items.AddRange(OrderedList.ToArray());
        }

        void MountDevice(int i)
        {
            btnNewFolder.Enabled = true;
            listViewRemote.Items.Clear();
            nfsClient.MountDevice(nfsDevs[i]);
            RefreshRemote();
        }

        public bool PingServer(IPAddress Ip)
        {
            //ping the server
            Ping pingSender = new Ping();
            PingOptions pingOptions = new PingOptions();
            pingOptions.DontFragment = true;
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 5000;
            PingReply reply = pingSender.Send(Ip, timeout, buffer, pingOptions);
            if (reply.Status == IPStatus.Success)
                return true;
            else
                return false;
        }

        #endregion

        #region Local Event

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (DialogResult.OK == fbd.ShowDialog())
                {
                    RefreshLocal(LocalFolder = fbd.SelectedPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
                pnlMain.Enabled = false;
            }
        }

        private void cboxRemoteDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = cboxRemoteDevices.SelectedIndex;
                if (i != -1)
                {
                    MountDevice(i);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
                pnlMain.Enabled = false;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ipAddress = new IPAddress(ipAddressControl1.GetAddressBytes());
                if (PingServer(ipAddress))
                {
                    NFSLibrary.NFSClient.NFSVersion ver = NFSLibrary.NFSClient.NFSVersion.v2;
                    if (cboxVer.SelectedItem.ToString() == "V3")
                        ver = NFSLibrary.NFSClient.NFSVersion.v3;

                    nfsClient = new NFSLibrary.NFSClient(ver);
                    nfsClient.DataEvent += new NFSLibrary.NFSClient.NFSDataEventHandler(nfsClient_DataEvent);
                    Encoding encoding = Encoding.ASCII;
                    if (chkUseUnicode.Checked)
                        encoding = Encoding.UTF8;
                    nfsClient.Connect(ipAddress, 1000, 1000, (int) nupTimeOut.Value * 1000, encoding, chkUseSecurePort.Checked);
                    nfsDevs = nfsClient.GetExportedDevices();
                    cboxRemoteDevices.Items.Clear();
                    foreach (string nfsdev in nfsDevs)
                        cboxRemoteDevices.Items.Add(nfsdev);
                    RefreshLocal(Environment.CurrentDirectory);
                    listViewRemote.Items.Clear();
                    pnlMain.Enabled = true;
                    
                }
                else
                    throw new Exception("Server not found!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
                pnlMain.Enabled = false;
            }
        }

        private void listViewLocal_ItemDrag(object sender, ItemDragEventArgs e)
        {
            CurrentList = "Local";
            CurrentEffect = DragDropEffects.Copy;
            ListView.SelectedListViewItemCollection sl = listViewLocal.SelectedItems;
            Bitmap bmp = (Bitmap)listViewLocal.SmallImageList.Images[sl[0].ImageIndex];
            lvDragItem.Clear();
            foreach (ListViewItem lvi in sl)
            {
                lvDragItem.Add(lvi);
            }
            this.DoDragDrop(bmp, CurrentEffect);
        }

        private void listViewLocal_DragDrop(object sender, DragEventArgs e)
        {
            if (CurrentList != "Local")
            {
                if (CurrentEffect == DragDropEffects.Copy)
                {
                    LocalFolder = tbLocalPath.Text;
                    downloadThread = new Thread(new ThreadStart(Download));
                    downloadThread.Start();
                }
            }
        }

        void Download()
        {
            try
            {
                ShowProgress(true);
                foreach (ListViewItem lvItem in lvDragItem)
                {
                    string OutputFile = System.IO.Path.Combine(LocalFolder, lvItem.Text);
                    if (System.IO.File.Exists(OutputFile))
                    {
                        if (MessageBox.Show("Do you want to overwrite " + OutputFile + "?", "NFSClient", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            try
                            {
                                System.IO.File.Delete(OutputFile);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("An error has occurred deleting the file (" + ex.Message + ")", "NFS Client", MessageBoxButtons.OK);
                                continue;
                            }
                        }
                        else
                            continue;
                    }
                    CurrentItem = lvItem.Text;
                    CurrentSize = long.Parse(lvItem.SubItems[1].Text);
                    _lTotal = 0;
                    nfsClient.Read(nfsClient.Combine(CurrentItem, RemoteFolder), OutputFile);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
                ShowProgress(false);
            }
            finally
            {
                ShowProgress(false);
            }
        }

        void Upload()
        {
            try
            {
                ShowProgress(true);
                foreach (ListViewItem lvItem in lvDragItem)
                {
                    if (nfsClient.FileExists(nfsClient.Combine(lvItem.Text, RemoteFolder)))
                    {
                        if (MessageBox.Show("Do you want to overwrite " + lvItem.Text + "?", "NFSClient", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            nfsClient.DeleteFile(nfsClient.Combine(lvItem.Text, RemoteFolder));
                        }
                        else
                            continue;
                    }
                    CurrentItem = lvItem.Text;
                    CurrentSize = long.Parse(lvItem.SubItems[1].Text);
                    _lTotal = 0;
                    string SourceName = System.IO.Path.Combine(LocalFolder, CurrentItem);
                    nfsClient.Write(nfsClient.Combine(CurrentItem, RemoteFolder), SourceName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
                ShowProgress(false);
            }
            finally
            {
                ShowProgress(false);
            }
        }

        void nfsClient_DataEvent(object sender, NFSLibrary.NFSClient.NFSEventArgs e)
        {
            UpdateProgress(CurrentItem, CurrentSize, e.Bytes);
        }

        private void listViewLocal_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = CurrentEffect;
        }

        private void listViewLocal_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = CurrentEffect;
        }

        private void listViewRemote_ItemDrag(object sender, ItemDragEventArgs e)
        {
            CurrentList = "Remote";
            Bitmap bmp = (Bitmap)listViewRemote.SmallImageList.Images[(int)0];
            CurrentEffect = DragDropEffects.Copy;
            ListView.SelectedListViewItemCollection sl = listViewRemote.SelectedItems;
            lvDragItem.Clear();
            foreach (ListViewItem lvi in sl)
            {
                lvDragItem.Add(lvi);
            }
            this.DoDragDrop(bmp, CurrentEffect);
        }

        private void listViewRemote_DragDrop(object sender, DragEventArgs e)
        {
            if (CurrentList != "Remote")
            {
                if (CurrentEffect == DragDropEffects.Copy)
                {
                    uploadThread = new Thread(new ThreadStart(Upload));
                    uploadThread.Start();
                }
            }
        }

        private void listViewRemote_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = CurrentEffect;
        }

        private void listViewRemote_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = CurrentEffect;
        }

        private void listViewRemote_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (listViewRemote.SelectedItems != null)
                    {
                        foreach (ListViewItem lvi in listViewRemote.SelectedItems)
                        {
                            if (MessageBox.Show("Do you really want to delete " + lvi.Text + " ?", "NFS Client", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (lvi.ImageIndex == 0)
                                {
                                    nfsClient.DeleteFile(nfsClient.Combine(lvi.Text, RemoteFolder));
                                }
                                else
                                    nfsClient.DeleteDirectory(nfsClient.Combine(lvi.Text, RemoteFolder));
                            }
                        }
                        RefreshRemote();
                    }
                }
                else if (e.KeyCode == Keys.F5)
                    RefreshRemote();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (nfsClient != null)
            {
                nfsClient.UnMountDevice();
                nfsClient.Disconnect();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (uploadThread != null && uploadThread.IsAlive)
                uploadThread.Abort();
            if (downloadThread != null && downloadThread.IsAlive)
                downloadThread.Abort();
            ShowProgress(false);
        }

        private void listViewLocal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (listViewLocal.SelectedItems != null)
                    {
                        foreach (ListViewItem lvi in listViewLocal.SelectedItems)
                        {
                            if (MessageBox.Show("Do you really want to delete " + lvi.Text + " ?", "NFS Client", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                System.IO.File.Delete(System.IO.Path.Combine(this.tbLocalPath.Text, lvi.Text));
                        }
                        RefreshLocal(tbLocalPath.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
            }
        }

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (nfsClient == null)
                    return;

                NewFolder nf = new NewFolder();
                if (nf.ShowDialog() == DialogResult.OK)
                {
                    byte UserP, GroupP, OtherP;

                    switch(nf.userPSelectedIndex)
                    {
                        case 0: UserP = 4; break;
                        case 1: UserP = 6; break;
                        case 2: UserP = 7; break;
                        default: UserP = 7; break;
                    }

                    switch(nf.groupPSelectedIndex)
                    {
                        case 0: GroupP = 4; break;
                        case 1: GroupP = 6; break;
                        case 2: GroupP = 7; break;
                        default: GroupP = 7; break;
                    }

                    switch(nf.otherPSelectedIndex)
                    {
                        case 0: OtherP = 4; break;
                        case 1: OtherP = 6; break;
                        case 2: OtherP = 7; break;
                        default: OtherP = 7; break;
                    }

                    nfsClient.CreateDirectory( 
                        nfsClient.Combine(nf.NewFolderName, RemoteFolder),
                        new NFSLibrary.Protocols.Commons.NFSPermission(UserP, GroupP, OtherP));
                    RefreshRemote();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
            }
        }

        private void listViewRemote_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                string NewLabel = e.Label;
                ListViewItem lvi = listViewRemote.Items[e.Item];
                nfsClient.Move(
                    nfsClient.Combine(lvi.Text, RemoteFolder),
                    nfsClient.Combine(NewLabel, RemoteFolder)
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
            }
        }

        private void listViewRemote_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (listViewRemote.SelectedItems != null)
                {
                    ListViewItem lvi = listViewRemote.SelectedItems[0];
                    if (lvi.ImageIndex == 1)
                    {
                        if (lvi.Text == ".")
                            RefreshRemote();
                        if (lvi.Text == "..")
                            RemoteFolder = nfsClient.GetDirectoryName(RemoteFolder);
                        else
                            RemoteFolder = nfsClient.Combine(lvi.Text, RemoteFolder);
                        RefreshRemote();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
            }
        }

        private void listViewLocal_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                string NewLabel = e.Label;
                ListViewItem lvi = listViewLocal.Items[e.Item];
                string Folder = tbLocalPath.Text;
                System.IO.File.Move(System.IO.Path.Combine(Folder, lvi.Text), System.IO.Path.Combine(Folder, NewLabel));
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occurred renaming the item (" + ex.ToString() + ")", "NFS Client", MessageBoxButtons.OK);
                e.CancelEdit = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tbLocalPath.Text = NFSClient.Properties.Settings.Default.DefaultFolder;
            ipAddressControl1.Text = NFSClient.Properties.Settings.Default.ServerAddress;
            nupTimeOut.Value = (Decimal) NFSClient.Properties.Settings.Default.Timeout;
            cboxVer.SelectedIndex = NFSClient.Properties.Settings.Default.DefaultProtocol;
            chkUseUnicode.Checked = NFSClient.Properties.Settings.Default.UseUnicode;
            chkUseSecurePort.Checked = NFSClient.Properties.Settings.Default.UseSecurePort;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            NFSClient.Properties.Settings.Default.DefaultFolder = tbLocalPath.Text;
            NFSClient.Properties.Settings.Default.ServerAddress = ipAddressControl1.Text;
            NFSClient.Properties.Settings.Default.Timeout = (int)nupTimeOut.Value;
            NFSClient.Properties.Settings.Default.DefaultProtocol = cboxVer.SelectedIndex;
            NFSClient.Properties.Settings.Default.UseUnicode = chkUseUnicode.Checked;
            NFSClient.Properties.Settings.Default.UseSecurePort = chkUseSecurePort.Checked;
            NFSClient.Properties.Settings.Default.Save();
        }

        private void showPermissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewRemote.SelectedItems != null)
                {
                    ListViewItem lvi = listViewRemote.SelectedItems[0];

                    String SearchItem = nfsClient.Combine(lvi.Text, RemoteFolder);
                    NFSLibrary.Protocols.Commons.NFSAttributes itemAttributes =
                        nfsClient.GetItemAttributes(SearchItem);

                    MessageBox.Show(
                        String.Format("Mode: {0}{1}{2}", itemAttributes.Mode.UserAccess, itemAttributes.Mode.GroupAccess, itemAttributes.Mode.OtherAccess)
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFS Client");
            }
        }

        #endregion

    }
}