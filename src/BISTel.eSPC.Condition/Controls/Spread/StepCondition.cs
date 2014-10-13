using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.eSPCWebService;
using BISTel.PeakPerformance.Client.CommonLibrary;
using System.Collections;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Condition.Controls.Interface;

namespace BISTel.eSPC.Condition.Controls.Spread
{
    public partial class StepCondition : ControlInterface
    {
        #region : Field

        public DataTable _dtStep = null;
        public ConditionType spType = ConditionType.STEP;
        private eSPCWebService.eSPCWebService _fdcWebService = null;

        private Initialization _Initialization;
        private SortedList _slColumnIndex = new SortedList();
        private bool _bSingleSelect = false;
        private DataSet _dsAllStep = new DataSet();

        int _iRawidColIndex = 0;

        #endregion

        #region : Initialization

        public StepCondition()
        {
            InitializeComponent();
            InitializePage();

            this._fdcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            _dtStep = new DataTable();
        }

        public void InitializePage()
        {
            //Temp Data. Container에 올리면 Container가 Configuration을 Initialize하기 때문에 이 호출 없이도 사용 가능.
            //mwkim에 의한 확인 2008.08.20
            //-------------------------------------------------------------------------------------------
            SessionData sessionData = base.GetSession();
            BasePageUCtrl basepageuntil = new BasePageUCtrl();
            basepageuntil.PageInfo(sessionData.GetXml(), " ", Configuration.getInstance().URL, "80", "", "", "", "");
            //-------------------------------------------------------------------------------------------
            this.InitializeVariable();
            this.InitializeBSpread();
        }

        public void InitializeVariable()
        {
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
        }

        public void InitializeBSpread()
        {
            this._slColumnIndex = new SortedList();

            this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprStep, Definition.PAGE_KEY_SERACH_CONDITION, false, Definition.PAGE_KEY_SERACH_CONDITION_STEP_LIST);
            this.bsprStep.UseHeadColor = true;

            if (!_bSingleSelect)
                this._Initialization.SetCheckColumnHeader(this.bsprStep, 0);

            string sRawidColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_RAWID);
            this._iRawidColIndex = (int)this._slColumnIndex[sRawidColName];
        }

        public void SetSpreadType(ConditionType type, string[] column)
        {
            st_step.Columns.Add(1, column.Length);

            for (int i = 0; i < column.Length; i++)
            {
                st_step.Columns[i + 1].Label = column[i];
            }

            spType = type;
        }

        #endregion

        #region : Public

        public void GetStepValue(object Info, bool bSpecExist)
        {
            LinkedList condition = (LinkedList)Info;

            if (condition[Definition.DynamicCondition_Search_key.RECIPE] == null) return;

            ResetCondition();

            LinkedList stepList = new LinkedList();

            DataTable dtRecipe = (DataTable)condition[Definition.DynamicCondition_Search_key.RECIPE];
            if (dtRecipe != null)
            {
                ArrayList recipeRawidList = new ArrayList();
                for (int i = 0; i < dtRecipe.Rows.Count; i++)
                {
                    string rawid = dtRecipe.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    recipeRawidList.Add(rawid);
                }
                stepList.Add(Definition.CONDITION_KEY_RECIPE_LIST_RAWID, recipeRawidList);
            }

            DataTable dtModule = (DataTable)condition[Definition.DynamicCondition_Search_key.MODULE];
            if (dtModule != null)
            {
                ArrayList rawidList = new ArrayList();
                for (int i = 0; i < dtModule.Rows.Count; i++)
                {
                    string rawid = dtModule.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    rawidList.Add(rawid);
                }
                stepList.Add(Definition.CONDITION_KEY_EQP_RAWID_LIST, rawidList);
            }

            if (bSpecExist)
                stepList.Add(Definition.CONDITION_KEY_SPEC_EXIST, bSpecExist);

            byte[] btdata = stepList.GetSerialData();

            DataSet ds = _fdcWebService.GetRecipeStep(btdata);
            _dsAllStep = ds;

            if (bSpecExist)
            {
                DataSet dsAll = _fdcWebService.GetAllStep(btdata);

                if (dsAll != null && dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
                {
                    ds = RefreshCondition(ds);
                }

                ApplySpecCondition(ds);
            }

            if (ds != null)
            {
                _dtStep = ds.Tables[0];

                //bsprStep.SetDataSource(ds);
                bsprStep.DataSet = ds;

                //FarPoint.Win.Spread.CellType.ICellType ct_Test_2 = this.bsprStep.ActiveSheet.Cells[0, (int)ParamColumn.SELECT].CellType;
            }
        }

        //public void GetStepValue(object Info, bool bSpecExist)
        //{
        //    DefaultInfo condition = (DefaultInfo)Info;

        //    if (condition._alRecipeList == null && condition._sRecipeID == string.Empty) return;

        //    ResetCondition();

        //    //FarPoint.Win.Spread.CellType.ICellType ct_Test = this.bsprStep.ActiveSheet.Cells[0, (int)ParamColumn.SELECT].CellType;

        //    LinkedList stepList = new LinkedList();

        //    if (condition._alRecipeList != null)
        //    {
        //        ArrayList recipeRawidList = new ArrayList();
        //        if (condition._alRecipeList != null)
        //        {
        //            int count = condition._alRecipeList.Count;
        //            for (int i = 0; i < count; i++)
        //            {
        //                string recipeRawid = (string)condition._htRecipeRawid[condition._alRecipeList[i].ToString()];
        //                recipeRawidList.Add(recipeRawid);
        //            }
        //        }

        //        //string recipeRawID = (string)condition._htRecipeRawid[condition._sArrRecipe];

        //        stepList.Add(Definition.CONDITION_KEY_RECIPE_LIST_RAWID, recipeRawidList);
        //    }
        //    else
        //    {
        //        if (condition._htRecipeRawid[condition._sRecipeID] == null)
        //            return;

        //        string recipeRawid = (string)condition._htRecipeRawid[condition._sRecipeID];
        //        stepList.Add(Definition.CONDITION_KEY_RECIPE_RAWID, recipeRawid);
        //    }

        //    if (condition._sModuleID != string.Empty)
        //    {
        //        if (condition._htModuleRawid[condition._sModuleID] != null)
        //        {
        //            string eqpRawid = condition._htModuleRawid[condition._sModuleID].ToString();
        //            stepList.Add(Definition.CONDITION_KEY_EQP_RAWID, eqpRawid);
        //        }
        //    }

        //    if (bSpecExist)
        //        stepList.Add(Definition.CONDITION_KEY_SPEC_EXIST, bSpecExist);
        //    byte[] btdata = stepList.GetSerialData();

        //    DataSet ds = _fdcWebService.GetRecipeStep(btdata);
        //    _dsAllStep = ds;

        //    if (bSpecExist)
        //    {
        //        DataSet dsAll = _fdcWebService.GetAllStep(btdata);

        //        if (dsAll != null && dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
        //        {
        //            ds = RefreshCondition(ds);
        //        }

        //        ApplySpecCondition(ds);
        //    }

        //    if (ds != null)
        //    {
        //        _dtStep = ds.Tables[0];

        //        //bsprStep.SetDataSource(ds);
        //        bsprStep.DataSet = ds;

        //        //FarPoint.Win.Spread.CellType.ICellType ct_Test_2 = this.bsprStep.ActiveSheet.Cells[0, (int)ParamColumn.SELECT].CellType;
        //    }
        //}

        public void GetDistinctStep(object Info)
        {
            LinkedList condition = (LinkedList)Info;

            if (condition[Definition.DynamicCondition_Search_key.RECIPE] == null) return;

            ResetCondition();

            LinkedList stepList = new LinkedList();

            DataTable dtRecipe = (DataTable)condition[Definition.DynamicCondition_Search_key.RECIPE];
            if (dtRecipe != null)
            {
                ArrayList recipeRawidList = new ArrayList();
                for (int i = 0; i < dtRecipe.Rows.Count; i++)
                {
                    string rawid = dtRecipe.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    recipeRawidList.Add(rawid);
                }
                stepList.Add(Definition.CONDITION_KEY_RECIPE_LIST_RAWID, recipeRawidList);
            }

            byte[] btdata = stepList.GetSerialData();

            DataSet ds = _fdcWebService.GetRecipeStep(btdata);

            if (ds != null)
            {
                _dsAllStep = ds;
                _dtStep = ds.Tables[0];

                //bsprStep.SetDataSource(ds);
                bsprStep.DataSet = ds;
            }

            DataSet dsDistinct = new DataSet();
            ArrayList alStepList = new ArrayList();
            if (ds != null && ds.Tables.Count > 0)
            {
                dsDistinct.Tables.Add(ds.Tables[0].Clone());
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];

                    string step = ds.Tables[0].Rows[i][Definition.CONDITION_KEY_STEP_ID].ToString();
                    if (!alStepList.Contains(step))
                    {
                        alStepList.Add(step);
                        dsDistinct.Tables[0].Rows.Add(ds.Tables[0].Rows[i].ItemArray);
                    }

                    bsprStep.SetCellText(i, 0, "False");
                }

                //bsprStep.SetDataSource(dsDistinct);
                bsprStep.DataSet = dsDistinct;
            }
        }

        //public void GetDistinctStep(object Info)
        //{
        //    DefaultInfo condition = (DefaultInfo)Info;

        //    if (condition._alRecipeList == null && condition._sRecipeID == string.Empty) return;

        //    ResetCondition();

        //    LinkedList stepList = new LinkedList();

        //    if (condition._alRecipeList != null)
        //    {
        //        ArrayList recipeRawidList = new ArrayList();
        //        if (condition._alRecipeList != null)
        //        {
        //            int count = condition._alRecipeList.Count;
        //            for (int i = 0; i < count; i++)
        //            {
        //                string recipeRawid = (string)condition._htRecipeRawid[condition._alRecipeList[i].ToString()];
        //                recipeRawidList.Add(recipeRawid);
        //            }
        //        }

        //        stepList.Add(Definition.CONDITION_KEY_RECIPE_LIST_RAWID, recipeRawidList);
        //    }
        //    else
        //    {
        //        string recipeRawid = (string)condition._htRecipeRawid[condition._sRecipeID];
        //        stepList.Add(Definition.CONDITION_KEY_RECIPE_RAWID, recipeRawid);
        //    }

        //    byte[] btdata = stepList.GetSerialData();

        //    DataSet ds = _fdcWebService.GetRecipeStep(btdata);

        //    if (ds != null)
        //    {
        //        _dsAllStep = ds;
        //        _dtStep = ds.Tables[0];

        //        //bsprStep.SetDataSource(ds);
        //        bsprStep.DataSet = ds;
        //    }

        //    DataSet dsDistinct = new DataSet();
        //    ArrayList alStepList = new ArrayList();
        //    if (ds != null && ds.Tables.Count > 0)
        //    {
        //        dsDistinct.Tables.Add(ds.Tables[0].Clone());
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            DataRow row = ds.Tables[0].Rows[i];

        //            string step = ds.Tables[0].Rows[i][Definition.CONDITION_KEY_STEP_ID].ToString();
        //            if (!alStepList.Contains(step))
        //            {
        //                alStepList.Add(step);
        //                dsDistinct.Tables[0].Rows.Add(ds.Tables[0].Rows[i].ItemArray);
        //            }

        //            bsprStep.SetCellText(i, 0, "False");
        //        }

        //        //bsprStep.SetDataSource(dsDistinct);
        //        bsprStep.DataSet = dsDistinct;
        //    }
        //}

        public LinkedList GetSelectedValue()
        {
            LinkedList list = new LinkedList();

            DataTable dtList = _dtStep.Clone();

            DataSet ds = (DataSet)bsprStep.DataSet;
            _dtStep = ds.Tables[0];

            ArrayList alCheckedList = bsprStep.GetCheckedList(0);

            for (int i = 0; i < alCheckedList.Count; i++)
            {
                int iIndex = Convert.ToInt32(alCheckedList[i]);
                if (bsprStep.GetCellValue(iIndex, 0) != null)
                {
                    string check = bsprStep.GetCellValue(iIndex, 0).ToString();
                    if (check.ToUpper() == "TRUE")
                    {
                        string sRawID = bsprStep.ActiveSheet.Cells[iIndex, _iRawidColIndex].Text;
                        string sFilter = "RAWID = '" + sRawID + "' ";

                        DataRow[] drs = ds.Tables[0].Select(sFilter);
                        list.Add(sRawID, drs[0]);
                    }
                }
            }
            return list;
        }

        //public DataSet GetSelectedValueToDataSet()
        //{
        //    DataSet dsStep = new DataSet(Definition.CONDITION_KEY_STEP_NAME);
        //    if (_dtStep != null)
        //    {
        //        DataTable dtStep = _dtStep.Copy();
        //        dtStep.Rows.Clear();

        //        LinkedList list = new LinkedList();
        //        for (int i = 0; i < st_step.Rows.Count; i++)
        //        {
        //            if (bsprStep.GetCellValue(i, 0) != null)
        //            {
        //                string check = bsprStep.GetCellValue(i, 0).ToString();
        //                if (check.ToUpper() == ("true").ToUpper())
        //                {
        //                    dtStep.Rows.Add(_dtStep.Rows[i].ItemArray);
        //                }
        //            }
        //        }
        //        dsStep.Tables.Add(dtStep);
        //    }
        //    return dsStep;
        //}
        public DataSet GetSelectedValueToDataSet()
        {
            BSpreadUtility utility = new BSpreadUtility();
            DataSet ds = utility.GetSelectedDataSet(bsprStep, 0);

            ds.Tables[0].Columns[Definition.CONDITION_KEY_STEP_ID].ColumnName = Definition.DynamicCondition_Search_key.DISPLAYDATA;
            ds.Tables[0].Columns[Definition.CONDITION_KEY_RAWID].ColumnName = Definition.DynamicCondition_Search_key.VALUEDATA;

            return ds;
        }

        public void SetValue(DataSet ds)
        {
            //ResetCondition();

            this.bsprStep.DataSet = ds;
        }

        public DataSet RefreshCondition(DataSet ds)
        {
            DataSet dsNew = new DataSet();
            LinkedList llStepList = new LinkedList();

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dtNew = ds.Tables[0].Copy();

                DataRow dr = dtNew.NewRow();
                dr[Definition.CONDITION_KEY_SPEC_EXIST] = Definition.VARIABLE_Y;
                dr[Definition.CONDITION_KEY_STEP_NAME] = Definition.VARIABLE_ALL;
                dr[Definition.CONDITION_KEY_STEP_ID] = Definition.VARIABLE_ALL;
                dr[Definition.CONDITION_KEY_RAWID] = Definition.VARIABLE_ALL;
                dtNew.Rows.Add(dr);

                dsNew.Tables.Add(dtNew);
                bsprStep.DataSet = dsNew;
                _dtStep = dsNew.Tables[0];
            }

            return dsNew;
        }

        public void ApplySpecCondition(DataSet ds)
        {
            //DataSet ds = (DataSet)bsprStep.DataSet;
            bsprStep.DataSet = ds;
            _dtStep = ds.Tables[0];

            if (ds != null && ds.Tables.Count > 0)
            {
                int iSelectedCount = 0;

                _dtStep = ds.Tables[0];

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];

                    if (row[Definition.CONDITION_KEY_SPEC_EXIST].ToString() == "Y")
                    {
                        bsprStep.SetCellText(i, 0, "True");
                        //bsprStep.SetCellValue(i, (int)StepColumn.SELECT, "True");
                        st_step.Cells[i, 0].Locked = true;
                        iSelectedCount++;
                    }
                    else
                        bsprStep.SetCellText(i, 0, "False");
                }

                if (iSelectedCount == ds.Tables[0].Rows.Count)
                {
                    bsprStep.ActiveSheet.ColumnHeader.Cells[0, 0].Value = "True";
                    bsprStep.ActiveSheet.ColumnHeader.Cells[0, 0].Locked = true;

                }
            }
        }

        public void SetSingleSelection(bool bSelect)
        {
            _bSingleSelect = bSelect;

            if (bSelect)
            {
                this._Initialization.SetTextColumnHeader(bsprStep, 0);
            }
            else
            {
                this._Initialization.SetCheckColumnHeader(bsprStep, 0);
            }
        }

        public void ResetCondition()
        {
            _dtStep.Clear();

            //if (bsprStep.RowCount() > 0)
            //{
            DataSet ds = (DataSet)bsprStep.DataSet;
            bsprStep.DataSet = ds.Clone();
            //}
        }

        public DataSet GetAllValue()
        {
            //return bsprParam.GetDataSource();
            return _dsAllStep;
        }

        #endregion

        #region : Event Handling

        private void bsprStep_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == bsprStep.ActiveSheet.ActiveColumn.Index)
            {
                if (e.Column == 0)
                {
                    if (_bSingleSelect)
                    {
                        ArrayList alCheckedList = bsprStep.GetCheckedList(0);
                        if (alCheckedList.Count > 0)
                        {
                            for (int i = 0; i < bsprStep.RowCount(); i++)
                            {
                                bsprStep.SetCellValue(i, 0, "0");
                            }
                            bsprStep.SetCellValue(e.Row, 0, "True");
                        }
                    }
                }
            }
        }

        private void bsprStep_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (!_bSingleSelect)
            {
                if (this.bsprStep.ActiveSheet.RowCount > 0)
                {
                    if (e.ColumnHeader)
                    {
                        if (e.Column == 0)
                        {
                            FarPoint.Win.Spread.CellType.ICellType ct = this.bsprStep.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].CellType;
                            if (ct is FarPoint.Win.Spread.CellType.CheckBoxCellType)
                            {
                                this._Initialization.SetCheckColumnHeaderStatus(this.bsprStep, 0);
                            }
                        }
                    }
                }
            }
        }

        private void ParameterCondition_Load(object sender, EventArgs e)
        {
            InitializeBSpread();
        }

        #endregion

    }
}
