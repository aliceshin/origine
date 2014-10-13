using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Statistics.Application.Common;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;

using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.Analysis
{


    public partial class SPCAnalysisUC : BasePageUCtrl
    {

        eSPCWebService.eSPCWebService _wsSPC = null;
        Initialization _Initialization;
        MultiLanguageHandler _lang;
        LinkedList _llstData = new LinkedList();
        ArrayList _arrSubData = new ArrayList();
        LinkedList _llstChartList = new LinkedList();
        SourceDataManager _dataManager = null;
        ChartUtility _chartUtil;

        BISTel.eSPC.Common.BSpreadUtility _bSpreadUtil = new BSpreadUtility();
        BISTel.eSPC.Common.CommonUtility _ComUtil = null;

        string _Line = string.Empty;
        string _LineRawID = string.Empty;
        string _AreaRawID = string.Empty;
        string _EQPModel = string.Empty;
        string _ParamItem = string.Empty;
        string _OperationID = string.Empty;
        string _ParamType = string.Empty;
        string _AnalysisType = string.Empty;
        string _Site = string.Empty;
        string _Fab = string.Empty;
        string _Target = string.Empty;
        string _SortingKey = string.Empty;
        string _sStartTime = string.Empty;
        string _sEndTime = string.Empty;
        string _sTargetType = string.Empty;
        bool _bTargetMet = true;

        string SPC_PARAM_TYPE_MET = "MET";
        string SPC_PARAM_TYPE_EVS = "EVS";
        string _complex_yn = "N";
        int SPC_V_SELECT = 0;

        List<string> _lstRawColumn;
        DateTime _DateTimeStart = DateTime.Now.AddDays(-30);
        DateTime _DateTimeEnd = DateTime.Now;
        AnalysisWindowType _AnalysisWindowType = AnalysisWindowType.Analysis;

        public SPCAnalysisUC()
        {
            InitializeComponent();
            this.InitializePage();
        }
        public SPCAnalysisUC(AnalysisWindowType _AnalysisWinType)
        {
            InitializeComponent();
            _AnalysisWindowType = _AnalysisWinType;
            this.InitializePage();
        }

        #region Properties

        public AnalysisWindowType AnalysisWinType
        {
            set { this._AnalysisWindowType = value; }
            get { return this._AnalysisWindowType; }
        }

        public DateTime DateTimeStart
        {
            set { this._DateTimeStart = value; }
            get { return this._DateTimeStart; }
        }

        public DateTime DateTimeEnd
        {
            set { this._DateTimeEnd = value; }
            get { return this._DateTimeEnd; }
        }

        public string LineRawID
        {
            set { this._LineRawID = value; }
            get { return this._LineRawID; }
        }

        public string AreaRawID
        {
            set { this._AreaRawID = value; }
            get { return this._AreaRawID; }
        }

        public string EQPModel
        {
            set { this._EQPModel = value; }
            get { return this._EQPModel; }
        }


        public string ParamItem
        {
            set { this._ParamItem = value; }
            get { return this._ParamItem; }
        }

        public string OperationID
        {
            set { this._OperationID = value; }
            get { return this._OperationID; }
        }

        public string AnalysisType
        {
            set { this._AnalysisType = value; }
            get { return this._AnalysisType; }
        }

        public string SITE
        {
            set { this._Site = value; }
            get { return this._Site; }
        }

        public string FAB
        {
            set { this._Fab = value; }
            get { return this._Fab; }
        }

        #endregion

        #region ::: Override Method

        private DataTable MakeDataTableForDCValue(string argValue)
        {         
            string [] arr = argValue.Split(';');
            string _Display = string.Empty;
            string _Value = string.Empty;
            for(int i=0; i<arr.Length; i++)
            {
                if(i==0)
                {
                    _Value = arr[i];
                    _Display = arr[i];
                }else
                {
                    _Display= arr[i];
                }
            }                    
            return DCUtil.MakeDataTableForDCValue(_Value,_Display);
        }
        public override void PageInit()
        {

            BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;

            this.KeyOfPage = this.GetType().FullName; // 필수 입력 (Key값을 받은 후 DC을 Active해주기 때문이다. 

            if (Request.GetValue(Definition.DynamicCondition_Search_key.SITE) != null)
            {
                LinkedList lk = new LinkedList();
                lk.Add(Definition.DynamicCondition_Search_key.SITE,  this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.SITE)[0]));
                lk.Add(Definition.DynamicCondition_Search_key.FAB, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.FAB)[0]));
                lk.Add(Definition.DynamicCondition_Search_key.LINE, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.LINE)[0]));
                lk.Add(Definition.DynamicCondition_Search_key.AREA, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.AREA)[0]));
                lk.Add(Definition.DynamicCondition_Search_key.PARAM_TYPE, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.PARAM_TYPE)[0]));

                if (Request.GetValue(Definition.DynamicCondition_Search_key.EQPMODEL) != null)
                lk.Add(Definition.DynamicCondition_Search_key.EQPMODEL, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.EQPMODEL)[0]));

                lk.Add(Definition.DynamicCondition_Search_key.OPERATION, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.OPERATION)[0]));
                lk.Add(Definition.DynamicCondition_Search_key.PARAM, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.PARAM)[0]));
                lk.Add(Definition.DynamicCondition_Search_key.SUBDATA, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.SUBDATA)[0]));
                lk.Add(Definition.DynamicCondition_Search_key.SORTING_KEY, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.SORTING_KEY)[0]));

                lk.Add(Definition.DynamicCondition_Search_key.TARGET_TYPE, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.TARGET_TYPE)[0]));
                lk.Add(Definition.DynamicCondition_Search_key.TARGET, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.TARGET)[0]));
                lk.Add(Definition.DynamicCondition_Search_key.CHART_LIST, this.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.CHART_LIST)[0]));

                lk.Add(Definition.DynamicCondition_Search_key.DATETIME_PERIOD, DCUtil.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.DATETIME_PERIOD)[0], Request.GetValue(Definition.DynamicCondition_Search_key.DATETIME_PERIOD)[0]));
                lk.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, DCUtil.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.DATETIME_FROM)[0], Request.GetValue(Definition.DynamicCondition_Search_key.DATETIME_FROM)[0]));
                lk.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, DCUtil.MakeDataTableForDCValue(Request.GetValue(Definition.DynamicCondition_Search_key.DATETIME_TO)[0], Request.GetValue(Definition.DynamicCondition_Search_key.DATETIME_TO)[0]));
                                                                                                                                                      
                this.DynaminCondition.RefreshCondition(lk);
                this.PageSearch(lk);                   
            }
        }


        public override void PageSearch(LinkedList llstCondition)
        {
            this._llstData.Clear();
            DataTable dt = null;

            if (llstCondition[Definition.DynamicCondition_Search_key.LINE] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.LINE];
                _Line = DataUtil.GetDisplayData(dt);
                _LineRawID = DCUtil.GetValueData(dt);
            }


            if (llstCondition[Definition.DynamicCondition_Search_key.AREA] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.AREA];
                _AreaRawID = DataUtil.GetConditionKeyDataList(dt, Definition.DynamicCondition_Condition_key.VALUEDATA);
            }


            if (llstCondition[Definition.DynamicCondition_Search_key.EQPMODEL] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.EQPMODEL];
                _EQPModel = DataUtil.GetConditionKeyDataList(dt, Definition.DynamicCondition_Condition_key.VALUEDATA, true);
            }




            if (llstCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM];
                this._sStartTime = CommonPageUtil.StartDate(dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
            }
            if (llstCondition[Definition.DynamicCondition_Search_key.DATETIME_TO] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.DATETIME_TO];
                this._sEndTime = CommonPageUtil.EndDate(dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
            }



            if (llstCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE] != null)
                this._ParamType = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE]);


            if (llstCondition[Definition.DynamicCondition_Search_key.PARAM] != null)
                this._ParamItem = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PARAM]);

            if (llstCondition[Definition.DynamicCondition_Search_key.OPERATION] != null)
                this._OperationID = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.OPERATION]);


            this._arrSubData.Clear();
            if (llstCondition[Definition.DynamicCondition_Search_key.SUBDATA] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.SUBDATA];
                foreach (DataRow dr in dt.Rows)
                {
                    _arrSubData.Add(dr[Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
                }
            }


            this._llstChartList.Clear();
            if (llstCondition[Definition.DynamicCondition_Search_key.CHART_LIST] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.CHART_LIST];
                foreach (DataRow dr in dt.Rows)
                {
                    string sCode = dr[Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();
                    string sName = dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
                    _llstChartList.Add(sCode, sName);
                }
            }


            if (llstCondition[Definition.DynamicCondition_Search_key.TARGET_TYPE] != null)
            {
                _sTargetType = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.TARGET_TYPE]);

                if (_sTargetType == SPC_PARAM_TYPE_MET)
                    _bTargetMet = true;
                else
                    _bTargetMet = false;
            }


            if (llstCondition[Definition.DynamicCondition_Search_key.TARGET] != null)
                this._Target = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.TARGET]);

            if (llstCondition[Definition.DynamicCondition_Search_key.SORTING_KEY] != null)
                this._SortingKey = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.SORTING_KEY]);



            if (this._arrSubData.Count == 0)
            {
                this.MsgClose();
                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_SELECT_OBJECT"), "Sub Data"));
                return;
            }

            if (this._llstChartList.Count == 0)
            {
                this.MsgClose();
                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_SELECT_OBJECT"), "Chart List"));
                return;
            }

            if (string.IsNullOrEmpty(this._Target))
            {
                this.MsgClose();
                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_SELECT_OBJECT"), "Sorting Key"));
                return;
            }

            if (string.IsNullOrEmpty(this._SortingKey))
            {
                this.MsgClose();
                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_SELECT_OBJECT"), "Target Step"));
                return;
            }
            PROC_DataBinding();
        }

        #endregion


        #region Initialize
        public void InitializePage()
        {
            this._dataManager = new SourceDataManager();
            this._ComUtil = new CommonUtility();
            this._bSpreadUtil = new BSpreadUtility();
            this._lang = MultiLanguageHandler.getInstance();
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
            this._chartUtil = new ChartUtility();

            this.InitializeLayout();
            if (AnalysisWindowType.SPCChart != _AnalysisWindowType)
            {
                this.InitializeDefaultSetting();
            }
        }

        public void InitializeLayout()
        {
            this.bTitlePanel1.Title = this._lang.GetVariable(Definition.TITLE_KEY_ANALYSIS);
            InitializeSpread();
            InitializeDataButton();
        }


        public void InitializeDataButton()
        {
            this._Initialization.InitializeButtonList(this.bButtonList1, Definition.PAGE_KEY_SPC_MULTIDATA_UC, Definition.BUTTONLIST_KEY_MULTIDATA, this.sessionData);
        }


        private void InitializeSpread()
        {

        }

        public void InitializeDefaultSetting()
        {


        }
        #endregion


        #region User Define


        private BaseAnalysisChart FindSPCChart(Control ctl)
        {
            Control parentControl = ctl.Parent;

            while (parentControl != null)
            {

                if (parentControl is BaseAnalysisChart)
                {
                    return parentControl as BaseAnalysisChart;
                }
                else
                {
                    parentControl = parentControl.Parent;
                }
            }

            return null;
        }

        private DataTable CreateColumn(ArrayList arrChartColumn, ArrayList arrSeries)
        {
            DataTable _RawDataTable = new DataTable();
            _RawDataTable.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, typeof(string));
            for (int i = 0; i < arrChartColumn.Count; i++)
            {
                string strKey = arrChartColumn[i].ToString();
                if (!_RawDataTable.Columns.Contains(strKey))
                {
                    if (strKey == Definition.CHART_COLUMN.TIME)
                    {
                        _RawDataTable.Columns.Add(strKey, typeof(DateTime));
                        _RawDataTable.Columns.Add(Definition.CHART_COLUMN.TIME2, typeof(string));
                    }
                    else
                        _RawDataTable.Columns.Add(strKey, typeof(string));
                }
            }

            if (arrSeries == null) return _RawDataTable;
            for (int j = 0; j < arrSeries.Count; j++)
            {
                string strKey = arrSeries[j].ToString();
                if (!_RawDataTable.Columns.Contains(strKey))
                    _RawDataTable.Columns.Add(strKey, typeof(double));
            }
            return _RawDataTable;
        }

        private DataTable CreateChartDataTable(DataTable _dtChartData, ArrayList arrChartColumn)
        {
            DataTable _RawDataTable = CreateColumn(arrChartColumn, this._arrSubData);
            DataRow drNew = null;
      
            //Target
            this._llstData.Clear();
            this._llstData.Add(Definition.DynamicCondition_Condition_key.TARGET_TYPE, this._sTargetType);
            this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this._Target);
            this._llstData.Add(Definition.DynamicCondition_Condition_key.START_DTTS, this._sStartTime);            
            DataSet _dsTarget = this._wsSPC.GetAnalysisTargetMachine(_llstData.GetSerialData());
            
            foreach (DataRow dr in _dtChartData.Rows)
            {
                drNew = _RawDataTable.NewRow();
                string _sLotID = dr[Definition.DynamicCondition_Condition_key.LOT_ID].ToString();
                string _sProductID = dr[Definition.DynamicCondition_Condition_key.PRODUCT_ID].ToString();
                              
                for (int j = 0; j < arrChartColumn.Count; j++)
                {
                    string strKey = arrChartColumn[j].ToString();
                    if (strKey == Definition.CHART_COLUMN.TIME)
                    {
                        string sTime = DateTime.Parse(dr[Definition.CHART_COLUMN.TIME].ToString()).ToString(Definition.DATETIME_FORMAT_MS).ToString();
                        drNew[Definition.CHART_COLUMN.TIME2] = sTime;
                        drNew[strKey] = sTime;
                    }
                    else if (_SortingKey == strKey)
                    {
                        drNew[strKey] = "NONE";              
                        if (!DataUtil.IsNullOrEmptyDataSet(_dsTarget))
                        {
                            DataRow[] drSelect = _dsTarget.Tables[0].Select(string.Format(" LOT_ID Like '%{0}%' AND PRODUCT_ID='{1}'", _sLotID, _sProductID));  
                            if(drSelect.Length>0)
                                drNew[strKey] = drSelect[0][this._SortingKey].ToString();                           
                        }                 
                    }
                    else
                        drNew[strKey] = dr[strKey];
                }

                for (int j = 0; j < this._arrSubData.Count; j++)
                {
                    string strKey = _arrSubData[j].ToString();
                    if (_dtChartData.Columns.Contains(strKey))
                        drNew[strKey] = dr[strKey];
                }
                _RawDataTable.Rows.Add(drNew);
            }

            _RawDataTable = DataUtil.DataTableImportRow(_RawDataTable.Select(null, Definition.CHART_COLUMN.TIME));

            for (int i = 0; i < _RawDataTable.Rows.Count; i++)
                _RawDataTable.Rows[i][CommonChart.COLUMN_NAME_SEQ_INDEX] = i.ToString();            
            _RawDataTable.AcceptChanges();
            return _RawDataTable;
        }





        private DataTable CreateChartDataTableBox(DataTable _dtOriginal, ArrayList arrChartColumn, LinkedList llstSortingKey, ArrayList arrSeries)
        {
            DataTableGroupBy dtGroupBy = new DataTableGroupBy();
            string sFieldList = string.Empty;
            string sGroupBy = string.Empty;
            string sFilter = string.Empty;

            DataTable dt = new DataTable();
            dt.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, typeof(int));
            dt.Columns.Add(this._SortingKey, typeof(string));
            dt.Columns.Add(Definition.CHART_COLUMN.VALUE, typeof(double));

            DataRow dr = null;
            int iCount = 0;
            for (int i = 0; i < llstSortingKey.Count; i++)
            {
                string _sSortingValue = llstSortingKey.GetKey(i).ToString();
                string _sSortingKey = llstSortingKey.GetValue(i).ToString();
                DataRow[] drSelect = _dtOriginal.Select(string.Format(" {0} = '{1}'", _sSortingKey, _sSortingValue));
                for (int j = 0; j < drSelect.Length; j++)
                {
                    for (int k = 0; k < arrSeries.Count; k++)
                    {
                        if (string.IsNullOrEmpty(drSelect[j][arrSeries[k].ToString()].ToString())) continue;
                        dr = dt.NewRow();
                        dr[CommonChart.COLUMN_NAME_SEQ_INDEX] = iCount;
                        dr[_sSortingKey] = drSelect[j][_sSortingKey].ToString();
                        dr[Definition.CHART_COLUMN.VALUE] = drSelect[j][arrSeries[k].ToString()];
                        dt.Rows.Add(dr);
                        iCount++;
                    }
                }
            }

            return dt;
        }

        private DataTable CreateChartDataTableEtc(DataTable _dtOriginal, ArrayList arrChartColumn, LinkedList llstSortingKey, ArrayList arrSeries)
        {
            DataTableGroupBy dtGroupBy = new DataTableGroupBy();
            string sFieldList = string.Empty;
            string sGroupBy = string.Empty;
            string sFilter = string.Empty;

            DataTable dt = new DataTable();
            dt.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, typeof(int));
            dt.Columns.Add(this._SortingKey, typeof(string));
            dt.Columns.Add(Definition.CHART_COLUMN.VALUE, typeof(double));
            dt.Columns.Add(Definition.CHART_COLUMN.SAMPLE_COUNT, typeof(int));

            DataRow dr = null;
            for (int i = 0; i < llstSortingKey.Count; i++)
            {
                string _sSortingValue = llstSortingKey.GetKey(i).ToString();
                string _sSortingKey = llstSortingKey.GetValue(i).ToString();
                DataRow[] drSelect = _dtOriginal.Select(string.Format(" {0} = '{1}'", _sSortingKey, _sSortingValue));
                for (int j = 0; j < drSelect.Length; j++)
                {
                    for (int k = 0; k < arrSeries.Count; k++)
                    {
                        if (string.IsNullOrEmpty(drSelect[j][arrSeries[k].ToString()].ToString())) continue;
                        dr = dt.NewRow();
                        dr[_sSortingKey] = drSelect[j][_sSortingKey].ToString();
                        dr[Definition.CHART_COLUMN.VALUE] = drSelect[j][arrSeries[k].ToString()];
                        dt.Rows.Add(dr);
                    }
                }
            }

            DataTable dtResult = dt.Clone();
            int iCount = 0;
            for (int i = 0; i < llstSortingKey.Count; i++)
            {
                string _sSortingValue = llstSortingKey.GetKey(i).ToString();
                string _sSortingKey = llstSortingKey.GetValue(i).ToString();


                sFieldList = this._SortingKey + string.Format(",{0}, count({0}) {1}", Definition.CHART_COLUMN.VALUE, Definition.CHART_COLUMN.SAMPLE_COUNT);
                sGroupBy = this._SortingKey + string.Format(",{0}", Definition.CHART_COLUMN.VALUE);
                sFilter = string.Format("{0} = '{1}' AND {2} is not null ", _sSortingKey, _sSortingValue, Definition.CHART_COLUMN.VALUE);
                DataTable dtSelect = dtGroupBy.SelectGroupByInto("Analysis", dt, sFieldList, sFilter, sGroupBy);
                foreach (DataRow nRow in dtSelect.Rows)
                {
                    dr = dtResult.NewRow();
                    for (int j = 0; j < dtResult.Columns.Count; j++)
                    {
                        string sCol = dtResult.Columns[j].ColumnName.ToString();
                        if (!dtSelect.Columns.Contains(sCol)) continue;
                        dr[sCol] = nRow[sCol];
                    }
                    dtResult.Rows.Add(dr);
                }
            }

            dtResult = DataUtil.DataTableImportRow(dtResult.Select(null, Definition.CHART_COLUMN.VALUE));
            iCount = 0;
            foreach (DataRow nRow in dtResult.Rows)
            {
                nRow[CommonChart.COLUMN_NAME_SEQ_INDEX] = iCount;
                iCount++;
            }

            return dtResult;
        }



        private string GetModelConfigRawID()
        {
            string sModelConfigRawID = string.Empty;

            this._llstData.Clear();
            this._llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawID);

            if (!string.IsNullOrEmpty(this.AreaRawID))
                this._llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.AreaRawID);

            if (!string.IsNullOrEmpty(this.EQPModel))
                this._llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.EQPModel);

            this._llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this._ParamType);
            this._llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, CommonPageUtil.GetConCatString(this._ParamItem));
            this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, CommonPageUtil.GetConCatString(this._OperationID));
            //this._llstData.Add(Definition.DynamicCondition_Condition_key.MAIN_YN, "Y");     

            DataSet ds = _wsSPC.GetSPCModelConfigSearch(_llstData.GetSerialData());

            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    sModelConfigRawID += ds.Tables[0].Rows[i][COLUMN.MODEL_CONFIG_RAWID].ToString() + ",";

                if (ds != null) ds.Dispose();
                if (!string.IsNullOrEmpty(sModelConfigRawID))
                    sModelConfigRawID = sModelConfigRawID.Substring(0, sModelConfigRawID.Length - 1);
            }

            return sModelConfigRawID;
        }

        private string GetColumString(BSpread _spread, ArrayList _alCheckRowIndex, int iCol)
        {
            string sList = string.Empty;
            for (int i = 0; i < _alCheckRowIndex.Count; i++)
            {
                string sCode = _spread.ActiveSheet.Cells[(int)_alCheckRowIndex[i], iCol].Text;
                sList += sCode + ",";
            }
            if (sList.Length > 0)
                sList = sList.Substring(0, sList.Length - 1);

            return sList;
        }

        #endregion

        #region Event

        private void PROC_DataBinding()
        {
            DataSet ds = null;
            DataTableGroupBy dtGroupBy = null;
            DataTable _dtChartData = null;
            DataTable _RawDataTable = null;
            DataTable _RawDataTableEtc = null;
            string sFieldList = string.Empty;
            string sGroupBy = string.Empty;
            string sFilter = string.Empty;
            try
            {
                CreateChartDataTable chartDataTable = new CreateChartDataTable();
                ArrayList arrChartColumn = new ArrayList();
                ArrayList arrSeries = new ArrayList();
                LinkedList llstSortingKey = new LinkedList();
                dtGroupBy = new DataTableGroupBy();

                this.MsgShow(_lang.GetMessage(Definition.LOADING_DATA));
                string sModelConfigRawID = GetModelConfigRawID();
                if (string.IsNullOrEmpty(sModelConfigRawID))
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                this._llstData.Clear();
                this._llstData.Add(Definition.DynamicCondition_Condition_key.START_DTTS, this._sStartTime);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.END_DTTS, this._sEndTime);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, sModelConfigRawID);
                //this._llstData.Add(Definition.CONDITION_KEY_CONTEXT_KEY_LIST, string.Format(" {0} = '{1}' ", sTargetOperationColName, this._OperationID));
                ds = _wsSPC.GetSPCControlChartData(_llstData.GetSerialData());
                if (DataUtil.IsNullOrEmptyDataSet(ds))
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }
                _dtChartData = CommonPageUtil.CLOBnBLOBParsing(ds, _llstData, false);
                if (ds != null) ds.Dispose();
                if (DataUtil.IsNullOrEmptyDataTable(_dtChartData))
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

               
                //**Start Chart Drawing
                arrChartColumn.Clear();
                arrChartColumn.Add(Definition.CHART_COLUMN.TIME);
                arrChartColumn.Add(Definition.CHART_COLUMN.PRODUCT_ID);
                arrChartColumn.Add(Definition.CHART_COLUMN.LOT_ID);
                arrChartColumn.Add(Definition.CHART_COLUMN.SUBSTRATE_ID);
                arrChartColumn.Add(this._SortingKey);

                this._dataManager.RawDataTable = _dtChartData;
                string sCol = CommonPageUtil.GetConditionKeyArrayList(arrChartColumn) + ",PRODUCT_ID,COMPLEX_YN,PARAM_LIST," + CommonPageUtil.GetConditionKeyArrayList(this._arrSubData);
                sFieldList = sCol;
                sGroupBy = sCol;
                sFilter = string.Empty;
                _dtChartData = dtGroupBy.SelectGroupByInto("ChartData", _dtChartData, sFieldList, sFilter, sGroupBy);
                chartDataTable.COMPLEX_YN = _dtChartData.Rows[0][COLUMN.COMPLEX_YN].ToString();
                _dtChartData = chartDataTable.GetMakeDataTable(_dtChartData);
                _complex_yn = chartDataTable.COMPLEX_YN;
                _lstRawColumn = chartDataTable.lstRawColumn;
                if (chartDataTable != null) chartDataTable = null;

                _RawDataTable = CreateChartDataTable(_dtChartData,arrChartColumn);
                //* SortingKey별 그룹지정                               
                sFieldList = this._SortingKey;
                sGroupBy = this._SortingKey;
                sFilter = string.Empty;
                DataTable dt = dtGroupBy.SelectGroupByInto(this._SortingKey, _RawDataTable, sFieldList, sFilter, sGroupBy);
                foreach (DataRow dr in dt.Rows)
                {
                    string sValue = dr[this._SortingKey].ToString();
                    if (!llstSortingKey.Contains(sValue))
                        llstSortingKey.Add(sValue, this._SortingKey);
                }

                BaseAnalysisChart analysisChart = null;
                Steema.TeeChart.ChartListBox chartListBox = null;
                Steema.TeeChart.Styles.LinePoint s = null;
                this.bTabControl1.TabPages.Clear();
                DataTable _RawDataTableEtc2 = null;
                for (int i = 0; i < this._llstChartList.Count; i++)
                {
                    string sCode = this._llstChartList.GetKey(i).ToString();
                    string sName = this._llstChartList.GetValue(i).ToString();
                    this.bTabControl1.TabPages.Add(sName);
                    this.bTabControl1.TabPages[this.bTabControl1.TabPages.Count - 1].Name = sCode;
                    if (i > 0)
                    {
                        arrChartColumn.Clear();
                        if (!string.IsNullOrEmpty(this._SortingKey))
                            arrChartColumn.Add(_SortingKey);
                    }

                    if ((sCode == Definition.CHART_TYPE.HISTOGRAM || sCode == Definition.CHART_TYPE.DOT_PLOT) && DataUtil.IsNullOrEmptyDataTable(_RawDataTableEtc2))
                    {
                        _RawDataTableEtc2 = CreateChartDataTableEtc(_RawDataTable, arrChartColumn, llstSortingKey, this._arrSubData);
                    }

                    analysisChart = new BaseAnalysisChart();
                    analysisChart.Pagekey = Definition.PAGE_KEY_SPC_CONTROL_CHART_UC;
                    if (sCode == Definition.CHART_TYPE.RUN)
                        analysisChart.Itemkey = Definition.CHART_BUTTON.CHART_ANALYSIS_RUN;
                    else
                        analysisChart.Itemkey = Definition.CHART_BUTTON.CHART_ANALYSIS;

                    analysisChart.IsShowInLegend = false;
                    analysisChart.IsViewButtonList = true;
                    analysisChart.IsVisibleLegendScrollBar = true;
                    analysisChart.CHART_TYPE = sCode;
                    analysisChart.AnalysisChart.Chart.Axes.Bottom.Labels.Angle = 90;
                    analysisChart.InitializeChart();
                    analysisChart.AnalysisChart.LegendVisible = false;
                    analysisChart.llstSortingKey = llstSortingKey;
                    if (sCode == Definition.CHART_TYPE.RUN)
                    {
                        DataTable dtRun = _RawDataTable.Copy();
                        arrSeries.Clear();
                        for (int j = 0; j < llstSortingKey.Count; j++)
                        {
                            string sKey = llstSortingKey.GetKey(j).ToString();
                            for (int k = 0; k < this._arrSubData.Count; k++)
                            {
                                string sValue = sKey + "_" + this._arrSubData[k].ToString();
                                if (!arrSeries.Contains(sValue))
                                {
                                    arrSeries.Add(sValue);
                                    dtRun.Columns.Add(sValue, typeof(double));
                                }
                            }
                        }

                        foreach (DataRow dr in dtRun.Rows)
                        {
                            for (int k = 0; k < this._arrSubData.Count; k++)
                            {
                                string sValue = dr[this._SortingKey].ToString() + "_" + this._arrSubData[k].ToString();
                                dr[sValue] = dr[this._arrSubData[k].ToString()];
                            }
                        }

                        dtRun.Columns.Remove(this._SortingKey);
                        for (int k = 0; k < this._arrSubData.Count; k++)
                            dtRun.Columns.Remove(this._arrSubData[k].ToString());

                        analysisChart.AnalysisChart.LegendVisible = true;
                        analysisChart.ArraySeriesColumn = arrSeries;
                        analysisChart.xLabelName = CommonChart.COLUMN_NAME_SEQ_INDEX;
                        analysisChart.ChangeXLabelColumn = Definition.CHART_COLUMN.TIME2;
                        analysisChart.dtDataSource = dtRun;
                    }
                    else if (sCode == Definition.CHART_TYPE.HISTOGRAM)
                    {
                        analysisChart.xLabelName = CommonChart.COLUMN_NAME_SEQ_INDEX;
                        analysisChart.ChangeXLabelColumn = Definition.CHART_COLUMN.VALUE;
                        analysisChart.dtDataSource = _RawDataTableEtc2;

                        analysisChart.ArraySeriesColumn.Clear();
                        analysisChart.ArraySeriesColumn.Add(Definition.CHART_COLUMN.SAMPLE_COUNT);
                    }
                    else if (sCode == Definition.CHART_TYPE.DOT_PLOT)
                    {
                        DataTable dtDot = _RawDataTableEtc2.Clone();
                        DataRow dRow = null;
                        int iRow = 0;
                        foreach (DataRow dr in _RawDataTableEtc2.Rows)
                        {
                            int dCount = int.Parse(dr[Definition.CHART_COLUMN.SAMPLE_COUNT].ToString());
                            int iIndex = int.Parse(dr[CommonChart.COLUMN_NAME_SEQ_INDEX].ToString());
                            double dData = double.Parse(dr[Definition.CHART_COLUMN.VALUE].ToString());

                            if (dCount > 0)
                            {   iRow++;
                                for (int xi = 1; xi <= dCount; xi++)
                                {
                                    dRow = dtDot.NewRow();
                                    dRow[CommonChart.COLUMN_NAME_SEQ_INDEX] = iRow;
                                    dRow[this._SortingKey] = dr[this._SortingKey];
                                    dRow[Definition.CHART_COLUMN.SAMPLE_COUNT] = xi;
                                    dRow[Definition.CHART_COLUMN.VALUE] = dr[Definition.CHART_COLUMN.VALUE];
                                    dtDot.Rows.Add(dRow);
                                }
                            }
                            else
                            {
                                dRow = dtDot.NewRow();
                                dRow[CommonChart.COLUMN_NAME_SEQ_INDEX] = iRow++;
                                dRow[this._SortingKey] = dr[this._SortingKey];
                                dRow[Definition.CHART_COLUMN.SAMPLE_COUNT] = dCount;
                                dRow[Definition.CHART_COLUMN.VALUE] = dr[Definition.CHART_COLUMN.VALUE];
                                dtDot.Rows.Add(dRow);
                            }
                        }

                        dtDot.AcceptChanges();
                        analysisChart.xLabelName = CommonChart.COLUMN_NAME_SEQ_INDEX;
                        analysisChart.ChangeXLabelColumn = Definition.CHART_COLUMN.VALUE;
                        analysisChart.dtDataSource = dtDot;
                    }
                    else if (sCode == Definition.CHART_TYPE.BOX)
                    {
                        _RawDataTableEtc = this.CreateChartDataTableBox(_RawDataTable, arrChartColumn, llstSortingKey, this._arrSubData);

                        analysisChart.xLabelName = this._SortingKey;
                        analysisChart.dtDataSource = _RawDataTableEtc;

                        analysisChart.ArraySeriesColumn.Clear();
                        analysisChart.ArraySeriesColumn.Add(Definition.CHART_COLUMN.VALUE);

                    }
                    bool bRtn = analysisChart.DrawAnalysisChart();
                    if (!bRtn) new Exception("error");

                    analysisChart.Dock = DockStyle.Fill;
                    analysisChart.AnalysisChart.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(AnalysisChart_ClickSeries);
                    analysisChart.AnalysisChart.Chart.MouseLeave += new EventHandler(Chart_MouseLeave);
                    this.bTabControl1.TabPages[this.bTabControl1.TabPages.Count - 1].Controls.Add(analysisChart);


                    chartListBox = new Steema.TeeChart.ChartListBox();
                    chartListBox.Dock = DockStyle.Left;
                    chartListBox.Width = 150;
                    chartListBox.Name = sCode;
                    chartListBox.ShowSeriesIcon = false;
                    for (int k = 0; k < llstSortingKey.Count; k++)
                    {
                        s = new Steema.TeeChart.Styles.LinePoint();
                        s.Title = llstSortingKey.GetKey(k).ToString();
                        s.Color = analysisChart.GetSeriesColor(k);
                        chartListBox.Items.Add(s);
                    }
                    chartListBox.TextChanged += new EventHandler(chartListBox_TextChanged);
                    chartListBox.SelectedValueChanged += new EventHandler(chartListBox_SelectedValueChanged);
                    chartListBox.Click += new EventHandler(chartListBox_Click);

                    this.bTabControl1.TabPages[this.bTabControl1.TabPages.Count - 1].BackColor = Color.White;
                    this.bTabControl1.TabPages[this.bTabControl1.TabPages.Count - 1].Controls.Add(chartListBox);


                }

                this.bTabControl1.SelectedIndex = bTabControl1.TabPages.Count - 1;
                this.bTabControl1.ResizeTabPage();
                //**End Chart Drawing     
            }
            catch (Exception ex)
            {
                this.MsgClose();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                this.MsgClose();
                if (_RawDataTable != null) _RawDataTable.Dispose();
                if (_RawDataTableEtc != null) _RawDataTableEtc.Dispose();
                if (_dtChartData != null) _dtChartData.Dispose();
                if (ds != null) ds.Dispose();
            }
        }

        private void chartListBox_Click(object sender, EventArgs e)
        {


        }

        private void chartListBox_SelectedValueChanged(object sender, EventArgs e)
        {


            Steema.TeeChart.ChartListBox _chart = sender as Steema.TeeChart.ChartListBox;

            if (_chart.SelectedItem == null) return;

            BaseAnalysisChart baseChart = null;
            for (int i = 0; i < this.bTabControl1.TabPages.Count; i++)
            {
                if (_chart.Name == this.bTabControl1.TabPages[i].Name.ToString())
                {
                    baseChart = this.bTabControl1.TabPages[i].Controls[0] as BaseAnalysisChart;
                    break;
                }
            }
            List<SeriesInfo> lst = baseChart.AnalysisChart.GetAllSeriesInfos();
            if (lst.Count == 0)
            {
                if (baseChart.CHART_TYPE == Definition.CHART_TYPE.DOT_PLOT
                || baseChart.CHART_TYPE == Definition.CHART_TYPE.BOX
                || baseChart.CHART_TYPE == Definition.CHART_TYPE.HISTOGRAM)
                {
                    for (int i = 0; i < baseChart.AnalysisChart.Chart.Series.Count; i++)
                    {
                        if (_chart.SelectedSeries.Title.Equals(baseChart.AnalysisChart.Chart.Series[i].Title))
                            baseChart.AnalysisChart.Chart.Series[i].Active = _chart.SelectedSeries.Active;
                    }
                }
                else
                {
                    for (int i = 0; i < baseChart.AnalysisChart.Chart.Series.Count; i++)
                    {
                        baseChart.AnalysisChart.Chart.Series[i].Active = _chart.SelectedSeries.Active;
                    }
                }

            }
            else
            {

                for (int i = 0; i < lst.Count; i++)
                {
                    SeriesInfo si = lst[i] as SeriesInfo;
                    if (si.Group.ToString() == _chart.SelectedItem.ToString())
                    {
                        si.Series.Active = _chart.SelectedSeries.Active;
                    }
                }
            }

        }

        private void chartListBox_TextChanged(object sender, EventArgs e)
        {

        }



        private void AnalysisChart_ClickSeries(object sender, Steema.TeeChart.Styles.Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                SeriesInfo seriesInfo = ((ChartSeriesGroup)sender).GetSeriesInfo(s);
                if (seriesInfo == null || !seriesInfo.SeriesData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX)) return;

                #region ToolTip

                BaseAnalysisChart baseChart = FindSPCChart((Control)sender);
                if (baseChart is BaseAnalysisChart)
                {
                    for (int i = 0; i < baseChart.AnalysisChart.Chart.Tools.Count; i++)
                    {
                        if (baseChart.AnalysisChart.Chart.Tools[i].GetType() == typeof(Steema.TeeChart.Tools.MarksTip))
                        {
                            Steema.TeeChart.Tools.MarksTip mkTip = (Steema.TeeChart.Tools.MarksTip)baseChart.AnalysisChart.Chart.Tools[i];
                            mkTip.MouseAction = Steema.TeeChart.Tools.MarksTipMouseAction.Click;
                            mkTip.Active = false;
                        }
                    }


                    DataTable _dt = (DataTable)seriesInfo.SeriesData;
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < _dt.Columns.Count; i++)
                    {
                        string sColName = _dt.Columns[i].ColumnName.ToString();
                        if (sColName == CommonChart.COLUMN_NAME_SEQ_INDEX || sColName == Definition.CHART_COLUMN.TIME2) continue;

                        if (baseChart.ArraySeriesColumn.Contains(sColName))
                        {
                            string[] arr = sColName.Split('_');
                            if (seriesInfo.Series.Title == sColName)
                            {
                                sb.AppendFormat("{0} : {1}\r\n", sColName, _dt.Rows[valueIndex][sColName].ToString());
                            }
                        }
                        else
                            sb.AppendFormat("{0} : {1}\r\n", this._lang.GetVariable(Definition.SPC_LABEL_ + sColName), _dt.Rows[valueIndex][sColName].ToString());

                    }

                    if (baseChart.AnalysisChart.Chart.Tools.Count > 0)
                    {
                        Annotation ann = (Annotation)baseChart.AnalysisChart.Chart.Tools[5];
                        this._chartUtil.ShowAnnotate(baseChart.AnalysisChart.Chart, ann, e.X, e.Y, sb.ToString());
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
            }
        }


        private void Chart_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                BaseAnalysisChart baseChart = FindSPCChart((Control)sender);
                Annotation ann;
                if (baseChart is DefaultChart)
                {
                    for (int i = 0; i < baseChart.AnalysisChart.Chart.Tools.Count; i++)
                    {
                        if (baseChart.AnalysisChart.Chart.Tools[i].GetType() == typeof(Annotation))
                        {
                            ann = baseChart.AnalysisChart.Chart.Tools[i] as Annotation;
                            if (ann.Active)
                            {
                                ann.Active = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
            }
        }



        #endregion


        private void ClickButtonShowChartData()
        {
            if (DataUtil.IsNullOrEmptyDataTable(_dataManager.RawDataTable))
            {
                this.MsgClose();
                MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                return;
            }
            ChartDataPopup chartDataPop = null;
            DialogResult result;

            //return ; 
            CreateChartDataTable _chartDT = new CreateChartDataTable();
            _chartDT.COMPLEX_YN = _dataManager.RawDataTable.Rows[0][COLUMN.COMPLEX_YN].ToString();
            DataTable dt = _chartDT.GetMakeDataTable(_dataManager.RawDataTable);
            this._lstRawColumn = _chartDT.lstRawColumn;
            if (_chartDT != null) _chartDT = null;
            try
            {
                chartDataPop = new ChartDataPopup();
                chartDataPop.URL = this.URL;
                chartDataPop.SessionData = this.sessionData;
                chartDataPop.DataTableParam = CommonPageUtil.ExcelExport(dt, _lstRawColumn); ;
                chartDataPop.InitializePopup();
                result = chartDataPop.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    chartDataPop.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                dt.Dispose();                
            }

        }


        private void bButtonList1_ButtonClick(string name)
        {

            try
            {
                //this.MsgShow(this._lang.GetVariable("RMS_PROGRESS_SEARCH"));
                if (name.ToUpper() == Definition.ButtonKey.EXPORT)
                {
                    ClickButtonShowChartData();
                }

            }
            catch (Exception ex)
            {
                //this.MsgClose();
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }finally
            {
                //this.MsgClose();
            }


        }


    }
}
