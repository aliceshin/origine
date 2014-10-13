using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;


using BISTel.eSPC.Common;

namespace BISTel.eSPC.Condition.Report
{

    public partial class SPCChartCondition_LED : ADynamicCondition
    {

         string mLine = string.Empty;
         string mLineRawid = string.Empty;
         string mAreaRawid = string.Empty;
         string mArea = string.Empty;
         string mSPCModelRawID = string.Empty;
         string mModelConfigRawID = string.Empty;
         string mParamTypeCD = string.Empty;
         string mStartTime = string.Empty;
         string mEndTime = string.Empty;
         string mParamAlias = string.Empty;
         string mSPCModel = string.Empty;
         string mMainYN = string.Empty;
         string mEQPModel = string.Empty;
         string mEQPModelRawID = string.Empty;
         string mDefaultChartList = string.Empty;

         string mAreaRawidUnit = string.Empty;
         string mAreaUnit = string.Empty;         
         string mEQPModelUnit = string.Empty;

         int iRestrictSampleDays = Definition.RESTRICT_SAMPLE_DAYS;

         eSPCWebService.eSPCWebService _wsSPC = null;
         CommonUtility _comUtil;       
         
         SPCStruct.ChartConditionContextList strucContextList;
         SPCStruct.ChartContextInfo strucContextinfo ;                                 
         
         MultiLanguageHandler _mlthandler;
         Initialization _Initialization;
         
         LinkedList llstData = new LinkedList();
         LinkedList _condition = new LinkedList();
         LinkedList mllstContext;
         LinkedList mllstCustomContext;                                                          
                                                   
         DataTable mDTModelContext = new DataTable();
         DataTable mDTResult = new DataTable();
         private bool _bUseComma;

         public SPCChartCondition_LED()
         {
             this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._comUtil = new CommonUtility();
            this._Initialization = new Initialization();  
            this.strucContextList = new SPCStruct.ChartConditionContextList();
            this.mllstCustomContext = new LinkedList();
            InitializeComponent();
            
            this.InitializePage();                   
         }

        #region override


        public void InitializePage()
        {
            InitializeBSpread();
            InitializeLayerOut();
        }

        public void InitializeLayerOut()
        {
            this.bTitlePnlPeriod.Title = _mlthandler.GetVariable(Definition.LABEL.PERIOD);    
            
            this.dateCondition1 = new BISTel.eSPC.Condition.Controls.Date.DateCondition();
            this.dateCondition1.Dock = DockStyle.Fill;
            this.bTitlePnlPeriod.Controls.Add(this.dateCondition1);
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_MODELING, Definition.CONFIG_USE_COMMA, false);
        }
        
        public void InitializeBSpread()
        {

            this.bSpreadContext.UseSpreadEdit = false;
            this.bSpreadContext.AutoGenerateColumns = false;
            this.bSpreadContext.ActiveSheet.DefaultStyle.ResetLocked();
            this.bSpreadContext.UseEdit = false;
            this.bSpreadContext.UseAutoSort = false;
            this.bSpreadContext.ClearHead();
            this.bSpreadContext.Locked = true;
            this.bSpreadContext.UseHeadColor = true;
            this.bSpreadContext.EditMode = true;
            this.bSpreadContext.ActiveSheet.RowCount = 0;
            this.bSpreadContext.ActiveSheet.ColumnCount = 0;
            //this.bSpreadContext.ActiveSheet.ColumnHeader.Visible =false;
            this.bSpreadContext.ActiveSheet.RowHeader.Visible = false; 
            this.bbtnSelectModel.Visible= false;                       
        }
        
        public override LinkedList GetParameter(LinkedList ll)
        {
            if (this._Initialization == null){
               
            }
                        
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_PERIOD, DynamicDefinition.DynamicConditionSetting(this.dateCondition1.DateType, this.dateCondition1.DateType));
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, this.dateCondition1.GetDateTimeSelectedValue("F"));
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, this.dateCondition1.GetDateTimeSelectedValue("T"));
            ll.Add(Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID, PROC_CreateSPCModelConfig());            
            ll.Add(Definition.DynamicCondition_Search_key.CONTEXT, PROC_CreateDTContext(this.mllstContext));                
            ll.Add(Definition.DynamicCondition_Search_key.CUSTOM_CONTEXT, PROC_CreateDTContext(this.mllstCustomContext));
                                               
            return ll;
        }
                        
        public override void RefreshCondition(LinkedList ll)
        {
            DataTable _dt =null;

            this.mSPCModelRawID = string.Empty;
            this.mSPCModel = string.Empty;
            this.mEQPModel = string.Empty;
            this.mEQPModelRawID = string.Empty;

            this.InitializeBSpread();
            
            if (ll.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))            
                this.mParamTypeCD = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.PARAM_TYPE]);            
            
            if (ll.Contains(Definition.DynamicCondition_Search_key.LINE))
            {
                _dt = (DataTable)ll[Definition.DynamicCondition_Search_key.LINE];
                this.mLineRawid = DCUtil.GetValueData(_dt);
                this.mLine = _dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();   
            }


            if (ll.Contains(Definition.DynamicCondition_Search_key.AREA))
            {
                _dt = (DataTable)ll[Definition.DynamicCondition_Search_key.AREA];
                this.mAreaRawid = DataUtil.GetConditionKeyDataList(_dt, DCUtil.VALUE_FIELD);
                this.mArea = DataUtil.GetConditionKeyDataList(_dt, DCUtil.DISPLAY_FIELD, true); 
            }

            if (ll.Contains(Definition.DynamicCondition_Search_key.SPCMODEL))
            {
                _dt = (DataTable)ll[Definition.DynamicCondition_Search_key.SPCMODEL];
                this.mSPCModelRawID = DCUtil.GetValueData(_dt);
                this.mSPCModel = _dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();                
            }

            if (ll.Contains(Definition.DynamicCondition_Search_key.EQPMODEL))
            {
                _dt = (DataTable)ll[Definition.DynamicCondition_Search_key.EQPMODEL];
                this.mEQPModelRawID = DataUtil.GetConditionKeyDataList(_dt, DCUtil.VALUE_FIELD, true);
                this.mEQPModel = DataUtil.GetConditionKeyDataList(_dt, DCUtil.DISPLAY_FIELD, true);
            }
            
            SetCondition(ll);
        }


        #endregion

        #region User Method       
        private void SetCondition(LinkedList ll)
        {

            //여기서 ll이랑 _condition이랑 복제한다. 
            byte[] tmp = ll.GetSerialData();

            _condition.SetSerialData((byte[])tmp.Clone());
            
            DataTable dt = null;
            this.mllstContext = new LinkedList();
            this.mllstCustomContext = new LinkedList();   
     
             
            if (_condition.Contains(Definition.DynamicCondition_Search_key.DATETIME_FROM))            
            {
                dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_FROM];
                this.mStartTime = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Condition_key.VALUEDATA]);                       
            }

            if (_condition.Contains(Definition.DynamicCondition_Search_key.DATETIME_TO))
            {
                dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_TO];
                this.mEndTime = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Condition_key.VALUEDATA]);                         
            }

            this.mModelConfigRawID =string.Empty;
            if (_condition.Contains(Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID))
            {
                dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID];
                this.mModelConfigRawID = DCUtil.GetValueData(dt);
                this.mParamAlias = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
               
                this.mMainYN = dt.Rows[0][Definition.DynamicCondition_Condition_key.MAIN_YN].ToString();                
                this.mDefaultChartList = dt.Rows[0][Definition.DynamicCondition_Condition_key.DEFAULT_CHART_LIST].ToString();

                this.mAreaRawidUnit = dt.Rows[0][Definition.DynamicCondition_Condition_key.AREA_RAWID].ToString();
                this.mAreaUnit = dt.Rows[0][Definition.DynamicCondition_Condition_key.AREA].ToString();
                this.mEQPModelUnit = dt.Rows[0][Definition.DynamicCondition_Condition_key.EQP_MODEL].ToString();

                this.iRestrictSampleDays = int.Parse(dt.Rows[0][Definition.DynamicCondition_Condition_key.RESTRICT_SAMPLE_DAYS].ToString());
            }

            
            if (_condition.Contains(Definition.DynamicCondition_Search_key.CONTEXT))
            {
                dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.CONTEXT];                           
                foreach (DataRow dr in dt.Rows)
                {
                    this.mllstContext.Add(dr[Definition.DynamicCondition_Search_key.VALUEDATA].ToString(), dr[Definition.DynamicCondition_Search_key.DISPLAYDATA].ToString());
                }
            }

            if (_condition.Contains(Definition.DynamicCondition_Search_key.CUSTOM_CONTEXT))
            {
                dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.CUSTOM_CONTEXT];               
                foreach (DataRow dr in dt.Rows)
                {
                    this.mllstCustomContext.Add(dr[Definition.DynamicCondition_Search_key.VALUEDATA].ToString(), dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString());
                }
            }
            
                        
            PROC_BindContext();            
            
            if (!string.IsNullOrEmpty(this.mStartTime))
            {
                this.dateCondition1.DateTimeEnd = DateTime.Parse(this.mEndTime);
                this.dateCondition1.DateTimeStart = DateTime.Parse(this.mStartTime);
            }
            else
            {
                this.dateCondition1.DateType = Definition.PERIOD_TYPE.CUSTOM;
                this.dateCondition1.DateTimeStart = DateTime.Now.AddDays(-this.iRestrictSampleDays);
            }
            
        }
        
       
        private void PROC_BindContext()
        {
            DataTableGroupBy dtGroupBy = new DataTableGroupBy();    
            this.PROC_ContextCall();
            if(DataUtil.IsNullOrEmptyDataTable(this.mDTModelContext)) return;

            this.AddHead();                                   
            DataTable dtModelConfigRawID = dtGroupBy.SelectGroupByInto(COLUMN.MODEL_CONFIG_RAWID, this.mDTModelContext, COLUMN.MODEL_CONFIG_RAWID, null, COLUMN.MODEL_CONFIG_RAWID);
            DataTable dtContextKey = dtGroupBy.SelectGroupByInto(COLUMN.CONTEXT_KEY, this.mDTModelContext, COLUMN.CONTEXT_KEY + "," + COLUMN.KEY_ORDER, null, COLUMN.CONTEXT_KEY + "," + COLUMN.KEY_ORDER);
            DataRow[] drSelect = dtContextKey.Select(null, COLUMN.KEY_ORDER);
            //this.Height = 100 + ((drSelect.Length + 1) * 28)+this.bTitlePnlPeriod.Height;
            //this.bTitlePnlContext.Dock = DockStyle.Fill;
            //this.bTitlePnlPeriod.Dock = DockStyle.Bottom;
            //this.bTitlePnlContext.Height = 97 + ((drSelect.Length+1) * 28);  
            //this.AutoScroll=true;   
            //this.AutoSize=true;                              
            if(!string.IsNullOrEmpty(this.mModelConfigRawID))
                this.PROC_SpreadContextBind(this.mllstContext, this.mllstCustomContext);
            else
            {
                this.pnlSelectModel.Visible = true;     
                if (dtModelConfigRawID.Rows.Count == 1)
                {
                    this.mModelConfigRawID = this.mDTModelContext.Rows[0][COLUMN.MODEL_CONFIG_RAWID].ToString();
                    this.mMainYN = this.mDTModelContext.Rows[0][COLUMN.MAIN_YN].ToString();
                    this.mParamAlias = this.mDTModelContext.Rows[0][COLUMN.PARAM_ALIAS].ToString();
                    this.mDefaultChartList = this.mDTModelContext.Rows[0][COLUMN.DEFAULT_CHART_LIST].ToString();
                    this.mAreaRawidUnit =  this.mDTModelContext.Rows[0][COLUMN.AREA_RAWID].ToString();
                    this.mAreaUnit = this.mDTModelContext.Rows[0][COLUMN.AREA].ToString();
                    this.mEQPModelUnit =  this.mDTModelContext.Rows[0][COLUMN.EQP_MODEL].ToString();

                    this.iRestrictSampleDays = int.Parse(string.IsNullOrEmpty(this.mDTModelContext.Rows[0][COLUMN.RESTRICT_SAMPLE_DAYS].ToString()) ? "15" : this.mDTModelContext.Rows[0][COLUMN.RESTRICT_SAMPLE_DAYS].ToString());

                    this.dateCondition1.DateType = Definition.PERIOD_TYPE.CUSTOM;
                    this.dateCondition1.DateTimeStart = DateTime.Now.AddDays(-this.iRestrictSampleDays);
                    this.dateCondition1.DateTimeEnd = DateTime.Now;
                                                                             
                    this.mllstContext = new LinkedList();
                    this.mllstCustomContext = new LinkedList();
                    foreach(DataRow dr in dtContextKey.Rows)
                    {                        
                        DataRow[] drSelect2 = this.mDTModelContext.Select(string.Format("{0}='{1}'",COLUMN.CONTEXT_KEY,dr[COLUMN.CONTEXT_KEY].ToString())); 
                        this.mllstContext.Add( dr[COLUMN.CONTEXT_KEY].ToString(),drSelect2[0][COLUMN.CONTEXT_VALUE].ToString());                     
                    }                    
                    PROC_SpreadContextBind(mllstContext, mllstCustomContext);                    
                }
                else
                {                    
                    this.bbtnSelectModel.Visible = true;
                }  
             }                                            
        }
        
        private void AddHead()        
        {
            this.bSpreadContext.DataSource = null;
            this.bSpreadContext.ClearHead();
            this.bSpreadContext.ActiveSheet.RowCount = 0;
            this.bSpreadContext.ActiveSheet.ColumnCount = 5;
            this.bSpreadContext.AddHead(0, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.KEY), COLUMN.CONTEXT_KEY, 100, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, true);
            this.bSpreadContext.AddHead(1, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.VALUE), COLUMN.CONTEXT_VALUE, 120, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, true);
            this.bSpreadContext.AddHead(2, COLUMN.MODEL_CONFIG_RAWID, COLUMN.MODEL_CONFIG_RAWID, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, false);
            this.bSpreadContext.AddHead(3, COLUMN.MAIN_YN, COLUMN.MAIN_YN, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, false);
            //Original Context Value
            this.bSpreadContext.AddHead(4, "Custom Key", COLUMN.CONTEXT_KEY, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, false);
            this.bSpreadContext.AddHead(5, "Custom Value", COLUMN.CONTEXT_VALUE, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, false);        
        }

        private void PROC_SpreadContextBind(LinkedList _llstContext, LinkedList _llstCustomContext)
        {
            this.bSpreadContext.ActiveSheet.Columns[0].BackColor = Color.SteelBlue;
            //this.bSpreadContext.ActiveSheet.Columns[0].Font = new Font("Gulim", 9, FontStyle.Bold);
            this.bSpreadContext.ActiveSheet.Columns[0].ForeColor = Color.White;  
                                                
            this.bSpreadContext.ActiveSheet.RowCount = _llstContext.Count+1;                      
            this.bSpreadContext.ActiveSheet.Cells[0, 0].Text = this._mlthandler.GetVariable(Definition.SPC_LABEL_ + COLUMN.PARAM_ALIAS);
            this.bSpreadContext.ActiveSheet.Cells[0, 1].Text = this.mParamAlias + "(" + this.mMainYN + ")";
            this.bSpreadContext.ActiveSheet.Cells[0, 2].Text = this.mModelConfigRawID;
            this.bSpreadContext.ActiveSheet.Cells[0, 3].Text = this.mMainYN;
            this.bSpreadContext.ActiveSheet.Cells[0, 4].Text = this.mParamAlias;
            this.bSpreadContext.ActiveSheet.Cells[0, 5].Text = this.mParamAlias;
        
                                                              
            for (int i = 0; i < _llstContext.Count; i++)
            {
                string sKey = _llstContext.GetKey(i).ToString();
                string sValue = _llstContext.GetValue(i).ToString();
                string sCustomValue = string.Empty;
                
                try{
                    sCustomValue = _llstCustomContext[sKey].ToString();
                    if(string.IsNullOrEmpty(sCustomValue))sCustomValue = sValue;
                }
                catch{
                    sCustomValue = sValue; 
                }
                                
                this.bSpreadContext.ActiveSheet.Cells[i + 1, 0].Text = this._mlthandler.GetVariable(Definition.SPC_LABEL_ + sKey);
                this.bSpreadContext.ActiveSheet.Cells[i + 1, 1].Text = sCustomValue;
                this.bSpreadContext.ActiveSheet.Cells[i + 1, 2].Text = this.mModelConfigRawID;
                this.bSpreadContext.ActiveSheet.Cells[i + 1, 3].Text = this.mMainYN;
                this.bSpreadContext.ActiveSheet.Cells[i + 1, 4].Text = sKey;
                this.bSpreadContext.ActiveSheet.Cells[i + 1, 5].Text = sValue;                                
            }
        }


        private DataTable PROC_CreateSPCModelConfig()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(Definition.DynamicCondition_Condition_key.VALUEDATA, typeof(string));
            dt.Columns.Add(Definition.DynamicCondition_Condition_key.DISPLAYDATA, typeof(string));
            dt.Columns.Add(Definition.DynamicCondition_Condition_key.MAIN_YN, typeof(string));
            dt.Columns.Add(Definition.DynamicCondition_Condition_key.DEFAULT_CHART_LIST, typeof(string));
            dt.Columns.Add(Definition.DynamicCondition_Condition_key.RESTRICT_SAMPLE_DAYS, typeof(string));
            dt.Columns.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, typeof(string));
            dt.Columns.Add(Definition.DynamicCondition_Condition_key.AREA, typeof(string));
            dt.Columns.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, typeof(string));


            DataRow dr = dt.NewRow();
            dr[Definition.DynamicCondition_Condition_key.VALUEDATA] = this.mModelConfigRawID;
            dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA] = this.mParamAlias;
            dr[Definition.DynamicCondition_Condition_key.MAIN_YN] = this.mMainYN;
            dr[Definition.DynamicCondition_Condition_key.DEFAULT_CHART_LIST] = this.mDefaultChartList;
            dr[Definition.DynamicCondition_Condition_key.RESTRICT_SAMPLE_DAYS] = this.iRestrictSampleDays.ToString();
            dr[Definition.DynamicCondition_Condition_key.AREA_RAWID] = this.mAreaRawidUnit;
            dr[Definition.DynamicCondition_Condition_key.AREA] = this.mAreaUnit;
            dr[Definition.DynamicCondition_Condition_key.EQP_MODEL] = this.mEQPModelUnit;
            dt.Rows.Add(dr);
            
            return dt;
        }
        
        private DataTable PROC_CreateDTContext(LinkedList _llstContext)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(Definition.DynamicCondition_Condition_key.VALUEDATA, typeof(string));
            dt.Columns.Add(Definition.DynamicCondition_Condition_key.DISPLAYDATA, typeof(string));
            
            DataRow dr = null;
            for (int i = 0; i < _llstContext.Count; i++)
            {                
                dr =   dt.NewRow();
                dr[Definition.DynamicCondition_Condition_key.VALUEDATA] = _llstContext.GetKey(i).ToString();
                dr[Definition.DynamicCondition_Condition_key.DISPLAYDATA] = _llstContext.GetValue(i).ToString();                          
                dt.Rows.Add(dr);
            }            
            return dt;
        }
        
                
        private void PROC_ContextCall()
        {
                                                          
            try{                        
                                                               
                this.mDTModelContext = new DataTable();                
                llstData.Clear();
                llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.mLineRawid);            
                llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.mAreaRawid);

                //llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.mParamTypeCD);
                if (!string.IsNullOrEmpty(this.mParamTypeCD))
                    llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.mParamTypeCD);

                llstData.Add(Definition.DynamicCondition_Condition_key.SPC_MODEL_RAWID, this.mSPCModelRawID);
                if(!string.IsNullOrEmpty(this.mEQPModel))
                    llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.mEQPModel);

                if (this.mLineRawid == null || this.mLineRawid.Length == 0)
                    return;

                llstData.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

                DataSet _dsContext = this._wsSPC.GetSPCContextList(llstData.GetSerialData());
                if (!DataUtil.IsNullOrEmptyDataSet(_dsContext))         
                {                              
                    //this.bTitlePnlContext.Visible = true;
                    this.bSpreadContext.Visible = true;
                    this.mDTModelContext = _dsContext.Tables[0];                                                             
                }else
                {                    
                    //this.bTitlePnlContext.Visible = false;
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
     
       
        #endregion


        #region Event       
        
        /// <summary>
        /// Slect SPC Model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbtnSelectModel_Click(object sender, EventArgs e)
        {
            BISTel.PeakPerformance.Client.BaseUserCtrl.BasePageUCtrl bpUC = new BasePageUCtrl();

            this.strucContextinfo = new SPCStruct.ChartContextInfo();
            this.strucContextinfo.AREA = this.mArea.Replace("'", "");
            this.strucContextinfo.LINE = this.mLine;
            this.strucContextinfo.MODEL_CONFIG_RAWID = this.mModelConfigRawID;
            this.strucContextinfo.SPC_MODEL_NAME = this.mSPCModel;
            this.strucContextinfo.PARAM_ALIAS = this.mParamAlias;
            this.strucContextinfo.llstCustomContext = this.mllstCustomContext;
            this.strucContextinfo.llstContext = this.mllstContext;
            this.strucContextinfo.DTModelContext = this.mDTModelContext;
            this.strucContextinfo.SPCModelType = SPCMODEL_TYPE.CONDITION;

            SPCChartConditionPopup _chartConditionPop = new SPCChartConditionPopup();
            _chartConditionPop.CONTEXT_INFO = strucContextinfo;
            try
            {
                bpUC.MsgShow(this._mlthandler.GetVariable("RMS_PROGRESS_SEARCH"));
                _chartConditionPop.InitializePopup();
            }
            catch (Exception ex)
            {
                bpUC.MsgClose();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                bpUC.MsgClose();
            }
            _chartConditionPop.ShowDialog();
            DialogResult dResult = _chartConditionPop.ButtonResult;
            if (dResult == DialogResult.OK)
            {
                this.strucContextinfo = _chartConditionPop.CONTEXT_INFO;
                this.mModelConfigRawID = this.strucContextinfo.MODEL_CONFIG_RAWID;
                this.mParamAlias = this.strucContextinfo.PARAM_ALIAS;
                this.mMainYN = this.strucContextinfo.MAIN_YN;

                this.mAreaUnit = this.strucContextinfo.AREA;
                this.mAreaRawidUnit = this.strucContextinfo.AREA_RAWID;
                this.mEQPModelUnit = this.strucContextinfo.EQP_MODEL;

                this.mllstContext = this.strucContextinfo.llstContext;
                this.mllstCustomContext = this.strucContextinfo.llstCustomContext;
                this.mDefaultChartList = this.strucContextinfo.DEFAULT_CHART_LIST;
                this.iRestrictSampleDays = this.strucContextinfo.RESTRICT_SAMPLE_DAYS;

                this.dateCondition1.DateType = Definition.PERIOD_TYPE.CUSTOM;
                this.dateCondition1.DateTimeStart = DateTime.Now.AddDays(-this.iRestrictSampleDays);
                this.dateCondition1.DateTimeEnd = DateTime.Now;

                //this.AddHead();
                this.PROC_SpreadContextBind(this.mllstContext, this.mllstCustomContext);

            }
        }


        #endregion

    }
}
