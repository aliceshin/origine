using System;
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
    public partial class BaseSPCChart : BasePageUCtrl
    {             
        protected Color COLOR_SERIES_SYNC = Color.Gold;
        protected SourceDataManager _dataManager = null;
        protected DataTable _dtDataSource = null;
        protected List<string> lstRawColumn = null;        
        protected LinkedList _llstChartSeriesVisibleType = null;
       
        
        protected System.Windows.Forms.Panel pnlParent= null;

        Steema.TeeChart.Tools.MarksTip mkTip = null;
        string _legendLocation = Definition.Right;
        protected string _AxisDateTimeFormat = "yyyy-MM-dd";

        bool _bRegular = false;
        bool _bSeriesAllCheck = true;

        protected string _xLabel="INDEX";
        protected string _Group =null;

        public BaseSPCChart()
        {
            InitializeComponent();

            this.pnlPCA.Visible = false;

            InitializeChart();

            this.bTitlePanel1.MaxResizable = true;

            //this.bButtonList1.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(bButtonList1_ButtonClick);

            this.bTitlePanel1.MouseDown += new MouseEventHandler(bTitlePanel1_MouseDown);
            this.bTitlePanel1.MouseMove += new MouseEventHandler(bTitlePanel1_MouseMove);
            this.bTitlePanel1.MouseUp += new MouseEventHandler(bTitlePanel1_MouseUp);
            this.bTitlePanel1.MouseDoubleClick += new MouseEventHandler(bTitlePanel1_MouseDoubleClick);
            this.bTitlePanel1.ClosedPanel += new EventHandler(bTitlePanel1_ClosedPanel);
        }


        #region PageLoad & Initialize.
        public void InitializeChart()
        {
            //this.bTeeChart1.ClickMenuOfBackground += new BTeeChart.ClickMenuOfBackgroundEventHandler(bTeeChart1_ClickMenuOfBackground);

            this.SPCChart.HeaderText = "test";
            //this.lblGroupName.Visible = false;

            //this.GenerateCommandButtonList();

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
        #endregion 





        #region ::: Property
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

        public Panel ParentControl
        {
            get { return this.pnlParent; }
            set { this.pnlParent = value; }
        }


        public bool isViewPCAPanel
        {
            set { this.pnlPCA.Visible = value; }
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
                    MSGHandler.DisplayMessage(MSGType.Warning, MultiLanguageHandler.getInstance().GetMessage("VMS_INFO_065"));
                    return false;
                }

                ClearChart();
                InitChart();
                MakeDataSourceToDrawSPCChart();
                DrawChartWithSeriesInfo();
                ChangeSeriesStyle();              
                MakeTipInfo();

                
                return true;
            }
            catch
            {
                MsgClose();

                this.SPCChart.ClearChart();
                MSGHandler.DisplayMessage(MSGType.Error, "There is an empty or non-numeric value.");
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


        protected virtual void ClearChart()
        {
            this.SPCChart.ClearChart();
            this._dtDataSource = null;
        }


        protected virtual void InitChart()
        {        
           this.SPCChart.Chart.Legend.CheckBoxes=false;           
           this.SPCChart.IsVisibleLegendScrollBar = false;           
           this.SPCChart.IsVisibleShadow = true;
           this.SPCChart.AxisDateTimeFormat = AxisDateTimeFormat;
           this.SPCChart.LegendPosition = Steema.TeeChart.LegendAlignments.Right;           
           this.SPCChartTitlePanel.MaxResizable = false;                                
        }

        protected virtual void MakeDataSourceToDrawSPCChart()
        {
            return;
        }

        protected virtual void MakeDataSourceToDrawSPCChart(DataRow groupInfo)
        {
            return;
        }
                
        private bool   GetSeriesVisibleType(string _key)
        {           
            bool bRtn = true;
            for (int j = 0; j < llstChartSeriesVisibleType.Count; j++)
            {
                string strKey= llstChartSeriesVisibleType.GetKey(j).ToString();
                if (_key.Equals(strKey))
                {
                    bRtn = (bool)llstChartSeriesVisibleType.GetValue(j);
                    break;
                }            
            }
            
            return bRtn;  
        }           

        protected virtual void DrawChartWithSeriesInfo()
        {
            //SeriesInfo si = null;
            //ChartUtility chartUtil = new ChartUtility();

            //for (int i = 0; i < this._dtDataSource.Columns.Count; i++)
            //{
            //    string seriesName = this._dtDataSource.Columns[i].ColumnName;
                  
            //    if (seriesName == CommonChart.COLUMN_NAME_SEQ_INDEX || seriesName == Definition.CHART_COLUMN.TIME)
            //    {
            //        continue;
            //    }
           
            //    si = new SeriesInfo(typeof(Line), this._dtDataSource, this._xLabel, seriesName);
           
            //    si.SeriesName = seriesName;                  
            //    si.Group="All";                                                                                                                               
            //    switch (seriesName)
            //    {
            //        case Definition.CHART_COLUMN.UCL: 
            //             si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.UCL);                         
            //            break;
            //        case Definition.CHART_COLUMN.LCL:
            //             si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LCL);                         
            //            break;
            //        case Definition.CHART_COLUMN.CL:
            //             si.SeriesColor =chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.CL);
            //            break;
            //        case Definition.CHART_COLUMN.LSL:
            //             si.SeriesColor =chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LSL);                        
            //            break;
            //        case Definition.CHART_COLUMN.USL:
            //            si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.USL);                       
            //            break;                                          
            //        case Definition.CHART_COLUMN.MIN:
            //            si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.MIN);                                            
            //            break;

            //        case Definition.CHART_COLUMN.MAX:
            //            si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.MAX);                     
            //            break;

            //        case Definition.CHART_COLUMN.MEAN:
            //            si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.AVG);                                                
            //            break;      
                                                                                                                
            //        default:
            //            si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.COUNT + i);                                                               
            //            break;
                                                
            //    }
                                                            
            //   this.DrawChart(si);
            //}
        }

        public virtual void ChangeSeriesStyle()
        {
           // ChartUtility chartUtil =new ChartUtility();          
           // Series s = null;
           //for(int i=0;i<this.SPCChart.Chart.Series.Count; i++){              
           //      s = this.SPCChart.Chart.Series[i];                                                                                                    
           //      if(s.Title==Definition.CHART_COLUMN.UCL || s.Title==Definition.CHART_COLUMN.LCL)
           //      {
           //         ((Line)s).LinePen.Width = 2;
           //         ((Line)s).Active= GetSeriesVisibleType(Definition.CHART_SERIES_VISIBLE_TYPE.Control_Limit);
           //         ((Line)s).Pointer.Style = PointerStyles.Circle;
           //      }                                                                              
           //      else if(s.Title==Definition.CHART_COLUMN.CL)
           //      {
           //         ((Line)s).LinePen.Width = 2;
           //         ((Line)s).LinePen.Style = System.Drawing.Drawing2D.DashStyle.Dot;
           //         ((Line)s).Pointer.Style = PointerStyles.Circle;                                         
           //      }
           //      else if (s.Title == Definition.CHART_COLUMN.LSL || s.Title == Definition.CHART_COLUMN.USL)
           //      {
           //         ((Line)s).LinePen.Width = 2;  
           //         ((Line)s).Active= GetSeriesVisibleType(Definition.CHART_SERIES_VISIBLE_TYPE.SPEC);
           //         ((Line)s).Pointer.Style = PointerStyles.Rectangle;          
           //      }               
           //      else if(s.Title==Definition.CHART_COLUMN.MEAN                         
           //      )
           //      {
           //         ((Line)s).LinePen.Width = 2;
           //         ((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES_VISIBLE_TYPE.Average);
           //         ((Line)s).Pointer.Style = PointerStyles.Star;                           
           //      }
           //      else if (s.Title == Definition.CHART_COLUMN.RANGE
           //              || s.Title == Definition.CHART_COLUMN.STDDEV
           //              || s.Title == Definition.CHART_COLUMN.EWMAMEAN
           //              || s.Title == Definition.CHART_COLUMN.EWMARANGE
           //              || s.Title == Definition.CHART_COLUMN.MA
           //              || s.Title == Definition.CHART_COLUMN.MSD )
           //     {
           //         ((Line)s).LinePen.Width = 2;
           //         ((Line)s).Active = true;
           //         ((Line)s).Pointer.Style = PointerStyles.Star;          
           //     }
           //     else if (s.Title == Definition.CHART_COLUMN.TARGET)
           //     {
           //         ((Line)s).LinePen.Visible = false;
           //         ((Line)s).Active = true;
           //         ((Line)s).Pointer.Style = PointerStyles.Triangle;          
           //     }
           //     else 
           //     {
           //          ((Line)s).LinePen.Visible = false;                                                       
           //          if (s.Title == Definition.CHART_COLUMN.MIN || s.Title == Definition.CHART_COLUMN.MAX)
           //          {
           //              ((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES_VISIBLE_TYPE.Min_Max);
           //              ((Line)s).Pointer.Style = PointerStyles.Diamond;            
           //          }else
           //          {                    
           //             //Point 인경우
           //             if(lstRawColumn.Count==1)
           //             {
           //                 ((Line)s).Active = true;
           //                 ((Line)s).Pointer.Style = PointerStyles.Cross;      
                            
           //             }else{
           //                 for (int j = 0; j < lstRawColumn.Count; j++)
           //                 {
           //                     ((Line)s).Active = GetSeriesVisibleType(Definition.CHART_SERIES_VISIBLE_TYPE.Ponint);
           //                     ((Line)s).Pointer.Style = PointerStyles.Cross;                                               
           //                 }
           //             }
           //          }
           //     }
                          
           // }               

        }

        protected virtual void MakeTipInfo()
        {
            Steema.TeeChart.Tools.MarksTip mkTip = new Steema.TeeChart.Tools.MarksTip();
            mkTip.Style = MarksStyles.Value;
            this.SPCChart.Chart.Tools.Add(mkTip);
                        
        }

        public virtual string[] UsedParameters(Steema.TeeChart.Styles.Series s)
        {
            return null;
        }

        public virtual void DrawCrossLine_WithParametersSEQIndex(string[] parameters, string seqIndex)
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

        void bTitlePanel1_ClosedPanel(object sender, EventArgs e)
        {
            this.Dispose();
        }


        public void bTitlePanel1_MinimizedPanel(object sender, System.EventArgs e)
        {
            BTitlePanel btitpnl= (BTitlePanel)sender;
            int iCount = pnlParent.Controls.Count;
            int iHeight =0;
            if(iCount==1)
            {
                iHeight = this.pnlParent.Height;
            }
            else if(iCount > 1)
            {
               iHeight = this.pnlParent.Height/2;
            }
            foreach (Control chart in pnlParent.Controls)
            {
                if (btitpnl.Name.Equals(chart.Name))
                {
                    chart.Visible = true;
                    chart.Dock = System.Windows.Forms.DockStyle.Top;
                    chart.Height = iHeight;
                }
                else
                {
                    chart.Visible = true;
                    chart.Dock = System.Windows.Forms.DockStyle.Top;
                }
                chart.Height = iHeight;
            }
        }

        public void bTitlePanel1_MaximizedPanel(object sender, System.EventArgs e)
        {
            BTitlePanel btitpnl= (BTitlePanel)sender;
            foreach (Control chart in pnlParent.Controls)
            {
                if (btitpnl.Name.Equals(chart.Name))
                {
                    chart.Visible = true;
                    chart.Dock = System.Windows.Forms.DockStyle.Fill;
                    btitpnl.Dock = System.Windows.Forms.DockStyle.Fill; 
                    btitpnl.Height=this.pnlParent.Height;           
                }else
                {
                    chart.Visible=false;
                    chart.Dock = System.Windows.Forms.DockStyle.None;
                }
               
            }
        }


        #endregion


        ///////////////////////////////////////////////////////////////////////////////////
        //  User Defined Method.
        ///////////////////////////////////////////////////////////////////////////////////


        protected void DrawCrossLine(double xValue, double yValue)
        {
            this.SPCChart.AddColorLineOfHori(yValue, COLOR_SERIES_SYNC);
            this.SPCChart.AddColorLineOfVert(xValue, COLOR_SERIES_SYNC);
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


        #region Button Function
        private void Click_ZOOM_IN_Button()
        {
            this.SPCChart.ClickAction = BTeeChart.ChartClickAction.DEFAULT;
        }

        private void Click_CHANGE_X_AXIS_Button()
        {
            if (this.SPCChart.XAxisLabel == BTeeChart.XAxisLabelType.LABEL)
            {
                this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.VALUE;
            }
            else
            {
                this.SPCChart.XAxisLabel = BTeeChart.XAxisLabelType.LABEL;
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

        private void Click_SERIES_SHIFT_HORI_Button()
        {
            this.SPCChart.ClickAction = BTeeChart.ChartClickAction.SERIES_SHIFT_HORI;
        }

        private void Click_SERIES_SHIFT_VERT_Button()
        {
            this.SPCChart.ClickAction = BTeeChart.ChartClickAction.SERIES_SHIFT_VERT;
        }

        private void Click_SAME_START_POINT_Button()
        {
            this.SPCChart.ShiftEqualStartSeries(BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.ChartAction.ShiftChartClickAction.ShiftDirectionAxis.BOTH);
        }

        private void Click_EXCEL_EXPORT_Button()
        {
            this.SPCChart.ExportDataForExcel(null, true); //@"D:\01.MyDoc\01.Dev\99.tmp\test.xls", true);
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
    }
}
