using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LazyEvo.Plugins.ExtraLazy
{
    public partial class frmFrameDumper : Form
    {
        public frmFrameDumper()
        {
            InitializeComponent();
        }

        public void addFrame(String frameName)
        {
            this.txtOutput.Text += Environment.NewLine + frameName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrameViewer.getChildren(this.txtParentName.Text);
        }
    }
}
