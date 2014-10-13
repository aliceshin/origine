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

namespace BISTel.eSPC.Page.OCAP.Popup
{
    public partial class ChartViewPopup : BasePopupFrm
    {
        #region : Field

        CommonUtility _comUtil;        
        SessionData _SessionData;
        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _wsSPC;

        DataSet _dsParam = new DataSet();
        
        LinkedList _llstSearch = null;
        List<string> _lstRawColumn = new List<string>();
        string _complex_yn;
        
        
        string _paramAlias = string.Empty;
        string _productID = string.Empty;
        string _lotID = string.Empty;
        string _substrateID = string.Empty;
        string _area = string.Empty;
        string _eqpID = string.Empty;
        string _defaultChart = string.Empty;
        string _spcModel = string.Empty;
        string _eqpModel = string.Empty;
        string _line = string.Empty;
        string _strRawCol=string.Empty;
        
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
        }

        public void InitializeLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.TITLE_SPC_CHART);      
                                                            
        }

        private void AddColumns(DataTable dt, LinkedList llsColumn)
        {
            for(int i=1;i<llsColumn.Count; i++)
            {                
                dt.Columns.Add(llsColumn.GetKey(i).ToString(), typeof(String));
            }            
        }
        

        public void InitializeCondition()
        {                   
            string[] arrChart = DEFAULT_CHART.Split(';');
            List<string> lstChart = new List<string>();
            for (int i = 0; i < arrChart.Length; i++)
            {
                lstChart.Add(arrChart[i].ToString());
            }      
            
            Common.SPCControlChartUC spcControlChart = new BISTel.eSPC.Page.Common.SPCControlChartUC();
            spcControlChart.PARAM_ALIAS = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_PARAM_ALIAS]);
            spcControlChart.PRODUCT_ID = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_PRODUCT_ID]);
            spcControlChart.LOT_ID = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_LOT_ID]); 
            spcControlChart.SUBSTRATE_ID = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_SUBSTRATE_ID]); 
            spcControlChart.AREA = AREA;
            spcControlChart.EQP_ID = _comUtil.NVL(llstSearchCondition[Definition.CONDITION_KEY_EQ_ID]);
            spcControlChart.SPC_MODEL = SPC_MODEL;
            spcControlChart.EQP_MODEL = null;
            spcControlChart.lstDefaultChart = lstChart;
            spcControlChart.LINE = LINE;   
            spcControlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            spcControlChart.DataSetPrameter = DataSetParam; //MakeDataSet();
            spcControlChart.lstRawColumn=lstRawColumn;
            spcControlChart.complex_yn= complex_yn;
            spcControlChart.InitializePage();
            this.pnlChart.Controls.Add(spcControlChart);             
           
        }

        #endregion

        

        #region : Popup Logic

 
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

        #endregion



        #region : Public

        public string AREA
        {
            set { this._area = value; }
            get { return this._area; }
        }

        public string DEFAULT_CHART
        {
            set { this._defaultChart = value; }
            get { return this._defaultChart; }
        }
        public string SPC_MODEL
        {
            set { this._spcModel = value; }
            get { return this._spcModel; }
        }
        
        public string LINE
        {
            set { this._line = value; }
            get { return this._line; }
        }

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        public DataSet DataSetParam
        {
            get { return _dsParam; }
            set { _dsParam = value; }
        }
             
        public LinkedList llstSearchCondition
        {
            get { return _llstSearch; }
            set { _llstSearch = value; }
        }

        public List<string> lstRawColumn
        {
            get { return _lstRawColumn; }
            set { _lstRawColumn = value; }
        }


        public string complex_yn
        {
            get { return _complex_yn; }
            set { _complex_yn = value; }
        }
      
        #endregion
       
    }
}
