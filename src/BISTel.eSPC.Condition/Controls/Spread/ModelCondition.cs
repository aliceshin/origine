using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.eSPCWebService;
using System.Collections;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Condition.Controls.Default;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Condition.Controls.Interface;

namespace BISTel.eSPC.Condition.Controls.Spread
{
    public partial class ModelCondition : ControlInterface
    {
        #region : Field

        public DataTable _dtParam = null;
        public ParameterType spType;
        private eSPCWebService.eSPCWebService _spcWebService = null;

        private Initialization _Initialization;
        private SortedList _slColumnIndex = new SortedList();
        private bool _bSingleSelect = false;

        private DataSet _dsOriginParam = new DataSet();
        private BSpreadUtility _bSpread;

        int _iRawidColIndex = 0;
        int _iParamNameColIndex = 0;
        int _iSpecExistColIndex = 0;
        int _iModuleNameColIndex = 0;

        #endregion

        #region : Editor

        [Editor(typeof(ParameterType), typeof(ParameterType))]
        public ParameterType ParamType
        {
            get { return spType; }
            set { spType = value; }
        }

        #endregion

        #region : Initialization

        public ModelCondition()
        {
            InitializeComponent();
            InitializePage();
            if (!DesignMode)
            {
                //Configuration.getInstance("http://localhost/ees" + "/Configuration/" + EESConstants.CONFIGURATION_FILE).URL = "http://localhost/ees";

                this._spcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
                _dtParam = new DataTable();
            }
        }

        public void InitializePage()
        {
            //Temp Data. Container에 올리면 Container가 Configuration을 Initialize하기 때문에 이 호출 없이도 사용 가능.
            //mwkim에 의한 확인 2008.08.20
            //-------------------------------------------------------------------------------------------
            SessionData sessionData = base.GetSession();
            BasePageUCtrl basepageuntil = new BasePageUCtrl();
            basepageuntil.PageInfo(sessionData.GetXml(), " ", "http://localhost/ees", "80", "", "", "", "");
            //-------------------------------------------------------------------------------------------
            this.InitializeVariable();
            this.InitializeBSpread();
        }

        public void InitializeVariable()
        {
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
            this._bSpread = new BSpreadUtility();
        }

        public void InitializeBSpread()
        {
            this._slColumnIndex = new SortedList();
            if (this.ParamType == ParameterType.TRACE)
            {
                this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprParam, Definition.PAGE_KEY_SERACH_CONDITION, false, Definition.PAGE_KEY_SERACH_CONDITION_MODELING);

                string sRawidColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_RAWID);
                this._iRawidColIndex = (int)this._slColumnIndex[sRawidColName];

                string sParamNameColName = this._mlthandler.GetVariable(Definition.CONDITION_KEY_MODEL_NAME);
                this._iParamNameColIndex = (int)this._slColumnIndex[sParamNameColName];

                //string sSpecExistColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_SPEC_EXIST);
                //this._iSpecExistColIndex = (int)this._slColumnIndex[sSpecExistColName];

                //string sModuleNameColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_MODULE_NAME);
                //this._iModuleNameColIndex = (int)this._slColumnIndex[sModuleNameColName];
            }

            this.bsprParam.UseHeadColor = true;

            if (!_bSingleSelect)
                this._Initialization.SetCheckColumnHeader(this.bsprParam, 0);

        }

        public void SetSpreadType(ParameterType type, string[] column)
        {
            st_param.Columns.Add(1, column.Length);

            for (int i = 0; i < column.Length; i++)
            {
                st_param.Columns[i + 1].Label = column[i];
            }

            spType = type;
        }

        #endregion

        #region : Public

        public void GetParameterValue(object llst, bool bSpecExist)
        {
            LinkedList condition = (LinkedList)llst;
            LinkedList paramList = new LinkedList();

            if (condition[Definition.DynamicCondition_Search_key.EQP_ID] == null) return;

            ResetCondition();

            paramList.Add(Definition.CONDITION_KEY_TYPE, spType);

            DataTable dtEqp = (DataTable)condition[Definition.DynamicCondition_Search_key.EQP_ID];
            DataTable dtModule = (DataTable)condition[Definition.DynamicCondition_Search_key.MODULE];

            ArrayList alRawidList = new ArrayList();
            if (dtModule == null)
            {
                for (int i = 0; i < dtEqp.Rows.Count; i++)
                {
                    string rawid = dtEqp.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    alRawidList.Add(rawid);
                }
                paramList.Add(Definition.CONDITION_KEY_EQP_RAWID_LIST, alRawidList);

            }
            else
            {
                for (int i = 0; i < dtModule.Rows.Count; i++)
                {
                    string rawid = dtModule.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    alRawidList.Add(rawid);
                }
                paramList.Add(Definition.CONDITION_KEY_MODULE_RAWID_LIST, alRawidList);
            }

            if (bSpecExist)
                paramList.Add(Definition.CONDITION_KEY_SPEC_EXIST, bSpecExist);

            //Recipe
            DataTable dtRecipe = (DataTable)condition[Definition.DynamicCondition_Search_key.RECIPE];
            if (dtRecipe != null)
            {
                for (int i = 0; i < dtRecipe.Rows.Count; i++)
                {
                    string rawid = dtRecipe.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    alRawidList.Add(rawid);
                }
                paramList.Add(Definition.CONDITION_KEY_RECIPE_LIST_RAWID, alRawidList);
            }

            byte[] btdata = paramList.GetSerialData();

            DataSet ds = _spcWebService.GetParameter(btdata);

            bsprParam.SetDataSource(ds);
            _dsOriginParam = ds;

            if (ds != null && ds.Tables.Count > 0)
            {
                _dtParam = ds.Tables[0];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    if (bSpecExist)
                    {
                        if (row[Definition.CONDITION_KEY_SPEC_EXIST].ToString() == "O")
                        {
                            bsprParam.SetCellText(i, _iSpecExistColIndex, "◎");
                            bsprParam.SetCellText(i, 0, "True");
                            //st_param.Cells[i, (int)ParamColumn.SELECT].Locked = true;
                        }
                    }
                    else
                        bsprParam.SetCellText(i, 0, "False");
                }
                bsprParam.ActiveSheet.Columns[_iSpecExistColIndex].ForeColor = Color.Red;
            }
        }


        public void GetDistinctParameter(object Info)
        {
            LinkedList condition = (LinkedList)Info;
            LinkedList paramList = new LinkedList();

            if (condition[Definition.DynamicCondition_Search_key.EQP_ID] == null) return;

            ResetCondition();

            paramList.Add(Definition.CONDITION_KEY_TYPE, spType);



            DataTable dtEqp = (DataTable)condition[Definition.DynamicCondition_Search_key.EQP_ID];
            DataTable dtModule = (DataTable)condition[Definition.DynamicCondition_Search_key.MODULE];

            ArrayList alRawidList = new ArrayList();
            if (dtModule == null)
            {
                for (int i = 0; i < dtEqp.Rows.Count; i++)
                {
                    string rawid = dtEqp.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    alRawidList.Add(rawid);
                }
                paramList.Add(Definition.CONDITION_KEY_EQP_RAWID_LIST, alRawidList);

            }
            else
            {
                for (int i = 0; i < dtModule.Rows.Count; i++)
                {
                    string rawid = dtModule.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    alRawidList.Add(rawid);
                }
                paramList.Add(Definition.CONDITION_KEY_MODULE_RAWID_LIST, alRawidList);
            }

            //Recipe
            DataTable dtRecipe = (DataTable)condition[Definition.DynamicCondition_Search_key.RECIPE];
            if (dtRecipe != null)
            {
                for (int i = 0; i < dtRecipe.Rows.Count; i++)
                {
                    string rawid = dtRecipe.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    alRawidList.Add(rawid);
                }
                paramList.Add(Definition.CONDITION_KEY_RECIPE_LIST_RAWID, alRawidList);
            }

            byte[] btdata = paramList.GetSerialData();

            DataSet ds = _spcWebService.GetParameter(btdata);
            _dsOriginParam = ds;

            DataSet dsDistinct = new DataSet();

            ArrayList alParam = new ArrayList();
            if (ds != null && ds.Tables.Count > 0)
            {
                dsDistinct.Tables.Add(ds.Tables[0].Clone());
                _dtParam = dsDistinct.Tables[0];

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string param = ds.Tables[0].Rows[i][Definition.CONDITION_KEY_PARAM_NAME.ToString()].ToString();
                    if (!alParam.Contains(param))
                    {
                        alParam.Add(param);
                        dsDistinct.Tables[0].Rows.Add(ds.Tables[0].Rows[i].ItemArray);
                    }

                }
            }
            bsprParam.SetDataSource(dsDistinct);
        }


        public LinkedList GetSelectedValue()
        {
            LinkedList list = new LinkedList();

            DataTable dtList = _dtParam.Clone();

            DataSet ds = (DataSet)bsprParam.DataSet;
            _dtParam = ds.Tables[0];

            ArrayList alCheckedList = bsprParam.GetCheckedList(0);

            for (int i = 0; i < alCheckedList.Count; i++)
            {
                int iIndex = Convert.ToInt32(alCheckedList[i]);
                if (bsprParam.GetCellValue(iIndex, 0) != null)
                {
                    string check = bsprParam.GetCellValue(iIndex, 0).ToString();
                    if (check.ToUpper() == "TRUE")
                    {
                        string sRawID = bsprParam.ActiveSheet.Cells[iIndex, _iRawidColIndex].Text;
                        string sFilter = "RAWID = '" + sRawID + "' ";

                        DataRow[] drs = ds.Tables[0].Select(sFilter);
                        list.Add(sRawID, drs[0]);
                    }
                }
            }
            return list;
        }


        public DataSet GetSelectedValueToDataSet()
        {
            BSpreadUtility utility = new BSpreadUtility();
            DataSet ds = utility.GetSelectedDataSet(bsprParam, 0);

            ds.Tables[0].Columns[Definition.CONDITION_KEY_MODEL_NAME].ColumnName = Definition.DynamicCondition_Search_key.DISPLAYDATA;
            if (ds.Tables[0].Columns[Definition.CONDITION_KEY_RAWID] == null)
                ds.Tables[0].Columns.Add(Definition.CONDITION_KEY_RAWID, typeof(string));
            ds.Tables[0].Columns[Definition.CONDITION_KEY_RAWID].ColumnName = Definition.DynamicCondition_Search_key.VALUEDATA;

            return ds;
        }

        public DataSet GetAllValue()
        {
            return _dsOriginParam;
        }

        public void SetSingleSelection(bool bSelect)
        {
            _bSingleSelect = bSelect;

            if (bSelect)
            {
                this._Initialization.SetTextColumnHeader(bsprParam, 0);
            }
            else
            {
                this._Initialization.SetCheckColumnHeader(bsprParam, 0);
            }
        }


        public void UncheckSelect()
        {
            for (int i = 0; i < bsprParam.RowCount(); i++)
            {
                bsprParam.SetCellValue(i, 0, "0");
            }
        }

        public void ResetCondition()
        {
            _dtParam.Clear();

            //if (bsprParam.RowCount() > 0)
            //{
            DataSet ds = (DataSet)bsprParam.DataSet;
            bsprParam.DataSet = ds.Clone();
            //}

        }

        public void ShowModuleColumn(bool show)
        {
            if (_iModuleNameColIndex > 0)
            {
                bsprParam.SetColHidden(_iModuleNameColIndex, !show);
            }
        }

        #endregion

        #region : Private

        private bool IsDuplicatedParameter(string rawid)
        {
            for (int i = 0; i < bsprParam.RowCount(); i++)
            {
                string sRawID = bsprParam.GetCellValue(i, _iRawidColIndex).ToString();
                if (sRawID == rawid)
                    return true;
            }
            return false;
        }

        #endregion

        #region : Event Handling

        private void bsprParam_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column == bsprParam.ActiveSheet.ActiveColumn.Index)
            {
                if (e.Column == 0)
                {
                    if (_bSingleSelect)
                    {
                        ArrayList alCheckedList = bsprParam.GetCheckedList(0);
                        if (alCheckedList.Count > 0)
                        {
                            UncheckSelect();
                            bsprParam.SetCellValue(e.Row, 0, "True");
                        }
                        _parent.SendEventToParent(spType, ConditionType.PARAM);
                    }
                }
            }
        }


        private void bsprParam_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //int iSelectColIndex = (int)ParamColumn.SELECT;
            //if (e.ColumnHeader && (iSelectColIndex == e.Column))
            //{
            //    int iRowCount =  this.bsprParam.ActiveSheet.RowCount;
            //    ArrayList saChecked = this._bSpread.GetCheckedRowIndex(this.bsprParam, (int)ParamColumn.SELECT);
            //    if (saChecked.Count > 0)
            //    {
            //        this.bsprParam.ActiveSheet.Cells[0, (int)ParamColumn.SELECT, iRowCount - 1, (int)ParamColumn.SELECT].Value = "False";
            //    }
            //    else
            //    {
            //        this.bsprParam.ActiveSheet.Cells[0, (int)ParamColumn.SELECT, iRowCount - 1, (int)ParamColumn.SELECT].Value = "True";
            //    }

            //}
        }

        private void bsprParam_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (!_bSingleSelect)
            {
                if (this.bsprParam.ActiveSheet.RowCount > 0)
                {
                    if (e.ColumnHeader)
                    {
                        if (e.Column == 0)
                        {
                            FarPoint.Win.Spread.CellType.ICellType ct = this.bsprParam.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].CellType;
                            if (ct is FarPoint.Win.Spread.CellType.CheckBoxCellType)
                            {
                                this._Initialization.SetCheckColumnHeaderStatus(this.bsprParam, 0);
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
