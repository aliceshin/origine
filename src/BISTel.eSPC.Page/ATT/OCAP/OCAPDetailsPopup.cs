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

using System.Web.Services;


using BISTel.eSPC.Common.ATT;
using BISTel.eSPC.Page.ATT.Common;

namespace BISTel.eSPC.Page.ATT.OCAP
{
    public partial class OCAPDetailsPopup : BasePopupFrm
    {
        #region : Field
        
        CommonUtility _comUtil;        
        MultiLanguageHandler _mlthandler;
        Initialization _Initialization;	
        BSpreadUtility _bSpreadUtil;
        eSPCWebService.eSPCWebService _wsSPC = null;             
        DataSet _dsContext = new DataSet();
        DataSet _dsRule = new DataSet();
        public DataTable dtComment = null;

        LinkedList _llstSearchCondition;
        LinkedList _llstCondition;
        BISTel.eSPC.Common.enum_PopupType _popupType;        
        SessionData _SessionData;                
        ChartInterface _ChartVariable = new ChartInterface();               
        
        int iCurrenRaw=0;
        string mParamTypeCd = string.Empty;

        DataSet _dsSelectedOCAP = null;
        LinkedList _mllstContextType = null;
        bool _isMulti = false;

        public delegate void LinkTraceDataViewEventHandlerOcap(object sender, EventArgs e, LinkedList llstTraceLinkData);
        public event LinkTraceDataViewEventHandlerOcap linkTraceDataViewEventOcap;

        private bool bShowViewChartButton = true;

        //SPC-1292, KBLEE, START
        private string _lineRawid;
        private string _areaRawid;
        private string _sGroupName;
        //SPC-1292, KBLEE, END

        public bool BSHOWVIEWCHARTBUTTON
        {
            get { return this.bShowViewChartButton; }
            set { this.bShowViewChartButton = value; }
        }

        #endregion

        #region : Constructor

        public OCAPDetailsPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region : Initialization

        public void InitializePopup()
        {
            if (!bShowViewChartButton)
            {
                this.bbtnChartView.Visible = false;
            }
            this._comUtil = new CommonUtility();
            this._bSpreadUtil = new BSpreadUtility();
            this._mlthandler = MultiLanguageHandler.getInstance();

            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
            this._llstSearchCondition = new LinkedList();
                
            this.InitializeLayout();
            this.InitializeCode();
            if (this._isMulti)
            {
                this.InitializeBSpread();
                this.InitializeBSpreadSelectedOCAP();
            }
            else
            {
                try
                {
                    this.MsgShow(BISTel.PeakPerformance.Client.BaseUserCtrl.BasePageUCtrl.COMMON_MSG.Query_Data);
                    this.InitializeBSpreadContext();
                    this.InitializeBSpreadRule();
                    this.initializeBCheckComboBinding(ChartVariable.MODEL_CONFIG_RAWID.ToString());

                    this.MsgClose();
                }
                catch
                {
                    this.MsgClose();
                }
            }
        }

      
        public void InitializeLayout()
        {
            try
            {
                if (this._dsSelectedOCAP == null || this._dsSelectedOCAP.Tables[0].Rows.Count == 1)
                {
                    this.pnlOCAPList.Visible = false;
                }

                if (PopUpType == BISTel.eSPC.Common.enum_PopupType.Modify)
                {
                    if (this._isMulti && this._dsSelectedOCAP.Tables[0].Rows.Count > 1)
                    {
                        this.bbtnSave.Visible = false;
                        this.bbtnSaveAll.Visible = true;
                        this.bapAdd.Visible = true;
                    }
                    else
                    {
                        this.bbtnSave.Visible = true;
                        this.bbtnSaveAll.Visible = false;
                        this.bapAdd.Visible = true;
                    }

                    this.bbtnCauseInput.Visible = true;
                    this.bbtnProblemInput.Visible = true;
                    this.bbtnSolutionInput.Visible = true;
                    this.bbtnCommentInput.Visible = true;
                }
                else
                {
                    if (this.ChartVariable != null && this.ChartVariable.CHART_PARENT_MODE != null && this.ChartVariable.CHART_PARENT_MODE == BISTel.eSPC.Common.CHART_PARENT_MODE.OCAP)
                    {
                        this.bbtnChartView.Visible = false;
                    }
                    this.bbtnSave.Visible = false;
                    this.bbtnSaveAll.Visible = false;
                    this.bapAdd.Visible = false;
                }
                
                this.btxtCause.BackColor = Color.White;
                this.btxtProblem.BackColor = Color.White;
                this.btxtsolution.BackColor = Color.White;
                this.btxtcomment.BackColor = Color.White;
                this.chkFalseAlarm.BackColor = Color.White;

                this.ContentsAreaMinWidth =780;
                this.ContentsAreaMinHeight=580;

                this.Title = _mlthandler.GetVariable(Definition.POPUP_TITLE_KEY_OCAP_DETAILS);
                              
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
        }

        public void InitializeCode()
        {
            this._llstSearchCondition.Clear();
            DataSet dsContextType = _wsSPC.GetContextType(_llstSearchCondition.GetSerialData());
            this._mllstContextType = CommonPageUtil.SetContextType(dsContextType);
        }

        public void InitializeBSpread()
        {
            this.bsprContext.ClearHead();
            this.bsprContext.AddHeadComplete();
            this.bsprContext.AutoGenerateColumns = true;
            this.bsprContext.Locked = true;

            this._Initialization.InitializeColumnHeader(ref bsprRule, Definition.PAGE_KEY_SPC_OCAP_DETAILS_POPUP, false, Definition.PAGE_KEY_SPC_OCAP_DETAILS_UC_HEADER_KEY);
            this.bsprRule.UseHeadColor = false;
            this.bsprRule.UseAutoSort = false;
            this.bsprRule.UseFilter = false;
            this.bsprRule.UseEdit = false;
            this.bsprRule.DataSource = null;
        }

        public void InitializeBSpreadContext()
        {
            SPCStruct.ContextTypeInfo mContextTypeInfo = null;        
           this._llstSearchCondition.Clear();
           this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.OCAP_RAWID, ChartVariable.OCAPRawID);

           
           _dsRule = _wsSPC.GetATTOCAPDetails(this._llstSearchCondition.GetSerialData());

           if (DataUtil.IsNullOrEmptyDataSet(_dsRule)) return;

           //Initialize Context List

           string strContext_list = _dsRule.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.CONTEXT_LIST].ToString();
           string [] arrCol = strContext_list.Split('\t');
           DataTable dtContext = new DataTable();
           DataRow nRow = null;
           
           dtContext.Columns.Add(Definition.SpreadHeaderColKey.KEY, typeof(string));
           dtContext.Columns.Add(Definition.SpreadHeaderColKey.VALUE, typeof(string));
           nRow = dtContext.NewRow();
                    
           nRow[Definition.SpreadHeaderColKey.KEY] = Definition.CHART_COLUMN.TIME;
           nRow[Definition.SpreadHeaderColKey.VALUE] = _dsRule.Tables[0].Rows[0]["ooc_dtts"].ToString();
           dtContext.Rows.Add(nRow);  
           for(int i=0; i<arrCol.Length; i++)
           {
                string []arrValue = arrCol[i].ToString().Split('=');
                if (arrValue[0] == BISTel.eSPC.Common.COLUMN.CONTEXT_KEY) continue;
                 mContextTypeInfo = this._mllstContextType[arrValue[0]] as SPCStruct.ContextTypeInfo;
                if (mContextTypeInfo == null) continue ;         
                 nRow = dtContext.NewRow();
                 nRow[Definition.SpreadHeaderColKey.KEY] = this._mlthandler.GetVariable(Definition.SPC_LABEL_ + mContextTypeInfo.NAME);
                 nRow[Definition.SpreadHeaderColKey.VALUE] = arrValue[1];

                 dtContext.Rows.Add(nRow);             
           }
           _dsContext.Tables.Add(dtContext);

           this.bsprContext.ClearHead();
           this.bsprContext.AddHeadComplete();
           this.bsprContext.AutoGenerateColumns = true;

           this.bsprContext.DataSet = _dsContext;
           this.bsprContext.ActiveSheet.Columns[0].BackColor = Color.WhiteSmoke;
           this.bsprContext.ActiveSheet.Columns[1].BackColor = Color.White;
           this.bsprContext.ActiveSheet.Columns[0].Width = 100;
           this.bsprContext.ActiveSheet.Columns[1].Width = 150;

           this.bsprContext.ActiveSheet.Columns[0].DataField = Definition.SpreadHeaderColKey.KEY;
           this.bsprContext.ActiveSheet.Columns[1].DataField = Definition.SpreadHeaderColKey.VALUE;
           this.bsprContext.ActiveSheet.ColumnHeader.Columns[0].Label = this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.KEY);
           this.bsprContext.ActiveSheet.ColumnHeader.Columns[1].Label = this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.VALUE);
           this.bsprContext.Locked = true;
        }

        public void InitializeBSpreadContext(string sOCAPRawID)
        {
            SPCStruct.ContextTypeInfo mContextTypeInfo = null;
            this._llstSearchCondition.Clear();
            this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.OCAP_RAWID, sOCAPRawID);
            _dsRule = _wsSPC.GetATTOCAPDetails(this._llstSearchCondition.GetSerialData());

            if (DataUtil.IsNullOrEmptyDataSet(_dsRule)) return;

            //Initialize Context List

            string strContext_list = _dsRule.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.CONTEXT_LIST].ToString();
            string[] arrCol = strContext_list.Split('\t');
            DataTable dtContext = new DataTable();
            DataRow nRow = null;

            dtContext.Columns.Add(Definition.SpreadHeaderColKey.KEY, typeof(string));
            dtContext.Columns.Add(Definition.SpreadHeaderColKey.VALUE, typeof(string));
            nRow = dtContext.NewRow();

            nRow[Definition.SpreadHeaderColKey.KEY] = Definition.CHART_COLUMN.TIME;
            nRow[Definition.SpreadHeaderColKey.VALUE] = _dsRule.Tables[0].Rows[0]["ooc_dtts"].ToString();
            dtContext.Rows.Add(nRow);

            for (int i = 0; i < _dsRule.Tables[0].Columns.Count; i++)
            {
                if (_dsRule.Tables[0].Columns[i].ColumnName == "MAIN_MODULE_ID" ||
                    _dsRule.Tables[0].Columns[i].ColumnName == "MODULE_ID" ||
                    _dsRule.Tables[0].Columns[i].ColumnName == "EQP_ID" ||
                    _dsRule.Tables[0].Columns[i].ColumnName == "LOT_ID" ||
                    _dsRule.Tables[0].Columns[i].ColumnName == "SUBSTRATE_ID" ||
                    _dsRule.Tables[0].Columns[i].ColumnName == "CASSETTE_SLOT" ||
                    _dsRule.Tables[0].Columns[i].ColumnName == "OPERATION_ID" ||
                    _dsRule.Tables[0].Columns[i].ColumnName == "PRODUCT_ID" ||
                    _dsRule.Tables[0].Columns[i].ColumnName == "RECIPE_ID" ||
                    _dsRule.Tables[0].Columns[i].ColumnName == "STEP_ID")
                {
                    if (_dsRule.Tables[0].Rows[0][i].ToString().Length > 0)
                    {
                        nRow = dtContext.NewRow();
                        nRow[Definition.SpreadHeaderColKey.KEY] = this._mlthandler.GetVariable(Definition.SPC_LABEL_ + _dsRule.Tables[0].Columns[i].ColumnName.ToUpper());
                        nRow[Definition.SpreadHeaderColKey.VALUE] = _dsRule.Tables[0].Rows[0][i].ToString();
                        dtContext.Rows.Add(nRow);
                    }
                }
            }

            for (int i = 0; i < arrCol.Length; i++)
            {
                bool bContextExist = false;
                string[] arrValue = arrCol[i].ToString().Split('=');
                if (arrValue[0] == BISTel.eSPC.Common.COLUMN.CONTEXT_KEY) continue;
                mContextTypeInfo = this._mllstContextType[arrValue[0]] as SPCStruct.ContextTypeInfo;
                if (mContextTypeInfo == null) continue;

                for (int j = 0; j < dtContext.Rows.Count; j++)
                {
                    if (dtContext.Rows[j][0].ToString() == this._mlthandler.GetVariable(Definition.SPC_LABEL_ + mContextTypeInfo.NAME))
                    {
                        bContextExist = true;
                        break;
                    }
                }

                if (bContextExist)
                    continue;

                nRow = dtContext.NewRow();
                nRow[Definition.SpreadHeaderColKey.KEY] = this._mlthandler.GetVariable(Definition.SPC_LABEL_ + mContextTypeInfo.NAME);
                nRow[Definition.SpreadHeaderColKey.VALUE] = arrValue[1];

                dtContext.Rows.Add(nRow);
            }
            _dsContext.Tables.Add(dtContext);

            this.bsprContext.ClearHead();
            this.bsprContext.AddHeadComplete();
            this.bsprContext.AutoGenerateColumns = true;

            this.bsprContext.DataSet = _dsContext;
            this.bsprContext.ActiveSheet.Columns[0].BackColor = Color.WhiteSmoke;
            this.bsprContext.ActiveSheet.Columns[1].BackColor = Color.White;
            this.bsprContext.ActiveSheet.Columns[0].Width = 100;
            this.bsprContext.ActiveSheet.Columns[1].Width = 150;

            this.bsprContext.ActiveSheet.Columns[0].DataField = Definition.SpreadHeaderColKey.KEY;
            this.bsprContext.ActiveSheet.Columns[1].DataField = Definition.SpreadHeaderColKey.VALUE;
            this.bsprContext.ActiveSheet.ColumnHeader.Columns[0].Label = this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.KEY);
            this.bsprContext.ActiveSheet.ColumnHeader.Columns[1].Label = this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.VALUE);
            this.bsprContext.Locked = true;
        }

        public void InitializeBSpreadRule()
        {
            this._Initialization.InitializeColumnHeader(ref bsprRule, Definition.PAGE_KEY_SPC_OCAP_DETAILS_POPUP, false, Definition.PAGE_KEY_SPC_OCAP_DETAILS_UC_HEADER_KEY);
            this.bsprRule.UseHeadColor = false;
            this.bsprRule.UseAutoSort = false;
            this.bsprRule.UseFilter=false;
            this.bsprRule.UseEdit=false;
            this.bsprRule.DataSource = null;
            this.bsprRule.DataSource = _dsRule.Tables[0];
            this.bsprRule.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(bsprRule_CellClick);

            if(this.bsprRule.ActiveSheet.RowCount>0)
            {
                this.bsprRule.ActiveSheet.Columns[1].Locked = true;
                iCurrenRaw=0;
                InitRuleInfo();                
            }

            if (PopUpType == BISTel.eSPC.Common.enum_PopupType.View)
            {
                this.bsprRule.ActiveSheet.Columns[1].Visible = false;
            }
                    
        }

        public void InitializeBSpreadSelectedOCAP()
        {
            try
            {
                this._Initialization.InitializeColumnHeader(ref bsprOCAPList, Definition.PAGE_KEY_SPC_OCAP_DETAILS_POPUP, false, Definition.PAGE_KEY_SPC_OCAP_DETAILS_UC_HEADER_KEY_SELECTED_OCAP);
                this.bsprOCAPList.UseHeadColor = false;
                this.bsprOCAPList.UseAutoSort = false;
                this.bsprOCAPList.UseFilter = false;
                this.bsprOCAPList.UseEdit = false;
                this.bsprOCAPList.DataSource = null;
                this.bsprOCAPList.DataSource = this._dsSelectedOCAP;
                this.bsprOCAPList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                this.bsprOCAPList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                this.bsprOCAPList.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(bsprOCAPList_ButtonClicked);
                if (this.bsprOCAPList.ActiveSheet.RowCount > 0)
                {
                    for (int i = 0; i < this.bsprOCAPList.ActiveSheet.Columns.Count; i++)
                    {
                        this.bsprOCAPList.ActiveSheet.Columns[i].Width = this.bsprOCAPList.ActiveSheet.GetPreferredColumnWidth(i);
                    }

                    this.bsprOCAPList.ActiveSheet.Columns[0].Locked = false;
                }

                this.bsprOCAPList.ActiveSheet.Cells[0, 0].Value = true;
                this.bsprOCAPList_ButtonClicked(this.bsprOCAPList, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        

        #region : Popup Logic

     
        #endregion

        #region :: Button

        #region ::: Function

        private void ClickSaveButton()
        {
            try
            {
                this.MsgShow(BISTel.PeakPerformance.Client.BaseUserCtrl.BasePageUCtrl.COMMON_MSG.Save_Data);
                if (this.bsprRule.ActiveSheet.RowCount > 0)
                {
                    for (int i = 0; i < this.bsprRule.ActiveSheet.RowCount; i++)
                    {
                        this.bsprRule.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.OOC_CAUSE].Text = this.btxtCause.Text;
                        this.bsprRule.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.OOC_PROBLEM].Text = this.btxtProblem.Text;
                        this.bsprRule.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.OOC_SOLUTION].Text = this.btxtsolution.Text;
                        this.bsprRule.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.OOC_COMMENT].Text = this.btxtcomment.Text;
                        string falseAlarm = string.Empty;
                        if (this.chkFalseAlarm.Checked == true)
                            falseAlarm = "Y";
                        else
                            falseAlarm = "N";

                        this.bsprRule.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.FALSE_ALARM_YN].Text = falseAlarm;

                        if (!string.IsNullOrEmpty(this.btxtCauseInput.Text))
                            this.bsprRule.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.OOC_CAUSE].Text += " " + this.btxtCauseInput.Text;
                        if (!string.IsNullOrEmpty(this.btxtProblemInput.Text))
                            this.bsprRule.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.OOC_PROBLEM].Text += " " + this.btxtProblemInput.Text;
                        if (!string.IsNullOrEmpty(this.btxtSolutionInput.Text))
                            this.bsprRule.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.OOC_SOLUTION].Text += " " + this.btxtSolutionInput.Text;
                        if (!string.IsNullOrEmpty(this.btxtCommentInput.Text))
                            this.bsprRule.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.OOC_COMMENT].Text += " " + this.btxtCommentInput.Text;

                        this.bsprRule.ActiveSheet.Cells[i, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.V_MODIFY].Value = "True";

                        this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_CAUSE] = this.btxtCause.Text;
                        this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_PROBLEM] = this.btxtProblem.Text;
                        this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_SOLUTION] = this.btxtsolution.Text;
                        this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_COMMENT] = this.btxtcomment.Text;
                        this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_FALSE_ALARM_YN] = falseAlarm;

                        string sDataTime = DateTime.Now.ToString(this._mlthandler.GetVariable(Definition.SPC_DATETIME_FORMAT_OCAP));

                        if (!string.IsNullOrEmpty(this.btxtCauseInput.Text))
                        {
                            if (this.btxtCause.Text.Length > 0)
                                this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_CAUSE] += "\r\n" + sDataTime + "     " + _SessionData.UserId + "     " + this.btxtCauseInput.Text;
                            else
                                this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_CAUSE] += sDataTime + "     " + _SessionData.UserId + "     " + this.btxtCauseInput.Text;
                        }
                        if (!string.IsNullOrEmpty(this.btxtProblemInput.Text))
                        {
                            if (this.btxtProblem.Text.Length > 0)
                                this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_PROBLEM] += "\r\n" + sDataTime + "     " + _SessionData.UserId + "     " + this.btxtProblemInput.Text;
                            else
                                this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_PROBLEM] += sDataTime + "     " + _SessionData.UserId + "     " + this.btxtProblemInput.Text;
                        }
                        if (!string.IsNullOrEmpty(this.btxtSolutionInput.Text))
                        {
                            if (this.btxtsolution.Text.Length > 0)
                                this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_SOLUTION] += "\r\n" + sDataTime + "     " + _SessionData.UserId + "     " + this.btxtSolutionInput.Text;
                            else
                                this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_SOLUTION] += sDataTime + "     " + _SessionData.UserId + "     " + this.btxtSolutionInput.Text;
                        }
                        if (!string.IsNullOrEmpty(this.btxtCommentInput.Text))
                        {
                            if (this.btxtcomment.Text.Length > 0)
                                this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_COMMENT] += "\r\n" + sDataTime + "     " + _SessionData.UserId + "     " + this.btxtCommentInput.Text;
                            else
                                this._dsRule.Tables[0].Rows[i][Definition.CONDITION_KEY_OOC_COMMENT] += sDataTime + "     " + _SessionData.UserId + "     " + this.btxtCommentInput.Text;
                        }

                        if (dtComment != null)
                        {
                            UpdateCommentTable();
                        }
                    }
                }

                DataSet dsSaveRule = new DataSet();
                LinkedList llstSave = new LinkedList();

                DataTable dtModifyRule = this._dsRule.Tables[0].GetChanges(DataRowState.Modified);
                if (dtModifyRule != null && dtModifyRule.Rows.Count > 0)
                {
                    dtModifyRule.TableName = "MODIFY";
                    dsSaveRule.Tables.Add(dtModifyRule);
                    llstSave.Add(Definition.DynamicCondition_Condition_key.OCAP_DATASET, dsSaveRule);
                    llstSave.Add(Definition.DynamicCondition_Condition_key.USER_ID, SessionData.UserId.ToString());

                    if (this._isMulti)
                    {
                        ArrayList arrOCAPList = new ArrayList();
                        for (int i = 0; i < this.bsprOCAPList.ActiveSheet.RowCount; i++)
                        {
                            if (this.bsprOCAPList.ActiveSheet.Cells[i, 7].Text != null && this.bsprOCAPList.ActiveSheet.Cells[i, 7].Text.Trim().Length > 0)
                            {
                                if (this.bsprOCAPList.ActiveSheet.Cells[i, 7].Text.Trim().Split(',').Length > 0)
                                {
                                    string[] strArrTempOcapRawID = this.bsprOCAPList.ActiveSheet.Cells[i, 7].Text.Trim().Split(',');
                                    for (int j = 0; j < strArrTempOcapRawID.Length; j++)
                                    {
                                        if (strArrTempOcapRawID[j] != null && strArrTempOcapRawID[j].Trim().Length > 0)
                                        {
                                            arrOCAPList.Add(strArrTempOcapRawID[j]);
                                        }
                                    }
                                }
                                else
                                {
                                    arrOCAPList.Add(this.bsprOCAPList.ActiveSheet.Cells[i, 2].Text);
                                }
                            }
                            else
                            {
                                arrOCAPList.Add(this.bsprOCAPList.ActiveSheet.Cells[i, 2].Text);
                            }
                        }
                        llstSave.Add(Definition.DynamicCondition_Condition_key.USE_YN, Definition.VARIABLE_Y);
                        llstSave.Add(Definition.DynamicCondition_Condition_key.OCAP_RAWID, arrOCAPList);
                    }
                    else
                    {
                        llstSave.Add(Definition.DynamicCondition_Condition_key.USE_YN, Definition.VARIABLE_N);
                    }

                    byte[] baData = llstSave.GetSerialData();
                    bool bSuccess = this._wsSPC.SaveATTOCAP(baData);

                    if (bSuccess)
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.GENERAL_SAVE_SUCCESS));

                        InitializePopup();
                    }
                    else
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.GENERAL_SAVE_FAIL));
                    }

                }
                else
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_NO_CHANGE_DATA));
                    return;
                }
            }
            catch
            {
                this.MsgClose();
            }                                              
           
        }
        

        private void UpdateCommentTable()
        {
            foreach(DataRow dr in _dsRule.Tables[0].Rows)
            {
                DataRow comment = FindCommentRow(dr[Definition.CONDITION_KEY_OOC_OCAP_RAWID].ToString());

                if(comment == null)
                    return;

                comment[Definition.CONDITION_KEY_OOC_CAUSE] = dr[Definition.CONDITION_KEY_OOC_CAUSE].ToString();
                comment[Definition.CONDITION_KEY_OOC_PROBLEM] = dr[Definition.CONDITION_KEY_OOC_PROBLEM].ToString();
                comment[Definition.CONDITION_KEY_OOC_SOLUTION] = dr[Definition.CONDITION_KEY_OOC_SOLUTION].ToString();
                comment[Definition.CONDITION_KEY_OOC_COMMENT] = dr[Definition.CONDITION_KEY_OOC_COMMENT].ToString();
                comment[Definition.CONDITION_KEY_FALSE_ALARM_YN] = dr[Definition.CONDITION_KEY_FALSE_ALARM_YN].ToString();

                comment.AcceptChanges();

                return;
            }
        }

        private DataRow FindCommentRow(string ocapRawid)
        {
            foreach(DataRow dr in dtComment.Rows)
            {
                if(dr[Definition.CONDITION_KEY_RAWID].ToString() == ocapRawid)
                {
                    return dr;
                }
            }

            return null;
        }

        private void ClearText()
        {
            this.btxtCause.Text = null;
            this.btxtProblem.Text = null;
            this.btxtsolution.Text = null;
            this.btxtcomment.Text = null;
            this.chkFalseAlarm.Checked = false;
        }
        
        private void InitRuleInfo()
        {
            this.bcboCause.Items.Clear();
            this.bcboComment.Items.Clear();
            this.bcboProblem.Items.Clear();
            this.bcboSolution.Items.Clear();

            this.bcboCause.Visible = false;
            this.bcboComment.Visible = false;
            this.bcboProblem.Visible = false;
            this.bcboSolution.Visible = false;

            this.btxtCauseInput.Text = string.Empty;
            this.btxtCommentInput.Text = string.Empty;
            this.btxtProblemInput.Text = string.Empty;
            this.btxtSolutionInput.Text = string.Empty;

            this.btxtCauseInput.Visible = false;
            this.btxtCommentInput.Visible = false;
            this.btxtProblemInput.Visible = false;
            this.btxtSolutionInput.Visible = false;

            this.btxtCause.Text = this._dsRule.Tables[0].Rows[iCurrenRaw][Definition.CHART_COLUMN.OCAP_CAUSE].ToString();
            this.btxtProblem.Text = this._dsRule.Tables[0].Rows[iCurrenRaw][Definition.CHART_COLUMN.OCAP_PROBLEM].ToString();
            this.btxtsolution.Text = this._dsRule.Tables[0].Rows[iCurrenRaw][Definition.CHART_COLUMN.OCAP_SOLUTION].ToString();
            this.btxtcomment.Text = this._dsRule.Tables[0].Rows[iCurrenRaw][Definition.CHART_COLUMN.OCAP_COMMENT].ToString();

            this.blblOCAPRawID.Text = this.bsprRule.ActiveSheet.Cells[iCurrenRaw, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.OCAP_RAWID].Text;
            this.blblOOCValue.Text = this.bsprRule.ActiveSheet.Cells[iCurrenRaw, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.OOC_VALUE].Text;
            this.blblRule.Text = string.Format("{0} ({1})",
                                 this.bsprRule.ActiveSheet.Cells[iCurrenRaw, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.RULE_NO].Text
                                 , this.bsprRule.ActiveSheet.Cells[iCurrenRaw, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.RULE_DESC].Text);

            if(this.bsprRule.ActiveSheet.Cells[iCurrenRaw, (int)BISTel.eSPC.Common.enum_OCAPList_Defails.FALSE_ALARM_YN].Text.ToUpper() == "Y")
            {
                this.chkFalseAlarm.Checked = true;
            }
            else
            {
                this.chkFalseAlarm.Checked = false;
            }

        }
        #endregion

        #region ::: Event


        private void bsprRule_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                e.Cancel = true;
                return;
            }

            //한 ROW만 선택되도록 함
            switch (this.bsprRule.columnInformation.GetColumnType(e.Column))
            {
                case ColumnType.Insert:
                case ColumnType.Modify:
                case ColumnType.Delete:
                    return;
            }
            
            iCurrenRaw = e.Row;

            InitRuleInfo();              
        }

        void bsprOCAPList_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            try
            {
                this.MsgShow(BISTel.PeakPerformance.Client.BaseUserCtrl.BasePageUCtrl.COMMON_MSG.Query_Data);
                if (e == null)
                {
                    if (this.bsprOCAPList.ActiveSheet.RowCount > 0)
                    {
                        if (this.bsprOCAPList.ActiveSheet.Cells[0, 7].Text != null && this.bsprOCAPList.ActiveSheet.Cells[0, 7].Text.Trim().Length > 0)
                        {
                            this.InitializeBSpreadContext(this.bsprOCAPList.ActiveSheet.Cells[0, 7].Text);
                            this.InitializeBSpreadRule();
                        }
                        else
                        {
                            this.InitializeBSpreadContext(this.bsprOCAPList.ActiveSheet.Cells[0, 2].Text);
                            this.InitializeBSpreadRule();
                            this.initializeBCheckComboBinding(this.bsprOCAPList.ActiveSheet.Cells[0, 1].Text);
                        }
                    }
                }
                else
                {
                    if (e.Column == 0)
                    {
                        if ((bool)this.bsprOCAPList.GetCellValue(e.Row, 0) == true)
                        {
                            for (int i = 0; i < bsprOCAPList.ActiveSheet.RowCount; i++)
                            {
                                if (i == e.Row)
                                {
                                    continue;
                                }
                                this.bsprOCAPList.ActiveSheet.Cells[i, 0].Value = 0;
                            }
                            if (this.bsprOCAPList.ActiveSheet.Cells[0, 7].Text != null && this.bsprOCAPList.ActiveSheet.Cells[0, 7].Text.Trim().Length > 0)
                            {
                                this.InitializeBSpreadContext(this.bsprOCAPList.ActiveSheet.Cells[e.Row, 7].Text);
                                this.InitializeBSpreadRule();
                            }
                            else
                            {
                                this.InitializeBSpreadContext(this.bsprOCAPList.ActiveSheet.Cells[e.Row, 2].Text);
                                this.InitializeBSpreadRule();
                                this.initializeBCheckComboBinding(this.bsprOCAPList.ActiveSheet.Cells[0, 1].Text);
                            }
                        }
                    }
                }
                this.MsgClose();
            }
            catch (Exception ex)
            {
                this.MsgClose();
                throw ex;
            }
        }

        private void initializeBCheckComboBinding(string sOCAPRawID)
        {
            DataSet dsCheckComboData = new DataSet();
            dsCheckComboData = _wsSPC.GetATTOCAPCommends(sOCAPRawID);

            DataTable dtOOCCause = new DataTable();
            DataTable dtOOCComment = new DataTable();
            DataTable dtOOCProblem = new DataTable();
            DataTable dtOOCSolution = new DataTable();

            this.bcboComment.Text = "";
            this.bcboProblem.Text = "";
            this.bcboCause.Text = "";
            this.bcboSolution.Text = "";

            dtOOCCause = dsCheckComboData.Tables[BISTel.eSPC.Common.TABLE.OOC_CAUSE_MST_SPC];
            dtOOCComment = dsCheckComboData.Tables[BISTel.eSPC.Common.TABLE.OOC_COMMENT_MST_SPC];
            dtOOCProblem = dsCheckComboData.Tables[BISTel.eSPC.Common.TABLE.OOC_PROBLEM_MST_SPC];
            dtOOCSolution = dsCheckComboData.Tables[BISTel.eSPC.Common.TABLE.OOC_SOLUTION_MST_SPC];

            if (dtOOCCause.Rows.Count > 0)
            {
                foreach(DataRow dr in dtOOCCause.Rows)
                {
                    this.bcboCause.Items.Add(dr["OOC_CAUSE"].ToString());
                }
            }
            if (dtOOCComment.Rows.Count > 0)
            {
                foreach (DataRow dr in dtOOCComment.Rows)
                {
                    this.bcboComment.Items.Add(dr["OOC_COMMENT"].ToString());
                }
            }
            if (dtOOCProblem.Rows.Count > 0)
            {
                foreach (DataRow dr in dtOOCProblem.Rows)
                {
                    this.bcboProblem.Items.Add(dr["OOC_PROBLEM"].ToString());
                }
            }
            if (dtOOCSolution.Rows.Count > 0)
            {
                foreach (DataRow dr in dtOOCSolution.Rows)
                {
                    this.bcboSolution.Items.Add(dr["OOC_SOLUTION"].ToString());
                }
            }

        }

        private void bbtnSave_Click(object sender, EventArgs e)
        {
            this.ClickSaveButton();
        }

        private void bbtnSaveAll_Click(object sender, EventArgs e)
        {
            this.ClickSaveButton();
        }


        private void bbtnChartView_Click(object sender, EventArgs e)
        {
            if (this._isMulti)
            {
                ArrayList alCheckRowIndex = _bSpreadUtil.GetCheckedRowIndex(this.bsprOCAPList, 0);
                if (alCheckRowIndex.Count < 1 || alCheckRowIndex.Count > 1)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("FDC_ALLOW_SINGLE_SELECTED_ROW"));
                    return;
                }

                this._ChartVariable = new ChartInterface();
                int iRowIndex = (int)alCheckRowIndex[0];
                if (iRowIndex < 0) return;

                string strModelConfigRawID = this.bsprOCAPList.ActiveSheet.Cells[iRowIndex, 1].Text;
                string strOCAPRawID = this.bsprOCAPList.ActiveSheet.Cells[iRowIndex, 2].Text;
                string strDefaultChartList = this.bsprOCAPList.ActiveSheet.Cells[iRowIndex, 6].Text;

                DataRow[] drArr = this._dsSelectedOCAP.Tables[0].Select(string.Format("MODEL_CONFIG_RAWID = '{0}' AND OCAP_RAWID = '{1}'", strModelConfigRawID, strOCAPRawID));

                if (drArr != null && drArr.Length == 1)
                {
                    DataRow dr = drArr[0];
                    string strTime = dr["OOC_DTTS"].ToString();
                    string sParamTypeCD = "ATT";
                    this.mParamTypeCd = sParamTypeCD;

                    _ChartVariable.AREA = dr[Definition.CHART_COLUMN.AREA].ToString();
                    _ChartVariable.SPC_MODEL = dr[Definition.CHART_COLUMN.SPC_MODEL_NAME].ToString();
                    _ChartVariable.PARAM_ALIAS = dr[Definition.CHART_COLUMN.PARAM_ALIAS].ToString();
                    _ChartVariable.OPERATION_ID = dr[Definition.CHART_COLUMN.OPERATION_ID].ToString();
                    _ChartVariable.PRODUCT_ID = dr[Definition.CHART_COLUMN.PRODUCT_ID].ToString();
                    _ChartVariable.complex_yn = Definition.VARIABLE_Y;
                    _ChartVariable.MODEL_CONFIG_RAWID = strModelConfigRawID;
                    _ChartVariable.MAIN_YN = dr[Definition.DynamicCondition_Condition_key.MAIN_YN].ToString();
                    _ChartVariable.OCAPRawID = strOCAPRawID;

                    LinkedList llstSearch = new LinkedList();
                    llstSearch.Add(Definition.CONDITION_KEY_START_DTTS,  _comUtil.NVL(this._llstCondition[Definition.CONDITION_KEY_START_DTTS]));
                    llstSearch.Add(Definition.CONDITION_KEY_END_DTTS, _comUtil.NVL(this._llstCondition[Definition.CONDITION_KEY_END_DTTS]));
                    llstSearch.Add(Definition.CONDITION_KEY_MODEL_CONFIG_RAWID, strModelConfigRawID);

                    DataSet dsTemp = _wsSPC.GetATTSPCControlChartData(llstSearch.GetSerialData());
                    DataTable dtChartData = null;
                    if (!DataUtil.IsNullOrEmptyDataSet(dsTemp))
                    {
                        llstSearch.Clear();
                        dtChartData = CommonPageUtil.CLOBnBLOBParsing(dsTemp, llstSearch, false, false, false);
                        //if(sParamTypeCD == "MET")
                        //    dtChartData = BISTel.eSPC.Page.Common.CommonPageUtil.CLOBnBLOBParsing(dsTemp, llstSearch, false);
                        //else
                        //    dtChartData = BISTel.eSPC.Page.Common.CommonPageUtil.CLOBnBLOBParsingRaw(dsTemp, llstSearch, false, false);
                        if (DataUtil.IsNullOrEmptyDataTable(dtChartData))
                        {
                            this.MsgClose();
                            MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                            return;
                        }
                    }
                    else
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }

                    _ChartVariable.dtResource = dtChartData;

                    List<string> rawIDs = new List<string>();
                    if (dtChartData.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                    {
                        bool bPOcap = dtChartData.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST);
                        bool bPNOcap = dtChartData.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST);
                        bool bCOcap = dtChartData.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST);
                        bool bUOcap = dtChartData.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST);

                        foreach (DataRow drTemp in dtChartData.Rows)
                        {
                            string rawid = drTemp[Definition.CHART_COLUMN.OCAP_RAWID].ToString();

                            string sTemp = rawid.Replace(";", "");
                            if (sTemp.Length > 0)
                            {
                                string[] ids = rawid.Split(';');
                                foreach (string id in ids)
                                {
                                    if (string.IsNullOrEmpty(id))
                                    {
                                        continue;
                                    }
                                    if (!rawIDs.Contains(id))
                                    {
                                        rawIDs.Add(id);
                                    }
                                }
                            }

                            if (bPOcap)
                            {
                                rawid = drTemp[Definition.CHART_COLUMN.P_OCAP_LIST].ToString();

                                sTemp = rawid.Replace("^", "").Replace(";", "");
                                if (sTemp.Length > 0)
                                {
                                    string[] ids = rawid.Replace(";", "^").Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id))
                                        {
                                            continue;
                                        }
                                        if (!rawIDs.Contains(id))
                                        {
                                            rawIDs.Add(id);
                                        }
                                    }
                                }
                            }

                            if (bPNOcap)
                            {
                                rawid = drTemp[Definition.CHART_COLUMN.PN_OCAP_LIST].ToString();

                                sTemp = rawid.Replace("^", "");
                                if (sTemp.Length > 0)
                                {
                                    string[] ids = rawid.Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id))
                                        {
                                            continue;
                                        }
                                        if (!rawIDs.Contains(id))
                                        {
                                            rawIDs.Add(id);
                                        }
                                    }
                                }
                            }

                            if (bCOcap)
                            {
                                rawid = drTemp[Definition.CHART_COLUMN.C_OCAP_LIST].ToString();

                                sTemp = rawid.Replace("^", "");
                                if (sTemp.Length > 0)
                                {
                                    string[] ids = rawid.Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id))
                                        {
                                            continue;
                                        }
                                        if (!rawIDs.Contains(id))
                                        {
                                            rawIDs.Add(id);
                                        }
                                    }
                                }
                            }

                            if (bUOcap)
                            {
                                rawid = drTemp[Definition.CHART_COLUMN.U_OCAP_LIST].ToString();

                                sTemp = rawid.Replace("^", "");
                                if (sTemp.Length > 0)
                                {
                                    string[] ids = rawid.Split('^');
                                    foreach (string id in ids)
                                    {
                                        if (string.IsNullOrEmpty(id))
                                        {
                                            continue;
                                        }
                                        if (!rawIDs.Contains(id))
                                        {
                                            rawIDs.Add(id);
                                        }
                                    }
                                }
                            }
                        }

                        if (rawIDs.Count == 0)
                        {
                            rawIDs.Add("");
                        }

                        LinkedList llstTmpOcapComment = new LinkedList();
                        llstTmpOcapComment.Add(Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID, rawIDs.ToArray());

                        if (this._llstCondition[Definition.DynamicCondition_Condition_key.START_DTTS] != null)
                        {
                            llstTmpOcapComment.Add(Definition.CONDITION_KEY_START_DTTS, (DateTime.Parse(this._llstCondition[Definition.DynamicCondition_Condition_key.START_DTTS].ToString())).ToString(Definition.DATETIME_FORMAT_MS));
                        }
                        if (this._llstCondition[Definition.DynamicCondition_Condition_key.END_DTTS] != null)
                        {
                            llstTmpOcapComment.Add(Definition.CONDITION_KEY_END_DTTS, (DateTime.Parse(this._llstCondition[Definition.DynamicCondition_Condition_key.END_DTTS].ToString())).ToString(Definition.DATETIME_FORMAT_MS));
                        }

                        byte[] baData = llstTmpOcapComment.GetSerialData();

                        _ChartVariable.dtOCAP = _wsSPC.GetOCAPCommentList_New(baData).Tables[0];
                    }

                    _ChartVariable.DEFAULT_CHART = strDefaultChartList;
                    _ChartVariable.CHART_PARENT_MODE = BISTel.eSPC.Common.CHART_PARENT_MODE.OCAP;
                    _ChartVariable.dateTimeStart = DateTime.Parse(strTime).AddDays(-7);
                    if (DateTime.Parse(strTime).AddDays(7) > DateTime.Parse(CommonPageUtil.ToDayStart()))
                    {
                        _ChartVariable.dateTimeEnd = DateTime.Parse(CommonPageUtil.ToDayEnd());
                    }
                    else
                    {
                        _ChartVariable.dateTimeEnd = DateTime.Parse(strTime).AddDays(7);
                    }


                    if (string.IsNullOrEmpty(_ChartVariable.DEFAULT_CHART))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, string.Format(MSGHandler.GetMessage("GENERAL_NO_ITEM"), "Default Charts"));
                        return;
                    }

                    if (ChartVariable.dtResource == null)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                        return;
                    }

                    //SPC-1292, KBLEE,START
                    DataSet ds = _wsSPC.GetATTGroupInfoByChartId(this._ChartVariable.MODEL_CONFIG_RAWID);

                    if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
                    {
                        this._lineRawid = ds.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.LOCATION_RAWID].ToString();
                        this._areaRawid = ds.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.AREA_RAWID].ToString();
                        this._sGroupName = ds.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString();
                    }
                    //SPC-1292, KBLEE,END

                    Common.ChartViewPopup chartViewPop = new Common.ChartViewPopup();
                    chartViewPop.ChartVariable = this._ChartVariable;
                    chartViewPop.SessionData = this.SessionData;
                    chartViewPop.URL = this.URL;
                    chartViewPop.LINE_RAWID = this._lineRawid; //SPC-1292, KBLEE
                    chartViewPop.AREA_RAWID = this._areaRawid; //SPC-1292, KBLEE
                    chartViewPop.GROUP_NAME = this._sGroupName; //SPC-1292, KBLEE
                    chartViewPop.InitializePopup();
                    chartViewPop.linkTraceDataViewEventPopup -= new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
                    chartViewPop.linkTraceDataViewEventPopup += new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
                    DialogResult result = chartViewPop.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(ChartVariable.DEFAULT_CHART))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, string.Format(MSGHandler.GetMessage("GENERAL_NO_ITEM"), "Default Charts"));
                    return;
                }

                if (ChartVariable.dtResource == null)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                Common.ChartViewPopup chartViewPop = new Common.ChartViewPopup();
                chartViewPop.ChartVariable = ChartVariable;
                chartViewPop.SessionData = this.SessionData;
                chartViewPop.URL = this.URL;
                chartViewPop.LINE_RAWID = this._lineRawid; //SPC-1292, KBLEE
                chartViewPop.AREA_RAWID = this._areaRawid; //SPC-1292, KBLEE
                chartViewPop.GROUP_NAME = this._sGroupName; //SPC-1292, KBLEE
                chartViewPop.InitializePopup();

                chartViewPop.linkTraceDataViewEventPopup -= new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);
                chartViewPop.linkTraceDataViewEventPopup += new Common.ChartViewPopup.LinkTraceDataViewEventHandlerPopup(chartViewPop_linkTraceDataViewEventPopup);

                DialogResult result = chartViewPop.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                }
            }
        }

        void chartViewPop_linkTraceDataViewEventPopup(object sender, EventArgs e, LinkedList llstTraceLinkData)
        {
            if(this.linkTraceDataViewEventOcap != null)
                linkTraceDataViewEventOcap(this, null, llstTraceLinkData);
        } 

        private void bbtnClose_Click(object sender, EventArgs e)
        {
            //this.Dispose();
            this.Close();
        }

        #endregion

        #endregion


        #region : Public

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }
        
        public BISTel.eSPC.Common.enum_PopupType PopUpType
        {
            get { return _popupType; }
            set { _popupType = value; }
        }

        public ChartInterface ChartVariable
        {
            get { return _ChartVariable; }
            set { _ChartVariable = value; }
        }

        public string ParamTypeCD
        {
            get { return this.mParamTypeCd; }
            set { this.mParamTypeCd = value; }
        }

        public DataSet DSSelectedOCAP
        {
            get { return this._dsSelectedOCAP; }
            set { this._dsSelectedOCAP = value; }
        }

        public bool ISMulti
        {
            get { return this._isMulti; }
            set { this._isMulti = value; }
        }

        public LinkedList llstCondition
        {
            get { return this._llstCondition; }
            set { this._llstCondition = value; }
        }

        //SPC-1292, KBLEE, START
        public string LINE_RAWID
        {
            get { return _lineRawid; }
            set { _lineRawid = value; }
        }

        public string AREA_RAWID
        {
            get { return _areaRawid; }
            set { _areaRawid = value; }
        }

        public string GROUP_NAME
        {
            get { return _sGroupName; }
            set { _sGroupName = value; }
        }
        //SPC-1292, KBLEE, END

        #endregion

        private void bbtnProblemInput_Click(object sender, EventArgs e)
        {
            if (this.bcboProblem.Items.Count != 0)
            {
                this.bcboProblem.Visible = true;
            }
            this.btxtProblemInput.Visible = true;            
        }

        private void bbtnCauseInput_Click(object sender, EventArgs e)
        {
            if (this.bcboCause.Items.Count != 0)
            {
                this.bcboCause.Visible = true;
            }
            
            this.btxtCauseInput.Visible = true;            
        }

        private void bbtnSolutionInput_Click(object sender, EventArgs e)
        {
            if (this.bcboSolution.Items.Count != 0)
            {
                this.bcboSolution.Visible = true;
            }
            
            this.btxtSolutionInput.Visible = true;            
        }

        private void bbtnCommentInput_Click(object sender, EventArgs e)
        {
            if (this.bcboComment.Items.Count != 0)
            {
                this.bcboComment.Visible = true;
            }
            
            this.btxtCommentInput.Visible = true;            
        }

        private void bcboProblem_TextChanged(object sender, EventArgs e)
        {
            this.btxtProblemInput.Text = bcboProblem.Text;
        }

        private void bcboCause_TextChanged(object sender, EventArgs e)
        {
            this.btxtCauseInput.Text = bcboCause.Text;
        }

        private void bcboSolution_TextChanged(object sender, EventArgs e)
        {
            this.btxtSolutionInput.Text = bcboSolution.Text;
        }

        private void bcboComment_TextChanged(object sender, EventArgs e)
        {
            this.btxtCommentInput.Text = bcboComment.Text;
        }

    }
}
