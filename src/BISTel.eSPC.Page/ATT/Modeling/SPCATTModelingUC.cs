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
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;

//PrograssBar by louis
using BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar;
using BISTel.PeakPerformance.Client.DataAsyncHandler;
//

using BISTel.PeakPerformance.Client.DataHandler;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;


using BISTel.eSPC.Common.ATT;
using System.CodeDom;
using System.Globalization;



namespace BISTel.eSPC.Page.ATT.Modeling
{
    public partial class SPCATTModelingUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition = new LinkedList();


        BISTel.eSPC.Common.ATT.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.ATT.CommonUtility _ComUtil;

        private SortedList _slParamColumnIndex = new SortedList();
        private SortedList _slSpcModelingIndex = new SortedList();

        private int _ColIdx_SELECT = 0;
        private int _colIdx_MAIN_YN = -1;
        private int _ColIdx_Interlock = 0;
        private int _ColIdx_Activation = 0;
        private int _ColIdx_RULE_LIST = 0;
        private int _ColIdx_OCAP_ACTION= 0;

        private int _ColIdx_PN_USL = 0;
        private int _ColIdx_PN_LSL = 0;
        private int _ColIdx_C_USL = 0;
        private int _ColIdx_C_LSL = 0;
        private int _ColIdx_PN_UCL = 0;
        private int _ColIdx_PN_LCL = 0;
        private int _ColIdx_C_UCL = 0;
        private int _ColIdx_C_LCL = 0;
        private int _ColIdx_P_UCL = 0;
        private int _ColIdx_P_LCL = 0;
        private int _ColIdx_U_UCL = 0;
        private int _ColIdx_U_LCL = 0;

       
        private int _ColIdx_PN_TARGET = 0;
        private int _ColIdx_C_TARGET = 0;
        private int _ColIdx_PN_CENTER_LINE = 0;
        private int _ColIdx_C_CENTER_LINE = 0;
        private int _ColIdx_P_CENTER_LINE = 0;
        private int _ColIdx_U_CENTER_LINE = 0;
        
        private int _ColIdx_CREATE_BY = 0;
        private int _ColIdx_CREATE_DTTS = 0;
        private int _ColIdx_LAST_UPDATE_BY = 0;
        private int _ColIdx_LAST_UPDATE_DTTS = 0;

        private int _Colldx_CHART_DESCRIPTION = 0;

        private string _sLineRawid;
        private string _sLine;
        private string _sAreaRawid;
        private string _sArea;
        private string _sEQPModel;
        private string _sSPCModelRawid;
        private string _sSPCModelName;
        private string _sSPCGroupName; //Group 관련 기능 추가, KBLEE
        private DataSet _dsSPCModeData = new DataSet();
        private DataTable _dtTableOriginal = new DataTable();

        private CellRange _bsprDataSelectedRange = null;

        private List<string> _valueForActivation = null;

        private bool _bsprDataValidate = true;

        private bool copyClicked = false;

        private bool _importcompleted = true;
        private bool _bUseComma;

       
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

        MenuItem fiterHide;
        MenuItem freeze;
        MenuItem unfreeze;
        MenuItem filter;
       
        private bool filterClicked = false;

        SpreadFilter _sfilter;

        MultiLanguageHandler _lang;

        #region ::: Properties


        #endregion


        #region ::: Constructor

        public SPCATTModelingUC()
        {
            InitializeComponent();
        }

        #endregion

        #region ::: Override Method

        public override void PageInit()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;

            this.KeyOfPage = this.GetType().FullName; // 필수 입력 (Key값을 받은 후 DC을 Active해주기 때문이다.

            this.InitializePage();
        }


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
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_AREA", null, null);
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
        }


        public void InitializeLayout()
        {
            this.bbtnCopyModel.Visible = false;
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_ATT_MODELING, Definition.CONFIG_USE_COMMA, false);

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
                filter = new MenuItem("Filter");
                filter.Click += new EventHandler(filter_Click);
                this.bsprData.ContextMenu.MenuItems.Add(filter);

                freeze = new MenuItem("Freeze");
                freeze.Click += new EventHandler(freeze_Click);
                this.bsprData.ContextMenu.MenuItems.Add(freeze);
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
        }


        public void InitializeDataButton()
        {
            this._slSpcModelingIndex = this._Initialization.InitializeButtonList(this.bbtnList, ref bsprData, Definition.PAGE_KEY_SPC_ATT_MODELING, "", this.sessionData);

            this.FunctionName = Definition.FUNC_KEY_SPC_ATT_MODELING;
            this.ApplyAuthory(this.bbtnList);
        }



        public void InitializeBSpread()
        {
            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
            this.bsprData.UseSpreadEdit = false;
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
            this._dtTableOriginal = new DataTable();
            this._ColIdx_Interlock = 0;
            this._ColIdx_Activation = 0;
        }




        #endregion

        private void bbtnList_ButtonClick(string name)
        {
            if (name.ToUpper().Equals("SPC_GROUPING"))
            {
                SPCModelGroupPopup popup = new SPCModelGroupPopup();
                popup.InitializePopup();
                popup.SESSIONDATA = this.sessionData;
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
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_REGION_SPREAD", null, null);
                }
            }
            else if (name.ToUpper().Equals(Definition.BUTTON_KEY_DEFAULTSETTING))
            {
                SPCConfigurationPopup spcConifgPopup = new SPCConfigurationPopup(true);
                spcConifgPopup.SESSIONDATA = this.sessionData;
                spcConifgPopup.URL = this.URL;
                spcConifgPopup.PORT = this.Port;
                spcConifgPopup.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.DEFAULT;
                spcConifgPopup.AREA_RAWID = this._sAreaRawid;
                spcConifgPopup.LINE_RAWID = this._sLineRawid;
                spcConifgPopup.EQP_MODEL = this._sEQPModel;
                spcConifgPopup.InitializePopup();
                DialogResult result = spcConifgPopup.ShowDialog();
            }
            else if (name.ToUpper().Equals(Definition.ButtonKey.CREATE_MAINMODEL))
            {
                SPCConfigurationPopup spcConifgPopup = new SPCConfigurationPopup();
                spcConifgPopup.SESSIONDATA = this.sessionData;
                spcConifgPopup.URL = this.URL;
                spcConifgPopup.PORT = this.Port;
                spcConifgPopup.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.CREATE_MAIN;
                spcConifgPopup.LINE_RAWID = this._sLineRawid;
                spcConifgPopup.AREA_RAWID = this._sAreaRawid;
                spcConifgPopup.EQP_MODEL = this._sEQPModel;
                spcConifgPopup.GROUP_NAME = Definition.VARIABLE_UNASSIGNED_MODEL;
                spcConifgPopup.MAIN_YN = "Y";
                spcConifgPopup.InitializePopup();

                DialogResult result = spcConifgPopup.ShowDialog();

                if (result == DialogResult.OK)
                {
                    DataTable dtModel = spcConifgPopup.SPCMODELDATA_DATASET.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC];
                    this._sSPCModelRawid = dtModel.Rows[0][BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    this._sSPCModelName = dtModel.Rows[0][BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString();

                    if (llstDynamicCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                    {
                        DataTable dtSPCModel = (DataTable)llstDynamicCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                        //SPC-1292, KBLEE
                        this._sSPCGroupName = spcConifgPopup.GROUP_NAME;
                        
                        dtSPCModel.Rows[0][DCUtil.VALUE_FIELD] = _sSPCModelRawid;
                        dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD] = _sSPCModelName;
                        dtSPCModel.Rows[0][BISTel.eSPC.Common.COLUMN.GROUP_NAME] = _sSPCGroupName; //SPC-1292, KBLEE
                        dtSPCModel.Rows[0]["CHECKED"] = "T";

                        while (dtSPCModel.Rows.Count > 1)
                        {
                            dtSPCModel.Rows.RemoveAt(1);
                        }
                    }
                    else
                    {
                        this._sSPCGroupName = spcConifgPopup.GROUP_NAME; //SPC-1292, KBLEE

                        DataTable dtSPCModel = new DataTable();
                        dtSPCModel.Columns.Add(DCUtil.VALUE_FIELD);
                        dtSPCModel.Columns.Add(DCUtil.DISPLAY_FIELD);
                        dtSPCModel.Columns.Add("CHECKED");
                        dtSPCModel.Columns.Add(BISTel.eSPC.Common.COLUMN.GROUP_NAME); //SPC-1292, KBLEE
                        dtSPCModel.Columns.Add(Definition.CONDITION_SEARCH_KEY_EQPMODEL);

                        DataRow drNewRow = dtSPCModel.NewRow();
                        drNewRow[DCUtil.VALUE_FIELD] = _sSPCModelRawid;
                        drNewRow[DCUtil.DISPLAY_FIELD] = _sSPCModelName;
                        drNewRow["CHECKED"] = "T";
                        drNewRow[BISTel.eSPC.Common.COLUMN.GROUP_NAME] = _sSPCGroupName; //SPC-1292, KBLEE


                        drNewRow[Definition.CONDITION_SEARCH_KEY_EQPMODEL] = _sEQPModel;

                        dtSPCModel.Rows.Add(drNewRow);

                        llstDynamicCondition.Add(Definition.CONDITION_SEARCH_KEY_SPCMODEL, dtSPCModel);

                        //SPC-1292, KBLEE, Start
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
                            dtSPCGroup.Columns.Add(BISTel.eSPC.Common.COLUMN.GROUP_NAME);
                            dtSPCGroup.Columns.Add("SPC MODEL LIST");

                            dtSPCGroup.Rows[0][DCUtil.VALUE_FIELD] = _sSPCGroupName;
                            dtSPCGroup.Rows[0][DCUtil.DISPLAY_FIELD] = _sSPCGroupName;
                            dtSPCGroup.Rows[0]["CHECKED"] = "T";
                            dtSPCGroup.Rows[0]["SPC MODEL LIST"] = "SPC MODEL LIST";

                            llstDynamicCondition.Add(Definition.CONDITION_KEY_GROUP_NAME, dtSPCGroup);
                        }
                        //SPC-1292, KBLEE, End
                    }

                    this.DynaminCondition.RefreshCondition(llstDynamicCondition);

                    this.ConfigListDataBinding();

                    Panel pnlDumy = new Panel();
                    pnlDumy.Size = new Size(250, 500);
                    this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", pnlDumy, false);
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
                spcConifgPopup.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.MODIFY;
                spcConifgPopup.LINE_RAWID = this._sLineRawid;
                spcConifgPopup.AREA_RAWID = this._sAreaRawid;
                spcConifgPopup.EQP_MODEL = this._sEQPModel;
                spcConifgPopup.GROUP_NAME = this._sSPCGroupName;
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
                            DataTable dtModelMst = spcConifgPopup.SPCMODELDATA_DATASET.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC];

                            DataTable dtSPCModel = (DataTable)llstDynamicCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                            dtSPCModel.Rows[0][DCUtil.VALUE_FIELD] = _sSPCModelRawid;
                            dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD] = dtModelMst.Rows[0][BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString();
                            dtSPCModel.Rows[0][BISTel.eSPC.Common.COLUMN.GROUP_NAME] = spcConifgPopup.GROUP_NAME;

                            DataTable dtSPCGroup = (DataTable)llstDynamicCondition[Definition.CONDITION_KEY_GROUP_NAME];
                            dtSPCGroup.Rows[0][DCUtil.VALUE_FIELD] = spcConifgPopup.GROUP_NAME;
                            dtSPCGroup.Rows[0][DCUtil.DISPLAY_FIELD] = spcConifgPopup.GROUP_NAME;
                            dtSPCGroup.Rows[0]["CHECKED"] = "F";

                            this.DynaminCondition.RefreshCondition(llstDynamicCondition);

                            this.btxtSPCModelName.Text = dtModelMst.Rows[0][BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString();
                            this._sSPCModelName = dtModelMst.Rows[0][BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString();
                            this._sSPCGroupName = spcConifgPopup.GROUP_NAME;
                        }
                    }

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

                    DataSet dsResult = _wsSPC.DeleteATTSPCModelConfig(_llstSearchCondition.GetSerialData()); //SPC-1292, KBLEE, 잘못된 Method명 수정

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

                    DataSet dsResult = _wsSPC.DeleteATTSPCModel(_llstSearchCondition.GetSerialData());

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
                chartViewPop.ChartVariable.CHART_PARENT_MODE = BISTel.eSPC.Common.CHART_PARENT_MODE.MODELING;
                chartViewPop.ChartVariable.LINE = this._sLine;
                chartViewPop.ChartVariable.AREA = this._sArea;
                chartViewPop.ChartVariable.SPC_MODEL = this._sSPCModelName;
                chartViewPop.ChartVariable.PARAM_ALIAS = sParamAlias;
                chartViewPop.ChartVariable.MODEL_CONFIG_RAWID = sConfigRawID;
                chartViewPop.AREA_RAWID = this._sAreaRawid; //SPC-1292, KBLEE
                chartViewPop.LINE_RAWID = this._sLineRawid; //SPC-1292, KBLEE
                chartViewPop.GROUP_NAME = this._sSPCGroupName; //SPC-1292, KBLEE
                chartViewPop.InitializePopup();
                chartViewPop.linkTraceDataViewEventPopup -= new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
                chartViewPop.linkTraceDataViewEventPopup += new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
                chartViewPop.ShowDialog(this);

                this._sSPCGroupName = chartViewPop.GROUP_NAME; //SPC-1292, KBLEE

            }
            else if (name.Equals(Definition.ButtonKey.SAVE))
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                if (!this._bsprDataValidate)
                {
                    this.bsprData.LeaveCellAction();
                    return;
                }

                DialogResult dg = MSGHandler.DialogQuestionResult(this._mlthandler.GetMessage("GENERAL_ASK_SAVE"), null, MessageBoxButtons.YesNo);
                if (dg == DialogResult.No)
                    return;

                DataSet _dsTemp = (DataSet)this.bsprData.DataSource;
                DataTable _dtTemp = _dsTemp.Tables[0];
                ArrayList arrTempRawID = new ArrayList();
                ArrayList arrTempInterlock = new ArrayList();
                ArrayList arrTempActivation = new ArrayList();
                ArrayList arrTempMainYN = new ArrayList();
                ArrayList arrTempSpecRawID = new ArrayList();
                ArrayList arrTempSpecMainYN = new ArrayList();
                ArrayList arrTempChartDescription = new ArrayList();
                ArrayList arrTempPN_UPPERSPEC = new ArrayList();
                ArrayList arrTempPN_LOWERSPEC = new ArrayList();
                ArrayList arrTempC_UPPERSPEC = new ArrayList();
                ArrayList arrTempC_LOWERSPEC = new ArrayList();
                ArrayList arrTempPN_UCL = new ArrayList();
                ArrayList arrTempPN_LCL = new ArrayList();
                ArrayList arrTempP_UCL = new ArrayList();
                ArrayList arrTempP_LCL = new ArrayList();
                ArrayList arrTempC_UCL = new ArrayList();
                ArrayList arrTempC_LCL = new ArrayList();
                ArrayList arrTempU_UCL = new ArrayList();
                ArrayList arrTempU_LCL = new ArrayList();
                ArrayList arrTempPN_TARGET = new ArrayList();
                ArrayList arrTempC_TARGET = new ArrayList();
                ArrayList arrTempPN_CENTER_LINE = new ArrayList();
                ArrayList arrTempP_CENTER_LINE = new ArrayList();
                ArrayList arrTempC_CENTER_LINE = new ArrayList();
                ArrayList arrTempU_CENTER_LINE = new ArrayList();


                for (int i = 0; i < this._dtTableOriginal.Rows.Count; i++)
                {
                    //바뀐 경우만 Check
                    if (this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.CHART_ID].ToString() == _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.CHART_ID].ToString() &&
                        (
                        (this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_N &&
                        (_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_TRUE || _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_Y)
                        ) ||
                        (this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_N &&
                        (_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_TRUE || _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_Y)
                        ) ||
                        (this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_Y &&
                        (_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_FALSE || _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.INTERLOCK_YN].ToString() == Definition.VARIABLE_N)
                        ) ||
                        (this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_Y &&
                        (_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_FALSE || _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.ACTIVATION_YN].ToString() == Definition.VARIABLE_N)
                        )
                        )
                        )
                    {
                        arrTempRawID.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.CHART_ID].ToString());
                        arrTempInterlock.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.INTERLOCK_YN].ToString());
                        arrTempActivation.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.ACTIVATION_YN].ToString());
                        arrTempMainYN.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString());
                    }

                    if (
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.CHART_ID].ToString() == _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.CHART_ID].ToString() &&
                        (
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.CHART_DESCRIPTON].ToString() !=  _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.CHART_DESCRIPTON].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.C_UPPERSPEC].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.C_UPPERSPEC].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.C_LOWERSPEC].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.C_LOWERSPEC].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.PN_UCL].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.PN_UCL].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.PN_LCL].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.PN_LCL].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.P_UCL].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.P_UCL].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.P_LCL].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.P_LCL].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.C_UCL].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.C_UCL].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.C_LCL].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.C_LCL].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.U_UCL].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.U_UCL].ToString() ||
                        this._dtTableOriginal.Rows[i][BISTel.eSPC.Common.COLUMN.U_LCL].ToString() != _dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.U_LCL].ToString() 
                        )
                        )
                    {
                        arrTempChartDescription.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.CHART_DESCRIPTON].ToString());
                        arrTempSpecRawID.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.CHART_ID].ToString());
                        arrTempSpecMainYN.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString());
                        arrTempPN_UPPERSPEC.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC].ToString());
                        arrTempPN_LOWERSPEC.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC].ToString());
                        arrTempC_UPPERSPEC.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.C_UPPERSPEC].ToString());
                        arrTempC_LOWERSPEC.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.C_LOWERSPEC].ToString());
                        arrTempPN_UCL.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.PN_UCL].ToString());
                        arrTempPN_LCL.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.PN_LCL].ToString());
                        arrTempP_UCL.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.P_UCL].ToString());
                        arrTempP_LCL.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.P_LCL].ToString());
                        arrTempC_UCL.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.C_UCL].ToString());
                        arrTempC_LCL.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.C_LCL].ToString());
                        arrTempU_UCL.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.U_UCL].ToString());
                        arrTempU_LCL.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.U_LCL].ToString());
                        arrTempPN_TARGET.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.PN_TARGET].ToString());
                        arrTempC_TARGET.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.C_TARGET].ToString());
                        arrTempPN_CENTER_LINE.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE].ToString());
                        arrTempP_CENTER_LINE.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.P_CENTER_LINE].ToString());
                        arrTempC_CENTER_LINE.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.C_CENTER_LINE].ToString());
                        arrTempU_CENTER_LINE.Add(_dtTemp.Rows[i][BISTel.eSPC.Common.COLUMN.U_CENTER_LINE].ToString());

                    }
                }

                if (arrTempRawID.Count > 0 || arrTempSpecRawID.Count > 0)
                {
                    if (arrTempRawID.Count > 0)
                    {
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

                        bool bResult = false;

                        bResult = this._wsSPC.SaveATTInterlockYN(lnkLst.GetSerialData());

                        if (!bResult)
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
                            return;
                        }

                    }
                    if (arrTempSpecRawID.Count > 0)
                    {
                        LinkedList lnkLst = new LinkedList();
                        lnkLst.Add(Definition.DynamicCondition_Condition_key.RAWID, arrTempSpecRawID);
                        lnkLst.Add(Definition.DynamicCondition_Condition_key.MAIN_YN, arrTempSpecMainYN);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.CHART_DESCRIPTON, arrTempChartDescription);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC, arrTempPN_UPPERSPEC);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC, arrTempPN_LOWERSPEC);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.C_UPPERSPEC, arrTempC_UPPERSPEC);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.C_LOWERSPEC, arrTempC_LOWERSPEC);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.PN_UCL, arrTempPN_UCL);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.PN_LCL, arrTempPN_LCL);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.P_UCL, arrTempP_UCL);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.P_LCL, arrTempP_LCL);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.C_UCL, arrTempC_UCL);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.C_LCL, arrTempC_LCL);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.U_UCL, arrTempU_UCL);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.U_LCL, arrTempU_LCL);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.PN_TARGET, arrTempPN_TARGET);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.C_TARGET, arrTempC_TARGET);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE, arrTempPN_CENTER_LINE);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.C_CENTER_LINE, arrTempC_CENTER_LINE);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.P_CENTER_LINE, arrTempC_CENTER_LINE);
                        lnkLst.Add(BISTel.eSPC.Common.COLUMN.U_CENTER_LINE, arrTempU_CENTER_LINE);
                        lnkLst.Add(Definition.CONDITION_KEY_USER_ID, this.sessionData.UserId);

                        bool bResult = false;

                        bResult = this._wsSPC.SaveATTSPCModelSpecData(lnkLst.GetSerialData());

                        if (!bResult)
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
                            return;
                        }
                    }

                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));

                    this.ConfigListDataBinding();

                    Panel pnlDumy = new Panel();
                    pnlDumy.Size = new Size(250, 500);
                    this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage,
                                               "SPC Model Property", pnlDumy, false);
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, this._mlthandler.GetMessage("GENERAL_NO_MODIFIED_DATA"));
                    return;
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
                spcConifgPopup.CONFIG_MODE = BISTel.eSPC.Common.ConfigMode.SAVE_AS;
                spcConifgPopup.CONFIG_RAWID = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.CHART_ID);
                spcConifgPopup.MAIN_YN = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.MAIN_YN);
                spcConifgPopup.HAS_SUBCONFIGS = false;
                spcConifgPopup.InitializePopup();
                result = spcConifgPopup.ShowDialog();

                if (result == DialogResult.OK)
                {
                    DataTable dtModel = spcConifgPopup.SPCMODELDATA_DATASET.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC];
                    this._sSPCModelRawid = dtModel.Rows[0][BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    this._sLineRawid = dtModel.Rows[0][BISTel.eSPC.Common.COLUMN.LOCATION_RAWID].ToString(); //SPC-1292, KBLEE
                    this._sSPCModelName = dtModel.Rows[0][BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString();
                    this._sSPCGroupName = spcConifgPopup.GROUP_NAME; //SPC-1292, KBLEE

                    if (llstDynamicCondition.Contains(Definition.CONDITION_SEARCH_KEY_SPCMODEL))
                    {
                        DataTable dtSPCModel = (DataTable)llstDynamicCondition[Definition.CONDITION_SEARCH_KEY_SPCMODEL];

                        dtSPCModel.Rows[0][DCUtil.VALUE_FIELD] = _sSPCModelRawid;
                        dtSPCModel.Rows[0][DCUtil.DISPLAY_FIELD] = _sSPCModelName;
                        //SPC-1292, KBLEE, START
                        dtSPCModel.Rows[0][BISTel.eSPC.Common.COLUMN.GROUP_NAME] = _sSPCGroupName;
                        dtSPCModel.Rows[0][BISTel.eSPC.Common.COLUMN.LOCATION_RAWID] = _sLineRawid;
                        dtSPCModel.Rows[0][BISTel.eSPC.Common.COLUMN.AREA_RAWID] = _sAreaRawid;
                        //SPC-1292, KBLEE, END
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
                
                if (alCheckRowIndex.Count == 1)
                {
                    iRowIndex = (int)alCheckRowIndex[0];
                    string sConfigRawID = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.CHART_ID);
                    string sParamAlias = this.bsprData.GetCellText(iRowIndex, (int)iColIdx.PARAM_ALIAS);

                   BISTel.eSPC.Page.ATT.Common.ChartCalculationPopup chartCalcPopup = new BISTel.eSPC.Page.ATT.Common.ChartCalculationPopup();
                    chartCalcPopup.URL = this.URL;
                    chartCalcPopup.SessionData = this.sessionData;
                    chartCalcPopup.SModelConfigRawID = sConfigRawID;
                    chartCalcPopup.SParamAlias = sParamAlias;
                    chartCalcPopup._sCalculation = false;
                    chartCalcPopup.InitializePopup();
                    chartCalcPopup.ShowDialog(this);
                    this.ConfigListDataBinding();
                    
                    Panel pnlDumy = new Panel();
                    pnlDumy.Size = new Size(250, 500);
                    this.InvokeMoreInformation(EESConstants.InformationType.UI, true, this.KeyOfPage, "SPC Model Property", pnlDumy, false);
                }
                else if (alCheckRowIndex.Count > 1)
                {
                    ArrayList sMulConfigRawID = new ArrayList();
                    ArrayList sMulParamAlias = new ArrayList();
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_MULTI_CALC_EXECUTE", null, null);

                    Common.Popup.MultiCalculationPopup SPCMultiCalPop = new BISTel.eSPC.Page.ATT.Common.Popup.MultiCalculationPopup();
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

                    return;
                }
                else
                {
                    iRowIndex = (int)alCheckRowIndex[0];
                }

                

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

            else if (name.Equals("EXPORT"))
            {
                this.bsprData.Export(true);

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
            this.BsprDataCheckboxSetting();
            
            if (!_importcompleted)
                return;

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
                    && col.Label != BISTel.eSPC.Common.COLUMN.SELECT)
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
            DataTable dtConfig = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC];
            DataTable dtContext = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC];

            DataRow[] drConfigs = dtConfig.Select(BISTel.eSPC.Common.COLUMN.MAIN_YN + " = 'Y'", BISTel.eSPC.Common.COLUMN.RAWID);
            List<string> contextColumnsForValidation = new List<string>();
            if (drConfigs != null && drConfigs.Length > 0)
            {
                DataRow[] drMainContexts = dtContext.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, drConfigs[0][BISTel.eSPC.Common.COLUMN.RAWID]), BISTel.eSPC.Common.COLUMN.KEY_ORDER);

                foreach (DataRow drMainContext in drMainContexts)
                {
                    contextColumnsForValidation.Add(drMainContext["CONTEXT_KEY_NAME"].ToString());
                }
            }

            for (int col = 0; col < this.bsprTempForImport.ActiveSheet.Columns.Count; col++)
            {
                string colLabel = this.bsprTempForImport.ActiveSheet.Columns[col].Label;
                if (colLabel == BISTel.eSPC.Common.COLUMN.SELECT)
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
                this.bsprData.GetDataSource().RejectChanges();
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
                
                string strParamAlias = "";
                _llstSearchCondition.Clear();
                _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_RAWID, this._sSPCModelRawid);
                _llstSearchCondition.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

                EESProgressBar.ShowProgress(this, this._lang.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_wsSPC, "GetSPCATTModelData", new object[] {_llstSearchCondition.GetSerialData()});

                EESProgressBar.CloseProgress(this);

                if (objDataSet != null)
                {
                    _dsSPCModeData = (DataSet)objDataSet;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                if (!DSUtil.CheckRowCount(_dsSPCModeData, BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_ALREADY_ELIMINATED", new string[]{_sSPCModelName}, null);
                    return;
                }

                this.btxtSPCModelName.Text = this._sSPCModelName;


                DataTable dtConfig = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC];
                DataTable dtContext = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC];
                DataTable dtRuleMst = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC];

                EESProgressBar.ShowProgress(this, MSGHandler.GetMessage("PROCESS_LOADING_PAGE_DATA"), false);

                //#01. SPC Model Chart List를 위한 Datatable 생성
                DataTable dtSPCModelChartList = new DataTable();

                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.SELECT, typeof(Boolean));
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.CHART_ID);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.PARAM_ALIAS);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.MAIN_YN);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.VERSION);
                dtSPCModelChartList.Columns.Add("MODE");

                //CONTEXT COLUMN 생성
                DataRow[] drConfigs = dtConfig.Select(BISTel.eSPC.Common.COLUMN.MAIN_YN + " = 'Y'", BISTel.eSPC.Common.COLUMN.RAWID);

                if (drConfigs != null && drConfigs.Length > 0)
                {
                    DataRow[] drMainContexts = dtContext.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, drConfigs[0][BISTel.eSPC.Common.COLUMN.RAWID]), BISTel.eSPC.Common.COLUMN.KEY_ORDER);

                    foreach (DataRow drMainContext in drMainContexts)
                    {
                        dtSPCModelChartList.Columns.Add(drMainContext["CONTEXT_KEY_NAME"].ToString());
                    }
                }

                //MODEL 정보 COLUMN 생성
                dtSPCModelChartList.Columns.Add(RULE_LIST);
                dtSPCModelChartList.Columns.Add(OCAP_ACTION);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.CHART_DESCRIPTON);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_TARGET);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_UCL);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_LCL);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_UPPERSPEC);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_LOWERSPEC);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_TARGET);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_UCL);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_LCL);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.C_CENTER_LINE);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.P_UCL);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.P_LCL);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.P_CENTER_LINE); 
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.U_UCL);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.U_LCL);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.U_CENTER_LINE);                
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.CREATE_BY);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.CREATE_DTTS);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.LAST_UPDATE_BY);
                dtSPCModelChartList.Columns.Add(BISTel.eSPC.Common.COLUMN.LAST_UPDATE_DTTS);
                 

                // Loading chart code code master data 
                LinkedList llCondition = new LinkedList();
                llCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CHART_MODE);
                DataSet _dsChartMode = this._wsSPC.GetCodeData(llCondition.GetSerialData());

                Dictionary<string, string> modeCodeData = new Dictionary<string, string>();
                if (_dsChartMode != null && _dsChartMode.Tables.Count > 0)
                {
                    foreach (DataRow dr in _dsChartMode.Tables[0].Rows)
                    {
                        modeCodeData.Add(dr[BISTel.eSPC.Common.COLUMN.CODE].ToString(), dr[BISTel.eSPC.Common.COLUMN.NAME].ToString());
                    }
                }

                //#02. CONFIG MST에 생성된 CONTEXT COLUMN에 Data 입력
                foreach (DataRow drConfig in dtConfig.Rows)
                {
                    DataRow drChartList = dtSPCModelChartList.NewRow();

                    drChartList[BISTel.eSPC.Common.COLUMN.CHART_ID] = drConfig[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.PARAM_ALIAS] = drConfig[BISTel.eSPC.Common.COLUMN.PARAM_ALIAS].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.MAIN_YN] = drConfig[BISTel.eSPC.Common.COLUMN.MAIN_YN].ToString();
                    //#Version이 Null 또는 Empty인 경우 대비 Check Logic 추가
                    if (!string.IsNullOrEmpty(drConfig[BISTel.eSPC.Common.COLUMN.VERSION].ToString()))
                        drChartList[BISTel.eSPC.Common.COLUMN.VERSION] = (1 + Convert.ToDouble(drConfig[BISTel.eSPC.Common.COLUMN.VERSION].ToString()) / 100).ToString("N2");
                    string modeValue = drConfig[BISTel.eSPC.Common.COLUMN.CHART_MODE_CD].ToString();
                    if (modeCodeData.ContainsKey(modeValue))
                    {
                        modeValue = modeCodeData[modeValue];
                    }
                    drChartList["MODE"] = modeValue;

                    if (strParamAlias == "")
                        strParamAlias = drConfig[BISTel.eSPC.Common.COLUMN.PARAM_ALIAS].ToString();

                    DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, drConfig[BISTel.eSPC.Common.COLUMN.RAWID]));

                    foreach (DataRow drContext in drContexts)
                    {
                        if (!dtSPCModelChartList.Columns.Contains(drContext["CONTEXT_KEY_NAME"].ToString()))
                            dtSPCModelChartList.Columns.Add(drContext["CONTEXT_KEY_NAME"].ToString());

                        drChartList[drContext["CONTEXT_KEY_NAME"].ToString()] = drContext[BISTel.eSPC.Common.COLUMN.CONTEXT_VALUE].ToString();
                    }

                    //MODEL 정보                
                    drChartList[BISTel.eSPC.Common.COLUMN.CHART_DESCRIPTON] = drConfig[BISTel.eSPC.Common.COLUMN.CHART_DESCRIPTON].ToString();                    
                    drChartList[BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC] = drConfig[BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC] = drConfig[BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.PN_TARGET] = drConfig[BISTel.eSPC.Common.COLUMN.PN_TARGET].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.PN_UCL] = drConfig[BISTel.eSPC.Common.COLUMN.PN_UCL].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.PN_LCL] = drConfig[BISTel.eSPC.Common.COLUMN.PN_LCL].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE] = drConfig[BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.C_UPPERSPEC] = drConfig[BISTel.eSPC.Common.COLUMN.C_UPPERSPEC].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.C_LOWERSPEC] = drConfig[BISTel.eSPC.Common.COLUMN.C_LOWERSPEC].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.C_TARGET] = drConfig[BISTel.eSPC.Common.COLUMN.C_TARGET].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.C_UCL] = drConfig[BISTel.eSPC.Common.COLUMN.C_UCL].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.C_LCL] = drConfig[BISTel.eSPC.Common.COLUMN.C_LCL].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.C_CENTER_LINE] = drConfig[BISTel.eSPC.Common.COLUMN.C_CENTER_LINE].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.P_UCL] = drConfig[BISTel.eSPC.Common.COLUMN.P_UCL].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.P_LCL] = drConfig[BISTel.eSPC.Common.COLUMN.P_LCL].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.P_CENTER_LINE] = drConfig[BISTel.eSPC.Common.COLUMN.P_CENTER_LINE].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.U_UCL] = drConfig[BISTel.eSPC.Common.COLUMN.U_UCL].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.U_LCL] = drConfig[BISTel.eSPC.Common.COLUMN.U_LCL].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.U_CENTER_LINE] = drConfig[BISTel.eSPC.Common.COLUMN.U_CENTER_LINE].ToString();

                    drChartList[BISTel.eSPC.Common.COLUMN.INTERLOCK_YN] = _ComUtil.NVL(drConfig[BISTel.eSPC.Common.COLUMN.INTERLOCK_YN], "N", true);
                    drChartList[BISTel.eSPC.Common.COLUMN.ACTIVATION_YN] = _ComUtil.NVL(drConfig[BISTel.eSPC.Common.COLUMN.ACTIVATION_YN], "N", true);
                    drChartList[BISTel.eSPC.Common.COLUMN.CREATE_BY] = drConfig[BISTel.eSPC.Common.COLUMN.CREATE_BY].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.CREATE_DTTS] = drConfig[BISTel.eSPC.Common.COLUMN.CREATE_DTTS] == DBNull.Value ? null : DateTime.Parse(drConfig[BISTel.eSPC.Common.COLUMN.CREATE_DTTS].ToString()).ToString(Definition.DATETIME_FORMAT_MS).ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.LAST_UPDATE_BY] = drConfig[BISTel.eSPC.Common.COLUMN.LAST_UPDATE_BY].ToString();
                    drChartList[BISTel.eSPC.Common.COLUMN.LAST_UPDATE_DTTS] = drConfig[BISTel.eSPC.Common.COLUMN.LAST_UPDATE_DTTS] == DBNull.Value ? null : DateTime.Parse(drConfig[BISTel.eSPC.Common.COLUMN.LAST_UPDATE_DTTS].ToString()).ToString(Definition.DATETIME_FORMAT_MS).ToString();

                                      
                    //RULE LIST
                    DataRow[] drRuleMsts = dtRuleMst.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, drConfig[BISTel.eSPC.Common.COLUMN.RAWID]));

                    if (drRuleMsts != null && drRuleMsts.Length > 0)
                    {

                        for (int i = 0; i < drRuleMsts.Length; i++)
                        {
                            if (i == 0)
                            {
                                drChartList["RULE_LIST"] = string.Format("RULE{0}", drRuleMsts[i][BISTel.eSPC.Common.COLUMN.SPC_RULE_NO]);
                                drChartList["OCAP"] = string.Format("[{0},{1}]",drChartList["RULE_LIST"], drRuleMsts[i][BISTel.eSPC.Common.COLUMN.OCAP_LIST]);
                            }
                            else
                            {
                                if (_bUseComma)
                                    drChartList["RULE_LIST"] = string.Format("{0},RULE{1}", drChartList["RULE_LIST"], drRuleMsts[i][BISTel.eSPC.Common.COLUMN.SPC_RULE_NO]);
                                else
                                    drChartList["RULE_LIST"] = string.Format("{0};RULE{1}", drChartList["RULE_LIST"], drRuleMsts[i][BISTel.eSPC.Common.COLUMN.SPC_RULE_NO]);
                                drChartList["OCAP"] = string.Format("{0},[RULE{1},{2}]", drChartList["OCAP"], drRuleMsts[i][BISTel.eSPC.Common.COLUMN.SPC_RULE_NO], drRuleMsts[i][BISTel.eSPC.Common.COLUMN.OCAP_LIST]);
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

                this._colIdx_MAIN_YN = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.MAIN_YN);
                this._ColIdx_RULE_LIST = dtSPCModelChartList.Columns.IndexOf(RULE_LIST);
                this._ColIdx_OCAP_ACTION = dtSPCModelChartList.Columns.IndexOf(OCAP_ACTION);
                this._Colldx_CHART_DESCRIPTION = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.CHART_DESCRIPTON);
                this._ColIdx_PN_USL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC);
                this._ColIdx_PN_LSL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC);                
                this._ColIdx_C_USL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.C_UPPERSPEC);
                this._ColIdx_C_LSL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.C_LOWERSPEC);
                this._ColIdx_PN_UCL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.PN_UCL);
                this._ColIdx_PN_LCL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.PN_LCL);
                this._ColIdx_P_UCL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.P_UCL);
                this._ColIdx_P_LCL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.P_LCL);
                this._ColIdx_C_UCL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.C_UCL);
                this._ColIdx_C_LCL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.C_LCL);
                this._ColIdx_U_UCL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.U_UCL);
                this._ColIdx_U_LCL = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.U_LCL);
                this._ColIdx_U_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.U_CENTER_LINE);
                this._ColIdx_P_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.P_CENTER_LINE);
                this._ColIdx_C_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.C_CENTER_LINE);
                this._ColIdx_PN_CENTER_LINE = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE);
                this._ColIdx_PN_TARGET = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.PN_TARGET);
                this._ColIdx_C_TARGET = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.C_TARGET);
                this._ColIdx_CREATE_BY = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.CREATE_BY);
                this._ColIdx_CREATE_DTTS = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.CREATE_DTTS);
                this._ColIdx_LAST_UPDATE_BY = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.LAST_UPDATE_BY);
                this._ColIdx_LAST_UPDATE_DTTS = dtSPCModelChartList.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.LAST_UPDATE_DTTS);

                for (int i = 0; i < dtSPCModelChartList.Columns.Count; i++)
                {
                    string sColumn = dtSPCModelChartList.Columns[i].ColumnName.ToString();
                    //this.bsprData.ActiveSheet.ColumnHeader.Cells[0, i].Text = sColumn;
                    if (i == (int)iColIdx.SELECT)
                    {
                        this.bsprData.AddHead(i, sColumn, sColumn, 50, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.CheckBox, null, null, null, false, true);
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
                this.bsprData.ActiveSheet.Columns[(int)iColIdx.PARAM_ALIAS].Visible = false;
                
                BsprDataCheckboxSetting();
                
                this.bsprData.AllowNewRow = false;
                this.bsprData.ActiveSheet.Columns[this._Colldx_CHART_DESCRIPTION, this._ColIdx_U_LCL].Locked = false;

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
             
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                EESProgressBar.CloseProgress(this);
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

            if (dt.Columns.Contains(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN))
            {
                this._ColIdx_Interlock = dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN);
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN)].CellType = cbct;
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN)].Locked = false;
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN)].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
                {
                    if (this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN)].Value == null)
                    {
                        _importcompleted = false;
                        MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_INTERLOCK_CANT_MODIFY", null, null);
                        break;
                    }
                    else
                    {
                        if (this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN)].Value.ToString() == Definition.VARIABLE_Y
                            || this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN)].Value.ToString().ToUpper() == "TRUE")
                        {
                            this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN)].Value = true;
                            this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN)].Text = Definition.VARIABLE_TRUE;
                        }
                        else
                        {
                            this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN)].Value = false;
                            this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN)].Text = Definition.VARIABLE_FALSE;
                        }
                    }
                }
            }

            if (dt.Columns.Contains(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN))
            {
                this._ColIdx_Activation = dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN);
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN)].CellType = cbct;
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN)].Locked = false;
                this.bsprData.ActiveSheet.Columns[dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN)].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                for (int i = 0; i < this.bsprData.ActiveSheet.RowCount; i++)
                {
                    if (this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN)].Value == null)
                    {
                        _importcompleted = false;
                        MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_ACTIVATION_CANT_MODIFY", null, null);
                        break;
                    }
                    if (this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN)].Value.ToString() == Definition.VARIABLE_Y
                        || this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN)].Value.ToString().ToUpper() == "TRUE")
                    {
                        this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN)].Value = true;
                        this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN)].Text = Definition.VARIABLE_TRUE;
                    }
                    else
                    {
                        this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN)].Value = false;
                        this.bsprData.ActiveSheet.Cells[i, dt.Columns.IndexOf(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN)].Text = Definition.VARIABLE_FALSE;
                    }
                }
            }
        }

        private void bsprData_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {

        }

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

        private void bsprData_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
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

        private void bsprData_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
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

        private void bsprData_KeyDown(object sender, KeyEventArgs e)
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
                        if (selectedRange.Column + backupData.GetUpperBound(1) > this._ColIdx_U_LCL)
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

        private void bsprData_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
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
                if (e.Column >= this._ColIdx_PN_USL && e.Column <= this._ColIdx_U_CENTER_LINE)
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

                    string sPNUSL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_PN_USL].Text.Trim();
                    string sPNLSL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_PN_LSL].Text.Trim();
                    string sPNUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_PN_UCL].Text.Trim();
                    string sPNLCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_PN_LCL].Text.Trim();
                    string sCUSL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_C_USL].Text.Trim();
                    string sCLSL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_C_LSL].Text.Trim();
                    string sCUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_C_UCL].Text.Trim();
                    string sCLCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_C_LCL].Text.Trim();

                    string sPUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_P_UCL].Text.Trim();
                    string sPLCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_P_LCL].Text.Trim();
                    string sUUCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_U_UCL].Text.Trim();
                    string sULCL = this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_U_LCL].Text.Trim();
                    

                    #region PN usl & ucl

                    if (sPNUSL.Length > 0 && sPNLSL.Length > 0)
                    {
                        dUpper = double.Parse(sPNUSL);
                        dLower = double.Parse(sPNLSL);

                        if (dUpper < 0 || dLower < 0)
                        {
                            return sErrMsg = MSGHandler.GetMessage("SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO");
                        }
                        else
                        {
                            if (dUpper < dLower)
                            {
                                sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_VALUE_SMALLER", new string[] {"USL", "LSL"});
                                return sErrMsg;
                            }
                            else
                            {
                                double dCenter = (dUpper + dLower) / 2;
                                this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_PN_TARGET].Text = dCenter.ToString();
                            }
                        }
                    }

                    if (sPNUSL.Length > 0 && sPNUCL.Length > 0)
                    {

                        dUpper = double.Parse(sPNUSL);
                        dLower = double.Parse(sPNUCL);

                        if (dUpper < 0 || dLower < 0)
                        {
                            return sErrMsg = MSGHandler.GetMessage("SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO");
                        }
                        else
                        {
                            if (dUpper < dLower)
                            {
                                sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_PN_VALUE", new string[]{"USL", "UCL"});
                                return sErrMsg;
                            }
                        }
                    }

                    if (sPNLSL.Length > 0 && sPNLCL.Length > 0)
                    {
                        dUpper = double.Parse(sPNLSL);
                        dLower = double.Parse(sPNLCL);

                        if (dUpper < 0 || dLower < 0)
                        {
                            return sErrMsg = MSGHandler.GetMessage("SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO");
                        }
                        else
                        {
                            if (dUpper > dLower)
                            {
                                sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_PN_VALUE", new string[] { "LCL", "LSL" });
                                return sErrMsg;
                            }
                        }
                    }

                    if (sPNUCL.Length > 0 && sPNLCL.Length > 0)
                    {
                        dUpper = double.Parse(sPNUCL);
                        dLower = double.Parse(sPNLCL);

                        if (dUpper < 0 || dLower < 0)
                        {
                            return sErrMsg = MSGHandler.GetMessage("SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO");
                        }
                        else
                        {
                            if (dUpper < dLower)
                            {
                                sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_PN_VALUE", new string[] { "UCL", "LCL" });
                                return sErrMsg;
                            }
                            else
                            {
                                double dCenter = (dUpper + dLower) / 2;
                                this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_PN_CENTER_LINE].Text = dCenter.ToString();
                            }
                        }
                    }

                    #endregion

                    #region P lsl & lcl

                    if (sPUCL.Length > 0 && sPLCL.Length > 0)
                    {
                        dUpper = double.Parse(sPUCL);
                        dLower = double.Parse(sPLCL);

                        if (dUpper < 0 || dLower < 0)
                        {
                            return sErrMsg = MSGHandler.GetMessage("SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO");
                        }
                        else
                        {
                            if (dUpper < dLower)
                            {
                                sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_P_VALUE", new string[] { "UCL", "LCL" });
                                return sErrMsg;
                            }
                            else
                            {
                                double dCenter = (dUpper + dLower) / 2;
                                this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_P_CENTER_LINE].Text = dCenter.ToString();
                            }
                        }
                    }

                    #endregion

                    #region C usl & ucl
                    if (sCUSL.Length > 0 && sCLSL.Length > 0)
                    {
                        dUpper = double.Parse(sCUSL);
                        dLower = double.Parse(sCLSL);

                        if (dUpper < 0 || dLower < 0)
                        {
                            return sErrMsg = MSGHandler.GetMessage("SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO");
                        }
                        else
                        {
                            if (dUpper < dLower)
                            {
                                sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_C_VALUE", new string[] { "USL", "LSL" });
                                return sErrMsg;
                            }
                            else
                            {
                                double dCenter = (dUpper + dLower) / 2;
                                this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_C_TARGET].Text = dCenter.ToString();
                            }
                        }
                    }

                    if (sCUSL.Length > 0 && sCUCL.Length > 0)
                    {
                        dUpper = double.Parse(sCUSL);
                        dLower = double.Parse(sCUCL);

                        if (dUpper < 0 || dLower < 0)
                        {
                            return sErrMsg = MSGHandler.GetMessage("SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO");
                        }
                        else
                        {
                            if (dUpper < dLower)
                            {
                                sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_C_VALUE", new string[] { "USL", "UCL" });
                                return sErrMsg;
                            }
                        }
                    }

                    if (sCLSL.Length > 0 && sCLCL.Length > 0)
                    {
                        dUpper = double.Parse(sCLSL);
                        dLower = double.Parse(sCLCL);

                        if (dUpper < 0 || dLower < 0)
                        {
                            return sErrMsg = MSGHandler.GetMessage("SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO");
                        }
                        else
                        {
                            if (dUpper > dLower)
                            {
                                sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_C_VALUE", new string[] { "LSL", "LCL" });
                                return sErrMsg;
                            }
                        }
                    }

                    if (sCUCL.Length > 0 && sCLCL.Length > 0)
                    {
                        dUpper = double.Parse(sCUCL);
                        dLower = double.Parse(sCLCL);

                        if (dUpper < 0 || dLower < 0)
                        {
                            return sErrMsg = MSGHandler.GetMessage("SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO");
                        }
                        else
                        {
                            if (dUpper < dLower)
                            {
                                sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_C_VALUE", new string[] { "UCL", "LCL" });
                                return sErrMsg;
                            }
                            else
                            {
                                double dCenter = (dUpper + dLower) / 2;
                                this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_C_CENTER_LINE].Text = dCenter.ToString();
                            }
                        }
                    }

                    #endregion

                    #region U Control Limit

                    if (sUUCL.Length > 0 && sULCL.Length > 0)
                    {
                        dUpper = double.Parse(sUUCL);
                        dLower = double.Parse(sULCL);

                        if (dUpper < 0 || dLower < 0)
                        {
                            return sErrMsg = MSGHandler.GetMessage("SPC_INFO_VALID_INPUT_NUM_GREATER_THAN_ZERO");
                        }
                        else
                        {
                            if (dUpper < dLower)
                            {
                                sErrMsg = MSGHandler.GetTransMessage("SPC_INFO_VALID_U_VALUE", new string[] { "UCL", "LCL" });
                                return sErrMsg;
                            }
                            else
                            {
                                double dCenter = (dUpper + dLower) / 2;
                                this.bsprData.ActiveSheet.Cells[e.Row, this._ColIdx_U_CENTER_LINE].Text = dCenter.ToString();
                            }
                        }
                    }
                    #endregion

                    
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

        private void bsprData_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
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


                DataTable dtConfig = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC];
                DataTable dtContext = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC];
                DataTable dtRule = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC];
                DataTable dtRuleOpt = _dsSPCModeData.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_OPT_ATT_MST_SPC];

                DataRow[] drConfigs = dtConfig.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.RAWID, sConfigRawID));
                DataRow drConfig = drConfigs[0];
                DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, sConfigRawID));
                DataRow[] drRules = dtRule.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, sConfigRawID));


                //# Category Name 정의
                string sCategoryContext = "1.Context";
                string sCategoryParameter = "2.Parameter";
                string sCategoryMasterOption = "3.Master Option";
                string sCategoryAppliedRule = "4.Applied Rule List";


                //#01.SPCModelInfo Class 생성
                DynamicCodeGenerator dynamicCodeGen = new DynamicCodeGenerator("BISTel.eSPC.Page.ATT.Modeling", "SPCATTModelInfo");

                dynamicCodeGen.AddReference("System.dll");

                dynamicCodeGen.AddImports("System");
                dynamicCodeGen.AddImports("System.ComponentModel");
                dynamicCodeGen.AddImports("System.Globalization");


                //#02. Expandable Converter Class 생성 후 SPCModelInfo.cs 파일의 Namespace내에 붙임
                DynamicCodeGenerator dcGenExConverter = new DynamicCodeGenerator("BISTel.eSPC.Page.ATT.Modeling", "ExpandableConverter", "ExpandableObjectConverter");

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
                    sPropertyName = drContext[BISTel.eSPC.Common.COLUMN.CONTEXT_KEY].ToString();
                    //sPropertyDisplayName = string.Format("{0}.{1}", iContextIdx, drContext[BISTel.eSPC.Common.COLUMN.CONTEXT_KEY]);
                    sPropertyDisplayName = drContext["CONTEXT_KEY_NAME"].ToString();

                    sVariableName = string.Format("_{0}", drContext[BISTel.eSPC.Common.COLUMN.CONTEXT_KEY]);
                    sVariableValue = drContext[BISTel.eSPC.Common.COLUMN.CONTEXT_VALUE].ToString();

                    dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                    dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryContext, "", true);

                    iContextIdx++;
                }


                //#04. Parameter Property 생성
                //Param Name
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.PARAM_ALIAS);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.PARAM_ALIAS].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.PARAM_ALIAS;
                sPropertyDisplayName = "Attrivutes Name";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryParameter, "", true);

                //PN Upper Spec
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC;
                sPropertyDisplayName = "PN Upper Spec";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //PN Lower Spec
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC;
                sPropertyDisplayName = "PN Lower Spec";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //PN Target
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.PN_TARGET);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.PN_TARGET].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.PN_TARGET;
                sPropertyDisplayName = "PN Target";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //PN UCL
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.PN_UCL);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.PN_UCL].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.PN_UCL;
                sPropertyDisplayName = "PN UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //PN LCL
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.PN_LCL);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.PN_LCL].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.PN_LCL;
                sPropertyDisplayName = "PN LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //PN Center Line
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.PN_CENTER_LINE;
                sPropertyDisplayName = "PN Center Line";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //C Upper Spec
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.C_UPPERSPEC);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.C_UPPERSPEC].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.C_UPPERSPEC;
                sPropertyDisplayName = "C Upper Spec";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //C Lower Spec
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.C_LOWERSPEC);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.C_LOWERSPEC].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.C_LOWERSPEC;
                sPropertyDisplayName = "C Lower Spec";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //C Target
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.C_TARGET);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.C_TARGET].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.C_TARGET;
                sPropertyDisplayName = "C Target";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //C UCL
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.C_UCL);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.C_UCL].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.C_UCL;
                sPropertyDisplayName = "C UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //C LCL
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.C_LCL);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.C_LCL].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.C_LCL;
                sPropertyDisplayName = "C LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //C Center Line
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.C_CENTER_LINE);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.C_CENTER_LINE].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.C_CENTER_LINE;
                sPropertyDisplayName = "C Center Line";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //P UCL
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.P_UCL);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.P_UCL].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.P_UCL;
                sPropertyDisplayName = "P UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //P LCL
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.P_LCL);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.P_LCL].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.P_LCL;
                sPropertyDisplayName = "P LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //P Center Line
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.P_CENTER_LINE);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.P_CENTER_LINE].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.P_CENTER_LINE;
                sPropertyDisplayName = "P Center Line";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //U UCL
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.U_UCL);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.U_UCL].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.U_UCL;
                sPropertyDisplayName = "U UCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //U LCL
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.U_LCL);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.U_LCL].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.U_LCL;
                sPropertyDisplayName = "U LCL";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                //U Center Line
                sVariableName = string.Format("_{0}", BISTel.eSPC.Common.COLUMN.U_CENTER_LINE);
                sVariableValue = drConfig[BISTel.eSPC.Common.COLUMN.U_CENTER_LINE].ToString();
                sPropertyName = BISTel.eSPC.Common.COLUMN.U_CENTER_LINE;
                sPropertyDisplayName = "U Center Line";

                dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, typeof(string), new CodePrimitiveExpression(sVariableValue));
                dynamicCodeGen.AddProperty(MemberAttributes.Public, typeof(string), sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryMasterOption, "", true);

                

                //#06. Rule Property
                foreach (DataRow drRule in drRules)
                {
                    //Rule Property
                    sVariableType = string.Format("RuleClass{0}", drRule[BISTel.eSPC.Common.COLUMN.SPC_RULE_NO]);
                    sVariableName = string.Format("_RuleClass{0}", drRule[BISTel.eSPC.Common.COLUMN.SPC_RULE_NO]);
                    sVariableValue = drRule[BISTel.eSPC.Common.COLUMN.SPC_RULE_NO].ToString();
                    sPropertyName = string.Format("Rule{0}", drRule[BISTel.eSPC.Common.COLUMN.SPC_RULE_NO]);
                    sPropertyDisplayName = string.Format("Rule{0}", drRule[BISTel.eSPC.Common.COLUMN.SPC_RULE_NO]);
                    sPropertyDescription = drRule[BISTel.eSPC.Common.COLUMN.DESCRIPTION].ToString();

                    dynamicCodeGen.AddMember(MemberAttributes.Private, sVariableName, sVariableType, new CodeSnippetExpression(string.Format("new {0}()", sVariableType)));
                    dynamicCodeGen.AddProperty(MemberAttributes.Public, sVariableType, sPropertyName, sPropertyDisplayName, DynamicCodeGenerator.PropertyType.GET, sVariableName, sCategoryAppliedRule, sPropertyDescription, true);


                    DataRow[] drRuleOpts = dtRuleOpt.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_RULE_RAWID, drRule[BISTel.eSPC.Common.COLUMN.RAWID]), BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO);

                    //Rule Class
                    DynamicCodeGenerator dcGenRuleClass = new DynamicCodeGenerator("BISTel.eSPC.Page.Modeling", sVariableType);
                    dcGenRuleClass.AddClassAttribute("TypeConverter", new CodeSnippetExpression("typeof(ExpandableConverter)"));

                    foreach (DataRow drRuleOpt in drRuleOpts)
                    {
                        sVariableName = string.Format("_RuleOption{0}", drRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO]);
                        sVariableValue = drRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE].ToString();
                        sPropertyName = string.Format("RuleOption{0}", drRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO]);
                        sPropertyDisplayName = drRuleOpt[BISTel.eSPC.Common.COLUMN.OPTION_NAME].ToString();
                        sPropertyDescription = drRuleOpt[BISTel.eSPC.Common.COLUMN.DESCRIPTION].ToString();

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

            this._popUp.SetCopyModelInfo(this._sEQPModel, this._sSPCModelName, mainYN, configRawID, this.btxtParamAlias.Text);
            DialogResult resultMessage = this._popUp.ShowDialog();

            if (resultMessage == DialogResult.OK)
            {
                this.bbtnCopyModel.Visible = true;

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

            //SPC-1218, KBLEE, START
            for (int i = 0; i < alCheckRowIndex.Count; i++)
            {
                int selectedRow = (int)alCheckRowIndex[i];

                if (this._popUp.CONFIG_RAWID.ToString() == this.bsprData.ActiveSheet.Cells[selectedRow, (int)iColIdx.CHART_ID].Text)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_TARGET_SAME", null, null);
                    return;
                }
            }

            for (int i = 0; i < alCheckRowIndex.Count; i++)
            {
                int selectedRow = (int)alCheckRowIndex[i];

                if (this._popUp.CONTEXT_CONTEXT_INFORMATION == "Y" && this.bsprData.ActiveSheet.Cells[selectedRow, (int)iColIdx.MAIN_YN].Text == "N")
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_COPY_CONTEXT_FOR_ONLY_MAIN", null, null);
                    return;
                }
            }
            //SPC-1218, KBLEE, END

            if (_popUp.RULE_PN_SPEC_LIMIT.ToString().Equals("Y") || _popUp.RULE_PN_CONTROL.ToString().Equals("Y"))
            {
                LinkedList toraltargetRawidList = new LinkedList();
                string[] targetRawid = new string[alCheckRowIndex.Count];
                for (int i = 0; i < alCheckRowIndex.Count; i++)
                {
                    LinkedList targetRawidList = new LinkedList();
                    int selectedRow = (int)alCheckRowIndex[i];
                    targetRawid[i] = this.bsprData.ActiveSheet.Cells[selectedRow, (int)iColIdx.CHART_ID].Text;
                }
                DataSet targetSpecData = _wsSPC.GetATTTargetConfigSpecData(targetRawid);
                DataSet sourceSpecData = _wsSPC.GetATTSourseConfigSpecData(_popUp.CONFIG_RAWID.ToString());

                if (targetSpecData != null && sourceSpecData != null)
                {
                    bool CompareResult = true;
                    if (_popUp.RULE_PN_SPEC_LIMIT.ToString().Equals("Y") && _popUp.RULE_PN_CONTROL.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "11", "PN");
                    }
                    else if (_popUp.RULE_PN_SPEC_LIMIT.ToString().Equals("N") && _popUp.RULE_PN_CONTROL.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "01", "PN");
                    }
                    else if (_popUp.RULE_PN_SPEC_LIMIT.ToString().Equals("Y") && _popUp.RULE_PN_CONTROL.ToString().Equals("N"))    
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "10", "PN");
                    }
                    if (!CompareResult)
                    {
                        return;
                    }
                }

            }
            if(_popUp.RULE_C_SPEC_LIMIT.ToString().Equals("Y") || _popUp.RULE_C_CONTROL.ToString().Equals("Y"))
            {
                LinkedList toraltargetRawidList = new LinkedList();
                string[] targetRawid = new string[alCheckRowIndex.Count];
                for (int i = 0; i < alCheckRowIndex.Count; i++)
                {
                    LinkedList targetRawidList = new LinkedList();
                    int selectedRow = (int)alCheckRowIndex[i];
                    targetRawid[i] = this.bsprData.ActiveSheet.Cells[selectedRow, (int)iColIdx.CHART_ID].Text;
                }
                DataSet targetSpecData = _wsSPC.GetATTTargetConfigSpecData(targetRawid);
                DataSet sourceSpecData = _wsSPC.GetATTSourseConfigSpecData(_popUp.CONFIG_RAWID.ToString());

                if (targetSpecData != null && sourceSpecData != null)
                {
                    bool CompareResult = true;
                    if (_popUp.RULE_C_SPEC_LIMIT.ToString().Equals("Y") && _popUp.RULE_C_CONTROL.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "11", "C");
                    }
                    else if (_popUp.RULE_C_SPEC_LIMIT.ToString().Equals("N") && _popUp.RULE_C_CONTROL.ToString().Equals("Y"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "01", "C");
                    }
                    else if (_popUp.RULE_C_SPEC_LIMIT.ToString().Equals("Y") && _popUp.RULE_C_CONTROL.ToString().Equals("N"))
                    {
                        CompareResult = this.compareSpecLimit(sourceSpecData, targetSpecData, "10", "C");
                    }
                    if (!CompareResult)
                    {
                        return;
                    }
                }
            }

            DialogResult result = MSGHandler.DialogQuestionResult("SPC_INFO_DIALOG_COPY",
                                                                  new string[] { "Model Copy" }, MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                if (this._popUp != null)
                {
                    try
                    {
                        this.MsgShow(COMMON_MSG.Query_Data);

                        StringBuilder sbResult = new StringBuilder();
                        string sourceConfigRawID = this._popUp.CONFIG_RAWID;

                        LinkedList llstTotalConfigInfo = new LinkedList();


                        for (int i = 0; i < alCheckRowIndex.Count; i++)
                        {
                            int selectedRow = (int)alCheckRowIndex[i];
                            string targetConfigRawID =
                                this.bsprData.ActiveSheet.Cells[selectedRow, (int)iColIdx.CHART_ID].Text;
                            string mainYN = this.bsprData.ActiveSheet.Cells[selectedRow, (int)iColIdx.MAIN_YN].Text;

                            bool hasSubConfigs = false;

                            if (this.bsprData.ActiveSheet.RowCount > 1) //SubConfig 존재여부
                            {
                                hasSubConfigs = true;
                            }

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
                            
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_CALCULATION,
                                                      this._popUp.CONTEXT_AUTO_CALCULATION);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_GENERATE_SUB_CHART,
                                                      this._popUp.CONTEXT_AUTO_GENERATE_SUB_CHART);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_ACTIVE, this._popUp.CONTEXT_ACTIVE);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_SAMPLE_COUNT,
                                                      this._popUp.CONTEXT_SAMPLE_COUNT);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_AUTO_SETTING,
                                                      this._popUp.CONTEXT_AUTO_SETTING);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK,
                                                      this._popUp.CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION,
                                                      this._popUp.CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_INHERIT_THE_SPEC_OF_MAIN,
                                                      this._popUp.CONTEXT_INHERIT_THE_SPEC_OF_MAIN);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_MODE, this._popUp.CONTEXT_MODE);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_CONTEXT_INFORMATION, this._popUp.CONTEXT_CONTEXT_INFORMATION); //SPC-1218, KBLEE

                            //SPC-676 by Louis
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.CONTEXT_CHART_DESCRIPTION, this._popUp.CONTEXT_CHART_DESCRIPTION);

                            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_PN_SPEC_LIMIT,
                                                      this._popUp.RULE_PN_SPEC_LIMIT);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_C_SPEC_LIMIT, this._popUp.RULE_C_SPEC_LIMIT);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_PN_CONTROL, this._popUp.RULE_PN_CONTROL);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_C_CONTROL, this._popUp.RULE_C_CONTROL);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_P_CONTROL, this._popUp.RULE_P_CONTROL);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.RULE_U_CONTROL, this._popUp.RULE_U_CONTROL);
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
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_PN_CONTROL_LIMIT,
                                                      this._popUp.AUTO_PN_CONTROL_LIMIT);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_P_CONTROL_LIMIT,
                                                      this._popUp.AUTO_P_CONTROL_LIMIT);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_C_CONTROL_LIMIT,
                                                      this._popUp.AUTO_C_CONTROL_LIMIT);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_U_CONTROL_LIMIT,
                                                      this._popUp.AUTO_U_CONTROL_LIMIT);
                            llstConfigurationInfo.Add(Definition.COPY_MODEL.AUTO_CALCULATION_THRESHOLD_OFF_YN,
                                                      this._popUp.AUTO_THRESHOLD_FUNTION);
                            #endregion

                            llstTotalConfigInfo.Add(i, llstConfigurationInfo);
                        }

                        DataSet dsResult = _wsSPC.ATTCopyModelInfo(llstTotalConfigInfo.GetSerialData());

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
                    catch (Exception ex)
                    {
                        LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                    }
                    finally
                    {
                        this.MsgClose();
                    }
                }
            }
        }

        private void bbtnCopyModel_Click_1(object sender, EventArgs e)
        {
            if (this._popUp != null)
            {
                this._popUp.ShowDialog();
            }
        }

        //SPC-855 by Louis
        private bool compareSpecLimit(DataSet SourceData, DataSet TargetData, string comparetype, string ChartType)
        {
            string sourceUSL = "";
            string sourceLSL = "";
            string sourceUCL = "";
            string sourceLCL = "";
            string sourceRawid = "";
            string targetUSL = "";
            string targetLSL = "";
            string targetUCL = "";
            string targetLCL = "";
            string targetRawid = "";


            if (SourceData != null)
            {
                if (ChartType == "PN")
                {
                    foreach (DataRow dr in SourceData.Tables[0].Rows)
                    {
                        sourceUSL = dr[BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC].ToString();
                        sourceLSL = dr[BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC].ToString();
                        sourceUCL = dr[BISTel.eSPC.Common.COLUMN.PN_UCL].ToString();
                        sourceLCL = dr[BISTel.eSPC.Common.COLUMN.PN_LCL].ToString();
                        sourceRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    }

                    foreach (DataRow dr in TargetData.Tables[0].Rows)
                    {
                        targetUSL = dr[BISTel.eSPC.Common.COLUMN.PN_UPPERSPEC].ToString();
                        targetLSL = dr[BISTel.eSPC.Common.COLUMN.PN_LOWERSPEC].ToString();
                        targetUCL = dr[BISTel.eSPC.Common.COLUMN.PN_UCL].ToString();
                        targetLCL = dr[BISTel.eSPC.Common.COLUMN.PN_LCL].ToString();
                        sourceRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    }

                    
                }
                else if (ChartType == "C")
                {
                    foreach (DataRow dr in SourceData.Tables[0].Rows)
                    {
                        sourceUSL = dr[BISTel.eSPC.Common.COLUMN.C_UPPERSPEC].ToString();
                        sourceLSL = dr[BISTel.eSPC.Common.COLUMN.C_LOWERSPEC].ToString();
                        sourceUCL = dr[BISTel.eSPC.Common.COLUMN.C_UCL].ToString();
                        sourceLCL = dr[BISTel.eSPC.Common.COLUMN.C_LCL].ToString();
                        sourceRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    }
                    foreach (DataRow dr in TargetData.Tables[0].Rows)
                    {
                        targetUSL = dr[BISTel.eSPC.Common.COLUMN.C_UPPERSPEC].ToString();
                        targetLSL = dr[BISTel.eSPC.Common.COLUMN.C_LOWERSPEC].ToString();
                        targetUCL = dr[BISTel.eSPC.Common.COLUMN.C_UCL].ToString();
                        targetLCL = dr[BISTel.eSPC.Common.COLUMN.C_LCL].ToString();
                        sourceRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    }
                }
            }
            if (comparetype == "11")
            {
                
                targetUSL = sourceUSL;
                targetLSL = sourceLSL;
                targetUCL = sourceUCL;
                targetLCL = sourceLCL;
                targetRawid = sourceRawid;

                return checkSpecLimit(targetUSL, targetLSL, targetUCL, targetLCL, targetRawid, ChartType);
              
               
            }
            else if(comparetype == "10")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUSL = sourceUSL;
                    targetLSL = sourceLSL;
                    targetRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetUCL, targetLCL, targetRawid, ChartType);
                  
                }
            }
            else if (comparetype == "01")
            {
                foreach (DataRow dr in TargetData.Tables[0].Rows)
                {
                    targetUCL = sourceUCL;
                    targetLCL = sourceLCL;
                    targetRawid = dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString();

                    return checkSpecLimit(targetUSL, targetLSL, targetUCL, targetLCL, targetRawid, ChartType);
                    
                }
            }
            return true;
        }
        private bool checkSpecLimit(string targetUSL, string targetLSL, string targetUCL, string targetLCL, string targetRawid, string ChartType)
        {
            double dUpper = 0;
            double dLower = 0;

            #region usl & ucl(raw,mean)

            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetLSL))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_ID_TYPE", new string[] { targetRawid, ChartType, "USL", "LSL"}, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetUCL))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetUCL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_ID_TYPE", new string[] { targetRawid, ChartType, "USL", "UCL" }, null);
                    return false;
                }
            }

            #endregion

            #region lsl & lcl(raw,mean)

            if (!string.IsNullOrEmpty(targetLCL) && !string.IsNullOrEmpty(targetUCL))
            {
                dUpper = double.Parse(targetUCL);
                dLower = double.Parse(targetLCL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_ID_TYPE", new string[] { targetRawid, ChartType, "UCL", "LCL" }, null);
                    return false;
                }
            }

            #endregion

            #region lsl & ucl(raw,mean)
            if (!string.IsNullOrEmpty(targetUSL) && !string.IsNullOrEmpty(targetLCL))
            {
                dUpper = double.Parse(targetUSL);
                dLower = double.Parse(targetLCL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_ID_TYPE", new string[] { targetRawid, ChartType, "USL", "LCL" }, null);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(targetUCL) && !string.IsNullOrEmpty(targetLSL))
            {
                dUpper = double.Parse(targetUCL);
                dLower = double.Parse(targetLSL);

                if (dUpper < dLower)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CHART_ID_TYPE", new string[] { targetRawid, ChartType, "UCL", "LSL" }, null);
                    return false;
                }
            }
            #endregion

            return true;
        }
    }
}
