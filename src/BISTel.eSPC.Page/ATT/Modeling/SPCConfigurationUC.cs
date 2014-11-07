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
    public partial class SPCConfigurationUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition;


        BISTel.eSPC.Common.ATT.CommonUtility _ComUtil;

        LinkedList _ModelConfigCondition = new LinkedList();

        private int _ColIdx_SELECT = 0;

        private int _ColIdx_MainChart = 0;
        private int _ColIdx_Parameter = 0;
        private int _ColIdx_EQPID = 0;
        private int _ColIdx_ModuleID = 0;
        private int _ColIdx_OperationID = 0;
        private int _ColIdx_ProductID = 0;

        private BISTel.eSPC.Common.ConfigMode _cofingMode = BISTel.eSPC.Common.ConfigMode.CREATE_SUB;

        private string _sConfigRawID;
        private string _sMainYN;
        private string _sAreaRawID;
        private string _sLineRawID;
        private string _sEQPModel;
        private string _sVersion;
        private string _sGroupName; //Group 관련 기능 추가, KBLEE

        private DataSet _dsSPCModelData = new DataSet();

        private DataTable _dtModel;
        private DataTable _dtConfig;
        private DataTable _dtContext;
        private DataTable _dtConfigOpt;
        private DataTable _dtAutoCalc;
        private DataTable _dtRule;
        private DataTable _dtRuleOpt;
        private DataTable _OridtModel;
        private DataTable _OridtConfig;
        private DataTable _OridtContext;
        private DataTable _OridtConfigOpt;
        private DataTable _OridtAutoCalc;
        private DataTable _OridtRule;
        private DataTable _OridtRuleOpt;

        private bool _hasSubConfigs;

        //SPC-1292, KBLEE, START
        private ArrayList _ruleColumns;
        private ArrayList _ConfigColums;
        private ArrayList _notCompareColumns;
        private bool _isConfig = false;
        private bool _isContext = false;
        private bool _isOption = false;
        private bool _isAutoCalc = false;
        private bool _isRule = false;
        private bool _isLimit = false;
        private bool _isETC = false;

        private DataSet _dsGroupList;
        //SPC-1292, KBLEE, END
        #endregion


        #region ::: Properties


        #endregion


        #region ::: Constructor

        public SPCConfigurationUC()
        {
            ShowParameterAlias = false;
            InitializeComponent();
        }

        private bool _isDefaultModel = false;
        private bool _bUseComma;
        public SPCConfigurationUC(bool isDefaultModel)
        {
            this._isDefaultModel = isDefaultModel;
            ShowParameterAlias = false;
            InitializeComponent();
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
            this._llstSearchCondition = new LinkedList();

            this.contextConfigurationUC.PageInfo(this.sessionData.GetXml(), "", this.URL, this.Port, this.FunctionName, "", "", "");
            this.ruleConfigurationUC.PageInfo(this.sessionData.GetXml(), "", this.URL, this.Port, this.FunctionName, "", "", "");
            this.optionalConfigurationUC.PageInfo(this.sessionData.GetXml(), "", this.URL, this.Port, this.FunctionName, "", "", "");
            this.autoCalcConfigurationUC.PageInfo(this.sessionData.GetXml(), "", this.URL, this.Port, this.FunctionName, "", "", "");
            this.ruleConfigurationUC.PageInfo(this.sessionData.GetXml(), "", this.URL, this.Port, this.FunctionName, "", "", "");

            this.InitializeCode();
            this.InitializeDataButton();
            this.InitializeBSpread();
            this.InitializeLayout();
        }


        public void InitializeLayout()
        {
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_ATT_MODELING, Definition.CONFIG_USE_COMMA, false);
        }

        private void InitializeCode()
        {
            
        }


        public void InitializeDataButton()
        {
            this.FunctionName = Definition.FUNC_KEY_SPC_ATT_MODELING;
        }



        public void InitializeBSpread()
        {
            
        }


        public void InitializeConfigData(BISTel.eSPC.Common.ConfigMode configMode)
        {
            //////-----Group 관련 기능 추가, KBLEE, Start
            LinkedList llParam = new LinkedList();
            llParam.Add(BISTel.eSPC.Common.COLUMN.LINE_RAWID, this._sLineRawID);
            llParam.Add(BISTel.eSPC.Common.COLUMN.AREA_RAWID, this._sAreaRawID);

            if (!String.IsNullOrEmpty(this._sEQPModel))
                llParam.Add(BISTel.eSPC.Common.COLUMN.EQP_MODEL, this._sEQPModel);

            bComboGroupList.Items.Clear();

            //Default로 Unssigned Model 추가.
            bComboGroupList.Items.Add(Definition.VARIABLE_UNASSIGNED_MODEL);

            _dsGroupList = new DataSet();
            _dsGroupList = this._wsSPC.GetATTGroupList(llParam.GetSerialData());

            if (_dsGroupList.Tables.Count > 0 && _dsGroupList.Tables[0].Rows.Count > 0)
            {
                //선택한 group을 제외한 group list를 추가.

                foreach (DataRow row in _dsGroupList.Tables[0].Rows)
                {
                    bComboGroupList.Items.Add(row[Definition.CONDITION_KEY_GROUP_NAME].ToString());
                }

                if (configMode == BISTel.eSPC.Common.ConfigMode.CREATE_MAIN || configMode == BISTel.eSPC.Common.ConfigMode.SAVE_AS || configMode == BISTel.eSPC.Common.ConfigMode.CREATE_MAIN_FROM)
                {
                    bComboGroupList.SelectedIndex = 0;
                }
                else        //Modify , Create Sub Model
                {
                    for (int i = 0; i < bComboGroupList.Items.Count; i++)
                    {
                        if (bComboGroupList.Items[i].ToString() == _sGroupName)
                        {
                            bComboGroupList.SelectedIndex = i;
                        }
                        else if (_sGroupName == Definition.VARIABLE_UNASSIGNED_MODEL)
                        {
                            bComboGroupList.SelectedIndex = 0;
                        }
                    }
                }
            }
            else
            {
                bComboGroupList.SelectedIndex = 0;
            }

            if (configMode != BISTel.eSPC.Common.ConfigMode.DEFAULT)
            {
                if (_sMainYN.ToString().ToUpper() == Definition.NO || configMode == BISTel.eSPC.Common.ConfigMode.ROLLBACK)
                {
                    this.bComboGroupList.Enabled = false;
                }
                else
                {
                    this.bComboGroupList.Enabled = true;
                }
            }
            else
            {
                this.bComboGroupList.Enabled = false;
            }

            //////-----Group 관련 기능 추가, KBLEE, End

            this._cofingMode = configMode;

            this.SetNormalizationInContextConfigUC(bShowNormalization);
            this.SetUnitOfSamples(sUnitOfSamples);

            this.SetConfigMode(configMode);

            switch (_cofingMode)
            {
                case BISTel.eSPC.Common.ConfigMode.DEFAULT:
                    this.gbSPCModelName.Visible = false;

                    this.SetConfigData();
                    break;
                case BISTel.eSPC.Common.ConfigMode.CREATE_MAIN:
                    this.SetConfigData();
                    break;

                case BISTel.eSPC.Common.ConfigMode.CREATE_SUB:

                    this.btxtSPCModelName.ReadOnly = true;
                    this.btxtSPCModelDesc.ReadOnly = true;

                    this.SetConfigData();
                    break;

                case BISTel.eSPC.Common.ConfigMode.MODIFY:
                    if (!(this._sMainYN == Definition.VARIABLE_Y))
                    {
                        this.btxtSPCModelName.ReadOnly = true;
                        this.btxtSPCModelDesc.ReadOnly = true;
                    }
                    this.SetConfigData();
                    break;
                case BISTel.eSPC.Common.ConfigMode.VIEW:
                    this.btxtSPCModelName.ReadOnly = true;
                    this.btxtSPCModelDesc.ReadOnly = true;
                    this.btxtChartDesc.ReadOnly = true;

                    this.SetConfigData();
                    break;

                case BISTel.eSPC.Common.ConfigMode.SAVE_AS:
                    this.SetConfigData();
                    break;

                case BISTel.eSPC.Common.ConfigMode.ROLLBACK:
                    this.btxtSPCModelName.ReadOnly = true;
                    this.btxtSPCModelDesc.ReadOnly = true;
                    this.btxtChartDesc.ReadOnly = true;
                    this.contextConfigurationUC.Enabled = false;
                    this.autoCalcConfigurationUC.Enabled = false;
                    this.optionalConfigurationUC.Enabled = false;
                    this.ruleConfigurationUC.Enabled = false;
                    this.SetConfigData();
                    break;
            }

            this.InitializeColumns();
        }

        //SPC-1292, KBLEE, START
        private void InitializeColumns()
        {
            _ruleColumns = new ArrayList();
            _ConfigColums = new ArrayList();
            _notCompareColumns = new ArrayList();

            _ruleColumns.Clear();
            _ruleColumns.Add("EWMA_LAMBDA");
            _ruleColumns.Add("MOVING_COUNT");
            _ruleColumns.Add("UPPER_SPEC");
            _ruleColumns.Add("LOWER_SPEC");
            _ruleColumns.Add("TARGET");
            _ruleColumns.Add("UPPER_CONTROL");
            _ruleColumns.Add("LOWER_CONTROL");
            _ruleColumns.Add("CENTER_LINE");
            _ruleColumns.Add("STD");

            _ruleColumns.Add("RAW_UCL");
            _ruleColumns.Add("RAW_LCL");
            _ruleColumns.Add("RAW_CENTER_LINE");
            _ruleColumns.Add("STD_UCL");
            _ruleColumns.Add("STD_LCL");
            _ruleColumns.Add("STD_CENTER_LINE");
            _ruleColumns.Add("RANGE_UCL");
            _ruleColumns.Add("RANGE_LCL");
            _ruleColumns.Add("RANGE_CENTER_LINE");
            _ruleColumns.Add("EWMA_M_UCL");
            _ruleColumns.Add("EWMA_M_LCL");
            _ruleColumns.Add("EWMA_M_CENTER_LINE");
            _ruleColumns.Add("EWMA_R_UCL");
            _ruleColumns.Add("EWMA_R_LCL");
            _ruleColumns.Add("EWMA_R_CENTER_LINE");
            _ruleColumns.Add("EWMA_S_UCL");
            _ruleColumns.Add("EWMA_S_LCL");
            _ruleColumns.Add("EWMA_S_CENTER_LINE");
            _ruleColumns.Add("MA_UCL");
            _ruleColumns.Add("MA_LCL");
            _ruleColumns.Add("MA_CENTER_LINE");
            _ruleColumns.Add("MS_UCL");
            _ruleColumns.Add("MS_LCL");
            _ruleColumns.Add("MS_CENTER_LINE");
            _ruleColumns.Add("MR_UCL");
            _ruleColumns.Add("MR_LCL");
            _ruleColumns.Add("MR_CENTER_LINE");
            _ruleColumns.Add("ZONE_A_UCL");
            _ruleColumns.Add("ZONE_A_LCL");
            _ruleColumns.Add("ZONE_B_UCL");
            _ruleColumns.Add("ZONE_B_LCL");
            _ruleColumns.Add("ZONE_C_UCL");
            _ruleColumns.Add("ZONE_C_LCL");

            _ruleColumns.Add("SPEC_USL_OFFSET");
            _ruleColumns.Add("SPEC_LSL_OFFSET");
            _ruleColumns.Add("RAW_UCL_OFFSET");
            _ruleColumns.Add("RAW_LCL_OFFSET");
            _ruleColumns.Add("MEAN_UCL_OFFSET");
            _ruleColumns.Add("MEAN_LCL_OFFSET");
            _ruleColumns.Add("STD_UCL_OFFSET");
            _ruleColumns.Add("STD_LCL_OFFSET");
            _ruleColumns.Add("RANGE_UCL_OFFSET");
            _ruleColumns.Add("RANGE_LCL_OFFSET");
            _ruleColumns.Add("EWMA_M_UCL_OFFSET");
            _ruleColumns.Add("EWMA_M_LCL_OFFSET");
            _ruleColumns.Add("EWMA_S_UCL_OFFSET");
            _ruleColumns.Add("EWMA_S_LCL_OFFSET");
            _ruleColumns.Add("EWMA_R_UCL_OFFSET");
            _ruleColumns.Add("EWMA_R_LCL_OFFSET");
            _ruleColumns.Add("MA_UCL_OFFSET");
            _ruleColumns.Add("MA_UCL_OFFSET");
            _ruleColumns.Add("MS_UCL_OFFSET");
            _ruleColumns.Add("MS_UCL_OFFSET");
            _ruleColumns.Add("MR_UCL_OFFSET");
            _ruleColumns.Add("MR_UCL_OFFSET");
            _ruleColumns.Add("ZONE_A_UCL_OFFSET");
            _ruleColumns.Add("ZONE_A_LCL_OFFSET");
            _ruleColumns.Add("ZONE_B_UCL_OFFSET");
            _ruleColumns.Add("ZONE_B_LCL_OFFSET");
            _ruleColumns.Add("ZONE_C_UCL_OFFSET");
            _ruleColumns.Add("ZONE_C_LCL_OFFSET");

            _ruleColumns.Add("UPPER_FILTER");
            _ruleColumns.Add("LOWER_FILTER");
            _ruleColumns.Add("UPPER_TECHNICAL_LIMIT");
            _ruleColumns.Add("LOWER_TECHNICAL_LIMIT");
            _ruleColumns.Add("OFFSET_YN");

            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.PARAM_TYPE_CD);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.PARAM_ALIAS);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.PARAM_LIST);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.COMPLEX_YN);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.REF_PARAM);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.REF_PARAM_LIST);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.AUTO_TYPE_CD);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.AUTO_SUB_YN);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.USE_EXTERNAL_SPEC_YN);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.INTERLOCK_YN);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.SAMPLE_COUNT);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.AUTOCALC_YN);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.MANAGE_TYPE_CD);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.ACTIVATION_YN);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.SUB_INTERLOCK_YN);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.INHERIT_MAIN_YN);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.SUB_AUTOCALC_YN);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.USE_NORM_YN);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.CHART_MODE_CD);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.VALIDATION_SAME_MODULE_YN);
            _ConfigColums.Add(BISTel.eSPC.Common.COLUMN.SPC_DATA_LEVEL);

            _notCompareColumns.Add(BISTel.eSPC.Common.COLUMN.RAWID);
            _notCompareColumns.Add(BISTel.eSPC.Common.COLUMN.VERSION);
            _notCompareColumns.Add(BISTel.eSPC.Common.COLUMN.CREATE_BY);
            _notCompareColumns.Add(BISTel.eSPC.Common.COLUMN.CREATE_DTTS);
            _notCompareColumns.Add(BISTel.eSPC.Common.COLUMN.LAST_UPDATE_BY);
            _notCompareColumns.Add(BISTel.eSPC.Common.COLUMN.LAST_UPDATE_DTTS);
            _notCompareColumns.Add(BISTel.eSPC.Common.COLUMN.SAVE_COMMENT);
            _notCompareColumns.Add(BISTel.eSPC.Common.COLUMN.CHANGED_ITEMS);
            _notCompareColumns.Add(BISTel.eSPC.Common.COLUMN.INPUT_DTTS);
            _notCompareColumns.Add(BISTel.eSPC.Common.COLUMN.MODEL_RULE_RAWID);
        }
        //SPC-1292, KBLEE, END

        private void SetConfigMode(BISTel.eSPC.Common.ConfigMode configMode)
        {
            this.contextConfigurationUC.CONFIG_MODE = configMode;
            this.ruleConfigurationUC.CONFIG_MODE = configMode;
            this.optionalConfigurationUC.CONFIG_MODE = configMode;
            this.autoCalcConfigurationUC.CONFIG_MODE = configMode;
        }

        private void SetNormalizationInContextConfigUC(bool bShowNormalization)
        {
            if (!bShowNormalization)
            {
                this.contextConfigurationUC.SetNormalization();
            }
        }
        
        private void SetUnitOfSamples(string sUnit)
        {
            //if(sUnit.)
            this.autoCalcConfigurationUC.SetUnitOfValue(sUnit);
        }

        private void SetCreateModel()
        {
            _dtModel = _wsSPC.GetTableSchema(BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC);
            _dtConfig = _wsSPC.GetTableSchema(BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC);
            _dtConfigOpt = _wsSPC.GetTableSchema(BISTel.eSPC.Common.TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC);
            _dtContext = _wsSPC.GetTableSchema(BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC);
            _dtRule = _wsSPC.GetTableSchema(BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC);
            _dtRuleOpt = _wsSPC.GetTableSchema(BISTel.eSPC.Common.TABLE.MODEL_RULE_OPT_ATT_MST_SPC);

            _dtRule.Columns.Add(BISTel.eSPC.Common.COLUMN.USE_MAIN_SPEC);
            _dtRule.Columns.Add(BISTel.eSPC.Common.COLUMN.RULE_OPTION);
            _dtRule.Columns.Add(BISTel.eSPC.Common.COLUMN.RULE_OPTION_DATA);
            _dtRule.Columns.Add(BISTel.eSPC.Common.COLUMN.DESCRIPTION);

            _dtRuleOpt.Columns.Add(BISTel.eSPC.Common.COLUMN.SPC_RULE_NO);
            _dtRuleOpt.Columns.Add(BISTel.eSPC.Common.COLUMN.OPTION_NAME);
            _dtRuleOpt.Columns.Add(BISTel.eSPC.Common.COLUMN.DESCRIPTION);


            DataRow newRow = _dtModel.NewRow();
            newRow[BISTel.eSPC.Common.COLUMN.LOCATION_RAWID] = _sLineRawID;
            newRow[BISTel.eSPC.Common.COLUMN.AREA_RAWID] = _sAreaRawID;
            _dtModel.Rows.Add(newRow);

            this.btxtSPCModelName.DataBindings.Add("Text", _dtModel, BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME);
            this.btxtSPCModelDesc.DataBindings.Add("Text", _dtModel, BISTel.eSPC.Common.COLUMN.DESCRIPTION);

            this.contextConfigurationUC.CONFIG_DATASET = _dtConfig;
            this.contextConfigurationUC.CONTEXT_DATASET = _dtContext;

            this.optionalConfigurationUC.CONFIG_OPT_DATASET = _dtConfigOpt;

            this.ruleConfigurationUC.CONFIG_DATASET = _dtConfig;
            this.ruleConfigurationUC.RULE_DATASET = _dtRule;
            this.ruleConfigurationUC.RULE_OPT_DATASET = _dtRuleOpt;


            this.contextConfigurationUC.LINE_RAWID = _sLineRawID;
            this.contextConfigurationUC.AREA_RAWID = _sAreaRawID;
        }

        private void DeleteColumnData(DataTable dtTable, params string[] columnNames)
        {
            if (dtTable.Rows.Count > 0)
            {
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    for (int j = 0; j < columnNames.Length; j++)
                    {
                        dtTable.Rows[i][columnNames[j]] = DBNull.Value;
                    }
                }

                dtTable.AcceptChanges();
            }
        }


        private void SetConfigData()
        {
            _llstSearchCondition.Clear();

            string spcModelName = "";
            DataSet dsTemp = new DataSet();

            switch (_cofingMode)
            {
                case BISTel.eSPC.Common.ConfigMode.DEFAULT:
                case BISTel.eSPC.Common.ConfigMode.CREATE_MAIN:
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_LINE_RAWID, this._sLineRawID);
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_AREA_RAWID, this._sAreaRawID);
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_EQP_MODEL, this._sEQPModel);
                    _llstSearchCondition.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

                    _dsSPCModelData = _wsSPC.GetATTSPCDefaultModelData(_llstSearchCondition.GetSerialData());
                    break;

                case BISTel.eSPC.Common.ConfigMode.CREATE_SUB:
                    //MAIN CONFIG 의 DATA를 먼저 LOAD한 후 AUTO SETTING TYPE이 DEFAULT일 경우
                    //DEFAULT DATA를 LOAD하여 MODEL 정보와 CONTEXT를 제외한 나머지 DATA는 DEFAULT로 대체한다
                    //단, CONTEXT VALUE는 Clear하고 Sub Config에서는 *를 사용하지 못하도록 함.

                    _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, this._sConfigRawID);
                    _llstSearchCondition.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

                    _dsSPCModelData = _wsSPC.GetSPCModelData(_llstSearchCondition.GetSerialData());

                    string autoType = _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.AUTO_TYPE_CD].ToString();

                    if (autoType.Equals("DEF"))
                    {
                        _llstSearchCondition.Clear();
                        _llstSearchCondition.Add(Definition.CONDITION_KEY_LINE_RAWID, this._sLineRawID);
                        _llstSearchCondition.Add(Definition.CONDITION_KEY_AREA_RAWID, this._sAreaRawID);
                        _llstSearchCondition.Add(Definition.CONDITION_KEY_EQP_MODEL, this._sEQPModel);
                        _llstSearchCondition.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);
                        DataSet dsDefault = _wsSPC.GetSPCDefaultModelData(_llstSearchCondition.GetSerialData());

                        if (!DataUtil.IsNullOrEmptyDataTable(dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC]))
                        {
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.UPPER_SPEC] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.UPPER_SPEC];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.LOWER_SPEC] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.LOWER_SPEC];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.UPPER_CONTROL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.UPPER_CONTROL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.LOWER_CONTROL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.LOWER_CONTROL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.SAMPLE_COUNT] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.SAMPLE_COUNT];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_LAMBDA] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_LAMBDA];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MOVING_COUNT] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MOVING_COUNT];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.CENTER_LINE] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.CENTER_LINE];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.STD] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.STD];

                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RAW_LCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RAW_LCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RAW_UCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RAW_UCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RAW_CENTER_LINE] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RAW_CENTER_LINE];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.STD_LCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.STD_LCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.STD_UCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.STD_UCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.STD_CENTER_LINE] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.STD_CENTER_LINE];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RANGE_LCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RANGE_LCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RANGE_UCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RANGE_UCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RANGE_CENTER_LINE] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.RANGE_CENTER_LINE];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_M_LCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_M_LCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_M_UCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_M_UCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_M_CENTER_LINE] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_M_CENTER_LINE];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_R_LCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_R_LCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_R_UCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_R_UCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_R_CENTER_LINE] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_R_CENTER_LINE];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_S_LCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_S_LCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_S_UCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_S_UCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_S_CENTER_LINE] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.EWMA_S_CENTER_LINE];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MA_LCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MA_LCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MA_UCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MA_UCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MA_CENTER_LINE] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MA_CENTER_LINE];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MS_LCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MS_LCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MS_UCL] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MS_UCL];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MS_CENTER_LINE] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.MS_CENTER_LINE];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.UPPER_TECHNICAL_LIMIT] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.UPPER_TECHNICAL_LIMIT];
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.LOWER_TECHNICAL_LIMIT] = dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.LOWER_TECHNICAL_LIMIT];
                        }
                        _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.DESCRIPTION] = "";


                        _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC].Clear();
                        _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC].Merge(dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC]);

                        _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC].Clear();
                        _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_AUTOCALC_ATT_MST_SPC].Merge(dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_AUTOCALC_ATT_MST_SPC]);

                        _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC].Clear();
                        _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC].Merge(dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC]);

                        _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_OPT_ATT_MST_SPC].Clear();
                        _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_OPT_ATT_MST_SPC].Merge(dsDefault.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_OPT_ATT_MST_SPC]);

                        _dsSPCModelData.AcceptChanges();

                        dsDefault.Clear();
                        dsDefault.Dispose();
                    }

                    foreach (DataRow drContexts in _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC].Rows)
                    {
                        drContexts[BISTel.eSPC.Common.COLUMN.CONTEXT_VALUE] = "";
                    }


                    _dsSPCModelData.AcceptChanges();

                    break;

                case BISTel.eSPC.Common.ConfigMode.SAVE_AS:
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, this._sConfigRawID);
                    _llstSearchCondition.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);

                    _dsSPCModelData = _wsSPC.GetSPCATTModelData(_llstSearchCondition.GetSerialData());

                    spcModelName = "Copy of " + this._dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString();
                    _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME] = spcModelName;

                    this.DeleteColumnData(_dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC],
                        BISTel.eSPC.Common.COLUMN.RAWID, BISTel.eSPC.Common.COLUMN.CREATE_BY, BISTel.eSPC.Common.COLUMN.CREATE_DTTS, BISTel.eSPC.Common.COLUMN.LAST_UPDATE_BY, BISTel.eSPC.Common.COLUMN.LAST_UPDATE_DTTS);

                    if(ShowParameterAlias)
                    {
                        if(_dsSPCModelData != null &&
                            _dsSPCModelData.Tables.Contains(BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC)  &&
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows.Count > 0)
                            this.contextConfigurationUC.PARAM_ALIAS =
                                _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC].Rows[0][BISTel.eSPC.Common.COLUMN.PARAM_ALIAS].ToString();
                    }
                    else
                    {
                        this.DeleteColumnData(_dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC], BISTel.eSPC.Common.COLUMN.PARAM_ALIAS);
                    }

                    this.DeleteColumnData(_dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC], BISTel.eSPC.Common.COLUMN.MODEL_RAWID, 
                        BISTel.eSPC.Common.COLUMN.CREATE_BY, BISTel.eSPC.Common.COLUMN.CREATE_DTTS, BISTel.eSPC.Common.COLUMN.LAST_UPDATE_BY,
                        BISTel.eSPC.Common.COLUMN.LAST_UPDATE_DTTS);

                    break;

                case BISTel.eSPC.Common.ConfigMode.ROLLBACK :
                    string version = _wsSPC.GetSPCLastestVersion(_sConfigRawID);
                    dsTemp = _wsSPC.GetSPCModelVersionData(_sConfigRawID, version);
                    _dsSPCModelData = _wsSPC.GetATTSPCModelVersionData(_sConfigRawID, _sVersion);
                    break;


                default:
                    _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, this._sConfigRawID);
                    _llstSearchCondition.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);
                    _dsSPCModelData = _wsSPC.GetSPCATTModelData(_llstSearchCondition.GetSerialData());
                    break;
            }


            this.contextConfigurationUC.CONFIG_RAWID = _sConfigRawID;
            this.ruleConfigurationUC.CONFIG_RAWID = _sConfigRawID;
            this.optionalConfigurationUC.CONFIG_RAWID = _sConfigRawID;
            this.autoCalcConfigurationUC.CONFIG_RAWID = _sConfigRawID;

            this.contextConfigurationUC.LINE_RAWID = _sLineRawID;
            this.contextConfigurationUC.AREA_RAWID = _sAreaRawID;
            this.contextConfigurationUC.EQP_MODEL = _sEQPModel;

            this.contextConfigurationUC.HAS_SUBCONFIGS = _hasSubConfigs;



            ///LOIOS///
            ////MODEL MST
            _dtModel = _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC];
                        
            ////CONFIG MST
            _dtConfig = _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC];

            ////CONTEXT MST
            _dtContext = _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC];


            ////CONFIG OPTION MST
            _dtConfigOpt = _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC];

            ////AUTO CALC MST
            _dtAutoCalc = _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_AUTOCALC_ATT_MST_SPC];

            ////RULE MST, RULE OPT MST
            _dtRule = _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC];
            _dtRuleOpt = _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_OPT_ATT_MST_SPC];


            _OridtModel = _dtModel.Copy();
            _OridtConfig = _dtConfig.Copy();
            _OridtContext = _dtContext.Copy();
            _OridtConfigOpt = _dtConfigOpt.Copy();
            _OridtAutoCalc = _dtAutoCalc.Copy();
            _OridtRule = _dtRule.Copy();
            _OridtRuleOpt = _dtRuleOpt.Copy();



            string sRuleOption = string.Empty;
            string sRuleOptionData = string.Empty;

            foreach (DataRow drRule in _dtRule.Rows)
            {
                DataRow[] drRuleOpts = _dtRuleOpt.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_RULE_RAWID, drRule[BISTel.eSPC.Common.COLUMN.RAWID]));

                foreach (DataRow drRuleOpt in drRuleOpts)
                {
                    sRuleOption += string.Format("{0}={1};", drRuleOpt[BISTel.eSPC.Common.COLUMN.OPTION_NAME], drRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE]);
                    sRuleOptionData += string.Format("{0}.{1}={2};", drRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO], drRuleOpt[BISTel.eSPC.Common.COLUMN.OPTION_NAME], drRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE]);
                }

                drRule[BISTel.eSPC.Common.COLUMN.RULE_OPTION] = sRuleOption;
                drRule[BISTel.eSPC.Common.COLUMN.RULE_OPTION_DATA] = sRuleOptionData;

                sRuleOption = string.Empty;
                sRuleOptionData = string.Empty;
            }

            if (_cofingMode == BISTel.eSPC.Common.ConfigMode.ROLLBACK)
            {
                _OridtModel = dsTemp.Tables[BISTel.eSPC.Common.TABLE.MODEL_MST_SPC].Copy();
                _OridtConfig = dsTemp.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_MST_SPC].Copy();
                _OridtContext = dsTemp.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_MST_SPC].Copy();
                _OridtConfigOpt = dsTemp.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONFIG_OPT_MST_SPC].Copy();
                _OridtAutoCalc = dsTemp.Tables[BISTel.eSPC.Common.TABLE.MODEL_AUTOCALC_MST_SPC].Copy();
                _OridtRule = dsTemp.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_MST_SPC].Copy();
                _OridtRuleOpt = dsTemp.Tables[BISTel.eSPC.Common.TABLE.MODEL_RULE_OPT_MST_SPC].Copy();

                foreach (DataRow drRule in _OridtRule.Rows)
                {
                    DataRow[] drRuleOpts = _OridtRuleOpt.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.MODEL_RULE_RAWID, drRule[BISTel.eSPC.Common.COLUMN.RAWID]));

                    foreach (DataRow drRuleOpt in drRuleOpts)
                    {
                        sRuleOption += string.Format("{0}={1};", drRuleOpt[BISTel.eSPC.Common.COLUMN.OPTION_NAME], drRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE]);
                        sRuleOptionData += string.Format("{0}.{1}={2};", drRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO], drRuleOpt[BISTel.eSPC.Common.COLUMN.OPTION_NAME], drRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE]);
                    }

                    drRule[BISTel.eSPC.Common.COLUMN.RULE_OPTION] = sRuleOption;
                    drRule[BISTel.eSPC.Common.COLUMN.RULE_OPTION_DATA] = sRuleOptionData;

                    sRuleOption = string.Empty;
                    sRuleOptionData = string.Empty;
                }
            }

            this.btxtSPCModelName.DataBindings.Add("Text", _dtModel, BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME);
            this.btxtSPCModelDesc.DataBindings.Add("Text", _dtModel, BISTel.eSPC.Common.COLUMN.DESCRIPTION);

            //SPC-676 
            this.btxtChartDesc.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.CHART_DESCRIPTON);

            this.contextConfigurationUC.CONTEXT_DATASET = _dtContext;
            //this.contextConfigurationUC.FILTER_DATASET = _dtFilter;
            this.contextConfigurationUC.CONFIG_DATASET = _dtConfig;

            this.optionalConfigurationUC.CONFIG_OPT_DATASET = _dtConfigOpt;
            this.autoCalcConfigurationUC.AUTOCALC_DATASET = _dtAutoCalc;

            this.ruleConfigurationUC.CONFIG_DATASET = _dtConfig;
            this.ruleConfigurationUC.RULE_DATASET = _dtRule;
            this.ruleConfigurationUC.RULE_OPT_DATASET = _dtRuleOpt;

            this.contextConfigurationUC.InitializeLayout(_cofingMode);
            this.optionalConfigurationUC.InitializeLayout(_cofingMode);
            this.ruleConfigurationUC.InitializeLayout(_cofingMode);
            this.autoCalcConfigurationUC.InitializeLayout(_cofingMode);
        }
               

        public bool SaveConfigData()
        {
            bool _isGroup = false;
            //DEFAULT가 아닐때 입력 Validation
            if (!_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.DEFAULT))
            {
                if (this.btxtSPCModelName.Text.Length.Equals(0))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_MODEL_NAME", null, null);
                    return false;
                }


                if (!contextConfigurationUC.InputValidation(true, false))
                {
                    return false;
                }

                if (!ruleConfigurationUC.InputValidation())
                {
                    return false;
                }

                if (!optionalConfigurationUC.InputValidation())
                {
                    return false;
                }
            }
            else
            {
                if (!contextConfigurationUC.InputValidation(true, true))
                {
                    return false;
                }
                if (!ruleConfigurationUC.InputValidation())
                {
                    return false;
                }
                if (contextConfigurationUC.AUTOCALC_YN.Equals("Y"))
                {
                    if (!autoCalcConfigurationUC.InputValidation(true)) return false;
                }
                if (!optionalConfigurationUC.InputValidation())
                {
                    return false;
                }
            }

                     

            this._dtConfig = this.contextConfigurationUC.CONFIG_DATASET;
            this._dtContext = this.contextConfigurationUC.CONTEXT_DATASET;
            this._dtConfigOpt = this.optionalConfigurationUC.CONFIG_OPT_DATASET;
            this._dtRule = this.ruleConfigurationUC.RULE_DATASET;
            this._dtRuleOpt = this.ruleConfigurationUC.RULE_OPT_DATASET;
            this._dtAutoCalc = this.autoCalcConfigurationUC.AUTOCALC_DATASET;

            

            _dtModel.AcceptChanges();
            _dtConfig.AcceptChanges();
            _dtConfigOpt.AcceptChanges();
            _dtContext.AcceptChanges();
            _dtRule.AcceptChanges();
            _dtRuleOpt.AcceptChanges();
            _dtAutoCalc.AcceptChanges();

            this.contextConfigurationUC.SetCheckBoxColumn();
            this.ruleConfigurationUC.RULE_DATASET = _dtRule;
            this.ruleConfigurationUC.RULE_OPT_DATASET = _dtRuleOpt;
            this.ruleConfigurationUC.SetCheckBoxColumn();

            bool DataComparer = DataComparerModel();

            if (!DataComparer || _cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.ROLLBACK) || _cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.DEFAULT)) //SPC-1292, KBLEE, 조건문 추가
            {
                bool result = true;
                LinkedList llSaveData = new LinkedList();
                llSaveData.Add(Definition.CONDITION_KEY_USER_ID, sessionData.UserId);
                llSaveData.Add(Definition.CONDITION_KEY_CONFIG_MODE, _cofingMode);
                if (this._sMainYN == null && (_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.CREATE_MAIN) || _cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.SAVE_AS) || _cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.DEFAULT)))
                    this._sMainYN = "Y";
                llSaveData.Add(Definition.CONDITION_KEY_MAIN_YN, this._sMainYN);
                llSaveData.Add(Definition.CONDITION_KEY_HAS_SUBCONFIG, this._hasSubConfigs.ToString());
                llSaveData.Add(BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC, _dtModel);
                if(this._cofingMode == BISTel.eSPC.Common.ConfigMode.SAVE_AS)
                {
                    this._dtModel.Rows[0]["LOCATION_RAWID"] = this._sLineRawID;
                    this._dtModel.Rows[0]["EQP_MODEL"] = this._sEQPModel;
                    this._dtModel.Rows[0]["AREA_RAWID"] = this._sAreaRawID;
                }

                llSaveData.Add(BISTel.eSPC.Common.TABLE.MODEL_CONFIG_ATT_MST_SPC, _dtConfig);
                llSaveData.Add(BISTel.eSPC.Common.TABLE.MODEL_CONFIG_OPT_ATT_MST_SPC, _dtConfigOpt);
                llSaveData.Add(BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC, _dtContext);
                llSaveData.Add(BISTel.eSPC.Common.TABLE.MODEL_AUTOCALC_ATT_MST_SPC, _dtAutoCalc);
                llSaveData.Add(BISTel.eSPC.Common.TABLE.MODEL_RULE_ATT_MST_SPC, _dtRule);
                llSaveData.Add(BISTel.eSPC.Common.TABLE.MODEL_RULE_OPT_ATT_MST_SPC, _dtRuleOpt);

                //SPC-1292, KBLEE, START
                string sChangedItems = this.SetChangedItems();
                if (string.IsNullOrEmpty(sChangedItems))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_CHANGE_DATA", null, null);
                    return false;
                }
                //SPC-1292, KBLEE, END

                if (_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.MODIFY))
                    llSaveData.Add("CHANGED_MASTER_COL_LIST", this.ruleConfigurationUC.CHANGED_MASTER_COL_LIST);

                if (_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.CREATE_MAIN) || _cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.SAVE_AS) ||
                    (_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.MODIFY) && this._sMainYN.Equals("Y")) || (_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.ROLLBACK) && this._sMainYN.Equals("Y")))
                {
                    string strCheckResult = _wsSPC.CheckDuplicateATTSPCModel(llSaveData.GetSerialData());
                    if (strCheckResult.Length > 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_EXIST_SAME_MODEL_CONTEXT", new string[]{ strCheckResult }, null);
                        result = false;
                        return result;
                    }
                }

                if ((_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.MODIFY) || _cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.ROLLBACK))&& this._sMainYN.Equals("Y") && this._hasSubConfigs)
                {
                    if (_dtContext != null && _dtContext.Rows.Count > 0)
                    {
                        for (int i = 0; i < _dtContext.Rows.Count; i++)
                        {
                            if (_dtContext.Rows[i][BISTel.eSPC.Common.COLUMN.GROUP_YN] != null && _dtContext.Rows[i][BISTel.eSPC.Common.COLUMN.GROUP_YN].ToString().Length > 0)
                            {
                                if (Convert.ToBoolean(_dtContext.Rows[i][BISTel.eSPC.Common.COLUMN.GROUP_YN].ToString()))
                                {
                                    string strGroup_YNOri   = "";
                                    DataRow[] drArr = this._dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC].Select(string.Format("CONTEXT_KEY = '{0}'", _dtContext.Rows[i]["CONTEXT_KEY"].ToString()));
                                    if (drArr.Length > 0)
                                    {
                                        strGroup_YNOri = drArr[0][BISTel.eSPC.Common.COLUMN.GROUP_YN].ToString();
                                        if (strGroup_YNOri == Definition.VARIABLE_N || strGroup_YNOri.Length == 0)
                                        {
                                            DialogResult dresult = MSGHandler.DialogQuestionResult("SPC_INFO_DIALOG_SAVE_CHECK_GROUP_MODEL", new string[] { "SAVE" }, MessageBoxButtons.YesNo);
                                            if (!(dresult == DialogResult.Yes))
                                            {
                                                return false;
                                            }
                                            else
                                            {
                                                llSaveData.Add(BISTel.eSPC.Common.COLUMN.GROUP_YN, _dtRuleOpt);
                                                _isGroup = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                

                if ((_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.MODIFY) || _cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.ROLLBACK)) && this._sMainYN.Equals("Y") && this._hasSubConfigs)
                {
                    bool isGroupModify = false;

                    for (int i = 0; i < _dtContext.Rows.Count; i++)
                    {
                        if (_dtContext.Rows[i][BISTel.eSPC.Common.COLUMN.GROUP_YN] != null && _dtContext.Rows[i][BISTel.eSPC.Common.COLUMN.GROUP_YN].ToString().Length > 0)
                        {
                            if (Convert.ToBoolean(_dtContext.Rows[i][BISTel.eSPC.Common.COLUMN.GROUP_YN].ToString()))
                            {
                                string strGroup_YNOri = "";
                                DataRow[] drArrTemp = this._dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_CONTEXT_ATT_MST_SPC].Select(string.Format("CONTEXT_KEY = '{0}'", _dtContext.Rows[i]["CONTEXT_KEY"].ToString()));
                                if (drArrTemp.Length > 0)
                                {
                                    strGroup_YNOri = drArrTemp[0][BISTel.eSPC.Common.COLUMN.GROUP_YN].ToString();
                                    if (strGroup_YNOri == Definition.VARIABLE_Y)
                                    {
                                        if (_dtContext.Rows[i][BISTel.eSPC.Common.COLUMN.CONTEXT_VALUE].ToString().Trim() != drArrTemp[0][BISTel.eSPC.Common.COLUMN.CONTEXT_VALUE].ToString().Trim() ||
                                            _dtContext.Rows[i][BISTel.eSPC.Common.COLUMN.EXCLUDE_LIST].ToString().Trim() != drArrTemp[0][BISTel.eSPC.Common.COLUMN.EXCLUDE_LIST].ToString().Trim())
                                        {
                                            isGroupModify = true;
                                            _dtContext.Rows[i]["_MODIFY"] = "True";
                                        }
                                    }
                                }
                            }
                        }
                    }

                    DataRow[] drArr = this._dtContext.Select("_INSERT = 'True' AND GROUP_YN = 'True'");

                    if (drArr.Length > 0 || isGroupModify)
                    {
                        DialogResult dresultContext = MSGHandler.DialogQuestionResult("SPC_INFO_DIALOG_GROUPING_CONTEXT_MAIN_OR_SUB", new string[] { "SAVE" }, MessageBoxButtons.YesNoCancel);

                        if (dresultContext == DialogResult.Yes)
                        {
                            llSaveData.Add("ONLY_MAIN_GROUP", "N");
                        }
                        else if (dresultContext == DialogResult.No)
                        {
                            llSaveData.Add("ONLY_MAIN_GROUP", "Y");
                        }
                        else
                        {
                            return false;
                        }

                    }

                    DialogResult dresult = MSGHandler.DialogQuestionResult("SPC_INFO_DIALOG_APPLY_SPEC_MAIN_OR_SUB", new string[] { "SAVE" }, MessageBoxButtons.YesNoCancel);

                    if (dresult == DialogResult.Yes)
                    {
                        llSaveData.Add("ONLY_MAIN", "N");
                    }
                    else if (dresult == DialogResult.No)
                    {
                        llSaveData.Add("ONLY_MAIN", "Y");
                    }
                else
                {
                    return false;
                }
            }

            //////-----SPC-1292, KBLEE, Start
            string sGroup = string.Empty;
            string sGroupRawid = string.Empty;

            if (CONFIG_MODE != BISTel.eSPC.Common.ConfigMode.DEFAULT)
            {
                sGroup = this.bComboGroupList.SelectedItem.ToString();
            }

            if (String.IsNullOrEmpty(this._sGroupName) || sGroup != this._sGroupName || (sGroup != Definition.VARIABLE_UNASSIGNED_MODEL))         //Model을 선택하지않고 Search한 경우. (Create SPC Model만 가능)
            {
                DataRow[] dr = this._dsGroupList.Tables[0].Select(string.Format("GROUP_NAME = '{0}'", sGroup));

                if (dr.Length > 0)
                {
                    sGroupRawid = dr[0][BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    llSaveData.Add(BISTel.eSPC.Common.COLUMN.GROUP_RAWID, sGroupRawid);
                }
            }

            llSaveData.Add(Definition.VARIABLE_USE_COMMA, _bUseComma);
            //////-------
            //llSaveData.Add(BISTel.eSPC.Common.COLUMN.CHANGED_ITEMS, sChangedItems);
            //////-----SPC-1292, KBLEE, End
           
            base.MsgShow(this._mlthandler.GetMessage(Definition.MSG_KEY_INFO_SAVING_DATA));

            try
            {

                byte[] baData = llSaveData.GetSerialData();
                DataSet dsResult = _wsSPC.SaveATTSPCModelData(baData);

                base.MsgClose();

                if (DSUtil.GetResultSucceed(dsResult) == 0)
                {
                    result = false;
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_FAIL_SAVE_CHANGE_DATA));
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));

                    //////-----Group 관련 기능 추가, KBLEE, Start
                    this._sGroupName = sGroup;
                    if (this.ParentForm is SPCConfigurationPopup)
                    {
                        ((SPCConfigurationPopup)this.ParentForm).GROUP_NAME = _sGroupName;
                    }
                    //////-----Group 관련 기능 추가, KBLEE, End

                    if (_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.CREATE_MAIN) || _cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.SAVE_AS))
                    {
                        if (_dsSPCModelData.Tables.Contains(BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC))
                        {
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC].Clear();
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC].Merge(dsResult.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC]);
                            _dsSPCModelData.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC].AcceptChanges();
                        }
                        else
                            _dsSPCModelData.Tables.Add(dsResult.Tables[BISTel.eSPC.Common.TABLE.MODEL_ATT_MST_SPC].Copy());
                    }
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

                return result;
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_CHANGE_DATA", null, null);
                return false;
            }
          
        }

        //SPC-1292, KBLEE
        private string SetChangedItems()
        {
            string sItems = string.Empty;

            if (_isConfig)
                sItems += "Config,";
            if (_isContext)
                sItems += "Context,";
            if (_isOption)
                sItems += "Option,";
            if (_isAutoCalc)
                sItems += "AutoCalc,";
            if (_isRule)
                sItems += "Rule,";
            if (_isLimit)
                sItems += "Limit,";
            if (_isETC)
                sItems += "ETC,";

            if (sItems.Length > 0)
                sItems = sItems.Substring(0, sItems.Length - 1);
            return sItems;
        }

        public bool DataComparerModel()
        {
            bool isSameData = true; //SPC-1292, KBLEE
            
            for (int i = 0; i < _OridtModel.Rows.Count; i++)
            {
                for (int j = 0; j < _OridtModel.Columns.Count; j++)
                {
                    string sNewData = _dtModel.Rows[i][j].ToString();
                    string sOldData = _OridtModel.Rows[i][j].ToString();

                    if (!sOldData.Equals(sNewData))
                    {
                        this._isETC = true;
                        isSameData = false;
                        //return false;
                    }

                }
            }

            for (int i = 0; i < _OridtConfig.Rows.Count; i++)
            {
                for (int j = 0; j < _OridtConfig.Columns.Count; j++)
                {
                   
                    string sNewData = _dtConfig.Rows[i][j].ToString();
                    string sOldData = _OridtConfig.Rows[i][j].ToString();

                    if (!sOldData.Equals(sNewData))
                    {
                        if (this._ruleColumns.Contains(_OridtConfig.Columns[j].ColumnName.ToString()))
                            this._isLimit = true;
                        else if (_OridtConfig.Columns[j].ColumnName.ToString() == BISTel.eSPC.Common.COLUMN.CHART_DESCRIPTON)
                            this._isETC = true;
                        else if (!this._notCompareColumns.Contains(_OridtConfig.Columns[j].ColumnName.ToString()))
                            this._isConfig = true;

                        isSameData = false;

                        //return false;
                    }

                }
            }
            for (int i = 0; i < _OridtConfigOpt.Rows.Count; i++)
            {
                for (int j = 0; j < _OridtConfigOpt.Columns.Count; j++)
                {
                   
                    string sNewData = _dtConfigOpt.Rows[i][j].ToString();
                    string sOldData = _OridtConfigOpt.Rows[i][j].ToString();

                    if (!sOldData.Equals(sNewData) && !this._notCompareColumns.Contains(_OridtConfigOpt.Columns[j].ColumnName.ToString()))
                    {
                        this._isOption = true;
                        isSameData = false;

                        //return false;
                    }

                }
            }
            if (_OridtContext.Rows.Count == _dtContext.Rows.Count)
            {
                for (int i = 0; i < _OridtContext.Rows.Count; i++)
                {

                    for (int j = 0; j < _OridtContext.Columns.Count; j++)
                    {

                        string sNewData = _dtContext.Rows[i][j].ToString();
                        string sOldData = _OridtContext.Rows[i][j].ToString();

                        if (sNewData.Equals("False") || string.IsNullOrEmpty(sNewData))
                            sNewData = "N";
                        else if (sNewData.Equals("True"))
                            sNewData = "Y";

                        if (string.IsNullOrEmpty(sOldData))
                            sOldData = "N";

                        if (!sOldData.Equals(sNewData) && !this._notCompareColumns.Contains(_OridtContext.Columns[j].ColumnName.ToString()))
                        {
                            this._isContext = true;
                            isSameData = false;

                            //return false;
                        }

                    }
                }
            }
            else
            {
                this._isContext = true;
                isSameData = false; 
                //return false;
            }
            if (_OridtRule.Rows.Count != _dtRule.Rows.Count)
            {
                this._isRule = true;
                isSameData = false; 
                //return false;
            }
            else
            {
                for (int i = 0; i < _OridtRule.Rows.Count; i++)
                {
                    for (int j = 0; j < _OridtRule.Columns.Count - 2; j++)
                    {
                        if (_dtRule.Columns[j].ColumnName.Equals("RULE_OPTION") || _dtRule.Columns[j].ColumnName.Equals("RULE_OPTION_DATA"))
                        {
                            continue;
                        }

                        string sNewData = _dtRule.Rows[i][j].ToString();
                        string sOldData = _OridtRule.Rows[i][j].ToString();

                        if (!sOldData.Equals(sNewData) && !this._notCompareColumns.Contains(_OridtRule.Columns[j].ColumnName.ToString()))
                        {
                            this._isRule = true;
                            isSameData = false;
                            //return false;
                        }

                    }
                }
            }
            if (_OridtRuleOpt.Rows.Count != _dtRuleOpt.Rows.Count)
            {
                this._isRule = true;
                isSameData = false;
            }
            else
            {
                for (int i = 0; i < _OridtRuleOpt.Rows.Count; i++)
                {
                    for (int j = 0; j < _OridtRuleOpt.Columns.Count; j++)
                    {

                        string sNewData = _dtRuleOpt.Rows[i][j].ToString();
                        string sOldData = _OridtRuleOpt.Rows[i][j].ToString();

                        if (!sOldData.Equals(sNewData) && !this._notCompareColumns.Contains(_OridtRuleOpt.Columns[j].ColumnName.ToString()))
                        {
                            this._isRule = true;
                            isSameData = false;
                        }

                    }
                }
            }
            //for (int i = 0; i < _OridtRuleOpt.Rows.Count; i++)
            //{
            //    for (int j = 0; j < _OridtRuleOpt.Columns.Count; j++)
            //    {
                   
            //        string sNewData = _dtRuleOpt.Rows[i][j].ToString();
            //        string sOldData = _OridtRuleOpt.Rows[i][j].ToString();

            //        if (!sOldData.Equals(sNewData))
            //        {
            //            return false;
            //        }

            //    }
            //}
            for (int i = 0; i < _OridtAutoCalc.Rows.Count; i++)
            {
                for (int j = 0; j < _OridtAutoCalc.Columns.Count; j++)
                {
                   
                    string sNewData = _dtAutoCalc.Rows[i][j].ToString();
                    string sOldData = _OridtAutoCalc.Rows[i][j].ToString();

                    if (string.IsNullOrEmpty(sNewData))
                        sNewData = "0";

                    if (string.IsNullOrEmpty(sOldData))
                        sOldData = "0";

                    if (!sOldData.Equals(sNewData) && !this._notCompareColumns.Contains(_OridtAutoCalc.Columns[j].ColumnName.ToString())
                        && !_OridtAutoCalc.Columns[j].ColumnName.ToString().EndsWith("CNT"))
                    {
                        this._isAutoCalc = true;
                        isSameData = false;
                        //return false;
                    }

                }
            }

            if (CONFIG_MODE != BISTel.eSPC.Common.ConfigMode.DEFAULT)
            {
                if (_sGroupName != this.bComboGroupList.SelectedItem.ToString())
                {
                    this._isETC = true;
                    isSameData = false;
                }

            }

            return isSameData;
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
        public bool bShowNormalization { get; set; }
        public string sUnitOfSamples { get; set; }
        public string sOCAPOfSingle { get; set; }

        #endregion


        #region ::: User Defined Method.

        #endregion

        #region ::: EventHandler

        #endregion



        public LinkedList ModelConfigCondition
        {
            get { return _ModelConfigCondition; }
            set { _ModelConfigCondition = value; }
        }

        //Group 관련 기능 추가, KBLEE
        public string GROUP_NAME
        {
            get { return _sGroupName; }
            set { _sGroupName = value; }
        }

        private void bTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RULE Tab일경우 Use External Spec Y 인 경우 Spec Limit을 disable 시킨다
            if (bTabControl.SelectedIndex.Equals(1))
            {
                if(sOCAPOfSingle.ToUpper().Equals("SINGLE"))
                    ruleConfigurationUC._sOCAPOfSIngle = true;
            }
        }

        public void SetCheckBoxColumn()
        {
            contextConfigurationUC.SetCheckBoxColumn();
            ruleConfigurationUC.SetCheckBoxColumn();
        }
    }

}
