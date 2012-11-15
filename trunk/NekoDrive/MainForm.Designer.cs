namespace NekoDrive
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.gbTargetConnection = new System.Windows.Forms.GroupBox();
            this.chkUsePrivilegedPorts = new System.Windows.Forms.CheckBox();
            this.chkUnicode = new System.Windows.Forms.CheckBox();
            this.chkAutoConnect = new System.Windows.Forms.CheckBox();
            this.tbGroupId = new System.Windows.Forms.TextBox();
            this.tbUserId = new System.Windows.Forms.TextBox();
            this.lblGroupId = new System.Windows.Forms.Label();
            this.lblUserId = new System.Windows.Forms.Label();
            this.ipAddressControl1 = new NekoDrive.Controls.IPAddressControl();
            this.lblTimeOut = new System.Windows.Forms.Label();
            this.nupTimeOut = new System.Windows.Forms.NumericUpDown();
            this.lblCurrentFile = new System.Windows.Forms.Label();
            this.cboxVer = new System.Windows.Forms.ComboBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.gboxMount = new System.Windows.Forms.GroupBox();
            this.chkNoCache = new System.Windows.Forms.CheckBox();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.rbFolder = new System.Windows.Forms.RadioButton();
            this.rbDisk = new System.Windows.Forms.RadioButton();
            this.tbDriveLabel = new System.Windows.Forms.TextBox();
            this.chkAutoMount = new System.Windows.Forms.CheckBox();
            this.lblDriveLabel = new System.Windows.Forms.Label();
            this.lblRemoteDevices = new System.Windows.Forms.Label();
            this.cboxLocalDrive = new System.Windows.Forms.ComboBox();
            this.cboxRemoteDevices = new System.Windows.Forms.ComboBox();
            this.btnClearCache = new System.Windows.Forms.Button();
            this.btnUnmount = new System.Windows.Forms.Button();
            this.btnMount = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbNewFolderSettings = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbOther = new System.Windows.Forms.ComboBox();
            this.cbGroup = new System.Windows.Forms.ComboBox();
            this.cbUser = new System.Windows.Forms.ComboBox();
            this.gbTargetConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupTimeOut)).BeginInit();
            this.gboxMount.SuspendLayout();
            this.gbNewFolderSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTargetConnection
            // 
            this.gbTargetConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTargetConnection.Controls.Add(this.chkUsePrivilegedPorts);
            this.gbTargetConnection.Controls.Add(this.chkUnicode);
            this.gbTargetConnection.Controls.Add(this.chkAutoConnect);
            this.gbTargetConnection.Controls.Add(this.tbGroupId);
            this.gbTargetConnection.Controls.Add(this.tbUserId);
            this.gbTargetConnection.Controls.Add(this.lblGroupId);
            this.gbTargetConnection.Controls.Add(this.lblUserId);
            this.gbTargetConnection.Controls.Add(this.ipAddressControl1);
            this.gbTargetConnection.Controls.Add(this.lblTimeOut);
            this.gbTargetConnection.Controls.Add(this.nupTimeOut);
            this.gbTargetConnection.Controls.Add(this.lblCurrentFile);
            this.gbTargetConnection.Controls.Add(this.cboxVer);
            this.gbTargetConnection.Controls.Add(this.btnDisconnect);
            this.gbTargetConnection.Controls.Add(this.btnConnect);
            this.gbTargetConnection.Location = new System.Drawing.Point(12, 12);
            this.gbTargetConnection.Name = "gbTargetConnection";
            this.gbTargetConnection.Size = new System.Drawing.Size(527, 97);
            this.gbTargetConnection.TabIndex = 5;
            this.gbTargetConnection.TabStop = false;
            this.gbTargetConnection.Text = "Target Connection";
            // 
            // chkUsePrivilegedPorts
            // 
            this.chkUsePrivilegedPorts.AutoSize = true;
            this.chkUsePrivilegedPorts.Location = new System.Drawing.Point(10, 74);
            this.chkUsePrivilegedPorts.Name = "chkUsePrivilegedPorts";
            this.chkUsePrivilegedPorts.Size = new System.Drawing.Size(121, 17);
            this.chkUsePrivilegedPorts.TabIndex = 13;
            this.chkUsePrivilegedPorts.Text = "Use Privileged Ports";
            this.chkUsePrivilegedPorts.UseVisualStyleBackColor = true;
            // 
            // chkUnicode
            // 
            this.chkUnicode.AutoSize = true;
            this.chkUnicode.Location = new System.Drawing.Point(234, 51);
            this.chkUnicode.Name = "chkUnicode";
            this.chkUnicode.Size = new System.Drawing.Size(66, 17);
            this.chkUnicode.TabIndex = 13;
            this.chkUnicode.Text = "Unicode";
            this.toolTip.SetToolTip(this.chkUnicode, "Support unicode encoding");
            this.chkUnicode.UseVisualStyleBackColor = true;
            // 
            // chkAutoConnect
            // 
            this.chkAutoConnect.AutoSize = true;
            this.chkAutoConnect.Location = new System.Drawing.Point(180, 51);
            this.chkAutoConnect.Name = "chkAutoConnect";
            this.chkAutoConnect.Size = new System.Drawing.Size(48, 17);
            this.chkAutoConnect.TabIndex = 0;
            this.chkAutoConnect.Text = "Auto";
            this.toolTip.SetToolTip(this.chkAutoConnect, "Connect automatically at startup");
            this.chkAutoConnect.UseVisualStyleBackColor = true;
            // 
            // tbGroupId
            // 
            this.tbGroupId.Location = new System.Drawing.Point(137, 49);
            this.tbGroupId.Name = "tbGroupId";
            this.tbGroupId.Size = new System.Drawing.Size(37, 20);
            this.tbGroupId.TabIndex = 12;
            this.tbGroupId.Text = "0";
            this.toolTip.SetToolTip(this.tbGroupId, "UNIX Group ID");
            // 
            // tbUserId
            // 
            this.tbUserId.Location = new System.Drawing.Point(50, 49);
            this.tbUserId.Name = "tbUserId";
            this.tbUserId.Size = new System.Drawing.Size(35, 20);
            this.tbUserId.TabIndex = 12;
            this.tbUserId.Text = "0";
            this.toolTip.SetToolTip(this.tbUserId, "UNIX User ID");
            // 
            // lblGroupId
            // 
            this.lblGroupId.AutoSize = true;
            this.lblGroupId.Location = new System.Drawing.Point(86, 53);
            this.lblGroupId.Name = "lblGroupId";
            this.lblGroupId.Size = new System.Drawing.Size(45, 13);
            this.lblGroupId.TabIndex = 11;
            this.lblGroupId.Text = "GroupId";
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(6, 52);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(38, 13);
            this.lblUserId.TabIndex = 11;
            this.lblUserId.Text = "UserId";
            // 
            // ipAddressControl1
            // 
            this.ipAddressControl1.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddressControl1.Location = new System.Drawing.Point(10, 21);
            this.ipAddressControl1.MinimumSize = new System.Drawing.Size(112, 20);
            this.ipAddressControl1.Name = "ipAddressControl1";
            this.ipAddressControl1.ReadOnly = false;
            this.ipAddressControl1.Size = new System.Drawing.Size(112, 20);
            this.ipAddressControl1.TabIndex = 10;
            this.toolTip.SetToolTip(this.ipAddressControl1, "Specify the ip of the NFS Server");
            // 
            // lblTimeOut
            // 
            this.lblTimeOut.AutoSize = true;
            this.lblTimeOut.Location = new System.Drawing.Point(177, 21);
            this.lblTimeOut.Name = "lblTimeOut";
            this.lblTimeOut.Size = new System.Drawing.Size(72, 13);
            this.lblTimeOut.TabIndex = 9;
            this.lblTimeOut.Text = "Time Out [ms]";
            // 
            // nupTimeOut
            // 
            this.nupTimeOut.Location = new System.Drawing.Point(250, 19);
            this.nupTimeOut.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.nupTimeOut.Name = "nupTimeOut";
            this.nupTimeOut.Size = new System.Drawing.Size(58, 20);
            this.nupTimeOut.TabIndex = 8;
            this.toolTip.SetToolTip(this.nupTimeOut, "Command timeout in milliseconds");
            this.nupTimeOut.Value = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            // 
            // lblCurrentFile
            // 
            this.lblCurrentFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentFile.AutoSize = true;
            this.lblCurrentFile.Location = new System.Drawing.Point(326, 13);
            this.lblCurrentFile.Name = "lblCurrentFile";
            this.lblCurrentFile.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentFile.TabIndex = 6;
            // 
            // cboxVer
            // 
            this.cboxVer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxVer.FormattingEnabled = true;
            this.cboxVer.Items.AddRange(new object[] {
            "V2",
            "V3"});
            this.cboxVer.Location = new System.Drawing.Point(128, 21);
            this.cboxVer.Name = "cboxVer";
            this.cboxVer.Size = new System.Drawing.Size(46, 21);
            this.cboxVer.TabIndex = 4;
            this.toolTip.SetToolTip(this.cboxVer, "NFS Server version");
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(448, 48);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(73, 23);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "Disconnect";
            this.toolTip.SetToolTip(this.btnDisconnect, "Disconnect the current session");
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(448, 19);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(73, 23);
            this.btnConnect.TabIndex = 3;
            this.btnConnect.Text = "Connect";
            this.toolTip.SetToolTip(this.btnConnect, "Connect to the NFS Server");
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // gboxMount
            // 
            this.gboxMount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gboxMount.Controls.Add(this.chkNoCache);
            this.gboxMount.Controls.Add(this.btnSelectFolder);
            this.gboxMount.Controls.Add(this.rbFolder);
            this.gboxMount.Controls.Add(this.rbDisk);
            this.gboxMount.Controls.Add(this.tbDriveLabel);
            this.gboxMount.Controls.Add(this.chkAutoMount);
            this.gboxMount.Controls.Add(this.lblDriveLabel);
            this.gboxMount.Controls.Add(this.lblRemoteDevices);
            this.gboxMount.Controls.Add(this.cboxLocalDrive);
            this.gboxMount.Controls.Add(this.cboxRemoteDevices);
            this.gboxMount.Controls.Add(this.btnClearCache);
            this.gboxMount.Controls.Add(this.btnUnmount);
            this.gboxMount.Controls.Add(this.btnMount);
            this.gboxMount.Location = new System.Drawing.Point(12, 115);
            this.gboxMount.Name = "gboxMount";
            this.gboxMount.Size = new System.Drawing.Size(527, 114);
            this.gboxMount.TabIndex = 6;
            this.gboxMount.TabStop = false;
            this.gboxMount.Text = "Mount";
            // 
            // chkNoCache
            // 
            this.chkNoCache.AutoSize = true;
            this.chkNoCache.Location = new System.Drawing.Point(368, 21);
            this.chkNoCache.Name = "chkNoCache";
            this.chkNoCache.Size = new System.Drawing.Size(74, 17);
            this.chkNoCache.TabIndex = 13;
            this.chkNoCache.Text = "No Cache";
            this.chkNoCache.UseVisualStyleBackColor = true;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(115, 79);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(225, 23);
            this.btnSelectFolder.TabIndex = 6;
            this.btnSelectFolder.Text = "Browse";
            this.toolTip.SetToolTip(this.btnSelectFolder, "Select the folder that will be binded to the mounted device");
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // rbFolder
            // 
            this.rbFolder.AutoSize = true;
            this.rbFolder.Location = new System.Drawing.Point(17, 79);
            this.rbFolder.Name = "rbFolder";
            this.rbFolder.Size = new System.Drawing.Size(82, 17);
            this.rbFolder.TabIndex = 5;
            this.rbFolder.TabStop = true;
            this.rbFolder.Text = "Mount Point";
            this.toolTip.SetToolTip(this.rbFolder, "Mount a folder to the selected device");
            this.rbFolder.UseVisualStyleBackColor = true;
            // 
            // rbDisk
            // 
            this.rbDisk.AutoSize = true;
            this.rbDisk.Location = new System.Drawing.Point(18, 51);
            this.rbDisk.Name = "rbDisk";
            this.rbDisk.Size = new System.Drawing.Size(46, 17);
            this.rbDisk.TabIndex = 5;
            this.rbDisk.TabStop = true;
            this.rbDisk.Text = "Disk";
            this.toolTip.SetToolTip(this.rbDisk, "Mount a disk to the selected device");
            this.rbDisk.UseVisualStyleBackColor = true;
            // 
            // tbDriveLabel
            // 
            this.tbDriveLabel.Location = new System.Drawing.Point(198, 53);
            this.tbDriveLabel.MaxLength = 10;
            this.tbDriveLabel.Name = "tbDriveLabel";
            this.tbDriveLabel.Size = new System.Drawing.Size(142, 20);
            this.tbDriveLabel.TabIndex = 4;
            this.toolTip.SetToolTip(this.tbDriveLabel, "Volume lavel if Disk is selected");
            // 
            // chkAutoMount
            // 
            this.chkAutoMount.AutoSize = true;
            this.chkAutoMount.Location = new System.Drawing.Point(314, 20);
            this.chkAutoMount.Name = "chkAutoMount";
            this.chkAutoMount.Size = new System.Drawing.Size(48, 17);
            this.chkAutoMount.TabIndex = 0;
            this.chkAutoMount.Text = "Auto";
            this.toolTip.SetToolTip(this.chkAutoMount, "Mount automatically the device at startup");
            this.chkAutoMount.UseVisualStyleBackColor = true;
            // 
            // lblDriveLabel
            // 
            this.lblDriveLabel.AutoSize = true;
            this.lblDriveLabel.Location = new System.Drawing.Point(121, 53);
            this.lblDriveLabel.Name = "lblDriveLabel";
            this.lblDriveLabel.Size = new System.Drawing.Size(71, 13);
            this.lblDriveLabel.TabIndex = 2;
            this.lblDriveLabel.Text = "Volume Label";
            // 
            // lblRemoteDevices
            // 
            this.lblRemoteDevices.AutoSize = true;
            this.lblRemoteDevices.Location = new System.Drawing.Point(14, 21);
            this.lblRemoteDevices.Name = "lblRemoteDevices";
            this.lblRemoteDevices.Size = new System.Drawing.Size(46, 13);
            this.lblRemoteDevices.TabIndex = 2;
            this.lblRemoteDevices.Text = "Devices";
            // 
            // cboxLocalDrive
            // 
            this.cboxLocalDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxLocalDrive.FormattingEnabled = true;
            this.cboxLocalDrive.Location = new System.Drawing.Point(77, 50);
            this.cboxLocalDrive.Name = "cboxLocalDrive";
            this.cboxLocalDrive.Size = new System.Drawing.Size(32, 21);
            this.cboxLocalDrive.TabIndex = 3;
            this.toolTip.SetToolTip(this.cboxLocalDrive, "Disk binded to the mounted device");
            // 
            // cboxRemoteDevices
            // 
            this.cboxRemoteDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxRemoteDevices.FormattingEnabled = true;
            this.cboxRemoteDevices.Location = new System.Drawing.Point(66, 20);
            this.cboxRemoteDevices.Name = "cboxRemoteDevices";
            this.cboxRemoteDevices.Size = new System.Drawing.Size(234, 21);
            this.cboxRemoteDevices.TabIndex = 3;
            this.toolTip.SetToolTip(this.cboxRemoteDevices, "Select the remote device to mount");
            // 
            // btnClearCache
            // 
            this.btnClearCache.Location = new System.Drawing.Point(448, 78);
            this.btnClearCache.Name = "btnClearCache";
            this.btnClearCache.Size = new System.Drawing.Size(73, 23);
            this.btnClearCache.TabIndex = 3;
            this.btnClearCache.Text = "Clear Cache";
            this.btnClearCache.UseVisualStyleBackColor = true;
            this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // btnUnmount
            // 
            this.btnUnmount.Location = new System.Drawing.Point(448, 49);
            this.btnUnmount.Name = "btnUnmount";
            this.btnUnmount.Size = new System.Drawing.Size(73, 23);
            this.btnUnmount.TabIndex = 3;
            this.btnUnmount.Text = "Unmount";
            this.toolTip.SetToolTip(this.btnUnmount, "Unmount the current device");
            this.btnUnmount.UseVisualStyleBackColor = true;
            this.btnUnmount.Click += new System.EventHandler(this.btnUnmount_Click);
            // 
            // btnMount
            // 
            this.btnMount.Location = new System.Drawing.Point(448, 16);
            this.btnMount.Name = "btnMount";
            this.btnMount.Size = new System.Drawing.Size(73, 23);
            this.btnMount.TabIndex = 3;
            this.btnMount.Text = "Mount";
            this.toolTip.SetToolTip(this.btnMount, "Mount the selected device");
            this.btnMount.UseVisualStyleBackColor = true;
            this.btnMount.Click += new System.EventHandler(this.btnMount_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "NekoDrive";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // gbNewFolderSettings
            // 
            this.gbNewFolderSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbNewFolderSettings.Controls.Add(this.label4);
            this.gbNewFolderSettings.Controls.Add(this.label3);
            this.gbNewFolderSettings.Controls.Add(this.label2);
            this.gbNewFolderSettings.Controls.Add(this.cbOther);
            this.gbNewFolderSettings.Controls.Add(this.cbGroup);
            this.gbNewFolderSettings.Controls.Add(this.cbUser);
            this.gbNewFolderSettings.Location = new System.Drawing.Point(12, 236);
            this.gbNewFolderSettings.Name = "gbNewFolderSettings";
            this.gbNewFolderSettings.Size = new System.Drawing.Size(527, 74);
            this.gbNewFolderSettings.TabIndex = 7;
            this.gbNewFolderSettings.TabStop = false;
            this.gbNewFolderSettings.Text = "New Folder Permissions";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(216, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Other";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(112, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Group";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "User";
            // 
            // cbOther
            // 
            this.cbOther.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOther.FormattingEnabled = true;
            this.cbOther.Items.AddRange(new object[] {
            "Read Only",
            "Read & Write",
            "Read, Write & Execute"});
            this.cbOther.Location = new System.Drawing.Point(219, 37);
            this.cbOther.Name = "cbOther";
            this.cbOther.Size = new System.Drawing.Size(98, 21);
            this.cbOther.TabIndex = 11;
            this.cbOther.SelectedIndexChanged += new System.EventHandler(this.cbOther_SelectedIndexChanged);
            // 
            // cbGroup
            // 
            this.cbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGroup.FormattingEnabled = true;
            this.cbGroup.Items.AddRange(new object[] {
            "Read Only",
            "Read & Write",
            "Read, Write & Execute"});
            this.cbGroup.Location = new System.Drawing.Point(115, 37);
            this.cbGroup.Name = "cbGroup";
            this.cbGroup.Size = new System.Drawing.Size(98, 21);
            this.cbGroup.TabIndex = 10;
            this.cbGroup.SelectedIndexChanged += new System.EventHandler(this.cbGroup_SelectedIndexChanged);
            // 
            // cbUser
            // 
            this.cbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUser.FormattingEnabled = true;
            this.cbUser.Items.AddRange(new object[] {
            "Read Only",
            "Read & Write",
            "Read, Write & Execute"});
            this.cbUser.Location = new System.Drawing.Point(11, 37);
            this.cbUser.Name = "cbUser";
            this.cbUser.Size = new System.Drawing.Size(98, 21);
            this.cbUser.TabIndex = 9;
            this.cbUser.SelectedIndexChanged += new System.EventHandler(this.cbUser_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 320);
            this.Controls.Add(this.gbNewFolderSettings);
            this.Controls.Add(this.gboxMount);
            this.Controls.Add(this.gbTargetConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "NekoDrive";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.gbTargetConnection.ResumeLayout(false);
            this.gbTargetConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupTimeOut)).EndInit();
            this.gboxMount.ResumeLayout(false);
            this.gboxMount.PerformLayout();
            this.gbNewFolderSettings.ResumeLayout(false);
            this.gbNewFolderSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbTargetConnection;
        private System.Windows.Forms.Label lblTimeOut;
        private System.Windows.Forms.NumericUpDown nupTimeOut;
        private System.Windows.Forms.Label lblCurrentFile;
        private System.Windows.Forms.ComboBox cboxVer;
        private NekoDrive.Controls.IPAddressControl ipAddressControl1;
        private System.Windows.Forms.Label lblGroupId;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.TextBox tbGroupId;
        private System.Windows.Forms.TextBox tbUserId;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox gboxMount;
        private System.Windows.Forms.Label lblRemoteDevices;
        private System.Windows.Forms.ComboBox cboxLocalDrive;
        private System.Windows.Forms.ComboBox cboxRemoteDevices;
        private System.Windows.Forms.Button btnMount;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnUnmount;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.CheckBox chkAutoConnect;
        private System.Windows.Forms.CheckBox chkAutoMount;
        private System.Windows.Forms.TextBox tbDriveLabel;
        private System.Windows.Forms.Label lblDriveLabel;
        private System.Windows.Forms.CheckBox chkUnicode;
        private System.Windows.Forms.RadioButton rbDisk;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.RadioButton rbFolder;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox chkUsePrivilegedPorts;
        private System.Windows.Forms.GroupBox gbNewFolderSettings;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cbOther;
        public System.Windows.Forms.ComboBox cbGroup;
        public System.Windows.Forms.ComboBox cbUser;
        private System.Windows.Forms.CheckBox chkNoCache;
        private System.Windows.Forms.Button btnClearCache;

    }
}