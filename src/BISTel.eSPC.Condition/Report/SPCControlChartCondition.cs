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

namespace BISTel.eSPC.Condition.Report
{
  
    public partial class SPCControlChartCondition : ADynamicCondition
    {

        private string sLineRawid = string.Empty;
        private string sAreaRawid = string.Empty;
        private string sArea = string.Empty;
        
        private eSPCWebService.eSPCWebService _wsSPC = null;
        private MultiLanguageHandler _mlthandler;
        private Initialization _Initialization;
        private LinkedList llstData = new LinkedList();
        private LinkedList _condition = new LinkedList();
        private LinkedList _llstArea = new LinkedList();
        private LinkedList _llstOperation = new LinkedList();

        private string sParamType = string.Empty;
        private string sOperationID = string.Empty;
        private string sProductID = Definition.VARIABLE.STAR;
        private string sParamItem = string.Empty;
        private string sEQPID = Definition.VARIABLE.STAR;
        private string sStartTime = string.Empty;
        private string sEndTime = string.Empty;
        private string sEqpModel = string.Empty;
                
        private ArrayList arrEQPID = new ArrayList();
        private ArrayList arrProductID = new ArrayList();
        List<string> lstModelConfigRawID = new List<string>();
        private CommonUtility _comUtil = null;
        private int iSearch = 0;
                
        int restrict_sample_count=Definition.RESTRICT_SAMPLE_COUNT;
        int restrict_sample_days = Definition.RESTRICT_SAMPLE_DAYS;

        public SPCControlChartCondition()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._comUtil = new CommonUtility();
            this._Initialization = new Initialization();
            
            InitializeComponent();

            this.dateCondition1 = new BISTel.eSPC.Condition.Controls.Date.DateCondition();
            this.dateCondition1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPeriod.Controls.Add(this.dateCondition1);
                                   
            this.grpEpqID.Text = _mlthandler.GetVariable(Definition.LABEL.EQP_ID);
            this.grpOperation.Text = _mlthandler.GetVariable(Definition.LABEL.OPERATION_ID);
            this.grpParameter.Text = _mlthandler.GetVariable(Definition.LABEL.PARAMETER);
            this.grpProduct.Text = _mlthandler.GetVariable(Definition.LABEL.PRODUCT);
            this.grpPeriod.Text = _mlthandler.GetVariable(Definition.LABEL.PERIOD);
            
        }



        #region override
        public override LinkedList GetParameter(LinkedList ll)
        {
            if (this._Initialization == null){
               
            }

            string[] arrProductID = new string[this.bChkProduct.chkBox.CheckedItems.Count];
            string[] arrEQPValue = new string[this.bchkEqpID.chkBox.CheckedItems.Count];


         
            for (int i = 0; i < this.bchkEqpID.chkBox.CheckedItems.Count; i++)
            {
                arrEQPValue[i] = bchkEqpID.chkBox.CheckedItems[i].ToString();
            }

            for (int i = 0; i < this.bChkProduct.chkBox.CheckedItems.Count; i++)
            {
                arrProductID[i] = bChkProduct.chkBox.CheckedItems[i].ToString();
            }



            GetModelConfigRawID();
                                                                
            ll.Add(Definition.DynamicCondition_Search_key.OPERATION, DynamicDefinition.DynamicConditionSetting(this.sOperationID, this.sOperationID));
            ll.Add(Definition.DynamicCondition_Search_key.PARAM, DynamicDefinition.DynamicConditionSetting(this.sParamItem, this.sParamItem));
            ll.Add(Definition.DynamicCondition_Search_key.PRODUCT, DynamicDefinition.DynamicConditionSetting(arrProductID, arrProductID));
            ll.Add(Definition.DynamicCondition_Search_key.EQP_ID, DynamicDefinition.DynamicConditionSetting(arrEQPValue, arrEQPValue));
            if (lstModelConfigRawID.Count > 0)            
                ll.Add(Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID, DynamicDefinition.DynamicConditionSetting(lstModelConfigRawID.ToArray(), lstModelConfigRawID.ToArray()));                
            
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_PERIOD, DynamicDefinition.DynamicConditionSetting(this.dateCondition1.DateType, this.dateCondition1.DateType));
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, this.dateCondition1.GetDateTimeSelectedValue("F"));
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, this.dateCondition1.GetDateTimeSelectedValue("T"));
            ll.Add(Definition.DynamicCondition_Search_key.RESTRICT_SAMPLE_COUNT, DynamicDefinition.DynamicConditionSetting(this.restrict_sample_count.ToString(), this.restrict_sample_count.ToString()));
            ll.Add(Definition.DynamicCondition_Search_key.RESTRICT_SAMPLE_DAYS, DynamicDefinition.DynamicConditionSetting(this.restrict_sample_days.ToString(), this.restrict_sample_days.ToString()));

            if (_condition[Definition.DynamicCondition_Search_key.SEARCH_COUNT] != null)
            {
                iSearch = 1;
                ll.Add(Definition.DynamicCondition_Search_key.SEARCH_COUNT, DynamicDefinition.DynamicConditionSetting(iSearch.ToString(), iSearch.ToString()));
                return ll;
            }
            
            ll.Add(Definition.DynamicCondition_Search_key.SEARCH_COUNT, DynamicDefinition.DynamicConditionSetting(iSearch.ToString(),iSearch.ToString()));
            _condition.Add(Definition.DynamicCondition_Search_key.SEARCH_COUNT, DynamicDefinition.DynamicConditionSetting(iSearch.ToString(), iSearch.ToString()));
                        
            return ll;
        }
                        
        public override void RefreshCondition(LinkedList ll)
        {

            if (this._Initialization == null)
            {
              
            }
            iSearch = 0;

            DataTable dt = null;
            if (ll.Contains(Definition.DynamicCondition_Search_key.LINE))
                this.sLineRawid = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.LINE]);

            if (ll.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
            {
                this.sParamType = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.PARAM_TYPE]);
            }



            if (ll.Contains(Definition.DynamicCondition_Search_key.SEARCH_COUNT))
            {
                this.sLineRawid = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.SEARCH_COUNT]);
            }


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

        private void GetModelConfigRawID()
        {
            LinkedList _llstModelConfigRawID = new LinkedList();
            _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);            
            _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);

            if (!string.IsNullOrEmpty(this.sEqpModel))
            _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.sEqpModel);
            
            _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.sParamType);
            _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, GetConcat(this.sParamItem));
            _llstModelConfigRawID.Add(Definition.DynamicCondition_Condition_key.MAIN_YN, "Y");
            DataSet _dsModel = _wsSPC.GetSPCModelConfigSearch(_llstModelConfigRawID.GetSerialData());

            this.lstModelConfigRawID.Clear();
            if (!DataUtil.IsNullOrEmptyDataSet(_dsModel))
            {

                restrict_sample_count = 0;
                restrict_sample_days = 0; 
                foreach (DataRow dr in _dsModel.Tables[0].Rows)
                {                
                    string strModelCongifRawID = dr[Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID].ToString();  
                    int iRestrict_sample_count    =   int.Parse(dr[COLUMN.RESTRICT_SAMPLE_COUNT].ToString());
                    int iRestrict_sample_days   = int.Parse(dr[COLUMN.RESTRICT_SAMPLE_DAYS].ToString());
                    if (iRestrict_sample_count > restrict_sample_count)restrict_sample_count = iRestrict_sample_count;
                    if (iRestrict_sample_days > restrict_sample_days) restrict_sample_days = iRestrict_sample_days; 
                                                                
                    if (!this.lstModelConfigRawID.Contains(strModelCongifRawID))                    
                        this.lstModelConfigRawID.Add(strModelCongifRawID);                                       
                }
            }else
            {
                 restrict_sample_count = Definition.RESTRICT_SAMPLE_COUNT;
                 restrict_sample_days = Definition.RESTRICT_SAMPLE_DAYS;
            }


           
                 

        }

        private void SetCondition(LinkedList ll)
        {

            //여기서 ll이랑 _condition이랑 복제한다. 
            byte[] tmp = ll.GetSerialData();

            _condition.SetSerialData((byte[])tmp.Clone());

            if (_condition.Contains(Definition.DynamicCondition_Search_key.OPERATION))
                this.sOperationID = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.OPERATION]);

            if (_condition.Contains(Definition.DynamicCondition_Search_key.PARAM))
                this.sParamItem = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.PARAM]);

            if (_condition.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
                this.sParamType = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.PARAM_TYPE]);

            DataTable dt =null;            
            if (_condition.Contains(Definition.DynamicCondition_Search_key.EQP_ID))
            {
                dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.EQP_ID];
                this.sEQPID = SetStringList(dt, this.arrEQPID, this.sEQPID);                                                  
            }

            if (_condition.Contains(Definition.DynamicCondition_Search_key.PRODUCT))
            {
                dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.PRODUCT];
                this.sProductID = SetStringList(dt, this.arrProductID, this.sProductID);
            }

            if (_condition.Contains(Definition.DynamicCondition_Search_key.DATETIME_FROM))            
            {
                dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_FROM];
                this.sStartTime = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.VALUEDATA]);         
              
            }

            if (_condition.Contains(Definition.DynamicCondition_Search_key.DATETIME_TO))
            {
                dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_TO];
                this.sEndTime = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.VALUEDATA]);    
                     
            }
            

            this.PROC_BindOperation();            
            this.PROC_BindParameter();            
            this.PROC_BindEQPID();            
            this.PROC_BindProduct();
            GetModelConfigRawID();

            if (!string.IsNullOrEmpty(this.sStartTime))
            {
                this.dateCondition1.DateTimeEnd = DateTime.Parse(this.sEndTime);
                this.dateCondition1.DateTimeStart = DateTime.Parse(this.sStartTime);
            }
            else
            {
                this.dateCondition1.DateType = Definition.PERIOD_TYPE.CUSTOM;
                this.dateCondition1.DateTimeStart = DateTime.Now.AddDays(-restrict_sample_days);
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
        private string SetStringList(DataTable _dt ,ArrayList _arr,string _stringList)
        {                     
            _arr.Clear();
            _stringList = string.Empty;
            foreach (DataRow nRow in _dt.Rows)
            {
                _arr.Add(nRow[Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
                _stringList = GetConcat(_stringList, nRow[Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
            }   
            
            return   _stringList;      
        }

        private string GetConcat(string _stringList, string _svalue)
        {                                
            if(!string.IsNullOrEmpty(_stringList)) 
            _stringList+=",";                                    
            
            _stringList += "'" + _svalue+"'";
            
            return _stringList;
        }

        private string GetConcat(string _svalue)
        {
            return GetConcat(null, _svalue);
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
            this.bcboOperation.Text=null;
            this.bcboOperation.Items.Clear();            
            this._llstOperation.Clear();
            string sSelected = string.Empty;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {
                string sValue = string.Empty;   
                string sKey = string.Empty;                             
                for (int i=0; i<ds.Tables[0].Rows.Count; i++)
                {
                    sKey = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString();
                    sValue = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION].ToString();
                    
                    if(sKey==Definition.VARIABLE.STAR) continue;
                    if (!this.bcboOperation.Items.Contains(sValue))
                    {
                        this.bcboOperation.Items.Add(sValue);
                        this._llstOperation.Add(sValue, sKey);

                        if (sKey == this.sOperationID)
                            this.bcboOperation.SelectedIndex = this.bcboOperation.Items.Count-1; 
                                        
                    }

                    
                }
                    
                if(this.bcboOperation.SelectedIndex==-1)
                    this.bcboOperation.SelectedIndex = 0;
            }
        }

        private void PROC_BindParameter()
        {

            llstData.Clear();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);            
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);
            if (!string.IsNullOrEmpty(this.sEqpModel))
                llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.sEqpModel);
                        
            llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.sParamType);
            llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, GetConcat(this.sOperationID));
            DataSet ds = this._wsSPC.GetParamName(llstData.GetSerialData());
            this.bcboParameter.Items.Clear();
            this.bcboParameter.Text=null;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sKey = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString();
                    this.bcboParameter.Items.Add(sKey);
                                       
                    if (sKey == this.sParamItem)
                        this.bcboParameter.SelectedIndex = i;

                }
                if (this.bcboParameter.SelectedIndex == -1)
                    this.bcboParameter.SelectedIndex = 0;              
            }
        }



        private void PROC_BindProduct()
        {
            llstData.Clear();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);            
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);
            if (!string.IsNullOrEmpty(this.sEqpModel))
                llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.sEqpModel);
                
            llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.sParamType);
            if (!string.IsNullOrEmpty(this.sOperationID) && this.sOperationID.IndexOf(Definition.VARIABLE.STAR) < 0)
                llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, GetConcat(this.sOperationID));

            if (!string.IsNullOrEmpty(this.sParamItem) && this.sParamItem.IndexOf(Definition.VARIABLE.STAR) < 0)
                llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, GetConcat(this.sParamItem));

            if (!string.IsNullOrEmpty(this.sEQPID) && this.sEQPID.IndexOf(Definition.VARIABLE.STAR) < 0)
                llstData.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this.sEQPID);

            DataSet ds = this._wsSPC.GetSPCProduct(llstData.GetSerialData());


            this.bChkProduct.Items.Clear();
            this.bChkProduct.Text = null;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {
                int icount = 0;                
                foreach (DataRow nRow in ds.Tables[0].Rows)
                {
                    if (nRow[Definition.DynamicCondition_Condition_key.PRODUCT_ID].ToString() == Definition.VARIABLE.STAR)
                        icount++;
                }
                if (icount == 0)
                {
                    DataRow row = ds.Tables[0].NewRow();
                    row[Definition.DynamicCondition_Condition_key.PRODUCT_ID] = Definition.VARIABLE.STAR;
                    ds.Tables[0].Rows.InsertAt(row, 0);
                }
                _comUtil.SetBCheckCombo(this.bChkProduct, ds, Definition.DynamicCondition_Condition_key.PRODUCT_ID, Definition.DynamicCondition_Condition_key.PRODUCT_ID, this.arrProductID);
                this.sProductID = _comUtil.GetSelectedString(this.bChkProduct);
                
                if(this.arrProductID.Count==0)
                {
                    this.bChkProduct.isSearchComboCheckBox=true;
                    this.bChkProduct.chkBox.SelectedItem = Definition.VARIABLE.STAR;
                    this.bChkProduct.Text = Definition.VARIABLE.STAR;
                }
            }

        }

        private void PROC_BindEQPID()
        {

            llstData.Clear();
            llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);            
            llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);
            if (!string.IsNullOrEmpty(this.sEqpModel))
                llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.sEqpModel);
            llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this.sParamType);

            if (!string.IsNullOrEmpty(this.sOperationID))
                llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, GetConcat(this.sOperationID));

            if (!string.IsNullOrEmpty(this.sParamItem))
                llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, GetConcat(this.sParamItem));

            DataSet ds = this._wsSPC.GetSPCEQP(llstData.GetSerialData());

            this.bchkEqpID.Items.Clear();
            this.bchkEqpID.Text=null;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {
                int icount = 0;
                foreach (DataRow nRow in ds.Tables[0].Rows)
                {
                    if (nRow[Definition.DynamicCondition_Condition_key.EQP_ID].ToString() == Definition.VARIABLE.STAR)
                        icount++;
                }
                if (icount == 0)
                {
                    DataRow row = ds.Tables[0].NewRow();
                    row[Definition.DynamicCondition_Condition_key.EQP_ID] = Definition.VARIABLE.STAR;                                        
                    ds.Tables[0].Rows.InsertAt(row, 0);
                }
                _comUtil.SetBCheckCombo(this.bchkEqpID, ds, Definition.DynamicCondition_Condition_key.EQP_ID, Definition.DynamicCondition_Condition_key.EQP_ID, this.arrEQPID);

                if (this.arrEQPID.Count==0)    
                {                
                    this.bchkEqpID.isSearchComboCheckBox=true; //chkBox.SetSelected(0, true);
                    this.bchkEqpID.chkBox.SelectedItem = Definition.VARIABLE.STAR;
                    this.bchkEqpID.Text = Definition.VARIABLE.STAR;
                    
                }
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
                    else
                    {                    
                        chkListBox.SetItemChecked(0, false);
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

        

        private void bcboOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.llstRemove(Definition.DynamicCondition_Search_key.OPERATION);
                this.llstRemove(Definition.DynamicCondition_Search_key.PARAM);
                this.llstRemove(Definition.DynamicCondition_Search_key.PRODUCT);
                this.llstRemove(Definition.DynamicCondition_Search_key.EQP_ID);

                BComboBox bcbo = sender as BComboBox;
                if (bcbo.SelectedIndex > -1)
                {                    
                    this.sOperationID =this._llstOperation.GetValue(bcbo.SelectedIndex).ToString();
                }                    

                this.bcboParameter.Items.Clear();
                this.bChkProduct.Items.Clear();
                this.bchkEqpID.Items.Clear();
                PROC_BindParameter();


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


        private void bcboParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.llstRemove(Definition.DynamicCondition_Search_key.PARAM);
            this.llstRemove(Definition.DynamicCondition_Search_key.PRODUCT);
            this.llstRemove(Definition.DynamicCondition_Search_key.EQP_ID);

            BComboBox bcbo = sender as BComboBox;

            if (bcbo.SelectedIndex > -1)
                this.sParamItem = bcbo.SelectedItem.ToString();

            this.bchkEqpID.Items.Clear();                        
            PROC_BindEQPID();           
            PROC_BindProduct();
        }


        private void chkProduct_Click(object sender, System.EventArgs e)
        {
            CheckedListBox chkListBox = sender as CheckedListBox;
            if (chkListBox == null) return;
            SetCheckListBoxSelected(chkListBox);
           
            this.llstRemove(Definition.DynamicCondition_Search_key.PRODUCT);
        }


        void bchkEqpID_CheckCombo_Select_Legend(object sender, System.EventArgs e)
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
                this.llstRemove(Definition.DynamicCondition_Search_key.EQP_ID);
                this.llstRemove(Definition.DynamicCondition_Search_key.PRODUCT);
                    
                BCheckCombo bchkCbo = sender as BCheckCombo;
                if (bchkCbo == null) return;

                if (bchkCbo.SelectedValue != this.sEQPID)
                {                    
                    this.sEQPID = _comUtil.GetSelectedString(this.bchkEqpID);                    
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
                iSearch = 0;
                this.llstRemove(Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID);
                this.llstRemove(Definition.DynamicCondition_Search_key.DATETIME_PERIOD);
                this.llstRemove(Definition.DynamicCondition_Search_key.DATETIME_FROM);
                this.llstRemove(Definition.DynamicCondition_Search_key.DATETIME_TO);
                this.llstRemove(Definition.DynamicCondition_Search_key.RESTRICT_SAMPLE_COUNT);
                this.llstRemove(Definition.DynamicCondition_Search_key.RESTRICT_SAMPLE_DAYS);
                this.llstRemove(Definition.DynamicCondition_Search_key.SEARCH_COUNT);

                BCheckCombo bchkCbo = sender as BCheckCombo;
                if (bchkCbo == null) return;

                if (bchkCbo.SelectedValue != this.sProductID)
                {
                    this.sProductID = _comUtil.GetSelectedString(bchkCbo);                   
                }
                GetModelConfigRawID();
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
