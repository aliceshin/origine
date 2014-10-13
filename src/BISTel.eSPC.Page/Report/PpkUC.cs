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

namespace BISTel.eSPC.Page.Report
{
    public enum enum_PpkUC
    {

        SPC_V_SELECT = 0,
        MODEL_CONFIG_RAWID,
        COMPLEX_YN ,
        MAIN_YN ,
        DEFAULT_CHART_LIST,
        //RESTRICT_SAMPLE_DAYS ,
        AREA ,
        OPERATION_ID ,
        OPERATION_DESC,
        PARAM_ALIAS
    }
    
         
    public partial class PpkUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        MultiLanguageHandler _lang;
        LinkedList _llstSearchCondition = new LinkedList();
        List<string> _lstLot = new List<string>();
        List<string> _lstRawColumn = null;

        DataTable _dtMain = new DataTable();
        DataTable _dtResult = new DataTable();

        SortedList _sortHeader = new SortedList();
        SortedList _sortHeaderLabel = new SortedList();

        BISTel.eSPC.Common.BSpreadUtility _bSpreadUtil = new BSpreadUtility();
        BISTel.eSPC.Common.CommonUtility _ComUtil = null;

        string _AreaRawID = string.Empty;
        string _Area = string.Empty;
        string _Line = string.Empty;
        string _LineRawID = string.Empty;
        string _EQPModel = string.Empty;

        string sStartTime = string.Empty;
        string sEndTime = string.Empty;

        string _sParamTypeCD = string.Empty;
        string _sOperationID = string.Empty;
        string _sParameter = string.Empty;
        string _sEQPID = string.Empty;
        string _sProductID = string.Empty;
        string _sPeriodPpk = string.Empty;
        string _OperationColumnName = Definition.CHART_COLUMN.MEASURE_OPERATION_ID;

        ArrayList arrSortingKey;
        DataTable dtParameter = null;
        ArrayList arrModelCongifRawID = new ArrayList();



        #endregion


        #region ::: Properties

        public LinkedList llstSearchCondition
        {
            set
            {
                this._llstSearchCondition = value;
            }
            get
            {
                return this._llstSearchCondition;
            }
        }

        #endregion


        public PpkUC()
        {
            arrSortingKey = new ArrayList();
            InitializeComponent();
        }


        #region ::: Override Method

        public override void PageInit()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;

            this.KeyOfPage = this.GetType().FullName; // 필수 입력 (Key값을 받은 후 DC을 Active해주기 때문이다.

            this.InitializePage();
        }



        public override void PageSearch(LinkedList llstCondition)
        {
            this._llstSearchCondition.Clear();
            DataTable dt = null;
            DataSet _ds = null;


            if (llstCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM];
                sStartTime = CommonPageUtil.StartDate(dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
            }
            if (llstCondition[Definition.DynamicCondition_Search_key.DATETIME_TO] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.DATETIME_TO];
                sEndTime = CommonPageUtil.EndDate(dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
            }

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
            

            DataTable dtParameter = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.PARAM];
            this._sParamTypeCD = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE]);
            this._sOperationID = DataUtil.GetConditionKeyDataList((DataTable)llstCondition[Definition.DynamicCondition_Search_key.OPERATION], Definition.DynamicCondition_Condition_key.VALUEDATA,true);
            this._sEQPID = DataUtil.GetConditionKeyDataList((DataTable)llstCondition[Definition.DynamicCondition_Search_key.EQP_ID], Definition.DynamicCondition_Condition_key.VALUEDATA, true);
            this._sProductID = DataUtil.GetConditionKeyDataList((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PRODUCT], Definition.DynamicCondition_Condition_key.VALUEDATA, true);
            this._sPeriodPpk = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PERIOD_PPK]);
            this.arrSortingKey = CommonPageUtil.GetConditionKeyDataListArr((DataTable)llstCondition[Definition.DynamicCondition_Search_key.SORTING_KEY], Definition.DynamicCondition_Condition_key.VALUEDATA);

            LinkedList _llstModelConfigRawID = new LinkedList();
            arrModelCongifRawID.Clear();
            string sParameter = DataUtil.GetConditionKeyDataList(dtParameter, Definition.DynamicCondition_Condition_key.VALUEDATA, true);            
            _llstModelConfigRawID.Clear();
            _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, _LineRawID);
            
            if (!string.IsNullOrEmpty(_AreaRawID))
                _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, _AreaRawID);

            if (!string.IsNullOrEmpty(_EQPModel))
                _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, _EQPModel);                                                            

            _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, _sParamTypeCD);
            if (!string.IsNullOrEmpty(sParameter) && sParameter.IndexOf(Definition.VARIABLE.STAR) < 0)
                _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, sParameter);
                
            _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.MAIN_YN, "Y");

            if (!string.IsNullOrEmpty(_sOperationID) && _sOperationID != "*")
                _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, _sOperationID);
            //sub modeling 존재여부 확인                      
            _ds = _wsSPC.GetSPCModelConfigSearch(_llstModelConfigRawID.GetSerialData());
            if (!DataUtil.IsNullOrEmptyDataSet(_ds))
            {
                for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                {
                    string strModelCongifRawID = _ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString();

                    if (!arrModelCongifRawID.Contains(strModelCongifRawID))
                        arrModelCongifRawID.Add(strModelCongifRawID);
                }
            }
           

            if (arrModelCongifRawID.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                return;
            }
            string sModelCongifRawid = CommonPageUtil.GetConditionKeyArrayList(arrModelCongifRawID);

            this._llstSearchCondition.Clear();
            this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, sStartTime);
            this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, sEndTime);
            this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PERIOD_PPK, this._sPeriodPpk);
            this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, sModelCongifRawid);

            if(this._sParamTypeCD.Equals("MET"))
                _OperationColumnName = Definition.CHART_COLUMN.MEASURE_OPERATION_ID;
            else
                _OperationColumnName = Definition.CHART_COLUMN.OPERATION_ID;
                
            PROC_DataBinding();

        }

        #endregion

        #region ::: PageLoad & Initialize

        public void InitializePage()
        {
            this._ComUtil = new CommonUtility();
            this._bSpreadUtil = new BSpreadUtility();
            this._lang = MultiLanguageHandler.getInstance();

            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this.InitializeDataButton();
            this.InitializeBSpread();
            this.InitializeLayout();
        }

        public void InitialUserInfo()
        {
        }

        public void InitializeLayout()
        {
            //this.bsprData.ActiveSheet.FrozenColumnCount = 0;
            //this.bsprData.ActiveSheet.FrozenRowCount = 0;
            this.bsprData.ColFronzen = 0;
            this.bsprData.RowFronzen = 0;
            this.btitMonthly.Title = this._lang.GetVariable(Definition.TITLE_PROCESS_CAPABILITY);
        }

        public void InitializeDataButton()
        {
            this._Initialization.InitializeButtonList(this.bbtnList, ref bsprData, Definition.PAGE_KEY_SPC_PPK_UC, Definition.BUTTONLIST_KEY_PROCESS_CAPABILITY, this.sessionData);
            this.FunctionName = Definition.FUNC_KEY_SPC_PROCESS_CAPABILITY;
            this.ApplyAuthory(this.bbtnList);
        }

        public void InitializeBSpread()
        {                        
            this.bsprData.ActiveSheet.RowCount=0;
            this.bsprData.ActiveSheet.ColumnCount=0;
            this.bsprData.AutoGenerateColumns = true;
            this.bsprData.AddHeadComplete();
            
        }

        #endregion

        #region ::: User Defined Method.


        private string GetSelectString()
        {

            LinkedList llstSelect = new LinkedList();

            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(_sEQPID) && _sEQPID.IndexOf(Definition.VARIABLE.STAR) < 0)
                sb.AppendFormat(" (EQP_ID IN ({0}) OR {1} IN ({0}))", _sEQPID, Definition.DynamicCondition_Condition_key.MEASURE_EQP_ID);

            if (!string.IsNullOrEmpty(_sOperationID) && _sOperationID.IndexOf(Definition.VARIABLE.STAR) < 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.AppendFormat(" ({0} in ({1}) OR {2} in ({1}))", Definition.DynamicCondition_Condition_key.OPERATION_ID, _sOperationID, Definition.DynamicCondition_Condition_key.MEASURE_OPERATION_ID);
            }

            if (!string.IsNullOrEmpty(_sProductID) && _sProductID.IndexOf(Definition.VARIABLE.STAR) < 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.AppendFormat(" PRODUCT_ID IN ({0})", _sProductID);
            }

            return sb.ToString();
        }

        private void SpreadHeader()
        {
            int iCount = 0;
            this._sortHeader.Clear();
            this._sortHeaderLabel.Clear();
            _sortHeader.Add(iCount++, "_SELECT");
            _sortHeader.Add(iCount++, COLUMN.MODEL_CONFIG_RAWID);
            _sortHeader.Add(iCount++, COLUMN.COMPLEX_YN);
            _sortHeader.Add(iCount++, COLUMN.MAIN_YN);
            _sortHeader.Add(iCount++, COLUMN.DEFAULT_CHART_LIST);
            //_sortHeader.Add(iCount++, "RESTRICT_SAMPLE_DAYS");
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.AREA);            
            _sortHeader.Add(iCount++, this._OperationColumnName);            
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.OPERATION_DESC);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PARAM_ALIAS);

            _sortHeaderLabel.Add("_SELECT", Definition.SpreadHeaderColKey.V_SELECT);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.AREA, Definition.SpreadHeaderColKey.AREA);           
            _sortHeaderLabel.Add(this._OperationColumnName, Definition.SpreadHeaderColKey.OPERATION_ID);                            
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.OPERATION_DESC, Definition.SpreadHeaderColKey.OPERATION_DESC);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PARAM_ALIAS, Definition.SpreadHeaderColKey.PARAM_ALIAS);
            for (int i = 0; i < this.arrSortingKey.Count; i++)
            {
                string sCol = this.arrSortingKey[i].ToString();
                _sortHeader.Add(iCount++, sCol);
                _sortHeaderLabel.Add(sCol, sCol);
            }
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PERIOD);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.SPEC);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.LSL);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.USL);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.MIN);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.MAX);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.AVG);         
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.STDDEV);            
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PPK);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PP);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PPU);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PPL);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.SUM);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.SUM_SQUARED);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.LOT_QTY);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.SAMPLE_QTY);

            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PERIOD, Definition.SpreadHeaderColKey.PERIOD);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.SPEC, Definition.SpreadHeaderColKey.SPEC);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.LSL, Definition.SpreadHeaderColKey.LSL);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.USL, Definition.SpreadHeaderColKey.USL);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.MIN, Definition.SpreadHeaderColKey.MIN);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.MAX, Definition.SpreadHeaderColKey.MAX);                        
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.AVG, Definition.SpreadHeaderColKey.AVG);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.STDDEV, Definition.SpreadHeaderColKey.STDDEV);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PPK, Definition.SpreadHeaderColKey.PPK);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PP, Definition.SpreadHeaderColKey.PP);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PPU, Definition.SpreadHeaderColKey.PPU);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PPL, Definition.SpreadHeaderColKey.PPL);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.SUM, Definition.SpreadHeaderColKey.SUM);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.SUM_SQUARED, Definition.SpreadHeaderColKey.SUM_SQUARED);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.LOT_QTY, Definition.SpreadHeaderColKey.LOT_QTY);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.SAMPLE_QTY, Definition.SpreadHeaderColKey.SAMPLE_QTY);
        }

        private void PROC_DataBinding()
        {
            ////초기화           
            StringBuilder sb = null;
            StringBuilder sbSort=null;
            
            CreateChartDataTable _chartData = null;
            DataTableGroupBy dtGroupBy = null;
            DataTable dtGB = null;
            DataTable dtSearch = null;
            DataSet _ds = null;
            CommonSPCStat comSPCStat = null;
            List<double> listRawData = null;
            DataRow nRow = null;
            string sField = string.Empty;
            string sGroupBy = string.Empty;
            string sRowFilter = string.Empty;
            string sWhere = string.Empty;
            StringBuilder sbField = new StringBuilder();
            StringBuilder sbGroup = new StringBuilder();

            try
            {
                dtGB = new DataTable();
                dtSearch = new DataTable();
                dtGroupBy = new DataTableGroupBy();
                _chartData = new CreateChartDataTable();
                sb = new StringBuilder();
                sbSort = new StringBuilder();
                comSPCStat = new CommonSPCStat();

                this.bsprData.ActiveSheet.ColumnCount = 0;
                this.bsprData.ActiveSheet.RowCount = 0;                
                SpreadHeader();
                this.bsprData.ActiveSheet.RowCount = 0;
                this.bsprData.DataSource = null;
                this.MsgShow(COMMON_MSG.Query_Data);
                _llstSearchCondition.Add(Definition.CONDITION_KEY_CONTEXT_KEY_LIST, GetSelectString());
                _ds = _wsSPC.GetPpkReport(_llstSearchCondition.GetSerialData());
                if (!DataUtil.IsNullOrEmptyDataSet(_ds))
                {
                    dtSearch = CommonPageUtil.CLOBnBLOBParsing(_ds, _llstSearchCondition, false);
                    if (_ds != null) _ds.Dispose();

                    if (DataUtil.IsNullOrEmptyDataTable(dtSearch))
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }
                                                                            
                    _chartData.COMPLEX_YN = "N";
                    if (dtSearch.Rows.Count > 0)
                        _chartData.COMPLEX_YN = dtSearch.Rows[0]["COMPLEX_YN"].ToString();

                    _dtResult = _chartData.GetPpkMakeDataTable(dtSearch);
                    _lstRawColumn = _chartData.lstRawColumn;
                    if (_chartData != null) _chartData = null;
                    
                    sbField.AppendFormat("MODEL_CONFIG_RAWID,COMPLEX_YN,MAIN_YN,DEFAULT_CHART_LIST,AREA,{0} ,PARAM_ALIAS",_OperationColumnName);
                    sbGroup.AppendFormat("MODEL_CONFIG_RAWID,COMPLEX_YN,MAIN_YN,DEFAULT_CHART_LIST,AREA,{0},PARAM_ALIAS", _OperationColumnName);
                    sbSort.AppendFormat("{0},PARAM_ALIAS", _OperationColumnName);                    
                                                             
                    for (int i = 0; i < this.arrSortingKey.Count; i++)
                    {
                        string sCol = this.arrSortingKey[i].ToString();
                        if (_dtResult.Columns.Contains(sCol))
                        {
                            sbField.AppendFormat(",{0}", sCol);
                            sbGroup.AppendFormat(",{0}", sCol);
                            sbSort.AppendFormat(",{0}", sCol);
                        }
                    }
                    sbSort.Append(",PERIOD,USL,LSL ASC");
                    sbField.Append(",PERIOD,MEAN_LSL LSL,MEAN_USL USL ");
                    sbGroup.Append(",PERIOD,MEAN_LSL,MEAN_USL ");
                    sRowFilter = string.Empty;
                    dtGB = dtGroupBy.SelectGroupByInto("PPK", _dtResult, sbField.ToString(), sRowFilter, sbGroup.ToString());
                    dtGB = DataUtil.DataTableImportRow(dtGB.Select(null, sbSort.ToString()));
                }
                else
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                _llstSearchCondition.Clear();
                _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.OPERATION_TYPE, this._sParamTypeCD == "MET" ? "METROLOGY" : "PROCESSING");
                DataSet dsOperation = _wsSPC.GetOperationID(_llstSearchCondition.GetSerialData());

                _dtMain = dtGB.Copy();
                _dtMain.Columns.Add(Definition.CHART_COLUMN.OPERATION_DESC, typeof(string));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.SPEC, typeof(string));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.MIN, typeof(double));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.MAX, typeof(double));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.SUM, typeof(double));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.SUM_SQUARED, typeof(double));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.STDDEV, typeof(double));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.AVG, typeof(double));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.PPK, typeof(double));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.PP, typeof(double));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.PPU, typeof(double));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.PPL, typeof(double));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.LOT_QTY, typeof(int));
                _dtMain.Columns.Add(Definition.CHART_COLUMN.SAMPLE_QTY, typeof(int));
                
                sField = "count(LOT_ID) count ";
                sGroupBy = "LOT_ID";
                sRowFilter = string.Empty;
                StringBuilder sbSelect = null;
                for (int i = 0; i < dtGB.Rows.Count; i++)
                {
                    string param_alias = dtGB.Rows[i][Definition.CHART_COLUMN.PARAM_ALIAS].ToString();
                    string operation = dtGB.Rows[i][_OperationColumnName].ToString();
                    string sUsl = dtGB.Rows[i][Definition.CHART_COLUMN.USL].ToString();
                    string sLsl = dtGB.Rows[i][Definition.CHART_COLUMN.LSL].ToString();
                    double usl = double.NaN;
                    double lsl = double.NaN;

                    if (!string.IsNullOrEmpty(sUsl))
                        usl = double.Parse(sUsl);
                    if (!string.IsNullOrEmpty(sLsl))
                        lsl = double.Parse(sLsl);

                    sbSelect = new StringBuilder();
                    if (double.IsNaN(usl) && double.IsNaN(lsl))
                    {
                        sbSelect.AppendFormat("PARAM_ALIAS = '{0}' AND {1} = '{2}' AND MEAN_USL IS NULL AND MEAN_LSL IS NULL AND PERIOD = '{3}' "
                            , param_alias
                            , this._OperationColumnName
                            , operation
                            , dtGB.Rows[i]["PERIOD"].ToString()
                            );
                    }
                    else if (double.IsNaN(usl) && !double.IsNaN(lsl))
                    {
                        sbSelect.AppendFormat("PARAM_ALIAS = '{0}' AND {1} = '{2}' AND MEAN_USL IS NULL AND MEAN_LSL = '{3}' AND PERIOD = '{4}' "
                            , param_alias
                            , this._OperationColumnName
                            , operation
                            , sLsl
                            , dtGB.Rows[i]["PERIOD"].ToString()
                            );
                    }
                    else if (!double.IsNaN(usl) && double.IsNaN(lsl))
                    {
                        sbSelect.AppendFormat("PARAM_ALIAS = '{0}' AND {1} = '{2}' AND MEAN_USL = '{3}' AND MEAN_LSL IS NULL AND PERIOD = '{4}' "
                            , param_alias
                            , this._OperationColumnName
                            , operation
                            , sUsl
                            , dtGB.Rows[i]["PERIOD"].ToString()
                            );
                    }
                    else
                    {
                        sbSelect.AppendFormat("PARAM_ALIAS = '{0}' AND {1} = '{2}' AND MEAN_USL = '{3}' AND MEAN_LSL = '{4}' AND PERIOD = '{5}' "
                            , param_alias
                            , this._OperationColumnName
                            , operation
                            , sUsl
                            , sLsl
                            , dtGB.Rows[i]["PERIOD"].ToString()
                            );
                    }
                    for (int k = 0; k < this.arrSortingKey.Count; k++)
                    {
                        string sCol = this.arrSortingKey[k].ToString();
                        if (_dtResult.Columns.Contains(sCol))
                        {
                            sbSelect.AppendFormat(" AND {0} = '{1}'", sCol, dtGB.Rows[i][sCol].ToString());
                        }
                    }

                    DataTable dt =DataUtil.DataTableImportRow(_dtResult.Select(sbSelect.ToString()));
                    listRawData = comSPCStat.AddDataList(dt);
                    if (listRawData.Count == 0) continue;

                    DataRow[] drSelectOperation = dsOperation.Tables[0].Select(string.Format("OPERATION_ID='{0}'", operation));
                    comSPCStat.CalcPpk(listRawData.ToArray(), usl, lsl);
                    nRow = _dtMain.Rows[i];
                    nRow[Definition.CHART_COLUMN.OPERATION_DESC] = drSelectOperation.Length > 0 ? drSelectOperation[0]["DESCRIPTION"].ToString() : null;
                    nRow[Definition.CHART_COLUMN.SPEC] = sLsl + "~" + sUsl;
                    nRow[Definition.CHART_COLUMN.SAMPLE_QTY] = listRawData.Count;
                    nRow[Definition.CHART_COLUMN.MIN] = comSPCStat.min;
                    nRow[Definition.CHART_COLUMN.MAX] = comSPCStat.max;
                    nRow[Definition.CHART_COLUMN.AVG] = comSPCStat.mean;
                    nRow[Definition.CHART_COLUMN.SUM] = comSPCStat.sum;
                    nRow[Definition.CHART_COLUMN.STDDEV] = comSPCStat.stddev;
                    nRow[Definition.CHART_COLUMN.SUM_SQUARED] = comSPCStat.sum2;
                    if (double.IsNaN(comSPCStat.ppk))
                        nRow[Definition.CHART_COLUMN.PPK] = DBNull.Value;
                    else
                        nRow[Definition.CHART_COLUMN.PPK] = comSPCStat.ppk;

                    if (double.IsNaN(comSPCStat.pp))
                        nRow[Definition.CHART_COLUMN.PP] = DBNull.Value;
                    else
                        nRow[Definition.CHART_COLUMN.PP] = comSPCStat.pp;

                    if (double.IsNaN(comSPCStat.ppu))
                        nRow[Definition.CHART_COLUMN.PPU] = DBNull.Value;
                    else
                        nRow[Definition.CHART_COLUMN.PPU] = comSPCStat.ppu;

                    if (double.IsNaN(comSPCStat.ppl))
                        nRow[Definition.CHART_COLUMN.PPL] = DBNull.Value;
                    else
                        nRow[Definition.CHART_COLUMN.PPL] = comSPCStat.ppl;
                    nRow[Definition.CHART_COLUMN.LOT_QTY] = dtGroupBy.SelectGroupByInto("LOT", dt, sField, sRowFilter, sGroupBy).Rows.Count;
                }
                _dtMain.AcceptChanges();

                this.bsprData.UseSpreadEdit = false;
                this.bsprData.AutoGenerateColumns = false;
                this.bsprData.ActiveSheet.DefaultStyle.ResetLocked();
                this.bsprData.ClearHead();
                this.bsprData.UseEdit = true;
                this.bsprData.Locked = true;
                
                //this.bsprData.ActiveSheet.FrozenColumnCount = 1;                                                                
                this.bsprData.ColFronzen = 1;

                this._Initialization.InitializePpkColumnHeader(ref this.bsprData,_sortHeader,this._sortHeaderLabel);                
                this.bsprData.DataSource = _dtMain;
                if (dsOperation != null) dsOperation.Dispose();
            }
            catch (Exception ex)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);

            }
            finally
            {
                this.MsgClose();
                if (dtGB != null) dtGB.Dispose();
                if (_dtMain != null) _dtMain.Dispose();
            }
        }


        private string GetModelConfigRawID(DataTable _dt, string _sModelCongifRawid)
        {

            if (_dt.Rows.Count > 1)
            {
                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    string strValue = _dt.Rows[i][Definition.DynamicCondition_Condition_key.MAIN_YN].ToString();
                    if (strValue == "Y")
                    {
                        if (!string.IsNullOrEmpty(_sModelCongifRawid)) _sModelCongifRawid += ",";
                        _sModelCongifRawid += strValue;
                    }
                }
            }
            else
            {
                _sModelCongifRawid = _dt.Rows[0][Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString();
            }

            return _sModelCongifRawid;
        }



        #endregion

        #region ::: EventHandler



        private void bsprData_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != (int)enum_PpkUC.SPC_V_SELECT) return;
            if ((bool)this.bsprData.GetCellValue(e.Row, (int)enum_ProcessCapability.SPC_V_SELECT) == true)
            {
                for (int i = 0; i < bsprData.ActiveSheet.RowCount; i++)
                {
                    if (i == e.Row)
                    {
                        continue;
                    }
                    this.bsprData.ActiveSheet.Cells[i, (int)enum_ProcessCapability.SPC_V_SELECT].Value = 0;
                }
            }

        }


        private void bsprData_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader) return;           
        }


        private void bbtnList_ButtonClick(string name)
        {
            try
            {
                if (name.ToUpper() == Definition.ButtonKey.VIEW_CHART)
                {
                    this.ClickButtonChartView();
                }
                else if (name.ToUpper() == Definition.ButtonKey.EXPORT)
                {
                    this.ClickExportButton();
                }
                else if (name.ToUpper() == Definition.ButtonKey.SORT)
                {


                    if (this.bsprData.ActiveSheet.Rows.Count == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }


                    this.bsprData.ShowSortForm();
                }
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }


        private void ClickExportButton()
        {
            if (this.bsprData.ActiveSheet.RowCount > 0)
            {
                this.bsprData.Export(true);
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_NO_SEARCH_DATA));
            }
        }

        private void ClickButtonChartView()
        {

            ArrayList alCheckRowIndex = _bSpreadUtil.GetCheckedRowIndex(this.bsprData, (int)enum_ProcessCapability.SPC_V_SELECT);
            if (alCheckRowIndex.Count < 1 || alCheckRowIndex.Count > 1)
            {

                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("FDC_ALLOW_SINGLE_SELECTED_ROW"));
                return;
            }

            int iRowIndex = (int)alCheckRowIndex[0];            
            string strSPCModeName = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_PpkUC.PARAM_ALIAS].Text;
            string strModelConfigRawID = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_PpkUC.MODEL_CONFIG_RAWID].Text;
            string strDefaultChart = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_PpkUC.DEFAULT_CHART_LIST].Text;
            string strComplex_yn = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_PpkUC.COMPLEX_YN].Text;            
            string strParamAlias = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_PpkUC.PARAM_ALIAS].Text;
            string strArea = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_PpkUC.AREA].Text;
            string strOperation = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_PpkUC.OPERATION_ID].Text;

            if (string.IsNullOrEmpty(strDefaultChart))
            {
                MSGHandler.DisplayMessage(MSGType.Information, string.Format(MSGHandler.GetMessage("GENERAL_NO_ITEM"), "Default Charts"));
                return;
            }
   
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" ({0} ={1} OR {2} ={1})", Definition.DynamicCondition_Condition_key.OPERATION_ID, CommonPageUtil.GetConCatString(strOperation), Definition.DynamicCondition_Condition_key.MEASURE_OPERATION_ID);
            sb.AppendFormat(" AND {0} = {1}", Definition.DynamicCondition_Condition_key.PARAM_ALIAS, CommonPageUtil.GetConCatString(strParamAlias));                                        
            
            for(int i=1; i<this._sortHeader.Count; i++)
            {   
                string sColumn = this._sortHeader[i].ToString();
                string sValue = this.bsprData.ActiveSheet.Cells[iRowIndex, i].Text;
                if(sColumn==Definition.CHART_COLUMN.USL)
                {
                    if (string.IsNullOrEmpty(sValue))
                        sb.AppendFormat(" AND {0} IS NULL", Definition.CHART_COLUMN.MEAN_USL);
                    else
                        sb.AppendFormat(" AND {0} = {1}", Definition.CHART_COLUMN.MEAN_USL, CommonPageUtil.GetConCatString(sValue));                                        
                }
                else if (sColumn == Definition.CHART_COLUMN.LSL)
                {
                    if (string.IsNullOrEmpty(sValue))
                        sb.AppendFormat(" AND {0} IS NULL", Definition.CHART_COLUMN.MEAN_LSL); 
                    else
                        sb.AppendFormat(" AND {0} = {1}", Definition.CHART_COLUMN.MEAN_LSL, CommonPageUtil.GetConCatString(sValue));                                        
                }
                else if (sColumn == Definition.CHART_COLUMN.PERIOD)
                {                    
                    sb.AppendFormat(" AND {0} = {1}", Definition.CHART_COLUMN.PERIOD, CommonPageUtil.GetConCatString(sValue));                                        
                }                                      
                if(this.arrSortingKey.Contains(sColumn))
                {
                    if(!string.IsNullOrEmpty(sValue))                    
                        sb.AppendFormat(" AND {0} = {1}", sColumn, CommonPageUtil.GetConCatString(sValue));                                                            
                }              
            }
            
            
            DataTable dtParamData = DataUtil.DataTableImportRow(_dtResult.Select(sb.ToString()));
            if (DataUtil.IsNullOrEmptyDataTable(dtParamData))
            {
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                return;
            }

            CreateChartDataTable _chartData = new CreateChartDataTable();
            _chartData.COMPLEX_YN = strComplex_yn;
            dtParamData = _chartData.GetPpkChkartMakeDataTable(dtParamData);   
                      
            ChartViewPopup chartViewPop = new ChartViewPopup();
            chartViewPop.ChartVariable.LINE = _Line;
            chartViewPop.ChartVariable.AREA = strArea;
            chartViewPop.ChartVariable.DEFAULT_CHART = strDefaultChart;
            chartViewPop.ChartVariable.SPC_MODEL = strSPCModeName;
            chartViewPop.ChartVariable.PARAM_ALIAS = strSPCModeName;
            chartViewPop.ChartVariable.OPERATION_ID = strOperation;
            chartViewPop.ChartVariable.PRODUCT_ID = null;
            chartViewPop.ChartVariable.lstRawColumn = _chartData.lstRawColumn;
            chartViewPop.ChartVariable.dtParamData = dtParamData;
            chartViewPop.ChartVariable.complex_yn = strComplex_yn;
            chartViewPop.ChartVariable.dateTimeStart = DateTime.Parse(sStartTime);
            chartViewPop.ChartVariable.dateTimeEnd = DateTime.Parse(sEndTime);
            chartViewPop.ChartVariable.MODEL_CONFIG_RAWID = strModelConfigRawID;
            chartViewPop.ChartVariable.MAIN_YN = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)enum_PpkUC.MAIN_YN].Text;
            chartViewPop.ChartVariable.CONTEXT_LIST = sb.ToString();            
            chartViewPop.ChartVariable.CHART_PARENT_MODE = CHART_PARENT_MODE.PPK_REPORT;            
            chartViewPop.URL = this.URL;
            chartViewPop.SessionData = this.sessionData;
            chartViewPop.InitializePopup();
            DialogResult result = chartViewPop.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                dtParamData = null;
                chartViewPop = null;
            }

            _chartData = null;            

        }

        #endregion
    }
}
