using System;
using System.Collections;
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

namespace BISTel.eSPC.Condition.Controls.Lot
{
    public partial class LotSearchControl : ADynamicCondition
    {
        #region : Field
        public SearchConditionInterface _parent = null;
        Initialization _mInitialization = null;
        #endregion

        #region : Initialization

        public LotSearchControl()
        {
            InitializeComponent();
            this.InitializeCondition();
        }

        private void InitializeCondition()
        {
            this._mInitialization = new Initialization();
            this._mInitialization.InitializePath();

            btxt_EQPID.Enabled = true;
            btxt_ModuleID.Enabled = true;
            btxt_LotID.Enabled = true;
            btxt_SubstrateID.Enabled = true;
            btxt_RecipeID.Enabled = true;

            string strValue = this._mInitialization.GetSearchConfig(Definition.PAGE_KEY_OCAP_LIST_UC);
            if (!string.IsNullOrEmpty(strValue.Trim()) && strValue.Trim().ToUpper() == Definition.VARIABLE_FALSE.ToUpper())
            {
                this.pnl_RadioBtn.Visible = false;
            }
            else
            {
                this.pnl_RadioBtn.Visible = true;
            }
        }

        #endregion

        #region : Override

        public override void RefreshCondition(LinkedList ll)
        {
            if (ll == null) return;

            if (ll.Contains(Definition.DynamicCondition_Search_key.CHART_ID))
            {
                DataTable dtChartID = (DataTable)ll[Definition.DynamicCondition_Search_key.CHART_ID];
                string value = DCUtil.GetValueData(dtChartID);
                this.btxt_ChartID.Text = value;
            }

            if (ll.Contains(Definition.CONDITION_SEARCH_KEY_EQP))
            {
                DataTable dtEQPID = (DataTable)ll[Definition.CONDITION_SEARCH_KEY_EQP];
                string value = DCUtil.GetValueData(dtEQPID);
                this.btxt_EQPID.Text = value;
            }

            if (ll.Contains(Definition.CONDITION_SEARCH_KEY_MODULE))
            {
                DataTable dtModuleID = (DataTable)ll[Definition.CONDITION_SEARCH_KEY_MODULE];
                string value = DCUtil.GetValueData(dtModuleID);
                this.btxt_ModuleID.Text = value;
            }

            if (ll.Contains(Definition.CONDITION_SEARCH_KEY_LOT))
            {
                DataTable dtLot = (DataTable)ll[Definition.CONDITION_SEARCH_KEY_LOT];
                string value = DCUtil.GetValueData(dtLot);
                this.btxt_LotID.Text = value;
            }

            if (ll.Contains(Definition.CONDITION_SEARCH_KEY_SUBSTRATE))
            {
                DataTable dtSubstrate = (DataTable)ll[Definition.CONDITION_SEARCH_KEY_SUBSTRATE];
                string value = DCUtil.GetValueData(dtSubstrate);
                this.btxt_SubstrateID.Text = value;
            }

            if (ll.Contains(Definition.CONDITION_SEARCH_KEY_RECIPE))
            {
                DataTable dtRecipe = (DataTable)ll[Definition.CONDITION_SEARCH_KEY_RECIPE];
                string value = DCUtil.GetValueData(dtRecipe);
                this.btxt_RecipeID.Text = value;
            }

            if (ll.Contains(Definition.CONDITION_SEARCH_KEY_OCAP_OOC))
            {
                DataTable dtOCAPOOC = (DataTable)ll[Definition.CONDITION_SEARCH_KEY_OCAP_OOC];
                string value = DCUtil.GetValueData(dtOCAPOOC);

                if (value == "OCAP")
                {
                    this.brbtn_OCAP.Checked = true;
                    this.brbtn_OOC.Checked = false;
                }
                else
                {
                    this.brbtn_OCAP.Checked = false;
                    this.brbtn_OOC.Checked = true;
                }
            }
        }

        public override LinkedList GetParameter(LinkedList ll)
        {
            if (ll == null) return ll;

            string sChartID = btxt_ChartID.Text.Trim();
            if (sChartID != string.Empty)
            {
                DataTable dtChartID = DCUtil.MakeDataTableForDCValue(sChartID, sChartID);

                if (ll.Contains(Definition.DynamicCondition_Search_key.CHART_ID))
                {
                    ll[Definition.DynamicCondition_Search_key.CHART_ID] = dtChartID;
                }
                else
                {
                    ll.Add(Definition.DynamicCondition_Search_key.CHART_ID, dtChartID);
                }
            }

            string sEQPID = btxt_EQPID.Text.Trim();
            if (sEQPID != string.Empty)
            {
                DataTable dtEQPID= DCUtil.MakeDataTableForDCValue(sEQPID, sEQPID);

                if (ll.Contains(Definition.CONDITION_SEARCH_KEY_EQP))
                {
                    ll[Definition.CONDITION_SEARCH_KEY_EQP] = dtEQPID;
                }
                else
                {
                    ll.Add(Definition.CONDITION_SEARCH_KEY_EQP, dtEQPID);
                }
            }

            string sModuleID = btxt_ModuleID.Text.Trim();
            if (sModuleID != string.Empty)
            {
                DataTable dtModuleID = DCUtil.MakeDataTableForDCValue(sModuleID, sModuleID);

                if (ll.Contains(Definition.CONDITION_SEARCH_KEY_MODULE))
                {
                    ll[Definition.CONDITION_SEARCH_KEY_MODULE] = dtModuleID;
                }
                else
                {
                    ll.Add(Definition.CONDITION_SEARCH_KEY_MODULE, dtModuleID);
                }
            }

            string sLotID = btxt_LotID.Text.Trim();
            if (sLotID != string.Empty)
            {
                DataTable dtLot = DCUtil.MakeDataTableForDCValue(sLotID, sLotID);

                if (ll.Contains(Definition.CONDITION_SEARCH_KEY_LOT))
                {
                    ll[Definition.CONDITION_SEARCH_KEY_LOT] = dtLot;
                }
                else
                {
                    ll.Add(Definition.CONDITION_SEARCH_KEY_LOT, dtLot);
                }
            }

            string sSubstrateID = btxt_SubstrateID.Text.Trim();
            if (sSubstrateID != string.Empty)
            {
                DataTable dtSubstrate = DCUtil.MakeDataTableForDCValue(sSubstrateID, sSubstrateID);

                if (ll.Contains(Definition.CONDITION_SEARCH_KEY_SUBSTRATE))
                {
                    ll[Definition.CONDITION_SEARCH_KEY_SUBSTRATE] = dtSubstrate;
                }
                else
                {
                    ll.Add(Definition.CONDITION_SEARCH_KEY_SUBSTRATE, dtSubstrate);
                }
            }

            string sRecipeID = btxt_RecipeID.Text.Trim();
            if (sRecipeID != string.Empty)
            {
                DataTable dtRecipe = DCUtil.MakeDataTableForDCValue(sRecipeID, sRecipeID);

                if (ll.Contains(Definition.CONDITION_SEARCH_KEY_RECIPE))
                {
                    ll[Definition.CONDITION_SEARCH_KEY_RECIPE] = dtRecipe;
                }
                else
                {
                    ll.Add(Definition.CONDITION_SEARCH_KEY_RECIPE, dtRecipe);
                }
            }

            //Chris
            if (this.brbtn_OCAP.Checked)
            {
                DataTable dtOCAP = DCUtil.MakeDataTableForDCValue("OCAP", "OCAP");

                if (ll.Contains(Definition.CONDITION_SEARCH_KEY_OCAP_OOC))
                {
                    ll[Definition.CONDITION_SEARCH_KEY_OCAP_OOC] = dtOCAP;
                }
                else
                {
                    ll.Add(Definition.CONDITION_SEARCH_KEY_OCAP_OOC, dtOCAP);
                }

            }

            //Chris
            if (this.brbtn_OOC.Checked)
            {
                DataTable dtOOC = DCUtil.MakeDataTableForDCValue("OOC", "OOC");

                if (ll.Contains(Definition.CONDITION_SEARCH_KEY_OCAP_OOC))
                {
                    ll[Definition.CONDITION_SEARCH_KEY_OCAP_OOC] = dtOOC;
                }
                else
                {
                    ll.Add(Definition.CONDITION_SEARCH_KEY_OCAP_OOC, dtOOC);
                }

            }

            return ll;
        }

        #endregion

    }
}
