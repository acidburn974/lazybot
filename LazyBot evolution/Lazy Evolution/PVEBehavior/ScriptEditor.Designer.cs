namespace LazyEvo.PVEBehavior
{
    partial class ScriptEditor
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
            this.groupPanel6 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.BtnSave = new DevComponents.DotNetBar.ButtonX();
            this.BCancel = new DevComponents.DotNetBar.ButtonX();
            this.TBRuleName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.TBScript = new System.Windows.Forms.RichTextBox();
            this.superTooltip1 = new DevComponents.DotNetBar.SuperTooltip();
            this.groupPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Windows7Blue;
            // 
            // groupPanel6
            // 
            this.groupPanel6.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel6.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel6.Controls.Add(this.BtnSave);
            this.groupPanel6.Controls.Add(this.BCancel);
            this.groupPanel6.Controls.Add(this.TBRuleName);
            this.groupPanel6.Controls.Add(this.labelX7);
            this.groupPanel6.Location = new System.Drawing.Point(10, 381);
            this.groupPanel6.Name = "groupPanel6";
            this.groupPanel6.Size = new System.Drawing.Size(596, 27);
            // 
            // 
            // 
            this.groupPanel6.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel6.Style.BackColorGradientAngle = 90;
            this.groupPanel6.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel6.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel6.Style.BorderBottomWidth = 1;
            this.groupPanel6.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel6.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel6.Style.BorderLeftWidth = 1;
            this.groupPanel6.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel6.Style.BorderRightWidth = 1;
            this.groupPanel6.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel6.Style.BorderTopWidth = 1;
            this.groupPanel6.Style.Class = "";
            this.groupPanel6.Style.CornerDiameter = 4;
            this.groupPanel6.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel6.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel6.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel6.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel6.StyleMouseDown.Class = "";
            this.groupPanel6.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel6.StyleMouseOver.Class = "";
            this.groupPanel6.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel6.TabIndex = 11;
            // 
            // BtnSave
            // 
            this.BtnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnSave.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.BtnSave.Location = new System.Drawing.Point(501, 0);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 21);
            this.BtnSave.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BtnSave.TabIndex = 61;
            this.BtnSave.Text = "Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSaveClick);
            // 
            // BCancel
            // 
            this.BCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.BCancel.Location = new System.Drawing.Point(411, 0);
            this.BCancel.Name = "BCancel";
            this.BCancel.Size = new System.Drawing.Size(75, 21);
            this.BCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.BCancel.TabIndex = 60;
            this.BCancel.Text = "Cancel";
            this.BCancel.Click += new System.EventHandler(this.BCancelClick);
            // 
            // TBRuleName
            // 
            // 
            // 
            // 
            this.TBRuleName.Border.Class = "TextBoxBorder";
            this.TBRuleName.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.TBRuleName.Location = new System.Drawing.Point(132, 1);
            this.TBRuleName.Name = "TBRuleName";
            this.TBRuleName.Size = new System.Drawing.Size(273, 20);
            this.TBRuleName.TabIndex = 58;
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX7.BackgroundStyle.Class = "";
            this.labelX7.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX7.Location = new System.Drawing.Point(3, 0);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(104, 23);
            this.labelX7.TabIndex = 57;
            this.labelX7.Text = "<b>Name of rule:</b>";
            // 
            // TBScript
            // 
            this.TBScript.AcceptsTab = true;
            this.TBScript.Location = new System.Drawing.Point(10, 13);
            this.TBScript.Name = "TBScript";
            this.TBScript.Size = new System.Drawing.Size(596, 362);
            this.TBScript.TabIndex = 12;
            this.TBScript.Text = "";
            // 
            // superTooltip1
            // 
            this.superTooltip1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            // 
            // ScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(233)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(617, 452);
            this.ControlBox = false;
            this.Controls.Add(this.TBScript);
            this.Controls.Add(this.groupPanel6);
            this.Name = "ScriptEditor";
            this.Text = "ScriptEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScriptEditor_FormClosing);
            this.Load += new System.EventHandler(this.ScriptEditor_Load);
            this.groupPanel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel6;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.TextBoxX TBRuleName;
        private DevComponents.DotNetBar.ButtonX BtnSave;
        private DevComponents.DotNetBar.ButtonX BCancel;
        private System.Windows.Forms.RichTextBox TBScript;
        private DevComponents.DotNetBar.SuperTooltip superTooltip1;
    }
}