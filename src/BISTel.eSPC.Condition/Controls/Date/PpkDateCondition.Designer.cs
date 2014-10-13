namespace BISTel.eSPC.Condition.Controls.Date
{
    partial class PpkDateCondition
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
            this.bcpnlStart = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.dtpStart = new BISTel.PeakPerformance.Client.BISTelControl.BDateTime();
            this.lblStart = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlEnd = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.dtpEnd = new BISTel.PeakPerformance.Client.BISTelControl.BDateTime();
            this.lblEnd = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bConditionPanel1 = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.cboType = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.bLabel2 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bLabel1 = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlStart.SuspendLayout();
            this.bcpnlEnd.SuspendLayout();
            this.bConditionPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bcpnlStart
            // 
            this.bcpnlStart.BottomImage = null;
            this.bcpnlStart.BottomLeftImage = null;
            this.bcpnlStart.BssClass = "";
            this.bcpnlStart.Controls.Add(this.dtpStart);
            this.bcpnlStart.Controls.Add(this.lblStart);
            this.bcpnlStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcpnlStart.DownImage = null;
            this.bcpnlStart.Key = "";
            this.bcpnlStart.LeftImage = null;
            this.bcpnlStart.LeftTopImage = null;
            this.bcpnlStart.Location = new System.Drawing.Point(0, 0);
            this.bcpnlStart.Name = "bcpnlStart";
            this.bcpnlStart.Padding = new System.Windows.Forms.Padding(3);
            this.bcpnlStart.RightBottomImage = null;
            this.bcpnlStart.RightImage = null;
            this.bcpnlStart.Size = new System.Drawing.Size(225, 30);
            this.bcpnlStart.TabIndex = 10;
            this.bcpnlStart.TopImage = null;
            this.bcpnlStart.TopRightImage = null;
            this.bcpnlStart.UpImage = null;
            // 
            // dtpStart
            // 
            this.dtpStart.BaseDayTime = "00:00:00";
            this.dtpStart.Daily = null;
            this.dtpStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpStart.EndDateTime = new System.DateTime(2009, 10, 8, 0, 0, 0, 0);
            this.dtpStart.Format = "Custom Custom";
            this.dtpStart.FormatDateTime = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.DateTimeFomat.SHORT;
            this.dtpStart.FormatOfDate = null;
            this.dtpStart.FormatOfTime = "HH:mm:ss";
            this.dtpStart.FromTo = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.FromToType.FROM_ONLY;
            this.dtpStart.HeightOfNeed = 0;
            this.dtpStart.Hourly = null;
            this.dtpStart.IsForcedResizeOfParent = false;
            this.dtpStart.IsVisibleDateControl = true;
            this.dtpStart.IsVisibleperiodControl = false;
            this.dtpStart.Location = new System.Drawing.Point(71, 3);
            this.dtpStart.MaxDate = new System.DateTime(2109, 12, 31, 0, 0, 0, 0);
            this.dtpStart.MinDate = new System.DateTime(1909, 1, 1, 0, 0, 0, 0);
            this.dtpStart.Monthly = null;
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Order = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.OrderOfControl.Period_Date;
            this.dtpStart.Shiftly = null;
            this.dtpStart.Size = new System.Drawing.Size(151, 24);
            this.dtpStart.StartDateTime = new System.DateTime(2009, 10, 8, 0, 0, 0, 0);
            this.dtpStart.TabIndex = 4;
            this.dtpStart.Text = "bDateTime1";
            this.dtpStart.Value = new System.DateTime(2009, 10, 8, 0, 0, 0, 0);
            this.dtpStart.ValuePeriod = null;
            this.dtpStart.Weekly = null;
            this.dtpStart.WeeklySimbol = "W";
            // 
            // lblStart
            // 
            this.lblStart.BackColor = System.Drawing.Color.White;
            this.lblStart.BssClass = "";
            this.lblStart.CustomTextAlign = "";
            this.lblStart.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblStart.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStart.ForeColor = System.Drawing.Color.Black;
            this.lblStart.IsMultiLanguage = true;
            this.lblStart.Key = "";
            this.lblStart.Location = new System.Drawing.Point(3, 3);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(68, 24);
            this.lblStart.TabIndex = 3;
            this.lblStart.Text = "From";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bcpnlEnd
            // 
            this.bcpnlEnd.BottomImage = null;
            this.bcpnlEnd.BottomLeftImage = null;
            this.bcpnlEnd.BssClass = "";
            this.bcpnlEnd.Controls.Add(this.dtpEnd);
            this.bcpnlEnd.Controls.Add(this.lblEnd);
            this.bcpnlEnd.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcpnlEnd.DownImage = null;
            this.bcpnlEnd.Key = "";
            this.bcpnlEnd.LeftImage = null;
            this.bcpnlEnd.LeftTopImage = null;
            this.bcpnlEnd.Location = new System.Drawing.Point(0, 30);
            this.bcpnlEnd.Name = "bcpnlEnd";
            this.bcpnlEnd.Padding = new System.Windows.Forms.Padding(3);
            this.bcpnlEnd.RightBottomImage = null;
            this.bcpnlEnd.RightImage = null;
            this.bcpnlEnd.Size = new System.Drawing.Size(225, 30);
            this.bcpnlEnd.TabIndex = 11;
            this.bcpnlEnd.TopImage = null;
            this.bcpnlEnd.TopRightImage = null;
            this.bcpnlEnd.UpImage = null;
            // 
            // dtpEnd
            // 
            this.dtpEnd.BaseDayTime = "00:00:00";
            this.dtpEnd.Daily = null;
            this.dtpEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpEnd.EndDateTime = new System.DateTime(2009, 10, 8, 0, 0, 0, 0);
            this.dtpEnd.Format = "Custom Custom";
            this.dtpEnd.FormatDateTime = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.DateTimeFomat.SHORT;
            this.dtpEnd.FormatOfDate = null;
            this.dtpEnd.FormatOfTime = "HH:mm:ss";
            this.dtpEnd.FromTo = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.FromToType.TO_ONLY;
            this.dtpEnd.HeightOfNeed = 0;
            this.dtpEnd.Hourly = null;
            this.dtpEnd.IsForcedResizeOfParent = false;
            this.dtpEnd.IsVisibleDateControl = true;
            this.dtpEnd.IsVisibleperiodControl = false;
            this.dtpEnd.Location = new System.Drawing.Point(71, 3);
            this.dtpEnd.MaxDate = new System.DateTime(2109, 12, 31, 0, 0, 0, 0);
            this.dtpEnd.MinDate = new System.DateTime(1909, 1, 1, 0, 0, 0, 0);
            this.dtpEnd.Monthly = null;
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Order = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.OrderOfControl.Period_Date;
            this.dtpEnd.Shiftly = null;
            this.dtpEnd.Size = new System.Drawing.Size(151, 24);
            this.dtpEnd.StartDateTime = new System.DateTime(2009, 10, 8, 0, 0, 0, 0);
            this.dtpEnd.TabIndex = 5;
            this.dtpEnd.Text = "bDateTime2";
            this.dtpEnd.Value = new System.DateTime(2009, 10, 8, 0, 0, 0, 0);
            this.dtpEnd.ValuePeriod = null;
            this.dtpEnd.Weekly = null;
            this.dtpEnd.WeeklySimbol = "W";
            // 
            // lblEnd
            // 
            this.lblEnd.BackColor = System.Drawing.Color.White;
            this.lblEnd.BssClass = "";
            this.lblEnd.CustomTextAlign = "";
            this.lblEnd.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblEnd.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblEnd.ForeColor = System.Drawing.Color.Black;
            this.lblEnd.IsMultiLanguage = true;
            this.lblEnd.Key = "";
            this.lblEnd.Location = new System.Drawing.Point(3, 3);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(68, 24);
            this.lblEnd.TabIndex = 4;
            this.lblEnd.Text = "To";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bConditionPanel1
            // 
            this.bConditionPanel1.BottomImage = null;
            this.bConditionPanel1.BottomLeftImage = null;
            this.bConditionPanel1.BssClass = "";
            this.bConditionPanel1.Controls.Add(this.cboType);
            this.bConditionPanel1.Controls.Add(this.bLabel2);
            this.bConditionPanel1.Controls.Add(this.bLabel1);
            this.bConditionPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bConditionPanel1.DownImage = null;
            this.bConditionPanel1.Key = "";
            this.bConditionPanel1.LeftImage = null;
            this.bConditionPanel1.LeftTopImage = null;
            this.bConditionPanel1.Location = new System.Drawing.Point(0, 60);
            this.bConditionPanel1.Name = "bConditionPanel1";
            this.bConditionPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.bConditionPanel1.RightBottomImage = null;
            this.bConditionPanel1.RightImage = null;
            this.bConditionPanel1.Size = new System.Drawing.Size(225, 30);
            this.bConditionPanel1.TabIndex = 12;
            this.bConditionPanel1.TopImage = null;
            this.bConditionPanel1.TopRightImage = null;
            this.bConditionPanel1.UpImage = null;
            // 
            // cboType
            // 
            this.cboType.BssClass = "";
            this.cboType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cboType.FormattingEnabled = true;
            this.cboType.IsMultiLanguage = true;
            this.cboType.Key = "";
            this.cboType.Location = new System.Drawing.Point(71, 3);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(151, 21);
            this.cboType.TabIndex = 6;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // bLabel2
            // 
            this.bLabel2.BssClass = "";
            this.bLabel2.CustomTextAlign = "";
            this.bLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bLabel2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel2.IsMultiLanguage = true;
            this.bLabel2.Key = "";
            this.bLabel2.Location = new System.Drawing.Point(71, 3);
            this.bLabel2.Name = "bLabel2";
            this.bLabel2.Size = new System.Drawing.Size(151, 24);
            this.bLabel2.TabIndex = 5;
            this.bLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bLabel1
            // 
            this.bLabel1.BackColor = System.Drawing.Color.White;
            this.bLabel1.BssClass = "";
            this.bLabel1.CustomTextAlign = "";
            this.bLabel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.bLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.bLabel1.ForeColor = System.Drawing.Color.Black;
            this.bLabel1.IsMultiLanguage = true;
            this.bLabel1.Key = "";
            this.bLabel1.Location = new System.Drawing.Point(3, 3);
            this.bLabel1.Name = "bLabel1";
            this.bLabel1.Size = new System.Drawing.Size(68, 24);
            this.bLabel1.TabIndex = 4;
            this.bLabel1.Text = "Type";
            this.bLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PpkDateCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.bConditionPanel1);
            this.Controls.Add(this.bcpnlEnd);
            this.Controls.Add(this.bcpnlStart);
            this.Name = "PpkDateCondition";
            this.Size = new System.Drawing.Size(225, 93);
            this.bcpnlStart.ResumeLayout(false);
            this.bcpnlEnd.ResumeLayout(false);
            this.bConditionPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlStart;
        private BISTel.PeakPerformance.Client.BISTelControl.BDateTime dtpStart;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblStart;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlEnd;
        private BISTel.PeakPerformance.Client.BISTelControl.BDateTime dtpEnd;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblEnd;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bConditionPanel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel bLabel2;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox cboType;
    }
}
