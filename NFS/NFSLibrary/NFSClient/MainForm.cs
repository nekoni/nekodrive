using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NekoDrive.NFS.Wrappers;
using NekoDrive.NFS;
using System.Net;
using System.Threading;

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

        INFS nfsClient;
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
            foreach (string Item in nfsClient.GetItemList())
            {
                NFSAttributes nfsAttribute = nfsClient.GetItemAttributes(Item);
                if (nfsAttribute.type == NFSType.NFDIR)
                {
                    ListViewItem lvi = new ListViewItem(new string[] { Item, nfsAttribute.size.ToString(), nfsAttribute.dateTime.ToString() });
                    lvi.ImageIndex = 1;
                    listViewRemote.Items.Add(lvi);
                }
                else
                    if (nfsAttribute.type == NFSType.NFREG)
                    {
                        ListViewItem lvi = new ListViewItem(new string[] { Item, nfsAttribute.size.ToString(), nfsAttribute.dateTime.ToString() });
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
                NFS.NFSVersion ver = NFS.NFSVersion.v2;
                if (cboxVer.SelectedItem.ToString() == "V3")
                    ver = NFS.NFSVersion.v3;

                nfsClient = NFS.GetNFS(new IPAddress(ipAddressControl1.GetAddressBytes()), ver);
                nfsClient.DataEvent += new NFSDataEventHandler(nfsClient_DataEvent);
                if (nfsClient.Connect() == NFSResult.NFS_SUCCESS)
                {
                    nfsDevs = nfsClient.GetExportedDevices();
                    cboxRemoteDevices.Items.Clear();
                    foreach (string nfsdev in nfsDevs)
                        cboxRemoteDevices.Items.Add(nfsdev);
                    RefreshLocal(Environment.CurrentDirectory);
                    listViewRemote.Items.Clear();
                    pnlMain.Enabled = true;
                }
                else
                    throw new ApplicationException("Connection error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "NFSv2 Client");
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
            foreach (ListViewItem lvItem in lvDragItem)
            {
                string OutputFile = Path.Combine(LocalFolder, lvItem.Text);
                if (File.Exists(OutputFile))
                {
                    if (MessageBox.Show("Do you want to overwrite " + OutputFile + "?", "NFSClient", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        File.Delete(OutputFile);
                    else
                        continue;
                }
                ShowProgress(true);
                CurrentItem = lvItem.Text;
                CurrentSize = ulong.Parse(lvItem.SubItems[1].Text);
                if (nfsClient.Read(CurrentItem, OutputFile) != NFSResult.NFS_SUCCESS)
                {
                    MessageBox.Show("An error has occurred while downloading " + CurrentItem);
                    continue;
                }
                ShowProgress(false);
            }   
        }

        void Upload()
        {
            foreach (ListViewItem lvItem in lvDragItem)
            {
                if (nfsClient.FileExists(lvItem.Text))
                {
                    if (MessageBox.Show("Do you want to overwrite " + lvItem.Text + "?", "NFSClient", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        nfsClient.DeleteFile(lvItem.Text);
                    else
                        continue;
                }
                ShowProgress(true);
                CurrentItem = lvItem.Text;
                CurrentSize = ulong.Parse(lvItem.SubItems[1].Text);
                string OutpuFileName = Path.Combine(LocalFolder, CurrentItem);
                if (nfsClient.Write(CurrentItem, OutpuFileName) != NFSResult.NFS_SUCCESS)
                {
                    MessageBox.Show("An error has occurred while uploading " + CurrentItem);
                    continue;
                }
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
            if (e.KeyCode == Keys.Delete)
            {
                if (listViewRemote.SelectedItems != null)
                {
                    foreach (ListViewItem lvi in listViewRemote.SelectedItems)
                    {
                        if (MessageBox.Show("Do you really want to delete " + lvi.Text + " ?", "NFS Client", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (lvi.ImageIndex == 0)
                                nfsClient.DeleteFile(lvi.Text);
                            else
                                nfsClient.DeleteDirectory(lvi.Text);
                        }
                    }
                    RefreshRemote();
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (nfsClient != null)
            {
                nfsClient.UnMountDevice();
                nfsClient.Disconnect();
                nfsClient.Dispose();
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

        #endregion

        private void btnNewFolder_Click(object sender, EventArgs e)
        {
            if (nfsClient == null)
                return;

            NewFolder nf = new NewFolder();
            if (nf.ShowDialog() == DialogResult.OK)
            {
                if (nfsClient.CreateDirectory(nf.NewFolderName) == NFSResult.NFS_SUCCESS)
                {
                    RefreshRemote();
                }
                else
                    MessageBox.Show("An error has occurred creting the directory", "NFS Client", MessageBoxButtons.OK);
            }
        }

        private void listViewRemote_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            string NewLabel = e.Label;
            ListViewItem lvi = listViewRemote.Items[e.Item];
            if (nfsClient.Rename(lvi.Text, NewLabel) != NFSResult.NFS_SUCCESS)
            {
                MessageBox.Show("An error has occurred renaming the directory", "NFS Client", MessageBoxButtons.OK);
            }   
        }

        private void listViewRemote_DoubleClick(object sender, EventArgs e)
        {
            if (listViewRemote.SelectedItems != null)
            {
                ListViewItem lvi = listViewRemote.SelectedItems[0];
                if (lvi.ImageIndex == 1)
                {
                    if(nfsClient.ChangeCurrentDirectory(lvi.Text) == NFSResult.NFS_SUCCESS)
                        RefreshRemote();
                }
            }
        }
    }
}