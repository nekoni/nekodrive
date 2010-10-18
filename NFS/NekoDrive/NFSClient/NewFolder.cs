using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NFSClient
{
    public partial class NewFolder : Form
    {
        public string NewFolderName;
        public int userPSelectedIndex;
        public int groupPSelectedIndex;
        public int otherPSelectedIndex;

        public NewFolder()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            NewFolderName = tbNewFolder.Text;
            userPSelectedIndex = comboBox1.SelectedIndex;
            groupPSelectedIndex = comboBox2.SelectedIndex;
            otherPSelectedIndex = comboBox3.SelectedIndex;
        }
    }
}
