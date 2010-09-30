using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
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
        ulong CurrentSize;
        delegate void ShowProgressDelegate(bool ShowHide);
        ShowProgressDelegate show;
        delegate void UpdateProgressDelegate(string name, ulong total, int current);
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

        void UpdateProgress(string name, ulong total, int current)
        {
            if (pb.InvokeRequired)
            {
                pb.Invoke(update, new object[] { name, total, current });
            }
            else
            {
                lblCurrentFile.Text = CurrentItem;
                pb.Maximum = (int)total;
                int Value = pb.Value + current;
                if (Value < (int) total)
                    pb.Value += current;
                else
                    pb.Value =(int) total;
            }
        }

        void RefreshLocal(string Dir)
        {
            if (Dir == string.Empty)
                return;

            Environment.CurrentDirectory = tbLocalPath.Text = Dir;
            DirectoryInfo CurrentDirecotry = new DirectoryInfo(tbLocalPath.Text);
            listViewLocal.Items.Clear();
            foreach (FileInfo file in CurrentDirecotry.GetFiles())
            {
                ListViewItem lvi = new ListViewItem(new string[] { file.Name, file.Length.ToString(), file.LastWriteTime.ToString() });
                lvi.ImageIndex = 0;
                listViewLocal.Items.Add(lvi);
            }
        }

        void RefreshRemote()
        {
            listViewRemote.Items.Clear();
            foreach (string Item in nfsClient.GetItemList(RemoteFolder))
            {
                NFSAttributes nfsAttribute = nfsClient.GetItemAttributes(nfsClient.Combine(Item, RemoteFolder));
                if (nfsAttribute != null)
                {
                    if (nfsAttribute.type == NFSType.NFDIR)
                    {
                        ListViewItem lvi = new ListViewItem(new string[] { Item, nfsAttribute.size.ToString(), nfsAttribute.cdateTime.ToString() });
                        lvi.ImageIndex = 1;
                        listViewRemote.Items.Add(lvi);
                    }
                    else
                        if (nfsAttribute.type == NFSType.NFREG)
                        {
                            ListViewItem lvi = new ListViewItem(new string[] { Item, nfsAttribute.size.ToString(), nfsAttribute.cdateTime.ToString() });
                            lvi.ImageIndex = 0;
                            listViewRemote.Items.Add(lvi);
                        }
                }
                else
                {
                    ListViewItem lvi = new ListViewItem(new string[] { Item, "", "" });
                    lvi.ImageIndex = 0;
                    listViewRemote.Items.Add(lvi);
                }
            }
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
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (DialogResult.OK == fbd.ShowDialog())
            {
                RefreshLocal(LocalFolder = fbd.SelectedPath);
            }
        }

        private void cboxRemoteDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = cboxRemoteDevices.SelectedIndex;
            if (i != -1)
            {
                MountDevice(i);
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
                    nfsClient.DataEvent += new NFSDataEventHandler(nfsClient_DataEvent);
                    nfsClient.Connect(ipAddress, 0, 0, (int) nupTimeOut.Value);
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
                    string OutputFile = Path.Combine(LocalFolder, lvItem.Text);
                    if (File.Exists(OutputFile))
                    {
                        if (MessageBox.Show("Do you want to overwrite " + OutputFile + "?", "NFSClient", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            try
                            {
                                File.Delete(OutputFile);
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
                    CurrentSize = ulong.Parse(lvItem.SubItems[1].Text);
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
                    CurrentSize = ulong.Parse(lvItem.SubItems[1].Text);
                    string SourceName = Path.Combine(LocalFolder, CurrentItem);
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

        void nfsClient_DataEvent(object sender, NFSEventArgs e)
        {
            UpdateProgress(CurrentItem, CurrentSize, (int)e.Bytes);
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
                                File.Delete(Path.Combine(this.tbLocalPath.Text, lvi.Text));
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
                    nfsClient.CreateDirectory(nfsClient.Combine(nf.NewFolderName, RemoteFolder));
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
                nfsClient.Move(RemoteFolder, lvi.Text, RemoteFolder, NewLabel);
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
                File.Move(Path.Combine(Folder, lvi.Text), Path.Combine(Folder, NewLabel));
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
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            NFSClient.Properties.Settings.Default.DefaultFolder = tbLocalPath.Text;
            NFSClient.Properties.Settings.Default.ServerAddress = ipAddressControl1.Text;
            NFSClient.Properties.Settings.Default.Timeout = (int)nupTimeOut.Value;
            NFSClient.Properties.Settings.Default.DefaultProtocol = cboxVer.SelectedIndex;
            NFSClient.Properties.Settings.Default.Save();
        }

        #endregion
    }
}