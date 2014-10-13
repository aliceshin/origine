using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Interface;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Condition.Controls.Filter
{
    public partial class FilterControl : ADynamicCondition
    {
        #region : Field
        public SearchConditionInterface _parent = null;
        private LinkedList lnkParent = null;
        eSPCWebService.eSPCWebService _ws = null;
        string _sSPC_MODEL_LEVEL = "";
        string _sSPC_MET_MODEL_LEVEL = "";
        #endregion

        #region : Initialization

        public FilterControl()
        {
            InitializeComponent();
            btxt_Filter.Enabled = true;
            _ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            LinkedList llstCondition = new LinkedList();
            if (this.GetType().FullName == "BISTel.eSPC.Condition.Controls.Filter.FilterControl")
            {
                llstCondition.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_MODEL_LEVEL");
            }
            else if (this.GetType().FullName == "BISTel.eSPC.Condition.ATT.Controls.Filter.FilterControl")
            {
                llstCondition.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_ATT_MODEL_LEVEL");
            }
            llstCondition.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            //llstCondition.Add(Definition.CONDITION_KEY_DEFAULT_COL, "Y");

            

            DataSet ds = _ws.GetCodeData(llstCondition.GetSerialData());

            if (ds != null && ds.Tables[0].Rows[0]["CODE"].ToString().ToUpper() == "AREA")
            {
                this._sSPC_MODEL_LEVEL = "AREA";
            }
            else
            {
                this._sSPC_MODEL_LEVEL = "EQP MODEL";
            }

            if (this.GetType().FullName == "BISTel.eSPC.Condition.Controls.Filter.FilterControl")
            {
                llstCondition.Clear();
                llstCondition.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_MET_MODEL_LEVEL");
                llstCondition.Add(Definition.CONDITION_KEY_USE_YN, "Y");
                //llstCondition.Add(Definition.CONDITION_KEY_DEFAULT_COL, "Y");

                ds = _ws.GetCodeData(llstCondition.GetSerialData());

                if (ds != null && ds.Tables[0].Rows[0]["CODE"].ToString().ToUpper() == "AREA")
                {
                    this._sSPC_MET_MODEL_LEVEL = "AREA";
                }
                else
                {
                    this._sSPC_MET_MODEL_LEVEL = "EQP MODEL";
                }
            }
        }

        #endregion

        #region : Override
        public override Control GetConditionComponent(string UI_LABEL)
        {
            return base.GetConditionComponent(UI_LABEL);
        }
        public override void RefreshCondition(LinkedList ll)
        {
            if (ll == null) return;

            if ((this._sSPC_MODEL_LEVEL == "AREA" && ll.Count == 4) || (this._sSPC_MODEL_LEVEL == "EQP MODEL" && ll.Count == 5))
            {
                lnkParent = ll;
            }

            if ((this._sSPC_MET_MODEL_LEVEL == "AREA" && ll.Count == 4) || (this._sSPC_MET_MODEL_LEVEL == "EQP MODEL" && ll.Count == 5))
            {
                lnkParent = ll;
            }

            this.btxt_Filter.Clear();

            if (ll.Contains("FILTER"))
            {
                DataTable dtFilter = (DataTable)ll["FILTER"];
                string value = DCUtil.GetValueData(dtFilter);
                this.btxt_Filter.Text = value;
                this.btxt_Filter_KeyDown(this.btxt_Filter, new KeyEventArgs(Keys.Enter));
            }
        }

        public override LinkedList GetParameter(LinkedList ll)
        {
            if (ll == null) return ll;

            if (this.lnkParent != null && this.lnkParent.Count > 0)
            {
                for (int i = 0; i < this.lnkParent.Count; i++)
                {
                    if (!ll.Contains(lnkParent.GetKey(i)))
                    {
                        ll.Add(lnkParent.GetKey(i), lnkParent.GetValue(i));
                    }
                }
            }

            return ll;
        }

        #endregion

        public void btxt_Filter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13 && this.lnkParent != null && this.lnkParent.Count > 0)
            {
                string sFilter = btxt_Filter.Text.Trim();
                DataTable dtFilter = DCUtil.MakeDataTableForDCValue(sFilter, sFilter);

                if (this.lnkParent.Contains("FILTER"))
                {
                    this.lnkParent["FILTER"] = dtFilter;
                }
                else
                {
                    this.lnkParent.Add("FILTER", dtFilter);
                }

                base.Search(this.lnkParent);                
            }
        }

    }
}
