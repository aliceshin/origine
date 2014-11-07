using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Page.History;
using BISTel.eSPC.Page.Modeling;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;
using System.Collections;

namespace BISTel.eSPC.Page.Compare
{
    public enum CompareType
    {
        Model,
        Version
    }

    public partial class SPCModelCompareResultUC : BasePageUCtrl
    {
        #region : Field

        private string itemKey = string.Empty;
        private Initialization _initialization = null;
        private ICompareResultController _controller = null;
        private string _line = string.Empty;
        private string _lineName = string.Empty; //SPC-1307, KBLEE
        private string _areaRawid = string.Empty;
        private string _area = string.Empty;
        //private string _area = string.Empty;
        private string _eqpModel = string.Empty;
        private string _paramAlias = string.Empty;
        private string[] _chartIds = null;
        private string _paramType = string.Empty;
        private string port = string.Empty;
        public CompareType compareType = CompareType.Model;
        private LinkedList _lastestCondition = null;

        //SPC-1287, KBLEE, Context Menu 관련 전역 변수
        private bool _isFilter = false;
        private bool _isFreeze = true;
        private SpreadFilter _spreadFilter = null;
        private MenuItem _miFilter = null;
        private MenuItem _miFilterHide = null;
        private MenuItem _miFreeze = null;
        private MenuItem _miUnFreeze = null;
        //SPC-1287, KBLEE, End

        public ModelModifiedEventHandler modelModifiedEventHandler = null;

        public delegate void LinkTraceDataViewEventHandlerResult(object sender, EventArgs e, LinkedList llstTraceLinkData);
        public event LinkTraceDataViewEventHandlerResult linkTraceDataViewEventResult;

        private bool isMET = false;
        private string _groupName;

        #endregion


        #region : Property

        public bool ISMET
        {
            get { return this.isMET; }
            set { this.isMET = value; }
        }
        public string AREA_RAWID
        {
            get { return this._areaRawid; }
            set { this._areaRawid = value; }
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

        #endregion


        #region : Constructor

        public SPCModelCompareResultUC()
        {
            InitializeComponent();
        }

        #endregion


        #region : Override Method

        public override void PageInit()
        {
            InitializePage();
        }

        public override void PageSearch(LinkedList llCondition)
        {
            string sSite = string.Empty;
            string sFab = string.Empty;
            string sLine = string.Empty;
            string sArea = string.Empty;

            if (llCondition[Definition.DynamicCondition_Search_key.SITE] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows.Count > 0)
            {
                sSite =
                   ((DataTable)llCondition[Definition.DynamicCondition_Search_key.SITE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if (llCondition[Definition.DynamicCondition_Search_key.FAB] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows.Count > 0)
            {
                sFab =
                   ((DataTable)llCondition[Definition.DynamicCondition_Search_key.FAB]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if (llCondition[Definition.DynamicCondition_Search_key.LINE] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows.Count > 0)
            {
                _line =
                    ((DataTable)llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                sLine = ((DataTable)llCondition[Definition.DynamicCondition_Search_key.LINE]).Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
                _lineName = sLine; //SPC-1307, KBLEE
            }

            if (llCondition[Definition.DynamicCondition_Search_key.AREA] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows.Count > 0)
            {
                _areaRawid =
                    ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                sArea = ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
                _area = ((DataTable)llCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();
            }
            if (llCondition[Definition.DynamicCondition_Search_key.EQPMODEL] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.EQPMODEL]).Rows.Count > 0)
            {
                _eqpModel =
                    ((DataTable)llCondition[Definition.DynamicCondition_Search_key.EQPMODEL]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }

            if (llCondition[Definition.DynamicCondition_Search_key.PARAM_ALIAS] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.PARAM_ALIAS]).Rows.Count > 0)
            {
                _paramAlias =
                    ((DataTable)llCondition[Definition.DynamicCondition_Search_key.PARAM_ALIAS]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }
            if (llCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE] != null
                && ((DataTable)llCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE]).Rows.Count > 0)
            {
                _paramType =
                    ((DataTable)llCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE]).Rows[0][Definition.CONDITION_SEARCH_KEY_VALUEDATA].ToString();
            }

            this.ApplyAuthory(this.bbtnList, sSite, sFab, sLine, sArea);
        }

        #endregion


        #region : Initialize

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
        
        public void InitializePage()
        {
            InitializeVariables();

            //SPC-1287, KBLEE, Start
            InitializeSpreadFilter();
            //SPC-1287, KBLEE, End

            InitializeBspr();

            InitializeDataButton();

            InitializeController();

            //SPC-1287, KBLEE, Start
            InitializeContextMenu();
            //SPC-1287, KBLEE, End
        }

        private void InitializeVariables()
        {
            if (this._initialization == null)
            {
                this._initialization = new Initialization();
                this._initialization.InitializePath();
            }

            _line = string.Empty;
            _areaRawid = string.Empty;
            //_eqpModel = string.Empty;
            //_paramAlias = string.Empty;
            _chartIds = null;
            //_paramType = string.Empty;
        }

        ///SPC-1287, KBLEE
        /// <summary>
        /// SpreadFilter를 초기화 해줌
        /// </summary>
        private void InitializeSpreadFilter()
        {
            //SPC-1287, KBLEE
            if (_spreadFilter != null)
            {
                _spreadFilter.Visible = false;
                _spreadFilter.Dispose();
            }

            _spreadFilter = new SpreadFilter(bsprData);
            _isFilter = false;
        }

        private void InitializeBspr()
        {
            this.bsprData.SetFilterVisible(false);
            this.bsprData.ActiveSheet.Reset();
            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
            this.bsprData.UseSpreadEdit = false;
            this.bsprData.UseAutoSort = false;
            this.bsprData.UseEdit = true;
            this.bsprData.DataSet = null;

            //SPC-871 by Louis ContextMenu삭제(Common제공)
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

        /// SPC-1287, KBLEE
        /// <summary>
        /// BSpread에서 쓰일 ContextMenu 초기화 (Filter, Freeze만 초기화)
        /// </summary>
        private void InitializeContextMenu()
        {

            //Freeze는 항상 있으므로 Freeze로 Context 
            if ( !bsprData.ContextMenu.MenuItems.Contains(_miFreeze) )
            {
                _miFilter = new MenuItem("Filter");
                _miFilter.Click += new EventHandler(_miFilter_Click);

                _miFilterHide = new MenuItem("Filter_Hide");
                _miFilterHide.Click += new EventHandler(_miFilterHide_Click);

                _miFreeze = new MenuItem("Freeze");
                _miFreeze.Click += new EventHandler(_miFreeze_Click);

                _miUnFreeze = new MenuItem("UnFreeze");
                _miUnFreeze.Click += new EventHandler(_miUnFreeze_Click);

                //Context Menu에서 구분자 넣기
                bsprData.ContextMenu.MenuItems.Add("-");

                if (_isFilter)
                {
                    bsprData.ContextMenu.MenuItems.Add(_miFilterHide);
                }
                else
                {
                    bsprData.ContextMenu.MenuItems.Add(_miFilter);
                }

                bsprData.ContextMenu.MenuItems.Add(_miFreeze);
                bsprData.ContextMenu.MenuItems.Add(_miUnFreeze);
            }
        }

        #endregion

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

            //SPC-1287, KBLEE, Start
            this.InitializeSpreadFilter();
            this.InitializeContextMenu();
            //SPC-1287, KBLEE, End

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

            //SPC-1230, KBLEE, Chart ID or Version Column 고정
            this.bsprData.ActiveSheet.FrozenColumnCount = 2;

            //SPC-1287, KBLEE, Start
            _spreadFilter.SetColumnFreeze(2);
            //SPC-1287, KBLEE, End

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

        /// SPC-1287, KBLEE
        /// <summary>
        /// Context Menu에서 Filter, Freeze의 위치 잡아주기
        /// </summary>
        private void RelocateContextMenu()
        {
            Menu.MenuItemCollection menuItem = bsprData.ContextMenu.MenuItems;

            //일단 Filter, Freeze 관련 ContextMenu 전부 삭제
            if ( menuItem.Contains(_miFilter) )
            {
                menuItem.Remove(_miFilter);
            }
            if (menuItem.Contains(_miFilterHide))
            {
                menuItem.Remove(_miFilterHide);
            }
            if (menuItem.Contains(_miFreeze))
            {
                menuItem.Remove(_miFreeze);
            }
            if (menuItem.Contains(_miUnFreeze))
            {
                menuItem.Remove(_miUnFreeze);
            }

            //다음으로 알맞은 Context Menu 삽입
            if (_isFilter)
            {
                menuItem.Add(_miFilterHide);
            }
            else
            {
                menuItem.Add(_miFilter);
            }

            menuItem.Add(_miFreeze);

            if (_isFreeze)
            {
                menuItem.Add(_miUnFreeze);
            }
        }

        public void bbtnList_ButtonClick(string name)
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

            ((SPCModelCompareUC)con).copyPopup.AREA = this._area;
            ((SPCModelCompareUC) con).copyPopup.SetCopyModelInfo(this._eqpModel, spcModelName, mainYN, chartId, this._paramAlias);
            ((SPCModelCompareUC) con).copyPopup.ShowDialog();
        }

        private void Paste()
        {
            Control con = this.Parent;
            while(con.Parent != null)
            {
                if(con is SPCModelCompareUC)
                    break;
                con = con.Parent;
            }

            SPCCopySpec copyPopup = ((SPCModelCompareUC) con).copyPopup;

            if(copyPopup == null
                || copyPopup.CONFIG_RAWID.Trim().Length < 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_COPY_SOURCE", null, null);
                return;
            }

            int[] selectedColumns = GetSelectedColumns();
            if(selectedColumns.Length == 0)
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

            if (copyPopup != null)
            {
                copyPopup.InitializePopup();
            }

            //SPC-1218, KBLEE, START
            for (int i = 0; i < selectedColumns.Length; i++)
            {
                if (copyPopup.CONTEXT_CONTEXT_INFORMATION == "Y" && GetMainYN(selectedColumns[i]) == "N")
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_COPY_CONTEXT_FOR_ONLY_MAIN", null, null);
                    return;
                }
            }
            //SPC-1218, KBLEE, END

            //SPC-855 by Louis ==> SpecLimit Check (raw,mean,master)
            if (copyPopup.RULE_MASTER_SPEC_LIMIT.ToString().Equals("Y") ||
                copyPopup.RULE_MEAN.ToString().Equals("Y") || copyPopup.RULE_RAW.ToString().Equals("Y"))
            {
                LinkedList toraltargetRawidList = new LinkedList();
                string[] targetRawid = new string[selectedColumns.Length];
                for (int i = 0; i < selectedColumns.Length; i++)
                {
                    LinkedList targetRawidList = new LinkedList();
                    targetRawid[i] = GetHeader(selectedColumns[i]);
                }
                DataSet targetSpecData = this._controller.GetTargetConfigSpecData(targetRawid);
                DataSet sourceSpecData = this._controller.GetSourseConfigSpecData(copyPopup.CONFIG_RAWID.ToString());

                if (targetSpecData != null && sourceSpecData != null)
                {
                    bool CompareResult = true;
                    if (copyPopup.RULE_MASTER_SPEC_LIMIT.ToString().Equals("Y") && copyPopup.RULE_RAW.ToString().Equals("Y") && copyPopup.RULE_MEAN.ToString().Equals("N"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "111");
                    }
                    else if (copyPopup.RULE_MASTER_SPEC_LIMIT.ToString().Equals("Y") && copyPopup.RULE_RAW.ToString().Equals("Y") && copyPopup.RULE_MEAN.ToString().Equals("N"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "110");
                    }
                    else if (copyPopup.RULE_MASTER_SPEC_LIMIT.ToString().Equals("Y") && copyPopup.RULE_RAW.ToString().Equals("N") && copyPopup.RULE_MEAN.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "101");
                    }
                    else if (copyPopup.RULE_MASTER_SPEC_LIMIT.ToString().Equals("Y") && copyPopup.RULE_RAW.ToString().Equals("N") && copyPopup.RULE_MEAN.ToString().Equals("N"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "100");
                    }
                    else if (copyPopup.RULE_MASTER_SPEC_LIMIT.ToString().Equals("N") && copyPopup.RULE_RAW.ToString().Equals("Y") && copyPopup.RULE_MEAN.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "011");
                    }
                    else if (copyPopup.RULE_MASTER_SPEC_LIMIT.ToString().Equals("N") && copyPopup.RULE_RAW.ToString().Equals("Y") && copyPopup.RULE_MEAN.ToString().Equals("N"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "010");
                    }
                    else if (copyPopup.RULE_MASTER_SPEC_LIMIT.ToString().Equals("N") && copyPopup.RULE_RAW.ToString().Equals("N") && copyPopup.RULE_MEAN.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "001");
                    }
                    if (!CompareResult)
                    {
                        return;
                    }
                }

            }
                       
            //DialogResult result = MSGHandler.DialogQuestionResult("Are you sure to copy selected configuration?",
            //                                                      new string[] {"Model Copy"}, MessageBoxButtons.YesNo);

            if (copyPopup != null)
            {
                StringBuilder resultMessage = new StringBuilder();

                //SPC-632 
                LinkedList TotalConfig = new LinkedList();
                LinkedList llstTotalConfigInfo = new LinkedList();
                ArrayList arrTotalConfigList = new ArrayList();

                string chartId = null;
                string mainYN = null;
                
                for (int i = 0; i < selectedColumns.Length; i++)
                {
                    //string chartId = GetHeader(selectedColumns[i]);
                    //string mainYN = GetMainYN(selectedColumns[i]);
                    chartId = GetHeader(selectedColumns[i]);
                    mainYN = GetMainYN(selectedColumns[i]);

                    //SPC-977
                    llstTotalConfigInfo = this._controller.Paste(copyPopup, chartId, mainYN, this.sessionData.UserRawID);
                    arrTotalConfigList.Add(llstTotalConfigInfo);
                }

                SPCModelSavePopup popup = new SPCModelSavePopup(ModifyMode.COPY, "");
                popup.InitializeControl();
                popup.CHANGED_ITEMS = llstTotalConfigInfo[COLUMN.CHANGED_ITEMS].ToString();
                DialogResult result = popup.ShowDialog();

                if (result == DialogResult.Yes)
                {
                    this.MsgShow(COMMON_MSG.Query_Data);

                    for (int i = 0; i < selectedColumns.Length; i++)
                    {
                        ((LinkedList)arrTotalConfigList[i]).Add(COLUMN.SAVE_COMMENT, popup.COMMENT);
                        TotalConfig.Add(i, arrTotalConfigList[i]);
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
                        MSGHandler.DisplayMessage(MSGType.Information,
                                                      MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));
                    }
                }
            }

            #region preCode
            //SPCModelSavePopup popup = new SPCModelSavePopup(ModifyMode.COPY, "");
            //popup.InitializeControl();
            //DialogResult result = popup.ShowDialog();

            //if (result == DialogResult.Yes)
            //{
            //    StringBuilder resultMessage = new StringBuilder();

            //    //SPC-632 
            //    LinkedList TotalConfig = new LinkedList();
            //    string chartId = null;
            //    string mainYN = null;


            //    this.MsgShow(COMMON_MSG.Query_Data);
            //    for (int i = 0; i < selectedColumns.Length; i++)
            //    {
            //        //string chartId = GetHeader(selectedColumns[i]);
            //        //string mainYN = GetMainYN(selectedColumns[i]);
            //        chartId = GetHeader(selectedColumns[i]);
            //        mainYN = GetMainYN(selectedColumns[i]);

            //        //SPC-629
            //        TotalConfig.Add(i, this._controller.Paste(copyPopup, chartId, mainYN, this.sessionData.UserRawID));


            //        //  string message = this._controller.Paste(copyPopup, chartId, mainYN, this.sessionData.UserRawID);
            //        //switch(message)
            //        //{
            //        //    case "FAIL" :
            //        //        resultMessage.AppendLine(chartId + " : " + MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
            //        //        break;
            //        //    case "There is no item to paste" :
            //        //        resultMessage.AppendLine(chartId + " : " + "There is no item to paste");
            //        //        break;
            //        //}
            //    }
            //    //SPC-629
            //    string message = this._controller.PasteModel(TotalConfig);
            //    switch (message)
            //    {
            //        case "FAIL":
            //            resultMessage.AppendLine(chartId + " : " + MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
            //            break;
            //        case "There is no item to paste":
            //            resultMessage.AppendLine(chartId + " : " + "There is no item to paste");
            //            break;
            //    }

            //    this.MsgClose();
            //    if (resultMessage.Length != 0)
            //    {
            //        MSGHandler.DisplayMessage(MSGType.Information, resultMessage.ToString());
            //    }
            //    else
            //    {
            //        this.RefreshData();
            //    }
            //}
            #endregion
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

            this._controller.ISMET(this.isMET);
            LinkedList llGroupName = this._controller.GetGroupNameByChartID(chartId);
            if (llGroupName.Count > 0)
            {
                _groupName = llGroupName[COLUMN.GROUP_NAME].ToString();
            }
            else
            {
                _groupName = Definition.VARIABLE_UNASSIGNED_MODEL;
            }

            if (this._controller.Modify(this.sessionData, this.URL, port, _line, _areaRawid, _eqpModel, chartId, mainYN, _groupName) == DialogResult.OK)
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

            this._controller.ISMET(this.isMET);

            if (this._controller.ViewModel(this.sessionData, this.URL, port, _line, _areaRawid, _eqpModel, chartId, mainYN, version, _groupName) == DialogResult.OK)
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

            this.ViewChart(this.sessionData, this.URL, this._line, this._lineName, this._areaRawid, this._area, spcModelName, this._paramAlias, chartId, this._paramType);
        }

        public void ViewChart(SessionData sessionData, string URL, string lineRawid, string line, string areaRawid, string area, string spcModelName, string paramAlias, string chartId, string paramType)
        {
            Common.ChartViewPopup chartViewPop = new Common.ChartViewPopup();
            chartViewPop.URL = URL;
            chartViewPop.SessionData = sessionData;

            //Louis SPC-834 [SPC Compare] Chart
            chartViewPop.ChartVariable.CHART_PARENT_MODE = CHART_PARENT_MODE.MODELING; //SPC-834 Chart Mode 수정
            //SPC-1307, KBLEE, START
            chartViewPop.ChartVariable.LINE = line;
            chartViewPop.ChartVariable.AREA = area;
            chartViewPop.AREA_RAWID = areaRawid;
            chartViewPop.LINE_RAWID = lineRawid;
            //SPC-1307, KBLEE, END
            chartViewPop.ChartVariable.SPC_MODEL = spcModelName;
            chartViewPop.ChartVariable.PARAM_ALIAS = paramAlias;
            chartViewPop.ChartVariable.MODEL_CONFIG_RAWID = chartId;
            chartViewPop.ParamTypeCD = paramType;

            LinkedList llGroupName = this._controller.GetGroupNameByChartID(chartId);
            if (llGroupName.Count > 0)
            {
                _groupName = llGroupName[COLUMN.GROUP_NAME].ToString();
            }
            else
            {
                _groupName = Definition.VARIABLE_UNASSIGNED_MODEL;
            }

            chartViewPop.GROUP_NAME = _groupName;
            chartViewPop.InitializePopup();

            chartViewPop.linkTraceDataViewEventPopup -= new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
            chartViewPop.linkTraceDataViewEventPopup += new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);

            chartViewPop.ShowDialog();
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

            if (checkbox == null)
            {
                return string.Empty;
            }

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
        //SPC-855
        private bool compareSpecLimit(DataSet SourceData, DataSet TargetData, string comparetype)
        {
            string sourceUSL = "";
            string sourceLSL = "";
            string sourceRawUCL = "";
            string sourceRawLCL = "";
            string sourceUpperControl = "";
            string sourceLowerControl = "";
            string sourceRawid = "";
            string targetUSL = "";
            string targetLSL = "";
            string targetRawUCL = "";
            string targetRawLCL = "";
            string targetUpperControl = "";
            string targetLowerControl = "";
            string targetRawid = "";


            if (SourceData != null)
            {
                foreach (DataRow dr in SourceData.Tables[0].Rows)
                {
                    sourceUSL = dr[COLUMN.UPPER_SPEC].ToString();
                    sourceLSL = dr[COLUMN.LOWER_SPEC].ToString();
                    sourceRawUCL = dr[COLUMN.RAW_UCL].ToString();
                    sourceRawLCL = dr[COLUMN.RAW_LCL].ToString();
                    sourceUpperControl = dr[COLUMN.UPPER_CONTROL].ToString();
                    sourceLowerControl = dr[COLUMN.LOWER_CONTROL].ToString();
                    sourceRawid = dr[COLUMN.RAWID].ToString();
                }
            }
            if (comparetype == "111")
            {

                targetUSL = sourceUSL;
                targetLSL = sourceLSL;
                targetRawUCL = sourceRawUCL;
                targetRawLCL = sourceRawLCL;
                targetUpperControl = sourceUpperControl;
                targetLowerControl = sourceLowerControl;
                targetRawid = sourceRawid;

                return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);


            }
            else if (comparetype == "110")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = sourceUSL;
                    targetLSL = sourceLSL;
                    targetRawUCL = sourceRawUCL;
                    targetRawLCL = sourceRawLCL;
                    targetUpperControl = dr[COLUMN.UPPER_CONTROL].ToString();
                    targetLowerControl = dr[COLUMN.LOWER_CONTROL].ToString();
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);

                }
            }
            else if (comparetype == "101")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = sourceUSL;
                    targetLSL = sourceLSL;
                    targetRawUCL = dr[COLUMN.RAW_UCL].ToString();
                    targetRawLCL = dr[COLUMN.RAW_LCL].ToString();
                    targetUpperControl = sourceUpperControl;
                    targetLowerControl = sourceLowerControl;
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);

                }
            }
            else if (comparetype == "100")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = sourceUSL;
                    targetLSL = sourceLSL;
                    targetRawUCL = dr[COLUMN.RAW_UCL].ToString();
                    targetRawLCL = dr[COLUMN.RAW_LCL].ToString();
                    targetUpperControl = dr[COLUMN.UPPER_CONTROL].ToString();
                    targetLowerControl = dr[COLUMN.LOWER_CONTROL].ToString();
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);

                }
            }
            else if (comparetype == "011")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = dr[COLUMN.UPPER_SPEC].ToString();
                    targetLSL = dr[COLUMN.LOWER_SPEC].ToString();
                    targetRawUCL = sourceRawUCL;
                    targetRawLCL = sourceRawLCL;
                    targetUpperControl = sourceUpperControl;
                    targetLowerControl = sourceLowerControl;
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);

                }
            }
            else if (comparetype == "010")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = dr[COLUMN.UPPER_SPEC].ToString();
                    targetLSL = dr[COLUMN.LOWER_SPEC].ToString();
                    targetRawUCL = sourceRawUCL;
                    targetRawLCL = sourceRawLCL;
                    targetUpperControl = dr[COLUMN.UPPER_CONTROL].ToString();
                    targetLowerControl = dr[COLUMN.LOWER_CONTROL].ToString();
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);

                }
            }
            else if (comparetype == "001")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = dr[COLUMN.UPPER_SPEC].ToString();
                    targetLSL = dr[COLUMN.LOWER_SPEC].ToString();
                    targetRawUCL = dr[COLUMN.RAW_UCL].ToString();
                    targetRawLCL = dr[COLUMN.RAW_LCL].ToString();
                    targetUpperControl = sourceUpperControl;
                    targetLowerControl = sourceLowerControl;
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);
                }

            }
            return true;
        }
        private bool checkSpecLimit(string targetUSL, string targetLSL, string targetRawUCL, string targetRawLCL, string targetUpperControl, string targetLowerControl, string targetRawid)
        {
            double dUpper = 0;
            double dLower = 0;

            #region usl & ucl(raw,mean)

            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetUpperControl))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetUpperControl);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "USL", "MEAN UCL" }, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetRawUCL))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetRawUCL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "USL", "RAW UCL" }, null);
                    return false;
                }
            }

            #endregion

            #region lsl & lcl(raw,mean)

            if (!string.IsNullOrEmpty(targetLSL) && !string.IsNullOrEmpty(targetLowerControl))
            {
                dUpper = double.Parse(targetLowerControl);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "MEAN LCL", "LSL" }, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetLSL) && !string.IsNullOrEmpty(targetRawLCL))
            {
                dUpper = double.Parse(targetRawLCL);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "RAW LCL", "LSL" }, null);
                    return false;
                }
            }
            #endregion

            #region lsl & ucl(raw,mean)
            if (!string.IsNullOrEmpty(targetLSL) && !string.IsNullOrEmpty(targetUpperControl))
            {
                dUpper = double.Parse(targetUpperControl);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "MEAN UCL", "LSL" }, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetLSL) && !string.IsNullOrEmpty(targetRawUCL))
            {
                dUpper = double.Parse(targetRawUCL);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "RAW UCL", "LSL" }, null);
                    return false;
                }
            }
            #endregion

            #region usl & lcl(raw,mean)
            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetLowerControl))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetLowerControl);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "USL", "MEAN LCL" }, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetRawLCL))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetRawLCL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "USL", "RAW LCL" }, null);
                    return false;
                }
            }
            #endregion

            return true;
        }

        //SPC-855 end


        #region : Event

        public void bsprData_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true && e.Column > 1)
            {
                if (this.bsprData.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].Text == "True")
                {
                    this.bsprData.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].Text = "False";
                }
                else
                {
                    this.bsprData.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].Text = "True";
                }
            }
        }

        void chartViewPop_linkTraceDataViewEventPopup(object sender, EventArgs e, LinkedList llstTraceLinkData)
        {
            if (this.linkTraceDataViewEventResult != null)
                linkTraceDataViewEventResult(this, null, llstTraceLinkData);
        }

        /// SPC-1287, KBLEE
        /// <summary>
        /// BSpread의 Context Menu 중 Filter를 Click할 시 일어나는 Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _miFilter_Click(object sender, EventArgs e)
        {
            _spreadFilter.Visible = true;
            _isFilter = true;

            RelocateContextMenu();
        }

        /// SPC-1287, KBLEE
        /// <summary>
        /// BSpread의 Context Menu 중 Filter_Hide를 Click할 시 일어나는 Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _miFilterHide_Click(object sender, EventArgs e)
        {
            _spreadFilter.Visible = false;
            _isFilter = false;

            RelocateContextMenu();
        }

        /// SPC-1287, KBLEE
        /// <summary>
        /// BSpread의 Context Menu 중 Freeze를 Click할 시 일어나는 Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _miFreeze_Click(object sender, EventArgs e)
        {
            int activeColumnIndex = bsprData.ActiveSheet.ActiveColumnIndex;

            //앞의 두 Column은 각 값을 설명하는 Column이므로 한 번에 Freeze 되게 함
            if (activeColumnIndex == 0 || activeColumnIndex == 1)
            {
                activeColumnIndex = 2;
            }

            bsprData.ActiveSheet.FrozenColumnCount = activeColumnIndex;
            _spreadFilter.SetColumnFreeze(activeColumnIndex);

            _isFreeze = true;

            RelocateContextMenu();
        }

        /// SPC-1287, KBLEE
        /// <summary>
        /// BSpread의 Context Menu 중 unFreeze를 Click할 시 일어나는 Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _miUnFreeze_Click(object sender, EventArgs e)
        {
            bsprData.ActiveSheet.FrozenColumnCount = 0;
            _spreadFilter.SetColumnFreeze(0);

            _isFreeze = false;

            RelocateContextMenu();
        }

        /// SPC-1287, KBLEE
        /// <summary>
        /// BSpread에서 Ctrl+F를 눌러도 BSpread 자체의 Filter가 뜨지 않게 하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsprData_KeyPress(object sender, KeyPressEventArgs e)
        {
            bsprData.SetFilterVisible(false);
        }

        /// SPC-1287, KBLEE
        /// <summary>
        /// Ctrl+F를 누르면 새로 적용시킨 Filter(BSpread의 자체 Filter 아님)가 나오게 하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsprData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.F)
                {
                    if (_isFilter)
                    {
                        _spreadFilter.Visible = false;
                        _isFilter = false;
                    }
                    else
                    {
                        _spreadFilter.Visible = true;
                        _isFilter = true;
                    }

                    RelocateContextMenu();
                }
            }
        }

        #endregion

    }

  


    public delegate void ModelModifiedEventHandler(object sender, EventArgs args);
}
