namespace BISTel.eSPC.Page.Compare
{
    partial class SPCModelCompareUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPCModelCompareUC));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btxtParamName = new BISTel.PeakPerformance.Client.BISTelControl.BTextBox();
            this.bLabel1 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bTabControl1 = new BISTel.PeakPerformance.Client.BISTelControl.BTabControl();
            this.tabSPCModelList = new System.Windows.Forms.TabPage();
            this.ucSPCModelList = new BISTel.eSPC.Page.Compare.SPCModelListUC();
            this.tabCompareResult = new System.Windows.Forms.TabPage();
            this.ucSpcModelCompareResultUC = new BISTel.eSPC.Page.Compare.SPCModelCompareResultUC();
            this.bTitlePanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.panel1.SuspendLayout();
            this.bTabControl1.SuspendLayout();
            this.tabSPCModelList.SuspendLayout();
            this.tabCompareResult.SuspendLayout();
            this.bTitlePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btxtParamName);
            this.panel1.Controls.Add(this.bLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(792, 32);
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
            this.bTabControl1.Size = new System.Drawing.Size(792, 611);
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
            this.tabSPCModelList.Controls.Add(this.ucSPCModelList);
            this.tabSPCModelList.Location = new System.Drawing.Point(4, 25);
            this.tabSPCModelList.Name = "tabSPCModelList";
            this.tabSPCModelList.Padding = new System.Windows.Forms.Padding(3);
            this.tabSPCModelList.Size = new System.Drawing.Size(784, 582);
            this.tabSPCModelList.TabIndex = 0;
            this.tabSPCModelList.Text = "SPC Model List      ";
            this.tabSPCModelList.UseVisualStyleBackColor = true;
            // 
            // ucSPCModelList
            // 
            this.ucSPCModelList.ApplicationName = "";
            this.ucSPCModelList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucSPCModelList.BackColorHtml = "#DCDCDC";
            this.ucSPCModelList.BssClass = null;
            this.ucSPCModelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSPCModelList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucSPCModelList.ForeColorHtml = "#000000";
            this.ucSPCModelList.FunctionName = "";
            this.ucSPCModelList.Location = new System.Drawing.Point(3, 3);
            this.ucSPCModelList.Name = "ucSPCModelList";
            this.ucSPCModelList.sessionData = null;
            this.ucSPCModelList.Size = new System.Drawing.Size(778, 576);
            this.ucSPCModelList.TabIndex = 0;
            this.ucSPCModelList.Title = null;
            this.ucSPCModelList.URL = "";
            // 
            // tabCompareResult
            // 
            this.tabCompareResult.Controls.Add(this.ucSpcModelCompareResultUC);
            this.tabCompareResult.Location = new System.Drawing.Point(4, 25);
            this.tabCompareResult.Name = "tabCompareResult";
            this.tabCompareResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabCompareResult.Size = new System.Drawing.Size(784, 611);
            this.tabCompareResult.TabIndex = 1;
            this.tabCompareResult.Text = "Compare Result      ";
            this.tabCompareResult.UseVisualStyleBackColor = true;
            // 
            // ucSpcModelCompareResultUC
            // 
            this.ucSpcModelCompareResultUC.ApplicationName = "";
            this.ucSpcModelCompareResultUC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ucSpcModelCompareResultUC.BackColorHtml = "#DCDCDC";
            this.ucSpcModelCompareResultUC.BssClass = null;
            this.ucSpcModelCompareResultUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSpcModelCompareResultUC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucSpcModelCompareResultUC.ForeColorHtml = "#000000";
            this.ucSpcModelCompareResultUC.FunctionName = "";
            this.ucSpcModelCompareResultUC.Location = new System.Drawing.Point(3, 3);
            this.ucSpcModelCompareResultUC.Name = "ucSpcModelCompareResultUC";
            this.ucSpcModelCompareResultUC.sessionData = null;
            this.ucSpcModelCompareResultUC.Size = new System.Drawing.Size(778, 605);
            this.ucSpcModelCompareResultUC.TabIndex = 0;
            this.ucSpcModelCompareResultUC.Title = null;
            this.ucSpcModelCompareResultUC.URL = "";
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
            this.bTitlePanel1.Size = new System.Drawing.Size(792, 672);
            this.bTitlePanel1.TabIndex = 1;
            this.bTitlePanel1.Title = "SPC Model Compare";
            this.bTitlePanel1.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(151)))), ((int)(((byte)(212)))));
            this.bTitlePanel1.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.bTitlePanel1.TopPadding = 5;
            // 
            // SPCModelCompareUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bTitlePanel1);
            this.Name = "SPCModelCompareUC";
            this.Size = new System.Drawing.Size(792, 672);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.bTabControl1.ResumeLayout(false);
            this.tabSPCModelList.ResumeLayout(false);
            this.tabCompareResult.ResumeLayout(false);
            this.bTitlePanel1.ResumeLayout(false);
            this.bTitlePanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BTextBox btxtParamName;
        private BISTel.PeakPerformance.Client.BISTelControl.BTabControl bTabControl1;
        private System.Windows.Forms.TabPage tabSPCModelList;
        private System.Windows.Forms.TabPage tabCompareResult;
        private SPCModelListUC ucSPCModelList;
        private SPCModelCompareResultUC ucSpcModelCompareResultUC;
        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel bTitlePanel1;
    }
}
