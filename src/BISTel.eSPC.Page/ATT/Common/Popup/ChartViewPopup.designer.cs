namespace BISTel.eSPC.Page.ATT.Common
{
    partial class ChartViewPopup
    {
        /// <summary>
        /// Required designer ChartVariable.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartViewPopup));
            this.bapSearch = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnSearch = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bDtEnd = new BISTel.PeakPerformance.Client.BISTelControl.BDateTime();
            this.bDtStart = new BISTel.PeakPerformance.Client.BISTelControl.BDateTime();
            this.bLabel9 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel1 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.blblTitSearch = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.pnlDefaultSettingPopup = new System.Windows.Forms.Panel();
            this.pnlChart = new System.Windows.Forms.Panel();
            this.bapnlButton = new BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel();
            this.bbtnCancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.pnlContentsArea.SuspendLayout();
            this.bapSearch.SuspendLayout();
            this.pnlDefaultSettingPopup.SuspendLayout();
            this.bapnlButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.pnlDefaultSettingPopup);
            this.pnlContentsArea.Controls.Add(this.bapSearch);
            this.pnlContentsArea.Size = new System.Drawing.Size(986, 726);
            // 
            // bapSearch
            // 
            this.bapSearch.BLabelAutoPadding = true;
            this.bapSearch.BssClass = "DataArrPanel";
            this.bapSearch.Controls.Add(this.bbtnSearch);
            this.bapSearch.Controls.Add(this.bDtEnd);
            this.bapSearch.Controls.Add(this.bDtStart);
            this.bapSearch.Controls.Add(this.bLabel9);
            this.bapSearch.Controls.Add(this.bLabel1);
            this.bapSearch.Controls.Add(this.blblTitSearch);
            this.bapSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.bapSearch.IsCondition = true;
            this.bapSearch.Key = "";
            this.bapSearch.Location = new System.Drawing.Point(3, 3);
            this.bapSearch.Name = "bapSearch";
            this.bapSearch.Padding_Bottom = 5;
            this.bapSearch.Padding_Left = 5;
            this.bapSearch.Padding_Right = 5;
            this.bapSearch.Padding_Top = 5;
            this.bapSearch.Size = new System.Drawing.Size(980, 38);
            this.bapSearch.Space = 5;
            this.bapSearch.TabIndex = 7;
            this.bapSearch.Title = "";
            this.bapSearch.Visible = false;
            // 
            // bbtnSearch
            // 
            this.bbtnSearch.AutoButtonSize = false;
            this.bbtnSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnSearch.BackgroundImage")));
            this.bbtnSearch.BssClass = "";
            this.bbtnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnSearch.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnSearch.IsMultiLanguage = true;
            this.bbtnSearch.Key = "";
            this.bbtnSearch.Location = new System.Drawing.Point(509, 7);
            this.bbtnSearch.Name = "bbtnSearch";
            this.bbtnSearch.Size = new System.Drawing.Size(86, 24);
            this.bbtnSearch.TabIndex = 23;
            this.bbtnSearch.Text = "Search";
            this.bbtnSearch.ToolTipText = "";
            this.bbtnSearch.UseVisualStyleBackColor = true;
            this.bbtnSearch.Click += new System.EventHandler(this.bbtnSearch_Click);
            // 
            // bDtEnd
            // 
            this.bDtEnd.BaseDayTime = "23:59:59";
            this.bDtEnd.Daily = null;
            this.bDtEnd.EndDateTime = new System.DateTime(2009, 8, 26, 0, 0, 0, 0);
            this.bDtEnd.Format = "Short Custom";
            this.bDtEnd.FormatDateTime = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.DateTimeFomat.LONG;
            this.bDtEnd.FormatOfDate = null;
            this.bDtEnd.FormatOfTime = "HH:mm:ss";
            this.bDtEnd.FromTo = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.FromToType.TO_ONLY;
            this.bDtEnd.HeightOfNeed = 0;
            this.bDtEnd.Hourly = null;
            this.bDtEnd.IsForcedResizeOfParent = false;
            this.bDtEnd.IsVisibleDateControl = true;
            this.bDtEnd.IsVisibleperiodControl = false;
            this.bDtEnd.Location = new System.Drawing.Point(316, 6);
            this.bDtEnd.MaxDate = new System.DateTime(2112, 12, 31, 0, 0, 0, 0);
            this.bDtEnd.MinDate = new System.DateTime(1912, 1, 1, 0, 0, 0, 0);
            this.bDtEnd.Monthly = null;
            this.bDtEnd.Name = "bDtEnd";
            this.bDtEnd.Order = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.OrderOfControl.Period_Date;
            this.bDtEnd.Shiftly = null;
            this.bDtEnd.Size = new System.Drawing.Size(188, 26);
            this.bDtEnd.StartDateTime = new System.DateTime(2009, 8, 26, 0, 0, 0, 0);
            this.bDtEnd.TabIndex = 22;
            this.bDtEnd.Text = "bDateTime1";
            this.bDtEnd.UserDefineAddTime = null;
            this.bDtEnd.UserDefineIsSetStartTime = false;
            this.bDtEnd.UserDefinePeriodCustom = false;
            this.bDtEnd.UserDefinePeriodDay = 0;
            this.bDtEnd.UserDefineStartTime = null;
            this.bDtEnd.Value = new System.DateTime(2009, 8, 26, 0, 0, 0, 0);
            this.bDtEnd.ValuePeriod = null;
            this.bDtEnd.Weekly = null;
            this.bDtEnd.WeeklySimbol = "W";
            this.bDtEnd.WIDTH_DATE = 90;
            this.bDtEnd.WIDTH_TIME = 80;
            // 
            // bDtStart
            // 
            this.bDtStart.BaseDayTime = "00:00:00";
            this.bDtStart.Daily = null;
            this.bDtStart.EndDateTime = new System.DateTime(2009, 8, 26, 0, 0, 0, 0);
            this.bDtStart.Format = "Short Custom";
            this.bDtStart.FormatDateTime = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.DateTimeFomat.LONG;
            this.bDtStart.FormatOfDate = null;
            this.bDtStart.FormatOfTime = "HH:mm:ss";
            this.bDtStart.FromTo = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.FromToType.FROM_ONLY;
            this.bDtStart.HeightOfNeed = 0;
            this.bDtStart.Hourly = null;
            this.bDtStart.IsForcedResizeOfParent = false;
            this.bDtStart.IsVisibleDateControl = true;
            this.bDtStart.IsVisibleperiodControl = false;
            this.bDtStart.Location = new System.Drawing.Point(107, 6);
            this.bDtStart.MaxDate = new System.DateTime(2112, 12, 31, 0, 0, 0, 0);
            this.bDtStart.MinDate = new System.DateTime(1912, 1, 1, 0, 0, 0, 0);
            this.bDtStart.Monthly = null;
            this.bDtStart.Name = "bDtStart";
            this.bDtStart.Order = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.OrderOfControl.Period_Date;
            this.bDtStart.Shiftly = null;
            this.bDtStart.Size = new System.Drawing.Size(180, 26);
            this.bDtStart.StartDateTime = new System.DateTime(2009, 8, 26, 0, 0, 0, 0);
            this.bDtStart.TabIndex = 22;
            this.bDtStart.Text = "bDateTime1";
            this.bDtStart.UserDefineAddTime = null;
            this.bDtStart.UserDefineIsSetStartTime = false;
            this.bDtStart.UserDefinePeriodCustom = false;
            this.bDtStart.UserDefinePeriodDay = 0;
            this.bDtStart.UserDefineStartTime = null;
            this.bDtStart.Value = new System.DateTime(2009, 8, 26, 0, 0, 0, 0);
            this.bDtStart.ValuePeriod = null;
            this.bDtStart.Weekly = null;
            this.bDtStart.WeeklySimbol = "W";
            this.bDtStart.WIDTH_DATE = 90;
            this.bDtStart.WIDTH_TIME = 80;
            // 
            // bLabel9
            // 
            this.bLabel9.BackColor = System.Drawing.Color.Transparent;
            this.bLabel9.BssClass = "";
            this.bLabel9.CustomTextAlign = "";
            this.bLabel9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel9.ForeColor = System.Drawing.Color.Black;
            this.bLabel9.Image = global::BISTel.eSPC.Page.Properties.Resources.bullet_01;
            this.bLabel9.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bLabel9.IsMultiLanguage = true;
            this.bLabel9.Key = "";
            this.bLabel9.Location = new System.Drawing.Point(5, 5);
            this.bLabel9.Name = "bLabel9";
            this.bLabel9.Size = new System.Drawing.Size(10, 28);
            this.bLabel9.TabIndex = 21;
            // 
            // bLabel1
            // 
            this.bLabel1.BackColor = System.Drawing.Color.Transparent;
            this.bLabel1.BssClass = "";
            this.bLabel1.CustomTextAlign = "";
            this.bLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel1.ForeColor = System.Drawing.Color.Black;
            this.bLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bLabel1.IsMultiLanguage = true;
            this.bLabel1.Key = "";
            this.bLabel1.Location = new System.Drawing.Point(292, 5);
            this.bLabel1.Name = "bLabel1";
            this.bLabel1.Size = new System.Drawing.Size(19, 28);
            this.bLabel1.TabIndex = 20;
            this.bLabel1.Text = "~";
            this.bLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // blblTitSearch
            // 
            this.blblTitSearch.BackColor = System.Drawing.Color.Transparent;
            this.blblTitSearch.BssClass = "";
            this.blblTitSearch.CustomTextAlign = "";
            this.blblTitSearch.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.blblTitSearch.ForeColor = System.Drawing.Color.Black;
            this.blblTitSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.blblTitSearch.IsMultiLanguage = true;
            this.blblTitSearch.Key = "";
            this.blblTitSearch.Location = new System.Drawing.Point(20, 5);
            this.blblTitSearch.Name = "blblTitSearch";
            this.blblTitSearch.Size = new System.Drawing.Size(82, 28);
            this.blblTitSearch.TabIndex = 20;
            this.blblTitSearch.Text = "Search Period :";
            this.blblTitSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDefaultSettingPopup
            // 
            this.pnlDefaultSettingPopup.Controls.Add(this.pnlChart);
            this.pnlDefaultSettingPopup.Controls.Add(this.bapnlButton);
            this.pnlDefaultSettingPopup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDefaultSettingPopup.Location = new System.Drawing.Point(3, 41);
            this.pnlDefaultSettingPopup.Name = "pnlDefaultSettingPopup";
            this.pnlDefaultSettingPopup.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pnlDefaultSettingPopup.Size = new System.Drawing.Size(980, 682);
            this.pnlDefaultSettingPopup.TabIndex = 14;
            // 
            // pnlChart
            // 
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChart.Location = new System.Drawing.Point(0, 0);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.pnlChart.Size = new System.Drawing.Size(980, 642);
            this.pnlChart.TabIndex = 7;
            // 
            // bapnlButton
            // 
            this.bapnlButton.BLabelAutoPadding = true;
            this.bapnlButton.BssClass = "TitleArrPanel";
            this.bapnlButton.Controls.Add(this.bbtnCancel);
            this.bapnlButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bapnlButton.IsCondition = false;
            this.bapnlButton.Key = "";
            this.bapnlButton.Location = new System.Drawing.Point(0, 642);
            this.bapnlButton.Name = "bapnlButton";
            this.bapnlButton.Padding_Bottom = 5;
            this.bapnlButton.Padding_Left = 5;
            this.bapnlButton.Padding_Right = 5;
            this.bapnlButton.Padding_Top = 5;
            this.bapnlButton.Size = new System.Drawing.Size(980, 35);
            this.bapnlButton.Space = 5;
            this.bapnlButton.TabIndex = 6;
            this.bapnlButton.Title = "";
            // 
            // bbtnCancel
            // 
            this.bbtnCancel.AutoButtonSize = false;
            this.bbtnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bbtnCancel.BackgroundImage")));
            this.bbtnCancel.BssClass = "ConditionButton";
            this.bbtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.bbtnCancel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bbtnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bbtnCancel.IsMultiLanguage = true;
            this.bbtnCancel.Key = "FDC_POPUP_BUTTON_CLOSE";
            this.bbtnCancel.Location = new System.Drawing.Point(899, 5);
            this.bbtnCancel.Name = "bbtnCancel";
            this.bbtnCancel.Size = new System.Drawing.Size(72, 25);
            this.bbtnCancel.TabIndex = 20;
            this.bbtnCancel.Text = "Close";
            this.bbtnCancel.ToolTipText = "";
            this.bbtnCancel.UseVisualStyleBackColor = true;
            this.bbtnCancel.Click += new System.EventHandler(this.bbtnCancel_Click);
            // 
            // ChartViewPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 759);
            this.ContentsAreaMinHeight = 720;
            this.ContentsAreaMinWidth = 980;
            this.Name = "ChartViewPopup";
            this.Resizable = true;
            this.Text = "ChartViewPopup";
            this.pnlContentsArea.ResumeLayout(false);
            this.bapSearch.ResumeLayout(false);
            this.pnlDefaultSettingPopup.ResumeLayout(false);
            this.bapnlButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

   
        #endregion

        private System.Windows.Forms.Panel pnlDefaultSettingPopup;
        private System.Windows.Forms.Panel pnlChart;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapnlButton;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnCancel;
        private BISTel.PeakPerformance.Client.BISTelControl.BArrangePanel bapSearch;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bbtnSearch;
        private BISTel.PeakPerformance.Client.BISTelControl.BDateTime bDtEnd;
        private BISTel.PeakPerformance.Client.BISTelControl.BDateTime bDtStart;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel9;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel blblTitSearch;

    }
}