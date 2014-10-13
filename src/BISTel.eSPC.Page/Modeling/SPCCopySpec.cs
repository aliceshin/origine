using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.Modeling
{
    public partial class SPCCopySpec : BasePopupFrm
    {
        public const string POPUP_TITLE = "SPC Model Copy";
        
        Initialization _Initialization = null;
        MultiLanguageHandler _mlthandler = null;
        SessionData _sessionData = null;

        eSPCWebService.eSPCWebService _ws = null;

        string _configRawID = "";
        string _eqpModel = "";
        string _spcModelName = "";
        string _paramAlias = "";
        string _mainYN = "";
        string _area = "";

        int _contextCount = 0;
        int _ruleCount = 0;
        int _optionCount = 0;
        int _autoCalcCount = 0;
        int _tempCount = 0;

        bool _isClosedOK = false; //SPC-1297, KBLEE

        SpcModelInfo_ForCopy _oldSpcModelInfo = null;
  
        
        public SPCCopySpec()
        {
            InitializeComponent();

            this.bbtnOK.Click += new EventHandler(bbtnOK_Click);
            this.bbtnCancel.Click += new EventHandler(bbtnCancel_Click);

            this.chkAllContext.CheckedChanged += new EventHandler(chkAllContext_CheckedChanged);
            this.chkAllRule.CheckedChanged += new EventHandler(chkAllRule_CheckedChanged);
            this.chkAllOption.CheckedChanged += new EventHandler(chkAllOption_CheckedChanged);
            this.chkAllAutoCalculation.CheckedChanged += new EventHandler(chkAllAutoCalculation_CheckedChanged);
        }




        ///////////////////////////////////////////////////////////////////////////////////
        //  PageLoad & Initialize.
        ///////////////////////////////////////////////////////////////////////////////////

        public string CONFIG_RAWID
        {
            get { return this._configRawID; }
        }

        #region CONTEXT Info.
        //SPC-676 By Louis
        public string CONTEXT_CHART_DESCRIPTION
        {
            get { return this.chkChartDesc.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_INTERLOCK
        {
            get { return this.chkInterlock.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_USE_EXTERNAL_SPEC_LIMIT
        {
            get { return this.chkUseExternalSpecLimit.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_AUTO_CALCULATION
        {
            get { return this.chkAutoCalculation.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_AUTO_GENERATE_SUB_CHART
        {
            get { return this.chkAutoGenerateSubChart.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_ACTIVE
        {
            get { return this.chkActive.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_SAMPLE_COUNT
        {
            get { return this.chkSampleCount.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_MANAGE_TYPE
        {
            get { return this.chkManageType.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_AUTO_SETTING
        {
            get { return this.chkAutoSetting.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK
        {
            get { return this.chkGenerateSubChartWithInterlock.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION
        {
            get { return this.chkGenerateSubChartWithAutoCalculation.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_USE_NORMALIZATION_VALUE
        {
            get { return this.chkUseNorm.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_INHERIT_THE_SPEC_OF_MAIN
        {
            get { return this.chkInherittheSpecOfMain.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_MODE
        {
            get { return this.chkMode.Checked ? "Y" : "N"; }
        }
        #endregion
        
        #region RULE Info.
        public string RULE_MASTER_SPEC_LIMIT
        {
            get { return this.chkMasterSpecLimit.Checked ? "Y" : "N"; }
        }

        public string RULE_RAW
        {
            get { return this.chkRaw.Checked ? "Y" : "N"; }
        }

        public string RULE_MEAN
        {
            get { return this.chkMean.Checked ? "Y" : "N"; }
        }

        public string RULE_STD
        {
            get { return this.chkSTD.Checked ? "Y" : "N"; }
        }

        public string RULE_RANGE
        {
            get { return this.chkRange.Checked ? "Y" : "N"; }
        }

        public string RULE_EWMA_MEAN
        {
            get { return this.chkEWMAMean.Checked ? "Y" : "N"; }
        }

        public string RULE_EWMA_STD
        {
            get { return this.chkEWMASTD.Checked ? "Y" : "N"; }
        }

        public string RULE_EWMA_RANGE
        {
            get { return this.chkEWMARange.Checked ? "Y" : "N"; }
        }

        public string RULE_MA
        {
            get { return this.chkMA.Checked ? "Y" : "N"; }
        }

        public string RULE_MS
        {
            get { return this.chkMS.Checked ? "Y" : "N"; }
        }

        public string RULE_MR
        {
            get { return this.chkMR.Checked ? "Y" : "N"; }
        }

        public string RULE_TECHNICAL_LIMIT
        {
            get { return this.chkTechLimit.Checked ? "Y" : "N"; }
        }

        public string RULE_MOVING_OPTIONS
        {
            get { return this.chkMovingOptions.Checked ? "Y" : "N"; }
        }

        public string RULE_MEAN_OPTIONS
        {
            get { return this.chkMeanOptions.Checked ? "Y" : "N"; }
        }

        public string RULE_ZONE_A
        {
            get { return this.chkZoneA.Checked ? "Y" : "N"; }
        }

        public string RULE_ZONE_B
        {
            get { return this.chkZoneB.Checked ? "Y" : "N"; }
        }

        public string RULE_ZONE_C
        {
            get { return this.chkZoneC.Checked ? "Y" : "N"; }
        }

        public string RULE_RULE_SELECTION
        {
            get { return this.chkRuleSelection.Checked ? "Y" : "N"; }
        }
        #endregion

        #region OPTION Info.
        public string OPTION_PARAMETER_CATEGORY
        {
            get { return this.chkParameterCategory.Checked ? "Y" : "N"; }
        }

        public string OPTION_CALCULATE_PPK
        {
            get { return this.chkCalculatePpk.Checked ? "Y" : "N"; }
        }

        public string OPTION_PRIORITY
        {
            get { return this.chkPriority.Checked ? "Y" : "N"; }
        }

        public string OPTION_SAMPLE_COUNTS
        {
            get { return this.chkSampleCounts.Checked ? "Y" : "N"; }
        }

        public string OPTION_DAYS
        {
            get { return this.chkDays.Checked ? "Y" : "N"; }
        }

        public string OPTION_DEFAULT_CHART_TO_SHOW
        {
            get { return this.chkDefaultChartToShow.Checked ? "Y" : "N"; }
        }
        #endregion

        #region AUTO Calculation Info.
        public string AUTO_AUTO_CALCULATION_PERIOD
        {
            get { return this.chkAutoCalculationPeriod.Checked ? "Y" : "N"; }
        }

        public string AUTO_AUTO_CALCULATION_COUNT
        {
            get { return this.chkAutoCalculationCount.Checked ? "Y" : "N"; }
        }

        public string AUTO_MINIMUM_SAMPLES_TO_USE
        {
            get { return this.chkMinimumSamplesToUse.Checked ? "Y" : "N"; }
        }

        public string AUTO_DEFAULT_PERIOD
        {
            get { return this.chkDefaultPeriod.Checked ? "Y" : "N"; }
        }

        public string AUTO_MAXIMUM_PERIOD_TO_USE
        {
            get { return this.chkMaximumPeriodToUse.Checked ? "Y" : "N"; }
        }

        public string AUTO_CONTROL_LIMIT_TO_USE
        {
            get { return this.chkControlLimitToUse.Checked ? "Y" : "N"; }
        }

        public string AUTO_CONTROL_LIMIT_THREASHOLD
        {
            get { return this.chkControlLimitThreshold.Checked ? "Y" : "N"; }
        }

        public string AUTO_CALCULATION_WITH_SHIFT_COMPENSATION
        {
            get { return this.chkCompensation.Checked ? "Y" : "N"; }
        }

        public string AUTO_CALCULATION_WITHOUT_IQR_FILTER
        {
            get { return this.chkIQRFilter.Checked ? "Y" : "N"; }
        }

        public string AUTO_RAW_CONTROL_LIMIT
        {
            get { return this.chkRawControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_MEAN_CONTROL_LIMIT
        {
            get { return this.chkMeanControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_STD_CONTROL_LIMIT
        {
            get { return this.chkSTDControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_RANGE_CONTROL_LIMIT
        {
            get { return this.chkRangeControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_EWMA_MEAN_CONTROL_LIMIT
        {
            get { return this.chkEWMAMeanControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_EWMA_STD_CONTROL_LIMIT
        {
            get { return this.chkEWMASTDControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_EWMA_RANGE_CONTROL_LIMIT
        {
            get { return this.chkEWMARangeControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_MA_CONTROL_LIMIT
        {
            get { return this.chkMAControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_MS_CONTROL_LIMIT
        {
            get { return this.chkMSControlLimit.Checked ? "Y" : "N"; }
        }

        //SPC-731 건으로 Threshold Column추가로 인한 수정 - 2012.02.06일
        public string AUTO_THRESHOLD_FUNTION
        {
            get { return this.chkThresholdFuntionOff.Checked ? "Y" : "N"; }
        }


        //SPC-1155 BY STELLA
        public string AUTO_MR_CONTROL_LIMIT
        {
            get { return this.chkMRControlLimit.Checked ? "Y" : "N"; }
        }

        public string USE_GLOBAL_YN
        {
            get { return this.chkGlobal.Checked ? "Y" : "N"; }
        }

        #endregion
        


        ///////////////////////////////////////////////////////////////////////////////////
        //  PageLoad & Initialize.
        ///////////////////////////////////////////////////////////////////////////////////

        public void InitializeControl()
        {
            this._Initialization = new Initialization();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization.InitializePath();

            this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            this.ContentsAreaMinWidth = 660;
            this.ContentsAreaMinHeight = 725;
        }



        ///////////////////////////////////////////////////////////////////////////////////
        //  Event Handler.
        ///////////////////////////////////////////////////////////////////////////////////
        protected override void OnLoad(EventArgs e)
        {
            InitializePopup();

            base.OnLoad(e);
        }

        private int InitializeCopyItemCount(Control control)
        {
            int Cnt=0;

            foreach (Control ctl in control.Controls)
            {
                CheckBox checkBox = ctl as CheckBox;

                if (checkBox != null && checkBox.Enabled == true)
                {
                    Cnt++;
                }
            }
            return Cnt;
        }

        private void InitailizeOldCopyInfo(Control control)
        {
            foreach (Control ctl in control.Controls)
            {
                CheckBox checkBox = ctl as CheckBox;

                if (checkBox != null && _oldSpcModelInfo.COPY_MODEL.Contains(checkBox.Name))
                {
                    checkBox.Checked = true;
                }
            }
        }

        private void SetOldCopyInfo(Control control, LinkedList ll, int itemCnt, CheckBox chkAllBox)
        {
            int chkCount = 0;

            foreach (Control ctl in control.Controls)
            {
                CheckBox checkBox = ctl as CheckBox;

                if (checkBox!=null &&checkBox.Checked)
                {
                    ll.Add(checkBox.Name, "Y");
                    chkCount++;
                }
            }

            if (control.Name == this.btpnlAutoCalculation.Name || control.Name == this.gbxAutoCalcItem.Name)
            {
                chkCount = _tempCount + chkCount;
                _tempCount = chkCount;
            }

            if (itemCnt > 0 && chkCount > 0 && itemCnt == chkCount)
            {
                ll.Add(chkAllBox.Name, "Y");
            }
        }

        void bbtnOK_Click(object sender, EventArgs e)
        {
            //SPC-1297, KBLEE, START
            if (ValidateChange())
            {
                LinkedList llCheckItems = new LinkedList();
                this.SetOldCopyInfo(this.btpnlContext, llCheckItems, this._contextCount, this.chkAllContext);
                this.SetOldCopyInfo(this.btpnlRule, llCheckItems, this._ruleCount, this.chkAllRule);
                this.SetOldCopyInfo(this.btpnlOption, llCheckItems, this._optionCount, this.chkAllOption);
                this.SetOldCopyInfo(this.btpnlAutoCalculation, llCheckItems, this._autoCalcCount, this.chkAllAutoCalculation);
                this.SetOldCopyInfo(this.gbxAutoCalcItem, llCheckItems, this._autoCalcCount, this.chkAllAutoCalculation);

                this._oldSpcModelInfo = new SpcModelInfo_ForCopy(this._eqpModel, this._spcModelName, this._mainYN, this._configRawID, this._paramAlias, llCheckItems);

                _isClosedOK = true;
                _tempCount = 0;
                this.Close();
            }
            else
            {
                //////후에 MSG 표기 변경
                MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_NO_CHANGED_ITEM", null, null);

                _isClosedOK = false;
                _tempCount = 0;
            }
            //SPC-1297, KBLEE, END
        }

        public void InitializePopup()
        {
            this._contextCount = InitializeCopyItemCount(this.btpnlContext);
            this._ruleCount= InitializeCopyItemCount(this.btpnlRule);
            this._optionCount = InitializeCopyItemCount(this.btpnlOption);
            this._autoCalcCount = InitializeCopyItemCount(this.btpnlAutoCalculation);
            this._autoCalcCount = _autoCalcCount + InitializeCopyItemCount(this.gbxAutoCalcItem);

            this.chkAllAutoCalculation.Checked = true;
            this.chkAllContext.Checked = true;
            this.chkAllOption.Checked = true;
            this.chkAllRule.Checked = true;

            this.chkAllAutoCalculation.Checked = false;
            this.chkAllContext.Checked = false;
            this.chkAllOption.Checked = false;
            this.chkAllRule.Checked = false;

            if (_oldSpcModelInfo != null && this._configRawID == this._oldSpcModelInfo.CONFIG_RAW_ID)
            {
                InitailizeOldCopyInfo(this.btpnlContext);
                InitailizeOldCopyInfo(this.btpnlRule);
                InitailizeOldCopyInfo(this.btpnlOption);
                InitailizeOldCopyInfo(this.btpnlAutoCalculation);
                InitailizeOldCopyInfo(this.gbxAutoCalcItem);

                if (_oldSpcModelInfo.COPY_MODEL.Contains(this.chkAllAutoCalculation.Name))
                    this.chkAllAutoCalculation.Checked = true;

                if (_oldSpcModelInfo.COPY_MODEL.Contains(this.chkAllRule.Name))
                    this.chkAllRule.Checked = true;

                if (_oldSpcModelInfo.COPY_MODEL.Contains(this.chkAllContext.Name))
                    this.chkAllContext.Checked = true;

                if (_oldSpcModelInfo.COPY_MODEL.Contains(this.chkAllOption.Name))
                    this.chkAllOption.Checked = true;
            }
        }

        void bbtnCancel_Click(object sender, EventArgs e)
        {
            //this.InitializePopup();
            this._configRawID = this._oldSpcModelInfo == null ? "" : this._oldSpcModelInfo.CONFIG_RAW_ID;
            this.lblEQPModel.Text = this._oldSpcModelInfo == null ? "" : this._oldSpcModelInfo.EQP_MODEL;
            this.lblSPCModelName.Text = this._oldSpcModelInfo == null ? "" : this._oldSpcModelInfo.SPC_MODEL_NAME;
            this.lblParamAlias.Text = this._oldSpcModelInfo == null ? "" : this._oldSpcModelInfo.PARAM_ALIAS;
            
            if (this._oldSpcModelInfo != null && this._oldSpcModelInfo.MAIN_YN.ToUpper().Equals("Y"))
            {
                this.SetEnabledCheckBoxByMainSub(true);
            }
            else
            {
                this.SetEnabledCheckBoxByMainSub(false);
            }

            _isClosedOK = true;

            this.Close();
        }


        void chkAllRule_CheckedChanged(object sender, EventArgs e)
        {
            this.AllCheckUnCheck(this.btpnlRule, this.chkAllRule.Checked);
        }

        void chkAllAutoCalculation_CheckedChanged(object sender, EventArgs e)
        {
            this.AllCheckUnCheck(this.btpnlAutoCalculation, this.chkAllAutoCalculation.Checked);
            this.AllCheckUnCheck(this.gbxAutoCalcItem, this.chkAllAutoCalculation.Checked);
        }

        void chkAllOption_CheckedChanged(object sender, EventArgs e)
        {
            this.AllCheckUnCheck(this.btpnlOption, this.chkAllOption.Checked);
        }

        void chkAllContext_CheckedChanged(object sender, EventArgs e)
        {
            this.chkInterlock.Checked = this.chkAllContext.Checked;
            this.chkUseExternalSpecLimit.Checked = this.chkAllContext.Checked;
            this.chkAutoCalculation.Checked = this.chkAllContext.Checked;
            this.chkSampleCount.Checked = this.chkAllContext.Checked;
            this.chkManageType.Checked = this.chkAllContext.Checked;
            this.chkMode.Checked = this.chkAllContext.Checked;
            this.chkChartDesc.Checked = this.chkAllContext.Checked;

            if (this._mainYN.Trim().ToUpper().Equals("Y"))
            {
                this.chkAutoGenerateSubChart.Checked = this.chkAllContext.Checked;
                this.chkActive.Checked = this.chkAllContext.Checked;
                this.chkAutoSetting.Checked = this.chkAllContext.Checked;
                this.chkGenerateSubChartWithInterlock.Checked = this.chkAllContext.Checked;
                this.chkGenerateSubChartWithAutoCalculation.Checked = this.chkAllContext.Checked;
                this.chkUseNorm.Checked = this.chkAllContext.Checked;
            }
            else
            {
                this.chkInherittheSpecOfMain.Checked = this.chkAllContext.Checked;
            }
        }

        //SPC-1297, KBLEE, START
        private void SPCCopySpec_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isClosedOK)
            {
                e.Cancel = true;
            }
        }
        //SPC-1297, KBLEE, END




        ///////////////////////////////////////////////////////////////////////////////////
        //  User Defined Method.
        ///////////////////////////////////////////////////////////////////////////////////

        public void SetCopyModelInfo(string eqpModel, string spcModelName, string mainYN, string configRawID, string paramAlias)
        {
            this.Title = POPUP_TITLE + " [" + configRawID + "]";

            this._configRawID = configRawID;

            if(String.IsNullOrEmpty(eqpModel))
            {
                this.bLabel2.Text = "Area :";
                this.lblEQPModel.Text = _area;
            }
            else
            {
                this.lblEQPModel.Text = eqpModel;
            }

            this.lblSPCModelName.Text = spcModelName;
            this.lblParamAlias.Text = paramAlias;

            if (mainYN.ToUpper().Equals("Y"))
            {
                this.SetEnabledCheckBoxByMainSub(true);
            }
            else
            {
                this.SetEnabledCheckBoxByMainSub(false);
            }

            this._configRawID = configRawID;
            this._eqpModel = eqpModel;
            this._spcModelName = spcModelName;
            this._paramAlias = paramAlias;
            this._mainYN = mainYN;
        }

        private void SetEnabledCheckBoxByMainSub(bool isMain)
        {
            this.chkAutoGenerateSubChart.Enabled = isMain;
            this.chkActive.Enabled = isMain;
            this.chkAutoSetting.Enabled = isMain;
            this.chkGenerateSubChartWithInterlock.Enabled = isMain;
            this.chkGenerateSubChartWithAutoCalculation.Enabled = isMain;
            this.chkUseNorm.Enabled = isMain;

            this.chkInherittheSpecOfMain.Enabled = !isMain;

            if (isMain)
            {
                this.chkInherittheSpecOfMain.Checked = false;
            }
            else
            {
                this.chkAutoGenerateSubChart.Checked = false;
                this.chkActive.Checked = false;
                this.chkAutoSetting.Checked = false;
                this.chkGenerateSubChartWithInterlock.Checked = false;
                this.chkGenerateSubChartWithAutoCalculation.Checked = false;
                this.chkUseNorm.Checked = false;
            }
        }

        private void AllCheckUnCheck(Control parentControl, bool bChecked)
        {
            foreach (Control ctl in parentControl.Controls)
            {
                CheckBox checkBox = ctl as CheckBox;

                if (checkBox != null)
                {
                    checkBox.Checked = bChecked;
                }
            }
        }

        //SPC-1297, KBLEE, START
        private bool ValidateChange()
        {
            bool bResult = false;

            if (ValidateControlChange(btpnlContext) ||
                ValidateControlChange(btpnlRule) ||
                ValidateControlChange(btpnlOption) ||
                ValidateControlChange(btpnlAutoCalculation) ||
                ValidateControlChange(gbxAutoCalcItem))
            {
                bResult = true;
            }

            return bResult;
        }
        //SPC-1297, KBLEE, END
        
        //SPC-1297, KBLEE, START
        private bool ValidateControlChange(Control parentControl)
        {
            bool bResult = false;

            foreach (Control ctl in parentControl.Controls)
            {
                CheckBox checkBox = ctl as CheckBox;

                if (checkBox != null && checkBox.Checked)
                {
                    bResult = true;
                }
            }

            return bResult;
        }
        //SPC-1297, KBLEE, END

        public string AREA
        {
            set { this._area = value; }
        }
    }


    public class SpcModelInfo_ForCopy
    {
        string _configRawID = "";
        string _eqpModel = "";
        string _spcModelName = "";
        string _paramAlias = "";
        string _mainYN = "";

        LinkedList _llCopyModel = new LinkedList();

        public SpcModelInfo_ForCopy(string eqpModel, string spcModelName, string mainYN, string configRawID, string paramAlias, LinkedList llCopyItem)
        {
            this._configRawID = configRawID;
            this._eqpModel = eqpModel;
            this._spcModelName = spcModelName;
            this._paramAlias = paramAlias;
            this._mainYN = mainYN;
            this._llCopyModel = llCopyItem;
        }

        public LinkedList COPY_MODEL
        {
            get { return this._llCopyModel; }
        }

        public string CONFIG_RAW_ID
        {
            get { return this._configRawID; }
        }

        public string EQP_MODEL
        {
            get { return this._eqpModel; }
        }

        public string SPC_MODEL_NAME
        {
            get { return this._spcModelName; }
        }

        public string PARAM_ALIAS
        {
            get { return this._paramAlias; }
        }

        public string MAIN_YN
        {
            get { return this._mainYN; }
        }
    }
}
