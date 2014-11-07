using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.Modeling
{
    //SPC-929, KBLEE, 본 Issue에 의해 제작된 Class.
    public partial class SummaryDataOptionPopup : BasePopupFrm
    {
        #region # Field

        Initialization _Initialization = null;
        MultiLanguageHandler _mlthandler = null;
        SessionData _sessionData = null;
        eSPCWebService.eSPCWebService _wsSPC = null;

        private string _sfab;
        private string _sLine;
        private string _sArea;
        private string _sEqpModel;
        private string _sParameter;
        private string _sRecipe;
        private string _sStep;

        private string _sTempModuleid;
        private int _iSelectedEqpChamberIndex = -1;

        private DataSet _dsEqpModules;
        private DataSet _dsSummaryOptionResult;

        private bool _isSelectedRawChange = false;
        private bool _isStepExist = false;
        private bool _isRecipeExist = false;
        private bool _isAllExist = false;

        SortedList _slSelectRecipe;
        SortedList _slSelectResult;

        enum iChamberColIdx
        {
            SELECT,
            EQP_ID,
            CHAMBER
        }

        enum iRecipeColIdx
        {
            SELECT,
            RECIPE_ID,
            STEP
        }

        enum iResultColIdx
        {
            SELECT,
            EQP_ID,
            CHAMBER,
            RECIPE_ID,
            STEP,
            EQP_MODLUE_ID
        }

        #endregion


        #region #Property

        public SessionData SSESIONDATA
        {
            get { return _sessionData; }
            set { _sessionData = value; }
        }

        public string FAB
        {
            get { return _sfab; }
            set { _sfab = value; }
        }

        public string LINE
        {
            get { return _sLine; }
            set { _sLine = value; }
        }

        public string AREA
        {
            get { return _sArea; }
            set { _sArea = value; }
        }

        public string PARAMETER
        {
            get { return _sParameter; }
            set { _sParameter = value; }
        }

        public DataSet EQPMODULEDATA
        {
            set { _dsEqpModules = value; }
        }

        public string EQPMODEL
        {
            set { _sEqpModel = value; }
        }

        public string RECIPE
        {
            get { return _sRecipe; }
            set { _sRecipe = value; }
        }
        public string STEP
        {
            get { return _sStep; }
            set { _sStep = value; }
        }

        public bool ISSELECTEDRAWCHANGE
        {
            set { _isSelectedRawChange = value; }
        }

        public bool ISSTEPEXIST
        {
            get { return _isStepExist; }
            set { _isStepExist = value; }
        }

        public bool ISRECIPEEXIST
        {
            get { return _isRecipeExist; }
            set { _isRecipeExist = value; }
        }

        public DataSet SUMMARYOPTIONRESULT
        {
            get { return _dsSummaryOptionResult; }
            set { _dsSummaryOptionResult = value; }
        }

        #endregion


        public SummaryDataOptionPopup()
        {
            InitializeComponent();
        }


        #region #Developer Defined Method

        public void InitializePopup()
        {
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            this.Title = Definition.POPUP_TITLE_SUMMARY_DATA_OPTION;
            this.tbxFab.Text = _sfab;
            this.tbxFab.Enabled = false;
            this.tbxLine.Text = _sLine;
            this.tbxLine.Enabled = false;
            this.tbxArea.Text = _sArea;
            this.tbxArea.Enabled = false;
            this.tbxEQPModel.Text = _sEqpModel;
            this.tbxEQPModel.Enabled = false;
            this.tbxParam.Text = _sParameter;
            this.tbxParam.Enabled = false;

            if (_sEqpModel.Equals(""))
            {
                this.bapnlEqpModel.Visible = false;
                this.pnlBlank.Visible = false;
            }

            bool isRecipeAll = false;

            if (!_isRecipeExist || _sRecipe.Equals("*"))
            {
                isRecipeAll = true;
            }

            if (_isStepExist && !isRecipeAll)
            {
                _isAllExist = true;
            }

            InitializeButtonList();
            InitializeSpread();

            if (!_isSelectedRawChange)
            {
                bsprResult.DataSet = _dsSummaryOptionResult;

                for (int i = 0; i < this.bsprResult.ActiveSheet.RowCount; i++)
                {
                    this.bsprResult.ActiveSheet.Cells[i, 0].Locked = false;
                }
            }

            LoadEqpChamberData();

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void InitializeButtonList()
        {
            _Initialization.InitializeButtonList(this.bblstInsert, Definition.PAGE_KEY_SPC_SUMMARYDATA_OPTION_POPUP, Definition.ButtonKey.SUMMARY_INSERT, _sessionData);
            _Initialization.InitializeButtonList(this.bblstResult, Definition.PAGE_KEY_SPC_SUMMARYDATA_OPTION_POPUP, Definition.ButtonKey.SUMMARY_RESULT, _sessionData);
        }

        private void InitializeSpread()
        {
            this.bsprChamber.ClearHead();
            this.bsprChamber.AddHead(0, Definition.COL_SELECT, Definition.COL_SELECT, 50, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);
            this.bsprChamber.AddHead(1, Definition.COL_EQP_ID, Definition.COL_EQP_ID, 100, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bsprChamber.AddHead(2, Definition.COL_CHAMBER, Definition.COL_CHAMBER, 100, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bsprChamber.AddHeadComplete();

            this.bsprRecipe.UseCheckAll = true;
            this.bsprRecipe.ClearHead();
            this.bsprRecipe.AddHead(0, Definition.COL_SELECT, Definition.COL_SELECT, 65, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);
            this.bsprRecipe.AddHead(1, Definition.COL_RECIPE, Definition.COL_RECIPE, 80, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bsprRecipe.AddHead(2, Definition.COL_STEP, Definition.COL_STEP, 80, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bsprRecipe.AddHeadComplete();
            this.bsprRecipe.ActiveSheet.ColumnHeader.Columns[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;

            this.bsprResult.UseCheckAll = true;
            this.bsprResult.ClearHead();
            this.bsprResult.AddHead(0, Definition.COL_SELECT, Definition.COL_SELECT, 65, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);
            this.bsprResult.AddHead(1, Definition.COL_EQP_ID, Definition.COL_EQP_ID, 100, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bsprResult.AddHead(2, Definition.COL_CHAMBER, Definition.COL_CHAMBER, 100, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bsprResult.AddHead(3, Definition.COL_RECIPE, Definition.COL_RECIPE, 80, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bsprResult.AddHead(4, Definition.COL_STEP, Definition.COL_STEP, 80, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bsprResult.AddHead(5, Definition.COL_EQP_MODULE_ID, Definition.COL_EQP_MODULE_ID, 50, 50, null, null,
                null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
            this.bsprResult.AddHeadComplete();
            this.bsprResult.ActiveSheet.ColumnHeader.Columns[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;

            this.bsprResult.ContextMenu = new ContextMenu();
            this.bsprResult.ContextMenu.MenuItems.Add(this.bsprResult.ContextMenu.MenuItems.Count,
                new MenuItem(Definition.CONTEXT_MENU_REMOVE, bblstResult_ContextMenuClick));
        }

        private void LoadEqpChamberData()
        {
            this.bsprChamber.DataSet = _dsEqpModules;

            for (int i = 0; i < this.bsprChamber.ActiveSheet.RowCount; i++)
            {
                this.bsprChamber.ActiveSheet.Cells[i, 0].Locked = false;
            }
        }

        private void LoadRecipeStepData(DataSet dsRecipeStep)
        {
            this.bsprRecipe.DataSet = dsRecipeStep;

            for (int i = 0; i < this.bsprRecipe.ActiveSheet.RowCount; i++)
            {
                this.bsprRecipe.ActiveSheet.Cells[i, 0].Locked = false;
            }
        }

        private void RemoveResultItem()
        {
            if (bsprResult.GetCheckedList((int)iResultColIdx.SELECT).Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROWS", null, null);
                return;
            }

            DataSet dsTemp = ((DataSet)bsprResult.DataSet).Copy();
            DataTable dtTemp = dsTemp.Tables[0];
            ArrayList alRemovedIndex = new ArrayList();

            for (int i = 0; i < bsprResult.ActiveSheet.RowCount; i++)
            {
                if (this.bsprResult.ActiveSheet.Cells[i, (int)iChamberColIdx.SELECT].Text == "True")
                {
                    DataRow drTemp = dtTemp.Rows[i];
                    alRemovedIndex.Add(drTemp);
                }
            }

            for (int i = 0; i < alRemovedIndex.Count; i++)
            {
                dtTemp.Rows.Remove((DataRow)alRemovedIndex[i]);
            }

            bsprResult.DataSet = dsTemp;

            for (int i = 0; i < this.bsprResult.ActiveSheet.RowCount; i++)
            {
                this.bsprResult.ActiveSheet.Cells[i, 0].Locked = false;
            }
        }

        #endregion


        #region #Event

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            if (this.bsprResult.ActiveSheet.RowCount <= 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_DATA_SUMMARY_OPTION", null, null);
                return;
            }

            //원래 form에 보내주는 값
            _dsSummaryOptionResult = (DataSet)bsprResult.DataSet;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bsprChamber_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != (int)iChamberColIdx.SELECT)
            {
                return;
            }

            if ((bool)this.bsprChamber.GetCellValue(e.Row, (int)iChamberColIdx.SELECT) == true)
            {
                for (int i = 0; i < this.bsprChamber.ActiveSheet.RowCount; i++)
                {
                    if (e.Row == i)
                    {
                        if (e.Column == (int)iChamberColIdx.SELECT)
                        {
                            this.bsprChamber.ActiveSheet.Cells[i, (int)iChamberColIdx.SELECT].Value = 1;

                            LinkedList llparam = new LinkedList();

                            if (_isAllExist)
                            {
                                llparam.Add(Definition.SUMMARY_DATA.DATA_EXIST_MODE, Definition.SUMMARY_DATA.ALL_EXIST);
                            }
                            else if (_isRecipeExist && !_sRecipe.Equals("*"))
                            {
                                llparam.Add(Definition.SUMMARY_DATA.DATA_EXIST_MODE, Definition.SUMMARY_DATA.ONLY_RECIPE_EXIST);
                            }
                            else if (_isStepExist)
                            {
                                llparam.Add(Definition.SUMMARY_DATA.DATA_EXIST_MODE, Definition.SUMMARY_DATA.ONLY_STEP_EXIST);
                            }

                            string eqpModuleId = ((DataSet)bsprChamber.DataSet).Tables[0].Rows[i][Definition.COL_EQP_MODULE_ID].ToString();

                            llparam.Add(Definition.COL_EQP_MODULE_ID, eqpModuleId);

                            byte[] badata = llparam.GetSerialData();
                            DataSet dsRecipeStep = _wsSPC.GetRecipeStepByEqpModuleId(badata);

                            LoadRecipeStepData(dsRecipeStep);

                            _sTempModuleid = eqpModuleId;
                        }

                        continue;
                    }

                    this.bsprChamber.ActiveSheet.Cells[i, (int)iChamberColIdx.SELECT].Value = 0;
                }

                this.bsprRecipe.CheckBoxManager(0, false);
                this.bsprRecipe.CheckBoxManager(0, true);
            }
            else
            {
                ((DataSet)bsprRecipe.DataSet).Tables[0].Rows.Clear();
            }

            _iSelectedEqpChamberIndex = e.Row;
        }

        private void bsprRecipe_MouseDown(object sender, MouseEventArgs e)
        {
            _slSelectRecipe = this.bsprRecipe.GetSelectedRows();
        }

        private void bsprRecipe_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (_slSelectRecipe.Count > 0)
            {
                if ((bool)this.bsprRecipe.GetCellValue(e.Row, (int)iRecipeColIdx.SELECT))
                {
                    for (int i = 0; i < _slSelectRecipe.Count; i++)
                    {
                        this.bsprRecipe.ActiveSheet.SetText((int)_slSelectRecipe.GetByIndex(i), (int)iRecipeColIdx.SELECT, "True");
                    }
                }
                else
                {
                    for (int i = 0; i < _slSelectRecipe.Count; i++)
                    {
                        this.bsprRecipe.ActiveSheet.SetText((int)_slSelectRecipe.GetByIndex(i), (int)iRecipeColIdx.SELECT, "False");
                    }
                }
            }

            _slSelectRecipe.Clear();
        }

        private void bblstInsert_Click(object sender, EventArgs e)
        {
            if (bsprChamber.GetCheckedList((int)iChamberColIdx.SELECT).Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_SELETED_CHAMBER", null, null);
                return;
            }

            DataSet dsResult = (DataSet)bsprResult.DataSet;
            DataRow newRow = null;
            int resultCount = dsResult.Tables[0].Rows.Count;

            ArrayList alDuplicateCheck = new ArrayList();

            for (int i = 0; i < resultCount; i++)
            {
                DataRow dr = dsResult.Tables[0].Rows[i];
                string key = dr[Definition.COL_EQP_ID].ToString() + dr[Definition.COL_CHAMBER].ToString() +
                    dr[Definition.COL_RECIPE].ToString() + dr[Definition.COL_STEP].ToString();

                alDuplicateCheck.Add(key);
            }

            int eqpChamberCount = this.bsprChamber.ActiveSheet.RowCount;
            int recipeStepCount = this.bsprRecipe.ActiveSheet.RowCount;
            string eqpId = string.Empty;
            string chamber = string.Empty;

            for (int i = 0; i < eqpChamberCount; i++)
            {
                if (this.bsprChamber.ActiveSheet.Cells[i, (int)iChamberColIdx.SELECT].Text == "True")
                {
                    eqpId = this.bsprChamber.ActiveSheet.Cells[i, (int)iChamberColIdx.EQP_ID].Text;
                    chamber = this.bsprChamber.ActiveSheet.Cells[i, (int)iChamberColIdx.CHAMBER].Text;

                    break;
                }
            }

            for (int i = 0; i < recipeStepCount; i++)
            {
                if (this.bsprRecipe.ActiveSheet.Cells[i, (int)iRecipeColIdx.SELECT].Text == "True")
                {
                    string recipe = this.bsprRecipe.ActiveSheet.Cells[i, (int)iRecipeColIdx.RECIPE_ID].Text;
                    string step = this.bsprRecipe.ActiveSheet.Cells[i, (int)iRecipeColIdx.STEP].Text;

                    string key = eqpId + chamber + recipe + step;

                    if (alDuplicateCheck.Contains(key))
                    {
                        continue;
                    }

                    newRow = dsResult.Tables[0].NewRow();
                    newRow[Definition.COL_SELECT] = false;
                    newRow[Definition.COL_EQP_ID] = eqpId;
                    newRow[Definition.COL_CHAMBER] = chamber;
                    newRow[Definition.COL_RECIPE] = recipe;
                    newRow[Definition.COL_STEP] = step;
                    newRow[Definition.COL_EQP_MODULE_ID] = _sTempModuleid;

                    dsResult.Tables[0].Rows.Add(newRow);

                    alDuplicateCheck.Add(key);
                }
            }

            bsprResult.DataSet = dsResult;

            for (int i = 0; i < this.bsprResult.ActiveSheet.RowCount; i++)
            {
                this.bsprResult.ActiveSheet.Cells[i, 0].Locked = false;
            }

            this.bsprResult.CheckBoxManager(0, false);
            this.bsprResult.CheckBoxManager(0, true);
        }

        private void bsprResult_MouseDown(object sender, MouseEventArgs e)
        {
            _slSelectResult = this.bsprResult.GetSelectedRows();
        }

        private void bsprResult_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (_slSelectResult.Count > 0)
            {
                if ((bool)this.bsprResult.GetCellValue(e.Row, (int)iResultColIdx.SELECT))
                {
                    for (int i = 0; i < _slSelectResult.Count; i++)
                    {
                        this.bsprResult.ActiveSheet.SetText((int)_slSelectResult.GetByIndex(i), (int)iResultColIdx.SELECT, "True");
                    }
                }
                else
                {
                    for (int i = 0; i < _slSelectResult.Count; i++)
                    {
                        this.bsprResult.ActiveSheet.SetText((int)_slSelectResult.GetByIndex(i), (int)iResultColIdx.SELECT, "False");
                    }
                }
            }

            _slSelectResult.Clear();
        }

        private void bblstResult_ButtonClick(string name)
        {
            if (name.ToUpper().Equals(Definition.BUTTON_KEY_REMOVE))
            {
                RemoveResultItem();
            }
        }

        private void bblstResult_ContextMenuClick(object sender, EventArgs e)
        {
            RemoveResultItem();
        }

        #endregion
    }
}
