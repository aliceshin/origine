using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common.ATT;

using Steema.TeeChart;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.ATT.Modeling
{
    public partial class AutoCalcConfigurationUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        MultiLanguageHandler _mlthandler;


        BISTel.eSPC.Common.ATT.CommonUtility _ComUtil;
        private BISTel.eSPC.Common.ConfigMode _cofingMode;
        private string _sConfigRawID;
        private string _CalcCount = "0";

        private DataTable _dtAutoCalc;

        #endregion


        #region ::: Properties


        #endregion


        #region ::: Constructor

        
        public AutoCalcConfigurationUC()
        {
            InitializeComponent();
        }

        public AutoCalcConfigurationUC(bool isDefaultModel)
        {
            InitializeComponent();

            this.bchkAutoCalcIQR.Visible = false;
            this.bchkAutoCalcShift.Visible = false;

            if (isDefaultModel)
            {
                this.bLabel3.Visible = false;
                this.blblAutoInitialCalcCount.Visible = false;
                this.btxtAutoInitialCalcCount.Visible = false;
            }
        }

        #endregion

        #region ::: Override Method

        public override void PageInit()
        {
            this.InitializePage();
        }

        #endregion

        #region ::: PageLoad & Initialize

        public void InitializePage()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();

            this._ComUtil = new CommonUtility();

            this.InitializeDataButton();

            this.blblMinSampleCnt.Text = this._mlthandler.GetVariable(Definition.LABEL.MINSAMPLECNT);
        }


        public void InitializeDataButton()
        {
            this.FunctionName = Definition.FUNC_KEY_SPC_ATT_MODELING;
        }

        
        #endregion


        #region :DATA PROPERTY
        //DATA PROPERTY
        public BISTel.eSPC.Common.ConfigMode CONFIG_MODE
        {
            get { return _cofingMode; }
            set { _cofingMode = value; }
        }

        public string CONFIG_RAWID
        {
            get { return _sConfigRawID; }
            set { _sConfigRawID = value; }
        }

        public string AUTOCALC_PERIOD
        {
            get { return this.btxtAutoCalcPeriod.Text; }
            set { this.btxtAutoCalcPeriod.Text = value; }
        }

        public string CALC_COUNT
        {
            get { return this.btxtAutoCalcCount.Text; }
            set { this.btxtAutoCalcCount.Text = value; }
        }

        public string INITIAL_CALC_COUNT
        {
            get { return this.btxtAutoInitialCalcCount.Text;  }
            set { this.btxtAutoInitialCalcCount.Text = value; }
                
        }

        public string MIN_SAMPLES
        {
            get { return this.btxtMinSamples.Text; }
            set { this.btxtMinSamples.Text = value; }
        }

        public string DEFAULT_PERIOD
        {
            get { return this.btxtDefaultPeriod.Text; }
            set { this.btxtDefaultPeriod.Text = value; }
        }

        public string MAX_PERIOD
        {
            get { return this.btxtMaxPeriod.Text; }
            set { this.btxtMaxPeriod.Text = value; }
        }


        public string CONTROL_THRESHOLD
        {
            get { return this.btxtControlThreshold.Text; }
            set { this.btxtControlThreshold.Text = value; }
        }


        public DataTable AUTOCALC_DATASET
        {
            get
            {
                return _dtAutoCalc;
            }
            set
            {
                _dtAutoCalc = value;

                if (_dtAutoCalc != null)
                {
                    //# Data가 없을 경우 Row를 하나 생성한다.
                    if (_dtAutoCalc.Rows.Count.Equals(0))
                    {
                        DataRow newRow = _dtAutoCalc.NewRow();
                        newRow[BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID] = _ComUtil.NVL(_sConfigRawID, "-1", true);

                        _dtAutoCalc.Rows.Add(newRow);
                    }

                    //# Default 값을 입력
                    if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.AUTOCALC_PERIOD].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.AUTOCALC_PERIOD] = "30";

                    if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.MIN_SAMPLES].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.MIN_SAMPLES] = "30";

                    if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.DEFAULT_PERIOD].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.DEFAULT_PERIOD] = "30";

                    if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.MAX_PERIOD].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.MAX_PERIOD] = "90";

                    if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.CONTROL_LIMIT].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.CONTROL_LIMIT] = "3";

                    if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.CONTROL_THRESHOLD].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.CONTROL_THRESHOLD] = "50";

                    if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.P_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.P_CL_YN] = "Y";

                    if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.PN_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.PN_CL_YN] = "Y";

                    if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.U_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.U_CL_YN] = "Y";

                    if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.C_CL_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.C_CL_YN] = "Y";
                   
                    if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.THRESHOLD_OFF_YN].Equals(DBNull.Value))
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.THRESHOLD_OFF_YN] = 'N';
                    
                    if (_cofingMode == BISTel.eSPC.Common.ConfigMode.MODIFY || _cofingMode == BISTel.eSPC.Common.ConfigMode.ROLLBACK)
                    {
                        if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.PN_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.PN_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.P_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.P_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.C_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.C_CALC_COUNT] = 0;

                        if (_dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.U_CALC_COUNT].Equals(DBNull.Value))
                            _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.U_CALC_COUNT] = 0;
                        
                    }
                    //# Control에 DataBinding 하기

                    //AUTOCALC_PERIOD
                    this.btxtAutoCalcPeriod.DataBindings.Add("Text", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.AUTOCALC_PERIOD);

                    //Init_CALC
                    this.btxtAutoInitialCalcCount.DataBindings.Add("Text", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.INITIAL_CALC_COUNT, true, DataSourceUpdateMode.Never);
                    
                    //CALC_
                    this.btxtAutoCalcCount.DataBindings.Add("Text", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.CALC_COUNT, true, DataSourceUpdateMode.Never);
                    this._CalcCount = CALC_COUNT;

                    //MIN_SAMPLES
                    this.btxtMinSamples.DataBindings.Add("Text", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.MIN_SAMPLES);

                    //DEFAULT_PERIOD
                    this.btxtDefaultPeriod.DataBindings.Add("Text", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.DEFAULT_PERIOD);

                    //MAX_PERIOD
                    this.btxtMaxPeriod.DataBindings.Add("Text", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.MAX_PERIOD);

                    //CONTROL_THRESHOLD
                    this.btxtControlThreshold.DataBindings.Add("Text", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.CONTROL_THRESHOLD);

                    if (_cofingMode == BISTel.eSPC.Common.ConfigMode.MODIFY || _cofingMode == BISTel.eSPC.Common.ConfigMode.ROLLBACK)
                    {                        
                        this.btxtPNCalcCount.DataBindings.Add("Text", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.P_CALC_COUNT);

                        this.btxtCCalcCount.DataBindings.Add("Text", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.PN_CALC_COUNT);

                        this.btxtPCalcCount.DataBindings.Add("Text", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.U_CALC_COUNT);

                        this.btxtUCalcCount.DataBindings.Add("Text", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.C_CALC_COUNT);

                    }
                   
                    //ATT//
                    Binding bdPCLYN = new Binding("Checked", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.P_CL_YN);
                    bdPCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdPCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkPCalcYN.DataBindings.Add(bdPCLYN);

                    Binding bdCCLYN = new Binding("Checked", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.C_CL_YN);
                    bdCCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdCCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkCCalcYN.DataBindings.Add(bdCCLYN);

                    Binding bdPNCLYN = new Binding("Checked", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.PN_CL_YN);
                    bdPNCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdPNCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkPNCalcYN.DataBindings.Add(bdPNCLYN);

                    Binding bdUCLYN = new Binding("Checked", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.U_CL_YN);
                    bdUCLYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdUCLYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkUCalcYN.DataBindings.Add(bdUCLYN);

                    //WITHOUT_IQR_YN
                    Binding bdIQRFilterYN = new Binding("Checked", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.WITHOUT_IQR_YN);
                    bdIQRFilterYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdIQRFilterYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkAutoCalcIQR.DataBindings.Add(bdIQRFilterYN);

                    //THRESHOLD_OFF_YN
                    Binding bdThresholdOffYN = new Binding("Checked", _dtAutoCalc, BISTel.eSPC.Common.COLUMN.THRESHOLD_OFF_YN);
                    bdThresholdOffYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdThresholdOffYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkThresholdOffYN.DataBindings.Add(bdThresholdOffYN);

                    this.InitializeLayout(this._cofingMode);
                }
            }
        }

        public void InitializeLayout(BISTel.eSPC.Common.ConfigMode configMode)
        {
            switch (configMode)
            {
                case BISTel.eSPC.Common.ConfigMode.DEFAULT:
                    this.btxtPNCalcCount.Visible = false;
                    this.btxtCCalcCount.Visible = false;
                    this.btxtPCalcCount.Visible = false;
                    this.btxtUCalcCount.Visible = false;

                    break;
                case BISTel.eSPC.Common.ConfigMode.CREATE_MAIN:
                    this.btxtPNCalcCount.Visible = false;
                    this.btxtCCalcCount.Visible = false;
                    this.btxtPCalcCount.Visible = false;
                    this.btxtUCalcCount.Visible = false;

                    break;
                case BISTel.eSPC.Common.ConfigMode.CREATE_SUB:
                    this.btxtPNCalcCount.Visible = false;
                    this.btxtCCalcCount.Visible = false;
                    this.btxtPCalcCount.Visible = false;
                    this.btxtUCalcCount.Visible = false;

                    break;
                case BISTel.eSPC.Common.ConfigMode.MODIFY:
                    break;

                case BISTel.eSPC.Common.ConfigMode.VIEW:
                    this.gbAutoCalcSelection.Enabled = false;
                    this.gbAutoCalcSetting.Enabled = false;
                    break;
            }
        }

        private void StringYNToBoolean(object sender, ConvertEventArgs cevent)
        {
            if (cevent.DesiredType != typeof(Boolean)) return;

            if (cevent.Value.Equals("Y"))
                cevent.Value = true;
            else
                cevent.Value = false;
        }

        private void BooleanToStringYN(object sender, ConvertEventArgs cevent)
        {
            if (cevent.DesiredType != typeof(string)) return;

            if (cevent.Value.Equals(true))
                cevent.Value = "Y";
            else
                cevent.Value = "N";
        }


        public bool InputValidation(bool isMsgDisplay)
        {
            return this.InputValidation(isMsgDisplay, false);
        }
        
        public bool InputValidation(bool isMsgDisplay, bool isEachCheck)
        {
            bool result = true;
            bool calcCount = false;

            if (isEachCheck)
            {
                
                if (AUTOCALC_PERIOD.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_AUTO_CALC_PERIOD", null, null);
                    result = false;
                }

                if (MIN_SAMPLES.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_MIN_SAMPLE", null, null);
                    result = false;
                }

                if (DEFAULT_PERIOD.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_DEFUALT_PERIOD", null, null);
                    result = false;
                }

                if (MAX_PERIOD.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_MAX_PERIOD", null, null);
                    result = false;
                }

                if (CONTROL_THRESHOLD.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_THRESHOLD", null, null);
                    result = false;
                }
            }
            else
            {
                if (AUTOCALC_PERIOD.Length.Equals(0) || MIN_SAMPLES.Length.Equals(0) || DEFAULT_PERIOD.Length.Equals(0) ||
                    MAX_PERIOD.Length.Equals(0) || CONTROL_THRESHOLD.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_AUTO_CALC_OPTION", null, null);
                    result = false;
                }

                if (double.Parse(CONTROL_THRESHOLD) > 99 || double.Parse(CONTROL_THRESHOLD) < 0 )
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_INPUT_NUM", null, null);
                    result = false;
                }
                if (CONTROL_THRESHOLD.Length > 5)
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_DECIMAL", null, null);
                    result = false;
                }

                if (CONTROL_THRESHOLD == "0")
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_THRESHOLD", null, null);
                    result = false;
                }

                if (this._CalcCount.Equals(null) || this._CalcCount.Equals(""))
                {
                    if (!CALC_COUNT.Equals(null) && !CALC_COUNT.Equals(""))
                    {
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.INITIAL_CALC_COUNT] = CALC_COUNT;
                        calcCount = true;
                    }
                }
                else if (CALC_COUNT.Equals(null) || CALC_COUNT.Equals(""))
                {
                    if (!this._CalcCount.Equals(null) && !this._CalcCount.Equals(""))
                    {
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.INITIAL_CALC_COUNT] = DBNull.Value;
                        calcCount = true;
                    }
                }
                else
                {
                    if (double.Parse(CALC_COUNT) != double.Parse(this._CalcCount))
                    {
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.INITIAL_CALC_COUNT] = CALC_COUNT;
                        calcCount = true;

                        
                    }
                }
                if (_cofingMode == BISTel.eSPC.Common.ConfigMode.MODIFY || _cofingMode == BISTel.eSPC.Common.ConfigMode.ROLLBACK)
                {
                    if (calcCount)
                    {
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.RAW_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.MEAN_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.STD_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.RANGE_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_STD_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_MEAN_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_RANGE_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.MA_CALC_COUNT] = 0;
                        _dtAutoCalc.Rows[0][BISTel.eSPC.Common.COLUMN.MS_CALC_COUNT] = 0;
                    }
                }
            }

            return result;
        }


        #endregion


        #region ::: User Defined Method.

        private void SetConfigRow(string argColumn, BTextBox bTxtBox)
        {
            if (bTxtBox.Text.Length > 0)
            {
                try
                {
                    _dtAutoCalc.Rows[0][argColumn] = double.Parse(bTxtBox.Text.Trim());
                }
                catch
                {
                    _dtAutoCalc.Rows[0][argColumn] = DBNull.Value;
                    bTxtBox.Text = null;
                }
            }
            else
                 _dtAutoCalc.Rows[0][argColumn] = DBNull.Value;
        }

        public void SetUnitOfValue(string sUnit)
        {
            blblMinSampleCnt.Text = sUnit;
        }
        #endregion

        #region ::: EventHandler

        private void btxtSpec_Validated(object sender, EventArgs e)
        {
            BTextBox btxtMst = (BTextBox)sender;

            if (btxtMst.Name.Equals(this.btxtAutoCalcCount.Name))
            {
                this.SetConfigRow(BISTel.eSPC.Common.COLUMN.CALC_COUNT, btxtMst);
            }
        }

        #endregion


    }

}
