using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls;
using BISTel.eSPC.Condition.Controls.ComboTree;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Condition.Compare
{
    public partial class SPCModelCompareCondition : ConditionContainer
    {
        private eSPCWebService.eSPCWebService ws = null;
        public SPCModelCompareCondition()
        {
            InitializeComponent();

            ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            InitializeComboTree();
            InitializeTreeView();

            this.ClickSearch += new ClickSearchEventHandler(SPCModelCompareCondition_ClickSearch);
        }

        void SPCModelCompareCondition_ClickSearch(object sender, ClickSearchEventArgs csea)
        {
            this.conditionComboTree1.Search(csea.LinkedListCondition);
            this.spcModelCompareConditionTree1.Search(csea.LinkedListCondition);
        }

        private void InitializeComboTree()
        {
            this.conditionComboTree1.CheckCountType = BTreeView.CheckCountTypes.Single;

            LinkedList llstCondition = new LinkedList();
            if (this.GetType().FullName == "BISTel.eSPC.Condition.Compare.SPCModelCompareCondition")
            {
                llstCondition.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_MODEL_LEVEL");
                this.spcModelCompareConditionTree1.ISMET = false;
            }
            else if (this.GetType().FullName == "BISTel.eSPC.Condition.Compare.MET.SPCModelCompareCondition")
            {
                llstCondition.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_MET_MODEL_LEVEL");
                this.spcModelCompareConditionTree1.ISMET = true;
            }
            else if (this.GetType().FullName == "BISTel.eSPC.Condition.ATT.Compare.SPCModelCompareCondition")
            {
                llstCondition.Add(Definition.CONDITION_KEY_CATEGORY, "SPC_ATT_MODEL_LEVEL");
                this.spcModelCompareConditionTree1.ISATT = true;
            }
            llstCondition.Add(Definition.CONDITION_KEY_USE_YN, "Y");
            //llstCondition.Add(Definition.CONDITION_KEY_DEFAULT_COL, "Y");

            DataSet ds = ws.GetCodeData(llstCondition.GetSerialData());

            this.conditionComboTree1.AddVisibleLevel(ConditionLevel.SITE, false);
            this.conditionComboTree1.AddVisibleLevel(ConditionLevel.FAB, false);
            this.conditionComboTree1.AddVisibleLevel(ConditionLevel.LINE, false);
            if(ds != null && ds.Tables[0].Rows[0]["CODE"].ToString().ToUpper() == "AREA")
            {
                this.conditionComboTree1.Title = "AREA";
                this.conditionComboTree1.AddVisibleLevel(ConditionLevel.AREA, true);
            }
            else
            {
                this.conditionComboTree1.Title = "EQP MODEL";
                this.conditionComboTree1.AddVisibleLevel(ConditionLevel.AREA, false);
                this.conditionComboTree1.AddVisibleLevel(ConditionLevel.EQPMODEL, true);
            }
        }

        private void InitializeTreeView()
        {
            this.spcModelCompareConditionTree1.CheckCountType = BTreeView.CheckCountTypes.Single;
            this.spcModelCompareConditionTree1.Title = "PARAMETER";
        }
    }
}
