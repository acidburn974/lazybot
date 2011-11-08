namespace LazyEvo.PVEBehavior.Builders
{
    partial class DeathknightBuilder
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
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.RBSpec3 = new System.Windows.Forms.RadioButton();
            this.RBSpec2 = new System.Windows.Forms.RadioButton();
            this.RBSpec1 = new System.Windows.Forms.RadioButton();
            this.Spec3 = new System.Windows.Forms.CheckedListBox();
            this.Spec2 = new System.Windows.Forms.CheckedListBox();
            this.Spec1 = new System.Windows.Forms.CheckedListBox();
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.BeGlobalCooldown = new DevComponents.Editors.IntegerInput();
            this.labelX25 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.CBSelectSpecial = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.Normal = new System.Windows.Forms.CheckedListBox();
            this.BtnCreate = new DevComponents.DotNetBar.ButtonX();
            this.TBName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.groupPanel1.SuspendLayout();
            this.groupPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BeGlobalCooldown)).BeginInit();
            this.groupPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel1
            // 
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.RBSpec3);
            this.groupPanel1.Controls.Add(this.RBSpec2);
            this.groupPanel1.Controls.Add(this.RBSpec1);
            this.groupPanel1.Controls.Add(this.Spec3);
            this.groupPanel1.Controls.Add(this.Spec2);
            this.groupPanel1.Controls.Add(this.Spec1);
            this.groupPanel1.Location = new System.Drawing.Point(7, 4);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(394, 262);
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
            this.groupPanel1.Text = "Talents";
            // 
            // RBSpec3
            // 
            this.RBSpec3.AutoSize = true;
            this.RBSpec3.BackColor = System.Drawing.Color.Transparent;
            this.RBSpec3.Location = new System.Drawing.Point(261, 3);
            this.RBSpec3.Name = "RBSpec3";
            this.RBSpec3.Size = new System.Drawing.Size(58, 17);
            this.RBSpec3.TabIndex = 8;
            this.RBSpec3.TabStop = true;
            this.RBSpec3.Text = "Unholy";
            this.RBSpec3.UseVisualStyleBackColor = false;
            this.RBSpec3.CheckedChanged += new System.EventHandler(this.RbSpecChanged);
            // 
            // RBSpec2
            // 
            this.RBSpec2.AutoSize = true;
            this.RBSpec2.BackColor = System.Drawing.Color.Transparent;
            this.RBSpec2.Location = new System.Drawing.Point(131, 3);
            this.RBSpec2.Name = "RBSpec2";
            this.RBSpec2.Size = new System.Drawing.Size(48, 17);
            this.RBSpec2.TabIndex = 7;
            this.RBSpec2.TabStop = true;
            this.RBSpec2.Text = "Frost";
            this.RBSpec2.UseVisualStyleBackColor = false;
            this.RBSpec2.CheckedChanged += new System.EventHandler(this.RbSpecChanged);
            // 
            // RBSpec1
            // 
            this.RBSpec1.AutoSize = true;
            this.RBSpec1.BackColor = System.Drawing.Color.Transparent;
            this.RBSpec1.Location = new System.Drawing.Point(3, 3);
            this.RBSpec1.Name = "RBSpec1";
            this.RBSpec1.Size = new System.Drawing.Size(52, 17);
            this.RBSpec1.TabIndex = 6;
            this.RBSpec1.TabStop = true;
            this.RBSpec1.Text = "Blood";
            this.RBSpec1.UseVisualStyleBackColor = false;
            this.RBSpec1.CheckedChanged += new System.EventHandler(this.RbSpecChanged);
            // 
            // Spec3
            // 
            this.Spec3.FormattingEnabled = true;
            this.Spec3.Location = new System.Drawing.Point(260, 23);
            this.Spec3.Name = "Spec3";
            this.Spec3.Size = new System.Drawing.Size(123, 214);
            this.Spec3.TabIndex = 2;
            // 
            // Spec2
            // 
            this.Spec2.FormattingEnabled = true;
            this.Spec2.Location = new System.Drawing.Point(131, 23);
            this.Spec2.Name = "Spec2";
            this.Spec2.Size = new System.Drawing.Size(123, 214);
            this.Spec2.TabIndex = 1;
            // 
            // Spec1
            // 
            this.Spec1.FormattingEnabled = true;
            this.Spec1.Location = new System.Drawing.Point(2, 23);
            this.Spec1.Name = "Spec1";
            this.Spec1.Size = new System.Drawing.Size(123, 214);
            this.Spec1.TabIndex = 0;
            // 
            // groupPanel2
            // 
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.BeGlobalCooldown);
            this.groupPanel2.Controls.Add(this.labelX25);
            this.groupPanel2.Controls.Add(this.labelX1);
            this.groupPanel2.Controls.Add(this.CBSelectSpecial);
            this.groupPanel2.Location = new System.Drawing.Point(7, 272);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(394, 44);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.Class = "";
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseDown.Class = "";
            this.groupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseOver.Class = "";
            this.groupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel2.TabIndex = 1;
            // 
            // BeGlobalCooldown
            // 
            // 
            // 
            // 
            this.BeGlobalCooldown.BackgroundStyle.Class = "DateTimeInputBackground";
            this.BeGlobalCooldown.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.BeGlobalCooldown.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.BeGlobalCooldown.Location = new System.Drawing.Point(321, 7);
            this.BeGlobalCooldown.Name = "BeGlobalCooldown";
            this.BeGlobalCooldown.ShowUpDown = true;
            this.BeGlobalCooldown.Size = new System.Drawing.Size(64, 20);
            this.BeGlobalCooldown.TabIndex = 88;
            this.BeGlobalCooldown.Value = 2000;
            // 
            // labelX25
            // 
            this.labelX25.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX25.BackgroundStyle.Class = "";
            this.labelX25.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX25.Location = new System.Drawing.Point(227, 2);
            this.labelX25.Name = "labelX25";
            this.labelX25.Size = new System.Drawing.Size(91, 27);
            this.labelX25.TabIndex = 87;
            this.labelX25.Text = "Global cooldown:";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(2, 4);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(48, 23);
            this.labelX1.TabIndex = 78;
            this.labelX1.Tag = "Select pre";
            this.labelX1.Text = "Presence";
            // 
            // CBSelectSpecial
            // 
            this.CBSelectSpecial.DisplayMember = "Text";
            this.CBSelectSpecial.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CBSelectSpecial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBSelectSpecial.FormattingEnabled = true;
            this.CBSelectSpecial.ItemHeight = 14;
            this.CBSelectSpecial.Location = new System.Drawing.Point(56, 7);
            this.CBSelectSpecial.Name = "CBSelectSpecial";
            this.CBSelectSpecial.Size = new System.Drawing.Size(123, 20);
            this.CBSelectSpecial.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.CBSelectSpecial.TabIndex = 77;
            // 
            // groupPanel3
            // 
            this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel3.Controls.Add(this.Normal);
            this.groupPanel3.Location = new System.Drawing.Point(407, 4);
            this.groupPanel3.Name = "groupPanel3";
            this.groupPanel3.Size = new System.Drawing.Size(183, 312);
            // 
            // 
            // 
            this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel3.Style.BackColorGradientAngle = 90;
            this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderBottomWidth = 1;
            this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderLeftWidth = 1;
            this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderRightWidth = 1;
            this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel3.Style.BorderTopWidth = 1;
            this.groupPanel3.Style.Class = "";
            this.groupPanel3.Style.CornerDiameter = 4;
            this.groupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel3.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel3.StyleMouseDown.Class = "";
            this.groupPanel3.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel3.StyleMouseOver.Class = "";
            this.groupPanel3.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel3.TabIndex = 2;
            this.groupPanel3.Text = "Normal";
            // 
            // Normal
            // 
            this.Normal.FormattingEnabled = true;
            this.Normal.Location = new System.Drawing.Point(3, 15);
            this.Normal.Name = "Normal";
            this.Normal.Size = new System.Drawing.Size(171, 274);
            this.Normal.TabIndex = 3;
            // 
            // BtnCreate
            // 
            this.BtnCreate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnCreate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.BtnCreate.Location = new System.Drawing.Point(515, 318);
            this.BtnCreate.Name = "BtnCreate";
            this.BtnCreate.Size = new System.Drawing.Size(75, 23);
            this.BtnCreate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnCreate.TabIndex = 3;
            this.BtnCreate.Text = "Create";
            this.BtnCreate.Click += new System.EventHandler(this.BtnCreateClick);
            // 
            // TBName
            // 
            // 
            // 
            // 
            this.TBName.Border.Class = "TextBoxBorder";
            this.TBName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.TBName.Location = new System.Drawing.Point(335, 320);
            this.TBName.Name = "TBName";
            this.TBName.Size = new System.Drawing.Size(176, 20);
            this.TBName.TabIndex = 4;
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(293, 323);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(38, 17);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "Name:";
            // 
            // DeathknightBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(593, 349);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.TBName);
            this.Controls.Add(this.BtnCreate);
            this.Controls.Add(this.groupPanel3);
            this.Controls.Add(this.groupPanel2);
            this.Controls.Add(this.groupPanel1);
            this.DoubleBuffered = true;
            this.Name = "DeathknightBuilder";
            this.Load += new System.EventHandler(this.DeathknightBuilderLoad);
            this.groupPanel1.ResumeLayout(false);
            this.groupPanel1.PerformLayout();
            this.groupPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BeGlobalCooldown)).EndInit();
            this.groupPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.ButtonX BtnCreate;
        private System.Windows.Forms.CheckedListBox Spec3;
        private System.Windows.Forms.CheckedListBox Spec2;
        private System.Windows.Forms.CheckedListBox Spec1;
        private System.Windows.Forms.CheckedListBox Normal;
        private DevComponents.DotNetBar.LabelX labelX1;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx CBSelectSpecial;
        private System.Windows.Forms.RadioButton RBSpec3;
        private System.Windows.Forms.RadioButton RBSpec2;
        private System.Windows.Forms.RadioButton RBSpec1;
        private DevComponents.Editors.IntegerInput BeGlobalCooldown;
        private DevComponents.DotNetBar.LabelX labelX25;
        private DevComponents.DotNetBar.Controls.TextBoxX TBName;
        private DevComponents.DotNetBar.LabelX labelX2;
    }
}