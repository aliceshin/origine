using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;

namespace BISTel.eSPC.Page.Report
{
    public partial class SPCControlChartModel : BasePageUCtrl
    {

        #region ::: Field

        eSPCWebService.eSPCWebService _wsSPC = null;
        CommonUtility _ComUtil;
        BSpreadUtility _bSpreadUtil;
        Initialization _Initialization;
        MultiLanguageHandler _lang;
        LinkedList _llstSearchCondition = new LinkedList();
        ChartInterface _chartVariable;
        DataTableGroupBy dtGroupBy = new DataTableGroupBy();
        ArrayList arrModelCongifRawID = new ArrayList();
        DataTable _dtChartData = new DataTable();

        string sStartTime = string.Empty;
        string sEndTime = string.Empty;
        string sParameter = string.Empty;
        string sSPCModel = string.Empty;
        string sProductID = string.Empty;
        string sEQPID = string.Empty;
        string sModelCongifRawid = string.Empty;
        string sDefaultChartList = string.Empty;
        string _line = string.Empty;

        int restrict_sample_count = Definition.RESTRICT_SAMPLE_COUNT;
        int iSearch = 1;


        #endregion


        #region ::: Properties

        public LinkedList llstSearchCondition
        {
            set
            {
                this._llstSearchCondition = value;
            }
            get
            {
                return this._llstSearchCondition;
            }
        }

        #endregion


        public SPCControlChartModel()
        {
            InitializeComponent();
        }


        #region ::: Override Method

        public override void PageInit()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.GlobalDefinition.IsDeveloping = true;

            this.KeyOfPage = this.GetType().FullName; // 필수 입력 (Key값을 받은 후 DC을 Active해주기 때문이다.

            this.InitializePage();
        }

        public override void PageSearch(LinkedList llstCondition)
        {

            _chartVariable = new ChartInterface();
            this._llstSearchCondition.Clear();

            if (!llstCondition.Contains(Definition.DynamicCondition_Search_key.AREA))
            {
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SELECT_CONDITION_DATA));
                return;
            }

            DataTable dt = null;
            if (llstCondition[Definition.DynamicCondition_Search_key.FROMDATE] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.FROMDATE];
                DateTime dtFrom = (DateTime)dt.Rows[0][Definition.DynamicCondition_Condition_key.DATETIME_VALUEDATA];
                sStartTime = CommonPageUtil.StartDate(dtFrom.ToString(Definition.DATETIME_FORMAT));
                
            }
            if (llstCondition[Definition.DynamicCondition_Search_key.TODATE] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.TODATE];
                DateTime dtTo = (DateTime)dt.Rows[0][Definition.DynamicCondition_Condition_key.DATETIME_VALUEDATA];
                sEndTime = CommonPageUtil.EndDate(dtTo.ToString(Definition.DATETIME_FORMAT));

            }

            this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.START_DTTS, sStartTime);
            this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.END_DTTS, sEndTime);



            if (llstCondition[Definition.DynamicCondition_Search_key.LINE] != null)
            {
                dt = (DataTable)llstCondition[Definition.DynamicCondition_Search_key.LINE];
                _line = dt.Rows[0][Definition.DynamicCondition_Condition_key.DISPLAYDATA].ToString();
                _chartVariable.LINE = _line;
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.LINE_RAWID, dt.Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString());
            }

            string strArea = _ComUtil.GetConditionString((DataTable)llstCondition[Definition.DynamicCondition_Search_key.AREA], Definition.DynamicCondition_Condition_key.AREA);
            if (!string.IsNullOrEmpty(strArea))
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.AREA, strArea);


            string strParamType = _ComUtil.GetConditionData((DataTable)llstCondition[Definition.DynamicCondition_Search_key.PARAM_TYPE], Definition.DynamicCondition_Condition_key.VALUEDATA);
            if (!string.IsNullOrEmpty(strParamType))
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.PARAM_TYPE_CD, strParamType);


            if (!llstCondition.Contains(Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "Please Select SPC Model.");
                return;
            }


            string strModelConfigRawID = ((DataTable)llstCondition[Definition.DynamicCondition_Search_key.SPC_MODEL_CONFIG_RAWID]).Rows[0][Definition.DynamicCondition_Condition_key.VALUEDATA].ToString();
            if (!string.IsNullOrEmpty(strModelConfigRawID))
                this._llstSearchCondition.Add(Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID, strModelConfigRawID);

            PROC_DataBinding();
        }

        #endregion

        #region ::: PageLoad & Initialize

        public void InitializePage()
        {
            this._ComUtil = new CommonUtility();
            this._bSpreadUtil = new BSpreadUtility();
            this._lang = MultiLanguageHandler.getInstance();
            this._wsSPC = new eSPCWebService.eSPCWebService();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
            this.InitializeLayout();
        }

        public void InitialUserInfo()
        {
        }

        public void InitializeLayout()
        {
            this.bTitlePanel1.Title = this._lang.GetVariable(Definition.TITLE_KEY_SPC_CONTROL_CHART);
            AddContrChart();

        }


        #endregion

        #region ::: User Defined Method.
        private void SetModelConfigRawID(DataTable _dt)
        {
            string _sModelCongifRawid = string.Empty;
            string strModelCongifRawID = string.Empty;
            string sCol = Definition.DynamicCondition_Condition_key.MODEL_CONFIG_RAWID;
            if (!_dt.Columns.Contains(sCol)) sCol = Definition.DynamicCondition_Condition_key.VALUEDATA;

            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                strModelCongifRawID = _dt.Rows[i][sCol].ToString();
                if (!arrModelCongifRawID.Contains(strModelCongifRawID))
                    arrModelCongifRawID.Add(strModelCongifRawID);
            }

        }

        private void PROC_DataBinding()
        {
            DataSet _ds = null;
            CreateChartDataTable _createChartDT = null;
            try
            {
                _dtChartData = new DataTable();
                _createChartDT = new CreateChartDataTable();

                this.MsgShow(this._lang.GetVariable("RMS_PROGRESS_SEARCH"));
                _ds = _wsSPC.GetSPCControlChartData(_llstSearchCondition.GetSerialData());
                if (DataUtil.IsNullOrEmptyDataSet(_ds))
                {
                    this.MsgClose();
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                }
                else
                {
                    this._dtChartData = CommonPageUtil.CLOBnBLOBParsing(_ds, _llstSearchCondition, false);
                    if (_ds != null) _ds.Dispose();

                    //초기설정된 값만큼만 조회
                    if (iSearch == 0)
                    {
                        if (_dtChartData.Rows.Count > this.restrict_sample_count)
                        {
                            DataTable dtCopy = new DataTable();
                            DataRow[] drSelect = _dtChartData.Select(null, Definition.CHART_COLUMN.TIME + " desc");
                            if (drSelect.Length > 0)
                                dtCopy = drSelect[0].Table.Clone();

                            for (int i = 0; i <= this.restrict_sample_count; i++)
                                dtCopy.ImportRow(drSelect[i]);

                            _dtChartData = DataUtil.DataTableImportRow(dtCopy.Select(null, "TIME"));
                            dtCopy.Dispose();
                        }
                    }
                    else
                        _dtChartData = DataUtil.DataTableImportRow(_dtChartData.Select(null, Definition.CHART_COLUMN.TIME));


                    if (string.IsNullOrEmpty(_dtChartData.Rows[0]["default_chart_list"].ToString()))
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Warning, string.Format(MSGHandler.GetMessage("GENERAL_NO_ITEM"), "Default Charts"));
                    }
                    else
                    {
                        _chartVariable.lstDefaultChart.Clear();
                        _chartVariable.DEFAULT_CHART = _dtChartData.Rows[0]["default_chart_list"].ToString();
                        _chartVariable.complex_yn = _dtChartData.Rows[0]["COMPLEX_YN"].ToString();
                        _chartVariable.MAIN_YN = _dtChartData.Rows[0]["MAIN_YN"].ToString();
                        _chartVariable.MODEL_CONFIG_RAWID = _dtChartData.Rows[0]["MODEL_CONFIG_RAWID"].ToString();
                        _chartVariable.SPC_MODEL = _dtChartData.Rows[0]["SPC_MODEL_NAME"].ToString();
                        //_chartVariable.PARAM_ALIAS = sParameter;
                        _chartVariable.PARAM_ALIAS = _dtChartData.Rows[0]["PARAM_ALIAS"].ToString();
                        _chartVariable.OPERATION_ID = _dtChartData.Rows[0][Definition.CHART_COLUMN.OPERATION_ID].ToString();
                        _chartVariable.PRODUCT_ID = _dtChartData.Rows[0][Definition.CHART_COLUMN.PRODUCT_ID].ToString();
                        _chartVariable.RESTRICT_SAMPLE_DAYS = _dtChartData.Rows[0]["RESTRICT_SAMPLE_DAYS"].ToString();
                        _chartVariable.AREA = _dtChartData.Rows[0]["AREA"].ToString();
                        _createChartDT.COMPLEX_YN = _chartVariable.complex_yn;
                        _chartVariable.lstDefaultChart = CommonPageUtil.DefaultChartSplit(_chartVariable.DEFAULT_CHART);
                        _chartVariable.CHART_PARENT_MODE = CHART_PARENT_MODE.SPC_CONTROL_CHART;
                        _chartVariable.lstRawColumn = _createChartDT.lstRawColumn; //_createChartDT.CallRefCol(_dtChartData);

                        Console.WriteLine(DateTime.Now.ToString());
                        _chartVariable.dtParamData = _createChartDT.GetMakeDataTable(_dtChartData);
                        Console.WriteLine(DateTime.Now.ToString());

                        if (_createChartDT != null) _createChartDT = null;
                    }
                }
                AddContrChart();

            }
            catch (Exception ex)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                this.MsgClose();
                if (_createChartDT != null) _createChartDT = null;
                if (_chartVariable != null) _chartVariable = null;
                if (_dtChartData != null) _dtChartData.Dispose();
            }
        }

        private void AddContrChart()
        {

            SPCChart spcChart = new SPCChart();
            spcChart.Dock = System.Windows.Forms.DockStyle.Fill;
            spcChart.sessionData = this.sessionData;
            spcChart.URL = this.URL;
            spcChart.ChartVariable = _chartVariable;
            spcChart.Height = this.bapChart.Height;
            spcChart.InitializePage();
            this.bapChart.Controls.Clear();
            this.bapChart.Controls.Add(spcChart);
        }
        #endregion

        #region ::: EventHandler

        private void bcboParamName_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //if(sParameter != this.bcboParamName.Text)
            //{ 
            //    ChartPaint();            
            //}
        }

        #endregion
    }
}
