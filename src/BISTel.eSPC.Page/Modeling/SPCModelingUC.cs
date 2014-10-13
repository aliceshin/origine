using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.CodeDom;
using System.Globalization;
using System.Xml;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;

//PrograssBar by louis
using BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar;
using BISTel.PeakPerformance.Client.DataAsyncHandler;
//

using BISTel.PeakPerformance.Client.DataHandler;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;

using BISTel.eSPC.Common;




namespace BISTel.eSPC.Page.Modeling
{
    public partial class SPCModelingUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition = new LinkedList();


        BISTel.eSPC.Common.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.CommonUtility _ComUtil;

        private SortedList _slParamColumnIndex = new SortedList();
        private SortedList _slSpcModelingIndex = new SortedList();

        private int _ColIdx_SELECT = 0;
        private int _colIdx_MAIN_YN = -1;
        private int _ColIdx_Interlock = 0;
        private int _ColIdx_Activation = 0;
        private int _ColIdx_MainChart = 0;
        private int _ColIdx_Parameter = 0;
        private int _ColIdx_EQPID = 0;
        private int _ColIdx_ModuleID = 0;
        private int _ColIdx_OperationID = 0;
        private int _ColIdx_ProductID = 0;
        private int _ColIdx_PARAM_TYPE_CD = 0;
        private int _ColIdx_RULE_LIST = 0;
        private int _ColIdx_OCAP_ACTION= 0;

        private int _ColIdx_USL = 0;
        private int _ColIdx_LSL = 0;
        private int _ColIdx_MEAN_UCL = 0;
        private int _ColIdx_MEAN_LCL = 0;
        private int _ColIdx_RAW_UCL = 0;
        private int _ColIdx_RAW_LCL = 0;
        private int _ColIdx_STD_UCL = 0;
        private int _ColIdx_STD_LCL = 0;
        private int _ColIdx_RANGE_UCL = 0;
        private int _ColIdx_RANGE_LCL = 0;
        private int _ColIdx_EWMA_M_UCL = 0;
        private int _ColIdx_EWMA_M_LCL = 0;
        private int _ColIdx_EWMA_R_UCL = 0;
        private int _ColIdx_EWMA_R_LCL = 0;
        private int _ColIdx_EWMA_S_UCL = 0;
        private int _ColIdx_EWMA_S_LCL = 0;
        private int _ColIdx_MA_UCL = 0;
        private int _ColIdx_MA_LCL = 0;
        private int _ColIdx_MS_UCL = 0;
        private int _ColIdx_MS_LCL = 0;
        private int _ColIdx_MR_UCL = 0;
        private int _ColIdx_MR_LCL = 0;
        private int _ColIdx_UPPER_TECHNICAL_LIMIT = 0;
        private int _ColIdx_LOWER_TECHNICAL_LIMIT = 0;
        private int _ColIdx_TARGET = 0;
        private int _ColIdx_CENTER_LINE = 0;
        private int _ColIdx_RAW_CENTER_LINE = 0;
        private int _ColIdx_STD_CENTER_LINE = 0;
        private int _ColIdx_RANGE_CENTER_LINE = 0;
        private int _ColIdx_EWMA_M_CENTER_LINE = 0;
        private int _ColIdx_EWMA_R_CENTER_LINE = 0;
        private int _ColIdx_EWMA_S_CENTER_LINE = 0;
        private int _ColIdx_MA_CENTER_LINE = 0;
        private int _ColIdx_MS_CENTER_LINE = 0;
        private int _ColIdx_MR_CENTER_LINE = 0;
        private int _ColIdx_STD = 0;
        private int _ColIdx_ZONE_A_UCL = 0;
        private int _ColIdx_ZONE_A_LCL = 0;
        private int _ColIdx_ZONE_B_UCL = 0;
        private int _ColIdx_ZONE_B_LCL = 0;
        private int _ColIdx_ZONE_C_UCL = 0;
        private int _ColIdx_ZONE_C_LCL = 0;
        private int _ColIdx_CREATE_BY = 0;
        private int _ColIdx_CREATE_DTTS = 0;
        private int _ColIdx_LAST_UPDATE_BY = 0;
        private int _ColIdx_LAST_UPDATE_DTTS = 0;

        //SPC-676 ChartDesc
        private int _Colldx_CHART_DESCRIPTION = 0;

        private string _sLineRawid;
        private string _sLine;
        private string _sAreaRawid;
        private string _sArea;
        private string _sEQPModel;
        private string _sSPCModelRawid;
        private string _sSPCModelName;
        private string _sSPCGroupName;

        private DataSet _dsSPCModeData = new DataSet();
        private DataTable _dtTableOriginal = new DataTable();

        private CellRange _bsprDataSelectedRange = null;

        private List<string> _valueForActivation = null;

        private bool _bsprDataValidate = true;

        private bool copyClicked = false;

        private bool _importcompleted = true;

        ArrayList _arrColConfig;
        ArrayList _arrColContext; 
        ArrayList _arrColOption;
        ArrayList _arrColAutoCalc;
        ArrayList _arrColRule;
        ArrayList _arrColLimit;

        private bool _bUseComma = false;

        enum iColIdx
        {
            SELECT,
            CHART_ID,
            PARAM_ALIAS,
            MAIN_YN
        }


        #endregion

        string _copyModel = "Copy Model Info";
        string _pasteModel = "Paste Model Info";
        private const string RULE_LIST = "RULE_LIST";
        private const string OCAP_ACTION = "OCAP";
        SPCCopySpec _popUp = null;


        //SPC-854 Louisyou  ContextMenu추가
        MenuItem fiterHide;
        MenuItem freeze;
        MenuItem unfreeze;
        MenuItem filter;
       
        private bool filterClicked = false;
        private bool freezeClicked = false;
        //////////////////////
        
        //private SpreadFilter _SpreadFilter = new SpreadFilter();
        //private FarPoint.Win.Spread.FpSpread _SpreadFilter = new FpSpread();

        SpreadFilter _sfilter;

        MultiLanguageHandler _lang;

        bool isMETModelingPage = false;

        #region ::: Properties


        #endregion


        #region ::: Constructor

        public SPCModelingUC()
        {
            InitializeComponent();
        }

        #endregion

        #region ::: Override Method

        public override void PageInit()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;

            this.KeyOfPage = this.GetType().FullName; // 필수 입력 (Key값을 받은 후 DC을 Active해주기 때문이다.

            if (this.KeyOfPage != null )
            {
                if (this.KeyOfPage == "BISTel.eSPC.Page.Modeling.SPCModelingUC")
                {
                    isMETModelingPage = false;
                }
                else if (this.KeyOfPage == "BISTel.eSPC.Page.Modeling.MET.Modeling.SPCModelingUC")
                {
                    isMETModelingPage = true;
                }
            }

            this.InitializePage();
        }

        //public override BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ADynamicCondition CreateCustomCondition()
        //{
        //    return new BISTel.eSPC.Condition.Modeling.SPCModelCondition();			
        //}

        LinkedList llstDynamicCondition = new LinkedList();
        public override void PageSearch(LinkedList llCondition)
        {
            //초기화
            this.InitializePageData();

            llstDynamicCondition = llCondition;

            DataTable dtSite = (DataTable)llstDynamicCondition[Definition.CONDITION_SEARCH_KEY_SITE];
            string site = string.Empty;
            if (dtSite != null)
                site = dtSite.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();

            DataTable dtFab = (DataTable)llstDynamicCondition[Definition.CONDITION_SEARCH_KEY_FAB];
            string fab = "";
            if (dtFab != null)
                fab = dtFab.Rows[0][Definition.CONDITION_SEARCH_KEY_DISPLAYDATA].ToString();


            DataTable dt = ((DataTable)llstDynamicCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL]);
            while (dt != null && dt.Rows.Count > 1)
            {
                dt.Rows.RemoveAt(0);
            }

            if (!llCondition.Contains(Definition.CONDITION_SEARCH_KEY_AREA))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_AREA",null, null);
                return;
            }

            DataTable dtArea = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_AREA];

            this._sAreaRawid = DCUtil.GetValueData(dtArea);
            this._sArea = dtArea.Rows[0][DCUtil.DISPLAY_FIELD].ToString();

            if (llCondition.Contains(Definition.CONDITION_SEARCH_KEY_LINE))
            {
                dtArea = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_LINE];
                this._sLine = dtArea.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
                this._sLineRawid = DCUtil.GetValueData(dtArea);
            }

            if (!base.ApplyAuthory(this.bbtnList, site, fab, this._sLine, this._sArea.Trim('\'')))
            {
                MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("GENERAL_NOT_ENOUGHT_SUFFICIENT"), null, null, true);
                this.InitializePage();
                return;
            }
            this.InitializeLayout();

            if (llCondition.Contains(Definition.CONDITION_KEY_GROUP_NAME))
            {
                DataTable dtGroup = (DataTable)llCondition[Definition.CONDITION_KEY_GROUP_NAME];

                this._sSPCGroupName = dtGroup.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
            }

            if (llCondition.Contains(Definition.CONDITION_SEARCH_KEY_EQPMODEL))
            {
                DataTable dtEQPModel = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_EQPMODEL];

                if (dtEQPModel.Rows[0]["CHECKED"].Equals("T"))
                {
                    if (!llCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                    {
                        this._sEQPModel = dtEQPModel.Rows[0][DCUtil.VALUE_FIELD].ToString();

                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_OR_CREATE_MODEL", null, null);
                        return;
                    }
                    else
                    {
                        DataTable dtSPCModel = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                        this._sSPCModelRawid = dtSPCModel.Rows[0][DCUtil.VALUE_FIELD].ToString();
                        this._sSPCModelName = dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
                        this._sEQPModel = dtEQPModel.Rows[0][DCUtil.VALUE_FIELD].ToString();
                    }
                }
                //else
                //{
                //    if (!llCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                //    {
                //        MSGHandler.DisplayMessage(MSGType.Information, "Please Select SPC Model.");
                //        return;
                //    }

                //    DataTable dtSPCModel = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                //    this._sSPCModelRawid = dtSPCModel.Rows[0][DCUtil.VALUE_FIELD].ToString();
                //    this._sSPCModelName = dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
                //    this._sEQPModel = dtEQPModel.Rows[0][DCUtil.VALUE_FIELD].ToString();
                //}
            }
            else
            {
                this._sEQPModel = "";

                if (!llCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_OR_CREATE_MODEL", null, null);
                    return;
                }

                DataTable dtSPCModel = (DataTable)llCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                this._sSPCModelRawid = dtSPCModel.Rows[0][DCUtil.VALUE_FIELD].ToString();
                this._sSPCModelName = dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD].ToString();
            }

            this.ConfigListDataBinding();
        }

        #endregion

        #region ::: PageLoad & Initialize

        public void InitializePage()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._lang = MultiLanguageHandler.getInstance();

            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._SpreadUtil = new BSpreadUtility();
            this._ComUtil = new CommonUtility();

            this.InitializeCode();
            this.InitializeDataButton();
            this.InitializeBSpread();
            this.InitializeLayout();
            this.ProcessLink();
        }

        private void ProcessLink()
        {
            try
            {
                if (!GlobalDefinition.PassData.ContainsKey("queryxml"))
                {
                    return;
                }

                XmlDocument queryXml = new XmlDocument();
                string tempXml = GlobalDefinition.PassData["queryxml"];
                GlobalDefinition.PassData.Clear();
                queryXml.LoadXml(tempXml);

                XmlElement linkData = queryXml.DocumentElement;
                XmlNode requestInfo = linkData.GetElementsByTagName("request")[0];

                base.MsgClose();

                XmlNode xNodeEQPID = ((XmlElement)requestInfo).GetElementsByTagName("EQP_ID")[0];
                XmlNode xNodeSumParamAlias = ((XmlElement)requestInfo).GetElementsByTagName("SUM_PARAM_ALIAS")[0];
                XmlNode xNodeEvSumParamAlias = ((XmlElement)requestInfo).GetElementsByTagName("EVENT_PARAM_ALIAS")[0];

                string strNodeEQPID = xNodeEQPID.InnerText;
                string strNodeSumParamAlias = "";
                if (xNodeSumParamAlias != null)
                {
                    strNodeSumParamAlias = xNodeSumParamAlias.InnerText;
                }
                string strNodeEvSumParamAlias = "";
                if (xNodeEvSumParamAlias != null)
                {
                    strNodeEvSumParamAlias = xNodeEvSumParamAlias.InnerText;
                }
                string strParamAliasT = "";
                bool istrace = true;

                if (!string.IsNullOrEmpty(strNodeSumParamAlias))
                {
                    strParamAliasT = strNodeSumParamAlias;
                }
                else
                {
                    strParamAliasT = strNodeEvSumParamAlias;
                    istrace = false;
                }

                //get area, eqpmodel, spc_model_level
                //SPC MODEL LEVEL을 가져옴
                LinkedList llstCondtion = new LinkedList();
                llstCondtion.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_MODEL_LEVEL");
                llstCondtion.Add(Definition.CONDITION_KEY_USE_YN, "Y");

                DataSet dsModelLevel = this._wsSPC.GetCodeData(llstCondtion.GetSerialData());

                string sSPCModelLevel = "";

                if (DSUtil.CheckRowCount(dsModelLevel))
                {
                    sSPCModelLevel = dsModelLevel.Tables[0].Rows[0][COLUMN.CODE].ToString();
                }

                llstCondtion.Clear();

                llstCondtion.Add(Definition.CONDITION_KEY_EQP_ID, strNodeEQPID);
                DataSet dsBaseInfo = this._wsSPC.GetBaseInfoWithEQPID(llstCondtion.GetSerialData());

                llstCondtion.Clear();

                string sAreaRawIDT = string.Empty;
                string sAreaT = string.Empty;
                string sEQPModelT = string.Empty;

                if (DSUtil.CheckRowCount(dsBaseInfo))
                {
                    sAreaRawIDT = dsBaseInfo.Tables[0].Rows[0][COLUMN.AREA_RAWID].ToString();
                    sAreaT = dsBaseInfo.Tables[0].Rows[0][COLUMN.AREA].ToString();
                    sEQPModelT = dsBaseInfo.Tables[0].Rows[0][COLUMN.EQP_MODEL].ToString();

                    this._sLineRawid = dsBaseInfo.Tables[0].Rows[0][COLUMN.LOCATION_RAWID].ToString();
                    this._sAreaRawid = sAreaRawIDT;
                    this._sEQPModel = "";
                }

                llstCondtion.Add(Definition.CONDITION_KEY_AREA_RAWID, sAreaRawIDT);

                if (sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
                {
                    llstCondtion.Add(Definition.CONDITION_KEY_EQP_MODEL, sEQPModelT);
                    this._sEQPModel = sEQPModelT;
                }

                llstCondtion.Add(Definition.CONDITION_KEY_PARAM_ALIAS, strParamAliasT);

                DataSet dsSPCModelInfo = this._wsSPC.GetSPCModelListbyBaseInfo(llstCondtion.GetSerialData());

                if (DSUtil.CheckRowCount(dsSPCModelInfo))
                {

                    if (dsSPCModelInfo.Tables[0].Rows.Count == 1) //one - search
                    {
                        this._sSPCModelRawid = dsSPCModelInfo.Tables[0].Rows[0]["RAWID"].ToString();
                        this._sSPCModelName = dsSPCModelInfo.Tables[0].Rows[0]["SPC_MODEL_NAME"].ToString();
                        this.ConfigListDataBinding();
                    }
                    else //multi - show select model popup
                    {
                        BISTel.eSPC.Page.Common.SelectSPCModelListPopup ssMls = new BISTel.eSPC.Page.Common.SelectSPCModelListPopup();

                        ssMls.isShowSubList = false;
                        ssMls.sSPCModelLevel = sSPCModelLevel;
                        ssMls.sAreaT = sAreaT;
                        ssMls.sEQPModelT = sEQPModelT;
                        ssMls.sAreaRawIDT = sAreaRawIDT;
                        ssMls.dsSPCModelList = dsSPCModelInfo;
                        ssMls.strParamAliasT = strParamAliasT;

                        ssMls.InitializeControl();

                        DialogResult dResult = ssMls.ShowDialog();

                        if (dResult == DialogResult.OK)
                        {
                            this._sSPCModelRawid = ssMls.MODEL_RAWID;
                            this._sSPCModelName = ssMls.MODEL_NAME;
                            this.ConfigListDataBinding();
                        }
                    }
                }
                else
                {
                    //show create spc model popup
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_SPC_MODEL", null, null);

                    SPCConfigurationPopup spcConifgPopup = new SPCConfigurationPopup();
                    spcConifgPopup.SESSIONDATA = this.sessionData;
                    spcConifgPopup.URL = this.URL;
                    spcConifgPopup.PORT = this.Port;
                    spcConifgPopup.CONFIG_MODE = ConfigMode.CREATE_MAIN_FROM;
                    spcConifgPopup.LINE_RAWID = this._sLineRawid;
                    spcConifgPopup.AREA_RAWID = this._sAreaRawid;
                    spcConifgPopup.EQP_MODEL = this._sEQPModel;
                    spcConifgPopup.MAIN_YN = "Y";
                    spcConifgPopup.MODELINGTYPE = "TRACE";
                    spcConifgPopup.ISTRACE = istrace;
                    spcConifgPopup.PARAMALIAST = strParamAliasT;
                    spcConifgPopup.InitializePopup();

                    DialogResult result = spcConifgPopup.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        DataTable dtModel = spcConifgPopup.SPCMODELDATA_DATASET.Tables[TABLE.MODEL_MST_SPC];
                        this._sSPCModelRawid = dtModel.Rows[0][COLUMN.RAWID].ToString();
                        this._sSPCModelName = dtModel.Rows[0][COLUMN.SPC_MODEL_NAME].ToString();

                        if (llstDynamicCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                        {
                            DataTable dtSPCModel = (DataTable)llstDynamicCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                            dtSPCModel.Rows[0][DCUtil.VALUE_FIELD] = _sSPCModelRawid;
                            dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD] = _sSPCModelName;
                            dtSPCModel.Rows[0]["CHECKED"] = "T";

                            while (dtSPCModel.Rows.Count > 1)
                            {
                                dtSPCModel.Rows.RemoveAt(1);
                            }
                        }
                        else
                        {
                            DataTable dtSPCModel = new DataTable();
                            dtSPCModel.Columns.Add(DCUtil.VALUE_FIELD);
                            dtSPCModel.Columns.Add(DCUtil.DISPLAY_FIELD);
                            dtSPCModel.Columns.Add("CHECKED");
                            dtSPCModel.Columns.Add(Definition.CONDITION_SEARCH_KEY_EQPMODEL);

                            DataRow drNewRow = dtSPCModel.NewRow();
                            drNewRow[DCUtil.VALUE_FIELD] = _sSPCModelRawid;
                            drNewRow[DCUtil.DISPLAY_FIELD] = _sSPCModelName;
                            drNewRow["CHECKED"] = "T";


                            drNewRow[Definition.CONDITION_SEARCH_KEY_EQPMODEL] = _sEQPModel;

                            dtSPCModel.Rows.Add(drNewRow);

                            llstDynamicCondition.Add(Definition.CONDITION_SEARCH_KEY_SPCMODEL, dtSPCModel);
                        }

                        this.DynaminCondition.RefreshCondition(llstDynamicCondition);

                        //초기화
                        this.ConfigListDataBinding();

                        Panel pnlDumy = new Panel();
                        pnlDumy.Size = new Size(250, 500);
                        this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", pnlDumy, false);
                    }
                }
            }
            catch
            {
            }
        }


        public void InitializeLayout()
        {
            this.bbtnCopyModel.Visible = false;
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_MODELING, Definition.CONFIG_USE_COMMA, false);

            //SPC-901
            //this.bbtnCopyModel.Click += new EventHandler(bbtnCopyModel_Click);
            this.ColumnMappingByItems();
            this.bsprData.ContextMenu.MenuItems.Add(this.bsprData.ContextMenu.MenuItems.Count, new MenuItem(_copyModel, copyModel_Click));
            if (copyClicked)
            {
                this.bsprData.ContextMenu.MenuItems.Add(this.bsprData.ContextMenu.MenuItems.Count, new MenuItem(_pasteModel, pasteModel_Click));
            }

            if (_sfilter == null)
            {
                _sfilter = new SpreadFilter(this.bsprData);
            }
            else
            {
                _sfilter.Visible = false;
            }

            this.bsprData.KeyUp += new KeyEventHandler(bsprData_KeyUp);


            if (!this.bsprData.ContextMenu.MenuItems.Contains(filter))
            {
                //SPC-854 by Louisyou ContextMenu추가
                filter = new MenuItem("Filter");
                filter.Click += new EventHandler(filter_Click);
                this.bsprData.ContextMenu.MenuItems.Add(filter);

                freeze = new MenuItem("Freeze");
                freeze.Click += new EventHandler(freeze_Click);
                this.bsprData.ContextMenu.MenuItems.Add(freeze);
                ///////////////////////
            }
            else
            {
            }

        }

        void bsprData_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.F)
                {
                    this.bsprData.SetFilterVisible(false);
                }
            }

            if (e.KeyCode == Keys.F)
            {
                if (!e.Control)
                {
                    this.bsprData.SetFilterVisible(false);
                }
            }
            
        }

        private void InitializeCode()
        {
            //LinkedList lk = new LinkedList();
            //lk.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_FAULT_LEVEL);

            //DataSet _dsFaultLevelCode = this._wsSPC.GetCodeData(lk.GetSerialData());
            //this._faultlevelnames = _ComUtil.convertdatacolumnintoarray(_dsfaultlevelcode.tables[0], "name");
            //this._faultlevelcodes = _ComUtil.convertdatacolumnintoarray(_dsfaultlevelcode.tables[0], "code");
        }


        public void InitializeDataButton()
        {
            if (this.isMETModelingPage)
            {
                this._slSpcModelingIndex = this._Initialization.InitializeButtonList(this.bbtnList, ref bsprData, Definition.PAGE_KEY_SPC_MET_MODELING, "", this.sessionData);

                this.FunctionName = Definition.FUNC_KEY_SPC_MET_MODELING;
            }
            else
            {
                this._slSpcModelingIndex = this._Initialization.InitializeButtonList(this.bbtnList, ref bsprData, Definition.PAGE_KEY_SPC_MODELING, "", this.sessionData);

                this.FunctionName = Definition.FUNC_KEY_SPC_MODELING;
            }

            this.ApplyAuthory(this.bbtnList);
        }



        public void InitializeBSpread()
        {

            //this._slParamColumnIndex = new SortedList();
            //this._slParamColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprData, Definition.PAGE_KEY_SPC_MODELING, true, "");
            //this.bsprData.UseHeadColor= true;

            //this._ColIdx_SELECT = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT)];
            //this._ColIdx_MainChart = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_MAIN_CHART)];
            //this._ColIdx_Parameter = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_PARAM_ALIAS)];
            //this._ColIdx_EQPID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_EQP_ID)];
            //this._ColIdx_ModuleID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_MODULE_ID)];
            //this._ColIdx_OperationID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_OPERATION_ID)];
            //this._ColIdx_ProductID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_PRODUCT_ID)];

            //this._Initialization.SetCheckColumnHeader(this.bsprData, 0);
            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
            this.bsprData.UseSpreadEdit = false;

            //SPC-743 by Louis you
            this.bsprData.UseGeneralContextMenu = false;
            
        }

        public void InitializePageData()
        {
            this._sLineRawid = null;
            this._sAreaRawid = null;
            this._sEQPModel = null;
            this._sSPCModelRawid = string.Empty;
            this._sSPCModelName = string.Empty;

            this.btxtSPCModelName.Text = string.Empty;
            this.btxtParamAlias.Text = string.Empty;
            this.bsprData.ActiveSheet.RowCount = 0;
            //this.bsprData.ClearHead();
            //this.bsprData.AddHeadComplete();
            this._dtTableOriginal = new DataTable();
            this._ColIdx_Interlock = 0;
            this._ColIdx_Activation = 0;
        }




        #endregion

        public void bbtnList_ButtonClick(string name)
        {
            if (name.Equals(Definition.ButtonKey.SEARCH_BY_AREA))
            {
                SearchModelByArea popUpSearchByArea = new SearchModelByArea();
                popUpSearchByArea.SESSIONDATA = this.sessionData;
                popUpSearchByArea.InitializeControl();

                popUpSearchByArea.ShowDialog();

                return;
            }

            else if (name.ToUpper().Equals("SPC_GROUPING"))
            {
                SPCModelGroupPopup popup = new SPCModelGroupPopup();
                popup.InitializePopup();
                popup.SESSIONDATA = this.sessionData;

                if (this.isMETModelingPage)
                    popup.IS_MET = true;
                else
                    popup.IS_MET = false;

                popup.ShowDialog();

                if (popup.DialogResult == DialogResult.Cancel)
                {
                    LinkedList ll = new LinkedList();
                    ll = this.DynaminCondition.GetParameter();
                    this.RefreshConditions(ll);
                }

                return;
            }
            
            if (_sLineRawid == null || _sAreaRawid == null || this._sEQPModel == null)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SEARCH", null, null);
                return;
            }

            //ArrayList alCheckRowIndex = this.bsprData.GetCheckedList(0);
            SortedList alSelectedRows = this.bsprData.GetSelectedRows();

            if (name.ToUpper().Equals("SELECT"))
            {
                if (alSelectedRows.Count > 0)
                {

                    for (int i = 0; i < alSelectedRows.Count; i++)
                    {
                        this.bsprData.ActiveSheet.SetText((int)alSelectedRows.GetByIndex(i), this._ColIdx_SELECT, "True");
                    }
                }
                //SPC-642 Louis
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_REGION_SPREAD", null, null);
                }
            }
            else if (name.ToUpper().Equals("UNSELECT"))
            {
                if (alSelectedRows.Count > 0)
                {
                    for (int i = 0; i < alSelectedRows.Count; i++)
                    {
                        this.bsprData.ActiveSheet.SetText((int)alSelectedRows.GetByIndex(i), this._ColIdx_SELECT, "False");
                    }
                }
                //SPC-642 Louis
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_REGION_SPREAD", null, null);
                }
            }
            else if (name.ToUpper().Equals("CHECK_INTERLOCK"))
            {
                if (alSelectedRows.Count > 0 && this._ColIdx_Interlock != 0)
                {
                    for (int i = 0; i < alSelectedRows.Count; i++)
                    {
                        this.bsprData.ActiveSheet.SetText((int)alSelectedRows.GetByIndex(i), this._ColIdx_Interlock, "True");
                    }
                }
                //SPC-642 Louis
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_REGION_SPREAD", null, null);
                }
            }
            else if (name.ToUpper().Equals("UNCHECK_INTERLOCK"))
            {
                if (alSelectedRows.Count > 0 && this._ColIdx_Interlock != 0)
                {
                    for (int i = 0; i < alSelectedRows.Count; i++)
                    {
                        this.bsprData.ActiveSheet.SetText((int)alSelectedRows.GetByIndex(i), this._ColIdx_Interlock, "False");
                    }
                }
                //SPC-642 Louis
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_REGION_SPREAD", null, null);
                }
            }
               
            else if (name.ToUpper().Equals("ACTIVE"))
            {
                if (alSelectedRows.Count > 0 && this._ColIdx_Activation != 0)
                {
                    for (int i = 0; i < alSelectedRows.Count; i++)
                    {
                        this.bsprData.ActiveSheet.SetText((int)alSelectedRows.GetByIndex(i), this._ColIdx_Activation, "True");
                    }
                }
                //SPC-642 Louis
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_REGION_SPREAD", null, null);
                }
            }
            else if (name.ToUpper().Equals("DEACTIVATE"))
            {
                if (alSelectedRows.Count > 0 && this._ColIdx_Activation != 0)
                {
                    for (int i = 0; i < alSelectedRows.Count; i++)
                    {
                        this.bsprData.ActiveSheet.SetText((int)alSelectedRows.GetByIndex(i), this._ColIdx_Activation, "False");
                    }
                }
                //SPC-642 Louis
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_REGION_SPREAD", null, null);
                }
            }
            else if (name.ToUpper().Equals(Definition.BUTTON_KEY_DEFAULTSETTING))
            {
                // [SPC-658]
                SPCConfigurationPopup spcConifgPopup = new SPCConfigurationPopup(true);
                //SPCConfigurationPopup spcConifgPopup = new SPCConfigurationPopup();
                //
                spcConifgPopup.SESSIONDATA = this.sessionData;
                spcConifgPopup.URL = this.URL;
                spcConifgPopup.PORT = this.Port;
                spcConifgPopup.CONFIG_MODE = ConfigMode.DEFAULT;
                spcConifgPopup.AREA_RAWID = this._sAreaRawid;
                spcConifgPopup.LINE_RAWID = this._sLineRawid;
                spcConifgPopup.EQP_MODEL = this._sEQPModel;
                if (isMETModelingPage)
                {
                    spcConifgPopup.MODELINGTYPE = "MET";
                }
                else
                {
                    spcConifgPopup.MODELINGTYPE = "TRACE";
                }
                spcConifgPopup.InitializePopup();
                DialogResult result = spcConifgPopup.ShowDialog();
            }
            else if (name.ToUpper().Equals(Definition.ButtonKey.CREATE_MAINMODEL))
            {
                SPCConfigurationPopup spcConifgPopup = new SPCConfigurationPopup();
                spcConifgPopup.SESSIONDATA = this.sessionData;
                spcConifgPopup.URL = this.URL;
                spcConifgPopup.PORT = this.Port;
                spcConifgPopup.CONFIG_MODE = ConfigMode.CREATE_MAIN;
                spcConifgPopup.LINE_RAWID = this._sLineRawid;
                spcConifgPopup.AREA_RAWID = this._sAreaRawid;
                spcConifgPopup.EQP_MODEL = this._sEQPModel;
                spcConifgPopup.GROUP_NAME = Definition.VARIABLE_UNASSIGNED_MODEL;
                spcConifgPopup.MAIN_YN = "Y";
                if (isMETModelingPage)
                {
                    spcConifgPopup.MODELINGTYPE = "MET";
                }
                else
                {
                    spcConifgPopup.MODELINGTYPE = "TRACE";
                }
                spcConifgPopup.InitializePopup();

                DialogResult result = spcConifgPopup.ShowDialog();

                if (result == DialogResult.OK)
                {
                    DataTable dtModel = spcConifgPopup.SPCMODELDATA_DATASET.Tables[TABLE.MODEL_MST_SPC];
                    this._sSPCModelRawid = dtModel.Rows[0][COLUMN.RAWID].ToString();
                    this._sSPCModelName = dtModel.Rows[0][COLUMN.SPC_MODEL_NAME].ToString();

                    if (llstDynamicCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                    {
                        DataTable dtSPCModel = (DataTable)llstDynamicCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];
                        
                        this._sSPCGroupName = spcConifgPopup.GROUP_NAME;
                        
                        dtSPCModel.Rows[0][DCUtil.VALUE_FIELD] = _sSPCModelRawid;
                        dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD] = _sSPCModelName;
                        dtSPCModel.Rows[0][COLUMN.GROUP_NAME] = _sSPCGroupName;
                        dtSPCModel.Rows[0]["CHECKED"] = "T";

                        while (dtSPCModel.Rows.Count > 1)
                        {
                            dtSPCModel.Rows.RemoveAt(1);
                        }
                    }
                    else
                    {
                        this._sSPCGroupName = spcConifgPopup.GROUP_NAME;

                        DataTable dtSPCModel = DCUtil.MakeDataTableForDCValue(_sSPCModelRawid, _sSPCModelName);
                        dtSPCModel.Columns.Add("CHECKED");
                        dtSPCModel.Columns.Add(COLUMN.GROUP_NAME);
                        dtSPCModel.Columns.Add(Definition.CONDITION_SEARCH_KEY_EQPMODEL);

                        dtSPCModel.Rows[0]["CHECKED"] = "T";
                        dtSPCModel.Rows[0][COLUMN.GROUP_NAME] = _sSPCGroupName;
                        dtSPCModel.Rows[0][Definition.CONDITION_SEARCH_KEY_EQPMODEL] = _sEQPModel;

                        //DataTable dtModel = DCUtil.MakeDataTableForDCValue(_sSPCGroupName, _sSPCGroupName);
                        llstDynamicCondition.Add(Definition.CONDITION_SEARCH_KEY_SPCMODEL, dtSPCModel);

                        if (llstDynamicCondition.Contains(Definition.CONDITION_KEY_GROUP_NAME))
                        {
                            DataTable dtSPCGroup = llstDynamicCondition[Definition.CONDITION_KEY_GROUP_NAME] as DataTable;
                            
                            dtSPCGroup.Rows[0][DCUtil.VALUE_FIELD] = _sSPCGroupName;
                            dtSPCGroup.Rows[0][DCUtil.DISPLAY_FIELD] = _sSPCGroupName;
                        }
                        else
                        {
                            DataTable dtSPCGroup = DCUtil.MakeDataTableForDCValue(_sSPCGroupName, _sSPCGroupName);
                            dtSPCGroup.Columns.Add("CHECKED");
                            dtSPCGroup.Columns.Add(COLUMN.GROUP_NAME);
                            dtSPCGroup.Columns.Add("SPC MODEL LIST");

                            dtSPCGroup.Rows[0][DCUtil.VALUE_FIELD] = _sSPCGroupName;
                            dtSPCGroup.Rows[0][DCUtil.DISPLAY_FIELD] = _sSPCGroupName;
                            dtSPCGroup.Rows[0]["CHECKED"] = "T";
                            dtSPCGroup.Rows[0]["SPC MODEL LIST"] = "SPC MODEL LIST";

                            llstDynamicCondition.Add(Definition.CONDITION_KEY_GROUP_NAME, dtSPCGroup);
                        }
                    }

                    this.DynaminCondition.RefreshCondition(llstDynamicCondition);

                    //초기화
                    this.ConfigListDataBinding();

                    Panel pnlDumy = new Panel();
                    pnlDumy.Size = new Size(250, 500);
                    this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", pnlDumy, false);
                }

            }
            else if (name.ToUpper().Equals(Definition.ButtonKey.ADD_SUBCONFIG))
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                DataRow[] drMainConfig = ((DataSet)bsprData.DataSet).Tables[0].Select(string.Format("{0} = 'Y'", COLUMN.MAIN_YN));

                if (drMainConfig != null && drMainConfig.Length > 0)
                {
                    SPCConfigurationPopup spcConifgPopup = new SPCConfigurationPopup();
                    spcConifgPopup.SESSIONDATA = this.sessionData;
                    spcConifgPopup.URL = this.URL;
                    spcConifgPopup.PORT = this.Port;
                    spcConifgPopup.CONFIG_MODE = ConfigMode.CREATE_SUB;
                    spcConifgPopup.LINE_RAWID = this._sLineRawid;
                    spcConifgPopup.AREA_RAWID = this._sAreaRawid;
                    spcConifgPopup.EQP_MODEL = this._sEQPModel;
                    spcConifgPopup.GROUP_NAME = this._sSPCGroupName;
                    spcConifgPopup.MAIN_YN = "N";
                    if (isMETModelingPage)
                    {
                        spcConifgPopup.MODELINGTYPE = "MET";
                    }
                    else
                    {
                        spcConifgPopup.MODELINGTYPE = "TRACE";
                    }
                    spcConifgPopup.CONFIG_RAWID = drMainConfig[0][COLUMN.CHART_ID].ToString();
                    spcConifgPopup.InitializePopup();
                    DialogResult result = spcConifgPopup.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        //MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));

                        this.ConfigListDataBinding();
                    }
                }
            }
            else if (name.ToUpper().Equals(Definition.ButtonKey.MODIFY_MODEL))
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                int iRowIndex = -1;

                ArrayList alCheckRowIndex = this.bsprData.GetCheckedList(0);

                if (alCheckRowIndex.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                    return;
                }
                else if (alCheckRowIndex.Count > 1)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_SINGLE_ITEM", null, null);
                    return;
                }
                else
                {
                    iRowIndex = (int)alCheckRowIndex[0];
                }

                SPCConfigurationPopup spcConifgPopup = new SPCConfigurationPopup();
                spcConifgPopup.SESSIONDATA = this.sessionData;
                spcConifgPopup.URL = this.URL;
                spcConifgPopup.PORT = this.Port;
                spcConifgPopup.CONFIG_MODE = ConfigMode.MODIFY;
                spcConifgPopup.LINE_RAWID = this._sLineRawid;
                spcConifgPopup.AREA_RAWID = this._sAreaRawid;
                spcConifgPopup.EQP_MODEL = this._sEQPModel;
                spcConifgPopup.GROUP_NAME = this._sSPCGroupName;
                spcConifgPopup.ORIGINAL_DATA = this._dtTableOriginal;
                if (isMETModelingPage)
                {
                    spcConifgPopup.MODELINGTYPE = "MET";
                }
                else
                {
                    spcConifgPopup.MODELINGTYPE = "TRACE";
                }
                spcConifgPopup.CONFIG_RAWID = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.CHART_ID);
                spcConifgPopup.MAIN_YN = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.MAIN_YN);

                if (this.bsprData.ActiveSheet.RowCount > 1) //SubConfig 존재여부
                    spcConifgPopup.HAS_SUBCONFIGS = true;

                spcConifgPopup.InitializePopup();

                DialogResult result = spcConifgPopup.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (spcConifgPopup.MAIN_YN.Equals("Y"))
                    {
                        if (llstDynamicCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                        {
                            DataTable dtModelMst = spcConifgPopup.SPCMODELDATA_DATASET.Tables[TABLE.MODEL_MST_SPC];

                            DataTable dtSPCModel = (DataTable)llstDynamicCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                            dtSPCModel.Rows[0][DCUtil.VALUE_FIELD] = _sSPCModelRawid;
                            dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD] = dtModelMst.Rows[0][COLUMN.SPC_MODEL_NAME].ToString();
                            dtSPCModel.Rows[0][COLUMN.GROUP_NAME] = spcConifgPopup.GROUP_NAME;

                            DataTable dtSPCGroup = (DataTable)llstDynamicCondition[Definition.CONDITION_KEY_GROUP_NAME];

                            dtSPCGroup.Rows[0][DCUtil.VALUE_FIELD] = spcConifgPopup.GROUP_NAME;
                            dtSPCGroup.Rows[0][DCUtil.DISPLAY_FIELD] = spcConifgPopup.GROUP_NAME;
                            dtSPCGroup.Rows[0]["CHECKED"] = "F";
                            
                            this.btxtSPCModelName.Text = dtModelMst.Rows[0][COLUMN.SPC_MODEL_NAME].ToString();
                            this._sSPCModelName = dtModelMst.Rows[0][COLUMN.SPC_MODEL_NAME].ToString();
                            this._sSPCGroupName = spcConifgPopup.GROUP_NAME;

                            this.RefreshConditions(llstDynamicCondition);
                        }
                    }

                    //MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));

                    this.ConfigListDataBinding();

                    Panel pnlDumy = new Panel();
                    pnlDumy.Size = new Size(250, 500);
                    this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", pnlDumy, false);

                }
            }
            else if (name.ToUpper().Equals(Definition.ButtonKey.DELETE_SUBCONFIG))
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                ArrayList alCheckRowIndex = this.bsprData.GetCheckedList(0);

                if (alCheckRowIndex.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                    return;
                }

                for (int i = 0; i < alCheckRowIndex.Count; i++)
                {
                    int iRowIndex = (int)alCheckRowIndex[i];
                    string sMainYN = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.MAIN_YN);

                    if (sMainYN.Equals("Y"))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_MAIN_NOT_ELIMINATE", null, null);
                        return;
                    }
                }

                DialogResult result = MSGHandler.DialogQuestionResult("SPC_DIALOG_DELETE_CONFIG", new string[] { "DELETE" }, MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    ArrayList arrTempModelConfigRawID = new ArrayList();
                    for (int i = 0; i < alCheckRowIndex.Count; i++)
                    {
                        string sConfigRawID = this.bsprData.GetCellText((int)alCheckRowIndex[i], (int)iColIdx.CHART_ID);
                        arrTempModelConfigRawID.Add(sConfigRawID);
                    }

                    _llstSearchCondition.Clear();
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_RAWID, this._sSPCModelRawid);
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, arrTempModelConfigRawID);
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_USER_ID, this.sessionData.UserId);

                    DataSet dsResult = _wsSPC.DeleteSPCModelConfig(_llstSearchCondition.GetSerialData());

                    if (DSUtil.GetResultSucceed(dsResult) == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));

                        this.ConfigListDataBinding();

                        Panel pnlDumy = new Panel();
                        pnlDumy.Size = new Size(250, 500);
                        this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", pnlDumy, false);

                    }
                }
            }
            else if (name.ToUpper().Equals(Definition.ButtonKey.DELETE_MODEL))
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                DialogResult result = MSGHandler.DialogQuestionResult("SPC_DIALOG_DELETE_SPC_MODEL", new string[] { "DELETE" }, MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    _llstSearchCondition.Clear();
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_RAWID, this._sSPCModelRawid);
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, this.bsprData.GetCellText(0, (int)iColIdx.CHART_ID));
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_USER_ID, this.sessionData.UserId);

                    DataSet dsResult = _wsSPC.DeleteSPCModel(_llstSearchCondition.GetSerialData());

                    if (DSUtil.GetResultSucceed(dsResult) == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));


                        //Dynamic Condition Refresh
                        if (llstDynamicCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                            llstDynamicCondition.Remove(Definition.CONDITION_SEARCH_KEY_SPCMODEL);

                        this.DynaminCondition.RefreshCondition(llstDynamicCondition);

                        //초기화

                        this._sSPCModelRawid = string.Empty;
                        this._sSPCModelName = string.Empty;

                        this.btxtSPCModelName.Text = string.Empty;
                        this.btxtParamAlias.Text = string.Empty;
                        this.bsprData.ActiveSheet.RowCount = 0;

                        Panel pnlDumy = new Panel();
                        pnlDumy.Size = new Size(250, 500);
                        this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", pnlDumy, false);

                    }
                }
            }
            else if (name.Equals(Definition.ButtonKey.VIEW_CHART))
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                int iRowIndex = -1;

                ArrayList alCheckRowIndex = this.bsprData.GetCheckedList(0);

                if (alCheckRowIndex.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                    return;
                }
                else if (alCheckRowIndex.Count > 1)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_SINGLE_ITEM", null, null);
                    return;
                }
                else
                {
                    iRowIndex = (int)alCheckRowIndex[0];
                }

                string sConfigRawID = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.CHART_ID);
                string sParamAlias = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.PARAM_ALIAS);

                Common.ChartViewPopup chartViewPop = new Common.ChartViewPopup();
                chartViewPop.URL = this.URL;
                chartViewPop.SessionData = this.sessionData;
                chartViewPop.ChartVariable.CHART_PARENT_MODE = CHART_PARENT_MODE.MODELING;
                chartViewPop.ChartVariable.LINE = this._sLine;
                chartViewPop.AREA_RAWID = this._sAreaRawid;
                chartViewPop.ChartVariable.SPC_MODEL = this._sSPCModelName;
                chartViewPop.ChartVariable.PARAM_ALIAS = sParamAlias;
                chartViewPop.ChartVariable.MODEL_CONFIG_RAWID = sConfigRawID;
                chartViewPop.ParamTypeCD = this.bsprData.GetCellText(iRowIndex, this._ColIdx_PARAM_TYPE_CD);
                chartViewPop.GROUP_NAME = this._sSPCGroupName;
                chartViewPop.LINE_RAWID = this._sLineRawid;
                chartViewPop.ChartVariable.AREA = this._sArea;
                //chartViewPop.ParamTypeCD = 

                chartViewPop.InitializePopup();

                chartViewPop.linkTraceDataViewEventPopup -= new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
                chartViewPop.linkTraceDataViewEventPopup += new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
                
                chartViewPop.ShowDialog(this);
                this._sSPCGroupName = chartViewPop.GROUP_NAME;

                //chartViewPop.ShowDialog();

            }
            //added by enkim 2010.01.14 SPC Model Interlock 일괄 설정.
            else if (name.Equals(Definition.ButtonKey.SAVE))
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                if (!this._bsprDataValidate)
                {
                    this.bsprData.LeaveCellAction();
                    return;
                }

                try
                {

                    //DialogResult dg = MSGHandler.DialogQuestionResult(this._mlthandler.GetMessage("GENERAL_ASK_SAVE"), null, MessageBoxButtons.YesNo);
                    //if (dg == DialogResult.No)
                    //    return;
                    
                    //SPCModelSavePopup popup = new SPCModelSavePopup(ModifyMode.SUB, "");
                    //popup.InitializeControl();
                    //popup.CHANGED_ITEMS = "Limit,";
                    //DialogResult result = popup.ShowDialog();
                    
                    //if (result != DialogResult.Yes)
                    //    return;
                    string changedItems = string.Empty;

                    DataSet _dsTemp = (DataSet)this.bsprData.DataSource;
                    DataTable _dtTemp = _dsTemp.Tables[0];
                    ArrayList arrTempRawID = new ArrayList();
                    ArrayList arrTempInterlock = new ArrayList();
                    ArrayList arrTempActivation = new ArrayList();
                    ArrayList arrTempMainYN = new ArrayList();

                    ArrayList arrTempSpecRawID = new ArrayList();
                    ArrayList arrTempSpecMainYN = new ArrayList();

                    //SPC-676 By Louis
                    ArrayList arrTempChartDescription = new ArrayList();

                    ArrayList arrTempUPPER_SPEC = new ArrayList();
                    ArrayList arrTempLOWER_SPEC = new ArrayList();
                    ArrayList arrTempUPPER_CONTROL = new ArrayList();
                    ArrayList arrTempLOWER_CONTROL = new ArrayList();
                    ArrayList arrTempRAW_UCL = new ArrayList();
                    ArrayList arrTempRAW_LCL = new ArrayList();
                    ArrayList arrTempSTD_UCL = new ArrayList();
                    ArrayList arrTempSTD_LCL = new ArrayList();
                    ArrayList arrTempRANGE_UCL = new ArrayList();
                    ArrayList arrTempRANGE_LCL = new ArrayList();
                    ArrayList arrTempEWMA_MEAN_UCL = new ArrayList();
                    ArrayList arrTempEWMA_MEAN_LCL = new ArrayList();
                    ArrayList arrTempEWMA_RANGE_UCL = new ArrayList();
                    ArrayList arrTempEWMA_RANGE_LCL = new ArrayList();
                    ArrayList arrTempEWMA_STD_UCL = new ArrayList();
                    ArrayList arrTempEWMA_STD_LCL = new ArrayList();
                    ArrayList arrTempMA_UCL = new ArrayList();
                    ArrayList arrTempMA_LCL = new ArrayList();
                    ArrayList arrTempMS_UCL = new ArrayList();
                    ArrayList arrTempMS_LCL = new ArrayList();
                    ArrayList arrTempMR_UCL = new ArrayList();
                    ArrayList arrTempMR_LCL = new ArrayList();
                    ArrayList arrTempUPPER_TECHNICAL_LIMIT = new ArrayList();
                    ArrayList arrTempLOWER_TECHNICAL_LIMIT = new ArrayList();
                    ArrayList arrTempTARGET = new ArrayList();
                    ArrayList arrTempCENTER_LINE = new ArrayList();
                    ArrayList arrTempRAW_CENTER_LINE = new ArrayList();
                    ArrayList arrTempSTD_CENTER_LINE = new ArrayList();
                    ArrayList arrTempRANGE_CENTER_LINE = new ArrayList();
                    ArrayList arrTempEWMA_MEAN_CENTER_LINE = new ArrayList();
                    ArrayList arrTempEWMA_RANGE_CENTER_LINE = new ArrayList();
                    ArrayList arrTempEWMA_STD_CENTER_LINE = new ArrayList();
                    ArrayList arrTempMA_CENTER_LINE = new ArrayList();
                    ArrayList arrTempMS_CENTER_LINE = new ArrayList();
                    ArrayList arrTempMR_CENTER_LINE = new ArrayList();
                    ArrayList arrTempSTD = new ArrayList();
                    ArrayList arrTempZONE_A_UCL = new ArrayList();
                    ArrayList arrTempZONE_A_LCL = new ArrayList();
                    ArrayList arrTempZONE_B_UCL = new ArrayList();
                    ArrayList arrTempZONE_B_LCL = new ArrayList();
                    ArrayList arrTempZONE_C_UCL = new ArrayList();
                    ArrayList arrTempZONE_C_LCL = new ArrayList();
                    LinkedList llConfigList = new LinkedList();

                    bool bDescChange = false;
                    bool bLimitChange = false;

                    for (int i = 0; i < this._dtTableOriginal.Rows.Count; i++)
                    {
                        //바뀐 경우만 Check
                        if (this._dtTableOriginal.Rows[i][COLUMN.CHART_ID].ToString() == _dtTemp.Rows[i][COLUMN.CHART_ID].ToString() &&
                            (
                            (this._dtTableOriginal.Rows[i][COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_N &&
                            (_dtTemp.Rows[i][COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_TRUE || _dtTemp.Rows[i][COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_Y)
                            ) ||
                            (this._dtTableOriginal.Rows[i][COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_N &&
                            (_dtTemp.Rows[i][COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_TRUE || _dtTemp.Rows[i][COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_Y)
                            ) ||
                            (this._dtTableOriginal.Rows[i][COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_Y &&
                            (_dtTemp.Rows[i][COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_FALSE || _dtTemp.Rows[i][COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_N)
                            ) ||
                            (this._dtTableOriginal.Rows[i][COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_Y &&
                            (_dtTemp.Rows[i][COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_FALSE || _dtTemp.Rows[i][COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_N)
                            )
                            )
                            )
                        {
                            arrTempRawID.Add(_dtTemp.Rows[i][COLUMN.CHART_ID].ToString());
                            arrTempInterlock.Add(_dtTemp.Rows[i][COLUMN.INTERLOCK_YN].ToString());
                            arrTempActivation.Add(_dtTemp.Rows[i][COLUMN.ACTIVATION_YN].ToString());
                            arrTempMainYN.Add(_dtTemp.Rows[i][COLUMN.MAIN_YN].ToString());
                            
                        }

                        if (
                            this._dtTableOriginal.Rows[i][COLUMN.CHART_ID].ToString() == _dtTemp.Rows[i][COLUMN.CHART_ID].ToString() &&
                            (
                            //SPC-676 by Louis
                            this._dtTableOriginal.Rows[i][COLUMN.CHART_DESCRIPTON].ToString() != _dtTemp.Rows[i][COLUMN.CHART_DESCRIPTON].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.UPPER_SPEC].ToString() != _dtTemp.Rows[i][COLUMN.UPPER_SPEC].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.LOWER_SPEC].ToString() != _dtTemp.Rows[i][COLUMN.LOWER_SPEC].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.UPPER_CONTROL].ToString() != _dtTemp.Rows[i][COLUMN.UPPER_CONTROL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.LOWER_CONTROL].ToString() != _dtTemp.Rows[i][COLUMN.LOWER_CONTROL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.RAW_UCL].ToString() != _dtTemp.Rows[i][COLUMN.RAW_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.RAW_LCL].ToString() != _dtTemp.Rows[i][COLUMN.RAW_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.STD_UCL].ToString() != _dtTemp.Rows[i][COLUMN.STD_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.STD_LCL].ToString() != _dtTemp.Rows[i][COLUMN.STD_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.RANGE_UCL].ToString() != _dtTemp.Rows[i][COLUMN.RANGE_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.RANGE_LCL].ToString() != _dtTemp.Rows[i][COLUMN.RANGE_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_MEAN_UCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_MEAN_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_MEAN_LCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_MEAN_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_RANGE_UCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_RANGE_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_RANGE_LCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_RANGE_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_STD_UCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_STD_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_STD_LCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_STD_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MA_UCL].ToString() != _dtTemp.Rows[i][COLUMN.MA_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MA_LCL].ToString() != _dtTemp.Rows[i][COLUMN.MA_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MS_UCL].ToString() != _dtTemp.Rows[i][COLUMN.MS_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MS_LCL].ToString() != _dtTemp.Rows[i][COLUMN.MS_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MR_UCL].ToString() != _dtTemp.Rows[i][COLUMN.MR_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MR_LCL].ToString() != _dtTemp.Rows[i][COLUMN.MR_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.UPPER_TECHNICAL_LIMIT].ToString() != _dtTemp.Rows[i][COLUMN.UPPER_TECHNICAL_LIMIT].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.LOWER_TECHNICAL_LIMIT].ToString() != _dtTemp.Rows[i][COLUMN.LOWER_TECHNICAL_LIMIT].ToString()
                            )
                            )
                        {
                            arrTempChartDescription.Add(_dtTemp.Rows[i][COLUMN.CHART_DESCRIPTON].ToString());
                            arrTempSpecRawID.Add(_dtTemp.Rows[i][COLUMN.CHART_ID].ToString());
                            arrTempSpecMainYN.Add(_dtTemp.Rows[i][COLUMN.MAIN_YN].ToString());
                            arrTempUPPER_SPEC.Add(_dtTemp.Rows[i][COLUMN.UPPER_SPEC].ToString());
                            arrTempLOWER_SPEC.Add(_dtTemp.Rows[i][COLUMN.LOWER_SPEC].ToString());
                            arrTempUPPER_CONTROL.Add(_dtTemp.Rows[i][COLUMN.UPPER_CONTROL].ToString());
                            arrTempLOWER_CONTROL.Add(_dtTemp.Rows[i][COLUMN.LOWER_CONTROL].ToString());
                            arrTempRAW_UCL.Add(_dtTemp.Rows[i][COLUMN.RAW_UCL].ToString());
                            arrTempRAW_LCL.Add(_dtTemp.Rows[i][COLUMN.RAW_LCL].ToString());
                            arrTempSTD_UCL.Add(_dtTemp.Rows[i][COLUMN.STD_UCL].ToString());
                            arrTempSTD_LCL.Add(_dtTemp.Rows[i][COLUMN.STD_LCL].ToString());
                            arrTempRANGE_UCL.Add(_dtTemp.Rows[i][COLUMN.RANGE_UCL].ToString());
                            arrTempRANGE_LCL.Add(_dtTemp.Rows[i][COLUMN.RANGE_LCL].ToString());
                            arrTempEWMA_MEAN_UCL.Add(_dtTemp.Rows[i][COLUMN.EWMA_MEAN_UCL].ToString());
                            arrTempEWMA_MEAN_LCL.Add(_dtTemp.Rows[i][COLUMN.EWMA_MEAN_LCL].ToString());
                            arrTempEWMA_RANGE_UCL.Add(_dtTemp.Rows[i][COLUMN.EWMA_RANGE_UCL].ToString());
                            arrTempEWMA_RANGE_LCL.Add(_dtTemp.Rows[i][COLUMN.EWMA_RANGE_LCL].ToString());
                            arrTempEWMA_STD_UCL.Add(_dtTemp.Rows[i][COLUMN.EWMA_STD_UCL].ToString());
                            arrTempEWMA_STD_LCL.Add(_dtTemp.Rows[i][COLUMN.EWMA_STD_LCL].ToString());
                            arrTempMA_UCL.Add(_dtTemp.Rows[i][COLUMN.MA_UCL].ToString());
                            arrTempMA_LCL.Add(_dtTemp.Rows[i][COLUMN.MA_LCL].ToString());
                            arrTempMS_UCL.Add(_dtTemp.Rows[i][COLUMN.MS_UCL].ToString());
                            arrTempMS_LCL.Add(_dtTemp.Rows[i][COLUMN.MS_LCL].ToString());
                            arrTempMR_UCL.Add(_dtTemp.Rows[i][COLUMN.MR_UCL].ToString());
                            arrTempMR_LCL.Add(_dtTemp.Rows[i][COLUMN.MR_LCL].ToString());
                            arrTempUPPER_TECHNICAL_LIMIT.Add(_dtTemp.Rows[i][COLUMN.UPPER_TECHNICAL_LIMIT].ToString());
                            arrTempLOWER_TECHNICAL_LIMIT.Add(_dtTemp.Rows[i][COLUMN.LOWER_TECHNICAL_LIMIT].ToString());
                            arrTempTARGET.Add(_dtTemp.Rows[i][COLUMN.TARGET].ToString());
                            arrTempCENTER_LINE.Add(_dtTemp.Rows[i][COLUMN.CENTER_LINE].ToString());
                            arrTempRAW_CENTER_LINE.Add(_dtTemp.Rows[i][COLUMN.RAW_CENTER_LINE].ToString());
                            arrTempSTD_CENTER_LINE.Add(_dtTemp.Rows[i][COLUMN.STD_CENTER_LINE].ToString());
                            arrTempRANGE_CENTER_LINE.Add(_dtTemp.Rows[i][COLUMN.RANGE_CENTER_LINE].ToString());
                            arrTempEWMA_MEAN_CENTER_LINE.Add(_dtTemp.Rows[i][COLUMN.EWMA_MEAN_CENTER_LINE].ToString());
                            arrTempEWMA_RANGE_CENTER_LINE.Add(_dtTemp.Rows[i][COLUMN.EWMA_RANGE_CENTER_LINE].ToString());
                            arrTempEWMA_STD_CENTER_LINE.Add(_dtTemp.Rows[i][COLUMN.EWMA_STD_CENTER_LINE].ToString());
                            arrTempMA_CENTER_LINE.Add(_dtTemp.Rows[i][COLUMN.MA_CENTER_LINE].ToString());
                            arrTempMS_CENTER_LINE.Add(_dtTemp.Rows[i][COLUMN.MS_CENTER_LINE].ToString());
                            arrTempMR_CENTER_LINE.Add(_dtTemp.Rows[i][COLUMN.MR_CENTER_LINE].ToString());
                            arrTempSTD.Add(_dtTemp.Rows[i][COLUMN.STD].ToString());
                            arrTempZONE_A_UCL.Add(_dtTemp.Rows[i][COLUMN.ZONE_A_UCL].ToString());
                            arrTempZONE_A_LCL.Add(_dtTemp.Rows[i][COLUMN.ZONE_A_LCL].ToString());
                            arrTempZONE_B_UCL.Add(_dtTemp.Rows[i][COLUMN.ZONE_B_UCL].ToString());
                            arrTempZONE_B_LCL.Add(_dtTemp.Rows[i][COLUMN.ZONE_B_LCL].ToString());
                            arrTempZONE_C_UCL.Add(_dtTemp.Rows[i][COLUMN.ZONE_C_UCL].ToString());
                            arrTempZONE_C_LCL.Add(_dtTemp.Rows[i][COLUMN.ZONE_C_LCL].ToString());

                            if(this._dtTableOriginal.Rows[i][COLUMN.CHART_DESCRIPTON].ToString() != _dtTemp.Rows[i][COLUMN.CHART_DESCRIPTON].ToString())
                            {
                                bDescChange = true;
                            }
                            if (this._dtTableOriginal.Rows[i][COLUMN.UPPER_SPEC].ToString() != _dtTemp.Rows[i][COLUMN.UPPER_SPEC].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.LOWER_SPEC].ToString() != _dtTemp.Rows[i][COLUMN.LOWER_SPEC].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.UPPER_CONTROL].ToString() != _dtTemp.Rows[i][COLUMN.UPPER_CONTROL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.LOWER_CONTROL].ToString() != _dtTemp.Rows[i][COLUMN.LOWER_CONTROL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.RAW_UCL].ToString() != _dtTemp.Rows[i][COLUMN.RAW_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.RAW_LCL].ToString() != _dtTemp.Rows[i][COLUMN.RAW_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.STD_UCL].ToString() != _dtTemp.Rows[i][COLUMN.STD_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.STD_LCL].ToString() != _dtTemp.Rows[i][COLUMN.STD_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.RANGE_UCL].ToString() != _dtTemp.Rows[i][COLUMN.RANGE_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.RANGE_LCL].ToString() != _dtTemp.Rows[i][COLUMN.RANGE_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_MEAN_UCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_MEAN_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_MEAN_LCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_MEAN_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_RANGE_UCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_RANGE_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_RANGE_LCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_RANGE_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_STD_UCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_STD_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.EWMA_STD_LCL].ToString() != _dtTemp.Rows[i][COLUMN.EWMA_STD_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MA_UCL].ToString() != _dtTemp.Rows[i][COLUMN.MA_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MA_LCL].ToString() != _dtTemp.Rows[i][COLUMN.MA_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MS_UCL].ToString() != _dtTemp.Rows[i][COLUMN.MS_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MS_LCL].ToString() != _dtTemp.Rows[i][COLUMN.MS_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MR_UCL].ToString() != _dtTemp.Rows[i][COLUMN.MR_UCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.MR_LCL].ToString() != _dtTemp.Rows[i][COLUMN.MR_LCL].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.UPPER_TECHNICAL_LIMIT].ToString() != _dtTemp.Rows[i][COLUMN.UPPER_TECHNICAL_LIMIT].ToString() ||
                            this._dtTableOriginal.Rows[i][COLUMN.LOWER_TECHNICAL_LIMIT].ToString() != _dtTemp.Rows[i][COLUMN.LOWER_TECHNICAL_LIMIT].ToString())
                            {
                                bLimitChange = true;
                            }
                        }

                    }

                    if (arrTempRawID.Count > 0 || arrTempSpecRawID.Count > 0 )
                    {
                        //base.MsgShow(COMMON_MSG.Save_Data);

                        if (arrTempRawID.Count > 0)
                        {
                            changedItems += "Config,";

                            bool isMainActive = true;
                            for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
                            {
                                if ((this.bsprData.GetCellValue(i, this._ColIdx_Activation).ToString() == Definition.VARIABLE_N || this.bsprData.GetCellValue(i, this._ColIdx_Activation).ToString() == Definition.VARIABLE_FALSE) && this.bsprData.GetCellText(i, (int)iColIdx.MAIN_YN) == Definition.VARIABLE_Y)
                                {
                                    isMainActive = false;
                                    break;
                                }
                            }

                            if (!isMainActive)
                            {
                                for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
                                {
                                    if (Convert.ToBoolean(this.bsprData.GetCellValue(i, this._ColIdx_Activation)) == true && this.bsprData.GetCellText(i, (int)iColIdx.MAIN_YN) == Definition.VARIABLE_N)
                                    {
                                        base.MsgClose();
                                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SUB_CANT_ACTIVE", null, null);
                                        return;
                                    }
                                }
                            }

                            LinkedList lnkLst = new LinkedList();
                            lnkLst.Add(Definition.DynamicCondition_Condition_key.RAWID, arrTempRawID);
                            lnkLst.Add(Definition.DynamicCondition_Condition_key.INTERLOCK_YN, arrTempInterlock);
                            lnkLst.Add(Definition.DynamicCondition_Condition_key.ACTIVATION_YN, arrTempActivation);
                            lnkLst.Add(Definition.DynamicCondition_Condition_key.MAIN_YN, arrTempMainYN);
                            lnkLst.Add(Definition.CONDITION_KEY_USER_ID, this.sessionData.UserId);

                            bool bResult = true;

                            
                            //spc-977
                            //Spec Limit 변경 없이 Activation, Interlock만 변경된 경우도 comment popup이 필요.  
                            if (arrTempSpecRawID.Count < 1)
                            {
                                SPCModelSavePopup popup = new SPCModelSavePopup(ModifyMode.SUB, "");
                                popup.InitializeControl();
                                popup.CHANGED_ITEMS = changedItems.Substring(0, changedItems.Length - 1);
                                DialogResult result = popup.ShowDialog();

                                if (result != DialogResult.Yes)
                                    return;

                                lnkLst.Add(COLUMN.SAVE_COMMENT, popup.COMMENT);
                                lnkLst.Add(COLUMN.CHANGED_ITEMS, popup.CHANGED_ITEMS);

                                //spc-977
                                base.MsgShow(COMMON_MSG.Save_Data);

                                bResult = this._wsSPC.SaveInterlockYN(lnkLst.GetSerialData());
                            }
                            else
                            {
                                //spc-977
                                llConfigList = lnkLst;
                            }

                            if (!bResult)
                            {
                                base.MsgClose();
                                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
                                return;
                            }

                        }
                        if (arrTempSpecRawID.Count > 0)
                        {
                            if (bLimitChange)
                                changedItems += "Limit,";
                            
                            if(bDescChange)
                                changedItems += "ETC,";

                            LinkedList lnkLst = new LinkedList();
                            lnkLst.Add(Definition.DynamicCondition_Condition_key.RAWID, arrTempSpecRawID);
                            lnkLst.Add(Definition.DynamicCondition_Condition_key.MAIN_YN, arrTempSpecMainYN);
                            lnkLst.Add(COLUMN.CHART_DESCRIPTON, arrTempChartDescription);
                            lnkLst.Add(COLUMN.UPPER_SPEC, arrTempUPPER_SPEC);
                            lnkLst.Add(COLUMN.LOWER_SPEC, arrTempLOWER_SPEC);
                            lnkLst.Add(COLUMN.UPPER_CONTROL, arrTempUPPER_CONTROL);
                            lnkLst.Add(COLUMN.LOWER_CONTROL, arrTempLOWER_CONTROL);
                            lnkLst.Add(COLUMN.RAW_UCL, arrTempRAW_UCL);
                            lnkLst.Add(COLUMN.RAW_LCL, arrTempRAW_LCL);
                            lnkLst.Add(COLUMN.STD_UCL, arrTempSTD_UCL);
                            lnkLst.Add(COLUMN.STD_LCL, arrTempSTD_LCL);
                            lnkLst.Add(COLUMN.RANGE_UCL, arrTempRANGE_UCL);
                            lnkLst.Add(COLUMN.RANGE_LCL, arrTempRANGE_LCL);
                            lnkLst.Add(COLUMN.EWMA_MEAN_UCL, arrTempEWMA_MEAN_UCL);
                            lnkLst.Add(COLUMN.EWMA_MEAN_LCL, arrTempEWMA_MEAN_LCL);
                            lnkLst.Add(COLUMN.EWMA_RANGE_UCL, arrTempEWMA_RANGE_UCL);
                            lnkLst.Add(COLUMN.EWMA_RANGE_LCL, arrTempEWMA_RANGE_LCL);
                            lnkLst.Add(COLUMN.EWMA_STD_UCL, arrTempEWMA_STD_UCL);
                            lnkLst.Add(COLUMN.EWMA_STD_LCL, arrTempEWMA_STD_LCL);
                            lnkLst.Add(COLUMN.MA_UCL, arrTempMA_UCL);
                            lnkLst.Add(COLUMN.MA_LCL, arrTempMA_LCL);
                            lnkLst.Add(COLUMN.MS_UCL, arrTempMS_UCL);
                            lnkLst.Add(COLUMN.MS_LCL, arrTempMS_LCL);
                            lnkLst.Add(COLUMN.MR_UCL, arrTempMR_UCL);
                            lnkLst.Add(COLUMN.MR_LCL, arrTempMR_LCL);
                            lnkLst.Add(COLUMN.UPPER_TECHNICAL_LIMIT, arrTempUPPER_TECHNICAL_LIMIT);
                            lnkLst.Add(COLUMN.LOWER_TECHNICAL_LIMIT, arrTempLOWER_TECHNICAL_LIMIT);
                            lnkLst.Add(COLUMN.TARGET, arrTempTARGET);
                            lnkLst.Add(COLUMN.CENTER_LINE, arrTempCENTER_LINE);
                            lnkLst.Add(COLUMN.RAW_CENTER_LINE, arrTempRAW_CENTER_LINE);
                            lnkLst.Add(COLUMN.STD_CENTER_LINE, arrTempSTD_CENTER_LINE);
                            lnkLst.Add(COLUMN.RANGE_CENTER_LINE, arrTempRANGE_CENTER_LINE);
                            lnkLst.Add(COLUMN.EWMA_MEAN_CENTER_LINE, arrTempEWMA_MEAN_CENTER_LINE);
                            lnkLst.Add(COLUMN.EWMA_RANGE_CENTER_LINE, arrTempEWMA_RANGE_CENTER_LINE);
                            lnkLst.Add(COLUMN.EWMA_STD_CENTER_LINE, arrTempEWMA_STD_CENTER_LINE);
                            lnkLst.Add(COLUMN.MA_CENTER_LINE, arrTempMA_CENTER_LINE);
                            lnkLst.Add(COLUMN.MS_CENTER_LINE, arrTempMS_CENTER_LINE);
                            lnkLst.Add(COLUMN.MR_CENTER_LINE, arrTempMR_CENTER_LINE);
                            lnkLst.Add(COLUMN.STD, arrTempSTD);
                            lnkLst.Add(COLUMN.ZONE_A_UCL, arrTempZONE_A_UCL);
                            lnkLst.Add(COLUMN.ZONE_A_LCL, arrTempZONE_A_LCL);
                            lnkLst.Add(COLUMN.ZONE_B_UCL, arrTempZONE_B_UCL);
                            lnkLst.Add(COLUMN.ZONE_B_LCL, arrTempZONE_B_LCL);
                            lnkLst.Add(COLUMN.ZONE_C_UCL, arrTempZONE_C_UCL);
                            lnkLst.Add(COLUMN.ZONE_C_LCL, arrTempZONE_C_LCL);
                            lnkLst.Add(Definition.CONDITION_KEY_USER_ID, this.sessionData.UserId);
                            

                            bool bResult = false;

                            //spc-977
                            SPCModelSavePopup popup = new SPCModelSavePopup(ModifyMode.SUB, "");
                            popup.InitializeControl();
                            popup.CHANGED_ITEMS = changedItems.Substring(0, changedItems.Length - 1);
                            DialogResult result = popup.ShowDialog();

                            if (result != DialogResult.Yes)
                                return;

                            base.MsgShow(COMMON_MSG.Save_Data);

                            //spc-977 Activation, Interlock y/n 변경이 없고 spec limit 변경만 있는 경우는 앞에서 save message를 보여주지 않으므로 보여주고,
                            //Limit과 Config가 동시에 변경되는 경우에는 Comment와 Changed Item이 함께 들어가므로 같이 Save함.
                            if (arrTempRawID.Count >0)
                            {
                                llConfigList.Add(COLUMN.SAVE_COMMENT, popup.COMMENT);
                                llConfigList.Add(COLUMN.CHANGED_ITEMS, popup.CHANGED_ITEMS);
                                bResult = this._wsSPC.SaveInterlockYN(llConfigList.GetSerialData());
                            }

                            lnkLst.Add(COLUMN.SAVE_COMMENT, popup.COMMENT);
                            lnkLst.Add(COLUMN.CHANGED_ITEMS, popup.CHANGED_ITEMS);

                            bResult = this._wsSPC.SaveSPCModelSpecData(lnkLst.GetSerialData());
                            
                            if(!bResult)
                            {
                                base.MsgClose();
                                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
                                return;
                            }
                        }

                        LinkedList llRawid = this.CheckRawidDuplicate(arrTempSpecRawID, arrTempRawID);

                        //SPC-977 한꺼번에 VERSION 올림.
                        bool bVersion = this._wsSPC.SetIncreaseVersion(llRawid.GetSerialData());

                        base.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));
                        
                        this.ConfigListDataBinding();

                        Panel pnlDumy = new Panel();
                        pnlDumy.Size = new Size(250, 500);
                        this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage,
                                                   "SPC Model Property", pnlDumy, false);
                    }
                    else
                    {
                        base.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Information, this._mlthandler.GetMessage("GENERAL_NO_MODIFIED_DATA"));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    base.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Information, ex.Message);
                }
                finally
                {
                    base.MsgClose();
                }
            }
            else if (name.Equals(Definition.ButtonKey.SAVEAS))
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                int iRowIndex = -1;

                ArrayList alCheckRowIndex = this.bsprData.GetCheckedList(0);

                if (alCheckRowIndex.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                    return;
                }
                else if (alCheckRowIndex.Count > 1)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_SINGLE_ITEM", null, null);
                    return;
                }
                else
                {
                    iRowIndex = (int)alCheckRowIndex[0];

                    if (this.bsprData.GetCellText(iRowIndex, (int)iColIdx.MAIN_YN).ToUpper() != "Y")
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_SINGLE_MAIN_MODEL", null, null);
                        return;
                    }
                }

                SPCConfigurationPopup spcConifgPopup = new SPCConfigurationPopup();

                SelectEQPModel selectEQPModel = new SelectEQPModel();
                if (isMETModelingPage)
                {
                    selectEQPModel.isMET = true;
                }
                else
                {
                    selectEQPModel.isMET = false;
                }
                DialogResult result = selectEQPModel.ShowDialog();
                if (result == DialogResult.OK)
                {
                    spcConifgPopup.LINE_RAWID = selectEQPModel.sLocationRawID;
                    spcConifgPopup.AREA_RAWID = selectEQPModel.sAreaRawID;
                    spcConifgPopup.EQP_MODEL = selectEQPModel.sEQPModel;
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
                else if (result == DialogResult.Ignore)
                {
                    spcConifgPopup.LINE_RAWID = this._sLineRawid;
                    spcConifgPopup.AREA_RAWID = this._sAreaRawid;
                    spcConifgPopup.EQP_MODEL = this._sEQPModel;
                    spcConifgPopup.ShowParameterAlias = false;
                }

                spcConifgPopup.SESSIONDATA = this.sessionData;
                
                spcConifgPopup.URL = this.URL;
                spcConifgPopup.PORT = this.Port;
                spcConifgPopup.CONFIG_MODE = ConfigMode.SAVE_AS;
                spcConifgPopup.CONFIG_RAWID = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.CHART_ID);
                spcConifgPopup.MAIN_YN = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.MAIN_YN);
                spcConifgPopup.HAS_SUBCONFIGS = false;
                if (isMETModelingPage)
                {
                    spcConifgPopup.MODELINGTYPE = "MET";
                }
                else
                {
                    spcConifgPopup.MODELINGTYPE = "TRACE";
                }

                //if (this.bsprData.ActiveSheet.RowCount > 1) //SubConfig 존재여부
                //    spcConifgPopup.HAS_SUBCONFIGS = true;

                spcConifgPopup.InitializePopup();

                result = spcConifgPopup.ShowDialog();

                if (result == DialogResult.OK)
                {
                    DataTable dtModel = spcConifgPopup.SPCMODELDATA_DATASET.Tables[TABLE.MODEL_MST_SPC];
                    this._sSPCModelRawid = dtModel.Rows[0][COLUMN.RAWID].ToString();
                    this._sSPCModelName = dtModel.Rows[0][COLUMN.SPC_MODEL_NAME].ToString();
                    this._sLineRawid = dtModel.Rows[0][COLUMN.LOCATION_RAWID].ToString();
                    this._sAreaRawid = dtModel.Rows[0][COLUMN.AREA_RAWID].ToString();

                    this._sLine = selectEQPModel.sLocation;
                    this._sArea = selectEQPModel.sArea;
                    this._sSPCGroupName = spcConifgPopup.GROUP_NAME;

                    if (llstDynamicCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                    {
                        DataTable dtSPCModel = (DataTable)llstDynamicCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                        dtSPCModel.Rows[0][DCUtil.VALUE_FIELD] = _sSPCModelRawid;
                        dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD] = _sSPCModelName;
                        dtSPCModel.Rows[0][COLUMN.GROUP_NAME] = _sSPCGroupName;

                        if(!dtSPCModel.Columns.Contains(COLUMN.LOCATION_RAWID))
                        {
                            dtSPCModel.Columns.Add(COLUMN.LOCATION_RAWID);
                        }

                        if (!dtSPCModel.Columns.Contains(COLUMN.AREA_RAWID))
                        {
                            dtSPCModel.Columns.Add(COLUMN.AREA_RAWID);
                        }
    
                        dtSPCModel.Rows[0][COLUMN.LOCATION_RAWID] = _sLineRawid;
                        dtSPCModel.Rows[0][COLUMN.AREA_RAWID] = _sAreaRawid;
                        
                    }
                    else
                    {
                        DataTable dtSPCModel = new DataTable();
                        dtSPCModel.Columns.Add(DCUtil.VALUE_FIELD);
                        dtSPCModel.Columns.Add(DCUtil.DISPLAY_FIELD);
                        dtSPCModel.Columns.Add("CHECKED");
                        dtSPCModel.Columns.Add(Definition.CONDITION_SEARCH_KEY_EQPMODEL);

                        DataRow drNewRow = dtSPCModel.NewRow();
                        drNewRow[DCUtil.VALUE_FIELD] = _sSPCModelRawid;
                        drNewRow[DCUtil.DISPLAY_FIELD] = _sSPCModelName;
                        drNewRow["CHECKED"] = "T";


                        drNewRow[Definition.CONDITION_SEARCH_KEY_EQPMODEL] = _sEQPModel;

                        dtSPCModel.Rows.Add(drNewRow);

                        llstDynamicCondition.Add(Definition.CONDITION_SEARCH_KEY_SPCMODEL, dtSPCModel);
                    }

                    this.DynaminCondition.RefreshCondition(llstDynamicCondition);

                    //초기화
                    this.ConfigListDataBinding();

                    Panel pnlDumy = new Panel();
                    pnlDumy.Size = new Size(250, 500);
                    this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", pnlDumy, false);
                }
            }
            else if (name.ToUpper().Equals("SPC_CALC"))
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                int iRowIndex = -1;
                
                ArrayList alCheckRowIndex = this.bsprData.GetCheckedList(0);
                

                if (alCheckRowIndex.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                    return;
                }
                else if(alCheckRowIndex.Count ==1)
                {
                    iRowIndex = (int)alCheckRowIndex[0];
                    string sConfigRawID = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.CHART_ID);
                    string sParamAlias = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.PARAM_ALIAS);

                    Common.ChartCalculationPopup chartCalcPopup = new BISTel.eSPC.Page.Common.ChartCalculationPopup();
                    chartCalcPopup.URL = this.URL;
                    chartCalcPopup.SessionData = this.sessionData;
                    chartCalcPopup.SModelConfigRawID = sConfigRawID;
                    chartCalcPopup.SParamAlias = sParamAlias;
                    //SPC-812
                    chartCalcPopup._sCalculation = false;
                    
                    chartCalcPopup.InitializePopup();
                    chartCalcPopup.ShowDialog(this);

                    this.ConfigListDataBinding();

                    Panel pnlDumy = new Panel();
                    pnlDumy.Size = new Size(250, 500);
                    this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", pnlDumy, false);
                }
                //SPC-704 Multi Calculate
                else if (alCheckRowIndex.Count > 1)
                {
                    ArrayList sMulConfigRawID = new ArrayList();
                    ArrayList sMulParamAlias = new ArrayList();
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_MULTI_CALC_EXECUTE", null, null);

                    //MSGHandler.DisplayMessage(MSGType.Information, "Select only single item.");

                    Common.Popup.MultiCalculationPopup SPCMultiCalPop = new BISTel.eSPC.Page.Common.Popup.MultiCalculationPopup();
                    SPCMultiCalPop.URL = this.URL;
                    SPCMultiCalPop.SessionData = this.sessionData;

                    for (int i = 0; i < (int)alCheckRowIndex.Count; i++)
                    {

                        iRowIndex = (int)alCheckRowIndex[i];
                        string ConfigRawID = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.CHART_ID);
                        string ParamAlias = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.PARAM_ALIAS);
                        SPCMultiCalPop.SModelConfigRawID.Add(ConfigRawID);
                        SPCMultiCalPop.SParamAlias.Add(ParamAlias);

                    }
                    SPCMultiCalPop.InitializeMultiPopup();
                    SPCMultiCalPop.ShowDialog(this);

                    this.ConfigListDataBinding();
                   
                    //return;
                }
                //else
                //{
                //    iRowIndex = (int)alCheckRowIndex[0];

                //    //string sMainYN = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.MAIN_YN);

                //    //if (sMainYN.Equals("Y"))
                //    //{
                //    //    MSGHandler.DisplayMessage(MSGType.Information, "The main configuration will not be able to calculate.");
                //    //    return;
                //    //}
                //}

                

            }

            else if (name.Equals(Definition.ButtonKey.BTN_HISTORY))
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                int iRowIndex = -1;

                ArrayList alCheckRowIndex = this.bsprData.GetCheckedList(0);

                if (alCheckRowIndex.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                    return;
                }
                else if (alCheckRowIndex.Count > 1)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_SINGLE_ITEM", null, null);
                    return;
                }
                else
                {
                    iRowIndex = (int)alCheckRowIndex[0];
                }

                SPCModelHistoryPopup spcModelHistory = new SPCModelHistoryPopup();
                spcModelHistory._sModelConfigRawID_Public = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.CHART_ID);
                spcModelHistory.InitializePopup();

                if (spcModelHistory._IsHistoryData_Pb)
                {
                    spcModelHistory.ShowDialog();
                }
            }

            //added end
            else if (name.Equals("EXPORT"))
            {
                // JIRA No.SPC-593 [GF]Dara Export Memory Out Issue - 2011.10.05 by ANDREW KO
                // No.47 on GF Communication Sheet 20110612 V1.21 - 2011.10.05 by ANDREW KO
                this.bsprData.Export(true);

                #region Export Old Version : [JIRA No.SPC-593] No.47 on GF Communication Sheet 20110612 V1.21 - 2011.10.05 by ANDREW KO
                ////bsprData.Export(false);
                ////bsprData.ContextMenuAction(name);

                //string file = "";
                //bool bProtect = this.bsprData.ActiveSheet.Protect;

                //this.bsprData.ActiveSheet.Protect = false;

                //SaveFileDialog openDlg = new SaveFileDialog();
                //openDlg.Filter = "Excel Files (*.xls)|*.xls";
                //openDlg.FileName = "";
                //openDlg.DefaultExt = ".xls";
                //openDlg.CheckFileExists = false;
                //openDlg.CheckPathExists = true;

                //DialogResult res = openDlg.ShowDialog();

                //if (res != DialogResult.OK)
                //{
                //    return;
                //}

                //file = openDlg.FileName;

                //FarPoint.Win.Spread.SheetView spread_Sheet1 = new FarPoint.Win.Spread.SheetView();
                //spread_Sheet1.SheetName = "_ExcelExportSheet";

                //FarPoint.Win.Spread.FpSpread spread = new FarPoint.Win.Spread.FpSpread();

                //spread.Sheets.Add(spread_Sheet1);
                //spread_Sheet1.Visible = true;
                //spread.ActiveSheet = spread_Sheet1;

                //byte[] buffer = null;
                //System.IO.MemoryStream stream = null;
                //this.bsprData.SetFilterVisible(false);

                //try
                //{
                //    stream = new System.IO.MemoryStream();
                //    this.bsprData.Save(stream, false);
                //    buffer = stream.ToArray();
                //    stream.Close();
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
                //finally
                //{
                //    if (stream != null)
                //    {
                //        stream.Dispose();
                //        stream = null;
                //    }
                //}

                //stream = new System.IO.MemoryStream(buffer);
                //spread.Open(stream);

                //if (stream != null)
                //{
                //    stream.Dispose();
                //    stream = null;
                //}

                //for (int i = spread.ActiveSheet.Columns.Count - 1; i >= 0; i--)
                //{
                //    if (!spread.ActiveSheet.Columns[i].Visible)
                //        spread.ActiveSheet.Columns[i].Remove();
                //}
                //spread.SaveExcel(file, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                //this.bsprData.ActiveSheet.Protect = bProtect;

                //string strMessage = "It was saved successfully. Do you open saved file?";

                //DialogResult result = MessageBox.Show(strMessage, "Open", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                //if (result == DialogResult.Yes)
                //{
                //    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\Applications\EXCEL.EXE");

                //    if (key == null)
                //    {
                //        MSGHandler.DisplayMessage(MSGType.Error, "You need microsoft office to read this file.");
                //    }
                //    else
                //    {

                //        System.Diagnostics.Process process = new System.Diagnostics.Process();
                //        process.StartInfo.FileName = file;
                //        process.Start();
                //    }
                //}
                #endregion
            }
            else if (name.Equals(Definition.ButtonKey.IMPORT))
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                ImportBsprData();
            }
        }

        void chartViewPop_linkTraceDataViewEventPopup(object sender, EventArgs e, LinkedList llstTraceLinkData)
        {
            this.SendMessage("TRACE_DATA", true, llstTraceLinkData, 0);
        }

        /// <summary>
        /// Import data of bsprData spread from a excel file
        /// </summary>
        private void ImportBsprData()
        {
            // import data from excel
            this.bsprTempForImport.AutoGenerateColumns = true;
            this.bsprTempForImport.DataSet = bsprData.GetDataSource().Copy();
            this._importcompleted = true;

            if (!this.bsprTempForImport.OpenExcelFile(true))
                return;

            this.bsprData.GetDataSource().AcceptChanges();
            BsprDataCheckboxSetting();

            // validate and copy
            // check the rawid of rows
            bool IsFound = true;
            string rawId = string.Empty;
            for (int i = 0; i < this.bsprTempForImport.ActiveSheet.Rows.Count; i++)
            {
                if (!FindRawID(i, this.bsprTempForImport.ActiveSheet, this.bsprData.ActiveSheet))
                {
                    rawId = this.bsprTempForImport.ActiveSheet.GetText(i, (int)iColIdx.CHART_ID);
                    IsFound = false;
                    break;
                }
            }
            if (!IsFound)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_ID_NOT_EXIST", new string[]{rawId}, null);
                return;
            }

            foreach (Column col in this.bsprData.ActiveSheet.Columns)
            {
                if (col.Visible
                    && col.Label != COLUMN.SELECT)
                {
                    bool isFound = false;
                    foreach (Column col2 in this.bsprTempForImport.ActiveSheet.Columns)
                    {
                        if (col.Index == (int)iColIdx.CHART_ID)
                        {
                            if (col.Label == col2.Label
                                || col2.Label.ToUpper() == "RAW_ID")
                            {
                                isFound = true;
                                break;
                            }
                        }

                        if (col.Label == col2.Label)
                        {
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        MSGHandler.DisplayMessage(MSGType.Warning,
                            string.Format(MSGHandler.GetMessage(Definition.MSG_KEY_WARNING_IMPORT_COLUMN_IS_NOT_EXIST), col.Label));
                        return;
                    }
                }
            }

            // copy, if some column's value is modified, it fails to copy
            DataTable dtConfig = _dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
            DataTable dtContext = _dsSPCModeData.Tables[TABLE.MODEL_CONTEXT_MST_SPC];

            DataRow[] drConfigs = dtConfig.Select(COLUMN.MAIN_YN + " = 'Y'", COLUMN.RAWID);
            List<string> contextColumnsForValidation = new List<string>();
            if (drConfigs != null && drConfigs.Length > 0)
            {
                DataRow[] drMainContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfigs[0][COLUMN.RAWID]), COLUMN.KEY_ORDER);

                foreach (DataRow drMainContext in drMainContexts)
                {
                    contextColumnsForValidation.Add(drMainContext["CONTEXT_KEY_NAME"].ToString());
                }
            }

            for (int col = 0; col < this.bsprTempForImport.ActiveSheet.Columns.Count; col++)
            {
                string colLabel = this.bsprTempForImport.ActiveSheet.Columns[col].Label;
                if (colLabel == COLUMN.SELECT)
                    continue;

                int colIndexOfBsprData = -1;
                for (int i = 0; i < this.bsprData.ActiveSheet.Columns.Count; i++)
                {
                    if (colLabel.ToUpper() == "RAW_ID")
                    {
                        if (this.bsprData.GetColumnHeaderLabel((int)iColIdx.CHART_ID).ToUpper() == this.bsprData.GetColumnHeaderLabel(i).ToUpper())
                        {
                            colIndexOfBsprData = i;
                            break;
                        }

                    }

                    if (colLabel.ToUpper() == this.bsprData.GetColumnHeaderLabel(i).ToUpper())
                    {
                        colIndexOfBsprData = i;
                        break;
                    }
                }
                if (-1 == colIndexOfBsprData)
                {
                    MSGHandler.DisplayMessage(MSGType.Warning,
                        string.Format(MSGHandler.GetMessage(Definition.MSG_KEY_WARNING_IMPORT_COLUMN_IS_NOT_EXIST), colLabel));
                    return;
                }

                for (int row = 0; row < this.bsprTempForImport.ActiveSheet.Rows.Count; row++)
                {
                    int rowIndexOfSpread = -1;
                    rowIndexOfSpread = FindRowIndexOfSheet(row, this.bsprTempForImport.ActiveSheet, this.bsprData.ActiveSheet);
                    if (-1 == rowIndexOfSpread)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RAWID_NOT_EXIST", new string[]{rawId}, null);
                        return;
                    }

                    if (colLabel == this.bsprData.GetColumnHeaderLabel(this._colIdx_MAIN_YN)
                        || colLabel == this.bsprData.GetColumnHeaderLabel(this._ColIdx_RULE_LIST)
                        || colLabel == this.bsprData.GetColumnHeaderLabel(this._ColIdx_OCAP_ACTION)
                        || colLabel == this.bsprData.GetColumnHeaderLabel((int)iColIdx.CHART_ID)
                        || colLabel.ToUpper() == "RAW_ID"
                        || colLabel.ToUpper() == "VERSION"
                        || contextColumnsForValidation.Contains(colLabel))
                    {
                        if (this.bsprData.GetCellText(rowIndexOfSpread, colIndexOfBsprData) != this.bsprTempForImport.GetCellText(row, col))
                        {
                            MSGHandler.DisplayMessage(
                                MSGType.Warning,
                                string.Format(MSGHandler.GetMessage(Definition.MSG_KEY_WARNING_IMPORT_COLUMN_VALUE_CANT_BE_MODIFIED), colLabel));
                            this.bsprData.GetDataSource().RejectChanges();
                            return;
                        }
                    }
                    else if (colLabel == this.bsprData.GetColumnHeaderLabel(this._ColIdx_CREATE_BY)
                        || colLabel == this.bsprData.GetColumnHeaderLabel(this._ColIdx_CREATE_DTTS)
                        || colLabel == this.bsprData.GetColumnHeaderLabel(this._ColIdx_LAST_UPDATE_BY)
                        || colLabel == this.bsprData.GetColumnHeaderLabel(this._ColIdx_LAST_UPDATE_DTTS))
                    {
                        continue;
                    }
                    else
                    {
                        this.bsprData.SetCellText(rowIndexOfSpread, colIndexOfBsprData, bsprTempForImport.GetCellText(row, col));
                    }
                }
            }

            // check all cell is validate
            bool isValidate = true;
            for (int col = 0; col < this.bsprData.ActiveSheet.Columns.Count; col++)
            {
                for (int row = 0; row < this.bsprData.ActiveSheet.Rows.Count; row++)
                {
                    ChangeEventArgs e = new ChangeEventArgs(this.bsprData.GetRootWorkbook(), row, col);
                    string msg = this.bsprDataChange(this.bsprData, e);
                    if (msg.Length > 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, msg);
                        isValidate = false;
                        break;
                    }
                }
                if (!isValidate)
                    break;
            }

            // apply if validate
            if (isValidate)
            {
                this.bsprData.GetDataSource().AcceptChanges();
                BsprDataCheckboxSetting();
                if (_importcompleted)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_IMPORT", null, null);
                }

            }
            else
            {
                this.bsprData.GetDataSource().RejectChanges();
            }

        }

        private bool FindRawID(int index, SheetView sheet, SheetView targetView)
        {
            string rawId = sheet.GetText(index, (int)iColIdx.CHART_ID);
            for (int i = 0; i < targetView.Rows.Count; i++)
            {
                if (rawId.Equals(targetView.GetText(i, (int)iColIdx.CHART_ID)))
                    return true;
            }

            return false;
        }

        private int FindRowIndexOfSheet(int index, SheetView sheet, SheetView targetView)
        {
            string rawId = sheet.GetText(index, (int)iColIdx.CHART_ID);
            for (int i = 0; i < targetView.Rows.Count; i++)
            {
                if (rawId.Equals(targetView.GetText(i, (int)iColIdx.CHART_ID)))
                    return i;
            }

            return -1;
        }

        #region ::: User Defined Method.

        private void ConfigListDataBinding()
        {
            try
            {
                DataTable dt = ((DataTable)llstDynamicCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL]);
                while (dt != null && dt.Rows.Count > 1)
                {
                    dt.Rows.RemoveAt(0);
                }

                //this.MsgShow(COMMON_MSG.Query_Data);

                string strParamAlias = "";
                //초기화
                _llstSearchCondition.Clear();
                _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_RAWID, this._sSPCModelRawid);
                _llstSearchCondition.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

                //Louis >> Cancle기능 추가
                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_wsSPC, "GetSPCModelData", new object[] { _llstSearchCondition.GetSerialData() });

                EESProgressBar.CloseProgress(this);
                //

                //_dsSPCModeData = _wsSPC.GetSPCModelData(_llstSearchCondition.GetSerialData());

                if (objDataSet != null)
                {
                    _dsSPCModeData = (DataSet)objDataSet;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                if (!DSUtil.CheckRowCount(_dsSPCModeData, TABLE.MODEL_MST_SPC))
                {
                    //this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_ALREADY_ELIMINATED",new string[]{ _sSPCModelName}, null);
                    return;
                }

                this.btxtSPCModelName.Text = this._sSPCModelName;


                DataTable dtConfig = _dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
                DataTable dtContext = _dsSPCModeData.Tables[TABLE.MODEL_CONTEXT_MST_SPC];
                DataTable dtRuleMst = _dsSPCModeData.Tables[TABLE.MODEL_RULE_MST_SPC];

                //
                EESProgressBar.ShowProgress(this, MSGHandler.GetMessage("PROCESS_LOADING_PAGE_DATA"), false);

                //#01. SPC Model Chart List를 위한 Datatable 생성
                DataTable dtSPCModelChartList = new DataTable();

                dtSPCModelChartList.Columns.Add(COLUMN.SELECT, typeof(Boolean));
                dtSPCModelChartList.Columns.Add(COLUMN.CHART_ID);
                dtSPCModelChartList.Columns.Add(COLUMN.PARAM_ALIAS);
                dtSPCModelChartList.Columns.Add(COLUMN.MAIN_YN);
                dtSPCModelChartList.Columns.Add(COLUMN.VERSION);
                dtSPCModelChartList.Columns.Add("MODE");

                //CONTEXT COLUMN 생성
                DataRow[] drConfigs = dtConfig.Select(COLUMN.MAIN_YN + " = 'Y'", COLUMN.RAWID);

                if (drConfigs != null && drConfigs.Length > 0)
                {
                    DataRow[] drMainContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfigs[0][COLUMN.RAWID]), COLUMN.KEY_ORDER);

                    foreach (DataRow drMainContext in drMainContexts)
                    {
                        dtSPCModelChartList.Columns.Add(drMainContext["CONTEXT_KEY_NAME"].ToString());
                    }
                }

                //MODEL 정보 COLUMN 생성
                dtSPCModelChartList.Columns.Add(RULE_LIST);
                dtSPCModelChartList.Columns.Add(OCAP_ACTION);
                dtSPCModelChartList.Columns.Add(COLUMN.CHART_DESCRIPTON);
                dtSPCModelChartList.Columns.Add(COLUMN.UPPER_SPEC);
                dtSPCModelChartList.Columns.Add(COLUMN.LOWER_SPEC);
                dtSPCModelChartList.Columns.Add(COLUMN.UPPER_CONTROL);
                dtSPCModelChartList.Columns.Add(COLUMN.LOWER_CONTROL);
                dtSPCModelChartList.Columns.Add(COLUMN.RAW_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.RAW_LCL);

                //added by enkim 2010.06.17  visible and edit column.
                dtSPCModelChartList.Columns.Add(COLUMN.STD_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.STD_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.RANGE_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.RANGE_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_MEAN_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_MEAN_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_RANGE_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_RANGE_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_STD_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_STD_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MA_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MA_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MS_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MS_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MR_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.MR_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.UPPER_TECHNICAL_LIMIT);
                dtSPCModelChartList.Columns.Add(COLUMN.LOWER_TECHNICAL_LIMIT);
                //added end

                dtSPCModelChartList.Columns.Add(COLUMN.PARAM_TYPE_CD);
                dtSPCModelChartList.Columns.Add(COLUMN.INTERLOCK_YN);
                dtSPCModelChartList.Columns.Add(COLUMN.ACTIVATION_YN);
                dtSPCModelChartList.Columns.Add(COLUMN.CREATE_BY);
                dtSPCModelChartList.Columns.Add(COLUMN.CREATE_DTTS);
                dtSPCModelChartList.Columns.Add(COLUMN.LAST_UPDATE_BY);
                dtSPCModelChartList.Columns.Add(COLUMN.LAST_UPDATE_DTTS);


                //added by enkim 2010.06.17  invisible colum
                dtSPCModelChartList.Columns.Add(COLUMN.TARGET);
                dtSPCModelChartList.Columns.Add(COLUMN.CENTER_LINE);
                dtSPCModelChartList.Columns.Add(COLUMN.RAW_CENTER_LINE);
                dtSPCModelChartList.Columns.Add(COLUMN.STD_CENTER_LINE);
                dtSPCModelChartList.Columns.Add(COLUMN.RANGE_CENTER_LINE);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_MEAN_CENTER_LINE);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_RANGE_CENTER_LINE);
                dtSPCModelChartList.Columns.Add(COLUMN.EWMA_STD_CENTER_LINE);
                dtSPCModelChartList.Columns.Add(COLUMN.MA_CENTER_LINE);
                dtSPCModelChartList.Columns.Add(COLUMN.MS_CENTER_LINE);
                dtSPCModelChartList.Columns.Add(COLUMN.MR_CENTER_LINE);
                dtSPCModelChartList.Columns.Add(COLUMN.STD);
                dtSPCModelChartList.Columns.Add(COLUMN.ZONE_A_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.ZONE_A_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.ZONE_B_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.ZONE_B_LCL);
                dtSPCModelChartList.Columns.Add(COLUMN.ZONE_C_UCL);
                dtSPCModelChartList.Columns.Add(COLUMN.ZONE_C_LCL);
                //added end

                // Loading chart code code master data 
                LinkedList llCondition = new LinkedList();
                llCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CHART_MODE);
                DataSet _dsChartMode = this._wsSPC.GetCodeData(llCondition.GetSerialData());

                Dictionary<string, string> modeCodeData = new Dictionary<string, string>();
                if (_dsChartMode != null && _dsChartMode.Tables.Count > 0)
                {
                    foreach (DataRow dr in _dsChartMode.Tables[0].Rows)
                    {
                        modeCodeData.Add(dr[COLUMN.CODE].ToString(), dr[COLUMN.NAME].ToString());
                    }
                }

                //#02. CONFIG MST에 생성된 CONTEXT COLUMN에 Data 입력
                foreach (DataRow drConfig in dtConfig.Rows)
                {
                    DataRow drChartList = dtSPCModelChartList.NewRow();

                    drChartList[COLUMN.CHART_ID] = drConfig[COLUMN.RAWID].ToString();
                    drChartList[COLUMN.PARAM_ALIAS] = drConfig[COLUMN.PARAM_ALIAS].ToString();
                    drChartList[COLUMN.MAIN_YN] = drConfig[COLUMN.MAIN_YN].ToString();
                    //#Version이 Null 또는 Empty인 경우 대비 Check Logic 추가
                    if (!string.IsNullOrEmpty(drConfig[COLUMN.VERSION].ToString()))
                        drChartList[COLUMN.VERSION] = (1 + Convert.ToDouble(drConfig[COLUMN.VERSION].ToString()) / 100).ToString("N2");
                    string modeValue = drConfig[COLUMN.CHART_MODE_CD].ToString();
                    if (modeCodeData.ContainsKey(modeValue))
                    {
                        modeValue = modeCodeData[modeValue];
                    }
                    drChartList["MODE"] = modeValue;

                    if (strParamAlias == "")
                        strParamAlias = drConfig[COLUMN.PARAM_ALIAS].ToString();

                    DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfig[COLUMN.RAWID]));

                    foreach (DataRow drContext in drContexts)
                    {
                        //2009-11-27 bskwon 추가 : Sub Model 상속 구조가 아닌경우 예외처리
                        if (!dtSPCModelChartList.Columns.Contains(drContext["CONTEXT_KEY_NAME"].ToString()))
                            dtSPCModelChartList.Columns.Add(drContext["CONTEXT_KEY_NAME"].ToString());

                        drChartList[drContext["CONTEXT_KEY_NAME"].ToString()] = drContext[COLUMN.CONTEXT_VALUE].ToString();
                    }

                    //MODEL 정보                
                    drChartList[COLUMN.CHART_DESCRIPTON] = drConfig[COLUMN.CHART_DESCRIPTON].ToString();
                    drChartList[COLUMN.UPPER_SPEC] = drConfig[COLUMN.UPPER_SPEC].ToString();
                    drChartList[COLUMN.LOWER_SPEC] = drConfig[COLUMN.LOWER_SPEC].ToString();
                    drChartList[COLUMN.UPPER_CONTROL] = drConfig[COLUMN.UPPER_CONTROL].ToString();
                    drChartList[COLUMN.LOWER_CONTROL] = drConfig[COLUMN.LOWER_CONTROL].ToString();
                    drChartList[COLUMN.RAW_UCL] = drConfig[COLUMN.RAW_UCL].ToString();
                    drChartList[COLUMN.RAW_LCL] = drConfig[COLUMN.RAW_LCL].ToString();
                    drChartList[COLUMN.PARAM_TYPE_CD] = drConfig[COLUMN.PARAM_TYPE_CD].ToString();

                    drChartList[COLUMN.INTERLOCK_YN] = _ComUtil.NVL(drConfig[COLUMN.INTERLOCK_YN], "N", true);
                    drChartList[COLUMN.ACTIVATION_YN] = _ComUtil.NVL(drConfig[COLUMN.ACTIVATION_YN], "N", true);
                    drChartList[COLUMN.CREATE_BY] = drConfig[COLUMN.CREATE_BY].ToString();
                    drChartList[COLUMN.CREATE_DTTS] = drConfig[COLUMN.CREATE_DTTS] == DBNull.Value ? null : DateTime.Parse(drConfig[COLUMN.CREATE_DTTS].ToString()).ToString(Definition.DATETIME_FORMAT_MS).ToString();
                    drChartList[COLUMN.LAST_UPDATE_BY] = drConfig[COLUMN.LAST_UPDATE_BY].ToString();
                    drChartList[COLUMN.LAST_UPDATE_DTTS] = drConfig[COLUMN.LAST_UPDATE_DTTS] == DBNull.Value ? null : DateTime.Parse(drConfig[COLUMN.LAST_UPDATE_DTTS].ToString()).ToString(Definition.DATETIME_FORMAT_MS).ToString();


                    //added by enkim 2010.06.17
                    drChartList[COLUMN.STD_UCL] = drConfig[COLUMN.STD_UCL].ToString();
                    drChartList[COLUMN.STD_LCL] = drConfig[COLUMN.STD_LCL].ToString();
                    drChartList[COLUMN.RANGE_UCL] = drConfig[COLUMN.RANGE_UCL].ToString();
                    drChartList[COLUMN.RANGE_LCL] = drConfig[COLUMN.RANGE_LCL].ToString();
                    drChartList[COLUMN.EWMA_MEAN_UCL] = drConfig[COLUMN.EWMA_M_UCL].ToString();
                    drChartList[COLUMN.EWMA_MEAN_LCL] = drConfig[COLUMN.EWMA_M_LCL].ToString();
                    drChartList[COLUMN.EWMA_RANGE_UCL] = drConfig[COLUMN.EWMA_R_UCL].ToString();
                    drChartList[COLUMN.EWMA_RANGE_LCL] = drConfig[COLUMN.EWMA_R_LCL].ToString();
                    drChartList[COLUMN.EWMA_STD_UCL] = drConfig[COLUMN.EWMA_S_UCL].ToString();
                    drChartList[COLUMN.EWMA_STD_LCL] = drConfig[COLUMN.EWMA_S_LCL].ToString();
                    drChartList[COLUMN.MA_UCL] = drConfig[COLUMN.MA_UCL].ToString();
                    drChartList[COLUMN.MA_LCL] = drConfig[COLUMN.MA_LCL].ToString();
                    drChartList[COLUMN.MS_UCL] = drConfig[COLUMN.MS_UCL].ToString();
                    drChartList[COLUMN.MS_LCL] = drConfig[COLUMN.MS_LCL].ToString();
                    drChartList[COLUMN.MR_UCL] = drConfig[COLUMN.MR_UCL].ToString();
                    drChartList[COLUMN.MR_LCL] = drConfig[COLUMN.MR_LCL].ToString();
                    drChartList[COLUMN.UPPER_TECHNICAL_LIMIT] = drConfig[COLUMN.UPPER_TECHNICAL_LIMIT].ToString();
                    drChartList[COLUMN.LOWER_TECHNICAL_LIMIT] = drConfig[COLUMN.LOWER_TECHNICAL_LIMIT].ToString();
                    drChartList[COLUMN.TARGET] = drConfig[COLUMN.TARGET].ToString();
                    drChartList[COLUMN.CENTER_LINE] = drConfig[COLUMN.CENTER_LINE].ToString();
                    drChartList[COLUMN.RAW_CENTER_LINE] = drConfig[COLUMN.RAW_CENTER_LINE].ToString();
                    drChartList[COLUMN.STD_CENTER_LINE] = drConfig[COLUMN.STD_CENTER_LINE].ToString();
                    drChartList[COLUMN.RANGE_CENTER_LINE] = drConfig[COLUMN.RANGE_CENTER_LINE].ToString();
                    drChartList[COLUMN.EWMA_MEAN_CENTER_LINE] = drConfig[COLUMN.EWMA_M_CENTER_LINE].ToString();
                    drChartList[COLUMN.EWMA_RANGE_CENTER_LINE] = drConfig[COLUMN.EWMA_R_CENTER_LINE].ToString();
                    drChartList[COLUMN.EWMA_STD_CENTER_LINE] = drConfig[COLUMN.EWMA_S_CENTER_LINE].ToString();
                    drChartList[COLUMN.MA_CENTER_LINE] = drConfig[COLUMN.MA_CENTER_LINE].ToString();
                    drChartList[COLUMN.MS_CENTER_LINE] = drConfig[COLUMN.MS_CENTER_LINE].ToString();
                    drChartList[COLUMN.MR_CENTER_LINE] = drConfig[COLUMN.MR_CENTER_LINE].ToString();
                    drChartList[COLUMN.STD] = drConfig[COLUMN.STD].ToString();
                    drChartList[COLUMN.ZONE_A_UCL] = drConfig[COLUMN.ZONE_A_UCL].ToString();
                    drChartList[COLUMN.ZONE_A_LCL] = drConfig[COLUMN.ZONE_A_LCL].ToString();
                    drChartList[COLUMN.ZONE_B_UCL] = drConfig[COLUMN.ZONE_B_UCL].ToString();
                    drChartList[COLUMN.ZONE_B_LCL] = drConfig[COLUMN.ZONE_B_LCL].ToString();
                    drChartList[COLUMN.ZONE_C_UCL] = drConfig[COLUMN.ZONE_C_UCL].ToString();
                    drChartList[COLUMN.ZONE_C_LCL] = drConfig[COLUMN.ZONE_C_LCL].ToString();
                    //added end


                    //RULE LIST
                    DataRow[] drRuleMsts = dtRuleMst.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfig[COLUMN.RAWID]));

                    if (drRuleMsts != null && drRuleMsts.Length > 0)
                    {

                        for (int i = 0; i < drRuleMsts.Length; i++)
                        {
                            if (i == 0)
                            {
                                drChartList["RULE_LIST"] = string.Format("RULE{0}", drRuleMsts[i][COLUMN.SPC_RULE_NO]);
                                drChartList["OCAP"] = string.Format("[{0},{1}]",drChartList["RULE_LIST"], drRuleMsts[i][COLUMN.OCAP_LIST]);
                            }
                            else
                            {
                                if(_bUseComma)
                                    drChartList["RULE_LIST"] = string.Format("{0},RULE{1}", drChartList["RULE_LIST"], drRuleMsts[i][COLUMN.SPC_RULE_NO]);
                                else
                                    drChartList["RULE_LIST"] = string.Format("{0};RULE{1}", drChartList["RULE_LIST"], drRuleMsts[i][COLUMN.SPC_RULE_NO]);
                                drChartList["OCAP"] = string.Format("{0},[RULE{1},{2}]", drChartList["OCAP"], drRuleMsts[i][COLUMN.SPC_RULE_NO],drRuleMsts[i][COLUMN.OCAP_LIST]);
                            }
                        }
                    }

                    dtSPCModelChartList.Rows.Add(drChartList);
                }

                dtSPCModelChartList.AcceptChanges();
                this._dtTableOriginal = dtSPCModelChartList;
                this._dtTableOriginal.AcceptChanges();

                bsprData.ClearHead();
                bsprData.UseEdit = true;
                bsprData.UseHeadColor = true;
                bsprData.Locked = true;

                this._colIdx_MAIN_YN = dtSPCModelChartList.Columns.IndexOf(COLUMN.MAIN_YN);
                this._ColIdx_RULE_LIST = dtSPCModelChartList.Columns.IndexOf(RULE_LIST);
                this._ColIdx_OCAP_ACTION = dtSPCModelChartList.Columns.IndexOf(OCAP_ACTION);

                //SPC-676 by Louis
                this._Colldx_CHART_DESCRIPTION = dtSPCModelChartList.Columns.IndexOf(COLUMN.CHART_DESCRIPTON);

                this._ColIdx_USL = dtSPCModelChartList.Columns.IndexOf(COLUMN.UPPER_SPEC);
                this._ColIdx_LSL = dtSPCModelChartList.Columns.IndexOf(COLUMN.LOWER_SPEC);
                this._ColIdx_MEAN_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.UPPER_CONTROL);
                this._ColIdx_MEAN_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.LOWER_CONTROL);
                this._ColIdx_RAW_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.RAW_UCL);
                this._ColIdx_RAW_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.RAW_LCL);
                this._ColIdx_STD_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.STD_UCL);
                this._ColIdx_STD_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.STD_LCL);
                this._ColIdx_RANGE_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.RANGE_UCL);
                this._ColIdx_RANGE_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.RANGE_LCL);
                this._ColIdx_EWMA_M_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.EWMA_MEAN_UCL);
                this._ColIdx_EWMA_M_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.EWMA_MEAN_LCL);
                this._ColIdx_EWMA_R_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.EWMA_RANGE_UCL);
                this._ColIdx_EWMA_R_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.EWMA_RANGE_LCL);
                this._ColIdx_EWMA_S_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.EWMA_STD_UCL);
                this._ColIdx_EWMA_S_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.EWMA_STD_LCL);
                this._ColIdx_MA_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.MA_UCL);
                this._ColIdx_MA_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.MA_LCL);
                this._ColIdx_MS_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.MS_UCL);
                this._ColIdx_MS_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.MS_LCL);
                this._ColIdx_MR_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.MR_UCL);
                this._ColIdx_MR_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.MR_LCL);
                this._ColIdx_UPPER_TECHNICAL_LIMIT = dtSPCModelChartList.Columns.IndexOf(COLUMN.UPPER_TECHNICAL_LIMIT);
                this._ColIdx_LOWER_TECHNICAL_LIMIT = dtSPCModelChartList.Columns.IndexOf(COLUMN.LOWER_TECHNICAL_LIMIT);
                this._ColIdx_TARGET = dtSPCModelChartList.Columns.IndexOf(COLUMN.TARGET);
                this._ColIdx_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(COLUMN.CENTER_LINE);
                this._ColIdx_RAW_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(COLUMN.RAW_CENTER_LINE);
                this._ColIdx_STD_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(COLUMN.STD_CENTER_LINE);
                this._ColIdx_RANGE_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(COLUMN.RANGE_CENTER_LINE);
                this._ColIdx_EWMA_M_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(COLUMN.EWMA_MEAN_CENTER_LINE);
                this._ColIdx_EWMA_R_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(COLUMN.EWMA_RANGE_CENTER_LINE);
                this._ColIdx_EWMA_S_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(COLUMN.EWMA_STD_CENTER_LINE);
                this._ColIdx_MA_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(COLUMN.MA_CENTER_LINE);
                this._ColIdx_MS_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(COLUMN.MS_CENTER_LINE);
                this._ColIdx_MR_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(COLUMN.MR_CENTER_LINE);
                this._ColIdx_STD = dtSPCModelChartList.Columns.IndexOf(COLUMN.STD);
                this._ColIdx_ZONE_A_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.ZONE_A_UCL);
                this._ColIdx_ZONE_A_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.ZONE_A_LCL);
                this._ColIdx_ZONE_B_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.ZONE_B_UCL);
                this._ColIdx_ZONE_B_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.ZONE_B_LCL);
                this._ColIdx_ZONE_C_UCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.ZONE_C_UCL);
                this._ColIdx_ZONE_C_LCL = dtSPCModelChartList.Columns.IndexOf(COLUMN.ZONE_C_LCL);
                this._ColIdx_PARAM_TYPE_CD = dtSPCModelChartList.Columns.IndexOf(COLUMN.PARAM_TYPE_CD);
                this._ColIdx_CREATE_BY = dtSPCModelChartList.Columns.IndexOf(COLUMN.CREATE_BY);
                this._ColIdx_CREATE_DTTS = dtSPCModelChartList.Columns.IndexOf(COLUMN.CREATE_DTTS);
                this._ColIdx_LAST_UPDATE_BY = dtSPCModelChartList.Columns.IndexOf(COLUMN.LAST_UPDATE_BY);
                this._ColIdx_LAST_UPDATE_DTTS = dtSPCModelChartList.Columns.IndexOf(COLUMN.LAST_UPDATE_DTTS);

                for (int i = 0; i < dtSPCModelChartList.Columns.Count; i++)
                {
                    string sColumn = dtSPCModelChartList.Columns[i].ColumnName.ToString();
                    //this.bsprData.ActiveSheet.ColumnHeader.Cells[0, i].Text = sColumn;
                    if (i == (int)iColIdx.SELECT)
                    {
                        this.bsprData.AddHead(i, sColumn, sColumn, 50, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.CheckBox, null, null, null, false, true);
                    }
                    else if (i == _ColIdx_MEAN_UCL)
                    {
                        string headerName = MSGHandler.GetVariable("SPC_MEAN_UCL");
                        this.bsprData.AddHead(i, headerName, sColumn, 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    }
                    else if (i == _ColIdx_MEAN_LCL)
                    {
                        string headerName = MSGHandler.GetVariable("SPC_MEAN_LCL");
                        this.bsprData.AddHead(i, headerName, sColumn, 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    }
                    else
                    {
                        this.bsprData.AddHead(i, sColumn, sColumn, 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    }
                }

                this.bsprData.AddHeadComplete();
                this.bsprData.DataSet = dtSPCModelChartList;

                this.bsprData.Locked = true;
                this.bsprData.ActiveSheet.Columns[(int)iColIdx.SELECT].Locked = false;
                //this.bsprData.ActiveSheet.Columns[(int)iColIdx.RAWID].Visible = false;
                this.bsprData.ActiveSheet.Columns[(int)iColIdx.PARAM_ALIAS].Visible = false;

                


                //added by enkim 2010.01.14 SPC Model Interlock 일괄 설정.
                BsprDataCheckboxSetting();
                //added end

                //this.bsprData.IsCellCopy = true;
                //this.bsprData.UseEdit = false;
                this.bsprData.AllowNewRow = false;
                this.bsprData.ActiveSheet.Columns[this._Colldx_CHART_DESCRIPTION, this._ColIdx_LOWER_TECHNICAL_LIMIT].Locked = false;
                this.bsprData.ActiveSheet.Columns[this._ColIdx_PARAM_TYPE_CD].Visible = false;
                this.bsprData.ActiveSheet.Columns[this._ColIdx_TARGET, this.bsprData.ActiveSheet.Columns.Count - 1].Visible = false;

                FarPoint.Win.Spread.CellType.TextCellType tc = new FarPoint.Win.Spread.CellType.TextCellType();
                tc.MaxLength = 1024;

                //Column Size 조절
                for (int cIdx = 0; cIdx < this.bsprData.ActiveSheet.Columns.Count; cIdx++)
                {
                    this.bsprData.ActiveSheet.Columns[cIdx].Width = this.bsprData.ActiveSheet.Columns[cIdx].GetPreferredWidth();

                    if (this.bsprData.ActiveSheet.Columns[cIdx].Width > 150)
                        this.bsprData.ActiveSheet.Columns[cIdx].Width = 150;

                    if (this.bsprData.ActiveSheet.Columns[cIdx].CellType != null
                        && this.bsprData.ActiveSheet.Columns[cIdx].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.TextCellType))
                    {
                        this.bsprData.ActiveSheet.Columns[cIdx].CellType = tc;
                    }
                }

                this.SetPropertyModelInfomation(-1);

                BsprDataCheckboxSetting();

                //MAIN은 첫번째 ROW에 배치하고 ROW HIGHLIGHT
                if (this.bsprData.GetCellText(0, (int)iColIdx.MAIN_YN).Equals("Y"))
                {
                    this.bsprData.ActiveSheet.Rows[0].BackColor = Color.LightGreen; //Color.LemonChiffon;
                }

                if (strParamAlias != "")
                    this.btxtParamAlias.Text = strParamAlias;

                this.bsprData.LeaveCellAction();

            }
            catch (Exception ex)
            {
                EESProgressBar.CloseProgress(this);
                if (ex is OperationCanceledException || ex is TimeoutException)
                {
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
                else
                {
                    LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
                //this.MsgClose();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                EESProgressBar.CloseProgress(this);
                //this.MsgClose();
            }
        }

        #endregion

        private void BsprDataCheckboxSetting()
        {
            if (bsprData.DataSet == null
                || ((DataSet)bsprData.DataSet).Tables.Count == 0)
                return;

            DataTable dt = ((DataSet)bsprData.DataSet).Tables[0];
            FarPoint.Win.Spread.CellType.CheckBoxCellType cbct = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

            if (dt.Columns.Contains(COLUMN.INTERLOCK_YN))
            {
                this._ColIdx_Interlock = dt.Columns.IndexOf(COLUMN.INTERLOCK_YN);
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(COLUMN.INTERLOCK_YN)].CellType = cbct;
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(COLUMN.INTERLOCK_YN)].Locked = false;
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(COLUMN.INTERLOCK_YN)].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
                {
                    if (this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.INTERLOCK_YN)].Value == null)
                    {
                        _importcompleted = false;
                        MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_INTERLOCK_CANT_MODIFY", null, null);
                        break;
                    }
                    else
                    {
                        if (this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.INTERLOCK_YN)].Value.ToString() == Definition.VARIABLE_Y
                            || this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.INTERLOCK_YN)].Value.ToString().ToUpper() == "TRUE")
                        {
                            this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.INTERLOCK_YN)].Value = true;
                            this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.INTERLOCK_YN)].Text = Definition.VARIABLE_TRUE;
                        }
                        else
                        {
                            this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.INTERLOCK_YN)].Value = false;
                            this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.INTERLOCK_YN)].Text = Definition.VARIABLE_FALSE;
                        }
                    }
                }
            }

            if (dt.Columns.Contains(COLUMN.ACTIVATION_YN))
            {
                this._ColIdx_Activation = dt.Columns.IndexOf(COLUMN.ACTIVATION_YN);
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(COLUMN.ACTIVATION_YN)].CellType = cbct;
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(COLUMN.ACTIVATION_YN)].Locked = false;
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(COLUMN.ACTIVATION_YN)].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
                {
                    if (this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.ACTIVATION_YN)].Value == null)
                    {
                        _importcompleted = false;
                        MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_ACTIVATION_CANT_MODIFY", null, null);
                        break;
                    }
                    if (this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.ACTIVATION_YN)].Value.ToString() == Definition.VARIABLE_Y
                        || this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.ACTIVATION_YN)].Value.ToString().ToUpper() == "TRUE")
                    {
                        this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.ACTIVATION_YN)].Value = true;
                        this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.ACTIVATION_YN)].Text = Definition.VARIABLE_TRUE;
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.ACTIVATION_YN)].Value = false;
                        this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(COLUMN.ACTIVATION_YN)].Text = Definition.VARIABLE_FALSE;
                    }
                }
            }
        }

        //public void bsprData_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        //{

        //}

        private string CheckActivation(int indexRow)
        {
            if (this.isMain(indexRow))
            {
                if (!this.isActivated(indexRow))
                {
                    for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
                    {
                        if (this.isActivated(i) && !this.isMain(i))
                        {
                            return "You can not deactivate the main configuration when the sub configuration is active.";
                        }
                    }
                }
            }
            else
            {
                if (this.isActivated(indexRow))
                {
                    for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
                    {
                        if (!this.isActivated(i) && this.isMain(i))
                        {
                            return "You can not activate the sub configuration when the main configuration is inactive.";
                        }
                    }

                }
            }

            return string.Empty;
        }

        public void bsprData_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            this._bsprDataSelectedRange = e.Range;
        }

        private bool isMain(int indexRow)
        {
            if ((string)this.bsprData.GetCellValue(indexRow, (int)iColIdx.MAIN_YN) == Definition.VARIABLE_Y)
            {
                return true;
            }
            return false;
        }

        private bool isActivated(int indexRow)
        {
            //return Convert.ToBoolean(this.bsprData.GetCellValue(indexRow, _ColIdx_Activation));

            if ((string)this.bsprData.GetCellValue(indexRow, _ColIdx_Activation) == Definition.VARIABLE_Y || (string)this.bsprData.GetCellValue(indexRow, _ColIdx_Activation) == Definition.VARIABLE_TRUE)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check it is included in selected rows(not select column)
        /// </summary>
        /// <param name="indexRow">index of row</param>
        /// <returns>True or false</returns>
        private bool IsSelectedRow(int indexRow)
        {
            if (this._bsprDataSelectedRange.Row <= indexRow
                && indexRow <= this._bsprDataSelectedRange.Row + this._bsprDataSelectedRange.RowCount - 1)
                return true;

            return false;
        }

        public void bsprData_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (!e.ColumnHeader && !e.RowHeader)
            {
                this.SetPropertyModelInfomation(e.Row);

                if (e.Button == MouseButtons.Right)
                {
                    this.bsprData.ActiveSheet.SetActiveCell(e.Row, e.Column);
                }
                else if (e.Column == this._ColIdx_Activation)
                {
                    if (_bsprDataSelectedRange != null
                        && this.IsSelectedRow(e.Row))
                    {
                        // make temp array for rollback
                        _valueForActivation = new List<string>();

                        for (int j = 0; j < this._bsprDataSelectedRange.RowCount; j++)
                        {
                            _valueForActivation.Add(
                                this.bsprData.ActiveSheet.GetText(this._bsprDataSelectedRange.Row + j,
                                                                  this._ColIdx_Activation));
                        }
                    }
                }
            }
        }

        public void bsprData_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control)
                {
                    if (e.KeyCode == Keys.C)
                    {
                        this.bsprData.ActiveSheet.ClipboardCopy();
                    }
                    else if (e.KeyCode == Keys.V)
                    {
                        CellRange selectedRange = bsprData.ActiveSheet.GetSelection(0);
                        object[,] backupData = this._ComUtil.GetBackUpDataForPasting(this.bsprData, selectedRange.Row, selectedRange.Column);
                        if (selectedRange.Column + backupData.GetUpperBound(1) > this._ColIdx_LOWER_TECHNICAL_LIMIT)
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_PASTE_ONLY_LIMIT", null, null);
                            return;
                        }
                        this.bsprData.ActiveSheet.ClipboardPaste();
                        string sErrMsg = this.CheckBsprDataValidation(selectedRange);
                        if (sErrMsg.Length > 0)
                        {
                            this._ComUtil.Rollback(this.bsprData, selectedRange.Row, selectedRange.Column, backupData);
                            MSGHandler.DisplayMessage(MSGType.Information, sErrMsg);
                        }
                    }
                    else if (e.KeyCode == Keys.F)
                    {
                        if (this.bsprData.ContextMenu.MenuItems.Contains(fiterHide))
                        {
                            this.bsprData.SetFilterVisible(false);
                            _sfilter.Visible = false;
                            bsprData.ContextMenu.MenuItems.Remove(fiterHide);
                            fiterHide = new MenuItem("Filter_Hide");
                            fiterHide.Click += new EventHandler(filter_Click);
                            this.bsprData.ContextMenu.MenuItems.Add(filter);
                            
                        }
                        else if (this.bsprData.ContextMenu.MenuItems.Contains(filter))
                        {
                            this.bsprData.SetFilterVisible(false);
                            _sfilter.Visible = true;
                            bsprData.ContextMenu.MenuItems.Remove(filter);
                            fiterHide = new MenuItem("Filter_Hide");
                            fiterHide.Click += new EventHandler(filter_Hide_Click);
                            this.bsprData.ContextMenu.MenuItems.Add(fiterHide);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        private string CheckBsprDataValidation(CellRange range)
        {
            FarPoint.Win.Spread.SpreadView view = new FarPoint.Win.Spread.SpreadView(this.bsprData);
            FarPoint.Win.Spread.Model.CellRange cellRange = this.bsprData.ActiveSheet.GetSelection(0);
            string sErrMsg = "";
            for (int i = 0; i < cellRange.ColumnCount; i++)
            {
                for (int j = 0; j < cellRange.RowCount; j++)
                {
                    FarPoint.Win.Spread.ChangeEventArgs ce = new FarPoint.Win.Spread.ChangeEventArgs(view, cellRange.Row + j, cellRange.Column + i);
                    sErrMsg = this.bsprDataChange(this.bsprData, ce);
                    if (sErrMsg.Length > 0)
                    {
                        return sErrMsg;
                    }
                }
            }

            return sErrMsg;
        }

        public void bsprData_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string sErrMsg = bsprDataChange(sender, e);
            if (sErrMsg.Length > 0)
            {
                this._bsprDataValidate = false;
                e.View.CancelCellEditing();
                MSGHandler.DisplayMessage(MSGType.Information, sErrMsg);
            }
        }

        private string bsprDataChange(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string sErrMsg = "";
            double dUpper = 0;
            double dLower = 0;
            if (this.bsprData.ActiveSheet.RowCount > 0)
            {
                if (e.Column >= this._ColIdx_USL && e.Column <= this._ColIdx_LOWER_TECHNICAL_LIMIT)
                {
                    string sValue = this.bsprData.ActiveSheet.Cells[e.Row, e.Column].Text.Trim();
                    if (sValue.Length > 0)
                    {
                        double dValue = 0;
                        bool isDouble = double.TryParse(sValue, out dValue);
                        if (!isDouble)
                        {
                            sErrMsg = MSGHandler.GetMessage("SPC_INFO_INPUT_SPEC_LIMIT");
                            return sErrMsg;
                        }

                    }

                    string sUSL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_USL].Text.Trim();
                    string sUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MEAN_UCL].Text.Trim();
                    string sRawUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_RAW_UCL].Text.Trim();
                    string sLSL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_LSL].Text.Trim();
                    string sLCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MEAN_LCL].Text.Trim();
                    string sRawLCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_RAW_LCL].Text.Trim();

                    string sRangeUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_RANGE_UCL].Text.Trim();
                    string sRangeLCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_RANGE_LCL].Text.Trim();
                    string sSTDUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_STD_UCL].Text.Trim();
                    string sSTDLCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_STD_LCL].Text.Trim();
                    string sEWMAMEANUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_M_UCL].Text.Trim();
                    string sEWMAMEANLCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_M_LCL].Text.Trim();
                    string sEWMARANGEUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_R_UCL].Text.Trim();
                    string sEWMARANGELCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_R_LCL].Text.Trim();
                    string sEWMASTDUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_S_UCL].Text.Trim();
                    string sEWMASTDLCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_S_LCL].Text.Trim();
                    string sMAUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MA_UCL].Text.Trim();
                    string sMALCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MA_LCL].Text.Trim();
                    string sMSUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MS_UCL].Text.Trim();
                    string sMSLCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MS_LCL].Text.Trim();
                    string sMRUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MR_UCL].Text.Trim();
                    string sMRLCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MR_LCL].Text.Trim();
                    string sUTL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_UPPER_TECHNICAL_LIMIT].Text.Trim();
                    string sLTL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_LOWER_TECHNICAL_LIMIT].Text.Trim();


                    #region usl & ucl(raw,mean)

                    if (sUSL.Length > 0 && sUCL.Length > 0)
                    {
                        dUpper = double.Parse(sUSL);
                        dLower = double.Parse(sUCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[]{"USL", "MEAN UCL"});
                            return sErrMsg;
                        }
                    }

                    if (sUSL.Length > 0 && sRawUCL.Length > 0)
                    {
                        dUpper = double.Parse(sUSL);
                        dLower = double.Parse(sRawUCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[]{"USL", "RAW UCL"});
                            return sErrMsg;
                        }
                    }

                    #endregion

                    #region lsl & lcl(raw,mean)

                    if (sLSL.Length > 0 && sLCL.Length > 0)
                    {
                        dUpper = double.Parse(sLCL);
                        dLower = double.Parse(sLSL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[]{"MEAN LCL", "LSL"});
                            return sErrMsg;
                        }
                    }

                    if (sLSL.Length > 0 && sRawLCL.Length > 0)
                    {
                        dUpper = double.Parse(sRawLCL);
                        dLower = double.Parse(sLSL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[]{"RAW LCL", "LSL"});
                            return sErrMsg;
                        }
                    }
                    #endregion

                    #region lsl & ucl(raw,mean)
                    if (sLSL.Length > 0 && sUCL.Length > 0)
                    {
                        dUpper = double.Parse(sUCL);
                        dLower = double.Parse(sLSL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[]{"MEAN UCL", "LSL"});
                            return sErrMsg;
                        }
                    }

                    if (sLSL.Length > 0 && sRawUCL.Length > 0)
                    {
                        dUpper = double.Parse(sRawUCL);
                        dLower = double.Parse(sLSL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[]{"RAW UCL", "LSL"});
                            return sErrMsg;
                        }
                    }

                    #endregion

                    #region usl & lcl(raw,mean)
                    if (sUSL.Length > 0 && sLCL.Length > 0)
                    {
                        dUpper = double.Parse(sUSL);
                        dLower = double.Parse(sLCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[]{"USL", "MEAN LCL"});
                            return sErrMsg;
                        }
                    }

                    if (sUSL.Length > 0 && sRawLCL.Length > 0)
                    {
                        dUpper = double.Parse(sUSL);
                        dLower = double.Parse(sRawLCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "USL", "RAW LCL" });
                            return sErrMsg;
                        }
                    }
                    #endregion


                    if (sUSL.Length > 0 && sLSL.Length > 0)
                    {
                        dUpper = double.Parse(sUSL);
                        dLower = double.Parse(sLSL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[]{"USL", "LSL"});
                            return sErrMsg;
                        }
                        else
                        {
                            double dCenter = (dUpper + dLower) / 2;
                            this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_TARGET].Text = dCenter.ToString();
                        }
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_TARGET].Text = "";
                    }

                    if (sRawUCL.Length > 0 && sRawLCL.Length > 0)
                    {
                        dUpper = double.Parse(sRawUCL);
                        dLower = double.Parse(sRawLCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "Raw UCL", "Raw LCL" });
                            return sErrMsg;
                        }
                        else
                        {
                            double dCenter = (dUpper + dLower) / 2;
                            this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_RAW_CENTER_LINE].Text = dCenter.ToString();
                        }
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_RAW_CENTER_LINE].Text = "";
                    }

                    if (sUCL.Length > 0 && sLCL.Length > 0)
                    {
                        dUpper = double.Parse(sUCL);
                        dLower = double.Parse(sLCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "Mean UCL", "Mean LCL" });
                            return sErrMsg;
                        }
                        else
                        {
                            double dCenter = (dUpper + dLower) / 2;
                            this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_CENTER_LINE].Text = dCenter.ToString();
                        }
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_CENTER_LINE].Text = "";
                    }

                    if (sSTDUCL.Length > 0 && sSTDLCL.Length > 0)
                    {
                        dUpper = double.Parse(sSTDUCL);
                        dLower = double.Parse(sSTDLCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "STD UCL", "STD LCL" });
                            return sErrMsg;
                        }
                        else
                        {
                            double dCenter = (dUpper + dLower) / 2;
                            this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_STD_CENTER_LINE].Text = dCenter.ToString();
                        }
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_STD_CENTER_LINE].Text = "";
                    }

                    if (sRangeUCL.Length > 0 && sRangeLCL.Length > 0)
                    {
                        dUpper = double.Parse(sRangeUCL);
                        dLower = double.Parse(sRangeLCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "Range UCL", "Range LCL" });
                            return sErrMsg;
                        }
                        else
                        {
                            double dCenter = (dUpper + dLower) / 2;
                            this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_RANGE_CENTER_LINE].Text = dCenter.ToString();
                        }
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_RANGE_CENTER_LINE].Text = "";
                    }

                    if (sEWMAMEANUCL.Length > 0 && sEWMAMEANLCL.Length > 0)
                    {
                        dUpper = double.Parse(sEWMAMEANUCL);
                        dLower = double.Parse(sEWMAMEANLCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "EWMA(Mean) UCL", "EWMA(Mean) LCL" });
                            return sErrMsg;
                        }
                        else
                        {
                            double dCenter = (dUpper + dLower) / 2;
                            this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_M_CENTER_LINE].Text = dCenter.ToString();
                        }
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_M_CENTER_LINE].Text = "";
                    }

                    if (sEWMASTDUCL.Length > 0 && sEWMASTDLCL.Length > 0)
                    {
                        dUpper = double.Parse(sEWMASTDUCL);
                        dLower = double.Parse(sEWMASTDLCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "EWMA(STD) UCL", "EWMA(STD) LCL" });
                            return sErrMsg;
                        }
                        else
                        {
                            double dCenter = (dUpper + dLower) / 2;
                            this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_S_CENTER_LINE].Text = dCenter.ToString();
                        }
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_S_CENTER_LINE].Text = "";
                    }

                    if (sEWMARANGEUCL.Length > 0 && sEWMARANGELCL.Length > 0)
                    {
                        dUpper = double.Parse(sEWMARANGEUCL);
                        dLower = double.Parse(sEWMARANGELCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "EWMA(Range) UCL", "EWMA(Range) LCL" });
                            return sErrMsg;
                        }
                        else
                        {
                            double dCenter = (dUpper + dLower) / 2;
                            this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_R_CENTER_LINE].Text = dCenter.ToString();
                        }
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_EWMA_R_CENTER_LINE].Text = "";
                    }

                    if (sMAUCL.Length > 0 && sMALCL.Length > 0)
                    {
                        dUpper = double.Parse(sMAUCL);
                        dLower = double.Parse(sMALCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "MA UCL", "MA LCL" });
                            return sErrMsg;
                        }
                        else
                        {
                            double dCenter = (dUpper + dLower) / 2;
                            this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MA_CENTER_LINE].Text = dCenter.ToString();
                        }
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MA_CENTER_LINE].Text = "";
                    }

                    if (sMSUCL.Length > 0 && sMSLCL.Length > 0)
                    {
                        dUpper = double.Parse(sMSUCL);
                        dLower = double.Parse(sMSLCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "MS UCL", "MS LCL" });
                            return sErrMsg;
                        }
                        else
                        {
                            double dCenter = (dUpper + dLower) / 2;
                            this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MS_CENTER_LINE].Text = dCenter.ToString();
                        }
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MS_CENTER_LINE].Text = "";
                    }

                    if (sMRUCL.Length > 0 && sMRLCL.Length > 0)
                    {
                        dUpper = double.Parse(sMRUCL);
                        dLower = double.Parse(sMRLCL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "MR UCL", "MR LCL" });
                            return sErrMsg;
                        }
                        else
                        {
                            double dCenter = (dUpper + dLower) / 2;
                            this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MR_CENTER_LINE].Text = dCenter.ToString();
                        }
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_MR_CENTER_LINE].Text = "";
                    }

                    #region usl & utl

                    if (sUSL.Length > 0 && sUTL.Length > 0)
                    {
                        dUpper = double.Parse(sUSL);
                        dLower = double.Parse(sUTL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "USL", "UTL" });
                            return sErrMsg;
                        }
                    }

                    #endregion

                    #region lsl & ltl

                    if (sLSL.Length > 0 && sLTL.Length > 0)
                    {
                        dUpper = double.Parse(sLTL);
                        dLower = double.Parse(sLSL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "LTL", "LSL" });
                            return sErrMsg;
                        }
                    }

                    #endregion

                    //Technical Limit
                    if (sUTL.Length > 0 && sLTL.Length > 0)
                    {
                        dUpper = double.Parse(sUTL);
                        dLower = double.Parse(sLTL);

                        if (dUpper < dLower)
                        {
                            sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] { "UTL", "LTL" });
                            return sErrMsg;
                        }
                    }

                    if (this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_CENTER_LINE].Text.Length > 0 && this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_STD].Text.Length > 0)
                    {
                        double iUpper_ZONEA = double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_CENTER_LINE].Text) + 3 * double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_STD].Text);
                        double iLower_ZONEA = double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_CENTER_LINE].Text) - 3 * double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_STD].Text);
                        double iUpper_ZONEB = double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_CENTER_LINE].Text) + 2 * double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_STD].Text);
                        double iLower_ZONEB = double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_CENTER_LINE].Text) - 2 * double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_STD].Text);
                        double iUpper_ZONEC = double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_CENTER_LINE].Text) + double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_STD].Text);
                        double iLower_ZONEC = double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_CENTER_LINE].Text) - double.Parse(this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_STD].Text);

                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_ZONE_A_UCL].Text = iUpper_ZONEA.ToString();
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_ZONE_A_LCL].Text = iLower_ZONEA.ToString();
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_ZONE_B_UCL].Text = iUpper_ZONEB.ToString();
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_ZONE_B_LCL].Text = iLower_ZONEB.ToString();
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_ZONE_C_UCL].Text = iUpper_ZONEC.ToString();
                        this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_ZONE_C_LCL].Text = iLower_ZONEC.ToString();
                    }

                    return sErrMsg;
                }
                else if (e.Column == _ColIdx_Activation)
                {
                    if (_bsprDataSelectedRange == null
                        || !this.IsSelectedRow(e.Row))
                    {
                        string msg = CheckActivation(e.Row);
                        if (string.Empty != msg)
                        {
                            _valueForActivation = null;
                            return msg;
                        }
                    }
                    else
                    {
                        // validation
                        for (int i = 0; i < _bsprDataSelectedRange.RowCount; i++)
                        {
                            string msg = CheckActivation(_bsprDataSelectedRange.Row + i);
                            if (string.Empty != msg)
                            {
                                // rollback
                                for (int j = 0; j < this._bsprDataSelectedRange.RowCount; j++)
                                {
                                    this.bsprData.SetCellText(j + this._bsprDataSelectedRange.Row,
                                                              this._ColIdx_Activation, this._valueForActivation[j]);
                                    this.bsprData.SetCellValue(j + this._bsprDataSelectedRange.Row,
                                                              this._ColIdx_Activation, this._valueForActivation[j]);
                                }

                                return msg;
                            }
                        }
                    }

                    return sErrMsg;
                }
            }

            return sErrMsg;
        }

        public void bsprData_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            this._bsprDataValidate = true;
        }




        #region ::: Generator Property Data

        private void SetPropertyModelInfomation(int iRowIdx)
        {
            try
            {
                if (this.bsprData.ActiveSheet.RowCount == 0 || iRowIdx < 0)
                {
                    Panel pnlDumy = new Panel();
                    pnlDumy.Size = new Size(250, 500);
                    this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", pnlDumy, false);
                    return;
                }

                string sConfigRawID = this.bsprData.GetCellText(iRowIdx, (int)iColIdx.CHART_ID);


                DataTable dtConfig = _dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
                DataTable dtContext = _dsSPCModeData.Tables[TABLE.MODEL_CONTEXT_MST_SPC];
                DataTable dtRule = _dsSPCModeData.Tables[TABLE.MODEL_RULE_MST_SPC];
                DataTable dtRuleOpt = _dsSPCModeData.Tables[TABLE.MODEL_RULE_OPT_MST_SPC];

                DataRow[] drConfigs = dtConfig.Select(string.Format("{0} = '{1}'", COLUMN.RAWID, sConfigRawID));
                DataRow drConfig = drConfigs[0];
                DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, sConfigRawID));
                DataRow[] drRules = dtRule.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, sConfigRawID));


                //# Category Name 정의
                string sCategoryContext = "1.Context";
                string sCategoryParameter = "2.Parameter";
                string sCategoryMasterOption = "3.Master Option";
                string sCategoryAppliedRule = "4.Applied Rule List";


                //#01.SPCModelInfo Class 생성
                DynamicCodeGenerator dynamicCodeGen = new DynamicCodeGenerator("BISTel.eSPC.Page.Modeling", "SPCModelInfo");

                dynamicCodeGen.AddReference("System.dll");

                dynamicCodeGen.AddImports("System");
                dynamicCodeGen.AddImports("System.ComponentModel");
                dynamicCodeGen.AddImports("System.Globalization");


                //#02. Expandable Converter Class 생성 후 SPCModelInfo.cs 파일의 Namespace내에 붙임
                DynamicCodeGenerator dcGenExConverter = new DynamicCodeGenerator("BISTel.eSPC.Page.Modeling", "ExpandableConverter", "ExpandableObjectConverter");

                CodeParameterDeclarationExpression exParam1 = new CodeParameterDeclarationExpression(typeof(ITypeDescriptorContext), "context");
                CodeParameterDeclarationExpression exParam2 = new CodeParameterDeclarationExpression(typeof(CultureInfo), "culture");
                CodeParameterDeclarationExpression exParam3 = new CodeParameterDeclarationExpression(typeof(object), "value");
                CodeParameterDeclarationExpression exParam4 = new CodeParameterDeclarationExpression(typeof(Type), "destinationType");

                dcGenExConverter.AddMethod(MemberAttributes.Public | MemberAttributes.Override, "ConvertTo", typeof(object), "return null;", exParam1, exParam2, exParam3, exParam4);

                dynamicCodeGen.AddClass(dcGenExConverter.GetClass());


                //#03.Context Property 생성
                string sVariableType = string.Empty;
                string sVariableName = string.Empty;
                string sVariableValue = string.Empty;
                string sPropertyName = string.Empty;
                string sPropertyDisplayName = string.Empty;
                string sPropertyDescription = string.Empty;

                int iContextIdx = 1;

                foreach (DataRow drContext in drContexts)
                {
                    sPropertyName = drContext[COLUMN.CONTEXT_KEY].ToString();
                    //sPropertyDisplayName = string.Format("{0}.{1}", iContextIdx, drContext[COLUMN.CONTEXT_KEY]);
                    sPropertyDisplayName = drContext["CONTEXT_KEY_NAME"].ToString();

                    sVariableName = string.Format("_{0}", drContext[COLUMN.CONTEXT_KEY]);
                    sVariableValue = drContext[COLUMN.CONTEXT_VALUE].ToString();

                    dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                    dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryContext, "", true);

                    iContextIdx++;
                }


                //#04. Parameter Property 생성
                //Param Name
                sVariableName = string.Format("_{0}", COLUMN.PARAM_ALIAS);
                sVariableValue = drConfig[COLUMN.PARAM_ALIAS].ToString();
                sPropertyName = COLUMN.PARAM_ALIAS;
                sPropertyDisplayName = "Parameter Alias";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryParameter, "", true);

                //Param Type
                sVariableName = string.Format("_{0}", COLUMN.PARAM_TYPE_CD);
                sVariableValue = drConfig[COLUMN.PARAM_TYPE].ToString();
                sPropertyName = COLUMN.PARAM_TYPE_CD;
                sPropertyDisplayName = "Parameter Type";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryParameter, "", true);

                //Complex
                sVariableName = string.Format("_{0}", COLUMN.COMPLEX_YN);
                sVariableValue = drConfig[COLUMN.COMPLEX_YN].ToString();
                sPropertyName = COLUMN.COMPLEX_YN;
                sPropertyDisplayName = "Complex Type";

                if (sVariableValue.Equals("Y"))
                    dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(Boolean), new CodeSnippetExpression("true"));
                else
                    dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(Boolean), new CodeSnippetExpression("false"));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(Boolean), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryParameter, "", true);

                //#05. Master Option
                //Upper Spec
                sVariableName = string.Format("_{0}", COLUMN.UPPER_SPEC);
                sVariableValue = drConfig[COLUMN.UPPER_SPEC].ToString();
                sPropertyName = COLUMN.UPPER_SPEC;
                sPropertyDisplayName = "Upper Spec";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //Lower Spec
                sVariableName = string.Format("_{0}", COLUMN.LOWER_SPEC);
                sVariableValue = drConfig[COLUMN.LOWER_SPEC].ToString();
                sPropertyName = COLUMN.LOWER_SPEC;
                sPropertyDisplayName = "Lower Spec";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //Center Line
                sVariableName = string.Format("_{0}", COLUMN.CENTER_LINE);
                sVariableValue = drConfig[COLUMN.CENTER_LINE].ToString();
                sPropertyName = COLUMN.CENTER_LINE;
                sPropertyDisplayName = "Center Line";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //Lambda
                sVariableName = string.Format("_{0}", COLUMN.EWMA_LAMBDA);
                sVariableValue = drConfig[COLUMN.EWMA_LAMBDA].ToString();
                sPropertyName = COLUMN.EWMA_LAMBDA;
                sPropertyDisplayName = "Lambda";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //Moving Count
                sVariableName = string.Format("_{0}", COLUMN.MOVING_COUNT);
                sVariableValue = drConfig[COLUMN.MOVING_COUNT].ToString();
                sPropertyName = COLUMN.MOVING_COUNT;
                sPropertyDisplayName = "Moving Count";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //Upper Control
                sVariableName = string.Format("_{0}", COLUMN.UPPER_CONTROL);
                sVariableValue = drConfig[COLUMN.UPPER_CONTROL].ToString();
                sPropertyName = COLUMN.UPPER_CONTROL;
                sPropertyDisplayName = "Mean UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //Lower Control
                sVariableName = string.Format("_{0}", COLUMN.LOWER_CONTROL);
                sVariableValue = drConfig[COLUMN.LOWER_CONTROL].ToString();
                sPropertyName = COLUMN.LOWER_CONTROL;
                sPropertyDisplayName = "Mean LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //Raw UCL
                sVariableName = string.Format("_{0}", COLUMN.RAW_UCL);
                sVariableValue = drConfig[COLUMN.RAW_UCL].ToString();
                sPropertyName = COLUMN.RAW_UCL;
                sPropertyDisplayName = "Raw UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //Raw LCL
                sVariableName = string.Format("_{0}", COLUMN.RAW_LCL);
                sVariableValue = drConfig[COLUMN.RAW_LCL].ToString();
                sPropertyName = COLUMN.RAW_LCL;
                sPropertyDisplayName = "Raw LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //STD UCL
                sVariableName = string.Format("_{0}", COLUMN.STD_UCL);
                sVariableValue = drConfig[COLUMN.STD_UCL].ToString();
                sPropertyName = COLUMN.STD_UCL;
                sPropertyDisplayName = "STD UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //STD LCL
                sVariableName = string.Format("_{0}", COLUMN.STD_LCL);
                sVariableValue = drConfig[COLUMN.STD_LCL].ToString();
                sPropertyName = COLUMN.STD_LCL;
                sPropertyDisplayName = "STD LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //Range UCL
                sVariableName = string.Format("_{0}", COLUMN.RANGE_UCL);
                sVariableValue = drConfig[COLUMN.RANGE_UCL].ToString();
                sPropertyName = COLUMN.RANGE_UCL;
                sPropertyDisplayName = "Range UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //Range LCL
                sVariableName = string.Format("_{0}", COLUMN.RANGE_LCL);
                sVariableValue = drConfig[COLUMN.RANGE_LCL].ToString();
                sPropertyName = COLUMN.RANGE_LCL;
                sPropertyDisplayName = "Range LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //EWMA(Mean) UCL
                sVariableName = string.Format("_{0}", COLUMN.EWMA_M_UCL);
                sVariableValue = drConfig[COLUMN.EWMA_M_UCL].ToString();
                sPropertyName = COLUMN.EWMA_M_UCL;
                sPropertyDisplayName = "EWMA(Mean) UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //EWMA(Mean) LCL
                sVariableName = string.Format("_{0}", COLUMN.EWMA_M_LCL);
                sVariableValue = drConfig[COLUMN.EWMA_M_LCL].ToString();
                sPropertyName = COLUMN.EWMA_M_LCL;
                sPropertyDisplayName = "EWMA(Mean) LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //EWMA(Range) UCL
                sVariableName = string.Format("_{0}", COLUMN.EWMA_R_UCL);
                sVariableValue = drConfig[COLUMN.EWMA_R_UCL].ToString();
                sPropertyName = COLUMN.EWMA_R_UCL;
                sPropertyDisplayName = "EWMA(Range) UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //EWMA(Range) LCL
                sVariableName = string.Format("_{0}", COLUMN.EWMA_R_LCL);
                sVariableValue = drConfig[COLUMN.EWMA_R_LCL].ToString();
                sPropertyName = COLUMN.EWMA_R_LCL;
                sPropertyDisplayName = "EWMA(Range) LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //EWMA(STD) UCL
                sVariableName = string.Format("_{0}", COLUMN.EWMA_S_UCL);
                sVariableValue = drConfig[COLUMN.EWMA_S_UCL].ToString();
                sPropertyName = COLUMN.EWMA_S_UCL;
                sPropertyDisplayName = "EWMA(STD) UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //EWMA(STD) LCL
                sVariableName = string.Format("_{0}", COLUMN.EWMA_S_LCL);
                sVariableValue = drConfig[COLUMN.EWMA_S_LCL].ToString();
                sPropertyName = COLUMN.EWMA_S_LCL;
                sPropertyDisplayName = "EWMA(STD) LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //MA UCL
                sVariableName = string.Format("_{0}", COLUMN.MA_UCL);
                sVariableValue = drConfig[COLUMN.MA_UCL].ToString();
                sPropertyName = COLUMN.MA_UCL;
                sPropertyDisplayName = "MA UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //MA LCL
                sVariableName = string.Format("_{0}", COLUMN.MA_LCL);
                sVariableValue = drConfig[COLUMN.MA_LCL].ToString();
                sPropertyName = COLUMN.MA_LCL;
                sPropertyDisplayName = "MA LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //MS UCL
                sVariableName = string.Format("_{0}", COLUMN.MS_UCL);
                sVariableValue = drConfig[COLUMN.MS_UCL].ToString();
                sPropertyName = COLUMN.MS_UCL;
                sPropertyDisplayName = "MS UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //MS LCL
                sVariableName = string.Format("_{0}", COLUMN.MS_LCL);
                sVariableValue = drConfig[COLUMN.MS_LCL].ToString();
                sPropertyName = COLUMN.MS_LCL;
                sPropertyDisplayName = "MS LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //MR UCL
                sVariableName = string.Format("_{0}", COLUMN.MR_UCL);
                sVariableValue = drConfig[COLUMN.MR_UCL].ToString();
                sPropertyName = COLUMN.MR_UCL;
                sPropertyDisplayName = "MR UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //MS LCL
                sVariableName = string.Format("_{0}", COLUMN.MR_LCL);
                sVariableValue = drConfig[COLUMN.MR_LCL].ToString();
                sPropertyName = COLUMN.MR_LCL;
                sPropertyDisplayName = "MR LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);


                //UPPER_TECHNICAL_LIMIT
                sVariableName = string.Format("_{0}", COLUMN.UPPER_TECHNICAL_LIMIT);
                sVariableValue = drConfig[COLUMN.UPPER_TECHNICAL_LIMIT].ToString();
                sPropertyName = COLUMN.UPPER_TECHNICAL_LIMIT;
                sPropertyDisplayName = "UTL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);


                //LOWER_TECHNICAL_LIMIT
                sVariableName = string.Format("_{0}", COLUMN.LOWER_TECHNICAL_LIMIT);
                sVariableValue = drConfig[COLUMN.LOWER_TECHNICAL_LIMIT].ToString();
                sPropertyName = COLUMN.LOWER_TECHNICAL_LIMIT;
                sPropertyDisplayName = "LTL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);


                //STD
                sVariableName = string.Format("_{0}", COLUMN.STD);
                sVariableValue = drConfig[COLUMN.STD].ToString();
                sPropertyName = COLUMN.STD;
                sPropertyDisplayName = "STD";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //#06. Rule Property
                foreach (DataRow drRule in drRules)
                {
                    //Rule Property
                    sVariableType = string.Format("RuleClass{0}", drRule[COLUMN.SPC_RULE_NO]);
                    sVariableName = string.Format("_RuleClass{0}", drRule[COLUMN.SPC_RULE_NO]);
                    sVariableValue = drRule[COLUMN.SPC_RULE_NO].ToString();
                    sPropertyName = string.Format("Rule{0}", drRule[COLUMN.SPC_RULE_NO]);
                    sPropertyDisplayName = string.Format("Rule{0}", drRule[COLUMN.SPC_RULE_NO]);
                    sPropertyDescription = drRule[COLUMN.DESCRIPTION].ToString();

                    dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, sVariableType, new CodeSnippetExpression(string.Format("new {0}()", sVariableType)));
                    dynamicCodeGen.AddProperty(MemberAttributes.Public, sVariableType, sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryAppliedRule, sPropertyDescription, true);


                    DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_RULE_RAWID, drRule[COLUMN.RAWID]), COLUMN.RULE_OPTION_NO);

                    //Rule Class
                    DynamicCodeGenerator dcGenRuleClass = new DynamicCodeGenerator("BISTel.eSPC.Page.Modeling", sVariableType);
                    dcGenRuleClass.AddClassAttribute("TypeConverter", new CodeSnippetExpression("typeof(ExpandableConverter)"));

                    foreach (DataRow drRuleOpt in drRuleOpts)
                    {
                        sVariableName = string.Format("_RuleOption{0}", drRuleOpt[COLUMN.RULE_OPTION_NO]);
                        sVariableValue = drRuleOpt[COLUMN.RULE_OPTION_VALUE].ToString();
                        sPropertyName = string.Format("RuleOption{0}", drRuleOpt[COLUMN.RULE_OPTION_NO]);
                        sPropertyDisplayName = drRuleOpt[COLUMN.OPTION_NAME].ToString();
                        sPropertyDescription = drRuleOpt[COLUMN.DESCRIPTION].ToString();

                        dcGenRuleClass.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                        dcGenRuleClass.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, "", sPropertyDescription, true);
                    }

                    dynamicCodeGen.AddClass(dcGenRuleClass.GetClass());
                }

                dynamicCodeGen.GeneratorCode();
                //dynamicCodeGen.Code
                object ob = dynamicCodeGen.GetInstance();

                PropertyGrid propertyGrid = new PropertyGrid();
                propertyGrid.CategoryForeColor = Color.Green;
                propertyGrid.ToolbarVisible = false;
                //propertyGrid.HelpVisible = false;
                propertyGrid.PropertySort = PropertySort.Categorized;
                propertyGrid.Size = new Size(250, 500);
                propertyGrid.SelectedObject = ob;
                propertyGrid.ExpandAllGridItems();

                //this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", propertyGrid, false);

                this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", propertyGrid, true, new Rectangle(Screen.PrimaryScreen.WorkingArea.Width - 250, 100, 250, 500), EESConstants.InformationDock.Right, EESConstants.InformationDock.Left);                
            }
            catch (Exception ex)
            {
                Panel pnlDumy = new Panel();
                pnlDumy.Size = new Size(250, 500);
                this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", pnlDumy, false);
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
        }

        #endregion





        void bbtnCopyModel_Click(object sender, EventArgs e)
        {
            if (this._popUp != null)
            {
                this._popUp.ShowDialog();
            }
        }

        void copyModel_Click(object sender, EventArgs e)
        {
            if (this.bsprData.ActiveSheet.ActiveCell == null)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                return;
            }

            if (this._popUp == null)
            {
                this._popUp = new SPCCopySpec();
                this._popUp.InitializeControl();
            }

            int selectedRow = this.bsprData.ActiveSheet.ActiveRowIndex;
            string configRawID = this.bsprData.ActiveSheet.Cells[selectedRow, (int)iColIdx.CHART_ID].Text;
            string mainYN = this.bsprData.ActiveSheet.Cells[selectedRow, (int)iColIdx.MAIN_YN].Text;
            this._popUp.AREA = this._sArea;

            this._popUp.SetCopyModelInfo(this._sEQPModel, this._sSPCModelName, mainYN, configRawID, this.btxtParamAlias.Text);
            DialogResult resultMessage = this._popUp.ShowDialog();

            if (resultMessage == DialogResult.OK)
            {
                //spc-1198
                //this.bbtnCopyModel.Visible = true;

                if (!copyClicked)
                {
                    copyClicked = true;
                    this.bsprData.ContextMenu.MenuItems.Add(this.bsprData.ContextMenu.MenuItems.Count, new MenuItem(_pasteModel, pasteModel_Click));
                }
            }
            else
            {
                if (this._popUp.CONFIG_RAWID.Trim().Length < 1)
                {
                    this.bbtnCopyModel.Visible = false;
                }
            }
        }

        //SPC-854 louis you - ContextMenu 추가 ( filter, FilterHide,freeze,unfreeze)
                
        void filter_Click(object sender, EventArgs e)
        {
            this.bsprData.SetFilterVisible(false);
            _sfilter.Visible = true;
            filterClicked = true;
            if (filterClicked)
            {
                fiterHide = new MenuItem("Filter_Hide");
                fiterHide.Click += new EventHandler(filter_Hide_Click);
                this.bsprData.ContextMenu.MenuItems.Add(fiterHide);
                this.bsprData.ContextMenu.MenuItems.Remove(filter);
            }
        }
        void filter_Hide_Click(object sender, EventArgs e)
        {
            this.bsprData.SetFilterVisible(false);
            _sfilter.Visible = false;
            filterClicked = false;
            if (!filterClicked)
            {
                bsprData.ContextMenu.MenuItems.Remove(fiterHide);
                bsprData.ContextMenu.MenuItems.Add(filter);
            }
        }
        int freeze_Count = 0;
        void freeze_Click(object sender, EventArgs e)
        {
            
            
            if (bsprData.ActiveSheet.ColumnCount <= 0)
                return;
            if (this.bsprData.ActiveSheet.ActiveColumnIndex >= 0)
            {
                bsprData.ActiveSheet.FrozenColumnCount = bsprData.ActiveSheet.ActiveColumnIndex;

                _sfilter.SetColumnFreeze(this.bsprData.ActiveSheet.ActiveColumnIndex);
                freeze_Count += 1;
            }
           

            unfreeze = new MenuItem("UnFreeze");
            unfreeze.Click +=new EventHandler(unfreeze_Click);
            if (freeze_Count == 1)
            {
                this.bsprData.ContextMenu.MenuItems.Add(unfreeze);
            }

            this.bsprData.Refresh();
        }
        
        void unfreeze_Click(object sender, EventArgs e)
        {
            
            bsprData.ActiveSheet.FrozenColumnCount = 0;
            _sfilter.SetColumnFreeze(0);

            bsprData.ContextMenu.MenuItems.Remove(unfreeze);            
            freeze_Count = 0;           
            
        }
        //SPC-854 end
        ///////////////////////////////////////
     
        //spc-779 by stella
        private void ColumnMappingByItems()
        {
            _arrColConfig = new ArrayList();
            _arrColContext = new ArrayList();
            _arrColOption = new ArrayList();
            _arrColAutoCalc = new ArrayList();
            _arrColRule = new ArrayList();
            _arrColLimit = new ArrayList();

            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_INTERLOCK);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_USE_EXTERNAL_SPEC_LIMIT);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_AUTO_CALCULATION);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_AUTO_GENERATE_SUB_CHART);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_ACTIVE);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_SAMPLE_COUNT);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_MANAGE_TYPE);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_AUTO_SETTING);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_USE_NORMALIZATION_VALUE);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_INHERIT_THE_SPEC_OF_MAIN);
            _arrColConfig.Add(Definition.COPY_MODEL.CONTEXT_MODE);

            //_arrColContext.Add(Definition.COPY_MODEL.CONTEXT_CHART_DESCRIPTION);

            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_RAW);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MEAN);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_STD);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_RANGE);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_EWMA_MEAN);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_EWMA_STD);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_EWMA_RANGE);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MA);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MS);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MR);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_TECHNICAL_LIMIT);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MOVING_OPTIONS);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_MEAN_OPTIONS);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_ZONE_A);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_ZONE_B);
            _arrColLimit.Add(Definition.COPY_MODEL.RULE_ZONE_C);
            _arrColRule.Add(Definition.COPY_MODEL.RULE_RULE_SELECTION);

            _arrColOption.Add(Definition.COPY_MODEL.OPTION_PARAMETER_CATEGORY);
            _arrColOption.Add(Definition.COPY_MODEL.OPTION_CALCULATE_PPK);
            _arrColOption.Add(Definition.COPY_MODEL.OPTION_PRIORITY);
            _arrColOption.Add(Definition.COPY_MODEL.OPTION_SAMPLE_COUNTS);
            _arrColOption.Add(Definition.COPY_MODEL.OPTION_DAYS);
            _arrColOption.Add(Definition.COPY_MODEL.OPTION_DEFAULT_CHART_TO_SHOW);

            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_PERIOD);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_COUNT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MINIMUM_SAMPLES_TO_USE);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_DEFAULT_PERIOD);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MAXIMUM_PERIOD_TO_USE);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_TO_USE);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_THREASHOLD);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_CALCULATION_WITH_SHIFT_COMPENSATION);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_CALCULATION_WITHOUT_IQR_FILTER);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_RAW_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MEAN_CONTROL_LIMIT); ;
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_STD_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_RANGE_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_EWMA_MEAN_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_EWMA_STD_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_EWMA_RANGE_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MA_CONTROL_LIMIT);
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MS_CONTROL_LIMIT);

            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_CALCULATION_THRESHOLD_OFF_YN);

            //spc-1155 by stella
            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_MR_CONTROL_LIMIT);

            _arrColAutoCalc.Add(Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN);

        }

        void pasteModel_Click(object sender, EventArgs e)
        {
            if (this._popUp.CONFIG_RAWID.Trim().Length < 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_PASTE_SPC_MODEL", null, null);
                return;
            }

            ArrayList alCheckRowIndex = this.bsprData.GetCheckedList((int)iColIdx.SELECT);

            if (alCheckRowIndex.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_TARGET_MODEL", null, null);
                return;
            }

            if (this._popUp != null)
                this._popUp.InitializePopup();

            //SPC-855 by Louis ==> SpecLimit Check (raw,mean,master)
            if (_popUp.RULE_MASTER_SPEC_LIMIT.ToString().Equals("Y") ||
                _popUp.RULE_MEAN.ToString().Equals("Y") || _popUp.RULE_RAW.ToString().Equals("Y"))
            {
                LinkedList toraltargetRawidList = new LinkedList();
                string[] targetRawid = new string[alCheckRowIndex.Count];
                for (int i = 0; i < alCheckRowIndex.Count; i++)
                {
                    LinkedList targetRawidList = new LinkedList();
                    int selectedRow = (int)alCheckRowIndex[i];
                    targetRawid[i] = this.bsprData.ActiveSheet.Cells[selectedRow, (int)iColIdx.CHART_ID].Text;
                }
                DataSet targetSpecData = _wsSPC.GetTargetConfigSpecData(targetRawid);
                DataSet sourceSpecData = _wsSPC.GetSourseConfigSpecData(_popUp.CONFIG_RAWID.ToString());

                if (targetSpecData != null && sourceSpecData != null)
                {
                    bool CompareResult = true;
                    if (_popUp.RULE_MASTER_SPEC_LIMIT.ToString().Equals("Y") && _popUp.RULE_RAW.ToString().Equals("Y") && _popUp.RULE_MEAN.ToString().Equals("N"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "111");
                    }
                    else if (_popUp.RULE_MASTER_SPEC_LIMIT.ToString().Equals("Y") && _popUp.RULE_RAW.ToString().Equals("Y") && _popUp.RULE_MEAN.ToString().Equals("N"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "110");
                    }
                    else if (_popUp.RULE_MASTER_SPEC_LIMIT.ToString().Equals("Y") && _popUp.RULE_RAW.ToString().Equals("N") && _popUp.RULE_MEAN.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "101");
                    }
                    else if (_popUp.RULE_MASTER_SPEC_LIMIT.ToString().Equals("Y") && _popUp.RULE_RAW.ToString().Equals("N") && _popUp.RULE_MEAN.ToString().Equals("N"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "100");
                    }
                    else if (_popUp.RULE_MASTER_SPEC_LIMIT.ToString().Equals("N") && _popUp.RULE_RAW.ToString().Equals("Y") && _popUp.RULE_MEAN.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "011");
                    }
                    else if (_popUp.RULE_MASTER_SPEC_LIMIT.ToString().Equals("N") && _popUp.RULE_RAW.ToString().Equals("Y") && _popUp.RULE_MEAN.ToString().Equals("N"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "010");
                    }
                    else if (_popUp.RULE_MASTER_SPEC_LIMIT.ToString().Equals("N") && _popUp.RULE_RAW.ToString().Equals("N") && _popUp.RULE_MEAN.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "001");
                    }
                    if (!CompareResult)
                    {
                        return;
                    }
                }

            }

            StringBuilder sbResult = new StringBuilder();
            string sourceConfigRawID = this._popUp.CONFIG_RAWID;

            LinkedList llCopyModel = new LinkedList();
            LinkedList llstTotalConfigInfo = new LinkedList();
            ArrayList arrTotalConfigList = new ArrayList();

            if (this._popUp != null)
            {
                for (int i = 0; i < alCheckRowIndex.Count; i++)
                {
                    int selectedRow = (int)alCheckRowIndex[i];
                    string targetConfigRawID =
                        this.bsprData.ActiveSheet.Cells[selectedRow, (int)iColIdx.CHART_ID].Text;
                    string mainYN = this.bsprData.ActiveSheet.Cells[selectedRow, (int)iColIdx.MAIN_YN].Text;

                    bool hasSubConfigs = false;
                    if (this.bsprData.ActiveSheet.RowCount > 1) //SubConfig 존재여부
                        hasSubConfigs = true;

                    llCopyModel = this.GetCopyModelList(mainYN, hasSubConfigs, sourceConfigRawID, targetConfigRawID);
                    arrTotalConfigList.Add(llCopyModel);
                }

                string changedItems = llCopyModel[COLUMN.CHANGED_ITEMS].ToString();

                SPCModelSavePopup popup = new SPCModelSavePopup(ModifyMode.COPY, "");
                popup.InitializeControl();
                popup.CHANGED_ITEMS = changedItems;
                DialogResult result = popup.ShowDialog();

                if (result == DialogResult.Yes)
                {
                    for (int i = 0; i < alCheckRowIndex.Count; i++)
                    {
                        ((LinkedList)arrTotalConfigList[i]).Add(COLUMN.SAVE_COMMENT, popup.COMMENT);
                        llstTotalConfigInfo.Add(i, arrTotalConfigList[i]);
                    }

                    DataSet dsResult = _wsSPC.CopyModelInfo(llstTotalConfigInfo.GetSerialData());

                    if (dsResult == null)
                    {
                        sbResult.AppendLine("There is no item to paste.");
                    }
                    else if (DSUtil.GetResultSucceed(dsResult) == 0)
                    {
                        sbResult.AppendLine(MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
                    }

                    this.ConfigListDataBinding();

                    Panel pnlDumy = new Panel();
                    pnlDumy.Size = new Size(250, 500);
                    this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage,
                                               "SPC Model Property", pnlDumy, false);

                    this.MsgClose();

                    if (sbResult.Length < 1)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information,
                                                  MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, sbResult.ToString());
                    }
                }
                
            }

        }

        private LinkedList GetCopyModelList(string mainYN, bool hasSubConfigs, string sourceConfigRawID, string targetConfigRawID)
        {
            LinkedList llstConfigurationInfo = new LinkedList();
            #region setting configuration info.

            llstConfigurationInfo.Add(Definition.DynamicCondition_Condition_key.USER_ID,
                                      this.sessionData.UserRawID);

            llstConfigurationInfo.Add(Definition.CONDITION_KEY_MAIN_YN, mainYN);
            llstConfigurationInfo.Add(Definition.CONDITION_KEY_HAS_SUBCONFIG, hasSubConfigs);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.SOURCE_MODEL_CONFIG_RAWID, sourceConfigRawID);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.TARGET_MODEL_CONFIG_RAWID, targetConfigRawID);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_INTERLOCK,
                                      this._popUp.CONTEXT_INTERLOCK);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_USE_EXTERNAL_SPEC_LIMIT,
                                      this._popUp.CONTEXT_USE_EXTERNAL_SPEC_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_CALCULATION,
                                      this._popUp.CONTEXT_AUTO_CALCULATION);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_GENERATE_SUB_CHART,
                                      this._popUp.CONTEXT_AUTO_GENERATE_SUB_CHART);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_ACTIVE, this._popUp.CONTEXT_ACTIVE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_SAMPLE_COUNT,
                                      this._popUp.CONTEXT_SAMPLE_COUNT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_MANAGE_TYPE,
                                      this._popUp.CONTEXT_MANAGE_TYPE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_SETTING,
                                      this._popUp.CONTEXT_AUTO_SETTING);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK,
                                      this._popUp.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION,
                                      this._popUp.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_USE_NORMALIZATION_VALUE,
                                      this._popUp.CONTEXT_USE_NORMALIZATION_VALUE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_INHERIT_THE_SPEC_OF_MAIN,
                                      this._popUp.CONTEXT_INHERIT_THE_SPEC_OF_MAIN);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_MODE, this._popUp.CONTEXT_MODE);

            //SPC-676 by Louis
            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_CHART_DESCRIPTION, this._popUp.CONTEXT_CHART_DESCRIPTION);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MASTER_SPEC_LIMIT,
                                      this._popUp.RULE_MASTER_SPEC_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_RAW, this._popUp.RULE_RAW);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MEAN, this._popUp.RULE_MEAN);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_STD, this._popUp.RULE_STD);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_RANGE, this._popUp.RULE_RANGE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_EWMA_MEAN, this._popUp.RULE_EWMA_MEAN);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_EWMA_STD, this._popUp.RULE_EWMA_STD);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_EWMA_RANGE, this._popUp.RULE_EWMA_RANGE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MA, this._popUp.RULE_MA);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MS, this._popUp.RULE_MS);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MR, this._popUp.RULE_MR);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_TECHNICAL_LIMIT, this._popUp.RULE_TECHNICAL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MOVING_OPTIONS,
                                      this._popUp.RULE_MOVING_OPTIONS);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_MEAN_OPTIONS,
                                      this._popUp.RULE_MEAN_OPTIONS);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_ZONE_A, this._popUp.RULE_ZONE_A);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_ZONE_B, this._popUp.RULE_ZONE_B);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_ZONE_C, this._popUp.RULE_ZONE_C);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_RULE_SELECTION,
                                      this._popUp.RULE_RULE_SELECTION);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_PARAMETER_CATEGORY,
                                      this._popUp.OPTION_PARAMETER_CATEGORY);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_CALCULATE_PPK,
                                      this._popUp.OPTION_CALCULATE_PPK);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_PRIORITY, this._popUp.OPTION_PRIORITY);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_SAMPLE_COUNTS,
                                      this._popUp.OPTION_SAMPLE_COUNTS);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_DAYS, this._popUp.OPTION_DAYS);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.OPTION_DEFAULT_CHART_TO_SHOW,
                                      this._popUp.OPTION_DEFAULT_CHART_TO_SHOW);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_PERIOD,
                                      this._popUp.AUTO_AUTO_CALCULATION_PERIOD);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_AUTO_CALCULATION_COUNT,
                                      this._popUp.AUTO_AUTO_CALCULATION_COUNT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MINIMUM_SAMPLES_TO_USE,
                                      this._popUp.AUTO_MINIMUM_SAMPLES_TO_USE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_DEFAULT_PERIOD,
                                      this._popUp.AUTO_DEFAULT_PERIOD);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MAXIMUM_PERIOD_TO_USE,
                                      this._popUp.AUTO_MAXIMUM_PERIOD_TO_USE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_TO_USE,
                                      this._popUp.AUTO_CONTROL_LIMIT_TO_USE);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CONTROL_LIMIT_THREASHOLD,
                                      this._popUp.AUTO_CONTROL_LIMIT_THREASHOLD);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CALCULATION_WITH_SHIFT_COMPENSATION,
                                      this._popUp.AUTO_CALCULATION_WITH_SHIFT_COMPENSATION);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CALCULATION_WITHOUT_IQR_FILTER,
                                      this._popUp.AUTO_CALCULATION_WITHOUT_IQR_FILTER);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_RAW_CONTROL_LIMIT,
                                      this._popUp.AUTO_RAW_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MEAN_CONTROL_LIMIT,
                                      this._popUp.AUTO_MEAN_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_STD_CONTROL_LIMIT,
                                      this._popUp.AUTO_STD_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_RANGE_CONTROL_LIMIT,
                                      this._popUp.AUTO_RANGE_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_EWMA_MEAN_CONTROL_LIMIT,
                                      this._popUp.AUTO_EWMA_MEAN_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_EWMA_STD_CONTROL_LIMIT,
                                      this._popUp.AUTO_EWMA_STD_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_EWMA_RANGE_CONTROL_LIMIT,
                                      this._popUp.AUTO_EWMA_RANGE_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MA_CONTROL_LIMIT,
                                      this._popUp.AUTO_MA_CONTROL_LIMIT);
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MS_CONTROL_LIMIT,
                                      this._popUp.AUTO_MS_CONTROL_LIMIT);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CALCULATION_THRESHOLD_OFF_YN,
                                      this._popUp.AUTO_THRESHOLD_FUNTION);

            //spc-1155 by stella
            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_MR_CONTROL_LIMIT,
                                      this._popUp.AUTO_MR_CONTROL_LIMIT);

            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_USE_GLOBAL_YN,
                                      this._popUp.USE_GLOBAL_YN);

            llstConfigurationInfo.Add(COLUMN.CHANGED_ITEMS, this.CompareCopyModel(llstConfigurationInfo));

            #endregion

            return llstConfigurationInfo;
        }

        private string CompareCopyModel(LinkedList llstConfigurationInfo)
        {
            string key = string.Empty;
            string changedItems = string.Empty;

            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (_arrColConfig.Contains(key) && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "Config,";
                    break;
                }
            }

            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (_arrColOption.Contains(key) && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "Option,";
                    break;
                }
            }

            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (_arrColAutoCalc.Contains(key) && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "AutoCalc,";
                    break;
                }

            }

            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (_arrColLimit.Contains(key) && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "Limit,";
                    break;
                }
            }

            for (int i = 0; i < llstConfigurationInfo.Count; i++)
            {
                key = llstConfigurationInfo.GetKey(i).ToString();

                if (key == Definition.COPY_MODEL.CONTEXT_CHART_DESCRIPTION && llstConfigurationInfo[key].ToString() == Definition.YES)
                {
                    changedItems += "ETC,";
                    break;
                }
            }

            changedItems = changedItems.Substring(0, changedItems.Length - 1);
            return changedItems;
        }

        public void bbtnCopyModel_Click_1(object sender, EventArgs e)
        {
            if (this._popUp != null)
            {
                this._popUp.ShowDialog();
            }
        }

        //SPC-855 by Louis
        private bool compareSpecLimit(DataSet SourceData, DataSet TargetData, string comparetype)
        {
            string sourceUSL = "";
            string sourceLSL = "";
            string sourceRawUCL = "";
            string sourceRawLCL = "";
            string sourceUpperControl = "";
            string sourceLowerControl = "";
            string sourceRawid = "";
            string targetUSL = "";
            string targetLSL = "";
            string targetRawUCL = "";
            string targetRawLCL = "";
            string targetUpperControl = "";
            string targetLowerControl = "";
            string targetRawid = "";


            if (SourceData != null)
            {
                foreach (DataRow dr in SourceData.Tables[0].Rows)
                {
                    sourceUSL = dr[COLUMN.UPPER_SPEC].ToString();
                    sourceLSL = dr[COLUMN.LOWER_SPEC].ToString();
                    sourceRawUCL = dr[COLUMN.RAW_UCL].ToString();
                    sourceRawLCL = dr[COLUMN.RAW_LCL].ToString();
                    sourceUpperControl = dr[COLUMN.UPPER_CONTROL].ToString();
                    sourceLowerControl = dr[COLUMN.LOWER_CONTROL].ToString();
                    sourceRawid = dr[COLUMN.RAWID].ToString();
                }
            }
            if (comparetype == "111")
            {
                
                targetUSL = sourceUSL;
                targetLSL = sourceLSL;
                targetRawUCL = sourceRawUCL;
                targetRawLCL = sourceRawLCL;
                targetUpperControl = sourceUpperControl;
                targetLowerControl = sourceLowerControl;
                targetRawid = sourceRawid;



                return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);
              
               
            }
            else if(comparetype == "110")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = sourceUSL;
                    targetLSL = sourceLSL;
                    targetRawUCL = sourceRawUCL;
                    targetRawLCL = sourceRawLCL;
                    targetUpperControl = dr[COLUMN.UPPER_CONTROL].ToString();
                    targetLowerControl = dr[COLUMN.LOWER_CONTROL].ToString();
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);
                  
                }
            }
            else if (comparetype == "101")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = sourceUSL;
                    targetLSL = sourceLSL;
                    targetRawUCL = dr[COLUMN.RAW_UCL].ToString();
                    targetRawLCL = dr[COLUMN.RAW_LCL].ToString();
                    targetUpperControl = sourceUpperControl;
                    targetLowerControl = sourceLowerControl;
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);
                    
                }
            }
            else if (comparetype == "100")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = sourceUSL;
                    targetLSL = sourceLSL;
                    targetRawUCL = dr[COLUMN.RAW_UCL].ToString();
                    targetRawLCL = dr[COLUMN.RAW_LCL].ToString();
                    targetUpperControl = dr[COLUMN.UPPER_CONTROL].ToString();
                    targetLowerControl = dr[COLUMN.LOWER_CONTROL].ToString();
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);
                 
                }
            }
            else if (comparetype == "011")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = dr[COLUMN.UPPER_SPEC].ToString();
                    targetLSL = dr[COLUMN.LOWER_SPEC].ToString();
                    targetRawUCL = sourceRawUCL;
                    targetRawLCL = sourceRawLCL;
                    targetUpperControl = sourceUpperControl;
                    targetLowerControl = sourceLowerControl;
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);
                   
                }
            }
            else if (comparetype == "010")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = dr[COLUMN.UPPER_SPEC].ToString();
                    targetLSL = dr[COLUMN.LOWER_SPEC].ToString();
                    targetRawUCL = sourceRawUCL;
                    targetRawLCL = sourceRawLCL;
                    targetUpperControl = dr[COLUMN.UPPER_CONTROL].ToString();
                    targetLowerControl = dr[COLUMN.LOWER_CONTROL].ToString();
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);
                   
                }
            }
            else if (comparetype == "001")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = dr[COLUMN.UPPER_SPEC].ToString();
                    targetLSL = dr[COLUMN.LOWER_SPEC].ToString();
                    targetRawUCL = dr[COLUMN.RAW_UCL].ToString();
                    targetRawLCL = dr[COLUMN.RAW_LCL].ToString();
                    targetUpperControl = sourceUpperControl;
                    targetLowerControl = sourceLowerControl;
                    targetRawid = dr[COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetRawUCL, targetRawLCL, targetUpperControl, targetLowerControl, targetRawid);
                }
                            
            }
            return true;
        }
        private bool checkSpecLimit(string targetUSL, string targetLSL, string targetRawUCL, string targetRawLCL, string targetUpperControl, string targetLowerControl, string targetRawid)
        {
            double dUpper = 0;
            double dLower = 0;

            #region usl & ucl(raw,mean)

            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetUpperControl))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetUpperControl);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_USL_SMALLER_MEAN_UCL", new string[]{targetRawid, "USL", "MEAN UCL"}, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetRawUCL))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetRawUCL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "USL", "RAW UCL" }, null);
                    return false;
                }
            }

            #endregion

            #region lsl & lcl(raw,mean)

            if (!string.IsNullOrEmpty(targetLSL) && !string.IsNullOrEmpty(targetLowerControl))
            {
                dUpper = double.Parse(targetLowerControl);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "MEAN LCL", "LSL" }, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetLSL) && !string.IsNullOrEmpty(targetRawLCL))
            {
                dUpper = double.Parse(targetRawLCL);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "RAW LCL", "LSL" }, null);
                    return false;
                }
            }
            #endregion

            #region lsl & ucl(raw,mean)
            if (!string.IsNullOrEmpty(targetLSL) && !string.IsNullOrEmpty(targetUpperControl))
            {
                dUpper = double.Parse(targetUpperControl);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "MEAN UCL", "LSL" }, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetLSL) && !string.IsNullOrEmpty(targetRawUCL))
            {
                dUpper = double.Parse(targetRawUCL);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "RAW UCL", "LSL" }, null);
                    return false;
                }
            }
            #endregion

            #region usl & lcl(raw,mean)
            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetLowerControl))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetLowerControl);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "USL", "MEAN LCL" }, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetRawLCL))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetRawLCL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_VALIDATION", new string[] { targetRawid, "USL", "MEAN LCL" }, null);
                    return false;
                }
            }
            #endregion

            return true;
        }

        private LinkedList CheckRawidDuplicate(ArrayList arrLimit, ArrayList arrConfig)
        {
            LinkedList llRawid = new LinkedList();
            ArrayList arrTemp = arrLimit.Clone() as ArrayList;

            for (int i = 0; i < arrConfig.Count; i++)
            {
                if (!arrLimit.Contains(arrConfig[i]))
                {
                    arrTemp.Add(arrConfig[i]);
                }
            }

            for (int i = 0; i < arrTemp.Count; i++)
            {
                llRawid.Add(i, arrTemp[i].ToString());
            }

            return llRawid;
        }
    }
}
