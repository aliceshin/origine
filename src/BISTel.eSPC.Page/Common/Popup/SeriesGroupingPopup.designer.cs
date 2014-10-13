namespace BISTel.eSPC.Page.Common
{
    partial class SeriesGroupingPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeriesGroupingPopup));
            BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo columnInfo1 = new BISTel.PeakPerformance.Client.BISTelControl.ColumnInfo();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.pnl_Button = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btn_Draw = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.bnt_Cancel = new BISTel.PeakPerformance.Client.BISTelControl.BButton();
            this.pnl_Spread = new System.Windows.Forms.Panel();
            this.bspr_SeriesGrouping = new BISTel.PeakPerformance.Client.BISTelControl.BSpread();
            this.bSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pnl_Sort = new System.Windows.Forms.Panel();
            this.cb_STEP_ID = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cb_Param_Name = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cb_SLOT_ID = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cb_LOT_ID = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cb_RECIPE_ID = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_MODULE_ID = new System.Windows.Forms.CheckBox();
            this.pnlContentsArea.SuspendLayout();
            this.pnl_Button.SuspendLayout();
            this.panel6.SuspendLayout();
            this.pnl_Spread.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bspr_SeriesGrouping)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).BeginInit();
            this.pnl_Sort.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContentsArea
            // 
            this.pnlContentsArea.Controls.Add(this.pnl_Spread);
            this.pnlContentsArea.Controls.Add(this.pnl_Button);
            resources.ApplyResources(this.pnlContentsArea, "pnlContentsArea");
            // 
            // pnl_Button
            // 
            this.pnl_Button.Controls.Add(this.panel6);
            resources.ApplyResources(this.pnl_Button, "pnl_Button");
            this.pnl_Button.Name = "pnl_Button";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btn_Draw);
            this.panel6.Controls.Add(this.bnt_Cancel);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // btn_Draw
            // 
            this.btn_Draw.AutoButtonSize = false;
            resources.ApplyResources(this.btn_Draw, "btn_Draw");
            this.btn_Draw.BssClass = "ConditionButton";
            this.btn_Draw.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.btn_Draw.IsMultiLanguage = true;
            this.btn_Draw.Key = "";
            this.btn_Draw.Name = "btn_Draw";
            this.btn_Draw.TabStop = false;
            this.btn_Draw.ToolTipText = "";
            this.btn_Draw.UseVisualStyleBackColor = true;
            this.btn_Draw.Click += new System.EventHandler(this.btn_Draw_Click);
            // 
            // bnt_Cancel
            // 
            this.bnt_Cancel.AutoButtonSize = false;
            resources.ApplyResources(this.bnt_Cancel, "bnt_Cancel");
            this.bnt_Cancel.BssClass = "ConditionButton";
            this.bnt_Cancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(97)))), ((int)(((byte)(133)))));
            this.bnt_Cancel.IsMultiLanguage = false;
            this.bnt_Cancel.Key = "";
            this.bnt_Cancel.Name = "bnt_Cancel";
            this.bnt_Cancel.TabStop = false;
            this.bnt_Cancel.ToolTipText = "";
            this.bnt_Cancel.UseVisualStyleBackColor = true;
            this.bnt_Cancel.Click += new System.EventHandler(this.bnt_Cancel_Click);
            // 
            // pnl_Spread
            // 
            this.pnl_Spread.Controls.Add(this.bspr_SeriesGrouping);
            this.pnl_Spread.Controls.Add(this.pnl_Sort);
            resources.ApplyResources(this.pnl_Spread, "pnl_Spread");
            this.pnl_Spread.Name = "pnl_Spread";
            // 
            // bspr_SeriesGrouping
            // 
            this.bspr_SeriesGrouping.About = "3.0.2005.2005";
            resources.ApplyResources(this.bspr_SeriesGrouping, "bspr_SeriesGrouping");
            this.bspr_SeriesGrouping.AddtionalCodeList = ((System.Collections.SortedList)(resources.GetObject("bspr_SeriesGrouping.AddtionalCodeList")));
            this.bspr_SeriesGrouping.AllowNewRow = true;
            this.bspr_SeriesGrouping.AutoClipboard = false;
            this.bspr_SeriesGrouping.AutoGenerateColumns = false;
            this.bspr_SeriesGrouping.BssClass = "";
            this.bspr_SeriesGrouping.ClickPos = new System.Drawing.Point(0, 0);
            this.bspr_SeriesGrouping.ColFronzen = 0;
            columnInfo1.KeyFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.KeyFields")));
            columnInfo1.MandatoryFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.MandatoryFields")));
            columnInfo1.UniqueFields = ((System.Collections.ArrayList)(resources.GetObject("columnInfo1.UniqueFields")));
            this.bspr_SeriesGrouping.columnInformation = columnInfo1;
            this.bspr_SeriesGrouping.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.bspr_SeriesGrouping.ComboEnable = true;
            this.bspr_SeriesGrouping.DataAutoHeadings = false;
            this.bspr_SeriesGrouping.DataSet = null;
            this.bspr_SeriesGrouping.DateToDateTimeFormat = false;
            this.bspr_SeriesGrouping.DefaultDeleteValue = true;
            this.bspr_SeriesGrouping.DisplayColumnHeader = true;
            this.bspr_SeriesGrouping.DisplayRowHeader = true;
            this.bspr_SeriesGrouping.EditModeReplace = true;
            this.bspr_SeriesGrouping.FilterVisible = false;
            this.bspr_SeriesGrouping.HeadHeight = 20F;
            this.bspr_SeriesGrouping.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bspr_SeriesGrouping.IsCellCopy = false;
            this.bspr_SeriesGrouping.IsMultiLanguage = true;
            this.bspr_SeriesGrouping.IsReport = false;
            this.bspr_SeriesGrouping.Key = "";
            this.bspr_SeriesGrouping.Name = "bspr_SeriesGrouping";
            this.bspr_SeriesGrouping.RowFronzen = 0;
            this.bspr_SeriesGrouping.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
            this.bspr_SeriesGrouping.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
            this.bspr_SeriesGrouping.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.bSpread1_Sheet1});
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bspr_SeriesGrouping.TextTipAppearance = tipAppearance1;
            this.bspr_SeriesGrouping.UseFilter = false;
            this.bspr_SeriesGrouping.UseGeneralContextMenu = true;
            this.bspr_SeriesGrouping.UseHeadColor = false;
            this.bspr_SeriesGrouping.UseOriginalEvent = false;
            this.bspr_SeriesGrouping.UseSpreadEdit = true;
            this.bspr_SeriesGrouping.UseWidthMemory = true;
            this.bspr_SeriesGrouping.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.bspr_SeriesGrouping.WhenDeleteUseModify = false;
            // 
            // bSpread1_Sheet1
            // 
            this.bSpread1_Sheet1.Reset();
            this.bSpread1_Sheet1.SheetName = "Sheet1";
            // 
            // pnl_Sort
            // 
            this.pnl_Sort.Controls.Add(this.cb_STEP_ID);
            this.pnl_Sort.Controls.Add(this.panel5);
            this.pnl_Sort.Controls.Add(this.cb_Param_Name);
            this.pnl_Sort.Controls.Add(this.panel4);
            this.pnl_Sort.Controls.Add(this.cb_SLOT_ID);
            this.pnl_Sort.Controls.Add(this.panel3);
            this.pnl_Sort.Controls.Add(this.cb_LOT_ID);
            this.pnl_Sort.Controls.Add(this.panel2);
            this.pnl_Sort.Controls.Add(this.cb_RECIPE_ID);
            this.pnl_Sort.Controls.Add(this.panel1);
            this.pnl_Sort.Controls.Add(this.cb_MODULE_ID);
            resources.ApplyResources(this.pnl_Sort, "pnl_Sort");
            this.pnl_Sort.Name = "pnl_Sort";
            // 
            // cb_STEP_ID
            // 
            resources.ApplyResources(this.cb_STEP_ID, "cb_STEP_ID");
            this.cb_STEP_ID.Name = "cb_STEP_ID";
            this.cb_STEP_ID.UseVisualStyleBackColor = true;
            this.cb_STEP_ID.CheckedChanged += new System.EventHandler(this.cb_STEP_ID_CheckedChanged);
            // 
            // panel5
            // 
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // cb_Param_Name
            // 
            resources.ApplyResources(this.cb_Param_Name, "cb_Param_Name");
            this.cb_Param_Name.Name = "cb_Param_Name";
            this.cb_Param_Name.UseVisualStyleBackColor = true;
            this.cb_Param_Name.CheckedChanged += new System.EventHandler(this.cb_Param_Name_CheckedChanged);
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // cb_SLOT_ID
            // 
            resources.ApplyResources(this.cb_SLOT_ID, "cb_SLOT_ID");
            this.cb_SLOT_ID.Name = "cb_SLOT_ID";
            this.cb_SLOT_ID.UseVisualStyleBackColor = true;
            this.cb_SLOT_ID.CheckedChanged += new System.EventHandler(this.cb_SLOT_ID_CheckedChanged);
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // cb_LOT_ID
            // 
            resources.ApplyResources(this.cb_LOT_ID, "cb_LOT_ID");
            this.cb_LOT_ID.Name = "cb_LOT_ID";
            this.cb_LOT_ID.UseVisualStyleBackColor = true;
            this.cb_LOT_ID.CheckedChanged += new System.EventHandler(this.cb_LOT_ID_CheckedChanged);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // cb_RECIPE_ID
            // 
            resources.ApplyResources(this.cb_RECIPE_ID, "cb_RECIPE_ID");
            this.cb_RECIPE_ID.Name = "cb_RECIPE_ID";
            this.cb_RECIPE_ID.UseVisualStyleBackColor = true;
            this.cb_RECIPE_ID.CheckedChanged += new System.EventHandler(this.cb_RECIPE_ID_CheckedChanged);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // cb_MODULE_ID
            // 
            resources.ApplyResources(this.cb_MODULE_ID, "cb_MODULE_ID");
            this.cb_MODULE_ID.Name = "cb_MODULE_ID";
            this.cb_MODULE_ID.UseVisualStyleBackColor = true;
            this.cb_MODULE_ID.CheckedChanged += new System.EventHandler(this.cb_MODULE_ID_CheckedChanged);
            // 
            // SeriesGroupingPopup
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContentsAreaMinHeight = 360;
            this.ContentsAreaMinWidth = 1150;
            this.Name = "SeriesGroupingPopup";
            this.Resizable = true;
            this.Title = "Series Grouping";
            this.pnlContentsArea.ResumeLayout(false);
            this.pnl_Button.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.pnl_Spread.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bspr_SeriesGrouping)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bSpread1_Sheet1)).EndInit();
            this.pnl_Sort.ResumeLayout(false);
            this.pnl_Sort.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnl_Button;
        private System.Windows.Forms.Panel pnl_Spread;
        private BISTel.PeakPerformance.Client.BISTelControl.BSpread bspr_SeriesGrouping;
        private FarPoint.Win.Spread.SheetView bSpread1_Sheet1;
        private System.Windows.Forms.Panel pnl_Sort;
        private System.Windows.Forms.CheckBox cb_STEP_ID;
        private System.Windows.Forms.CheckBox cb_SLOT_ID;
        private System.Windows.Forms.CheckBox cb_LOT_ID;
        private System.Windows.Forms.CheckBox cb_RECIPE_ID;
        private System.Windows.Forms.CheckBox cb_MODULE_ID;
        private System.Windows.Forms.CheckBox cb_Param_Name;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton btn_Draw;
        private BISTel.PeakPerformance.Client.BISTelControl.BButton bnt_Cancel;
        private System.Windows.Forms.Panel panel6;
    }
}