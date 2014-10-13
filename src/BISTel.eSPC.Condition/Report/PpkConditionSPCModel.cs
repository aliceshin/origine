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
    public partial class PpkConditionSPCModel : ADynamicCondition
    {
        private string sLineRawid = string.Empty;
        private string sAreaRawid = string.Empty;
        private eSPCWebService.eSPCWebService _wsSPC = null;
        private MultiLanguageHandler _mlthandler;
        private Initialization _Initialization;        
        private LinkedList _condition = new LinkedList();        

        private string sArea = string.Empty;
        private string sParamType = string.Empty;        
        private string sPpkPeriod = Definition.PERIOD_PPK.DAILY;
        private string sStartTime = string.Empty;
        private string sEndTime = string.Empty;        
               
        private CommonUtility _comUtil = null;
        private List<string> lstType  = new List<string>();        
        BISTel.eSPC.Condition.Controls.Date.PpkDateCondition  ppkDate;

        public PpkConditionSPCModel()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._comUtil = new CommonUtility();            
            this.ppkDate = new BISTel.eSPC.Condition.Controls.Date.PpkDateCondition();

            InitializeComponent();                        
            InitializeLayout();      
        }

        private void InitializeLayout()
        {
            DataSet ds = null;
            LinkedList llstData = null;
            try
            {                                                                                                         
                ppkDate.InitializePage();
                ppkDate.Dock = DockStyle.Fill;
                this.pnlPeriod.Controls.Add(ppkDate);
                this.PROC_BindComboType();                
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

            this.llstRemove(Definition.DynamicCondition_Search_key.DATETIME_PERIOD);
            this.llstRemove(Definition.DynamicCondition_Search_key.DATETIME_FROM);
            this.llstRemove(Definition.DynamicCondition_Search_key.DATETIME_TO);
            this.llstRemove(Definition.DynamicCondition_Search_key.PERIOD_PPK);
            
                                                
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_PERIOD, DynamicDefinition.DynamicConditionSetting("CUSTOM", "CUSTOM"));
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_FROM, this.ppkDate.GetDateTimeSelectedValue("F"));
            ll.Add(Definition.DynamicCondition_Search_key.DATETIME_TO, this.ppkDate.GetDateTimeSelectedValue("T"));
            ll.Add(Definition.DynamicCondition_Search_key.PERIOD_PPK, DynamicDefinition.DynamicConditionSetting(this.ppkDate.PeriodType, this.ppkDate.PeriodType));                                                                   
            return ll;
        }

        public override void RefreshCondition(LinkedList ll)
        {
            if(ll == null)
                return;

            if (this._Initialization == null)
            {
                                          
            }

            if (ll.Contains(Definition.DynamicCondition_Search_key.LINE))
                this.sLineRawid = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.LINE]);


            if (ll.Contains(Definition.DynamicCondition_Search_key.PARAM_TYPE))
                this.sParamType = DCUtil.GetValueData((DataTable)ll[Definition.DynamicCondition_Search_key.PARAM_TYPE]);
            
            SetCondition(ll);
        }


        #endregion


        #region User Method


        private void llstRemove(string sSearchKey)
        {
            if (_condition.Contains(sSearchKey))
                _condition.Remove(sSearchKey);
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


        private void SetCondition(LinkedList ll)
        {
            DataTable dt = null;
            try
            {                
                //여기서 ll이랑 _condition이랑 복제한다. 
                byte[] tmp = ll.GetSerialData();

                _condition.SetSerialData((byte[])tmp.Clone());

     
                if (_condition.Contains(Definition.DynamicCondition_Search_key.AREA))
                    this.sArea = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.VALUEDATA]);                
                        
              
                //SPC_PERIOD_PPK
                if (_condition.Contains(Definition.DynamicCondition_Search_key.PERIOD_PPK))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.PERIOD_PPK];    
                    this.ppkDate.PeriodType  = dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();                                             
                }



                if (_condition.Contains(Definition.DynamicCondition_Search_key.DATETIME_FROM))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_FROM];
                    this.sStartTime = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_FROM]);

                    this.ppkDate.DateTimeStart = DateTime.Parse(this.sStartTime);



                }

                if (_condition.Contains(Definition.DynamicCondition_Search_key.DATETIME_TO))
                {
                    dt = (DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_TO];
                    this.sEndTime = DCUtil.GetValueData((DataTable)_condition[Definition.DynamicCondition_Search_key.DATETIME_TO]);
                    this.ppkDate.DateTimeEnd = DateTime.Parse(this.sEndTime);
                }
            
                                                                                 
            
            } catch
            {
            
            }finally
            {
                if(dt !=null) dt.Dispose();
            }         

        }

        #endregion


        #region Event
        

        #endregion

      
        

      
  
 



    }
}
