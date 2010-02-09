using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NFSv2Client
{
    public partial class Progress : Form
    {
        public Progress()
        {
            InitializeComponent();
        }

        public void Update(string FileName, UInt32 Position, UInt32 TotalLenght)
        {
            label1.Text = FileName;
            progressBar1.Value = (int)Position;
            progressBar1.Maximum = (int) TotalLenght;
        }

        public void Close()
        {
            this.Close();
        }
    }
}
