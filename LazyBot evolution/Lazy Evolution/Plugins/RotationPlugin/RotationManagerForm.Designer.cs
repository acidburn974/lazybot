namespace LazyEvo.Plugins.RotationPlugin
{
    partial class RotationManagerForm
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
            this.groupPanel4 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.BtnAllowScripts = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.BeTBSelectBehavior = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX20 = new DevComponents.DotNetBar.LabelX();
            this.BeTBNewBehavior = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX18 = new DevComponents.DotNetBar.LabelX();
            this.BeTabs = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.BeRotations = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.TabRotations = new DevComponents.DotNetBar.SuperTabItem();
            this.labelItem3 = new DevComponents.DotNetBar.LabelItem();
            this.BeSaveBeheavior = new DevComponents.DotNetBar.ButtonItem();
            this.BeBarRuleModifier = new DevComponents.DotNetBar.Bar();
            this.labelItem1 = new DevComponents.DotNetBar.LabelItem();
            this.BeComAddRule = new DevComponents.DotNetBar.ButtonItem();
            this.labelItem4 = new DevComponents.DotNetBar.LabelItem();
            this.BeComEditRule = new DevComponents.DotNetBar.ButtonItem();
            this.labelItem2 = new DevComponents.DotNetBar.LabelItem();
            this.BeComDeleteRule = new DevComponents.DotNetBar.ButtonItem();
            this.BtnSave = new DevComponents.DotNetBar.ButtonX();
            this.BtnSaveAndClose = new DevComponents.DotNetBar.ButtonX();
            this.BtnCopy = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BeTabs)).BeginInit();
            this.BeTabs.SuspendLayout();
            this.superTabControlPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BeRotations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BeBarRuleModifier)).BeginInit();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Windows7Blue;
            // 
            // groupPanel4
            // 
            this.groupPanel4.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel4.Controls.Add(this.BtnAllowScripts);
            this.groupPanel4.Controls.Add(this.BeTBSelectBehavior);
            this.groupPanel4.Controls.Add(this.labelX20);
            this.groupPanel4.Controls.Add(this.BeTBNewBehavior);
            this.groupPanel4.Controls.Add(this.labelX18);
            this.groupPanel4.Location = new System.Drawing.Point(3, 0);
            this.groupPanel4.Name = "groupPanel4";
            this.groupPanel4.Size = new System.Drawing.Size(532, 97);
            // 
            // 
            // 
            this.groupPanel4.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel4.Style.BackColorGradientAngle = 90;
            this.groupPanel4.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel4.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderBottomWidth = 1;
            this.groupPanel4.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel4.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderLeftWidth = 1;
            this.groupPanel4.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderRightWidth = 1;
            this.groupPanel4.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel4.Style.BorderTopWidth = 1;
            this.groupPanel4.Style.Class = "";
            this.groupPanel4.Style.CornerDiameter = 4;
            this.groupPanel4.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel4.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel4.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel4.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel4.StyleMouseDown.Class = "";
            this.groupPanel4.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel4.StyleMouseOver.Class = "";
            this.groupPanel4.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel4.TabIndex = 11;
            // 
            // BtnAllowScripts
            // 
            this.BtnAllowScripts.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.BtnAllowScripts.BackgroundStyle.Class = "";
            this.BtnAllowScripts.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.BtnAllowScripts.Location = new System.Drawing.Point(427, 60);
            this.BtnAllowScripts.Name = "BtnAllowScripts";
            this.BtnAllowScripts.Size = new System.Drawing.Size(90, 23);
            this.BtnAllowScripts.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnAllowScripts.TabIndex = 10;
            this.BtnAllowScripts.Text = "Allow scripts ";
            this.BtnAllowScripts.CheckedChanged += new System.EventHandler(this.BtnAllowScripts_CheckedChanged);
            // 
            // BeTBSelectBehavior
            // 
            this.BeTBSelectBehavior.DisplayMember = "Text";
            this.BeTBSelectBehavior.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.BeTBSelectBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BeTBSelectBehavior.FormattingEnabled = true;
            this.BeTBSelectBehavior.ItemHeight = 14;
            this.BeTBSelectBehavior.Location = new System.Drawing.Point(140, 34);
            this.BeTBSelectBehavior.Name = "BeTBSelectBehavior";
            this.BeTBSelectBehavior.Size = new System.Drawing.Size(377, 20);
            this.BeTBSelectBehavior.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BeTBSelectBehavior.TabIndex = 47;
            this.BeTBSelectBehavior.SelectedIndexChanged += new System.EventHandler(this.BeTbSelectBehaviorSelectedIndexChanged);
            // 
            // labelX20
            // 
            this.labelX20.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX20.BackgroundStyle.Class = "";
            this.labelX20.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX20.Location = new System.Drawing.Point(3, 32);
            this.labelX20.Name = "labelX20";
            this.labelX20.Size = new System.Drawing.Size(131, 23);
            this.labelX20.TabIndex = 2;
            this.labelX20.Text = "Select rotation manager:";
            // 
            // BeTBNewBehavior
            // 
            // 
            // 
            // 
            this.BeTBNewBehavior.Border.Class = "TextBoxBorder";
            this.BeTBNewBehavior.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.BeTBNewBehavior.Location = new System.Drawing.Point(140, 6);
            this.BeTBNewBehavior.Name = "BeTBNewBehavior";
            this.BeTBNewBehavior.Size = new System.Drawing.Size(377, 20);
            this.BeTBNewBehavior.TabIndex = 1;
            this.BeTBNewBehavior.Text = "Enter name and press return to create new";
            this.BeTBNewBehavior.Click += new System.EventHandler(this.BeTbNewBehaviorClick);
            this.BeTBNewBehavior.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BeTbNewBehaviorPreviewKeyDown);
            // 
            // labelX18
            // 
            this.labelX18.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX18.BackgroundStyle.Class = "";
            this.labelX18.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX18.Location = new System.Drawing.Point(3, 3);
            this.labelX18.Name = "labelX18";
            this.labelX18.Size = new System.Drawing.Size(131, 23);
            this.labelX18.TabIndex = 0;
            this.labelX18.Text = "Create rotation manager:";
            // 
            // BeTabs
            // 
            this.BeTabs.BackColor = System.Drawing.Color.Silver;
            // 
            // 
            // 
            // 
            // 
            // 
            this.BeTabs.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.BeTabs.ControlBox.MenuBox.Name = "";
            this.BeTabs.ControlBox.Name = "";
            this.BeTabs.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.BeTabs.ControlBox.MenuBox,
            this.BeTabs.ControlBox.CloseBox});
            this.BeTabs.Controls.Add(this.superTabControlPanel2);
            this.BeTabs.Location = new System.Drawing.Point(3, 103);
            this.BeTabs.Name = "BeTabs";
            this.BeTabs.ReorderTabsEnabled = true;
            this.BeTabs.SelectedTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.BeTabs.SelectedTabIndex = 0;
            this.BeTabs.Size = new System.Drawing.Size(532, 307);
            this.BeTabs.TabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BeTabs.TabIndex = 10;
            this.BeTabs.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.TabRotations,
            this.labelItem3,
            this.BeSaveBeheavior});
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Controls.Add(this.BeRotations);
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(0, 26);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.Size = new System.Drawing.Size(532, 281);
            this.superTabControlPanel2.TabIndex = 0;
            this.superTabControlPanel2.TabItem = this.TabRotations;
            // 
            // BeRotations
            // 
            this.BeRotations.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.BeRotations.AllowDrop = true;
            this.BeRotations.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.BeRotations.BackgroundStyle.Class = "TreeBorderKey";
            this.BeRotations.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.BeRotations.Dock = System.Windows.Forms.DockStyle.Top;
            this.BeRotations.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.BeRotations.Location = new System.Drawing.Point(0, 0);
            this.BeRotations.Name = "BeRotations";
            this.BeRotations.NodesConnector = this.nodeConnector1;
            this.BeRotations.NodeStyle = this.elementStyle1;
            this.BeRotations.PathSeparator = ";";
            this.BeRotations.Size = new System.Drawing.Size(532, 278);
            this.BeRotations.Styles.Add(this.elementStyle1);
            this.BeRotations.TabIndex = 0;
            this.BeRotations.Text = "advTree1";
            this.BeRotations.NodeDragFeedback += new DevComponents.AdvTree.TreeDragFeedbackEventHander(this.BeComRules_NodeDragFeedback);
            this.BeRotations.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.BeComRulesNodeClick);
            this.BeRotations.NodeDoubleClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.BeComRulesNodeDoubleClick);
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.Class = "";
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // TabRotations
            // 
            this.TabRotations.AttachedControl = this.superTabControlPanel2;
            this.TabRotations.GlobalItem = false;
            this.TabRotations.Name = "TabRotations";
            this.TabRotations.Text = "Rotations";
            // 
            // labelItem3
            // 
            this.labelItem3.Name = "labelItem3";
            this.labelItem3.Text = "                                                                                 " +
                "                                             ";
            // 
            // BeSaveBeheavior
            // 
            this.BeSaveBeheavior.Name = "BeSaveBeheavior";
            this.BeSaveBeheavior.Text = "Save rotations";
            this.BeSaveBeheavior.Click += new System.EventHandler(this.BeSaveBeheaviorClick);
            // 
            // BeBarRuleModifier
            // 
            this.BeBarRuleModifier.AntiAlias = true;
            this.BeBarRuleModifier.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.labelItem1,
            this.BeComAddRule,
            this.labelItem4,
            this.BeComEditRule,
            this.labelItem2,
            this.BeComDeleteRule});
            this.BeBarRuleModifier.Location = new System.Drawing.Point(3, 416);
            this.BeBarRuleModifier.Name = "BeBarRuleModifier";
            this.BeBarRuleModifier.Size = new System.Drawing.Size(532, 25);
            this.BeBarRuleModifier.Stretch = true;
            this.BeBarRuleModifier.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BeBarRuleModifier.TabIndex = 12;
            this.BeBarRuleModifier.TabStop = false;
            this.BeBarRuleModifier.Text = "bar1";
            // 
            // labelItem1
            // 
            this.labelItem1.Name = "labelItem1";
            this.labelItem1.Text = "              ";
            // 
            // BeComAddRule
            // 
            this.BeComAddRule.Name = "BeComAddRule";
            this.BeComAddRule.Text = "Add Rotation";
            this.BeComAddRule.Click += new System.EventHandler(this.BeComAddRuleClick);
            // 
            // labelItem4
            // 
            this.labelItem4.Name = "labelItem4";
            this.labelItem4.Text = "              ";
            // 
            // BeComEditRule
            // 
            this.BeComEditRule.Name = "BeComEditRule";
            this.BeComEditRule.Text = "Double click on rotation to edit";
            // 
            // labelItem2
            // 
            this.labelItem2.Name = "labelItem2";
            this.labelItem2.Text = "              ";
            // 
            // BeComDeleteRule
            // 
            this.BeComDeleteRule.Name = "BeComDeleteRule";
            this.BeComDeleteRule.Text = "Delete Rotation";
            this.BeComDeleteRule.Click += new System.EventHandler(this.BeComDeleteRuleClick);
            // 
            // BtnSave
            // 
            this.BtnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.BtnSave.Location = new System.Drawing.Point(352, 447);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 22);
            this.BtnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnSave.TabIndex = 62;
            this.BtnSave.Text = "Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnSaveAndClose
            // 
            this.BtnSaveAndClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnSaveAndClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.BtnSaveAndClose.Location = new System.Drawing.Point(433, 447);
            this.BtnSaveAndClose.Name = "BtnSaveAndClose";
            this.BtnSaveAndClose.Size = new System.Drawing.Size(102, 22);
            this.BtnSaveAndClose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnSaveAndClose.TabIndex = 63;
            this.BtnSaveAndClose.Text = "Save and close";
            this.BtnSaveAndClose.Click += new System.EventHandler(this.BtnSaveAndClose_Click);
            // 
            // BtnCopy
            // 
            this.BtnCopy.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnCopy.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.BtnCopy.Location = new System.Drawing.Point(3, 447);
            this.BtnCopy.Name = "BtnCopy";
            this.BtnCopy.Size = new System.Drawing.Size(120, 23);
            this.BtnCopy.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnCopy.TabIndex = 64;
            this.BtnCopy.Text = "Copy selected rotation";
            this.BtnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // RotationManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(540, 475);
            this.Controls.Add(this.BtnCopy);
            this.Controls.Add(this.BtnSaveAndClose);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.groupPanel4);
            this.Controls.Add(this.BeTabs);
            this.Controls.Add(this.BeBarRuleModifier);
            this.DoubleBuffered = true;
            this.Name = "RotationManagerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BehaviorFormFormClosing);
            this.Load += new System.EventHandler(this.BehaviorFormLoad);
            this.groupPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BeTabs)).EndInit();
            this.BeTabs.ResumeLayout(false);
            this.superTabControlPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BeRotations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BeBarRuleModifier)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel4;
        internal DevComponents.DotNetBar.Controls.ComboBoxEx BeTBSelectBehavior;
        private DevComponents.DotNetBar.LabelX labelX20;
        private DevComponents.DotNetBar.Controls.TextBoxX BeTBNewBehavior;
        private DevComponents.DotNetBar.LabelX labelX18;
        private DevComponents.DotNetBar.SuperTabControl BeTabs;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.AdvTree.AdvTree BeRotations;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.SuperTabItem TabRotations;
        private DevComponents.DotNetBar.LabelItem labelItem3;
        private DevComponents.DotNetBar.ButtonItem BeSaveBeheavior;
        private DevComponents.DotNetBar.Bar BeBarRuleModifier;
        private DevComponents.DotNetBar.ButtonItem BeComAddRule;
        private DevComponents.DotNetBar.LabelItem labelItem1;
        private DevComponents.DotNetBar.ButtonItem BeComEditRule;
        private DevComponents.DotNetBar.LabelItem labelItem2;
        private DevComponents.DotNetBar.ButtonItem BeComDeleteRule;
        private DevComponents.DotNetBar.LabelItem labelItem4;
        private DevComponents.DotNetBar.Controls.CheckBoxX BtnAllowScripts;
        private DevComponents.DotNetBar.ButtonX BtnSave;
        private DevComponents.DotNetBar.ButtonX BtnSaveAndClose;
        private DevComponents.DotNetBar.ButtonX BtnCopy;
    }
}