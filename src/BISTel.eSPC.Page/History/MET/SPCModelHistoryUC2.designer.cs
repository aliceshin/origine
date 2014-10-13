using BISTel.eSPC.Page.Compare;

namespace BISTel.eSPC.Page.History.MET
{
    partial class SPCModelHistoryUC2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCModelHistoryUC2));
            this.bTitlePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bTabControl1 = new BISTel.PeakPerformance.Client.BISTelControl.BTabControl();
            this.tabSPCModelList = new System.Windows.Forms.TabPage();
            this.tabCompareResult = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btxtParamName = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.bLabel1 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.spcModelUCForHistory31 = new BISTel.eSPC.Page.History.MET.SPCModelUCForHistory3();
            this.spcModelVersionHistory1 = new BISTel.eSPC.Page.History.MET.SPCModelVersionHistory();
            this.spcModelVersionCompareResultUC1 = new SPCModelCompareResultUC();
            this.bTitlePanel1.SuspendLayout();
            this.bTabControl1.SuspendLayout();
            this.tabSPCModelList.SuspendLayout();
            this.tabCompareResult.SuspendLayout();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bTitlePanel1
            // 
            this.bTitlePanel1.BssClass = "TitlePanel";
            this.bTitlePanel1.Controls.Add(this.bTabControl1);
            this.bTitlePanel1.Controls.Add(this.panel1);
            this.bTitlePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTitlePanel1.ImageBottom = null;
            this.bTitlePanel1.ImageBottomRight = null;
            this.bTitlePanel1.ImageClose = null;
            this.bTitlePanel1.ImageLeft = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageLeft")));
            this.bTitlePanel1.ImageLeftBottom = null;
            this.bTitlePanel1.ImageLeftFill = null;
            this.bTitlePanel1.ImageMax = null;
            this.bTitlePanel1.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageMiddle")));
            this.bTitlePanel1.ImageMin = null;
            this.bTitlePanel1.ImagePanelMax = null;
            this.bTitlePanel1.ImagePanelMin = null;
            this.bTitlePanel1.ImagePanelNormal = null;
            this.bTitlePanel1.ImageRight = ((System.Drawing.Image)(resources.GetObject("bTitlePanel1.ImageRight")));
            this.bTitlePanel1.ImageRightFill = null;
            this.bTitlePanel1.IsPopupMax = false;
            this.bTitlePanel1.IsSelected = false;
            this.bTitlePanel1.Key = "";
            this.bTitlePanel1.Location = new System.Drawing.Point(0, 0);
            this.bTitlePanel1.MaxControlName = "";
            this.bTitlePanel1.MaxResizable = false;
            this.bTitlePanel1.MaxVisibleStateForMaxNormal = false;
            this.bTitlePanel1.Name = "bTitlePanel1";
            this.bTitlePanel1.Padding = new System.Windows.Forms.Padding(0, 29, 0, 0);
            this.bTitlePanel1.PaddingTop = 4;
            this.bTitlePanel1.PopMaxHeight = 0;
            this.bTitlePanel1.PopMaxWidth = 0;
            this.bTitlePanel1.RightPadding = 5;
            this.bTitlePanel1.ShowCloseButton = false;
            this.bTitlePanel1.ShowMaxNormalButton = false;
            this.bTitlePanel1.ShowMinButton = false;
            this.bTitlePanel1.Size = new System.Drawing.Size(981, 591);
            this.bTitlePanel1.TabIndex = 2;
            this.bTitlePanel1.Title = "SPC Model Version History";
            this.bTitlePanel1.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(151)))), ((int)(((byte)(212)))));
            this.bTitlePanel1.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTitlePanel1.TopPadding = 5;
            // 
            // bTabControl1
            // 
            this.bTabControl1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(115)))), ((int)(((byte)(161)))), ((int)(((byte)(184)))));
            this.bTabControl1.BssClass = "Tab";
            this.bTabControl1.CloseImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.CloseImage")));
            this.bTabControl1.CloseRollOverImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.CloseRollOverImage")));
            this.bTabControl1.Controls.Add(this.tabSPCModelList);
            this.bTabControl1.Controls.Add(this.tabCompareResult);
            this.bTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.bTabControl1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTabControl1.FontSizeSet = "11";
            this.bTabControl1.IsDrawBorder = true;
            this.bTabControl1.IsFillBorder = false;
            this.bTabControl1.IsSpace = true;
            this.bTabControl1.Key = "";
            this.bTabControl1.Location = new System.Drawing.Point(0, 61);
            this.bTabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.bTabControl1.Name = "bTabControl1";
            this.bTabControl1.SelectedIndex = 0;
            this.bTabControl1.Size = new System.Drawing.Size(981, 530);
            this.bTabControl1.TabBackColor = System.Drawing.Color.White;
            this.bTabControl1.TabBackImage = null;
            this.bTabControl1.TabIndex = 1;
            this.bTabControl1.TabPageClose = false;
            this.bTabControl1.TabSelectFontColor = System.Drawing.Color.White;
            this.bTabControl1.TabSelectLeftImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectLeftImage")));
            this.bTabControl1.TabSelectMiddleImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectMiddleImage")));
            this.bTabControl1.TabSelectRightImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabSelectRightImage")));
            this.bTabControl1.TabSelectRollOverFontColor = System.Drawing.Color.Black;
            this.bTabControl1.TabUnSelectFontColor = System.Drawing.Color.Black;
            this.bTabControl1.TabUnSelectLeftImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectLeftImage")));
            this.bTabControl1.TabUnSelectMiddleImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectMiddleImage")));
            this.bTabControl1.TabUnSelectRightImage = ((System.Drawing.Image)(resources.GetObject("bTabControl1.TabUnSelectRightImage")));
            this.bTabControl1.TabUnSelectRollOverFontColor = System.Drawing.Color.White;
            this.bTabControl1.TextLeftPosition = 5;
            this.bTabControl1.UseShadowFont = false;
            // 
            // tabSPCModelList
            // 
            this.tabSPCModelList.Controls.Add(this.splitContainer1);
            this.tabSPCModelList.Location = new System.Drawing.Point(4, 25);
            this.tabSPCModelList.Name = "tabSPCModelList";
            this.tabSPCModelList.Padding = new System.Windows.Forms.Padding(3);
            this.tabSPCModelList.Size = new System.Drawing.Size(973, 501);
            this.tabSPCModelList.TabIndex = 0;
            this.tabSPCModelList.Text = "SPC Model List      ";
            this.tabSPCModelList.UseVisualStyleBackColor = true;
            // 
            // tabCompareResult
            // 
            this.tabCompareResult.Controls.Add(this.spcModelVersionCompareResultUC1);
            this.tabCompareResult.Location = new System.Drawing.Point(4, 25);
            this.tabCompareResult.Name = "tabCompareResult";
            this.tabCompareResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabCompareResult.Size = new System.Drawing.Size(973, 501);
            this.tabCompareResult.TabIndex = 1;
            this.tabCompareResult.Text = "Compare Result      ";
            this.tabCompareResult.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btxtParamName);
            this.panel1.Controls.Add(this.bLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(981, 32);
            this.panel1.TabIndex = 0;
            // 
            // btxtParamName
            // 
            this.btxtParamName.BackColor = System.Drawing.Color.White;
            this.btxtParamName.BssClass = "";
            this.btxtParamName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btxtParamName.InputDataType = BISTel.PeakPerformance.Client.BISTelControl.BTextBox.InputType.String;
            this.btxtParamName.IsMultiLanguage = true;
            this.btxtParamName.Key = "";
            this.btxtParamName.Location = new System.Drawing.Point(103, 5);
            this.btxtParamName.Name = "btxtParamName";
            this.btxtParamName.ReadOnly = true;
            this.btxtParamName.Size = new System.Drawing.Size(256, 20);
            this.btxtParamName.TabIndex = 1;
            // 
            // bLabel1
            // 
            this.bLabel1.AutoSize = true;
            this.bLabel1.BssClass = "";
            this.bLabel1.CustomTextAlign = "";
            this.bLabel1.IsMultiLanguage = true;
            this.bLabel1.Key = "";
            this.bLabel1.Location = new System.Drawing.Point(5, 7);
            this.bLabel1.Name = "bLabel1";
            this.bLabel1.Size = new System.Drawing.Size(92, 13);
            this.bLabel1.TabIndex = 0;
            this.bLabel1.Text = "Parameter Name :";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.spcModelUCForHistory31);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.spcModelVersionHistory1);
            this.splitContainer1.Size = new System.Drawing.Size(967, 495);
            this.splitContainer1.SplitterDistance = 450;
            this.splitContainer1.TabIndex = 0;
            // 
            // spcModelUCForHistory31
            // 
            this.spcModelUCForHistory31.ApplicationName = "";
            this.spcModelUCForHistory31.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.spcModelUCForHistory31.BackColorHtml = "#DCDCDC";
            this.spcModelUCForHistory31.BssClass = null;
            this.spcModelUCForHistory31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcModelUCForHistory31.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.spcModelUCForHistory31.ForeColorHtml = "#000000";
            this.spcModelUCForHistory31.FunctionName = "";
            this.spcModelUCForHistory31.Location = new System.Drawing.Point(0, 0);
            this.spcModelUCForHistory31.Name = "spcModelUCForHistory31";
            this.spcModelUCForHistory31.sessionData = null;
            this.spcModelUCForHistory31.Size = new System.Drawing.Size(322, 495);
            this.spcModelUCForHistory31.TabIndex = 0;
            this.spcModelUCForHistory31.Title = null;
            this.spcModelUCForHistory31.URL = "";
            // 
            // spcModelVersionHistory1
            // 
            this.spcModelVersionHistory1.ApplicationName = "";
            this.spcModelVersionHistory1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.spcModelVersionHistory1.BackColorHtml = "#DCDCDC";
            this.spcModelVersionHistory1.BssClass = null;
            this.spcModelVersionHistory1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcModelVersionHistory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.spcModelVersionHistory1.ForeColorHtml = "#000000";
            this.spcModelVersionHistory1.FunctionName = "";
            this.spcModelVersionHistory1.Location = new System.Drawing.Point(0, 0);
            this.spcModelVersionHistory1.Name = "spcModelVersionHistory1";
            this.spcModelVersionHistory1.sessionData = null;
            this.spcModelVersionHistory1.Size = new System.Drawing.Size(641, 495);
            this.spcModelVersionHistory1.TabIndex = 0;
            this.spcModelVersionHistory1.Title = null;
            this.spcModelVersionHistory1.URL = "";
            // 
            // spcModelVersionCompareResultUC1
            // 
            this.spcModelVersionCompareResultUC1.ApplicationName = "";
            this.spcModelVersionCompareResultUC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.spcModelVersionCompareResultUC1.BackColorHtml = "#DCDCDC";
            this.spcModelVersionCompareResultUC1.BssClass = null;
            this.spcModelVersionCompareResultUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcModelVersionCompareResultUC1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.spcModelVersionCompareResultUC1.ForeColorHtml = "#000000";
            this.spcModelVersionCompareResultUC1.FunctionName = "";
            this.spcModelVersionCompareResultUC1.Location = new System.Drawing.Point(3, 3);
            this.spcModelVersionCompareResultUC1.Name = "spcModelVersionCompareResultUC1";
            this.spcModelVersionCompareResultUC1.sessionData = null;
            this.spcModelVersionCompareResultUC1.Size = new System.Drawing.Size(967, 495);
            this.spcModelVersionCompareResultUC1.TabIndex = 0;
            this.spcModelVersionCompareResultUC1.Title = null;
            this.spcModelVersionCompareResultUC1.URL = "";
            // 
            // SPCModelHistoryUC2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bTitlePanel1);
            this.Name = "SPCModelHistoryUC2";
            this.Size = new System.Drawing.Size(981, 591);
            this.bTitlePanel1.ResumeLayout(false);
            this.bTitlePanel1.PerformLayout();
            this.bTabControl1.ResumeLayout(false);
            this.tabSPCModelList.ResumeLayout(false);
            this.tabCompareResult.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTitlePanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BTabControl bTabControl1;
        private System.Windows.Forms.TabPage tabSPCModelList;
        private System.Windows.Forms.TabPage tabCompareResult;
        private System.Windows.Forms.Panel panel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtParamName;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private SPCModelUCForHistory3 spcModelUCForHistory31;
        private SPCModelVersionHistory spcModelVersionHistory1;
        private SPCModelCompareResultUC spcModelVersionCompareResultUC1;

    }
}
