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
        Progress pg = new Progress();
        delegate void ShowProgressDelegate(bool ShowHide);
        ShowProgressDelegate show;
        ulong Size;
        uint CurrentPos;

        delegate void UpdateProgressDelegate(string name, int total, int current);
        UpdateProgressDelegate update;

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();
            cboxVer.SelectedIndex = 0;
            show = new ShowProgressDelegate(ShowProgress);
            update = new UpdateProgressDelegate(UpdateProgress);
        }

        #endregion

        #region Methods

        void ShowProgress(bool Show)
        {
            if (this.InvokeRequired)
            {
                pg.Invoke(show, new object[] { Show });
            }
            else
            {
                if (Show)
                    pg.ShowDialog();
                else
                    pg.Close();
            }
        }

        void UpdateProgress(string name, int total, int current)
        {
            if (this.InvokeRequired)
            {
                pg.Invoke(show, new object[] { name, total, current });
            }
            else
            {
                pg.Update(name, (uint) current, (uint) total);
            }
        }

        void RefreshLocal(string Dir)
        {
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

        void RefreshRemote(int i)
        {
            listViewRemote.Items.Clear();
            nfsClient.MountDevice(nfsDevs[i]);
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

        #endregion

        #region Local Event

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (DialogResult.OK == fbd.ShowDialog())
            {
                RefreshLocal(fbd.SelectedPath);
            }
        }

        private void cboxRemoteDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = cboxRemoteDevices.SelectedIndex;
            if (i != -1)
            {
                RefreshRemote(i);
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
                    string LocalFolder = tbLocalPath.Text;
                    ThreadPool.QueueUserWorkItem(new
                    WaitCallback(delegate
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
                            if(nfsClient.Read(CurrentItem, OutputFile) != NFSResult.NFS_SUCCESS)
                            {
                                MessageBox.Show("An error has occurred while downloading " + CurrentItem);
                                continue;
                            }
                            ShowProgress(false);
                            
                        }   
                    }));
                    pg.ShowDialog();
                    
                    RefreshLocal(tbLocalPath.Text);
                }
            }
        }

        void nfsClient_DataEvent(object sender, NFSEventArgs e)
        {
            
            //pg.Update(CurrentItem, e.CurrentByte, e.TotalBytes);
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
                    foreach (ListViewItem lvItems in lvDragItem)
                    {
                        FileStream wfs = new FileStream(Path.Combine(tbLocalPath.Text, lvItems.Text), FileMode.Open, FileAccess.Read);
                        if (nfsClient.Write(Path.GetFileName(Path.Combine(tbLocalPath.Text, lvItems.Text)), wfs) != NFSResult.NFS_SUCCESS)
                            Console.WriteLine("Write error");
                        wfs.Close();
                        //nfsClient.UploadFile(Path.Combine(tbLocalPath.Text, lvItems.Text));
                    }
                    RefreshRemote(cboxRemoteDevices.SelectedIndex);
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
                        if(MessageBox.Show("Do you really want to delete " + lvi.Text + " ?", "NFSv3 Client", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            nfsClient.DeleteFile(lvi.Text);
                    }
                    RefreshRemote(cboxRemoteDevices.SelectedIndex);
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
        #endregion
       
    }
}