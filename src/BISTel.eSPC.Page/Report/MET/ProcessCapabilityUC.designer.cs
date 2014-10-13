namespace BISTel.eSPC.Page.Report.MET
{
	partial class ProcessCapabilityUC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessCapabilityUC));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo2 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            this.btitMonthly = new BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel();
            this.bsprData = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bsprData_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.bbtnList = new BISTel.PeakPerformance.Client.BISTelControl.BButtonList();
            this.btitMonthly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // btitMonthly
            // 
            this.btitMonthly.BssClass = "TitlePanel";
            this.btitMonthly.Controls.Add(this.bsprData);
            this.btitMonthly.Controls.Add(this.bbtnList);
            this.btitMonthly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btitMonthly.ImageBottom = null;
            this.btitMonthly.ImageBottomRight = null;
            this.btitMonthly.ImageClose = null;
            this.btitMonthly.ImageLeft = ((System.Drawing.Image)(resources.GetObject("btitMonthly.ImageLeft")));
            this.btitMonthly.ImageLeftBottom = null;
            this.btitMonthly.ImageLeftFill = null;
            this.btitMonthly.ImageMax = null;
            this.btitMonthly.ImageMiddle = ((System.Drawing.Image)(resources.GetObject("btitMonthly.ImageMiddle")));
            this.btitMonthly.ImageMin = null;
            this.btitMonthly.ImageRight = ((System.Drawing.Image)(resources.GetObject("btitMonthly.ImageRight")));
            this.btitMonthly.ImageRightFill = null;
            this.btitMonthly.IsPopupMax = false;
            this.btitMonthly.IsSelected = false;
            this.btitMonthly.Key = "";
            this.btitMonthly.Location = new System.Drawing.Point(0, 0);
            this.btitMonthly.MaxControlName = "";
            this.btitMonthly.MaxResizable = false;
            this.btitMonthly.Name = "btitMonthly";
            this.btitMonthly.Padding = new System.Windows.Forms.Padding(0, 32, 0, 0);
            this.btitMonthly.PaddingTop = 4;
            this.btitMonthly.PopMaxHeight = 0;
            this.btitMonthly.PopMaxWidth = 0;
            this.btitMonthly.RightPadding = 5;
            this.btitMonthly.ShowCloseButton = false;
            this.btitMonthly.Size = new System.Drawing.Size(798, 503);
            this.btitMonthly.TabIndex = 1;
            this.btitMonthly.Title = "Process Capability";
            this.btitMonthly.TitleColor = System.Drawing.Color.Black;
            this.btitMonthly.TitleFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btitMonthly.TopPadding = 5;
            // 
            // bsprData
            // 
            this.bsprData.About = "3.0.2005.2005";
            this.bsprData.AccessibleDescription = "";
            this.bsprData.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bsprData.AddtionalCodeList")));
            this.bsprData.AllowNewRow = true;
            this.bsprData.AutoClipboard = false;
            this.bsprData.AutoGenerateColumns = false;
            this.bsprData.BssClass = "";
            this.bsprData.ClickPos = new System.Drawing.Point(0, 0);
            this.bsprData.ColFronzen = 0;
            columnInfo2.CheckBoxFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo2.CheckBoxFields")));
            columnInfo2.ComboFields = ((System.Collections.Generic.List<int>)(resources.GetObject("columnInfo2.ComboFields")));
            columnInfo2.DefaultValue = ((System.Collections.Generic.Dictionary<int, string>)(resources.GetObject("columnInfo2.DefaultValue")));
            columnInfo2.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.KeyFields")));
            columnInfo2.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.MandatoryFields")));
            columnInfo2.SaveTableInfo = ((System.Collections.SortedList)(resources.GetObject("columnInfo2.SaveTableInfo")));
            columnInfo2.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo2.UniqueFields")));
            this.bsprData.columnInformation = columnInfo2;
            this.bsprData.ComboEnable = true;
            this.bsprData.DataAutoHeadings = false;
            this.bsprData.DataSet = null;
            this.bsprData.DateToDateTimeFormat = false;
            this.bsprData.DefaultDeleteValue = true;
            this.bsprData.DisplayColumnHeader = true;
            this.bsprData.DisplayRowHeader = true;
            this.bsprData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bsprData.EditModeReplace = true;
            this.bsprData.FilterVisible = false;
            this.bsprData.HeadHeight = 20F;
            this.bsprData.IsCellCopy = false;
            this.bsprData.IsMultiLanguage = true;
            this.bsprData.IsReport = false;
            this.bsprData.Key = "";
            this.bsprData.Location = new System.Drawing.Point(0, 62);
            this.bsprData.Name = "bsprData";
            this.bsprData.RowFronzen = 0;
            this.bsprData.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bsprData.ScrollBarTrackPolicy = FarPoint.Win.Spread.ScrollBarTrackPolicy.Both;
            this.bsprData.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bsprData_Sheet1});
            this.bsprData.Size = new System.Drawing.Size(798, 441);
            this.bsprData.TabIndex = 5;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bsprData.TextTipAppearance = tipAppearance2;
            this.bsprData.UseCheckAll = false;
            this.bsprData.UseCommandIcon = false;
            this.bsprData.UseFilter = false;
            this.bsprData.UseGeneralContextMenu = true;
            this.bsprData.UseHeadColor = false;
            this.bsprData.UseOriginalEvent = false;
            this.bsprData.UseSpreadEdit = true;
            this.bsprData.UseWidthMemory = true;
            this.bsprData.WhenDeleteUseModify = false;
		    this.bsprData.Locked = true;
            this.bsprData.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(bsprData_ButtonClicked);
            this.bsprData.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(bsprData_CellDoubleClick);
            // 
            // bsprData_Sheet1
            // 
            this.bsprData_Sheet1.Reset();
            this.bsprData_Sheet1.SheetName = "Sheet1";
            // 
            // bbtnList
            // 
            this.bbtnList.BssClass = "";
            this.bbtnList.ButtonHorizontalSpacing = 1;
            this.bbtnList.ButtonMouseOverStyle = System.Windows.Forms.ButtonState.Normal;
            this.bbtnList.Dock = System.Windows.Forms.DockStyle.Top;
            this.bbtnList.HorizontalMarginSpacing = 5;
            this.bbtnList.IsCondition = false;
            this.bbtnList.Location = new System.Drawing.Point(0, 32);
            this.bbtnList.MarginSpace = 5;
            this.bbtnList.Name = "bbtnList";
            this.bbtnList.Size = new System.Drawing.Size(798, 30);
            this.bbtnList.TabIndex = 4;
            this.bbtnList.ButtonClick +=new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(bbtnList_ButtonClick);
            
            // 
            // ProcessCapabilityUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btitMonthly);
            this.Name = "ProcessCapabilityUC";
            this.Size = new System.Drawing.Size(798, 503);
            this.btitMonthly.ResumeLayout(false);
            this.btitMonthly.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsprData_Sheet1)).EndInit();
            this.ResumeLayout(false);

		}

       
  



		#endregion

        private BISTel.PeakPerformance.Client.BISTelControl.BTitlePanel btitMonthly;
        private BISTel.PeakPerformance.Client.BISTelControl.BButtonList bbtnList;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bsprData;
        private FarPoint.Win.Spread.SheetView bsprData_Sheet1;
	}
}
