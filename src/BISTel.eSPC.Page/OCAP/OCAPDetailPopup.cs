using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using System.Web.Services;


using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.OCAP
{
    public partial class OCAPDetailPopup : BasePopupFrm
    {
        #region : Field
        
        CommonUtility _comUtil;        
        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _wsSPC = null;     
        DataSet _dsOCAPView = new DataSet();
        
        LinkedList _llstSearch;        
        enum_PopupType _popupType;        
        SessionData _SessionData;
        DataTable _dtParamData = null;
        string _contextList = string.Empty;
        string _area = string.Empty;
        string _default_chart = string.Empty;
        string _model_name = string.Empty;
        string _param_alias = string.Empty;
        string _line =string.Empty;
        string _complex_yn = string.Empty;
        #endregion

        #region : Constructor

        public OCAPDetailPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region : Initialization

        public void InitializePopup()
        {
            _comUtil = new CommonUtility();
            _mlthandler= MultiLanguageHandler.getInstance();
            _wsSPC = new BISTel.eSPC.Page.eSPCWebService.eSPCWebService();
            this.InitializeLayout();
            this.InitializeCondition();
        }

      
        public void InitializeLayout()
        {
            try
            {
                this.blblLotID.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_LOT_ID]);
                this.blblSubstrateID.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_SUBSTRATE_ID]);
                this.blblClasstteSlot.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_CASSETTE_SLOT]);
                this.blblRecipeID.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_RECIPE_ID]);
                this.blblProductID.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_PRODUCT_ID]);
                this.blblOperationID.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_OPERATION_ID]);
                this.blblEQPID.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_EQP_ID]);
                this.blblModuleName.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_MODULE_NAME]);
                this.blblStepID.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_STEP_ID]);
                this.blblContextKey.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_CONTEXT_KEY]);


                this.btxtProblem.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_OOC_PROBLEM]);
                this.btxtCause.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_OOC_CAUSE]);
                this.btxtsolution.Text = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_OOC_SOLUTION]);
                
                if(PopUpType==enum_PopupType.Modify)
                {
                    this.btxtProblem.Enabled = true;
                    this.btxtCause.Enabled = true;
                    this.btxtsolution.Enabled = true;

                    this.bbtnSave.Visible = true;
                    
                }else
                {
                    this.btxtProblem.Enabled = false;
                    this.btxtCause.Enabled = false;
                    this.btxtsolution.Enabled = false;
                    this.bbtnSave.Visible = false;
                }
                //title
                this.blblTitleLotID.Text = this._mlthandler.GetVariable(Definition.LABEL.LOT_ID)+" : ";
                this.blblTitleSubstrateID.Text = this._mlthandler.GetVariable(Definition.LABEL.SUBSTRATE_ID) + " : ";
                this.blblTitleClasstteSlot.Text = this._mlthandler.GetVariable(Definition.LABEL.CLASSTTE_SLOT) + " : ";
                this.blblTitleRecipeID.Text = this._mlthandler.GetVariable(Definition.LABEL.RECIPE_ID) + " : ";
                this.blblTitleProductID.Text = this._mlthandler.GetVariable(Definition.LABEL.PRODUCT_ID) + " : ";
                this.blblTitleOperationID.Text = this._mlthandler.GetVariable(Definition.LABEL.OPERATION_ID) + " : ";
                this.blblTitleEQPID.Text = this._mlthandler.GetVariable(Definition.LABEL.EQP_ID) + " : ";
                this.blblTitleModuleName.Text = this._mlthandler.GetVariable(Definition.LABEL.MODULE_NAME) + " : ";
                this.blblTitleStepID.Text = this._mlthandler.GetVariable(Definition.LABEL.STEP_ID) + " : ";
                this.blblTitleContextKey.Text = this._mlthandler.GetVariable(Definition.LABEL.CONTEXT_KEY) + " : ";
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
        }
     
        public void InitializeCondition()
        {
           
        }

        #endregion
        

        #region : Popup Logic

     
        #endregion

        #region :: Button

        #region ::: Function

        private void ClickSaveButton()
        {
            LinkedList llstSave = new LinkedList();
            llstSave.Add(Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID, _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_OOC_TRX_SPC_RAWID]));
            llstSave.Add(Definition.CONDITION_KEY_CONTEXT_LIST, _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_CONTEXT_LIST]));
            llstSave.Add(Definition.CONDITION_KEY_OOC_PROBLEM, this.btxtProblem.Text);
            llstSave.Add(Definition.CONDITION_KEY_OOC_CAUSE, this.btxtCause.Text);
            llstSave.Add(Definition.CONDITION_KEY_OOC_SOLUTION, this.btxtsolution.Text);
            llstSave.Add(Definition.CONDITION_KEY_USER_ID,this.SessionData.UserId);

            byte[] baData = llstSave.GetSerialData();

            bool bSuccess = this._wsSPC.SaveOCAP(baData);

            if (bSuccess)
            {
        
                llstSearchCondition.Remove(Definition.CONDITION_KEY_OOC_PROBLEM);
                llstSearchCondition.Remove(Definition.CONDITION_KEY_OOC_CAUSE);
                llstSearchCondition.Remove(Definition.CONDITION_KEY_OOC_SOLUTION);

                llstSearchCondition.Add(Definition.CONDITION_KEY_OOC_PROBLEM, this.btxtProblem.Text);
                llstSearchCondition.Add(Definition.CONDITION_KEY_OOC_CAUSE, this.btxtCause.Text);
                llstSearchCondition.Add(Definition.CONDITION_KEY_OOC_SOLUTION, this.btxtsolution.Text);
                
                                
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.GENERAL_SAVE_SUCCESS));
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.GENERAL_SAVE_FAIL));
            }
           
        }
        #endregion

        #region ::: Event

        private void bbtnSave_Click(object sender, EventArgs e)
        {
            this.ClickSaveButton();
        }

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


        private void bbtnChartView_Click(object sender, EventArgs e)
        {

            //Common.ChartViewPopup chartViewPop = new Common.ChartViewPopup();
            //chartViewPop.AREA = AREA;          
            //chartViewPop.DEFAULT_CHART = DEFAULT_CHART;
            //chartViewPop.SPC_MODEL = SPC_MODEL_NAME;          
            //chartViewPop.URL = this.URL;
            //chartViewPop.dtParamData = dtParamData;
            //chartViewPop.SessionData = this.SessionData;
            //chartViewPop.llstInfoCondition =llstSearchCondition;              
            //chartViewPop.complex_yn=complex_yn;
            //chartViewPop.InitializePopup();
            //DialogResult result = chartViewPop.ShowDialog(this);
            //if (result == DialogResult.OK)
            //{
            //}

        }

        #endregion

        #endregion


        #region : Public



        public string LINE
        {
            get { return _line; }
            set { _line = value; }
        }
        
        public string PARAM_ALIAS
        {
            set { this._param_alias = value; }
            get { return this._param_alias; }

        }

        public string AREA
        {
            set { this._area = value; }
            get { return this._area; }

        }

        public string DEFAULT_CHART
        {
            set { this._default_chart = value; }
            get { return this._default_chart; }

        }
        

        public string SPC_MODEL_NAME
        {
            set { this._model_name = value; }
            get { return this._model_name; }

        }        

        public string CONTEXT_LIST
        {
            set { this._contextList = value; }
            get { return this._contextList; }

        }
      

        public LinkedList llstSearchCondition
        {
            set { this._llstSearch = value; }
            get { return this._llstSearch; }
        }

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }
        
        public enum_PopupType PopUpType
        {
            get { return _popupType; }
            set { _popupType = value; }
        }


        public DataTable dtParamData
        {
            get { return _dtParamData; }
            set { _dtParamData = value; }
        }

        public string complex_yn
        {
            set { this._complex_yn = value; }
            get { return this._complex_yn; }

        }
        
        
        #endregion

       

       

    }
}
