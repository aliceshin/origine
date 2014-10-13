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


        BISTel.eSPC.Common.ATT.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.ATT.CommonUtility _ComUtil;

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

        private BISTel.eSPC.Common.ConfigMode _cofingMode = BISTel.eSPC.Common.ConfigMode.CREATE_SUB;

        string _sConfigRawID;

        string _sRuleNO;

        private DataSet _dsSPCRuleMstData;
        private DataTable _dtSPCRule;
        private DataTable _dtSPCRuleOpt;
        public bool _sOCAPOfSingle = false;
        private bool _bUseComma;

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


        #endregion

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
                case BISTel.eSPC.Common.ConfigMode.CREATE_SUB:
                    this.Initialize_Create();
                    break;

                case BISTel.eSPC.Common.ConfigMode.MODIFY:
                    this.Initialize_Modify();
                    break;
            }
        }

        private void Initialize_Create()
        {
            _dsSPCRuleMstData = this._wsSPC.GetSPCATTRuleMasterData(null);

            if (_dsSPCRuleMstData == null || _dsSPCRuleMstData.Tables.Count == 0 || _dsSPCRuleMstData.Tables[0].Rows.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Error, "SPC_INFO_NOT_EXIST_RULE_MASTER", null, null);
                return;
            }

            _ComUtil.SetBComboBoxData(this.bcboRule, _dsSPCRuleMstData, BISTel.eSPC.Common.COLUMN.SPC_RULE, BISTel.eSPC.Common.COLUMN.RAWID, "", true);

            this.bbtnOK.Key = Definition.BUTTON_KEY_ADD;
            this.bbtnCancel.Key = Definition.BUTTON_KEY_CLOSE;
        }

        private void Initialize_Modify()
        {
            //#01. RULE
            DataRow[] drSelectRule = _dtSPCRule.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.SPC_RULE_NO, _sRuleNO));

            this.bcboRule.DropDownStyle = ComboBoxStyle.Simple;
            this.bcboRule.Text = string.Format("{0}. {1}", drSelectRule[0][BISTel.eSPC.Common.COLUMN.SPC_RULE_NO], drSelectRule[0][BISTel.eSPC.Common.COLUMN.DESCRIPTION]);
            this.bcboRule.Enabled = false;

            //#02. RULE OPTION
            DataTable dtSelectRuleOPT = _dtSPCRuleOpt.Clone();

            DataRow[] drSelectRuleOPTs = _dtSPCRuleOpt.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.SPC_RULE_NO, _sRuleNO), BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO);

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
                        this.bsprRuleOpt.ActiveSheet.Cells[i, this._ColIdx_VALUE].Locked = false;
                }
            }

            this.bsprRuleOpt.Focus();


            //#03. OCAP LIST
            string sOCAPList = drSelectRule[0][BISTel.eSPC.Common.COLUMN.OCAP_LIST].ToString();

            List<string> lstOCAPList = new List<string>(sOCAPList.Split(';'));

            string sOCAPCode = string.Empty;

            for (int i = 0; i < bsprOCAP.ActiveSheet.RowCount; i++)
            {
                sOCAPCode = bsprOCAP.GetCellText(i, _ColIdx_CODE);

                if (lstOCAPList.Contains(sOCAPCode))
                {
                    bsprOCAP.SetCellValue(i, _ColIdx_SELECT, true);
                    bsprOCAP.ActiveSheet.Rows[i].BackColor = Color.Yellow;
                }
            }

            this.bbtnOK.Key = Definition.BUTTON_KEY_OK;
            this.bbtnCancel.Key = Definition.BUTTON_KEY_CANCEL;
        }


        private void bcboRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CONFIG OPTION MST
            DataTable dtTmpRuleOpt = _dsSPCRuleMstData.Tables[BISTel.eSPC.Common.TABLE.RULE_OPT_ATT_MST_SPC];

            DataTable dtRuleOpt = dtTmpRuleOpt.Clone();
            DataRow[] drRuleOpts = dtTmpRuleOpt.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.RULE_RAWID, bcboRule.SelectedValue), BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO);

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

        private void bsprOCAP_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != (int)_ColIdx_SELECT)
                return;

            if (this._sOCAPOfSingle)
            {
                if (e.Column == (int)_ColIdx_SELECT)
                {

                    if ((bool)this.bsprOCAP.GetCellValue(e.Row, (int)_ColIdx_SELECT) == true)
                    {
                        this.bsprOCAP.ActiveSheet.Rows[e.Row].BackColor = Color.Yellow;
                        for (int i = 0; i < this.bsprOCAP.ActiveSheet.RowCount; i++)
                        {
                            if (e.Row == i)
                            {
                                if (e.Column != (int)_ColIdx_SELECT)
                                {
                                    this.bsprOCAP.ActiveSheet.Cells[i, (int)_ColIdx_SELECT].Value = 1;
                                }
                                continue;
                            }

                            this.bsprOCAP.ActiveSheet.Cells[i, (int)_ColIdx_SELECT].Value = 0;
                            this.bsprOCAP.ActiveSheet.Rows[i].BackColor = Color.White;
                        }
                    }
                    else
                        this.bsprOCAP.ActiveSheet.Rows[e.Row].BackColor = Color.White;

                }
            }
            else
            {
                ArrayList alSelectedRows = this.bsprOCAP.GetCheckedList((int)_ColIdx_SELECT);

                for (int idxTmpChartList = 0; idxTmpChartList < this.bsprOCAP.ActiveSheet.RowCount; idxTmpChartList++)
                {
                    if (alSelectedRows.Contains(idxTmpChartList))
                        this.bsprOCAP.ActiveSheet.Rows[idxTmpChartList].BackColor = Color.Yellow;
                    else
                        this.bsprOCAP.ActiveSheet.Rows[idxTmpChartList].BackColor = Color.White;
                }
            }
        }

        public void InitializeLayout()
        {
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_ATT_MODELING, Definition.CONFIG_USE_COMMA, false);
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

        public BISTel.eSPC.Common.ConfigMode CONFIG_MODE
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

        public bool USE_COMMA
        {
            get { return _bUseComma; }
            set { _bUseComma = value; }
        }

        public DataTable SPCRULEOPT_DATATABLE
        {
            get { return _dtSPCRuleOpt; }
            set
            {
                _dtSPCRuleOpt = value;

                if (_cofingMode.Equals(BISTel.eSPC.Common.ConfigMode.CREATE_SUB) && _dtSPCRuleOpt == null)
                {
                    _dtSPCRuleOpt = new DataTable(BISTel.eSPC.Common.TABLE.MODEL_RULE_OPT_ATT_MST_SPC);

                    _dtSPCRuleOpt.Columns.Add(BISTel.eSPC.Common.COLUMN.MODEL_RULE_RAWID);
                    _dtSPCRuleOpt.Columns.Add(BISTel.eSPC.Common.COLUMN.SPC_RULE_NO);
                    _dtSPCRuleOpt.Columns.Add(BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO);
                    _dtSPCRuleOpt.Columns.Add(BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE);
                    _dtSPCRuleOpt.Columns.Add(BISTel.eSPC.Common.COLUMN.OPTION_NAME);
                    _dtSPCRuleOpt.Columns.Add(BISTel.eSPC.Common.COLUMN.DESCRIPTION);
                }
            }
        }

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            //선택된 OCAP LIST만들기
            DataSet dsSelectOCAP = _SpreadUtil.GetSelectedDataSet(bsprOCAP, _ColIdx_SELECT);
            string sOCAPList = _ComUtil.ConvertDataColumnIntoStringList(dsSelectOCAP.Tables[0], BISTel.eSPC.Common.COLUMN.CODE, ";");

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
                case BISTel.eSPC.Common.ConfigMode.CREATE_SUB:

                    string sRuleRawid = bcboRule.SelectedValue.ToString();

                    DataRow[] drSelectRuleMst = _dsSPCRuleMstData.Tables[BISTel.eSPC.Common.TABLE.RULE_ATT_MST_SPC].Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.RAWID, sRuleRawid));

                    //기존 RULE이 있는지 확인
                    string sRuleNo = drSelectRuleMst[0][BISTel.eSPC.Common.COLUMN.SPC_RULE_NO].ToString();
                    string sRuleDesc = drSelectRuleMst[0][BISTel.eSPC.Common.COLUMN.DESCRIPTION].ToString();

                    DataRow[] drExistRuleMst = _dtSPCRule.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.SPC_RULE_NO, sRuleNo));

                    if (drExistRuleMst != null && drExistRuleMst.Length > 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_DUPLICATE_RULE", null, null);
                        return;
                    }

                    //없으면 RULE 추가
                    DataRow drNewRuleMst = _dtSPCRule.NewRow();

                    string sNewModelRuleRawid = "-" + sRuleNo;

                    drNewRuleMst[BISTel.eSPC.Common.COLUMN.RAWID] = sNewModelRuleRawid;
                    drNewRuleMst[BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID] = _ComUtil.NVL(_sConfigRawID, "-1", true);
                    drNewRuleMst[BISTel.eSPC.Common.COLUMN.SPC_RULE_NO] = sRuleNo;
                    drNewRuleMst[BISTel.eSPC.Common.COLUMN.OCAP_LIST] = sOCAPList;
                    drNewRuleMst[BISTel.eSPC.Common.COLUMN.USE_MAIN_SPEC_YN] = "Y";
                    drNewRuleMst[BISTel.eSPC.Common.COLUMN.USE_MAIN_SPEC] = "True";
                    drNewRuleMst[BISTel.eSPC.Common.COLUMN.DESCRIPTION] = sRuleDesc;

                    DataTable dtSelectRuleOpt = ((DataSet)bsprRuleOpt.DataSet).Tables[0];


                    foreach (DataRow drSelectRuleOpt in dtSelectRuleOpt.Rows)
                    {
                        DataRow drNewRuleOpt = _dtSPCRuleOpt.NewRow();

                        //drNewRuleOpt["RAWID"] = "";
                        drNewRuleOpt[BISTel.eSPC.Common.COLUMN.MODEL_RULE_RAWID] = sNewModelRuleRawid;
                        drNewRuleOpt[BISTel.eSPC.Common.COLUMN.SPC_RULE_NO] = drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.SPC_RULE_NO];
                        drNewRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO] = drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO];

                        if (drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE] != null && drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE].ToString().Length > 0)
                        {
                            char[] charArray = drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE].ToString().ToCharArray();
                            for (int i = 0; i < charArray.Length; i++)
                            {
                                if (_bUseComma)
                                {
                                    if (!(char.IsNumber(charArray[i]) || charArray[i].ToString() == "," || charArray[i].ToString() == "."))
                                    {
                                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VAILD_INPUT_FIELD_COMMA", null, null);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (!(char.IsNumber(charArray[i]) || charArray[i].ToString() == ";" || charArray[i].ToString() == "."))
                                    {
                                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VAILD_INPUT_FIELD", null, null);
                                        return;
                                    }
                                }
                            }
                        }

                        drNewRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE] = drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE];
                        drNewRuleOpt[BISTel.eSPC.Common.COLUMN.OPTION_NAME] = drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.OPTION_NAME];
                        drNewRuleOpt[BISTel.eSPC.Common.COLUMN.DESCRIPTION] = drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.DESCRIPTION];

                        _dtSPCRuleOpt.Rows.Add(drNewRuleOpt);

                        sRuleOption += string.Format("{0}={1};", drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.OPTION_NAME], drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE]);
                        sRuleOptionData += string.Format("{0}.{1}={2};", drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO], drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.OPTION_NAME], drSelectRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE]);
                    }

                    drNewRuleMst[BISTel.eSPC.Common.COLUMN.RULE_OPTION] = sRuleOption;
                    drNewRuleMst[BISTel.eSPC.Common.COLUMN.RULE_OPTION_DATA] = sRuleOptionData;

                    _dtSPCRule.Rows.Add(drNewRuleMst);

                    break;

                case BISTel.eSPC.Common.ConfigMode.MODIFY :

                    //#01. RULE OPTION
                    this.bsprRuleOpt.LeaveCellAction();
                    DataTable dtModifyRuleOpt = ((DataSet)this.bsprRuleOpt.DataSet).Tables[0];

                    foreach (DataRow drModifyRuleOpt in dtModifyRuleOpt.Rows)
                    {
                        DataRow[] drModifyRuleOPTs = _dtSPCRuleOpt.Select(string.Format("{0} = '{1}' AND {2} = '{3}'",
                                                                                                        BISTel.eSPC.Common.COLUMN.MODEL_RULE_RAWID,
                                                                                                        drModifyRuleOpt[BISTel.eSPC.Common.COLUMN.MODEL_RULE_RAWID],
                                                                                                        BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO,
                                                                                                        drModifyRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO])
                                                                                                        );
                        if (drModifyRuleOPTs != null && drModifyRuleOPTs.Length > 0)
                            drModifyRuleOPTs[0][BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE] = drModifyRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE];

                        if (drModifyRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE] != null && drModifyRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE].ToString().Length > 0)
                        {
                            char[] charArray = drModifyRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE].ToString().ToCharArray();
                            for (int i = 0; i < charArray.Length; i++)
                            {
                                if (_bUseComma)
                                {
                                    if (!(char.IsNumber(charArray[i]) || charArray[i].ToString() == "," || charArray[i].ToString() == "."))
                                    {
                                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VAILD_INPUT_FIELD_COMMA", null, null);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (!(char.IsNumber(charArray[i]) || charArray[i].ToString() == ";" || charArray[i].ToString() == "."))
                                    {
                                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VAILD_INPUT_FIELD", null, null);
                                        return;
                                    }
                                }
                            }
                        }

                        //
                        sRuleOption += string.Format("{0}={1};", drModifyRuleOpt[BISTel.eSPC.Common.COLUMN.OPTION_NAME], drModifyRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE]);
                        sRuleOptionData += string.Format("{0}.{1}={2};", drModifyRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_NO], drModifyRuleOpt[BISTel.eSPC.Common.COLUMN.OPTION_NAME], drModifyRuleOpt[BISTel.eSPC.Common.COLUMN.RULE_OPTION_VALUE]);
                    }

                    //#02. RULE
                    DataRow[] drSelectRule = _dtSPCRule.Select(string.Format("{0} = '{1}'", BISTel.eSPC.Common.COLUMN.SPC_RULE_NO, _sRuleNO));

                    drSelectRule[0][BISTel.eSPC.Common.COLUMN.OCAP_LIST] = sOCAPList;
                    drSelectRule[0][BISTel.eSPC.Common.COLUMN.RULE_OPTION] = sRuleOption;
                    drSelectRule[0][BISTel.eSPC.Common.COLUMN.RULE_OPTION_DATA] = sRuleOptionData;

                    this.DialogResult = DialogResult.OK;

                    break;
            }
        }

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
