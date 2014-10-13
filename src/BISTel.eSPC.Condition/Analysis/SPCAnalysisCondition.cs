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
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;


using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Default;

namespace BISTel.eSPC.Condition.Analysis
{
    public partial class SPCAnalysisCondition : ADynamicCondition
    {
        BISTel.eSPC.Condition.Controls.Date.DateCondition dateCondition1;
        
        
        LinkedList _llstData = new LinkedList();
        LinkedList _llstOperation = new LinkedList();
        LinkedList _condition = new LinkedList();
        
        DataSet _dsSubItem = new DataSet();
        
        ArrayList _arrChartList = new ArrayList();
        ArrayList _arrSubData = new ArrayList();


        List<string> _lstRawColumn;
        DateTime _DateTimeStart = DateTime.Now.AddDays(-30);
        DateTime _DateTimeEnd = DateTime.Now;

        CommonUtility _comUtil;
        MultiLanguageHandler _mlthandler;
        Initialization _Initialization;
        
        eSPCWebService.eSPCWebService _wsSPC;
                       
        string _Site = string.Empty;
        string _Fab = string.Empty;

        string SPC_PARAM_TYPE_MET = "MET";
        string SPC_PARAM_TYPE_PROC = "PROC";
        string SPC_PARAM_TYPE_EVS = "EVS";
        string _complex_yn = "N";

        string _sSortingKey = string.Empty;
        string _sTargetType = "MET";
        string _sTargetOperationID = string.Empty;
        string _sParamItem = string.Empty;
        string _sOperationID = string.Empty;
        string _sAreaRawID = string.Empty;        
        string _sLineRawID = string.Empty;
        string _sSubItem = string.Empty;
        string _sParamType = string.Empty;
        string _sChartList = string.Empty;
        string _sEqpModel = string.Empty;
        

        string _sStartTime = string.Empty;
        string _sEndTime = string.Empty;


        public SPCAnalysisCondition()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._comUtil = new CommonUtility();       
            this._Initialization = new Initialization();
                                               
            InitializeComponent();
            this.dateCondition1 = new BISTel.eSPC.Condition.Controls.Date.DateCondition();
            this.dateCondition1.Dock = DockStyle.Fill;
            this.bTitlePnlPeriod.Controls.Add(dateCondition1);  
            
            this.grpOperation.Text = _mlthandler.GetVariable(Definition.LABEL.OPERATION_ID);
            this.grpParamItem.Text = _mlthandler.GetVariable(Definition.LABEL.PARAMETER);                        
            this.grpSubData.Text = _mlthandler.GetVariable(Definition.Analysis.LABEL_SUB_DATA);
            this.grpTarget.Text = _mlthandler.GetVariable(Definition.Analysis.LABEL_TARGET);
            this.grpChartList.Text = _mlthandler.GetVariable(Definition.Analysis.LABEL_CHART_LIST);
            this.bTitlePnlPeriod.Title = _mlthandler.GetVariable(Definition.LABEL.PERIOD);
                       
            InitializePage();            
        }


        private void InitializePage()
        {
            GetSubItem();
            

            this.dateCondition1.DateType = "CUSTOM";
            this.dateCondition1.DateTimeStart = DateTime.Now.AddDays(-7);
            this.dateCondition1.DateTimeEnd = DateTime.Now;
        }
 
        private void InitializeLayout()
        {
           
        }



        #region override
        public override LinkedList GetParameter(LinkedList ll)
        {
            if (this._Initialization == null)
            {

            }


            List<string> arrChartList = new List<string>();
            List<string> arrSubData = new List<string>();
            
            string sTargetType = SPC_PARAM_TYPE_MET;
            string sTarget = string.Empty;
            string sSortingKey = string.Empty;
             DataTable dt = null;
            
                        
            if(this.brbMet.Checked)
                sTargetType = SPC_PARAM_TYPE_MET;
            else
                sTargetType = SPC_PARAM_TYPE_PROC;      
                
              if(this.bcboTargetOperation.SelectedIndex >-1)
              {
                 dt = (DataTable)this.bcboTargetOperation.DataSource;
                 sTarget = dt.Rows[this.bcboTargetOperation.SelectedIndex][Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString();
              }
              

            dt = (DataTable)this.bcboSortingKey.DataSource;
            if(this.bcboSortingKey.SelectedIndex > -1)
            {            
               sSortingKey = dt.Rows[this.bcboSortingKey.SelectedIndex][Definition.DynamicCondition_Condition_key.CODE].ToString();                
            }
                            
                            
            dt = (DataTable)this.bchkSubData.DataSource;
            for (int i = 0; i < this.bchkSubData.Items.Count; i++)
            {
                string sValue = dt.Rows[i][Definition.DynamicCondition_Condition_key.CODE_NAME].ToString();
                if (this.bchkSubData.chkBox.CheckedItems.Contains(sValue))
                {
                    arrSubData.Add(dt.Rows[i][Definition.DynamicCondition_Condition_key.CODE].ToString());
                }
            }               

            dt = (DataTable)this.bchkChartList.DataSource;
            for(int  i=0; i< this.bchkChartList.Items.Count; i++)
            {
                string sValue = dt.Rows[i][Definition.DynamicCondition_Condition_key.CODE_NAME].ToString();
                if(this.bchkChartList.chkBox.CheckedItems.Contains(sValue))
                {
                    arrChartList.Add(dt.Rows[i][Definition.DynamicCondition_Condition_key.CODE].ToString());                                    
                }                
            }

            ll.Add(Definition.DynamicCondition_Search_key.OPERATION, DynamicDefinition.DynamicConditionSetting(this._sOperationID, this._sOperationID));
            ll.Add(Definition.DynamicCondition_Search_key.PARAM, DynamicDefinition.DynamicConditionSetting(this._sParamItem, this._sParamItem));
            ll.Add(Definition.DynamicCondition_Search_key.SUBDATA, DynamicDefinition.DynamicConditionSetting(arrSubData.ToArray(), arrSubData.ToArray()));
            ll.Add(Definition.DynamicCondition_Search_key.SORTING_KEY, DynamicDefinition.DynamicConditionSetting(sSortingKey, sSortingKey));

            ll.Add(Definition.DynamicCondition_Search_key.TARGET_TYPE, DynamicDefinition.DynamicConditionSetting(sTargetType, sTargetType));
            ll.Add(Definition.DynamicCondition_Search_key.TARGET, DynamicDefinition.DynamicConditionSetting(sTarget, sTarget));
            ll.Add(Definition.DynamicCondition_Search_key.CHART_LIST, DynamicDefinition.DynamicConditionSetting(arrChartList.ToArray(), arrChartList.ToArray()));
            

            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_PERIOD, DynamicDefinition.DynamicConditionSetting(this.dateCondition1.DateType, this.dateCondition1.DateType));
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, this.dateCondition1.GetDateTimeSelectedValue("F"));
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, this.dateCondition1.GetDateTimeSelectedValue("T"));
            
            return ll;
                      
        }


        public override void RefreshCondition(LinkedList ll)
        {

            if (this._Initialization == null)
            {

            }

            if (ll.Contains(Definition.DynamicCondition_Search_key.LINE))
                this._sLineRawID = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.LINE]);


            if (ll.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
                this._sParamType = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.PARAM_TYPE]);

            DataTable dt=null;
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


                if (_condition.Contains(Definition.DynamicCondition_Search_key.OPERATION))
                {
                    this._sOperationID = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.OPERATION]);
                }

                if (_condition.Contains(Definition.DynamicCondition_Search_key.PARAM))
                {
                    this._sParamItem = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.PARAM]);
                }


                if (_condition.Contains(Definition.DynamicCondition_Search_key.SUBDATA))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.SUBDATA];
                    this._arrSubData = _comUtil.GetConditionKeyArrayList(dt, Definition.DynamicCondition_Condition_key.VALUEDATA);
                }


                if (_condition.Contains(Definition.DynamicCondition_Search_key.TARGET_TYPE))
                {                    
                    this._sTargetType = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.TARGET_TYPE]);
                }


                if (_condition.Contains(Definition.DynamicCondition_Search_key.TARGET))
                {                    
                    this._sTargetOperationID = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.TARGET]);
                }

                if (_condition.Contains(Definition.DynamicCondition_Search_key.SORTING_KEY))
                {                    
                    this._sSortingKey = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.SORTING_KEY]);
                }


                //SPC_PERIOD_PPK
                if (_condition.Contains(Definition.DynamicCondition_Search_key.DATETIME_PERIOD))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_PERIOD];
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

                if (_condition.Contains(Definition.DynamicCondition_Search_key.CHART_LIST))
                {                
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.CHART_LIST];
                    this._arrChartList = _comUtil.GetConditionKeyArrayList(dt, Definition.DynamicCondition_Search_key.VALUEDATA);   
                                                                                
                }
                
                this.PROC_BindMetOperation();
                this.PROC_BindParameter();                
                this.PROC_BindSubData();
                this.PROC_BindSortingKey();
                this.PROC_BindChartList();
                                
                if(_sTargetType == this.SPC_PARAM_TYPE_MET)
                {
                    this.brbMet.Checked = true;
                    this.brbProc.Checked =false;
                }
                else if (_sTargetType == this.SPC_PARAM_TYPE_PROC)
                {
                    this.brbMet.Checked = false;
                    this.brbProc.Checked = true;                
                }else
                {
                    this.brbMet.Checked = true;
                    this.brbProc.Checked = false;                 
                }
                                                               
                this.PROC_BindTargetOperation();

                        
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
 
        #region Properties
        public DateTime DateTimeStart
        {
            set { this._DateTimeStart = value; }
            get { return this._DateTimeStart; }
        }

        public DateTime DateTimeEnd
        {
            set { this._DateTimeEnd = value; }
            get { return this._DateTimeEnd; }
        }

       
        #endregion 
        #region User Defiend

        private void llstRemove(string sSearchKey)
        {
            if (_condition.Contains(sSearchKey))
                _condition.Remove(sSearchKey);
        }
        
        private void PROC_BindChartList()
        {
            _llstData.Clear();
            _llstData.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_ANALYSIS_CHART);
            _llstData.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            DataSet ds = this._wsSPC.GetCodeData(_llstData.GetSerialData());

            if (ds != null && ds.Tables.Count > 0)
            {
                for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                {
                    if (ds.Tables[0].Rows[i][Definition.CONDITION_KEY_CODE].ToString() == Definition.CHART_TYPE.CA)
                        ds.Tables[0].Rows[i].Delete();
                }
                ds.AcceptChanges();
            }

            this.bchkChartList.DataSource = null;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {                                
                this._comUtil.SetBCheckCombo(this.bchkChartList, ds, Definition.CONDITION_KEY_NAME, Definition.CONDITION_KEY_CODE, this._arrChartList);                                         
            }
        }

        #region Target

        private void GetSubItem()
        {
            _llstData.Clear();
            _llstData.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_SUBITEM);
            _llstData.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            this._dsSubItem = this._wsSPC.GetCodeData(_llstData.GetSerialData());
        }
        
        private void PROC_BindSortingKey()
        {
            _llstData.Clear();
            _llstData.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_SORTING_KEY);
            _llstData.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            DataSet ds = this._wsSPC.GetCodeData(_llstData.GetSerialData());            
            this.bcboSortingKey.DataSource = null;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {

                this._comUtil.SetBComboBoxData(this.bcboSortingKey, ds, Definition.CONDITION_KEY_NAME
                    , Definition.CONDITION_KEY_CODE, this._sSortingKey, true);                                  
            }
        }

        private void PROC_BindTargetOperation()
        {
            DataSet _dsCode = null;
            if (brbMet.Checked)
            {
                _llstData.Clear();
                _llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_TYPE, "METROLOGY");
            }
            else
            {
                _llstData.Clear();
                _llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_TYPE, "PROCESSING");
            }
            _dsCode = _wsSPC.GetOperationID(_llstData.GetSerialData());
            this.bcboTargetOperation.DataSource = null;
            if (!DataUtil.IsNullOrEmptyDataSet(_dsCode))
            {
            
                this._comUtil.SetBComboBoxData(bcboTargetOperation, _dsCode, Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION
                , Definition.DynamicCondition_Condition_key.OPERATION_ID, this._sTargetOperationID, true);
                         
            }
        }
                
        #endregion 
        
        
        private void PROC_BindMetOperation()
        {
            
            this.bcboOperation.TextChanged -= new EventHandler(bcboOperation_TextChanged);
            _llstData.Clear();
            _llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this._sLineRawID);
            _llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this._sParamType);
            _llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this._sAreaRawID);
            if (!string.IsNullOrEmpty(this._sEqpModel))
                _llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this._sEqpModel);
            
            DataSet ds = this._wsSPC.GetSPCOperation(_llstData.GetSerialData());

            this.bcboOperation.Items.Clear();
            this._llstOperation.Clear();
            string sSelected = string.Empty;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {
                string sValue = string.Empty;
                string sKey = string.Empty;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sKey = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.OPERATION_ID].ToString();
                    sValue = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.CODE_DESCRIPTION].ToString();

                    if (sKey == Definition.VARIABLE.STAR) continue;
                    if (!this.bcboOperation.Items.Contains(sValue))
                    {
                        this.bcboOperation.Items.Add(sValue);
                        this._llstOperation.Add(sValue, sKey);

                        if (sKey == this._sOperationID)
                        {
                            this.bcboOperation.SelectedIndex = this.bcboOperation.Items.Count - 1;
                        }
                    }

                }

                if (this.bcboOperation.SelectedIndex == -1)
                    this.bcboOperation.SelectedIndex = 0;
            }

            if (this.bcboOperation.SelectedIndex > -1)
                this._sOperationID= this._llstOperation.GetValue(this.bcboOperation.SelectedIndex).ToString();
            else
                this._sOperationID = string.Empty;

            this.bcboOperation.TextChanged += new EventHandler(bcboOperation_TextChanged);

        }

       

        private void PROC_BindParameter()
        {
                                
            this.bcboParameter.TextChanged -= new System.EventHandler(this.bcboParameter_TextChanged);
            
            _llstData.Clear();
            _llstData.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this._sLineRawID);
            _llstData.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this._sAreaRawID);
            if (!string.IsNullOrEmpty(this._sEqpModel))
                _llstData.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this._sEqpModel);
                
            _llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this._sParamType);
            _llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, string .Format("'{0}'",this._sOperationID));
            
            DataSet ds = this._wsSPC.GetParamName(_llstData.GetSerialData());
            this.bcboParameter.Items.Clear();
            this.bcboParameter.Text = null;
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sKey = ds.Tables[0].Rows[i][Definition.DynamicCondition_Condition_key.PARAM_ALIAS].ToString();
                    this.bcboParameter.Items.Add(sKey);

                    if (sKey == this._sParamItem)
                        this.bcboParameter.SelectedIndex = i;

                }
                if (this.bcboParameter.SelectedIndex == -1)
                    this.bcboParameter.SelectedIndex = 0;
            }

            if (this.bcboParameter.SelectedIndex > -1)
                this._sParamItem = this.bcboParameter.Text;
            else
                this._sParamItem = string.Empty;

            this.bcboParameter.TextChanged += new EventHandler(bcboParameter_TextChanged);
        }

  
        
        private void PROC_BindSubData()
        {
            DataSet ds = null;            
            try
            {                
                _llstData.Clear();                                       
                _llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, this._sParamType);
                _llstData.Add(Definition.DynamicCondition_Condition_key.PARAM_ALIAS, this._sParamItem);
                _llstData.Add(Definition.DynamicCondition_Condition_key.OPERATION_ID, this._sOperationID);
                ds = this._wsSPC.GetParamList(_llstData.GetSerialData());                
                
                this.bchkSubData.Items.Clear();
                if (!DataUtil.IsNullOrEmptyDataSet(ds))
                {
                    string sValue = ds.Tables[0].Rows[0][COLUMN.PARAM_LIST].ToString();
                    string[] arr = sValue.Split(';');                                        
                    DataTable dt = _dsSubItem.Tables[0].Copy();                  
                    DataRow dr = null;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        dr = dt.NewRow();
                        if (string.IsNullOrEmpty(arr[i])) continue;
                        if (!this.bchkSubData.Items.Contains(arr[i]))   
                        {       
                            dr[Definition.CONDITION_KEY_CODE]=  arr[i];
                            dr[Definition.CONDITION_KEY_NAME] = arr[i];                                             
                        }
                        dt.Rows.Add(dr);
                    }
                    ds= new DataSet();
                    ds.Tables.Add(dt);
                    this._comUtil.SetBCheckCombo(this.bchkSubData, ds, Definition.CONDITION_KEY_NAME, Definition.CONDITION_KEY_CODE, this._arrSubData);                    
                }
               
            }
            catch
            {
            }
            finally
            {
                if (ds != null) ds.Dispose();               
            }     
        }
        
            
        #endregion



        #region Event



        private void bcboOperation_TextChanged(object sender, EventArgs e)
        {

            BComboBox cbo = sender as BComboBox;
            this._sOperationID = this._llstOperation.GetValue(cbo.SelectedIndex).ToString();
            this.PROC_BindParameter();
            this.PROC_BindSubData();            
            
        }


        private void bcboParameter_TextChanged(object sender, EventArgs e)
        {
            BComboBox cbo = sender as BComboBox;
            this._sParamItem = cbo.Text;         
            this.PROC_BindSubData();
        }
        
       


        private void brbMet_CheckedChanged(object sender, EventArgs e)
        {
            BRadioButton brdo = sender as BRadioButton;

            if (brdo.Checked)
            {
                this.brbProc.Checked = false;             
            }
            else
            {
                this.brbProc.Checked = true;
                brdo.Checked =false;
            }
            
            this.PROC_BindTargetOperation();
            
        }

        private void brbProc_CheckedChanged(object sender, EventArgs e)
        {
            BRadioButton brdo = sender as BRadioButton;


            if (brdo.Checked)
            {
                this.brbMet.Checked = false;
            }
            else
            {
                this.brbMet.Checked = true;
                brdo.Checked = false;
            }                        
            this.PROC_BindTargetOperation();
            
        }
        
        #endregion
        
    }
}
