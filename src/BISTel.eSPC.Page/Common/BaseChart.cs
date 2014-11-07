using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Page.Report;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
//modified by enkim 2012.05.21 SPC-851
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles;
//modified end SPC-851

using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl;

namespace BISTel.eSPC.Page.Common
{
    public partial class BaseChart : BasePageUCtrl
    {
        public MultiLanguageHandler _mlthandler;
        Color COLOR_SERIES_SYNC = Color.Gold;
        SourceDataManager _dataManager = null;
        Initialization _Initialization;
        DataTable _dtDataSource = null;
        DataTable _dtDataSourceToolTip = null;
        List<string> _lstRawColumn = null;
        LinkedList _llstChartSeriesVisibleType = null;
        ChartUtility _chartUtil;
        SortedList _slButtonList = new SortedList();
        SeriesToolTip _stt = new SeriesToolTip();
        LinkedList _llstGroup = new LinkedList();

        DataSet _dsChangeXAxis = null;
        string _legendLocation = Definition.Right;
        string _AxisDateTimeFormat = Definition.DATETIME_FORMAT_yyMMdd;
        string _xLabel = "INDEX";
        string _Group = null;

        bool _bRegular = false;
        bool _bSeriesAllCheck = true;
        bool _IsVisibleLegendScrollBar = false;
        bool _IsVisibleShadow = false;
        bool _IsVisibleZone = false;
        

        private DateTime _StartDateTime = DateTime.Now;
        private DateTime _EndDateTime = DateTime.Now;


        System.Windows.Forms.Panel pnlParent = null;
        Steema.TeeChart.Tools.MarksTip mkTip = null;

        string mParamTypeCd = string.Empty;

        bool isWaferColumn = false;

        private List<Series> groupSeries = new List<Series>();

        GrouperAndFilter _grouperAndFilter = new GrouperAndFilter();

        public LinkedList _llstEQPModule = new LinkedList();

        LinkedList _llstChartSeriesColor = null;

        public LinkedList LlstChartSeriesColor
        {
            get { return _llstChartSeriesColor; }
            set { _llstChartSeriesColor = value; }
        }

        public BaseChart()
        {
            InitializeComponent();
            InitializeVariable();
            this.Disposed += new EventHandler(BaseChart_Disposed);
        }

        void BaseChart_Disposed(object sender, EventArgs e)
        {
            this.SPCChart.ClearChart();
            this.SPCChart.Dispose();

            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref _dtDataSource);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref _dtDataSourceToolTip);
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

            this.bchkModuleID.IsIncludeAll = true;
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
                if (this.Name != Definition.CHART_TYPE.XBAR)
                {
                    for (int i = 0; i < this.bButtonList1.Items.Count; i++)
                    {
                        if (this.bButtonList1.Items[i].Name.ToUpper() == "SPC_ZONE_DISPLAY")
                        {
                            this.bButtonList1.Items[i].Visible = false;
                        }
                    }
                }


            }
        }

        #region PageLoad & Initialize.
        public virtual void InitializeChart()
        {

            //if (mkTip == null)
            //{
            //    mkTip = new Steema.TeeChart.Tools.MarksTip();
            //    mkTip.Style = MarksStyles.Value;              
            //}

            //if (this.SPCChart.Chart.Tools.IndexOf(mkTip) < 0)
            //{
            //    this.SPCChart.Chart.Tools.Add(mkTip);
            //}

            this.bTitlePanel1.MaxResizable = true;
            this.bButtonList1.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(bButtonList1_ButtonClick);
            this.bTitlePanel1.MouseDown += new MouseEventHandler(bTitlePanel1_MouseDown);
            this.bTitlePanel1.MouseMove += new MouseEventHandler(bTitlePanel1_MouseMove);
            this.bTitlePanel1.MouseUp += new MouseEventHandler(bTitlePanel1_MouseUp);
            this.bTitlePanel1.MouseDoubleClick += new MouseEventHandler(bTitlePanel1_MouseDoubleClick);
            this.bTitlePanel1.ClosedPanel += new EventHandler(bTitlePanel1_ClosedPanel);

            this.SPCChartTitlePanel.MaxResizable = true;
            this.SPCChart.Chart.Legend.CheckBoxes = true;
            this.SPCChart.IsVisibleLegendScrollBar = this.IsVisibleLegendScrollBar;
            this.SPCChart.IsVisibleShadow = this.IsVisibleShadow;
            this.SPCChart.AxisDateTimeFormat = this.AxisDateTimeFormat;
            this.SPCChart.IsUseAutomaticAxisOfTeeChart = true;
            this.SPCChart.IsAutoValueFormatLeftAxis = true;
            this.SPCChart.IsExponentialFormatLeftAxis = this._Initialization.GetExponentialAxis(Definition.PAGE_KEY_SPC_CONTROL_CHART_UC);
            this.SPCChart.IsUseChartEditor = true;
            this.SPCChart.LegendPosition = Steema.TeeChart.LegendAlignments.Right;
            this.SPCChart.Chart.Axes.DrawBehind = true;
            this.SPCChart.Chart.Axes.Left.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Right.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Top.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Bottom.Grid.Visible = false;

            this.SPCChart.SetXAxisRegularInterval(true);
            this.SPCChart.Chart.ClickLegend += new MouseEventHandler(Chart_ClickLegend);

            this.SPCChart.PointCrossLine.IsDrawPointCrossLine = true;
            this.SPCChart.PointCrossLine.IsSnapPointCrossLine = false;
            this.SPCChart.PointCrossLine.Color = Color.Purple;

        }

        private void RefreshChart()
        {
            if (!this.SPCChartTitlePanel.MaxResizable)
            {
                this.SPCChartTitlePanel.MaxResizable = true;
            }
            this.SPCChart.Chart.Legend.CheckBoxes = true;
            this.SPCChart.IsVisibleLegendScrollBar = this.IsVisibleLegendScrollBar;
            this.SPCChart.IsVisibleShadow = this.IsVisibleShadow;
            this.SPCChart.AxisDateTimeFormat = this.AxisDateTimeFormat;
            this.SPCChart.IsUseAutomaticAxisOfTeeChart = true;
            this.SPCChart.IsAutoValueFormatLeftAxis = true;
            this.SPCChart.IsExponentialFormatLeftAxis = this._Initialization.GetExponentialAxis(Definition.PAGE_KEY_SPC_CONTROL_CHART_UC);
            this.SPCChart.IsUseChartEditor = true;
            this.SPCChart.LegendPosition = Steema.TeeChart.LegendAlignments.Right;
            this.SPCChart.Chart.Axes.DrawBehind = true;
            this.SPCChart.Chart.Axes.Left.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Right.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Top.Grid.Visible = false;
            this.SPCChart.Chart.Axes.Bottom.Grid.Visible = false;
            this.SPCChart.SetXAxisRegularInterval(true);

            this.SPCChart.PointCrossLine.IsDrawPointCrossLine = true;
            this.SPCChart.PointCrossLine.IsSnapPointCrossLine = false;
            this.SPCChart.PointCrossLine.Color = Color.Purple;
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

        public DataTable dtDataSourceToolTip
        {

            get
            {
                return _dtDataSourceToolTip;
            }
            set
            {
                this._dtDataSourceToolTip = value;
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

        //public bool BtnVisible
        //{
        //    get { return this.bbtnTimeDisplay.Visible; }
        //    set { this.bbtnTimeDisplay.Visible = value; }
        //}

        public bool GroupVisible
        {
            get { return this.bapnlGroup.Visible; }
            set
            {
                this.bapnlGroup.Visible = value;
                if (this.bapnlGroup.Visible)
                {
                    this.panel5.Visible = false;
                }
                else
                {
                    this.panel5.Visible = true;
                }
            }
        }

        public bool FilterVisible
        {
            get { return this.bapnlEQP.Visible; }
            set
            {
                this.bapnlEQP.Visible = value;
            }
        }

        public DataSet DSGROUP
        {
            set
            {
                ArrayList arrGroup = new ArrayList();
                for (int i = 0; i < value.Tables[0].Rows.Count; i++)
                {
                    string strConKey = value.Tables[0].Rows[i][COLUMN.CONTEXT_KEY].ToString();
                    string strConVal = value.Tables[0].Rows[i][COLUMN.CONTEXT_VALUE].ToString();
                    string[] strValues = strConVal.Split(';');
                    for (int j = 0; j < strValues.Length; j++)
                    {
                        if (strValues[j].Trim().Length > 0)
                        {
                            arrGroup.Add(strValues[j].Trim() + " [" + strConKey.Trim() + "]");
                        }
                    }
                }
                if (arrGroup.Count > 0)
                {
                    string[] strArrGroup = (string[])arrGroup.ToArray(typeof(string));
                    this.bCheckCombo.AddItems(strArrGroup);
                }
            }
        }

        public ArrayList ARREQPID
        {
            set
            {
                if (value.Count > 0)
                {
                    string[] strArrEQPID = (string[])value.ToArray(typeof(string));
                    this.bchkEQPID.AddItems(true, strArrEQPID);
                    this.bchkEQPID.TextChanged += new EventHandler(bchkEQPID_TextChanged);
                    this.bchkEQPID.Text = this.bchkEQPID.Text;
                }
            }
        }
        string[] strArrModuleID;

        void bchkEQPID_TextChanged(object sender, EventArgs e)
        {
            if (this.bchkEQPID.Text == string.Empty)
            {
                this.bchkModuleID.Items.Clear();
                this.bchkModuleID.ResetText();
            }
            else
            {
                this.bchkModuleID.Items.Clear();
                this.bchkModuleID.ResetText();
                string[] strArrEQPID = this.bchkEQPID.Text.Split(',');
                ArrayList arrModuleValue = new ArrayList();
                for (int i = 0; i < strArrEQPID.Length; i++)
                {
                    if (this._llstEQPModule.Contains(strArrEQPID[i]))
                    {
                        for (int j = 0; j < ((ArrayList)_llstEQPModule[strArrEQPID[i]]).Count; j++)
                        {
                            if (!arrModuleValue.Contains(((ArrayList)_llstEQPModule[strArrEQPID[i]])[j].ToString()))
                            {
                                arrModuleValue.Add(((ArrayList)_llstEQPModule[strArrEQPID[i]])[j].ToString());
                            }
                        }
                    }
                }
                if (arrModuleValue.Count > 0)
                {
                    strArrModuleID = (string[])arrModuleValue.ToArray(typeof(string));
                    this.bchkModuleID.AddItems(true, strArrModuleID);
                }
            }
        }

        public ArrayList ARRMODULEID
        {
            set
            {
                if (value.Count > 0)
                {
                    strArrModuleID = (string[])value.ToArray(typeof(string));
                    this.bchkModuleID.AddItems(true, strArrModuleID);
                }
            }
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

        public string ParamTypeCD
        {
            get { return this.mParamTypeCd; }
            set { this.mParamTypeCd = value; }
        }

        public bool IsWaferColumn
        {
            get { return this.isWaferColumn; }
            set { this.isWaferColumn = value; }
        }


        #endregion

        #region ::: Override Method.
        #endregion


        #region ::: Virtual Method.

        ///////////////////////////////////////////////////////////////////////////////////
        public virtual bool DrawSPCChart()
         {
            try
            {
                if (this.DataManager.RawDataTable.Rows.Count < 1)
                {
                    //MSGHandler.DisplayMessage(MSGType.Warning, MultiLanguageHandler.getInstance().GetMessage("VMS_INFO_065"));
                    return false;
                }

                if (this.mParamTypeCd == "MET")
                {
                    MakeDataSourceToDrawSPCChart();
                    DrawChartWithSeriesInfo();
                    if (this.Name == Definition.CHART_TYPE.RAW || this.Name == Definition.CHART_TYPE.XBAR)
                    {
                        DrawChartWithRawSeriesInfo();
                    }
                    ChangeSeriesStyle();
                    MakeTipInfo();
                    this._dtDataSourceToolTip = this._dataManager.RawDataTable.Copy();
                }
                else
                {
                    if (this.Name == Definition.CHART_TYPE.RAW)
                    {
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
                            if (this.DataManager.RawDataTable.Rows.Count > 0)
                            {
                                this._dtDataSource = this.DataManager.RawDataTable.Clone();
                                DataTable dtTemp = new DataTable();
                                dtTemp = this.DataManager.RawDataTable.Clone();
                                foreach (DataRow dr in DataManager.RawDataTable.Rows)
                                {
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
                                                            //modified by enkim 2012.05.21 SPC-851
                                                            if (dtTemp.Rows[j][k].ToString().Split(';')[j] == null || dtTemp.Rows[j][k].ToString().Split(';')[j] == "NaN")
                                                                dtTemp.Rows[j][k] = null;
                                                            else
                                                                dtTemp.Rows[j][k] = dtTemp.Rows[j][k].ToString().Split(';')[j];
                                                            //modified end 2012.05.21 SPC-851
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
                                                //if (dtTemp.Rows[j]["CASSETTE_SLOT"] != null && dtTemp.Rows[j]["CASSETTE_SLOT"].ToString().Length > 0 && dtTemp.Rows[j]["CASSETTE_SLOT"].ToString().Split(';').Length > 1)
                                                //{
                                                //    dtTemp.Rows[j]["CASSETTE_SLOT"] = dtTemp.Rows[j]["CASSETTE_SLOT"].ToString().Split(';')[j];
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
                                    else
                                    {
                                        if (dr["RAW"] != null && dr["RAW"].ToString().Length > 0)
                                        {
                                            dr[COLUMN.RAW_DTTS] = dr["TIME"];
                                            dtTemp.ImportRow(dr);
                                            this._dtDataSource.Merge(dtTemp);
                                        }
                                    }
                                }

                                if (this._dtDataSource.Rows.Count > 0)
                                {
                                    this._dtDataSource = DataUtil.DataTableImportRow(this._dtDataSource.Select(null, COLUMN.RAW_DTTS));
                                }

                                if (this._dtDataSource.Columns.Contains("TIME"))
                                {
                                    this.dtDataSource.Columns.Remove("TIME");
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
                                    //modified by enkim 2012.05.21 SPC-851
                                    //old-logic start
                                    //if (seriesName == "RAW" || seriesName == "RAW_UCL" || seriesName == "RAW_LCL" || seriesName == "RAW_LSL" || seriesName == "RAW_USL" || seriesName == "RAW_TARGET")
                                    //{
                                    //    si = new SeriesInfo(typeof(Line), seriesName.Replace("RAW_", ""), this._dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName.ToLower());
                                    //    switch (seriesName)
                                    //    {
                                    //        case "RAW_UCL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.UCL); break;
                                    //        case "RAW_LCL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LCL); break;
                                    //        case "RAW_LSL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LSL); break;
                                    //        case "RAW_USL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.USL); break;
                                    //        case "RAW_TARGET": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.TARGET); break;
                                    //        case "RAW": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.AVG); break;
                                    //    }
                                    //    this.SPCChart.AddSeries(si);
                                    //}
                                    //old-logic end

                                    if (seriesName == "RAW")
                                    {
                                        si = new SeriesInfo(typeof(ExtremeBFastLine), seriesName.Replace("RAW_", ""), this._dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName.ToLower());
                                        si.TreatNulls = TreatNullsStyle.DoNotPaint;
                                        switch (seriesName)
                                        {
                                            case "RAW": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.AVG); break;
                                        }
                                        this.SPCChart.AddSeries(si);
                                    }

                                    if (seriesName == "RAW_UCL" || seriesName == "RAW_LCL" || seriesName == "RAW_LSL" || seriesName == "RAW_USL" || seriesName == "RAW_TARGET")
                                    {
                                        si = new SeriesInfo(typeof(ExtremeBFastLine), seriesName.Replace("RAW_", ""), this._dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName.ToLower());
                                        si.TreatNulls = TreatNullsStyle.DoNotPaint;
                                        switch (seriesName)
                                        {
                                            case "RAW_UCL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.UCL); break;
                                            case "RAW_LCL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LCL); break;
                                            case "RAW_LSL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LSL); break;
                                            case "RAW_USL": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.USL); break;
                                            case "RAW_TARGET": si.SeriesColor = _chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.TARGET); break;
                                        }
                                        this.SPCChart.AddSeries(si);
                                    }
                                    //modified end SPC-851
                                }

                                //modified by enkim 2012.05.21 SPC-851
                                Series s = null;
                                for (int i = 0; i < this.SPCChart.Chart.Series.Count; i++)
                                {
                                    s = this.SPCChart.Chart.Series[i];

                                    //((Line)s).LinePen.Width = 2;
                                    //((Line)s).OutLine.Visible = false;
                                    //((Line)s).ColorEachLine = false;
                                    //((Line)s).Pointer.Visible = false;
                                    //((Line)s).Active = false;

                                    if (s.Title == "RAW")
                                    {
                                        ((ExtremeBFastLine)s).LinePen.Width = 1;
                                        ((ExtremeBFastLine)s).OutLine.Visible = false;
                                        ((ExtremeBFastLine)s).ColorEachLine = false;
                                        ((ExtremeBFastLine)s).Pointer.Visible = true;
                                        ((ExtremeBFastLine)s).Active = true;
                                        ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
                                        if (this._dtDataSource.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                                        {
                                            for (int j = 0; j < s.Labels.Count; j++)
                                            {
                                                string sOcapRawID = this._dtDataSource.Rows[j][Definition.CHART_COLUMN.OCAP_RAWID].ToString().Replace(";", "").TrimEnd();
                                                if (!string.IsNullOrEmpty(sOcapRawID))
                                                    s[j].Color = Color.Red;
                                            }
                                        }

                                        if (this._dtDataSource.Columns.Contains(Definition.CHART_COLUMN.RAW_OCAP_LIST))
                                        {
                                            for (int j = 0; j < s.Labels.Count; j++)
                                            {
                                                string sOcapRawID = this._dtDataSource.Rows[j][Definition.CHART_COLUMN.RAW_OCAP_LIST].ToString().Replace(";", "").Replace("^", "").TrimEnd();
                                                if (!string.IsNullOrEmpty(sOcapRawID))
                                                    s[j].Color = Color.Red;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ((ExtremeBFastLine)s).LinePen.Width = 2;
                                        ((ExtremeBFastLine)s).OutLine.Visible = false;
                                        ((ExtremeBFastLine)s).ColorEachLine = false;
                                        ((ExtremeBFastLine)s).Pointer.Visible = false;
                                        ((ExtremeBFastLine)s).Active = false;

                                        if (s.Title == "UCL" || s.Title == "LCL")
                                            ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.CONTROL_LIMIT);
                                        else if (s.Title == "USL" || s.Title == "LSL")
                                            ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.SPEC);
                                    }
                                }
                                //modified end SPC-851

                                this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
                                this.SPCChart.Chart.Axes.Bottom.Labels.ValueFormat = "yy-MM-dd";
                                this.SPCChart.ChangeXLabelColumnDefault("TIME2");
                                this.SPCChart.Chart.Axes.Bottom.Increment = Steema.TeeChart.Utils.DateTimeStep[(int)Steema.TeeChart.DateTimeSteps.OneDay];
                                this.SPCChart.Refresh();
                                this._dtDataSourceToolTip = this._dtDataSource.Copy();
                                this._dataManager.RawDataTable = this._dataManager.RawDataTableOriginal.Copy();
                                //ChangeSeriesStyle();
                            }
                        }
                        else
                        {
                            if (this.DataManager.RawDataTable.Rows.Count > 0)
                            {
                                this._dtDataSource = this.DataManager.RawDataTable.Copy();


                                this._dtDataSource.Columns.Add(Definition.CHART_COLUMN.TIME2, typeof(string));
                                this._dtDataSource.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, typeof(int));


                                for (int i = 0; i < this._dtDataSource.Rows.Count; i++)
                                {
                                    string sTime = this._dtDataSource.Rows[i]["TIME"].ToString();
                                    this._dtDataSource.Rows[i][Definition.CHART_COLUMN.TIME2] = sTime.Substring(0, 16);
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
                                        //modified by enkim 2012.05.25 SPC-851
                                        //si = new SeriesInfo(typeof(Line), seriesName.Replace("RAW_", ""), this._dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName.ToLower());
                                        si = new SeriesInfo(typeof(ExtremeBFastLine), seriesName.Replace("RAW_", ""), this._dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName.ToLower());
                                        //modified end SPC-851

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
                                    //modified by enkim 2012.05.25 SPC-851
                                    s = this.SPCChart.Chart.Series[i];

                                    //((Line)s).LinePen.Width = 2;
                                    //((Line)s).OutLine.Visible = false;
                                    //((Line)s).ColorEachLine = false;
                                    //((Line)s).Pointer.Visible = false;
                                    //((Line)s).Active = false;
                                    ((ExtremeBFastLine)s).LinePen.Width = 2;
                                    ((ExtremeBFastLine)s).OutLine.Visible = false;
                                    ((ExtremeBFastLine)s).ColorEachLine = false;
                                    ((ExtremeBFastLine)s).Pointer.Visible = false;
                                    ((ExtremeBFastLine)s).Active = false;

                                    if (s.Title == "RAW")
                                    {
                                        //((Line)s).LinePen.Width = 1;
                                        //((Line)s).Pointer.Visible = true;
                                        //((Line)s).Active = true;
                                        ((ExtremeBFastLine)s).LinePen.Width = 1;
                                        ((ExtremeBFastLine)s).Pointer.Visible = true;
                                        ((ExtremeBFastLine)s).Active = true;
                                        ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
                                        if (this._dtDataSource.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                                        {
                                            for (int j = 0; j < s.Labels.Count; j++)
                                            {
                                                string sOcapRawID = this._dtDataSource.Rows[j][Definition.CHART_COLUMN.OCAP_RAWID].ToString().Replace(";", "").TrimEnd();
                                                if (!string.IsNullOrEmpty(sOcapRawID))
                                                    s[j].Color = Color.Red;
                                            }
                                        }

                                        if (this._dtDataSource.Columns.Contains(Definition.CHART_COLUMN.RAW_OCAP_LIST))
                                        {
                                            for (int j = 0; j < s.Labels.Count; j++)
                                            {
                                                string sOcapRawID = this._dtDataSource.Rows[j][Definition.CHART_COLUMN.RAW_OCAP_LIST].ToString().Replace(";", "").Replace("^", "").TrimEnd();
                                                if (!string.IsNullOrEmpty(sOcapRawID))
                                                    s[j].Color = Color.Red;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (s.Title == "UCL" || s.Title == "LCL")
                                        {
                                            //((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.CONTROL_LIMIT);
                                            ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.CONTROL_LIMIT);
                                        }
                                        else if (s.Title == "USL" || s.Title == "LSL")
                                        {
                                            //((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.SPEC);
                                            ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.SPEC);
                                        }
                                    }
                                    //modified end SPC-851
                                }

                                this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
                                this.SPCChart.Chart.Axes.Bottom.Labels.ValueFormat = "yy-MM-dd";
                                this.SPCChart.ChangeXLabelColumnDefault("TIME2");
                                this.SPCChart.Chart.Axes.Bottom.Increment = Steema.TeeChart.Utils.DateTimeStep[(int)Steema.TeeChart.DateTimeSteps.OneDay];
                                this.SPCChart.Refresh();
                                this._dtDataSourceToolTip = this._dtDataSource.Copy();
                                this._dataManager.RawDataTable = this._dataManager.RawDataTableOriginal.Copy();
                            }
                        }
                    }
                    else
                    {
                        MakeDataSourceToDrawSPCChart();
                        DrawChartWithSeriesInfoForSumChart();
                        if (this.Name == Definition.CHART_TYPE.XBAR)
                        {
                            DrawChartWithRawSeriesInfoForSumChart();
                        }
                        ChangeSeriesStyleForSumChart();
                        MakeTipInfo();
                        this._dtDataSourceToolTip = this._dataManager.RawDataTable.Copy();
                    }
                }

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

            //2012-03-23 added by rachel -->
            //[SPC-659]
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref _dtDataSource);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref _dtDataSourceToolTip);
            //<--

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

        protected virtual void DrawChartWithSeriesInfoForSumChart()
        {
            return;
        }

        protected virtual void DrawChartWithRawSeriesInfoForSumChart()
        {
            return;
        }

        //modified by enkim 2012.05.25 SPC-851
        public virtual void ChangeSeriesStyle()
        {
            Series s = null;
            for (int i = 0; i < this.SPCChart.Chart.Series.Count; i++)
            {
                s = this.SPCChart.Chart.Series[i];

                ((ExtremeBFastLine)s).LinePen.Width = 2;
                ((ExtremeBFastLine)s).OutLine.Visible = false;
                ((ExtremeBFastLine)s).ColorEachLine = false;

                if (s.Title == Definition.CHART_COLUMN.UCL || s.Title == Definition.CHART_COLUMN.LCL)
                {
                    ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.CONTROL_LIMIT);
                    ((ExtremeBFastLine)s).Pointer.Visible = isPointerVisible();
                }
                else if (s.Title == Definition.CHART_COLUMN.LSL || s.Title == Definition.CHART_COLUMN.USL)
                {
                    ((ExtremeBFastLine)s).LinePen.Width = 2;
                    ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.SPEC);
                    ((ExtremeBFastLine)s).Pointer.Visible = isPointerVisible();
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
                    ((ExtremeBFastLine)s).Active = true;
                    ((ExtremeBFastLine)s).LinePen.Width = 1;
                    ((ExtremeBFastLine)s).LinePen.Color = Color.Gray;
                    ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
                    ValidateOCAP(s);
                }
                else if (s.Title == Definition.CHART_COLUMN.TARGET)
                {
                    ((ExtremeBFastLine)s).LinePen.Width = 1;
                    ((ExtremeBFastLine)s).Pointer.Visible = false;
                    ((ExtremeBFastLine)s).Active = false;

                }
                else if (s.Title == Definition.CHART_COLUMN.AVG)
                {
                    ((ExtremeBFastLine)s).LinePen.Width = 1;
                    //((Line)s).Pointer.Style = PointerStyles.Circle;
                    ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.AVG);
                    ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
                    ValidateOCAP(s);
                }
                else if (s.Title == Definition.CHART_COLUMN.MIN)
                {

                    ((ExtremeBFastLine)s).LinePen.Visible = false;
                    ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.MIN_MAX);
                    //((Line)s).Pointer.Style = PointerStyles.Triangle;
                    ((ExtremeBFastLine)s).Pointer.HorizSize = 2;
                    ((ExtremeBFastLine)s).Pointer.VertSize = 2;
                    ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
                }
                else if (s.Title == Definition.CHART_COLUMN.MAX)
                {

                    ((ExtremeBFastLine)s).LinePen.Visible = false;
                    ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.MIN_MAX);
                    //((Line)s).Pointer.Style = PointerStyles.DownTriangle;
                    ((ExtremeBFastLine)s).Pointer.HorizSize = 2;
                    ((ExtremeBFastLine)s).Pointer.VertSize = 2;
                    ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
                }
                else
                {
                    ((ExtremeBFastLine)s).LinePen.Visible = false;
                    //((Line)s).Pointer.Style = PointerStyles.Triangle;
                    ((ExtremeBFastLine)s).Active = true;
                    for (int j = 0; j < lstRawColumn.Count; j++)
                    {
                        if (lstRawColumn[j].Contains(s.Title))
                        {
                            ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.POINT);
                            ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
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
        //modified end SPC-851

        //modified by enkim 2012.05.21 SPC-851
        public virtual void ChangeSeriesStyleForSumChart()
        {
            Series s = null;
            for (int i = 0; i < this.SPCChart.Chart.Series.Count; i++)
            {
                s = this.SPCChart.Chart.Series[i];

                if (s.Title == Definition.CHART_COLUMN.UCL || s.Title == Definition.CHART_COLUMN.LCL)
                {
                    ((ExtremeBFastLine)s).LinePen.Width = 2;
                    ((ExtremeBFastLine)s).OutLine.Visible = false;
                    ((ExtremeBFastLine)s).ColorEachLine = false;
                    ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.CONTROL_LIMIT);
                    ((ExtremeBFastLine)s).Pointer.Visible = isPointerVisible();
                }
                else if (s.Title == Definition.CHART_COLUMN.LSL || s.Title == Definition.CHART_COLUMN.USL)
                {
                    ((ExtremeBFastLine)s).LinePen.Width = 2;
                    ((ExtremeBFastLine)s).OutLine.Visible = false;
                    ((ExtremeBFastLine)s).ColorEachLine = false;
                    ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.SPEC);
                    ((ExtremeBFastLine)s).Pointer.Visible = isPointerVisible();
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
                    ((ExtremeBFastLine)s).LinePen.Width = 1;
                    ((ExtremeBFastLine)s).OutLine.Visible = false;
                    ((ExtremeBFastLine)s).ColorEachLine = false;
                    ((ExtremeBFastLine)s).Active = true;
                    ((ExtremeBFastLine)s).LinePen.Color = Color.Gray;
                    ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
                    ValidateOCAP(s);
                }
                else if (s.Title == Definition.CHART_COLUMN.TARGET)
                {
                    ((ExtremeBFastLine)s).LinePen.Width = 1;
                    ((ExtremeBFastLine)s).OutLine.Visible = false;
                    ((ExtremeBFastLine)s).ColorEachLine = false;
                    ((ExtremeBFastLine)s).Pointer.Visible = false;
                    ((ExtremeBFastLine)s).Active = false;
                }
                else if (s.Title == Definition.CHART_COLUMN.AVG)
                {
                    ((ExtremeBFastLine)s).LinePen.Width = 1;
                    ((ExtremeBFastLine)s).OutLine.Visible = false;
                    ((ExtremeBFastLine)s).ColorEachLine = false;
                    ((ExtremeBFastLine)s).LinePen.Color = Color.Gray;
                    ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.AVG);
                    ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
                    ValidateOCAP(s);
                }
                else if (s.Title == Definition.CHART_COLUMN.MIN)
                {
                    ((ExtremeBFastLine)s).OutLine.Visible = false;
                    ((ExtremeBFastLine)s).ColorEachLine = false;
                    ((ExtremeBFastLine)s).LinePen.Visible = false;
                    ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.MIN_MAX);
                    ((ExtremeBFastLine)s).Pointer.HorizSize = 2;
                    ((ExtremeBFastLine)s).Pointer.VertSize = 2;
                    ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
                }
                else if (s.Title == Definition.CHART_COLUMN.MAX)
                {
                    ((ExtremeBFastLine)s).OutLine.Visible = false;
                    ((ExtremeBFastLine)s).ColorEachLine = false;
                    ((ExtremeBFastLine)s).LinePen.Visible = false;
                    ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.MIN_MAX);
                    ((ExtremeBFastLine)s).Pointer.HorizSize = 2;
                    ((ExtremeBFastLine)s).Pointer.VertSize = 2;
                    ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
                }
                else
                {
                    ((ExtremeBFastLine)s).OutLine.Visible = false;
                    ((ExtremeBFastLine)s).ColorEachLine = false;
                    ((ExtremeBFastLine)s).LinePen.Visible = false;
                    ((ExtremeBFastLine)s).Active = true;
                    for (int j = 0; j < lstRawColumn.Count; j++)
                    {
                        if (lstRawColumn[j].Contains(s.Title))
                        {
                            ((ExtremeBFastLine)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.POINT);
                            ((ExtremeBFastLine)s).Pointer.InflateMargins = true;
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
        //modified end SPC-851


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

        //modified by enkim 2012.05.21 SPC-851
        public virtual void ChangeSeriesStyle(string pTitle, bool bActive)
        {

            Series s = null;

            if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.VAL))
            {
                s = this.SPCChart.Chart.Series.WithTitle(this.NAME);
                if (s != null) s.Active = bActive;
                else
                {
                    switch (this.NAME)
                    {
                        case Definition.CHART_TYPE.EWMA_STDDEV:
                            s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.STDDEV);
                            break;
                        case Definition.CHART_TYPE.EWMA_MEAN:
                            s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.AVG);
                            break;
                        case Definition.CHART_TYPE.EWMA_RANGE:
                            s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.RANGE);
                            break;
                        default:
                            s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.AVG);
                            break;
                    }
                    //s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.AVG);
                    if (s != null) s.Active = bActive;
                }

            }
            else if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.CONTROL_LIMIT))
            {
                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.UCL);
                if (s != null) s.Active = bActive;

                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.LCL);
                if (s != null) s.Active = bActive;

            }
            else if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.MIN_MAX))
            {
                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.MIN);
                if (s != null) s.Active = bActive;
                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.MAX);
                if (s != null) s.Active = bActive;
            }
            else if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.POINT))
            {
                for (int j = 0; j < lstRawColumn.Count; j++)
                {
                    s = this.SPCChart.Chart.Series.WithTitle(lstRawColumn[j].ToString().Split('^')[0]);
                    if (s != null) s.Active = bActive;
                }
            }
            else if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.SPEC))
            {
                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.USL);
                if (s != null) s.Active = bActive;

                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.LSL);
                if (s != null) s.Active = bActive;
            }
        }
        //modified end SPC-851


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
                //this.bButtonList1.Focus();

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
                else if (name.ToUpper() == "SPC_ZONE_DISPLAY")
                {
                    this.ClickDisplayZone();
                }
                else if (name.ToUpper() == "SPC_NORMALDIST")
                {
                    this.ClickShowNormalDistribution();
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

        private void ClickShowNormalDistribution()
        {
            try
            {
                Series sSource = null;
                for (int i = 0; i < this.SPCChart.Chart.Series.Count; i++)
                {
                    if (this.SPCChart.Chart.Series[i].Title == Definition.CHART_COLUMN.RANGE
                    || this.SPCChart.Chart.Series[i].Title == Definition.CHART_COLUMN.STDDEV
                    || this.SPCChart.Chart.Series[i].Title == Definition.CHART_COLUMN.EWMAMEAN
                    || this.SPCChart.Chart.Series[i].Title == Definition.CHART_COLUMN.EWMARANGE
                    || this.SPCChart.Chart.Series[i].Title == Definition.CHART_COLUMN.EWMASTDDEV
                    || this.SPCChart.Chart.Series[i].Title == Definition.CHART_COLUMN.RAW
                    || this.SPCChart.Chart.Series[i].Title == Definition.CHART_COLUMN.MA
                    || this.SPCChart.Chart.Series[i].Title == Definition.CHART_COLUMN.MSD
                    || this.SPCChart.Chart.Series[i].Title == Definition.CHART_COLUMN.MR
                    || this.SPCChart.Chart.Series[i].Title == Definition.CHART_COLUMN.MEAN
                    || this.SPCChart.Chart.Series[i].Title == Definition.CHART_COLUMN.AVG)
                    {
                        sSource = this.SPCChart.Chart.Series[i];
                        break;
                    }
                }


                DataSet dsTemp = ((DataSet)sSource.DataSource).Copy();

                NormalDistributionPopup ndp = new NormalDistributionPopup();
                ndp.Title = "Data Distribution (" + this.Name + ")";
                if (dsTemp != null && dsTemp.Tables.Count > 0)
                {
                    if (dsTemp.Tables[0].Rows.Count == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_DISPLAY_DATA", null, null);
                        BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref dsTemp);
                        return;
                    }
                    dsTemp.Tables[0].TableName = this.Name;
                    
                }
                ndp.ndcUC.IsStdError = false;
                ndp.hstUC.IsStdError = false;

                ndp.ndcUC.BNormalDistributionChart.IsUseChartEditor = true;
                ndp.hstUC.BNormalHistogramChart.IsUseChartEditor = true;
                ndp.bpcUC.BBoxPlotChart.IsUseChartEditor = true;
                ndp.ndcUC.MakeNormalizeDistribution(dsTemp, null);
                if (ndp.ndcUC.IsStdError)
                {
                    return;
                }
                ndp.ndcUC.BNormalDistributionChart.Chart.Axes.Left.Visible = false;
                ndp.hstUC.IsNumberTypeOfYAxis = true;
                ndp.hstUC.MakeHistogram(dsTemp, null);
                if (ndp.hstUC.IsStdError)
                {
                    return;
                }
                ndp.bpcUC.MakeBoxPlot(dsTemp, null);

                ndp.ShowDialog();

                BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref dsTemp);
            }
            catch
            {
            }
            //this.SPCChart.too
        }

        private void ClickDisplayZone()
        {
            try
            {
                if (this.dtDataSourceToolTip.Rows.Count > 0 && !this._IsVisibleZone)
                {
                    Series sZone = null;
                    for (int i = 0; i < this.SPCChart.Chart.Series.Count; i++)
                    {
                        if (this.SPCChart.Chart.Series[i].Title == "AVG")
                        {
                            sZone = this.SPCChart.Chart.Series[i];
                            break;
                        }
                    }

                    DataSet dsTemp = (DataSet)sZone.DataSource;
                    sZone = null;
                    ArrayList arrData = new ArrayList();

                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsTemp.Tables[0].Rows.Count; i++)
                        {
                            if (dsTemp.Tables[0].Rows[i]["Y"].ToString().Length > 0)
                            {
                                double dOutPut = 0;
                                bool isParse = double.TryParse(dsTemp.Tables[0].Rows[i]["Y"].ToString(), out dOutPut);

                                if (isParse)
                                    arrData.Add(dOutPut);
                            }
                        }
                    }

                    double[] dsValueList = (double[])arrData.ToArray(typeof(double));

                    double avg = BISTel.PeakPerformance.Statistics.Algorithm.Stat.StStat.mean(dsValueList);
                    double std = BISTel.PeakPerformance.Statistics.Algorithm.Stat.StStat.std(dsValueList);

                    double dZoneAUpper = avg + (std * 3);
                    double dZoneBUpper = avg + (std * 2);
                    double dZoneCUpper = avg + (std * 1);
                    double dZoneALower = avg - (std * 3);
                    double dZoneBLower = avg - (std * 2);
                    double dZoneCLower = avg - (std * 1);

                    arrData = null;
                    dsValueList = null;
                    dsTemp = null;

                    ColorBand cbZoneA = new ColorBand();
                    ColorBand cbZoneB = new ColorBand();
                    ColorBand cbZoneC = new ColorBand();

                    this.SPCChart.Chart.Tools.Add(cbZoneA);
                    cbZoneA.Axis = this.SPCChart.Chart.Axes.Left;
                    cbZoneA.Brush.Color = Color.Yellow;
                    cbZoneA.Brush.Transparency = 80;
                    cbZoneA.End = dZoneAUpper;
                    cbZoneA.ResizeEnd = false;
                    cbZoneA.ResizeStart = false;
                    cbZoneA.Start = dZoneALower;
                    cbZoneA.DrawBehind = true;
                    cbZoneA.Active = true;


                    this.SPCChart.Chart.Tools.Add(cbZoneB);
                    cbZoneB.Axis = this.SPCChart.Chart.Axes.Left;
                    cbZoneB.Brush.Color = Color.Orange;
                    cbZoneB.Brush.Transparency = 80;
                    cbZoneB.End = dZoneBUpper;
                    cbZoneB.ResizeEnd = false;
                    cbZoneB.ResizeStart = false;
                    cbZoneB.Start = dZoneBLower;
                    cbZoneA.DrawBehind = true;
                    cbZoneB.Active = true;

                    this.SPCChart.Chart.Tools.Add(cbZoneC);
                    cbZoneC.Axis = this.SPCChart.Chart.Axes.Left;
                    cbZoneC.Brush.Color = Color.DarkOrange;
                    cbZoneC.Brush.Transparency = 80;
                    cbZoneC.End = dZoneCUpper;
                    cbZoneC.ResizeEnd = false;
                    cbZoneC.ResizeStart = false;
                    cbZoneC.Start = dZoneCLower;
                    cbZoneA.DrawBehind = true;
                    cbZoneC.Active = true;

                    this._IsVisibleZone = true;
                }
            }
            catch
            {
            }
            //this.SPCChart.too
        }

        void bTitlePanel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.OnMouseDoubleClick(e);
        }

        void bTitlePanel1_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }

        void bTitlePanel1_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMove(e);
        }

        void bTitlePanel1_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }

        void bTitlePanel1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }


        void bTitlePanel1_ClosedPanel(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void bTitlePanel1_MinimizedPanel(object sender, System.EventArgs e)
        {
            BTitlePanel btitpnl = (BTitlePanel)sender;

            int iCount = pnlParent.Controls.Count;
            int iHeight = 0;
            if (iCount == 1)
                iHeight = this.pnlParent.Height;
            else if (iCount > 1)
                iHeight = this.pnlParent.Height / 2;

            //SortedList sl = new SortedList();
            LinkedList llstChart = new LinkedList();
            foreach (Control chart in pnlParent.Controls)
            {
                chart.Height = iHeight;
                if (btitpnl.Name.Equals(chart.Name))
                {
                    chart.Dock = System.Windows.Forms.DockStyle.Top;
                }
                else
                    chart.Visible = true;

                llstChart.Add(chart.Name, chart);
            }
            this.pnlParent.Controls.Clear();
            for (int i = 0; i < llstChart.Count; i++)
            {
                Control chart = (Control)llstChart[llstChart.GetKey(i)];
                this.pnlParent.Controls.Add(chart);
            }
        }

        public void bTitlePanel1_MaximizedPanel(object sender, System.EventArgs e)
        {
            BTitlePanel btitpnl = (BTitlePanel)sender;
            foreach (Control chart in pnlParent.Controls)
            {
                if (btitpnl.Name.Equals(chart.Name))
                {
                    chart.Visible = true;
                    chart.Dock = System.Windows.Forms.DockStyle.Fill;
                    btitpnl.Dock = System.Windows.Forms.DockStyle.Fill;
                }
                else
                {
                    chart.Visible = false;
                    //chart.Dock = System.Windows.Forms.DockStyle.None;
                }

            }
        }


        #endregion



        #region Button Function
        private void Click_ZOOM_IN_Button()
        {
            this.SPCChart.ClickAction = BTeeChart.ChartClickAction.DEFAULT;
            ZoomPopup Zoom = new ZoomPopup();
            Zoom.SPC_CHART_TYPE = SPC_CHART_TYPE.BASECHART;
            Zoom.BaseChart = this;
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
            pop.BaseChart = this;
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


            //if (this.SPCChart.XAxisLabel == BTeeChart.XAxisLabelType.LABEL)
            //{
            //    this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.VALUE;
            //}
            //else
            //{
            //    this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
            //}
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

            //if (mkTip == null)
            //{
            //    mkTip = new Steema.TeeChart.Tools.MarksTip();
            //    mkTip.Style = MarksStyles.Value;
            //}

            //if (this.SPCChart.Chart.Tools.IndexOf(mkTip) < 0)
            //{
            //    this.SPCChart.Chart.Tools.Add(mkTip);
            //}
        }

        private void ValidateOCAP(Series s)
        {
            int _valueIndex = -1;

            //ArrayList arrOCAP = new ArrayList();
            for (int i = 0; i < ((DataSet)s.DataSource).Tables[0].Rows.Count; i++)
            {
                string sIndex = ((DataSet)s.DataSource).Tables[0].Rows[i]["X"].ToString();
                string sLabels = ((DataSet)s.DataSource).Tables[0].Rows[i]["Labels"].ToString();
                if (string.IsNullOrEmpty(sIndex)) continue;
                if (string.IsNullOrEmpty(sLabels)) continue;

                s[i].Color = Color.Black;

                _valueIndex = Convert.ToInt32(sIndex);

                string sOcapRawID = string.Empty;
                if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                {
                    sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.OCAP_RAWID].ToString().Replace(";", "").TrimEnd();

                    if (!string.IsNullOrEmpty(sOcapRawID))
                        s[i].Color = Color.Red;
                }

                //added by enkim 2012.08.28 for ocap data
                sOcapRawID = string.Empty;
                switch (this.Name)
                {
                    case Definition.CHART_TYPE.XBAR:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MEAN_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.MEAN_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.RANGE:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.RANGE_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.RANGE_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.STDDEV:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.STDDEV_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.STDDEV_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.MA:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MA_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.MA_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.MSD:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MSD_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.MSD_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.MR:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MR_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.MR_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.EWMA_MEAN:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.EWMA_RANGE:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.EWMA_STDDEV:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;
                }
                //added end
            }
        }

        private void ValidateOCAPforFilter(Series s)
        {
            int _valueIndex = -1;

            //ArrayList arrOCAP = new ArrayList();
            for (int i = 0; i < ((DataSet)s.DataSource).Tables[0].Rows.Count; i++)
            {
                string sIndex = ((DataSet)s.DataSource).Tables[0].Rows[i]["X"].ToString();
                string sLabels = ((DataSet)s.DataSource).Tables[0].Rows[i]["Labels"].ToString();
                if (string.IsNullOrEmpty(sIndex)) continue;
                if (string.IsNullOrEmpty(sLabels)) continue;

                _valueIndex = Convert.ToInt32(sIndex);

                string sOcapRawID = string.Empty;
                if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.OCAP_RAWID))
                {
                    sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.OCAP_RAWID].ToString().Replace(";", "").TrimEnd();

                    if (!string.IsNullOrEmpty(sOcapRawID))
                        s[i].Color = Color.Red;
                }

                //added by enkim 2012.08.28 for ocap data
                sOcapRawID = string.Empty;
                switch (this.Name)
                {
                    case Definition.CHART_TYPE.XBAR:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MEAN_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.MEAN_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.RANGE:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.RANGE_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.RANGE_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.STDDEV:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.STDDEV_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.STDDEV_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.MA:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MA_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.MA_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.MSD:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MSD_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.MSD_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.MR:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MR_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.MR_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.EWMA_MEAN:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.EWMAMEAN_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.EWMA_RANGE:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.EWMARANGE_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.EWMA_STDDEV:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.EWMASTDDEV_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;
                }
                //added end
            }
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

        private void bbtnTimeDisplay_Click(object sender, EventArgs e)
        {
            if (this.DataManager.RawDataTable != null && this.DataManager.RawDataTable.Rows.Count > 0 && this.DataManager.RawDataTable.Columns.Contains(COLUMN.RAW_DTTS))
            {
                BISTel.eSPC.Page.Report.Popup.SPCTimeBaseRawPopup spcTimeBaseRowPopup = new BISTel.eSPC.Page.Report.Popup.SPCTimeBaseRawPopup();
                spcTimeBaseRowPopup.dtOriData = this.DataManager.RawDataTable.Copy();
                spcTimeBaseRowPopup.InitializePopup();
                spcTimeBaseRowPopup.BringToFront();
                spcTimeBaseRowPopup.Show();
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_DISPLAY_DATA", null, null);
            }
        }

        private void bbtnRefresh_Click(object sender, EventArgs e)
        {
            if (this.bchkEQPID.Text.Length == 0 || this.bchkModuleID.Text.Length == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_EQP_MODULE", null, null);
                return;
            }
            this._IsVisibleZone = false;
            string strSelectedItems = this.bCheckCombo.Text;
            string strSelectedEQP = this.bchkEQPID.Text;
            //SPC-932 Old
            //string strSelectedModule = this.bchkModuleID.Text;

            //SPC-932 By Louis
            string strSelectedModule = "";
            if (this.bchkModuleID.Text.Contains("ALL"))
            {
                if (strArrModuleID != null && strArrModuleID.Length > 0)
                {
                    for (int i = 0; i < strArrModuleID.Length; i++)
                    {
                        if (i == 0)
                        {
                            strSelectedModule += strArrModuleID[i].ToString();
                        }
                        else
                        {
                            strSelectedModule += "," + strArrModuleID[i].ToString();
                        }
                    }
                }
            }
            else
            {
                strSelectedModule = this.bchkModuleID.Text;
            }
            //SPC-932 End
            this._dataManager.RawDataTable = this._dataManager.RawDataTableOriginal.Copy();
            Steema.TeeChart.Tools.CursorTool _cursortoolori = null;

            if (strSelectedItems.Length == 0)
            {
                string[] strArrEQP = strSelectedEQP.Split(',');
                string[] strArrModule = null;
                if (strSelectedModule.Length > 0)
                {
                    strArrModule = strSelectedModule.Split(',');
                }
                for (int i = 0; i < _dataManager.RawDataTable.Rows.Count; i++)
                {
                    bool bVisibleEQP = false;
                    bool bVisibleModule = false;

                    for (int k = 0; k < strArrEQP.Length; k++)
                    {
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                        {
                            if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.EQP_ID] != null &&
                                _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.EQP_ID].ToString().Length > 0)
                            {
                                for (int iTmp = 0; iTmp < _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.EQP_ID].ToString().Split(';').Length; iTmp++)
                                {
                                    if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.EQP_ID].ToString().Split(';')[iTmp] == strArrEQP[k])
                                    {
                                        bVisibleEQP = true;
                                        break;
                                    }
                                }                                
                            }
                        }
                    }

                    if (strArrModule != null)
                    {
                        for (int j = 0; j < strArrModule.Length; j++)
                        {
                            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MODULE_ALIAS))
                            {
                                if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ALIAS] != null &&
                                    _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ALIAS].ToString().Length > 0)
                                {
                                    for (int iTmp = 0; iTmp < _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ALIAS].ToString().Split(';').Length; iTmp++)
                                    {
                                        if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ALIAS].ToString().Split(';')[iTmp] == strArrModule[j])
                                        {
                                            bVisibleModule = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bVisibleModule = true;
                    }

                    if (!(bVisibleEQP && bVisibleModule))
                    {
                        _dataManager.RawDataTable.Rows.RemoveAt(i);
                        i--;
                    }
                }


                for (int i = 0; i < this.SPCChart.Chart.Tools.Count; i++)
                {
                    Steema.TeeChart.Tools.CursorTool _cursortool = this.SPCChart.Chart.Tools[i] as Steema.TeeChart.Tools.CursorTool;

                    if (_cursortool != null)
                    {
                        _cursortoolori = _cursortool;
                    }
                }
                this.ClearChart();
                this.RefreshChart();
                this.DrawSPCChart();
                if (_cursortoolori != null)
                    this.SPCChart.Chart.Tools.Add(_cursortoolori);
                this._dataManager.RawDataTable = this._dataManager.RawDataTableOriginal.Copy();
            }
            else
            {
                string[] strArrEQP = strSelectedEQP.Split(',');
                string[] strArrModule = null;
                if (strSelectedModule.Length > 0)
                {
                    strArrModule = strSelectedModule.Split(',');
                }
                for (int i = 0; i < _dataManager.RawDataTable.Rows.Count; i++)
                {
                    bool bVisibleEQP = false;
                    bool bVisibleModule = false;

                    for (int k = 0; k < strArrEQP.Length; k++)
                    {
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.EQP_ID))
                        {
                            if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.EQP_ID] != null &&
                                _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.EQP_ID].ToString().Length > 0)
                            {
                                for (int iTmp = 0; iTmp < _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.EQP_ID].ToString().Split(';').Length; iTmp++)
                                {
                                    if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.EQP_ID].ToString().Split(';')[iTmp] == strArrEQP[k])
                                    {
                                        bVisibleEQP = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (strArrModule != null)
                    {
                        for (int j = 0; j < strArrModule.Length; j++)
                        {
                            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MODULE_ALIAS))
                            {
                                if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ALIAS] != null &&
                                    _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ALIAS].ToString().Length > 0)
                                {
                                    for (int iTmp = 0; iTmp < _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ALIAS].ToString().Split(';').Length; iTmp++)
                                    {
                                        if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ALIAS].ToString().Split(';')[iTmp] == strArrModule[j])
                                        {
                                            bVisibleModule = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bVisibleModule = true;
                    }

                    if (!(bVisibleEQP && bVisibleModule))
                    {
                        _dataManager.RawDataTable.Rows.RemoveAt(i);
                        i--;
                    }
                }

                string[] strArrGroup = strSelectedItems.Split(',');


                for (int i = 0; i < _dataManager.RawDataTable.Rows.Count; i++)
                {
                    bool bVisible = false;
                    for (int k = 0; k < strArrGroup.Length; k++)
                    {
                        string strConKey = strArrGroup[k].Split('[')[1].Replace("]", "").Trim();
                        string strConVal = strArrGroup[k].Split('[')[0].Trim();
                        if (_dataManager.RawDataTable.Columns.Contains(strConKey))
                        {
                            if (_dataManager.RawDataTable.Rows[i][strConKey] != null &&
                                _dataManager.RawDataTable.Rows[i][strConKey].ToString().Length > 0)
                            {
                                for (int iTmp = 0; iTmp < _dataManager.RawDataTable.Rows[i][strConKey].ToString().Split(';').Length; iTmp++)
                                {
                                    if (_dataManager.RawDataTable.Rows[i][strConKey].ToString().Split(';')[iTmp] == strConVal)
                                    {
                                        bVisible = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (!bVisible)
                    {
                        _dataManager.RawDataTable.Rows.RemoveAt(i);
                        i--;
                    }
                }

                for (int i = 0; i < this.SPCChart.Chart.Tools.Count; i++)
                {
                    Steema.TeeChart.Tools.CursorTool _cursortool = this.SPCChart.Chart.Tools[i] as Steema.TeeChart.Tools.CursorTool;

                    if (_cursortool != null)
                    {
                        _cursortoolori = _cursortool;
                    }
                }
                this.ClearChart();
                this.RefreshChart();
                this.DrawSPCChart();
                if (_cursortoolori != null)
                    this.SPCChart.Chart.Tools.Add(_cursortoolori);
                this._dataManager.RawDataTable = this._dataManager.RawDataTableOriginal.Copy();
            }
        }

        internal void Filter(Dictionary<string, string[]> groupingAndFilteringValue)
        {
            InitializeFiltering();

            if(groupingAndFilteringValue.Count == 0)
                return;

            List<string> seriesNames = new List<string>();

            // Add columns to Datasource table for new series
            for(int i=0; i<this._dtDataSource.Rows.Count; i++)
            {
                DataRow dr = FindMatchedRowInRawTable(i);
                if(dr == null)
                    continue;

                if(_grouperAndFilter.IsRowNeededToBeAdded(dr, groupingAndFilteringValue))
                {
                    string columnName = "";

                    if (this.mParamTypeCd == "MET" && this._lstRawColumn.Count > 0 && this.Name == "RAW")
                    {
                        foreach (string strSeriesName in this._lstRawColumn)
                        {
                            columnName = GetColumnNameForNewSerie(strSeriesName, dr, groupingAndFilteringValue);

                            if (!this.dtDataSource.Columns.Contains(columnName))
                            {
                                this.dtDataSource.Columns.Add(columnName);
                            }
                            if (!seriesNames.Contains(columnName))
                            {
                                seriesNames.Add(columnName);
                            }

                            this.dtDataSource.Rows[i][columnName] = this.dtDataSource.Rows[i][strSeriesName];
                        }
                    }
                    else
                    {
                        columnName = GetColumnNameForNewSerie(CommonChart.GetChartPointSeries(this.Name), dr, groupingAndFilteringValue);

                        if (!this.dtDataSource.Columns.Contains(columnName))
                        {
                            this.dtDataSource.Columns.Add(columnName);
                        }
                        if (!seriesNames.Contains(columnName))
                        {
                            seriesNames.Add(columnName);
                        }

                        this.dtDataSource.Rows[i][columnName] = this.dtDataSource.Rows[i][CommonChart.GetChartPointSeries(this.Name)];
                    }
                }
            }

            if (this._llstChartSeriesColor == null)
            {
                this._llstChartSeriesColor = new LinkedList();
            }

            // Add new series to the chart
            foreach(string s in seriesNames)
            {
                //modified by enkim 2012.05.25 SPC-851
                //SeriesInfo si = new SeriesInfo(typeof (Line), s, this._dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, s);
                SeriesInfo si = new SeriesInfo(typeof(ExtremeBFastLine), s, this._dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, s);
                //si.TreatNulls = TreatNullsStyle.Skip;
                
                
                this.SPCChart.AddSeries(si);

                groupSeries.Add(si.Series);

                string sColorFilterName = s.Substring(s.IndexOf('^') + 1);

                if (this._llstChartSeriesColor.Contains(sColorFilterName))
                {
                    si.SeriesColor = (Color)this._llstChartSeriesColor[sColorFilterName];
                }
                else
                {
                    si.SeriesColor = _chartUtil.GetSeriesColor(this.bTeeChart1.GetAllSeries().Count - 1);
                    this._llstChartSeriesColor.Add(sColorFilterName, si.SeriesColor);
                }

                Series series = this.SPCChart.Chart.Series[this.SPCChart.Chart.Series.Count - 1];
                
                //((Line) series).Active = true;
                //((Line) series).LinePen.Width = 1;
                //((Line) series).LinePen.Color = Color.Gray;
                //((Line) series).TreatNulls = TreatNullsStyle.Skip;

                ((ExtremeBFastLine)series).LinePen.Width = 1;
                ((ExtremeBFastLine)series).LinePen.Color = Color.Gray;
                ((ExtremeBFastLine)series).Active = true;
                SetNullWrongPoints(series, s, this._dtDataSource);

                if(this.Name != "RAW")
                    this.ValidateOCAPforFilter(series);

                for(int i=0; i<series.Labels.Count; i++)
                {
                    if (string.IsNullOrEmpty(series.Labels[i]))
                        series.SetNull(i);
                }
                si.TreatNulls = TreatNullsStyle.DoNotPaint;
                //modified end SPC-851
            }

            // hide point series
            Series pointSeries = this.SPCChart.GetSeriesByName(CommonChart.GetChartPointSeries(this.NAME));
            if (pointSeries != null)
            {
                pointSeries.Visible = false;
                pointSeries.ShowInLegend = false;
            }

            // sorting
            SortLegendForFiltering();

            //added by enkim 2012.12.12 SPC-839
            if (!this.SPCChart.Chart.Axes.Left.Labels.ValueFormat.Contains("E"))
            {
                this.SPCChart.Chart.Axes.Left.Labels.ValueFormat = "#,##0.###";
            }
            //added end
  
            // refresh axes
            this.SPCChart.Chart.Axes.Bottom.Labels.Style = AxisLabelStyle.Text;
            this.SPCChart.Chart.Axes.Bottom.Labels.ValueFormat = this.AxisDateTimeFormat;
            this.SPCChart.ChangeXLabelColumnDefault(Definition.CHART_COLUMN.TIME2);
            this.SPCChart.Chart.Axes.Bottom.Increment = Steema.TeeChart.Utils.DateTimeStep[(int)Steema.TeeChart.DateTimeSteps.OneDay];
            this.SPCChart.Refresh();
            foreach (Series series in this.SPCChart.Chart.Series)
            {
                if (series.GetType().Equals(typeof(ExtremeBFastLine)))
                {
                    if(!((ExtremeBFastLine)series).Pointer.InflateMargins)
                    {
                        ((ExtremeBFastLine)series).Pointer.InflateMargins = true;
                    }
                }
            }
        }

        private void SetNullWrongPoints(Series series, string s, DataTable dataTable)
        {
            // find last null values of datatable
            int lastIndex = -1;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if(string.IsNullOrEmpty(dataTable.Rows[i][s].ToString()))
                {    
                    if(lastIndex == -1)
                        lastIndex = i;
                }
                else
                {
                    lastIndex = -1;
                }
            }

            // set null in series
            if(lastIndex == -1)
                return;

            for( ; lastIndex < dataTable.Rows.Count; lastIndex++)
                series.SetNull(lastIndex);
        }

        /// <summary>
        /// Find the matched row in the raw table with the DataSource table 
        /// </summary>
        /// <param name="rowIndex">the row index of the datasoruce table</param>
        /// <returns>the matched row of the raw table with the row of the datasource table</returns>
        private DataRow FindMatchedRowInRawTable(int rowIndex)
        {
            string sourceID = this._dtDataSource.Rows[rowIndex][Definition.CHART_COLUMN.DTSOURCEID].ToString();

            DataRow[] row = this._dataManager.RawDataTable.Select(Definition.CHART_COLUMN.DTSOURCEID + " = '" + sourceID + "'");

            if(row != null && row.Length > 0)
                return row[0];

            return null;
        }

        /// <summary>
        /// Making column name using by Grouping and Filtering condition
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="dr">The row of the raw table</param>
        /// <param name="groupingAndFilteringValue">condition</param>
        /// <returns>column name</returns>
        private string GetColumnNameForNewSerie(string prefix, DataRow dr, Dictionary<string, string[]> groupingAndFilteringValue)
        {
            string columnName = prefix;

            foreach(var kvp in groupingAndFilteringValue)
            {
                columnName += "^" + _grouperAndFilter.GetFirstValue(dr[kvp.Key].ToString());
            }

            return columnName;
        }

        private void InitializeFiltering()
        {
            foreach(Series s in groupSeries)
            {
                this.SPCChart.RemoveSeries(s);

                //SPC-874 by Louis
               //this.dtDataSource.Columns.Remove(s.Title);
            }

            Series series = this.SPCChart.GetSeriesByName(CommonChart.GetChartPointSeries(this.Name));
            if (series != null)
            {
                series.Visible = true;
                series.ShowInLegend = true;
            }

            if (this.mParamTypeCd == "MET" && this._lstRawColumn.Count > 0)
            {
                foreach (string strSeriesName in this._lstRawColumn)
                {
                    series = this.SPCChart.GetSeriesByName(strSeriesName);
                    if (series != null)
                    {
                        if (series.Visible)
                        {
                            series.Visible = true;
                            series.ShowInLegend = true;
                        }
                    }
                }
            }

            groupSeries = new List<Series>();
        }

        private void SortLegendForFiltering()
        {
            var seriesInfos = this.SPCChart.GetAllSeriesInfos();
            var removedSeriesInfos = new List<SeriesInfo>();
            var maxMinSeriesInfos = new List<SeriesInfo>();
            for(int i=0; i<seriesInfos.Count; i++)
            {
               if(seriesInfos[i].SeriesName.ToUpper() == "USL" ||
                seriesInfos[i].SeriesName.ToUpper() == "UCL" ||
                seriesInfos[i].SeriesName.ToUpper() == "LSL" ||
                seriesInfos[i].SeriesName.ToUpper() == "LCL" ||
                seriesInfos[i].SeriesName.ToUpper() == "TARGET" ||
                seriesInfos[i].SeriesName.ToUpper() == CommonChart.GetChartPointSeries(this.Name) ||
                this.groupSeries.Exists(s => s.Title.ToUpper() == seriesInfos[i].SeriesName.ToUpper()))
                   continue;

                if(seriesInfos[i].SeriesName.ToUpper() == "MAX" ||
                    seriesInfos[i].SeriesName.ToUpper() == "MIN")
                {
                    maxMinSeriesInfos.Add(seriesInfos[i]);
                }
                else
                {
                    removedSeriesInfos.Add(seriesInfos[i]);
                }

                this.SPCChart.RemoveSeries(seriesInfos[i].SeriesName);
            }

            for(int i=0; i<maxMinSeriesInfos.Count; i++)
            {
                this.SPCChart.AddSeries(maxMinSeriesInfos[i]);
                maxMinSeriesInfos[i].Visible = false;
            }


            for(int i=0; i<removedSeriesInfos.Count; i++)
            {
                this.SPCChart.AddSeries(removedSeriesInfos[i]);
                removedSeriesInfos[i].Visible = false;
            }
        }

        //2012-03-23 added by rachel -->
        //[SPC-659]
        public void ReleaseData()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref this._dtDataSource);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref this._dtDataSourceToolTip);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref this._dsChangeXAxis);


        }

        //2012-03-23 added by rachel -->
        //[SPC-659]
        public void ReleaseDataManager()
        {
            if (_dataManager != null)
                _dataManager.ReleaseData();
        }
    }
}
