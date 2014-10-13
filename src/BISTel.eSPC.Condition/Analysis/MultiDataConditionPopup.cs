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

namespace BISTel.eSPC.Condition.Analysis
{
    public partial class MultiDataConditionPopup : BasePopupFrm
    {
        #region : Constructor
        public MultiDataConditionPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region : Field
        Initialization _Initialization;
        BSpreadUtility _bspreadutility;
		CommonUtility _ComUtil;
        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _wsSPC;
        SessionData _SessionData;
        DataTable _dtParam = new DataTable();
        LinkedList _llstData = new LinkedList();

        DataSet _dsProbeOperation = null;


        bool _bProbe = false;
        string _sFab= string.Empty;
        string _sOperation = string.Empty;
        string _sOperationDesc = string.Empty;

        string _sLineRawID = string.Empty;
        string _sParamType = string.Empty;
        string _sParamItem = string.Empty;

        DialogResult _digResult = new DialogResult();
        #endregion


        #region Properties

        public bool bProbe
        {
            set { this._bProbe = value; }
            get { return this._bProbe; }
        }

        public string LineRawID
        {
            set { this._sLineRawID = value; }
            get { return this._sLineRawID; }
        }


        public DialogResult ButtonResult
        {
            get { return this._digResult; }
            set { this._digResult = value; }
        }


        public string Fab
        {
            set { this._sFab = value; }
            get { return this._sFab; }
        }


        public string PARAM_TYPE
        {
            set { this._sParamType = value; }
            get { return this._sParamType; }
        }


        public DataTable dtParam
        {
            set { this._dtParam = value; }
            get { return this._dtParam; }
        }

 
        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }
        #endregion



        #region : Initialization

        public void InitializePopup()
        {
            this.InitializeVariable();
            this.InitializeLayout();  

            this.InitializeBSpread(this.bSpreadStepMet);
            this.InitializeBSpread(this.bSpreadSorting);
            this.InitializeBSpread(this.bSpreadItem);
            this.InitializeBSpread(this.bSpreadSubData);
            this.InitializeBSpread(this.bSpreadSelect);

            this.InitializeCode();
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

        }

        public void InitializeLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.POPUP_TITLE_KEY_STEP_DATAMAPPING);
        }

        public void InitializeBSpread(BSpread bspread)
        {
            bspread.UseSpreadEdit = false;
            bspread.AutoGenerateColumns = false;
            bspread.ActiveSheet.DefaultStyle.ResetLocked();
            bspread.UseEdit = true;
            bspread.ClearHead();
            bspread.Locked = true;
            bspread.UseHeadColor = true;
            bspread.EditMode = false;
            bspread.ActiveSheet.RowCount = 0;
            bspread.ActiveSheet.ColumnCount = 0; 
        }

        public void InitializeCode()
        {
            //Select,Target,Step,Info,Item,SubData
            this.bSpreadSelect.ActiveSheet.RowCount = 0;
            this.bSpreadSelect.ActiveSheet.Columns.Count = 8;
            this.bSpreadSelect.AddHead(0, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.V_SELECT), "_SELECT", 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);
            this.bSpreadSelect.AddHead(1, Definition.STEP_DATAMAPPING.BASE, Definition.STEP_DATAMAPPING.BASE, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);
            this.bSpreadSelect.AddHead(2, Definition.CHART_COLUMN.OPERATION_ID, Definition.STEP_DATAMAPPING.OPERATION_ID, 100, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
            this.bSpreadSelect.AddHead(3, this._mlthandler.GetVariable(Definition.CHART_COLUMN.OPERATION_ID), Definition.STEP_DATAMAPPING.OPERATION_DESCRIPTION, 180, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bSpreadSelect.AddHead(4, Definition.STEP_DATAMAPPING.INFORMATION, Definition.STEP_DATAMAPPING.INFORMATION, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bSpreadSelect.AddHead(5, Definition.STEP_DATAMAPPING.ITEM, Definition.STEP_DATAMAPPING.ITEM, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bSpreadSelect.AddHead(6, Definition.STEP_DATAMAPPING.SUBDATA, Definition.STEP_DATAMAPPING.SUBDATA, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
            this.bSpreadSelect.AddHead(7, Definition.STEP_DATAMAPPING.PROBE, Definition.STEP_DATAMAPPING.PROBE, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
			this.bSpreadSelect.AddHead(8, Definition.STEP_DATAMAPPING.TYPE, Definition.STEP_DATAMAPPING.TYPE, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
            this.bSpreadSelect.AddHeadComplete();
            this.bSpreadSelect.DataSource = this.dtParam;
            
            //Probe 공정
            this.PROC_ProbDataCall();

            this._llstData.Clear();

			if (brbtnMetrology.Checked == true)
				this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_TYPE, "METROLOGY");
			else if (brbtnMain.Checked == true)
				this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_TYPE, "PROCESSING");
			else //Default = Metrology
				this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_TYPE, "METROLOGY");

            DataSet _ds= _wsSPC.GetOperationID(_llstData.GetSerialData());

            if (!DataUtil.IsNullOrEmptyDataSet(_ds))
            {
                DataTable dt = _ds.Tables[0];

                if (!DataUtil.IsNullOrEmptyDataSet(this._dsProbeOperation))
                {
                    DataRow _dr = null;
                    foreach(DataRow dr in _dsProbeOperation.Tables[0].Rows)
                    {
                        string sCode = dr[Definition.CHART_COLUMN.OPERATION_ID].ToString();
                        bool bContains = false;
                        foreach(DataRow dRow in dt.Rows)
                        {
                            if (dRow[Definition.CHART_COLUMN.OPERATION_ID].ToString().Equals(sCode))
                            {
                                bContains = true;
                                break;
                            }
                        }
                        if (!bContains)
                        {
                            _dr = dt.NewRow();
                            _dr[Definition.CHART_COLUMN.OPERATION_ID] = sCode;
                            _dr[Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION] = sCode+" "+dr[Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION].ToString();
                            dt.Rows.Add(_dr);
                        }
                    }
                }

                this.bSpreadStepMet.ClearHead();
                this.bSpreadStepMet.DataSource = null;
                this.bSpreadStepMet.ActiveSheet.RowCount = 0;
                this.bSpreadStepMet.ActiveSheet.Columns.Count = 3;
                this.bSpreadStepMet.AddHead(0, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.V_SELECT), "_SELECT", 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);
                this.bSpreadStepMet.AddHead(1, Definition.CONDITION_KEY_CODE, Definition.CHART_COLUMN.OPERATION_ID, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false );
                this.bSpreadStepMet.AddHead(2, Definition.CONDITION_KEY_NAME, Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                this.bSpreadStepMet.AddHeadComplete();
                this.bSpreadStepMet.DataSource = dt;      

            }

            this._llstData.Clear();
            this._llstData.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_SORTING_KEY);
            this._llstData.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            _ds = _wsSPC.GetCodeData(_llstData.GetSerialData());
            if (!DataUtil.IsNullOrEmptyDataSet(_ds))
            {
                this.bSpreadSorting.ClearHead();
                this.bSpreadSorting.DataSource = null;
                this.bSpreadSorting.ActiveSheet.RowCount = 0;
                this.bSpreadSorting.ActiveSheet.Columns.Count = 3;
                this.bSpreadSorting.AddHead(0, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.V_SELECT), "_SELECT", 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);
                this.bSpreadSorting.AddHead(1, Definition.CONDITION_KEY_CODE, Definition.CONDITION_KEY_CODE, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
                this.bSpreadSorting.AddHead(2, Definition.CONDITION_KEY_NAME, Definition.CONDITION_KEY_NAME, 80, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                this.bSpreadSorting.AddHeadComplete();
                this.bSpreadSorting.DataSource = _ds;
            }
        }

        #endregion

        #region User Defined


        //Probe 공정 조회
        private void PROC_ProbDataCall()
        {
            //GetIsNullMultiOperation
            DataSet ds = null;
            try
            {
                this._llstData.Clear();
                this._llstData.Add(Definition.DynamicCondition_Condition_key.FAB, this._sFab);
                //this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this._sOperation);
                _dsProbeOperation = this._wsSPC.GetIsNullMultiOperation(_llstData.GetSerialData());
            }
            catch
            {
            }
            finally
            {
                if (ds != null) ds.Dispose();
            }


        }

        private void PROC_BindItem()
        {
            DataSet _ds =null;
            DataSet _dsItem = null;
            bool _bProbeOperation = false;
            this.bProbe = false;

            //Probe Operation 공정 확인 
            if(!DataUtil.IsNullOrEmptyDataSet(this._dsProbeOperation))
            {
                foreach (DataRow dRow in this._dsProbeOperation.Tables[0].Rows)
                {
                    if (dRow[Definition.CHART_COLUMN.OPERATION_ID].ToString().Equals(this._sOperation))
                    {
                        _bProbeOperation = true;
                        break;
                    }
                }
            }

            if (_bProbeOperation)
            {
                this.bProbe =true;
                this._llstData.Clear();
                this._llstData.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_PROBE_ITEM);
                this._llstData.Add(Definition.CONDITION_KEY_USE_YN, "Y");
                _dsItem = this._wsSPC.GetCodeData(this._llstData.GetSerialData());
                if (!DataUtil.IsNullOrEmptyDataSet(_dsItem))
                {
                    this.bSpreadItem.ClearHead();
                    this.bSpreadItem.DataSource = null;
                    this.bSpreadItem.ActiveSheet.RowCount = 0;
                    this.bSpreadItem.ActiveSheet.Columns.Count = 3;
                    this.bSpreadItem.AddHead(0, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.V_SELECT), "_SELECT", 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);
                    this.bSpreadItem.AddHead(1, Definition.CONDITION_KEY_CODE, Definition.CONDITION_KEY_CODE, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
                    this.bSpreadItem.AddHead(2, Definition.CONDITION_KEY_NAME, Definition.CONDITION_KEY_NAME, 80, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                    this.bSpreadItem.AddHeadComplete();
                    this.bSpreadItem.DataSource = _dsItem;
                }
                else
                {
                    DataSet ds = (DataSet)this.bSpreadItem.DataSource;

                    ds.Tables[0].Rows.Clear();
                    ds.AcceptChanges();
                    bSpreadItem.DataSet = ds;
                }
            }
			else
            {
                this._llstData.Clear();
                this._llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.LineRawID);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.PARAM_TYPE);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, "'"+this._sOperation+"'");
                _dsItem = this._wsSPC.GetParamName(_llstData.GetSerialData());
                if (!DataUtil.IsNullOrEmptyDataSet(_dsItem))
                {
                    this.bSpreadItem.ClearHead();
                    this.bSpreadItem.DataSource = null;
                    this.bSpreadItem.ActiveSheet.RowCount = 0;
                    this.bSpreadItem.ActiveSheet.Columns.Count = 3;
                    this.bSpreadItem.AddHead(0, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.V_SELECT), "_SELECT", 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);
                    this.bSpreadItem.AddHead(1, Definition.CONDITION_KEY_CODE, COLUMN.PARAM_ALIAS, 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
                    this.bSpreadItem.AddHead(2, Definition.CONDITION_KEY_NAME, COLUMN.PARAM_ALIAS, 80, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                    this.bSpreadItem.AddHeadComplete();
                    this.bSpreadItem.DataSource = _dsItem;
                }
                else
                {
                    DataSet ds = (DataSet)this.bSpreadItem.DataSource;

                    if (!DataUtil.IsNullOrEmptyDataSet(ds))
                    {
                        ds.Tables[0].Rows.Clear();
                        ds.AcceptChanges();
                        bSpreadItem.DataSet = ds;
                    }
                }

            }
        }

        private void PROC_BindSubData()
        {

            DataSet _ds = null;
            DataTable dt = null;
            try
            {
                if(bProbe)
                {
                    this._llstData.Clear();
                    this._llstData.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_PROBE_SUBITEM);
                    this._llstData.Add(Definition.CONDITION_KEY_USE_YN, "Y");
                    _ds = this._wsSPC.GetCodeData(_llstData.GetSerialData());
                    if (!DataUtil.IsNullOrEmptyDataSet(_ds))
                    {
                        dt = _ds.Tables[0].Copy();
                    }
                }else
                {

                    _llstData.Clear();
                    _llstData.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_MULTI_SUBITEM);
                    _llstData.Add(Definition.CONDITION_KEY_USE_YN, "Y");
                    DataSet _dsSubItem = this._wsSPC.GetCodeData(_llstData.GetSerialData());

                    _llstData.Clear();
                    _llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.PARAM_TYPE);
                    _llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, this._sParamItem);
                    _llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this._sOperation);
                    _ds = this._wsSPC.GetParamList(_llstData.GetSerialData());
                    if (!DataUtil.IsNullOrEmptyDataSet(_ds))
                    {
                        string sValue = _ds.Tables[0].Rows[0][COLUMN.PARAM_LIST].ToString();
                        string[] arr = sValue.Split(';');
                        if (!DataUtil.IsNullOrEmptyDataSet(_dsSubItem))
                            dt = _dsSubItem.Tables[0].Copy();
                        else
                        {
                            dt = new DataTable();
                            dt.Columns.Add(Definition.CONDITION_KEY_CODE,typeof(string));
                            dt.Columns.Add(Definition.CONDITION_KEY_NAME, typeof(string));
                        }
                        DataRow dr = null;
                        for (int i = 0; i < arr.Length; i++)
                        {
                            dr = dt.NewRow();
                            if (string.IsNullOrEmpty(arr[i])) continue;
                            dr[Definition.CONDITION_KEY_CODE] = arr[i];
                            dr[Definition.CONDITION_KEY_NAME] = arr[i];
                            dt.Rows.Add(dr);
                        }
                    }

                }

                this.bSpreadSubData.ClearHead();
                this.bSpreadSubData.DataSource = null;
                this.bSpreadSubData.ActiveSheet.RowCount = 0;
                this.bSpreadSubData.ActiveSheet.Columns.Count = 4;
                this.bSpreadSubData.AddHead(0, this._mlthandler.GetVariable(Definition.SpreadHeaderColKey.V_SELECT), "_SELECT", 50, 20, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, true);
                this.bSpreadSubData.AddHead(1, this._mlthandler.GetVariable(Definition.CONDITION_KEY_CODE), Definition.CONDITION_KEY_CODE, 80, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
                this.bSpreadSubData.AddHead(2, this._mlthandler.GetVariable(Definition.CONDITION_KEY_NAME), Definition.CONDITION_KEY_NAME, 80, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                this.bSpreadSubData.AddHeadComplete();
                this.bSpreadSubData.DataSource = dt;
            }
            catch
            {
            }
            finally
            {
                if (_ds != null) _ds.Dispose();
                if (dt != null) dt.Dispose();
            }
        }


        private string GetCodeString(BSpread _bSpread)
        {
            ArrayList alCheckRowIndex = null;
            string sRtnValue = string.Empty;
            
            alCheckRowIndex = this._bspreadutility.GetCheckedRowIndex(_bSpread, (int)enumMultiDataConditionPopup.SELECT);
            for (int i = 0; i < alCheckRowIndex.Count; i++)
            {
                sRtnValue +=_bSpread.ActiveSheet.Cells[(int)alCheckRowIndex[i], 1].Text+",";
            }
            
            if(!string.IsNullOrEmpty(sRtnValue))
            {
                sRtnValue = sRtnValue.Substring(0, sRtnValue.Length - 1);
            }
            return sRtnValue;

        }
        #endregion




        #region Event Button

        private void bBtnAdd_Click(object sender, EventArgs e)
        {
            ArrayList alCheckRowIndex = null;
            if (string.IsNullOrEmpty(this._sOperation))
            {
                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_SELECT_OBJECT"), "Step"));
                return;
            }

            alCheckRowIndex = this._bspreadutility.GetCheckedRowIndex(this.bSpreadItem, 0);
            if (alCheckRowIndex.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_SELECT_OBJECT"), "Item"));
                return;
            }

            alCheckRowIndex = this._bspreadutility.GetCheckedRowIndex(this.bSpreadSubData, 0);
            if (alCheckRowIndex.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_SELECT_OBJECT"), "Sub Data"));
                return;
            }

			//if (this.bSpreadSelect.ActiveSheet.RowCount > 2)
			//{
			//    MSGHandler.DisplayMessage(MSGType.Warning, "You can't select more than 2 items.");
			//    return;
			//}
			string sProbe =this.bProbe ? "1":"0";                                                            
            this.bSpreadSelect.ActiveSheet.AddRows(this.bSpreadSelect.ActiveSheet.RowCount, 1);
            int iRow = this.bSpreadSelect.ActiveSheet.RowCount - 1;     
            this.bSpreadSelect.ActiveSheet.Cells[iRow, (int)enumMultiDataConditionPopup.OPERATION_ID].Text = this._sOperation;
            this.bSpreadSelect.ActiveSheet.Cells[iRow, (int)enumMultiDataConditionPopup.OPERATION_DESCRIPTION].Text = this._sOperationDesc;
            this.bSpreadSelect.ActiveSheet.Cells[iRow, (int)enumMultiDataConditionPopup.INFORMATION].Text = GetCodeString(this.bSpreadSorting);
            this.bSpreadSelect.ActiveSheet.Cells[iRow, (int)enumMultiDataConditionPopup.ITEM].Text = GetCodeString(this.bSpreadItem);
            this.bSpreadSelect.ActiveSheet.Cells[iRow, (int)enumMultiDataConditionPopup.SUBDATA].Text = GetCodeString(this.bSpreadSubData);
            this.bSpreadSelect.ActiveSheet.Cells[iRow, (int)enumMultiDataConditionPopup.PROBE].Text = sProbe;
			string sType = (this.brbtnMain.Checked == true) ? "MAIN" : "METROLOGY";
			this.bSpreadSelect.ActiveSheet.Cells[iRow, (int)enumMultiDataConditionPopup.TYPE].Text = sType;
        }
        
		private void bBtnDel_Click(object sender, EventArgs e)
        {
            ArrayList alCheckRowIndex = this._bspreadutility.GetCheckedRowIndex(this.bSpreadSelect, 0);
            if (alCheckRowIndex.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("FDC_ALLOW_SINGLE_SELECTED_ROW"));
                return;
            }
            else
            {
                for (int i = alCheckRowIndex.Count-1; i >= 0; i--)
                {
                    this.bSpreadSelect.ActiveSheet.RemoveRows((int)alCheckRowIndex[i], 1);
                }
            }
            
            if(this.bSpreadSelect.ActiveSheet.RowCount==0) 
            this._bProbe =false;

        }

        private void bSpread_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            BSpread bsprData = sender as BSpread;

			///
			DataSet ds = (DataSet)bsprData.DataSource;

			if (ds.Tables[0].Columns.Contains(Definition.STEP_DATAMAPPING.BASE))
			{
				if(e.Column == (int)enumMultiDataConditionPopup.BASE && (bool)bsprData.GetCellValue(e.Row, (int)enumMultiDataConditionPopup.BASE) == true)
				{
					for (int i = 0; i < bsprData.ActiveSheet.RowCount; i++)
					{
						if (i == e.Row) continue;
						bsprData.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.BASE].Value = false;
					}
				}
			}

			///

			if (e.Column != (int)enumMultiDataConditionPopup.SELECT) return;

            if ((bool)bsprData.GetCellValue(e.Row, (int)enumMultiDataConditionPopup.SELECT) == true)
            {
                if (bsprData.Name == this.bSpreadStepMet.Name
                    ||(bsprData.Name == this.bSpreadItem.Name && !this.bProbe))
                {
                    for (int i = 0; i < bsprData.ActiveSheet.RowCount; i++)
                    {
                        if (i == e.Row) continue;
                        bsprData.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.SELECT].Value = 0;
                    }
                }

                ArrayList alCheckRowIndex = this._bspreadutility.GetCheckedRowIndex(bsprData, (int)enumMultiDataConditionPopup.SELECT);
                if (alCheckRowIndex.Count > 0)
                {
                    if (bsprData.Name == this.bSpreadStepMet.Name)
                    {
                        if (alCheckRowIndex.Count > 1)
                        {
                            MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("FDC_ALLOW_SINGLE_SELECTED_ROW"));
                            return;
                        }

                        this._sOperation = bsprData.ActiveSheet.Cells[(int)alCheckRowIndex[0], 1].Text;
                        this._sOperationDesc = bsprData.ActiveSheet.Cells[(int)alCheckRowIndex[0], 2].Text;

                        this.PROC_BindItem();
                        //if(this.bProbe)
                        this.PROC_BindSubData();
                    }
                    else if (bsprData.Name == this.bSpreadItem.Name)
                    {
                        if (!this.bProbe)
                        {
                            if (alCheckRowIndex.Count > 1)
                            {
                                MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("FDC_ALLOW_SINGLE_SELECTED_ROW"));
                                return;
                            }

                            this._sParamItem = bsprData.ActiveSheet.Cells[(int)alCheckRowIndex[0], 1].Text;
                            this.PROC_BindSubData();
                        }
                    }
                    else if (bsprData.Name == this.bSpreadSelect.Name)
                    {
                        //this.PROC_BindSubItem(this.SPC_PARAM_TYPE_EVS, bsprData.ActiveSheet.Cells[(int)alCheckRowIndex[0], 1].Text);
                    }
                }
            }else
            {
                 if (bsprData.Name == this.bSpreadStepMet.Name)
                 {
                     this._sOperation = string.Empty;
                     this.PROC_BindItem();
                 }
            }
           
        }

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if(this.bSpreadSelect.ActiveSheet.RowCount==0)
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_NO_DATA_MAPPING_FOR_STEP", null, null);
                    return;
                }

				//if (this.bSpreadSelect.ActiveSheet.RowCount >2)
				//{
				//    MSGHandler.DisplayMessage(MSGType.Warning, "You can't select more than 2 items.");
				//    return;
				//}

                ArrayList alCheckRowIndex = this._bspreadutility.GetCheckedRowIndex(this.bSpreadSelect, (int)enumMultiDataConditionPopup.BASE);
                if (this.bSpreadSelect.ActiveSheet.RowCount >= 1 && alCheckRowIndex.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_CHOOSE_ITEM_FOR_STEP", null, null);
                    return;
                }

				//if (this.bSpreadSelect.ActiveSheet.RowCount == alCheckRowIndex.Count)
				//{
				//    MSGHandler.DisplayMessage(MSGType.Warning, "Please choose the item to use for target.");
				//    return;
				//}

                dtParam = new DataTable();
                DataRow dr = null;
                dtParam.Columns.Add(Definition.STEP_DATAMAPPING.BASE, typeof(bool));
                dtParam.Columns.Add(Definition.STEP_DATAMAPPING.OPERATION_ID, typeof(string));
                dtParam.Columns.Add(Definition.STEP_DATAMAPPING.OPERATION_DESCRIPTION, typeof(string));
                dtParam.Columns.Add(Definition.STEP_DATAMAPPING.INFORMATION, typeof(string));
                dtParam.Columns.Add(Definition.STEP_DATAMAPPING.ITEM, typeof(string));
                dtParam.Columns.Add(Definition.STEP_DATAMAPPING.SUBDATA, typeof(string));
				dtParam.Columns.Add(Definition.STEP_DATAMAPPING.PROBE, typeof(string));
				dtParam.Columns.Add(Definition.STEP_DATAMAPPING.TYPE, typeof(string));

                for(int i=0; i<this.bSpreadSelect.ActiveSheet.RowCount; i++)
                {
                    dr = dtParam.NewRow();
                    dr[Definition.STEP_DATAMAPPING.BASE] = this.bSpreadSelect.GetCellValue(i, (int)enumMultiDataConditionPopup.BASE) == null ? DBNull.Value : this.bSpreadSelect.GetCellValue(i, (int)enumMultiDataConditionPopup.BASE);
					if (dr[Definition.STEP_DATAMAPPING.BASE] == DBNull.Value)
						dr[Definition.STEP_DATAMAPPING.BASE] = false;
                    dr[Definition.STEP_DATAMAPPING.OPERATION_ID] = this.bSpreadSelect.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.OPERATION_ID].Text;
                    dr[Definition.STEP_DATAMAPPING.OPERATION_DESCRIPTION] = this.bSpreadSelect.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.OPERATION_DESCRIPTION].Text;
                    dr[Definition.STEP_DATAMAPPING.INFORMATION] = this.bSpreadSelect.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.INFORMATION].Text;
                    dr[Definition.STEP_DATAMAPPING.ITEM] = this.bSpreadSelect.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.ITEM].Text;
                    dr[Definition.STEP_DATAMAPPING.SUBDATA] = this.bSpreadSelect.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.SUBDATA].Text;
                    dr[Definition.STEP_DATAMAPPING.PROBE] = this.bSpreadSelect.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.PROBE].Text;
					dr[Definition.STEP_DATAMAPPING.TYPE] = this.bSpreadSelect.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.TYPE].Text;

                    dtParam.Rows.Add(dr);
                }

				//Base가 True인 row가 dtParam의 제일 윗줄에 오도록 한다.
				if (dtParam.Rows[0][Definition.STEP_DATAMAPPING.BASE].ToString() != "True")
				{
					DataSet dsResult = new DataSet();
					DataRow[] sortedData = dtParam.Select("BASE = False");
					dsResult.Merge(sortedData);

					for (int i = dtParam.Rows.Count - 1; i >= 0; i--)
					{
						if (dtParam.Rows[i][Definition.STEP_DATAMAPPING.BASE].ToString() != "True")
							dtParam.Rows.RemoveAt(i);
					}

					dtParam.Merge(dsResult.Tables[0]);
				}

                this.ButtonResult = DialogResult.OK;
                this.Close();
            }
            catch
            {
            }
            finally
            {

            }
        }

        private void bbtnClose_Click(object sender, System.EventArgs e)
        {
            this.ButtonResult = DialogResult.Cancel;
            this.Close();
        }

		private void brbtnMain_CheckedChanged(object sender, EventArgs e)
		{
			this.SearchStepInfo();
		}

		private void brbtnMetrology_CheckedChanged(object sender, EventArgs e)
		{
			this.SearchStepInfo();
		}

		public void SearchStepInfo()
		{
			this._llstData.Clear();

			if (brbtnMetrology.Checked == true)
				this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_TYPE, "METROLOGY");
			else if (brbtnMain.Checked == true)
				this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_TYPE, "PROCESSING");
			else //Default = Metrology
				this._llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_TYPE, "METROLOGY");

			if (btbID.Text.Length > 0)
				this._llstData.Add(Definition.CONDITION_KEY_SEARCH_OPERATION_ID, btbID.Text.ToString());

			DataSet _ds = _wsSPC.GetOperationID(_llstData.GetSerialData());

			if (_ds != null) //!DataUtil.IsNullOrEmptyDataSet(_ds))
			{
				DataTable dt = _ds.Tables[0];

				this.bSpreadStepMet.ClearData();
				this.bSpreadStepMet.DataSource = dt;
			}

			this._llstData.Clear();
			this._llstData.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_SORTING_KEY);
			this._llstData.Add(Definition.CONDITION_KEY_USE_YN, "Y");
			_ds = _wsSPC.GetCodeData(_llstData.GetSerialData());
			if (!DataUtil.IsNullOrEmptyDataSet(_ds))
			{
				this.bSpreadSorting.ClearData();
				this.bSpreadSorting.DataSource = _ds;
			}
		}

		private void bbtnSearch_Click(object sender, EventArgs e)
		{
			this.SearchStepInfo();
		}

        #endregion

		private void btbID_KeyPress(object sender, KeyPressEventArgs e)
		{
			if(e.KeyChar == 13)
			{
				this.SearchStepInfo();
			}
		}
    }
}
