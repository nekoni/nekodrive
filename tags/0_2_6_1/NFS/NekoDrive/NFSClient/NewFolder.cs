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

        public NewFolder()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            NewFolderName = tbNewFolder.Text;
        }
    }
}
