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
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;


using BISTel.eSPC.Common;
using Steema.TeeChart.Styles;



namespace BISTel.eSPC.Page.Common
{

    public partial class SPCControlChartUC : BasePageUCtrl
    {
       
       
        #region ::: Field

        protected CommonUtility _ComUtil = new CommonUtility();
        
        protected eSPCWebService.eSPCWebService _wsSPC = null;
        protected MultiLanguageHandler _mlthandler;        
                
        protected SourceDataManager _dataManager = null;
        protected Initialization _Initialization=null;   
             
        protected SortedList _slParamColumnIndex = null;
        protected SortedList _slSpcModelingIndex = null;
        protected LinkedList _llstSearchCondition = null;
        protected LinkedList _llstChartSearchCondition = new LinkedList();  
        protected LinkedList _llstValue = null;        
        protected LinkedList _llstSpread = null;
        protected LinkedList _llstChart = new LinkedList();
        
           
        protected List<string> _lstDefaultChart = null;

        protected DataSet dsChart = null;
        protected DataSet dsParam = null;
        SessionData _SessionData;
        List<string> _lstRawColumn = null;
        LinkedList _llstChartSeriesVisibleType = null;
       
        
        int _iKey = 0;
        int _iValue = 0;

        string _paramAlias = string.Empty;
        string _productID = string.Empty;
        string _lotID = string.Empty;
        string _substrateID = string.Empty;
        string _area = string.Empty;
        string _eqpID = string.Empty;
        string _defaultChart = string.Empty;
        string _spcModel = string.Empty;
        string _eqpModel = string.Empty;
        string _line = string.Empty;
        string _complex_yn= string.Empty;
        string _Pp = string.Empty;
        string _Ppk = string.Empty;
        
        
        

        
		#endregion

		#region ::: Properties



        public List<string> lstDefaultChart
        {
            set
            {
                this._lstDefaultChart = value;
            }
            get
            {
                return this._lstDefaultChart;
            }
        }


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


        public string PARAM_ALIAS
        {
            set { this._paramAlias = value; }
            get { return this._paramAlias; }
        }

        public string PRODUCT_ID
        {
            set { this._productID = value; }
            get { return this._productID; }
        }

        public string LOT_ID
        {
            set { this._lotID = value; }
            get { return this._lotID; }
        }


        public string EQP_ID
        {
            set { this._eqpID = value; }
            get { return this._eqpID; }
        }


        public string SUBSTRATE_ID
        {
            set { this._substrateID = value; }
            get { return this._substrateID; }
        }

        public string AREA
        {
            set { this._area = value; }
            get { return this._area; }
        }

        public string SPC_MODEL
        {
            set { this._spcModel = value; }
            get { return this._spcModel; }
        }

        public string EQP_MODEL
        {
            set { this._eqpModel = value; }
            get { return this._eqpModel; }
        }


        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        public DataSet DataSetPrameter
        {
            get { return dsParam; }
            set { dsParam = value; }
        }

        public string LINE
        {
            get { return _line; }
            set { _line = value; }
        }


        public List<string> lstRawColumn
        {
            get { return _lstRawColumn; }
            set { _lstRawColumn = value; }
        }

        public string complex_yn
        {
            get { return _complex_yn; }
            set { _complex_yn = value; }
        }


        public string Pp
        {
            set
            {
                this._Pp = value;
            }
            get
            {
                return this._Pp;
            }
        }

        public string Ppk
        {
            set
            {
                this._Ppk = value;
            }
            get
            {
                return this._Ppk;
            }
        }
        
		#endregion

		#region ::: Constructor
        public SPCControlChartUC()
        {
            _wsSPC =new BISTel.eSPC.Page.eSPCWebService.eSPCWebService();
            this._mlthandler = MultiLanguageHandler.getInstance();
            _dataManager = new SourceDataManager();
            _Initialization = new Initialization();
            this._Initialization.InitializePath();
            _llstSearchCondition = new LinkedList();    
                    
            InitializeComponent();                                                                    
        }
       
        #endregion

		#region ::: Override Method

        
        #endregion

		#region ::: PageLoad & Initialize

		public void InitializePage()
		{            
		    this.InitializeCode();		   
			this.InitializeDataButton();
			this.InitializeBSpread();			
			this.InitializeLayout();
            this.InitializeChart();
		}

        private void InitializeCode()
        {
            //SPC Control Chart List
             LinkedList lk = new LinkedList();
            lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CONTROL_CHART);
            dsChart = this._wsSPC.GetCodeData(lk.GetSerialData());

            _llstChart.Clear();
            _llstChart = GetDefaultChart(); 
            
            
            ////Chart Search Condtion   
            lk.Clear();
            lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CHART_SERARCH_CONDITION);
            DataSet dsSearchCondition = this._wsSPC.GetCodeData(lk.GetSerialData());

            DataTable dt = new DataTable();
            if (complex_yn.Equals("N"))
            {
                DataRow[] drSeletItems = dsSearchCondition.Tables[0].Select("CODE <>'POINT'");
                dt =dsSearchCondition.Tables[0].Clone();
                foreach(DataRow dr in  drSeletItems)
                {
                    dt.ImportRow(dr);
                }                
            }else
            dt = dsSearchCondition.Tables[0];

            this.bCheckCombo1.DataSource = dt;
            this.bCheckCombo1.DisplayMember ="description";
            this.bCheckCombo1.ValueMember = "description";

            _llstChartSeriesVisibleType = new LinkedList();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strKey = dt.Rows[i]["description"].ToString();
                string strDefaultCol = dt.Rows[i]["default_col"].ToString();                
                if (strDefaultCol.Equals("Y"))
                {
                    this.bCheckCombo1.chkBox.SetItemChecked(i, true);
                    _llstChartSeriesVisibleType.Add(strKey, true);                    
                }
                else
                {
                    _llstChartSeriesVisibleType.Add(strKey, false);
                }
                
              
            }         
                        
          
            
        }
        		    
        public void InitializeChart(){                               
            _dataManager.RawDataTable = this.DataSetPrameter.Tables[0];
            int iChart = _llstChart.Count;
            int iHeight = 300;

            if(iChart==2)
                iHeight = (this.splitContainer1.Panel1.Height-25) / 2;            
           
            this.pnlChart.Controls.Clear();
            ControlChart chartBase = null;
            for (int i = _llstChart.Count-1; i >= 0; i--)
            {
                string strKey = _llstChart.GetKey(i).ToString();
                string strValue = _llstChart.GetValue(i).ToString();

                chartBase = new ControlChart(_dataManager);
                chartBase.Title = strValue;
                chartBase.Name = strKey;
                chartBase.Height=iHeight;
                chartBase.URL= this.URL;                
                chartBase.SPCChartTitlePanel.BssClass = "TitlePanel";                             
                chartBase.ContextMenu = this.bbtnListChart.ContextMenu;
                chartBase.Dock = System.Windows.Forms.DockStyle.Top;
                chartBase.SPCChartTitlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
                chartBase.SPCChart.ClickSeries += new Steema.TeeChart.TChart.SeriesEventHandler(SPCChart_ClickSeries);
                chartBase.ParentControl = this.pnlChart;
                chartBase.SettingXBarInfo(strKey, Definition.CHART_COLUMN.TIME, lstRawColumn, _llstChartSeriesVisibleType);
                chartBase.SPCChart.Chart.Legend.CheckBoxes=false;
                if (!chartBase.DrawSPCChart()) continue;                                                                         
                this.pnlChart.Controls.Add(chartBase);                                                                     
            }
                                          
        }

        public void InitialUserInfo()
        {
        }

        public void InitializeLayout()
        {           
            this.bsprData.ActiveSheet.FrozenColumnCount = 0;
            this.bsprData.ActiveSheet.FrozenRowCount = 0; 
            
            if(complex_yn.Equals("Y"))
            {
                this.bShowRawData.Checked=true;                
            }  
            else
                this.bShowRawData.Checked =false;                
                
                
                    
        }

        public void InitializeDataButton()
        {
            this._slSpcModelingIndex = this._Initialization.InitializeButtonList(this.bbtnListChart, Definition.PAGE_KEY_SPC_CHART_UC, Definition.BUTTONLIST_KEY_CHART, this.sessionData);                      
        }
		
        public void InitializeBSpread()
        {
            this._slParamColumnIndex = new SortedList();

            this._slParamColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprData, Definition.PAGE_KEY_SPC_CHART_UC, false, Definition.PAGE_KEY_SPC_CHART_UC_HEADER_CHART_INFOMATION_KEY);            
            this.bsprData.UseHeadColor = true;
            this._Initialization.SetCheckColumnHeader(this.bsprData, 0);

            //-------------------------------------------------------------------
            string strKey = this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.INFO_KEY);
            this._iKey = (int)this._slParamColumnIndex[strKey];

            string strValue = this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.INFO_VALUE);
            this._iValue = (int)this._slParamColumnIndex[strValue];
            
            MakeInfomationDataTable();
            
        }
           
        #endregion

		#region ::: User Defined Method.	
		
        private LinkedList GetDefaultChart()
        {
        
             LinkedList _llstDefaultChart = new LinkedList();
             _llstDefaultChart.Clear();
            foreach (DataRow dr in dsChart.Tables[0].Rows)
            {
                string strkey = dr["CODE"].ToString();
                for (int i = 0; i < lstDefaultChart.Count; i++)
                {
                    if (dr["CODE"].ToString() == lstDefaultChart[i].ToString())
                    {
                        _llstDefaultChart.Add(strkey, dr["DESCRIPTION"].ToString());
                    }
                }
            }                 
            return  _llstDefaultChart;   
        }
        
				
		private void MakeInfomationDataTable()
		{
		
            //_llstSpread =new LinkedList();
		    
            //DataRow dr =null;
            //List<string> lstRow =null;
            //DataTable dtInfo =new DataTable();		    
            //dtInfo.Columns.Add(Definition.SpreadHeaderColKey.INFO_KEY, typeof(string));
            //dtInfo.Columns.Add(Definition.SpreadHeaderColKey.INFO_VALUE, typeof(string));
                        
            //dr = dtInfo.NewRow();
            //dr[Definition.SpreadHeaderColKey.INFO_KEY]=Definition.ChartInfomationData.INFORMATION;
            //dr[Definition.SpreadHeaderColKey.INFO_VALUE] = null;            
            //dtInfo.Rows.Add(dr);
            
            //dr = dtInfo.NewRow();
            //dr[Definition.SpreadHeaderColKey.INFO_KEY] = Definition.ChartInfomationData.CHART_CONTEXT;
            //dr[Definition.SpreadHeaderColKey.INFO_VALUE] = null;
            //dtInfo.Rows.Add(dr);

            //_llstSpread.Add((int)enum_ChartInfomationData.INFORMATION, Definition.ChartInfomationData.INFORMATION);
            //_llstSpread.Add((int)enum_ChartInfomationData.CHART_CONTEXT, Definition.ChartInfomationData.CHART_CONTEXT);

            //lstRow=CommonChart.ROW_CHART_CONTEXT;
            //for(int i=0; i<lstRow.Count; i++){
       
            //    int iKey = (int)enum_ChartInfomationData.CHART_CONTEXT + (i + 1);
            //    _llstSpread.Add(iKey, lstRow[i]);

            //    dr = dtInfo.NewRow();
            //    dr[Definition.SpreadHeaderColKey.INFO_KEY] = lstRow[i];

            //    switch (iKey)
            //    {
            //        case (int)enum_ChartInfomationData.PRODUCT_ID:
            //            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = PRODUCT_ID;
            //            break;

            //        //case (int)enum_ChartInfomationData.EQP_MODEL:
            //        //    dr[Definition.SpreadHeaderColKey.INFO_VALUE] = EQP_MODEL;
            //        //    break;

            //        case (int)enum_ChartInfomationData.SPC_MODEL:
            //            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = SPC_MODEL;
            //            break;

            //        case (int)enum_ChartInfomationData.STEPSEQ:
            //            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = SUBSTRATE_ID;
            //            break;

            //        case (int)enum_ChartInfomationData.LINE:
            //            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = LINE;
            //            break;

            //        case (int)enum_ChartInfomationData.AREA:
            //            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = AREA;
            //            break;
                        
            //        default:
            //            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = null;
            //            break;
            //    }
            //    dtInfo.Rows.Add(dr);                                
            //}

            //dr = dtInfo.NewRow();
            //dr[Definition.SpreadHeaderColKey.INFO_KEY] = Definition.ChartInfomationData.CHART_DATA;
            //dr[Definition.SpreadHeaderColKey.INFO_VALUE] = null;
            //dtInfo.Rows.Add(dr);

            //_llstSpread.Add((int)enum_ChartInfomationData.CHART_DATA, Definition.ChartInfomationData.CHART_DATA);

            //lstRow = CommonChart.ROW_CHART_DATA;
            //for (int i = 0; i < lstRow.Count; i++)
            //{
            //    dr = dtInfo.NewRow();
            //    dr[Definition.SpreadHeaderColKey.INFO_KEY] = lstRow[i];
            //    dr[Definition.SpreadHeaderColKey.INFO_VALUE] = null;               
            //    dtInfo.Rows.Add(dr);

            //    _llstSpread.Add((int)enum_ChartInfomationData.CHART_DATA + (i + 1), lstRow[i]);
            //}

            //dr = dtInfo.NewRow();
            //dr[Definition.SpreadHeaderColKey.INFO_KEY] = Definition.ChartInfomationData.DATA_RELATED;
            //dr[Definition.SpreadHeaderColKey.INFO_VALUE] = null;
            //dtInfo.Rows.Add(dr);

            //_llstSpread.Add((int)enum_ChartInfomationData.DATA_RELATED, Definition.ChartInfomationData.DATA_RELATED);
            
            //lstRow = CommonChart.ROW_DATA_RELATED;
            //for (int i = 0; i < lstRow.Count; i++)
            //{
            //    int iKey = (int)enum_ChartInfomationData.DATA_RELATED + (i + 1);
            //    _llstSpread.Add(iKey, lstRow[i]);
                
            //    dr = dtInfo.NewRow();
            //    dr[Definition.SpreadHeaderColKey.INFO_KEY] = lstRow[i];
                
            //    switch(iKey)
            //    {
            //        case (int)enum_ChartInfomationData.LOT_ID:
            //            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = LOT_ID;
            //            break;

            //        case (int)enum_ChartInfomationData.SUBSTRATE_ID:
            //            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = SUBSTRATE_ID;
            //            break;

            //        //case (int)enum_ChartInfomationData.EQP_MODEL:
            //        //    dr[Definition.SpreadHeaderColKey.INFO_VALUE] = EQP_ID;
            //        //    break;                                              
            //        default:
            //            dr[Definition.SpreadHeaderColKey.INFO_VALUE] = null;
            //        break;
            //    }
                
            //    dtInfo.Rows.Add(dr);
                
                
                
            //}
            
            //this.bsprData.DataSource = dtInfo;             
            //this.bsprData.ActiveSheet.ColumnHeader.Visible=false;
                                    
            //this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.INFORMATION, 0].ColumnSpan=2;
            //this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.CHART_CONTEXT, 0].ColumnSpan = 2;
            //this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.CHART_DATA, 0].ColumnSpan = 2;
            //this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.DATA_RELATED, 0].ColumnSpan = 2;            
            
            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.INFORMATION].Font = new Font("Gulim", 10, FontStyle.Bold);
            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.CHART_CONTEXT].Font = new Font("Gulim", 9, FontStyle.Bold);
            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.CHART_DATA].Font = new Font("Gulim", 9, FontStyle.Bold);
            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.DATA_RELATED].Font = new Font("Gulim", 9, FontStyle.Bold);
            
            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.INFORMATION].ForeColor = Color.White;
            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.CHART_CONTEXT].ForeColor = Color.White;
            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.CHART_DATA].ForeColor = Color.White;
            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.DATA_RELATED].ForeColor = Color.White;

            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.INFORMATION].BackColor = Color.SlateBlue;
            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.CHART_CONTEXT].BackColor = Color.SteelBlue;
            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.CHART_DATA].BackColor = Color.SteelBlue;
            //this.bsprData.ActiveSheet.Rows[(int)enum_ChartInfomationData.DATA_RELATED].BackColor = Color.SteelBlue;

            //this.bsprData.ActiveSheet.Columns[0].BackColor = Color.WhiteSmoke;
            //this.bsprData.ActiveSheet.Columns[1].BackColor = Color.White;
            //this.bsprData.ActiveSheet.Columns[0].Width=80;
            //this.bsprData.ActiveSheet.Columns[1].Width=150;

		}
		
		
		/// <summary>
		/// series클릭시 값 변경
		/// </summary>
		/// <param name="row"></param>
        private void PROC_spreadReSet(DataRow row)
        {
				 
            //for(int i=0; i<_llstSpread.Count; i++)
            //{
            //    switch(i)
            //    {
            //        case (int)enum_ChartInfomationData.UPPER_CONTROL:		            
            //            this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.UPPER_CONTROL,1].Text= row[Definition.CHART_COLUMN.MEAN_UCL].ToString();
            //            break;

            //        case (int)enum_ChartInfomationData.LOWER_CONTROL:
            //            this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.LOWER_CONTROL, 1].Text = row[Definition.CHART_COLUMN.MEAN_LCL].ToString();
            //            break;

            //        case (int)enum_ChartInfomationData.CENTER_LINE:
            //            this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.CENTER_LINE, 1].Text = row[Definition.CHART_COLUMN.MEAN_CL].ToString();
            //            break;

            //        case (int)enum_ChartInfomationData.UPPER_SPEC:
            //            this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.UPPER_SPEC, 1].Text = row[Definition.CHART_COLUMN.MEAN_USL].ToString();
            //            break;

            //        case (int)enum_ChartInfomationData.LOWER_SPEC:
            //            this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.LOWER_SPEC, 1].Text = row[Definition.CHART_COLUMN.MEAN_LSL].ToString();
            //            break;

            //        case (int)enum_ChartInfomationData.TIME:
            //            this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.TIME, 1].Text = row[Definition.CHART_COLUMN.TIME].ToString();
            //            break;

            //        //case (int)enum_ChartInfomationData.PP:
            //        //    this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.PP, 1].Text = Pp;//row[Definition.CHART_COLUMN.CP].ToString();
            //        //    break;

            //        //case (int)enum_ChartInfomationData.PPK:
            //        //    this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.PPK, 1].Text = Ppk;// row[Definition.CHART_COLUMN.CPK].ToString();
            //        //    break;
            //        case (int)enum_ChartInfomationData.MEAN:
            //            this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.MEAN, 1].Text = row[Definition.CHART_COLUMN.MEAN].ToString();
            //            break;

            //        case (int)enum_ChartInfomationData.RCL:
            //            this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.RCL, 1].Text = row[Definition.CHART_COLUMN.RANGE_CL].ToString();
            //            break;

            //        case (int)enum_ChartInfomationData.RLCL:
            //            this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.RLCL, 1].Text = row[Definition.CHART_COLUMN.RANGE_LCL].ToString();
            //            break;

            //        case (int)enum_ChartInfomationData.RUCL:
            //            this.bsprData.ActiveSheet.Cells[(int)enum_ChartInfomationData.RUCL, 1].Text = row[Definition.CHART_COLUMN.RANGE_UCL].ToString();
            //            break;                                                             		                                                        		                    		        
                        
            //     }		    
		         
            //}
		}

        private BaseSPCChart FindSPCChart(Control ctl)
        {
            Control parentControl = ctl.Parent;

            while (parentControl != null)
            {
                if (parentControl is BaseSPCChart)
                {
                    return parentControl as BaseSPCChart;
                }
                else
                {
                    parentControl = parentControl.Parent;
                }
            }

            return null;
        }

        private void SetCheckComboChecked()
        {

            string[] arr = this.bCheckCombo1.Text.Split(',');
            int icount=0;
            //DataTable dt = (DataTable)this.bCheckCombo1.DataSource;
                        
            _llstChartSeriesVisibleType.Clear();
            for (int i = 0; i < this.bCheckCombo1.Items.Count; i++)
            {
                string strKey = this.bCheckCombo1.chkBox.Items[i].ToString();
                icount=0;
                for(int j=0; j<arr.Length; j++)
                {
                    if(strKey.Equals(arr[j])) icount++;                                        
                }
                if (icount>0)                
                    _llstChartSeriesVisibleType.Add(strKey, true);                
                else                
                    _llstChartSeriesVisibleType.Add(strKey, false);                
            }                     
            
            
            BaseSPCChart baseChart=null;            
            for(int i=0; i<this.pnlChart.Controls.Count; i++)
            {
                  baseChart = (BaseSPCChart)this.pnlChart.Controls[i]; 
                  baseChart.llstChartSeriesVisibleType = _llstChartSeriesVisibleType;               
                  baseChart.ChangeSeriesStyle();
            }
            
        }
        
		#endregion

		#region ::: EventHandler


        private void bCheckCombo1_CheckCombo_Select(object sender, System.EventArgs e)
        {
         
            //SetCheckComboChecked();
        }

        private void bCheckCombo1_TextChanged(object sender, System.EventArgs e)
        {
            
            SetCheckComboChecked();
        }
        
        private void SPCChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {            
            SeriesInfo seriesInfo = ((ChartSeriesGroup)sender).GetSeriesInfo(s);
            if (seriesInfo == null || !seriesInfo.SeriesData.Columns.Contains(CommonChart.COLUMN_NAME_SEQ_INDEX))
            {
                return;
            }

            DataTable dtSeriesInfo = seriesInfo.SeriesData;  
            PROC_spreadReSet(this._dataManager.RawDataTable.Rows[valueIndex]); 
               
        }
        

        private void bbtnListChart_ButtonClick(string name)
        {
            try
            {
                if (name.ToUpper() == Definition.ButtonKey.CALC) 
                {
                    this.ClickButtonCalc();
                }
                else if (name.ToUpper() == Definition.ButtonKey.SELECT_CHART_TO_VIEW)
                {
                    this.ClickButtonChartView();
                }
                else if (name.ToUpper() == Definition.ButtonKey.DEFAULT_CHART)
                {
                    _llstChart.Clear();
                    _llstChart = GetDefaultChart();
                    this.InitializeChart(); //chart 다시 그림   
                }      
                else if (name.ToUpper() == Definition.ButtonKey.CONFIGURATION)
                {
                    this.ClickButtonConfiguration();
                }                
                else if (name.ToUpper() == Definition.ButtonKey.CHART_DATA)
                {
                    this.ClickButtonShowChartData();
                }                                                                      
                else
                {
                    this.bsprData.ContextMenuAction(name);
                }

            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }


        private void ClickButtonCalc()
        {        
          
        }

        private void ClickButtonChartView()
        {
            Popup.ChartViewSelectedPopup chartViewPop = new Popup.ChartViewSelectedPopup();
            chartViewPop.URL= this.URL;
            chartViewPop.SessionData = this.sessionData;
            chartViewPop.DataSetChart=dsChart; //Chart 종류   
            chartViewPop.llstDefaultChart = _llstChart;                                                   
            chartViewPop.InitializePopup();                        
            DialogResult result = chartViewPop.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                _llstChart.Clear();
                _llstChart = chartViewPop.llstResultListChart;
                this.InitializeChart(); //chart 다시 그림                
            }            
        }

        private void ClickButtonConfiguration()
        {

        }

        private void ClickButtonShowChartData()
        {
            //Popup.ChartDataPopup chartDataPop = new Popup.ChartDataPopup();
            //chartDataPop.URL = this.URL;
            //chartDataPop.SessionData = this.sessionData;
            //chartDataPop.DataTableParam = _dataManager.RawDataTable;            
            //chartDataPop.InitializePopup();
            //DialogResult result = chartDataPop.ShowDialog(this);
            //if (result == DialogResult.OK)
            //{                
            //}            

        }        
        
        

       
		#endregion
        
        
    }
}
