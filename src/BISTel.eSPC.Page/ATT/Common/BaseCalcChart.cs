using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.eSPC.Common.ATT;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts;
using java.lang;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl;
using Exception = System.Exception;

namespace BISTel.eSPC.Page.ATT.Common
{
    public partial class BaseCalcChart : BasePageUCtrl
    {
        public MultiLanguageHandler _mlthandler;
        Color COLOR_SERIES_SYNC = Color.Gold;
        SourceDataManager _dataManager = null;
        Initialization _Initialization;
        DataTable _dtDataSource = null;
        List<string> _lstRawColumn = null;
        LinkedList _llstChartSeriesVisibleType = null;
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


        private DateTime _StartDateTime = DateTime.Now;
        private DateTime _EndDateTime = DateTime.Now;


        System.Windows.Forms.Panel pnlParent = null;

        string mParamTypeCd = string.Empty;

        bool isWaferColumn = false;

        private Steema.TeeChart.Styles.Shape _ShapeSeries = null;

        private bool _isPointSelected = false;

        private int _startPositionX = 0;
        private int _startPositionY = 0;
        private int _endPositionX = 0;
        private int _endPositionY = 0;

        public BaseCalcChart()
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
                for (int i = 0; i < this.bButtonList1.Items.Count; i++)
                {
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
            this.bTitlePanel1.MaxResizable = true;
            this.bButtonList1.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(bButtonList1_ButtonClick);
            this.bTitlePanel1.MouseDown += new MouseEventHandler(bTitlePanel1_MouseDown);
            this.bTitlePanel1.MouseMove += new MouseEventHandler(bTitlePanel1_MouseMove);
            this.bTitlePanel1.MouseUp += new MouseEventHandler(bTitlePanel1_MouseUp);
            this.bTitlePanel1.MouseDoubleClick += new MouseEventHandler(bTitlePanel1_MouseDoubleClick);
            this.bTitlePanel1.ClosedPanel += new EventHandler(bTitlePanel1_ClosedPanel);

            this.SPCChartTitlePanel.MaxResizable = false;
            this.SPCChart.Chart.Legend.CheckBoxes = true;
            this.SPCChart.IsVisibleLegendScrollBar = this.IsVisibleLegendScrollBar;
            this.SPCChart.IsVisibleShadow = this.IsVisibleShadow;
            this.SPCChart.AxisDateTimeFormat = this.AxisDateTimeFormat;
            this.SPCChart.IsUseAutomaticAxisOfTeeChart = true;
            this.SPCChart.IsAutoValueFormatLeftAxis = true;
            this.SPCChart.IsExponentialFormatLeftAxis = this._Initialization.GetExponentialAxis(Definition.PAGE_KEY_SPC_ATT_CONTROL_CHART_UC);
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
            this.SPCChartTitlePanel.MaxResizable = true;
            this.SPCChart.Chart.Legend.CheckBoxes = true;
            this.SPCChart.IsVisibleLegendScrollBar = this.IsVisibleLegendScrollBar;
            this.SPCChart.IsVisibleShadow = this.IsVisibleShadow;
            this.SPCChart.AxisDateTimeFormat = this.AxisDateTimeFormat;
            this.SPCChart.IsUseAutomaticAxisOfTeeChart = true;
            this.SPCChart.IsAutoValueFormatLeftAxis = true;
            this.SPCChart.IsExponentialFormatLeftAxis = this._Initialization.GetExponentialAxis(Definition.PAGE_KEY_SPC_ATT_CONTROL_CHART_UC);
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

        public bool IsPointMarking
        {
            get { return this.bckbPointMarking.Checked; }
        }
        #endregion


        #region ::: Virtual Method.

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
        public virtual bool DrawSPCChart(bool sample, int samplePeriod, int sampleCount)
        {
            try
            {
                if (this.DataManager.RawDataTable.Rows.Count < 1)
                {
                    return false;
                }

                MakeDataSourceToDrawSPCChart(sample, samplePeriod, sampleCount);
                DrawChartWithSeriesInfo();
                ChangeSeriesStyle();
                MakeTipInfo();

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



        public virtual void ClearChart()
        {
            this.SPCChart.ClearChart();
            this._dtDataSource = null;
        }

        protected virtual void MakeDataSourceToDrawSPCChart(bool sample, int samplePeriod, int sampleCount)
        {
            return;
        }


        protected virtual void DrawChartWithSeriesInfo()
        {
            return;
        }

        public virtual void ChangeSeriesStyle()
        {
            Series s = null;
            for (int i = 0; i < this.SPCChart.Chart.Series.Count; i++)
            {
                s = this.SPCChart.Chart.Series[i];

                ((Line)s).LinePen.Width = 2;
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
                else if (s.Title == Definition.CHART_COLUMN.P
                        || s.Title == Definition.CHART_COLUMN.PN
                        || s.Title == Definition.CHART_COLUMN.C
                        || s.Title == Definition.CHART_COLUMN.U)
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
                    ((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.AVG);
                }
                else
                {
                    ((Line)s).LinePen.Visible = false;
                    ((Line)s).Active = true;
                    for (int j = 0; j < lstRawColumn.Count; j++)
                    {
                        if (lstRawColumn[j].Contains(s.Title))
                        {
                            ((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES.POINT);
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
                if (s != null) ((Line)s).Active = bActive;

            }
            else if (pTitle == GetSeriesMultiLanguage(Definition.CHART_SERIES.CONTROL_LIMIT))
            {
                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.UCL);
                if (s != null) ((Line)s).Active = bActive;

                s = this.SPCChart.Chart.Series.WithTitle(Definition.CHART_COLUMN.LCL);
                if (s != null) ((Line)s).Active = bActive;

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

            SortedList sl = new SortedList();
            foreach (Control chart in pnlParent.Controls)
            {
                chart.Height = iHeight;
                if (btitpnl.Name.Equals(chart.Name))
                {
                    chart.Dock = System.Windows.Forms.DockStyle.Top;
                }
                else
                    chart.Visible = true;

                sl.Add(chart.Name, chart);
            }
            this.pnlParent.Controls.Clear();
            for (int i = 0; i < sl.Count; i++)
            {
                Control chart = (Control)sl[sl.GetKey(i)];
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
            if(this.bckbPointMarking.Checked == true)
            {
                this.bckbPointMarking.Checked = false;
            }

            this.SPCChart.ClickAction = BTeeChart.ChartClickAction.DEFAULT;

            ZoomPopup Zoom = new ZoomPopup();
            Zoom.SPC_CHART_TYPE = BISTel.eSPC.Common.SPC_CHART_TYPE.BASECALCCHART;
            Zoom.BaseCalcChart = this;
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

        private void bbtnTimeDisplay_Click(object sender, EventArgs e)
        {
            if (this.DataManager.RawDataTable != null && this.DataManager.RawDataTable.Rows.Count > 0 && this.DataManager.RawDataTable.Columns.Contains(BISTel.eSPC.Common.COLUMN.RAW_DTTS))
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

        private void bckbPointMarking_CheckedChanged(object sender, EventArgs e)
        {
            if(this._dtDataSource != null && this.bckbPointMarking.Checked)
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
    }
}
