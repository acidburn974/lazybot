namespace LazyEvo.Plugins.RotationPlugin
{
    partial class RotatorForm
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
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.superTooltip1 = new DevComponents.DotNetBar.SuperTooltip();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.StartMonitoring = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.CBOpenRotationManager = new DevComponents.DotNetBar.ButtonX();
            this.CBShowStatusWindow = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Windows7Blue;
            // 
            // superTooltip1
            // 
            this.superTooltip1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.StartMonitoring);
            this.groupPanel1.Controls.Add(this.CBOpenRotationManager);
            this.groupPanel1.Location = new System.Drawing.Point(2, 6);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(244, 58);
            // 
            // 
            // 
            this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel1.Style.BackColorGradientAngle = 90;
            this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderBottomWidth = 1;
            this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderLeftWidth = 1;
            this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderRightWidth = 1;
            this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel1.Style.BorderTopWidth = 1;
            this.groupPanel1.Style.Class = "";
            this.groupPanel1.Style.CornerDiameter = 4;
            this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseDown.Class = "";
            this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel1.StyleMouseOver.Class = "";
            this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel1.TabIndex = 0;
            // 
            // StartMonitoring
            // 
            this.StartMonitoring.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.StartMonitoring.BackgroundStyle.Class = "";
            this.StartMonitoring.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.StartMonitoring.Location = new System.Drawing.Point(7, 13);
            this.StartMonitoring.Name = "StartMonitoring";
            this.StartMonitoring.Size = new System.Drawing.Size(80, 23);
            this.StartMonitoring.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.StartMonitoring.TabIndex = 1;
            this.StartMonitoring.Text = "Enabled";
            this.StartMonitoring.CheckedChanged += new System.EventHandler(this.StartMonitoringCheckedChanged);
            // 
            // CBOpenRotationManager
            // 
            this.CBOpenRotationManager.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.CBOpenRotationManager.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.CBOpenRotationManager.Location = new System.Drawing.Point(93, 3);
            this.CBOpenRotationManager.Name = "CBOpenRotationManager";
            this.CBOpenRotationManager.Size = new System.Drawing.Size(120, 42);
            this.CBOpenRotationManager.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.CBOpenRotationManager.TabIndex = 0;
            this.CBOpenRotationManager.Text = "Open rotation manager";
            this.CBOpenRotationManager.Click += new System.EventHandler(this.CbOpenRotationManagerClick);
            // 
            // CBShowStatusWindow
            // 
            // 
            // 
            // 
            this.CBShowStatusWindow.BackgroundStyle.Class = "";
            this.CBShowStatusWindow.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.CBShowStatusWindow.Location = new System.Drawing.Point(118, 70);
            this.CBShowStatusWindow.Name = "CBShowStatusWindow";
            this.CBShowStatusWindow.Size = new System.Drawing.Size(128, 23);
            this.CBShowStatusWindow.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.CBShowStatusWindow.TabIndex = 1;
            this.CBShowStatusWindow.Text = "Show status window";
            this.CBShowStatusWindow.CheckedChanged += new System.EventHandler(this.CBShowStatusWindow_CheckedChanged);
            // 
            // RotatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(250, 93);
            this.Controls.Add(this.CBShowStatusWindow);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.Name = "RotatorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RotatorFormFormClosing);
            this.Load += new System.EventHandler(this.RotatorFormLoad);
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.SuperTooltip superTooltip1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.CheckBoxX StartMonitoring;
        private DevComponents.DotNetBar.ButtonX CBOpenRotationManager;
        private DevComponents.DotNetBar.Controls.CheckBoxX CBShowStatusWindow;
    }
}