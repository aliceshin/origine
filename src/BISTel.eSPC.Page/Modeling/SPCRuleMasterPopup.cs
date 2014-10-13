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
    public partial class SPCRuleMasterPopup : BasePopupFrm
    {
        #region : Constructor
        public SPCRuleMasterPopup()
        {
            InitializeComponent();
        }

        #endregion


        #region : Field

        SessionData _SessionData;
        string _sPort;

        Initialization _Initialization;
        BSpreadUtility _bspreadutility;
        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition = new LinkedList();

        eSPCWebService.eSPCWebService _wsSPC = null;


        BISTel.eSPC.Common.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.CommonUtility _ComUtil;

        private SortedList _slColumnIndex = new SortedList();
        private SortedList _slBtnListIndex = new SortedList();

        private SortedList _slColumnIndexOCAP = new SortedList();

        private int _ColIdx_SELECT;

        private int _ColIdx_RULE_NO;
        private int _ColIdx_RULE_OPTION_NO;
        private int _ColIdx_RULE_OPTION_NAME;
        private int _ColIdx_DESCRIPTION;
        private int _ColIdx_VALUE;

        private int _ColIdx_CODE;
        private int _ColIdx_NAME;

        private ConfigMode _cofingMode = ConfigMode.CREATE_SUB;

        string _sConfigRawID;

        string _sRuleNO;

        private DataSet _dsSPCRuleMstData;
        private DataTable _dtSPCRule;
        private DataTable _dtSPCRuleOpt;

        //SPC-736 OCAPAction(option) by louis you 2012.02.10
        public bool _sOCAPOfSingle = false;

        private bool _bUseComma = false;

        #endregion


        #region : Property

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

        public ConfigMode CONFIG_MODE
        {
            get { return _cofingMode; }
            set
            {
                _cofingMode = value;

                this.InitializeLayout();
            }
        }

        public string CONFIG_RAWID
        {
            get { return _sConfigRawID; }
            set { _sConfigRawID = value; }
        }

        public string RULE_NO
        {
            get { return _sRuleNO; }
            set { _sRuleNO = value; }
        }

        public DataTable SPCRULE_DATATABLE
        {
            get { return _dtSPCRule; }
            set { _dtSPCRule = value; }
        }

        public DataTable SPCRULEOPT_DATATABLE
        {
            get { return _dtSPCRuleOpt; }
            set
            {
                _dtSPCRuleOpt = value;

                if (_cofingMode.Equals(ConfigMode.CREATE_SUB) && _dtSPCRuleOpt == null)
                {
                    _dtSPCRuleOpt = new DataTable(TABLE.MODEL_RULE_OPT_MST_SPC);

                    _dtSPCRuleOpt.Columns.Add(COLUMN.MODEL_RULE_RAWID);
                    _dtSPCRuleOpt.Columns.Add(COLUMN.SPC_RULE_NO);
                    _dtSPCRuleOpt.Columns.Add(COLUMN.RULE_OPTION_NO);
                    _dtSPCRuleOpt.Columns.Add(COLUMN.RULE_OPTION_VALUE);
                    _dtSPCRuleOpt.Columns.Add(COLUMN.OPTION_NAME);
                    _dtSPCRuleOpt.Columns.Add(COLUMN.DESCRIPTION);
                }
            }
        }

        public bool USE_COMMA
        {
            get { return _bUseComma; }
            set { _bUseComma = value; }
        }

        #endregion


        #region : Initialization

        public void InitializePopup()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._SpreadUtil = new BSpreadUtility();
            this._ComUtil = new CommonUtility();

            this.InitializeBSpread();
            this.InitializeRuleData();
        }

        public void InitializeBSpread()
        {
            //#01. RULE OPTION
            this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprRuleOpt, Definition.PAGE_KEY_SPC_CONFIGURATION, true, Definition.PAGE_KEY_SPC_CONFIGURATION_RULE_MASTER);
            this.bsprRuleOpt.UseHeadColor = true;

            this.bsprRuleOpt.UseAutoSort = false;

            this._ColIdx_RULE_NO = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_RULE_NO)];
            this._ColIdx_RULE_OPTION_NO = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_RULE_OPTION_NO)];
            this._ColIdx_RULE_OPTION_NAME = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_RULE_OPTION_NAME)];
            this._ColIdx_DESCRIPTION = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_DESCRIPTION)];
            this._ColIdx_VALUE = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_VALUE)];

            //SPC-907 ContextMenu Delete  by Louis
            this.bsprRuleOpt.UseGeneralContextMenu = false;
            this.bsprOCAP.UseGeneralContextMenu = false;

            this.bsprRuleOpt.ActiveSheet.Columns[_ColIdx_VALUE].BackColor = Color.Yellow;

            //this._Initialization.SetCheckColumnHeader(this.bsprContext, 0);

            //#02. OCAP LIST
            this._slColumnIndexOCAP = this._Initialization.InitializeColumnHeader(ref bsprOCAP, Definition.PAGE_KEY_SPC_CONFIGURATION, true, Definition.PAGE_KEY_SPC_CONFIGURATION_RULE_OCAP);
            this.bsprOCAP.UseHeadColor = true;
            this.bsprOCAP.UseAutoSort = false;

            this._ColIdx_SELECT = (int)this._slColumnIndexOCAP[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT)];
            this._ColIdx_CODE = (int)this._slColumnIndexOCAP[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_CODE)];
            this._ColIdx_NAME = (int)this._slColumnIndexOCAP[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_NAME)];

            //OCAP LIST Binding
            LinkedList llconidtion = new LinkedList();
            llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_OCAP_TYPE);

            DataSet dsOCAPType = this._wsSPC.GetCodeData(llconidtion.GetSerialData());

            this.bsprOCAP.DataSet = dsOCAPType;
        }
        
        public void InitializeRuleData()
        {
            switch (_cofingMode)
            {
                case ConfigMode.CREATE_SUB:
                    this.Initialize_Create();
                    break;

                case ConfigMode.MODIFY:
                    this.Initialize_Modify();
                    break;
            }
        }
        
        private void Initialize_Create()
        {
            _dsSPCRuleMstData = this._wsSPC.GetSPCRuleMasterData(null);

            if (_dsSPCRuleMstData == null || _dsSPCRuleMstData.Tables.Count == 0 || _dsSPCRuleMstData.Tables[0].Rows.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Error, "SPC_INFO_NOT_EXIST_RULE_MASTER", null, null);
                return;
            }

            _ComUtil.SetBComboBoxData(this.bcboRule, _dsSPCRuleMstData, COLUMN.SPC_RULE, COLUMN.RAWID, "", true);

            this.bbtnOK.Key = Definition.BUTTON_KEY_ADD;
            this.bbtnCancel.Key = Definition.BUTTON_KEY_CLOSE;
        }

        private void Initialize_Modify()
        {
            //#01. RULE
            DataRow[] drSelectRule = _dtSPCRule.Select(string.Format("{0} = '{1}'", COLUMN.SPC_RULE_NO, _sRuleNO));

            this.bcboRule.DropDownStyle = ComboBoxStyle.Simple;
            this.bcboRule.Text = string.Format("{0}. {1}", drSelectRule[0][COLUMN.SPC_RULE_NO], drSelectRule[0][COLUMN.DESCRIPTION]);
            this.bcboRule.Enabled = false;

            //#02. RULE OPTION
            DataTable dtSelectRuleOPT = _dtSPCRuleOpt.Clone();

            DataRow[] drSelectRuleOPTs = _dtSPCRuleOpt.Select(string.Format("{0} = '{1}'", COLUMN.SPC_RULE_NO, _sRuleNO), COLUMN.RULE_OPTION_NO);

            foreach (DataRow drSelectRuleOPT in drSelectRuleOPTs)
            {
                dtSelectRuleOPT.ImportRow(drSelectRuleOPT);
            }

            this.bsprRuleOpt.DataSet = dtSelectRuleOPT;

            if (bsprRuleOpt.ActiveSheet.RowCount > 0)
            {
                for (int i = 0; i < this.bsprRuleOpt.ActiveSheet.RowCount; i++)
                {
                    if (this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "Center line" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "LCL" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "UCL" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "USL" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "LSL" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "zone A" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "zone B" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "zone C" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "Trend limit" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "lambda")
                    {
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Locked = true;
                    }
                    else if (this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text.ToUpper() == "SAMPLE COUNT")
                    {
                        if (this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_NO].Text == "60" ||
                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_NO].Text == "61" ||
                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_NO].Text == "62" ||
                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_NO].Text == "63")
                        {

                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Locked = false;
                        }
                        else
                        {
                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Text = "";
                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Locked = true;
                        }
                    }
                    else
                    {
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Locked = false;
                    }
                }
            }

            this.bsprRuleOpt.Focus();


            //#03. OCAP LIST
            string sOCAPList = drSelectRule[0][COLUMN.OCAP_LIST].ToString();

            List<string> lstOCAPList = new List<string>(sOCAPList.Split(';'));

            string sOCAPCode = string.Empty;

            for (int i = 0; i < bsprOCAP.ActiveSheet.RowCount; i++)
            {
                sOCAPCode = bsprOCAP.GetCellText(i, _ColIdx_CODE);

                if (lstOCAPList.Contains(sOCAPCode))
                {
                    bsprOCAP.SetCellValue(i, _ColIdx_SELECT, true);
                }
            }

            this.bbtnOK.Key = Definition.BUTTON_KEY_OK;
            this.bbtnCancel.Key = Definition.BUTTON_KEY_CANCEL;
        }

        public void InitializeLayout()
        {

        }
        
        #endregion


        #region : Private Method

        /// <summary>
        /// Rule Option에 적은 값이 올바른지 검증한다.
        /// </summary>
        /// <returns>검증완료되면 true, 아니면 false</returns>
        private bool ValidateRuleOption()
        {
            //SPC-1262 KB LEE : Rule Option Validation 기능 추가

            for (int i = 0; i < this.bsprRuleOpt.ActiveSheet.RowCount; i++)
            {
                if (this.bsprRuleOpt.ActiveSheet.Cells[i, _ColIdx_VALUE].Text != null && this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == Definition.RULE_OPT_LIST_OF_RULES)
                {
                    if (this.bsprRuleOpt.ActiveSheet.Cells[i, _ColIdx_VALUE].Text.ToString().Length > 0)
                    {
                        char[] charArray = this.bsprRuleOpt.ActiveSheet.Cells[i, _ColIdx_VALUE].Text.ToString().ToCharArray();
                        for (int j = 0; j < charArray.Length; j++)
                        {
                            if (_bUseComma)
                            {
                                if (!(char.IsNumber(charArray[j]) || charArray[j].ToString() == "," || charArray[j].ToString() == "."))
                                {
                                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VAILD_INPUT_FIELD_COMMA", null, null);
                                    return false;
                                }
                            }
                            else
                            {
                                if (!(char.IsNumber(charArray[j]) || charArray[j].ToString() == ";" || charArray[j].ToString() == "."))
                                {
                                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VAILD_INPUT_FIELD", null, null);
                                    return false;
                                }
                            }
                        }
                    }
                }

                //SPC-1303, KBLEE, START
                //Cell의 Text를 Parse했을 때 숫자일 경우 반환되는 값
                int textParseResult;

                //N값이 0보다 커야 한다는 Validation
                if (this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text.ToUpper() == "N")
                {
                    if (!Int32.TryParse(this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Text, out textParseResult))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { "N" }, null);
                        return false;
                    }

                    //0보다 커야 한다는 Validation
                    if (textParseResult <= 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { "N" }, null);
                        return false;
                    }
                }

                //M값에 관한 Validation (0보다 커야 한다. & N보다 크거나 같아야 한다.)
                if (this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text.ToUpper() == "M")
                {
                    if (!Int32.TryParse(this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Text, out textParseResult))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { "M" }, null);
                        return false;
                    }

                    int valueM = textParseResult;
                    int valueN = 0;

                    for (int j = 0; j < this.bsprRuleOpt.ActiveSheet.RowCount; j++)
                    {
                        if (this.bsprRuleOpt.ActiveSheet.Cells[j, this._ColIdx_RULE_OPTION_NAME].Text.ToUpper() == "N")
                        {
                            if (!Int32.TryParse(this.bsprRuleOpt.ActiveSheet.Cells[j, this._ColIdx_VALUE].Text, out textParseResult))
                            {
                                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { "N" }, null);
                                return false;
                            }

                            valueN = textParseResult;
                            break;
                        }
                    }

                    //0보다 커야 한다는 Validation
                    if (valueM <= 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { "M" }, null);
                        return false;
                    }

                    //N보다 크거나 같아야 한다는 Validation
                    if (valueN > valueM)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_BIGGER", new string[] { "N", "M" }, null);
                        return false;
                    }
                }

                //Rule No. 60 이상의 Option에 관한 Validation (0보다 커야 한다. & Sample Count가 Violation Count보다 크거나 같아야 한다.)
                if (this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == Definition.RULE_OPT_VIOLATION_COUNT)
                {
                    if (!Int32.TryParse(this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Text, out textParseResult))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { Definition.RULE_OPT_VIOLATION_COUNT }, null);
                        return false;
                    }

                    int valueViolationCount = textParseResult;
                    int valueSampleCount = 0;

                    for (int j = 0; j < this.bsprRuleOpt.ActiveSheet.RowCount; j++)
                    {
                        if (this.bsprRuleOpt.ActiveSheet.Cells[j, this._ColIdx_RULE_OPTION_NAME].Text == Definition.RULE_OPT_SAMPLE_COUNT)
                        {
                            if (!Int32.TryParse(this.bsprRuleOpt.ActiveSheet.Cells[j, this._ColIdx_VALUE].Text, out textParseResult))
                            {
                                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { Definition.RULE_OPT_SAMPLE_COUNT }, null);
                                return false;
                            }

                            valueSampleCount = textParseResult;
                            break;
                        }
                    }

                    //Sample count가 0보다 커야 한다는 Validation
                    if (valueSampleCount <= 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { Definition.RULE_OPT_SAMPLE_COUNT }, null);
                        return false;
                    }

                    //Violation count가 0보다 커야 한다는 Validation
                    if (valueViolationCount <= 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RULE_VALID_POSITIVE_NUMBER", new string[] { Definition.RULE_OPT_VIOLATION_COUNT }, null);
                        return false;
                    }

                    //Sample Count가 Violation Count보다 크거나 같아야 한다는 Validation
                    if (valueViolationCount > valueSampleCount)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VALID_VALUE_BIGGER", new string[] { Definition.RULE_OPT_VIOLATION_COUNT, Definition.RULE_OPT_SAMPLE_COUNT }, null);
                        return false;
                    }
                }

                //SPC-1303, KBLEE, END
            }

            return true;
        }

        #endregion


        #region : Event

        private void bcboRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CONFIG OPTION MST
            DataTable dtTmpRuleOpt = _dsSPCRuleMstData.Tables[TABLE.RULE_OPT_MST_SPC];

            DataTable dtRuleOpt = dtTmpRuleOpt.Clone();
            DataRow[] drRuleOpts = dtTmpRuleOpt.Select(string.Format("{0} = '{1}'", COLUMN.RULE_RAWID, bcboRule.SelectedValue), COLUMN.RULE_OPTION_NO);

            foreach (DataRow drRuleOpt in drRuleOpts)
            {
                dtRuleOpt.ImportRow(drRuleOpt);
            }

            this.bsprRuleOpt.DataSet = dtRuleOpt;

            for (int i = 0; i < this.bsprRuleOpt.ActiveSheet.RowCount; i++)
            {
                this.bsprRuleOpt.ActiveSheet.Cells[i, _ColIdx_RULE_OPTION_NAME].Note = this.bsprRuleOpt.GetCellText(i, _ColIdx_DESCRIPTION);
            }

            if (bsprRuleOpt.ActiveSheet.RowCount > 0)
            {
                for (int i = 0; i < this.bsprRuleOpt.ActiveSheet.RowCount; i++)
                {
                    if (this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "Center line" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "LCL" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "UCL" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "USL" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "LSL" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "zone A" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "zone B" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "zone C" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "Trend limit" ||
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text == "lambda")
                    {
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Locked = true;
                    }
                    else if (this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_OPTION_NAME].Text.ToUpper() == "SAMPLE COUNT")
                    {
                        if (this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_NO].Text == "60" ||
                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_NO].Text == "61" ||
                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_NO].Text == "62" ||
                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_RULE_NO].Text == "63")
                        {

                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Locked = false;
                        }
                        else
                        {
                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Text = "";
                            this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Locked = true;
                        }
                    }
                    else
                    {
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Locked = false;
                    }
                }
            }
        }

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            //선택된 OCAP LIST만들기
            DataSet dsSelectOCAP = _SpreadUtil.GetSelectedDataSet(bsprOCAP, _ColIdx_SELECT);
            string sOCAPList = _ComUtil.ConvertDataColumnIntoStringList(dsSelectOCAP.Tables[0], COLUMN.CODE, ";");
            
            if (string.IsNullOrEmpty(sOCAPList))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_OCAP_ITEM", null, null);
                return;
            }

            if (this.bsprRuleOpt.ActiveSheet.Rows.Count > 0)
            {
                for (int i = 0; i < this.bsprRuleOpt.ActiveSheet.RowCount; i++)
                {
                    if (!this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Locked)
                    {
                        if (string.IsNullOrEmpty(this.bsprRuleOpt.GetCellText(i, this._ColIdx_VALUE)))
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_RULE_OPT", null, null);
                            return;
                        }
                    }
                }
            }

            string sRuleOption = string.Empty;
            string sRuleOptionData = string.Empty;

            switch (_cofingMode)
            {
                case ConfigMode.CREATE_SUB:
                    this.bsprRuleOpt.LeaveCellAction();
                    string sRuleRawid = bcboRule.SelectedValue.ToString();

                    DataRow[] drSelectRuleMst = _dsSPCRuleMstData.Tables[TABLE.RULE_MST_SPC].Select(string.Format("{0} = '{1}'", COLUMN.RAWID, sRuleRawid));

                    //기존 RULE이 있는지 확인
                    string sRuleNo = drSelectRuleMst[0][COLUMN.SPC_RULE_NO].ToString();
                    string sRuleDesc = drSelectRuleMst[0][COLUMN.DESCRIPTION].ToString();

                    DataRow[] drExistRuleMst = _dtSPCRule.Select(string.Format("{0} = '{1}'", COLUMN.SPC_RULE_NO, sRuleNo));

                    if (drExistRuleMst != null && drExistRuleMst.Length > 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_DUPLICATE_RULE", null, null);
                        return;
                    }

                    //없으면 RULE 추가
                    DataRow drNewRuleMst = _dtSPCRule.NewRow();

                    string sNewModelRuleRawid = "-" + sRuleNo;

                    drNewRuleMst[COLUMN.RAWID] = sNewModelRuleRawid;
                    drNewRuleMst[COLUMN.MODEL_CONFIG_RAWID] = _ComUtil.NVL(_sConfigRawID, "-1", true);
                    drNewRuleMst[COLUMN.SPC_RULE_NO] = sRuleNo;
                    drNewRuleMst[COLUMN.OCAP_LIST] = sOCAPList;
                    drNewRuleMst[COLUMN.USE_MAIN_SPEC_YN] = "Y";
                    drNewRuleMst[COLUMN.USE_MAIN_SPEC] = "True";
                    drNewRuleMst[COLUMN.DESCRIPTION] = sRuleDesc;
                    //drNewRuleMst["RULE_OPTION"] = "";
                    //drNewRuleMst["RULE_OPTION_DATA"] = "";

                    DataTable dtSelectRuleOpt = ((DataSet)bsprRuleOpt.DataSet).Tables[0];
                    bool bcheckString = false;

                    foreach (DataRow drSelectRuleOpt in dtSelectRuleOpt.Rows)
                    {
                        DataRow drNewRuleOpt = _dtSPCRuleOpt.NewRow();

                        //drNewRuleOpt["RAWID"] = "";
                        drNewRuleOpt[COLUMN.MODEL_RULE_RAWID] = sNewModelRuleRawid;
                        drNewRuleOpt[COLUMN.SPC_RULE_NO] = drSelectRuleOpt[COLUMN.SPC_RULE_NO];
                        drNewRuleOpt[COLUMN.RULE_OPTION_NO] = drSelectRuleOpt[COLUMN.RULE_OPTION_NO];

                        //SPC-1262 KB LEE : Rule Option Validation 기능 추가
                        if (!ValidateRuleOption())
                        {
                            return;
                        }

                        drNewRuleOpt[COLUMN.RULE_OPTION_VALUE] = drSelectRuleOpt[COLUMN.RULE_OPTION_VALUE];
                        drNewRuleOpt[COLUMN.OPTION_NAME] = drSelectRuleOpt[COLUMN.OPTION_NAME];
                        drNewRuleOpt[COLUMN.DESCRIPTION] = drSelectRuleOpt[COLUMN.DESCRIPTION];

                        _dtSPCRuleOpt.Rows.Add(drNewRuleOpt);

                        sRuleOption += string.Format("{0}={1};", drSelectRuleOpt[COLUMN.OPTION_NAME], drSelectRuleOpt[COLUMN.RULE_OPTION_VALUE]);
                        sRuleOptionData += string.Format("{0}.{1}={2};", drSelectRuleOpt[COLUMN.RULE_OPTION_NO], drSelectRuleOpt[COLUMN.OPTION_NAME], drSelectRuleOpt[COLUMN.RULE_OPTION_VALUE]);

                    }

                    drNewRuleMst[COLUMN.RULE_OPTION] = sRuleOption;
                    drNewRuleMst[COLUMN.RULE_OPTION_DATA] = sRuleOptionData;

                    _dtSPCRule.Rows.Add(drNewRuleMst);

                    break;

                case ConfigMode.MODIFY :

                    //#01. RULE OPTION
                    this.bsprRuleOpt.LeaveCellAction();
                    DataTable dtModifyRuleOpt = ((DataSet)this.bsprRuleOpt.DataSet).Tables[0];

                    foreach (DataRow drModifyRuleOpt in dtModifyRuleOpt.Rows)
                    {
                        DataRow[] drModifyRuleOPTs = _dtSPCRuleOpt.Select(string.Format("{0} = '{1}' AND {2} = '{3}'",
                                                                                                        COLUMN.MODEL_RULE_RAWID,
                                                                                                        drModifyRuleOpt[COLUMN.MODEL_RULE_RAWID],
                                                                                                        COLUMN.RULE_OPTION_NO,
                                                                                                        drModifyRuleOpt[COLUMN.RULE_OPTION_NO])
                                                                                                        );
                        
                        //SPC-1262 KB LEE : Rule Option Validation 기능 추가
                        if (!ValidateRuleOption())
                        {
                            return;
                        }

                        if (drModifyRuleOPTs != null && drModifyRuleOPTs.Length > 0)
                            drModifyRuleOPTs[0][COLUMN.RULE_OPTION_VALUE] = drModifyRuleOpt[COLUMN.RULE_OPTION_VALUE];
                        //
                        sRuleOption += string.Format("{0}={1};", drModifyRuleOpt[COLUMN.OPTION_NAME], drModifyRuleOpt[COLUMN.RULE_OPTION_VALUE]);
                        sRuleOptionData += string.Format("{0}.{1}={2};", drModifyRuleOpt[COLUMN.RULE_OPTION_NO], drModifyRuleOpt[COLUMN.OPTION_NAME], drModifyRuleOpt[COLUMN.RULE_OPTION_VALUE]);
                    }

                    //#02. RULE
                    DataRow[] drSelectRule = _dtSPCRule.Select(string.Format("{0} = '{1}'", COLUMN.SPC_RULE_NO, _sRuleNO));

                    drSelectRule[0][COLUMN.OCAP_LIST] = sOCAPList;
                    drSelectRule[0][COLUMN.RULE_OPTION] = sRuleOption;
                    drSelectRule[0][COLUMN.RULE_OPTION_DATA] = sRuleOptionData;

                    this.DialogResult = DialogResult.OK;

                    break;
            }

            //this.DialogResult = DialogResult.OK;
        }

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

    }
}
