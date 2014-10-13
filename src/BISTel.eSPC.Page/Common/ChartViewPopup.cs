using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Web.Services;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;

namespace BISTel.eSPC.Page.Common
{
    public partial class ChartViewPopup : BasePopupFrm
    {
        #region : Field

        CommonUtility _comUtil;
        SessionData _SessionData;
        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _wsSPC;

        ChartInterface _ChartVariable = new ChartInterface();
        private CHART_PARENT_MODE _chart_parent_mode = CHART_PARENT_MODE.SPC_CONTROL_CHART;



        #endregion

        #region : Constructor

        public ChartViewPopup()
        {
            InitializeComponent();

        }

        #endregion

        #region : Initialization

        public void InitializePopup()
        {
            this.InitializeVariable();
            this.InitializeLayout();
            this.InitializeCondition();

        }

        public void InitializeVariable()
        {
            this._wsSPC = new BISTel.eSPC.Page.eSPCWebService.eSPCWebService();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._comUtil = new CommonUtility();


            this.bapSearch.Visible = true;
            this.bDtStart.Value = ChartVariable.dateTimeStart;
            this.bDtEnd.Value = ChartVariable.dateTimeEnd;

            if (chart_parent_mode == CHART_PARENT_MODE.MODELING)
            {
                CallDataTrxData();
            }



            if (ChartVariable.llstInfoCondition[Definition.CONDITION_KEY_EQP_ID] != null)
                ChartVariable.EQP_ID = _comUtil.GetConditionString(ChartVariable.llstInfoCondition, Definition.CONDITION_KEY_EQP_ID, Definition.CONDITION_KEY_EQP_ID);

            if (ChartVariable.llstInfoCondition[Definition.CONDITION_KEY_PRODUCT_ID] != null)
                ChartVariable.PRODUCT_ID = _comUtil.GetConditionString(ChartVariable.llstInfoCondition, Definition.CONDITION_KEY_PRODUCT_ID, Definition.CONDITION_KEY_PRODUCT_ID);

            if (ChartVariable.llstInfoCondition[Definition.CONDITION_KEY_LOT_ID] != null)
                ChartVariable.LOT_ID = _comUtil.GetConditionString(ChartVariable.llstInfoCondition, Definition.CONDITION_KEY_LOT_ID, Definition.CONDITION_KEY_LOT_ID);

            if (ChartVariable.llstInfoCondition[Definition.CONDITION_KEY_OPERATION_ID] != null)
                ChartVariable.OPERATION_ID = _comUtil.GetConditionString(ChartVariable.llstInfoCondition, Definition.CONDITION_KEY_OPERATION_ID, Definition.CONDITION_KEY_OPERATION_ID);

            if (ChartVariable.llstInfoCondition[Definition.CONDITION_KEY_SUBSTRATE_ID] != null)
                ChartVariable.SUBSTRATE_ID = _comUtil.GetConditionString(ChartVariable.llstInfoCondition, Definition.CONDITION_KEY_SUBSTRATE_ID, Definition.CONDITION_KEY_SUBSTRATE_ID);

        }

        public void InitializeLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.TITLE_SPC_CHART);

        }



        public void InitializeCondition()
        {


            if (chart_parent_mode == CHART_PARENT_MODE.PPK_REPORT)
            {

                if (ChartVariable.dtParamData == null || ChartVariable.dtParamData.Rows.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

            }
            else
            {
                if (ChartVariable.dtResource == null || ChartVariable.dtResource.Rows.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                CreateParamDataTable();
            }



            ChartVariable.lstDefaultChart.Clear();
            ChartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(ChartVariable.DEFAULT_CHART);

            Common.SPCChart spcControlChart = new BISTel.eSPC.Page.Common.SPCChart();
            spcControlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            spcControlChart.URL = this.URL;
            spcControlChart.ChartVariable = ChartVariable;
            spcControlChart.sessionData = this.SessionData;
            spcControlChart.Height = this.pnlChart.Height;
            spcControlChart.InitializePage();
            this.pnlChart.Controls.Add(spcControlChart);

        }

        #endregion



        #region : Popup Logic

        public void CreateParamDataTable()
        {
            CreateChartDataTable _createChartDT = new CreateChartDataTable();
            _createChartDT.dtResourceData = ChartVariable.dtResource;
            _createChartDT.COMPLEX_YN = ChartVariable.complex_yn;
            ChartVariable.dtParamData = _createChartDT.GetMakeDataTable();
            ChartVariable.lstRawColumn = _createChartDT.lstRawColumn;
        }

        private void CallDataTrxData()
        {

            DataSet _ds = null;
            try
            {
                LinkedList _llstSearchCondition = new LinkedList();
                _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, ChartVariable.MODEL_CONFIG_RAWID);
                _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, this.bDtStart.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                _llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, this.bDtEnd.Value.ToString("yyyy-MM-dd") + " 23:59:59");

                _ds = _wsSPC.GetDataTRXData(_llstSearchCondition.GetSerialData());
                if (_ds == null)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                ChartVariable.dtResource = _ds.Tables[0];
                this.pnlChart.Controls.Clear();
                InitializeCondition();

            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
            finally
            {
                _ds = null;
            }
        }
        #endregion

        #region :: Button Event & Method

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void bbtnSave_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }


        private void bbtnSearch_Click(object sender, EventArgs e)
        {
            CallDataTrxData();
        }
        #endregion



        #region : Public


        public ChartInterface ChartVariable
        {
            get { return _ChartVariable; }
            set { _ChartVariable = value; }
        }


        /// <summary>
        /// SPC_Modeling 화면에서 호출하는 경우 CHART_PARENT_MODE.MODELING 으로 넘긴다.
        /// </summary>
        public CHART_PARENT_MODE chart_parent_mode
        {

            get { return _chart_parent_mode; }
            set { _chart_parent_mode = value; }
        }


        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }


        #endregion



    }
}
