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

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    public partial class BaseAnalysisChart : BasePageUCtrl
    {
        
        private  MultiLanguageHandler _mlthandler;
        private SourceDataManager _dataManager = null;
        private Initialization _Initialization;
        private ChartUtility _chartUtil;
        private DataTable _dtDataSource = null;
       
        ArrayList _arrSeriesColumn = new ArrayList() ;
        LinkedList _llstSortingKey;   
            
        SeriesToolTip _stt = new SeriesToolTip();
        SortedList _slButtonList;

        string _legendLocation = Definition.Right;
        string _AxisDateTimeFormat = Definition.DATETIME_FORMAT_yyMMdd;
        string _chartType = Definition.CHART_TYPE.RUN;
        string _changeXLabelColumn =string.Empty;
        string _xLabelName = string.Empty;
        string _xSotingKey = string.Empty;

        bool _bRegular = false;
        bool _bSeriesAllCheck = true;
        bool _IsVisibleLegendScrollBar = false;
        bool _IsVisibleShadow = false;
        bool _isDateTime = false;
        bool _isShowInLegend = true;
        bool _isViewButtonList = false;

        System.Windows.Forms.Panel pnlParent = null;
        Steema.TeeChart.Tools.MarksTip mkTip = null;

        private ArrayList SeriesColor = new ArrayList();
        private ArrayList LinePointerStyle = new ArrayList();
        private Steema.TeeChart.Functions.HistogramFunction histogramFunction1;

        public BaseAnalysisChart()
        {
            InitializeComponent();
            InitializeVariable();
        }


        public void InitializeVariable()
        {
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();            
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._chartUtil = new ChartUtility();
            this.AnalysisChart.ClearChart();  
            this.InitSeriesColor();              
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
        

        public  void InitializeChart()
        {
            if (mkTip == null)
            {
                mkTip = new Steema.TeeChart.Tools.MarksTip();
                mkTip.Style = MarksStyles.Value;
            }

            if (this.AnalysisChart.Chart.Tools.IndexOf(mkTip) < 0)            
                this.AnalysisChart.Chart.Tools.Add(mkTip);
                
            this.InitializeDataButton();                    
            this.bButtonList1.ButtonClick += new ButtonList.ImageDelegate(bButtonList1_ButtonClick);            
            
            this.AnalysisChart.Chart.Legend.CheckBoxes = true;
            this.AnalysisChart.IsVisibleLegendScrollBar = this.IsVisibleLegendScrollBar;
            this.AnalysisChart.IsVisibleShadow = this.IsVisibleShadow;
            this.AnalysisChart.AxisDateTimeFormat = this.AxisDateTimeFormat;
            this.AnalysisChart.IsUseAutomaticAxisOfTeeChart = true;
            this.AnalysisChart.IsUseChartEditor = false;
            this.AnalysisChart.LegendPosition = Steema.TeeChart.LegendAlignments.Right;            
            this.AnalysisChart.LegendVisible = true;
                        
            this.AnalysisChart.Chart.Axes.DrawBehind = true;
            this.AnalysisChart.Chart.Axes.Left.Grid.Visible = false;
            this.AnalysisChart.Chart.Axes.Right.Grid.Visible = false;
            this.AnalysisChart.Chart.Axes.Top.Grid.Visible = false;
            this.AnalysisChart.Chart.Axes.Bottom.Grid.Visible = false;            
            this.AnalysisChart.SetXAxisRegularInterval(true);
            this.AnalysisChart.Chart.Axes.Bottom.MinorTickCount=0;

            
            this.bButtonList1.Visible = this.IsViewButtonList;
            

            

            if (ParentPagekey != "")
            {
                LinkedList slToolTipList = this._Initialization.InitializeToolTipList(this.ParentPagekey, ParentToolTipItemkey, this.sessionData);

                for (int i = 0; i < slToolTipList.Count; i++)
                {
                    string sLabel = slToolTipList.GetKey(i).ToString();
                    string sFieldName = slToolTipList.GetValue(i).ToString();

                    this._stt.Add(new SeriesToolTipOneRowInfo(sLabel, new string[] { sFieldName }));
                }
                this.AnalysisChart.DefaultToolTipInfo = this._stt;
            }
        }
   
        public  bool DrawAnalysisChart()
        {
            try
            {
                if (this.dtDataSource.Rows.Count < 1)
                {                    
                    return false;
                }

                if (CHART_TYPE == Definition.CHART_TYPE.HISTOGRAM)
                    this.DrawChartWithSeriesInfoHist();
                else if (CHART_TYPE == Definition.CHART_TYPE.DOT_PLOT)
                    this.DrawChartWithSeriesInfoDot();                                           
                else if(CHART_TYPE == Definition.CHART_TYPE.BOX)
                    this.DrawChartWithSeriesInfoBox();
                else if (CHART_TYPE == Definition.CHART_TYPE.RUN)
                {
                    this.DrawChartWithSeriesInfo(null, this.dtDataSource, 0);
                    this.ChangeSeriesStyle();
                    this.MakeTipInfo();                                                             
                }
                //else
                //{                    
                //    DataTable dt = null;
                //    for (int i = 0; i < _llstSortingKey.Count; i++)
                //    {
                //        string sGroupName = _llstSortingKey.GetKey(i).ToString();
                //        string sColumn = _llstSortingKey.GetValue(i).ToString();
                //        dt = DataUtil.DataTableImportRow(this.dtDataSource.Select(string.Format("{0} = '{1}'", sColumn, sGroupName)));
                //        DrawChartWithSeriesInfo(sGroupName, dt, i);
                //    }      
         
                //    this.ChangeSeriesStyle();
                //    this.MakeTipInfo();
                //}                                                                        
         
                return true;
            }
            catch (Exception ex)
            {
                MsgClose();
                this.AnalysisChart.ClearChart();
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                //MSGHandler.DisplayMessage(MSGType.Error, "There is an empty or non-numeric value.");
                return false;
            }
        }
                
        public  void ChangeSeriesStyle()
        {
            Series s = null;            
            string sGropName = string.Empty;        
            SortedList _slSeries = new SortedList();
            int iCount =0;
            for (int j = 0; j < this.AnalysisChart.Chart.Series.Count; j++)
            {
                s = this.AnalysisChart.Chart.Series[j];
                string [] arr = s.Title.Split('_');
                if (!_slSeries.Contains(arr[1]))
                {
                    _slSeries.Add(arr[1], iCount);
                    iCount++;
                }                                   
                ((Line)s).OutLine.Visible = false;
                ((Line)s).ColorEachLine = false;
                ((Line)s).LinePen.Visible = false;
                ((Line)s).Pointer.HorizSize = 3;
                ((Line)s).Pointer.VertSize = 3;
                ((Line)s).Pointer.Style = this.GetLinePointerStyle((int)_slSeries[arr[1]]);                                                  
            }
                  
            this.AnalysisChart.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
            this.AnalysisChart.ChangeXLabelColumnDefault(ChangeXLabelColumn);            
            this.AnalysisChart.Refresh();
        }

        protected  void MakeTipInfo()
        {
            Steema.TeeChart.Tools.MarksTip mkTip = new Steema.TeeChart.Tools.MarksTip();
            mkTip.Style = MarksStyles.Value;
            this.AnalysisChart.Chart.Tools.Add(mkTip);                        
        }


    
        protected void DrawChartWithSeriesInfo(string sGroupName, DataTable _dtDataSource, int iColor)
        {
            SeriesInfo si = null;
            SortedList _slSeries = new SortedList();    
            int iCount =  iColor;    
            for (int i = 0; i < _dtDataSource.Columns.Count; i++)
            {
                string seriesName = _dtDataSource.Columns[i].ColumnName;
                if (CHART_TYPE == Definition.CHART_TYPE.RUN)
                {                    
                    if (!this.ArraySeriesColumn.Contains(seriesName)) continue;
                    si = new SeriesInfo(typeof(Line), seriesName, _dtDataSource, this.xLabelName, seriesName);

                    string[] arr = seriesName.Split('_');                    
                    if(!_slSeries.Contains(arr[0]))
                    {
                        _slSeries.Add(arr[0], iCount);
                        iCount++;      
                    }                        
                    sGroupName = arr[0];                               
                    si.Group = sGroupName;
                    si.SeriesColor = this.GetSeriesColor((int)_slSeries[arr[0]]); 
                }              
                this.AnalysisChart.SetActiveGroup(si.Group);
                si.SeriesName = seriesName;                                               
                this.DrawChart(si);
               
            }                                    
        }

        protected void DrawChartWithSeriesInfoBox()
        {

            Steema.TeeChart.Axis axis = null;
            Steema.TeeChart.Styles.Box s;
            DataTable dt = null;
            List<double> lstValue;
            this.AnalysisChart.Chart.Series.Clear();
            this.AnalysisChart.SetXAxisRegularInterval(true);
            
            for (int i = 0; i < _llstSortingKey.Count; i++)
            {
                string strSortingValue = _llstSortingKey.GetKey(i).ToString();
                string strSortingKey = _llstSortingKey.GetValue(i).ToString();
                Color c = GetSeriesColor(i);

                s = new Box();
                s.Color = c;
                s.Title = strSortingValue;
                s.XValues.Name = strSortingValue;                               
                this.AnalysisChart.Chart.Series.Add(s);
                lstValue = new List<double>();
                dt = DataUtil.DataTableImportRow(this.dtDataSource.Select(string.Format("{0} = '{1}'", strSortingKey, strSortingValue)));                
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(dr[Definition.CHART_COLUMN.VALUE].ToString())) continue;
                    lstValue.Add((double)dr[Definition.CHART_COLUMN.VALUE]);
                   // s.Add(i, (double)dr[Definition.CHART_COLUMN.VALUE], c);
                }
                s.Add(lstValue.ToArray());              
               s.ReconstructFromData();
                if (i == 0)
                {
                    axis = this.AnalysisChart.Chart.Axes.Bottom;
                    axis.Horizontal = true;
                    axis.StartPosition = 0;
                    axis.EndPosition = 100 / _llstSortingKey.Count;
                    axis.AxisPen.Color = c;
                    axis.Title.Font.Color = c;
                    axis.Title.Font.Bold = true;
                    axis.Title.Text = strSortingValue;                    
                    axis.LogarithmicBase = 2;
                    axis.Grid.Visible = false;
                    axis.AxisPen.Visible = false;       
                }
                else
                {
                    axis = new Axis(true, false, this.AnalysisChart.Chart.Chart);
                    this.AnalysisChart.Chart.Axes.Custom.Add(axis);
                    s.CustomHorizAxis = axis;
                    axis.StartPosition = 100 / _llstSortingKey.Count * i;
                    axis.EndPosition = 100 / _llstSortingKey.Count * (i + 1);
                    axis.AxisPen.Color = c;
                    axis.Title.Font.Color = c;
                    axis.Title.Font.Bold = true;
                    axis.Title.Text = strSortingValue;
                    axis.LogarithmicBase = 2;                    
                    axis.Grid.Visible=false;                    
                    axis.AxisPen.Visible =false;                                      
                }
                axis.Labels.Angle = 90;
                axis.Title.Angle = 90;
            }
            this.AnalysisChart.Chart.Axes.Bottom.Labels.Angle = 90;   
            this.AnalysisChart.Chart.Axes.Left.MinorTickCount = 0;                
            //this.AnalysisChart.Chart.Axes.Left.MinAxisIncrement = .25;
            //this.AnalysisChart.Chart.Axes.Left.SetMinMax(0,5);          
        }

        protected void DrawChartWithSeriesInfoHist()
        {
            Steema.TeeChart.Axis axis = null;
            Steema.TeeChart.Styles.Histogram s;            
            DataTable dt = null;
            this.AnalysisChart.Chart.Series.Clear();
            this.AnalysisChart.SetXAxisRegularInterval(true);

            int iOldEnd = 0;
            for (int i = 0; i < _llstSortingKey.Count; i++)
            {
                int iEnd = 0;
                double min = 0;
                double max = 0;                                     
                string strSortingValue = _llstSortingKey.GetKey(i).ToString();
                string strSortingKey = _llstSortingKey.GetValue(i).ToString();
                Color c = GetSeriesColor(i);
                s = new Histogram();
                s.Color = c;
                s.Title = strSortingValue;
                s.XValues.Name = Definition.CHART_COLUMN.VALUE;
                                                
                this.AnalysisChart.Chart.Series.Add(s);                                
                foreach (DataRow dr in this.dtDataSource.Rows)
                {
                    string sSortingKey = dr[strSortingKey].ToString();
                    int dCount = int.Parse(dr[Definition.CHART_COLUMN.SAMPLE_COUNT].ToString());
                    int iIndex = int.Parse(dr[CommonChart.COLUMN_NAME_SEQ_INDEX].ToString());
                    double dData = double.Parse(dr[Definition.CHART_COLUMN.VALUE].ToString());

                    
                    if (sSortingKey.Equals(strSortingValue))
                    {
                        if (dCount > iEnd) iEnd = dCount;
                        s.Add(iIndex, dCount, c);
                        if (dData > min)
                            max = dData;  
                        else
                            min = dData;                                                                    
                    }
                    else
                    {
                        s.Add(iIndex, -999, c);
                       
                    }

                }
                for (int index = 0; index < this.dtDataSource.Rows.Count; index++)
                    s.Labels[index] = this.dtDataSource.Rows[index][Definition.CHART_COLUMN.VALUE].ToString();                
                iOldEnd = iEnd;        
            }            
            this.AnalysisChart.Chart.Axes.Left.Maximum = iOldEnd;
            this.AnalysisChart.Chart.Axes.Left.Minimum = 0;
            this.AnalysisChart.Chart.Axes.Left.MinAxisIncrement = 1;
            this.AnalysisChart.Chart.Axes.Left.MinorTickCount = 0;                                       
            this.AnalysisChart.Chart.Axes.Left.SetMinMax(0,iOldEnd);
            this.AnalysisChart.Chart.Axes.Left.MinimumOffset= 0;
            this.AnalysisChart.Chart.Axes.Left.MaximumOffset = iOldEnd+1;
            this.AnalysisChart.Chart.Axes.Bottom.MinorTickCount = 0;                                       
        }
        
        protected void DrawChartWithSeriesInfoDot()
        {

            Steema.TeeChart.Axis axis = null;
            Steema.TeeChart.Styles.Points s;
            DataTable dt = null;
            this.AnalysisChart.Chart.Series.Clear();
            int iOldEnd = 0;
            for (int i = 0; i < _llstSortingKey.Count; i++)
            {
                int iEnd = 0;
                string strSortingValue = _llstSortingKey.GetKey(i).ToString();
                string strSortingKey = _llstSortingKey.GetValue(i).ToString();
                Color c = GetSeriesColor(i);
                
                s = new Points();
                this.AnalysisChart.Chart.Series.Add(s);
                               
                s.Color = c;
                s.Pointer.Style = PointerStyles.Circle;
                s.Title = strSortingValue;

                //s.BoxSize =2;
                foreach (DataRow dr in this.dtDataSource.Rows)
                {
                    string sSortingKey = dr[strSortingKey].ToString();
                    int dCount = int.Parse(dr[Definition.CHART_COLUMN.SAMPLE_COUNT].ToString());
                    int iIndex = int.Parse(dr[CommonChart.COLUMN_NAME_SEQ_INDEX].ToString());
                    double dData = double.Parse(dr[Definition.CHART_COLUMN.VALUE].ToString());
                   
                    if (sSortingKey.Equals(strSortingValue))
                    {
                        if (dCount > iEnd) iEnd = dCount;                                                      
                        s.Add(iIndex, dCount, c);                                                           
                    }
                    else
                    {
                        s.Add(iIndex, -999, c);                      
                    }                   
                }
                                           
                if (i == 0)
                {
                    axis = this.AnalysisChart.Chart.Axes.Left;
                    axis.StartPosition = 0;
                    axis.EndPosition = 100 / _llstSortingKey.Count;                   
                    axis.Minimum = 0;
                    axis.Maximum = iEnd;
                    axis.SetMinMax(0, iEnd * 2);                                       
                }
                else
                {
                    axis = new Axis(false, false, this.AnalysisChart.Chart.Chart);
                    this.AnalysisChart.Chart.Axes.Custom.Add(axis);
                    s.CustomVertAxis = axis;
                    axis.StartPosition = 100 / _llstSortingKey.Count * i;
                    axis.EndPosition = 100 / _llstSortingKey.Count * (i + 1);                    
                    axis.Minimum = iOldEnd;
                    axis.Maximum = (iOldEnd + iEnd);
                    axis.SetMinMax(0, iEnd * 2);                                                             
                }

                axis.AxisPen.Color = c;
                axis.Title.Font.Color = c;
                axis.Title.Font.Bold = true;
                axis.Title.Text = strSortingValue;
                axis.Title.Angle = 90;
                axis.Grid.Visible = false;
                axis.AxisPen.Visible = false;
                axis.MinAxisIncrement =1;                
                                          
                iOldEnd = iEnd;
                for (int index = 0; index < this.dtDataSource.Rows.Count; index++)
                {
                    s.Labels[index] = this.dtDataSource.Rows[index][Definition.CHART_COLUMN.VALUE].ToString();
                }
  
            }
                                    
            this.AnalysisChart.Chart.Axes.Bottom.Labels.Angle = 90;                    
            this.AnalysisChart.Chart.Invalidate();
        }

        public void DrawChart(SeriesInfo Series)
        {
            this.AnalysisChart.AddSeries(Series);            
        
            if (mkTip == null)
            {
                mkTip = new Steema.TeeChart.Tools.MarksTip();
                mkTip.Style = MarksStyles.Value;
            }

            if (this.AnalysisChart.Chart.Tools.IndexOf(mkTip) < 0)
            {
                this.AnalysisChart.Chart.Tools.Add(mkTip);
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
            Steema.TeeChart.Styles.Candle seriesStyle = new Candle();
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

        #region ::: Events
        protected void bButtonList1_ButtonClick(string name)
        {
            if (this.AnalysisChart.Chart.Series.Count < 1)
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
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
            finally
            {

            }
        }


        #region Button Function
        private void Click_ZOOM_IN_Button()
        {
            this.AnalysisChart.ClickAction = BTeeChart.ChartClickAction.DEFAULT;
            ZoomPopup Zoom = new ZoomPopup();
            Zoom.SPC_CHART_TYPE = SPC_CHART_TYPE.ANALYSISCHART;
            Zoom.BaseAnalysisChart = this;
            Zoom.ShowDialog();
        }

        private void Click_CHANGE_X_AXIS_Button()
        {           
            ChangeXAxisPopup2 pop = new ChangeXAxisPopup2();           
            pop.BaseAnalysisChart = this;
            pop.SPC_CHART_TYPE = SPC_CHART_TYPE.ANALYSISCHART;                                    
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
                
                if (sColumn.Equals("TIME"))
                {
                    //this.AnalysisChart.AxisDateTimeFormat = this.AxisDateTimeFormat;                    
                    this.AnalysisChart.ChangeXLabelColumnDefault(Definition.CHART_COLUMN.TIME2);
                }
                else if (sColumn.Equals("LOT_ID"))
                {
                    this.AnalysisChart.ChangeXLabelColumnDefault(Definition.CHART_COLUMN.LOT_ID);
                }                
            }
            else
            {
                return;
            }


        }

        private void Click_TOOL_TIP_COMMAND_Button()
        {
            this.AnalysisChart.AddMenuOfSeries(BTeeChart.ChartMenuItem.ADD_COMMENT_TO_TOOLTIP, "Add Comment");
        }

        private void Click_SNAP_SHOT_Button()
        {
            this.AnalysisChart.ClickAction = BTeeChart.ChartClickAction.SNAPSHOT;
        }

        private void Click_LEGEND_VISIBLITY_Button()
        {
            this.AnalysisChart.LegendVisible = !this.AnalysisChart.LegendVisible;
        }

        private void Click_LEGEND_ALL_CHECK_Button()
        {
            this._bSeriesAllCheck = !this._bSeriesAllCheck;
            this.AnalysisChart.SetSeriesAllCheck(this._bSeriesAllCheck);
        }

        public void Click_SERIES_SHIFT_HORI_Button()
        {
            this.AnalysisChart.ClickAction = BTeeChart.ChartClickAction.SERIES_SHIFT_HORI;
        }

        public void Click_SERIES_SHIFT_VERT_Button()
        {
            this.AnalysisChart.ClickAction = BTeeChart.ChartClickAction.SERIES_SHIFT_VERT;
        }

        public void Click_SAME_START_POINT_Button()
        {
            this.AnalysisChart.ShiftEqualStartSeries(BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.ChartAction.ShiftChartClickAction.ShiftDirectionAxis.BOTH);
        }


        public void Click_INITAILIZE_Button()
        {
            if (this.dtDataSource != null)
            {
                this.ClearChart();
                this.DrawAnalysisChart();
            }
        }

        public void ClearChart()
        {
            this.AnalysisChart.ClearChart();
            this._dtDataSource = null;
        }


        public void Click_CLEAR_SHIFT_Button()
        {
            this.AnalysisChart.ClearShift();
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
                this.bSpread1_Sheet1.SheetName = this.Name;
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
            this.AnalysisChart.SetXAxisRegularInterval(this._bRegular);
        }

        private void Click_POINT_MARKING_Button()
        {
        }

        private void Click_VARIATION_Button()
        {
            this.AnalysisChart.ClickAction = BTeeChart.ChartClickAction.DELTA;
        }

        private void Click_REALTIME_Button()
        {
            int interval = 1000;
            int period = 60;
            int saveCount = 50;
            this.AnalysisChart.RealTimeStart(interval, period, saveCount);
        }

        private void Click_MULTI_TOOL_TIP_Button()
        {
            this.AnalysisChart.IsMultiToolTip = !this.AnalysisChart.IsMultiToolTip;
        }

        private void Click_COPY_CHART_Button()
        {
            this.AnalysisChart.ChartCopyToClipboard();
        }

        private void Click_CHANGE_SERIES_STYLE_Button()
        {
            if (this.AnalysisChart.Chart.Series != null)
            {
                int i = 0;
                int j = 0;

                switch (i)
                {
                    case 0:
                        this.AnalysisChart.ChangeAllSeriesStyle(typeof(Steema.TeeChart.Styles.Line));
                        j++;
                        i = j % 2;
                        break;
                    case 1:
                        this.AnalysisChart.ChangeAllSeriesStyle(typeof(Steema.TeeChart.Styles.Points));
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
                this.AnalysisChart.LegendPosition = Steema.TeeChart.LegendAlignments.Bottom;
                this._legendLocation = Definition.Bottom;
            }
            else if (this._legendLocation == Definition.Bottom)
            {
                this.AnalysisChart.LegendPosition = Steema.TeeChart.LegendAlignments.Left;
                this._legendLocation = Definition.Left;
            }
            else if (this._legendLocation == Definition.Left)
            {
                this.AnalysisChart.LegendPosition = Steema.TeeChart.LegendAlignments.Top;
                this._legendLocation = Definition.Top;
            }
            else
            {
                this.AnalysisChart.LegendPosition = Steema.TeeChart.LegendAlignments.Right;
                this._legendLocation = Definition.Right;
            }
        }
        #endregion
        #endregion

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
            return this.AnalysisChart;
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

        public BTeeChart AnalysisChart
        {
            get { return this.bTeeChart1; }
        }

        public bool IsViewButtonList
        {
            get { return this._isViewButtonList ; }
            set { this._isViewButtonList= value; }
        }

        public string xLabelName
        {
            get { return this._xLabelName; }
            set { this._xLabelName = value; }
        }
        
        
        public bool IsDateTime
        {
            get { return this._isDateTime; }
            set { this._isDateTime = value; }
        }
        
        public Steema.TeeChart.LegendAlignments LegendPosition
        {
            get { return this.AnalysisChart.LegendPosition; }
            set { this.AnalysisChart.LegendPosition = value; }
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
        
        public string  CHART_TYPE
        {
            get { return this._chartType; }
            set { this._chartType = value; }
        }



        public ArrayList ArraySeriesColumn
        {
            get { return this._arrSeriesColumn; }
            set { this._arrSeriesColumn = value; }
        }

        public LinkedList llstSortingKey
        {
            get { return this._llstSortingKey; }
            set { this._llstSortingKey = value; }
        }
        
        

        public string ChangeXLabelColumn
        {
            get { return this._changeXLabelColumn; }
            set { this._changeXLabelColumn = value; }
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
                Count = this.AnalysisChart.Chart.Series.Count;

                for (int i = 0; i < Count; i++)
                {
                    if (SeriesName == this.AnalysisChart.Chart.Series[i].Title)
                    {
                        this.AnalysisChart.Chart.Series[i].VertAxis = Steema.TeeChart.Styles.VerticalAxis.Right;
                        this.AnalysisChart.ResetAllAxis();
                        this.AnalysisChart.Chart.Axes.Right.Visible = true;
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
                Count = this.AnalysisChart.Chart.Series.Count;

                for (int i = 0; i < Count; i++)
                {
                    if (SeriesName == this.AnalysisChart.Chart.Series[i].Title)
                    {
                        this.AnalysisChart.Chart.Series[i].VertAxis = Steema.TeeChart.Styles.VerticalAxis.Left;
                        this.AnalysisChart.ResetAllAxis();
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
            this.AnalysisChart.DefaultToolTipInfo = this._stt;
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

       
        private bool IsPointerVisible()
        {
            if (_dataManager.RawDataTable.Rows.Count == 1)
                return true;
            else
                return false;
        }

        #endregion 
        
        
        public Color GetSeriesColor(int index)
        {
            int SeriesIdx =  index % SeriesColor.Count;
            return (Color)SeriesColor[SeriesIdx];
        }


        public PointerStyles GetLinePointerStyle(int index)
        {

            int SeriesIdx = 0;
            if(index>0)
            SeriesIdx = index % LinePointerStyle.Count;
            return (PointerStyles)LinePointerStyle[SeriesIdx];
        }
        
        public void InitSeriesColor()
        {
        
            SeriesColor.Add(Color.Blue);            
            SeriesColor.Add(Color.Orange);            
            SeriesColor.Add(Color.PaleGreen);                        
            SeriesColor.Add(Color.DarkViolet);
            SeriesColor.Add(Color.DarkKhaki);
                        
            SeriesColor.Add(Color.Lime);                        
            SeriesColor.Add(Color.Yellow);
            SeriesColor.Add(Color.DeepPink);
            SeriesColor.Add(Color.Green);                        
            SeriesColor.Add(Color.Violet);            
            SeriesColor.Add(Color.Goldenrod);
            SeriesColor.Add(Color.LightCoral);
            SeriesColor.Add(Color.Sienna);
            SeriesColor.Add(Color.Purple);
            SeriesColor.Add(Color.Red);
            SeriesColor.Add(Color.SkyBlue);
            SeriesColor.Add(Color.Black);            
                                    
            SeriesColor.Add(Color.Aqua);            
            SeriesColor.Add(Color.Crimson);
            SeriesColor.Add(Color.MidnightBlue);
            SeriesColor.Add(Color.YellowGreen);
            SeriesColor.Add(Color.Magenta);            
            SeriesColor.Add(Color.SlateGray);
            SeriesColor.Add(Color.LemonChiffon);                        
            SeriesColor.Add(Color.PowderBlue);
            SeriesColor.Add(Color.DarkGoldenrod);
            SeriesColor.Add(Color.GreenYellow);
            
            SeriesColor.Add(Color.MediumSeaGreen);            
            SeriesColor.Add(Color.SlateBlue);
            SeriesColor.Add(Color.Pink);                        
            SeriesColor.Add(Color.MediumTurquoise);
            SeriesColor.Add(Color.SteelBlue);
                        
            SeriesColor.Add(Color.Gray);
            SeriesColor.Add(Color.HotPink);
            SeriesColor.Add(Color.IndianRed);
            SeriesColor.Add(Color.LightSalmon);


            LinePointerStyle.Add(PointerStyles.Circle);
            LinePointerStyle.Add(PointerStyles.Diamond);
            LinePointerStyle.Add(PointerStyles.Rectangle);
            LinePointerStyle.Add(PointerStyles.Star);
            LinePointerStyle.Add(PointerStyles.Triangle);
            LinePointerStyle.Add(PointerStyles.LeftTriangle);
            LinePointerStyle.Add(PointerStyles.RightTriangle);            
            LinePointerStyle.Add(PointerStyles.DownTriangle);       
            LinePointerStyle.Add(PointerStyles.Sphere);                        
            LinePointerStyle.Add(PointerStyles.Cross);
            LinePointerStyle.Add(PointerStyles.DiagCross);            
            LinePointerStyle.Add(PointerStyles.SmallDot);            
            LinePointerStyle.Add(PointerStyles.Hexagon);
            LinePointerStyle.Add(PointerStyles.PolishedSphere);            
         }               
    }
}