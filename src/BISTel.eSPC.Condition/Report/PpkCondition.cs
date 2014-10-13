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
    public partial class PpkCondition : ADynamicCondition
    {
        private string sLineRawid = string.Empty;
        private string sAreaRawid = string.Empty;
        private eSPCWebService.eSPCWebService _wsSPC = null;
        private MultiLanguageHandler _mlthandler;
        private Initialization _Initialization;
        private LinkedList llstData = new LinkedList();
        private LinkedList _condition = new LinkedList();        
        private LinkedList _llstArea = new LinkedList();


        private string sArea = string.Empty;
        private string sParamType = string.Empty;
        private string sOperationID = string.Empty;
        private string sProductID = string.Empty;
        private string sParamItem = string.Empty;
        private string sEQPID = string.Empty;
        private string sPpkPeriod = Definition.PERIOD_PPK.DAILY;
        private string sStartTime = string.Empty;
        private string sEndTime = string.Empty;
        private string sEqpModel = string.Empty; 
               
                
        private ArrayList arrEQPID = new ArrayList();
        private ArrayList arrProductID = new ArrayList();
        private ArrayList arrOperationID = new ArrayList();
        private ArrayList arrParameter = new ArrayList();
        private CommonUtility _comUtil = null;
        private List<string> lstType  = new List<string>();
        private ListBox lstSorting = new ListBox();
        
        SortingKey sortUC;
        BISTel.eSPC.Condition.Controls.Date.PpkDateCondition  ppkDate;

        public PpkCondition()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._comUtil = new CommonUtility();
            this.sortUC = new SortingKey();
            this.ppkDate = new BISTel.eSPC.Condition.Controls.Date.PpkDateCondition();

            InitializeComponent();

            this.blblEQPID.Text = _mlthandler.GetVariable(Definition.LABEL.EQP_ID);
            this.blblOperation.Text = _mlthandler.GetVariable(Definition.LABEL.OPERATION_ID);
            this.blblParameter.Text = _mlthandler.GetVariable(Definition.LABEL.PARAMETER);
            this.blblProduct.Text = _mlthandler.GetVariable(Definition.LABEL.PRODUCT);                        
            this.bTPnlSortingKey.Text = _mlthandler.GetVariable(Definition.LABEL.SORTING_KEY);           
            InitializeLayout();      
        }



        private void InitializeLayout()
        {
            DataSet ds = null;
            LinkedList llstData = null;
            try
            {                                                         
                                                
                ppkDate.InitializePage();                
                this.PROC_BindComboType();

                sortUC.Dock = DockStyle.Fill;
                this.bTPnlSortingKey.Controls.Add(sortUC);                
            }
            catch
            {
            }
            finally
            {
                if (llstData != null) llstData = null;
                if (ds != null) ds.Dispose();
            }
        }
        
        #region override
        public override LinkedList GetParameter(LinkedList ll)
        {
            if (this._Initialization == null)
            {
               
            }
          
            string[] arrOperation = new string[this.bchkOperation.chkBox.CheckedItems.Count];
            string[] arrOperationDesc = new string[this.bchkOperation.chkBox.CheckedItems.Count];
            string[] arrParameter = new string[this.bchkParameter.chkBox.CheckedItems.Count];
            string[] arrProductID = new string[this.bChkProduct.chkBox.CheckedItems.Count];
            string[] arrEQPValue = new string[this.bchkEqpID.chkBox.CheckedItems.Count];
            int iCount = 0;           
            
            if(bchkOperation.Items.Count>0)
            {
                DataTable dt = (DataTable)bchkOperation.DataSource;            
                for (int i = 0; i < dt.Rows.Count;i++)
                {
                    string sValue = dt.Rows[i][Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION].ToString();
                    if(bchkOperation.chkBox.CheckedItems.Contains(sValue))
                    {
                        arrOperation[iCount] = dt.Rows[i][Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString();
                        arrOperationDesc[iCount]=sValue; 
                        iCount++;
                    }
                }
            }

            for (int i = 0; i < bchkParameter.chkBox.CheckedItems.Count; i++)
            {
                arrParameter[i] = bchkParameter.chkBox.CheckedItems[i].ToString();
            }

            for (int i = 0; i < this.bChkProduct.chkBox.CheckedItems.Count; i++)
            {
                arrProductID[i] = bChkProduct.chkBox.CheckedItems[i].ToString();
            }
            

            for (int i = 0; i < this.bchkEqpID.chkBox.CheckedItems.Count; i++)
            {
                arrEQPValue[i] = bchkEqpID.chkBox.CheckedItems[i].ToString();
            }
                        
            ll.Add(Definition.DynamicCondition_Search_key.OPERATION, DynamicDefinition.DynamicConditionSetting(arrOperation, arrOperationDesc));
            ll.Add(Definition.DynamicCondition_Search_key.PARAM, DynamicDefinition.DynamicConditionSetting(arrParameter, arrParameter));
            ll.Add(Definition.DynamicCondition_Search_key.PRODUCT, DynamicDefinition.DynamicConditionSetting(arrProductID, arrProductID));
            ll.Add(Definition.DynamicCondition_Search_key.EQP_ID, DynamicDefinition.DynamicConditionSetting(arrEQPValue, arrEQPValue));

            //ll.Add(Definition.DynamicCondition_Search_key.DATETIME_PERIOD, DynamicDefinition.DynamicConditionSetting("CUSTOM", "CUSTOM"));
            //ll.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, this.ppkDate.GetDateTimeSelectedValue("F"));
            //ll.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, this.ppkDate.GetDateTimeSelectedValue("T"));
            //ll.Add(Definition.DynamicCondition_Search_key.PERIOD_PPK, DynamicDefinition.DynamicConditionSetting(this.ppkDate.PeriodType, this.ppkDate.PeriodType));
            ll.Add(Definition.DynamicCondition_Search_key.SORTING_KEY, DynamicDefinition.DynamicConditionSetting(this.sortUC.SelectedItems, this.sortUC.SelectedItems));
                                                
       
            return ll;
        }


       
        
        public override void RefreshCondition(LinkedList ll)
        {

            if (this._Initialization == null)
            {
                                          
            }

            if (ll.Contains(Definition.DynamicCondition_Search_key.LINE))
                this.sLineRawid = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.LINE]);


            if (ll.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
                this.sParamType = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.PARAM_TYPE]);


            DataTable dt = null;
            if (ll.Contains(Definition.DynamicCondition_Search_key.AREA))
            {
                dt = (DataTable)ll[Definition.DynamicCondition_Search_key.AREA];
                this.sAreaRawid = DataUtil.GetConditionKeyDataList(dt, DCUtil.VALUE_FIELD);
            }

            if (ll.Contains(Definition.DynamicCondition_Search_key.EQPMODEL))
            {
                dt = (DataTable)ll[Definition.DynamicCondition_Search_key.EQPMODEL];
                this.sEqpModel = DataUtil.GetConditionKeyDataList(dt, DCUtil.VALUE_FIELD, true);
            }
               
                    
    
            SetCondition(ll);
        }


        #endregion


        #region User Method

       

        private void SetCondition(LinkedList ll)
        {
            DataTable dt = null;
            try
            {                
                //여기서 ll이랑 _condition이랑 복제한다. 
                byte[] tmp = ll.GetSerialData();

                _condition.SetSerialData((byte[])tmp.Clone());

    
                if (_condition.Contains(Definition.DynamicCondition_Search_key.OPERATION))
                {                                      
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.OPERATION];                           
                   this.arrOperationID =  _comUtil.GetConditionKeyArrayList(dt,Definition.DynamicCondition_Condition_key.VALUEDATA);              
                }

                if (_condition.Contains(Definition.DynamicCondition_Search_key.PARAM))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.PARAM];
                    this.arrParameter = _comUtil.GetConditionKeyArrayList(dt, Definition.DynamicCondition_Condition_key.VALUEDATA);
                }                        

                if (_condition.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
                    this.sParamType = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.PARAM_TYPE]);

                if (_condition.Contains(Definition.DynamicCondition_Search_key.EQP_ID))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.EQP_ID];
                    this.arrEQPID = _comUtil.GetConditionKeyArrayList(dt, Definition.DynamicCondition_Condition_key.VALUEDATA);                                                  
                }

                if (_condition.Contains(Definition.DynamicCondition_Search_key.PRODUCT))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.PRODUCT];
                    this.arrProductID = _comUtil.GetConditionKeyArrayList(dt, Definition.DynamicCondition_Condition_key.VALUEDATA);     
                }


                //SPC_PERIOD_PPK
                if (_condition.Contains(Definition.DynamicCondition_Search_key.PERIOD_PPK))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.PERIOD_PPK];    
                    this.ppkDate.PeriodType  = dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();                                             
                }


                //if (_condition.Contains(Definition.DynamicCondition_Search_key.DATETIME_FROM))
                //{
                //    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_FROM];
                //    this.sStartTime = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.VALUEDATA]);
                //    this.ppkDate.DateTimeStart = DateTime.Parse(this.sStartTime);
                //}

                //if (_condition.Contains(Definition.DynamicCondition_Search_key.DATETIME_TO))
                //{
                //    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_TO];
                //    this.sEndTime = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.VALUEDATA]);
                //    this.ppkDate.DateTimeEnd = DateTime.Parse(this.sEndTime);
                //}
            

                                                                  
                this.PROC_BindOperation();            
                this.PROC_BindParameter();            
                this.PROC_BindEQPID();            
                this.PROC_BindProduct();
                this.PROC_BindSortingKey();

                //SPC_PERIOD_PPK
                if (_condition.Contains(Definition.DynamicCondition_Search_key.SORTING_KEY))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.SORTING_KEY];
                    sortUC.AddItemsSelected(dt, Definition.DynamicCondition_Condition_key.VALUEDATA);
                }
                    

                
            
            } catch
            {
            
            }finally
            {
                if(dt !=null) dt.Dispose();
            }         

        }

        private void SetChecked(CheckedListBox _chkListBox, ArrayList _arr)
        {
            for (int i = 0; i < _chkListBox.Items.Count; i++)
            {
                string sValue = _chkListBox.Items[i].ToString();
                if (_arr.Contains(sValue))
                {
                    _chkListBox.SetItemChecked(i, true);
                }
            }        
        }
        private string SetStringList(DataTable _dt ,string _stringList)
        {                              
            _stringList = string.Empty;
            foreach (DataRow nRow in _dt.Rows)
            {                
                _stringList = SetConcat(_stringList, nRow[Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
            }   
            
            return   _stringList;      
        }

        private string SetConcat(string _stringList, string _svalue)
        {
            if(!string.IsNullOrEmpty(_stringList)) _stringList+=",";
            _stringList += "'" + _svalue+"'";
            
            return _stringList;
        }


        
        private void PROC_BindOperation()
        {
            llstData.Clear();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);            
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);
            if (!string.IsNullOrEmpty(this.sEqpModel))
                llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.sEqpModel);          
                
            llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.sParamType);
            DataSet ds = this._wsSPC.GetSPCOperation(llstData.GetSerialData());

            this.bchkOperation.Items.Clear();
            this.bchkOperation.Text=null;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {                
                int icount=0;
                foreach(DataRow nRow in ds.Tables[0].Rows)
                {                    
                    if(nRow[Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString()==Definition.VARIABLE.STAR)
                    icount++;
                }
                if(icount==0)
                {
                    DataRow row = ds.Tables[0].NewRow();
                    row[Definition.DynamicCondition_Condition_key.OPERATION_ID] = Definition.VARIABLE.STAR;
                    row[Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION] = Definition.VARIABLE.STAR;
                    
                    ds.Tables[0].Rows.InsertAt(row,0);
                }
                _comUtil.SetBCheckCombo(this.bchkOperation,ds,Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION, Definition.DynamicCondition_Condition_key.OPERATION_ID,this.arrOperationID);                        
                
                this.sOperationID = _comUtil.GetSelectedString(this.bchkOperation);

                if (this.arrOperationID.Count == 0)
                {
                    this.bchkOperation.isSearchComboCheckBox = true;
                    this.bchkOperation.Text = Definition.VARIABLE.STAR;
                }
            }
           
        }
  
        private void PROC_BindParameter()
        {                
            llstData.Clear();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);
            if (!string.IsNullOrEmpty(this.sAreaRawid))
                llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);

            if (!string.IsNullOrEmpty(this.sEqpModel))
                llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.sEqpModel);                
                           
            llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.sParamType);

            if (!string.IsNullOrEmpty(this.sOperationID) && this.sOperationID.IndexOf(Definition.VARIABLE.STAR) < 0)
            llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this.sOperationID);
            DataSet ds = this._wsSPC.GetParamName(llstData.GetSerialData());

            this.bchkParameter.Items.Clear();
            this.bchkParameter.Text = null;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {                
                int icount = 0;
                foreach (DataRow nRow in ds.Tables[0].Rows)
                {
                    if (nRow[Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString() == Definition.VARIABLE.STAR)
                        icount++;
                }
                if (icount == 0)
                {
                    DataRow row = ds.Tables[0].NewRow();
                    row[Definition.DynamicCondition_Condition_key.PARAM_ALIAS] = Definition.VARIABLE.STAR;                    

                    ds.Tables[0].Rows.InsertAt(row, 0);
                }
                _comUtil.SetBCheckCombo(this.bchkParameter, ds, Definition.DynamicCondition_Condition_key.PARAM_ALIAS, Definition.DynamicCondition_Condition_key.PARAM_ALIAS, this.arrParameter);

                this.sParamItem = _comUtil.GetSelectedString(this.bchkParameter);

                if (this.arrParameter.Count == 0)
                {
                    this.bchkParameter.isSearchComboCheckBox = true;
                    this.bchkParameter.Text = Definition.VARIABLE.STAR;
                }
            }
                       
        }

        private void PROC_BindProduct()
        {
            llstData.Clear();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);
            if (!string.IsNullOrEmpty(this.sAreaRawid))
                llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);
            llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.sParamType);
            if (!string.IsNullOrEmpty(this.sOperationID) && this.sOperationID.IndexOf(Definition.VARIABLE.STAR) < 0)
                llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this.sOperationID);

            if (!string.IsNullOrEmpty(this.sParamItem) && this.sParamItem.IndexOf(Definition.VARIABLE.STAR) < 0)
                llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, this.sParamItem);

            if (!string.IsNullOrEmpty(this.sEQPID) && this.sEQPID.IndexOf(Definition.VARIABLE.STAR) < 0)
                llstData.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this.sEQPID);

            DataSet ds = this._wsSPC.GetSPCProduct(llstData.GetSerialData());
            
            
            this.bChkProduct.Items.Clear();
            this.bChkProduct.Text = null;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {
                int icount = 0;                         
                SortedList stList = new SortedList();
                foreach (DataRow nRow in ds.Tables[0].Rows)
                {
                    string str = nRow[Definition.DynamicCondition_Condition_key.PRODUCT_ID].ToString();
                    string[] arr = str.Split(';');
                    if (str == Definition.VARIABLE.STAR)
                        icount++;

                    for(int i=0; i<arr.Length; i++)
                    {                                        
                        if(string.IsNullOrEmpty(arr[i])) continue;
                        if(stList.Contains(arr[i]))continue;

                        stList.Add(arr[i], stList.Count);                        
                    }
                }

                _comUtil.SetBCheckCombo(this.bChkProduct, GetDataSet(stList, ds,Definition.DynamicCondition_Condition_key.PRODUCT_ID, icount), Definition.DynamicCondition_Condition_key.PRODUCT_ID, Definition.DynamicCondition_Condition_key.PRODUCT_ID, this.arrProductID);
                this.sProductID = _comUtil.GetSelectedString(this.bChkProduct);

                if (this.arrProductID.Count == 0)
                {
                    this.bChkProduct.isSearchComboCheckBox = true;
                    this.bChkProduct.Text = Definition.VARIABLE.STAR;
                }
            }
         
        }

        private DataSet GetDataSet(SortedList _sList, DataSet ds , string _colName, int icount)
        {
            DataTable dt = new DataTable();
            DataSet dsResult = new DataSet();
            DataRow row = null;
            dt = ds.Tables[0].Clone();
            for (int i = 0; i < _sList.Count; i++)
            {
                row = dt.NewRow();
                row[_colName] = _sList.GetKey(i).ToString();
                dt.Rows.Add(row);

            }
            if (icount == 0)
            {
                row = dt.NewRow();
                row[_colName] = Definition.VARIABLE.STAR;
                dt.Rows.InsertAt(row, 0);
            }
            ds = new DataSet();
            dsResult.Tables.Add(dt);

            return dsResult;              
        }

        private void PROC_BindEQPID()
        {

            llstData.Clear();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);            
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);

            if (!string.IsNullOrEmpty(this.sEqpModel))
                llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.sEqpModel);
                
            llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.sParamType);

            if (!string.IsNullOrEmpty(this.sOperationID) && this.sOperationID.IndexOf(Definition.VARIABLE.STAR) < 0)
                llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this.sOperationID);

            if (!string.IsNullOrEmpty(this.sParamItem) && this.sParamItem.IndexOf(Definition.VARIABLE.STAR) < 0)
                llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, this.sParamItem);

            DataSet ds = this._wsSPC.GetSPCEQP(llstData.GetSerialData());

            this.bchkEqpID.Items.Clear();
            this.bchkEqpID.Text = null;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {
                int icount = 0;
                SortedList stList = new SortedList();
                foreach (DataRow nRow in ds.Tables[0].Rows)
                {
                    string str = nRow[Definition.DynamicCondition_Condition_key.EQP_ID].ToString();
                    string[] arr = str.Split(';');
                    if (str == Definition.VARIABLE.STAR)
                        icount++;

                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(arr[i])) continue;
                        if (stList.Contains(arr[i])) continue;

                        stList.Add(arr[i], stList.Count);
                    }
                }

                _comUtil.SetBCheckCombo(this.bchkEqpID, GetDataSet(stList, ds,Definition.DynamicCondition_Condition_key.EQP_ID, icount), Definition.DynamicCondition_Condition_key.EQP_ID, Definition.DynamicCondition_Condition_key.EQP_ID, this.arrEQPID);
                this.sEQPID = _comUtil.GetSelectedString(this.bchkEqpID);

                if (this.arrEQPID.Count == 0)
                {
                    this.bchkEqpID.isSearchComboCheckBox = true;
                    this.bchkEqpID.Text = Definition.VARIABLE.STAR;
                }
            }

        }

        private void PROC_BindComboType()
        {
            DataSet ds = null;
            LinkedList llstData = null;
            try
            {
                llstData = new LinkedList();
                llstData.Add(Definition.DynamicCondition_Condition_key.CATEGORY, Definition.CODE_CATEGORY_PERIOD_PPK);
                llstData.Add(Definition.DynamicCondition_Condition_key.USE_YN, "Y");
                ds = this._wsSPC.GetCodeData(llstData.GetSerialData());                
                if (!DataUtil.IsNullOrEmptyDataSet(ds))
                {
                    this.ppkDate.AddItems(ds);
                }
            }
            catch
            {
            }
            finally
            {
                if (llstData != null) llstData = null;
                if (ds != null) ds.Dispose();
            }
        }


        private void PROC_BindSortingKey()
        {
            DataSet ds = null;
            LinkedList llstData = null;
            try
            {               
                llstData = new LinkedList();
                llstData.Add(Definition.DynamicCondition_Condition_key.CATEGORY, Definition.CODE_CATEGORY_PPK_SORTING_KEY);
                llstData.Add(Definition.DynamicCondition_Condition_key.USE_YN, "Y");
                ds = this._wsSPC.GetCodeData(llstData.GetSerialData());                               
                this.sortUC.listBoxSKeyUnSelected.Items.Clear();
                if (!DataUtil.IsNullOrEmptyDataSet(ds))
                {
                    sortUC.AddItemsUnSelected(ds.Tables[0],Definition.CONDITION_KEY_CODE);                    
                }
            }
            catch
            {
            }
            finally
            {
                if (llstData != null) llstData = null;
                if (ds != null) ds.Dispose();
            }
        }
        
        private void llstRemove(string sSearchKey)
        {
            if (_condition.Contains(sSearchKey))
                _condition.Remove(sSearchKey);
        }


        private void llstAdd(string sSearchKey)
        {
            if (_condition.Contains(sSearchKey))
                _condition.Remove(sSearchKey);
        }


        private void SetCheckListBoxSelected(CheckedListBox chkListBox)
        {
            int iSelectedIndex = -1;              
            if (!chkListBox.CheckOnClick)
            {                       
                iSelectedIndex = chkListBox.SelectedIndex;
                if(chkListBox.GetSelected(iSelectedIndex))
                {
                    if (chkListBox.Items[iSelectedIndex].ToString() == Definition.VARIABLE.STAR)
                    {
                        for (int i = 1; i < chkListBox.Items.Count; i++)
                        {
                            chkListBox.SetItemChecked(i, false);                                                     
                        }
                    }                    
                    chkListBox.SetSelected(iSelectedIndex, true);
                }
            }
            else
            {
                chkListBox.SetSelected(chkListBox.SelectedIndex, false);
            }


        }




        
        #endregion


        #region Event

        private void bchkOperation_CheckCombo_Select(object sender, System.EventArgs e)
        {

            try
            {
                BCheckCombo bchkCbo = sender as BCheckCombo;
                if (bchkCbo == null) return;
                if (this.sOperationID != bchkCbo.SelectedValue)
                {
                    this.sOperationID = _comUtil.GetSelectedString(this.bchkOperation);
                    PROC_BindParameter();

                }

            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }

        }
        

        private void bchkParameter_CheckCombo_Select_Legend(object sender, System.EventArgs e)
        {
            try
            {
                BCheckCombo bchkCbo = sender as BCheckCombo;
                if (bchkCbo == null) return;

                if (bchkCbo.chkBox.SelectedItem != null)
                {
                    DataTable dt = (DataTable)bchkCbo.DataSource;
                    _comUtil.SetBCheckCombo(this.arrParameter, dt, bchkCbo);
                }

            }
            catch (System.Web.Services.Protocols.SoapException sex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, sex.Message);
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }          
           
        }


        void bchkParameter_CheckCombo_Select(object sender, System.EventArgs e)
        {
            try
            {
                BCheckCombo bchkCbo = sender as BCheckCombo;
                if (bchkCbo == null) return;
                if (bchkCbo.SelectedValue !=this.sParamItem)
                {
                    this.sParamItem = _comUtil.GetSelectedString(bchkCbo);
                    this.llstRemove(Definition.DynamicCondition_Search_key.EQP_ID);
                    this.PROC_BindEQPID();                    
                }

            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }



        private void bchkEqpID_CheckCombo_Select_Legend(object sender, System.EventArgs e)
        {
            try
            {
                BCheckCombo bchkCbo = sender as BCheckCombo;
                if (bchkCbo == null) return;

                if (bchkCbo.chkBox.SelectedItem != null)
                {                                                  
                   DataTable dt = (DataTable)bchkCbo.DataSource;
                   _comUtil.SetBCheckCombo(this.arrEQPID, dt, bchkCbo);                   
                }                                                                
            }
            catch (System.Web.Services.Protocols.SoapException sex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, sex.Message);
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }    
        }

        void bchkEqpID_CheckCombo_Select(object sender, System.EventArgs e)
        {
            try
            {
                BCheckCombo bchkCbo = sender as BCheckCombo;
                if (bchkCbo == null) return;

                if (bchkCbo.SelectedValue != this.sEQPID)
                {                    
                    this.sEQPID = _comUtil.GetSelectedString(this.bchkEqpID);                                        
                    this.llstRemove(Definition.DynamicCondition_Search_key.PRODUCT);
                    PROC_BindProduct();
                }


            }
            catch (System.Web.Services.Protocols.SoapException sex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, sex.Message);
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }    
        }



        void bChkProduct_CheckCombo_Select_Legend(object sender, System.EventArgs e)
        {
            try
            {
                BCheckCombo bchkCbo = sender as BCheckCombo;
                if (bchkCbo == null) return;

                if (bchkCbo.chkBox.SelectedItem != null)
                {                    
                    DataTable dt = (DataTable)bchkCbo.DataSource;
                    _comUtil.SetBCheckCombo(this.arrProductID, dt, bchkCbo);
                }                                                                  
                             
            }
            catch (System.Web.Services.Protocols.SoapException sex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, sex.Message);
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }    
            
        }


        void bChkProduct_CheckCombo_Select(object sender, System.EventArgs e)
        {

            try
            {
                BCheckCombo bchkCbo = sender as BCheckCombo;
                if (bchkCbo == null) return;

                if (bchkCbo.SelectedValue != this.sProductID)
                {                   
                    this.sProductID = _comUtil.GetSelectedString(bchkCbo);
                    this.llstRemove(Definition.DynamicCondition_Search_key.PERIOD_PPK);
                    this.llstRemove(Definition.DynamicCondition_Search_key.SORTING_KEY);
                }

            }
            catch (System.Web.Services.Protocols.SoapException sex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, sex.Message);
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }    
        }


        #endregion

   


    }
}
