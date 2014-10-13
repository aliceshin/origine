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

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;
namespace BISTel.eSPC.Page.Analysis
{
    public partial class MultiDataUC : BasePageUCtrl
    {
        eSPCWebService.eSPCWebService _wsSPC = null;
        Initialization _Initialization;
        MultiLanguageHandler _lang;        
        SourceDataManager _dataManager = null;

        BISTel.eSPC.Common.BSpreadUtility _bSpreadUtil = new BSpreadUtility();
        BISTel.eSPC.Common.CommonUtility _ComUtil = null;

        LinkedList _llstData = new LinkedList();                
                       

        string _Line = string.Empty;
        string _LineRawID = string.Empty;
        string _AreaRawID = string.Empty;
        string _EQPModel = string.Empty;
        
        string _ParamType = string.Empty;
        string _sStartTime = string.Empty;
        string _sEndTime = string.Empty;
        string _sFab = string.Empty;
        bool _bProb = false;
        bool _bTartProb = false;
       
        List<string> _lstSoringKey = new List<string>();
        ArrayList _arrSortingKey = new ArrayList();
        ArrayList _arrInfo = new ArrayList();
        ArrayList _arrItem = new ArrayList();
        ArrayList _arrSubData = new ArrayList();
        ArrayList _arrTargetInfo = new ArrayList();
        ArrayList _arrTargetItem = new ArrayList();
        ArrayList _arrTargetSubData = new ArrayList();
        
        BSpread bSpreadTemp=null; 
        
        string _sOperation = string.Empty;
        string _sTargetOperation = string.Empty;
        string _sOperationDesc = string.Empty;
        string _sTargetOperationDesc = string.Empty;
        
        public MultiDataUC()
        {
            InitializeComponent();
            InitializePage();
        }



        #region ::: PageLoad & Initialize

        public void InitializePage()
        {
            this._ComUtil = new CommonUtility();
            this._bSpreadUtil = new BSpreadUtility();
            this._lang = MultiLanguageHandler.getInstance();
            this._wsSPC = new eSPCWebService.eSPCWebService();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
            
            this.InitializeLayout();
            this.InitializeDataButton();            
            this.InitializeBSpread();
       
        }

   
        public void InitializeLayout()
        {
            this.bSpread1.ActiveSheet.FrozenColumnCount = 0;
            this.bSpread1.ActiveSheet.FrozenRowCount = 0;
            this.bTitlePanel1.Title = this._lang.GetVariable(Definition.TITLE_KEY_PROBE_DATA);
        }

        public void InitializeDataButton()
        {
            this._Initialization.InitializeButtonList(this.bButtonList1, ref this.bSpread1, Definition.PAGE_KEY_SPC_MULTIDATA_UC, Definition.BUTTONLIST_KEY_MULTIDATA, this.sessionData);            
        }

        public void InitializeBSpread()
        {
            this.bSpread1.ClearHead();
            this.bSpread1.AddHeadComplete();           
        }

        #endregion


        #region ::: Override Method

        public override void PageInit()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;
            this.KeyOfPage = this.GetType().FullName; // 필수 입력 (Key값을 받은 후 DC을 Active해주기 때문이다.          
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
            
            
            if (llstCondition[Definition.DynamicCondition_Search_key.FAB] != null)
            {
                _sFab = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.FAB]);
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
                

            if (llstCondition[Definition.DynamicCondition_Search_key.TYPE] != null)
            {
                string _sType  = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.TYPE]);    
                if(_sType.Equals("1"))  _bProb=true;                                                           
            }else
            {               
                MSGHandler.DisplayMessage(MSGType.Warning, "선택된 Step별 Data Mapping 정보가 없습니다.");
                return;            
            }            
            
            if (llstCondition[Definition.DynamicCondition_Search_key.OPERATION] != null)
            {
                this._sOperation = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.OPERATION]);
                this._sOperationDesc = DataUtil.GetDisplayData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.OPERATION]);
            }
            
            
            if(string.IsNullOrEmpty(this._sOperation))
            {
                MSGHandler.DisplayMessage(MSGType.Warning, "기준 Step이 없습니다.");
                return;              
            }

            if (llstCondition[Definition.DynamicCondition_Search_key.PROBE] != null)
            {
              string _sProb = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PROBE]);
              this._bProb = !string.IsNullOrEmpty(_sProb) ? (_sProb=="1" ?true :false) : false;
            }
                
            if (llstCondition[Definition.DynamicCondition_Search_key.INFORMATION] != null)
                this._arrInfo = CommonPageUtil.GetConditionKeyDataListArr((DataTable)llstCondition[Definition.DynamicCondition_Search_key.INFORMATION], Definition.DynamicCondition_Condition_key.VALUEDATA);

            if (llstCondition[Definition.DynamicCondition_Search_key.ITEM] != null)
                this._arrItem = CommonPageUtil.GetConditionKeyDataListArr((DataTable)llstCondition[Definition.DynamicCondition_Search_key.ITEM], Definition.DynamicCondition_Condition_key.VALUEDATA);

            if (llstCondition[Definition.DynamicCondition_Search_key.SUBDATA] != null)
                this._arrSubData = CommonPageUtil.GetConditionKeyDataListArr((DataTable)llstCondition[Definition.DynamicCondition_Search_key.SUBDATA], Definition.DynamicCondition_Condition_key.VALUEDATA);
                
            
            if (llstCondition[Definition.DynamicCondition_Search_key.TARGET] != null)
            {                        
                this._sTargetOperation = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.TARGET]);
                this._sTargetOperationDesc = DataUtil.GetDisplayData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.TARGET]);
                
            }

            
            if (llstCondition[Definition.DynamicCondition_Search_key.TARGET_PROBE] != null)
            {
                string _sTartProb = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.TARGET_PROBE]);
                this._bTartProb = !string.IsNullOrEmpty(_sTartProb) ? (_sTartProb == "1" ? true : false) : false;
            }

            if (llstCondition[Definition.DynamicCondition_Search_key.TARGET_INFORMATION] != null)
                this._arrTargetInfo = CommonPageUtil.GetConditionKeyDataListArr((DataTable)llstCondition[Definition.DynamicCondition_Search_key.TARGET_INFORMATION], Definition.DynamicCondition_Condition_key.VALUEDATA);

            if (llstCondition[Definition.DynamicCondition_Search_key.TARGET_ITEM] != null)
                this._arrTargetItem = CommonPageUtil.GetConditionKeyDataListArr((DataTable)llstCondition[Definition.DynamicCondition_Search_key.TARGET_ITEM], Definition.DynamicCondition_Condition_key.VALUEDATA);

            if (llstCondition[Definition.DynamicCondition_Search_key.TARGET_SUBDATA] != null)
                this._arrTargetSubData = CommonPageUtil.GetConditionKeyDataListArr((DataTable)llstCondition[Definition.DynamicCondition_Search_key.TARGET_SUBDATA], Definition.DynamicCondition_Condition_key.VALUEDATA);

            if (llstCondition[Definition.DynamicCondition_Search_key.SORTING_KEY] != null)
                this._arrSortingKey = CommonPageUtil.GetConditionKeyDataListArr((DataTable)llstCondition[Definition.DynamicCondition_Search_key.SORTING_KEY], Definition.DynamicCondition_Condition_key.VALUEDATA);


            if (this._arrSortingKey.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_SELECT_OBJECT"), "Sorting Key"));
                return;
            }

                                                                                                                                           
            PROC_DataBinding();
        }

        #endregion


        #region User Defined

        private void PROC_DataBinding()
        {
            DataSet ds = null;
            DataTableGroupBy dtGroupBy = null;
            string sFieldList = string.Empty;
            string sGroupBy = string.Empty;
            string sFilter = string.Empty;
            LinkedList _llstCol = new LinkedList();
            DataRow nRow = null;
            DataTable dtResult = null;
            DataTable dtBasis = null;
            DataTable dtTarget = null;
            try
            {
                this.MsgShow(_lang.GetVariable("RMS_PROGRESS_SEARCH"));
                dtGroupBy = new DataTableGroupBy();
                CreateChartDataTable _chartData = new CreateChartDataTable();
                LinkedList _llstWhere = new LinkedList();

                //Table 생성   
                dtResult = new DataTable();
                CreateTable(dtResult);

                //기준Step                
                string sParam = "'" + CommonPageUtil.GetConditionKeyArrayList(this._arrItem).Replace(",", "','") + "'";
                DataSet dsBasis = GetResultDataSet(this._bProb, sParam, this._sOperation, this._arrInfo, this._arrSubData);
                if (!DataUtil.IsNullOrEmptyDataSet(dsBasis))
                {
                    if (!this._bProb)
                    {
                        _llstCol = GetQueryColumn(_bProb);
                        if (this._arrSortingKey.Contains(Definition.CHART_COLUMN.MOCVDNAME))
                            dtBasis = CommonPageUtil.CLOBnBLOBParsing(dsBasis, _llstWhere, false, true);
                        else
                            dtBasis = CommonPageUtil.CLOBnBLOBParsing(dsBasis, _llstWhere, false, false);

                        _llstCol.Clear();
                        for (int i = 0; i < this._arrSubData.Count; i++)
                            _llstCol.Add(this._arrSubData[i].ToString(), "1");

                        dtBasis = _chartData.GetMakeDataTable(dtBasis, _llstCol);
                    }
                    else
                        dtBasis = dsBasis.Tables[0];
                }

                //Target
                if (!string.IsNullOrEmpty(_sTargetOperation))
                {
                    string sParamTarget = "'" + CommonPageUtil.GetConditionKeyArrayList(this._arrTargetItem).Replace(",", "','") + "'";
                    DataSet dsTarget = GetResultDataSet(this._bTartProb, sParamTarget, this._sTargetOperation, this._arrTargetInfo, this._arrTargetSubData);
                    if (!DataUtil.IsNullOrEmptyDataSet(dsTarget))
                    {
                        if (!this._bTartProb)
                        {
                            if (this._arrSortingKey.Contains(Definition.CHART_COLUMN.MOCVDNAME))
                                dtTarget = CommonPageUtil.CLOBnBLOBParsing(dsTarget, _llstWhere, false, true);
                            else
                                dtTarget = CommonPageUtil.CLOBnBLOBParsing(dsTarget, _llstWhere, false, false);

                            _llstCol.Clear();
                            for (int i = 0; i < this._arrTargetSubData.Count; i++)
                                _llstCol.Add(this._arrTargetSubData[i].ToString(), "1");

                            dtTarget = _chartData.GetMakeDataTable(dtTarget, _llstCol);
                        }
                        else
                            dtTarget = dsTarget.Tables[0];
                    }

                }


                this.bSpread1.DataSource = null;
                if (!DataUtil.IsNullOrEmptyDataTable(dtBasis))
                    this.MakeCrateTable(dtBasis, dtTarget, this._bProb, this._bTartProb, dtResult);                                 
                else
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                }

                this.bSpread1.DataSource = dtResult;
                this.bSpread1.ActiveSheet.ColumnHeader.Cells[0, this._arrSortingKey.Count].Text = this._sOperationDesc;
                if (!string.IsNullOrEmpty(this._sTargetOperation))
                    this.bSpread1.ActiveSheet.ColumnHeader.Cells[0, this.bSpread1.ActiveSheet.ColumnCount - (this._arrTargetItem.Count * this._arrTargetSubData.Count) - this._arrTargetInfo.Count].Text = this._sTargetOperationDesc;

            }   
            catch (Exception ex)
            {
                this.MsgClose();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                if (ds != null) ds.Dispose();
                this.MsgClose();
            }

        }

        private DataSet GetResultDataSet(bool _pProb, string _pParamItem, string _pOperationID, ArrayList _arrInfo, ArrayList _arrSubData)
        {
            DataSet _dsResult = null;                        
            if (_pProb)
            {
                                
                LinkedList _llstCol = new LinkedList();             
                string sCol = string.Empty;
                string sGroup = string.Empty;                
                for (int i = 0; i < this._arrSortingKey.Count; i++)
                {
                    if (this._arrSortingKey[i].ToString().Equals("SAMPLE"))
                     continue;
                    else if (this._arrSortingKey[i].ToString().Equals("WORKDATE"))
                    {
                        sCol += "TO_CHAR(EVENT_DTTS,'yyyy-MM-dd') AS WORKDATE,";
                        sGroup += "TO_CHAR(EVENT_DTTS,'yyyy-MM-dd'),";
                    }
                    else
                    {
                        sCol += this._arrSortingKey[i].ToString() + ",";
                        sGroup += this._arrSortingKey[i].ToString() + ",";
                    }
                }                
                for (int i = 0; i < _arrInfo.Count; i++)
                {
                    sCol += _arrInfo[i].ToString() + ",";
                    sGroup += _arrInfo[i].ToString() + ",";                    
                }

                for (int i = 0; i < _arrSubData.Count; i++)
                {
                    if(_arrSubData[i].ToString().Equals(Definition.CHART_COLUMN.USL) || _arrSubData[i].ToString().Equals(Definition.CHART_COLUMN.LSL))
                    {
                        sCol += _arrSubData[i].ToString() + ",";
                        sGroup += _arrSubData[i].ToString() + ",";
                    }
                }

                if (!string.IsNullOrEmpty(sCol))
                {
                    sCol = sCol.Substring(0, sCol.Length - 1);
                    sGroup = sGroup.Substring(0, sGroup.Length - 1);
                }
                                                              
                this._llstData.Clear();
                this._llstData.Add(Definition.DynamicCondition_Condition_key.FAB, this._sFab);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.START_DTTS, this._sStartTime);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.END_DTTS, this._sEndTime);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.PARAM, _pParamItem);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, _pOperationID);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.COLUMN_LIST, sCol);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.GROUP, sGroup);
                _dsResult = this._wsSPC.GetMultiData(this._llstData.GetSerialData());
               
            }
            else
            {

                string sModelConfigRawID = string.Empty;
                this._llstData.Clear();
                
                this._llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this._LineRawID);
                if (!string.IsNullOrEmpty(this._AreaRawID))
                    this._llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this._AreaRawID);

                if (!string.IsNullOrEmpty(this._EQPModel))
                    this._llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this._EQPModel);
            
            
                this._llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this._ParamType);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, _pParamItem);                                                
                this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, CommonPageUtil.GetConCatString(_pOperationID));
                DataSet ds = _wsSPC.GetSPCModelConfigSearch(_llstData.GetSerialData());
                if (!DataUtil.IsNullOrEmptyDataSet(ds))
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        sModelConfigRawID += ds.Tables[0].Rows[i][COLUMN.MODEL_CONFIG_RAWID].ToString() + ",";

                    if (ds != null) ds.Dispose();
                    if (!string.IsNullOrEmpty(sModelConfigRawID))
                        sModelConfigRawID = sModelConfigRawID.Substring(0, sModelConfigRawID.Length - 1);
                }
                
                if(ds !=null)ds.Dispose();
                                                
                this._llstData.Clear();
                this._llstData.Add(Definition.DynamicCondition_Condition_key.START_DTTS, this._sStartTime);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.END_DTTS, this._sEndTime);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, sModelConfigRawID);
                _dsResult = this._wsSPC.GetSPCControlChartData(this._llstData.GetSerialData());
            }        
            
            return _dsResult;
        }


        private void AddColumn(ArrayList _arrInfo, ArrayList _arrItem, ArrayList _arrSubData, string _pOperation, DataTable _dtResult,string _ColType)
        {
            int iCol = 0;              
            string sCol = string.Empty;            
            string sColName = string.Empty;
            
            bSpread1.ActiveSheet.Columns.Add(this.bSpread1.ActiveSheet.Columns.Count, _arrInfo.Count + (_arrSubData.Count * _arrItem.Count));
            iCol = bSpread1.ActiveSheet.Columns.Count - (_arrInfo.Count + (_arrSubData.Count * _arrItem.Count) );            
            bSpread1.ActiveSheet.ColumnHeader.Cells[0, iCol].ColumnSpan = _arrInfo.Count + (_arrSubData.Count * _arrItem.Count);
            bSpread1.ActiveSheet.ColumnHeader.Cells[0, iCol].Text = _pOperation;
                       
            for (int j = 0; j < _arrInfo.Count; j++)
            {
                sCol = _arrInfo[j].ToString();
                sColName =_pOperation + "_" + sCol + "_" + _ColType;
                          
                iCol = bSpread1.ActiveSheet.Columns.Count - (_arrInfo.Count + (_arrSubData.Count * _arrItem.Count) + j);
                bSpread1.AddHead(iCol, this._lang.GetVariable(sCol), sColName, 120, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, true);
                _dtResult.Columns.Add(sColName, typeof(string));

                bSpread1.ActiveSheet.ColumnHeader.Cells[1, iCol].RowSpan = 2;
                bSpread1.ActiveSheet.ColumnHeader.Cells[1, iCol].Text = this._lang.GetVariable(sCol);
                bSpread1.ActiveSheet.ColumnHeader.Cells[0, iCol].Text = _pOperation;   
            }
                               
            for (int i = 0; i < _arrItem.Count; i++)
            {
                string sItem = _arrItem[i].ToString();                                                           
                iCol = bSpread1.ActiveSheet.Columns.Count - ((_arrSubData.Count * (_arrItem.Count-i)) );                
                                               
                for (int j = 0; j < _arrSubData.Count; j++)
                {
                    sCol = _arrSubData[j].ToString();
                    sColName = sItem + "_" + sCol + "_" + _ColType;      
                             
                    if(j>0)
                        iCol = bSpread1.ActiveSheet.Columns.Count - (_arrSubData.Count * (_arrItem.Count - i)) + j;

                    if(sCol.Equals("QTY"))
                    bSpread1.AddHead(iCol, this._lang.GetVariable(sCol), sColName, 120, 20, null, null, null, ColumnAttribute.Null, ColumnType.Int, null, null, null, false, true);
                    else
                        bSpread1.AddHead(iCol, this._lang.GetVariable(sCol), sColName, 120, 20, null, null, null, ColumnAttribute.Null, ColumnType.Double, null, null, null, false, true);
                        
                    if (j == 0)
                    {
                        bSpread1.ActiveSheet.ColumnHeader.Cells[1, iCol].ColumnSpan = _arrSubData.Count;
                        bSpread1.ActiveSheet.ColumnHeader.Cells[1, iCol].Text = sItem;
                    }
                    bSpread1.ActiveSheet.ColumnHeader.Cells[2, iCol].Text = this._lang.GetVariable(sCol);
                    _dtResult.Columns.Add(sColName, typeof(double));                                       
                }                                                
            }
            
        }

        private void CreateTable(DataTable _dtResult)
        {            
            string sCol = string.Empty;
            int iCol = 0;
                                                                     
            bSpread1.ClearHead();
            bSpread1.ActiveSheet.ColumnCount = 0;
            bSpread1.ActiveSheet.RowCount = 0;
            bSpread1.ActiveSheet.ColumnHeader.RowCount = 3;
                                                      
            for (int i = 0; i < this._arrSortingKey.Count; i++)
            {
                string sColName = _arrSortingKey[i].ToString();
                _dtResult.Columns.Add(sColName, typeof(string));

                bSpread1.ActiveSheet.Columns.Add(bSpread1.ActiveSheet.Columns.Count, 1);
                iCol = bSpread1.ActiveSheet.Columns.Count - 1;
                bSpread1.ActiveSheet.ColumnHeader.Cells[0, iCol].RowSpan = 3;
                bSpread1.ActiveSheet.ColumnHeader.Cells[0, iCol].Text = this._lang.GetVariable(sColName);
                bSpread1.AddHead(iCol, this._lang.GetVariable(sColName), sColName, 120, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, true);
                                                     
            }
            AddColumn(this._arrInfo, this._arrItem, this._arrSubData, this._sOperation, _dtResult,"B");
            
            if(!string.IsNullOrEmpty(this._sTargetOperation)) 
            AddColumn(this._arrTargetInfo, this._arrTargetItem, this._arrTargetSubData, this._sTargetOperation, _dtResult,"T");                 
        }


        private LinkedList GetQueryColumn(bool _pProbe)
        {                       
            LinkedList _llstColumn = new LinkedList();                        
            List<string> _lstCol = new List<string>();
            List<string> _lstGroup = new List<string>();
            
            string sCol = string.Empty;
            string sGroup = string.Empty;
            if(_pProbe)
            {
                for (int i = 0; i < this._arrSortingKey.Count; i++)
                {
                   
                    if (this._arrSortingKey[i].ToString().Equals("WORKDATE"))
                    {
                        sCol += "TO_CHAR(EVENT_DTTS,'yyyy-MM-dd') AS WORKDATE,";
                        sGroup += "TO_CHAR(EVENT_DTTS,'yyyy-MM-dd'),";                        
                    }                    
                    else
                    {
                        sCol += this._arrSortingKey[i].ToString() + ",";
                        sGroup += this._arrSortingKey[i].ToString() + ",";                                 
                    }                    
                }   
                
                if(!string.IsNullOrEmpty( sCol))
                {
                    sCol = sCol.Substring(0,sCol.Length-1);
                    sGroup = sGroup.Substring(0, sGroup.Length - 1);                
                }                           
                _llstColumn.Add(Definition.DynamicCondition_Condition_key.COLUMN_LIST, sCol);
                _llstColumn.Add(Definition.DynamicCondition_Condition_key.GROUP, sGroup);
            }                   
            return _llstColumn;            
        }
        
        
        #region ::: MakeTable 
        
        private void MakeCrateTable(DataTable dtBasis, DataTable dtTarget, bool bProbe, bool bTargetProbe, DataTable dtResult)
        {
            DataTable dtGroup = null;
            StringBuilder sbSelect = null;
            DataRow nRow = null;
            string sCol = string.Empty;
            
            try
            {  
                if(bProbe)
                    dtGroup = GetProbeDataTableGroup(dtBasis);
                else
                    dtGroup = GetMeasDataTableGroup(dtBasis);
                                
                foreach (DataRow dr in dtGroup.Rows)
                {
                    nRow = dtResult.NewRow();
                    sbSelect = new StringBuilder();
                    sbSelect.Append(" 1=1");
                    for (int j = 0; j < this._arrSortingKey.Count; j++)
                    {
                        sCol = this._arrSortingKey[j].ToString();
                                                                                                   
                        if (dtBasis.Columns.Contains(sCol))
                        {
                            if (string.IsNullOrEmpty(dr[sCol].ToString())) continue;                                                 
                            nRow[sCol] = dr[sCol];
                            sbSelect.AppendFormat(" AND {0} = '{1}'", sCol, nRow[sCol].ToString());
                        }                       
                    }

                    string sItem = string.Empty;
                    if (bProbe)
                        ResetRow(nRow, dtBasis, this._arrItem, this._arrInfo, this._arrSubData, "ITEM", "B", this._sOperation, sbSelect);
                    else
                    {
                        sItem = this._arrItem[0].ToString();
                        MeasResetRow(sbSelect, sItem, dtBasis, nRow, dr, this._arrInfo, this._arrSubData, "B", this._sOperation);
                    }

                    if (this._arrTargetItem.Count > 0 && !DataUtil.IsNullOrEmptyDataTable(dtTarget))
                    {
                        if(bTargetProbe)                        
                            ResetRow(nRow, dtTarget, this._arrTargetItem, this._arrTargetInfo, this._arrTargetSubData, "ITEM", "T", this._sTargetOperation, sbSelect);                            
                        else
                        {
                            sItem = this._arrTargetItem[0].ToString();
                            MeasResetRow(sbSelect, sItem, dtTarget, nRow, dr, this._arrTargetInfo, this._arrTargetSubData, "T", this._sTargetOperation);
                        }
                    }                                                                                                                                                                                                       
                    dtResult.Rows.Add(nRow);                                                
                }
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
            finally
            {               
            }                        
        }
        private DataTable GetProbeDataTableGroup(DataTable dtBasis)
        {            
            string sCol = string.Empty;
            string sColumnList = string.Empty;                
            DataTable dtGroup =null;
            DataTableGroupBy dtGroupBy = new DataTableGroupBy();          
                                                
            for (int i = 0; i < this._arrSortingKey.Count; i++)
            {
                if(_arrSortingKey[i].ToString().Equals("SAMPLE")) continue;
                sColumnList += _arrSortingKey[i].ToString() + ",";
            }
                
            if(!string.IsNullOrEmpty(sColumnList))  
            sColumnList = sColumnList.Substring(0,sColumnList.Length-1);                

            string sField = string.Format("{0},{1},sum(QTY) SAMPLE", sColumnList, Definition.CHART_COLUMN.OPERATION_ID);
            string sGroup = string.Format("{0},{1}", sColumnList, Definition.CHART_COLUMN.OPERATION_ID);
            string sRowFilter = string.Empty;
             
            return dtGroupBy.SelectGroupByInto("GROUP", dtBasis, sField, sRowFilter, sGroup);                   
        }

        private DataTable GetMeasDataTableGroup(DataTable dtBasis)
        {
            DataRow nRow= null;
            string sCol = string.Empty;
            DataTable dtGroup = null;
            DataTableGroupBy dtGroupBy = new DataTableGroupBy();           
           
            string sField = string.Empty;
            string sGroup = string.Empty;
            string sColumnList = string.Empty;
            string sRowFilter = string.Empty; 
                                                                        
             for(int i=0;i<this._arrSortingKey.Count;i++)   
             {         
               sCol = _arrSortingKey[i].ToString();                   
               if(dtBasis.Columns.Contains(sCol))                   
                   sColumnList += sCol + ",";                   
             }
             
             sColumnList +=Definition.CHART_COLUMN.OPERATION_ID+",";                     
             for(int i=0; i<this._arrInfo.Count; i++)
             {
                sCol= this._arrInfo[i].ToString();
                if(dtBasis.Columns.Contains(sCol) && !this._arrSortingKey.Contains(sCol))
                sColumnList +=sCol+",";       
             }
            
             for(int i=0; i<this._arrTargetInfo.Count; i++)
             {
                sCol= this._arrTargetInfo[i].ToString();
                if (dtBasis.Columns.Contains(sCol) && !this._arrInfo.Contains(sCol))
                sColumnList +=sCol+",";       
             }
             
             if(!string.IsNullOrEmpty(sColumnList))
             sColumnList = sColumnList.Substring(0,sColumnList.Length-1);
          
            sField = sColumnList;
            sGroup = sField;
            sRowFilter = string.Empty;
             
            return dtGroupBy.SelectGroupByInto("GROUP", dtBasis, sField, sRowFilter, sGroup);     
        }

        #endregion 

        #region  ::: ResetRow

        private void ResetRow(DataRow nRow, DataTable dtBasis, ArrayList arrItem, ArrayList arrInfo, ArrayList arrSubData, string _pParamItem, string _ColType, string _pOperation, StringBuilder sbSelect)
        {
            string sCol = string.Empty;
            string sColName = string.Empty;
            CommonSPCStat comSPCStat = new CommonSPCStat();

            for (int i = 0; i < arrItem.Count; i++)
            {
                string sItem = arrItem[i].ToString();                
                DataRow[] drSelect = dtBasis.Select(string.Format("{0} AND {1} = '{2}'", sbSelect.ToString(), _pParamItem, sItem));

                foreach (DataRow subRow in drSelect)
                {
                    for (int j = 0; j < arrInfo.Count; j++)
                    {
                        sCol = arrInfo[j].ToString();
                        nRow[_pOperation + "_" + sCol + "_" + _ColType] = subRow[sCol];
                    }

                    for (int j = 0; j < arrSubData.Count; j++)
                    {
                        sCol = arrSubData[j].ToString();
                        sColName = sItem + "_" + sCol + "_" + _ColType;

                        if (sCol.Equals(Definition.CHART_COLUMN.STDDEV))
                        {
                            double dSum2 = string.IsNullOrEmpty(subRow["SUM2"].ToString())? 0 : double.Parse(subRow["SUM2"].ToString());
                            double dSum = string.IsNullOrEmpty(subRow["SUM"].ToString()) ? 0 : double.Parse(subRow["SUM"].ToString());
                            int iQty = string.IsNullOrEmpty(subRow["QTY"].ToString()) ? 0 : int.Parse(subRow["QTY"].ToString());                                                        
                            nRow[sColName] = Stat.std(dSum2, dSum, iQty);                        
                        }
                        else
                            nRow[sColName] = subRow[sCol].ToString();
                    }
                }
            }

        }
        
        private void MeasResetRow(StringBuilder sbSelect, string _Item, DataTable dtBasis, DataRow nRow, DataRow dr, ArrayList _arrInfo, ArrayList _arrSubData, string _ColType, string _pOperation)
        {
            string sCol =string.Empty;
            string sColName = string.Empty;
            CommonSPCStat comSPCStat = new CommonSPCStat();
                                     
            DataTable dt = DataUtil.DataTableImportRow(dtBasis.Select(sbSelect.ToString()));
            List<double> listRawData = comSPCStat.AddDataList(dt);
            if (listRawData.Count == 0) return;
            comSPCStat.CalcMulti(listRawData.ToArray());

            if(this._arrSortingKey.Contains(Definition.CHART_COLUMN.SAMPLE))
                nRow[Definition.CHART_COLUMN.SAMPLE] = listRawData.Count;
                        
            for (int j = 0; j < _arrInfo.Count; j++)
            {
                sCol = _arrInfo[j].ToString();
                sColName = _pOperation + "_" + sCol + "_" + _ColType;
                
                if (sCol.Equals(Definition.CHART_COLUMN.SAMPLE))
                    nRow[sColName] = listRawData.Count;
                else
                    nRow[sColName] = dt.Rows[0][sCol];
            }

            for (int j = 0; j < _arrSubData.Count; j++)
            {
                sCol = _arrSubData[j].ToString();
                sColName = _Item + "_" + sCol + "_" + _ColType;
                
                if (sCol.Equals(Definition.CHART_COLUMN.STDDEV))
                    nRow[sColName] = comSPCStat.stddev;
                else if (sCol.Equals(Definition.CHART_COLUMN.MEAN))
                    nRow[sColName] = comSPCStat.mean;
                else if (sCol.Equals(Definition.CHART_COLUMN.MIN))
                    nRow[sColName] = comSPCStat.min;
                else if (sCol.Equals(Definition.CHART_COLUMN.MAX))
                    nRow[sColName] = comSPCStat.max;
                else if (sCol.Equals(Definition.CHART_COLUMN.RANGE))
                    nRow[sColName] = comSPCStat.range;
                else
                    nRow[sColName] = dt.Rows[0][sCol];
            }        
        }       
        
        #endregion 
          
        #endregion
        
        
        #region Events
        private void bButtonList1_ButtonClick(string name)
        {
            try
            {                
                if (name.ToUpper() == Definition.ButtonKey.EXPORT)
                {
                    if (this.bSpread1.ActiveSheet.RowCount > 0)                    
                        this.bSpread1.Export(false);                    
                    else                    
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_NO_SEARCH_DATA));                    
                }
               
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }
        
        #endregion 
    }
}
