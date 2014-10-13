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


        BISTel.eSPC.Common.ATT.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.ATT.CommonUtility _ComUtil;

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

        private BISTel.eSPC.Common.ConfigMode _cofingMode;
        private string _sConfigRawID;

        private string _sAreaRawID;
        private string _sLineRawID;
        private string _sEQPModel;

       
        private string[] _saContextTypeName;
        //private string[] _saFilterTypeName;

        private DataTable _dtConfig;

        private bool _hasSubConfigs;

        FarPoint.Win.Spread.CellType.TextCellType tc = new FarPoint.Win.Spread.CellType.TextCellType();

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

            this.btxtSampleCount.Visible = false;
            this.blblSampleCount.Visible = false;

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

            //MANAGE TYPE
            llconidtion.Clear();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_MANAGE_TYPE);
            llconidtion.Add(Definition.CONDITION_KEY_USE_YN, Definition.VARIABLE_Y);

            DataSet _dsDisplayType = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            //_ComUtil.SetBComboBoxData(this.bcboManageType, _dsDisplayType, BISTel.eSPC.Common.COLUMN.NAME, BISTel.eSPC.Common.COLUMN.CODE, "", true);

            //AUTO TYPE
            llconidtion.Clear();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_AUTO_TYPE);
            llconidtion.Add(Definition.CONDITION_KEY_USE_YN, Definition.VARIABLE_Y);

            DataSet _dsAutoType = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            _ComUtil.SetBComboBoxData(this.bcboAutoType, _dsAutoType, BISTel.eSPC.Common.COLUMN.NAME, BISTel.eSPC.Common.COLUMN.CODE, "", true);

            // CHART MODE
            llconidtion.Clear();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CHART_MODE);
            llconidtion.Add(Definition.CONDITION_KEY_USE_YN, Definition.VARIABLE_Y);

            DataSet _dsChartMode = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            _ComUtil.SetBComboBoxData(this.bcboMode, _dsChartMode, BISTel.eSPC.Common.COLUMN.NAME, BISTel.eSPC.Common.COLUMN.CODE, "", true);

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
                if (this.llstContextType.Contains(arrDr[k][BISTel.eSPC.Common.COLUMN.NAME].ToString())) continue;

                this._contextTypeInfo = new SPCStruct.ContextTypeInfo(arrDr[k][BISTel.eSPC.Common.COLUMN.CODE].ToString(), arrDr[k][BISTel.eSPC.Common.COLUMN.NAME].ToString());
                this.llstContextType.Add(arrDr[k][BISTel.eSPC.Common.COLUMN.NAME].ToString(), this._contextTypeInfo);

                _dsContextType.Tables[0].Rows.Add(arrDr[k].ItemArray);
            }

            _saContextTypeName = _ComUtil.ConvertDataColumnIntoArray(_dsContextType.Tables[0], BISTel.eSPC.Common.COLUMN.NAME);
        }


        public void InitializeDataButton()
        {
            this._slBtnListIndex = this._Initialization.InitializeButtonList(this.bbtnListContext, ref bsprContext, Definition.PAGE_KEY_SPC_CONFIGURATION, Definition.PAGE_KEY_SPC_CONFIGURATION_CONTEXT, this.sessionData);

            this.FunctionName = Definition.FUNC_KEY_SPC_ATT_MODELING;
        }

        public void InitializeBSpread()
        {
            this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprContext, Definition.PAGE_KEY_SPC_CONFIGURATION, true, Definition.PAGE_KEY_SPC_CONFIGURATION_CONTEXT);
            this.bsprContext.UseHeadColor = true;

            this.bsprContext.UseAutoSort = false;
            this.bsprContext.RowInsertType = InsertType.Last;

            this._Initialization.SetCheckColumnHeader(this.bsprContext, 0);

            this.bsprContext.UseGeneralContextMenu = false;

            this._ColIdx_CONTEXT_KEY = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_CONTEXT_KEY)];
            this._ColIdx_SELECT = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT)];
            this._ColIdx_GROUP_YN = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_SPC_GROUP)];
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
                                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_SET_CONTEXT_SINGLE_ITEM", null);
                                    continue;
                                }
                            }
                        }

                        this.bsprContext.ActiveSheet.Cells[i, (int)SPCModelContextSpread.CONTEXT_VALUE].CellType = tc;
                        if (alTempContextValues[idxTemp].ToString().Length > 0)
                        {
                            this.bsprContext.ActiveSheet.SetText(i, (int)SPCModelContextSpread.CONTEXT_VALUE, alTempContextValues[idxTemp].ToString());
                        }
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

        public bool InputValidation(bool isMsgDisplay, bool isDefault)
        {
            bool result = true;

            if (!isDefault)
            {

                if (PARAM_ALIAS.Length.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_PARAM", null, null);
                    result = false;
                }

                if (this.bsprContext.ActiveSheet.RowCount.Equals(0))
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_COMPOSE_CONTEXT", null, null);
                    result = false;
                }

            }

            if (_ComUtil.DuplicateCheck(this.CONTEXT_DATASET, BISTel.eSPC.Common.COLUMN.CONTEXT_KEY))
            {
                if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_DUPLICATE_CONTEXT", null, null);
                result = false;
            }

            foreach (DataRow drContexts in this.CONTEXT_DATASET.Rows)
            {
                if (drContexts.RowState.Equals(DataRowState.Deleted))
                    continue;

                if (_ComUtil.NVL(drContexts[BISTel.eSPC.Common.COLUMN.CONTEXT_VALUE]).Length.Equals(0))
                {
                    if (!isDefault)
                    {
                        if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_CONTEXT_VALUE", null, null);
                        result = false;
                        break;
                    }
                }
                else if (_ComUtil.NVL(drContexts[BISTel.eSPC.Common.COLUMN.CONTEXT_VALUE]).Length > 1024)
                {
                    if (isMsgDisplay) MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_CONTEXT_CHAR_SIZE", null, null);
                    result = false;
                    break;
                }
                else
                {
                }
            }

            return result;
        }

        #region :DATA PROPERTY
        //DATA PROPERTY

        public LinkedList llstContextType
        {
            get { return _llstContextType; }
            set { _llstContextType = value; }
        }

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


        public string PARAM_ALIAS
        {
            get { return this.btxtParamAlias.Text; }
            set { this.btxtParamAlias.Text = value; }
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

                    if (_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.CREATE_SUB))
                    {
                        ((DataSet)this.bsprContext.DataSet).AcceptChanges();
                    }
                }
            }
        }

        public DataTable CONFIG_DATASET
        {
            get
            {
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

                    if (_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.CREATE_MAIN))
                    {
                        //기본값
                        _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.MAIN_YN] = "Y";
                        //_dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.AUTO_SUB_YN] = "Y";

                        if (this.bcboAutoType.Items.Count > 0)
                            _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.AUTO_TYPE_CD] = ((DataRowView)this.bcboAutoType.Items[0]).Row["CODE"];

                        if (this.bcboMode.Items.Count > 0)
                            _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.CHART_MODE_CD] = ((DataRowView)this.bcboMode.Items[0]).Row["CODE"];

                        _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.VALIDATION_SAME_MODULE_YN] = "N";
                    }
                    else if (_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.CREATE_SUB))
                    {
                        _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.MAIN_YN] = "N";
                    }
                    else if (_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.DEFAULT))
                    {

                    }


                    //# Control에 DataBinding 하기

                    //PARAM_ALIAS
                    this.btxtParamAlias.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.PARAM_ALIAS);

                    //COMPLEX_YN
                    Binding bdComplexYN = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.COMPLEX_YN);
                    bdComplexYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdComplexYN.Parse += new ConvertEventHandler(BooleanToStringYN);

                    //MAIN_YN
                    Binding bdMainYN = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.MAIN_YN);
                    bdMainYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdMainYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkMainYN.DataBindings.Add(bdMainYN);

                    //USE_EXTERNAL_SPEC_YN
                    Binding bdUseExtSpecYN = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.USE_EXTERNAL_SPEC_YN);
                    bdUseExtSpecYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdUseExtSpecYN.Parse += new ConvertEventHandler(BooleanToStringYN);

                    //INTERLOCK_YN
                    Binding bdInterlockYN = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.INTERLOCK_YN);
                    bdInterlockYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdInterlockYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkInterlockYN.DataBindings.Add(bdInterlockYN);

                    //AUTOCALC_YN
                    Binding bdAutoCalcYN = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.AUTOCALC_YN);
                    bdAutoCalcYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdAutoCalcYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkAutoCalcYN.DataBindings.Add(bdAutoCalcYN);

                    //AUTO_SUB_YN
                    Binding bdAutoSubYN = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.AUTO_SUB_YN);
                    bdAutoSubYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdAutoSubYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkAutoSubYN.DataBindings.Add(bdAutoSubYN);

                    //AUTO_TYPE_CD
                    this.bcboAutoType.DataBindings.Add("SelectedValue", _dtConfig, BISTel.eSPC.Common.COLUMN.AUTO_TYPE_CD);

                    //SAMPLE_COUNT
                    //this.btxtSampleCount.DataBindings.Add("Text", _dtConfig, BISTel.eSPC.Common.COLUMN.SAMPLE_COUNT);

                    //ACTIVATION_YN
                    Binding bdActivationYN = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.ACTIVATION_YN);
                    bdActivationYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdActivationYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkActive.DataBindings.Add(bdActivationYN);

                    //SUB_INTERLOCK_YN
                    Binding bdSubInterlockYN = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.SUB_INTERLOCK_YN);
                    bdSubInterlockYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdSubInterlockYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkCreateSubInterlock.DataBindings.Add(bdSubInterlockYN);

                    //ACTIVATION_YN
                    Binding bdInheritMainYN = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.INHERIT_MAIN_YN);
                    bdInheritMainYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdInheritMainYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkInheritYN.DataBindings.Add(bdInheritMainYN);

                    //SUB_AUTOCALC_YN
                    Binding bdSubAutoCalcYN = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.SUB_AUTOCALC_YN);
                    bdSubAutoCalcYN.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdSubAutoCalcYN.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkCreateSubAutoCalc.DataBindings.Add(bdSubAutoCalcYN);

                    // USE_NORAMLIZATION
                    Binding bdUseNormalization = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.USE_NORM_YN);
                    bdUseNormalization.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdUseNormalization.Parse += new ConvertEventHandler(BooleanToStringYN);
                    //this.bchkUseNormalization.DataBindings.Add(bdUseNormalization);

                    // CHART_MODE_CD
                    this.bcboMode.DataBindings.Add("SelectedValue", _dtConfig, BISTel.eSPC.Common.COLUMN.CHART_MODE_CD);

                    // VALIDATION_SAME_MODULE
                    Binding bdValidationSameModule = new Binding("Checked", _dtConfig, BISTel.eSPC.Common.COLUMN.VALIDATION_SAME_MODULE_YN);
                    bdValidationSameModule.Format += new ConvertEventHandler(StringYNToBoolean);
                    bdValidationSameModule.Parse += new ConvertEventHandler(BooleanToStringYN);
                    this.bchkValSameModule.DataBindings.Add(bdValidationSameModule);
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



        public void InitializeLayout(BISTel.eSPC.Common.ConfigMode configMode)
        {
            this.bchkMainYN.Enabled = false;

            switch (configMode)
            {
                case BISTel.eSPC.Common.ConfigMode.DEFAULT://디폴트 셋팅
                    this.gbParam.Visible = false;
                    this.tlpModel4.Visible = false;
                    this.tlpModel5.Visible = false;
                    this.bchkActive.Visible = false;
                    this.bcboAutoType.Visible = false;
                    this.bcboMode.Visible = false;
                    this.blblAutoSettings.Visible = false;
                    this.blbMode.Visible = false;
                    break;

                case BISTel.eSPC.Common.ConfigMode.CREATE_MAIN:
                case BISTel.eSPC.Common.ConfigMode.SAVE_AS:

                    this.tlpModel5.Visible = false;
                    break;

                case BISTel.eSPC.Common.ConfigMode.CREATE_SUB:
                    this.gbParam.Enabled = false;

                    this.bbtnParamAlias.Visible = false;
                    this.bbtnListContext.Visible = false;
                    this.bsprContext.ContextMenu.MenuItems.Clear();

                    this.bchkAutoSubYN.Visible = false;
                    this.blblAutoSettings.Visible = false;
                    this.bcboAutoType.Visible = false;
                    this.tlpModel4.Visible = false;
                    this.bsprContext.ActiveSheet.Columns[(int)SPCModelContextSpread.GROUP_YN].Visible = false;

                    break;

                case BISTel.eSPC.Common.ConfigMode.MODIFY:
                    if (this.bchkMainYN.Checked)
                    {//Main인 경우

                        if (_hasSubConfigs)
                        {
                            this.gbParam.Enabled = false;
                            this.bbtnParamAlias.Visible = false;

                            this.bbtnListContext.Visible = true;
                            this.bsprContext.ContextMenu.MenuItems.Clear();
                            this.bsprContext.ActiveSheet.Columns[(int)SPCModelContextSpread.CONTEXT_KEY_NAME].Locked = true;

                            this.gbContext.Enabled = true;
                        }

                        this.tlpModel5.Visible = false;
                    }
                    else
                    {//Sub인 경우

                        this.gbParam.Enabled = false;
                        this.bbtnParamAlias.Visible = false;
                        this.bbtnListContext.Visible = false;
                        this.bsprContext.ContextMenu.MenuItems.Clear();
                        this.bchkAutoSubYN.Visible = false;
                        this.blblAutoSettings.Visible = false;
                        this.bcboAutoType.Visible = false;
                        this.tlpModel4.Visible = false;
                        this.bsprContext.ActiveSheet.Columns[(int)SPCModelContextSpread.GROUP_YN].Visible = false;
                    }
                    break;

                case BISTel.eSPC.Common.ConfigMode.VIEW:

                    this.gbParam.Enabled = false;
                    this.bbtnParamAlias.Visible = false;
                    this.gbContext.Enabled = false;
                    this.bbtnListContext.Visible = false;
                    this.gbModel.Enabled = false;
                    if (this.bchkMainYN.Checked)
                    {//Main인 경우
                        this.tlpModel5.Visible = false;
                    }
                    else
                    {//Sub인 경우
                        this.bchkAutoSubYN.Visible = false;
                        this.blblAutoSettings.Visible = false;
                        this.bcboAutoType.Visible = false;
                        this.tlpModel4.Visible = false;
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
            SelectParamPopup selectParamPopup = new SelectParamPopup();
            selectParamPopup.URL = this.URL;
            selectParamPopup.PORT = this.Port;
            selectParamPopup.LINE_RAWID = this._sLineRawID;
            selectParamPopup.AREA_RAWID = this._sAreaRawID;
            selectParamPopup.EQP_MODEL = this._sEQPModel;
            selectParamPopup.InitializePopup();

            DialogResult result = selectParamPopup.ShowDialog();

            if (result.Equals(DialogResult.OK))
            {
                this.btxtParamAlias.Text = selectParamPopup.SELECTED_PARAM_ALIAS;
                _dtConfig.Rows[0][BISTel.eSPC.Common.COLUMN.PARAM_ALIAS] = selectParamPopup.SELECTED_PARAM_ALIAS;
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
                    SortedList slSelectedRows = this.bsprContext.GetSelectedRows();
                    if (slSelectedRows.Count > 0)
                    {
                        int iOffset = 0;
                        foreach (int iRow in slSelectedRows.Values)
                        {
                            int iRemoveRow = iRow - iOffset;
                            this.bsprContext.RemoveRow(iRemoveRow, iRemoveRow);
                            iOffset++;
                        }
                        this.SetContextKeyOrder();
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_REGION_SPREAD", null, null);
                    }
                }
            }
            else if (name.ToUpper().Equals(Definition.BUTTON_KEY_GET_VALUE))
            {
                if (bsprContext.ActiveSheet.RowCount > 0)
                {
                    if (this.bchkMainYN.Checked && _hasSubConfigs)
                    {
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
            }
        }          
      
        private void bsprContext_AddRowEvent(object sender, int row)
        {
            _SpreadUtil.SetComboBoxCellType(bsprContext.ActiveSheet.Cells[row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME], _saContextTypeName, false);

            this.SetContextKeyOrder();

            string _contextKeyName = bsprContext.GetCellValue(row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME).ToString();
            bsprContext.SetCellValue(row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME, bsprContext.GetCellValue(row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME));

            if (this.llstContextType[_contextKeyName] != null)
            {
                this._contextTypeInfo = this.llstContextType[_contextKeyName] as SPCStruct.ContextTypeInfo;
                this.bsprContext.ActiveSheet.Cells[row, (int)SPCModelContextSpread.CONTEXT_KEY].Text = _contextTypeInfo.CODE;
            }
            else
                bsprContext.SetCellValue(row, (int)SPCModelContextSpread.CONTEXT_KEY, bsprContext.GetCellValue(row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME));
        }

       

        private void bsprContext_ComboSelChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string _contextKeyName = bsprContext.GetCellValue(e.Row, e.Column).ToString();
            bsprContext.SetCellValue(e.Row, (int)SPCModelContextSpread.CONTEXT_KEY_NAME, bsprContext.GetCellValue(e.Row, e.Column));

            if (this.llstContextType[_contextKeyName] != null)
            {
                this._contextTypeInfo = this.llstContextType[_contextKeyName] as SPCStruct.ContextTypeInfo;
                this.bsprContext.ActiveSheet.Cells[e.Row, (int)SPCModelContextSpread.CONTEXT_KEY].Text = _contextTypeInfo.CODE;
            }
            else
                bsprContext.SetCellValue(e.Row, (int)SPCModelContextSpread.CONTEXT_KEY, bsprContext.GetCellValue(e.Row, e.Column));
        }

       


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

        private void bsprContext_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {

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

       
        public void SetNormalization()
        {
            //this.bchkUseNormalization.Visible = false;
            //SPC-705
            //this.bchkValSameModule.Visible = false;
            //this.tlpModel6.ColumnStyles[0].Width = 0;

        }
    }
}
