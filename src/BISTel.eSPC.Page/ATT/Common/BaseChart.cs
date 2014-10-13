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
using BISTel.eSPC.Common.ATT;
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

namespace BISTel.eSPC.Page.ATT.Common
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

        bool _bSeriesAllCheck = true;
        bool _IsVisibleLegendScrollBar = false;
        bool _IsVisibleShadow = false;

        private DateTime _StartDateTime = DateTime.Now;
        private DateTime _EndDateTime = DateTime.Now;


        System.Windows.Forms.Panel pnlParent = null;

        string mParamTypeCd = string.Empty;

        bool isWaferColumn = false;

        private List<Series> groupSeries = new List<Series>();

        GrouperAndFilter _grouperAndFilter = new GrouperAndFilter();

        public BaseChart()
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
            }
        }

        #region PageLoad & Initialize.
        public virtual void InitializeChart()
        {
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
            this.SPCChart.IsExponentialFormatLeftAxis = this._Initialization.GetExponentialAxis(Definition.PAGE_KEY_SPC_ATT_CONTROL_CHART_UC);
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
            this.SPCChart.IsExponentialFormatLeftAxis = this._Initialization.GetExponentialAxis(Definition.PAGE_KEY_SPC_ATT_CONTROL_CHART_UC);
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
                    string strConKey = value.Tables[0].Rows[i][BISTel.eSPC.Common.COLUMN.CONTEXT_KEY].ToString();
                    string strConVal = value.Tables[0].Rows[i][BISTel.eSPC.Common.COLUMN.CONTEXT_VALUE].ToString();
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
                }
            }
        }
        string[] strArrModuleID;
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


        #region ::: Virtual Method.

        public virtual bool DrawSPCChart()
         {
            try
            {
                if (this.DataManager.RawDataTable.Rows.Count < 1)
                {
                    return false;
                }

                MakeDataSourceToDrawSPCChart();
                DrawChartWithSeriesInfoForSumChart();
                ChangeSeriesStyleForSumChart();
                MakeTipInfo();
                this._dtDataSourceToolTip = this._dataManager.RawDataTable.Copy();

                if (!this.SPCChart.Chart.Axes.Left.Labels.ValueFormat.Contains("E"))
                {
                    this.SPCChart.Chart.Axes.Left.Labels.ValueFormat = "#,##0.###";
                }

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

            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref _dtDataSource);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref _dtDataSourceToolTip);

            this._dtDataSource = null;
        }

        protected virtual void InitChart()
        {
        }


        protected virtual void MakeDataSourceToDrawSPCChart()
        {
            return;
        }

        protected virtual void DrawChartWithSeriesInfoForSumChart()
        {
            return;
        }

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
                else if (s.Title == Definition.CHART_COLUMN.P
                        || s.Title == Definition.CHART_COLUMN.PN
                        || s.Title == Definition.CHART_COLUMN.C
                        || s.Title == Definition.CHART_COLUMN.U)
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
                if (s != null) s.Active = bActive;

            }
            else if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.CONTROL_LIMIT))
            {
                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.UCL);
                if (s != null) s.Active = bActive;

                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.LCL);
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

        protected virtual void MakeTipInfo()
        {
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
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_LEGEND_VISIBLITY)
                {
                    this.Click_LEGEND_VISIBLITY_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_LEGEND_ALL_CHECK)
                {
                    this.Click_LEGEND_ALL_CHECK_Button();
                }
                else if (name.ToUpper() == Definition.BUTTON_KEY_CHART_COPY_CHART)
                {
                    this.Click_COPY_CHART_Button();
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
                }

            }
        }


        #endregion



        #region Button Function
        private void Click_ZOOM_IN_Button()
        {
            this.SPCChart.ClickAction = BTeeChart.ChartClickAction.DEFAULT;

            ZoomPopup Zoom = new ZoomPopup();
            Zoom.SPC_CHART_TYPE = BISTel.eSPC.Common.SPC_CHART_TYPE.BASECHART;
            Zoom.BaseChart = this;
            Zoom.ShowDialog();
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

        private void Click_COPY_CHART_Button()
        {
            this.SPCChart.ChartCopyToClipboard();
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


        protected void DrawCrossLine(double xValue, double yValue)
        {
            this.SPCChart.AddColorLineOfHori(yValue, COLOR_SERIES_SYNC);
            this.SPCChart.AddColorLineOfVert(xValue, COLOR_SERIES_SYNC);
        }

        public void DrawChart(SeriesInfo Series)
        {
            this.SPCChart.AddSeries(Series);
        }

        private void ValidateOCAP(Series s)
        {
            int _valueIndex = -1;

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

                sOcapRawID = string.Empty;
                switch (this.Name)
                {
                    case Definition.CHART_TYPE.P:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.P_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.PN:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.PN_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.C:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.C_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.U:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.U_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;
                }
            }
        }

        private void ValidateOCAPforFilter(Series s)
        {
            int _valueIndex = -1;

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

                sOcapRawID = string.Empty;
                switch (this.Name)
                {
                    case Definition.CHART_TYPE.P:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.P_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.P_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.PN:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.PN_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.PN_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.C:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.C_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.C_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;

                    case Definition.CHART_TYPE.U:
                        if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.U_OCAP_LIST))
                        {
                            sOcapRawID = _dataManager.RawDataTable.Rows[_valueIndex][Definition.CHART_COLUMN.U_OCAP_LIST].ToString().Replace("^", "").TrimEnd();

                            if (!string.IsNullOrEmpty(sOcapRawID))
                                s[i].Color = Color.Red;
                        }
                        break;
                }
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

        private void bbtnRefresh_Click(object sender, EventArgs e)
        {
            if (this.bchkEQPID.Text.Length == 0 || this.bchkModuleID.Text.Length == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_EQP_MODULE", null, null);
                return;
            }
            string strSelectedItems = this.bCheckCombo.Text;
            string strSelectedEQP = this.bchkEQPID.Text;
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
                            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MODULE_ID))
                            {
                                if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ID] != null &&
                                    _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ID].ToString().Length > 0)
                                {
                                    for (int iTmp = 0; iTmp < _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ID].ToString().Split(';').Length; iTmp++)
                                    {
                                        if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ID].ToString().Split(';')[iTmp] == strArrModule[j])
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
                            if (_dataManager.RawDataTable.Columns.Contains(Definition.CHART_COLUMN.MODULE_ID))
                            {
                                if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ID] != null &&
                                    _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ID].ToString().Length > 0)
                                {
                                    for (int iTmp = 0; iTmp < _dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ID].ToString().Split(';').Length; iTmp++)
                                    {
                                        if (_dataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.MODULE_ID].ToString().Split(';')[iTmp] == strArrModule[j])
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
                    string columnName = GetColumnNameForNewSerie(CommonChart.GetChartPointSeries(this.Name), dr, groupingAndFilteringValue);

                    if(!this.dtDataSource.Columns.Contains(columnName))
                    {
                        this.dtDataSource.Columns.Add(columnName);
                    }
                    if(!seriesNames.Contains(columnName))
                    {
                        seriesNames.Add(columnName);
                    }

                    this.dtDataSource.Rows[i][columnName] = this.dtDataSource.Rows[i][CommonChart.GetChartPointSeries(this.Name)];
                }
            }

            // Add new series to the chart
            foreach(string s in seriesNames)
            {
                SeriesInfo si = new SeriesInfo(typeof(ExtremeBFastLine), s, this._dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, s);
                
                this.SPCChart.AddSeries(si);

                groupSeries.Add(si.Series);

                si.SeriesColor = _chartUtil.GetSeriesColor(this.bTeeChart1.GetAllSeries().Count - 1);

                Series series = this.SPCChart.Chart.Series[this.SPCChart.Chart.Series.Count - 1];
                
                ((ExtremeBFastLine)series).Active = true;
                ((ExtremeBFastLine)series).LinePen.Width = 1;
                ((ExtremeBFastLine)series).LinePen.Color = Color.Gray;
                ((ExtremeBFastLine)series).Pointer.InflateMargins = true;
                SetNullWrongPoints(series, s, this._dtDataSource);

                if(this.Name != "RAW")
                    this.ValidateOCAPforFilter(series);

                for(int i=0; i<series.Labels.Count; i++)
                {
                    if (string.IsNullOrEmpty(series.Labels[i]))
                        series.SetNull(i);
                }
                si.TreatNulls = TreatNullsStyle.DoNotPaint;
            }

            // hide point series
            Series pointSeries = this.SPCChart.GetSeriesByName(CommonChart.GetChartPointSeries(this.NAME));
            pointSeries.Visible = false;
            pointSeries.ShowInLegend = false;

            // sorting
            SortLegendForFiltering();

            if (!this.SPCChart.Chart.Axes.Left.Labels.ValueFormat.Contains("E"))
            {
                this.SPCChart.Chart.Axes.Left.Labels.ValueFormat = "#,##0.###";
            }
  
            // refresh axes
            this.SPCChart.Chart.Axes.Bottom.Labels.Style = AxisLabelStyle.Text;
            this.SPCChart.Chart.Axes.Bottom.Labels.ValueFormat = this.AxisDateTimeFormat;
            this.SPCChart.ChangeXLabelColumnDefault(Definition.CHART_COLUMN.TIME2);
            this.SPCChart.Chart.Axes.Bottom.Increment = Steema.TeeChart.Utils.DateTimeStep[(int)Steema.TeeChart.DateTimeSteps.OneDay];
            this.SPCChart.Refresh();
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
            }

            Series series = this.SPCChart.GetSeriesByName(CommonChart.GetChartPointSeries(this.Name));
            series.Visible = true;
            series.ShowInLegend = true;

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

        public void ReleaseData()
        {
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref this._dtDataSource);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataTableRelease(ref this._dtDataSourceToolTip);
            BISTel.PeakPerformance.Client.CommonLibrary.MemoryTool.DataSetRelease(ref this._dsChangeXAxis);


        }

        public void ReleaseDataManager()
        {
            if (_dataManager != null)
                _dataManager.ReleaseData();
        }
    }
}
