namespace LazyEvo.LGrindEngine
{
    partial class Settings
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
            this.SaveSettings = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel10 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.UseMount = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel9 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.GrinderCBWaitForLoot = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.GrinderCBJumpRandomly = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.GrinderCBSkin = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.GrinderCBStopLootOnFull = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.GrinderCBLoot = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel8 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.labelX44 = new DevComponents.DotNetBar.LabelX();
            this.GrinderIntSkipAddsDis = new DevComponents.Editors.IntegerInput();
            this.labelX46 = new DevComponents.DotNetBar.LabelX();
            this.labelX45 = new DevComponents.DotNetBar.LabelX();
            this.GrinderIntAppRange = new DevComponents.Editors.IntegerInput();
            this.GrinderIntSkipAddMaxCount = new DevComponents.Editors.IntegerInput();
            this.GrinderCBSkipPullOnAdds = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.CBTrain = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.groupPanel10.SuspendLayout();
            this.groupPanel9.SuspendLayout();
            this.groupPanel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrinderIntSkipAddsDis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrinderIntAppRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrinderIntSkipAddMaxCount)).BeginInit();
            this.groupPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Windows7Blue;
            // 
            // SaveSettings
            // 
            this.SaveSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.SaveSettings.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.SaveSettings.Location = new System.Drawing.Point(159, 239);
            this.SaveSettings.Name = "SaveSettings";
            this.SaveSettings.Size = new System.Drawing.Size(101, 32);
            this.SaveSettings.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.SaveSettings.TabIndex = 138;
            this.SaveSettings.Text = "Save and close";
            this.SaveSettings.Click += new System.EventHandler(this.SaveSettingsClick);
            // 
            // groupPanel10
            // 
            this.groupPanel10.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel10.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel10.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel10.Controls.Add(this.UseMount);
            this.groupPanel10.Location = new System.Drawing.Point(214, 179);
            this.groupPanel10.Name = "groupPanel10";
            this.groupPanel10.Size = new System.Drawing.Size(213, 56);
            // 
            // 
            // 
            this.groupPanel10.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel10.Style.BackColorGradientAngle = 90;
            this.groupPanel10.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel10.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel10.Style.BorderBottomWidth = 1;
            this.groupPanel10.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel10.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel10.Style.BorderLeftWidth = 1;
            this.groupPanel10.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel10.Style.BorderRightWidth = 1;
            this.groupPanel10.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel10.Style.BorderTopWidth = 1;
            this.groupPanel10.Style.Class = "";
            this.groupPanel10.Style.CornerDiameter = 4;
            this.groupPanel10.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel10.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel10.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel10.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel10.StyleMouseDown.Class = "";
            this.groupPanel10.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel10.StyleMouseOver.Class = "";
            this.groupPanel10.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel10.TabIndex = 141;
            this.groupPanel10.Text = "Mount options";
            // 
            // UseMount
            // 
            this.UseMount.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.UseMount.BackgroundStyle.Class = "";
            this.UseMount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.UseMount.Location = new System.Drawing.Point(3, 6);
            this.UseMount.Name = "UseMount";
            this.UseMount.Size = new System.Drawing.Size(83, 23);
            this.UseMount.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.UseMount.TabIndex = 47;
            this.UseMount.Text = "Use mount";
            // 
            // groupPanel9
            // 
            this.groupPanel9.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel9.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel9.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel9.Controls.Add(this.GrinderCBWaitForLoot);
            this.groupPanel9.Controls.Add(this.GrinderCBJumpRandomly);
            this.groupPanel9.Controls.Add(this.GrinderCBSkin);
            this.groupPanel9.Controls.Add(this.GrinderCBStopLootOnFull);
            this.groupPanel9.Controls.Add(this.GrinderCBLoot);
            this.groupPanel9.Location = new System.Drawing.Point(4, 3);
            this.groupPanel9.Name = "groupPanel9";
            this.groupPanel9.Size = new System.Drawing.Size(204, 170);
            // 
            // 
            // 
            this.groupPanel9.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel9.Style.BackColorGradientAngle = 90;
            this.groupPanel9.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel9.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel9.Style.BorderBottomWidth = 1;
            this.groupPanel9.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel9.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel9.Style.BorderLeftWidth = 1;
            this.groupPanel9.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel9.Style.BorderRightWidth = 1;
            this.groupPanel9.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel9.Style.BorderTopWidth = 1;
            this.groupPanel9.Style.Class = "";
            this.groupPanel9.Style.CornerDiameter = 4;
            this.groupPanel9.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel9.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel9.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel9.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel9.StyleMouseDown.Class = "";
            this.groupPanel9.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel9.StyleMouseOver.Class = "";
            this.groupPanel9.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel9.TabIndex = 140;
            this.groupPanel9.Text = "General";
            // 
            // GrinderCBWaitForLoot
            // 
            this.GrinderCBWaitForLoot.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.GrinderCBWaitForLoot.BackgroundStyle.Class = "";
            this.GrinderCBWaitForLoot.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GrinderCBWaitForLoot.Location = new System.Drawing.Point(3, 32);
            this.GrinderCBWaitForLoot.Name = "GrinderCBWaitForLoot";
            this.GrinderCBWaitForLoot.Size = new System.Drawing.Size(162, 23);
            this.GrinderCBWaitForLoot.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.GrinderCBWaitForLoot.TabIndex = 11;
            this.GrinderCBWaitForLoot.Text = "Wait for loot";
            // 
            // GrinderCBJumpRandomly
            // 
            this.GrinderCBJumpRandomly.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.GrinderCBJumpRandomly.BackgroundStyle.Class = "";
            this.GrinderCBJumpRandomly.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GrinderCBJumpRandomly.Location = new System.Drawing.Point(3, 119);
            this.GrinderCBJumpRandomly.Name = "GrinderCBJumpRandomly";
            this.GrinderCBJumpRandomly.Size = new System.Drawing.Size(162, 23);
            this.GrinderCBJumpRandomly.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.GrinderCBJumpRandomly.TabIndex = 10;
            this.GrinderCBJumpRandomly.Text = "Jump randomly";
            // 
            // GrinderCBSkin
            // 
            this.GrinderCBSkin.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.GrinderCBSkin.BackgroundStyle.Class = "";
            this.GrinderCBSkin.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GrinderCBSkin.Location = new System.Drawing.Point(3, 90);
            this.GrinderCBSkin.Name = "GrinderCBSkin";
            this.GrinderCBSkin.Size = new System.Drawing.Size(162, 23);
            this.GrinderCBSkin.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.GrinderCBSkin.TabIndex = 8;
            this.GrinderCBSkin.Text = "Skin";
            // 
            // GrinderCBStopLootOnFull
            // 
            this.GrinderCBStopLootOnFull.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.GrinderCBStopLootOnFull.BackgroundStyle.Class = "";
            this.GrinderCBStopLootOnFull.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GrinderCBStopLootOnFull.Location = new System.Drawing.Point(3, 61);
            this.GrinderCBStopLootOnFull.Name = "GrinderCBStopLootOnFull";
            this.GrinderCBStopLootOnFull.Size = new System.Drawing.Size(162, 23);
            this.GrinderCBStopLootOnFull.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.GrinderCBStopLootOnFull.TabIndex = 9;
            this.GrinderCBStopLootOnFull.Text = "Stop loot on full bags";
            // 
            // GrinderCBLoot
            // 
            this.GrinderCBLoot.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.GrinderCBLoot.BackgroundStyle.Class = "";
            this.GrinderCBLoot.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GrinderCBLoot.Location = new System.Drawing.Point(3, 3);
            this.GrinderCBLoot.Name = "GrinderCBLoot";
            this.GrinderCBLoot.Size = new System.Drawing.Size(162, 23);
            this.GrinderCBLoot.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.GrinderCBLoot.TabIndex = 7;
            this.GrinderCBLoot.Text = "Loot";
            // 
            // groupPanel8
            // 
            this.groupPanel8.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel8.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel8.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel8.Controls.Add(this.labelX44);
            this.groupPanel8.Controls.Add(this.GrinderIntSkipAddsDis);
            this.groupPanel8.Controls.Add(this.labelX46);
            this.groupPanel8.Controls.Add(this.labelX45);
            this.groupPanel8.Controls.Add(this.GrinderIntAppRange);
            this.groupPanel8.Controls.Add(this.GrinderIntSkipAddMaxCount);
            this.groupPanel8.Controls.Add(this.GrinderCBSkipPullOnAdds);
            this.groupPanel8.Location = new System.Drawing.Point(214, 3);
            this.groupPanel8.Name = "groupPanel8";
            this.groupPanel8.Size = new System.Drawing.Size(213, 170);
            // 
            // 
            // 
            this.groupPanel8.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel8.Style.BackColorGradientAngle = 90;
            this.groupPanel8.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel8.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel8.Style.BorderBottomWidth = 1;
            this.groupPanel8.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel8.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel8.Style.BorderLeftWidth = 1;
            this.groupPanel8.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel8.Style.BorderRightWidth = 1;
            this.groupPanel8.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel8.Style.BorderTopWidth = 1;
            this.groupPanel8.Style.Class = "";
            this.groupPanel8.Style.CornerDiameter = 4;
            this.groupPanel8.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel8.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel8.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel8.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel8.StyleMouseDown.Class = "";
            this.groupPanel8.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel8.StyleMouseOver.Class = "";
            this.groupPanel8.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel8.TabIndex = 139;
            this.groupPanel8.Text = "Limits";
            // 
            // labelX44
            // 
            this.labelX44.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX44.BackgroundStyle.Class = "";
            this.labelX44.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX44.Location = new System.Drawing.Point(3, 3);
            this.labelX44.Name = "labelX44";
            this.labelX44.Size = new System.Drawing.Size(92, 23);
            this.labelX44.TabIndex = 0;
            this.labelX44.Text = "Approach range:";
            // 
            // GrinderIntSkipAddsDis
            // 
            // 
            // 
            // 
            this.GrinderIntSkipAddsDis.BackgroundStyle.Class = "DateTimeInputBackground";
            this.GrinderIntSkipAddsDis.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GrinderIntSkipAddsDis.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.GrinderIntSkipAddsDis.Location = new System.Drawing.Point(110, 61);
            this.GrinderIntSkipAddsDis.Name = "GrinderIntSkipAddsDis";
            this.GrinderIntSkipAddsDis.ShowUpDown = true;
            this.GrinderIntSkipAddsDis.Size = new System.Drawing.Size(55, 20);
            this.GrinderIntSkipAddsDis.TabIndex = 4;
            // 
            // labelX46
            // 
            this.labelX46.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX46.BackgroundStyle.Class = "";
            this.labelX46.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX46.Location = new System.Drawing.Point(3, 84);
            this.labelX46.Name = "labelX46";
            this.labelX46.Size = new System.Drawing.Size(92, 23);
            this.labelX46.TabIndex = 5;
            this.labelX46.Text = "Max adds count:";
            // 
            // labelX45
            // 
            this.labelX45.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX45.BackgroundStyle.Class = "";
            this.labelX45.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX45.Location = new System.Drawing.Point(3, 58);
            this.labelX45.Name = "labelX45";
            this.labelX45.Size = new System.Drawing.Size(92, 23);
            this.labelX45.TabIndex = 3;
            this.labelX45.Text = "Distance:";
            // 
            // GrinderIntAppRange
            // 
            // 
            // 
            // 
            this.GrinderIntAppRange.BackgroundStyle.Class = "DateTimeInputBackground";
            this.GrinderIntAppRange.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GrinderIntAppRange.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.GrinderIntAppRange.Location = new System.Drawing.Point(110, 6);
            this.GrinderIntAppRange.Name = "GrinderIntAppRange";
            this.GrinderIntAppRange.ShowUpDown = true;
            this.GrinderIntAppRange.Size = new System.Drawing.Size(55, 20);
            this.GrinderIntAppRange.TabIndex = 1;
            // 
            // GrinderIntSkipAddMaxCount
            // 
            // 
            // 
            // 
            this.GrinderIntSkipAddMaxCount.BackgroundStyle.Class = "DateTimeInputBackground";
            this.GrinderIntSkipAddMaxCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GrinderIntSkipAddMaxCount.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.GrinderIntSkipAddMaxCount.Location = new System.Drawing.Point(110, 87);
            this.GrinderIntSkipAddMaxCount.Name = "GrinderIntSkipAddMaxCount";
            this.GrinderIntSkipAddMaxCount.ShowUpDown = true;
            this.GrinderIntSkipAddMaxCount.Size = new System.Drawing.Size(55, 20);
            this.GrinderIntSkipAddMaxCount.TabIndex = 6;
            // 
            // GrinderCBSkipPullOnAdds
            // 
            this.GrinderCBSkipPullOnAdds.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.GrinderCBSkipPullOnAdds.BackgroundStyle.Class = "";
            this.GrinderCBSkipPullOnAdds.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.GrinderCBSkipPullOnAdds.Location = new System.Drawing.Point(3, 32);
            this.GrinderCBSkipPullOnAdds.Name = "GrinderCBSkipPullOnAdds";
            this.GrinderCBSkipPullOnAdds.Size = new System.Drawing.Size(162, 23);
            this.GrinderCBSkipPullOnAdds.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.GrinderCBSkipPullOnAdds.TabIndex = 2;
            this.GrinderCBSkipPullOnAdds.Text = "Skip mob with adds (pull)";
            // 
            // groupPanel1
            // 
            this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel1.Controls.Add(this.CBTrain);
            this.groupPanel1.Location = new System.Drawing.Point(4, 179);
            this.groupPanel1.Name = "groupPanel1";
            this.groupPanel1.Size = new System.Drawing.Size(204, 56);
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
            this.groupPanel1.TabIndex = 142;
            this.groupPanel1.Text = "Train options";
            // 
            // CBTrain
            // 
            this.CBTrain.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.CBTrain.BackgroundStyle.Class = "";
            this.CBTrain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.CBTrain.Location = new System.Drawing.Point(3, 6);
            this.CBTrain.Name = "CBTrain";
            this.CBTrain.Size = new System.Drawing.Size(83, 23);
            this.CBTrain.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.CBTrain.TabIndex = 47;
            this.CBTrain.Text = "Train";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(234)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(434, 274);
            this.Controls.Add(this.groupPanel1);
            this.Controls.Add(this.groupPanel10);
            this.Controls.Add(this.groupPanel9);
            this.Controls.Add(this.groupPanel8);
            this.Controls.Add(this.SaveSettings);
            this.DoubleBuffered = true;
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.SettingsLoad);
            this.groupPanel10.ResumeLayout(false);
            this.groupPanel9.ResumeLayout(false);
            this.groupPanel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrinderIntSkipAddsDis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrinderIntAppRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrinderIntSkipAddMaxCount)).EndInit();
            this.groupPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.ButtonX SaveSettings;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel10;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel9;
        private DevComponents.DotNetBar.Controls.CheckBoxX GrinderCBWaitForLoot;
        private DevComponents.DotNetBar.Controls.CheckBoxX GrinderCBJumpRandomly;
        private DevComponents.DotNetBar.Controls.CheckBoxX GrinderCBSkin;
        protected internal DevComponents.DotNetBar.Controls.CheckBoxX GrinderCBStopLootOnFull;
        private DevComponents.DotNetBar.Controls.CheckBoxX GrinderCBLoot;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel8;
        private DevComponents.DotNetBar.LabelX labelX44;
        private DevComponents.Editors.IntegerInput GrinderIntSkipAddsDis;
        private DevComponents.DotNetBar.LabelX labelX46;
        private DevComponents.DotNetBar.LabelX labelX45;
        private DevComponents.Editors.IntegerInput GrinderIntAppRange;
        private DevComponents.Editors.IntegerInput GrinderIntSkipAddMaxCount;
        private DevComponents.DotNetBar.Controls.CheckBoxX GrinderCBSkipPullOnAdds;
        private DevComponents.DotNetBar.Controls.CheckBoxX UseMount;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.CheckBoxX CBTrain;
    }
}