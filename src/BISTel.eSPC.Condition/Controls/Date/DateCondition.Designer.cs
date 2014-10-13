namespace BISTel.eSPC.Condition.Controls.Date
{
    partial class DateCondition
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
            this.bcpnlType = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.cboType = new BISTel.PeakPerformance.Client.BISTelControl.BComboBox();
            this.lblType = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlStart = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.dtpStart = new BISTel.PeakPerformance.Client.BISTelControl.BDateTime();
            this.lblStart = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlEnd = new BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel();
            this.dtpEnd = new BISTel.PeakPerformance.Client.BISTelControl.BDateTime();
            this.lblEnd = new BISTel.PeakPerformance.Client.BISTelControl.BLabel();
            this.bcpnlType.SuspendLayout();
            this.bcpnlStart.SuspendLayout();
            this.bcpnlEnd.SuspendLayout();
            this.SuspendLayout();
            // 
            // bcpnlType
            // 
            this.bcpnlType.BackColor = System.Drawing.Color.Transparent;
            this.bcpnlType.BottomImage = null;
            this.bcpnlType.BottomLeftImage = null;
            this.bcpnlType.BssClass = "ConditionArrPanel";
            this.bcpnlType.Controls.Add(this.cboType);
            this.bcpnlType.Controls.Add(this.lblType);
            this.bcpnlType.Dock = System.Windows.Forms.DockStyle.Top;
            this.bcpnlType.DownImage = null;
            this.bcpnlType.Key = "";
            this.bcpnlType.LeftImage = null;
            this.bcpnlType.LeftTopImage = null;
            this.bcpnlType.Location = new System.Drawing.Point(0, 0);
            this.bcpnlType.Name = "bcpnlType";
            this.bcpnlType.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.bcpnlType.RightBottomImage = null;
            this.bcpnlType.RightImage = null;
            this.bcpnlType.Size = new System.Drawing.Size(180, 23);
            this.bcpnlType.TabIndex = 9;
            this.bcpnlType.TopImage = null;
            this.bcpnlType.TopRightImage = null;
            this.bcpnlType.UpImage = null;
            // 
            // cboType
            // 
            this.cboType.AutoDropDownWidth = true;
            this.cboType.BssClass = "";
            this.cboType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cboType.FormattingEnabled = true;
            this.cboType.IsMultiLanguage = true;
            this.cboType.Key = "";
            this.cboType.Location = new System.Drawing.Point(61, 0);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(116, 21);
            this.cboType.TabIndex = 0;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.cboType_SelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.BackColor = System.Drawing.Color.White;
            this.lblType.BssClass = "";
            this.lblType.CustomTextAlign = "";
            this.lblType.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblType.ForeColor = System.Drawing.Color.Black;
            this.lblType.IsMultiLanguage = true;
            this.lblType.Key = "";
            this.lblType.Location = new System.Drawing.Point(3, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(58, 23);
            this.lblType.TabIndex = 5;
            this.lblType.Text = "Date Type";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.bcpnlStart.Location = new System.Drawing.Point(0, 23);
            this.bcpnlStart.Name = "bcpnlStart";
            this.bcpnlStart.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.bcpnlStart.RightBottomImage = null;
            this.bcpnlStart.RightImage = null;
            this.bcpnlStart.Size = new System.Drawing.Size(180, 60);
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
            this.dtpStart.FormatDateTime = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.DateTimeFomat.LONG;
            this.dtpStart.FormatOfDate = null;
            this.dtpStart.FormatOfTime = "HH:mm:ss";
            this.dtpStart.FromTo = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.FromToType.FROM_ONLY;
            this.dtpStart.HeightOfNeed = 0;
            this.dtpStart.Hourly = null;
            this.dtpStart.IsForcedResizeOfParent = false;
            this.dtpStart.IsVisibleDateControl = true;
            this.dtpStart.IsVisibleperiodControl = false;
            this.dtpStart.Location = new System.Drawing.Point(61, 0);
            this.dtpStart.MaxDate = new System.DateTime(2110, 12, 31, 0, 0, 0, 0);
            this.dtpStart.MinDate = new System.DateTime(1910, 1, 1, 0, 0, 0, 0);
            this.dtpStart.Monthly = null;
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Order = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.OrderOfControl.Period_Date;
            this.dtpStart.Shiftly = null;
            this.dtpStart.Size = new System.Drawing.Size(116, 60);
            this.dtpStart.StartDateTime = new System.DateTime(2009, 10, 8, 0, 0, 0, 0);
            this.dtpStart.TabIndex = 4;
            this.dtpStart.Text = "bDateTime1";
            this.dtpStart.UserDefineAddTime = null;
            this.dtpStart.UserDefineIsSetStartTime = false;
            this.dtpStart.UserDefinePeriodCustom = false;
            this.dtpStart.UserDefinePeriodDay = 0;
            this.dtpStart.UserDefineStartTime = null;
            this.dtpStart.Value = new System.DateTime(2009, 10, 8, 0, 0, 0, 0);
            this.dtpStart.ValuePeriod = null;
            this.dtpStart.Weekly = null;
            this.dtpStart.WeeklySimbol = "W";
            this.dtpStart.WIDTH_DATE = 90;
            this.dtpStart.WIDTH_TIME = 80;
            this.dtpStart.ValueChanged += new System.EventHandler(this.dtpStart_ValueChanged);
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
            this.lblStart.Location = new System.Drawing.Point(3, 0);
            this.lblStart.Name = "lblStart";
            this.lblStart.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblStart.Size = new System.Drawing.Size(58, 60);
            this.lblStart.TabIndex = 3;
            this.lblStart.Text = "From";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bcpnlEnd
            // 
            this.bcpnlEnd.BottomImage = null;
            this.bcpnlEnd.BottomLeftImage = null;
            this.bcpnlEnd.BssClass = "";
            this.bcpnlEnd.Controls.Add(this.dtpEnd);
            this.bcpnlEnd.Controls.Add(this.lblEnd);
            this.bcpnlEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcpnlEnd.DownImage = null;
            this.bcpnlEnd.Key = "";
            this.bcpnlEnd.LeftImage = null;
            this.bcpnlEnd.LeftTopImage = null;
            this.bcpnlEnd.Location = new System.Drawing.Point(0, 83);
            this.bcpnlEnd.Name = "bcpnlEnd";
            this.bcpnlEnd.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.bcpnlEnd.RightBottomImage = null;
            this.bcpnlEnd.RightImage = null;
            this.bcpnlEnd.Size = new System.Drawing.Size(180, 57);
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
            this.dtpEnd.FormatDateTime = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.DateTimeFomat.LONG;
            this.dtpEnd.FormatOfDate = null;
            this.dtpEnd.FormatOfTime = "HH:mm:ss";
            this.dtpEnd.FromTo = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.FromToType.TO_ONLY;
            this.dtpEnd.HeightOfNeed = 0;
            this.dtpEnd.Hourly = null;
            this.dtpEnd.IsForcedResizeOfParent = false;
            this.dtpEnd.IsVisibleDateControl = true;
            this.dtpEnd.IsVisibleperiodControl = false;
            this.dtpEnd.Location = new System.Drawing.Point(61, 0);
            this.dtpEnd.MaxDate = new System.DateTime(2110, 12, 31, 0, 0, 0, 0);
            this.dtpEnd.MinDate = new System.DateTime(1910, 1, 1, 0, 0, 0, 0);
            this.dtpEnd.Monthly = null;
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Order = BISTel.PeakPerformance.Client.BISTelControl.BDateTime.OrderOfControl.Period_Date;
            this.dtpEnd.Shiftly = null;
            this.dtpEnd.Size = new System.Drawing.Size(116, 57);
            this.dtpEnd.StartDateTime = new System.DateTime(2009, 10, 8, 0, 0, 0, 0);
            this.dtpEnd.TabIndex = 5;
            this.dtpEnd.Text = "bDateTime2";
            this.dtpEnd.UserDefineAddTime = null;
            this.dtpEnd.UserDefineIsSetStartTime = false;
            this.dtpEnd.UserDefinePeriodCustom = false;
            this.dtpEnd.UserDefinePeriodDay = 0;
            this.dtpEnd.UserDefineStartTime = null;
            this.dtpEnd.Value = new System.DateTime(2009, 10, 8, 0, 0, 0, 0);
            this.dtpEnd.ValuePeriod = null;
            this.dtpEnd.Weekly = null;
            this.dtpEnd.WeeklySimbol = "W";
            this.dtpEnd.WIDTH_DATE = 90;
            this.dtpEnd.WIDTH_TIME = 80;
            this.dtpEnd.ValueChanged += new System.EventHandler(this.dtpEnd_ValueChanged);
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
            this.lblEnd.Location = new System.Drawing.Point(3, 0);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.lblEnd.Size = new System.Drawing.Size(58, 57);
            this.lblEnd.TabIndex = 4;
            this.lblEnd.Text = "To";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // DateCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.bcpnlEnd);
            this.Controls.Add(this.bcpnlStart);
            this.Controls.Add(this.bcpnlType);
            this.Name = "DateCondition";
            this.Size = new System.Drawing.Size(180, 140);
            this.bcpnlType.ResumeLayout(false);
            this.bcpnlStart.ResumeLayout(false);
            this.bcpnlEnd.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlType;
        private BISTel.PeakPerformance.Client.BISTelControl.BComboBox cboType;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblType;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlStart;
        private BISTel.PeakPerformance.Client.BISTelControl.BDateTime dtpStart;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblStart;
        private BISTel.PeakPerformance.Client.BISTelControl.BConditionPanel bcpnlEnd;
        private BISTel.PeakPerformance.Client.BISTelControl.BDateTime dtpEnd;
        private BISTel.PeakPerformance.Client.BISTelControl.BLabel lblEnd;
    }
}
