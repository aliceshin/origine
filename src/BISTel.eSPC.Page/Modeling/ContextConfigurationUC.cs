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


using BISTel.eSPC.Common;

using Steema.TeeChart;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.Modeling
{
    public enum SPCModelContextSpread
    {
        V_INSERT = 0,
        V_MODIFY,
        V_DELETE,
        V_SELECT,
        CONTEXT_KEY_NAME,
        CONTEXT_VALUE,
        EXCLUDE_LIST,
        KEY_ORDER,
        CONTEXT_KEY,
        GROUP_YN
    }

    //public enum SPCModelFilterSpread
    //{
    //    V_INSERT = 0,
    //    V_MODIFY,
    //    V_DELETE,
    //    V_SELECT,
    //    FILTER_KEY_NAME,
    //    FILTER_VALUE,
    //    FILTER_KEY
    //}
    public partial class ContextConfigurationUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition = new LinkedList();
        LinkedList _llstContextType = new LinkedList();
        //LinkedList _llstFilterType = new LinkedList();

        SPCStruct.ContextTypeInfo _contextTypeInfo = null;
        //SPCStruct.FilterTypeInfo _filterTypeInfo = null;


        BISTel.eSPC.Common.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.CommonUtility _ComUtil;

        private SortedList _slColumnIndex = new SortedList();
        //private SortedList _slColumnIndexFilter = new SortedList();
        private SortedList _slBtnListIndex = new SortedList();
        //private SortedList _slBtnListIndexFilter = new SortedList();



        private int _ColIdx_SELECT;
        private int _ColIdx_CONTEXT_KEY;
        private int _ColIdx_GROUP_YN;
        //private int _ColIdx_FILTER_SELECT;
        //private int _ColIdx_FILTER_KEY;
        //private int _ColIdx_CONTEXT_VALUE;
        //private int _ColIdx_EXCLUDE_LIST;
        //private int _ColIdx_ORDER;

        private ConfigMode _cofingMode;
        private string _sConfigRawID;

        private string _sAreaRawID;
        private string _sLineRawID;
        private string _sEQPModel;

        private string _ModelingType;

       
        private string[] _saContextTypeName;
        //private string[] _saFilterTypeName;

        private DataTable _dtConfig;

        private bool _hasSubConfigs;
        private bool _isTraceSum = true;
        private string _ParamAliasT = "";

        FarPoint.Win.Spread.CellType.TextCellType tc = new FarPoint.Win.Spread.CellType.TextCellType();

        private DataTable _dtOriginalData = new DataTable();
        private bool _bUseComma;

        #endregion


        #region ::: Properties


        #endregion


        #region ::: Constructor

        public ContextConfigurationUC()
        {
            InitializeComponent();
            

        }

        #endregion

        #region ::: Override Method

        public override void PageInit()
        {
            //BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;

            //this.KeyOfPage = this.GetType().FullName; // 필수 입력 (Key값을 받은 후 DC을 Active해주기 때문이다.

            this.InitializePage();

            tc.MaxLength = 1024;
        }

        //public override BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ADynamicCondition CreateCustomCondition()
        //{
        //    return new BISTel.eSPC.Condition.Modeling.SPCModelCondition();			
        //}

        #endregion

        #region ::: PageLoad & Initialize

        public void InitializePage()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();

            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._SpreadUtil = new BSpreadUtility();
            this._ComUtil = new CommonUtility();

            this.InitializeBSpread();
            this.InitializeCode();
            this.InitializeDataButton();

            //this.InitializeLayout();
        }

        /// <summary>
        /// 2009-12-07 bskwon 수정 
        /// Context Key : Display : name, Save:code로 함.
        /// </summary>
        private void InitializeCode()
        {
            LinkedList llconidtion = new LinkedList();

            //PARAM TYPE
            if(_ModelingType.Equals("MET"))
            {
                llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_MET_PARAM_TYPE);
            }
            else
            {
                llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_PARAM_TYPE);
            }

            llconidtion.Add(Definition.CONDITION_KEY_USE_YN, Definition.VARIABLE_Y);
                        

            DataSet _dsParamType = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            _ComUtil.SetBComboBoxData(this.bcboParamType, _dsParamType, COLUMN.NAME, COLUMN.CODE, "", true);

            //MANAGE TYPE
            llconidtion.Clear();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_MANAGE_TYPE);
            llconidtion.Add(Definition.CONDITION_KEY_USE_YN, Definition.VARIABLE_Y);

            DataSet _dsDisplayType = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            _ComUtil.SetBComboBoxData(this.bcboManageType, _dsDisplayType, COLUMN.NAME, COLUMN.CODE, "", true);

            //AUTO TYPE
            llconidtion.Clear();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_AUTO_TYPE);
            llconidtion.Add(Definition.CONDITION_KEY_USE_YN, Definition.VARIABLE_Y);

            DataSet _dsAutoType = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            _ComUtil.SetBComboBoxData(this.bcboAutoType, _dsAutoType, COLUMN.NAME, COLUMN.CODE, "", true);

            // CHART MODE
            llconidtion.Clear();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CHART_MODE);
            llconidtion.Add(Definition.CONDITION_KEY_USE_YN, Definition.VARIABLE_Y);

            DataSet _dsChartMode = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            _ComUtil.SetBComboBoxData(this.bcboMode, _dsChartMode, COLUMN.NAME, COLUMN.CODE, "", true);

            //SPC_DATA_LEVEL
            DataSet dsDataLevel = new DataSet();
            DataTable dtDataLevel = new DataTable();
            dtDataLevel.Columns.Add(COLUMN.NAME);
            dtDataLevel.Columns.Add(COLUMN.CODE);
            for (int idxTemp = 0; idxTemp < 2; idxTemp++)
            {
                DataRow drDataLevel = dtDataLevel.NewRow();
                switch(idxTemp)
                {
                    case 0:
                        drDataLevel[0] = this._mlthandler.GetVariable(Definition.SPC_DATA_LEVEL.SPC_DATA_LEVEL_L);
                        drDataLevel[1] = "L";
                        break;
                    case 1:
                        drDataLevel[0] = this._mlthandler.GetVariable(Definition.SPC_DATA_LEVEL.SPC_DATA_LEVEL_W);
                        drDataLevel[1] = "W";
                        break;
                }
                dtDataLevel.Rows.Add(drDataLevel);
            }
            dsDataLevel.Tables.Add(dtDataLevel);

            _ComUtil.SetBComboBoxData(this.bcboDataLevel, dsDataLevel, COLUMN.NAME, COLUMN.CODE, "", true);

            //CONTEXT TYPE
            llconidtion.Clear();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CONTEXT_TYPE);
            llconidtion.Add(Definition.CONDITION_KEY_USE_YN, Definition.VARIABLE_Y);

            DataSet _dsContextTempType1 = this._wsSPC.GetCodeData(llconidtion.GetSerialData());

            llconidtion.Clear();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_SPC_CONTEXT_TYPE);
            llconidtion.Add(Definition.CONDITION_KEY_USE_YN, Definition.VARIABLE_Y);

            DataSet _dsContextTempType2 = this._wsSPC.GetCodeData(llconidtion.GetSerialData());

            DataSet _dsContextTempType3 = new DataSet();
            _dsContextTempType3.Merge(_dsContextTempType1);
            _dsContextTempType3.Merge(_dsContextTempType2);

            DataSet _dsContextType = _dsContextTempType3.Clone();
            DataRow[] arrDr = _dsContextTempType3.Tables[0].Select("CATEGORY='CONTEXT_TYPE' OR CATEGORY = 'SPC_CONTEXT_TYPE' ", "NAME");
            for (int k = 0; k < arrDr.Length; k++)
            {
                if (this.llstContextType.Contains(arrDr[k][COLUMN.NAME].ToString())) continue;

                this._contextTypeInfo = new SPCStruct.ContextTypeInfo(arrDr[k][COLUMN.CODE].ToString(), arrDr[k][COLUMN.NAME].ToString());
                this.llstContextType.Add(arrDr[k][COLUMN.NAME].ToString(), this._contextTypeInfo);

                _dsContextType.Tables[0].Rows.Add(arrDr[k].ItemArray);
            }

            //for (int k = 0; k < arrDr.Length; k++)
            //{
            //    if (this.llstFilterType.Contains(arrDr[k][COLUMN.NAME].ToString())) continue;

            //    this._filterTypeInfo = new SPCStruct.FilterTypeInfo(arrDr[k][COLUMN.CODE].ToString(), arrDr[k][COLUMN.NAME].ToString());
            //    this.llstFilterType.Add(arrDr[k][COLUMN.NAME].ToString(), this._filterTypeInfo);
            //}

            _saContextTypeName = _ComUtil.ConvertDataColumnIntoArray(_dsContextType.Tables[0], COLUMN.NAME);
            //_saFilterTypeName = _ComUtil.ConvertDataColumnIntoArray(_dsContextType.Tables[0], COLUMN.NAME);
        }


        public void InitializeDataButton()
        {
            this._slBtnListIndex = this._Initialization.InitializeButtonList(this.bbtnListContext, ref bsprContext, Definition.PAGE_KEY_SPC_CONFIGURATION, Definition.PAGE_KEY_SPC_CONFIGURATION_CONTEXT, this.sessionData);
            //this._slBtnListIndexFilter = this._Initialization.InitializeButtonList(this.bbtnListFilter, ref bsprFilter, Definition.PAGE_KEY_SPC_CONFIGURATION, Definition.PAGE_KEY_SPC_CONFIGURATION_FILTER, this.sessionData);

            this.FunctionName = Definition.FUNC_KEY_SPC_MODELING;
            //this.ApplyAuthory(this.bbtnListContext);
            //this.ApplyAuthory(this.bbtnListFilter);
        }

        public void InitializeBSpread()
        {
            this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprContext, Definition.PAGE_KEY_SPC_CONFIGURATION, true, Definition.PAGE_KEY_SPC_CONFIGURATION_CONTEXT);
            this.bsprContext.UseHeadColor = true;

            this.bsprContext.UseAutoSort = false;
            this.bsprContext.RowInsertType = InsertType.Last;

            this._Initialization.SetCheckColumnHeader(this.bsprContext, 0);

            //SPC-760 by Louisyou
            this.bsprContext.UseGeneralContextMenu = false;

            this._ColIdx_CONTEXT_KEY = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_CONTEXT_KEY)];
            this._ColIdx_SELECT = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT)];
            this._ColIdx_GROUP_YN = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_SPC_GROUP)];

            //this._slColumnIndexFilter = this._Initialization.InitializeColumnHeader(ref bsprFilter, Definition.PAGE_KEY_SPC_CONFIGURATION, true, Definition.PAGE_KEY_SPC_CONFIGURATION_FILTER);
            //this.bsprFilter.UseHeadColor = true;

            //this.bsprFilter.UseAutoSort = false;
            //this.bsprFilter.RowInsertType = InsertType.Last;

            //this._Initialization.SetCheckColumnHeader(this.bsprFilter, 0);

            //this._ColIdx_FILTER_KEY = (int)this._slColumnIndexFilter[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_FILTER_KEY)];
            //this._ColIdx_FILTER_SELECT = (int)this._slColumnIndexFilter[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT)];

            this.bsprContext.SelectionBlockOptions = FarPoint.Win.Spread.SelectionBlockOptions.None;


        }

        #endregion

        private void SetContextKeyOrder()
        {
            for (int i = 0; i < bsprContext.ActiveSheet.RowCount; i++)
            {
                bsprContext.SetCellValue(i, (int)SPCModelContextSpread.KEY_ORDER, i);
            }
        }

        private void GetContextValue()
        {
            ArrayList strContextKeys = new ArrayList();
            ArrayList strContextValues = new ArrayList();
            for (int i = 0; i < this.bsprContext.ActiveSheet.RowCount; i++)
            {
                strContextKeys.Add(this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_KEY_NAME].Text);
                strContextValues.Add(this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_VALUE].Text);
            }

            SelectContextValuePopup_New selectContextValuePopup = new SelectContextValuePopup_New();
            selectContextValuePopup.URL = this.URL;
            selectContextValuePopup.PORT = this.Port;
            selectContextValuePopup.LINE_RAWID = this._sLineRawID;
            selectContextValuePopup.AREA_RAWID = this._sAreaRawID;
            selectContextValuePopup.EQP_MODEL = this._sEQPModel;
            selectContextValuePopup.SELECTED_CONTEXT_KEYS = strContextKeys;
            selectContextValuePopup.SELECTED_CONTEXT_VALUES = strContextValues;
            selectContextValuePopup.USE_COMMA = this._bUseComma;
            selectContextValuePopup.InitializePopup();

            DialogResult result = selectContextValuePopup.ShowDialog();
            if (result == DialogResult.OK)
            {
                ArrayList alTempContextKeys = selectContextValuePopup.SELECTED_CONTEXT_KEYS;
                ArrayList alTempContextValues = selectContextValuePopup.SELECTED_CONTEXT_VALUES;

                for (int i = 0; i < this.bsprContext.ActiveSheet.RowCount; i++)
                {
                    if (alTempContextKeys.Contains(this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_KEY_NAME].Text))
                    {
                        int idxTemp = alTempContextKeys.IndexOf(this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_KEY_NAME].Text);

                        if (Convert.ToBoolean(this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.GROUP_YN].Value))
                        {
                            if (alTempContextValues[idxTemp].ToString() == Definition.VARIABLE.STAR)
                            {
                                //MSGHandler.DisplayMessage(MSGType.Information, "It's not allow to set context value as '*' when the group is checked..");
                                //continue;
                            }
                            else if (alTempContextValues[idxTemp].ToString().Trim().Length == 0)
                            {
                                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_SET_CONTEXT_EMPTY", null, null);
                                continue;
                            }
                            else if (alTempContextValues[idxTemp].ToString().Split(';').Length == 1)
                            {
                                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_SET_CONTEXT_SINGLE_ITEM", null, null);
                                continue;
                            }
                            else if (alTempContextValues[idxTemp].ToString().Split(';').Length > 1)
                            {
                                int _iContextValue = 0;
                                string[] strArrTemp = alTempContextValues[idxTemp].ToString().Split(';');
                                for (int j = 0; j < strArrTemp.Length; j++)
                                {
                                    if (strArrTemp[j].Trim().Length > 0)
                                    {
                                        _iContextValue++;
                                    }
                                }
                                if (_iContextValue == 0)
                                {
                                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_SET_CONTEXT_EMPTY", null, null);
                                    continue;
                                }
                                else if (_iContextValue == 1)
                                {
                                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_SET_CONTEXT_SINGLE_ITEM", null, null);
                                    continue;
                                }
                            }
                        }

                        this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_VALUE].CellType = tc;
                        if (alTempContextValues[idxTemp].ToString().Length > 0)
                        {
                            this.bsprContext.ActiveSheet.SetText(i, (int)SPCModelContextSpread.CONTEXT_VALUE, alTempContextValues[idxTemp].ToString());
                        }
                        //this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_VALUE].Text = alTempContextValues[idxTemp].ToString();
                    }
                }

                
            }
        }

        private void GetExcludeValue()
        {
            ArrayList strContextKeys = new ArrayList();
            ArrayList strExcludeValues = new ArrayList();
            for (int i = 0; i < this.bsprContext.ActiveSheet.RowCount; i++)
            {
                strContextKeys.Add(this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_KEY_NAME].Text);
                strExcludeValues.Add(this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.EXCLUDE_LIST].Text);
            }

            SelectContextValuePopup_New selectContextValuePopup = new SelectContextValuePopup_New();
            selectContextValuePopup.URL = this.URL;
            selectContextValuePopup.PORT = this.Port;
            selectContextValuePopup.LINE_RAWID = this._sLineRawID;
            selectContextValuePopup.AREA_RAWID = this._sAreaRawID;
            selectContextValuePopup.EQP_MODEL = this._sEQPModel;
            selectContextValuePopup.SELECTED_CONTEXT_KEYS = strContextKeys;
            selectContextValuePopup.SELECTED_EXCLUDE_VALUES = strExcludeValues;
            selectContextValuePopup.InitializePopup();

            DialogResult result = selectContextValuePopup.ShowDialog();
            if (result == DialogResult.OK)
            {
                ArrayList alTempContextKeys = selectContextValuePopup.SELECTED_CONTEXT_KEYS;
                ArrayList alTempExcludeValues = selectContextValuePopup.SELECTED_EXCLUDE_VALUES;

                for (int i = 0; i < this.bsprContext.ActiveSheet.RowCount; i++)
                {
                    if (alTempContextKeys.Contains(this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_KEY_NAME].Text))
                    {
                        int idxTemp = alTempContextKeys.IndexOf(this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_KEY_NAME].Text);

                        this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.EXCLUDE_LIST].CellType = tc;
                        if (alTempExcludeValues[idxTemp].ToString().Length > 0)
                        {
                            this.bsprContext.ActiveSheet.SetText(i, (int)SPCModelContextSpread.EXCLUDE_LIST, alTempExcludeValues[idxTemp].ToString());
                        }
                    }
                }
            }
        }

        //private void GetFilterValue()
        //{
        //    ArrayList strFilterKeys = new ArrayList();
        //    ArrayList strFilterValues = new ArrayList();
        //    for (int i = 0; i < this.bsprFilter.ActiveSheet.RowCount; i++)
        //    {
        //        strFilterKeys.Add(this.bsprFilter.ActiveSheet.Cells[i, (int)SPCModelFilterSpread.FILTER_KEY_NAME].Text);
        //        strFilterValues.Add(this.bsprFilter.ActiveSheet.Cells[i, (int)SPCModelFilterSpread.FILTER_VALUE].Text);
        //    }

        //    SelectContextValuePopup_New selectContextValuePopup = new SelectContextValuePopup_New();
        //    selectContextValuePopup.URL = this.URL;
        //    selectContextValuePopup.PORT = this.Port;
        //    selectContextValuePopup.LINE_RAWID = this._sLineRawID;
        //    selectContextValuePopup.AREA_RAWID = this._sAreaRawID;
        //    selectContextValuePopup.EQP_MODEL = this._sEQPModel;
        //    selectContextValuePopup.SELECTED_CONTEXT_KEYS = strFilterKeys;
        //    selectContextValuePopup.SELECTED_CONTEXT_VALUES = strFilterValues;
        //    selectContextValuePopup.InitializePopup();

        //    DialogResult result = selectContextValuePopup.ShowDialog();
        //    if (result == DialogResult.OK)
        //    {
        //        ArrayList alTempFilterKeys = selectContextValuePopup.SELECTED_CONTEXT_KEYS;
        //        ArrayList alTempFilterValues = selectContextValuePopup.SELECTED_CONTEXT_VALUES;

        //        for (int i = 0; i < this.bsprFilter.ActiveSheet.RowCount; i++)
        //        {
        //            if (alTempFilterKeys.Contains(this.bsprFilter.ActiveSheet.Cells[i, (int)SPCModelFilterSpread.FILTER_KEY_NAME].Text))
        //            {
        //                int idxTemp = alTempFilterKeys.IndexOf(this.bsprFilter.ActiveSheet.Cells[i, (int)SPCModelFilterSpread.FILTER_KEY_NAME].Text);
        //                this.bsprFilter.ActiveSheet.Cells[i, (int)SPCModelFilterSpread.FILTER_VALUE].CellType = tc;
        //                this.bsprFilter.ActiveSheet.Cells[i, (int)SPCModelFilterSpread.FILTER_VALUE].Text = alTempFilterValues[idxTemp].ToString();
        //            }
        //        }
        //    }
        //}



        public bool InputValidation(bool isMsgDisplay, bool isDefault)
        {
            bool result = true;

            if (!isDefault)
            {

                if (PARAM_TYPE_CD.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_PARAM_TYPE", null, null);
                    result = false;
                }

                if (PARAM_ALIAS.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_PARAM", null, null);
                    result = false;
                }

                if (SAMPLE_COUNT.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_SAMPLE_CNT", null, null);
                    result = false;
                }

                if (this.bsprContext.ActiveSheet.RowCount.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_COMPOSE_CONTEXT", null, null);
                    result = false;
                }

            }

            if (_ComUtil.DuplicateCheck(this.CONTEXT_DATASET, COLUMN.CONTEXT_KEY))
            {
                if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_DUPLICATE_CONTEXT", null, null);
                result = false;
            }

            //if (_ComUtil.DuplicateCheck(this.FILTER_DATASET, COLUMN.FILTER_KEY))
            //{
            //    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "Duplicated filter. Please check filter.");
            //    result = false;
            //}

            foreach (DataRow drContexts in this.CONTEXT_DATASET.Rows)
            {
                if (drContexts.RowState.Equals(DataRowState.Deleted))
                    continue;

                if (_ComUtil.NVL(drContexts[COLUMN.CONTEXT_VALUE]).Length.Equals(0))
                {
                    if (!isDefault)
                    {
                        if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_CONTEXT_VALUE", null, null);
                        result = false;
                        break;
                    }
                }
                else if (_ComUtil.NVL(drContexts[COLUMN.CONTEXT_VALUE]).Length > 1024)
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_CONTEXT_CHAR_SIZE", null, null);
                    result = false;
                    break;
                }
                else
                {
                    //if (this.MAIN_YN.Equals("N"))
                    //{
                    //    if (_ComUtil.NVL(drContexts[COLUMN.CONTEXT_VALUE]).Equals("*"))
                    //    {
                    //        if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "Sub configuration can not use the astrisks(*) as the context value.");
                    //        result = false;
                    //        break;
                    //    }
                    //}
                }
            }

            //foreach (DataRow drFilters in this.FILTER_DATASET.Rows)
            //{
            //    if (drFilters.RowState.Equals(DataRowState.Deleted))
            //        continue;

            //    if (_ComUtil.NVL(drFilters[COLUMN.FILTER_VALUE]).Length.Equals(0))
            //    {
            //        if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "Please input filter value.");
            //        result = false;
            //        break;
            //    }
            //    else if (_ComUtil.NVL(drFilters[COLUMN.FILTER_VALUE]).Length > 1024)
            //    {
            //        if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "filter value can not over 1024 characters. Please check it.");
            //        result = false;
            //        break;
            //    }
            //    else
            //    {
            //        if (this.MAIN_YN.Equals("N"))
            //        {
            //            if (_ComUtil.NVL(drFilters[COLUMN.FILTER_VALUE]).Equals("*"))
            //            {
            //                if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "Sub configuration can not use the astrisks(*) as the filter value.");
            //                result = false;
            //                break;
            //            }
            //        }
            //    }
            //}
            return result;
        }

        #region :DATA PROPERTY
        //DATA PROPERTY

        public LinkedList llstContextType
        {
            get { return _llstContextType; }
            set { _llstContextType = value; }
        }

        //public LinkedList llstFilterType
        //{
        //    get { return _llstFilterType; }
        //    set { _llstFilterType = value; }
        //}

        public ConfigMode CONFIG_MODE
        {
            get { return _cofingMode; }
            set { _cofingMode = value; }
        }

        public string CONFIG_RAWID
        {
            get { return _sConfigRawID; }
            set { _sConfigRawID = value; }
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

        public string MODELINGTYPE
        {
            get { return _ModelingType; }
            set { _ModelingType = value; }
        }


        public string PARAM_TYPE_CD
        {
            get
            {
                if (this.bcboParamType.SelectedValue != null)
                    return this.bcboParamType.SelectedValue.ToString();
                else
                    return string.Empty;
            }
            set { this.bcboParamType.SelectedValue = value; }
        }

        public string SPC_DATA_LEVEL
        {
            get
            {
                if (this.bcboDataLevel.SelectedValue != null)
                    return this.bcboDataLevel.SelectedValue.ToString();
                else
                    return string.Empty;
            }
            set { this.bcboDataLevel.SelectedValue = value; }
        }

        public string MANAGE_TYPE_CD
        {
            get
            {
                if (this.bcboManageType.SelectedValue != null)
                    return this.bcboManageType.SelectedValue.ToString();
                else
                    return string.Empty;
            }
            set { this.bcboManageType.SelectedValue = value; }
        }

        public string COMPLEX_YN
        {
            get
            {
                if (this.bchkComplexYN.Checked)
                    return "Y";
                else
                    return "N";
            }
            set
            {
                if (value.Equals("Y"))
                    this.bchkComplexYN.Checked = true;
                else
                {
                    this.bchkComplexYN.Checked = false;
                    this.btxtParamList.Enabled = false;
                }
            }
        }

        public string PARAM_ALIAS
        {
            get { return this.btxtParamAlias.Text; }
            set { this.btxtParamAlias.Text = value; }
        }

        public string PARAM_LIST
        {
            get { return this.btxtParamList.Text; }
            set { this.btxtParamList.Text = value; }
        }

        public string REF_PARAM
        {
            get { return this.btxtRefParam.Text; }
            set { this.btxtRefParam.Text = value; }
        }

        public string REF_PARAM_LIST
        {
            get { return this.btxtRefParamList.Text; }
            set { this.btxtRefParamList.Text = value; }
        }


        public string MAIN_YN
        {
            get
            {
                if (this.bchkMainYN.Checked)
                    return "Y";
                else
                    return "N";
            }
            set
            {
                if (value.Equals("Y"))
                    this.bchkMainYN.Checked = true;
                else
                    this.bchkMainYN.Checked = false;
            }
        }

        public bool HAS_SUBCONFIGS
        {
            set { this._hasSubConfigs = value; }
        }

        public bool ISTRACESUM
        {
            set { this._isTraceSum = value; }
        }

        public string PARAMALIAST
        {
            set { this._ParamAliasT = value; }
        }

        public string USE_EXTERNAL_SPEC_YN
        {
            get
            {
                if (this.bchkUseExtSpecYN.Checked)
                    return "Y";
                else
                    return "N";
            }
            set
            {
                if (value.Equals("Y"))
                    this.bchkUseExtSpecYN.Checked = true;
                else
                    this.bchkUseExtSpecYN.Checked = false;
            }
        }

        public string INTERLOCK_YN
        {
            get
            {
                if (this.bchkInterlockYN.Checked)
                    return "Y";
                else
                    return "N";
            }
            set
            {
                if (value.Equals("Y"))
                    this.bchkInterlockYN.Checked = true;
                else
                    this.bchkInterlockYN.Checked = false;
            }
        }

        public string AUTOCALC_YN
        {
            get
            {
                if (this.bchkAutoCalcYN.Checked)
                    return "Y";
                else
                    return "N";
            }
            set
            {
                if (value.Equals("Y"))
                    this.bchkAutoCalcYN.Checked = true;
                else
                    this.bchkAutoCalcYN.Checked = false;
            }
        }

        public string AUTO_SUB_YN
        {
            get
            {
                if (this.bchkAutoSubYN.Checked)
                    return "Y";
                else
                    return "N";
            }
            set
            {
                if (value.Equals("Y"))
                    this.bchkAutoSubYN.Checked = true;
                else
                    this.bchkAutoSubYN.Checked = false;
            }
        }

        public string AUTO_TYPE_CD
        {
            get
            {
                if (this.bcboAutoType.SelectedValue != null)
                    return this.bcboAutoType.SelectedValue.ToString();
                else
                    return string.Empty;
            }
            set { this.bcboAutoType.SelectedValue = value; }
        }

        public string SAMPLE_COUNT
        {
            get { return this.btxtSampleCount.Text; }
            set { this.btxtSampleCount.Text = value; }
        }

        public string CHART_MODE_CD
        {
            get 
            {
                if(this.bcboMode.SelectedValue != null)
                    return this.bcboMode.SelectedValue.ToString();
                else
                    return string.Empty;
            }
            set { this.bcboMode.SelectedValue = value; }
        }

        public DataTable CONTEXT_DATASET
        {
            get
            {
                if (this.bsprContext.DataSet != null)
                {
                    this.bsprContext.LeaveCellAction();
                    return ((DataSet)this.bsprContext.DataSet).Tables[0];
                }
                else
                    return null;
            }
            set
            {
                this.bsprContext.DataSet = value;

                if (this.bsprContext.DataSet != null)
                {
                    for (int i = 0; i < bsprContext.ActiveSheet.RowCount; i++)
                    {
                        this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_KEY_NAME].Locked = true;
                        this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_VALUE].CellType = tc;
                        this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.EXCLUDE_LIST].CellType = tc;
                        if (this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.GROUP_YN].Text == Definition.VARIABLE_Y)
                        {
                            this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.GROUP_YN].Value = true;
                        }
                        else
                        {
                            this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.GROUP_YN].Value = false;
                        }
                        


                    }

                    //SPC-781 by louis
                    if (_cofingMode.Equals(ConfigMode.CREATE_SUB))
                    {
                        ((DataSet)this.bsprContext.DataSet).AcceptChanges();
                    }
                }
            }
        }

        //public DataTable FILTER_DATASET
        //{
        //    get
        //    {
        //        if (this.bsprFilter.DataSet != null)
        //        {
        //            this.bsprFilter.LeaveCellAction();
        //            return ((DataSet)this.bsprFilter.DataSet).Tables[0];
        //        }
        //        else
        //            return null;
        //    }
        //    set
        //    {
        //        this.bsprFilter.DataSet = value;

        //        if (this.bsprFilter.DataSet != null)
        //        {
        //            for (int i = 0; i < bsprFilter.ActiveSheet.RowCount; i++)
        //            {
        //                this.bsprFilter.ActiveSheet.Cells[i, (int)SPCModelFilterSpread.FILTER_KEY_NAME].Locked = true;
        //                this.bsprFilter.ActiveSheet.Cells[i, (int)SPCModelFilterSpread.FILTER_VALUE].CellType = tc;
        //            }
        //        }
        //    }
        //}

        public DataTable CONFIG_DATASET
        {
            get
            {
                if (this.bcboManageType.Items.Count > 0 && this.bcboManageType.SelectedIndex > -1)
                {
                    _dtConfig.Rows[0][COLUMN.MANAGE_TYPE_CD] = ((DataRowView)this.bcboManageType.Items[this.bcboManageType.SelectedIndex]).Row[COLUMN.CODE];
                    _dtConfig.Rows[0]["MANAGE_TYPE_NAME"] = ((DataRowView)this.bcboManageType.Items[this.bcboManageType.SelectedIndex]).Row[COLUMN.NAME];
                }

                return _dtConfig;
            }
            set
            {
                _dtConfig = value;

                if (_dtConfig != null)
                {
                    //# Data가 없을 경우 Row를 하나 생성한다.

                    if (_dtConfig.Rows.Count.Equals(0))
                    {
                        DataRow newRow = _dtConfig.NewRow();
                        _dtConfig.Rows.Add(newRow);
                    }

                    if (_cofingMode.Equals(ConfigMode.CREATE_MAIN) || _cofingMode.Equals(ConfigMode.CREATE_MAIN_FROM))
                    {
                        //기본값
                        _dtConfig.Rows[0][COLUMN.MAIN_YN] = "Y";
                        //_dtConfig.Rows[0][COLUMN.AUTO_SUB_YN] = "Y";

                        if (this.bcboParamType.Items.Count > 0)
                            _dtConfig.Rows[0][COLUMN.PARAM_TYPE_CD] = ((DataRowView)this.bcboParamType.Items[0]).Row["CODE"];

                        if (this.bcboManageType.Items.Count > 0)
                            _dtConfig.Rows[0][COLUMN.MANAGE_TYPE_CD] = ((DataRowView)this.bcboManageType.Items[0]).Row["CODE"];

                        if (this.bcboAutoType.Items.Count > 0)
                            _dtConfig.Rows[0][COLUMN.AUTO_TYPE_CD] = ((DataRowView)this.bcboAutoType.Items[0]).Row["CODE"];

                        if (this.bcboMode.Items.Count > 0)
                            _dtConfig.Rows[0][COLUMN.CHART_MODE_CD] = ((DataRowView)this.bcboMode.Items[0]).Row["CODE"];

                        if (this.bcboDataLevel.Items.Count > 0)
                            _dtConfig.Rows[0][COLUMN.SPC_DATA_LEVEL] = ((DataRowView)this.bcboDataLevel.Items[0]).Row["CODE"];

                        _dtConfig.Rows[0][COLUMN.USE_NORM_YN] = "N";
                        _dtConfig.Rows[0][COLUMN.VALIDATION_SAME_MODULE_YN] = "N";
                        _dtConfig.Rows[0][COLUMN.SPC_DATA_LEVEL] = "L";


                        
                    }
                    else if (_cofingMode.Equals(ConfigMode.CREATE_SUB))
                    {
                        _dtConfig.Rows[0][COLUMN.MAIN_YN] = "N";
                    }
                    else if (_cofingMode.Equals(ConfigMode.DEFAULT))
                    {

                    }


                    //# Control에 DataBinding 하기

                    //PARAM_ALIAS
                    this.btxtParamAlias.DataBindings.Add("Text", _dtConfig, COLUMN.PARAM_ALIAS);

                    //PARAM_TYPE_CD
                    this.bcboParamType.DataBindings.Add("SelectedValue", _dtConfig, COLUMN.PARAM_TYPE_CD);

                    //MANAGE_TYPE_CD
                    this.bcboManageType.DataBindings.Add("SelectedValue", _dtConfig, COLUMN.MANAGE_TYPE_NAME);


                    //COMPLEX_YN
                    Binding bdComplexYN = new Binding("Checked", _dtConfig, COLUMN.COMPLEX_YN);
                    bdComplexYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdComplexYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkComplexYN.DataBindings.Add(bdComplexYN);

                    //PARAM_LIST
                    this.btxtParamList.DataBindings.Add("Text", _dtConfig, COLUMN.PARAM_LIST);

                    //REF_PARAM
                    this.btxtRefParam.DataBindings.Add("Text", _dtConfig, COLUMN.REF_PARAM);

                    //REF_PARAM_LIST
                    this.btxtRefParamList.DataBindings.Add("Text", _dtConfig, COLUMN.REF_PARAM_LIST);

                    //MAIN_YN
                    Binding bdMainYN = new Binding("Checked", _dtConfig, COLUMN.MAIN_YN);
                    bdMainYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdMainYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkMainYN.DataBindings.Add(bdMainYN);

                    //USE_EXTERNAL_SPEC_YN
                    Binding bdUseExtSpecYN = new Binding("Checked", _dtConfig, COLUMN.USE_EXTERNAL_SPEC_YN);
                    bdUseExtSpecYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdUseExtSpecYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkUseExtSpecYN.DataBindings.Add(bdUseExtSpecYN);

                    //INTERLOCK_YN
                    Binding bdInterlockYN = new Binding("Checked", _dtConfig, COLUMN.INTERLOCK_YN);
                    bdInterlockYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdInterlockYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkInterlockYN.DataBindings.Add(bdInterlockYN);

                    //AUTOCALC_YN
                    Binding bdAutoCalcYN = new Binding("Checked", _dtConfig, COLUMN.AUTOCALC_YN);
                    bdAutoCalcYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdAutoCalcYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkAutoCalcYN.DataBindings.Add(bdAutoCalcYN);

                    //AUTO_SUB_YN
                    Binding bdAutoSubYN = new Binding("Checked", _dtConfig, COLUMN.AUTO_SUB_YN);
                    bdAutoSubYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdAutoSubYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkAutoSubYN.DataBindings.Add(bdAutoSubYN);

                    //AUTO_TYPE_CD
                    this.bcboAutoType.DataBindings.Add("SelectedValue", _dtConfig, COLUMN.AUTO_TYPE_CD);

                    //SAMPLE_COUNT
                    this.btxtSampleCount.DataBindings.Add("Text", _dtConfig, COLUMN.SAMPLE_COUNT);

                    //ACTIVATION_YN
                    Binding bdActivationYN = new Binding("Checked", _dtConfig, COLUMN.ACTIVATION_YN);
                    bdActivationYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdActivationYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkActive.DataBindings.Add(bdActivationYN);

                    //SUB_INTERLOCK_YN
                    Binding bdSubInterlockYN = new Binding("Checked", _dtConfig, COLUMN.SUB_INTERLOCK_YN);
                    bdSubInterlockYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdSubInterlockYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkCreateSubInterlock.DataBindings.Add(bdSubInterlockYN);

                    //ACTIVATION_YN
                    Binding bdInheritMainYN = new Binding("Checked", _dtConfig, COLUMN.INHERIT_MAIN_YN);
                    bdInheritMainYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdInheritMainYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkInheritYN.DataBindings.Add(bdInheritMainYN);

                    //SUB_AUTOCALC_YN
                    Binding bdSubAutoCalcYN = new Binding("Checked", _dtConfig, COLUMN.SUB_AUTOCALC_YN);
                    bdSubAutoCalcYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdSubAutoCalcYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkCreateSubAutoCalc.DataBindings.Add(bdSubAutoCalcYN);

                    // USE_NORAMLIZATION
                    Binding bdUseNormalization = new Binding("Checked", _dtConfig, COLUMN.USE_NORM_YN);
                    bdUseNormalization.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdUseNormalization.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkUseNormalization.DataBindings.Add(bdUseNormalization);

                    // CHART_MODE_CD
                    this.bcboMode.DataBindings.Add("SelectedValue", _dtConfig, COLUMN.CHART_MODE_CD);

                    // VALIDATION_SAME_MODULE
                    Binding bdValidationSameModule = new Binding("Checked", _dtConfig, COLUMN.VALIDATION_SAME_MODULE_YN);
                    bdValidationSameModule.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdValidationSameModule.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkValSameModule.DataBindings.Add(bdValidationSameModule);

                    // SPC_DATA_LEVEL
                    this.bcboDataLevel.DataBindings.Add("SelectedValue", _dtConfig, COLUMN.SPC_DATA_LEVEL);
                }
            }
        }

        public string ACTIVATION_YN
        {
            get
            {
                if (this.bchkActive.Checked)
                    return "Y";
                else
                    return "N";
            }
            set
            {
                if (value.Equals("Y"))
                    this.bchkActive.Checked = true;
                else
                    this.bchkActive.Checked = false;
            }
        }

        //spc-1199 by stella
        public DataTable ORIGINAL_DATA
        {
            set { _dtOriginalData = value; }
        }



        public void InitializeLayout(ConfigMode configMode)
        {
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_MODELING, Definition.CONFIG_USE_COMMA, false);
            this.bchkMainYN.Enabled = false;

            if (!this.bchkComplexYN.Checked)
            {
                this.btxtParamList.Enabled = false;
            }

            switch (configMode)
            {
                case ConfigMode.DEFAULT://디폴트 셋팅
                    this.gbParam.Visible = false;
                    //this.bchkActive.Visible = false;
                    //this.bchkCreateSubInterlock.Visible = false;
                    //this.bchkInheritYN.Visible = false;
                    this.tlpModel4.Visible = false;
                    this.tlpModel5.Visible = false;
                    this.tlpModel6.Visible = false;

                    //SPC-757 Defalult Setting시 Active설정에 대한 문의
                    this.bchkActive.Visible = false;
                    //this.gbFilter.Visible = false;

                    //SPC-749
                    this.bcboAutoType.Visible = false;
                    this.bcboMode.Visible = false;
                    this.blblAutoSettings.Visible = false;
                    this.blbMode.Visible = false;

                    //SPC-987 MET Modeling
                    if (this.bcboParamType.Text != Definition.VARIABLE.METROLOGY)
                    {
                        //this.bcboManageType.Enabled = false;
                        this.bcboManageType.Visible = false;
                        this.blblManageType.Visible = false;
                    }
                    else
                    {
                        this.bchkUseNormalization.Visible = false;
                        this.bcboDataLevel.Visible = false;
                        this.blblDataLevel.Visible = false;
                    }
                    //SPC-987 end



                    //  this.display.Visible = false;//Manage Type 콤보박스 안보이게..
                    break;

                case ConfigMode.CREATE_MAIN:
                case ConfigMode.SAVE_AS:
                case ConfigMode.CREATE_MAIN_FROM:

                    //this.bchkInheritYN.Visible = false;
                    this.tlpModel5.Visible = false;


                    //SPC-987 METModeling
                    if (this.bcboParamType.Text != Definition.VARIABLE.METROLOGY)
                    {
                        //this.bcboManageType.Enabled = false;
                        this.bcboManageType.Visible = false;
                        this.blblManageType.Visible = false;
                    }
                    else
                    {
                        this.bchkUseNormalization.Visible = false;
                        this.bcboDataLevel.Visible = false;
                        this.blblDataLevel.Visible = false;
                    }
                    //SPC-987 end

                    if (configMode == ConfigMode.CREATE_MAIN_FROM)
                    {
                        if (this._isTraceSum)
                        {
                            for (int idxTT = 0; idxTT < this.bcboParamType.Items.Count; idxTT++)
                            {
                                if (((DataRowView)this.bcboParamType.Items[idxTT]).Row[COLUMN.CODE].ToString() == "TRS")
                                {
                                    this.bcboParamType.SelectedIndex = idxTT;
                                    break;
                                }
                            }

                            this.bcboParamType.Enabled = false;
                            this.btxtParamAlias.Text = this._ParamAliasT;
                            _dtConfig.Rows[0][COLUMN.PARAM_ALIAS] = this._ParamAliasT;
                            this.btxtParamAlias.ReadOnly = true;
                            this.bbtnParamAlias.Enabled = false;
                        }
                        else
                        {
                            for (int idxTT = 0; idxTT < this.bcboParamType.Items.Count; idxTT++)
                            {
                                if (((DataRowView)this.bcboParamType.Items[idxTT]).Row[COLUMN.CODE].ToString() == "EVS")
                                {
                                    this.bcboParamType.SelectedIndex = idxTT;
                                    break;
                                }
                            }
                            this.bcboParamType.Enabled = false;
                            this.btxtParamAlias.Text = this._ParamAliasT;
                            _dtConfig.Rows[0][COLUMN.PARAM_ALIAS] = this._ParamAliasT;
                            this.btxtParamAlias.ReadOnly = true;
                            this.bbtnParamAlias.Enabled = false;
                        }
                    }

                    //SPC-705
                    //if (bchkUseNormalization.CheckState == CheckState.Checked)
                    //{
                    //    this.bchkValSameModule.Enabled = true;
                    //}
                    //else
                    //{
                    //    this.bchkValSameModule.Enabled = false;
                    //}

                    break;

                case ConfigMode.CREATE_SUB:
                    this.gbParam.Enabled = false;

                    this.bcboParamType.DropDownStyle = ComboBoxStyle.Simple;
                    if (this.bcboParamType.Text != Definition.VARIABLE.METROLOGY)
                    {
                        //this.bcboManageType.Enabled = false;
                        this.bcboManageType.Visible = false;
                        this.blblManageType.Visible = false;
                    }
                    else
                    {
                        this.bchkUseNormalization.Visible = false;
                        this.bcboDataLevel.Visible = false;
                        this.blblDataLevel.Visible = false;
                    }


                    this.bbtnParamAlias.Visible = false;
                    this.bbtnRefParam.Visible = false;

                    //this.bbtnListContext.Enabled = false;
                    this.bbtnListContext.Visible = false;
                    this.bsprContext.ContextMenu.MenuItems.Clear();

                    //Auto Sub Chart
                    this.bchkAutoSubYN.Visible = false;
                    this.blblAutoSettings.Visible = false;
                    this.bcboAutoType.Visible = false;
                    //this.bchkActive.Visible = false;
                    //this.bchkCreateSubInterlock.Visible = false;
                    this.tlpModel4.Visible = false;
                    this.tlpModel6.Visible = false;
                    //this.gbFilter.Visible = false;
                    this.bsprContext.ActiveSheet.Columns[(int)SPCModelContextSpread.GROUP_YN].Visible = false;

                    break;

                case ConfigMode.MODIFY:
                    if (this.bchkMainYN.Checked)
                    {//Main인 경우

                        if (_hasSubConfigs)
                        {
                            this.gbParam.Enabled = false;
                            this.bcboManageType.Enabled = false;
                            this.bcboParamType.DropDownStyle = ComboBoxStyle.Simple;
                            this.bcboManageType.DropDownStyle = ComboBoxStyle.Simple;

                            this.bbtnParamAlias.Visible = false;
                            this.bbtnRefParam.Visible = false;

                            this.bbtnListContext.Visible = true;
                            this.bsprContext.ContextMenu.MenuItems.Clear();
                            this.bsprContext.ActiveSheet.Columns[(int)SPCModelContextSpread.CONTEXT_KEY_NAME].Locked = true;
                            //this.bsprContext.ActiveSheet.Columns[(int)SPCModelContextSpread.CONTEXT_VALUE].Locked = true;
                            //this.bsprContext.ActiveSheet.Columns[(int)SPCModelContextSpread.EXCLUDE_LIST].Locked = true;

                            this.gbContext.Enabled = true;

                            //this.bchkInheritYN.Visible = false;
                        }

                        if (this.bcboParamType.Text != Definition.VARIABLE.METROLOGY)
                        {
                            //this.bcboManageType.Enabled = false;
                            this.bcboManageType.Visible = false;
                            this.blblManageType.Visible = false;
                        }
                        else
                        {
                            this.bchkUseNormalization.Visible = false;
                            this.bcboDataLevel.Visible = false;
                            this.blblDataLevel.Visible = false;
                        }

                        //SPC-705
                        //if (bchkUseNormalization.CheckState == CheckState.Checked)
                        //{
                        //    this.bchkValSameModule.Enabled = true;
                        //}
                        //else
                        //{
                        //    this.bchkValSameModule.Enabled = false;
                        //}

                        this.tlpModel5.Visible = false;
                    }
                    else
                    {//Sub인 경우

                        this.gbParam.Enabled = false;

                        this.bcboParamType.DropDownStyle = ComboBoxStyle.Simple;

                        if (this.bcboParamType.Text != Definition.VARIABLE.METROLOGY)
                        {
                            //this.bcboManageType.Enabled = false;
                            this.bcboManageType.Visible = false;
                            this.blblManageType.Visible = false;
                        }
                        else
                        {
                            this.bchkUseNormalization.Visible = false;
                            this.bcboDataLevel.Visible = false;
                            this.blblDataLevel.Visible = false;
                        }


                        this.bbtnParamAlias.Visible = false;
                        this.bbtnRefParam.Visible = false;

                        this.bbtnListContext.Visible = false;
                        this.bsprContext.ContextMenu.MenuItems.Clear();

                        //Auto Sub Chart
                        this.bchkAutoSubYN.Visible = false;
                        this.blblAutoSettings.Visible = false;
                        this.bcboAutoType.Visible = false;
                        //this.bchkActive.Visible = false;
                        //this.bchkCreateSubInterlock.Visible = false;
                        this.tlpModel4.Visible = false;
                        this.tlpModel6.Visible = false;
                        //this.gbFilter.Visible = false;
                        this.bsprContext.ActiveSheet.Columns[(int)SPCModelContextSpread.GROUP_YN].Visible = false;
                        this.bsprContext.Enabled = false;
                    }
                    break;

                case ConfigMode.VIEW:

                    this.gbParam.Enabled = false;
                    this.bcboParamType.DropDownStyle = ComboBoxStyle.Simple;

                    if (this.bcboParamType.Text != Definition.VARIABLE.METROLOGY)
                    {
                        this.bcboManageType.Enabled = false;

                    }

                    this.bbtnParamAlias.Visible = false;
                    this.bbtnRefParam.Visible = false;
                    this.gbContext.Enabled = false;
                    this.bbtnListContext.Visible = false;
                    this.gbModel.Enabled = false;
                    if (this.bchkMainYN.Checked)
                    {//Main인 경우
                        //this.bchkInheritYN.Visible = false;
                        this.tlpModel5.Visible = false;
                    }
                    else
                    {//Sub인 경우
                        this.bchkAutoSubYN.Visible = false;
                        this.blblAutoSettings.Visible = false;
                        this.bcboAutoType.Visible = false;
                        //this.bchkActive.Visible = false;
                        //this.bchkCreateSubInterlock.Visible = false;
                        this.tlpModel4.Visible = false;
                        this.tlpModel6.Visible = false;
                        //this.gbFilter.Visible = false;
                        this.bsprContext.ActiveSheet.Columns[(int)SPCModelContextSpread.GROUP_YN].Visible = false;
                    }

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

        #endregion



        #region ::: User Defined Method.

        #endregion

        #region ::: EventHandler

        private void bbtnParamAlias_Click(object sender, EventArgs e)
        {
            if (this.bcboParamType.SelectedIndex < 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_PARAM_TYPE", null, null);
                return;
            }

            SelectParamPopup selectParamPopup = new SelectParamPopup();
            selectParamPopup.URL = this.URL;
            selectParamPopup.PORT = this.Port;
            selectParamPopup.LINE_RAWID = this._sLineRawID;
            selectParamPopup.AREA_RAWID = this._sAreaRawID;
            selectParamPopup.EQP_MODEL = this._sEQPModel;
            selectParamPopup.PARAM_TYPE_CD = _ComUtil.NVL(this.bcboParamType.SelectedValue);
            selectParamPopup.InitializePopup();

            DialogResult result = selectParamPopup.ShowDialog();

            if (result.Equals(DialogResult.OK))
            {
                this.btxtParamAlias.Text = selectParamPopup.SELECTED_PARAM_ALIAS;
                _dtConfig.Rows[0][COLUMN.PARAM_ALIAS] = selectParamPopup.SELECTED_PARAM_ALIAS;
            }
        }


        private void bbtnRefParam_Click(object sender, EventArgs e)
        {
            if (this.bcboParamType.SelectedIndex < 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_PARAM_TYPE", null, null);
                return;
            }



            if (this.btxtParamAlias.Text.Length == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_ENTER_PARAM", null, null);
                return;
            }

            List<string> paramList = new List<string>();
            paramList.Add(this.btxtParamAlias.Text);

            SelectParamPopup selectParamPopup = new SelectParamPopup();
            selectParamPopup.URL = this.URL;
            selectParamPopup.PORT = this.Port;
            selectParamPopup.LINE_RAWID = this._sLineRawID;
            selectParamPopup.AREA_RAWID = this._sAreaRawID;
            selectParamPopup.EQP_MODEL = this._sEQPModel;
            selectParamPopup.PARAM_TYPE_CD = _ComUtil.NVL(this.bcboParamType.SelectedValue);
            selectParamPopup.EXCLUDE_PARAM_LIST = paramList;
            selectParamPopup.InitializePopup();

            DialogResult result = selectParamPopup.ShowDialog();

            if (result.Equals(DialogResult.OK))
            {
                this.btxtRefParam.Text = selectParamPopup.SELECTED_PARAM_ALIAS;
                _dtConfig.Rows[0][COLUMN.REF_PARAM] = selectParamPopup.SELECTED_PARAM_ALIAS;
            }
        }


        private void bbtnListContext_ButtonClick(string name)
        {
            if (name.ToUpper().Equals(Definition.BUTTON_KEY_UP))
            {
                if (bsprContext.ActiveSheet.RowCount > 0)
                {
                    if (this.bchkMainYN.Checked && _hasSubConfigs)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_MAIN_CANT_USE_OPERTION", null, null);
                        return;
                    }
                    else
                    {
                        int iActiveRow = bsprContext.ActiveSheet.ActiveRowIndex;

                        if (iActiveRow == 0)
                            return;

                        bsprContext.MoveRow(iActiveRow, iActiveRow - 1, true);


                        bsprContext.SetCellValue(iActiveRow, (int)SPCModelContextSpread.KEY_ORDER, iActiveRow);
                        bsprContext.SetCellValue(iActiveRow - 1, (int)SPCModelContextSpread.KEY_ORDER, iActiveRow - 1);
                    }
                }
            }
            else if (name.ToUpper().Equals(Definition.BUTTON_KEY_DOWN))
            {
                if (bsprContext.ActiveSheet.RowCount > 0)
                {
                    if (this.bchkMainYN.Checked && _hasSubConfigs)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_MAIN_CANT_USE_OPERTION", null, null);
                        return;
                    }
                    else
                    {
                        int iActiveRow = bsprContext.ActiveSheet.ActiveRow.Index;

                        if (iActiveRow == bsprContext.ActiveSheet.RowCount - 1)
                            return;

                        bsprContext.MoveRow(iActiveRow, iActiveRow + 1, true);

                        bsprContext.SetCellValue(iActiveRow, (int)SPCModelContextSpread.KEY_ORDER, iActiveRow);
                        bsprContext.SetCellValue(iActiveRow + 1, (int)SPCModelContextSpread.KEY_ORDER, iActiveRow + 1);
                    }
                }
            }
            else if (name.ToUpper().Equals(Definition.BUTTON_KEY_REMOVE))
            {
                if (this.bsprContext.ActiveSheet.RowCount > 0)
                {
                    //if (this.bchkMainYN.Checked && _hasSubConfigs)
                    //{
                    //    MSGHandler.DisplayMessage(MSGType.Information, "Main Config that has sub configuration can not use this operation.");
                    //    return;
                    //}
                    //else
                    //{

                    //SPC-864 by Louis - ContextRemove
                    int iActiveRow = bsprContext.ActiveSheet.ActiveRowIndex;
                    if (bsprContext.ActiveSheet.ActiveRow==null || iActiveRow < 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_REGION_SPREAD", null, null);
                        return;
                    }
                    this.bsprContext.RemoveRow(iActiveRow, iActiveRow);
                    this.SetContextKeyOrder();

                    //SortedList slSelectedRows = this.bsprContext.GetSelectedRows();
                    //if (slSelectedRows.Count > 0)
                    //{
                    //    int iOffset = 0;
                    //    foreach (int iRow in slSelectedRows.Values)
                    //    {
                    //        int iRemoveRow = iRow - iOffset;
                    //        this.bsprContext.RemoveRow(iRemoveRow, iRemoveRow);
                    //        iOffset++;
                    //    }
                    //    this.SetContextKeyOrder();
                    //}
                    //else
                    //{
                    //    MSGHandler.DisplayMessage(MSGType.Information, "Please, specify the region on the spread.");
                    //}
                    //}
                }
                
                //if (bsprContext.ActiveSheet.RowCount > 0)
                //{
                ////    if (this.bchkMainYN.Checked && _hasSubConfigs)
                ////    {
                ////        MSGHandler.DisplayMessage(MSGType.Information, "Main Config that has sub configuration can not use this operation.");
                ////        return;
                ////    }
                ////    else
                ////    {
                //   // if(bsprContext.ActiveSheet.ActiveRow==null)
                //        bsprContext.RemoveRow(bsprContext.ActiveSheet.ActiveRowIndex, bsprContext.ActiveSheet.ActiveRowIndex);
                //        this.SetContextKeyOrder();
                //    //}
                //}
            }
            else if (name.ToUpper().Equals(Definition.BUTTON_KEY_GET_VALUE))
            {
                if (bsprContext.ActiveSheet.RowCount > 0)
                {
                    if (this.bchkMainYN.Checked && _hasSubConfigs)
                    {
                        //MSGHandler.DisplayMessage(MSGType.Information, "Main Config that has sub configuration can not use this operation.");
                        //return;
                        this.GetContextValue();
                        for (int i = 0; i < this.bsprContext.ActiveSheet.Rows.Count; i++)
                        {
                            FarPoint.Win.Spread.SpreadView view = new FarPoint.Win.Spread.SpreadView(this.bsprContext);
                            FarPoint.Win.Spread.ChangeEventArgs ce = new FarPoint.Win.Spread.ChangeEventArgs(view, i, (int)SPCModelContextSpread.CONTEXT_VALUE);
                            this.bsprContext_Change(this.bsprContext, ce);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < bsprContext.ActiveSheet.RowCount; i++)
                        {
                            if (bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_KEY_NAME].Text.Trim().Length == 0)
                            {
                                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_CONTEXT_FIELD", null, null);
                                return;
                            }
                        }
                        this.GetContextValue();
                    }
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_CONTEXT_FIELD", null, null);
                }
            }
            else if (name.ToUpper().Equals(Definition.BUTTON_KEY_GET_EXCLUDE_VALUE))
            {
                if (bsprContext.ActiveSheet.RowCount > 0)
                {
                    if (this.bchkMainYN.Checked && _hasSubConfigs)
                    {
                        this.GetExcludeValue();
                        for (int i = 0; i < this.bsprContext.ActiveSheet.Rows.Count; i++)
                        {
                            FarPoint.Win.Spread.SpreadView view = new FarPoint.Win.Spread.SpreadView(this.bsprContext);
                            FarPoint.Win.Spread.ChangeEventArgs ce = new FarPoint.Win.Spread.ChangeEventArgs(view, i, (int)SPCModelContextSpread.EXCLUDE_LIST);
                            this.bsprContext_Change(this.bsprContext, ce);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < bsprContext.ActiveSheet.RowCount; i++)
                        {
                            if (bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_KEY_NAME].Text.Trim().Length == 0)
                            {
                                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_CONTEXT_FIELD", null, null);
                                return;
                            }
                        }
                        this.GetExcludeValue();
                    }
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_CONTEXT_FIELD", null, null);
                }
            }
            else
            {
                //if (this.bchkMainYN.Checked && _hasSubConfigs)
                //{
                //    MSGHandler.DisplayMessage(MSGType.Information, "Main Config that has sub configuration can not use this operation.");
                //    return;
                //}
                //else
                //{
                    bsprContext.ContextMenuAction(name);
                    for (int i = 0; i < bsprContext.ActiveSheet.RowCount; i++)
                    {
                        this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_VALUE].CellType = tc;
                        this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.EXCLUDE_LIST].CellType = tc;
                        if (this.bsprContext.ActiveSheet.GetCellType(i, (int)SPCModelContextSpread.CONTEXT_KEY_NAME) is FarPoint.Win.Spread.CellType.ComboBoxCellType)
                        {
                            if (this.bsprContext.ActiveSheet.Columns[(int)SPCModelContextSpread.CONTEXT_KEY_NAME].Locked)
                            {
                                this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_KEY_NAME].Locked = false;
                            }
                        }
                    }
                //}
            }
        }

        //private void bbtnListFilter_ButtonClick(string name)
        //{
        //    if (name.ToUpper().Equals(Definition.BUTTON_KEY_REMOVE))
        //    {
        //        if (bsprFilter.ActiveSheet.RowCount > 0)
        //        {
        //            bsprFilter.RemoveRow(bsprFilter.ActiveSheet.ActiveRowIndex, bsprFilter.ActiveSheet.ActiveRowIndex);
        //        }
        //    }
        //    else if (name.ToUpper().Equals(Definition.BUTTON_KEY_GET_VALUE))
        //    {
        //        if (bsprFilter.ActiveSheet.RowCount > 0)
        //        {
        //            for (int i = 0; i < bsprFilter.ActiveSheet.RowCount; i++)
        //            {
        //                if (bsprFilter.ActiveSheet.Cells[i, (int)SPCModelFilterSpread.FILTER_KEY_NAME].Text.Trim().Length == 0)
        //                {
        //                    MSGHandler.DisplayMessage(MSGType.Information, "No FILTER KEY Datas.");
        //                    return;
        //                }
        //            }
        //            this.GetFilterValue();
        //        }
        //        else
        //        {
        //            MSGHandler.DisplayMessage(MSGType.Information, "No FILTER KEY Datas.");
        //        }
        //    }
        //    else
        //    {
        //        bsprFilter.ContextMenuAction(name);
        //        for (int i = 0; i < bsprFilter.ActiveSheet.RowCount; i++)
        //        {
        //            this.bsprFilter.ActiveSheet.Cells[i, (int)SPCModelFilterSpread.FILTER_VALUE].CellType = tc;
        //        }
        //    }
        //}

        private void bchkComplexYN_CheckedChanged(object sender, EventArgs e)
        {
            if (bchkComplexYN.Checked)
            {
                this.btxtParamList.Enabled = true;
            }
            else
            {
                _dtConfig.Rows[0][COLUMN.PARAM_LIST] = "";
                this.btxtParamList.Text = "";
                this.btxtParamList.Enabled = false;
            }
        }

        private void bcboParamType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BComboBox bcob = sender as BComboBox;

            if (bcob.Text.Equals(Definition.VARIABLE.METROLOGY))
            {
                this.bcboManageType.Enabled = true;

                if (this.bcboManageType.Items.Count > 0 && this.bcboManageType.SelectedIndex == -1)
                {
                    this.bcboManageType.SelectedIndex = 0;
                }

                this.bchkUseNormalization.Enabled = false;
                this.bchkUseNormalization.CheckState = CheckState.Unchecked;
                this.bcboDataLevel.Enabled = false;
            }
            else
            {
                this.bcboManageType.Enabled = false;
                this.bcboManageType.Text = "";
                this.bcboManageType.SelectedIndex = -1;

                this.bchkUseNormalization.Enabled = true;
                this.bcboDataLevel.Enabled = true;
            }
        }

        private void bsprContext_AddRowEvent(object sender, int row)
        {
            _SpreadUtil.SetComboBoxCellType(bsprContext.ActiveSheet.Cells[row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME], _saContextTypeName, false);

            this.SetContextKeyOrder();

            string _contextKeyName = bsprContext.GetCellValue(row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME).ToString();
            bsprContext.SetCellValue(row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME, bsprContext.GetCellValue(row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME));

            //2009-12-07 bskwon 추가
            if (this.llstContextType[_contextKeyName] != null)
            {
                this._contextTypeInfo = this.llstContextType[_contextKeyName] as SPCStruct.ContextTypeInfo;
                this.bsprContext.ActiveSheet.Cells[row, (int)SPCModelContextSpread.CONTEXT_KEY].Text = _contextTypeInfo.CODE;
            }
            else
                bsprContext.SetCellValue(row, (int)SPCModelContextSpread.CONTEXT_KEY, bsprContext.GetCellValue(row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME));
        }

        //private void bsprFilter_AddRowEvent(object sender, int row)
        //{
        //    _SpreadUtil.SetComboBoxCellType(bsprFilter.ActiveSheet.Cells[row, (int)SPCModelFilterSpread.FILTER_KEY_NAME], _saFilterTypeName, false);

        //    string _filterKeyName = bsprFilter.GetCellValue(row, (int)SPCModelFilterSpread.FILTER_KEY_NAME).ToString();
        //    bsprFilter.SetCellValue(row, (int)SPCModelFilterSpread.FILTER_KEY_NAME, bsprFilter.GetCellValue(row, (int)SPCModelFilterSpread.FILTER_KEY_NAME));

        //    if (this.llstFilterType[_filterKeyName] != null)
        //    {
        //        this._filterTypeInfo = this.llstFilterType[_filterKeyName] as SPCStruct.FilterTypeInfo;
        //        this.bsprFilter.ActiveSheet.Cells[row, (int)SPCModelFilterSpread.FILTER_KEY].Text = _filterTypeInfo.CODE;
        //    }
        //    else
        //        bsprFilter.SetCellValue(row, (int)SPCModelFilterSpread.FILTER_KEY, bsprFilter.GetCellValue(row, (int)SPCModelFilterSpread.FILTER_KEY_NAME));
        //}

        private void bsprContext_ComboSelChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string _contextKeyName = bsprContext.GetCellValue(e.Row, e.Column).ToString();
            bsprContext.SetCellValue(e.Row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME, bsprContext.GetCellValue(e.Row, e.Column));

            //2009-12-07 bskwon 추가
            if (this.llstContextType[_contextKeyName] != null)
            {
                this._contextTypeInfo = this.llstContextType[_contextKeyName] as SPCStruct.ContextTypeInfo;
                this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_KEY].Text = _contextTypeInfo.CODE;
            }
            else
                bsprContext.SetCellValue(e.Row, (int)SPCModelContextSpread.CONTEXT_KEY, bsprContext.GetCellValue(e.Row, e.Column));
        }

        //private void bsprFilter_ComboSelChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        //{
        //    string _filterKeyName = bsprFilter.GetCellValue(e.Row, e.Column).ToString();
        //    bsprFilter.SetCellValue(e.Row, (int)SPCModelFilterSpread.FILTER_KEY_NAME, bsprFilter.GetCellValue(e.Row, e.Column));

        //    //2009-12-07 bskwon 추가
        //    if (this.llstFilterType[_filterKeyName] != null)
        //    {
        //        this._filterTypeInfo = this.llstFilterType[_filterKeyName] as SPCStruct.FilterTypeInfo;
        //        this.bsprFilter.ActiveSheet.Cells[e.Row, (int)SPCModelFilterSpread.FILTER_KEY].Text = _filterTypeInfo.CODE;
        //    }
        //    else
        //        bsprFilter.SetCellValue(e.Row, (int)SPCModelFilterSpread.FILTER_KEY, bsprFilter.GetCellValue(e.Row, e.Column));
        //}



        private void bsprContext_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (this.bsprContext.ActiveSheet.RowCount > 0)
                {
                    int iActiveRowIndex = this.bsprContext.ActiveSheet.ActiveRowIndex;
                    int iActiveColumnIndex = this.bsprContext.ActiveSheet.ActiveColumnIndex;

                    if (iActiveColumnIndex == this._ColIdx_SELECT || iActiveColumnIndex == this._ColIdx_CONTEXT_KEY)
                    {
                        this.bsprContext.IsCellCopy = false;
                    }
                    else
                    {
                        if (e.Control)
                        {
                            if (e.KeyCode == Keys.V || e.KeyCode == Keys.C)
                            {
                                int itempRowCount = this.bsprContext.ActiveSheet.RowCount;
                                this.bsprContext.AllowNewRow = false;
                                this.bsprContext.IsCellCopy = true;
                                //FarPoint.Win.Spread.SpreadView view = new FarPoint.Win.Spread.SpreadView(this.bsprContext);

                                //DataSet ds = (DataSet)this.bsprContext.DataSource;

                                //FarPoint.Win.Spread.ChangeEventArgs ce = new FarPoint.Win.Spread.ChangeEventArgs(view,
                                //    iActiveRowIndex,
                                //    iActiveColumnIndex);

                                //FarPoint.Win.Spread.LeaveCellEventArgs es = new FarPoint.Win.Spread.LeaveCellEventArgs(view,
                                //    this.bsprContext.ActiveSheet.ActiveRowIndex,
                                //    this.bsprContext.ActiveSheet.ActiveColumnIndex,
                                //    0, 0);

                                //this.bsprContext_LeaveCell(this.bsprContext, es);
                                //this.bsprContext_Change(this.bsprContext, ce);

                            }

                            DataSet ds_Test = (DataSet)this.bsprContext.DataSource;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //private void bsprFilter_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        if (this.bsprFilter.ActiveSheet.RowCount > 0)
        //        {
        //            int iActiveRowIndex = this.bsprFilter.ActiveSheet.ActiveRowIndex;
        //            int iActiveColumnIndex = this.bsprFilter.ActiveSheet.ActiveColumnIndex;

        //            if (iActiveColumnIndex == this._ColIdx_FILTER_SELECT || iActiveColumnIndex == this._ColIdx_FILTER_KEY)
        //            {
        //                this.bsprFilter.IsCellCopy = false;
        //            }
        //            else
        //            {
        //                if (e.Control)
        //                {
        //                    if (e.KeyCode == Keys.V || e.KeyCode == Keys.C)
        //                    {
        //                        int itempRowCount = this.bsprFilter.ActiveSheet.RowCount;
        //                        this.bsprFilter.AllowNewRow = false;
        //                        this.bsprFilter.IsCellCopy = true;
        //                        FarPoint.Win.Spread.SpreadView view = new FarPoint.Win.Spread.SpreadView(this.bsprFilter);

        //                        DataSet ds = (DataSet)this.bsprFilter.DataSource;

        //                        FarPoint.Win.Spread.ChangeEventArgs ce = new FarPoint.Win.Spread.ChangeEventArgs(view,
        //                            iActiveRowIndex,
        //                            iActiveColumnIndex);

        //                        FarPoint.Win.Spread.LeaveCellEventArgs es = new FarPoint.Win.Spread.LeaveCellEventArgs(view,
        //                            this.bsprFilter.ActiveSheet.ActiveRowIndex,
        //                            this.bsprFilter.ActiveSheet.ActiveColumnIndex,
        //                            0, 0);

        //                        this.bsprFilter_LeaveCell(this.bsprFilter, es);
        //                        this.bsprFilter_Change(this.bsprFilter, ce);

        //                    }
        //                    DataSet ds_Test = (DataSet)this.bsprFilter.DataSource;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }

        //}

        private void bsprContext_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if (e.Column == (int)SPCModelContextSpread.CONTEXT_VALUE && this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.GROUP_YN].Text.Length > 0)
            {
                bool bResultOut = false;
                bool bTryResult = Boolean.TryParse(this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.GROUP_YN].Value.ToString(), out bResultOut);
                if (!bTryResult)
                {
                    if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.GROUP_YN].Text == Definition.VARIABLE_Y)
                    {
                        this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.GROUP_YN].Value = true;
                    }
                    else
                    {
                        this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.GROUP_YN].Value = false;
                    }
                }
               
                if (Convert.ToBoolean(this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.GROUP_YN].Value))
                {
                    if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Trim() == Definition.VARIABLE.STAR)
                    {
                    }
                    else if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Trim().Length == 0)
                    {
                        this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.GROUP_YN].Value = false;
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_EMPTY_CHANGE_UNCHECK", null, null);
                    }
                    else if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Split(';').Length == 1)
                    {
                        if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Split(';')[0].Trim().Length > 0)
                        {
                            if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Split(';')[0].Trim().IndexOf('*') < 0)
                            {
                                this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.GROUP_YN].Value = false;
                                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SINGLE_ITEM_CHANGE_UNCHECK", null, null);
                            }
                        }
                    }
                    else if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Split(';').Length > 1)
                    {
                        int _iContextValue = 0;
                        int _iHasValueIdx = -1;
                        string[] strArrTemp = this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Split(';');
                        for (int i = 0; i < strArrTemp.Length; i++)
                        {
                            if (strArrTemp[i].Trim().Length > 0)
                            {
                                _iContextValue++;
                                _iHasValueIdx = i;
                            }
                        }
                        if (_iContextValue == 0)
                        {
                            this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.GROUP_YN].Value = false;
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_EMPTY_CHANGE_UNCHECK", null, null);
                        }
                        else if (_iContextValue == 1)
                        {
                            if (strArrTemp[_iHasValueIdx].Trim().Length > 0)
                            {
                                if (strArrTemp[_iHasValueIdx].Trim().IndexOf('*') < 0)
                                {
                                    this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.GROUP_YN].Value = false;
                                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SINGLE_ITEM_CHANGE_UNCHECK", null, null);
                                    
                                    
                                }
                            }
                        }
                    }
                }
            }
            this.bsprContext.LeaveCellAction();
        }

        #endregion

        private void bsprContext_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (this.bsprContext.ActiveSheet.Rows.Count > 0)
            {
                if (e.Column == this._ColIdx_GROUP_YN)
                {
                    if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Trim() == Definition.VARIABLE.STAR 
                        && Convert.ToBoolean(this.bsprContext.ActiveSheet.Cells[e.Row, e.Column].Value))
                    {
                        //MSGHandler.DisplayMessage(MSGType.Information, "It's not allow to check group when the context value is '*'.");
                        //this.bsprContext.ActiveSheet.Cells[e.Row, e.Column].Value = false;
                    }
                    else if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Trim().Length == 0 
                        && Convert.ToBoolean(this.bsprContext.ActiveSheet.Cells[e.Row, e.Column].Value))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_CHECK_GROUP_EMPTY", null, null);
                        this.bsprContext.ActiveSheet.Cells[e.Row, e.Column].Value = false;
                    }
                    else if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Split(';').Length == 1 
                        && Convert.ToBoolean(this.bsprContext.ActiveSheet.Cells[e.Row, e.Column].Value))
                    {
                        if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Split(';')[0].Trim().Length > 0)
                        {
                            if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Split(';')[0].Trim().IndexOf('*') < 0)
                            {
                                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_CHECK_GROUP_SINGLE_ITEM", null, null);
                                this.bsprContext.ActiveSheet.Cells[e.Row, e.Column].Value = false;
                            }
                        }
                    }
                    else if (this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Split(';').Length > 1
                        && Convert.ToBoolean(this.bsprContext.ActiveSheet.Cells[e.Row, e.Column].Value))
                    {
                        int _iContextValue = 0;
                        int _iHasValueIdx = -1;
                        string [] strArrTemp = this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_VALUE].Text.Split(';');
                        for (int i = 0; i < strArrTemp.Length; i++)
                        {
                            if (strArrTemp[i].Trim().Length > 0)
                            {
                                _iContextValue++;
                                _iHasValueIdx = i;
                            }
                        }
                        if (_iContextValue == 0)
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_CHECK_GROUP_EMPTY", null, null);
                            this.bsprContext.ActiveSheet.Cells[e.Row, e.Column].Value = false;
                        }
                        else if (_iContextValue == 1)
                        {
                            if (strArrTemp[_iHasValueIdx].Trim().Length > 0)
                            {
                                if (strArrTemp[_iHasValueIdx].Trim().IndexOf('*') < 0)
                                {
                                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_CHECK_GROUP_SINGLE_ITEM", null, null);
                                    this.bsprContext.ActiveSheet.Cells[e.Row, e.Column].Value = false;
                                }
                            }
                        }
                        
                    }
                    
                }
            }
        }

        public void SetCheckBoxColumn()
        {
            FarPoint.Win.Spread.CellType.CheckBoxCellType cbcT = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            if (this.bsprContext != null && this.bsprContext.ActiveSheet.Rows.Count > 0)
            {
                this.bsprContext.ActiveSheet.Columns[(int)SPCModelContextSpread.GROUP_YN].CellType = cbcT;
            }
        }

        private void bchkUseNormalization_CheckedChanged(object sender, EventArgs e)
        {
            //SPC-705 Validate SameModule for All Value
            //if (((BCheckBox)sender).CheckState == CheckState.Checked)
            //{
            //    this.bchkValSameModule.CheckState = CheckState.Checked;
            //    this.bchkValSameModule.Enabled = true;
            //}
            //else
            //{
            //    this.bchkValSameModule.CheckState = CheckState.Unchecked;
            //    this.bchkValSameModule.Enabled = false;
            //}
        }

        // JIRA SPC-592 [GSMC] Disable "Use Normalization Value" - 2011.08.29 by ANDREW KO
        public void SetNormalization()
        {
            this.bchkUseNormalization.Visible = false;
            //SPC-705
            //this.bchkValSameModule.Visible = false;
            //this.tlpModel6.ColumnStyles[0].Width = 0;

        }

        private bool isMain(int indexRow)
        {
            if ((string)this._dtOriginalData.Rows[indexRow][COLUMN.MAIN_YN]== Definition.VARIABLE_Y)
            {
                return true;
            }
            return false;
        }

        private bool isActivated(int indexRow)
        {
            //return Convert.ToBoolean(this.bsprData.GetCellValue(indexRow, _ColIdx_Activation));

            if ((string)this._dtOriginalData.Rows[indexRow][COLUMN.ACTIVATION_YN] == Definition.VARIABLE_Y || (string)this._dtOriginalData.Rows[indexRow][COLUMN.ACTIVATION_YN] == Definition.VARIABLE_TRUE)
            {
                return true;
            }
            return false;
        }

        private string CheckActivation(int indexRow)
        {
            if (this.isMain(indexRow))
            {
                if (!this.bchkActive.Checked)
                {
                    for (int i = 0; i < this._dtOriginalData.Rows.Count; i++)
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
                if (this.bchkActive.Checked)
                {
                    for (int i = 0; i < this._dtOriginalData.Rows.Count; i++)
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

        private void bchkActive_CheckedChanged(object sender, EventArgs e)
        {
            int index = 0;

            for (int i = 0; i < this._dtOriginalData.Rows.Count; i++)
            {
                if (this._dtOriginalData.Rows[i][COLUMN.CHART_ID].ToString() == this.CONFIG_RAWID.ToString())
                    index = i;
            }

            if (this._dtOriginalData.Rows.Count > 0)
            {
                string msg = CheckActivation(index);
                if (string.Empty != msg)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, msg);

                    if (this.bchkActive.Checked)
                    {
                        this.bchkActive.Checked = false;
                    }
                    else
                    {
                        this.bchkActive.Checked = true;
                    }
                }
            }
        }


    }
}
