namespace LazyLib.LazyRadar
{
    partial class RadarForm
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MapTimer = new System.Windows.Forms.Timer(this.components);
            this.expandableSplitter1 = new DevComponents.DotNetBar.ExpandableSplitter();
            this.ControlSettings = new DevComponents.DotNetBar.ItemPanel();
            this.CBTopMost = new DevComponents.DotNetBar.CheckBoxItem();
            this.SuspendLayout();
            // 
            // MapTimer
            // 
            this.MapTimer.Enabled = true;
            this.MapTimer.Interval = 150;
            this.MapTimer.Tick += new System.EventHandler(this.MapTimerTick);
            // 
            // expandableSplitter1
            // 
            this.expandableSplitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(146)))), ((int)(((byte)(166)))));
            this.expandableSplitter1.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.expandableSplitter1.ExpandableControl = this.ControlSettings;
            this.expandableSplitter1.ExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(146)))), ((int)(((byte)(166)))));
            this.expandableSplitter1.ExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.ExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(66)))), ((int)(((byte)(100)))));
            this.expandableSplitter1.ExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(66)))), ((int)(((byte)(100)))));
            this.expandableSplitter1.GripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.GripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.GripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.HotBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
            this.expandableSplitter1.HotBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
            this.expandableSplitter1.HotBackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground2;
            this.expandableSplitter1.HotBackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemPressedBackground;
            this.expandableSplitter1.HotExpandFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(146)))), ((int)(((byte)(166)))));
            this.expandableSplitter1.HotExpandFillColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotExpandLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(66)))), ((int)(((byte)(100)))));
            this.expandableSplitter1.HotExpandLineColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.expandableSplitter1.HotGripDarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(146)))), ((int)(((byte)(166)))));
            this.expandableSplitter1.HotGripDarkColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.expandableSplitter1.HotGripLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(251)))), ((int)(((byte)(255)))));
            this.expandableSplitter1.HotGripLightColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.expandableSplitter1.Location = new System.Drawing.Point(108, 0);
            this.expandableSplitter1.Name = "expandableSplitter1";
            this.expandableSplitter1.Size = new System.Drawing.Size(10, 271);
            this.expandableSplitter1.Style = DevComponents.DotNetBar.eSplitterStyle.Office2007;
            this.expandableSplitter1.TabIndex = 8;
            this.expandableSplitter1.TabStop = false;
            // 
            // ControlSettings
            // 
            this.ControlSettings.BackColor = System.Drawing.Color.LightGray;
            // 
            // 
            // 
            this.ControlSettings.BackgroundStyle.BackColor = System.Drawing.Color.White;
            this.ControlSettings.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ControlSettings.BackgroundStyle.BorderBottomWidth = 1;
            this.ControlSettings.BackgroundStyle.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.ControlSettings.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ControlSettings.BackgroundStyle.BorderLeftWidth = 1;
            this.ControlSettings.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ControlSettings.BackgroundStyle.BorderRightWidth = 1;
            this.ControlSettings.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.ControlSettings.BackgroundStyle.BorderTopWidth = 1;
            this.ControlSettings.BackgroundStyle.Class = "";
            this.ControlSettings.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ControlSettings.BackgroundStyle.PaddingBottom = 1;
            this.ControlSettings.BackgroundStyle.PaddingLeft = 1;
            this.ControlSettings.BackgroundStyle.PaddingRight = 1;
            this.ControlSettings.BackgroundStyle.PaddingTop = 1;
            this.ControlSettings.ContainerControlProcessDialogKey = true;
            this.ControlSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.ControlSettings.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.CBTopMost});
            this.ControlSettings.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.ControlSettings.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.ControlSettings.Location = new System.Drawing.Point(0, 0);
            this.ControlSettings.Name = "ControlSettings";
            this.ControlSettings.Size = new System.Drawing.Size(108, 271);
            this.ControlSettings.Style = DevComponents.DotNetBar.eDotNetBarStyle.Windows7;
            this.ControlSettings.TabIndex = 7;
            this.ControlSettings.Text = "itemPanel1";
            // 
            // CBTopMost
            // 
            this.CBTopMost.Name = "CBTopMost";
            this.CBTopMost.Text = "Top Most";
            this.CBTopMost.Click += new System.EventHandler(this.CbTopMostClick);
            // 
            // RadarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(501, 271);
            this.Controls.Add(this.expandableSplitter1);
            this.Controls.Add(this.ControlSettings);
            this.DoubleBuffered = true;
            this.Name = "RadarForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapControlFormClosed);
            this.Load += new System.EventHandler(this.MapControlLoad);
            this.ResizeBegin += new System.EventHandler(this.MapControlResizeBegin);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MapMouseClick);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MapMouseWheel);
            this.Resize += new System.EventHandler(this.MapControlResize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer MapTimer;
        private DevComponents.DotNetBar.ExpandableSplitter expandableSplitter1;
        private DevComponents.DotNetBar.ItemPanel ControlSettings;
        private DevComponents.DotNetBar.CheckBoxItem CBTopMost;

    }
}
