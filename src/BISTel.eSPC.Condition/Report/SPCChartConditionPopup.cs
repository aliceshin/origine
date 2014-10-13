using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;



using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Default;

namespace BISTel.eSPC.Condition.Report
{
    public partial class SPCChartConditionPopup : BasePopupFrm
    {

        public enum SPCChartConditionContext
        {
            SELECT = 0,
            MODEL_CONFIG_RAWID,
            DEFAULT_CHART_LIST,
            RESTRICT_SAMPLE_DAYS,
            AREA_RAWID,
            AREA,
            EQP_MODEL,
            PARAM_ALIAS,
            MAIN_YN,
            COUNT = 9,

        }


        #region : Field
        Initialization _Initialization;
        BSpreadUtility _bspreadutility;
        CommonUtility _ComUtil;
        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _wsSPC;
        SessionData _SessionData;


        LinkedList _llstData = new LinkedList();
        DialogResult _digResult = new DialogResult();

        DataTable mDTMCRawIDContext;
        DataTable mDTBind;
        DataTable mDTContextKey;
        DataTable mDTModelConfigRawID;
        DataSet mDSOperation;

        SPCStruct.ChartConditionContextList strucContextList;
        SPCStruct.ChartContextInfo strucContextinfo;

        FarPoint.Win.Spread.SheetView bSpreadContext_Sheet1;
        BSpread bSpreadContext;

        string mModelConfigRawID = string.Empty;
        string mParamAlias = string.Empty;

        private bool isAtt = false;
        private bool _bUseComma;

        public bool ISATT
        {
            get { return this.isAtt; }
            set { this.isAtt = value; }
        }

        #endregion


        #region Properties

        public DialogResult ButtonResult
        {
            get { return this._digResult; }
            set { this._digResult = value; }
        }

        public SPCStruct.ChartConditionContextList StrucContextList
        {
            get { return strucContextList; }
            set { strucContextList = value; }
        }

        public SPCStruct.ChartContextInfo CONTEXT_INFO
        {
            get { return this.strucContextinfo; }
            set { strucContextinfo = value; }
        }

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }
        #endregion

        #region : Constructor
        public SPCChartConditionPopup()
        {
            InitializeComponent();
            this.InitializeVariable();

        }

        #endregion


        #region : Initialization

        public void InitializePopup()
        {
            this.InitializeLayout();
            this.InitializeBSpread();
            this.InitializeBind();
        }

        public void InitializeVariable()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
            this._bspreadutility = new BSpreadUtility();
            this._ComUtil = new CommonUtility();
            this._llstData = new LinkedList();
            this.strucContextList = new SPCStruct.ChartConditionContextList();
            this.strucContextinfo = new SPCStruct.ChartContextInfo();
        }

        public void InitializeLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.POPUP_TITLE_KEY_SPCMODEL_LIST);
            this.btxtLine.Text = this.strucContextinfo.LINE;
            this.btxtArea.Text = this.strucContextinfo.AREA;
            this.btxtSpcModel.Text = this.strucContextinfo.SPC_MODEL_NAME;
            this.mModelConfigRawID = this.strucContextinfo.MODEL_CONFIG_RAWID;
            this.bTabControl1.TabPages[0].Text = "Model List";
            this._bUseComma = AppConfiguration.GetConfigValue(Definition.PATH_ESPC, Definition.PATH_SPC_MODELING, Definition.CONFIG_USE_COMMA, false);

            //SPC-744 By Louis TabContol에 Scrollbar추가
          //  this.bTabControl1.AutoScrollOffset.
        }

        public void InitializeBSpread()
        {
            this.bSpreadModelList.UseSpreadEdit = false;
            this.bSpreadModelList.AutoGenerateColumns = false;
            this.bSpreadModelList.ActiveSheet.DefaultStyle.ResetLocked();
            this.bSpreadModelList.UseEdit = true;
            this.bSpreadModelList.ClearHead();
            this.bSpreadModelList.Locked = true;
            this.bSpreadModelList.UseHeadColor = true;
            this.bSpreadModelList.EditMode = false;
            this.bSpreadModelList.ActiveSheet.RowCount = 0;
            this.bSpreadModelList.ActiveSheet.ColumnCount = 0;

            

            DataTableGroupBy dtGroupBy = new DataTableGroupBy();

            string _sField = string.Format("{0}, max({1}) {1}", COLUMN.CONTEXT_KEY, COLUMN.KEY_ORDER);
            string _sFilter = string.Empty;
            this.mDTContextKey = dtGroupBy.SelectGroupByInto(COLUMN.CONTEXT_KEY, this.strucContextinfo.DTModelContext, _sField, null, COLUMN.CONTEXT_KEY);

            StringBuilder sbField = new StringBuilder();
            sbField.AppendFormat("{0},", COLUMN.AREA_RAWID);
            sbField.AppendFormat("{0},", COLUMN.AREA);
            sbField.AppendFormat("{0},", COLUMN.EQP_MODEL);
            sbField.AppendFormat("{0},", COLUMN.MODEL_CONFIG_RAWID);
            sbField.AppendFormat("{0},", COLUMN.MAIN_YN);
            sbField.AppendFormat("{0},", COLUMN.PARAM_ALIAS);
            sbField.AppendFormat("{0},{1}", COLUMN.RESTRICT_SAMPLE_DAYS, COLUMN.DEFAULT_CHART_LIST);

            //Parent가 SPC_CONTROL_CHART 화면인경우 해당 Model만조회한다.
            if (this.strucContextinfo.SPCModelType == SPCMODEL_TYPE.SPC_CHART)
                _sFilter = string.Format("{0}='{1}'", COLUMN.MODEL_CONFIG_RAWID, this.mModelConfigRawID);

            this.mDTModelConfigRawID = dtGroupBy.SelectGroupByInto(COLUMN.MODEL_CONFIG_RAWID, this.strucContextinfo.DTModelContext, sbField.ToString(), _sFilter, sbField.ToString());


            this.mDTContextKey = DataUtil.DataTableImportRow(mDTContextKey.Select(null, COLUMN.KEY_ORDER));
            int iRow = (int)SPCChartConditionContext.COUNT;
            this.bSpreadModelList.DataSource = null;
            this.bSpreadModelList.ClearHead();
            this.bSpreadModelList.ActiveSheet.RowCount = this.mDTModelConfigRawID.Rows.Count;
            this.bSpreadModelList.ActiveSheet.ColumnCount = this.mDTContextKey.Rows.Count + iRow;


            if (this.strucContextinfo.SPCModelType == SPCMODEL_TYPE.SPC_CHART)
                this.bSpreadModelList.AddHead((int)SPCChartConditionContext.SELECT, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.V_SELECT), COLUMN.SELECT, 40, 20, null, null, null, ColumnAttribute.ReadOnly, ColumnType.CheckBox, null, null, null, false, true);
            else
                this.bSpreadModelList.AddHead((int)SPCChartConditionContext.SELECT, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.V_SELECT), COLUMN.SELECT, 40, 20, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);


            this.bSpreadModelList.AddHead((int)SPCChartConditionContext.MODEL_CONFIG_RAWID, COLUMN.MODEL_CONFIG_RAWID, COLUMN.MODEL_CONFIG_RAWID, 40, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, false);
            this.bSpreadModelList.AddHead((int)SPCChartConditionContext.DEFAULT_CHART_LIST, COLUMN.DEFAULT_CHART_LIST, COLUMN.DEFAULT_CHART_LIST, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, false);
            this.bSpreadModelList.AddHead((int)SPCChartConditionContext.RESTRICT_SAMPLE_DAYS, COLUMN.RESTRICT_SAMPLE_DAYS, COLUMN.RESTRICT_SAMPLE_DAYS, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, false);
            this.bSpreadModelList.AddHead((int)SPCChartConditionContext.AREA_RAWID, COLUMN.AREA_RAWID, COLUMN.AREA_RAWID, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, false);
            this.bSpreadModelList.AddHead((int)SPCChartConditionContext.AREA, COLUMN.AREA, COLUMN.AREA, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, false);
            this.bSpreadModelList.AddHead((int)SPCChartConditionContext.EQP_MODEL, COLUMN.EQP_MODEL, COLUMN.EQP_MODEL, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, false);
            this.bSpreadModelList.AddHead((int)SPCChartConditionContext.PARAM_ALIAS, COLUMN.PARAM_ALIAS, COLUMN.PARAM_ALIAS, 80, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, true);
            this.bSpreadModelList.AddHead((int)SPCChartConditionContext.MAIN_YN, COLUMN.MAIN_YN, COLUMN.MAIN_YN, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, true);

            for (int i = 0; i < this.mDTContextKey.Rows.Count; i++)
            {
                string sContextKey = mDTContextKey.Rows[i][COLUMN.CONTEXT_KEY].ToString();
                this.bSpreadModelList.AddHead(iRow++, sContextKey, sContextKey, 120, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, true);
            }


            for (int i = 1; i < this.bSpreadModelList.ActiveSheet.ColumnCount; i++)
                this.bSpreadModelList.ActiveSheet.Columns[i].Locked = true;

            //SPC-777 By Louis ContextMenu Delete
            this.bSpreadModelList.UseGeneralContextMenu = false;
        }


        public void InitializeBind()
        {
            PROC_CreateDTBind();
        }



        private void InitializeBSpreadContxt(string sName, DataTable _dt)
        {
            bool bDesc = false;

            bSpreadContext_Sheet1 = new FarPoint.Win.Spread.SheetView();
            bSpreadContext_Sheet1.Reset();
            bSpreadContext_Sheet1.SheetName = "Sheet1";

            bSpreadContext = new BSpread();
            bSpreadContext.Name = "bSpreadContext_" + sName;
            bSpreadContext.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] { bSpreadContext_Sheet1 });
            bSpreadContext.UseSpreadEdit = false;
            bSpreadContext.AutoGenerateColumns = false;
            bSpreadContext.ActiveSheet.DefaultStyle.ResetLocked();
            bSpreadContext.UseEdit = true;
            bSpreadContext.ClearHead();
            bSpreadContext.Locked = true;
            bSpreadContext.UseHeadColor = true;
            bSpreadContext.EditMode = false;
            bSpreadContext.ActiveSheet.RowCount = 0;
            bSpreadContext.ActiveSheet.ColumnCount = 0;
            bSpreadContext.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            bSpreadContext.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            bSpreadContext.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(bSpreadContext_ButtonClicked);

            bSpreadContext.DataSource = null;
            bSpreadContext.ClearHead();

            //SPC-777 Louis ContextMenu 삭제;
            bSpreadContext.UseGeneralContextMenu = false;

            //if (sName == Definition.CHART_COLUMN.OPERATION_ID || sName == Definition.CHART_COLUMN.MEASURE_OPERATION_ID) bDesc = true;

            //if (bDesc)
            //{
            //    bSpreadContext.ActiveSheet.ColumnCount = 3;
            //    bSpreadContext.Width = 300;
            //}
            //else
            //{
            //    bSpreadContext.ActiveSheet.ColumnCount = 2;
            //    bSpreadContext.Width = 180;
            //}

            bSpreadContext.AddHead((int)SPCChartConditionContext.SELECT, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.V_SELECT), COLUMN.SELECT, 40, 20, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);
            bSpreadContext.AddHead(1, sName, COLUMN.CONTEXT_VALUE, 80, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, true);

            if (bDesc)
                bSpreadContext.AddHead(2, COLUMN.DESCRIPTION, COLUMN.DESCRIPTION, 120, 20, null, null, null, ColumnAttribute.Null, ColumnType.String, null, null, null, false, true);

            bSpreadContext.DataSource = _dt;

            this.bSpreadContext.Width = 40;

            for (int i = 0; i < bSpreadContext.ActiveSheet.Columns.Count; i++)
            {
                float _fSpreadWidth = bSpreadContext.ActiveSheet.Columns[i].GetPreferredWidth();
                this.bSpreadContext.ActiveSheet.Columns[i].Width = _fSpreadWidth;
                this.bSpreadContext.Width += (int)_fSpreadWidth;

            }
        }




        #endregion

        #region User Defined

        private void PROC_Operation()
        {
            LinkedList mllstData = new LinkedList();
            //mllstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_TYPE, argParamTYPE);            
            this.mDSOperation = _wsSPC.GetOperationID(mllstData.GetSerialData());
        }

        private string GetOperationDesc(string argOperationID)
        {
            string sDesc = string.Empty;
            if (!DataUtil.IsNullOrEmptyDataSet(this.mDSOperation))
            {
                DataRow[] drSelect = this.mDSOperation.Tables[0].Select(string.Format("OPERATION_ID='{0}'", argOperationID));
                if (drSelect.Length > 0)
                    sDesc = drSelect[0][COLUMN.DESCRIPTION].ToString();
            }

            return sDesc;
        }



        public void PROC_CreateDTBind()
        {
            DataRow drNew = null;
            bool bOperation = false;
            int iRowOperation = 0;
            int iSelect = -1;
            try
            {
                mDTBind = new DataTable();
                mDTBind.Columns.Add(COLUMN.SELECT, typeof(bool));
                mDTBind.Columns.Add(COLUMN.MODEL_CONFIG_RAWID, typeof(string));
                mDTBind.Columns.Add(COLUMN.DEFAULT_CHART_LIST, typeof(string));
                mDTBind.Columns.Add(COLUMN.RESTRICT_SAMPLE_DAYS, typeof(string));
                mDTBind.Columns.Add(COLUMN.AREA_RAWID, typeof(string));
                mDTBind.Columns.Add(COLUMN.AREA, typeof(string));
                mDTBind.Columns.Add(COLUMN.EQP_MODEL, typeof(string));
                mDTBind.Columns.Add(COLUMN.MAIN_YN, typeof(string));
                mDTBind.Columns.Add(COLUMN.PARAM_ALIAS, typeof(string));
                foreach (DataRow dr in this.mDTModelConfigRawID.Rows)
                {
                    string sWhere = string.Format(" {0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, dr[COLUMN.MODEL_CONFIG_RAWID].ToString());
                    drNew = mDTBind.NewRow();

                    if (!string.IsNullOrEmpty(this.strucContextinfo.MODEL_CONFIG_RAWID) && this.strucContextinfo.MODEL_CONFIG_RAWID == dr[COLUMN.MODEL_CONFIG_RAWID].ToString())
                    {
                        drNew[COLUMN.SELECT] = true;
                        iSelect = mDTBind.Rows.Count;
                    }
                    else
                        drNew[COLUMN.SELECT] = false;

                    drNew[COLUMN.MAIN_YN] = dr[COLUMN.MAIN_YN];
                    drNew[COLUMN.PARAM_ALIAS] = dr[COLUMN.PARAM_ALIAS];
                    drNew[COLUMN.RESTRICT_SAMPLE_DAYS] = dr[COLUMN.RESTRICT_SAMPLE_DAYS];
                    drNew[COLUMN.DEFAULT_CHART_LIST] = dr[COLUMN.DEFAULT_CHART_LIST];
                    drNew[COLUMN.MODEL_CONFIG_RAWID] = dr[COLUMN.MODEL_CONFIG_RAWID];
                    drNew[COLUMN.AREA_RAWID] = dr[COLUMN.AREA_RAWID];
                    drNew[COLUMN.AREA] = dr[COLUMN.AREA];
                    drNew[COLUMN.EQP_MODEL] = dr[COLUMN.EQP_MODEL];

                    foreach (DataRow drC in this.mDTContextKey.Rows)
                    {
                        string sColumn = drC[COLUMN.CONTEXT_KEY].ToString();
                        if (!mDTBind.Columns.Contains(sColumn))
                            mDTBind.Columns.Add(sColumn, typeof(string));

                        //공정인경우 DESCRIPTION 표현
                        if (!bOperation && sColumn.IndexOf("OPERATION_ID") > -1)
                        {
                            bOperation = true;
                            this.PROC_Operation();
                            iRowOperation = mDTBind.Columns.Count - 1;
                        }

                        DataRow[] drSelect = this.strucContextinfo.DTModelContext.Select(string.Format("{0} AND {1}='{2}'", sWhere, COLUMN.CONTEXT_KEY, sColumn), COLUMN.KEY_ORDER);
                        foreach (DataRow drSelect2 in drSelect)
                            drNew[sColumn] = drSelect2[COLUMN.CONTEXT_VALUE];

                    }
                    mDTBind.Rows.Add(drNew);
                }

                this.bSpreadModelList.DataSource = mDTBind;
                this.bSpreadModelList.ActiveSheet.Columns[1, this.bSpreadModelList.ActiveSheet.ColumnCount - 1].Locked = true;

                for (int i = 0; i < this.bSpreadModelList.ActiveSheet.ColumnCount; i++)
                {
                    this.bSpreadModelList.ActiveSheet.Columns[i].Width = this.bSpreadModelList.ActiveSheet.Columns[i].GetPreferredWidth();
                }

                if (bOperation && !DataUtil.IsNullOrEmptyDataSet(this.mDSOperation))
                {
                    for (int i = 0; i < this.bSpreadModelList.ActiveSheet.RowCount; i++)
                    {
                        string sOperation = this.bSpreadModelList.ActiveSheet.Cells[i, iRowOperation].Text;
                        this.bSpreadModelList.ActiveSheet.Cells[i, iRowOperation].Note = this.GetOperationDesc(sOperation);
                    }
                }

                if (iSelect > -1)
                    this.PROC_MainModelContext(iSelect);
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
            finally
            {

            }
        }

        private bool isContains(string argKey, string argValue)
        {
            bool bRtrn = false;

            if (this.strucContextinfo.llstCustomContext != null)
            {
                if (this.strucContextinfo.llstCustomContext.Contains(argKey) && this.strucContextinfo.llstCustomContext[argKey].ToString().IndexOf(argValue) > -1)
                {
                    bRtrn = true;
                }
            }

            return bRtrn;
        }

        private void PROC_MainModelContext(int iSelect)
        {

            DataTableGroupBy dtGroupBy = new DataTableGroupBy();
            int iContext = (int)SPCChartConditionContext.COUNT;
            DataRow dr = null;
            string _model_config_rawid = this.mModelConfigRawID;
            string _main_yn = this.strucContextinfo.MAIN_YN;
            DataTable _mDTContextKey = this.mDTContextKey;
            if (iSelect > -1)
            {
                if (this.bSpreadModelList != null && this.bSpreadModelList.ActiveSheet.RowCount > 0)
                    _model_config_rawid = this.bSpreadModelList.ActiveSheet.Cells[iSelect, (int)SPCChartConditionContext.MODEL_CONFIG_RAWID].Text;
                _main_yn = this.bSpreadModelList.ActiveSheet.Cells[iSelect, (int)SPCChartConditionContext.MAIN_YN].Text;

                if (_main_yn != "Y") return;

                string _sFilter = string.Format("{0}='{1}'", COLUMN.MODEL_CONFIG_RAWID, _model_config_rawid);
                string _sColumn = COLUMN.CONTEXT_KEY + "," + COLUMN.KEY_ORDER;
                _mDTContextKey = dtGroupBy.SelectGroupByInto(COLUMN.CONTEXT_KEY, this.strucContextinfo.DTModelContext, _sColumn, _sFilter, _sColumn);
                _mDTContextKey = DataUtil.DataTableImportRow(_mDTContextKey.Select(null, COLUMN.KEY_ORDER));
            }

            if (this.bTabControl1.TabPages.Count > 1)
                this.bTabControl1.TabPages.RemoveAt(this.bTabControl1.TabPages.Count - 1);

            this.bTabControl1.TabPages.Add("Main Model Context");
            this.bTabControl1.TabPages[this.bTabControl1.TabPages.Count - 1].BackColor = Color.White;

            //SPC-744 By Louis - tabpage AutoScroll 추가
            this.bTabControl1.TabPages[this.bTabControl1.TabPages.Count - 1].AutoScroll = true;
            ///////
            if (this.strucContextinfo.SPCModelType == SPCMODEL_TYPE.SPC_CHART)
                this.bTabControl1.SelectedIndex = this.bTabControl1.TabPages.Count - 1;

            for (int xi = _mDTContextKey.Rows.Count - 1; xi >= 0; xi--)
            {
                string sColumn = _mDTContextKey.Rows[xi][COLUMN.CONTEXT_KEY].ToString();
                string sValue = string.Empty;

                for (int j = iContext; j < this.bSpreadModelList.ActiveSheet.ColumnCount; j++)
                {
                    if (sColumn == this.bSpreadModelList.ActiveSheet.Columns[j].DataField)
                    {
                        sValue = this.bSpreadModelList.ActiveSheet.Cells[iSelect, j].Text;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(sValue)) continue;

                mDTMCRawIDContext = new DataTable();
                mDTMCRawIDContext.Columns.Add(COLUMN.SELECT, typeof(bool));
                mDTMCRawIDContext.Columns.Add(COLUMN.CONTEXT_VALUE, typeof(string));
                mDTMCRawIDContext.Columns.Add(COLUMN.DESCRIPTION, typeof(string));


                if (sValue == Definition.VARIABLE.STAR)
                {
                    DataTable _dt = dtGroupBy.SelectGroupByInto(sColumn, this.strucContextinfo.DTModelContext, COLUMN.CONTEXT_VALUE, string.Format("{0}='{1}'", COLUMN.CONTEXT_KEY, sColumn), COLUMN.CONTEXT_VALUE);
                    foreach (DataRow drData in _dt.Rows)
                    {
                        dr = mDTMCRawIDContext.NewRow();
                        if (this.strucContextinfo.llstCustomContext != null && this.strucContextinfo.llstCustomContext.Count > 0)
                            dr[COLUMN.SELECT] = isContains(sColumn, drData[COLUMN.CONTEXT_VALUE].ToString());
                        else if (drData[COLUMN.CONTEXT_VALUE].ToString() == sValue)
                            dr[COLUMN.SELECT] = true;
                        else
                            dr[COLUMN.SELECT] = false;


                        dr[COLUMN.CONTEXT_VALUE] = drData[COLUMN.CONTEXT_VALUE];
                        if (sColumn.IndexOf("OPERATION_ID") > -1)
                            dr[COLUMN.DESCRIPTION] = this.GetOperationDesc(drData[COLUMN.CONTEXT_VALUE].ToString());
                        mDTMCRawIDContext.Rows.Add(dr);
                    }
                }
                else
                {
                    string[] arr = sValue.Split(';');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(arr[i])) continue;
                        dr = mDTMCRawIDContext.NewRow();

                        if (this.strucContextinfo.llstCustomContext != null && this.strucContextinfo.llstCustomContext.Count > 0)
                            dr[COLUMN.SELECT] = isContains(sColumn, arr[i]);
                        else
                            dr[COLUMN.SELECT] = true;

                        dr[COLUMN.CONTEXT_VALUE] = arr[i];

                        if (sColumn.IndexOf("OPERATION_ID") > -1)
                            dr[COLUMN.DESCRIPTION] = this.GetOperationDesc(arr[i]);

                        mDTMCRawIDContext.Rows.Add(dr);
                    }
                }
                this.InitializeBSpreadContxt(sColumn, mDTMCRawIDContext);
                this.bSpreadContext.Dock = DockStyle.Left;
                this.bTabControl1.TabPages[this.bTabControl1.TabPages.Count - 1].Controls.Add(this.bSpreadContext);
                

            }
        }
        #endregion


        #region Event Button

        public void bSpreadModelList_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != (int)SPCChartConditionContext.SELECT) return;
            BSpread bsprData = sender as BSpread;
            if ((bool)bsprData.GetCellValue(e.Row, (int)SPCChartConditionContext.SELECT) == true)
            {
                for (int i = 0; i < bsprData.ActiveSheet.RowCount; i++)
                {
                    if (i == e.Row) continue;
                    bsprData.ActiveSheet.Cells[i, (int)SPCChartConditionContext.SELECT].Value = 0;
                }

                ArrayList alCheckRowIndex = this._bspreadutility.GetCheckedRowIndex(bsprData, 0);
                if (alCheckRowIndex.Count > 0)
                {
                    string sMainYN = bsprData.ActiveSheet.Cells[(int)alCheckRowIndex[0], (int)SPCChartConditionContext.MAIN_YN].Text;
                    this.mModelConfigRawID = bsprData.ActiveSheet.Cells[(int)alCheckRowIndex[0], (int)SPCChartConditionContext.MODEL_CONFIG_RAWID].Text;
                    if (sMainYN.Equals("Y"))
                    {
                        this.PROC_MainModelContext((int)alCheckRowIndex[0]);
                    }
                    else
                    {
                        for (int j = 1; j < this.bTabControl1.TabPages.Count; j++)
                            this.bTabControl1.TabPages.RemoveAt(j);
                    }
                }
            }
        }



        public void bSpreadModelList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //this.toolTip1.ToolTipTitle  = "dddd";
        }



        private void bSpreadContext_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != (int)SPCChartConditionContext.SELECT) return;
            BSpread bsprData = sender as BSpread;
            if ((bool)bsprData.GetCellValue(e.Row, (int)SPCChartConditionContext.SELECT) == true)
            {

                string sValue = bsprData.ActiveSheet.Cells[e.Row, 1].Text;
                ArrayList alCheckRowIndex = this._bspreadutility.GetCheckedRowIndex(bsprData, 0);
                if (sValue == Definition.VARIABLE.STAR)
                {
                    for (int j = 0; j < alCheckRowIndex.Count; j++)
                    {
                        string sTarget = bsprData.ActiveSheet.Cells[(int)alCheckRowIndex[j], 1].Text;
                        if (sTarget != sValue)
                            bsprData.ActiveSheet.Cells[(int)alCheckRowIndex[j], (int)SPCChartConditionContext.SELECT].Value = 0;
                    }
                }
                else
                {
                    for (int j = 0; j < alCheckRowIndex.Count; j++)
                    {
                        string sTarget = bsprData.ActiveSheet.Cells[(int)alCheckRowIndex[j], 1].Text;
                        if (sTarget == Definition.VARIABLE.STAR)
                            bsprData.ActiveSheet.Cells[(int)alCheckRowIndex[j], (int)SPCChartConditionContext.SELECT].Value = 0;
                    }

                }
            }
        }



        public void bbtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList alCheckRowIndex = this._bspreadutility.GetCheckedRowIndex(this.bSpreadModelList, 0);
                if (alCheckRowIndex.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_CHOOSE_ITEM_FOR_TARGET", null, null);
                    return;
                }
                else if (alCheckRowIndex.Count > 1)
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_CANT_SELECT_MORE_THAN_ONE", null, null);
                    return;
                }

                int iRow = (int)alCheckRowIndex[0];
                this.strucContextinfo.MODEL_CONFIG_RAWID = this.bSpreadModelList.ActiveSheet.Cells[iRow, (int)SPCChartConditionContext.MODEL_CONFIG_RAWID].Text;
                this.strucContextinfo.PARAM_ALIAS = this.bSpreadModelList.ActiveSheet.Cells[iRow, (int)SPCChartConditionContext.PARAM_ALIAS].Text;
                this.strucContextinfo.MAIN_YN = this.bSpreadModelList.ActiveSheet.Cells[iRow, (int)SPCChartConditionContext.MAIN_YN].Text;
                this.strucContextinfo.DEFAULT_CHART_LIST = this.bSpreadModelList.ActiveSheet.Cells[iRow, (int)SPCChartConditionContext.DEFAULT_CHART_LIST].Text;

                this.strucContextinfo.AREA = this.bSpreadModelList.ActiveSheet.Cells[iRow, (int)SPCChartConditionContext.AREA].Text;
                this.strucContextinfo.AREA_RAWID = this.bSpreadModelList.ActiveSheet.Cells[iRow, (int)SPCChartConditionContext.AREA_RAWID].Text;
                this.strucContextinfo.EQP_MODEL = this.bSpreadModelList.ActiveSheet.Cells[iRow, (int)SPCChartConditionContext.EQP_MODEL].Text;

                string sRESTRICT_SAMPLE_DAYS = this.bSpreadModelList.ActiveSheet.Cells[iRow, (int)SPCChartConditionContext.RESTRICT_SAMPLE_DAYS].Text;
                this.strucContextinfo.RESTRICT_SAMPLE_DAYS = string.IsNullOrEmpty(sRESTRICT_SAMPLE_DAYS) ? Definition.RESTRICT_SAMPLE_DAYS : int.Parse(sRESTRICT_SAMPLE_DAYS);


                int iCol = ((int)SPCChartConditionContext.MAIN_YN) + 1;
                this.strucContextinfo.llstContext = new LinkedList();
                this.strucContextinfo.llstContext.Clear();
                foreach (DataRow dr in this.mDTContextKey.Rows)
                {
                    string sContextKey = dr[COLUMN.CONTEXT_KEY].ToString();
                    string sValue = this.bSpreadModelList.ActiveSheet.Cells[iRow, iCol++].Text;
                    if (!string.IsNullOrEmpty(sValue))

                        this.strucContextinfo.llstContext.Add(sContextKey, sValue);
                }

                this.strucContextinfo.llstCustomContext = new LinkedList();
                if (this.strucContextinfo.MAIN_YN.Equals("Y"))
                {
                    Control.ControlCollection ctrl = this.bTabControl1.TabPages[this.bTabControl1.TabPages.Count - 1].Controls;
                    BSpread _bSpread = null;
                    this.strucContextinfo.llstCustomContext.Clear();
                    for (int i = 0; i < this.strucContextinfo.llstContext.Count; i++)
                    {
                        string sKey = this.strucContextinfo.llstContext.GetKey(i).ToString();
                        string sValue = this.strucContextinfo.llstContext.GetValue(i).ToString();
                        string sControlName = "bSpreadContext_" + sKey;
                        string sCustomValue = string.Empty;
                        bool bFlag = false;

                        for (int j = 0; j < ctrl.Count; j++)
                        {
                            if (ctrl[j].Name == sControlName)
                            {
                                bFlag = true;
                                _bSpread = ctrl[j] as BSpread;
                                alCheckRowIndex = this._bspreadutility.GetCheckedRowIndex(_bSpread, 0);
                                for (int k = 0; k < alCheckRowIndex.Count; k++)
                                {
                                    sCustomValue += _bSpread.ActiveSheet.Cells[(int)alCheckRowIndex[k], 1].Text + ";";
                                }
                                if (!string.IsNullOrEmpty(sCustomValue))
                                {
                                    sCustomValue = sCustomValue.Substring(0, sCustomValue.Length - 1);
                                }
                                break;
                            }
                        }

                        if (bFlag)
                        {
                            if (!string.IsNullOrEmpty(sCustomValue))
                            {
                                //if (sCustomValue != sValue)
                                    this.strucContextinfo.llstCustomContext.Add(sKey, sCustomValue);
                            }
                        }

                    }
                }
                this.ButtonResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {

            }
        }


        public void bbtnClose_Click(object sender, System.EventArgs e)
        {
            this.ButtonResult = DialogResult.Cancel;
            this.Close();
        }


        #endregion




    }
}
