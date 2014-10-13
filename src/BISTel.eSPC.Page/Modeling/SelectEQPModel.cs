using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.Modeling
{
    public partial class SelectEQPModel : BasePopupFrm
    {
        private ADynamicCondition _dynamicEQP = null;
        private BTreeCombo _btreeComboEQP = null;
        private TreeNode _tnCheckedComboNode = null;

        public string sEQPModel = string.Empty;
        public string sLocationRawID = string.Empty;
        public string sAreaRawID = string.Empty;
        public string _sSPCModelLevel = string.Empty;
        public string sLocation = string.Empty;
        public string sArea = string.Empty;

        public bool isMET = false;

        public SelectEQPModel()
        {
            InitializeComponent();
        }

        private void SelectEQPModel_Load(object sender, EventArgs e)
        {
            if (isMET)
            {
                DynamicConditionFactory dcf = new DynamicConditionFactory();
                _dynamicEQP = dcf.GetDCOfPageByKey("BISTel.eSPC.Page.Modeling.MET.Modeling.SPCModelingUC");
                Control ctrlEQP = ((DCBar)_dynamicEQP).GetControlByContextID("ESPC_COMBO_TREE");
                this._btreeComboEQP = (BTreeCombo)ctrlEQP;
                this._btreeComboEQP.SelectionCommitted += new EventHandler(_btreeComboEQP_SelectionCommitted);
                this.bplEQPModel.Controls.Add(ctrlEQP);
                ctrlEQP.Dock = DockStyle.Fill;

                BISTel.eSPC.Page.eSPCWebService.eSPCWebService spcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

                //SPC MODEL LEVEL을 가져옴
                LinkedList llstCondtion = new LinkedList();
                llstCondtion.Add(BISTel.eSPC.Common.Definition.CONDITION_KEY_CATEGORY, "SPC_MET_MODEL_LEVEL");
                llstCondtion.Add(BISTel.eSPC.Common.Definition.CONDITION_KEY_USE_YN, "Y");
                //llstCondtion.Add(BISTel.eSPC.Common.Definition.CONDITION_KEY_DEFAULT_COL, "Y");

                DataSet dsModelLevel = spcWebService.GetCodeData(llstCondtion.GetSerialData());
                if (DSUtil.CheckRowCount(dsModelLevel))
                {
                    _sSPCModelLevel = dsModelLevel.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.CODE].ToString();
                    if (!_sSPCModelLevel.Equals(BISTel.eSPC.Common.Definition.CONDITION_KEY_EQP_MODEL))
                    {
                        this.btnIgnore.Text = "Use Current Area";
                    }
                }
            }
            else
            {
                DynamicConditionFactory dcf = new DynamicConditionFactory();
                _dynamicEQP = dcf.GetDCOfPageByKey("BISTel.eSPC.Page.Modeling.SPCModelingUC");
                Control ctrlEQP = ((DCBar)_dynamicEQP).GetControlByContextID("ESPC_COMBO_TREE");
                this._btreeComboEQP = (BTreeCombo)ctrlEQP;
                this._btreeComboEQP.SelectionCommitted += new EventHandler(_btreeComboEQP_SelectionCommitted);
                this.bplEQPModel.Controls.Add(ctrlEQP);
                ctrlEQP.Dock = DockStyle.Fill;

                BISTel.eSPC.Page.eSPCWebService.eSPCWebService spcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

                //SPC MODEL LEVEL을 가져옴
                LinkedList llstCondtion = new LinkedList();
                llstCondtion.Add(BISTel.eSPC.Common.Definition.CONDITION_KEY_CATEGORY, "SPC_MODEL_LEVEL");
                llstCondtion.Add(BISTel.eSPC.Common.Definition.CONDITION_KEY_USE_YN, "Y");
                //llstCondtion.Add(BISTel.eSPC.Common.Definition.CONDITION_KEY_DEFAULT_COL, "Y");

                DataSet dsModelLevel = spcWebService.GetCodeData(llstCondtion.GetSerialData());
                if (DSUtil.CheckRowCount(dsModelLevel))
                {
                    _sSPCModelLevel = dsModelLevel.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.CODE].ToString();
                    if (!_sSPCModelLevel.Equals(BISTel.eSPC.Common.Definition.CONDITION_KEY_EQP_MODEL))
                    {
                        this.btnIgnore.Text = "Use Current Area";
                    }
                }
            }
        }

        void _btreeComboEQP_SelectionCommitted(object sender, EventArgs e)
        {
            TreeNodeCollection tnc = this._btreeComboEQP.TreeView.Nodes;
            this.FindCheckedNode(tnc);

            DCValueOfTree dvtModel = TreeDCUtil.GetDCValue(this._tnCheckedComboNode);
            List<DCValueOfTree> lstSelectedValue = TreeDCUtil.GetDCValueOfAllParent(this._tnCheckedComboNode);

            if (_sSPCModelLevel.Equals(BISTel.eSPC.Common.Definition.CONDITION_KEY_EQP_MODEL))
            {
                sEQPModel = dvtModel.Value;

                if (lstSelectedValue != null)
                {
                    string contextID = "";

                    for (int i = 0; i < lstSelectedValue.Count; i++)
                    {
                        contextID = lstSelectedValue[i].ContextId;

                        switch (contextID)
                        {
                            case "ESPC_AREA":
                                sAreaRawID = lstSelectedValue[i].Value;
                                sArea = lstSelectedValue[i].Disp;
                                break;
                            case "ESPC_LINE":
                                sLocationRawID = lstSelectedValue[i].Value;
                                sLocation = lstSelectedValue[i].Disp;
                                break;
                            default: break;
                        }
                    }
                }
            }
            else
            {
                sAreaRawID = dvtModel.Value;
                sArea = dvtModel.Disp;

                if (lstSelectedValue != null)
                {
                    string contextID = "";

                    for (int i = 0; i < lstSelectedValue.Count; i++)
                    {
                        contextID = lstSelectedValue[i].ContextId;

                        switch (contextID)
                        {
                            case "ESPC_LINE":
                                sLocationRawID = lstSelectedValue[i].Value;
                                sLocation = lstSelectedValue[i].Disp;
                                break;
                            default: break;
                        }
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_sSPCModelLevel.Equals(BISTel.eSPC.Common.Definition.CONDITION_KEY_EQP_MODEL))
            {
                if (sEQPModel == string.Empty)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_EQPMODEL", null, null);
                    return;
                }
            }
            else
            {
                if (sAreaRawID == string.Empty)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_AREA", null, null);
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FindCheckedNode(TreeNodeCollection nodes)
        {
            int i = 0;
            int count = 0;

            count = nodes.Count;

            for (i = 0; i < count; i++)
            {
                if (nodes[i].Checked.Equals(true))
                {
                    this._tnCheckedComboNode = nodes[i];
                }
                else
                {
                    FindCheckedNode(nodes[i].Nodes);
                }
            }
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;
        }
    }
}
