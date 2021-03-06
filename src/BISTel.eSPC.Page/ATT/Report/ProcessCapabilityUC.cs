﻿using System;
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
using BISTel.PeakPerformance.Statistics.Application.Common;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;


using BISTel.PeakPerformance.Client.DataAsyncHandler;
using BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar;


using BISTel.eSPC.Common.ATT;
using BISTel.eSPC.Page.ATT.Common;

namespace BISTel.eSPC.Page.ATT.Report
{
	public partial class ProcessCapabilityUC : BasePageUCtrl
	{

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        MultiLanguageHandler _lang;
        LinkedList _llstSearchCondition = new LinkedList();
        List<string> _lstLot = new List<string>();
        List<string> _lstRawColumn=null;                
        SortedList _sortHeader = new SortedList();        
        SortedList _sortHeaderLabel = new SortedList();
         StringBuilder sbSort=new StringBuilder();
        ArrayList arrDynamicColKey = new ArrayList();

        BISTel.eSPC.Common.BSpreadUtility _bSpreadUtil = new BSpreadUtility();
        BISTel.eSPC.Common.CommonUtility _ComUtil = null;
                
        string _AreaRawID = string.Empty;
        string _LineRawid = string.Empty; //SPC-1292, KBLEE
        string _Area = string.Empty;
        string _Line = string.Empty;
        string _ParamTypeCD =string.Empty;
        string sStartTime = string.Empty;
        string sEndTime = string.Empty;
        //string sParamType = string.Empty;
        string sPeriod_ppk = Definition.PERIOD_PPK.NONE;
        bool bShowOOCCount = false;

        string _GroupName = string.Empty; //SPC-1292, KBLEE

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
        
		
		public ProcessCapabilityUC()
		{
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

            DataTable dtSite = (DataTable) llstCondition[Definition.CONDITION_SEARCH_KEY_SITE];
			string site = "";
			if ( dtSite != null )
				site = dtSite.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString ();

			DataTable dtFab = (DataTable) llstCondition[Definition.CONDITION_SEARCH_KEY_FAB];
			string fab = "";
			if ( dtFab != null )
				fab = dtFab.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString ();

            if (!llstCondition.Contains(Definition.DynamicCondition_Search_key.AREA))
            {
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SELECT_CONDITION_DATA));
                return;
            }

            if (llstCondition[Definition.DynamicCondition_Search_key.LINE] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.LINE];
                _Line = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
                _LineRawid = dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString(); //SPC-1292, KBLEE
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
            }
            

            string strArea = _ComUtil.GetConditionString((DataTable)llstCondition[Definition.DynamicCondition_Search_key.AREA], Definition.DynamicCondition_Condition_key.AREA);
            _AreaRawID = ((DataTable)llstCondition[Definition.DynamicCondition_Search_key.AREA]).Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString(); //SPC-1292, KBLEE
            if (!string.IsNullOrEmpty(strArea))
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.AREA, strArea);

            if(!base.ApplyAuthory(this.bbtnList, site, fab, _Line, strArea.Trim('\'')))
            {
                MSGHandler.DisplayMessage ( MSGType.Warning, MSGHandler.GetMessage("GENERAL_NOT_ENOUGHT_SUFFICIENT"), null, null, true );
                this.InitializePage();
				return;
            }

            if (llstCondition[Definition.CONDITION_KEY_GROUP_NAME] != null)
            {
                _GroupName = ((DataTable)llstCondition[Definition.CONDITION_KEY_GROUP_NAME]).Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();
            }

            if (llstCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM];
                sStartTime = CommonPageUtil.CalcStartDate(dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
            }
            if (llstCondition[Definition.DynamicCondition_Search_key.DATETIME_TO] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.DATETIME_TO];
                sEndTime = CommonPageUtil.CalcEndDate(dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
            }

            if(llstCondition[Definition.DynamicCondition_Search_key.DATETIME_FROM] != null
                && llstCondition[Definition.DynamicCondition_Search_key.DATETIME_TO] != null
                && DateTime.Parse(sStartTime) > DateTime.Parse(sEndTime))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHECK_PERIOD", null, null);
                return;
            }

            this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, sStartTime);
            this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, sEndTime);

            //sParamType = _ComUtil.GetConditionData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE], Definition.DynamicCondition_Condition_key.VALUEDATA);
            //if (!string.IsNullOrEmpty(sParamType))
            //    this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, sParamType);


            string strModelRawID = _ComUtil.GetConditionString((DataTable)llstCondition[Definition.DynamicCondition_Search_key.SPCMODEL], Definition.DynamicCondition_Condition_key.VALUEDATA);
            if (!string.IsNullOrEmpty(strModelRawID))
            {
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_RAWID, strModelRawID);
            }
            else
            {
                //SPC-899 Model선택하지 않은상태에서 Search시 message출력 by Louis
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                return;
            }



            this.sPeriod_ppk = DCUtil.GetValueData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PERIOD_PPK]);
            if(!string.IsNullOrEmpty(this.sPeriod_ppk))
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PERIOD_PPK, sPeriod_ppk);        
                                                                 
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
            this.bShowOOCCount = this._Initialization.GetPpkShowOOC(Definition.PAGE_KEY_SPC_ATT_PROCESS_CAPABILITY_UC);
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
            _sortHeader = this._Initialization.InitializeButtonList(this.bbtnList, ref bsprData, Definition.PAGE_KEY_SPC_ATT_PROCESS_CAPABILITY_UC, Definition.BUTTONLIST_KEY_ATT_PROCESS_CAPABILITY, this.sessionData);
            this.FunctionName = Definition.FUNC_KEY_SPC_ATT_PROCESS_CAPABILITY;
            this.ApplyAuthory(this.bbtnList);
        }

        public void InitializeBSpread()
        {
            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
        }

        #endregion

        #region ::: User Defined Method.


        private void SpreadHeader()
        {
            int iCount = 0;            
            this._sortHeader.Clear();
            this._sortHeaderLabel.Clear();
            sbSort = new StringBuilder();
            
            _sortHeader.Add(iCount++, "_SELECT");
            _sortHeader.Add(iCount++, BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID);
            _sortHeader.Add(iCount++, BISTel.eSPC.Common.COLUMN.COMPLEX_YN);
            _sortHeader.Add(iCount++, BISTel.eSPC.Common.COLUMN.MAIN_YN);
            _sortHeader.Add(iCount++, BISTel.eSPC.Common.COLUMN.DEFAULT_CHART_LIST);            
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.AREA);            
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PARAM_ALIAS);
            _sortHeader.Add(iCount++, BISTel.eSPC.Common.COLUMN.PARAM_TYPE_CD);

            _sortHeaderLabel.Add("_SELECT", Definition.SpreadHeaderColKey.V_SELECT);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.AREA, Definition.SpreadHeaderColKey.AREA);            
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PARAM_ALIAS, Definition.SpreadHeaderColKey.PARAM_ALIAS);

            sbSort.Append(Definition.CHART_COLUMN.AREA);
            sbSort.Append(","+Definition.CHART_COLUMN.PARAM_ALIAS);
            for (int i = 0; i < this.arrDynamicColKey.Count; i++)
            {
                string sCol = this.arrDynamicColKey[i].ToString();
                _sortHeader.Add(iCount++, sCol);
                if(sCol==Definition.CHART_COLUMN.OPERATION_DESC)
                    _sortHeaderLabel.Add(sCol, Definition.SpreadHeaderColKey.OPERATION_DESC);
                else
                    _sortHeaderLabel.Add(sCol, "SPC_"+sCol);

                sbSort.Append("," + sCol);                   
            }
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PERIOD);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.SPEC);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.LSL);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.USL);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.AVG);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PPK);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PP);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PPU);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.PPL);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.SUM);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.SUM_SQUARED);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.LOT_QTY);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.SAMPLE_QTY);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.CPK_AVG);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.CPK_STDDEV);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.CPK);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.CP);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.CPU);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.CPL);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.CPK_SUM);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.CPK_SUM_SQUARED);
            _sortHeader.Add(iCount++, Definition.CHART_COLUMN.CPK_SAMPLE_QTY);
            if (this.bShowOOCCount)
            {
                _sortHeader.Add(iCount++, Definition.CHART_COLUMN.CA);
                _sortHeader.Add(iCount++, Definition.CHART_COLUMN.OOC_COUNT);
                _sortHeader.Add(iCount++, Definition.CHART_COLUMN.OOS_COUNT);
            }

            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PERIOD, Definition.SpreadHeaderColKey.PERIOD);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.SPEC, Definition.SpreadHeaderColKey.SPEC);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.LSL, Definition.SpreadHeaderColKey.LSL);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.USL, Definition.SpreadHeaderColKey.USL);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.AVG, Definition.SpreadHeaderColKey.AVG);                  
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PPK, Definition.SpreadHeaderColKey.PPK);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PP, Definition.SpreadHeaderColKey.PP);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PPU, Definition.SpreadHeaderColKey.PPU);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.PPL, Definition.SpreadHeaderColKey.PPL);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.SUM, Definition.SpreadHeaderColKey.SUM);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.SUM_SQUARED, Definition.SpreadHeaderColKey.SUM_SQUARED);            
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.LOT_QTY, Definition.SpreadHeaderColKey.LOT_QTY);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.SAMPLE_QTY, Definition.SpreadHeaderColKey.SAMPLE_QTY);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.CPK_AVG, Definition.SpreadHeaderColKey.CPK_AVG);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.CPK_STDDEV, Definition.SpreadHeaderColKey.CPK_STDDEV);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.CPK, Definition.SpreadHeaderColKey.CPK);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.CP, Definition.SpreadHeaderColKey.CP);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.CPU, Definition.SpreadHeaderColKey.CPU);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.CPL, Definition.SpreadHeaderColKey.CPL);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.CPK_SUM, Definition.SpreadHeaderColKey.CPK_SUM);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.CPK_SUM_SQUARED, Definition.SpreadHeaderColKey.CPK_SUM_SQUARED);
            _sortHeaderLabel.Add(Definition.CHART_COLUMN.CPK_SAMPLE_QTY, Definition.SpreadHeaderColKey.CPK_SAMPLE_QTY);
            if (this.bShowOOCCount)
            {
                _sortHeaderLabel.Add(Definition.CHART_COLUMN.CA, Definition.SpreadHeaderColKey.CA);
                _sortHeaderLabel.Add(Definition.CHART_COLUMN.OOC_COUNT, Definition.SpreadHeaderColKey.OOC_COUNT);
                _sortHeaderLabel.Add(Definition.CHART_COLUMN.OOS_COUNT, Definition.SpreadHeaderColKey.OOS_COUNT);
            }
        }
        
        private void PROC_DataBinding()
        {   
            DataTable dtPpk = null;
            DataTable dtContext = null;            
            DataSet _ds =null;
            CommonSPCStat comSPCStat=null;                                             
            try
            {
                string sField = string.Empty;
                string sGroupBy = string.Empty;
                string sRowFilter = string.Empty;
                comSPCStat = new CommonSPCStat();
                arrDynamicColKey.Clear();
                this.bsprData.ActiveSheet.RowCount = 0;
                this.bsprData.ActiveSheet.ColumnCount = 0;
                this.bsprData.DataSource = null;

                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_wsSPC, "GetATTProcessCapabilityList", new object[] { _llstSearchCondition.GetSerialData() });

                EESProgressBar.CloseProgress(this);

                if (objDataSet != null)
                {
                    _ds = (DataSet)objDataSet;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                //this.MsgShow(COMMON_MSG.Query_Data);                
                // _ds = _wsSPC.GetProcessCapabilityList(_llstSearchCondition.GetSerialData());   
                if(!DataUtil.IsNullOrEmptyDataSet(_ds))
                {
                    EESProgressBar.ShowProgress(this, MSGHandler.GetMessage("PROCESS_LOADING_PAGE_DATA"), false);
                    for(int i=0; i<_ds.Tables.Count; i++)
                    {
                        if(_ds.Tables[i].TableName==BISTel.eSPC.Common.TABLE.PPK_TRX_SPC)
                            dtPpk = _ds.Tables[i];   
                        else if (_ds.Tables[i].TableName==BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC)
                            dtContext = _ds.Tables[i]; 
                    }
                    if (_ds != null) _ds.Dispose();               
                }                                                                                                                                            
                _llstSearchCondition.Clear();
                //_llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.OPERATION_TYPE, this.sParamType == "MET" ? "METROLOGY" : "PROCESSING");
                DataSet dsOperation = _wsSPC.GetOperationID(_llstSearchCondition.GetSerialData());
                                               
                if(dtPpk == null || dtPpk.Rows.Count == 0)
                {
                    this.MsgClose();
                    this.bsprData.ClearHead();
                    this.bsprData.ClearData();
                    MSGHandler.DisplayMessage(MSGType.Information,
                                              MSGHandler.GetMessage(Definition.MSG_KEY_NO_SEARCH_DATA));
                    return;
                }

                dtPpk.Columns.Add(Definition.CHART_COLUMN.STDDEV, typeof(string));
                dtPpk.Columns.Add(Definition.CHART_COLUMN.PPK, typeof(string));
                dtPpk.Columns.Add(Definition.CHART_COLUMN.PP, typeof(string));
                dtPpk.Columns.Add(Definition.CHART_COLUMN.PPU, typeof(string));
                dtPpk.Columns.Add(Definition.CHART_COLUMN.PPL, typeof(string));

                dtPpk.Columns.Add(Definition.CHART_COLUMN.CPK_STDDEV, typeof(string));
                dtPpk.Columns.Add(Definition.CHART_COLUMN.CPK, typeof(string));
                dtPpk.Columns.Add(Definition.CHART_COLUMN.CP, typeof(string));
                dtPpk.Columns.Add(Definition.CHART_COLUMN.CPU, typeof(string));
                dtPpk.Columns.Add(Definition.CHART_COLUMN.CPL, typeof(string));

                if (this.bShowOOCCount)
                {
                    dtPpk.Columns.Add(Definition.CHART_COLUMN.CA, typeof(string));
                }

                foreach (DataRow dr in dtPpk.Rows)
                {
                    /* Column Create : SPC Model Context List*/
                    string sModelConfigRawID = dr[BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID].ToString();
                    DataRow[] drSelect = dtContext.Select(string.Format(" MODEL_CONFIG_RAWID = {0}", sModelConfigRawID), "KEY_ORDER");
                    foreach (DataRow nRow in drSelect)
                    {
                        string sCol = nRow[BISTel.eSPC.Common.COLUMN.CONTEXT_KEY].ToString();
                        string sValue = nRow[BISTel.eSPC.Common.COLUMN.CONTEXT_VALUE].ToString();
                        if (!dtPpk.Columns.Contains(sCol))
                        {
                            dtPpk.Columns.Add(sCol, typeof(string));
                            arrDynamicColKey.Add(sCol);
                            if (sCol == Definition.CHART_COLUMN.OPERATION_ID)
                            {
                                dtPpk.Columns.Add(Definition.CHART_COLUMN.OPERATION_DESC, typeof(string));
                                arrDynamicColKey.Add(Definition.CHART_COLUMN.OPERATION_DESC);
                            }                                                                                            
                        }
                        dr[sCol] = sValue;
                        if (sCol == Definition.CHART_COLUMN.OPERATION_ID && sValue != Definition.VARIABLE.STAR)
                        {
                            DataRow[] drSelectOperation  = dsOperation.Tables[0].Select(string.Format("OPERATION_ID='{0}'", sValue));
                            dr[Definition.CHART_COLUMN.OPERATION_DESC] = drSelectOperation.Length > 0 ? drSelectOperation[0]["DESCRIPTION"].ToString() : null;
                        }
                    } 
                    
                    /*  PP 계산 */
                    //Louis ppk;/////-Data값 중에서 Usl, Lsl값이 Null인경우 발생
                    if ((dr[Definition.CHART_COLUMN.USL].ToString() != null && dr[Definition.CHART_COLUMN.USL].ToString() != "") ||
                        dr[Definition.CHART_COLUMN.LSL].ToString() != null && dr[Definition.CHART_COLUMN.LSL].ToString() != "")
                    {
                        double usl = double.Parse(dr[Definition.CHART_COLUMN.USL].ToString());
                        double lsl = double.Parse(dr[Definition.CHART_COLUMN.LSL].ToString());
                        double sum_squared = double.Parse(dr[Definition.CHART_COLUMN.SUM_SQUARED].ToString());
                        double avg = double.Parse(dr[Definition.CHART_COLUMN.AVG].ToString());
                        double sample_cnt = double.Parse(dr[Definition.CHART_COLUMN.SAMPLE_QTY].ToString());

                        comSPCStat.stddev = Math.Round(Math.Sqrt(sum_squared / sample_cnt - (avg * avg)), 5);
                        comSPCStat.mean = avg;
                        comSPCStat.CalcPpk(usl, lsl);

                        double cpk_sum_squared = double.Parse(dr[Definition.CHART_COLUMN.CPK_SUM_SQUARED].ToString());
                        double cpk_avg = double.Parse(dr[Definition.CHART_COLUMN.CPK_AVG].ToString());
                        double cpk_sample_cnt = double.Parse(dr[Definition.CHART_COLUMN.CPK_SAMPLE_QTY].ToString());

                        comSPCStat.cpkstddev = Math.Round(cpk_avg / 3.931, 5);
                        comSPCStat.CalcCpk(usl, lsl);

                        if (this.bShowOOCCount)
                        {
                            comSPCStat.CalcCa(usl, lsl);
                        }
                    }//Louis ppk if-end

                    dr[Definition.CHART_COLUMN.STDDEV] = comSPCStat.stddev;
                    dr[Definition.CHART_COLUMN.PPK] = comSPCStat.ppk;
                    dr[Definition.CHART_COLUMN.PP] = comSPCStat.pp;
                    dr[Definition.CHART_COLUMN.PPU] = comSPCStat.ppu;
                    dr[Definition.CHART_COLUMN.PPL] = comSPCStat.ppl;

                    dr[Definition.CHART_COLUMN.CPK_STDDEV] = comSPCStat.cpkstddev;
                    dr[Definition.CHART_COLUMN.CPK] = comSPCStat.cpk;
                    dr[Definition.CHART_COLUMN.CP] = comSPCStat.cp;
                    dr[Definition.CHART_COLUMN.CPU] = comSPCStat.cpu;
                    dr[Definition.CHART_COLUMN.CPL] = comSPCStat.cpl;

                    if (this.bShowOOCCount)
                    {
                        dr[Definition.CHART_COLUMN.CA] = comSPCStat.ca;
                    }
                }
                
                SpreadHeader();                
                dtPpk = DataUtil.DataTableImportRow(dtPpk.Select(null, sbSort.ToString()));
                this.bsprData.ClearHead();
                this.bsprData.UseEdit = true;
                this.bsprData.ActiveSheet.DefaultStyle.ResetLocked();
                
                //this.bsprData.ActiveSheet.FrozenColumnCount = 1;
                this.bsprData.ColFronzen = 1;

                this._Initialization.InitializePpkColumnHeader(ref this.bsprData, _sortHeader, this._sortHeaderLabel);
                this.bsprData.DataSource = dtPpk;
                this.bsprData.Locked = true;
                this.bsprData.ColumnsLock((int)BISTel.eSPC.Common.enum_ProcessCapability.SPC_V_SELECT, false);
                                                                                                  
            }
            catch (Exception ex)
            {
                EESProgressBar.CloseProgress(this);
                if (ex is OperationCanceledException || ex is TimeoutException)
                {
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
                else
                {
                    LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);

            }
            finally
            {

                if (dtPpk != null) dtPpk.Dispose();
                if (dtContext != null) dtContext.Dispose();
                if (_ds != null) _ds.Dispose();
                EESProgressBar.CloseProgress(this);
                //this.MsgClose();
            }
        }

        
       
        #endregion

        #region ::: EventHandler



        private void bsprData_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != (int)BISTel.eSPC.Common.enum_ProcessCapability.SPC_V_SELECT) return;            
            if ((bool)this.bsprData.GetCellValue(e.Row, (int)BISTel.eSPC.Common.enum_ProcessCapability.SPC_V_SELECT) == true)
            {
                for (int i = 0; i < bsprData.ActiveSheet.RowCount; i++)
                {
                    if (i == e.Row)
                    {
                        continue;
                    }
                    this.bsprData.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_ProcessCapability.SPC_V_SELECT].Value = 0;
                }
            }
           
        }


        private void bsprData_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader) return;

            if (e.Column > (int)BISTel.eSPC.Common.enum_ProcessCapability.SPC_V_SELECT)
            {
                string sMODEL_CONFIG_RAWID = this.bsprData.ActiveSheet.Cells[e.Row, (int)BISTel.eSPC.Common.enum_ProcessCapability.MODEL_CONFIG_RAWID].Text;
                string sMain_YN = this.bsprData.ActiveSheet.Cells[e.Row, (int)BISTel.eSPC.Common.enum_ProcessCapability.MAIN_YN].Text;
                Modeling.SPCConfigurationPopup spcConifgPopup = new Modeling.SPCConfigurationPopup();
                spcConifgPopup.SESSIONDATA = this.sessionData;
                spcConifgPopup.URL = this.URL;
                spcConifgPopup.PORT = this.Port;
                spcConifgPopup.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.VIEW;
                spcConifgPopup.CONFIG_RAWID = sMODEL_CONFIG_RAWID;
                spcConifgPopup.MAIN_YN = sMain_YN;
                spcConifgPopup.InitializePopup();
                DialogResult result = spcConifgPopup.ShowDialog(this);
                if (result == DialogResult.OK)
                {

                }

            }
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
                       
                       
                    if (this.bsprData.ActiveSheet.Rows.Count==0)
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
            DataSet _ds = null;
            DataTable dtResource = null;
            CreateChartDataTable _ccdt = new CreateChartDataTable();
            try
            {
                if (this.bsprData.ActiveSheet.RowCount <= 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_NO_SEARCH_DATA));
                    return;
                }

                ArrayList alCheckRowIndex = _bSpreadUtil.GetCheckedRowIndex(this.bsprData, (int)BISTel.eSPC.Common.enum_ProcessCapability.SPC_V_SELECT);
                if (alCheckRowIndex.Count < 1 || alCheckRowIndex.Count > 1)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("FDC_ALLOW_SINGLE_SELECTED_ROW"));
                    return;
                }

                int iRowIndex = (int)alCheckRowIndex[0];

                string strParamAlias = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_ProcessCapability.PARAM_ALIAS].Text;
                string strModelConfigRawID = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_ProcessCapability.MODEL_CONFIG_RAWID].Text;
                string strDefaultChart = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_ProcessCapability.DEFAULT_CHART_LIST].Text;
                string strComplex_yn = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_ProcessCapability.COMPLEX_YN].Text;
                string strArea = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_ProcessCapability.AREA].Text;
                string sOperationID = string.Empty;
                string sProductID = string.Empty;
                string strParamType = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_ProcessCapability.PARAM_TYPE_CD].Text;

                if (string.IsNullOrEmpty(strDefaultChart))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, string.Format(MSGHandler.GetMessage("GENERAL_NO_ITEM"), "Default Charts"));
                    return;
                }

                //Parameter
                LinkedList _llstDTSelectCondition = new LinkedList();

                _llstDTSelectCondition.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, CommonPageUtil.GetConCatString(strParamAlias));
                _llstDTSelectCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, strModelConfigRawID);


                //검색조건
                LinkedList _llstSearchCondition = new LinkedList();
                _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, strParamAlias);
                _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, strModelConfigRawID);
                _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, this.sStartTime);
                _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, this.sEndTime);
                _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PERIOD_PPK, sPeriod_ppk);

                StringBuilder sbSelect = new StringBuilder();
                sbSelect.Append("1=1");
                for (int i = 1; i < this._sortHeader.Count; i++)
                {
                    string sColumn = this._sortHeader[i].ToString();
                    string sValue = this.bsprData.ActiveSheet.Cells[iRowIndex, i].Text;
                    //if (sColumn == Definition.CHART_COLUMN.USL)                    
                    //    sbSelect.AppendFormat(" AND {0}='{1}'",Definition.CHART_COLUMN.MEAN_USL,sValue);
                    //else if (sColumn == Definition.CHART_COLUMN.LSL)                    
                    //    sbSelect.AppendFormat(" AND {0}='{1}'",Definition.CHART_COLUMN.MEAN_LSL,sValue);
                    if (sColumn == Definition.CHART_COLUMN.PERIOD)
                        _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PERIOD, sValue);
                    else if (sColumn == Definition.CHART_COLUMN.OPERATION_ID)
                    {
                        sOperationID = sValue == Definition.VARIABLE.STAR ? null : sValue;
                    }
                    else if (sColumn == Definition.CHART_COLUMN.PRODUCT_ID)
                    {
                        sProductID = sValue == Definition.VARIABLE.STAR ? null : sValue;
                    }

                    if (this.arrDynamicColKey.Contains(sColumn) && sColumn != Definition.CHART_COLUMN.OPERATION_DESC)
                    {
                        if (!string.IsNullOrEmpty(sValue) && sValue != Definition.VARIABLE.STAR)
                        {
                            _llstDTSelectCondition.Add(sColumn, CommonPageUtil.GetConCatString(sValue));
                        }
                    }
                }

                
                //DataSet _ds = _wsSPC.GetATTDataTRXData(_llstSearchCondition.GetSerialData());
                
                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);
                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);
                object objDataSet = ach.SendWait(_wsSPC, "GetATTDataTRXData", new object[] { _llstSearchCondition.GetSerialData() });
                EESProgressBar.CloseProgress(this);
                if (objDataSet != null)
                {
                    _ds = (DataSet)objDataSet;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                
                DataTable dtParamData = null;
                if (!DataUtil.IsNullOrEmptyDataSet(_ds))
                {
                    this.MsgShow("Drawing Chart... Can't Cancel!!!");
                    dtResource = CommonPageUtil.CLOBnBLOBParsing(_ds, new LinkedList(), false, false, false);
                    //if (strParamType == "MET")
                    //    dtResource = CommonPageUtil.CLOBnBLOBParsing(_ds, new LinkedList(), false, false, false);
                    //else
                    //    dtResource = CommonPageUtil.CLOBnBLOBParsing(_ds, new LinkedList(), false);

                    //dtResource = CommonPageUtil.CLOBnBLOBParsing(_ds, new LinkedList(), false);
                    if (_ds != null) _ds.Dispose();

                    DataSet dsOcapComment = null;
                    List<string> rawIDs = new List<string>();
                    if (dtResource.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                    {
                        bool bPNOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST);
                        bool bPOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST);
                        bool bCOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST);
                        bool bUOcap = dtResource.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST);

                        foreach (DataRow dr in dtResource.Rows)
                        {
                            string rawid = dr[Definition.CHART_COLUMN.OCAP_RAWID].ToString();
                            string sTemp = rawid.Replace(";", "");
                            if (sTemp.Length > 0)
                            {
                                string[] ids = rawid.Split(';');
                                foreach (string id in ids)
                                {
                                    if (string.IsNullOrEmpty(id))
                                        continue;
                                    if (!rawIDs.Contains(id))
                                        rawIDs.Add(id);
                                }
                            }

                            if (bPNOcap)
                            {
                                rawid = dr[Definition.CHART_COLUMN.PN_OCAP_LIST].ToString();

                                sTemp = rawid.Replace("^", "").Replace(";", "");
                                if (sTemp.Length > 0)
                                {
                                    string[] ids = rawid.Replace(";", "^").Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id))
                                            continue;
                                        if (!rawIDs.Contains(id))
                                            rawIDs.Add(id);
                                    }
                                }
                            }

                            if (bPOcap)
                            {
                                rawid = dr[Definition.CHART_COLUMN.P_OCAP_LIST].ToString();

                                sTemp = rawid.Replace("^", "");
                                if (sTemp.Length > 0)
                                {
                                    string[] ids = rawid.Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id))
                                            continue;
                                        if (!rawIDs.Contains(id))
                                            rawIDs.Add(id);
                                    }
                                }
                            }

                            if (bCOcap)
                            {
                                rawid = dr[Definition.CHART_COLUMN.C_OCAP_LIST].ToString();

                                sTemp = rawid.Replace("^", "");
                                if (sTemp.Length > 0)
                                {
                                    string[] ids = rawid.Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id))
                                            continue;
                                        if (!rawIDs.Contains(id))
                                            rawIDs.Add(id);
                                    }
                                }
                            }

                            if (bUOcap)
                            {
                                rawid = dr[Definition.CHART_COLUMN.U_OCAP_LIST].ToString();

                                sTemp = rawid.Replace("^", "");
                                if (sTemp.Length > 0)
                                {
                                    string[] ids = rawid.Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id))
                                            continue;
                                        if (!rawIDs.Contains(id))
                                            rawIDs.Add(id);
                                    }
                                }
                            }

                        }

                        if (rawIDs.Count == 0)
                            rawIDs.Add("");

                        LinkedList llstTmpOcapComment = new LinkedList();
                        llstTmpOcapComment.Add(Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID, rawIDs.ToArray());
                        llstTmpOcapComment.Add(Definition.CONDITION_KEY_START_DTTS, DateTime.Parse(this.sStartTime).ToString(Definition.DATETIME_FORMAT_MS));
                        llstTmpOcapComment.Add(Definition.CONDITION_KEY_END_DTTS, DateTime.Parse(this.sEndTime).ToString(Definition.DATETIME_FORMAT_MS));

                        byte[] baData = llstTmpOcapComment.GetSerialData();

                        dsOcapComment = _wsSPC.GetOCAPCommentList_New(baData);

                        //dsOcapComment = _wsSPC.GetOCAPCommentList(rawIDs.ToArray());
                    }

                    _ccdt.COMPLEX_YN = strComplex_yn;
                    dtParamData = _ccdt.GetMakeDataTable(dtResource);
                    //dtParamData = DataUtil.DataTableImportRow(dtResource.Select(sbSelect.ToString()));

                    ChartViewPopup chartViewPop = new ChartViewPopup();
                    chartViewPop.ChartVariable.LINE = _Line;
                    chartViewPop.ChartVariable.AREA = strArea;
                    chartViewPop.LINE_RAWID = _LineRawid; //SPC-1292, KBLEE
                    chartViewPop.AREA_RAWID = _AreaRawID; //SPC-1292, KBLEE
                    chartViewPop.ChartVariable.DEFAULT_CHART = strDefaultChart;
                    chartViewPop.ChartVariable.SPC_MODEL = strParamAlias;
                    chartViewPop.ChartVariable.PARAM_ALIAS = strParamAlias;
                    chartViewPop.ChartVariable.OPERATION_ID = sOperationID;
                    chartViewPop.ChartVariable.PRODUCT_ID = sProductID;
                    //chartViewPop.ChartVariable.dtResource = dtResource;
                    chartViewPop.ChartVariable.dtParamData = dtParamData;
                    chartViewPop.ChartVariable.complex_yn = Definition.VARIABLE_Y;
                    if (_llstSearchCondition != null && _llstSearchCondition.Contains(Definition.DynamicCondition_Condition_key.PERIOD))
                    {
                        if (_llstSearchCondition.Contains(Definition.DynamicCondition_Condition_key.PERIOD_PPK))
                        {
                            string strType = _llstSearchCondition[Definition.DynamicCondition_Condition_key.PERIOD_PPK].ToString();
                            string sPeriod = _llstSearchCondition[Definition.DynamicCondition_Condition_key.PERIOD].ToString();
                            if (strType == Definition.PERIOD_PPK.DAILY)
                            {
                                chartViewPop.ChartVariable.dateTimeStart = DateTime.Parse(CommonPageUtil.CalcStartDate(sPeriod));
                                chartViewPop.ChartVariable.dateTimeEnd = DateTime.Parse(CommonPageUtil.CalcEndDate(sPeriod));
                            }
                            else if (strType == Definition.PERIOD_PPK.WEEKLY)
                            {
                                string strTempStart = DateTime.Parse(sStartTime).AddDays(-1).ToString(Definition.DATETIME_FORMAT_YMD);
                                string strTempEnd = DateTime.Parse(sEndTime).ToString(Definition.DATETIME_FORMAT_YMD);
                                LinkedList llTemp = new LinkedList();
                                llTemp.Add("SDT", strTempStart);
                                llTemp.Add("EDT", strTempEnd);
                                llTemp.Add("VAL", sPeriod);

                                DataSet dsTempDate = _wsSPC.QueryWeeklyStartEnd(llTemp.GetSerialData());

                                if (dsTempDate != null && dsTempDate.Tables != null && dsTempDate.Tables[0].Rows.Count > 0)
                                {
                                    chartViewPop.ChartVariable.dateTimeStart = DateTime.Parse(CommonPageUtil.CalcStartDate(dsTempDate.Tables[0].Rows[0][0].ToString()));
                                    chartViewPop.ChartVariable.dateTimeEnd = DateTime.Parse(CommonPageUtil.CalcEndDate(dsTempDate.Tables[0].Rows[0][1].ToString()));
                                }
                            }
                            else if (strType == Definition.PERIOD_PPK.MONTHLY)
                            {
                                string strTempStart = DateTime.Parse(sStartTime).AddDays(-1).ToString(Definition.DATETIME_FORMAT_YMD);
                                string strTempEnd = DateTime.Parse(sEndTime).ToString(Definition.DATETIME_FORMAT_YMD);

                                LinkedList llTemp = new LinkedList();
                                llTemp.Add("SDT", strTempStart);
                                llTemp.Add("EDT", strTempEnd);
                                llTemp.Add("VAL", sPeriod);

                                DataSet dsTempDate = _wsSPC.QueryMonthStartEnd(llTemp.GetSerialData());

                                if (dsTempDate != null && dsTempDate.Tables != null && dsTempDate.Tables[0].Rows.Count > 0)
                                {
                                    chartViewPop.ChartVariable.dateTimeStart = DateTime.Parse(CommonPageUtil.CalcStartDate(dsTempDate.Tables[0].Rows[0][0].ToString()));
                                    chartViewPop.ChartVariable.dateTimeEnd = DateTime.Parse(CommonPageUtil.CalcEndDate(dsTempDate.Tables[0].Rows[0][1].ToString()));
                                }
                            }
                            else
                            {

                                chartViewPop.ChartVariable.dateTimeStart = DateTime.Parse(sStartTime);
                                chartViewPop.ChartVariable.dateTimeEnd = DateTime.Parse(sEndTime);
                            }
                        }
                        else
                        {
                            chartViewPop.ChartVariable.dateTimeStart = DateTime.Parse(sStartTime);
                            chartViewPop.ChartVariable.dateTimeEnd = DateTime.Parse(sEndTime);
                        }
                    }
                    else
                    {
                        chartViewPop.ChartVariable.dateTimeStart = DateTime.Parse(sStartTime);
                        chartViewPop.ChartVariable.dateTimeEnd = DateTime.Parse(sEndTime);
                    }
                    chartViewPop.ChartVariable.MODEL_CONFIG_RAWID = strModelConfigRawID;
                    chartViewPop.ChartVariable.CHART_PARENT_MODE = BISTel.eSPC.Common.CHART_PARENT_MODE.PPK_REPORT;
                    chartViewPop.ChartVariable.MAIN_YN = this.bsprData.ActiveSheet.Cells[iRowIndex, (int)BISTel.eSPC.Common.enum_ProcessCapability.MAIN_YN].Text;
                    chartViewPop.ChartVariable.llstDTSelectCondition = _llstDTSelectCondition;
                    chartViewPop.URL = this.URL;
                    chartViewPop.SessionData = this.sessionData;
                    chartViewPop.ParamTypeCD = strParamType;
                    chartViewPop.ChartVariable.dtOCAP = dsOcapComment.Tables[0];
                    chartViewPop.GROUP_NAME = _GroupName;
                    chartViewPop.InitializePopup();

                    chartViewPop.linkTraceDataViewEventPopup -= new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
                    chartViewPop.linkTraceDataViewEventPopup += new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);

                    DialogResult result = chartViewPop.ShowDialog(this);
                    _GroupName = chartViewPop.GROUP_NAME;

                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }
            }
            catch (Exception ex)
            {
                EESProgressBar.CloseProgress(this);
                if (ex is OperationCanceledException || ex is TimeoutException)
                {
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
                else
                {
                    LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
            }
            finally
            {
                if (_ccdt != null) _ccdt = null;
                if (dtResource != null) dtResource.Dispose();
                if (_ds != null) _ds.Dispose();
            }
        }

        void chartViewPop_linkTraceDataViewEventPopup(object sender, EventArgs e, LinkedList llstTraceLinkData)
        {
            this.SendMessage("TRACE_DATA", true, llstTraceLinkData, 0);
        }
        
        #endregion
	}
}
