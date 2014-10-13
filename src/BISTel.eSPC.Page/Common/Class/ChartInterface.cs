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
    public class ChartInterface : System.ICloneable
    {          
        private DataTable _dtParamData = new DataTable();
        private DataTable _dtResource = new DataTable();
        private DataTable _dtOCAP = new DataTable();
        
        private List<string> _lstRawColumn = new List<string>();
        protected List<string> _lstDefaultChart = new List<string>();
        private LinkedList _llstInfoCondition = new LinkedList();
        private LinkedList _llstDTSelectCondition = new LinkedList();

        private DateTime _dateTimeStart = DateTime.Now.AddDays(-7).Subtract(TimeSpan.FromHours(1));
        private DateTime _dateTimeEnd = DateTime.Now.Subtract(TimeSpan.FromHours(-1));
        

        private string _complex_yn="N";
        private string _paramAlias = string.Empty;
        private string _area = string.Empty;
        private string _defaultChart = string.Empty;
        private string _spcModel = string.Empty;
        private string _spcModelRawID = string.Empty;        
        private string _eqpModel = string.Empty;
        private string _line = string.Empty;
        private string _strRawCol = string.Empty;

        private string _eqp_id = string.Empty;
        private string _productID = string.Empty;
        private string _lotID = string.Empty;
        private string _operation_id = string.Empty;
        private string _substrate_id = string.Empty;
        private string _param_alias = string.Empty;
        

        private string _model_config_rawid = string.Empty;
        private string _OCAPRawID = string.Empty;
        private string _contextList = string.Empty;
        private string _main_yn = "N";
        private string _restrict_sample_days = "0";

        private CHART_PARENT_MODE _chart_parent_mode = CHART_PARENT_MODE.SPC_CONTROL_CHART;
                
        public string RESTRICT_SAMPLE_DAYS
        {
            set { this._restrict_sample_days = value; }
            get { return this._restrict_sample_days; }
        }                                  

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

        public string SPC_MODEL_RAWID
        {
            set { this._spcModelRawID = value; }
            get { return this._spcModelRawID; }
        }

        public string EQP_MODEL
        {
            set { this._eqpModel = value; }
            get { return this._eqpModel; }
        }


        public string MODEL_CONFIG_RAWID
        {
            set { this._model_config_rawid = value; }
            get { return this._model_config_rawid; }
        }

        public string MAIN_YN
        {
            set { this._main_yn = value; }
            get { return this._main_yn; }
        }


        public string PRODUCT_ID
        {
            set { this._productID = value; }
            get { return this._productID; }
        }

        public string LOT_ID
        {
            set { this._lotID = value; }
            get { return this._lotID; }
        }

        public string OPERATION_ID
        {
            set { this._operation_id = value; }
            get { return this._operation_id; }
        }


        public string EQP_ID
        {
            set { this._eqp_id = value; }
            get { return this._eqp_id; }
        }

        public string SUBSTRATE_ID
        {
            set { this._substrate_id = value; }
            get { return this._substrate_id; }
        }

        public string PARAM_ALIAS
        {
            set { this._param_alias = value; }
            get { return this._param_alias; }
        }


        public string LINE
        {
            set { this._line = value; }
            get { return this._line; }
        }


        public DataTable dtResource
        {
            get { return _dtResource; }
            set { _dtResource = value; }
        }


        public DataTable dtParamData
        {
            get { return _dtParamData; }
            set { _dtParamData = value; }
        }

        /// <summary>
        /// SPC_MODEL, EQP_MODEL,PRODUCT_ID, LOT_ID, STEP_ID
        /// </summary>
        public LinkedList llstInfoCondition
        {
            get { return _llstInfoCondition; }
            set { _llstInfoCondition = value; }
        }

        public List<string> lstRawColumn
        {
            get { return _lstRawColumn; }
            set { _lstRawColumn = value; }
        }

        public List<string> lstDefaultChart
        {
            get { return _lstDefaultChart; }
            set { _lstDefaultChart = value; }
        }


        public string complex_yn
        {
            get { return _complex_yn; }
            set { _complex_yn = value; }
        }


        public DateTime dateTimeStart
        {
            get { return _dateTimeStart; }
            set { _dateTimeStart = value; }
        }

        public DateTime dateTimeEnd
        {
            get { return _dateTimeEnd; }
            set { _dateTimeEnd = value; }
        }


        public string OCAPRawID
        {
            get { return _OCAPRawID; }
            set { _OCAPRawID = value; }
        }

        public DataTable dtOCAP
        {
            get { return _dtOCAP; }
            set { _dtOCAP = value; }
        }

        public string CONTEXT_LIST
        {
            set { this._contextList = value; }
            get { return this._contextList; }

        }

        public LinkedList llstDTSelectCondition
        {
            get { return _llstDTSelectCondition; }
            set { _llstDTSelectCondition = value; }
        }


        /// <summary>
        /// SPC_Modeling 화면에서 호출하는 경우 CHART_PARENT_MODE.MODELING 으로 넘긴다.
        /// </summary>
        public CHART_PARENT_MODE CHART_PARENT_MODE
        {

            get { return _chart_parent_mode; }
            set { _chart_parent_mode = value; }
        }

        //2012-03-23 added by rachel -->
        //[SPC-659]
        public void ReleaseChartData()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref _dtParamData);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref _dtResource);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref _dtOCAP);

            this._llstDTSelectCondition.Dispose();
            this._llstInfoCondition.Dispose();
            this._lstDefaultChart.Clear();
            this._lstRawColumn.Clear();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
