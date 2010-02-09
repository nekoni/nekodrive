using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NekoDrive.NFS.Wrappers;

namespace NFSv2Client
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

        NFSv2 nfsClient;
        List<string> nfsDevs = null;
        DragDropEffects CurrentEffect;
        List<ListViewItem> lvDragItem = new List<ListViewItem>();
        string CurrentList;
        string CurrentItem;
        Progress pg = new Progress();

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();

            ipAddressControl1.Text = "161.55.201.250";
            pg.Show();
            pg.Hide();

            cboxVer.SelectedIndex = 0;
        }

        #endregion

        #region Methods

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
                nfsClient = new NFSv2(ipAddressControl1.Text);
                nfsClient.DataEvent += new NFSDataEventHandler(nfsClient_DataEvent);
                if (nfsClient.Connect() == NFSResult.NFS_SUCCESS)
                {
                    nfsDevs = nfsClient.GetExportedDevices();
                    cboxRemoteDevices.Items.Clear();
                    foreach (string nfsdev in nfsDevs)
                        cboxRemoteDevices.Items.Add(nfsdev);
                    //MessageBox.Show("Connected to the target.", "NFSv2 Client", MessageBoxButtons.OK);
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
                    foreach (ListViewItem lvItem in lvDragItem)
                    {
                        pg.Visible = true;
                        CurrentItem = lvItem.Text;
                        FileStream fs = new FileStream(Path.Combine(tbLocalPath.Text, lvItem.Text), FileMode.CreateNew);
                        if (nfsClient.Read(lvItem.Text, ref fs) != NFSResult.NFS_SUCCESS)
                            Console.WriteLine("Read error");
                        fs.Close();
                        pg.Visible = false;
                        //nfsClient.DownloadFile(lvItem.Text, Path.Combine(tbLocalPath.Text, lvItem.Text));
                    }
                    RefreshLocal(tbLocalPath.Text);
                }
            }
        }

        void nfsClient_DataEvent(object sender, NFSEventArgs e)
        {
            pg.Update(CurrentItem, e.CurrentByte, e.TotalBytes);
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