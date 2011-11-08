namespace LazyEvo.Forms
{
    partial class Debug
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.DebugTBLog = new System.Windows.Forms.RichTextBox();
            this.DebugBtnClick = new DevComponents.DotNetBar.ButtonX();
            this.DebugBtnFindUI = new DevComponents.DotNetBar.ButtonX();
            this.DebugTBUIName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.UIBtnDump = new DevComponents.DotNetBar.ButtonX();
            this.superTabItem2 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.listView2 = new System.Windows.Forms.ListView();
            this.superTabItem1 = new DevComponents.DotNetBar.SuperTabItem();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.superTabControl1.SuspendLayout();
            this.superTabControlPanel2.SuspendLayout();
            this.superTabControlPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(307, 505);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Windows7Blue;
            // 
            // superTabControl1
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl1.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControl1.ControlBox.MenuBox.Name = "";
            this.superTabControl1.ControlBox.Name = "";
            this.superTabControl1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl1.ControlBox.MenuBox,
            this.superTabControl1.ControlBox.CloseBox});
            this.superTabControl1.Controls.Add(this.superTabControlPanel2);
            this.superTabControl1.Controls.Add(this.superTabControlPanel1);
            this.superTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl1.Location = new System.Drawing.Point(0, 0);
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = true;
            this.superTabControl1.SelectedTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.superTabControl1.SelectedTabIndex = 1;
            this.superTabControl1.Size = new System.Drawing.Size(633, 574);
            this.superTabControl1.TabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.superTabControl1.TabIndex = 4;
            this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItem1,
            this.superTabItem2});
            this.superTabControl1.Text = "UI";
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Controls.Add(this.DebugTBLog);
            this.superTabControlPanel2.Controls.Add(this.DebugBtnClick);
            this.superTabControlPanel2.Controls.Add(this.DebugBtnFindUI);
            this.superTabControlPanel2.Controls.Add(this.DebugTBUIName);
            this.superTabControlPanel2.Controls.Add(this.labelX7);
            this.superTabControlPanel2.Controls.Add(this.UIBtnDump);
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(0, 25);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.Size = new System.Drawing.Size(633, 549);
            this.superTabControlPanel2.TabIndex = 0;
            this.superTabControlPanel2.TabItem = this.superTabItem2;
            // 
            // DebugTBLog
            // 
            this.DebugTBLog.Location = new System.Drawing.Point(12, 90);
            this.DebugTBLog.Name = "DebugTBLog";
            this.DebugTBLog.Size = new System.Drawing.Size(249, 112);
            this.DebugTBLog.TabIndex = 11;
            this.DebugTBLog.Text = "";
            // 
            // DebugBtnClick
            // 
            this.DebugBtnClick.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DebugBtnClick.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.DebugBtnClick.Location = new System.Drawing.Point(93, 61);
            this.DebugBtnClick.Name = "DebugBtnClick";
            this.DebugBtnClick.Size = new System.Drawing.Size(75, 23);
            this.DebugBtnClick.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.DebugBtnClick.TabIndex = 10;
            this.DebugBtnClick.Text = "Click";
            this.DebugBtnClick.Click += new System.EventHandler(this.DebugBtnClickClick);
            // 
            // DebugBtnFindUI
            // 
            this.DebugBtnFindUI.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DebugBtnFindUI.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.DebugBtnFindUI.Location = new System.Drawing.Point(12, 61);
            this.DebugBtnFindUI.Name = "DebugBtnFindUI";
            this.DebugBtnFindUI.Size = new System.Drawing.Size(75, 23);
            this.DebugBtnFindUI.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.DebugBtnFindUI.TabIndex = 9;
            this.DebugBtnFindUI.Text = "Search";
            this.DebugBtnFindUI.Click += new System.EventHandler(this.DebugBtnFindUiClick);
            // 
            // DebugTBUIName
            // 
            // 
            // 
            // 
            this.DebugTBUIName.Border.Class = "TextBoxBorder";
            this.DebugTBUIName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.DebugTBUIName.Location = new System.Drawing.Point(12, 35);
            this.DebugTBUIName.Name = "DebugTBUIName";
            this.DebugTBUIName.Size = new System.Drawing.Size(128, 20);
            this.DebugTBUIName.TabIndex = 8;
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = "";
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(12, 13);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(146, 23);
            this.labelX7.TabIndex = 7;
            this.labelX7.Text = "Find UI object by name:";
            // 
            // UIBtnDump
            // 
            this.UIBtnDump.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.UIBtnDump.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.UIBtnDump.Location = new System.Drawing.Point(15, 208);
            this.UIBtnDump.Name = "UIBtnDump";
            this.UIBtnDump.Size = new System.Drawing.Size(196, 23);
            this.UIBtnDump.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.UIBtnDump.TabIndex = 6;
            this.UIBtnDump.Text = "Dump all UI object to the log file";
            this.UIBtnDump.Click += new System.EventHandler(this.UiBtnDumpClick);
            // 
            // superTabItem2
            // 
            this.superTabItem2.AttachedControl = this.superTabControlPanel2;
            this.superTabItem2.GlobalItem = false;
            this.superTabItem2.Name = "superTabItem2";
            this.superTabItem2.Text = "Ui";
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.superTabControlPanel1.Controls.Add(this.listView2);
            this.superTabControlPanel1.Controls.Add(this.listView1);
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 25);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(633, 549);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this.superTabItem1;
            // 
            // listView2
            // 
            this.listView2.Location = new System.Drawing.Point(316, 3);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(307, 505);
            this.listView2.TabIndex = 3;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // superTabItem1
            // 
            this.superTabItem1.AttachedControl = this.superTabControlPanel1;
            this.superTabItem1.GlobalItem = false;
            this.superTabItem1.Name = "superTabItem1";
            this.superTabItem1.Text = "Unit info";
            // 
            // Debug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(234)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(633, 574);
            this.Controls.Add(this.superTabControl1);
            this.DoubleBuffered = true;
            this.Name = "Debug";
            this.Load += new System.EventHandler(this.Debug_Load);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.superTabControl1.ResumeLayout(false);
            this.superTabControlPanel2.ResumeLayout(false);
            this.superTabControlPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Timer timer1;
        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.SuperTabControl superTabControl1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private DevComponents.DotNetBar.SuperTabItem superTabItem1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.DotNetBar.ButtonX DebugBtnClick;
        private DevComponents.DotNetBar.ButtonX DebugBtnFindUI;
        private DevComponents.DotNetBar.Controls.TextBoxX DebugTBUIName;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.ButtonX UIBtnDump;
        private DevComponents.DotNetBar.SuperTabItem superTabItem2;
        private System.Windows.Forms.RichTextBox DebugTBLog;
        private System.Windows.Forms.ListView listView2;
    }
}