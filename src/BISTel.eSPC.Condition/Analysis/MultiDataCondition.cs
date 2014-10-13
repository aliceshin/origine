using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;

using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Default;


namespace BISTel.eSPC.Condition.Analysis
{
    public partial class MultiDataCondition : ADynamicCondition
    {

        BISTel.eSPC.Condition.Controls.Date.DateCondition dateCondition1;
        eSPCWebService.eSPCWebService _wsSPC;
        SortingKey sortUC;
        

        LinkedList _llstData = new LinkedList();        
        LinkedList _condition = new LinkedList();

        List<string> _lstRawColumn;
        DateTime _DateTimeStart = DateTime.Now.AddDays(-30);
        DateTime _DateTimeEnd = DateTime.Now;

        CommonUtility _comUtil;
        MultiLanguageHandler _mlthandler;
        Initialization _Initialization;
        BSpreadUtility _bspreadutility;        
        DataTable _dtParam = null;     
              
        string _sSortingKey = string.Empty;                
        string _sAreaRawID = string.Empty;
        string _sLineRawID = string.Empty;        
        string _sFab = string.Empty;
        string _sParamType = string.Empty;        
        string _sStartTime = string.Empty;
        string _sEndTime = string.Empty;
        string _sEqpModel = string.Empty;
        
        
        bool _bProbe= false;
                
        public MultiDataCondition()
        {       
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._comUtil = new CommonUtility();
            this._Initialization = new Initialization();
            this._bspreadutility = new BSpreadUtility();

            InitializeComponent();
            this.dateCondition1 = new BISTel.eSPC.Condition.Controls.Date.DateCondition();
            this.dateCondition1.Dock = DockStyle.Fill;
            this.bTPnlPeriod.Controls.Add(dateCondition1);
            
            this.sortUC = new SortingKey();
            this.sortUC.Dock = DockStyle.Fill;            
            this.bTPnlSortingKey.Controls.Add(sortUC);
            this.PROC_BindSortingKey();
            this.bTPnlPeriod.Title = _mlthandler.GetVariable(Definition.LABEL.PERIOD);
            this.bTPnlSortingKey.Title = _mlthandler.GetVariable(Definition.Analysis.LABEL_SORTING_KEY);

            InitializePage();
        }

        private void InitializePage()
        {
            this.dateCondition1.DateType = "CUSTOM";
            this.dateCondition1.DateTimeStart = DateTime.Now.AddDays(-7);
            this.dateCondition1.DateTimeEnd = DateTime.Now;

            this.bSpread1.UseSpreadEdit = false;
            this.bSpread1.AutoGenerateColumns = false;
            this.bSpread1.ActiveSheet.DefaultStyle.ResetLocked();
            this.bSpread1.UseEdit = true;
            this.bSpread1.ClearHead();
            this.bSpread1.Locked = true;
            this.bSpread1.UseHeadColor = true;
            this.bSpread1.EditMode = false;
            this.bSpread1.ActiveSheet.RowCount = 0;
            this.bSpread1.ActiveSheet.ColumnCount = 0;
            this.bSpread1.DataSource = null;    
        }

        #region override
        public override LinkedList GetParameter(LinkedList ll)
        {
            if (this._Initialization == null)
            {

            }                        
                                               
            string _Type = "0";
            string _Operation = string.Empty;
            string _TargetOperation = string.Empty;
            string _OperationDesc = string.Empty;
            string _TargetOperationDesc = string.Empty;
            string _probe ="0";
            string _TargetProbe = "0";
            List<string> _lstInfo = new List<string>();
            List<string> _lstItem = new List<string>();
            List<string> _lstSubData = new List<string>();
            List<string> _lstTargetInfo = new List<string>();
            List<string> _lstTargetItem = new List<string>();
            List<string> _lstTargetSubData = new List<string>();



			List<string> slTargetOperation = new List<string>();
			List<string> slTargetOperationDesc = new List<string>();
			List<string> slTargetProbe = new List<string>();

			ArrayList aTargetInfo = new ArrayList();
                                                                        
            if(_bProbe) _Type = "1";
            
			//if(this.bSpread1.ActiveSheet.RowCount>0)
			//{
			//    for(int i=0; i<this.bSpread1.ActiveSheet.RowCount; i++)
			//    {
			//        if (this.bSpread1.GetCellValue(i, 0)==null || (bool)this.bSpread1.GetCellValue(i, 0)==false)
			//        {
			//            slTargetOperation.Add(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.OPERATION_ID - 1].Text);
			//            slTargetOperationDesc.Add(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.OPERATION_DESCRIPTION - 1].Text);

			//            //_lstTargetInfo = this.GetSplit(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.INFORMATION - 1].Text);
			//            //_lstTargetItem = this.GetSplit(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.ITEM - 1].Text);
			//            //_lstTargetSubData = this.GetSplit(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.SUBDATA - 1].Text);

			//            _lstTargetInfo.Add(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.INFORMATION - 1].Text); //= this.GetSplit(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.INFORMATION - 1].Text));
			//            _lstTargetItem.Add(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.ITEM - 1].Text); //= this.GetSplit(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.ITEM - 1].Text);
			//            _lstTargetSubData.Add(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.SUBDATA - 1].Text); //= this.GetSplit(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.SUBDATA - 1].Text);
			//            slTargetProbe.Add(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.PROBE - 1].Text);
			//        }else
			//        {
			//            _Operation = this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.OPERATION_ID - 1].Text;
			//            _OperationDesc = this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.OPERATION_DESCRIPTION - 1].Text;

			//            _lstInfo = this.GetSplit(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.INFORMATION - 1].Text);
			//            _lstItem = this.GetSplit(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.ITEM - 1].Text);
			//            _lstSubData = this.GetSplit(this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.SUBDATA - 1].Text);
			//            _probe = this.bSpread1.ActiveSheet.Cells[i, (int)enumMultiDataConditionPopup.PROBE - 1].Text;
			//        }
			//    }
			//}

            //ll.Add(Definition.DynamicCondition_Search_key.TYPE, DynamicDefinition.DynamicConditionSetting(_Type, _Type));                        
            //Operation
			//ll.Add(Definition.DynamicCondition_Search_key.OPERATION, DynamicDefinition.DynamicConditionSetting(_Operation, _OperationDesc));
			//ll.Add(Definition.DynamicCondition_Search_key.INFORMATION, DynamicDefinition.DynamicConditionSetting(_lstInfo.ToArray(), _lstInfo.ToArray()));
			//ll.Add(Definition.DynamicCondition_Search_key.ITEM, DynamicDefinition.DynamicConditionSetting(_lstItem.ToArray(), _lstItem.ToArray()));
			//ll.Add(Definition.DynamicCondition_Search_key.SUBDATA, DynamicDefinition.DynamicConditionSetting(_lstSubData.ToArray(), _lstSubData.ToArray()));
			//ll.Add(Definition.DynamicCondition_Search_key.PROBE, DynamicDefinition.DynamicConditionSetting(_probe, _probe));
            
			////Target
			//ll.Add(Definition.DynamicCondition_Search_key.BASE, DynamicDefinition.DynamicConditionSetting(slTargetOperation.ToArray(), slTargetOperationDesc.ToArray()));
			//ll.Add(Definition.DynamicCondition_Search_key.BASE_INFORMATION, DynamicDefinition.DynamicConditionSetting(_lstTargetInfo.ToArray(), _lstTargetInfo.ToArray()));
			//ll.Add(Definition.DynamicCondition_Search_key.BASE_ITEM, DynamicDefinition.DynamicConditionSetting(_lstTargetItem.ToArray(), _lstTargetItem.ToArray()));
			//ll.Add(Definition.DynamicCondition_Search_key.BASE_SUBDATA, DynamicDefinition.DynamicConditionSetting(_lstTargetSubData.ToArray(), _lstTargetSubData.ToArray()));
			ll.Add(Definition.DynamicCondition_Search_key.BASE_PROBE, DynamicDefinition.DynamicConditionSetting(slTargetProbe.ToArray(), slTargetProbe.ToArray()));  


			ll.Add(Definition.DynamicCondition_Search_key.CUSTOM_CONTEXT, this._dtParam);  

            
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_PERIOD, DynamicDefinition.DynamicConditionSetting(this.dateCondition1.DateType, this.dateCondition1.DateType));
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, this.dateCondition1.GetDateTimeSelectedValue("F"));
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, this.dateCondition1.GetDateTimeSelectedValue("T"));
            ll.Add(Definition.DynamicCondition_Search_key.SORTING_KEY, DynamicDefinition.DynamicConditionSetting(this.sortUC.SelectedItems, this.sortUC.SelectedItems));                        
           
            return ll;
        }


        public override void RefreshCondition(LinkedList ll)
        {

            if (this._Initialization == null)
            {

            }

            if (ll.Contains(Definition.DynamicCondition_Search_key.LINE))
                this._sLineRawID = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.LINE]);

            if (ll.Contains(Definition.DynamicCondition_Search_key.FAB))
                this._sFab = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.FAB]);                


            if (ll.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
                this._sParamType = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.PARAM_TYPE]);


            DataTable dt = null;
            if (ll.Contains(Definition.DynamicCondition_Search_key.AREA))
            {
                dt = (DataTable)ll[Definition.DynamicCondition_Search_key.AREA];
                this._sAreaRawID = DataUtil.GetConditionKeyDataList(dt, DCUtil.VALUE_FIELD);
            }

            if (ll.Contains(Definition.DynamicCondition_Search_key.EQPMODEL))
            {
                dt = (DataTable)ll[Definition.DynamicCondition_Search_key.EQPMODEL];
                this._sEqpModel = DataUtil.GetConditionKeyDataList(dt, DCUtil.VALUE_FIELD, true);
            }
                

            SetCondition(ll);
        }


        private void SetCondition(LinkedList ll)
        {
            DataTable dt = null;
            try
            {
                //여기서 ll이랑 _condition이랑 복제한다. 
                byte[] tmp = ll.GetSerialData();

                _condition.SetSerialData((byte[])tmp.Clone());


 

                if (_condition.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
                    this._sParamType = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.PARAM_TYPE]);
               
              
                if (_condition.Contains(Definition.DynamicCondition_Search_key.PERIOD_PPK))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.PERIOD_PPK];
                    this.dateCondition1.DateType = dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();
                }

                if (_condition.Contains(Definition.DynamicCondition_Search_key.DATETIME_FROM))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_FROM];
                    this._sStartTime = DCUtil.GetValueData(dt);
                    this.dateCondition1.DateTimeStart = DateTime.Parse(this._sStartTime);
                }

                if (_condition.Contains(Definition.DynamicCondition_Search_key.DATETIME_TO))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_TO];
                    this._sEndTime = DCUtil.GetValueData(dt);
                    this.dateCondition1.DateTimeEnd = DateTime.Parse(this._sEndTime);
                }
              
                if (_condition.Contains(Definition.DynamicCondition_Search_key.SORTING_KEY))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.SORTING_KEY];
                    sortUC.AddItemsSelected(dt, Definition.DynamicCondition_Condition_key.VALUEDATA);
                }


                this.bSpread1.ClearHead();
                this.bSpread1.DataSource = null;
                this.bSpread1.ActiveSheet.RowCount = 0;
                this.bSpread1.ActiveSheet.ColumnCount= 0;                
                if (_condition.Contains(Definition.DynamicCondition_Search_key.TYPE))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.TYPE];
                    string sType = DCUtil.GetValueData(dt);
                    if(sType.Equals("1"))                    
                        this._bProbe =true;                        
                    else
                        this._bProbe = false;                        
                                                    
                    this._dtParam = new DataTable();
                    this._dtParam.Columns.Add(Definition.STEP_DATAMAPPING.BASE, typeof(bool));
                    this._dtParam.Columns.Add(Definition.STEP_DATAMAPPING.OPERATION_ID, typeof(string));
                    this._dtParam.Columns.Add(Definition.STEP_DATAMAPPING.OPERATION_DESCRIPTION, typeof(string));
                    this._dtParam.Columns.Add(Definition.STEP_DATAMAPPING.INFORMATION, typeof(string));
                    this._dtParam.Columns.Add(Definition.STEP_DATAMAPPING.ITEM, typeof(string));
                    this._dtParam.Columns.Add(Definition.STEP_DATAMAPPING.SUBDATA, typeof(string));
                    this._dtParam.Columns.Add(Definition.STEP_DATAMAPPING.PROBE, typeof(string));                                        
                    
                    DataRow dr = null;                    
                    dr = this._dtParam.NewRow();
                    dr[Definition.STEP_DATAMAPPING.BASE] = 0;
                    dr[Definition.STEP_DATAMAPPING.OPERATION_ID] = GetStringList((DataTable)_condition[Definition.DynamicCondition_Search_key.OPERATION],Definition.DynamicCondition_Search_key.VALUEDATA);
                    dr[Definition.STEP_DATAMAPPING.OPERATION_DESCRIPTION] = GetStringList((DataTable)_condition[Definition.DynamicCondition_Search_key.OPERATION], Definition.DynamicCondition_Search_key.DISPLAYDATA);
                    dr[Definition.STEP_DATAMAPPING.INFORMATION] = GetStringList((DataTable)_condition[Definition.DynamicCondition_Search_key.INFORMATION],Definition.DynamicCondition_Search_key.VALUEDATA);
                    dr[Definition.STEP_DATAMAPPING.ITEM] = GetStringList((DataTable)_condition[Definition.DynamicCondition_Search_key.ITEM],Definition.DynamicCondition_Search_key.VALUEDATA);
                    dr[Definition.STEP_DATAMAPPING.SUBDATA] = GetStringList((DataTable)_condition[Definition.DynamicCondition_Search_key.SUBDATA],Definition.DynamicCondition_Search_key.VALUEDATA);

                    if (_condition.Contains(Definition.DynamicCondition_Search_key.PROBE))
                    {
                        dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.PROBE];
                        dr[Definition.STEP_DATAMAPPING.PROBE] = DCUtil.GetValueData(dt)=="0" ?false:true;;
                    }
                    
                    this._dtParam.Rows.Add(dr);

                    if (_condition.Contains(Definition.DynamicCondition_Search_key.BASE_PROBE))
                    {                
                        string sTarget =  DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.BASE]);
                        
                        if(!string.IsNullOrEmpty(sTarget))
                        {
                            dr = this._dtParam.NewRow();
                            dr[Definition.STEP_DATAMAPPING.BASE] = 1;
                            dr[Definition.STEP_DATAMAPPING.OPERATION_ID] = GetStringList((DataTable)_condition[Definition.DynamicCondition_Search_key.BASE],Definition.DynamicCondition_Search_key.VALUEDATA);
                            dr[Definition.STEP_DATAMAPPING.OPERATION_DESCRIPTION] = GetStringList((DataTable)_condition[Definition.DynamicCondition_Search_key.BASE], Definition.DynamicCondition_Search_key.DISPLAYDATA);
                            dr[Definition.STEP_DATAMAPPING.INFORMATION] = GetStringList((DataTable)_condition[Definition.DynamicCondition_Search_key.BASE_INFORMATION],Definition.DynamicCondition_Search_key.VALUEDATA);
                            dr[Definition.STEP_DATAMAPPING.ITEM] = GetStringList((DataTable)_condition[Definition.DynamicCondition_Search_key.BASE_ITEM],Definition.DynamicCondition_Search_key.VALUEDATA);
                            dr[Definition.STEP_DATAMAPPING.SUBDATA] = GetStringList((DataTable)_condition[Definition.DynamicCondition_Search_key.BASE_SUBDATA],Definition.DynamicCondition_Search_key.VALUEDATA);

                            if (_condition.Contains(Definition.DynamicCondition_Search_key.BASE_PROBE))
                            {
                                dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.BASE_PROBE];
                                dr[Definition.STEP_DATAMAPPING.PROBE] = DCUtil.GetValueData(dt) == "0" ? false : true; ;
                            }                                        
                            this._dtParam.Rows.Add(dr);
                        }
                    }
                    this._dtParam.AcceptChanges();
                    
                                                         
                    this.bSpread1.ActiveSheet.RowCount = 0;
                    this.bSpread1.ActiveSheet.ColumnCount = 7;
                    this.bSpread1.AddHead(0, Definition.STEP_DATAMAPPING.BASE, Definition.STEP_DATAMAPPING.BASE, 50, 20, null, null, null, ColumnAttribute.ReadOnly, ColumnType.CheckBox, null, null, null, false, true);
                    this.bSpread1.AddHead(1, Definition.CHART_COLUMN.OPERATION_ID, Definition.STEP_DATAMAPPING.OPERATION_ID, 100, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
                    this.bSpread1.AddHead(2, this._mlthandler.GetVariable(Definition.CHART_COLUMN.OPERATION_ID), Definition.STEP_DATAMAPPING.OPERATION_DESCRIPTION, 100, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                    this.bSpread1.AddHead(3, Definition.STEP_DATAMAPPING.INFORMATION, Definition.STEP_DATAMAPPING.INFORMATION, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                    this.bSpread1.AddHead(4, Definition.STEP_DATAMAPPING.ITEM, Definition.STEP_DATAMAPPING.ITEM, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                    this.bSpread1.AddHead(5, Definition.STEP_DATAMAPPING.SUBDATA, Definition.STEP_DATAMAPPING.SUBDATA, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                    this.bSpread1.AddHead(6, Definition.STEP_DATAMAPPING.PROBE, Definition.STEP_DATAMAPPING.PROBE, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
                    this.bSpread1.AddHeadComplete();
                    this.bSpread1.DataSource = this._dtParam;
                                                                             
                }
               
                    
            }
            catch
            {

            }
            finally
            {
                if (dt != null) dt.Dispose();
            }

        }
        #endregion


        #region User Define

        private string GetStringList(DataTable _dt , string _col)
        {
            string _sRtnValue=string.Empty;  
            if(DataUtil.IsNullOrEmptyDataTable(_dt)) return _sRtnValue;
            
            foreach(DataRow dr in _dt.Rows)
            {
                _sRtnValue += dr[_col].ToString()+",";
            }
            
            if(!string.IsNullOrEmpty(_sRtnValue))
            {
                _sRtnValue = _sRtnValue.Substring(0,_sRtnValue.Length-1);            
            }
            return _sRtnValue;
        }
        

        private List<string> GetSplit(string _Value)
        {
            List<string> lst = new List<string>();
            
            if(string.IsNullOrEmpty(_Value)) return lst;
            
            string [] arr =_Value.Split(',');
            for(int i=0; i<arr.Length; i++)
            {
                if(string.IsNullOrEmpty(arr[i])) continue;
                //if(!lst.Contains(arr[i]))
                lst.Add(arr[i].ToString());
            }
            
            return lst;
        }

        private void llstRemove(string sSearchKey)
        {
            if (_condition.Contains(sSearchKey))
                _condition.Remove(sSearchKey);
        }






   
        
        private void PROC_BindSortingKey()
        {
            DataSet ds = null;            
            try
            {
                this._llstData = new LinkedList();
                this._llstData.Add(Definition.DynamicCondition_Condition_key.CATEGORY, Definition.CODE_CATEGORY_PROBE_SORTING_KEY);
                this._llstData.Add(Definition.DynamicCondition_Condition_key.USE_YN, "Y");
                ds = this._wsSPC.GetCodeData(this._llstData.GetSerialData());
                this.sortUC.listBoxSKeyUnSelected.Items.Clear();
                if (!DataUtil.IsNullOrEmptyDataSet(ds))
                {
                    sortUC.AddItemsUnSelected(ds.Tables[0], Definition.CONDITION_KEY_CODE);
                }
            }
            catch
            {
            }
            finally
            {
                if (this._llstData != null) this._llstData = null;
                if (ds != null) ds.Dispose();
            }
        }
        
        #endregion 

        #region Events
        private void btnPlus_Click(object sender, EventArgs e)
        {
            MultiDataConditionPopup multiPop = new MultiDataConditionPopup();
            multiPop.LineRawID = this._sLineRawID;
            multiPop.Fab = this._sFab;
            multiPop.PARAM_TYPE = this._sParamType;
            multiPop.dtParam= this._dtParam;
            multiPop.bProbe = this._bProbe;            
            multiPop.InitializePopup();
            multiPop.ShowDialog();
            DialogResult dResult = multiPop.ButtonResult;
            if (dResult == DialogResult.OK)
            {            
                this._dtParam = multiPop.dtParam;
                this._bProbe = multiPop.bProbe;
                
                this.bSpread1.ClearHead();
                this.bSpread1.DataSource = null;
                this.bSpread1.ActiveSheet.RowCount = 0;
                this.bSpread1.ActiveSheet.Columns.Count = 7;
                this.bSpread1.AddHead(0, Definition.STEP_DATAMAPPING.BASE, Definition.STEP_DATAMAPPING.BASE, 50, 20, null, null, null, ColumnAttribute.ReadOnly, ColumnType.CheckBox, null, null, null, false, true);
                this.bSpread1.AddHead(1, Definition.CHART_COLUMN.OPERATION_ID, Definition.STEP_DATAMAPPING.OPERATION_ID, 100, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
                this.bSpread1.AddHead(2, this._mlthandler.GetVariable(Definition.CHART_COLUMN.OPERATION_ID), Definition.STEP_DATAMAPPING.OPERATION_DESCRIPTION, 100, 20, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                this.bSpread1.AddHead(3, Definition.STEP_DATAMAPPING.INFORMATION, Definition.STEP_DATAMAPPING.INFORMATION, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                this.bSpread1.AddHead(4, Definition.STEP_DATAMAPPING.ITEM, Definition.STEP_DATAMAPPING.ITEM, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                this.bSpread1.AddHead(5, Definition.STEP_DATAMAPPING.SUBDATA, Definition.STEP_DATAMAPPING.SUBDATA, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, true);
                this.bSpread1.AddHead(6, Definition.STEP_DATAMAPPING.PROBE, Definition.STEP_DATAMAPPING.PROBE, 150, 300, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, false);
                this.bSpread1.AddHeadComplete();
                this.bSpread1.DataSource = this._dtParam;
                                                
            }                        
        }


        private void bSpread1_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {

            if (e.Column != (int)enumMultiDataConditionPopup.SELECT) return;

            BSpread bsprData = sender as BSpread;
            if ((bool)bsprData.GetCellValue(e.Row, 0) == true)
            {

                for (int i = 0; i < bsprData.ActiveSheet.RowCount; i++)
                {
                    if (i == e.Row) continue;
                    bsprData.ActiveSheet.Cells[i, 0].Value = 0;
                }
                      
            }
            
           
        }
        
        #endregion 
 
    }
}
