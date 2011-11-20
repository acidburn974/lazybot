using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LazyLib.Helpers;
using LazyLib.Wow;
using System.Diagnostics;
using LazyEvo.Plugins.ExtraLazy;

namespace LazyEvo.Plugins.LazyData
{
    public partial class frmProfessions : Form
    {
        private Professions _professions;
        private String _blueChat;

        public String BlueChat
        {
            get
            {
                return this._blueChat;
            }
            set
            {
                this._blueChat = value;
            }
        }

        public Professions Professions {
            get
            {
                return this._professions;
            }
            set
            {
                this._professions = value;
            }
        }

        public frmProfessions()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }
        private delegate void CreateDisplay();
        public void createDisplay()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CreateDisplay(createDisplay));

                return;
            }
            Profession profession = this._professions.Primary1;
            this.label3.Text = profession.Name;
            this.label5.Text = profession.Rank;
            this.label7.Text = profession.Level.ToString();
            this.label9.Text = profession.MaxLevel.ToString();

            profession = this._professions.Primary2;
            this.label13.Text = profession.Name;
            this.label1.Text = profession.Rank;
            this.label14.Text = profession.Level.ToString();
            this.label10.Text = profession.MaxLevel.ToString();

            profession = this._professions.Secondary1;
            this.label21.Text = profession.Name;
            this.label17.Text = profession.Rank;
            this.label22.Text = profession.Level.ToString();
            this.label18.Text = profession.MaxLevel.ToString();

            profession = this._professions.Secondary2;
            this.label29.Text = profession.Name;
            this.label25.Text = profession.Rank;
            this.label30.Text = profession.Level.ToString();
            this.label26.Text = profession.MaxLevel.ToString();

            profession = this._professions.Secondary3;
            this.label37.Text = profession.Name;
            this.label33.Text = profession.Rank;
            this.label38.Text = profession.Level.ToString();
            this.label34.Text = profession.MaxLevel.ToString();

            profession = this._professions.Secondary4;
            this.label45.Text = profession.Name;
            this.label41.Text = profession.Rank;
            this.label46.Text = profession.Level.ToString();
            this.label42.Text = profession.MaxLevel.ToString();

            this.label49.Visible = false;
        }

        private void label1_Click_2(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label49_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.label49.Visible = true;
            Loader.getProfessions();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrameViewer viewer = new FrameViewer();
        }
        private delegate void SetBlueChat();
        public void setBlueChat()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new SetBlueChat(setBlueChat));

                return;
            }
            this.txtBoxChat.Text = this._blueChat;
        }
    }

}
