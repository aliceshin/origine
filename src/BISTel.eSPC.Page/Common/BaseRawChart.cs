using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;


using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl;

namespace BISTel.eSPC.Page.Common
{
    public partial class BaseRawChart : BasePageUCtrl
    {
        public MultiLanguageHandler _mlthandler;
        Color COLOR_SERIES_SYNC = Color.Gold;
        SourceDataManager _dataManager = null;
        Initialization _Initialization;
        DataTable _dtDataSource = null;
        List<string> _lstRawColumn = null;
        LinkedList _llstChartSeriesVisibleType = null;
        public LinkedList _mllstContextType = null;
        ChartUtility _chartUtil;
        SortedList _slButtonList = new SortedList();
        SeriesToolTip _stt = new SeriesToolTip();
        LinkedList _llstGroup = new LinkedList();

        string _legendLocation = Definition.Right;
        string _AxisDateTimeFormat = Definition.DATETIME_FORMAT_yyMMdd;
        string _xLabel = "INDEX";

        bool _bRegular = false;
        bool _bSeriesAllCheck = true;
        bool _IsVisibleLegendScrollBar = false;
        bool _IsVisibleShadow = false;

        private bool _isSummaryData = false; //SPC-929, KBLEE

        private DateTime _StartDateTime = DateTime.Now;
        private DateTime _EndDateTime = DateTime.Now;
        
        private DataSet _dsFilter;
        private DataSet _dsContextData;

        System.Windows.Forms.Panel pnlParent = null;

        bool isWaferColumn = false;

        private Steema.TeeChart.Styles.Shape _ShapeSeries = null;

        private bool _isPointSelected = false;

        private int _startPositionX = 0;
        private int _startPositionY = 0;
        private int _endPositionX = 0;
        private int _endPositionY = 0;

        public BaseRawChart()
        {
            InitializeComponent();
            InitializeVariable();
        }

        public void InitializePage()
        {
            this.InitializeDataButton();
            this.InitializeChart();
        }

        public void InitializeVariable()
        {
            this._Initialization = new Initialization();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization.InitializePath();
            this._lstRawColumn = new List<string>();
            this._chartUtil = new ChartUtility();
        }

        public void InitializeDataButton()
        {
            if (this.ParentItemkey == "" || this.ParentPagekey == "")
            {
                System.Console.WriteLine("XML NOT EXIST!");
                this.bButtonList1.Visible = false;
            }
            else
            {
                this._slButtonList = this._Initialization.InitializeChartButtonList(this.bButtonList1, this.ParentPagekey, this.ParentItemkey, this.sessionData);
                if (this._slButtonList.Count == 0)
                {
                    this.bButtonList1.Visible = false;
                }

                //SPC-820 By Louis
                for (int i = 0; i < this.bButtonList1.Items.Count; i++)
                {
                    if (this.bButtonList1.Items[i].Name.ToUpper() == "SPC_ZONE_DISPLAY" || this.bButtonList1.Items[i].Name.ToUpper() == "SPC_NORMALDIST")
                    {
                        this.bButtonList1.Items[i].Visible = false;
                    }
                    if (this.bButtonList1.Items[i].Name.ToUpper() == "BTN_ZOOM_IN")
                    {
                        this.bButtonList1.Items[i].Visible = false;
                    }
                }
            }
        }

        #region PageLoad & Initialize.
        public virtual void InitializeChart()
        {
            this.bButtonList1.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(bButtonList1_ButtonClick);

            this.SPCChart.Chart.Legend.CheckBoxes = true;
            this.SPCChart.IsVisibleLegendScrollBar = this.IsVisibleLegendScrollBar;
            this.SPCChart.IsVisibleShadow = this.IsVisibleShadow;
            this.SPCChart.AxisDateTimeFormat = this.AxisDateTimeFormat;
            this.SPCChart.IsUseAutomaticAxisOfTeeChart = true;
            this.SPCChart.IsAutoValueFormatLeftAxis = true;
            this.SPCChart.IsExponentialFormatLeftAxis = this._Initialization.GetExponentialAxis(Definition.PAGE_KEY_SPC_CONTROL_CHART_UC);
            this.SPCChart.IsUseChartEditor = false;
            this.SPCChart.LegendPosition = Steema.TeeChart.LegendAlignments.Right;
            this.SPCChart.Chart.Axes.DrawBehind = true;
            this.SPCChart.Chart.Axes.Left.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Right.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Top.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Bottom.Grid.Visible = false;

            this.SPCChart.SetXAxisRegularInterval(true);
            this.SPCChart.Chart.ClickLegend += new MouseEventHandler(Chart_ClickLegend);
        }

        private void RefreshChart()
        {
            this.SPCChart.Chart.Legend.CheckBoxes = true;
            this.SPCChart.IsVisibleLegendScrollBar = this.IsVisibleLegendScrollBar;
            this.SPCChart.IsVisibleShadow = this.IsVisibleShadow;
            this.SPCChart.AxisDateTimeFormat = this.AxisDateTimeFormat;
            this.SPCChart.IsUseAutomaticAxisOfTeeChart = true;
            this.SPCChart.IsAutoValueFormatLeftAxis = true;
            this.SPCChart.IsExponentialFormatLeftAxis = this._Initialization.GetExponentialAxis(Definition.PAGE_KEY_SPC_CONTROL_CHART_UC);
            this.SPCChart.IsUseChartEditor = false;
            this.SPCChart.LegendPosition = Steema.TeeChart.LegendAlignments.Right;
            this.SPCChart.Chart.Axes.DrawBehind = true;
            this.SPCChart.Chart.Axes.Left.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Right.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Top.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Bottom.Grid.Visible = false;
            this.SPCChart.SetXAxisRegularInterval(true);
        }


        #endregion



        #region ::: Properties



        public DataTable dtDataSource
        {

            get
            {
                return _dtDataSource;
            }
            set
            {
                this._dtDataSource = value;
            }

        }

        public DataSet dsFilter
        {
            get
            {
                return _dsFilter;
            }
            set
            {
                this._dsFilter = value;
            }
        }

        public DataSet dsContextData
        {
            get
            {
                return _dsContextData;
            }
            set
            {
                this._dsContextData = value;
            }
        }

        public BTeeChart Get_Chart()
        {
            return this.SPCChart;
        }



        public List<string> lstRawColumn
        {

            get
            {
                return _lstRawColumn;
            }
            set
            {
                this._lstRawColumn = value;
            }

        }


        public string xLabel
        {
            get
            {
                return _xLabel;
            }
            set
            {
                this._xLabel = value;
            }
        }

        private string ParentToolTipItemkey = string.Empty;
        public string ToolTipItemkey
        {
            get
            {
                return ParentToolTipItemkey;
            }
            set
            {
                this.ParentToolTipItemkey = value;
            }
        }

        private string ParentPagekey = "";
        public string Pagekey
        {
            get
            {
                return ParentPagekey;
            }
            set
            {
                this.ParentPagekey = value;
            }
        }

        private string ParentName = "";
        public string PageName
        {
            get
            {
                return ParentName;
            }
            set
            {
                this.ParentName = value;
            }
        }

        private string ParentItemkey = "";
        public string Itemkey
        {
            get
            {
                return ParentItemkey;
            }
            set
            {
                this.ParentItemkey = value;
            }
        }

        public DateTime StartDateTime
        {
            get { return this._StartDateTime; }
            set { this._StartDateTime = value; }
        }


        public DateTime EndDateTime
        {
            get { return this._EndDateTime; }
            set { this._EndDateTime = value; }
        }

        public SourceDataManager DataManager
        {
            get { return this._dataManager; }
            set { this._dataManager = value; }
        }

        public BTeeChart SPCChart
        {
            get { return this.bTeeChart1; }
        }

        public BTitlePanel SPCChartTitlePanel
        {
            get { return this.bTitlePanel1; }
        }

        public string Title
        {
            get { return this.bTitlePanel1.Title; }
            set { this.bTitlePanel1.Title = value; }
        }

        public string NAME
        {
            get { return this.bTitlePanel1.Name; }
            set { this.bTitlePanel1.Name = value; }
        }

        public System.Windows.Forms.Panel ParentControl
        {
            get { return this.pnlParent; }
            set { this.pnlParent = value; }
        }

        public bool isViewButtonList
        {
            set { this.bButtonList1.Visible = value; }
        }


        public LinkedList llstChartSeriesVisibleType
        {
            get { return this._llstChartSeriesVisibleType; }
            set { this._llstChartSeriesVisibleType = value; }
        }

        public LinkedList mllstContextType
        {
            get { return this._mllstContextType; }
            set { this._mllstContextType = value; }
        }


        public Steema.TeeChart.LegendAlignments LegendPosition
        {
            get { return this.SPCChart.LegendPosition; }
            set { this.SPCChart.LegendPosition = value; }
        }


        public string AxisDateTimeFormat
        {
            get { return this._AxisDateTimeFormat; }
            set { this._AxisDateTimeFormat = value; }
        }


        public bool IsVisibleLegendScrollBar
        {
            get { return this._IsVisibleLegendScrollBar; }
            set { this._IsVisibleLegendScrollBar = value; }
        }


        public bool IsVisibleShadow
        {
            get { return this._IsVisibleShadow; }
            set { this._IsVisibleShadow = value; }
        }

        public bool IsWaferColumn
        {
            get { return this.isWaferColumn; }
            set { this.isWaferColumn = value; }
        }

        public bool IsPointMarking
        {
            get { return this.bckbPointMarking.Checked; }
        }

        //SPC-929, KBLEE, START
        public bool ISSUMMARYDATA
        {
            get { return this._isSummaryData; }
            set { this._isSummaryData = value; }
        }
        //SPC-929, KBLEE, END
        #endregion

        #region ::: Override Method.
        #endregion


        #region ::: Virtual Method.

        ///////////////////////////////////////////////////////////////////////////////////
        public virtual bool DrawSPCChart()
        {
            return DrawSPCChart(false, -1, -1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample">Whether sampling data or not</param>
        /// <param name="samplePeriod">sampling period</param>
        /// <param name="sampleCount">sample count in the sampling period</param>
        /// <returns></returns>
        public virtual bool DrawSPCChart(bool sample, int samplePeriod, int sampleCount)
        {
            try
            {
                if (this.DataManager.RawDataTable.Rows.Count < 1)
                {
                    return false;
                }

                

                for (int i = 0; i < this.DataManager.RawDataTable.Columns.Count; i++)
                {
                    if (this.DataManager.RawDataTable.Columns[i].ColumnName.Contains("wafer"))
                    {
                        isWaferColumn = true;
                        break;
                    }
                }

                if (isWaferColumn)
                {

                    this._mllstContextType.Add("RAW", null);
                    this._mllstContextType.Add("RAW_USL", null);
                    this._mllstContextType.Add("RAW_UCL", null);
                    this._mllstContextType.Add("RAW_TARGET", null);
                    this._mllstContextType.Add("RAW_LCL", null);
                    this._mllstContextType.Add("RAW_LSL", null);
                    this._mllstContextType.Add("PARAM_LIST", null);
                    this._mllstContextType.Add("RAW_DTTS", null);
                    if (!this._mllstContextType.Contains("EQP_ID"))
                        this._mllstContextType.Add("EQP_ID", null);
                    if (!this._mllstContextType.Contains("MODULE_ALIAS"))
                        this._mllstContextType.Add("MODULE_ALIAS", null);
                    if (!this._mllstContextType.Contains("MODULE_NAME"))
                        this._mllstContextType.Add("MODULE_NAME", null);
                    for (int i = 0; i < this.DataManager.RawDataTableOriginal.Columns.Count; i++)
                    {
                        if (!this._mllstContextType.Contains(this.DataManager.RawDataTableOriginal.Columns[i].ColumnName.ToUpper()))
                        {
                            bool bExist = false;
                            foreach (SPCStruct.ContextTypeInfo spcstruct in _mllstContextType.GetValueList())
                            {
                                if (spcstruct != null && spcstruct.NAME.ToUpper().Equals(this.DataManager.RawDataTableOriginal.Columns[i].ColumnName.ToUpper()))
                                {
                                    bExist = true;
                                    break;
                                }
                            }

                            if(!bExist)
                            {
                                this.DataManager.RawDataTableOriginal.Columns.Remove(this.DataManager.RawDataTableOriginal.Columns[i].ColumnName);
                                i--;
                            }
                        }
                    }

                    if (this.DataManager.RawDataTableOriginal.Rows.Count > 0)
                    {
                        this._dtDataSource = this.DataManager.RawDataTableOriginal.Clone();
                        DataTable dtTemp = new DataTable();
                        dtTemp = this.DataManager.RawDataTableOriginal.Clone();

                        DateTime nextSampleStartDate =
                                    DateTime.Parse(this.DataManager.RawDataTableOriginal.Rows[0][COLUMN.RAW_DTTS].ToString().Split(';')[0]);
                                nextSampleStartDate = new DateTime(nextSampleStartDate.Year, nextSampleStartDate.Month, nextSampleStartDate.Day);
                        int sampleCountTemp = 0;

                        foreach (DataRow dr in DataManager.RawDataTableOriginal.Rows)
                        {
                            if(sample)
                            {
                                if (DateTime.Parse(dr[COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
                                {
                                    while (DateTime.Parse(dr[COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
                                    {
                                        nextSampleStartDate = nextSampleStartDate.AddDays(samplePeriod);
                                    }
                                    sampleCountTemp = 1;
                                }
                                else if (sampleCountTemp < sampleCount)
                                {
                                    sampleCountTemp++;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            dtTemp.Clear();
                            if (dr[COLUMN.RAW_DTTS].ToString().Length != 0)
                            {
                                if ((dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr["RAW"].ToString().Split(';').Length && dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr[COLUMN.PARAM_LIST].ToString().Split(';').Length) ||
                                    (dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr["RAW"].ToString().Split(';').Length && dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr[COLUMN.PARAM_LIST].ToString().Split(';').Length - 1))
                                {
                                    for (int i = 0; i < dr["RAW"].ToString().Split(';').Length; i++)
                                    {
                                        if (dr["RAW"].ToString().Split(';')[i].Length > 0)
                                            dtTemp.ImportRow(dr);
                                    }

                                    for (int j = 0; j < dtTemp.Rows.Count; j++)
                                    {
                                        dtTemp.Rows[j]["RAW"] = dtTemp.Rows[j]["RAW"].ToString().Split(';')[j];
                                        for (int k = 0; k < dtTemp.Columns.Count; k++)
                                        {
                                            if (dtTemp.Columns[k].ColumnName.ToUpper() != COLUMN.DEFAULT_CHART_LIST && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.RAW && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.OCAP_RAWID)
                                            {
                                                if (dtTemp.Rows[j][k] != null && dtTemp.Rows[j][k].ToString().Length > 0 && dtTemp.Rows[j][k].ToString().Split(';').Length >= dtTemp.Rows.Count)
                                                {
                                                    dtTemp.Rows[j][k] = dtTemp.Rows[j][k].ToString().Split(';')[j];
                                                }
                                            }
                                        }
                                        //dtTemp.Rows[j]["RAW"] = dtTemp.Rows[j]["RAW"].ToString().Split(';')[j];
                                        //dtTemp.Rows[j][COLUMN.PARAM_LIST] = dtTemp.Rows[j][COLUMN.PARAM_LIST].ToString().Split(';')[j];
                                        //dtTemp.Rows[j][COLUMN.RAW_DTTS] = dtTemp.Rows[j][COLUMN.RAW_DTTS].ToString().Split(';')[j];
                                        //if (dtTemp.Rows[j]["SUBSTRATE_ID"] != null && dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Length > 0 && dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["SUBSTRATE_ID"] = dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Split(';')[j];
                                        //}

                                        //if (dtTemp.Rows[j]["RAW_USL"] != null && dtTemp.Rows[j]["RAW_USL"].ToString().Length > 0 && dtTemp.Rows[j]["RAW_USL"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["RAW_USL"] = dtTemp.Rows[j]["RAW_USL"].ToString().Split(';')[j];
                                        //}
                                        //if (dtTemp.Rows[j]["RAW_UCL"] != null && dtTemp.Rows[j]["RAW_UCL"].ToString().Length > 0 && dtTemp.Rows[j]["RAW_UCL"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["RAW_UCL"] = dtTemp.Rows[j]["RAW_UCL"].ToString().Split(';')[j];
                                        //}
                                        //if (dtTemp.Rows[j]["RAW_TARGET"] != null && dtTemp.Rows[j]["RAW_TARGET"].ToString().Length > 0 && dtTemp.Rows[j]["RAW_TARGET"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["RAW_TARGET"] = dtTemp.Rows[j]["RAW_TARGET"].ToString().Split(';')[j];
                                        //}
                                        //if (dtTemp.Rows[j]["RAW_LCL"] != null && dtTemp.Rows[j]["RAW_LCL"].ToString().Length > 0 && dtTemp.Rows[j]["RAW_LCL"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["RAW_LCL"] = dtTemp.Rows[j]["RAW_LCL"].ToString().Split(';')[j];
                                        //}
                                        //if (dtTemp.Rows[j]["RAW_LSL"] != null && dtTemp.Rows[j]["RAW_LSL"].ToString().Length > 0 && dtTemp.Rows[j]["RAW_LSL"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["RAW_LSL"] = dtTemp.Rows[j]["RAW_LSL"].ToString().Split(';')[j];
                                        //}
                                    }

                                    this._dtDataSource.Merge(dtTemp);
                                }
                            }
                        }

                        if (this.PageName == "BISTel.eSPC.Page.Modeling.SPCDataRestrictionUC")
                        {
                            if (_dsFilter != null && _dsFilter.Tables.Count > 0 && _dsFilter.Tables[0].Rows.Count > 0)
                            {
                                DataRestriction restriction = new DataRestriction();
                                DataTable dtFilterData = restriction.GetEQPFilterData(_dsFilter, _dsContextData, this._dtDataSource);
                                if (dtFilterData != null)
                                {
                                    this._dtDataSource = dtFilterData;
                                }
                                this._dtDataSource = restriction.GetIncludeFilterResultDataByRaw(_dsFilter, _dsContextData, this._dtDataSource);

                                if (this._dtDataSource != null)
                                {
                                    this._dtDataSource = restriction.GetExcludeFilterResultDataByRaw(_dsFilter, _dsContextData, this._dtDataSource);
                                }
                            }
                        }

                        if (this._dtDataSource.Rows.Count > 0)
                        {
                            this._dtDataSource = DataUtil.DataTableImportRow(this._dtDataSource.Select(null, COLUMN.RAW_DTTS));
                        }

                        this._dtDataSource.Columns.Add("TIME", typeof(DateTime));
                        this._dtDataSource.Columns.Add(Definition.CHART_COLUMN.TIME2, typeof(string));
                        this._dtDataSource.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, typeof(int));


                        for (int i = 0; i < this._dtDataSource.Rows.Count; i++)
                        {
                            string sTime = this._dtDataSource.Rows[i]["raw_dtts"].ToString();
                            this._dtDataSource.Rows[i][Definition.CHART_COLUMN.TIME2] = sTime.Substring(0, 16);
                            this._dtDataSource.Rows[i]["TIME"] = sTime;
                            this._dtDataSource.Rows[i][CommonChart.COLUMN_NAME_SEQ_INDEX] = i;
                        }
                    }

                    

                    SeriesInfo si = null;
                    if (this._dtDataSource.Rows.Count > 0)
                    {
                        for (int i = 0; i < this._dtDataSource.Columns.Count; i++)
                        {
                            string seriesName = this._dtDataSource.Columns[i].ColumnName.ToUpper();
                            if (seriesName == "RAW" || seriesName == "RAW_UCL" || seriesName == "RAW_LCL" || seriesName == "RAW_LSL" || seriesName == "RAW_USL" || seriesName == "RAW_TARGET")
                            {
                                si = new SeriesInfo(typeof(Line), seriesName.Replace("RAW_", ""), this._dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName.ToLower());
                                switch (seriesName)
                                {
                                    case "RAW_UCL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.UCL); break;
                                    case "RAW_LCL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LCL); break;
                                    case "RAW_LSL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LSL); break;
                                    case "RAW_USL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.USL); break;
                                    case "RAW_TARGET": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.TARGET); break;
                                    case "RAW": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.AVG); break;
                                }
                                this.SPCChart.AddSeries(si);
                            }
                        }

                        Series s = null;
                        for (int i = 0; i < this.SPCChart.Chart.Series.Count; i++)
                        {
                            s = this.SPCChart.Chart.Series[i];

                            ((Line)s).LinePen.Width = 2;
                            ((Line)s).OutLine.Visible = false;
                            ((Line)s).ColorEachLine = false;
                            ((Line)s).Pointer.Visible = false;
                            ((Line)s).Active = true;

                            if (s.Title == "RAW")
                            {
                                ((Line)s).LinePen.Width = 1;
                                ((Line)s).Pointer.Visible = true;
                            }
                        }

                        this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
                        this.SPCChart.Chart.Axes.Bottom.Labels.ValueFormat = "yy-MM-dd";
                        this.SPCChart.ChangeXLabelColumnDefault("TIME2");
                        this.SPCChart.Chart.Axes.Bottom.Increment = Steema.TeeChart.Utils.DateTimeStep[(int)Steema.TeeChart.DateTimeSteps.OneDay];
                        this.SPCChart.Refresh();

                    }
                }
                else
                {
                    if (this.ParentName == "BISTel.eSPC.Page.Modeling.SPCDataRestrictionUC")
                    {
                        if (_dsFilter != null && _dsFilter.Tables.Count > 0 && _dsFilter.Tables[0].Rows.Count > 0)
                        {
                            DataRestriction restriction = new DataRestriction();

                            DataTable dtFilterData = restriction.GetEQPFilterData(_dsFilter, _dsContextData, this.DataManager.RawDataTable);
                            if (dtFilterData != null)
                            {
                                this.DataManager.RawDataTable = dtFilterData;
                            }

                            DataTable dtResult = restriction.GetIncludeFilterResultData(_dsFilter, _dsContextData, this.DataManager.RawDataTable);
                            if (dtResult != null)
                            {
                                this._dataManager.RawDataTable = dtResult.Copy();
                            }
                            else
                            {
                                this._dataManager.RawDataTable.Clear();
                            }

                            dtResult = restriction.GetExcludeFilterResultData(_dsFilter, _dsContextData, this.DataManager.RawDataTable);
                            if (dtResult != null)
                            {
                                this._dataManager.RawDataTable = dtResult.Copy();
                            }
                            else
                            {
                                this._dataManager.RawDataTable.Clear();
                            }
                        }
                    }

                    MakeDataSourceToDrawSPCChart();
                    DrawChartWithSeriesInfo();
                    ChangeSeriesStyle();
                }
                
                MakeTipInfo();

                //added by enkim 2012.12.12 SPC-839
                if (!this.SPCChart.Chart.Axes.Left.Labels.ValueFormat.Contains("E"))
                {
                    this.SPCChart.Chart.Axes.Left.Labels.ValueFormat = "#,##0.###";
                }
                //added end

                return true;
            }
            catch (Exception ex)
            {
                MsgClose();
                this.SPCChart.ClearChart();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                //MSGHandler.DisplayMessage(MSGType.Error, "There is an empty or non-numeric value.");
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sample">Whether sampling data or not</param>
        /// <param name="samplePeriod">sampling period</param>
        /// <param name="sampleCount">sample count in the sampling period</param>
        /// <returns></returns>
        public virtual bool DrawSPCChart(bool sample, int samplePeriod, int sampleCount, int sampleWaferCount)
        {
            try
            {
                if (this.DataManager.RawDataTable.Rows.Count < 1)
                {
                    return false;
                }



                for (int i = 0; i < this.DataManager.RawDataTable.Columns.Count; i++)
                {
                    if (this.DataManager.RawDataTable.Columns[i].ColumnName.Contains("wafer"))
                    {
                        isWaferColumn = true;
                        break;
                    }
                }

                if (isWaferColumn)
                {

                    this._mllstContextType.Add("RAW", null);
                    this._mllstContextType.Add("RAW_USL", null);
                    this._mllstContextType.Add("RAW_UCL", null);
                    this._mllstContextType.Add("RAW_TARGET", null);
                    this._mllstContextType.Add("RAW_LCL", null);
                    this._mllstContextType.Add("RAW_LSL", null);
                    this._mllstContextType.Add("PARAM_LIST", null);
                    this._mllstContextType.Add("RAW_DTTS", null);
                    if (!this._mllstContextType.Contains("EQP_ID"))
                        this._mllstContextType.Add("EQP_ID", null);
                    if (!this._mllstContextType.Contains("MODULE_ALIAS"))
                        this._mllstContextType.Add("MODULE_ALIAS", null);
                    if (!this._mllstContextType.Contains("MODULE_NAME"))
                        this._mllstContextType.Add("MODULE_NAME", null);
                    //for (int i = 0; i < this.DataManager.RawDataTableOriginal.Columns.Count; i++)
                    //{
                    //    if (!this._mllstContextType.Contains(this.DataManager.RawDataTableOriginal.Columns[i].ColumnName.ToUpper()))
                    //    {
                    //        this.DataManager.RawDataTableOriginal.Columns.Remove(this.DataManager.RawDataTableOriginal.Columns[i].ColumnName);
                    //        i--;
                    //    }
                    //}

                    for (int i = 0; i < this.DataManager.RawDataTableOriginal.Columns.Count; i++)
                    {
                        if (!this._mllstContextType.Contains(this.DataManager.RawDataTableOriginal.Columns[i].ColumnName.ToUpper()))
                        {
                            bool isContainsColumn = false;
                            for (int j = 0; j < this._mllstContextType.Count; j++)
                            {
                                if (this._mllstContextType.GetValue(j) != null)
                                {
                                    string strContextName = ((BISTel.eSPC.Common.SPCStruct.ContextTypeInfo)this._mllstContextType.GetValue(j)).NAME;
                                    if (strContextName != null)
                                    {
                                        if (this.DataManager.RawDataTableOriginal.Columns[i].ColumnName.ToUpper() == strContextName)
                                        {
                                            isContainsColumn = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (!isContainsColumn)
                            {
                                this.DataManager.RawDataTableOriginal.Columns.Remove(this.DataManager.RawDataTableOriginal.Columns[i].ColumnName);
                                i--;
                            }
                        }
                    }

                    if (this.DataManager.RawDataTableOriginal.Rows.Count > 0)
                    {
                        this._dtDataSource = this.DataManager.RawDataTableOriginal.Clone();
                        DataTable dtTemp = new DataTable();
                        dtTemp = this.DataManager.RawDataTableOriginal.Clone();

                        DateTime nextSampleStartDate =
                                    DateTime.Parse(this.DataManager.RawDataTableOriginal.Rows[0][COLUMN.RAW_DTTS].ToString().Split(';')[0]);
                        nextSampleStartDate = new DateTime(nextSampleStartDate.Year, nextSampleStartDate.Month, nextSampleStartDate.Day);
                        int sampleCountTemp = 0;

                        foreach (DataRow dr in DataManager.RawDataTableOriginal.Rows)
                        {
                            if (sample)
                            {
                                if (DateTime.Parse(dr[COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
                                {
                                    while (DateTime.Parse(dr[COLUMN.RAW_DTTS].ToString().Split(';')[0]) > nextSampleStartDate)
                                    {
                                        nextSampleStartDate = nextSampleStartDate.AddDays(samplePeriod);
                                    }
                                    sampleCountTemp = 1;
                                }
                                else if (sampleCountTemp < sampleCount)
                                {
                                    sampleCountTemp++;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            dtTemp.Clear();
                            if (dr[COLUMN.RAW_DTTS].ToString().Length != 0)
                            {
                                if (dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr["RAW"].ToString().Split(';').Length && dr[COLUMN.RAW_DTTS].ToString().Split(';').Length == dr[COLUMN.PARAM_LIST].ToString().Split(';').Length)
                                {
                                    for (int i = 0; i < dr["RAW"].ToString().Split(';').Length; i++)
                                    {
                                        if (dr["RAW"].ToString().Split(';')[i].Length > 0)
                                        {
                                            if (sample)
                                            {
                                                if (i < sampleWaferCount)
                                                {
                                                    dtTemp.ImportRow(dr);
                                                }
                                            }
                                            else
                                                dtTemp.ImportRow(dr);
                                        }
                                    }

                                    for (int j = 0; j < dtTemp.Rows.Count; j++)
                                    {
                                        dtTemp.Rows[j]["RAW"] = dtTemp.Rows[j]["RAW"].ToString().Split(';')[j];
                                        for (int k = 0; k < dtTemp.Columns.Count; k++)
                                        {
                                            if (dtTemp.Columns[k].ColumnName.ToUpper() != COLUMN.DEFAULT_CHART_LIST && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.RAW && dtTemp.Columns[k].ColumnName.ToUpper() != Definition.CHART_COLUMN.OCAP_RAWID)
                                            {
                                                if (dtTemp.Rows[j][k] != null && dtTemp.Rows[j][k].ToString().Length > 0 && dtTemp.Rows[j][k].ToString().Split(';').Length >= dtTemp.Rows.Count)
                                                {
                                                    dtTemp.Rows[j][k] = dtTemp.Rows[j][k].ToString().Split(';')[j];
                                                }
                                            }
                                        }
                                        //dtTemp.Rows[j]["RAW"] = dtTemp.Rows[j]["RAW"].ToString().Split(';')[j];
                                        //dtTemp.Rows[j][COLUMN.PARAM_LIST] = dtTemp.Rows[j][COLUMN.PARAM_LIST].ToString().Split(';')[j];
                                        //dtTemp.Rows[j][COLUMN.RAW_DTTS] = dtTemp.Rows[j][COLUMN.RAW_DTTS].ToString().Split(';')[j];
                                        //if (dtTemp.Rows[j]["SUBSTRATE_ID"] != null && dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Length > 0 && dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["SUBSTRATE_ID"] = dtTemp.Rows[j]["SUBSTRATE_ID"].ToString().Split(';')[j];
                                        //}

                                        //if (dtTemp.Rows[j]["RAW_USL"] != null && dtTemp.Rows[j]["RAW_USL"].ToString().Length > 0 && dtTemp.Rows[j]["RAW_USL"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["RAW_USL"] = dtTemp.Rows[j]["RAW_USL"].ToString().Split(';')[j];
                                        //}
                                        //if (dtTemp.Rows[j]["RAW_UCL"] != null && dtTemp.Rows[j]["RAW_UCL"].ToString().Length > 0 && dtTemp.Rows[j]["RAW_UCL"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["RAW_UCL"] = dtTemp.Rows[j]["RAW_UCL"].ToString().Split(';')[j];
                                        //}
                                        //if (dtTemp.Rows[j]["RAW_TARGET"] != null && dtTemp.Rows[j]["RAW_TARGET"].ToString().Length > 0 && dtTemp.Rows[j]["RAW_TARGET"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["RAW_TARGET"] = dtTemp.Rows[j]["RAW_TARGET"].ToString().Split(';')[j];
                                        //}
                                        //if (dtTemp.Rows[j]["RAW_LCL"] != null && dtTemp.Rows[j]["RAW_LCL"].ToString().Length > 0 && dtTemp.Rows[j]["RAW_LCL"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["RAW_LCL"] = dtTemp.Rows[j]["RAW_LCL"].ToString().Split(';')[j];
                                        //}
                                        //if (dtTemp.Rows[j]["RAW_LSL"] != null && dtTemp.Rows[j]["RAW_LSL"].ToString().Length > 0 && dtTemp.Rows[j]["RAW_LSL"].ToString().Split(';').Length > 1)
                                        //{
                                        //    dtTemp.Rows[j]["RAW_LSL"] = dtTemp.Rows[j]["RAW_LSL"].ToString().Split(';')[j];
                                        //}
                                    }

                                    this._dtDataSource.Merge(dtTemp);
                                }
                            }
                        }


                        if (this.PageName == "BISTel.eSPC.Page.Modeling.SPCDataRestrictionUC")
                        {
                            if (_dsFilter != null && _dsFilter.Tables.Count > 0 && _dsFilter.Tables[0].Rows.Count > 0)
                            {
                                DataRestriction restriction = new DataRestriction();
                                DataTable dtFilterData = restriction.GetEQPFilterData(_dsFilter, _dsContextData, this._dtDataSource);
                                if (dtFilterData != null)
                                {
                                    this._dtDataSource = dtFilterData;
                                }
                                this._dtDataSource = restriction.GetIncludeFilterResultDataByRaw(_dsFilter, _dsContextData, this._dtDataSource);

                                if (this._dtDataSource != null)
                                {
                                    this._dtDataSource = restriction.GetExcludeFilterResultDataByRaw(_dsFilter, _dsContextData, this._dtDataSource);
                                }
                            }
                        }

                        if (this._dtDataSource.Rows.Count > 0)
                        {
                            this._dtDataSource = DataUtil.DataTableImportRow(this._dtDataSource.Select(null, COLUMN.RAW_DTTS));
                        }

                        this._dtDataSource.Columns.Add("TIME", typeof(DateTime));
                        this._dtDataSource.Columns.Add(Definition.CHART_COLUMN.TIME2, typeof(string));
                        this._dtDataSource.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, typeof(int));


                        for (int i = 0; i < this._dtDataSource.Rows.Count; i++)
                        {
                            string sTime = this._dtDataSource.Rows[i]["raw_dtts"].ToString();
                            this._dtDataSource.Rows[i][Definition.CHART_COLUMN.TIME2] = sTime.Substring(0, 16);
                            this._dtDataSource.Rows[i]["TIME"] = sTime;
                            this._dtDataSource.Rows[i][CommonChart.COLUMN_NAME_SEQ_INDEX] = i;
                        }
                    }

                    SeriesInfo si = null;
                    if (this._dtDataSource.Rows.Count > 0)
                    {
                        for (int i = 0; i < this._dtDataSource.Columns.Count; i++)
                        {
                            string seriesName = this._dtDataSource.Columns[i].ColumnName.ToUpper();
                            if (seriesName == "RAW" || seriesName == "RAW_UCL" || seriesName == "RAW_LCL" || seriesName == "RAW_LSL" || seriesName == "RAW_USL" || seriesName == "RAW_TARGET")
                            {
                                si = new SeriesInfo(typeof(Line), seriesName.Replace("RAW_", ""), this._dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName.ToLower());
                                switch (seriesName)
                                {
                                    case "RAW_UCL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.UCL); break;
                                    case "RAW_LCL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LCL); break;
                                    case "RAW_LSL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LSL); break;
                                    case "RAW_USL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.USL); break;
                                    case "RAW_TARGET": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.TARGET); break;
                                    case "RAW": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.AVG); break;
                                }
                                this.SPCChart.AddSeries(si);
                            }
                        }

                        Series s = null;
                        for (int i = 0; i < this.SPCChart.Chart.Series.Count; i++)
                        {
                            s = this.SPCChart.Chart.Series[i];

                            ((Line)s).LinePen.Width = 2;
                            ((Line)s).OutLine.Visible = false;
                            ((Line)s).ColorEachLine = false;
                            ((Line)s).Pointer.Visible = false;
                            ((Line)s).Active = true;

                            if (s.Title == "RAW")
                            {
                                ((Line)s).LinePen.Width = 1;
                                ((Line)s).Pointer.Visible = true;
                            }
                        }

                        this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
                        this.SPCChart.Chart.Axes.Bottom.Labels.ValueFormat = "yy-MM-dd";
                        this.SPCChart.ChangeXLabelColumnDefault("TIME2");
                        this.SPCChart.Chart.Axes.Bottom.Increment = Steema.TeeChart.Utils.DateTimeStep[(int)Steema.TeeChart.DateTimeSteps.OneDay];
                        this.SPCChart.Refresh();

                    }
                }
                else
                {
                    if (this.PageName == "BISTel.eSPC.Page.Modeling.SPCDataRestrictionUC")
                    {
                        if (_dsFilter != null && _dsFilter.Tables.Count > 0 && _dsFilter.Tables[0].Rows.Count > 0)
                        {
                            DataRestriction restriction = new DataRestriction();

                            DataTable dtFilterData = restriction.GetEQPFilterData(_dsFilter, _dsContextData, this.DataManager.RawDataTable);
                            if (dtFilterData != null)
                            {
                                this.DataManager.RawDataTable = dtFilterData;
                            }

                            DataTable dtResult = restriction.GetIncludeFilterResultData(_dsFilter, _dsContextData, this.DataManager.RawDataTable);
                            if (dtResult != null)
                            {
                                this._dataManager.RawDataTable = dtResult.Copy();
                            }
                            else
                            {
                                this._dataManager.RawDataTable.Clear();
                            }

                            dtResult = restriction.GetExcludeFilterResultData(_dsFilter, _dsContextData, this.DataManager.RawDataTable);
                            if (dtResult != null)
                            {
                                this._dataManager.RawDataTable = dtResult.Copy();
                            }
                            else
                            {
                                this._dataManager.RawDataTable.Clear();
                            }
                        }
                    }

                    MakeDataSourceToDrawSPCChart();
                    DrawChartWithSeriesInfo();
                    ChangeSeriesStyle();
                }

                //added by enkim 2012.12.12 SPC-839
                if (!this.SPCChart.Chart.Axes.Left.Labels.ValueFormat.Contains("E"))
                {
                    this.SPCChart.Chart.Axes.Left.Labels.ValueFormat = "#,##0.###";
                }
                //added end

                MakeTipInfo();

                return true;
            }
            catch (Exception ex)
            {
                MsgClose();
                this.SPCChart.ClearChart();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                //MSGHandler.DisplayMessage(MSGType.Error, "There is an empty or non-numeric value.");
                return false;
            }
        }



        /// <summary>
        /// Chart를 Draw 한다. [Scatter/Line Chart에서 사용]
        /// Group별로 Chart를 생성한다.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="groupInfo"></param>
        /// <returns></returns>
        public virtual bool DrawSPCChart(DataRow groupInfo)
        {
            return false;
        }


        public virtual void ClearChart()
        {
            this.SPCChart.ClearChart();
            this._dtDataSource = null;
        }

        protected virtual void InitChart()
        {

        }


        protected virtual void MakeDataSourceToDrawSPCChart()
        {
            return;
        }

        protected virtual void MakeDataSourceToDrawSPCChart(DataRow groupInfo)
        {
            return;
        }



        protected virtual void DrawChartWithSeriesInfo()
        {
            return;
        }

        protected virtual void DrawChartWithRawSeriesInfo()
        {
            return;
        }

        public virtual void ChangeSeriesStyle()
        {
            Series s = null;
            for (int i = 0; i < this.SPCChart.Chart.Series.Count; i++)
            {
                s = this.SPCChart.Chart.Series[i];

                //((Line)s).Pointer.HorizSize = 3;
                //((Line)s).Pointer.VertSize = 3;
                ((Line)s).LinePen.Width = 2;
                //((Line)s).Pointer.Style = PointerStyles.Circle;
                ((Line)s).OutLine.Visible = false;
                ((Line)s).ColorEachLine = false;

                if (s.Title == Definition.CHART_COLUMN.UCL || s.Title == Definition.CHART_COLUMN.LCL)
                {
                    ((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.CONTROL_LIMIT);
                    ((Line)s).Pointer.Visible = isPointerVisible();
                }
                else if (s.Title == Definition.CHART_COLUMN.LSL || s.Title == Definition.CHART_COLUMN.USL)
                {
                    ((Line)s).LinePen.Width = 2;
                    ((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.SPEC);
                    ((Line)s).Pointer.Visible = isPointerVisible();
                }
                else if (s.Title == Definition.CHART_COLUMN.RANGE
                        || s.Title == Definition.CHART_COLUMN.STDDEV
                        || s.Title == Definition.CHART_COLUMN.EWMAMEAN
                        || s.Title == Definition.CHART_COLUMN.EWMARANGE
                        || s.Title == Definition.CHART_COLUMN.EWMASTDDEV
                        || s.Title == Definition.CHART_COLUMN.RAW
                        || s.Title == Definition.CHART_COLUMN.MA
                        || s.Title == Definition.CHART_COLUMN.MSD
                        || s.Title == Definition.CHART_COLUMN.MR
                        || s.Title == Definition.CHART_COLUMN.MEAN)
                {
                    ((Line)s).Active = true;
                    ((Line)s).LinePen.Width = 1;
                    ((Line)s).LinePen.Color = Color.Gray;
                }
                else if (s.Title == Definition.CHART_COLUMN.TARGET)
                {
                    ((Line)s).LinePen.Width = 1;
                    ((Line)s).Pointer.Visible = false;
                    ((Line)s).Active = false;

                }
                else if (s.Title == Definition.CHART_COLUMN.AVG)
                {
                    ((Line)s).LinePen.Width = 1;
                    //((Line)s).Pointer.Style = PointerStyles.Circle;
                    ((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.AVG);
                }
                else if (s.Title == Definition.CHART_COLUMN.MIN)
                {

                    ((Line)s).LinePen.Visible = false;
                    ((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.MIN_MAX);
                    //((Line)s).Pointer.Style = PointerStyles.Triangle;
                    ((Line)s).Pointer.HorizSize = 2;
                    ((Line)s).Pointer.VertSize = 2;
                }
                else if (s.Title == Definition.CHART_COLUMN.MAX)
                {

                    ((Line)s).LinePen.Visible = false;
                    ((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.MIN_MAX);
                    //((Line)s).Pointer.Style = PointerStyles.DownTriangle;
                    ((Line)s).Pointer.HorizSize = 2;
                    ((Line)s).Pointer.VertSize = 2;
                }
                else
                {
                    ((Line)s).LinePen.Visible = false;
                    //((Line)s).Pointer.Style = PointerStyles.Triangle;
                    ((Line)s).Active = true;
                    for (int j = 0; j < lstRawColumn.Count; j++)
                    {
                        if (lstRawColumn[j].Contains(s.Title))
                        {
                            ((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.POINT);
                            //((Line)s).Pointer.Style = PointerStyles.Diamond;
                            break;
                        }
                    }
                }
            }

            this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
            this.SPCChart.Chart.Axes.Bottom.Labels.ValueFormat = this.AxisDateTimeFormat;
            this.SPCChart.ChangeXLabelColumnDefault(Definition.CHART_COLUMN.TIME2);
            this.SPCChart.Chart.Axes.Bottom.Increment = Steema.TeeChart.Utils.DateTimeStep[(int)Steema.TeeChart.DateTimeSteps.OneDay];
            this.SPCChart.Refresh();
        }


        //public void ChangeXLabelColumnDefault(string labelColumn)
        //{
        //    if (_slLinkSeriesToInfo == null || _slLinkSeriesToInfo.Count <= 0) return;
        //    foreach (SeriesInfo si in _slLinkSeriesToInfo.Values)
        //    {
        //        si.XLabelColumn = labelColumn;
        //    }
        //    this.Chart.Refresh();
        //}


        private string GetSeriesMultiLanguage(string _seriesName)
        {
            return this._mlthandler.GetVariable(_seriesName);
        }

        public virtual void ChangeSeriesStyle(string pTitle, bool bActive)
        {

            Series s = null;

            if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.AVG))
            {
                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.AVG);
                if (s != null) ((Line)s).Active = bActive;

            }
            else if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.CONTROL_LIMIT))
            {
                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.UCL);
                if (s != null) ((Line)s).Active = bActive;

                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.LCL);
                if (s != null) ((Line)s).Active = bActive;

            }
            else if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.MIN_MAX))
            {
                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.MIN);
                if (s != null) ((Line)s).Active = bActive;
                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.MAX);
                if (s != null) ((Line)s).Active = bActive;

                //s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.MIN);
                //if (s != null)
                //    ((Steema.TeeChart.Styles.Error)s).Active = bActive;
                //s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.MAX);
                //if (s != null)
                //    ((Steema.TeeChart.Styles.Error)s).Active = bActive;       

            }
            else if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.POINT))
            {
                for (int j = 0; j < lstRawColumn.Count; j++)
                {
                    s = this.SPCChart.Chart.Series.WithTitle(lstRawColumn[j].ToString().Split('^')[0]);
                    if (s == null) continue;
                    ((Line)s).Active = bActive;
                }
            }
            else if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.SPEC))
            {
                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.USL);
                if (s != null) ((Line)s).Active = bActive;

                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.LSL);
                if (s != null) ((Line)s).Active = bActive;
            }
        }


        protected virtual void MakeTipInfo()
        {
            //Steema.TeeChart.Tools.MarksTip mkTip = new Steema.TeeChart.Tools.MarksTip();
            //mkTip.Style = MarksStyles.Value;              
            //this.SPCChart.Chart.Tools.Add(mkTip);                        
        }

        public virtual string[] UsedParameters(Steema.TeeChart.Styles.Series s)
        {
            return null;
        }

        public virtual void DrawCrossLine_WithParametersSEQIndex(string seqIndex)
        {
            return;
        }

        public virtual void ClearCrossLine()
        {
            for (int i = this.SPCChart.Chart.Tools.Count - 1; i >= 0; i--)
            {
                Steema.TeeChart.Tools.ColorLine colorLine = this.SPCChart.Chart.Tools[i] as Steema.TeeChart.Tools.ColorLine;

                if (colorLine != null && colorLine.Pen.Color == COLOR_SERIES_SYNC)
                {
                    this.SPCChart.Chart.Tools.Remove(colorLine);
                }
            }
        }


        #endregion




        #region ButtonList & TitlePanel
        void bButtonList1_ButtonClick(string name)
        {
            if (this.SPCChart.Chart.Series.Count < 1)
                return;

            try
            {
                if (name.ToUpper() == Definition.BUTTON_KEY_CHART_ZOOM_IN)
                {
                    this.Click_ZOOM_IN_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_CHANGE_X_AXIS)
                {
                    this.Click_CHANGE_X_AXIS_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_TOOL_TIP_COMMAND)
                {
                    this.Click_TOOL_TIP_COMMAND_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_SNAP_SHOT)
                {
                    this.Click_SNAP_SHOT_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_LEGEND_VISIBLITY)
                {
                    this.Click_LEGEND_VISIBLITY_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_LEGEND_ALL_CHECK)
                {
                    this.Click_LEGEND_ALL_CHECK_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_SERIES_SHIFT_HORI)
                {
                    this.Click_SERIES_SHIFT_HORI_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_SERIES_SHIFT_VERT)
                {
                    this.Click_SERIES_SHIFT_VERT_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_SAME_START_POINT)
                {
                    this.Click_SAME_START_POINT_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_EXCEL_EXPORT)
                {
                    this.Click_EXCEL_EXPORT_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_DELETE_BLANK)
                {
                    this.Click_DELETE_BLANK_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_POINT_MARKING)
                {
                    this.Click_POINT_MARKING_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_VARIATION)
                {
                    this.Click_VARIATION_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_REALTIME)
                {
                    this.Click_REALTIME_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_MULTI_TOOL_TIP)
                {
                    this.Click_MULTI_TOOL_TIP_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_COPY_CHART)
                {
                    this.Click_COPY_CHART_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_CHANGE_SERIES_STYLE)
                {
                    this.Click_CHANGE_SERIES_STYLE_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_CHANGE_LEGEND_LOCATION)
                {
                    this.Click_CHANGE_LEGEND_LOCATION_Button();
                }
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
            finally
            {

            }
        }

        #endregion



        #region Button Function
        private void Click_ZOOM_IN_Button()
        {
            this.SPCChart.ClickAction = BTeeChart.ChartClickAction.DEFAULT;

            ZoomPopup Zoom = new ZoomPopup();
            //SPC-805
            Zoom.SPC_CHART_TYPE = SPC_CHART_TYPE.BASERAWCHART;
            Zoom.BaseRawChart = this;
            Zoom.ShowDialog();
        }

        private void Click_CHANGE_X_AXIS_Button()
        {

            DataTable dt = this.dtDataSource;
            //Additional Problem??
            if (dt == null || dt.Rows.Count == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_DRAW_CHART", null, null);
                return;
            }

            ChangeXAxisPopup pop = new ChangeXAxisPopup();
            pop.ds = dt.DataSet;
            pop.BaseRawChart = this;
            pop.SPC_CHART_TYPE = SPC_CHART_TYPE.BASECHART;
            pop.ShowDialog();
            DialogResult dResult = pop.ButtonResult;

            if (dResult.ToString().Equals("OK"))
            {
                string sColumn = pop.getXAxisColumn();
                if (sColumn == null)
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_SELECT_TYPE", null, null);
                    return;
                }
                if (sColumn.Equals("VALUE"))
                {
                    this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.VALUE;
                }
                else if (sColumn.Equals("TIME"))
                {
                    this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.TIME_ELAPSE;
                }
                else
                {
                    for (int i = 0; i < SPCChart.Chart.Series.Count; i++)
                    {
                        if (this.SPCChart.Chart.Series[i].GetType().Name.Equals("Histogram"))
                        {
                            this.SPCChart.Chart.Series[i].Labels.Clear();
                        }
                    }
                    this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
                    this.SPCChart.ChangeXLabelColumnDefault(sColumn);
                }
            }
            else
            {
                return;
            }
        }

        private void Click_TOOL_TIP_COMMAND_Button()
        {
            this.SPCChart.AddMenuOfSeries(BTeeChart.ChartMenuItem.ADD_COMMENT_TO_TOOLTIP, "Add Comment");
        }

        private void Click_SNAP_SHOT_Button()
        {
            this.SPCChart.ClickAction = BTeeChart.ChartClickAction.SNAPSHOT;
        }

        private void Click_LEGEND_VISIBLITY_Button()
        {
            this.SPCChart.LegendVisible = !this.SPCChart.LegendVisible;
        }

        private void Click_LEGEND_ALL_CHECK_Button()
        {
            this._bSeriesAllCheck = !this._bSeriesAllCheck;
            this.SPCChart.SetSeriesAllCheck(this._bSeriesAllCheck);
        }

        public void Click_SERIES_SHIFT_HORI_Button()
        {
            this.SPCChart.ClickAction = BTeeChart.ChartClickAction.SERIES_SHIFT_HORI;
        }

        public void Click_SERIES_SHIFT_VERT_Button()
        {
            this.SPCChart.ClickAction = BTeeChart.ChartClickAction.SERIES_SHIFT_VERT;
        }

        public void Click_SAME_START_POINT_Button()
        {
            this.SPCChart.ShiftEqualStartSeries(BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.ChartAction.ShiftChartClickAction.ShiftDirectionAxis.BOTH);
        }


        public void Click_INITAILIZE_Button()
        {
            if (this.dtDataSource != null)
            {
                this.ClearChart();
                this.DrawSPCChart();
            }
        }


        public void Click_CLEAR_SHIFT_Button()
        {
            this.SPCChart.ClearShift();
        }


        private void Click_EXCEL_EXPORT_Button()
        {
            try
            {
                DataTable dt = this.dtDataSource.Copy();
                if (dt.Columns.Contains(Definition.CHART_COLUMN.TIME2))
                {
                    dt.Columns.Remove(Definition.CHART_COLUMN.TIME2);
                }

                this.bSpread1.ActiveSheet.RowCount = 0;
                this.bSpread1.ActiveSheet.ColumnCount = 0;
                this.bSpread1.ClearHead();
                this.bSpread1.AddHeadComplete();
                this.bSpread1.UseSpreadEdit = false;
                this.bSpread1.AutoGenerateColumns = true;
                this.bSpread1.DataSource = dt;
                this.bSpread1_Sheet1.SheetName = this.SPCChartTitlePanel.Name;
                this.bSpread1.Export(false);
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.ToString());
                EESUtil.DebugLog("BTeeChart", "ExportDataForExcel", ex);
            }
        }


        private void Click_DELETE_BLANK_Button()
        {
            this._bRegular = !this._bRegular;
            this.SPCChart.SetXAxisRegularInterval(this._bRegular);
        }

        private void Click_POINT_MARKING_Button()
        {
        }

        private void Click_VARIATION_Button()
        {
            this.SPCChart.ClickAction = BTeeChart.ChartClickAction.DELTA;
        }

        private void Click_REALTIME_Button()
        {
            int interval = 1000;
            int period = 60;
            int saveCount = 50;
            this.SPCChart.RealTimeStart(interval, period, saveCount);
        }

        private void Click_MULTI_TOOL_TIP_Button()
        {
            this.SPCChart.IsMultiToolTip = !this.SPCChart.IsMultiToolTip;
        }

        private void Click_COPY_CHART_Button()
        {
            this.SPCChart.ChartCopyToClipboard();
        }

        private void Click_CHANGE_SERIES_STYLE_Button()
        {
            if (this.SPCChart.Chart.Series != null)
            {
                int i = 0;
                int j = 0;

                switch (i)
                {
                    case 0:
                        this.SPCChart.ChangeAllSeriesStyle(typeof(Steema.TeeChart.Styles.Line));
                        j++;
                        i = j % 2;
                        break;
                    case 1:
                        this.SPCChart.ChangeAllSeriesStyle(typeof(Steema.TeeChart.Styles.Points));
                        j++;
                        i = j % 2;
                        break;

                }
            }
        }

        private void Click_CHANGE_LEGEND_LOCATION_Button()
        {
            if (this._legendLocation == Definition.Right)
            {
                this.SPCChart.LegendPosition = Steema.TeeChart.LegendAlignments.Bottom;
                this._legendLocation = Definition.Bottom;
            }
            else if (this._legendLocation == Definition.Bottom)
            {
                this.SPCChart.LegendPosition = Steema.TeeChart.LegendAlignments.Left;
                this._legendLocation = Definition.Left;
            }
            else if (this._legendLocation == Definition.Left)
            {
                this.SPCChart.LegendPosition = Steema.TeeChart.LegendAlignments.Top;
                this._legendLocation = Definition.Top;
            }
            else
            {
                this.SPCChart.LegendPosition = Steema.TeeChart.LegendAlignments.Right;
                this._legendLocation = Definition.Right;
            }
        }
        #endregion


        #region Chart Event


        void Chart_ClickLegend(object sender, MouseEventArgs e)
        {

        }




        #endregion


        #region User Defined Method.



        /// <summary>
        /// 선택한 시리즈의 축을 Right로 변경
        /// </summary>
        /// <param name="SeriesName"></param>
        public void Axes_Right(string SeriesName)
        {
            try
            {
                int Count = 0;
                Count = this.SPCChart.Chart.Series.Count;

                for (int i = 0; i < Count; i++)
                {
                    if (SeriesName == this.SPCChart.Chart.Series[i].Title)
                    {
                        this.SPCChart.Chart.Series[i].VertAxis = Steema.TeeChart.Styles.VerticalAxis.Right;
                        this.SPCChart.ResetAllAxis();
                        this.SPCChart.Chart.Axes.Right.Visible = true;

                    }
                }
            }
            catch (Exception exec)
            {
                System.Console.WriteLine("Axes_Right Exception");
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, exec);
            }
        }

        /// <summary>
        /// 선택한 시리즈의 축을 Left로 변경
        /// </summary>
        /// <param name="SeriesName"></param>
        public void Axes_Left(string SeriesName)
        {
            try
            {
                int Count = 0;
                Count = this.SPCChart.Chart.Series.Count;

                for (int i = 0; i < Count; i++)
                {
                    if (SeriesName == this.SPCChart.Chart.Series[i].Title)
                    {
                        this.SPCChart.Chart.Series[i].VertAxis = Steema.TeeChart.Styles.VerticalAxis.Left;
                        this.SPCChart.ResetAllAxis();
                    }
                }
            }
            catch (Exception exec)
            {
                System.Console.WriteLine("Axes_Left Exception");
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, exec);
            }
        }


        public void Complete_Tool_Tip()
        {
            this.SPCChart.DefaultToolTipInfo = this._stt;
        }

        public void Add_Tool_Tip(string label, string[] valueColumnNames)
        {
            this._stt.Add(new SeriesToolTipOneRowInfo(label, valueColumnNames));
        }
        public void Add_Tool_Tip(string label, string valueFormat, string[] valueColumnNames)
        {
            this._stt.Add(new SeriesToolTipOneRowInfo(label, valueFormat, valueColumnNames));
        }
        public void Add_Tool_Tip(string label, string valueFormat, string[] valueColumnNames, string spliter)
        {
            this._stt.Add(new SeriesToolTipOneRowInfo(label, valueFormat, valueColumnNames, spliter));
        }

        protected void DrawCrossLine(double xValue, double yValue)
        {
            this.SPCChart.AddColorLineOfHori(yValue, COLOR_SERIES_SYNC);
            this.SPCChart.AddColorLineOfVert(xValue, COLOR_SERIES_SYNC);
        }

        public void DrawChart(SeriesInfo Series)
        {
            this.SPCChart.AddSeries(Series);
        }

        private bool isPointerVisible()
        {
            if (_dataManager.RawDataTable.Rows.Count == 1)
                return true;
            else
                return false;
        }

        private bool GetSeriesVisibleType(string _key)
        {
            bool bRtn = true;
            _key = this._mlthandler.GetVariable(_key);
            if (llstChartSeriesVisibleType == null) return bRtn;
            for (int j = 0; j < llstChartSeriesVisibleType.Count; j++)
            {
                string strKey = llstChartSeriesVisibleType.GetKey(j).ToString();
                if (_key.Equals(strKey))
                {
                    bRtn = (bool)llstChartSeriesVisibleType.GetValue(j);
                    break;
                }
            }

            return bRtn;
        }
        #endregion

        private void PointMarking(bool use)
        {
            if(use)
            {
                if (_ShapeSeries != null && this.bTeeChart1.Chart.Series.IndexOf(_ShapeSeries) > -1)
                    this.bTeeChart1.Chart.Series.Remove(_ShapeSeries);

                _ShapeSeries = new Steema.TeeChart.Styles.Shape();
                this.bTeeChart1.Chart.Series.Add(_ShapeSeries);
                _ShapeSeries.Active = false;
                _ShapeSeries.ShowInLegend = false;
                _ShapeSeries.Pen.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                _ShapeSeries.Style = Steema.TeeChart.Styles.ShapeStyles.Rectangle;
                _ShapeSeries.XYStyle = Steema.TeeChart.Styles.ShapeXYStyles.Axis;
                _ShapeSeries.Color = Color.FromArgb(125, Color.Blue);
                _ShapeSeries.Pen.Color = Color.FromArgb(200, Color.Blue);

                this.bTeeChart1.ClickAction = BTeeChart.ChartClickAction.SELECTED_SERIESPOINT;

                this.pnlParent.Refresh();
            }
            else
            {
                this.bTeeChart1.ClickAction = BTeeChart.ChartClickAction.DEFAULT;
                this._isPointSelected = false;

                List<Steema.TeeChart.Styles.Series> lSeries = this.bTeeChart1.GetAllSeries();

                int seriesCount = lSeries.Count;
                for (int index1 = seriesCount - 1; index1 >= 0; index1--)
                {
                    if (lSeries[index1] is Steema.TeeChart.Styles.Shape)
                    {
                        this.bTeeChart1.RemoveSeries(lSeries[index1], false);
                    }
                }

                this.bTeeChart1.Chart.Axes.Left.Automatic = true;
                this.bTeeChart1.Chart.Axes.Bottom.Automatic = true;
            }
        }

        private void bTeeChart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.bckbPointMarking.Checked && e.Button.Equals(MouseButtons.Left))
            {
                this._endPositionX = e.X;
                this._endPositionY = e.Y;

                if (this._isPointSelected)
                {
                    if ((this._startPositionX == this._endPositionX) && (this._startPositionY == this._endPositionY))
                    {
                        this._ShapeSeries.Active = false;
                        _isPointSelected = false;
                        return;
                    }

                    this._ShapeSeries.Active = true;
                    this._ShapeSeries.X0 = this.bTeeChart1.Chart.Axes.Bottom.CalcPosPoint(this._startPositionX);
                    this._ShapeSeries.Y0 = this.bTeeChart1.Chart.Axes.Left.CalcPosPoint(this._startPositionY);
                    this._ShapeSeries.X1 = this.bTeeChart1.Chart.Axes.Bottom.CalcPosPoint(this._endPositionX);
                    this._ShapeSeries.Y1 = this.bTeeChart1.Chart.Axes.Left.CalcPosPoint(this._endPositionY);

                    this._isPointSelected = false;
                }
            }
        }

        private void bTeeChart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.bckbPointMarking.Checked)
                {
                    this.bTeeChart1.Chart.Axes.Left.Automatic = false;
                    this.bTeeChart1.Chart.Axes.Bottom.Automatic = false;

                    this._startPositionX = e.X;
                    this._startPositionY = e.Y;

                    if (!_isPointSelected)
                    {
                        //_ShapeSeries = new Shape();
                        //bTeeChart1.Chart.Series.Add(_ShapeSeries);
                        //_ShapeSeries.Active = false;
                        //_ShapeSeries.ShowInLegend = false;
                        //_ShapeSeries.Pen.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                        //_ShapeSeries.Style = ShapeStyles.Rectangle;
                        //_ShapeSeries.XYStyle = ShapeXYStyles.Axis;
                        //_ShapeSeries.Color = Color.FromArgb(125, Color.Blue);
                        //_ShapeSeries.Pen.Color = Color.FromArgb(200, Color.Blue);
                    }

                    this._isPointSelected = true;
                    this.bTeeChart1.ClickAction = BTeeChart.ChartClickAction.SELECTED_SERIESPOINT;
                }
                else
                {
                    this._isPointSelected = false;
                }
            }
        }

        private void bTeeChart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.bckbPointMarking.Checked)
            {
                this._endPositionX = e.X;
                this._endPositionY = e.Y;

                if (this._isPointSelected)
                {
                    if (_endPositionX - _startPositionX > 0 && _endPositionY - _startPositionY > 0)
                    {
                        this._ShapeSeries.Active = true;
                        this._ShapeSeries.X0 = this.bTeeChart1.Chart.Axes.Bottom.CalcPosPoint(this._startPositionX);
                        this._ShapeSeries.Y0 = this.bTeeChart1.Chart.Axes.Left.CalcPosPoint(this._startPositionY);
                        this._ShapeSeries.X1 = this.bTeeChart1.Chart.Axes.Bottom.CalcPosPoint(this._endPositionX);
                        this._ShapeSeries.Y1 = this.bTeeChart1.Chart.Axes.Left.CalcPosPoint(this._endPositionY);
                    }
                }
            }
        }

        public Shape GetSelectedShape()
        {
            if (!this.bckbPointMarking.Checked)
            {
                throw new NullReferenceException();
            }

            return this._ShapeSeries;
        }

        private void bckbPointMarking_CheckedChanged(object sender, EventArgs e)
        {
            if (this._dtDataSource != null && this.bckbPointMarking.Checked)
            {
                if (this._dtDataSource.Rows.Count < 2)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SINGLE_DATA_CANT_MARKING", null, null);
                    this.bckbPointMarking.Checked = false;
                    return;
                }
            }

            PointMarking(((BCheckBox) sender).Checked);
        }
    }
}
