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

namespace BISTel.eSPC.Condition.Modeling
{
    public partial class ContextCondition : ADynamicCondition
    {
        private string sLineRawid = string.Empty;
        private string sAreaRawid = string.Empty;
        private eSPCWebService.eSPCWebService _wsSPC = null;
        private LinkedList llstData = new LinkedList();
        private LinkedList _condition = new LinkedList();
        private LinkedList _llstArea = new LinkedList();


        private string _sSPCModelLevel = string.Empty;
        private string sArea = string.Empty;
        private string sEqpModel = string.Empty;


        private ArrayList arrEQPID = new ArrayList();
        private ArrayList arrModuleID = new ArrayList();
        private ArrayList arrRecipeID = new ArrayList();
        private ArrayList arrStepID = new ArrayList();
        private CommonUtility _comUtil = null;
        private List<string> lstType = new List<string>();
        private ListBox lstSorting = new ListBox();

        public ContextCondition()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._comUtil = new CommonUtility();

            InitializeComponent();

            this.bcboModuleID.DropDownWidth = 300;

            LinkedList llstCondtion = new LinkedList();
            llstCondtion.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_MODEL_LEVEL");
            llstCondtion.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            //llstCondtion.Add(Definition.CONDITION_KEY_DEFAULT_COL, "Y");

            DataSet dsModelLevel = this._wsSPC.GetCodeData(llstCondtion.GetSerialData());

            if (DSUtil.CheckRowCount(dsModelLevel))
            {
                this._sSPCModelLevel = dsModelLevel.Tables[0].Rows[0][COLUMN.CODE].ToString();
            }
        }

        #region override
        public override LinkedList GetParameter(LinkedList ll)
        {

            if (this.bcboEQPID.SelectedItem != null)
            {
                if (this.bcboEQPID.SelectedItem.ToString().Length > 0)
                {
                    string sEQPID = this.bcboEQPID.SelectedItem.ToString();
                    DataTable dt = DCUtil.MakeDataTableForDCValue(sEQPID, sEQPID);
                    if (ll.Contains(Definition.DynamicCondition_Condition_key.EQP_ID))
                    {
                        ll[Definition.DynamicCondition_Condition_key.EQP_ID] = dt;
                    }
                    else
                    {
                        ll.Add(Definition.DynamicCondition_Condition_key.EQP_ID, dt);
                    }
                }

            }

            if (this.bcboModuleID.SelectedItem != null)
            {
                if (this.bcboModuleID.SelectedItem.ToString().Length > 0)
                {
                    string sModuleID = this.bcboModuleID.SelectedItem.ToString();
                    DataTable dt = DCUtil.MakeDataTableForDCValue(sModuleID, sModuleID);
                    if (ll.Contains(Definition.DynamicCondition_Condition_key.MODULE_ID))
                    {
                        ll[Definition.DynamicCondition_Condition_key.MODULE_ID] = dt;
                    }
                    else
                    {
                        ll.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, dt);
                    }
                }

            }

            if (this.bcboRecipeID.SelectedItem != null)
            {
                if (this.bcboRecipeID.SelectedItem.ToString().Length > 0)
                {
                    string sRecipeID = this.bcboRecipeID.SelectedItem.ToString();
                    DataTable dt = DCUtil.MakeDataTableForDCValue(sRecipeID, sRecipeID);
                    if (ll.Contains(Definition.DynamicCondition_Condition_key.RECIPE_ID))
                    {
                        ll[Definition.DynamicCondition_Condition_key.RECIPE_ID] = dt;
                    }
                    else
                    {
                        ll.Add(Definition.DynamicCondition_Condition_key.RECIPE_ID, dt);
                    }
                }

            }

            if (this.bcboStepID.SelectedItem != null)
            {
                if (this.bcboStepID.SelectedItem.ToString().Length > 0)
                {
                    string sStepID = this.bcboStepID.SelectedItem.ToString();
                    DataTable dt = DCUtil.MakeDataTableForDCValue(sStepID, sStepID);
                    if (ll.Contains(Definition.DynamicCondition_Condition_key.STEP_ID))
                    {
                        ll[Definition.DynamicCondition_Condition_key.STEP_ID] = dt;
                    }
                    else
                    {
                        ll.Add(Definition.DynamicCondition_Condition_key.STEP_ID, dt);
                    }
                }
            }

            return ll;
        }




        public override void RefreshCondition(LinkedList ll)
        {
            this.sLineRawid = "";
            this.sAreaRawid = "";
            this.sEqpModel = "";

            DataTable dt = null;

            if (ll.Contains(Definition.DynamicCondition_Search_key.LINE))
            {
                dt = (DataTable)ll[Definition.DynamicCondition_Search_key.LINE];
                this.sLineRawid = DCUtil.GetValueData(dt);
            }

            if (ll.Contains(Definition.DynamicCondition_Search_key.AREA))
            {
                dt = (DataTable)ll[Definition.DynamicCondition_Search_key.AREA];
                this.sAreaRawid = DataUtil.GetConditionKeyDataList(dt, DCUtil.VALUE_FIELD);
            }

            if (this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
            {
                if (ll.Contains(Definition.CONDITION_SEARCH_KEY_EQPMODEL))
                {
                    dt = (DataTable)ll[Definition.CONDITION_SEARCH_KEY_EQPMODEL];
                    this.sEqpModel = DataUtil.GetConditionKeyDataList(dt, DCUtil.VALUE_FIELD);
                }
            }

            SetCondition();
        }


        #endregion


        #region User Method



        private void SetCondition()
        {
            try
            {
                this.bcboEQPID.Items.Clear();
                this.bcboModuleID.Items.Clear();
                this.bcboRecipeID.Items.Clear();
                this.bcboStepID.Items.Clear();

                if(this.sLineRawid == "" || this.sAreaRawid == "") return;

                if(this._sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL) && this.sEqpModel == "") return;

                LinkedList lnkCondition = new LinkedList();
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.sEqpModel);


                DataSet ds = this._wsSPC.GetSPCCalcContextKey(lnkCondition.GetSerialData());

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    this.PROC_BindEQPID(ds);
                    this.PROC_BindModuleID(ds);
                    this.PROC_BindRecipeID(ds);
                    this.PROC_BindStepID(ds);
                }
            }
            catch
            {

            }
        }

        private void PROC_BindEQPID(DataSet ds)
        {
            this.bcboEQPID.Items.Clear();

            arrEQPID.Clear();

            foreach (DataRow nRow in ds.Tables[0].Rows)
            {
                string strKey = nRow["CONTEXT_KEY"].ToString();
                string strValue = nRow["CONTEXT_VALUE"].ToString();

                if (strKey == "EQP_ID")
                {
                    if (strValue.Length > 0)
                    {
                        string[] strArrValue = strValue.Split(';');

                        for (int i = 0; i < strArrValue.Length; i++)
                        {
                            if (strArrValue[i].Trim().Length > 0)
                            {
                                if (!arrEQPID.Contains(strArrValue[i].Trim()))
                                {
                                    arrEQPID.Add(strArrValue[i].Trim());
                                }
                            }
                        }
                    }
                }
            }

            if (arrEQPID.Count > 0)
            {
                for (int i = 0; i < arrEQPID.Count; i++)
                {
                    this.bcboEQPID.Items.Add(arrEQPID[i].ToString());
                }
            }
        }

        private void PROC_BindModuleID(DataSet ds)
        {
            this.bcboModuleID.Items.Clear();

            arrModuleID.Clear();

            foreach (DataRow nRow in ds.Tables[0].Rows)
            {
                string strKey = nRow["CONTEXT_KEY"].ToString();
                string strValue = nRow["CONTEXT_VALUE"].ToString();

                if (strKey == "MODULE_ID")
                {
                    if (strValue.Length > 0)
                    {
                        string[] strArrValue = strValue.Split(';');

                        for (int i = 0; i < strArrValue.Length; i++)
                        {
                            if (strArrValue[i].Trim().Length > 0)
                            {
                                if (!arrModuleID.Contains(strArrValue[i].Trim()))
                                {
                                    arrModuleID.Add(strArrValue[i].Trim());
                                }
                            }
                        }
                    }
                }
            }

            if (arrModuleID.Count > 0)
            {
                for (int i = 0; i < arrModuleID.Count; i++)
                {
                    this.bcboModuleID.Items.Add(arrModuleID[i].ToString());
                }
            }
        }

        private void PROC_BindRecipeID(DataSet ds)
        {
            this.bcboRecipeID.Items.Clear();

            arrRecipeID.Clear();

            foreach (DataRow nRow in ds.Tables[0].Rows)
            {
                string strKey = nRow["CONTEXT_KEY"].ToString();
                string strValue = nRow["CONTEXT_VALUE"].ToString();

                if (strKey == "RECIPE_ID")
                {
                    if (strValue.Length > 0)
                    {
                        string[] strArrValue = strValue.Split(';');

                        for (int i = 0; i < strArrValue.Length; i++)
                        {
                            if (strArrValue[i].Trim().Length > 0)
                            {
                                if (!arrRecipeID.Contains(strArrValue[i].Trim()))
                                {
                                    arrRecipeID.Add(strArrValue[i].Trim());
                                }
                            }
                        }
                    }
                }
            }

            if (arrRecipeID.Count > 0)
            {
                for (int i = 0; i < arrRecipeID.Count; i++)
                {
                    this.bcboRecipeID.Items.Add(arrRecipeID[i].ToString());
                }
            }
        }

        private void PROC_BindStepID(DataSet ds)
        {
            this.bcboStepID.Items.Clear();

            arrStepID.Clear();

            foreach (DataRow nRow in ds.Tables[0].Rows)
            {
                string strKey = nRow["CONTEXT_KEY"].ToString();
                string strValue = nRow["CONTEXT_VALUE"].ToString();

                if (strKey == "STEP_ID")
                {
                    if (strValue.Length > 0)
                    {
                        string[] strArrValue = strValue.Split(';');

                        for (int i = 0; i < strArrValue.Length; i++)
                        {
                            if (strArrValue[i].Trim().Length > 0)
                            {
                                if (!arrStepID.Contains(strArrValue[i].Trim()))
                                {
                                    arrStepID.Add(strArrValue[i].Trim());
                                }
                            }
                        }
                    }
                }
            }

            if (arrStepID.Count > 0)
            {
                for (int i = 0; i < arrStepID.Count; i++)
                {
                    this.bcboStepID.Items.Add(arrStepID[i].ToString());
                }
            }
        }


        #endregion

        #region Event

        private void bcboEQPID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.bcboEQPID.SelectedItem.ToString().Length > 0)
            {
                LinkedList lnkCondition = new LinkedList();
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.sEqpModel);
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this.bcboEQPID.SelectedItem.ToString());
                
                DataSet ds = this._wsSPC.GetSPCCalcContextKey(lnkCondition.GetSerialData());

                this.PROC_BindModuleID(ds);
                this.PROC_BindRecipeID(ds);
                this.PROC_BindStepID(ds);
            }
        }

        private void bcboModuleID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.bcboModuleID.SelectedItem.ToString().Length > 0)
            {
                LinkedList lnkCondition = new LinkedList();
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.sEqpModel);
                if (this.bcboEQPID.SelectedItem.ToString().Length > 0)
                {
                    lnkCondition.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this.bcboEQPID.SelectedItem.ToString());
                }
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, this.bcboModuleID.SelectedItem.ToString());

                DataSet ds = this._wsSPC.GetSPCCalcContextKey(lnkCondition.GetSerialData());

                this.PROC_BindRecipeID(ds);
                this.PROC_BindStepID(ds);
            }
        }

        private void bcboRecipeID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.bcboEQPID.SelectedItem.ToString().Length > 0)
            {
                LinkedList lnkCondition = new LinkedList();
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, this.sLineRawid);
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.AREA_RAWID, this.sAreaRawid);
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.EQP_MODEL, this.sEqpModel);
                if (this.bcboEQPID.SelectedItem.ToString().Length > 0)
                {
                    lnkCondition.Add(Definition.DynamicCondition_Condition_key.EQP_ID, this.bcboEQPID.SelectedItem.ToString());
                }
                if (this.bcboModuleID.SelectedItem.ToString().Length > 0)
                {
                    lnkCondition.Add(Definition.DynamicCondition_Condition_key.MODULE_ID, this.bcboModuleID.SelectedItem.ToString());
                }
                lnkCondition.Add(Definition.DynamicCondition_Condition_key.RECIPE_ID, this.bcboRecipeID.SelectedItem.ToString());

                DataSet ds = this._wsSPC.GetSPCCalcContextKey(lnkCondition.GetSerialData());

                this.PROC_BindStepID(ds);
            }
        }

        private void bcboStepID_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }


        

        #endregion




    }
}
