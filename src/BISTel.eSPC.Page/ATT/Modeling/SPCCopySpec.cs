using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.eSPC.Common.ATT;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.ATT.Modeling
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

        public string CONFIG_RAWID
        {
            get { return this._configRawID; }
        }

        #region CONTEXT Info.
        
        public string CONTEXT_CHART_DESCRIPTION
        {
            get { return this.chkChartDesc.Checked ? "Y" : "N"; }
        }

        public string CONTEXT_INTERLOCK
        {
            get { return this.chkInterlock.Checked ? "Y" : "N"; }
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
        public string RULE_PN_SPEC_LIMIT
        {
            get { return this.chkPNSpecLimit.Checked ? "Y" : "N"; }
        }

        public string RULE_C_SPEC_LIMIT
        {
            get { return this.chkCSpecLimit.Checked ? "Y" : "N"; }
        }

        public string RULE_PN_CONTROL
        {
            get { return this.chkPNControl.Checked ? "Y" : "N"; }
        }

        public string RULE_C_CONTROL
        {
            get { return this.chkCControl.Checked ? "Y" : "N"; }
        }

        public string RULE_P_CONTROL
        {
            get { return this.chkPControl.Checked ? "Y" : "N"; }
        }

        public string RULE_U_CONTROL
        {
            get { return this.chkUControl.Checked ? "Y" : "N"; }
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

        public string AUTO_PN_CONTROL_LIMIT
        {
            get { return this.chkPNControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_P_CONTROL_LIMIT
        {
            get { return this.chkPControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_C_CONTROL_LIMIT
        {
            get { return this.chkCControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_U_CONTROL_LIMIT
        {
            get { return this.chkUControlLimit.Checked ? "Y" : "N"; }
        }

        public string AUTO_THRESHOLD_FUNTION
        {
            get { return this.chkThresholdFuntionOff.Checked ? "Y" : "N"; }
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
            this.chkIQRFilter.Visible = false;
            this.chkCompensation.Visible = false;
            

            this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();


            this.ContentsAreaMinWidth = 660;
            this.ContentsAreaMinHeight = 725;
        }



        ///////////////////////////////////////////////////////////////////////////////////
        //  Event Handler.
        ///////////////////////////////////////////////////////////////////////////////////
        protected override void OnLoad(EventArgs e)
        {
            this.chkAllAutoCalculation.Checked = true;
            this.chkAllContext.Checked = true;
            this.chkAllOption.Checked = true;
            this.chkAllRule.Checked = true;

            this.chkAllAutoCalculation.Checked = false;
            this.chkAllContext.Checked = false;
            this.chkAllOption.Checked = false;
            this.chkAllRule.Checked = false;

            base.OnLoad(e);
        }

        void bbtnOK_Click(object sender, EventArgs e)
        {
            this._oldSpcModelInfo = new SpcModelInfo_ForCopy(this._eqpModel, this._spcModelName, this._mainYN, this._configRawID, this._paramAlias);
            
            this.Close();
        }

        void bbtnCancel_Click(object sender, EventArgs e)
        {
            this._configRawID = this._oldSpcModelInfo == null ? "" : this._oldSpcModelInfo.CONFIR_RAW_ID;
            this.lblEQPModel.Text = this._oldSpcModelInfo == null ? "" : this._oldSpcModelInfo.EQP_MODEL;
            this.lblSPCModelName.Text = this._oldSpcModelInfo == null ? "" : this._oldSpcModelInfo.SPC_MODEL_NAME;
            this.lblAttributesName.Text = this._oldSpcModelInfo == null ? "" : this._oldSpcModelInfo.PARAM_ALIAS;

            if (this._oldSpcModelInfo != null && this._oldSpcModelInfo.MAIN_YN.ToUpper().Equals("Y"))
            {
                this.SetEnabledCheckBoxByMainSub(true);
            }
            else
            {
                this.SetEnabledCheckBoxByMainSub(false);
            }

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
            this.chkAutoCalculation.Checked = this.chkAllContext.Checked;
            this.chkSampleCount.Checked = this.chkAllContext.Checked;
            this.chkMode.Checked = this.chkAllContext.Checked;
            this.chkChartDesc.Checked = this.chkAllContext.Checked;

            if (this._mainYN.Trim().ToUpper().Equals("Y"))
            {
                this.chkAutoGenerateSubChart.Checked = this.chkAllContext.Checked;
                this.chkActive.Checked = this.chkAllContext.Checked;
                this.chkAutoSetting.Checked = this.chkAllContext.Checked;
                this.chkGenerateSubChartWithInterlock.Checked = this.chkAllContext.Checked;
                this.chkGenerateSubChartWithAutoCalculation.Checked = this.chkAllContext.Checked;
            }
            else
            {
                this.chkInherittheSpecOfMain.Checked = this.chkAllContext.Checked;
            }
        }




        ///////////////////////////////////////////////////////////////////////////////////
        //  User Defined Method.
        ///////////////////////////////////////////////////////////////////////////////////

        public void SetCopyModelInfo(string eqpModel, string spcModelName, string mainYN, string configRawID, string paramAlias)
        {
            this.Title = POPUP_TITLE + " [" + configRawID + "]";

            this._configRawID = configRawID;

            this.lblEQPModel.Text = eqpModel;
            this.lblSPCModelName.Text = spcModelName;
            this.lblAttributesName.Text = paramAlias;

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
    }


    public class SpcModelInfo_ForCopy
    {
        string _configRawID = "";
        string _eqpModel = "";
        string _spcModelName = "";
        string _paramAlias = "";
        string _mainYN = "";


        public SpcModelInfo_ForCopy(string eqpModel, string spcModelName, string mainYN, string configRawID, string paramAlias)
        {
            this._configRawID = configRawID;
            this._eqpModel = eqpModel;
            this._spcModelName = spcModelName;
            this._paramAlias = paramAlias;
            this._mainYN = mainYN;
        }


        public string CONFIR_RAW_ID
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
