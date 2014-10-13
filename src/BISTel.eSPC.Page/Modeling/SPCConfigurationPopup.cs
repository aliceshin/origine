using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Modeling
{
    public partial class SPCConfigurationPopup : BasePopupFrm
    {
        #region : Constructor
        public SPCConfigurationPopup()
        {
            InitializeComponent();
        }

        // [SPC-658]
        //====================================
        private bool _isDefaultModel = false;
        public SPCConfigurationPopup(bool isDefaultModel)
        {
            this._isDefaultModel = isDefaultModel;
            InitializeComponent();
        }
        //====================================

        #endregion

        #region : Field

        Initialization _Initialization;
        SortCondition _sortPopup;

        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _ws;

        SessionData _SessionData;
        string _sPort;


        private ConfigMode _cofingMode = ConfigMode.CREATE_SUB;

        private string _sConfigRawID;
        private string _sMainYN;
        private string _sAreaRawID;
        private string _sLineRawID;
        private string _sEQPModel;
        private string _sGroupName;
        private string _sVersion = string.Empty;

        private string _ModelingType;
        private bool _IsTrace;
        private string _ParamAliasT;

        DataSet _dsSPCModelData = new DataSet();
        DataTable _dtOriginalData = new DataTable();

        private bool _hasSubConfigs;
        public bool SaveButtonVisible
        {
            get { return this.bbtnOK.Visible; }
            set { this.bbtnOK.Visible = value; }
        }
        // JIRA SPC-592 [GSMC] Disable "Use Normalization Value" - 2011.08.29 by ANDREW KO
        private bool _bShowNormalization = true;
        // JIRA No.SPC-600 Change unit of Minimum Samples to flexable unit (like Lot, Wafer...) - 2011.09.10 by ANDREW KO
        // No.5 on GF Communication Sheet 20110612 V1.21 - 2011.09.10 by ANDREW KO
        private string _sUnitOfSamples = "";

        //SPC-736 OCAP action(option) - 2012.02.10 by louis you
        private string _sOCAPOfSingle = "";
        #endregion



        #region : Initialization

        public void InitializePopup()
        {
            this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            // JIRA SPC-592 [GSMC] Disable "Use Normalization Value" - 2011.08.29 by ANDREW KO
            this._bShowNormalization = this._Initialization.GetContextShowNormalization(Definition.PAGE_KEY_SPC_CONFIGURATION);
            // JIRA No.SPC-600 Change unit of Minimum Samples to flexable unit (like Lot, Wafer...) - 2011.09.10 by ANDREW KO
            // No.5 on GF Communication Sheet 20110612 V1.21 - 2011.09.10 by ANDREW KO
            this._sUnitOfSamples = this._Initialization.GetUnitOfSamples(Definition.PAGE_KEY_SPC_CONFIGURATION);

            //SPC-736 OCAP action(option) - 2012.02.10 by louis you
            this._sOCAPOfSingle = this._Initialization.GetOCAPOfSingle(Definition.PAGE_KEY_SPC_CONFIGURATION);

            this.spcConfigurationUC.MODELINGTYPE = this._ModelingType;
            this.spcConfigurationUC.PageInfo(_SessionData.GetXml(), "", this.URL, this.PORT, "", "", "", "");

            //spc-1199
            this.spcConfigurationUC.ORIGINAL_DATA = this._dtOriginalData;

            this.InitializeLayout();

            this.InitializeConfigData();
            this.ContentsAreaMinHeight = 580;
            this.ContentsAreaMinWidth = 780;
        }

        public void InitializeLayout()
        {
            //this.Title = this._mlthandler.GetVariable(Definition.POPUP_TITLE_KEY_ADD_SPC_RULE);

            switch (_cofingMode)
            {
                case ConfigMode.DEFAULT:
                    this.Title = "Default Setting";
                    break;
                case ConfigMode.CREATE_MAIN:
                case ConfigMode.CREATE_MAIN_FROM:
                    this.Title = "Create SPC Model";
                    break;

                case ConfigMode.CREATE_SUB:
                    this.Title = "Add Sub Configuration";
                    break;

                case ConfigMode.MODIFY:
                    this.Title = "Modify Configuration";
                    break;
                
                case ConfigMode.VIEW:
                    this.Title = "View Configuration";
                    this.bbtnOK.Visible = false;
                    break;

                case ConfigMode.SAVE_AS:
                    this.Title = "Save As SPC Main Model";
                    break;

                case ConfigMode.ROLLBACK:
                    this.Title = "View Model";
                    this.bbtnOK.Key = "SPC_BUTTON_RESUSE";
                    break;
            }
        }

        #endregion

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            if (spcConfigurationUC.SaveConfigData())
            {
                _dsSPCModelData = spcConfigurationUC.SPCMODELDATA_DATASET;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                spcConfigurationUC.SetCheckBoxColumn();
            }
        }

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


        private void InitializeConfigData()
        {
            this.spcConfigurationUC.AREA_RAWID = _sAreaRawID;
            this.spcConfigurationUC.LINE_RAWID = _sLineRawID;
            this.spcConfigurationUC.EQP_MODEL = _sEQPModel;
            this.spcConfigurationUC.CONFIG_RAWID = _sConfigRawID;
            this.spcConfigurationUC.MAIN_YN = _sMainYN;
            this.spcConfigurationUC.HAS_SUBCONFIGS = _hasSubConfigs;
            this.spcConfigurationUC.VERSION = _sVersion;
            this.spcConfigurationUC.MODELINGTYPE = _ModelingType;
            this.spcConfigurationUC.ShowParameterAlias = ShowParameterAlias;
            // JIRA SPC-592 [GSMC] Disable "Use Normalization Value" - 2011.08.29 by ANDREW KO
            this.spcConfigurationUC.bShowNormalization = _bShowNormalization;
            // JIRA No.SPC-600 Change unit of Minimum Samples to flexable unit (like Lot, Wafer...) - 2011.09.10 by ANDREW KO
            // No.5 on GF Communication Sheet 20110612 V1.21 - 2011.09.10 by ANDREW KO
            this.spcConfigurationUC.sUnitOfSamples = _sUnitOfSamples;

            //SPC-736 OCAP action(option) - 2012.02.10 by louis you
            this.spcConfigurationUC.sOCAPOfSingle = _sOCAPOfSingle;
            this.spcConfigurationUC.GROUP_NAME = _sGroupName;

            if (_cofingMode == ConfigMode.CREATE_MAIN_FROM)
            {
                this.spcConfigurationUC.isTraceSum = this._IsTrace;
                this.spcConfigurationUC.sParamAliasT = this._ParamAliasT;
            }

            this.spcConfigurationUC.InitializeConfigData(_cofingMode);
        }


        public SessionData SESSIONDATA
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        public string PORT
        {
            get { return _sPort; }
            set { _sPort = value; }
        }

        public string CONFIG_RAWID
        {
            get { return _sConfigRawID; }
            set { _sConfigRawID = value; }
        }

        public string MAIN_YN
        {
            get { return _sMainYN; }
            set { _sMainYN = value; }
        }

        public bool HAS_SUBCONFIGS
        {
            set { this._hasSubConfigs = value; }
        }

        public DataSet SPCMODELDATA_DATASET
        {
            get { return _dsSPCModelData; }
            set { _dsSPCModelData = value; }
        }

        public ConfigMode CONFIG_MODE
        {
            get { return _cofingMode; }
            set { _cofingMode = value; }
        }

        public string AREA_RAWID
        {
            get { return _sAreaRawID; }
            set { _sAreaRawID = value; }
        }

        public string LINE_RAWID
        {
            get { return _sLineRawID; }
            set { _sLineRawID = value; }
        }

        public string EQP_MODEL
        {
            get { return _sEQPModel; }
            set { _sEQPModel = value; }
        }

        public string VERSION
        {
            get { return _sVersion; }
            set { _sVersion = value; }
        }

        public string MODELINGTYPE
        {
            get { return _ModelingType; }
            set { _ModelingType = value;}
        }

        public bool ISTRACE
        {
            get { return _IsTrace; }
            set { _IsTrace = value; }
        }

        public string PARAMALIAST
        {
            get { return _ParamAliasT; }
            set { _ParamAliasT = value; }
        }

        public bool ShowParameterAlias { get; set; }

        //spc-1199 by stella
        public DataTable ORIGINAL_DATA
        {
            get { return _dtOriginalData; }
            set { _dtOriginalData = value; }
        }

        //SPC-880 by stella
        public string GROUP_NAME
        {
            get { return _sGroupName; }
            set { _sGroupName = value; }
        }
    }
}
