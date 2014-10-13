using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.eSPC.Common.ATT;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;
using System.Collections;
using BISTel.eSPC.Page.ATT.Common;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.ATT.Modeling
{
    public partial class SPCMultiCalculationUC : BasePageUCtrl
    {
        #region :::Field

        public delegate void ClickCloseEventDelegate();
        public event ClickCloseEventDelegate ClickCloseEvent;
        
        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        LinkedList _llstSearchCondition = new LinkedList();

        BISTel.eSPC.Common.ATT.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.ATT.CommonUtility _ComUtil;

        MultiLanguageHandler _mlthandler;

        private SortedList _slParamColumnIndex = new SortedList();
        private SortedList _slSpcModelingIndex = new SortedList();
        private DataSet _dsSPCModeData = new DataSet();
        private DataTable _dtTableOriginal = new DataTable();
        private DataTable _dtSPCMultiCalcList = new DataTable();
        private DataTable _dtTableCalcOriginal = new DataTable();

        ChartInterface mChartVariable;
        SourceDataManager _dataManager = null;

        private string mStartTime = string.Empty;
        private string mEndTime = string.Empty;
        private string _sModelConfigRawIDforSave;
        private string _sMainYNfroSave;
        private ArrayList _sParamAlias;

        private string _OutOfSpec = "N";
        
        ArrayList arrOutlierList = new ArrayList();

        private string _sChartType = string.Empty;
        LinkedList _mllstChart = new LinkedList();
        

        private List<int> IstTempForSetOulierPreview = new List<int>();
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


        double _raw_lowerFilter = Double.NaN;
        double _raw_upperFilter = Double.NaN;
        private bool _bUseComma;

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

        public void InitializeMultiPopup()
        {
            this.InitializeMultiLayout();
            this.InitializePopup();
        }

        public void InitializeMultiLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.TITLE_SPC_MULTICALCULATION);
        }


        public SPCMultiCalculationUC()
        {
            InitializeComponent();
            this._mlthandler = MultiLanguageHandler.getInstance();
        }

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


        }
        public void InitializeLayout()
        {
            this.bDtStart.Value = DateTime.Now.AddDays(-7).Subtract(TimeSpan.FromHours(1));
            this.bDtEnd.Value = DateTime.Now.Subtract(TimeSpan.FromHours(-1));
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_ATT_MODELING, Definition.CONFIG_USE_COMMA, false);
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
            this.bsprMulValue.Locked = true;
            this.bsprMulValue.UseGeneralContextMenu = false;

            this.btxtThreshold.Enabled = false;
            this.bchkThresholdOff.Enabled = false;
            
        }

        private void InitializeCode()
        {
            LinkedList lk = new LinkedList();
            lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_ATT_CHART);
            lk.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            this.mDSChart = this._wsSPC.GetATTCodeData(lk.GetSerialData());

            if (!DataUtil.IsNullOrEmptyDataSet(this.mDSChart) && this.bcboChartType.Items.Count == 0)
            {
                _ComUtil.SetBComboBoxData(this.bcboChartType, this.mDSChart, BISTel.eSPC.Common.COLUMN.CODE, BISTel.eSPC.Common.COLUMN.CODE, "", true);

                foreach (DataRow dr in this.mDSChart.Tables[0].Rows)
                {
                    if (!this._mllstChart.Contains(dr[BISTel.eSPC.Common.COLUMN.CODE]))
                    {
                        this._mllstChart.Add(dr[BISTel.eSPC.Common.COLUMN.CODE], dr[BISTel.eSPC.Common.COLUMN.DESCRIPTION]);
                    }
                }
            }

            LinkedList _llstData = new LinkedList();
            DataSet dsContextType = _wsSPC.GetContextType(_llstData.GetSerialData());
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
                _llstSearchCondition.Clear();

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

                _dsSPCModeData = _wsSPC.GetATTSPCMulCalcModelDataPopup(_llstSearchCondition.GetSerialData());

                if (!DSUtil.CheckRowCount(_dsSPCModeData, BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC))
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                DataTable dtMaster = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC];
                DataTable dtConfig = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC];
                DataTable dtContext = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC];

                //#01. SPC Model Chart List를 위한 Datatable 생성
                DataTable dtSPCModelChartList = new DataTable();

                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.SELECT, typeof(Boolean));
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.CHART_ID);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.PARAM_ALIAS);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.MAIN_YN);
                
                int count = 0;
                int contextKeyCount = dtContext.Rows.Count / _sMulModelConfigRawID.Count;

                foreach (DataRow drContext in dtContext.Rows)
                {

                    if (count == contextKeyCount)
                        break;
                    dtSPCModelChartList.Columns.Add(drContext[BISTel.eSPC.Common.COLUMN.CONTEXT_KEY].ToString());
                    count += 1;
                }

                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.CREATE_BY);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.CREATE_DTTS);

                //#02. CONFIG MST에 생성된 CONTEXT COLUMN에 Data 입력
                foreach (DataRow drConfig in dtConfig.Rows)
                {
                    count =0;
                    DataRow drChartList = dtSPCModelChartList.NewRow();

                    drChartList[BISTel.eSPC.Common.COLUMN.CHART_ID] = drConfig[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.PARAM_ALIAS] = drConfig[BISTel.eSPC.Common.COLUMN.PARAM_ALIAS].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.MAIN_YN] = drConfig[BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString();

                    DataRow[] drMaster = dtMaster.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.RAWID, drConfig[BISTel.eSPC.Common.COLUMN.MODEL_RAWID]));
                    drChartList[BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME] = drMaster[0][BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString();

                    DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, drConfig[BISTel.eSPC.Common.COLUMN.RAWID]));

                  
                    foreach (DataRow drContext in drContexts)
                    {
                        if (count == contextKeyCount)
                            break;
                            drChartList[drContext[BISTel.eSPC.Common.COLUMN.CONTEXT_KEY].ToString()] = drContext[BISTel.eSPC.Common.COLUMN.CONTEXT_VALUE].ToString();
                            count++;
                    }

                    //MODEL 정보                
                    drChartList[BISTel.eSPC.Common.COLUMN.INTERLOCK_YN] = _ComUtil.NVL(drConfig[BISTel.eSPC.Common.COLUMN.INTERLOCK_YN], "N", true);

                    drChartList[BISTel.eSPC.Common.COLUMN.CREATE_BY] = drConfig[BISTel.eSPC.Common.COLUMN.CREATE_BY].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.CREATE_DTTS] = drConfig[BISTel.eSPC.Common.COLUMN.CREATE_DTTS] == DBNull.Value ? null : DateTime.Parse(drConfig[BISTel.eSPC.Common.COLUMN.CREATE_DTTS].ToString()).ToString(Definition.DATETIME_FORMAT_MS).ToString();

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

        private void bsprData_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
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


            if (bcboChartType.Text == "PN" || bcboChartType.Text == "C")
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
                    string.IsNullOrEmpty(btxtSamplePeriod.Text))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_SAMPLE_CONDITION", null, null);
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
            }

            ArrayList arrData = new ArrayList();
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
                case Definition.CHART_TYPE.P:
                    return Definition.BLOB_FIELD_NAME.P;
                case Definition.CHART_TYPE.PN:
                    return Definition.BLOB_FIELD_NAME.PN;
                case Definition.CHART_TYPE.C:
                    return Definition.BLOB_FIELD_NAME.C;
                case Definition.CHART_TYPE.U:
                    return Definition.BLOB_FIELD_NAME.U;
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
                DataTable dtMaster = this._dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC];
                DataTable dtConfig = this._dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC];
                DataTable dtContext = this._dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC];

                _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.SELECT, typeof(Boolean));
                _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.CHART_ID);
                _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.MAIN_YN);
                _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.SAVE_YN);
                _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.OUT_OF_SPEC);

                if (_sChartType == "P")
                {
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.P_UCL);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.P_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.P_LCL);
                    
                }
                else if (_sChartType == "PN")
                {
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_TARGET);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_UCL);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_LCL);
                    if (!this.bchkThresholdOff.Checked)
                    {
                        _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.THRESHOLD_UCL);
                        _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.THRESHOLD_TARGET);
                        _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.THRESHOLD_LCL);
                    }
                    

                }
                else if (_sChartType == "C")
                {
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_UPPERSPEC);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_TARGET);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_LOWERSPEC);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_UCL);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_LCL);
                    if (!this.bchkThresholdOff.Checked)
                    {
                        _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.THRESHOLD_UCL);
                        _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.THRESHOLD_TARGET);
                        _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.THRESHOLD_LCL);
                    }
                    
                }
                else if (_sChartType == "U")
                {
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.U_UCL);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.U_CENTER_LINE);
                    _dtSPCMultiCalcList.Columns.Add(BISTel.eSPC.Common.COLUMN.U_LCL);
                }

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
                   _ds = _wsSPC.GetATTSPCMulControlChartData(lnkChartSearchData.GetSerialData());
                   if (DataUtil.IsNullOrEmptyDataSet(_ds))
                   {
                       this.PROC_DATACalc(mDTChartData, _sMulValueModelConfiRawID[i].ToString());
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
                       }
                       else
                       {
                           bool result = false;
                           if (_sChartType == "PN" || _sChartType == "C")
                           {
                               if (this.bchkSampling.Checked)
                               {
                                   result = DrawSPCChart(true, int.Parse(btxtSamplePeriod.Text), int.Parse(btxtSampleCnt.Text), mDTChartData, _sMulValueModelConfiRawID[i].ToString());
                               }
                               else
                               {
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
                                   this.PROC_DATACalc(mDTChartData, _sMulValueModelConfiRawID[i].ToString());
                               }
                           }
                       }
                   }
                }
                this.MsgClose();
                MSGHandler.DisplayMessage(MSGType.Information, "Calculate completed!");
               
                _dtSPCMultiCalcList.AcceptChanges();
                this.bsprMulValue.DataSet = _dtSPCMultiCalcList;
                this._dtTableCalcOriginal = _dtSPCMultiCalcList.Copy();

                this.bsprMulValue.Locked = true;
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
                        DateTime.Parse(MultiCalcData.Rows[0][BISTel.eSPC.Common.COLUMN.RAW_DTTS].ToString().Split(';')[0]);
                        nextSampleStartDate = new DateTime(nextSampleStartDate.Year, nextSampleStartDate.Month, nextSampleStartDate.Day);
                        int sampleCountTemp = 0;

                        foreach (DataRow dr in MultiCalcData.Rows)
                        {
                            if (sample)
                            {
                                if (DateTime.Parse(dr[BISTel.eSPC.Common.COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
                                {
                                    while (DateTime.Parse(dr[BISTel.eSPC.Common.COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
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
                            if (dr[BISTel.eSPC.Common.COLUMN.RAW_DTTS].ToString().Length != 0)
                            {
                                if (dr[BISTel.eSPC.Common.COLUMN.RAW_DTTS].ToString().Split(';').Length == dr["RAW"].ToString().Split(';').Length && dr[BISTel.eSPC.Common.COLUMN.RAW_DTTS].ToString().Split(';').Length == dr[BISTel.eSPC.Common.COLUMN.PARAM_LIST].ToString().Split(';').Length)
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
                                            if (dtTemp.Columns[k].ColumnName.ToUpper() != BISTel.eSPC.Common.COLUMN.DEFAULT_CHART_LIST && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.OCAP_RAWID)
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

                if (MultiCalcData.Rows.Count > 0)
                {
                    this._dtDataSource = MultiCalcData.Clone();
                    DataTable dtTemp = new DataTable();
                    dtTemp = MultiCalcData.Clone();

                    DateTime nextSampleStartDate =
                                DateTime.Parse(MultiCalcData.Rows[0][BISTel.eSPC.Common.COLUMN.RAW_DTTS].ToString().Split(';')[0]);
                    nextSampleStartDate = new DateTime(nextSampleStartDate.Year, nextSampleStartDate.Month, nextSampleStartDate.Day);
                    int sampleCountTemp = 0;

                    foreach (DataRow dr in MultiCalcData.Rows)
                    {
                        if (sample)
                        {
                            if (DateTime.Parse(dr[BISTel.eSPC.Common.COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
                            {
                                while (DateTime.Parse(dr[BISTel.eSPC.Common.COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
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
                        if (dr[BISTel.eSPC.Common.COLUMN.RAW_DTTS].ToString().Length != 0)
                        {
                            if (_sChartType == "PN")
                            {
                                for (int i = 0; i < dr["pn"].ToString().Split(';').Length; i++)
                                {
                                    if (dr["pn"].ToString().Split(';')[i].Length > 0)
                                    {
                                        dtTemp.ImportRow(dr);
                                    }
                                }

                                for (int j = 0; j < dtTemp.Rows.Count; j++)
                                {
                                    dtTemp.Rows[j]["pn"] = dtTemp.Rows[j]["pn"].ToString().Split(';')[j];
                                    for (int k = 0; k < dtTemp.Columns.Count; k++)
                                    {
                                        if (dtTemp.Columns[k].ColumnName.ToUpper() != BISTel.eSPC.Common.COLUMN.DEFAULT_CHART_LIST && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.OCAP_RAWID)
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
                            else if (_sChartType == "C")
                            {
                                for (int i = 0; i < dr["c"].ToString().Split(';').Length; i++)
                                {
                                    if (dr["c"].ToString().Split(';')[i].Length > 0)
                                    {
                                        dtTemp.ImportRow(dr);
                                    }
                                }

                                for (int j = 0; j < dtTemp.Rows.Count; j++)
                                {
                                    dtTemp.Rows[j]["c"] = dtTemp.Rows[j]["c"].ToString().Split(';')[j];
                                    for (int k = 0; k < dtTemp.Columns.Count; k++)
                                    {
                                        if (dtTemp.Columns[k].ColumnName.ToUpper() != BISTel.eSPC.Common.COLUMN.DEFAULT_CHART_LIST && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.OCAP_RAWID)
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
                            else if (_sChartType == "P")
                            {
                                for (int i = 0; i < dr["p"].ToString().Split(';').Length; i++)
                                {
                                    if (dr["P"].ToString().Split(';')[i].Length > 0)
                                    {
                                        dtTemp.ImportRow(dr);
                                    }
                                }

                                for (int j = 0; j < dtTemp.Rows.Count; j++)
                                {
                                    dtTemp.Rows[j]["p"] = dtTemp.Rows[j]["p"].ToString().Split(';')[j];
                                    for (int k = 0; k < dtTemp.Columns.Count; k++)
                                    {
                                        if (dtTemp.Columns[k].ColumnName.ToUpper() != BISTel.eSPC.Common.COLUMN.DEFAULT_CHART_LIST && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.OCAP_RAWID)
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
                            else if (_sChartType == "U")
                            {
                                for (int i = 0; i < dr["u"].ToString().Split(';').Length; i++)
                                {
                                    if (dr["U"].ToString().Split(';')[i].Length > 0)
                                    {
                                        dtTemp.ImportRow(dr);
                                    }
                                }

                                for (int j = 0; j < dtTemp.Rows.Count; j++)
                                {
                                    dtTemp.Rows[j]["u"] = dtTemp.Rows[j]["u"].ToString().Split(';')[j];
                                    for (int k = 0; k < dtTemp.Columns.Count; k++)
                                    {
                                        if (dtTemp.Columns[k].ColumnName.ToUpper() != BISTel.eSPC.Common.COLUMN.DEFAULT_CHART_LIST && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.OCAP_RAWID)
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
            catch
            {
            }
            return true;
        }


        void bchkSampling_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (bchkSampling.Checked)
            {
                this.btxtSampleCnt.Enabled = true;
                this.btxtSamplePeriod.Enabled = true;
                
            }
            else
            {
                this.btxtSampleCnt.Enabled = false;
                this.btxtSamplePeriod.Enabled = false;
                this.btxtSampleCnt.Text = "";
                this.btxtSamplePeriod.Text = "";
            }
        }

        private void bcboChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bchkSampling.Checked)
            {
                if (bcboChartType.Text == "PN" || bcboChartType.Text == "C")
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
            else
            {
                if (bcboChartType.Text == "PN" || bcboChartType.Text == "C")
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
        private void MultiCalcValueDataBinding(double resultbar,double dUCL, double dLCL, string chartType 
            , string ModelconfigRawID, double threshold_UCL, double threshold_LCL,string result, string OutOfSpec)
        {
            DataTable dtMaster = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC];
            DataTable dtConfig = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC];
            DataTable dtContext = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC];

            
            foreach (DataRow drConfig in dtConfig.Rows)
            {
                if (drConfig[BISTel.eSPC.Common.COLUMN.RAWID].ToString().Equals(ModelconfigRawID))
                {
                    DataRow drChartList = _dtSPCMultiCalcList.NewRow();

                    drChartList[BISTel.eSPC.Common.COLUMN.CHART_ID] = drConfig[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.MAIN_YN] = drConfig[BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.SAVE_YN] = result;
                    drChartList[BISTel.eSPC.Common.COLUMN.OUT_OF_SPEC] = OutOfSpec;

                    resultbar = (dUCL + dLCL) / 2;

                    if (_sChartType == "P")
                    {
                        drChartList[BISTel.eSPC.Common.COLUMN.P_UCL] = dUCL;
                        drChartList[BISTel.eSPC.Common.COLUMN.P_CENTER_LINE] = resultbar.ToString();
                        drChartList[BISTel.eSPC.Common.COLUMN.P_LCL] = dLCL;
                    }
                    else if (_sChartType == "PN")
                    {
                        drChartList[BISTel.eSPC.Common.COLUMN.PN_UCL] = dUCL;
                        drChartList[BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE] = resultbar.ToString();
                        drChartList[BISTel.eSPC.Common.COLUMN.PN_LCL] = dLCL;
                        drChartList[BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC] = drConfig[BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC].ToString();
                        drChartList[BISTel.eSPC.Common.COLUMN.PN_TARGET] = drConfig[BISTel.eSPC.Common.COLUMN.PN_TARGET].ToString();
                        drChartList[BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC] = drConfig[BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC].ToString();

                        if (!this.bchkThresholdOff.Checked)
                        {
                            drChartList[BISTel.eSPC.Common.COLUMN.THRESHOLD_UCL] = threshold_UCL.ToString();
                            drChartList[BISTel.eSPC.Common.COLUMN.THRESHOLD_TARGET] = resultbar.ToString();
                            drChartList[BISTel.eSPC.Common.COLUMN.THRESHOLD_LCL] = threshold_LCL.ToString();
                        }

                    }

                    else if (_sChartType == "C")
                    {
                        drChartList[BISTel.eSPC.Common.COLUMN.C_UCL] = dUCL;
                        drChartList[BISTel.eSPC.Common.COLUMN.C_CENTER_LINE] = resultbar.ToString();
                        drChartList[BISTel.eSPC.Common.COLUMN.C_LCL] = dLCL;
                        drChartList[BISTel.eSPC.Common.COLUMN.C_UPPERSPEC] = drConfig[BISTel.eSPC.Common.COLUMN.C_UPPERSPEC].ToString();
                        drChartList[BISTel.eSPC.Common.COLUMN.C_TARGET] = drConfig[BISTel.eSPC.Common.COLUMN.C_TARGET].ToString();
                        drChartList[BISTel.eSPC.Common.COLUMN.C_LOWERSPEC] = drConfig[BISTel.eSPC.Common.COLUMN.C_LOWERSPEC].ToString();

                        if (!this.bchkThresholdOff.Checked)
                        {
                            drChartList[BISTel.eSPC.Common.COLUMN.THRESHOLD_UCL] = threshold_UCL.ToString();
                            drChartList[BISTel.eSPC.Common.COLUMN.THRESHOLD_TARGET] = resultbar.ToString();
                            drChartList[BISTel.eSPC.Common.COLUMN.THRESHOLD_LCL] = threshold_LCL.ToString();
                        }

                    }
                    else if (_sChartType == "U")
                    {
                        drChartList[BISTel.eSPC.Common.COLUMN.U_UCL] = dUCL;
                        drChartList[BISTel.eSPC.Common.COLUMN.U_CENTER_LINE] = resultbar.ToString();
                        drChartList[BISTel.eSPC.Common.COLUMN.U_LCL] = dLCL;

                    }
                    _dtSPCMultiCalcList.Rows.Add(drChartList);
                }
            }
           
           
        }

        private double GetPBarCalculation(DataTable dtCalcData)
        {
            ArrayList arPValueList = new ArrayList();
            ArrayList arSampleValueList = new ArrayList();
            double[] dsPNValueList;
            double[] dsSampleValueList;
            double totalSampleCnt = 0;
            double totalPCnt = 0;

            if (dtCalcData != null)
            {
                for (int i = 0; i < dtCalcData.Rows.Count; i++)
                {
                    if (dtCalcData.Rows[i]["PN"].ToString().Length > 0)
                    {
                        double dOutPut = 0;
                        bool isParse = double.TryParse(dtCalcData.Rows[i]["PN"].ToString(), out dOutPut);

                        if (isParse)
                            arPValueList.Add(dOutPut);
                    }
                    if (dtCalcData.Rows[i]["sample_count"].ToString().Length > 0)
                    {
                        double dOutPut = 0;
                        bool isParse = double.TryParse(dtCalcData.Rows[i]["sample_count"].ToString(), out dOutPut);

                        if (isParse)
                            arSampleValueList.Add(dOutPut);
                    }

                }
                dsPNValueList = (double[])arPValueList.ToArray(typeof(double));
                dsSampleValueList = (double[])arSampleValueList.ToArray(typeof(double));

                for (int i = 0; i < dsSampleValueList.Length; i++)
                {
                    totalSampleCnt += double.Parse(dsSampleValueList[i].ToString());
                }
                for (int i = 0; i < dsPNValueList.Length; i++)
                {
                    totalPCnt += double.Parse(dsPNValueList[i].ToString());
                }
            }

            return totalPCnt / totalSampleCnt;
        }

        private double GetNCountCalculation(DataTable dtCalcData)
        {
            ArrayList arPValueList = new ArrayList();
            ArrayList arSampleValueList = new ArrayList();
            double[] dsSampleValueList;
            double totalSampleCnt = 0;
            double sNCount = double.NaN;

            if (dtCalcData != null)
            {
                for (int i = 0; i < dtCalcData.Rows.Count; i++)
                {
                    if (dtCalcData.Rows[i]["sample_count"].ToString().Length > 0)
                    {
                        double dOutPut = 0;
                        bool isParse = double.TryParse(dtCalcData.Rows[i]["sample_count"].ToString(), out dOutPut);

                        if (isParse)
                            arSampleValueList.Add(dOutPut);
                    }

                }
                dsSampleValueList = (double[])arSampleValueList.ToArray(typeof(double));

                for (int i = 0; i < dsSampleValueList.Length; i++)
                {
                    totalSampleCnt += double.Parse(dsSampleValueList[i].ToString());
                }

                sNCount = totalSampleCnt / dsSampleValueList.Length;
            }


            return sNCount;

        }

        private void PROC_DATACalc(DataTable ChartData , string sMulModelConfigRawID)
        {
            try
            {
                double dPBar = double.NaN;
                double dPNBar = double.NaN;
                double dCBar = double.NaN;
                double dUBar = double.NaN;
                double dUCL = double.NaN;
                double dLCL = double.NaN;
                double dSpecLimit = double.NaN;
                double dNCount = 0;
                double dUnitCount = double.NaN;                
                double dResultBar = double.NaN;
                double thresholdUCL = double.NaN;
                double thresholdLCL = double.NaN;
                double UpperSpec = double.NaN;
                double LowerSpec = double.NaN;
                double threshold = double.NaN;

                string result = "N";

                if (!DataUtil.IsNullOrEmptyDataTable(ChartData))
                {
                    if (this.bcboChartType.Text.Equals("PN"))
                    {
                        dPBar = GetPBarCalculation(ChartData);
                        dNCount = GetNCountCalculation(ChartData);
                        dPNBar = dPBar * dNCount;

                        dSpecLimit = 3 * Math.Sqrt(dPNBar * (1 - dPBar));
                        dUCL = dPNBar + dSpecLimit;
                        dLCL = Math.Max(0, dPNBar - dSpecLimit);

                        dResultBar = dPNBar;
                    }
                    else if (this.bcboChartType.Text.Equals("P"))
                    {
                        dPBar = GetPBarCalculation(ChartData);

                        dNCount = GetNCountCalculation(ChartData);


                        dSpecLimit = 3 * Math.Sqrt(dPBar * ((1 - dPBar) / dNCount));
                        dUCL = dSpecLimit + dPBar;
                        dLCL = Math.Max(0, dPBar - dSpecLimit);
                        dResultBar = dPBar;
                    }
                    else if (this.bcboChartType.Text.Equals("C"))
                    {
                        ArrayList arCValueList = new ArrayList();
                        ArrayList arSampleValueList = new ArrayList();
                        double dCTotalCount = 0;
                        double dUCount = 0;

                        if (ChartData != null)
                        {
                            for (int i = 0; i < ChartData.Rows.Count; i++)
                            {
                                if (ChartData.Rows[i]["C"].ToString().Length > 0)
                                {
                                    double dOutPut = 0;
                                    bool isParse = double.TryParse(ChartData.Rows[i]["C"].ToString(), out dOutPut);

                                    if (isParse)
                                        arCValueList.Add(dOutPut);
                                }
                            }
                            for (int i = 0; i < arCValueList.Count; i++)
                            {
                                dCTotalCount += double.Parse(arCValueList[i].ToString());
                            }
                            for (int i = 0; i < ChartData.Rows.Count; i++)
                            {
                                if (ChartData.Rows[i]["sample_count"].ToString().Length > 0)
                                {
                                    double dOutPut = 0;
                                    bool isParse = double.TryParse(ChartData.Rows[i]["sample_count"].ToString(), out dOutPut);

                                    if (isParse)
                                        arSampleValueList.Add(dOutPut);
                                }

                            }

                            for (int i = 0; i < arSampleValueList.Count; i++)
                            {
                                dUCount += double.Parse(arSampleValueList[i].ToString());
                            }


                            dCBar = dCTotalCount / dUCount;
                            dUnitCount =

                            dUCL = dCBar + (3 * Math.Sqrt(dCBar));
                            dLCL = Math.Max(0, dCBar - (3 * Math.Sqrt(dCBar)));

                            dResultBar = dCBar;
                        }
                    }
                    else if (this.bcboChartType.Text.Equals("U"))
                    {
                        ArrayList arUnitValueList = new ArrayList();
                        ArrayList arSampleValueList = new ArrayList();
                        double dUnitTotalCount = 0;

                        if (ChartData != null)
                        {
                            for (int i = 0; i < ChartData.Rows.Count; i++)
                            {
                                if (ChartData.Rows[i]["unit_count"].ToString().Length > 0)
                                {
                                    double dOutPut = 0;
                                    bool isParse = double.TryParse(ChartData.Rows[i]["unit_count"].ToString(), out dOutPut);

                                    if (isParse)
                                        arUnitValueList.Add(dOutPut);
                                }

                            }

                            for (int i = 0; i < arUnitValueList.Count; i++)
                            {
                                dUnitTotalCount += double.Parse(arUnitValueList[i].ToString());
                            }

                            for (int i = 0; i < ChartData.Rows.Count; i++)
                            {
                                if (ChartData.Rows[i]["C"].ToString().Length > 0)
                                {
                                    double dOutPut = 0;
                                    bool isParse = double.TryParse(ChartData.Rows[i]["C"].ToString(), out dOutPut);

                                    if (isParse)
                                        arSampleValueList.Add(dOutPut);
                                }

                            }

                            for (int i = 0; i < arSampleValueList.Count; i++)
                            {
                                dNCount += double.Parse(arSampleValueList[i].ToString());
                            }

                            dUBar = dNCount / dUnitTotalCount;
                            dUnitCount = dUnitTotalCount / arSampleValueList.Count;

                            dUCL = dUBar + (3 * Math.Sqrt(dUBar / dUnitCount));
                            dLCL = Math.Max(0, dUBar - (3 * Math.Sqrt(dUBar / dUnitCount)));

                            dResultBar = dUBar;
                        }
                    }

                    if (_sChartType == "PN" || _sChartType == "C")
                    {
                        if (DSUtil.CheckRowCount(_dsSPCModeData, BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC))
                        {
                            DataTable dtConfig = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC];

                            foreach (DataRow drConfig in dtConfig.Rows)
                            {
                                if (drConfig["RAWID"].ToString().Contains(sMulModelConfigRawID))
                                {
                                    if (_sChartType == "PN")
                                    {
                                        UpperSpec = Convert.ToDouble(drConfig[Definition.CHART_COLUMN.PN_USL].ToString());
                                        LowerSpec = Convert.ToDouble(drConfig[Definition.CHART_COLUMN.PN_LSL].ToString());
                                    }
                                    else if (_sChartType == "C")
                                    {
                                        UpperSpec = Convert.ToDouble(drConfig[Definition.CHART_COLUMN.C_USL].ToString());
                                        LowerSpec = Convert.ToDouble(drConfig[Definition.CHART_COLUMN.C_LSL].ToString());
                                    }
                                }
                            }
                        }

                        if (!this.bchkThresholdOff.Checked)
                        {
                            double thresholdSpecLimit = 0;
                            if (!UpperSpec.Equals(double.NaN) && !LowerSpec.Equals(double.NaN))
                            {
                                double upper_threshold = UpperSpec - dResultBar;
                                double lower_threshold = dResultBar - LowerSpec;
                                threshold = Math.Min(Math.Abs(upper_threshold), Math.Abs(lower_threshold));
                                thresholdSpecLimit = threshold * (double.Parse(this.btxtThreshold.Text) * 0.01);
                                thresholdUCL = dResultBar + thresholdSpecLimit;
                                thresholdLCL = dResultBar - thresholdSpecLimit;

                                if (thresholdUCL > dUCL)
                                {
                                    dUCL = thresholdUCL;
                                }
                                if (thresholdLCL < dLCL)
                                {
                                    dLCL = thresholdLCL;
                                }
                            }
                            else if (!UpperSpec.Equals(double.NaN) && LowerSpec.Equals(double.NaN))
                            {
                                threshold = dResultBar - LowerSpec;
                                thresholdSpecLimit = threshold * (double.Parse(this.btxtThreshold.Text) * 0.01);
                                thresholdUCL = dResultBar + thresholdSpecLimit;
                                thresholdLCL = dResultBar - thresholdSpecLimit;

                                if (thresholdUCL > dUCL)
                                {
                                    dUCL = thresholdUCL;
                                }
                                if (thresholdLCL < dLCL)
                                {
                                    dLCL = thresholdLCL;
                                }
                            }
                            else if (UpperSpec.Equals(double.NaN) && !LowerSpec.Equals(double.NaN))
                            {
                                threshold = UpperSpec - dResultBar;
                                thresholdSpecLimit = threshold * (double.Parse(this.btxtThreshold.Text) * 0.01);
                                thresholdUCL = dResultBar + thresholdSpecLimit;
                                thresholdLCL = dResultBar - thresholdSpecLimit;

                                if (thresholdUCL > dUCL)
                                {
                                    dUCL = thresholdUCL;
                                }
                                if (thresholdLCL < dLCL)
                                {
                                    dLCL = thresholdLCL;
                                }
                            }
                            else
                            {
                                thresholdSpecLimit = double.NaN;
                                thresholdUCL = double.NaN;
                                thresholdLCL = double.NaN;
                            }

                            if (dUCL > UpperSpec || dLCL < LowerSpec)
                            {
                                result = "N";
                                _OutOfSpec = "Y";
                            }
                            else
                            {
                                result = "Y";
                            }
                            MultiCalcValueDataBinding(Math.Round(dResultBar, 10), Math.Round(dUCL, 10), Math.Round(dLCL, 10), this._sChartType, sMulModelConfigRawID
                                                          , Math.Round(thresholdUCL,10),Math.Round(thresholdLCL,10), result, _OutOfSpec);
                        }
                        else
                        {
                            if (dUCL > UpperSpec || dLCL < LowerSpec)
                            {
                                result = "N";
                                _OutOfSpec = "Y";
                            }
                            else
                            {
                                result = "Y";
                            }
                            MultiCalcValueDataBinding(Math.Round(dResultBar, 10), Math.Round(dUCL, 10), Math.Round(dLCL, 10), this._sChartType, sMulModelConfigRawID
                                                         , Math.Round(thresholdUCL, 10), Math.Round(thresholdLCL, 10), result, _OutOfSpec);
                        }
                    }
                    else
                    {
                        result = "Y";
                        MultiCalcValueDataBinding(Math.Round(dResultBar, 10), Math.Round(dUCL, 10), Math.Round(dLCL, 10), this._sChartType, sMulModelConfigRawID
                                                     , Math.Round(thresholdUCL, 10), Math.Round(thresholdLCL, 10), result, _OutOfSpec);
                    }
                }
                else
                {
                    MultiCalcValueDataBinding(Math.Round(dResultBar, 10), Math.Round(dUCL, 10), Math.Round(dLCL, 10), this._sChartType, sMulModelConfigRawID
                                                 , Math.Round(thresholdUCL, 10), Math.Round(thresholdLCL, 10), result, _OutOfSpec);
                }
            }

            catch
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_ERROR_CALC", null, null);
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
        }

        private void btxtThreshold_TextChanged(object sender, EventArgs e)
        {
            CommonUtility commutil = new CommonUtility();
            if (this.btxtThreshold.Text != "")
            {
                if (!commutil.IsInteger(this.btxtThreshold.Text) || int.Parse(this.btxtThreshold.Text) < 1 || int.Parse(this.btxtThreshold.Text) > 100)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_ENTER_INT", null, null);
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
                foreach(DataRow dr in dtSaveData.Rows)
                {
                    string a = dr[BISTel.eSPC.Common.COLUMN.SELECT].ToString();
                    if (a == "True")
                    {
                        LinkedList SaveData = new LinkedList();
                        SaveData.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID,dr[BISTel.eSPC.Common.COLUMN.CHART_ID].ToString() );
                        SaveData.Add(BISTel.eSPC.Common.COLUMN.MAIN_YN, dr[BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString());
                        if (_sChartType == "PN")
                        {
                            SaveData.Add(BISTel.eSPC.Common.COLUMN.PN_UCL, dr[BISTel.eSPC.Common.COLUMN.PN_UCL].ToString());
                            SaveData.Add(BISTel.eSPC.Common.COLUMN.PN_LCL, dr[BISTel.eSPC.Common.COLUMN.PN_LCL].ToString());                            
                        }
                        else if (_sChartType == "P")
                        {
                            SaveData.Add(BISTel.eSPC.Common.COLUMN.P_UCL, dr[BISTel.eSPC.Common.COLUMN.P_UCL].ToString());
                            SaveData.Add(BISTel.eSPC.Common.COLUMN.P_LCL, dr[BISTel.eSPC.Common.COLUMN.P_LCL].ToString());
                        }
                        else if (_sChartType == "C")
                        {
                            SaveData.Add(BISTel.eSPC.Common.COLUMN.C_UCL, dr[BISTel.eSPC.Common.COLUMN.C_UCL].ToString());
                            SaveData.Add(BISTel.eSPC.Common.COLUMN.C_LCL, dr[BISTel.eSPC.Common.COLUMN.C_LCL].ToString());
                        } 
                        else if (_sChartType == "U")
                        {
                            SaveData.Add(BISTel.eSPC.Common.COLUMN.U_UCL, dr[BISTel.eSPC.Common.COLUMN.U_UCL].ToString());
                            SaveData.Add(BISTel.eSPC.Common.COLUMN.U_LCL, dr[BISTel.eSPC.Common.COLUMN.U_LCL].ToString());
                        }

                        this.MsgShow(COMMON_MSG.Save_Data);

                        SaveData.Add("CHART_TYPE", this._sChartType);
                        SaveData.Add(Definition.CONDITION_KEY_USER_ID, this.sessionData.UserId);

                        bSuccess = this._wsSPC.SaveSPCATTMulitiCalcModelData(SaveData.GetSerialData());
                        

                       

                    }
                }
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
            }
        }

        private void bbtnClose_Click(object sender, EventArgs e)
        {
            if (this.ClickCloseEvent != null)
            {
                ClickCloseEvent();
            }
        }

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

    }
}
