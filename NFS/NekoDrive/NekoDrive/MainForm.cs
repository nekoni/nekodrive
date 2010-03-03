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
using NekoDrive.NFS.Wrappers;
using Dokan;
using NekoDrive.NFS;
using System.Threading;

namespace NekoDrive
{
    public partial class MainForm : Form
    {
        #region Fields

        public NFS.NFS mNFS = null;
        public DokanNet mDokanNet = null; 
        private static MainForm mInstance;

        #endregion

        #region Properties

        public static MainForm Instance
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

        private void UnmountDrive()
        {
            if (mNFS == null)
                throw new ApplicationException("NFS object is null!");

            if (mNFS.UnMountDevice() == NFSResult.NFS_SUCCESS)
            {
                int res = DokanNet.DokanUnmount(((string)cboxLocalDrive.SelectedItem).ToCharArray()[0]);
                cboxLocalDrive.Enabled = true;
                cboxRemoteDevices.Enabled = true;
                btnMount.Enabled = true;
                btnUnmount.Enabled = false;
                
            }
            else
                throw new ApplicationException("Unmount error (" + mNFS.GetLastError() + ")");
        }

        private void MountDrive()
        {
            if (mNFS == null)
                throw new ApplicationException("NFS object is null!");

            string strDev = (string)cboxRemoteDevices.SelectedItem;
            char cDrive = ((string)cboxLocalDrive.SelectedItem).ToCharArray()[0];
            if (MainForm.Instance.mNFS.MountDevice(strDev) == NFSResult.NFS_SUCCESS)
            {
                cboxLocalDrive.Enabled = false;
                cboxRemoteDevices.Enabled = false;
                btnMount.Enabled = false;
                btnUnmount.Enabled = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(
                    delegate
                    {
                        System.IO.Directory.SetCurrentDirectory(Application.StartupPath);
                        DokanOptions dokanOptions = new DokanOptions();
                        dokanOptions.DebugMode = true;
                        dokanOptions.DriveLetter = cDrive;
                        dokanOptions.UseKeepAlive = true;
                        dokanOptions.VolumeLabel = "NekoDrive";
                        dokanOptions.ThreadCount = 1;
                        Operations nfsOperations = new Operations();
                        DokanNet.DokanMain(dokanOptions, nfsOperations);
                    }));
            }
            else
                throw new ApplicationException("Mount error (" + mNFS.GetLastError() + ")");
        }

        private void Connect()
        {
            IPAddress ipAddress = new IPAddress(ipAddressControl1.GetAddressBytes());
            if (PingServer(ipAddress))
            {
                NFS.NFS.NFSVersion ver = NekoDrive.NFS.NFS.NFSVersion.v2;
                if (cboxVer.SelectedItem.ToString() == "V3")
                    ver = NFS.NFS.NFSVersion.v3;

                mNFS = new NFS.NFS(ver);
                mNFS.DataEvent += new NekoDrive.NFS.Wrappers.NFSDataEventHandler(mNFS_DataEvent);
                int UserId = int.Parse(tbUserId.Text);
                int GroupId = int.Parse(tbGroupId.Text);
                if (mNFS.Connect(ipAddress, UserId, GroupId, (int)nupTimeOut.Value) == NFSResult.NFS_SUCCESS)
                {
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

                }
                else
                    throw new ApplicationException("Connection error (" + mNFS.GetLastError() + ")");
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

            if (mNFS.Disconnect() == NFSResult.NFS_SUCCESS)
            {
                gboxMount.Enabled = false;
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                ipAddressControl1.Enabled = true;
                cboxVer.Enabled = true;
                tbGroupId.Enabled = true;
                tbUserId.Enabled = true;
                nupTimeOut.Enabled = true;
            }
            else
                throw new ApplicationException("Disconnection error (" + mNFS.GetLastError() + ")");
        }

        List<string> GetDriveLetters()
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
            cboxLocalDrive.Items.Clear();
            List<string> DriveLetters = GetDriveLetters();
            char c = 'D';
            while (c <= 'Z')
            {
               if(!DriveLetters.Contains(c.ToString()))
                   cboxLocalDrive.Items.Add(c.ToString());
                c++;
            }

            gboxMount.Enabled = false;
            btnDisconnect.Enabled = false;
            btnUnmount.Enabled = false;
            cboxVer.SelectedIndex = 0;
            cboxLocalDrive.SelectedItem = 0;
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
                MessageBox.Show(ex.ToString(), this.Text);
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
                MessageBox.Show(ex.ToString(), this.Text);
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
                MessageBox.Show(ex.ToString(), this.Text);
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
                MessageBox.Show(ex.ToString(), this.Text);
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
                MessageBox.Show(ex.ToString(), this.Text);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), this.Text);
            }
        }

        void mNFS_DataEvent(object sender, NekoDrive.NFS.Wrappers.NFSEventArgs e)
        {
            //
        }

        #endregion        
    }
}
