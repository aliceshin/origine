using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;

using Steema.TeeChart;
using Steema.TeeChart.Styles;

namespace BISTel.eSPC.Page.Modeling
{
    public partial class MailingGroupConfigurationUC : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;

        Initialization _Initialization;
        BSpreadUtility _bspreadutility;
        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition = new LinkedList();


        BISTel.eSPC.Common.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.CommonUtility _ComUtil;

        private SortedList _slParamColumnIndex = new SortedList();
        private SortedList _slSpcModelingIndex = new SortedList();

        private int _ColIdx_SELECT = 0;

        private int _ColIdx_MainChart = 0;
        private int _ColIdx_Parameter = 0;
        private int _ColIdx_EQPID = 0;
        private int _ColIdx_ModuleID = 0;
        private int _ColIdx_OperationID = 0;
        private int _ColIdx_ProductID = 0;

        private ConfigMode _cofingMode;
        string _sConfigRawID;

        #endregion


        #region ::: Properties


        #endregion


        #region ::: Constructor

        public MailingGroupConfigurationUC()
        {
            InitializeComponent();

        }

        #endregion

        #region ::: Override Method

        public override void PageInit()
        {
            //BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;

            //this.KeyOfPage = this.GetType().FullName; // 필수 입력 (Key값을 받은 후 DC을 Active해주기 때문이다.

            this.InitializePage();
        }

        //public override BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.ADynamicCondition CreateCustomCondition()
        //{
        //    return new BISTel.eSPC.Condition.Modeling.SPCModelCondition();			
        //}

        #endregion

        #region ::: PageLoad & Initialize

        public void InitializePage()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();

            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._SpreadUtil = new BSpreadUtility();
            this._ComUtil = new CommonUtility();

            this.InitializeCode();
            this.InitializeDataButton();
            this.InitializeBSpread();
            this.InitializeLayout();
        }


        public void InitializeLayout()
        {
        }

        private void InitializeCode()
        {
            //LinkedList llconidtion = new LinkedList();

            ////PARAM TYPE
            //llconidtion.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_PARAM_CATEGORY);

            //DataSet _dsParamCategory = this._wsSPC.GetCodeData(llconidtion.GetSerialData());
            //_ComUtil.SetBComboBoxData(this.bcboParamCategory, _dsParamCategory, "NAME", "CODE", "", true);

        }


        public void InitializeDataButton()
        {
            //this._slSpcModelingIndex = this._Initialization.InitializeButtonList(this.bbtnList, ref bsprData, Definition.PAGE_KEY_SPC_MODELING_UC, "", this.sessionData);

            this.FunctionName = Definition.FUNC_KEY_SPC_MODELING;
            //this.ApplyAuthory(this.bbtnList);
        }



        public void InitializeBSpread()
        {
            //this._slParamColumnIndex = new SortedList();

            //this._slParamColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprData, Definition.PAGE_KEY_SPC_MODELING_UC, true, "");
            //this.bsprData.UseHeadColor= true;

            //this._ColIdx_SELECT = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT)];
            //this._ColIdx_MainChart = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_MAIN_CHART)];
            //this._ColIdx_Parameter = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_PARAM_ALIAS)];
            //this._ColIdx_EQPID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_EQP_ID)];
            //this._ColIdx_ModuleID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_MODULE_ID)];
            //this._ColIdx_OperationID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_OPERATION_ID)];
            //this._ColIdx_ProductID = (int)this._slParamColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_PRODUCT_ID)];

            //this._Initialization.SetCheckColumnHeader(this.bsprData, 0);
        }





        #endregion



        #region :DATA PROPERTY
        //DATA PROPERTY
        public ConfigMode CONFIG_MODE
        {
            get { return _cofingMode; }
            set { _cofingMode = value; }
        }

        public string CONFIG_RAWID
        {
            get { return _sConfigRawID; }
            set { _sConfigRawID = value; }
        }


        #endregion


        #region ::: User Defined Method.

        #endregion

        #region ::: EventHandler

        #endregion

    }

}
