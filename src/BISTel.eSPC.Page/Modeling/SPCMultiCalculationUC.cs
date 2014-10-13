using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;
using System.Collections;
using BISTel.eSPC.Page.Common;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.Modeling
{
    public partial class SPCMultiCalculationUC : BasePageUCtrl
    {
        #region :::Field

        public delegate void ClickCloseEventDelegate();
        public event ClickCloseEventDelegate ClickCloseEvent;
        
        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        LinkedList _llstSearchCondition = new LinkedList();

        BISTel.eSPC.Common.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.CommonUtility _ComUtil;

        MultiLanguageHandler _mlthandler;
        //SessionData _SessionData;

        private SortedList _slParamColumnIndex = new SortedList();
        private SortedList _slSpcModelingIndex = new SortedList();

        //private string _sLineRawid;
        //private string _sLine;
        //private string _sAreaRawid;
        //private string _sArea;
        //private string _sEQPModel;
        //private string _sSPCModelRawid;
        //private string _sSPCModelName;

        private DataSet _dsSPCModeData = new DataSet();
        private DataTable _dtTableOriginal = new DataTable();
        private DataTable _dtSPCMultiCalcList = new DataTable();
        private DataTable _dtTableCalcOriginal = new DataTable();

        ChartInterface mChartVariable;
        SourceDataManager _dataManager = null;
        //DataSet mDSChart = null;
        //LinkedList mllstChartSeriesVisibleType = null;

        private string mStartTime = string.Empty;
        private string mEndTime = string.Empty;

        //DefaultRawChart chartBase;
        //DefaultCalcChart chartCalcBase;

        //int iValueIndex = -1;

       
        private string _sModelConfigRawIDforSave;
        private string _sMainYNfroSave;
        private ArrayList _sParamAlias;

        private string _OutOfSpec = "N";
        //LinkedList mllstContextType;
        //private string _sUSL = "";
        //private string _sLSL = "";

        //ContextMenu ctSpread;
        ArrayList arrOutlierList = new ArrayList();

        private string _sChartType = string.Empty;
        LinkedList _mllstChart = new LinkedList();
        

        private List<int> IstTempForSetOulierPreview = new List<int>();

        //private int _selectedRawID = -1;

        //SPC-704 MultiCalculate
        private ArrayList _sMulModelConfigRawID = new ArrayList();
        private ArrayList _sMulValueModelConfiRawID = new ArrayList();
        private ArrayList _SMulParamAlias = new ArrayList();

        private int _selectedRawID = -1;
        private string _sMainYNforSave;

        DataSet mDSChart = null;
        bool isWaferColumn = false;
        public LinkedList _mllstContextType = null;
        DataTable _dtDataSource = null;
        SortedList alSelect;
        public LinkedList _llColumnMapping = null;

        double _raw_lowerFilter = Double.NaN;
        double _raw_upperFilter = Double.NaN;

        enum iColIdx
        {
            SELECT,
            RAWID,
            MAIN_YN,
            SAVE_YN,
            OUT_OF_SPEC
        }
        enum OutlierLimitType
        {
            CONSTANT,
            VALUE,
            SIGMA,
            CONTROL
        }

        #endregion

        #region :::Properties

       
        //SPC-704
        public ArrayList SMulModelConfigRawID
        {
            get { return this._sMulModelConfigRawID; }
            set { this._sMulModelConfigRawID = value; }
        }

        public ArrayList SMulValueModelConfigRawID
        {
            get { return this._sMulValueModelConfiRawID; }
            set { this._sMulValueModelConfiRawID = value; }
        }

        public SessionData SessionData
        {
            get { return sessionData; }
            set { sessionData = value; }
        }

        public ArrayList SParamAlias
        {
            get { return this._sParamAlias; }
            set { this._sParamAlias = value; }
        }

        public DataTable dtDataSource
        {
            get { return _dtDataSource; }
            set { this._dtDataSource = value; }
        }

        #endregion

        //SPC-704 multical
        public void InitializeMultiPopup()
        {
            this.InitializeMultiLayout();
            this.InitializePopup();
        }

        //SPC-704 multical
        public void InitializeMultiLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.TITLE_SPC_MULTICALCULATION);
        }


        public SPCMultiCalculationUC()
        {
            InitializeComponent();
            this._mlthandler = MultiLanguageHandler.getInstance();
        }

        //public override void PageInit()
        //{
        //    BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;

        //    this.KeyOfPage = "BISTel.eSPC.Page.Modeling.SPCMultiCalculationPopup";
        //    this.InitializePage();
        //}

        public void InitializePage()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();

            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._SpreadUtil = new BSpreadUtility();
            this._ComUtil = new CommonUtility();

            this._dataManager = new SourceDataManager();

            this.InitializeBSpread();
            this.InitializeLayout();
            this.InitializeColumnMapping();
        }

        private void InitializeColumnMapping()
        {
            this._llColumnMapping = new LinkedList();
            this._llColumnMapping.Add(Definition.MULTICALC_CHART_TYPE.RAW, Definition.AUTOCALC_COLUMN.RAW);
            this._llColumnMapping.Add(Definition.MULTICALC_CHART_TYPE.XBAR, Definition.AUTOCALC_COLUMN.XBAR);
            this._llColumnMapping.Add(Definition.MULTICALC_CHART_TYPE.STDDEV, Definition.AUTOCALC_COLUMN.STDDEV);
            this._llColumnMapping.Add(Definition.MULTICALC_CHART_TYPE.MA, Definition.AUTOCALC_COLUMN.MA);
            this._llColumnMapping.Add(Definition.MULTICALC_CHART_TYPE.MSD, Definition.AUTOCALC_COLUMN.MSD);
            this._llColumnMapping.Add(Definition.MULTICALC_CHART_TYPE.MR, Definition.AUTOCALC_COLUMN.MR);
            this._llColumnMapping.Add(Definition.MULTICALC_CHART_TYPE.EWMA_MEAN, Definition.AUTOCALC_COLUMN.EWMA_MEAN);
            this._llColumnMapping.Add(Definition.MULTICALC_CHART_TYPE.EWMA_RANGE, Definition.AUTOCALC_COLUMN.EWMA_RANGE);
            this._llColumnMapping.Add(Definition.MULTICALC_CHART_TYPE.EWMA_STDDEV, Definition.AUTOCALC_COLUMN.EWMA_STDDEV);
            this._llColumnMapping.Add(Definition.MULTICALC_CHART_TYPE.RANGE, Definition.AUTOCALC_COLUMN.RANGE);
        }

        public void InitializeLayout()
        {
            //spc-1281
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_MODELING, Definition.CONFIG_USE_COMMA, false);

            this.bDtStart.Value = DateTime.Now.AddDays(-7).Subtract(TimeSpan.FromHours(1));
            this.bDtEnd.Value = DateTime.Now.Subtract(TimeSpan.FromHours(-1));

            this.bcboCalcSide.Items.Clear();
            this.bcboCalcSide.Items.AddRange(Definition.CALCULATION_SIDED);
            this.bcboCalcSide.SelectedIndex = 0;
        }

        public void InitializeBSpread()
        {
            this.bsprMulData.ClearHead();
            this.bsprMulData.AddHeadComplete();
            this.bsprMulData.UseSpreadEdit = false;
            this.bsprMulData.AutoGenerateColumns = true;
            this.bsprMulData.UseAutoSort = false;
            this.bsprMulData.UseGeneralContextMenu = false;


            this.bsprMulValue.ClearHead();
            this.bsprMulValue.AddHeadComplete();
            this.bsprMulValue.UseSpreadEdit = false;
            this.bsprMulValue.AutoGenerateColumns = true;
            this.bsprMulValue.UseAutoSort = false;
            //this.bsprMulValue.Locked = true;
            this.bsprMulValue.UseGeneralContextMenu = false;
            
        }

        private void InitializeCode()
        {
            LinkedList lk = new LinkedList();
            lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CONTROL_CHART);
            lk.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            this.mDSChart = this._wsSPC.GetCodeData(lk.GetSerialData());

            if (!DataUtil.IsNullOrEmptyDataSet(this.mDSChart) && this.bcboChartType.Items.Count == 0)
            {
                _ComUtil.SetBComboBoxData(this.bcboChartType, this.mDSChart, COLUMN.CODE, COLUMN.CODE, "", true);

                foreach (DataRow dr in this.mDSChart.Tables[0].Rows)
                {
                    if (!this._mllstChart.Contains(dr[COLUMN.CODE]))
                    {
                        this._mllstChart.Add(dr[COLUMN.CODE], dr[COLUMN.DESCRIPTION]);
                    }
                }
            }

            LinkedList _llstData = new LinkedList();
            DataSet dsContextType = _wsSPC.GetContextType(_llstData.GetSerialData());
            //this.mllstContextType = CommonPageUtil.SetContextType(dsContextType);
        }

        public void InitializePopup()
        {
            this.InitializePage();
            this.ConfigListDataBindingPopup();
        }

        public void ConfigListDataBindingPopup()
        {
            try
            {
                //this.MsgShow(COMMON_MSG.Query_Data);

                _llstSearchCondition.Clear();

                //SPC-704
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < _sMulModelConfigRawID.Count; i++)
                {
                    if (i == 0)
                    {
                        sb.Append(this._sMulModelConfigRawID[i]);
                    }
                    else
                    {
                        sb.Append("," + this._sMulModelConfigRawID[i]);
                    }
                }

                _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, sb.ToString());
                _llstSearchCondition.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

                _dsSPCModeData = _wsSPC.GetSPCMulCalcModelDataPopup(_llstSearchCondition.GetSerialData());

                if (!DSUtil.CheckRowCount(_dsSPCModeData, TABLE.MODEL_MST_SPC))
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                DataTable dtMaster = _dsSPCModeData.Tables[TABLE.MODEL_MST_SPC];
                DataTable dtConfig = _dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
                DataTable dtContext = _dsSPCModeData.Tables[TABLE.MODEL_CONTEXT_MST_SPC];

                //#01. SPC Model Chart List를 위한 Datatable 생성
                DataTable dtSPCModelChartList = new DataTable();

                dtSPCModelChartList.Columns.Add(COLUMN.SELECT, typeof(Boolean));
                dtSPCModelChartList.Columns.Add(COLUMN.CHART_ID);
                dtSPCModelChartList.Columns.Add(COLUMN.SPC_MODEL_NAME);
                dtSPCModelChartList.Columns.Add(COLUMN.PARAM_ALIAS);
                dtSPCModelChartList.Columns.Add(COLUMN.MAIN_YN);
                
                foreach (DataRow drContext in dtContext.Rows)
                {
                    if (!dtSPCModelChartList.Columns.Contains(drContext[COLUMN.CONTEXT_KEY].ToString()))
                    {
                        dtSPCModelChartList.Columns.Add(drContext[COLUMN.CONTEXT_KEY].ToString());
                    }
                }

                dtSPCModelChartList.Columns.Add(COLUMN.UPPER_SPEC);
                dtSPCModelChartList.Columns.Add(COLUMN.LOWER_SPEC);
                dtSPCModelChartList.Columns.Add(COLUMN.RAW_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.RAW_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.INTERLOCK_YN);
                dtSPCModelChartList.Columns.Add(COLUMN.CREATE_BY);
                dtSPCModelChartList.Columns.Add(COLUMN.CREATE_DTTS);

                //#02. CONFIG MST에 생성된 CONTEXT COLUMN에 Data 입력
                foreach (DataRow drConfig in dtConfig.Rows)
                {
                    DataRow drChartList = dtSPCModelChartList.NewRow();

                    drChartList[COLUMN.CHART_ID] = drConfig[COLUMN.RAWID].ToString();
                    drChartList[COLUMN.PARAM_ALIAS] = drConfig[COLUMN.PARAM_ALIAS].ToString();
                    drChartList[COLUMN.MAIN_YN] = drConfig[COLUMN.MAIN_YN].ToString();

                    DataRow[] drMaster = dtMaster.Select(string.Format("{0} = '{1}'", COLUMN.RAWID, drConfig[COLUMN.MODEL_RAWID]));
                    drChartList[COLUMN.SPC_MODEL_NAME] = drMaster[0][COLUMN.SPC_MODEL_NAME].ToString();

                    DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfig[COLUMN.RAWID]));

                  
                    foreach (DataRow drContext in drContexts)
                    {
                        //2009-11-27 bskwon 추가 : Sub Model 상속 구조가 아닌경우 예외처리
                        drChartList[drContext[COLUMN.CONTEXT_KEY].ToString()] = drContext[COLUMN.CONTEXT_VALUE].ToString();
                    }

                    //MODEL 정보                
                    drChartList[COLUMN.UPPER_SPEC] = drConfig[COLUMN.UPPER_SPEC].ToString();
                    drChartList[COLUMN.LOWER_SPEC] = drConfig[COLUMN.LOWER_SPEC].ToString();
                    drChartList[COLUMN.RAW_UCL] = drConfig[COLUMN.RAW_UCL].ToString();
                    drChartList[COLUMN.RAW_LCL] = drConfig[COLUMN.RAW_LCL].ToString();
                    drChartList[COLUMN.INTERLOCK_YN] = _ComUtil.NVL(drConfig[COLUMN.INTERLOCK_YN], "N", true);

                    drChartList[COLUMN.CREATE_BY] = drConfig[COLUMN.CREATE_BY].ToString();
                    drChartList[COLUMN.CREATE_DTTS] = drConfig[COLUMN.CREATE_DTTS] == DBNull.Value ? null : DateTime.Parse(drConfig[COLUMN.CREATE_DTTS].ToString()).ToString(Definition.DATETIME_FORMAT_MS).ToString();

                    dtSPCModelChartList.Rows.Add(drChartList);
                }

                dtSPCModelChartList.AcceptChanges();

                this.bsprMulData.DataSet = dtSPCModelChartList;
                this._dtTableOriginal = dtSPCModelChartList.Copy();

                this.bsprMulData.Locked = true;
                this.bsprMulData.ActiveSheet.Columns[(int)iColIdx.SELECT].Locked = false;
                this.bsprMulData.ActiveSheet.Columns[(int)iColIdx.RAWID].Visible = true;
                //this.bsprMulData.columnInformation.ColumnDatas[(int)iColIdx.RAWID].ColumnAttributeValue = PeakPerformance.Client.BISTelControl.ColumnAttribute.Key;

                FarPoint.Win.Spread.CellType.TextCellType tc = new FarPoint.Win.Spread.CellType.TextCellType();
                tc.MaxLength = 1024;

                //Column Size 조절
                for (int cIdx = 0; cIdx < this.bsprMulData.ActiveSheet.Columns.Count; cIdx++)
                {
                    this.bsprMulData.ActiveSheet.ColumnHeader.Cells[0, cIdx].Text = dtSPCModelChartList.Columns[cIdx].ToString();
                    this.bsprMulData.ActiveSheet.Columns[cIdx].Width = this.bsprMulData.ActiveSheet.Columns[cIdx].GetPreferredWidth();

                    if (this.bsprMulData.ActiveSheet.Columns[cIdx].Width > 150)
                        this.bsprMulData.ActiveSheet.Columns[cIdx].Width = 150;

                    if (this.bsprMulData.ActiveSheet.Columns[cIdx].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.TextCellType))
                    {
                        this.bsprMulData.ActiveSheet.Columns[cIdx].CellType = tc;
                    }
                }

                if (this.bsprMulData.ActiveSheet.Rows.Count > 0)
                {
                    this.bsprMulData.ActiveSheet.Cells[0, 0].Value = true;
                }


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


        //SPC-875 by Louis
        private void bsprData_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //SortedList alSelectedRows = this.bsprMulData.GetSelectedRows();
            if ( alSelect.Count > 0)
            {
                if (!(bool)this.bsprMulData.GetCellValue(e.Row,0))
                {
                    for (int i = 0; i < alSelect.Count; i++)
                    {
                        this.bsprMulData.ActiveSheet.SetText((int)alSelect.GetByIndex(i), 0, "False");
                    }
                }
                else
                {
                    for (int i = 0; i < alSelect.Count; i++)
                    {
                        this.bsprMulData.ActiveSheet.SetText((int)alSelect.GetByIndex(i), 0, "True");
                    }
                }
            }
            alSelect.Clear();
            
        }

        

        private void bbtnCalculation_Click(object sender, EventArgs e)
        {

            _dtSPCMultiCalcList.Clear();
            _dtSPCMultiCalcList.Columns.Clear();
            _sMulValueModelConfiRawID.Clear();
            ArrayList alCheckRowIndex = this.bsprMulData.GetCheckedList(0);
            int iRowIndex = -1;
            for (int i = 0; i < (int)alCheckRowIndex.Count; i++)
            {

                iRowIndex = (int)alCheckRowIndex[i];
                string ConfigRawID = this.bsprMulData.GetCellText(iRowIndex, (int)iColIdx.RAWID);
                this.SMulValueModelConfigRawID.Add(ConfigRawID);
            }


            this._dataManager = new SourceDataManager();

            if (bsprMulData.ActiveSheet.RowCount <= 0) return;

            if (alCheckRowIndex.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                return;
            }          
            else
            {
                iRowIndex = (int)alCheckRowIndex[0];
            }

            if (this.btxtSigma.Text.Trim().Length == 0)
            {
                if (this.bcboOption.Text == "SIGMA")
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_SIGMA", null, null);
                    return;
                }
                else if (this.bcboOption.Text == "%")
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_PERCENT", null, null);
                    return;
                }
                else if (this.bcboOption.Text == "CONSTANT")
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_CONSTANT", null, null);
                    return;
                }
            }


            if (bcboChartType.Text == "RAW" || bcboChartType.Text == "X-BAR")
            {
                if (!bchkThresholdOff.Checked)
                {
                    if (btxtThreshold.Text == "")
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_THRESHOLD_CONDITION", null, null);
                        return;
                    }
                }
            }

            if (bchkSampling.Checked)
            {
                if (string.IsNullOrEmpty(btxtSampleCnt.Text) ||
                    string.IsNullOrEmpty(btxtSamplePeriod.Text) ||
                    (bcboChartType.Text == "RAW" && string.IsNullOrEmpty(btxtWSampleCnt.Text)))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_SAMPLE_CONDITION", null, null);
                    return;
                }

                //spc-1188 by stella : period, sample count의 Max값 먼저 체크후 0 체크.
                if (Convert.ToDouble(btxtSamplePeriod.Text) > 100)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_SAMPLE_PERIOD", null, null);
                    return;
                }

                if (Convert.ToDouble(btxtSampleCnt.Text) > 10000 || (bcboChartType.Text == "RAW" && Convert.ToDouble(btxtWSampleCnt.Text) > 10000))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_CNT", null, null);
                    return;
                }

                if (int.Parse(btxtSampleCnt.Text) == 0 ||
                    int.Parse(btxtSamplePeriod.Text) == 0 ||
                    (bcboChartType.Text == "RAW" && int.Parse(btxtWSampleCnt.Text) == 0))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_CONDITION", null, null);
                    return;
                }
            }

            if (this.bDtStart.Value > this.bDtEnd.Value)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHECK_PERIOD", null, null);
                return;
            }

            this.mChartVariable = new ChartInterface();
            this.mStartTime = CommonPageUtil.CalcStartDate(this.bDtStart.Value.ToString(Definition.DATETIME_FORMAT));
            this.mEndTime = CommonPageUtil.CalcEndDate(this.bDtEnd.Value.ToString(Definition.DATETIME_FORMAT));
            this.mChartVariable.MODEL_CONFIG_RAWID = this.bsprMulData.ActiveSheet.Cells[iRowIndex, 1].Text;
            this.mChartVariable.MAIN_YN = this.bsprMulData.ActiveSheet.Cells[iRowIndex, 4].Text;
            
            this.PROC_DataBinding();
            

        }
        private double[] GetValueListOfChartData(bool containOutlier, DataTable ChartData)
        {
            ArrayList arrOutlierData = new ArrayList();

            if (containOutlier == false)
            {
                //for (int i = 0; i < ChartData.Tables[0].Rows.Count; i++)
                //{
                //    if (this.bsprMulValue.ActiveSheet.Rows[i].BackColor == Color.LightGreen)
                //    {
                //        arrOutlierData.Add(i);
                //    }
                //}
            }

            ArrayList arrData = new ArrayList();
           // DataSet dsTemp = (DataSet)this.bsprMulValue.DataSource;
            string BLOBFieldName = GetBLOBFieldNameFromChartType(this._sChartType);
            if (!string.IsNullOrEmpty(BLOBFieldName))
            {
                for (int i = 0; i < ChartData.Rows.Count; i++)
                {
                    if (arrOutlierData.Contains(i))
                        continue;
                    if (ChartData.Rows[i][BLOBFieldName].ToString().Length > 0)
                    {
                        double dOutPut = 0;
                        bool isParse = double.TryParse(ChartData.Rows[i][BLOBFieldName].ToString(), out dOutPut);

                        if (isParse)
                            arrData.Add(dOutPut);
                    }
                }
            }

            return (double[])arrData.ToArray(typeof(double));
        }
        private string GetBLOBFieldNameFromChartType(string chartType)
        {
            switch (chartType)
            {
                case Definition.CHART_TYPE.RAW:
                    return Definition.BLOB_FIELD_NAME.RAW;
                case Definition.CHART_TYPE.XBAR:
                    return Definition.BLOB_FIELD_NAME.MEAN;
                case Definition.CHART_TYPE.STDDEV:
                    return Definition.BLOB_FIELD_NAME.STDDEV;
                case Definition.CHART_TYPE.RANGE:
                    return Definition.BLOB_FIELD_NAME.RANGE;
                case Definition.CHART_TYPE.EWMA_MEAN:
                    return Definition.BLOB_FIELD_NAME.EWMAMEAN;
                case Definition.CHART_TYPE.EWMA_RANGE:
                    return Definition.BLOB_FIELD_NAME.EWMARANGE;
                case Definition.CHART_TYPE.EWMA_STDDEV:
                    return Definition.BLOB_FIELD_NAME.EWMASTDDEV;
                case Definition.CHART_TYPE.MA:
                    return Definition.BLOB_FIELD_NAME.MA;
                case Definition.CHART_TYPE.MSD:
                    return Definition.BLOB_FIELD_NAME.MSD;
                case Definition.CHART_TYPE.MR:
                    return Definition.BLOB_FIELD_NAME.MR;
                default:
                    return string.Empty;
            }
        }
        private void PROC_DataBinding()
        {
            
            DataSet _ds = null;
            CreateChartDataTable _createChartDT = null;
            DataTable mDTChartData = null;
            _sModelConfigRawIDforSave = "";
            _sMainYNforSave = "";
            this._sChartType = this.bcboChartType.Text;
            try
            {
                DataTable dtMaster = this._dsSPCModeData.Tables[TABLE.MODEL_MST_SPC];
                DataTable dtConfig = this._dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
                DataTable dtContext = this._dsSPCModeData.Tables[TABLE.MODEL_CONTEXT_MST_SPC];

                _dtSPCMultiCalcList.Columns.Add(COLUMN.SELECT, typeof(Boolean));
                _dtSPCMultiCalcList.Columns.Add(COLUMN.CHART_ID);
                //_dtSPCMultiCalcList.Columns.Add(COLUMN.SPC_MODEL_NAME);
                //_dtSPCMultiCalcList.Columns.Add(COLUMN.PARAM_ALIAS);
                _dtSPCMultiCalcList.Columns.Add(COLUMN.MAIN_YN);
                _dtSPCMultiCalcList.Columns.Add(COLUMN.SAVE_YN);
                _dtSPCMultiCalcList.Columns.Add(COLUMN.OUT_OF_SPEC);
                _dtSPCMultiCalcList.Columns.Add(COLUMN.UPPER_SPEC);
                _dtSPCMultiCalcList.Columns.Add(COLUMN.TARGET);
                _dtSPCMultiCalcList.Columns.Add(COLUMN.LOWER_SPEC);

                //int count = 0;
                //foreach (DataRow drContext in dtContext.Rows)
                //{

                //    if (count == 8)
                //        break;
                //    _dtSPCMultiCalcList.Columns.Add(drContext[COLUMN.CONTEXT_KEY].ToString());
                //    count += 1;
                //}              
                                             
                if (_sChartType == "RAW")
                {
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.RAW_UCL);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.RAW_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.RAW_LCL);
                    if (!this.bchkThresholdOff.Checked)
                    {
                        _dtSPCMultiCalcList.Columns.Add(COLUMN.THRESHOLD_UCL);
                        _dtSPCMultiCalcList.Columns.Add(COLUMN.THRESHOLD_TARGET);
                        _dtSPCMultiCalcList.Columns.Add(COLUMN.THRESHOLD_LCL);
                    }
                    
                }
                else if (_sChartType == "X-BAR")
                {
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.UPPER_CONTROL);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.LOWER_CONTROL);
                    if (!this.bchkThresholdOff.Checked)
                    {
                        _dtSPCMultiCalcList.Columns.Add(COLUMN.THRESHOLD_UCL);
                        _dtSPCMultiCalcList.Columns.Add(COLUMN.THRESHOLD_TARGET);
                        _dtSPCMultiCalcList.Columns.Add(COLUMN.THRESHOLD_LCL);
                    }
                    

                }
                else if (_sChartType == "STDDEV")
                {
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.STD_UCL);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.STD_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.STD_LCL);
                    
                }
                else if (_sChartType == "RANGE")
                {
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.RANGE_UCL);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.RANGE_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.RANGE_LCL);
                }
                else if (_sChartType == "EWMA_MEAN")
                {
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.EWMA_M_UCL);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.EWMA_M_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.EWMA_M_LCL);
                }
                else if (_sChartType == "EWMA_RANGE")
                {
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.EWMA_R_UCL);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.EWMA_R_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.EWMA_R_LCL);
                }
                else if (_sChartType == "EWMA_STDDEV")
                {
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.EWMA_S_UCL);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.EWMA_S_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.EWMA_S_LCL);
                }
                else if (_sChartType == "MA")
                {
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.MA_UCL);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.MA_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.MA_LCL);
                }
                else if (_sChartType == "MSD")
                {
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.MS_UCL);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.MS_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.MS_LCL);
                }
                else if (_sChartType == "MR")
                {
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.MR_UCL);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.MR_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(COLUMN.MR_LCL);
                }

                _dtSPCMultiCalcList.Columns.Add(COLUMN.IQR_UCL);
                _dtSPCMultiCalcList.Columns.Add(COLUMN.IQR_LCL);

                
               

                mDTChartData = new DataTable();
                _createChartDT = new CreateChartDataTable();

                this.MsgShow(COMMON_MSG.Query_Data);

                LinkedList lnkChartSearchData = new LinkedList();

                for (int i = 0; i < _sMulValueModelConfiRawID.Count; i++)
                {
                    _OutOfSpec = "N";
                   // _ds.Clear();
                   lnkChartSearchData.Clear();
                   lnkChartSearchData.Add(Definition.DynamicCondition_Condition_key.START_DTTS, this.mStartTime);
                   lnkChartSearchData.Add(Definition.DynamicCondition_Condition_key.END_DTTS, this.mEndTime);
                   lnkChartSearchData.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, _sMulValueModelConfiRawID[i]);
                   _ds = _wsSPC.GetSPCMulControlChartData(lnkChartSearchData.GetSerialData());
                   if (DataUtil.IsNullOrEmptyDataSet(_ds))
                   {
                       this.PROC_DATACalc(mDTChartData, _sMulValueModelConfiRawID[i].ToString());
                       //this.MsgClose();
                       //MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                   }
                   else
                   {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
                       lnkChartSearchData.Add(Definition.DynamicCondition_Condition_key.CONTEXT_KEY_LIST, "");
                       mDTChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, lnkChartSearchData, false);
                       GC.Collect();
                       GC.WaitForPendingFinalizers();

                       if (DataUtil.IsNullOrEmptyDataTable(mDTChartData))
                       {
                           this.PROC_DATACalc(mDTChartData, _sMulValueModelConfiRawID[i].ToString());
                           //this.MsgClose();
                           //MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                       }
                       else
                       {
                           //this.bsprMulValue.DataSet = mDTChartData;
                           //this.mDSChart = (DataSet)bsprMulValue.DataSource;
                          // this.mDSChart = mDTChartData.DataSet;

                           //mDTChartData.Columns.Add(
                           bool result = false;
                           if (_sChartType == Definition.CHART_TYPE.RAW)
                           {
                               if (bchkSampling.Checked)
                               {
                                   //result = chartBase.DrawSPCChart(true, int.Parse(btxtSamplePeriod.Text), int.Parse(btxtSampleCnt.Text));
                                   result = DrawSPCChart(true, int.Parse(btxtSamplePeriod.Text), int.Parse(btxtSampleCnt.Text), int.Parse(btxtWSampleCnt.Text), mDTChartData, _sMulValueModelConfiRawID[i].ToString());
                               }
                               else
                               {
                                   //this.PROC_DATACalc(mDTChartData, _sMulValueModelConfiRawID[i].ToString());
                                   result = DrawSPCChart(mDTChartData, _sMulValueModelConfiRawID[i].ToString());
                               }
                           }
                           else
                           {
                               if (bchkSampling.Checked)
                               {
                                   result = DrawSPCChart(true, int.Parse(btxtSamplePeriod.Text), int.Parse(btxtSampleCnt.Text), mDTChartData, _sMulValueModelConfiRawID[i].ToString());
                               }
                               else
                               {
                                  //result = DrawSPCChart(mDTChartData, _sMulValueModelConfiRawID[i].ToString());
                                   this.PROC_DATACalc(mDTChartData, _sMulValueModelConfiRawID[i].ToString());
                               }
                           }
                       }
                   }
                }
                this.MsgClose();
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CACL_COMPLETE", null, null);
               
                _dtSPCMultiCalcList.AcceptChanges();
                this.bsprMulValue.DataSet = _dtSPCMultiCalcList;
                this._dtTableCalcOriginal = _dtSPCMultiCalcList.Copy();

                //this.bsprMulValue.Locked = true;
                this.bsprMulValue.ActiveSheet.Columns[(int)iColIdx.SELECT].Locked = false;
                this.bsprMulValue.ActiveSheet.Columns[(int)iColIdx.RAWID].Visible = true;
                this.bsprMulValue.ActiveSheet.Columns[(int)iColIdx.OUT_OF_SPEC].Visible = false;

                FarPoint.Win.Spread.CellType.TextCellType tc = new FarPoint.Win.Spread.CellType.TextCellType();
                tc.MaxLength = 1024;

                //Column Size 조절
                for (int cIdx = 0; cIdx < this.bsprMulValue.ActiveSheet.Columns.Count; cIdx++)
                {
                    this.bsprMulValue.ActiveSheet.ColumnHeader.Cells[0, cIdx].Text = _dtSPCMultiCalcList.Columns[cIdx].ToString();
                    this.bsprMulValue.ActiveSheet.Columns[cIdx].Width = this.bsprMulValue.ActiveSheet.Columns[cIdx].GetPreferredWidth();

                    if (this.bsprMulValue.ActiveSheet.Columns[cIdx].Width > 150)
                        this.bsprMulValue.ActiveSheet.Columns[cIdx].Width = 150;

                    if (this.bsprMulValue.ActiveSheet.Columns[cIdx].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.TextCellType))
                    {
                        this.bsprMulValue.ActiveSheet.Columns[cIdx].CellType = tc;
                    }
                }

                //if (this.bsprMulValue.ActiveSheet.Rows.Count > 0)
                //{
                //    this.bsprMulValue.ActiveSheet.Cells[0, 0].Value = true;
                //}

                for (int i = 0; i < bsprMulValue.ActiveSheet.Rows.Count; i++)
                {
                    if (this.bsprMulValue.GetCellText(i, (int)iColIdx.SAVE_YN).Equals("N"))
                    {
                        if (this.bsprMulValue.GetCellText(i,(int)iColIdx.OUT_OF_SPEC).Equals("Y"))
                        {
                            this.bsprMulValue.ActiveSheet.Rows[i].BackColor = Color.SkyBlue;
                            this.bsprMulValue.ActiveSheet.Cells[i, (int)iColIdx.SELECT].Locked = true;
                        }
                        else
                        {
                            this.bsprMulValue.ActiveSheet.Rows[i].BackColor = Color.Pink;
                            this.bsprMulValue.ActiveSheet.Cells[i, (int)iColIdx.SELECT].Locked = true;
                        }
                    }
                }

                this.ControlLimitColumsEnableSetting();
                //this.InitializeCode();
                //this.InitializeChartSeriesVisibleType();
                //this.InitializeChart();
               
                 
           }
        
            catch (Exception ex)
            {
                this.MsgClose();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                this.MsgClose();
            }
        }

        int colIndexUCL = 0;
        int colIndexLCL = 0;
        private bool _bUseComma;

        private void ControlLimitColumsEnableSetting()
        {
            colIndexUCL = 0;
            colIndexLCL = 0;

            string colNameUCL = string.Empty;
            string colNameLCL = string.Empty;

            if (_llColumnMapping != null)
            {
                if (_sChartType == Definition.MULTICALC_CHART_TYPE.XBAR)
                {
                    colNameUCL = COLUMN.UPPER_CONTROL;
                    colNameLCL = COLUMN.LOWER_CONTROL;
                }
                else
                {
                    colNameUCL = _llColumnMapping[_sChartType].ToString() + "_UCL";
                    colNameLCL = _llColumnMapping[_sChartType].ToString() + "_LCL";
                }
            }

            for (int i = 0; i < this.bsprMulValue.ActiveSheet.Columns.Count; i++)
            {
                if (this.bsprMulValue.ActiveSheet.Columns[i].Label.ToString() == colNameUCL)
                {
                    colIndexUCL = i;
                }
                else if (this.bsprMulValue.ActiveSheet.Columns[i].Label.ToString() == colNameLCL)
                {
                    colIndexLCL = i;
                }
            }

            for (int k = 0; k < this.bsprMulValue.ActiveSheet.Rows.Count; k++)
            {
                if (colIndexLCL == 0 || colIndexUCL == 0)
                {
                    return;
                }
                else
                {
                    for (int i = 0; i < this.bsprMulValue.ActiveSheet.Columns.Count; i++)
                    {
                        if (i == colIndexUCL || i == colIndexLCL || i == 0)
                        {
                            if (!this.bsprMulValue.ActiveSheet.Cells[k, (int)iColIdx.SELECT].Locked)
                                continue;
                            else
                            {
                                this.bsprMulValue.ActiveSheet.Cells[k, i].Locked = true;
                                this.bsprMulValue.ActiveSheet.Cells[k, i].Locked = true;
                            }

                        }
                        else    //cell Lock.
                        {
                            this.bsprMulValue.ActiveSheet.Cells[k, i].Locked = true;
                            this.bsprMulValue.ActiveSheet.Cells[k, i].Locked = true;
                        }
                    }
                }
            }
        }

        public virtual bool DrawSPCChart(DataTable MultiCalcData, string ModelconfigRawID)
        {
            return DrawSPCChart(false, -1, -1,MultiCalcData , ModelconfigRawID);
        }

        public virtual bool DrawSPCChart(bool sample, int samplePeriod, int sampleCount, int sampleWaferCount, DataTable MultiCalcData, string ModelconfigRawID)
        {
            try
            {
                if (MultiCalcData.Rows.Count < 1)
                {
                    return false;
                }

                for (int i = 0; i < MultiCalcData.Columns.Count; i++)
                {
                    if (MultiCalcData.Columns[i].ColumnName.Contains("wafer"))
                    {
                        isWaferColumn = true;
                        break;
                    }
                }

                if (isWaferColumn)
                {              
                    if (MultiCalcData.Rows.Count > 0)
                    {
                        this._dtDataSource = MultiCalcData.Clone();
                        DataTable dtTemp = new DataTable();
                        dtTemp = MultiCalcData.Clone();

                        DateTime nextSampleStartDate =
                        DateTime.Parse(MultiCalcData.Rows[0][COLUMN.RAW_DTTS].ToString().Split(';')[0]);
                        nextSampleStartDate = new DateTime(nextSampleStartDate.Year, nextSampleStartDate.Month, nextSampleStartDate.Day);
                        int sampleCountTemp = 0;

                        foreach (DataRow dr in MultiCalcData.Rows)
                        {
                            if (sample)
                            {
                                if (DateTime.Parse(dr[COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
                                {
                                    while (DateTime.Parse(dr[COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
                                    {
                                        nextSampleStartDate = nextSampleStartDate.AddDays(samplePeriod);
                                    }
                                    sampleCountTemp = 1;
                                }
                                else if (sampleCountTemp < sampleCount)
                                {
                                    sampleCountTemp++;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            dtTemp.Clear();
                            if (dr[COLUMN.RAW_DTTS].ToString().Length != 0)
                            {
                                if ((dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr["RAW"].ToString().Split(';').Length && dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr[COLUMN.PARAM_LIST].ToString().Split(';').Length) ||
                                    (dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr["RAW"].ToString().Split(';').Length && dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr[COLUMN.PARAM_LIST].ToString().Split(';').Length - 1))
                                {
                                    for (int i = 0; i < dr["RAW"].ToString().Split(';').Length; i++)
                                    {
                                        if (dr["RAW"].ToString().Split(';')[i].Length > 0)
                                        {
                                            if (sample)
                                            {
                                                if (i < sampleWaferCount)
                                                {
                                                    dtTemp.ImportRow(dr);
                                                }
                                            }
                                            else
                                                dtTemp.ImportRow(dr);
                                        }
                                    }
                                     
                                    for (int j = 0; j < dtTemp.Rows.Count; j++)
                                    {
                                        dtTemp.Rows[j]["RAW"] = dtTemp.Rows[j]["RAW"].ToString().Split(';')[j];
                                        for (int k = 0; k < dtTemp.Columns.Count; k++)
                                        {
                                            if (dtTemp.Columns[k].ColumnName.ToUpper() != COLUMN.DEFAULT_CHART_LIST && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.RAW && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.OCAP_RAWID)
                                            {
                                                if (dtTemp.Rows[j][k] != null && dtTemp.Rows[j][k].ToString().Length > 0 && dtTemp.Rows[j][k].ToString().Split(';').Length >= dtTemp.Rows.Count)
                                                {
                                                    dtTemp.Rows[j][k] = dtTemp.Rows[j][k].ToString().Split(';')[j];
                                                }
                                            }
                                        }

                                    }

                                    this._dtDataSource.Merge(dtTemp);
                                }
                            }
                        }

                    }
                    this.PROC_DATACalc(_dtDataSource, ModelconfigRawID);

                }
                else
                {

                }
            }
            catch
            {
            }
            return true;
        }

        public virtual bool DrawSPCChart(bool sample, int samplePeriod, int sampleCount, DataTable MultiCalcData, string ModelconfigRawID)
        {
            try
            {
                if (MultiCalcData.Rows.Count < 1)
                {
                    return false;
                }

                for (int i = 0; i < MultiCalcData.Columns.Count; i++)
                {
                    if (MultiCalcData.Columns[i].ColumnName.Contains("wafer"))
                    {
                        isWaferColumn = true;
                        break;
                    }
                }

                if (isWaferColumn)
                {

                    if (MultiCalcData.Rows.Count > 0)
                    {
                        this._dtDataSource = MultiCalcData.Clone();
                        DataTable dtTemp = new DataTable();
                        dtTemp = MultiCalcData.Clone();

                        DateTime nextSampleStartDate =
                                    DateTime.Parse(MultiCalcData.Rows[0][COLUMN.RAW_DTTS].ToString().Split(';')[0]);
                        nextSampleStartDate = new DateTime(nextSampleStartDate.Year, nextSampleStartDate.Month, nextSampleStartDate.Day);
                        int sampleCountTemp = 0;

                        foreach (DataRow dr in MultiCalcData.Rows)
                        {
                            if (sample)
                            {
                                if (DateTime.Parse(dr[COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
                                {
                                    while (DateTime.Parse(dr[COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
                                    {
                                        nextSampleStartDate = nextSampleStartDate.AddDays(samplePeriod);
                                    }
                                    sampleCountTemp = 1;
                                }
                                else if (sampleCountTemp < sampleCount)
                                {
                                    sampleCountTemp++;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            dtTemp.Clear();
                            if (dr[COLUMN.RAW_DTTS].ToString().Length != 0)
                            {
                                if ((dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr["RAW"].ToString().Split(';').Length && dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr[COLUMN.PARAM_LIST].ToString().Split(';').Length) ||
                                    (dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr["RAW"].ToString().Split(';').Length && dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr[COLUMN.PARAM_LIST].ToString().Split(';').Length - 1))
                                {
                                    for (int i = 0; i < dr["RAW"].ToString().Split(';').Length; i++)
                                    {
                                        if (dr["RAW"].ToString().Split(';')[i].Length > 0)
                                        {
                                           dtTemp.ImportRow(dr);                                            
                                        }
                                    }

                                    for (int j = 0; j < dtTemp.Rows.Count; j++)
                                    {
                                        dtTemp.Rows[j]["RAW"] = dtTemp.Rows[j]["RAW"].ToString().Split(';')[j];
                                        for (int k = 0; k < dtTemp.Columns.Count; k++)
                                        {
                                            if (dtTemp.Columns[k].ColumnName.ToUpper() != COLUMN.DEFAULT_CHART_LIST && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.RAW && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.OCAP_RAWID)
                                            {
                                                if (dtTemp.Rows[j][k] != null && dtTemp.Rows[j][k].ToString().Length > 0 && dtTemp.Rows[j][k].ToString().Split(';').Length >= dtTemp.Rows.Count)
                                                {
                                                    dtTemp.Rows[j][k] = dtTemp.Rows[j][k].ToString().Split(';')[j];
                                                }
                                            }
                                            //dtTemp.Rows[j]["RAW"] = dtTemp.Rows[j]["RAW"].ToString().Split(';')[j];
                                            //dtTemp.Rows[j][COLUMN.PARAM_LIST] = dtTemp.Rows[j][COLUMN.PARAM_LIST].ToString().Split(';')[j];
                                            //dtTemp.Rows[j][COLUMN.RAW_DTTS] = dtTemp.Rows[j][COLUMN.RAW_DTTS].ToString().Split(';')[j];
                                            //if (dtTemp.Rows[j]["SUBSTRATE_ID"] != null && dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Length > 0 && dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Split(';').Length > 1)
                                            //{
                                            //    dtTemp.Rows[j]["SUBSTRATE_ID"] = dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Split(';')[j];
                                            //}
                                        }

                                    }

                                    this._dtDataSource.Merge(dtTemp);
                                }
                            }
                        }
                        

                    }
                    this.PROC_DATACalc(_dtDataSource, ModelconfigRawID);

                }
                else
                {

                }
            }
            catch
            {
            }
            return true;
        }


        void bchkSampling_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (bchkSampling.Checked)
            {
                if (this.bcboChartType.Text == "RAW")
                {
                    this.btxtWSampleCnt.Enabled = true;
                    this.btxtWSampleCnt.Enabled = true;
                }

                this.btxtSampleCnt.Enabled = true;
                this.btxtSamplePeriod.Enabled = true;
                
            }
            else
            {
                this.btxtWSampleCnt.Enabled = false;
                this.btxtSampleCnt.Enabled = false;
                this.btxtSamplePeriod.Enabled = false;
                this.btxtWSampleCnt.Text = "";
                this.btxtSampleCnt.Text = "";
                this.btxtSamplePeriod.Text = "";
            }
        }

        private void bcboChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bchkSampling.Checked)
            {
                if (bcboChartType.Text == "RAW")
                {
                    this.btxtWSampleCnt.Enabled = true;
                    this.bchkThresholdOff.Enabled = true;
                    this.btxtThreshold.Text = "";
                    this.btxtThreshold.Enabled = true;
                    if (bchkThresholdOff.Checked)
                    {
                        btxtThreshold.Enabled = false;
                    }
                }
                else if (bcboChartType.Text == "X-BAR")
                {
                    this.btxtWSampleCnt.Enabled = false;
                    this.btxtWSampleCnt.Text = "";
                    this.bchkThresholdOff.Enabled = true;
                    this.btxtThreshold.Text = "";
                    this.btxtThreshold.Enabled = true;
                    if (bchkThresholdOff.Checked)
                    {
                        btxtThreshold.Enabled = false;
                        
                    }
                }
                else
                {
                    this.bchkThresholdOff.Enabled = false;
                    this.btxtThreshold.Text = "";
                    this.btxtThreshold.Enabled = false;
                    this.btxtWSampleCnt.Enabled = false;
                    this.btxtWSampleCnt.Text = "";
                    if (bchkThresholdOff.Checked)
                    {
                        btxtThreshold.Enabled = false;
                        
                    }
                }
            }
            else
            {
                if (bcboChartType.Text == "RAW" || bcboChartType.Text == "X-BAR")
                {
                    this.bchkThresholdOff.Enabled = true;
                    this.btxtThreshold.Text = "";
                    this.btxtThreshold.Enabled = true;
                    if (bchkThresholdOff.Checked)
                    {
                        btxtThreshold.Enabled = false;
                    }
                }
                
                else
                {
                    this.bchkThresholdOff.Enabled = false;
                    this.btxtThreshold.Text = "";
                    this.btxtThreshold.Enabled = false;
                    if (bchkThresholdOff.Checked)
                    {
                        btxtThreshold.Enabled = false;
                    }
                }

            }
        }
        private void MultiCalcValueDataBinding(double avg, double std, double dUCL, double dLCL, string chartType 
            , string ModelconfigRawID, double IQR_UCL , double IQR_LCL ,double threshold_UCL, double threshold_LCL,string result, string OutOfSpec)
        {
            DataTable dtMaster = _dsSPCModeData.Tables[TABLE.MODEL_MST_SPC];
            DataTable dtConfig = _dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
            DataTable dtContext = _dsSPCModeData.Tables[TABLE.MODEL_CONTEXT_MST_SPC];

            
            foreach (DataRow drConfig in dtConfig.Rows)
            {
                if (drConfig[COLUMN.RAWID].ToString().Equals(ModelconfigRawID))
                {
                    DataRow drChartList = _dtSPCMultiCalcList.NewRow();

                    drChartList[COLUMN.CHART_ID] = drConfig[COLUMN.RAWID].ToString();
                    //drChartList[COLUMN.SPC_MODEL_NAME] = drConfig[COLUMN.SPC_MODEL_NAME].ToString();
                    //drChartList[COLUMN.PARAM_ALIAS] = drConfig[COLUMN.PARAM_ALIAS].ToString();
                    drChartList[COLUMN.MAIN_YN] = drConfig[COLUMN.MAIN_YN].ToString();
                    drChartList[COLUMN.SAVE_YN] = result;
                    drChartList[COLUMN.OUT_OF_SPEC] = OutOfSpec;

                   //DataRow[] drMaster = dtMaster.Select(string.Format("{0} = '{1}'", COLUMN.RAWID, drConfig[COLUMN.MODEL_RAWID]));
                   //drChartList[COLUMN.SPC_MODEL_NAME] = drMaster[0][COLUMN.SPC_MODEL_NAME].ToString();
                    drChartList[COLUMN.UPPER_SPEC] = drConfig[COLUMN.UPPER_SPEC].ToString();
                    drChartList[COLUMN.TARGET] = drConfig[COLUMN.TARGET].ToString();
                    drChartList[COLUMN.LOWER_SPEC] = drConfig[COLUMN.LOWER_SPEC].ToString();

                    avg = (dUCL + dLCL) / 2;

                    if (_sChartType == "RAW")
                    {
                        drChartList[COLUMN.RAW_UCL] = dUCL;
                        drChartList[COLUMN.RAW_CENTER_LINE] = avg.ToString();
                        drChartList[COLUMN.RAW_LCL] = dLCL;
                        if (!this.bchkThresholdOff.Checked)
                        {
                            drChartList[COLUMN.THRESHOLD_UCL] = threshold_UCL.ToString();
                            drChartList[COLUMN.THRESHOLD_TARGET] = avg.ToString();
                            drChartList[COLUMN.THRESHOLD_LCL] = threshold_LCL.ToString();
                        }

                    }
                    else if (_sChartType == "X-BAR")
                    {
                        drChartList[COLUMN.UPPER_CONTROL] = dUCL;
                        drChartList[COLUMN.CENTER_LINE] = avg.ToString();
                        drChartList[COLUMN.LOWER_CONTROL] = dLCL;
                        if (!this.bchkThresholdOff.Checked)
                        {
                            drChartList[COLUMN.THRESHOLD_UCL] = threshold_UCL.ToString();
                            drChartList[COLUMN.THRESHOLD_TARGET] = avg.ToString();
                            drChartList[COLUMN.THRESHOLD_LCL] = threshold_LCL.ToString();
                        }

                    }

                    else if (_sChartType == "STDDEV")
                    {
                        drChartList[COLUMN.STD_UCL] = dUCL;
                        drChartList[COLUMN.STD_CENTER_LINE] = avg.ToString();
                        drChartList[COLUMN.STD_LCL] = dLCL;

                    }
                    else if (_sChartType == "RANGE")
                    {
                        drChartList[COLUMN.RANGE_UCL] = dUCL;
                        drChartList[COLUMN.RANGE_CENTER_LINE] = avg.ToString();
                        drChartList[COLUMN.RANGE_LCL] = dLCL;

                    }
                    else if (_sChartType == "EWMA_MEAN")
                    {
                        drChartList[COLUMN.EWMA_M_UCL] = dUCL;
                        drChartList[COLUMN.EWMA_M_CENTER_LINE] = avg.ToString();
                        drChartList[COLUMN.EWMA_M_LCL] = dLCL;

                    }
                    else if (_sChartType == "EWMA_RANGE")
                    {
                        drChartList[COLUMN.EWMA_R_UCL] = dUCL;
                        drChartList[COLUMN.EWMA_R_CENTER_LINE] = avg.ToString();
                        drChartList[COLUMN.EWMA_R_LCL] = dLCL;

                    }
                    else if (_sChartType == "EWMA_STDDEV")
                    {
                        drChartList[COLUMN.EWMA_S_UCL] = dUCL;
                        drChartList[COLUMN.EWMA_S_CENTER_LINE] = avg.ToString();
                        drChartList[COLUMN.EWMA_S_LCL] = dLCL;

                    }
                    else if (_sChartType == "MA")
                    {
                        drChartList[COLUMN.MA_UCL] = dUCL;
                        drChartList[COLUMN.MA_CENTER_LINE] = avg.ToString();
                        drChartList[COLUMN.MA_LCL] = dLCL;

                    }
                    else if (_sChartType == "MSD")
                    {
                        drChartList[COLUMN.MS_UCL] = dUCL;
                        drChartList[COLUMN.MS_CENTER_LINE] = avg.ToString();
                        drChartList[COLUMN.MS_LCL] = dLCL;

                    }
                    else if (_sChartType == "MR")
                    {
                        drChartList[COLUMN.MR_UCL] = dUCL;
                        drChartList[COLUMN.MR_CENTER_LINE] = avg.ToString();
                        drChartList[COLUMN.MR_LCL] = dLCL;

                    }


                    drChartList[COLUMN.IQR_UCL] = IQR_UCL.ToString();
                    drChartList[COLUMN.IQR_LCL] = IQR_LCL.ToString();

                    //DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfig[COLUMN.RAWID]));

                    //foreach (DataRow drContext in drContexts)
                    //{
                    //    //2009-11-27 bskwon 추가 : Sub Model 상속 구조가 아닌경우 예외처리
                    //    drChartList[drContext[COLUMN.CONTEXT_KEY].ToString()] = drContext[COLUMN.CONTEXT_VALUE].ToString();
                    //}


                    
                    
                    _dtSPCMultiCalcList.Rows.Add(drChartList);
                }
            }
           
           
        }

        private void PROC_DATACalc(DataTable ChartData , string sMulModelConfigRawID)
        {
            
            
            try
            {
                if (this.btxtSigma.Text.Trim().Length == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_SIGMA", null, null);
                    return;
                }

                if (bcboOption.Text.Equals("SIGMA"))
                {
                   
                    double dsigma = 0;

                    bool isParseSigma = double.TryParse(this.btxtSigma.Text.Trim(), out dsigma);
                    if (!isParseSigma)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[]{"sigma"}, null);
                        return;
                    }

                    double[] dsValueList = GetValueListOfChartData(false, ChartData);

                    if (dsValueList.Length == 0)
                    {
                        //MSGHandler.DisplayMessage(MSGType.Information, "There is no data to calculate control limit.");
                        //return;
                    }
                    else
                    {
                        double avg = StStat.mean(dsValueList);
                        double std = StStat.std(dsValueList);
                        double dUCL = avg + (std * dsigma);
                        double dLCL = avg - (std * dsigma);
                    }

                    
                    this.semiAutoCalc(dsValueList, dsigma,sMulModelConfigRawID ,bcboOption.Text);
                   
                    
            
                   
                }
                else if (bcboOption.Text.Equals("CONSTANT"))
                {
                    double dconstant = 0;

                    bool isParseConstant = double.TryParse(this.btxtSigma.Text.Trim(), out dconstant);
                    if (!isParseConstant)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[]{"constant"}, null);
                        return;
                    }

                    double[] dsValueList = GetValueListOfChartData(false, ChartData);

                    if (dsValueList.Length == 0)
                    {
                    //    MSGHandler.DisplayMessage(MSGType.Information, "There is no data to calculate control limit.");
                    //    return;
                    }
                    else
                    {
                        double avg = StStat.mean(dsValueList);
                        double std = StStat.std(dsValueList);

                        double dUCL = avg + dconstant;
                        double dLCL = avg - dconstant;
                    }
                    

                    this.semiAutoCalc(dsValueList, dconstant, sMulModelConfigRawID, bcboOption.Text);
                    //MultiCalcValueDataBinding(avg, std, dUCL, dLCL, this._sChartType, sMulModelConfigRawID);


                }
                else if (bcboOption.Text.Equals("%"))
                {
                    double dPercentage = 0;

                    bool isParsePercent = double.TryParse(this.btxtSigma.Text.Trim(), out dPercentage);
                    if (!isParsePercent)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_VALUE_AS_NUM", new string[]{"Percent"}, null);
                        return;
                    }

                    double[] dsValueList = GetValueListOfChartData(false, ChartData);

                    if (dsValueList.Length == 0)
                    {
                        //MSGHandler.DisplayMessage(MSGType.Information, "There is no data to calculate control limit.");
                        //return;
                    }
                    else
                    {
                        double avg = StStat.mean(dsValueList);
                        double std = StStat.std(dsValueList);

                        double dUCL = avg + avg * dPercentage / 100;
                        double dLCL = avg - avg * dPercentage / 100;
                    }

                    this.semiAutoCalc(dsValueList, dPercentage, sMulModelConfigRawID, bcboOption.Text);
                   // MultiCalcValueDataBinding(avg, std, dUCL, dLCL, this._sChartType, sMulModelConfigRawID);


                }

            }
            catch
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_ERROR_CALC", null, null);
            }
        }

        public void semiAutoCalc(double[] dsValueList, double optionValue, string ModelconfigRawID, string option)
        {
            string calcType = bcboCalcSide.SelectedItem.ToString();
            
            DataTable dtConfig = _dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
            double upper_spec = Double.NaN;
            double lower_spec = Double.NaN;
            double cavg = Double.NaN;
            double cstd = Double.NaN;
            double cUCL = Double.NaN;
            double cLCL = Double.NaN;
            double resultUCL = Double.NaN;
            double resultLCL = Double.NaN;
            double thresholdUCL = double.NaN;
            double thresholdLCL = double.NaN;
            _raw_lowerFilter = double.NaN;
            _raw_upperFilter = double.NaN;
            double dUCL = double.NaN;
            double dLCL = double.NaN;

            double median = Double.NaN;
            double IQR = Double.NaN;
            double[] quantile = null;
            double[] tempDouble = null;

            string result = "N";

            double[] values = null;
            // double calcControlcenter = 0;
            double threshold = double.NaN;
            ArrayList dArrTemp = new ArrayList();

            foreach (DataRow drConfig in dtConfig.Rows)
            {
                if(drConfig[COLUMN.RAWID].ToString()==ModelconfigRawID)
                {
                    if (drConfig[COLUMN.UPPER_SPEC].ToString() != "" && drConfig[COLUMN.LOWER_SPEC].ToString() != "")
                    {
                        string upper = drConfig[COLUMN.UPPER_SPEC].ToString();
                        string lower = drConfig[COLUMN.LOWER_SPEC].ToString();
                        upper_spec = double.Parse(upper);
                        lower_spec = double.Parse(lower);
                    }
                    else if (drConfig[COLUMN.UPPER_SPEC].ToString() != "" && drConfig[COLUMN.LOWER_SPEC].ToString() == "")
                    {
                        upper_spec = double.Parse(drConfig[COLUMN.UPPER_SPEC].ToString());
                    }
                    else if (drConfig[COLUMN.UPPER_SPEC].ToString() == "" && drConfig[COLUMN.LOWER_SPEC].ToString() != "")
                    {
                        lower_spec = double.Parse(drConfig[COLUMN.LOWER_SPEC].ToString());
                    }

                    if (_llColumnMapping != null)
                    {
                        string colNameUCL = string.Empty;
                        string colNameLCL = string.Empty;

                        if (_sChartType == Definition.MULTICALC_CHART_TYPE.XBAR)
                        {
                            colNameUCL = COLUMN.UPPER_CONTROL;
                            colNameLCL = COLUMN.LOWER_CONTROL;
;
                        }
                        else
                        {
                            colNameUCL = _llColumnMapping[_sChartType].ToString() + "_UCL";
                            colNameLCL = _llColumnMapping[_sChartType].ToString() + "_LCL";
                        }

                        if (drConfig[colNameUCL].ToString() != "" && drConfig[colNameLCL].ToString() != "")
                        {
                            string upper = drConfig[colNameUCL].ToString();
                            string lower = drConfig[colNameLCL].ToString();
                            dUCL = double.Parse(upper);
                            dLCL = double.Parse(lower);
                        }
                    }
                }
              
            }



            //Calculation => IQR
            values = dsValueList;
            if (values.Length > 5)
            {
                quantile = Quantile(values);
                if (quantile == null || quantile.Length < 3)
                {
                    // IQR nodata
                }
                else
                {
                    median = quantile[1];
                    IQR = quantile[2] - quantile[0];
                    //IQR_UCL,IQR_LCL

                    _raw_upperFilter = quantile[2] + IQR * 1.5;
                    _raw_lowerFilter = quantile[0] - IQR * 1.5;
                }
               
            }
            else
            {
                _OutOfSpec = "N";
               // MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
            }


           

            //IQR Filter
            if (!_raw_upperFilter.Equals(double.NaN) && !_raw_lowerFilter.Equals(double.NaN))
            {
                if (!(!_ComUtil.IsDouble(_raw_upperFilter.ToString()) && !_ComUtil.IsDouble(_raw_lowerFilter.ToString())))
                {
                    if (!this.bchkIQRFilter.Checked)
                    {
                        for (int i = 0; i < dsValueList.Length; i++)
                        {
                            if (isFiltered(dsValueList[i]))
                                //dArrTemp[i] = dsValueList[i];
                                dArrTemp.Add(dsValueList[i]);
                        }
                        tempDouble = (double[])dArrTemp.ToArray(typeof(Double));
                        
                    }
                    else
                    {
                        tempDouble = dsValueList;
                    }
                }
                
                cavg = StStat.mean(tempDouble);
                cstd = StStat.std(tempDouble);

                if (option == "SIGMA")
                {
                    cUCL = cavg + (cstd * optionValue);
                    cLCL = cavg - (cstd * optionValue);
                }
                else if (option == "%")
                {
                    cUCL = cavg + cavg * optionValue / 100;
                    cLCL = cavg - cavg * optionValue / 100;
                }
                else if (option == "CONSTANT")
                {
                    cUCL = cavg + optionValue;
                    cLCL = cavg - optionValue;
                }

                if (_sChartType == Definition.MULTICALC_CHART_TYPE.RAW || _sChartType == Definition.MULTICALC_CHART_TYPE.XBAR)
                {
                    double IQRSpecLimit = 0;


                    if (!upper_spec.Equals(Double.NaN) && !lower_spec.Equals(Double.NaN))
                    {
                        if (cUCL < lower_spec || cLCL > upper_spec)
                        {
                            _OutOfSpec = "Y";
                            result = "N";
                            resultLCL = cLCL;
                            resultUCL = cUCL;
                        }
                        else if (cUCL > upper_spec && cLCL < lower_spec)
                        {
                            cUCL = upper_spec;
                            cLCL = lower_spec;
                            IQRSpecLimit = Math.Min(cUCL - cavg, cavg - cLCL);
                        }
                        else if (cUCL > upper_spec)
                        {
                            cUCL = upper_spec;
                            //cavg = (upper_spec + lower_spec) / 2;
                            IQRSpecLimit = Math.Min(cUCL - cavg, cavg - cLCL);
                        }
                        else if (cLCL < lower_spec)
                        {
                            cLCL = lower_spec;
                            IQRSpecLimit = Math.Min(cUCL - cavg, cavg - cLCL);
                        }
                        else
                        {
                            IQRSpecLimit = Math.Min(cUCL - cavg, cavg - cLCL);
                        }
                    }
                    else if (!upper_spec.Equals(Double.NaN) && lower_spec.Equals(Double.NaN))
                    {
                        if (cUCL < lower_spec)
                        {
                            cUCL = double.NaN;
                            cLCL = double.NaN;
                            _OutOfSpec = "Y";
                            IQRSpecLimit = double.NaN;
                        }
                        else if (cLCL < lower_spec)
                        {
                            cLCL = lower_spec;
                            IQRSpecLimit = cavg - cLCL;
                        }
                        else
                        {
                            IQRSpecLimit = cavg - cLCL;
                        }
                    }
                    else if (upper_spec.Equals(Double.NaN) && !lower_spec.Equals(Double.NaN))
                    {
                        if (cLCL > upper_spec)
                        {
                            cUCL = double.NaN;
                            cLCL = double.NaN;
                            _OutOfSpec = "Y";
                            IQRSpecLimit = double.NaN;
                        }
                        if (cUCL > upper_spec)
                        {
                            cUCL = upper_spec;
                            IQRSpecLimit = cUCL - cavg;
                        }
                        else
                        {
                            IQRSpecLimit = cUCL - cavg;
                        }

                    }

                    if (!this.bchkThresholdOff.Checked)
                    {
                        double thresholdSpecLimit = 0;
                        if (!upper_spec.Equals(double.NaN) && !lower_spec.Equals(double.NaN))
                        {
                            double upper_threshold = upper_spec - cavg;
                            double lower_threshold = cavg - lower_spec;
                            threshold = Math.Min(Math.Abs(upper_threshold), Math.Abs(lower_threshold));
                            thresholdSpecLimit = threshold * (double.Parse(this.btxtThreshold.Text) * 0.01);

                            thresholdUCL = cavg + thresholdSpecLimit;
                            thresholdLCL = cavg - thresholdSpecLimit;
                        }
                        else if (!upper_spec.Equals(double.NaN) && lower_spec.Equals(double.NaN))
                        {
                            threshold = cavg - lower_spec;
                            thresholdSpecLimit = threshold * (double.Parse(this.btxtThreshold.Text) * 0.01);
                            thresholdUCL = cavg + thresholdSpecLimit;
                            thresholdLCL = cavg - thresholdSpecLimit;
                        }
                        else if (upper_spec.Equals(double.NaN) && !lower_spec.Equals(double.NaN))
                        {
                            threshold = upper_spec - cavg;
                            thresholdSpecLimit = threshold * (double.Parse(this.btxtThreshold.Text) * 0.01);
                            thresholdUCL = cavg + thresholdSpecLimit;
                            thresholdLCL = cavg - thresholdSpecLimit;
                        }
                        else
                        {
                            thresholdSpecLimit = double.NaN;
                            thresholdUCL = double.NaN;
                            thresholdLCL = double.NaN;
                        }
                        //IQR Filter Spec Limit vs Threshold Spec Limit
                        if (!thresholdSpecLimit.Equals(double.NaN))
                        {
                            if (IQRSpecLimit <= thresholdSpecLimit)
                            {
                                resultUCL = thresholdUCL;
                                resultLCL = thresholdLCL;
                            }
                            else
                            {
                                resultUCL = cUCL; 
                                resultLCL = cLCL;
                            }
                        }
                        else
                        {
                            resultUCL = cUCL;
                            resultLCL = cLCL;
                        }
                        //if (thresholdUCL.Equals(double.NaN))
                        //{
                        //    result = "N";
                        //}
                        if (resultUCL.Equals(double.NaN) || resultLCL.Equals(double.NaN))
                        {
                            result = "N";
                        }
                        else
                        {
                            result = "Y";
                        }

                        if (resultLCL > upper_spec || resultUCL < lower_spec)
                        {
                            _OutOfSpec = "Y";
                            IQRSpecLimit = double.NaN;
                            result = "N";

                        }
                      
                    }
                    else
                    {
                        if (resultLCL > upper_spec || resultUCL < lower_spec)
                        {
                            _OutOfSpec = "Y";
                            IQRSpecLimit = double.NaN;
                            result = "N";

                        }
                        else
                        {
                            resultUCL = cUCL;
                            resultLCL = cLCL;
                            result = "Y";
                        }
                       
                    }

                    if(calcType== Definition.VARIABLE_UPPER)
                    {
                        resultLCL = dLCL;
                    }
                    if (calcType == Definition.VARIABLE_LOWER)
                    {
                        resultUCL = dUCL;
                    }
                    
                    // cavg, cstd ,resultUCL,resultLCL,chartID,ModelconfigRawID,thresholdUCL,thresholdLCL,
                    MultiCalcValueDataBinding(cavg, cstd, Math.Round(resultUCL,10), Math.Round(resultLCL,10), this._sChartType, ModelconfigRawID
                        , _raw_upperFilter, _raw_lowerFilter, thresholdUCL, thresholdLCL,result,_OutOfSpec);

                }
                else
                {
                    resultUCL = cUCL;
                    resultLCL = cLCL;
                    result = "Y";

                    if (calcType == Definition.VARIABLE_UPPER)
                    {
                        resultLCL = dLCL;
                    }
                    if (calcType == Definition.VARIABLE_LOWER)
                    {
                        resultUCL = dUCL;
                    }

                    MultiCalcValueDataBinding(cavg, cstd, Math.Round(resultUCL, 10), Math.Round(resultLCL, 10), this._sChartType, ModelconfigRawID
                        , _raw_upperFilter, _raw_lowerFilter, thresholdUCL, thresholdLCL, result,_OutOfSpec);
                }

            }
            else
            {
                if (calcType == Definition.VARIABLE_UPPER)
                {
                    resultLCL = dLCL;
                }
                if (calcType == Definition.VARIABLE_LOWER)
                {
                    resultUCL = dUCL;
                }

                MultiCalcValueDataBinding(cavg, cstd, Math.Round(resultUCL, 10), Math.Round(resultLCL, 10), this._sChartType, ModelconfigRawID
                            , _raw_upperFilter, _raw_lowerFilter, thresholdUCL, thresholdLCL, result,_OutOfSpec);
            }
        }
            
            
        
        public double[] Quantile(double[] value)
        {
            double[] sortedValue = sort(value);
            int length = sortedValue.Length - 1;
            double dQ1 = (double)length * 0.25D;
            double dQ2 = (double)length * 0.5D;
            double dQ3 = (double)length * 0.75D;
            int iQ1 = (int)dQ1;
            int iQ2 = (int)dQ2;
            int iQ3 = (int)dQ3;
            double[] result = new double[3];
            result[0] = interpolation((double)iQ1 / (double)length, (double)(iQ1 + 1) / (double)length, sortedValue[iQ1], sortedValue[iQ1 + 1], 0.25D);
            result[1] = interpolation((double)iQ2 / (double)length, (double)(iQ2 + 1) / (double)length, sortedValue[iQ2], sortedValue[iQ2 + 1], 0.5D);
            result[2] = interpolation((double)iQ3 / (double)length, (double)(iQ3 + 1) / (double)length, sortedValue[iQ3], sortedValue[iQ3 + 1], 0.75D);
            return result;
        }

        public static double[] sort(double[] data)
        {
            int sampleSize = data.Length;
            double[] xData = copyVector(data);
            for(int i = sampleSize / 2; i >= 0; i--)
                adjust(xData, i, sampleSize);

            for(int i = sampleSize - 2; i >= 0; i--)
            {
                double tmp = xData[i + 1];
                xData[i + 1] = xData[0];
                xData[0] = tmp;
                adjust(xData, 0, i + 1);
            }

            return xData;
        }

        public static double[] copyVector(double[] a)
        {
            double[] x = new double[a.Length];
            for(int i = 0; i < a.Length; i++)
            x[i] = a[i];

            return x;
        }

        public static double interpolation(double x1, double x2, double y1, double y2, double t)
        {
            double result = (0.0D / 0.0D);
            result = y1 + ((y2 - y1) * (t - x1)) / (x2 - x1);
            return result;
        }
        protected static void adjust(double[] X, int i, int n)
        {
            double tmp = X[i];
            int j;
            for(j = i * 2; j < n; j *= 2)
            {
                if(j < n - 1 && X[j] < X[j + 1])
                    j++;
                if(tmp >= X[j])
                {
                    X[j / 2] = tmp;
                    break;
                }
                X[j / 2] = X[j];
            }

            X[j / 2] = tmp;
        }
        private bool isFiltered(double value)
        {                     
            if (this._raw_upperFilter == this._raw_lowerFilter || value < this._raw_upperFilter && value > this._raw_lowerFilter)
            {
                return true;
            }
            return false;
        }

        private void bchkThresholdOff_CheckedChanged(object sender, EventArgs e)
        {
            if (bchkThresholdOff.Checked)
            {
                btxtThreshold.Enabled = false;
            }
            else
            {
                btxtThreshold.Enabled = true;
            }
        }

        private void btxtSigma_TextChanged(object sender, EventArgs e)
        {
            CommonUtility commutil = new CommonUtility();
            if (this.btxtSigma.Text != "")
            {
                if (!commutil.IsInteger(this.btxtSigma.Text) || int.Parse(this.btxtSigma.Text) < 1)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_INT", null, null);
                    btxtSigma.Text = "";
                }
            }
          
        }

        private void btxtThreshold_TextChanged(object sender, EventArgs e)
        {
            CommonUtility commutil = new CommonUtility();
            if (this.btxtThreshold.Text != "")
            {
                if (!commutil.IsInteger(this.btxtThreshold.Text) || int.Parse(this.btxtThreshold.Text) < 1 || int.Parse(this.btxtThreshold.Text) > 100)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_ENTER_INT",null, null);
                    btxtThreshold.Text = "";
                }
            }
        }

        private void bbtnSave_Click(object sender, EventArgs e)
        {
            if (this.bsprMulValue.ActiveSheet.Rows.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_EXECUTE_CALC", null, null);
                return;
            }
            DialogResult dg = MSGHandler.DialogQuestionResult(this._mlthandler.GetMessage("GENERAL_ASK_SAVE"), null, MessageBoxButtons.YesNo);
            if (dg == DialogResult.No)
                return;

            LinkedList lnkListSaveData = new LinkedList();
            ArrayList alCheckRowIndex = this.bsprMulValue.GetCheckedList(0);

            DataTable dtSaveData = ((DataSet)bsprMulValue.DataSource).Tables[0];
            if (alCheckRowIndex.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROWS", null, null);

            }
            else
            {
                bool bSuccess = false;
                bool bValidation = false;

                for (int i = 0; i < this.bsprMulValue.ActiveSheet.Rows.Count; i++)
                {
                    if (dtSaveData.Rows[i][(int)iColIdx.SELECT].ToString() == Definition.VARIABLE_TRUE)
                    {
                        bValidation = this.ValidationControlLimit(i, (int)iColIdx.RAWID);
                        if (bValidation)
                            continue;
                        else
                            break;
                    }
                }

                if (bValidation)
                {
                    foreach (DataRow dr in dtSaveData.Rows)
                    {
                        string a = dr[COLUMN.SELECT].ToString();
                        //if(dr[COLUMN.SELECT].ToString();
                        if (a == "True")
                        {
                            if (dr[colIndexLCL].ToString() == double.NaN.ToString())
                                dr[colIndexLCL] = null;
                            if (dr[colIndexUCL].ToString() == double.NaN.ToString())
                                dr[colIndexUCL] = null;
                            if (dr[colIndexUCL + 1].ToString() == double.NaN.ToString())
                                dr[colIndexUCL + 1] = null;

                            LinkedList SaveData = new LinkedList();
                            SaveData.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, dr[COLUMN.CHART_ID].ToString());
                            SaveData.Add(COLUMN.UPPER_SPEC, dr[COLUMN.UPPER_SPEC].ToString());
                            SaveData.Add(COLUMN.LOWER_SPEC, dr[COLUMN.LOWER_SPEC].ToString());
                            SaveData.Add(COLUMN.MAIN_YN, dr[COLUMN.MAIN_YN].ToString());
                            if (_sChartType == Definition.MULTICALC_CHART_TYPE.RAW)
                            {
                                SaveData.Add(COLUMN.RAW_LCL, dr[COLUMN.RAW_LCL].ToString());
                                SaveData.Add(COLUMN.RAW_UCL, dr[COLUMN.RAW_UCL].ToString());
                            }
                            else if (_sChartType == Definition.MULTICALC_CHART_TYPE.XBAR)
                            {
                                SaveData.Add(COLUMN.UPPER_CONTROL, dr[COLUMN.UPPER_CONTROL].ToString());
                                SaveData.Add(COLUMN.LOWER_CONTROL, dr[COLUMN.LOWER_CONTROL].ToString());
                            }

                            else if (_sChartType == Definition.MULTICALC_CHART_TYPE.STDDEV)
                            {
                                SaveData.Add(COLUMN.STD_UCL, dr[COLUMN.STD_UCL].ToString());
                                SaveData.Add(COLUMN.STD_LCL, dr[COLUMN.STD_LCL].ToString());
                            }
                            else if (_sChartType == Definition.MULTICALC_CHART_TYPE.RANGE)
                            {
                                SaveData.Add(COLUMN.RANGE_UCL, dr[COLUMN.RANGE_UCL].ToString());
                                SaveData.Add(COLUMN.RANGE_LCL, dr[COLUMN.RANGE_LCL].ToString());
                            }
                            else if (_sChartType == Definition.MULTICALC_CHART_TYPE.EWMA_MEAN)
                            {
                                SaveData.Add(COLUMN.EWMA_M_UCL, dr[COLUMN.EWMA_M_UCL].ToString());
                                SaveData.Add(COLUMN.EWMA_M_LCL, dr[COLUMN.EWMA_M_LCL].ToString());
                            }
                            else if (_sChartType == Definition.MULTICALC_CHART_TYPE.EWMA_RANGE)
                            {
                                SaveData.Add(COLUMN.EWMA_R_UCL, dr[COLUMN.EWMA_R_UCL].ToString());
                                SaveData.Add(COLUMN.EWMA_R_LCL, dr[COLUMN.EWMA_R_LCL].ToString());
                            }
                            else if (_sChartType == Definition.MULTICALC_CHART_TYPE.EWMA_STDDEV)
                            {
                                SaveData.Add(COLUMN.EWMA_S_UCL, dr[COLUMN.EWMA_S_UCL].ToString());
                                SaveData.Add(COLUMN.EWMA_S_LCL, dr[COLUMN.EWMA_S_LCL].ToString());
                            }
                            else if (_sChartType == Definition.MULTICALC_CHART_TYPE.MA)
                            {
                                SaveData.Add(COLUMN.MA_UCL, dr[COLUMN.MA_UCL].ToString());
                                SaveData.Add(COLUMN.MA_LCL, dr[COLUMN.MA_LCL].ToString());
                            }
                            else if (_sChartType == Definition.MULTICALC_CHART_TYPE.MSD)
                            {
                                SaveData.Add(COLUMN.MS_UCL, dr[COLUMN.MS_UCL].ToString());
                                SaveData.Add(COLUMN.MS_LCL, dr[COLUMN.MS_LCL].ToString());
                            }
                            else if (_sChartType == Definition.MULTICALC_CHART_TYPE.MR)
                            {
                                SaveData.Add(COLUMN.MR_UCL, dr[COLUMN.MR_UCL].ToString());
                                SaveData.Add(COLUMN.MR_LCL, dr[COLUMN.MR_LCL].ToString());
                            }

                            this.MsgShow(COMMON_MSG.Save_Data);

                            SaveData.Add("CHART_TYPE", this._sChartType);
                            SaveData.Add(Definition.CONDITION_KEY_USER_ID, this.sessionData.UserId);
                            SaveData.Add(COLUMN.SAVE_COMMENT, "Manual Calculation");
                            SaveData.Add(COLUMN.CHANGED_ITEMS, "Limit");

                            bSuccess = this._wsSPC.SaveSPCMulitiCalcModelData(SaveData.GetSerialData());
                        }
                    } //foreach
                    this.MsgClose();
                    if (bSuccess)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));
                        ConfigListDataBindingPopup();
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
                    }
                } //if validation
                else
                {
                    this.MsgClose();
                }
            }
        }

        private void bbtnClose_Click(object sender, EventArgs e)
        {
            if (this.ClickCloseEvent != null)
            {
                ClickCloseEvent();
            }
        }

        //SPC-875 by Louis
        private void bsprMulData_MouseDown(object sender, MouseEventArgs e)
        {
            alSelect = this.bsprMulData.GetSelectedRows();
        }

        private void bsprMulValue_MouseDown(object sender, MouseEventArgs e)
        {
            alSelect = this.bsprMulValue.GetSelectedRows();
        }

        private void bsprMulValue_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (alSelect.Count > 0)
            {
                if (!(bool)this.bsprMulValue.GetCellValue(e.Row, 0))
                {
                    for (int i = 0; i < alSelect.Count; i++)
                    {
                        if (!this.bsprMulValue.ActiveSheet.Cells[(int)alSelect.GetByIndex(i), (int)iColIdx.SELECT].Locked)
                        {
                            this.bsprMulValue.ActiveSheet.SetText((int)alSelect.GetByIndex(i), 0, "False");
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < alSelect.Count; i++)
                    {
                        if (!this.bsprMulValue.ActiveSheet.Cells[(int)alSelect.GetByIndex(i), (int)iColIdx.SELECT].Locked)
                        {
                            this.bsprMulValue.ActiveSheet.SetText((int)alSelect.GetByIndex(i), 0, "True");
                        }
                    }
                }
            }
            alSelect.Clear();
        }

        private bool ValidationControlLimit(int rowIndex, int columnIndex)
        {
            DataTable dtConfig = _dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
            double upperSpec = double.NaN;
            double lowerSpec = double.NaN;
            string ModelconfigRawID = this.bsprMulValue.ActiveSheet.Cells[rowIndex, (int)iColIdx.RAWID].Value.ToString();

            if (columnIndex != 0)
            {
                foreach (DataRow drConfig in dtConfig.Rows)
                {
                    if (drConfig[COLUMN.RAWID].ToString() == ModelconfigRawID)
                    {
                        if (drConfig[COLUMN.UPPER_SPEC].ToString() != "" && drConfig[COLUMN.LOWER_SPEC].ToString() != "")
                        {
                            string upper = drConfig[COLUMN.UPPER_SPEC].ToString();
                            string lower = drConfig[COLUMN.LOWER_SPEC].ToString();
                            upperSpec = double.Parse(upper);
                            lowerSpec = double.Parse(lower);
                        }
                        else if (drConfig[COLUMN.UPPER_SPEC].ToString() != "" && drConfig[COLUMN.LOWER_SPEC].ToString() == "")
                        {
                            upperSpec = double.Parse(drConfig[COLUMN.UPPER_SPEC].ToString());
                        }
                        else if (drConfig[COLUMN.UPPER_SPEC].ToString() == "" && drConfig[COLUMN.LOWER_SPEC].ToString() != "")
                        {
                            lowerSpec = double.Parse(drConfig[COLUMN.LOWER_SPEC].ToString());
                        }
                    }
                }

                double dUCL = double.NaN;
                double dLCL = double.NaN;

                if (this.bsprMulValue.ActiveSheet.Cells[rowIndex, colIndexUCL].Value != null)
                {
                    dUCL = double.Parse(this.bsprMulValue.ActiveSheet.Cells[rowIndex, colIndexUCL].Value.ToString());
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALUE_IS_EMPTY", new string[] { "UCL" }, null);
                    this.bsprMulValue.ActiveSheet.Cells[rowIndex, colIndexUCL + 1].Value = null;
                    return false;
                }

                if (this.bsprMulValue.ActiveSheet.Cells[rowIndex, colIndexLCL].Value != null)
                {
                    dLCL = double.Parse(this.bsprMulValue.ActiveSheet.Cells[rowIndex, colIndexLCL].Value.ToString());
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALUE_IS_EMPTY", new string[] { "LCL" }, null);
                    this.bsprMulValue.ActiveSheet.Cells[rowIndex, colIndexUCL + 1].Value = null;
                    return false;
                }

                if (_sChartType == Definition.MULTICALC_CHART_TYPE.RAW || _sChartType == Definition.MULTICALC_CHART_TYPE.XBAR)
                {
                    if (dLCL < lowerSpec)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_SMALLER", new string[] { "LCL", "LSL" }, null);
                        return false;
                    }
                    else if (dUCL > upperSpec)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_BIGGER", new string[] { "UCL", "USL" }, null);
                        return false;
                    }
                    else if (dUCL < dLCL)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_BIGGER", new string[] { "LCL", "UCL" }, null);
                        return false;
                    }
                    else
                    {
                        double dCenter = (dUCL + dLCL) / 2;
                        this.bsprMulValue.ActiveSheet.Cells[rowIndex, colIndexUCL + 1].Value = dCenter;
                    }
                }
                else
                {
                    if (dUCL < dLCL)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_BIGGER", new string[] { "LCL", "UCL" }, null);
                        return false;
                    }
                    else
                    {
                        double dCenter = (dUCL + dLCL) / 2;
                        this.bsprMulValue.ActiveSheet.Cells[rowIndex, colIndexUCL + 1].Value = dCenter;
                    }
                }
            }
            return true;
        }

        private void bsprMulValue_EditModeOff(object sender, EventArgs e)
        {
            if (this.bsprMulValue.ActiveSheet.ActiveColumnIndex != (int)iColIdx.SELECT)
            {
                if (this.bsprMulValue.ActiveSheet.Cells[this.bsprMulValue.ActiveSheet.ActiveRowIndex, this.bsprMulValue.ActiveSheet.ActiveColumnIndex].Value != null)
                {
                    double result;
                    if (double.TryParse(this.bsprMulValue.ActiveSheet.Cells[this.bsprMulValue.ActiveSheet.ActiveRowIndex, this.bsprMulValue.ActiveSheet.ActiveColumnIndex].Value.ToString(), out result))
                    {
                        this.ValidationControlLimit(this.bsprMulValue.ActiveSheet.ActiveRowIndex, this.bsprMulValue.ActiveSheet.ActiveColumnIndex);
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_EMPTY_OR_NUM", null, null);
                    }
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_EMPTY_OR_NUM", null, null);
                }
            }
        }
    }
}
