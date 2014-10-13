using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common.ATT;
using BISTel.eSPC.Page.ATT.History;
using BISTel.eSPC.Page.ATT.Modeling;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;

namespace BISTel.eSPC.Page.ATT.Compare
{
    public enum CompareType
    {
        Model,
        Version
    }

    public partial class SPCModelCompareResultUC : BasePageUCtrl
    {
        private string itemKey = string.Empty;
        private Initialization _initialization = null;
        private ICompareResultController _controller = null;
        private string _line = string.Empty;
        private string _area = string.Empty;
        private string _eqpModel = string.Empty;
        private string _paramAlias = string.Empty;
        private string _paramType = string.Empty;
        private string port = string.Empty;
        public CompareType compareType = CompareType.Model;
        private LinkedList _lastestCondition = null;

        public ModelModifiedEventHandler modelModifiedEventHandler = null;

        public delegate void LinkTraceDataViewEventHandlerResult(object sender, EventArgs e, LinkedList llstTraceLinkData);
        public event LinkTraceDataViewEventHandlerResult linkTraceDataViewEventResult;

        private string _groupName; //SPC-1292, KBLEE

        //SPC-1292, KBLEE, START
        public string AREA_RAWID
        {
            get { return this._area; }
            set { this._area = value; }
        }
        public string LOCATION_RAWID
        {
            get { return this._line; }
            set { this._line = value; }
        }
        public string EQP_MODEL
        {
            get { return this._eqpModel; }
            set { this._eqpModel = value; }
        }
        public string GROUP_NAME
        {
            get { return this._groupName; }
            set { this._groupName = value; }
        }
        //SPC-1292, KBLEE, END

        public SPCModelCompareResultUC()
        {
            InitializeComponent();
        }

        public override void PageInit()
        {
            InitializePage();
        }

        public void InitializePage(string functionName, string pageKey, string itemKey, SessionData session, string URL, string Port)
        {
            this.FunctionName = functionName;
            this.KeyOfPage = pageKey;
            this.itemKey = itemKey;
            this.sessionData = session;
            this.URL = URL;
            port = Port;

            this.InitializePage();
        }

        public override void PageSearch(LinkedList llCondition)
        {
            string sSite = string.Empty;
            string sFab = string.Empty;
            string sLine = string.Empty;
            string sArea = string.Empty;

            if(llCondition[Definition.DynamicCondition_Search_key.SITE] != null
                && ((DataTable) llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows.Count > 0)
            {
                 sSite =
                    ((DataTable) llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if(llCondition[Definition.DynamicCondition_Search_key.FAB] != null
                && ((DataTable) llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows.Count > 0)
            {
                 sFab =
                    ((DataTable) llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if(llCondition[Definition.DynamicCondition_Search_key.LINE] != null
                && ((DataTable) llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows.Count > 0)
            {
                _line =
                    ((DataTable) llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                sLine =  ((DataTable) llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
            }

            if(llCondition[Definition.DynamicCondition_Search_key.AREA] != null
                && ((DataTable) llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows.Count > 0)
            {
                _area =
                    ((DataTable) llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                sArea = ((DataTable) llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if(llCondition[Definition.DynamicCondition_Search_key.EQPMODEL] != null
                && ((DataTable) llCondition[Definition.DynamicCondition_Search_key.EQPMODEL]).Rows.Count > 0)
            {
                _eqpModel =
                    ((DataTable) llCondition[Definition.DynamicCondition_Search_key.EQPMODEL]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }

            if(llCondition[Definition.DynamicCondition_Search_key.PARAM_ALIAS] != null
                && ((DataTable) llCondition[Definition.DynamicCondition_Search_key.PARAM_ALIAS]).Rows.Count > 0)
            {
                _paramAlias =
                    ((DataTable) llCondition[Definition.DynamicCondition_Search_key.PARAM_ALIAS]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if(llCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE] != null
                && ((DataTable) llCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE]).Rows.Count > 0)
            {
                _paramType =
                    ((DataTable) llCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }

            this.ApplyAuthory(this.bbtnList, sSite, sFab, sLine, sArea);
        }

        public void InitializePage()
        {
            InitializeVariables();

            InitializeBspr();

            InitializeDataButton();

            InitializeController();
        }

        private void InitializeVariables()
        {
            if (this._initialization == null)
            {
                this._initialization = new Initialization();
                this._initialization.InitializePath();
            }

            _line = string.Empty;
            _area = string.Empty;
        }

        private void InitializeBspr()
        {
            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
            this.bsprData.UseSpreadEdit = false;
            this.bsprData.UseAutoSort = false;
            this.bsprData.UseEdit = true;
            this.bsprData.DataSet = null;
            this.bsprData.SetFilterVisible(false);
            this.bsprData.UseGeneralContextMenu = false;
        }

        private void InitializeDataButton()
        {
            this._initialization.InitializeButtonList(this.bbtnList, ref bsprData, this.KeyOfPage, itemKey, this.sessionData);

            this.ApplyAuthory(this.bbtnList);
        }

        private void InitializeController()
        {
            if(compareType == CompareType.Model)
            {
                _controller = new SPCModelCompareResultController();
            }
            else if(compareType == CompareType.Version)
            {
                _controller = new SPCModelVersionCompareResultController();
            }
            else
            {
                _controller = new SPCModelCompareResultController();
            }
        }

        internal void Compare(LinkedList llCondition)
        {
            _lastestCondition = llCondition;

            if(compareType == CompareType.Model)
            {
                string[] chartIDs = (string[])llCondition["CHART_ID"];

                if (chartIDs == null || chartIDs.Length < 1)
                {
                    MSGHandler.DisplayMessage(MSGType.Error, "SPC_INFO_NO_COMPARE_CHART_ID", null, null);
                    return;
                }
            }

            this.InitializeController();

            DataTable dtData = this._controller.GetComparedDataTable(llCondition);

            DisplayData(dtData);

            ApplyColor();

            ApplyDifferentValueColor();

            this.bsprData.Locked = true;
        }

        private void DisplayData(DataTable dtData)
        {
            InitializeBspr();

            for (int i = 0; i < dtData.Columns.Count; i++)
            {
                string columnName = dtData.Columns[i].ColumnName;
                this.bsprData.AddHead(i, columnName, columnName, 130, 200, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false,
                                      true);
            }

            FarPoint.Win.Spread.CellType.CheckBoxCellType checkboxType = null;
            for (int i = 2; i < dtData.Columns.Count; i++)
            {
                checkboxType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                checkboxType.Caption = dtData.Columns[i].ColumnName;
                this.bsprData.ActiveSheet.ColumnHeader.Cells[0, i].CellType = checkboxType;
            }

            this.bsprData.AddHeadComplete();

            this.bsprData.DataSet = dtData;

            this.bsprData.ActiveSheet.Columns[0].MergePolicy = MergePolicy.Always;

            for (int i = 2; i < dtData.Columns.Count; i++)
            {
                this.bsprData.ActiveSheet.ColumnHeader.Cells[0, i].Text = "False";
            }

            this.bsprData.ActiveSheet.AddSpanCell(0, 0, 1, 2);
            this.bsprData.ActiveSheet.AddSpanCell(1, 0, 1, 2);
            this.bsprData.ActiveSheet.AddColumnHeaderSpanCell(0, 0, 1, 2);
        }

        private void ApplyColor()
        {
            this.bsprData.ActiveSheet.Columns[0, 1].BackColor = Color.FromArgb(153, 153, 255);
        }

        private void ApplyDifferentValueColor()
        {
            for (int i = 2; i < this.bsprData.ActiveSheet.Rows.Count; i++)
            {
                for (int j = 2; j < this.bsprData.ActiveSheet.Columns.Count - 1; j++)
                {
                    if (this.bsprData.ActiveSheet.Cells[i, j].Text
                        != this.bsprData.ActiveSheet.Cells[i, j + 1].Text)
                    {
                        this.bsprData.ActiveSheet.Cells[i, 2, i, this.bsprData.ActiveSheet.Columns.Count - 1].BackColor = Color.FromArgb(255, 153, 102);
                        break;
                    }
                }
            }
        }

        private void bbtnList_ButtonClick(string name)
        {
            switch(name.ToUpper())
            {
                case "COPY" :
                    Copy();
                    break;
                case "PASTE" :
                    Paste();
                    break;
                case "SPC_MODIFY_MODEL":
                    Modify();
                    break;
                case "SPC_CONFIGURATION":
                    ViewModel();
                    break;
                case "SPC_VIEW_CHART" :
                    ViewChart();
                    break;
                case "EXPORT" :
                   // this.bsprData.Export(true);
                    BSpreadUtility bsprUtil = new BSpreadUtility();
                    bsprUtil.Export(this.bsprData, true);
                    break;
            }
        }

        private void bsprData_CellClick(object sender, CellClickEventArgs e)
        {
            if(e.ColumnHeader == true && e.Column > 1)
            {
                if(this.bsprData.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].Text == "True")
                {
                    this.bsprData.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].Text = "False";
                }
                else
                {
                    this.bsprData.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].Text = "True";
                }
            }
        }

        private void Copy()
        {
            int[] selectedColumns = GetSelectedColumns();
            if(selectedColumns.Length != 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ONE_SPC_MODEL", null, null);
                return;
            }

            Control con = this.Parent;
            while(con.Parent != null)
            {
                if(con is SPCModelCompareUC)
                    break;
                con = con.Parent;
            }

            if(((SPCModelCompareUC) con).copyPopup == null)
            {
                ((SPCModelCompareUC) con).copyPopup = new SPCCopySpec();
                ((SPCModelCompareUC) con).copyPopup.InitializeControl();
            }
            string chartId = GetHeader(selectedColumns[0]);
            string mainYN = GetMainYN(selectedColumns[0]);
            string spcModelName = GetSPCModelName(selectedColumns[0]);

            ((SPCModelCompareUC) con).copyPopup.SetCopyModelInfo(this._eqpModel, spcModelName, mainYN, chartId, this._paramAlias);
            ((SPCModelCompareUC) con).copyPopup.ShowDialog();
        }

        private void Paste()
        {
            Control con = this.Parent;
            while (con.Parent != null)
            {
                if (con is SPCModelCompareUC)
                    break;
                con = con.Parent;
            }

            SPCCopySpec copyPopup = ((SPCModelCompareUC)con).copyPopup;

            if (copyPopup == null
                || copyPopup.CONFIG_RAWID.Trim().Length < 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_COPY_SOURCE", null, null);
                return;
            }

            int[] selectedColumns = GetSelectedColumns();
            if (selectedColumns.Length == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_TARGET_MODEL", null, null);
                return;
            }

            for (int i = 0; i < selectedColumns.Length; i++)
            {
                if (copyPopup.CONFIG_RAWID.ToString() == GetHeader(selectedColumns[i]))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_TARGET_SAME", null, null);
                    return;
                }
            }

            if (copyPopup.RULE_PN_SPEC_LIMIT.ToString().Equals("Y") ||
                copyPopup.RULE_PN_CONTROL.ToString().Equals("Y") )
            {
                LinkedList toraltargetRawidList = new LinkedList();
                string[] targetRawid = new string[selectedColumns.Length];
                for (int i = 0; i < selectedColumns.Length; i++)
                {
                    LinkedList targetRawidList = new LinkedList();
                    targetRawid[i] = GetHeader(selectedColumns[i]);
                }
                DataSet targetSpecData = this._controller.GetATTTargetConfigSpecData(targetRawid);
                DataSet sourceSpecData = this._controller.GetATTSourseConfigSpecData(copyPopup.CONFIG_RAWID.ToString());

                if (targetSpecData != null && sourceSpecData != null)
                {
                    bool CompareResult = true;
                    if (copyPopup.RULE_PN_SPEC_LIMIT.ToString().Equals("Y") && copyPopup.RULE_PN_CONTROL.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "11", "PN");
                    }
                    else if (copyPopup.RULE_PN_SPEC_LIMIT.ToString().Equals("N") && copyPopup.RULE_PN_CONTROL.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "01", "PN");
                    }
                    else if (copyPopup.RULE_PN_SPEC_LIMIT.ToString().Equals("Y") && copyPopup.RULE_PN_CONTROL.ToString().Equals("N"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "10", "PN");
                    }
                    if (!CompareResult)
                    {
                        return;
                    }
                }

            }

            DialogResult result = MSGHandler.DialogQuestionResult("SPC_INFO_DIALOG_COPY?",
                                                                  new string[] { "Model Copy" }, MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                StringBuilder resultMessage = new StringBuilder();

                //SPC-632 
                LinkedList TotalConfig = new LinkedList();
                string chartId = null;
                string mainYN = null;


                this.MsgShow(COMMON_MSG.Query_Data);
                for (int i = 0; i < selectedColumns.Length; i++)
                {
                    chartId = GetHeader(selectedColumns[i]);
                    mainYN = GetMainYN(selectedColumns[i]);

                    TotalConfig.Add(i, this._controller.Paste(copyPopup, chartId, mainYN, this.sessionData.UserRawID));

                }
                string message = this._controller.PasteModel(TotalConfig);
                switch (message)
                {
                    case "FAIL":
                        resultMessage.AppendLine(chartId + " : " + MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
                        break;
                    case Definition.MSG_KEY_NO_PASTE_ITEM:
                        resultMessage.AppendLine(chartId + " : " + MSGHandler.GetMessage("SPC_INFO_NO_ITEM_PASTE"));
                        break;
                }

                this.MsgClose();
                if (resultMessage.Length != 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, resultMessage.ToString());
                }
                else
                {
                    this.RefreshData();
                }
            }
        }

        private void Modify()
        {
            int[] selectedColumns = GetSelectedColumns();
            if(selectedColumns.Length == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                return;
            }
            else if(selectedColumns.Length > 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ONE_SPC_MODEL", null, null);
                return;
            }

            string chartId = GetHeader(selectedColumns[0]);
            string mainYN = GetMainYN(selectedColumns[0]);

            //SPC-1292, KBLEE, START
            LinkedList llGroupName = this._controller.GetGroupNameByChartID(chartId);
            if (llGroupName.Count > 0)
            {
                _groupName = llGroupName[BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString();
            }
            else
            {
                _groupName = Definition.VARIABLE_UNASSIGNED_MODEL;
            }
            //SPC-1292, KBLEE, END

            if(this._controller.Modify(this.sessionData, this.URL, port, _line, _area, _eqpModel, chartId, mainYN, _groupName) == DialogResult.OK)
            {
                RefreshData();
            }
        }

        private void ViewModel()
        {
            int[] selectedColumns = GetSelectedColumns();
            if(selectedColumns.Length == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                return;
            }
            else if(selectedColumns.Length > 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ONE_SPC_MODEL", null, null);
                return;
            }

            string chartId = _lastestCondition["CHART_ID"].ToString();
            string version = GetHeader(selectedColumns[0]);
            if (version != null && version.Length > 0)
            {
                version = Convert.ToInt32(((Convert.ToDouble(version) - 1)*100)).ToString();
            }
            string mainYN = GetMainYN(selectedColumns[0]);

            if(this._controller.ViewModel(this.sessionData, this.URL, port, _line, _area, _eqpModel, chartId, mainYN, version, _groupName) == DialogResult.OK)
            {
                if(modelModifiedEventHandler != null)
                    modelModifiedEventHandler(this, null);

                InitializePage();
            }
        }

        private void ViewChart()
        {
            int[] selectedColumns = GetSelectedColumns();
            if(selectedColumns.Length == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_CHART_ID", null, null);
                return;
            }
            if(selectedColumns.Length > 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_SINGLE_CHART_ID", null, null);
                return;
            }

            string spcModelName = GetSPCModelName(selectedColumns[0]);
            string chartId = GetHeader(selectedColumns[0]);

            this.ViewChart(this.sessionData, this.URL, this._line, this._area, spcModelName, this._paramAlias, chartId, this._paramType);
        }

        public void ViewChart(SessionData sessionData, string URL, string line, string area, string spcModelName, string paramAlias, string chartId, string paramType)
        {
            Common.ChartViewPopup chartViewPop = new Common.ChartViewPopup();
            chartViewPop.URL = URL;
            chartViewPop.SessionData = sessionData;

            chartViewPop.ChartVariable.CHART_PARENT_MODE = BISTel.eSPC.Common.CHART_PARENT_MODE.MODELING; 
            chartViewPop.ChartVariable.LINE = line;
            chartViewPop.ChartVariable.AREA = area;
            chartViewPop.AREA_RAWID = area;
            chartViewPop.LINE_RAWID = line;
            chartViewPop.ChartVariable.SPC_MODEL = spcModelName;
            chartViewPop.ChartVariable.PARAM_ALIAS = paramAlias;
            chartViewPop.ChartVariable.MODEL_CONFIG_RAWID = chartId;

            //SPC-1292, KBLEE,START
            LinkedList llGroupName = this._controller.GetGroupNameByChartID(chartId);
            if (llGroupName.Count > 0)
            {
                _groupName = llGroupName[BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString();
            }
            else
            {
                _groupName = Definition.VARIABLE_UNASSIGNED_MODEL;
            }

            chartViewPop.GROUP_NAME = _groupName;
            //SPC-1292, KBLEE,END

            chartViewPop.InitializePopup();
            chartViewPop.linkTraceDataViewEventPopup -= new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
            chartViewPop.linkTraceDataViewEventPopup += new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);

            chartViewPop.ShowDialog();
        }

        void chartViewPop_linkTraceDataViewEventPopup(object sender, EventArgs e, LinkedList llstTraceLinkData)
        {
            if(this.linkTraceDataViewEventResult != null)
                linkTraceDataViewEventResult(this, null, llstTraceLinkData);
        } 

        private int[] GetSelectedColumns()
        {
            List<int> selected = new List<int>();

            for (int i = 2; i < this.bsprData.ActiveSheet.Columns.Count; i++)
            {
                if(this.bsprData.ActiveSheet.ColumnHeader.Cells[0, i].Text == "True")
                    selected.Add(i);
            }

            return selected.ToArray();
        }

        private void RefreshData()
        {
            if(this._lastestCondition != null)
                Compare(_lastestCondition);
        }

        private string GetHeader(int columnIndex)
        {
            CheckBoxCellType checkbox = this.bsprData.ActiveSheet.ColumnHeader.Cells[0, columnIndex].CellType as CheckBoxCellType; 

            if(checkbox == null)
                return string.Empty;

            return checkbox.Caption;
        }

        private string GetSPCModelName(int columnIndex)
        {
            return this.bsprData.ActiveSheet.Cells[this._controller.GetRowIndex("SPC MODEL NAME"), columnIndex].Text;
        }

        private string GetMainYN(int columnIndex)
        {
            return this.bsprData.ActiveSheet.Cells[this._controller.GetRowIndex("Main YN"), columnIndex].Text;
        }
        
        private bool compareSpecLimit(DataSet SourceData, DataSet TargetData, string comparetype, string ChartType)
        {
            string sourceUSL = "";
            string sourceLSL = "";
            string sourceUCL = "";
            string sourceLCL = "";
            string sourceRawid = "";
            string targetUSL = "";
            string targetLSL = "";
            string targetUCL = "";
            string targetLCL = "";
            string targetRawid = "";

            if (SourceData != null)
            {
                if (ChartType == "PN")
                {
                    foreach (DataRow dr in SourceData.Tables[0].Rows)
                    {
                        sourceUSL = dr[BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC].ToString();
                        sourceLSL = dr[BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC].ToString();
                        sourceUCL = dr[BISTel.eSPC.Common.COLUMN.PN_UCL].ToString();
                        sourceLCL = dr[BISTel.eSPC.Common.COLUMN.PN_LCL].ToString();
                        sourceRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    }

                    foreach (DataRow dr in TargetData.Tables[0].Rows)
                    {
                        targetUSL = dr[BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC].ToString();
                        targetLSL = dr[BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC].ToString();
                        targetUCL = dr[BISTel.eSPC.Common.COLUMN.PN_UCL].ToString();
                        targetLCL = dr[BISTel.eSPC.Common.COLUMN.PN_LCL].ToString();
                        sourceRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    }


                }
                else if (ChartType == "C")
                {
                    foreach (DataRow dr in SourceData.Tables[0].Rows)
                    {
                        sourceUSL = dr[BISTel.eSPC.Common.COLUMN.C_UPPERSPEC].ToString();
                        sourceLSL = dr[BISTel.eSPC.Common.COLUMN.C_LOWERSPEC].ToString();
                        sourceUCL = dr[BISTel.eSPC.Common.COLUMN.C_UCL].ToString();
                        sourceLCL = dr[BISTel.eSPC.Common.COLUMN.C_LCL].ToString();
                        sourceRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    }
                    foreach (DataRow dr in TargetData.Tables[0].Rows)
                    {
                        targetUSL = dr[BISTel.eSPC.Common.COLUMN.C_UPPERSPEC].ToString();
                        targetLSL = dr[BISTel.eSPC.Common.COLUMN.C_LOWERSPEC].ToString();
                        targetUCL = dr[BISTel.eSPC.Common.COLUMN.C_UCL].ToString();
                        targetLCL = dr[BISTel.eSPC.Common.COLUMN.C_LCL].ToString();
                        sourceRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    }
                }
            }
            if (comparetype == "11")
            {

                targetUSL = sourceUSL;
                targetLSL = sourceLSL;
                targetUCL = sourceUCL;
                targetLCL = sourceLCL;
                targetRawid = sourceRawid;

                return checkSpecLimit(targetUSL, targetLSL, targetUCL, targetLCL, targetRawid, ChartType);


            }
            else if (comparetype == "10")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = sourceUSL;
                    targetLSL = sourceLSL;
                    targetRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetUCL, targetLCL, targetRawid, ChartType);

                }
            }
            else if (comparetype == "01")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUCL = sourceUCL;
                    targetLCL = sourceLCL;
                    targetRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetUCL, targetLCL, targetRawid, ChartType);

                }
            }
            return true;
            
        }
        private bool checkSpecLimit(string targetUSL, string targetLSL, string targetUCL, string targetLCL, string targetRawid, string ChartType)
        {
            double dUpper = 0;
            double dLower = 0;

            #region usl & ucl(raw,mean)

            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetLSL))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_ID_TYPE", new string[]{ targetRawid, ChartType, "USL" , "LSL"}, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetUCL))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetUCL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_ID_TYPE", new string[]{ targetRawid, ChartType, "USL" , "UCL"}, null);
                    return false;
                }
            }

            #endregion

            #region lsl & lcl(raw,mean)

            if (!string.IsNullOrEmpty(targetLCL) && !string.IsNullOrEmpty(targetUCL))
            {
                dUpper = double.Parse(targetUCL);
                dLower = double.Parse(targetLCL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_ID_TYPE", new string[] { targetRawid, ChartType, "UCL", "LCL" }, null);
                    return false;
                }
            }

            #endregion

            #region lsl & ucl(raw,mean)
            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetLCL))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetLCL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_ID_TYPE", new string[] { targetRawid, ChartType, "USL", "LCL" }, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetUCL) && !string.IsNullOrEmpty(targetLSL))
            {
                dUpper = double.Parse(targetUCL);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_ID_TYPE", new string[] { targetRawid, ChartType, "UCL", "LSL" }, null);
                    return false;
                }
            }
            #endregion

            return true;
        }

    }

    public delegate void ModelModifiedEventHandler(object sender, EventArgs args);
}
