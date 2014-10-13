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
using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Page.ATT.Modeling
{
    public partial class SPCConfigurationPopup : BasePopupFrm
    {
        #region : Constructor
        public SPCConfigurationPopup()
        {
            InitializeComponent();
        }
        private bool _isDefaultModel = false;
        public SPCConfigurationPopup(bool isDefaultModel)
        {
            this._isDefaultModel = isDefaultModel;
            InitializeComponent();
        }

        #endregion

        #region : Field

        Initialization _Initialization;

        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _ws;

        SessionData _SessionData;
        string _sPort;


        private BISTel.eSPC.Common.ConfigMode _cofingMode = BISTel.eSPC.Common.ConfigMode.CREATE_SUB;

        private string _sConfigRawID;
        private string _sMainYN;
        private string _sAreaRawID;
        private string _sLineRawID;
        private string _sEQPModel;
        private string _sGroupName; //Group 관련 기능 추가, KBLEE
        private string _sVersion = string.Empty;

        DataSet _dsSPCModelData = new DataSet();

        private bool _hasSubConfigs;
        public bool SaveButtonVisible
        {
            get { return this.bbtnOK.Visible; }
            set { this.bbtnOK.Visible = value; }
        }
        private bool _bShowNormalization = true;
        private string _sUnitOfSamples = "";
        private string _sOCAPOfSingle = "";
        #endregion



        #region : Initialization

        public void InitializePopup()
        {
            this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
            this._sOCAPOfSingle = this._Initialization.GetOCAPOfSingle(Definition.PAGE_KEY_SPC_CONFIGURATION);
            this.spcConfigurationUC.PageInfo(_SessionData.GetXml(), "", this.URL, this.PORT, "", "", "", "");
            this.InitializeLayout();
            this.InitializeConfigData();
            this.ContentsAreaMinHeight = 580;
            this.ContentsAreaMinWidth = 780;
        }

        public void InitializeLayout()
        {
            switch (_cofingMode)
            {
                case BISTel.eSPC.Common.ConfigMode.DEFAULT:
                    this.Title = "Default Setting";
                    break;
                case BISTel.eSPC.Common.ConfigMode.CREATE_MAIN:
                    this.Title = "Create SPC Model";
                    break;

                case BISTel.eSPC.Common.ConfigMode.CREATE_SUB:
                    this.Title = "Add Sub Configuration";
                    break;

                case BISTel.eSPC.Common.ConfigMode.MODIFY:
                    this.Title = "Modify Configuration";
                    break;
                
                case BISTel.eSPC.Common.ConfigMode.VIEW:
                    this.Title = "View Configuration";
                    this.bbtnOK.Visible = false;
                    break;

                case BISTel.eSPC.Common.ConfigMode.SAVE_AS:
                    this.Title = "Save As SPC Main Model";
                    break;

                case BISTel.eSPC.Common.ConfigMode.ROLLBACK:
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
            this.spcConfigurationUC.ShowParameterAlias = ShowParameterAlias;
            this.spcConfigurationUC.bShowNormalization = _bShowNormalization;
            this.spcConfigurationUC.sUnitOfSamples = _sUnitOfSamples;

            this.spcConfigurationUC.sOCAPOfSingle = _sOCAPOfSingle;
            this.spcConfigurationUC.GROUP_NAME = _sGroupName; //SPC-1292, KBLEE

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

        public BISTel.eSPC.Common.ConfigMode CONFIG_MODE
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

        public bool ShowParameterAlias { get; set; }

        //SPC-1292, KBLEE
        public string GROUP_NAME
        {
            get { return _sGroupName; }
            set { _sGroupName = value; }
        }
    }
}
