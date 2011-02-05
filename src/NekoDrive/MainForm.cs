using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.Management;
using Dokan;
using System.Threading;
using System.Diagnostics;
using NFSLibrary;
using NekoDrive.NFS;
using System.IO;

namespace NekoDrive
{
    public partial class MainForm : Form
    {
        #region Fields

        private static MainForm mInstance;
        private string MountPoint;

        #endregion

        #region Properites

        public NFSClient mNFS = null;
        public DokanNet mDokanNet = null;
        public bool DebugMode = false;

        public static MainForm In
        {
            get
            {
                return mInstance;
            }
        }

        #endregion

        #region Constructor

        public MainForm()
        {
            InitializeComponent();

            mInstance = this;
        }

        #endregion

        #region Methods

        private bool PingServer(IPAddress Ip)
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

        private void UnmountDrive()
        {
            if (mNFS == null)
                throw new ApplicationException("NFS object is null!");

            mNFS.UnMountDevice();
            if (rbDisk.Checked)
                DokanNet.DokanUnmount(MountPoint.ToCharArray()[0]);
            else
                DokanNet.DokanRemoveMountPoint(MountPoint);
            cboxLocalDrive.Enabled = true;
            cboxRemoteDevices.Enabled = true;
            btnMount.Enabled = true;
            btnUnmount.Enabled = false;
            tbDriveLabel.Enabled = true;
            rbDisk.Enabled = true;
            rbFolder.Enabled = true;
            btnSelectFolder.Enabled = true;
            chkAutoMount.Enabled = true;
        }

        private void MountDrive()
        {
            if (mNFS == null)
                throw new ApplicationException("NFS object is null!");

            string strDev = (string)cboxRemoteDevices.SelectedItem;
            char cDrive = ((string)cboxLocalDrive.SelectedItem).ToCharArray()[0];
            MountPoint = String.Format(@"{0}:\", cDrive);
            string Folder = btnSelectFolder.Text;
            bool DiskOrFolder = rbDisk.Checked;
            string strDriveLabel = tbDriveLabel.Text;
            MainForm.In.mNFS.MountDevice(strDev);
            cboxLocalDrive.Enabled = false;
            cboxRemoteDevices.Enabled = false;
            btnMount.Enabled = false;
            btnUnmount.Enabled = true;
            tbDriveLabel.Enabled = false;
            rbDisk.Enabled = false;
            rbFolder.Enabled = false;
            btnSelectFolder.Enabled = false;
            chkAutoMount.Enabled = false;

            if (!DiskOrFolder)
            {
                if (!Directory.Exists(Folder))
                    throw new ApplicationException(String.Format("{0} not found.", Folder));
                else
                    MountPoint = Folder;
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(
                delegate
                {
                    try
                    {
                        System.IO.Directory.SetCurrentDirectory(Application.StartupPath);
                        DokanOptions dokanOptions = new DokanOptions();
                        dokanOptions.DebugMode = DebugMode;
                        dokanOptions.NetworkDrive = DiskOrFolder;
                        dokanOptions.MountPoint = MountPoint;
                        dokanOptions.UseKeepAlive = true;
                        dokanOptions.UseAltStream = true;
                        dokanOptions.VolumeLabel = strDriveLabel;
                        dokanOptions.ThreadCount = 1;
                        Operations nfsOperations = new Operations();
                        CacheOperations cacheOperations = new CacheOperations(nfsOperations);
                        int status = DokanNet.DokanMain(dokanOptions, cacheOperations);
                        switch (status)
                        {
                            case DokanNet.DOKAN_DRIVE_LETTER_ERROR:
                                throw new ApplicationException("Drvie letter error");
                            case DokanNet.DOKAN_DRIVER_INSTALL_ERROR:
                                throw new ApplicationException("Driver install error");
                            case DokanNet.DOKAN_MOUNT_ERROR:
                                throw new ApplicationException("Mount error");
                            case DokanNet.DOKAN_START_ERROR:
                                throw new ApplicationException("Start error");
                            case DokanNet.DOKAN_ERROR:
                                throw new ApplicationException("Unknown error");
                            case DokanNet.DOKAN_SUCCESS:
                                break;
                            default:
                                throw new ApplicationException("Unknown status: " + status);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }));

            ThreadPool.QueueUserWorkItem(new WaitCallback(
                delegate
                {
                    Thread.Sleep(2000);
                    Process.Start("explorer.exe", " " + MountPoint);
                }));
        }

        private void Connect()
        {
            IPAddress ipAddress = new IPAddress(ipAddressControl1.GetAddressBytes());
            if (PingServer(ipAddress))
            {
                NFSClient.NFSVersion ver = NFSClient.NFSVersion.v2;
                if (cboxVer.SelectedItem.ToString() == "V3")
                    ver = NFSClient.NFSVersion.v3;

                mNFS = new NFSClient(ver);
                int UserId = int.Parse(tbUserId.Text);
                int GroupId = int.Parse(tbGroupId.Text);
                Encoding encoding = Encoding.ASCII;
                if (chkUnicode.Checked)
                    encoding = Encoding.UTF8;
                mNFS.Connect(ipAddress, UserId, GroupId, (int)nupTimeOut.Value, encoding);
                cboxRemoteDevices.Items.Clear();
                foreach (string strDev in mNFS.GetExportedDevices())
                    cboxRemoteDevices.Items.Add(strDev);
                if (cboxRemoteDevices.Items.Count > 0)
                    gboxMount.Enabled = true;
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                ipAddressControl1.Enabled = false;
                cboxVer.Enabled = false;
                tbGroupId.Enabled = false;
                tbUserId.Enabled = false;
                nupTimeOut.Enabled = false;
                chkUnicode.Enabled = false;
                chkAutoConnect.Enabled = false;

                if (cboxLocalDrive.Items.Count > NekoDrive.Properties.Settings.Default.DriveLetter)
                    cboxLocalDrive.SelectedIndex = NekoDrive.Properties.Settings.Default.DriveLetter;

                if (cboxRemoteDevices.Items.Count > NekoDrive.Properties.Settings.Default.RemoteDevice)
                    cboxRemoteDevices.SelectedIndex = NekoDrive.Properties.Settings.Default.RemoteDevice;

                chkAutoMount.Checked = NekoDrive.Properties.Settings.Default.AutoMount;
                tbDriveLabel.Text = NekoDrive.Properties.Settings.Default.DriveLabel;
                chkUnicode.Checked = NekoDrive.Properties.Settings.Default.Unicode;

                if (chkAutoMount.Checked)
                {
                    this.WindowState = FormWindowState.Minimized;
                    MountDrive();
                }
            }
            else
                throw new ApplicationException("Server not found!");
        }

        private void Disconnect()
        {
            if (mNFS == null)
                throw new ApplicationException("NFS object is null!");

            if (mNFS.IsMounted)
                UnmountDrive();

            mNFS.Disconnect();
            gboxMount.Enabled = false;
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            ipAddressControl1.Enabled = true;
            cboxVer.Enabled = true;
            tbGroupId.Enabled = true;
            tbUserId.Enabled = true;
            nupTimeOut.Enabled = true;
            chkUnicode.Enabled = true;
            chkAutoConnect.Enabled = true;
        }

        private List<string> GetDriveLetters()
        {
            List<string> DriveLetters = new List<string>();
            SelectQuery query = new SelectQuery( "select name from win32_logicaldisk" );
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach( ManagementObject mo in searcher.Get() )
                DriveLetters.Add(mo["name"].ToString().ToUpper().Replace(":",""));

            return DriveLetters;
        }

        private void InitializeForm()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text += " " + version.Major + "." + version.Minor + "." + version.Revision;
            cboxLocalDrive.Items.Clear();
            List<string> DriveLetters = GetDriveLetters();
            char c = 'D';
            while (c <= 'Z')
            {
               if(!DriveLetters.Contains(c.ToString()))
                   cboxLocalDrive.Items.Add(c.ToString());
                c++;
            }

            ipAddressControl1.Text = NekoDrive.Properties.Settings.Default.ServerAddress;
            nupTimeOut.Value = (Decimal)NekoDrive.Properties.Settings.Default.Timeout;
            cboxVer.SelectedIndex = NekoDrive.Properties.Settings.Default.DefaultProtocol;
            tbUserId.Text = NekoDrive.Properties.Settings.Default.UserId.ToString();
            tbGroupId.Text = NekoDrive.Properties.Settings.Default.GroupId.ToString();
            chkAutoConnect.Checked = NekoDrive.Properties.Settings.Default.AutoConnect;
            btnSelectFolder.Text = NekoDrive.Properties.Settings.Default.Folder;
            bool DiskOrFolder = NekoDrive.Properties.Settings.Default.DiskOrFolder;

            if (DiskOrFolder)
                rbDisk.Checked = true;
            else
                rbFolder.Checked = true;

            gboxMount.Enabled = false;
            btnDisconnect.Enabled = false;
            btnUnmount.Enabled = false;

            if (chkAutoConnect.Checked)
                Connect();
        }

        #endregion

        #region Local Events

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                Connect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMount_Click(object sender, EventArgs e)
        {
            try
            {
                MountDrive();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUnmount_Click(object sender, EventArgs e)
        {
            try
            {
                UnmountDrive();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
      
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (mNFS != null)
                {
                    if (mNFS.IsMounted)
                        UnmountDrive();

                    if (mNFS.IsConnected)
                        Disconnect();
                }

                NekoDrive.Properties.Settings.Default.ServerAddress = ipAddressControl1.Text;
                NekoDrive.Properties.Settings.Default.Timeout = (int)nupTimeOut.Value;
                NekoDrive.Properties.Settings.Default.DefaultProtocol = cboxVer.SelectedIndex;
                NekoDrive.Properties.Settings.Default.UserId = int.Parse(tbUserId.Text);
                NekoDrive.Properties.Settings.Default.GroupId = int.Parse(tbGroupId.Text);
                NekoDrive.Properties.Settings.Default.AutoConnect = chkAutoConnect.Checked;
                NekoDrive.Properties.Settings.Default.RemoteDevice = cboxRemoteDevices.SelectedIndex;
                NekoDrive.Properties.Settings.Default.DriveLetter = cboxLocalDrive.SelectedIndex;
                NekoDrive.Properties.Settings.Default.AutoMount = chkAutoMount.Checked;
                NekoDrive.Properties.Settings.Default.DriveLabel = tbDriveLabel.Text;
                NekoDrive.Properties.Settings.Default.Unicode = chkUnicode.Checked;
                NekoDrive.Properties.Settings.Default.Folder = btnSelectFolder.Text;
                NekoDrive.Properties.Settings.Default.DiskOrFolder = rbDisk.Checked;

                NekoDrive.Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog FolderBrowserDialog = new FolderBrowserDialog())
            {
                if (FolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    btnSelectFolder.Text = FolderBrowserDialog.SelectedPath;
                    toolTip.SetToolTip(btnSelectFolder, btnSelectFolder.Text);
                }
            }
        }

        #endregion        
    }
}
