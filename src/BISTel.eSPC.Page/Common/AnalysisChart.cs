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


using Steema.TeeChart.Styles;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl;

namespace BISTel.eSPC.Page.Common
{
    public partial class AnalysisChart : BasePageUCtrl
    {
        private  MultiLanguageHandler _mlthandler;
        private SourceDataManager _dataManager = null;
        private Initialization _Initialization;
        private ChartUtility chartUtil;
        private DataTable _dtDataSource = null;


        string _legendLocation = Definition.Right;
        string _AxisDateTimeFormat = Definition.DATETIME_FORMAT_yyMMdd;

        bool _bRegular = false;
        bool _bSeriesAllCheck = true;
        bool _IsVisibleLegendScrollBar = false;
        bool _IsVisibleShadow = false;
        bool _isDateTime = false;
        bool _isShowInLegend = true;
        

        Steema.TeeChart.Tools.MarksTip mkTip = null;

        public AnalysisChart()
        {
            InitializeComponent();
            InitializeVariable();
        }


        public void InitializeVariable()
        {
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();            
            this._mlthandler = MultiLanguageHandler.getInstance(); 
            this.chartUtil = new ChartUtility();           
        }


        public virtual bool DrawSPCChart()
        {
            try
            {
                if (this.DataManager.RawDataTable.Rows.Count < 1)
                {                    
                    return false;
                }
                
                //DrawChartWithSeriesInfoCandle();
                //ChangeSeriesStyle();
                //MakeTipInfo();

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


        public virtual void ChangeSeriesStyle()
        {
            Series s = null;
            for (int i = 0; i < this.SPCChart.Chart.Series.Count; i++)
            {
                s = this.SPCChart.Chart.Series[i];                
                ((Line)s).Pointer.HorizSize = 3;
                ((Line)s).Pointer.VertSize = 3;
                ((Line)s).LinePen.Width = 2;
                ((Line)s).Pointer.Style = PointerStyles.Circle;
                ((Line)s).OutLine.Visible = false;
                ((Line)s).ColorEachLine = false;
            }

            //this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
            //this.SPCChart.Chart.Axes.Bottom.Labels.ValueFormat = this.AxisDateTimeFormat;
            //this.SPCChart.ChangeXLabelColumnDefault(Definition.CHART_COLUMN.TIME2);
            //this.SPCChart.Chart.Axes.Bottom.Increment = Steema.TeeChart.Utils.DateTimeStep[(int)Steema.TeeChart.DateTimeSteps.OneDay];
            //this.SPCChart.Refresh();
        }

        protected virtual void MakeTipInfo()
        {
            //Steema.TeeChart.Tools.MarksTip mkTip = new Steema.TeeChart.Tools.MarksTip();
            //mkTip.Style = MarksStyles.Value;              
            //this.SPCChart.Chart.Tools.Add(mkTip);                        
        }
        

        protected virtual void MakeDataSourceToDrawSPCChart()
        {

            DataRow dr = null;
            this.dtDataSource = new DataTable();
            this.dtDataSource.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, typeof(int));            
            //MakeDataTable(llstChartColumn);
        }


        protected virtual void DrawChartWithSeriesInfoBoxPlot()
        {
            SeriesInfo si = null;                        
            for (int i = 0; i < this.dtDataSource.Columns.Count; i++)
            {
                string seriesName = this.dtDataSource.Columns[i].ColumnName;
                si = new SeriesInfo(typeof(Box), seriesName, this.dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName);
                si.SeriesName = seriesName;                                   
                //this.SPCChart.Chart.Series.Add(AddCandleSeries(seriesName, vertAxis, chartUtil.GetSeriesColor(i)));                            
                this.DrawChart(si);
            }                        
        }


        protected virtual void DrawChartWithSeriesInfoHistogram()
        {
            SeriesInfo si = null;
            for (int i = 0; i < this.dtDataSource.Columns.Count; i++)
            {
                string seriesName = this.dtDataSource.Columns[i].ColumnName;
                si = new SeriesInfo(typeof(Histogram), seriesName, this.dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName);
                si.SeriesName = seriesName;
                //this.SPCChart.Chart.Series.Add(AddCandleSeries(seriesName, vertAxis, chartUtil.GetSeriesColor(i)));                            
                this.DrawChart(si);
            }
        }

        protected virtual void DrawChartWithSeriesInfoRun()
        {
            SeriesInfo si = null;
            for (int i = 0; i < this.dtDataSource.Columns.Count; i++)
            {
                string seriesName = this.dtDataSource.Columns[i].ColumnName;
                si = new SeriesInfo(typeof(Line), seriesName, this.dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName);
                si.SeriesName = seriesName;
                //this.SPCChart.Chart.Series.Add(AddCandleSeries(seriesName, vertAxis, chartUtil.GetSeriesColor(i)));                            
                this.DrawChart(si);
            }
        }

        protected virtual void DrawChartWithSeriesInfoDotPlot()
        {
            SeriesInfo si = null;
            for (int i = 0; i < this.dtDataSource.Columns.Count; i++)
            {
                string seriesName = this.dtDataSource.Columns[i].ColumnName;
                si = new SeriesInfo(typeof(Line), seriesName, this.dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName);
                si.SeriesName = seriesName;
                //this.SPCChart.Chart.Series.Add(AddCandleSeries(seriesName, vertAxis, chartUtil.GetSeriesColor(i)));                            
                this.DrawChart(si);
            }
        }

        public void DrawChart(SeriesInfo Series)
        {
            this.SPCChart.AddSeries(Series);

            if (mkTip == null)
            {
                mkTip = new Steema.TeeChart.Tools.MarksTip();
                mkTip.Style = MarksStyles.Value;
            }

            if (this.SPCChart.Chart.Tools.IndexOf(mkTip) < 0)
            {
                this.SPCChart.Chart.Tools.Add(mkTip);
            }
        }



    
        public Histogram AddHistogramSeries(string seriesName, VerticalAxis vertAxis, Color seriesColor)
        {
            Steema.TeeChart.Styles.Histogram histogramSeries = new Histogram();
            histogramSeries.VertAxis = vertAxis;
            histogramSeries.XValues.DateTime = this.IsDateTime;
            histogramSeries.Title = seriesName;
            histogramSeries.Marks.Visible = false;
            histogramSeries.ShowInLegend = this.IsShowInLegend;
            histogramSeries.Color = seriesColor;
            histogramSeries.LinePen.Visible = false;
            return histogramSeries;
        }
        
        public Candle AddCandleSeries(string seriesName, VerticalAxis vertAxis, Color seriesColor)
        {
            Steema.TeeChart.Styles.Candle seriesStyle= new Candle();
            seriesStyle.VertAxis = vertAxis;
            seriesStyle.XValues.DateTime = this.IsDateTime;            
            seriesStyle.Title = seriesName;
            seriesStyle.Marks.Visible = false;
            seriesStyle.ShowInLegend = this.IsShowInLegend;
            seriesStyle.LinePen.Visible = false;                                    
            seriesStyle.Color = seriesColor;            
            seriesStyle.CandleWidth = 13;
            seriesStyle.ShowClose = true;
            seriesStyle.ShowOpen = true;
            seriesStyle.HighLowPen.Width = 2;
            seriesStyle.HighLowPen.Color = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(92)))), ((int)(((byte)(153)))));
            seriesStyle.UpCloseColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(155)))), ((int)(((byte)(254)))));
            seriesStyle.DownCloseColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(209)))), ((int)(((byte)(36)))));
            
            return seriesStyle;
        }


        #region ::: Properties

        public DataTable dtDataSource
        {

            get
            { return _dtDataSource;}
            set
            {   this._dtDataSource = value;}

        }

        public BTeeChart Get_Chart()
        {
            return this.SPCChart;
        }


        private string ParentToolTipItemkey = string.Empty;
        public string ToolTipItemkey
        {
            get
            { return ParentToolTipItemkey;}
            set
            {this.ParentToolTipItemkey = value;}
        }

        private string ParentPagekey = "";
        public string Pagekey
        {
            get
            {return ParentPagekey;}
            set
            { this.ParentPagekey = value;}
        }

        private string ParentItemkey = "";
        public string Itemkey
        {
            get
            {  return ParentItemkey; }
            set
            { this.ParentItemkey = value;}
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

        public bool IsViewButtonList
        {
            set { this.bButtonList1.Visible = value; }
        }

        public bool IsDateTime
        {
            get { return this._isDateTime; }
            set { this._isDateTime = value; }
        }
        
        public Steema.TeeChart.LegendAlignments LegendPosition
        {
            get { return this.SPCChart.LegendPosition; }
            set { this.SPCChart.LegendPosition = value; }
        }

        public bool IsShowInLegend
        {
            get { return this._isShowInLegend; }
            set { this._isShowInLegend = value; }
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

       
        

        #endregion
        
    }
}