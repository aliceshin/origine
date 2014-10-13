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
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;

using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.Report
{
	public partial class SPCControlChartUC : BasePageUCtrl
	{

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;
        CommonUtility _ComUtil;
        BSpreadUtility _bSpreadUtil;
        Initialization _Initialization;
        MultiLanguageHandler _mlthandler;  
        ChartUtility _chartUtil;      
        LinkedList _llstSearchCondition = new LinkedList();            
        DataTableGroupBy dtGroupBy = new DataTableGroupBy();
        ArrayList arrModelCongifRawID = new ArrayList();       
        DataTable _dtChartData = new DataTable();


        protected LinkedList _llstChartSearchCondition = new LinkedList();  
        protected LinkedList _llstValue = null;               
        protected LinkedList _llstChart = new LinkedList();                   
        protected List<string> _lstDefaultChart = null;

        protected DataSet _dsChart = null;        
        protected DataSet _dsOCAPValue = null;
        LinkedList _llstChartSeriesVisibleType = null;
        
        ChartInterface _ChartVariable;        
        ChartInformation  _chartInfomationUI = new ChartInformation();
        SortedList sListInformation = null;
        CHART_PARENT_MODE _chart_mode = CHART_PARENT_MODE.SPC_CONTROL_CHART;
                
        protected SourceDataManager _dataManager = null;    
        protected double xval;
        int _valueIndex=-1;
             
        string _Fab = string.Empty;
        string _Site = string.Empty;
        string _LineRawID = string.Empty;
        string _AreaRawID = string.Empty;  
        
        string sStartTime = string.Empty;
        string sEndTime = string.Empty;
        string sParameter = string.Empty;
        string sSPCModel = string.Empty;
        string sProductID = string.Empty;
        string sEQPID = string.Empty;            
        string sModelCongifRawid = string.Empty;
        string sDefaultChartList = string.Empty;
        string sMainYN = "N";
        bool _bDataTableSelect = false;
        string sParamTypeCd = string.Empty;
        string _OperationColumnName = Definition.CHART_COLUMN.MEASURE_OPERATION_ID;

        int restrict_sample_count = Definition.RESTRICT_SAMPLE_COUNT;
        int iSearch = 1;
                
       
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


        public SPCControlChartUC()
		{

            this._ComUtil = new CommonUtility();
            this._bSpreadUtil = new BSpreadUtility();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._wsSPC = new eSPCWebService.eSPCWebService();
            this._Initialization = new Initialization();
            this._dataManager = new SourceDataManager();
            this._Initialization.InitializePath();
            this._chartUtil = new ChartUtility();
            InitializeComponent();
            this.InitializeLayout();  
            this.InitializeDataButton();
            this.InitializeCode();
            InitializePage();  			
		}


        public void InitializePage()
		{
            if( ChartVariable==null) return;

            _llstChart.Clear();
            _llstChart = GetDefaultChart();

            for (int i = 0; i < this.bbtnListChart.Items.Count; i++)
            {
                if (this.bbtnListChart.Items[i].Name.ToUpper() == Definition.ButtonKey.CUSTOM_CONTEXT)
                    this.bbtnListChart.Items[i].Visible = false;

                if (CHART_MODE != CHART_PARENT_MODE.SPC_CONTROL_CHART && this.bbtnListChart.Items[i].Name.ToUpper() == Definition.ButtonKey.ANALYSIS_CHART)
                    this.bbtnListChart.Items[i].Visible = false;
            }
                                             		    
			this.InitializeBSpread();						
            this.InitializeChartSeriesVisibleType();
            this.InitializeChart();                        
		}

        private void InitializeChartSeriesVisibleType()
        {           
            _llstChartSeriesVisibleType = new LinkedList(); 
            this.panel1.Controls.Clear();
            Hashtable htButtonList = this._Initialization.InitializeChartSeriesList(Definition.PAGE_KEY_SPC_CONTROL_CHART_UC, Definition.CHART_BUTTON.CHART_SERIES, this.sessionData);
            BCheckBox bchk = null;
            for (int i = htButtonList.Count - 1; i >= 0; i--)
            {                                
                Hashtable htButtonAttributes = (Hashtable)htButtonList[i.ToString()];
                string sButtonName = htButtonAttributes[Definition.XML_VARIABLE_BUTTONKEY].ToString();
                string Visible = htButtonAttributes[Definition.XML_VARIABLE_BUTTONVISIBLE].ToString().ToUpper();
                string DefaultVisible = htButtonAttributes[Definition.XML_VARIABLE_DEFAULTVALUE].ToString().ToUpper();
                
                if (Visible == Definition.VARIABLE_TRUE.ToUpper())
                {
                    bchk = new BCheckBox();
                    bchk.Text = this._mlthandler.GetVariable(sButtonName);
                    bchk.Name = sButtonName;                                                                       
                    bchk.Click +=new EventHandler(bchk_Click);                    
                    bchk.Dock = DockStyle.Left;
                    bchk.TabIndex = i;
                    if (DefaultVisible == Definition.VARIABLE_TRUE.ToUpper())                    
                        bchk.Checked = true;
                    else
                        bchk.Checked = false;
                    
                    if(sButtonName==Definition.CHART_SERIES.POINT )
                    {
                        if(ChartVariable.complex_yn!="Y")
                            bchk.Visible = false ;
                        else
                            bchk.Visible = true;
                    }                                               
                    _llstChartSeriesVisibleType.Add(bchk.Text, bchk.Checked);
                    this.panel1.Controls.Add(bchk);                                
                }                    
            }     

                             
        }
        
        private void InitializeCode()
        {
            LinkedList lk = new LinkedList();
            lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CONTROL_CHART);
            lk.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            _dsChart = this._wsSPC.GetCodeData(lk.GetSerialData());                                                                                                                                  
        }        		    
        		    
        public void InitializeChart(){

            if (ChartVariable.dtParamData != null && ChartVariable.dtParamData.Rows.Count > 0)
                _dataManager.RawDataTable = ChartVariable.dtParamData;
            
            int iChart = _llstChart.Count;
            int iHeight = 300;
            if (iChart == 1)            
                iHeight = this.pnlChart.Height;            
            else if (iChart > 1)     
            {       
                iHeight = this.pnlChart.Height / 2;            
                this.pnlChart.Height = iChart*iHeight;
            }
             
            this.pnlChart.Controls.Clear();
            DefaultChart chartBase = null;    
            Steema.TeeChart.Tools.CursorTool _cursorTool =null;
            Steema.TeeChart.Tools.CursorTool _cursorTool2 = null;
                               
            for (int i = _llstChart.Count-1; i >= 0; i--)
            {
                string strKey = _llstChart.GetKey(i).ToString();
                string strValue = _llstChart.GetValue(i).ToString();                                                        
                chartBase = new DefaultChart(_dataManager);
                chartBase.ClearChart();
                chartBase.Title = strValue;
                chartBase.Name = strKey;
                chartBase.Pagekey = Definition.PAGE_KEY_SPC_CONTROL_CHART_UC;
                chartBase.Itemkey = Definition.CHART_BUTTON.CHART_DEFAULT;
                chartBase.SPCChartTitlePanel.Name = strKey;
                chartBase.Height=iHeight;
                chartBase.URL= this.URL;                
                chartBase.Dock = System.Windows.Forms.DockStyle.Top;                                        
                chartBase.ContextMenu = this.bbtnListChart.ContextMenu;                                                
                chartBase.ParentControl = this.pnlChart;
                chartBase.StartDateTime = ChartVariable.dateTimeStart;
                chartBase.EndDateTime = ChartVariable.dateTimeEnd;
                chartBase.SettingXBarInfo(strKey, Definition.CHART_COLUMN.TIME, ChartVariable.lstRawColumn, _llstChartSeriesVisibleType);                                                 
                if (!chartBase.DrawSPCChart()) continue;               
                _cursorTool = new Steema.TeeChart.Tools.CursorTool();                                                                                
                _cursorTool.Active = true;
                _cursorTool.Style = Steema.TeeChart.Tools.CursorToolStyles.Both;
                _cursorTool.Pen.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                _cursorTool.Pen.Color = Color.Gray;
                _cursorTool.FollowMouse = true;
                _cursorTool.Change += new Steema.TeeChart.Tools.CursorChangeEventHandler(_cursorTool_Change);
                chartBase.SPCChart.Chart.Tools.Add(_cursorTool);                                                
                chartBase.SPCChart.Chart.MouseLeave += new EventHandler(Chart_MouseLeave);
                chartBase.SPCChart.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(SPCChart_ClickSeries);                                                          
                this.pnlChart.Controls.Add(chartBase);                                                                     
            }
                                          
        }
    
        public void InitializeLayout()
        {
            this.bTitlePanel1.Title = this._mlthandler.GetVariable(Definition.TITLE_KEY_SPC_CONTROL_CHART);           
        }

        public void InitializeDataButton()
        {
            this._Initialization.InitializeButtonList(this.bbtnListChart, Definition.PAGE_KEY_SPC_CONTROL_CHART_UC, Definition.BUTTONLIST_KEY_CHART, this.sessionData);
            
        }
		
        public void InitializeBSpread()
        {           
            _chartInfomationUI = new ChartInformation();         
            _chartInfomationUI.SessionData = this.sessionData;
            _chartInfomationUI.ChartVariable = ChartVariable;            
            _chartInfomationUI.dsChart = this._dsChart;  
            _chartInfomationUI.Name = "CHART_INFO";    
            _chartInfomationUI.InitializePopup();
            _chartInfomationUI.Dock = DockStyle.Fill;
            sListInformation = _chartInfomationUI.sListInformation;            
            //if(CHART_MODE==CHART_PARENT_MODE.SPC_CONTROL_CHART)
            //{         
            //    this.bplInformation.Visible =false;                             
            //    this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "Information", _chartInfomationUI, false, new Rectangle(Screen.PrimaryScreen.WorkingArea.Width - 250, 100, 250, 500), EESConstants.InformationDock.Right, EESConstants.InformationDock.Left);
                
            //}else
            //{
                this.bplInformation.Visible = true;
                for(int i=0; i<this.bTitPanelInformation.Controls.Count; i++)
                {
                    if (this.bTitPanelInformation.Controls[i].GetType() == typeof(ChartInformation))
                    this.bTitPanelInformation.Controls.RemoveAt(i);
                }
                                                                        
                this.bTitPanelInformation.Controls.Add(_chartInfomationUI);
            //}
                      
        }
   
		#region ::: User Defined Method.	
		
		
        private void GetLine()
        {             
            DataSet  ds  = null;  
            try{      
                _llstChartSearchCondition.Clear();
                _llstChartSearchCondition.Add(Definition.DynamicCondition_Condition_key.LINE, ChartVariable.LINE);
                ds = _wsSPC.GetAnalysisLine(_llstChartSearchCondition.GetSerialData());                    
                if(!DataUtil.IsNullOrEmptyDataSet(ds))
                {
                    this._Site = ds.Tables[0].Rows[0]["SITE"].ToString();
                    this._Fab = ds.Tables[0].Rows[0]["FAB"].ToString();
                    this._LineRawID = ds.Tables[0].Rows[0]["RAWID"].ToString(); 
                                                       
                }
                
            }catch
            {
            }finally
            {
                if (ds != null) ds.Dispose();
            
            }
        }


        private void GetArea()
        {
            DataSet ds = null;
            try
            {
                _llstChartSearchCondition.Clear();
                _llstChartSearchCondition.Add(Definition.DynamicCondition_Condition_key.AREA, ChartVariable.AREA);
                ds = _wsSPC.GetAnalysisArea(_llstChartSearchCondition.GetSerialData());
                if (!DataUtil.IsNullOrEmptyDataSet(ds))
                {
                    this._AreaRawID = ds.Tables[0].Rows[0]["RAWID"].ToString();                    
                }

            }
            catch
            {
            }
            finally
            {
                if (ds != null) ds.Dispose();

            }
        }
		
				
        private void GetOCAPRawID()
		{
		    if(ChartVariable.dtParamData==null) return;		    		                
            if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
            {
                string _fieldList = Definition.CHART_COLUMN.OCAP_RAWID;
                string _groupby = Definition.CHART_COLUMN.OCAP_RAWID;
                string _rowFilter = "ocap_rawid <>''";

                dtGroupBy = new DataTableGroupBy();        
                DataTable dtOCAP = dtGroupBy.SelectGroupByInto("OCAP", ChartVariable.dtParamData, _fieldList, _rowFilter, _groupby);

                _llstChartSearchCondition.Clear();
                _llstChartSearchCondition.Add(Definition.DynamicCondition_Condition_key.OCAP_DATATABLE, dtOCAP);
                _dsOCAPValue = _wsSPC.GetOCAPDetails(_llstChartSearchCondition.GetSerialData());            
            }            
		}
		
        private LinkedList GetDefaultChart()
        {        
             LinkedList _llstDefaultChart = new LinkedList();
             _llstDefaultChart.Clear();             
            if(DataUtil.IsNullOrEmptyDataSet(_dsChart)) return _llstDefaultChart;
            if(ChartVariable == null) return _llstDefaultChart;
            foreach (DataRow dr in _dsChart.Tables[0].Rows)
            {                                  
                string strkey = dr["CODE"].ToString();
                if(ChartVariable.lstDefaultChart.Contains(strkey))
                {
                    _llstDefaultChart.Add(strkey, dr["DESCRIPTION"].ToString());
                }                
            }                 
            return  _llstDefaultChart;   
        }
        
        
		#endregion



        #region ::: Public                


        public CHART_PARENT_MODE CHART_MODE
        {
            get { return this._chart_mode; }
            set { this._chart_mode = value; }
        }
        
        public ChartInterface ChartVariable
        {
            get { return _ChartVariable; }
            set { _ChartVariable = value; }
        }
        #endregion 
        

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
            DataSet _ds = new DataSet();
            
            string sLineRawid=string.Empty;
            string sAreaRawid = string.Empty;           
            try{

                this.arrModelCongifRawID.Clear();
                _ChartVariable = new ChartInterface();
                if (!llstCondition.Contains(Definition.DynamicCondition_Search_key.PARAM))
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("FDC_ALLOW_SINGLE_ONE"), "Parameter"));
                    return;
                }

                if (!llstCondition.Contains(Definition.DynamicCondition_Search_key.OPERATION))
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("FDC_ALLOW_SINGLE_ONE"), "OperationID"));
                    return;
                }
                
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
                    _ChartVariable.LINE = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
                    sLineRawid = dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();            
                }

                if (llstCondition[Definition.DynamicCondition_Search_key.AREA] != null)
                {
                    dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.AREA];
                    sAreaRawid = dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();
                }
                
                sParamTypeCd = _ComUtil.GetConditionData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE], Definition.DynamicCondition_Condition_key.VALUEDATA);
                _ChartVariable.OPERATION_ID = _ComUtil.GetConditionData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.OPERATION], Definition.DynamicCondition_Condition_key.VALUEDATA);       
                sParameter = _ComUtil.GetConditionData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PARAM], Definition.DynamicCondition_Condition_key.VALUEDATA);
                sEQPID = _ComUtil.GetConditionString((DataTable)llstCondition[Definition.DynamicCondition_Search_key.EQP_ID], Definition.DynamicCondition_Condition_key.VALUEDATA);
                sProductID = _ComUtil.GetConditionString((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PRODUCT], Definition.DynamicCondition_Condition_key.VALUEDATA);

                if (llstCondition[Definition.DynamicCondition_Search_key.SEARCH_COUNT] != null)
                {
                    dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.SEARCH_COUNT];                
                    this.iSearch = int.Parse(dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
                }
                
                if (llstCondition[Definition.DynamicCondition_Search_key.RESTRICT_SAMPLE_COUNT] != null)
                {
                    dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.RESTRICT_SAMPLE_COUNT];
                    string sRestrict_sample_count=dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();
                    restrict_sample_count = string.IsNullOrEmpty(sRestrict_sample_count) ? Definition.RESTRICT_SAMPLE_COUNT : int.Parse(sRestrict_sample_count);
                }
                

                if (llstCondition[Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID] != null)
                {
                    dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID];                    
                    SetModelConfigRawID(dt);                   
                }        
     
            }catch{
            }finally
            {
                if (dt != null) dt.Dispose();
                if (_ds != null) _ds.Dispose();               
            }

                          
            try
            {
                if (this.arrModelCongifRawID.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));                   
                    return;
                }
                
                sModelCongifRawid = CommonPageUtil.GetConditionKeyArrayList(arrModelCongifRawID);
                this._llstSearchCondition.Clear();
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, sStartTime);
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, sEndTime);
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, sModelCongifRawid);

                if (this.sParamTypeCd.Equals("MET"))
                    _OperationColumnName = Definition.CHART_COLUMN.MEASURE_OPERATION_ID;
                else
                    _OperationColumnName = Definition.CHART_COLUMN.OPERATION_ID;
                                    
                 PROC_DataBinding();

                 
            }
            catch (Exception ex)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }

        }

        #endregion


        #region ::: User Defined Method.                
        private void SetModelConfigRawID(DataTable _dt)
        {
            string _sModelCongifRawid =string.Empty;
            string strModelCongifRawID=string.Empty;
            string sCol =Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID;            
            if(!_dt.Columns.Contains(sCol)) sCol= Definition.DynamicCondition_Condition_key.VALUEDATA;
            
            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                 strModelCongifRawID = _dt.Rows[i][sCol].ToString();
                if (!arrModelCongifRawID.Contains(strModelCongifRawID))
                    arrModelCongifRawID.Add(strModelCongifRawID);                                       
            }
                     
        }

        private string GetSelectWhereString()
        {                    
            StringBuilder sb = new StringBuilder();

            
               sb.AppendFormat(" TIME >='{0}' AND TIME <='{1}'",this.sStartTime,this.sEndTime);
                
            if (!string.IsNullOrEmpty(sEQPID) && sEQPID.IndexOf(Definition.VARIABLE.STAR) < 0)
                sb.AppendFormat(" AND  (EQP_ID IN ({0}) OR {1} IN ({0}))", sEQPID,Definition.DynamicCondition_Condition_key.MEASURE_EQP_ID);

            if (!string.IsNullOrEmpty(this._ChartVariable.OPERATION_ID) && _ChartVariable.OPERATION_ID.IndexOf(Definition.VARIABLE.STAR) < 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.AppendFormat(" ({0} ='{1}' OR {2} ='{1}')", Definition.DynamicCondition_Condition_key.OPERATION_ID, _ChartVariable.OPERATION_ID, Definition.DynamicCondition_Condition_key.MEASURE_OPERATION_ID);
            }

            if (!string.IsNullOrEmpty(sProductID) && sProductID.IndexOf(Definition.VARIABLE.STAR) < 0)
            {
                if (sb.Length > 0) sb.Append(" AND ");
                sb.AppendFormat(" PRODUCT_ID IN ({0})", sProductID);
            }

            return sb.ToString();  
            
        }
        private void PROC_DataBinding()
        {                   
            DataSet _ds =null;
            CreateChartDataTable _createChartDT = null;                                          
            try
            {                  
                _dtChartData = new DataTable();
                _createChartDT = new CreateChartDataTable();
               
                this.MsgShow(this._mlthandler.GetVariable("RMS_PROGRESS_SEARCH"));
                _llstSearchCondition.Add(Definition.CONDITION_KEY_CONTEXT_KEY_LIST, GetSelectWhereString());
                _ds = _wsSPC.GetSPCControlChartData(_llstSearchCondition.GetSerialData());                
                 if(DataUtil.IsNullOrEmptyDataSet(_ds))
                 {
                     this.MsgClose();
                     MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));                        
                 }else
                 {
                    this._dtChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, _llstSearchCondition,false);
                    if(_ds !=null) _ds.Dispose();                    
                    if (DataUtil.IsNullOrEmptyDataTable(_dtChartData))
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }
                                                                                      
                     _dtChartData = DataUtil.DataTableImportRow(_dtChartData.Select(null, Definition.CHART_COLUMN.TIME));



                     if (string.IsNullOrEmpty(_dtChartData.Rows[0][COLUMN.DEFAULT_CHART_LIST].ToString()))
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_NO_ITEM"), "Default Charts"));                    
                    }
                    else{
                        _ChartVariable.lstDefaultChart.Clear();

                        _ChartVariable.DEFAULT_CHART = _dtChartData.Rows[0][COLUMN.DEFAULT_CHART_LIST].ToString();
                        _ChartVariable.complex_yn = _dtChartData.Rows[0][COLUMN.COMPLEX_YN].ToString();
                        _ChartVariable.MAIN_YN = _dtChartData.Rows[0][COLUMN.MAIN_YN].ToString();
                        _ChartVariable.MODEL_CONFIG_RAWID = _dtChartData.Rows[0][COLUMN.MODEL_CONFIG_RAWID].ToString();
                        _ChartVariable.SPC_MODEL = _dtChartData.Rows[0][COLUMN.SPC_MODEL_NAME].ToString();
                        _ChartVariable.PARAM_ALIAS = sParameter;
                        _ChartVariable.OPERATION_ID = _dtChartData.Rows[0][Definition.CHART_COLUMN.OPERATION_ID].ToString();
                        _ChartVariable.PRODUCT_ID = _dtChartData.Rows[0][Definition.CHART_COLUMN.PRODUCT_ID].ToString();
                        _ChartVariable.RESTRICT_SAMPLE_DAYS = _dtChartData.Rows[0][COLUMN.RESTRICT_SAMPLE_DAYS].ToString();
                        _ChartVariable.AREA = _dtChartData.Rows[0][Definition.CHART_COLUMN.AREA].ToString();
                        _createChartDT.COMPLEX_YN = _ChartVariable.complex_yn;
                        _ChartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(_ChartVariable.DEFAULT_CHART);
                        _ChartVariable.dtParamData = _createChartDT.GetMakeDataTable(_dtChartData);
                        _ChartVariable.lstRawColumn = _createChartDT.lstRawColumn; //_createChartDT.CallRefCol(_dtChartData);
                        _ChartVariable.CHART_PARENT_MODE = CHART_PARENT_MODE.SPC_CONTROL_CHART;
                        _ChartVariable.dateTimeStart = DateTime.Parse(this.sStartTime);
                        _ChartVariable.dateTimeEnd = DateTime.Parse(this.sEndTime);
                        
                        if (_dtChartData != null) _dtChartData.Dispose();                                
                        if(_createChartDT !=null)_createChartDT =null;

                                                      
                    }
                }                
                                                              
                InitializePage();                                                                                                                                      
                     
            }
            catch (Exception ex)
            {
                this.MsgClose();           
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {    
                this.MsgClose();                                                
            }
        }
  
        #endregion       
        
        #region ::: EventHandler

        #region ::: Event Methods
        private BaseChart FindSPCChart(Control ctl)
        {
            Control parentControl = ctl.Parent;

            while (parentControl != null)
            {
                if (parentControl is BaseChart)
                {
                    return parentControl as BaseChart;
                }
                else
                {
                    parentControl = parentControl.Parent;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCol"></param>
        /// <param name="strValue"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private string CreateToolTipString(string strCol, string strValue, int rowIndex, string strChartName)
        {
            StringBuilder sb = new StringBuilder();
            LinkedList llstChartSeries = new LinkedList();
            
            string _sEQPID = "-";
            string _sLOTID = "-";
            string _sOperationID = "-";
            string _sSubstrateID = "-";

            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                _sEQPID =  _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.EQP_ID].ToString();

            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.LOT_ID))
                _sLOTID =  _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.LOT_ID].ToString();

            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OPERATION_ID))
                _sOperationID =  _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.OPERATION_ID].ToString();

            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.SUBSTRATE_ID))
                _sSubstrateID =  _dataManager.RawDataTable.Rows[rowIndex][Definition.CHART_COLUMN.SUBSTRATE_ID].ToString();
                  
                                      
            sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.EQP_ID),_sEQPID);
            sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.LOT_ID), _sLOTID);
            sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.OPERATION_ID), _sOperationID);
            sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(Definition.SPC_LABEL_ + Definition.CHART_COLUMN.SUBSTRATE_ID), _sSubstrateID);
                                    
            if(_llstChart.Contains(strChartName))
            {           
                llstChartSeries = CommonChart.GetChartSeries(strChartName, ChartVariable.lstRawColumn);
                for(int i=0; i<llstChartSeries.Count; i++)
                {
                    string sKey = llstChartSeries.GetKey(i).ToString();
                    string sValue = llstChartSeries.GetValue(i).ToString(); 
                    
                    if(sValue.Equals(Definition.CHART_COLUMN.USL)  ||   
                        sValue.Equals(Definition.CHART_COLUMN.LSL) ||
                        sValue.Equals(Definition.CHART_COLUMN.UCL) ||
                        sValue.Equals(Definition.CHART_COLUMN.LCL) ||
                        sValue.Equals(Definition.CHART_COLUMN.TARGET)) continue;
                                                                                                                                             
                    if (_dataManager.RawDataTable.Columns.Contains(sKey))
                    {       
                      sb.AppendFormat("{0} : {1}\r\n", _mlthandler.GetVariable(sValue), _dataManager.RawDataTable.Rows[rowIndex][sKey].ToString());                            
                    }
                }                   
                llstChartSeries = null;                               
            }
                           
            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
            {
                string strRawid = GetOCAPRawID(_dataManager.RawDataTable, rowIndex);               
                DataTable dtOCAP = null;
                dtGroupBy = new DataTableGroupBy();           
                if (!string.IsNullOrEmpty(strRawid))
                {
                    string _fieldList = "ocap_rawid,RAW_RULE";
                    string _groupby = "ocap_rawid,RAW_RULE";
                    string _sRuleNo = string.Empty;
                    if (string.IsNullOrEmpty(strRawid.Replace(";", ""))) return sb.ToString();                    
                    if (ChartVariable.dtParamData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                    {                        
                        string _rowFilter = string.Format("ocap_rawid ='{0}'",strRawid) ;
                        dtOCAP = dtGroupBy.SelectGroupByInto("OCAP", ChartVariable.dtParamData, _fieldList, _rowFilter, _groupby);                        
                        foreach (DataRow dr in dtOCAP.Rows)
                        {
                            if (!string.IsNullOrEmpty(_sRuleNo)) _sRuleNo += ",";
                            _sRuleNo += dr["RAW_RULE"].ToString().Replace(";",",");
                        }
                        sb.AppendFormat("{0} : {1}\r\n", "OOC", _sRuleNo);               
                    }                  
                }
                dtGroupBy = null;
                if(dtOCAP !=null) dtOCAP.Dispose();
            }

            return sb.ToString();
        }

        private string GetOCAPRawID(DataTable dt, int rowIndex)
        {
            string strRawid = string.Empty;
            if (dt.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))            
                strRawid = dt.Rows[rowIndex][Definition.DynamicCondition_Condition_key.OCAP_RAWID].ToString();
                            
            return strRawid;
        }




        private void Chart_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                BaseChart baseChart = FindSPCChart((Control)sender);
                Annotation ann;
                if (baseChart is DefaultChart)
                {
                    for(int i=0; i< baseChart.SPCChart.Chart.Tools.Count; i++)
                    {
                        if (baseChart.SPCChart.Chart.Tools[i].GetType() == typeof(Annotation))
                        {
                            ann = baseChart.SPCChart.Chart.Tools[i] as Annotation;
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

        private void ClickButtonCalc()
        {

        }

        private void ClickButtonChartView()
        {


            BISTel.eSPC.Page.Common.Popup.ChartViewSelectedPopup chartViewPop = new BISTel.eSPC.Page.Common.Popup.ChartViewSelectedPopup();
            chartViewPop.URL = this.URL;
            chartViewPop.SessionData = this.sessionData;
            chartViewPop.DataSetChart = _dsChart; //Chart 종류   
            chartViewPop.llstDefaultChart = _llstChart;
            chartViewPop.InitializePopup();
            DialogResult result = chartViewPop.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                _llstChart.Clear();
                _llstChart = chartViewPop.llstResultListChart;
                this.InitializeChartSeriesVisibleType();
                this.InitializeChart(); //chart 다시 그림  
                
                chartViewPop.Dispose();              
            }
                        
        }

        private void ClickButtonConfiguration()
        {                        
            Modeling.SPCConfigurationPopup spcConifgPopup = new Modeling.SPCConfigurationPopup();
            spcConifgPopup.SESSIONDATA = this.sessionData;
            spcConifgPopup.URL = this.URL;
            spcConifgPopup.PORT = this.Port;                                    
            spcConifgPopup.CONFIG_MODE = ConfigMode.MODIFY;
            spcConifgPopup.CONFIG_RAWID = ChartVariable.MODEL_CONFIG_RAWID;
            spcConifgPopup.MAIN_YN = ChartVariable.MAIN_YN;
            spcConifgPopup.InitializePopup();
            DialogResult result = spcConifgPopup.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                spcConifgPopup.Dispose();
            }

        }

        private void ClickButtonShowChartData()
        {            
            DataTable dt = _dataManager.RawDataTable.Copy();
            DataTable dtExcel = new DataTable();  
            ChartDataPopup chartDataPop = null;          
            try
            {

                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    string sCol = dt.Columns[i].ColumnName.ToString();
                    if (sCol.StartsWith(Definition.CHART_COLUMN.RAW))
                    {
                        if (sCol == Definition.CHART_COLUMN.RAW)
                        {
                            if (ChartVariable.complex_yn == "Y" && ChartVariable.lstRawColumn.Count > 0)
                                dt.Columns.RemoveAt(i);
                            else
                                dt.Columns[i].ColumnName = Definition.CHART_COLUMN.PARAM_VALUE;
                        }
                        else
                            dt.Columns.RemoveAt(i);
                    }
                    else if (sCol.StartsWith(Definition.CHART_COLUMN.MEAN))
                    {
                        if ((sCol.IndexOf("_RULE") > -1))
                            dt.Columns.RemoveAt(i);
                        else
                        {
                            string[] arr = sCol.Split('_');
                            if(arr.Length>1)
                            dt.Columns[i].ColumnName = arr[1].ToString();
                        }
                    }
                    else if (sCol.StartsWith(Definition.CHART_COLUMN.MIN) || sCol.StartsWith(Definition.CHART_COLUMN.MAX))
                    {
                        if ((sCol.IndexOf("_USL") > -1) || (sCol.IndexOf("_LSL") > -1)
                         || (sCol.IndexOf("_CL") > -1) || (sCol.IndexOf("_TARGET") > -1)
                         || (sCol.IndexOf("_UCL") > -1) || (sCol.IndexOf("_LCL") > -1) || (sCol.IndexOf("_RULE") > -1)
                         )
                        {
                            dt.Columns.RemoveAt(i);
                        }
                    }
                    else if ((sCol.IndexOf("CHART_LIST") > -1)
                            || (sCol.IndexOf("|") > -1)
                            || (sCol.IndexOf("SAMPLE_COUNT") > -1)
                            || (sCol.IndexOf("SAMPLE_QTY") > -1)
                            || (sCol.IndexOf("REF_PARAM_ALIAS") > -1)
                            || (sCol.IndexOf("REF_PARAM_LIST") > -1)
                            || (sCol.IndexOf("REF_DATA") > -1)
                            || (sCol.IndexOf("REF_PARAM") > -1)
                            || (sCol.IndexOf("PARAM_LIST") > -1)
                            || (sCol.IndexOf("param_list") > -1)
                            || (sCol.IndexOf("PARAM_VALUE") > -1)
                            || (sCol.IndexOf("CONTEXT_LIST") > -1)
                            || (sCol.IndexOf("DATA_LIST") > -1)
                            || (sCol.IndexOf("FILE_DATA") > -1)
                            || (sCol.IndexOf("MODEL_CONFIG_RAWID") > -1)
                            || (sCol.IndexOf(COLUMN.COMPLEX_YN) > -1)
                            || (sCol.IndexOf(COLUMN.MAIN_YN) > -1)
                            || (sCol.IndexOf(COLUMN.RESTRICT_SAMPLE_DAYS) > -1)
                            || (sCol.IndexOf(COLUMN.RESTRICT_SAMPLE_COUNT) > -1)
                            )
                    {
                        dt.Columns.RemoveAt(i);
                    }
                    else if ((sCol.IndexOf("_USL") > -1)
                         || (sCol.IndexOf("_LSL") > -1)
                         || (sCol.IndexOf("_CL") > -1)
                         || (sCol.IndexOf("_RULE") > -1)
                         || (sCol.IndexOf("_UCL") > -1)
                         || (sCol.IndexOf("_LCL") > -1)
                         || (sCol.IndexOf("_TARGET") > -1)
                         || (sCol.IndexOf("WORKDATE") > -1)
                         )
                    {
                        dt.Columns.RemoveAt(i);
                    }
                }
                dt.AcceptChanges();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string sCol = dt.Columns[i].ColumnName.ToString().ToUpper();
                    if (CommonChart.PARAM_ITEM.Contains(sCol))
                        dtExcel.Columns.Add(sCol, typeof(double));
                    else
                        dtExcel.Columns.Add(sCol, dt.Columns[i].DataType);
                }

                foreach (DataRow dr in dt.Rows)
                    dtExcel.ImportRow(dr);

                chartDataPop = new ChartDataPopup();
                chartDataPop.URL = this.URL;
                chartDataPop.SessionData = this.sessionData;
                chartDataPop.DataTableParam = dtExcel;
                chartDataPop.InitializePopup();
                DialogResult result = chartDataPop.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    chartDataPop.Dispose();
                }
            }catch
            {
            
            }finally
            {
                 dt.Dispose();
                 dtExcel.Dispose();
            }

        }        
        

        private void CursorSynchronize(Steema.TeeChart.Tools.CursorTool SRC, Steema.TeeChart.Tools.CursorTool DEST)
        {
            DEST.XValue = SRC.XValue;
            DEST.YValue = SRC.YValue;                                    
        }
        
        #endregion 



        void SPCChart_Resize(object sender, System.EventArgs e)
        {
            BaseChart baseChart = null;
            int iCount = this.pnlChart.Controls.Count;
            int iHeight = 0;
            if (iCount == 1)
            {
                iHeight = this.pnlChart.Height;
            }
            else if (iCount > 1)
            {
                iHeight = this.pnlChart.Height / 2;
            }
    
            for (int i = 0; i < this.pnlChart.Controls.Count; i++)
            {
                baseChart = (BaseChart)this.pnlChart.Controls[i];  
                baseChart.Height = iHeight;           
            }
        }


        void bTitlePanel1_Resize(object sender, System.EventArgs e)
        {
           BTitlePanel _bTitlePanel = sender as BTitlePanel;
            SPCChart_Resize(this, null);
           
        }

        private void bchk_Click(object sender, System.EventArgs e)
        {
            BCheckBox chk = (BCheckBox)sender;
            if (chk == null) return;
            BaseChart baseChart = null;
            for (int i = 0; i < this.pnlChart.Controls.Count; i++)
            {
                baseChart = (BaseChart)this.pnlChart.Controls[i];
                baseChart.llstChartSeriesVisibleType = _llstChartSeriesVisibleType;
                baseChart.ChangeSeriesStyle(chk.Text, chk.Checked);
            }
        }



        private void _cursorTool_Change(object sender, Steema.TeeChart.Tools.CursorChangeEventArgs e)
        {
            Steema.TeeChart.Tools.CursorTool _currentCursor = sender as Steema.TeeChart.Tools.CursorTool;
            double xValue = _currentCursor.XValue;
            double yValue = _currentCursor.YValue;
            ControlCollection parentControl = this.pnlChart.Controls;
            for (int i = 0; i < parentControl.Count; i++)
            {
                if (parentControl[i] is BaseChart)
                {
                    BaseChart baseChart = parentControl[i] as BaseChart;
                    for (int j = 0; j < baseChart.SPCChart.Chart.Tools.Count; j++)
                    {
                        if (baseChart.SPCChart.Chart.Tools[j].GetType() == typeof(Steema.TeeChart.Tools.CursorTool))
                        {
                        
                            Steema.TeeChart.Tools.CursorTool _CursorDesc = (Steema.TeeChart.Tools.CursorTool)baseChart.SPCChart.Chart.Tools[j];
                            if(_CursorDesc.Pen.Color == Color.Gray)
                            {
                                if (xValue != _CursorDesc.XValue && yValue != _CursorDesc.YValue)
                                {
                                    CursorSynchronize(_currentCursor, _CursorDesc);
                                    break;
                                }
                            }
                          
                        }
                    }
                }
            }
        }
                                         
        private void SPCChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                SeriesInfo seriesInfo = ((ChartSeriesGroup)sender).GetSeriesInfo(s);
                if (seriesInfo == null || !seriesInfo.SeriesData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX)) return;

                #region ToolTip
                BaseChart baseChart = FindSPCChart((Control)sender);
                if (baseChart is DefaultChart)
                {                    
                    _chartInfomationUI.InfomationSpreadReSet(this._dataManager.RawDataTable.Rows[valueIndex]);   
                    _valueIndex =   valueIndex;            
                    string strCol = s.Title;
                    if (strCol == Definition.CHART_COLUMN.USL_LSL)
                        strCol = Definition.CHART_COLUMN.USL;
                    else if (strCol == Definition.CHART_COLUMN.UCL_LCL)
                        strCol = Definition.CHART_COLUMN.UCL;

                    string strValue = seriesInfo.SeriesData.Rows[valueIndex][strCol].ToString();

                    for (int i = 0; i < baseChart.SPCChart.Chart.Tools.Count; i++)
                    {
                        if (baseChart.SPCChart.Chart.Tools[i].GetType() == typeof(Steema.TeeChart.Tools.Annotation))
                        {
                            Annotation ann = (Annotation)baseChart.SPCChart.Chart.Tools[i];
                            this._chartUtil.ShowAnnotate(baseChart.SPCChart.Chart, ann, e.X, e.Y, CreateToolTipString(strCol, strValue, valueIndex, baseChart.NAME));
                            break;
                        }
                    }    
                                  
                }

                #endregion
            }
            catch (Exception ex)
            {
                EESUtil.DebugLog(ex);
                _valueIndex = -1;
            }                                       
        }


        private void bbtnListChart_ButtonClick(string name)
        {            
            try
            {

                if (ChartVariable==null || ChartVariable.dtParamData == null || ChartVariable.dtParamData.Rows.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }
                                
                if (name.ToUpper() == Definition.ButtonKey.CALC)                 
                    this.ClickButtonCalc();                
                else if (name.ToUpper() == Definition.ButtonKey.SELECT_CHART_TO_VIEW)                
                    this.ClickButtonChartView();                
                else if (name.ToUpper() == Definition.ButtonKey.DEFAULT_CHART)
                {                    
                    _llstChart.Clear();
                    _llstChart = GetDefaultChart();
                    this.InitializeChartSeriesVisibleType();
                    this.InitializeChart(); 
                }      
                else if (name.ToUpper() == Definition.ButtonKey.CONFIGURATION)                
                    this.ClickButtonConfiguration();                
                else if (name.ToUpper() == Definition.ButtonKey.CHART_DATA)                
                    this.ClickButtonShowChartData();                
                else if (name.ToUpper() == Definition.ButtonKey.OCAP_VIEW)
                {

                    if (_chartInfomationUI.bsprInformationData.ActiveSheet.RowCount == 0)
                   {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));                   
                        return;
                   }

                   if (_valueIndex == -1) return;                   
                   string strRawid = GetOCAPRawID(_dataManager.RawDataTable, _valueIndex);                   
                   string [] arr = strRawid.Split(';');
                   string sRawid=string.Empty;
                   for(int i=0; i<arr.Length; i++)
                   {
                       if(string.IsNullOrEmpty(arr[i])) continue;
                       sRawid +=arr[i]+",";                       
                   }
                   if (sRawid.EndsWith(",")) sRawid = sRawid.Substring(0, sRawid.Length - 1);
                   
                   if (string.IsNullOrEmpty(sRawid))
                   {
                       MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                       return;
                   }          
                                                                                                         
                   BISTel.eSPC.Page.OCAP.OCAPDetailsPopup popupOCAP = new BISTel.eSPC.Page.OCAP.OCAPDetailsPopup();
                   popupOCAP.ChartVariable = ChartVariable;

                   popupOCAP.ChartVariable.OPERATION_ID = this._chartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this._chartInfomationUI.sListInformation[Definition.CHART_COLUMN.OPERATION_ID], 1].Text;
                   popupOCAP.ChartVariable.PRODUCT_ID = this._chartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this._chartInfomationUI.sListInformation[Definition.CHART_COLUMN.PRODUCT_ID], 1].Text;                   
                   popupOCAP.ChartVariable.llstInfoCondition = CommonPageUtil.GetOCAPParameter(_dataManager.RawDataTable, _valueIndex);
                   popupOCAP.ChartVariable.OCAPRawID = sRawid;                   
                   popupOCAP.PopUpType = enum_PopupType.View;
                   popupOCAP.SessionData = this.sessionData;
                   popupOCAP.URL = this.URL;               
                   popupOCAP.InitializePopup();
                   DialogResult result = popupOCAP.ShowDialog(this);
                   if (result == DialogResult.OK)
                   {
                        popupOCAP.Dispose();
                   }             
                }else if( name.ToUpper() == Definition.ButtonKey.ANALYSIS_CHART)
                {                
                    this.GetLine();
                    this.GetArea();
                    
                    //string _paraItem = this._chartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this._chartInfomationUI.sListInformation[Definition.CHART_COLUMN.PARAM_ALIAS], 1].Text;
                    //string _operation = this._chartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)this._chartInfomationUI.sListInformation[Definition.CHART_COLUMN.OPERATION_ID], 1].Text;
                    
                    LinkedList lk = new LinkedList();
                    //lk.Add(Definition.DynamicCondition_Search_key.SITE, DynamicDefinition.DynamicConditionSetting(this._Site, this._Site));
                    //lk.Add(Definition.DynamicCondition_Search_key.FAB, DynamicDefinition.DynamicConditionSetting(this._Fab, this._Fab));
                    //lk.Add(Definition.DynamicCondition_Search_key.LINE, DynamicDefinition.DynamicConditionSetting(ChartVariable.LINE, this._LineRawID));
                    //lk.Add(Definition.DynamicCondition_Search_key.AREA, DynamicDefinition.DynamicConditionSetting(ChartVariable.AREA, this._AreaRawID));
                    //lk.Add(Definition.DynamicCondition_Search_key.OPERATION, DynamicDefinition.DynamicConditionSetting(_operation, _operation));
                    //lk.Add(Definition.DynamicCondition_Search_key.PARAM, DynamicDefinition.DynamicConditionSetting(_paraItem, _paraItem));
                    //lk.Add(Definition.DynamicCondition_Search_key.DATETIME_PERIOD, DynamicDefinition.DynamicConditionSetting("CUSTOM", "CUSTOM"));
                    //lk.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, DCUtil.MakeDataTableForDCValue(ChartVariable.dateTimeStart.ToString(Definition.DATETIME_FORMAT_MS), ChartVariable.dateTimeStart.ToString(Definition.DATETIME_FORMAT_MS)));
                    //lk.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, DCUtil.MakeDataTableForDCValue(ChartVariable.dateTimeEnd.ToString(Definition.DATETIME_FORMAT_MS), ChartVariable.dateTimeEnd.ToString(Definition.DATETIME_FORMAT_MS)));
                    //AnalysisPopup analysisPop = new AnalysisPopup();
                    //analysisPop.LineRawID = this._LineRawID;
                    //analysisPop.AreaRawID = this._AreaRawID;
                    //analysisPop.SITE = this._Site;
                    //analysisPop.FAB = this._Fab;                    
                    //analysisPop.DateTimeStart = ChartVariable.dateTimeStart;
                    //analysisPop.DateTimeEnd = ChartVariable.dateTimeEnd;
                    //analysisPop.ParamItem = this._chartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)enum_ChartInfomationData.PARAM_NAME, 1].Text;
                    //analysisPop.OperationID = this._chartInfomationUI.bsprInformationData.ActiveSheet.Cells[(int)enum_ChartInfomationData.OPERATION_ID, 1].Text;                    
                    //analysisPop.ContentsAreaMinWidth= 1020;   
                    //analysisPop.ContentsAreaMinHeight = 700;                 
                    //analysisPop.InitializePopup();
                    //analysisPop.ShowDialog(this);
                    //analysisPop.Dispose();
                                                            
                    base.SubmitPage("DLLS/eSPC/BISTel.eSPC.Page.dll", "BISTel.eSPC.Page.Analysis.SPCAnalysisUC", "Analysis", "Analysis", false, lk);                                                                               
                                        
                }
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }
        
            void  SPCChart_ActiveDynamicCondition(object sender, BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ActiveDCEventArgs csea)
            {
 	            //throw new NotImplementedException();
            }



            private void bTitPanelInformation_MinimizedPanel(object sender, System.EventArgs e)
            {
               this.bplInformation.Width = 300;
               BTitlePanel bTitlePanel = this.bplInformation.Controls[0] as BTitlePanel;
               bTitlePanel.Dock = DockStyle.Fill;             
            }

            private void bTitPanelInformation_MaximizedPanel(object sender, System.EventArgs e)
            {
                this.bplInformation.Width = 30;
                BTitlePanel bTitlePanel = this.bplInformation.Controls[0] as BTitlePanel;
                bTitlePanel.Dock = DockStyle.Fill;
            }

		#endregion
	}
}
