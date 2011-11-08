namespace LazyEvo.LFlyingEngine
{
    partial class FlyingProfiles
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ProfileList = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.BtnRefresh = new DevComponents.DotNetBar.ButtonX();
            this.BtnDownload = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.BtnUpload = new DevComponents.DotNetBar.ButtonX();
            this.TBName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.TBComment = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.TBZone = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.BtnBrowse = new DevComponents.DotNetBar.ButtonX();
            this.TBProfile = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.TBImage = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.BtnBrowseImage = new DevComponents.DotNetBar.ButtonX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.ProfileList)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProfileList
            // 
            this.ProfileList.AllowUserToAddRows = false;
            this.ProfileList.AllowUserToDeleteRows = false;
            this.ProfileList.AllowUserToResizeRows = false;
            this.ProfileList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ProfileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ProfileList.DefaultCellStyle = dataGridViewCellStyle2;
            this.ProfileList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.ProfileList.Location = new System.Drawing.Point(5, 3);
            this.ProfileList.MultiSelect = false;
            this.ProfileList.Name = "ProfileList";
            this.ProfileList.ReadOnly = true;
            this.ProfileList.RowHeadersVisible = false;
            this.ProfileList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProfileList.ShowCellErrors = false;
            this.ProfileList.ShowCellToolTips = false;
            this.ProfileList.ShowEditingIcon = false;
            this.ProfileList.ShowRowErrors = false;
            this.ProfileList.Size = new System.Drawing.Size(811, 181);
            this.ProfileList.TabIndex = 4;
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnRefresh.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.BtnRefresh.Location = new System.Drawing.Point(104, 193);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(75, 23);
            this.BtnRefresh.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnRefresh.TabIndex = 5;
            this.BtnRefresh.Text = "Refresh";
            this.BtnRefresh.Click += new System.EventHandler(this.BtnRefreshClick);
            // 
            // BtnDownload
            // 
            this.BtnDownload.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnDownload.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.BtnDownload.Location = new System.Drawing.Point(185, 193);
            this.BtnDownload.Name = "BtnDownload";
            this.BtnDownload.Size = new System.Drawing.Size(75, 23);
            this.BtnDownload.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnDownload.TabIndex = 6;
            this.BtnDownload.Text = "Download";
            this.BtnDownload.Click += new System.EventHandler(this.BtnDownloadClick);
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.BtnUpload);
            this.groupPanel1.Controls.Add(this.TBName);
            this.groupPanel1.Controls.Add(this.labelX4);
            this.groupPanel1.Controls.Add(this.TBComment);
            this.groupPanel1.Controls.Add(this.labelX3);
            this.groupPanel1.Controls.Add(this.TBZone);
            this.groupPanel1.Controls.Add(this.labelX2);
            this.groupPanel1.Controls.Add(this.BtnBrowse);
            this.groupPanel1.Controls.Add(this.TBProfile);
            this.groupPanel1.Controls.Add(this.labelX1);
            this.groupPanel1.Location = new System.Drawing.Point(103, 219);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(603, 110);
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
            this.groupPanel1.TabIndex = 7;
            this.groupPanel1.Text = "Add profile";
            // 
            // BtnUpload
            // 
            this.BtnUpload.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnUpload.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.BtnUpload.Location = new System.Drawing.Point(524, 60);
            this.BtnUpload.Name = "BtnUpload";
            this.BtnUpload.Size = new System.Drawing.Size(72, 23);
            this.BtnUpload.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnUpload.TabIndex = 18;
            this.BtnUpload.Text = "Upload";
            this.BtnUpload.Click += new System.EventHandler(this.BtnUploadClick);
            // 
            // TBName
            // 
            // 
            // 
            // 
            this.TBName.Border.Class = "TextBoxBorder";
            this.TBName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.TBName.Location = new System.Drawing.Point(78, 35);
            this.TBName.Name = "TBName";
            this.TBName.Size = new System.Drawing.Size(165, 20);
            this.TBName.TabIndex = 13;
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX4.BackgroundStyle.Class = "";
            this.labelX4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX4.Location = new System.Drawing.Point(3, 31);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(40, 26);
            this.labelX4.TabIndex = 12;
            this.labelX4.Text = "Name:";
            // 
            // TBComment
            // 
            // 
            // 
            // 
            this.TBComment.Border.Class = "TextBoxBorder";
            this.TBComment.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.TBComment.Location = new System.Drawing.Point(78, 63);
            this.TBComment.Name = "TBComment";
            this.TBComment.Size = new System.Drawing.Size(395, 20);
            this.TBComment.TabIndex = 11;
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(3, 60);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(69, 23);
            this.labelX3.TabIndex = 10;
            this.labelX3.Text = "Comment:";
            // 
            // TBZone
            // 
            // 
            // 
            // 
            this.TBZone.Border.Class = "TextBoxBorder";
            this.TBZone.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.TBZone.Location = new System.Drawing.Point(302, 35);
            this.TBZone.Name = "TBZone";
            this.TBZone.Size = new System.Drawing.Size(171, 20);
            this.TBZone.TabIndex = 9;
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(252, 33);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(48, 23);
            this.labelX2.TabIndex = 8;
            this.labelX2.Text = "Zone(s):";
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnBrowse.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.BtnBrowse.Location = new System.Drawing.Point(479, 4);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(75, 23);
            this.BtnBrowse.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnBrowse.TabIndex = 7;
            this.BtnBrowse.Text = "Browse";
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowseClick);
            // 
            // TBProfile
            // 
            // 
            // 
            // 
            this.TBProfile.Border.Class = "TextBoxBorder";
            this.TBProfile.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.TBProfile.Enabled = false;
            this.TBProfile.Location = new System.Drawing.Point(78, 7);
            this.TBProfile.Name = "TBProfile";
            this.TBProfile.Size = new System.Drawing.Size(395, 20);
            this.TBProfile.TabIndex = 1;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(3, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(40, 26);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Profile:";
            // 
            // TBImage
            // 
            // 
            // 
            // 
            this.TBImage.Border.Class = "TextBoxBorder";
            this.TBImage.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.TBImage.Enabled = false;
            this.TBImage.Location = new System.Drawing.Point(552, 196);
            this.TBImage.Name = "TBImage";
            this.TBImage.Size = new System.Drawing.Size(28, 20);
            this.TBImage.TabIndex = 17;
            this.TBImage.Visible = false;
            // 
            // BtnBrowseImage
            // 
            this.BtnBrowseImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnBrowseImage.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.BtnBrowseImage.Location = new System.Drawing.Point(628, 193);
            this.BtnBrowseImage.Name = "BtnBrowseImage";
            this.BtnBrowseImage.Size = new System.Drawing.Size(75, 23);
            this.BtnBrowseImage.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnBrowseImage.TabIndex = 16;
            this.BtnBrowseImage.Text = "Browse";
            this.BtnBrowseImage.Visible = false;
            this.BtnBrowseImage.Click += new System.EventHandler(this.BtnBrowseImageClick);
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX5.BackgroundStyle.Class = "";
            this.labelX5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX5.Location = new System.Drawing.Point(586, 190);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(40, 26);
            this.labelX5.TabIndex = 14;
            this.labelX5.Text = "Image:";
            this.labelX5.Visible = false;
            // 
            // FlyingProfiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(821, 334);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.TBImage);
            this.Controls.Add(this.BtnDownload);
            this.Controls.Add(this.BtnBrowseImage);
            this.Controls.Add(this.BtnRefresh);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.ProfileList);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(837, 372);
            this.Name = "FlyingProfiles";
            this.Text = "Flying profiles";
            ((System.ComponentModel.ISupportInitialize)(this.ProfileList)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.Controls.DataGridViewX ProfileList;
        private DevComponents.DotNetBar.ButtonX BtnRefresh;
        private DevComponents.DotNetBar.ButtonX BtnDownload;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.ButtonX BtnBrowse;
        private DevComponents.DotNetBar.Controls.TextBoxX TBProfile;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.TextBoxX TBZone;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX TBComment;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.TextBoxX TBName;
        private DevComponents.DotNetBar.ButtonX BtnBrowseImage;
        private DevComponents.DotNetBar.Controls.TextBoxX TBImage;
        private DevComponents.DotNetBar.ButtonX BtnUpload;
    }
}