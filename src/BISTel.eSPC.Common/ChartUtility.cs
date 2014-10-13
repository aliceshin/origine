using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Steema;
using Steema.TeeChart;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;


namespace BISTel.eSPC.Common
{
    public class ChartUtility
    {
        public Color SpecBandColor = System.Drawing.Color.FromArgb(((System.Byte)(127)), ((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));

        private ArrayList SeriesColor = new ArrayList();
        private ArrayList LinePointerStyle = new ArrayList();

        public enum CHAERT_SERIES_COLOR
        {
            LCL=0,
            UCL = 0,
            USL = 1,
            LSL = 1,
            MEAN = 2,                        
            AVG = 3,
            MIN = 4,
            MAX = 5,            
            TARGET = 6,            
            COUNT=7,
            OUTLIERPREVIEW = 4,
            SPECPREVIEW = 5,
            UTL = 36,
            LTL = 36
        }


        public ChartUtility()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
            SeriesColor.Add(Color.Blue);
            SeriesColor.Add(Color.Red);
            SeriesColor.Add(Color.SkyBlue);              
            SeriesColor.Add(Color.Black);            
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
            SeriesColor.Add(Color.Green);
                                    
            LinePointerStyle.Add(PointerStyles.SmallDot);
            LinePointerStyle.Add(PointerStyles.Sphere);
            LinePointerStyle.Add(PointerStyles.Star);
            LinePointerStyle.Add(PointerStyles.Triangle);
            LinePointerStyle.Add(PointerStyles.Circle);
            LinePointerStyle.Add(PointerStyles.Cross);
            LinePointerStyle.Add(PointerStyles.DiagCross);
            LinePointerStyle.Add(PointerStyles.Diamond);
            LinePointerStyle.Add(PointerStyles.DownTriangle);            
            LinePointerStyle.Add(PointerStyles.Hexagon);
            LinePointerStyle.Add(PointerStyles.PolishedSphere);
            LinePointerStyle.Add(PointerStyles.LeftTriangle);
            LinePointerStyle.Add(PointerStyles.RightTriangle);            
            LinePointerStyle.Add(PointerStyles.Rectangle);
        }

        public void ChartClear(Steema.TeeChart.TChart objChart)
        {
            objChart.Panel.Color = System.Drawing.Color.FromArgb(((System.Byte)(243)), ((System.Byte)(251)), ((System.Byte)(239)));//Color.White;
            objChart.Walls.Left.Color = objChart.Panel.Color;//System.Drawing.ColorTranslator.FromHtml("#F3FBEF");
            objChart.Tools.Clear();
            objChart.Axes.Custom.Clear();
            objChart.Series.RemoveAllSeries();

            objChart.Aspect.View3D = false;
            objChart.Header.Visible = false;

            objChart.Axes.Left.Labels.Style = AxisLabelStyle.None;
            objChart.Axes.Right.Labels.Style = AxisLabelStyle.None;
            objChart.Axes.Top.Labels.Style = AxisLabelStyle.None;
            objChart.Axes.Bottom.Labels.Style = AxisLabelStyle.None;

            objChart.Axes.Bottom.Title.Visible = false;

            objChart.Axes.Left.Grid.Visible = false;
            objChart.Axes.Right.Grid.Visible = false;

            objChart.Axes.Left.AxisPen.Visible = false;
            objChart.Axes.Right.AxisPen.Visible = false;
            objChart.Axes.Top.AxisPen.Visible = false;
            objChart.Axes.Bottom.AxisPen.Visible = false;

            //chart.Aspect.ColorPaletteIndex = 1; //excel type
            objChart.Aspect.ColorPaletteIndex = 9; //win xp type
            objChart.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            objChart.Aspect.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            objChart.Zoom.Undo();

        }

        public void ChartClear(Steema.TeeChart.TChart objChart, Color PanelColor)
        {
            ChartClear(objChart);
            objChart.Panel.Color = PanelColor;
        }

        public Color GetSeriesColor(int index)
        {
            int SeriesIdx = index % SeriesColor.Count;
            return (Color)SeriesColor[SeriesIdx];
        }


        public PointerStyles GetLinePointerStyle(int index)
        {
        
            int SeriesIdx = index % LinePointerStyle.Count;
            return (PointerStyles)LinePointerStyle[SeriesIdx];
        }

        public void SetAxisMinMax(Steema.TeeChart.Axis axis, int OffsetMin, int OffsetMax)
        {
            if (axis.Horizontal == true)
            {
                //				axis.Automatic = true;
                //				axis.MinimumOffset = OffsetMin;
                //				axis.MaximumOffset = OffsetMax;

                if (axis.MaxXValue == axis.MinXValue)
                {
                    if (axis.MaxXValue == 0)
                    {
                        SetAxisMinMax(axis, Convert.ToDouble(0), Convert.ToDouble(3));
                    }
                    else
                    {
                        axis.Automatic = true;
                        axis.MinimumOffset = OffsetMin;
                        axis.MaximumOffset = OffsetMax;
                    }
                }
                else
                {
                    axis.Automatic = true;
                    axis.MinimumOffset = OffsetMin;
                    axis.MaximumOffset = OffsetMax;
                }

            }
            else
            {
                if (axis.MaxYValue == axis.MinYValue)
                {
                    if (axis.MaxYValue == 0)
                    {
                        SetAxisMinMax(axis, Convert.ToDouble(-3), Convert.ToDouble(3));
                    }
                    else
                    {
                        axis.Automatic = true;
                        axis.MinimumOffset = OffsetMin;
                        axis.MaximumOffset = OffsetMax;
                    }
                }
                else
                {
                    axis.Automatic = true;
                    axis.MinimumOffset = OffsetMin;
                    axis.MaximumOffset = OffsetMax;
                }
            }
        }

        public void SetAxisMinMax(Steema.TeeChart.Axis axis, double MinValue, double MaxValue)
        {
            axis.Automatic = false;
            axis.SetMinMax(MinValue, MaxValue);
        }

        public void SetAxisMinMax(Steema.TeeChart.Axis axis, DateTime FromDateTime, DateTime ToDateTime)
        {
            axis.Automatic = false;
            axis.SetMinMax(FromDateTime, ToDateTime);
        }

        public void SetAxisTitle(Steema.TeeChart.Axis axis, string Title)
        {
            axis.Title.Text = Title;
            axis.Title.Visible = true;
        }

        public void SetAxisLabel(Steema.TeeChart.Axis axis, Steema.TeeChart.AxisLabelStyle axisLabelStyle)
        {
            axis.Labels.Visible = true;
            axis.Labels.Style = axisLabelStyle;
            axis.Grid.Visible = true;

            //			axis.Labels.Font.Size = 7;
            //			axis.Labels.Font.Name = "Tahoma";
        }

        public void SetAxisLabel(Steema.TeeChart.Axis axis, Steema.TeeChart.AxisLabelStyle axisLabelStyle, bool p_GridVisible)
        {
            axis.Labels.Visible = true;
            axis.Labels.Style = axisLabelStyle;
            axis.Grid.Visible = p_GridVisible;

            //			axis.Labels.Font.Size = 7;
            //			axis.Labels.Font.Name = "Tahoma";
        }

        public void LegendInitialize(Steema.TeeChart.TChart objChart, LegendStyles legendStyle,
            LegendTextStyles legendTextStyle, bool VisibleCheckBox)
        {
            objChart.Legend.Visible = true;
            objChart.Legend.Alignment = LegendAlignments.Right;
            objChart.Legend.LegendStyle = legendStyle;
            objChart.Legend.TextStyle = legendTextStyle;
            objChart.Legend.Font.Name = "Arial";
            objChart.Legend.Font.Size = 8;
            objChart.Legend.Symbol.Pen.Visible = false;
            objChart.Legend.Symbol.Width = 10;
            objChart.Legend.Symbol.WidthUnits = LegendSymbolSize.Pixels;
            objChart.Legend.TopLeftPos = 0;
            objChart.Legend.CheckBoxes = VisibleCheckBox;
            objChart.Legend.Shadow.Visible = false;
            objChart.Legend.Brush.Color = objChart.Panel.Color;
            objChart.Legend.Pen.Visible = false;
            objChart.Legend.Pen.Color = System.Drawing.Color.FromArgb(((System.Byte)(224)), ((System.Byte)(224)), ((System.Byte)(224)));

        }

        public void LegendInitialize(Steema.TeeChart.TChart objChart, LegendStyles legendStyle,
            LegendTextStyles legendTextStyle, bool VisibleCheckBox,
            LegendAlignments legendAlignments, bool VisibleLegendPen)
        {
            objChart.Legend.Visible = true;
            objChart.Legend.Alignment = legendAlignments;
            objChart.Legend.LegendStyle = legendStyle;
            objChart.Legend.TextStyle = legendTextStyle;
            objChart.Legend.Font.Name = "Arial";
            objChart.Legend.Font.Size = 8;
            objChart.Legend.Symbol.Pen.Visible = false;
            objChart.Legend.Symbol.Width = 10;
            objChart.Legend.Symbol.WidthUnits = LegendSymbolSize.Pixels;
            objChart.Legend.TopLeftPos = 0;
            objChart.Legend.CheckBoxes = VisibleCheckBox;
            objChart.Legend.Shadow.Visible = false;
            objChart.Legend.Brush.Color = objChart.Panel.Color;
            objChart.Legend.Pen.Visible = VisibleLegendPen;
            objChart.Legend.Pen.Color = System.Drawing.Color.FromArgb(((System.Byte)(224)), ((System.Byte)(224)), ((System.Byte)(224)));
        }

        public void LegendInitialize(Steema.TeeChart.TChart objChart, LegendStyles legendStyle,
            LegendTextStyles legendTextStyle, bool VisibleCheckBox, LegendAlignments p_legendAlignments)
        {
            objChart.Legend.Visible = true;
            objChart.Legend.Alignment = p_legendAlignments;
            objChart.Legend.LegendStyle = legendStyle;
            objChart.Legend.TextStyle = legendTextStyle;
            objChart.Legend.Font.Name = "Arial";
            objChart.Legend.Font.Size = 8;
            objChart.Legend.Symbol.Pen.Visible = false;
            objChart.Legend.Symbol.Width = 10;
            objChart.Legend.Symbol.WidthUnits = LegendSymbolSize.Pixels;
            objChart.Legend.TopLeftPos = 0;
            objChart.Legend.CheckBoxes = VisibleCheckBox;
            objChart.Legend.Shadow.Visible = false;
            objChart.Legend.Brush.Color = objChart.Panel.Color;
            objChart.Legend.Pen.Visible = false;
            objChart.Legend.Pen.Color = System.Drawing.Color.FromArgb(((System.Byte)(224)), ((System.Byte)(224)), ((System.Byte)(224)));
        }




        public void CopyToClipboard(TChart objChart)
        {
            objChart.Export.Image.GIF.CopyToClipboard();
        }

        public void ChangeSeriesType(TChart objChart)
        {
            int SeriesCount = 0;
            for (int index1 = 0; index1 < objChart.Series.Count; index1++)
            {
                if (objChart.Series[index1].ShowInLegend == true)
                {
                    if (objChart.Series[index1].GetType() == typeof(Line))
                    {
                        if (!((Line)objChart.Series[index1]).Pointer.Visible)	// Line Type의 Point가 Visible False일 경우
                        {
                            ((Line)objChart.Series[index1]).ColorEachLine = false;
                            ((Line)objChart.Series[index1]).Pointer.Visible = true;
                            ((Line)objChart.Series[index1]).Pointer.HorizSize = 2;
                            ((Line)objChart.Series[index1]).Pointer.VertSize = 2;
                            ((Line)objChart.Series[index1]).Pointer.Pen.Color = this.GetSeriesColor(SeriesCount);
                            ((Line)objChart.Series[index1]).Pointer.Pen.Width = 1;
                            ((Line)objChart.Series[index1]).Pointer.Brush.Color = this.GetSeriesColor(SeriesCount);
                        }
                        else
                        {
                            Steema.TeeChart.Styles.Points pointSeries = new Points();
                            Steema.TeeChart.Styles.Series series = objChart.Series[index1];

                            Steema.TeeChart.Styles.Series.ChangeType(ref series, pointSeries.GetType());
                            ((Points)series).Pointer.Visible = true;
                            ((Points)series).Pointer.HorizSize = 2;
                            ((Points)series).Pointer.VertSize = 2;
                            ((Points)series).Pointer.Pen.Color = this.GetSeriesColor(SeriesCount);
                            ((Points)series).Pointer.Pen.Width = 1;
                            ((Points)series).Pointer.Brush.Color = this.GetSeriesColor(SeriesCount);
                        }
                    }
                    else if (objChart.Series[index1].GetType() == typeof(Points))
                    {
                        Steema.TeeChart.Styles.Line lineSeriees = new Line();
                        Steema.TeeChart.Styles.Series series = objChart.Series[index1];

                        VerticalAxis oriaxis = series.VertAxis;

                        Steema.TeeChart.Styles.Series.ChangeType(ref series, lineSeriees.GetType());

                        ((Line)objChart.Series[index1]).Color = GetSeriesColor(SeriesCount);
                        ((Line)objChart.Series[index1]).ColorEachLine = false;
                        ((Line)objChart.Series[index1]).Pointer.Visible = false;
                    }

                    SeriesCount++;
                }
            }
        }

        // Trace Data Chart에 Interlock정보, Glass정보를 나타내기 위한 Custom Axis 추가
        public void AddCustomVertAxis(TChart objChart, Axis p_faultAxis, Axis p_labelAxis)
        {
            // Chart 상측 2% 위치에 Interlock 정보 표시
            p_faultAxis = new Steema.TeeChart.Axis();
            objChart.Axes.Custom.Add(p_faultAxis);

            p_faultAxis.PositionUnits = Steema.TeeChart.PositionUnits.Pixels;
            p_faultAxis.StartPosition = 0;
            p_faultAxis.EndPosition = 2;
            p_faultAxis.Horizontal = false;
            p_faultAxis.OtherSide = false;
            p_faultAxis.AxisPen.Visible = false;
            p_faultAxis.Labels.Visible = false;
            p_faultAxis.SetMinMax(0, 1);


            // Chart 하측 10% 위치에 Glass 정보 표시
            p_labelAxis = new Steema.TeeChart.Axis();
            objChart.Axes.Custom.Add(p_labelAxis);

            p_labelAxis.PositionUnits = Steema.TeeChart.PositionUnits.Pixels;
            p_labelAxis.StartPosition = 90;
            p_labelAxis.EndPosition = 100;
            p_labelAxis.Horizontal = false;
            p_labelAxis.OtherSide = false;
            p_labelAxis.AxisPen.Visible = false;
            p_labelAxis.Labels.Visible = false;
            p_labelAxis.SetMinMax(0, 1);

            // Trace Chart를 Chart의 2%-90%에 표시
            objChart.Axes.Left.PositionUnits = Steema.TeeChart.PositionUnits.Pixels;
            objChart.Axes.Left.StartPosition = 2;
            objChart.Axes.Left.EndPosition = 90;

            objChart.Axes.Right.PositionUnits = Steema.TeeChart.PositionUnits.Pixels;
            objChart.Axes.Right.StartPosition = 2;
            objChart.Axes.Right.EndPosition = 90;
        }

        public void MoveSeriesIndex(TChart objChart)
        {
            int l_SeriesIndex = 0;
            for (int index1 = 0; index1 < objChart.Series.Count; index1++)
            {
                if (objChart.Series[index1].GetType() == typeof(HighLow))
                {
                    objChart.Series.MoveTo(objChart.Series[index1], l_SeriesIndex);
                    l_SeriesIndex++;
                }
            }
        }

        public Line AddLineSeries(TChart objChart, string p_SeriesName, VerticalAxis p_VertAxis,
            Color p_seriesColor, bool p_IsDateTime, bool p_showInLegend)
        {
            Steema.TeeChart.Styles.Line lineSeries = new Steema.TeeChart.Styles.Line();
            objChart.Series.Add(lineSeries);

            lineSeries.VertAxis = p_VertAxis;
            lineSeries.XValues.DateTime = p_IsDateTime;
            lineSeries.Title = p_SeriesName;
            lineSeries.Marks.Visible = false;
            lineSeries.ShowInLegend = p_showInLegend;
            lineSeries.Color = p_seriesColor;

            // Line-Point Display
            lineSeries.Pointer.Visible = true;
            lineSeries.Pointer.Style = PointerStyles.Circle;
            lineSeries.Pointer.Pen.Visible = false;
            lineSeries.Pointer.HorizSize = 3;
            lineSeries.Pointer.VertSize = 3;

            return lineSeries;
        }

        public Line AddLineSeries(TChart objChart, string p_SeriesName, VerticalAxis p_VertAxis,
            Color p_seriesColor, bool p_IsDateTime, bool p_showInLegend, bool pointVisible,
            System.Drawing.Drawing2D.DashStyle p_lineStyle, int p_lineWidth)
        {
            Steema.TeeChart.Styles.Line lineSeries = new Steema.TeeChart.Styles.Line();
            objChart.Series.Add(lineSeries);

            lineSeries.VertAxis = p_VertAxis;
            lineSeries.XValues.DateTime = p_IsDateTime;
            lineSeries.Title = p_SeriesName;
            lineSeries.Marks.Visible = false;
            lineSeries.ShowInLegend = p_showInLegend;
            lineSeries.Color = p_seriesColor;

            // Line-Point Display
            lineSeries.Pointer.Visible = pointVisible;
            lineSeries.Pointer.Style = PointerStyles.Circle;
            lineSeries.Pointer.Pen.Visible = false;
            lineSeries.Pointer.HorizSize = 3;
            lineSeries.Pointer.VertSize = 3;

            lineSeries.LinePen.Style = p_lineStyle;
            lineSeries.LinePen.Width = p_lineWidth;

            return lineSeries;
        }

        public Points AddPointsSeries(TChart objChart, string p_SeriesName, VerticalAxis p_VertAxis,
            Color p_seriesColor, bool p_IsDateTime, bool p_showInLegend)
        {
            Steema.TeeChart.Styles.Points pointsSeries = new Points();
            objChart.Series.Add(pointsSeries);

            pointsSeries.Pointer.Style = PointerStyles.Circle;
            pointsSeries.VertAxis = p_VertAxis;
            pointsSeries.XValues.DateTime = p_IsDateTime;
            pointsSeries.Title = p_SeriesName;
            pointsSeries.Marks.Visible = false;
            pointsSeries.ShowInLegend = p_showInLegend;
            pointsSeries.Color = p_seriesColor;
            pointsSeries.Pointer.HorizSize = 3;
            pointsSeries.Pointer.VertSize = 3;
            pointsSeries.Pointer.Pen.Visible = false;

            //if (p_VertAxis == VerticalAxis.Left || p_VertAxis == VerticalAxis.Right)
            //{
            //    pointsSeries.Pointer.HorizSize = 2;
            //    pointsSeries.Pointer.VertSize = 2;
            //}
            //else
            //{
            //    pointsSeries.Pointer.HorizSize = 4;
            //    pointsSeries.Pointer.VertSize = 4;
            //}

            return pointsSeries;
        }

        public HighLow AddHighLowSeries(TChart objChart, string p_SeriesName, VerticalAxis p_VertAxis, Color p_seriesColor, bool p_IsDateTime, bool p_showInLegend)
        {
            Steema.TeeChart.Styles.HighLow highLowSeries = new Steema.TeeChart.Styles.HighLow();
            objChart.Series.Add(highLowSeries);

            highLowSeries.VertAxis = p_VertAxis;
            highLowSeries.XValues.DateTime = p_IsDateTime;
            highLowSeries.Title = p_SeriesName;
            highLowSeries.Marks.Visible = false;
            highLowSeries.ShowInLegend = p_showInLegend;
            highLowSeries.Color = p_seriesColor;

            highLowSeries.HighPen.Visible = false;
            highLowSeries.LowPen.Visible = false;
            highLowSeries.Pen.Visible = false;

            highLowSeries.HighBrush.Color = System.Drawing.Color.FromArgb(((System.Byte)(127)), ((System.Byte)(0)), ((System.Byte)(192)), ((System.Byte)(0)));
            highLowSeries.HighBrush.Visible = true;
            highLowSeries.LowBrush.Color = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(128)), ((System.Byte)(255)), ((System.Byte)(128)));
            highLowSeries.LowBrush.Visible = true;
            highLowSeries.HighBrush.Transparency = 50;
            highLowSeries.HighBrush.Solid = true;
            highLowSeries.LowBrush.Transparency = 50;
            highLowSeries.LowBrush.Solid = true;


            return highLowSeries;
        }

        public HorizBar AddHorizBarSeries(TChart objChart, string p_SeriesName, VerticalAxis p_VertAxis, Color p_seriesColor, bool p_EachColor, bool p_showInLegend)
        {
            Steema.TeeChart.Styles.HorizBar horizBar = new HorizBar();
            objChart.Series.Add(horizBar);

            horizBar.VertAxis = p_VertAxis;
            horizBar.BarStyle = Steema.TeeChart.Styles.BarStyles.Cylinder;
            horizBar.Pen.Visible = false;
            horizBar.Color = p_seriesColor;
            horizBar.ColorEach = p_EachColor;
            horizBar.Marks.Visible = false;
            horizBar.Title = p_SeriesName;

            return horizBar;
        }

        public Bar AddBarSeries(TChart objChart, string p_SeriesName, VerticalAxis p_VertAxis, Color p_seriesColor, bool p_EachColor, bool p_showInLegend)
        {
            Steema.TeeChart.Styles.Bar bar = new Bar();
            objChart.Series.Add(bar);

            bar.VertAxis = p_VertAxis;
            bar.BarStyle = Steema.TeeChart.Styles.BarStyles.Cylinder;
            bar.Pen.Visible = true;
            bar.Color = p_seriesColor;
            bar.ColorEach = p_EachColor;
            bar.Marks.Visible = true;
            bar.Marks.Style = MarksStyles.Value;
            bar.Title = p_SeriesName;

            return bar;
        }

        public Histogram AddHistogramSeries(TChart objChart, string p_SeriesName, VerticalAxis p_VertAxis,
            Color p_seriesColor, bool p_IsDateTime, bool p_showInLegend)
        {
            Steema.TeeChart.Styles.Histogram histogramSeries = new Histogram();
            objChart.Series.Add(histogramSeries);

            histogramSeries.VertAxis = p_VertAxis;
            histogramSeries.XValues.DateTime = p_IsDateTime;
            histogramSeries.Title = p_SeriesName;
            histogramSeries.Marks.Visible = false;
            histogramSeries.ShowInLegend = p_showInLegend;
            histogramSeries.Color = p_seriesColor;
            histogramSeries.LinePen.Visible = false;


            return histogramSeries;
        }

        public Shape AddShapeSeries(TChart Chart, bool Active, System.Drawing.Drawing2D.DashStyle PenStyle, ShapeStyles ShapeStyle, Color SeriesColor, Color PenColor, int Transparency)
        {
            Shape shapeSeries = new Shape();
            Chart.Series.Add(shapeSeries);
            shapeSeries.Active = Active;
            shapeSeries.ShowInLegend = false;
            shapeSeries.Pen.Style = PenStyle;
            shapeSeries.Style = ShapeStyle;
            shapeSeries.XYStyle = ShapeXYStyles.Axis;
            shapeSeries.Color = SeriesColor;
            shapeSeries.Pen.Color = PenColor;
            shapeSeries.Brush.Transparency = Transparency;

            return shapeSeries;
        }

        public void DrawVerticalLine(TChart objChart, Line lineSeries, float Value)
        {
            float[] HorizontalX = new float[2];
            float[] HorizontalY = new float[2];

            HorizontalX[0] = HorizontalX[1] = Value;
            HorizontalY[0] = (float)objChart.Axes.Left.Minimum;
            HorizontalY[1] = (float)objChart.Axes.Left.Maximum;
            lineSeries.Add(HorizontalX, HorizontalY);
        }


        public void AddMarksTip(TChart objChart)
        {
            MarksTip marksTip = new MarksTip();
            marksTip.Active = true;
            objChart.Tools.Add(marksTip);
        }

        public ColorBand AddColorBand(TChart objChart, Axis axis, Color bandColor)
        {
            ColorBand band = new ColorBand();
            objChart.Tools.Add(band);
            band.Active = false;
            band.Axis = axis;
            band.Pen.Visible = false;
            band.Brush.Color = bandColor;
            band.Brush.Transparency = 50;

            return band;
        }

        public LegendScrollBar AddLegendScrollBar(TChart objChart)
        {
            LegendScrollBar legendScrollBar = new LegendScrollBar();
            objChart.Tools.Add(legendScrollBar);
            legendScrollBar.Active = true;
            legendScrollBar.ArrowBrush.Color = System.Drawing.Color.FromArgb(((System.Byte)(45)), ((System.Byte)(45)), ((System.Byte)(255)));
            legendScrollBar.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.Raised;
            legendScrollBar.Brush.Color = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(255)));
            legendScrollBar.DrawStyle = Steema.TeeChart.Tools.ScrollBarDrawStyle.Always;
            legendScrollBar.Pen.Color = System.Drawing.Color.FromArgb(((System.Byte)(174)), ((System.Byte)(177)), ((System.Byte)(232)));
            legendScrollBar.Size = 15;
            legendScrollBar.ThumbBrush.Color = System.Drawing.Color.FromArgb(((System.Byte)(176)), ((System.Byte)(216)), ((System.Byte)(255)));
            legendScrollBar.DrawStyle = ScrollBarDrawStyle.WhenNeeded;

            objChart.Legend.Pen.Visible = true;
            objChart.Legend.Pen.Color = System.Drawing.Color.FromArgb(((System.Byte)(224)), ((System.Byte)(224)), ((System.Byte)(224)));

            return legendScrollBar;
        }

        public void ShowEditor(TChart objChart)
        {
            objChart.ShowEditor();
        }

        public ColorLine AddColorLine(TChart objChart, Axis axis, Color lineColor, System.Drawing.Drawing2D.DashStyle dashStyle)
        {
            ColorLine colorLine = new ColorLine();
            objChart.Tools.Add(colorLine);

            colorLine.Axis = axis;
            colorLine.Pen.Color = lineColor;
            colorLine.Pen.Style = dashStyle;

            return colorLine;
        }

        // Tooltip Display 관련
        public Annotation AddAnnotate(Steema.TeeChart.TChart objChart)
        {
            Annotation annotation = new Annotation();
            objChart.Tools.Add(annotation);
            annotation.Active = false;
            annotation.Shape.CustomPosition = true;
            annotation.Shape.Shadow.Visible = false;
            annotation.Shape.Color = Color.YellowGreen;
            annotation.Shape.Visible = true;
            annotation.Shape.Font.Name = "Tahoma";
            annotation.Shape.Transparency = 45;
            annotation.Shape.Pen.Color = Color.DarkRed;
            annotation.Shape.ShapeStyle = Steema.TeeChart.Drawing.TextShapeStyle.RoundRectangle;

            return annotation;
        }

        public void ShowAnnotate(TChart objChart, Annotation annotation, int X, int Y, string displayText)
        {
            Size size = CalculateSizeOfAnnotation(displayText);
            Point p = CaculateLocationOfAnnotation(objChart, X, Y, size);

            annotation.Shape.Left = p.X;
            annotation.Shape.Top = 10;
            annotation.Text = displayText;
            annotation.Active = true;
        }

        public void HideAnnotate(Annotation annotation)
        {
            if (annotation == null)
            {
                return;
            }
            annotation.Active = false;
        }

        private Size CalculateSizeOfAnnotation(string tipText)
        {
            string[] tips = tipText.Replace("\r\n", "^").Split('^');
            int maxLength = 0;
            int valLength = 0;

            for (int i = 0; i < tips.Length; i++)
            {
                if (i == 0)
                {
                    maxLength = tips[i].Length;
                }
                else
                {
                    valLength = tips[i].Length;
                    if (valLength > maxLength)
                    {
                        maxLength = valLength;
                    }
                }
            }

            //			int fontWidth  = (int)Graphics.FromHwnd(this.Handle).MeasureString(values[0], control.Font).Width;
            //			int fontHeight = (int)Graphics.FromHwnd(this.Handle).MeasureString(values[0], control.Font).Width;
            //			return new Size(maxLength*6 - 10, tips.Length*11+5);
            return new Size(maxLength * 6 - 5, tips.Length * 11 + 5);
        }



        /// <summary>
        /// Annotation의 위치를 계산한다.
        /// 차트를 4등분해서 그리는 위치를 찾는다.
        /// 1 | 2
        /// ------
        /// 3 | 4
        /// </summary>
        /// <param name="x">마우스 클릭 x좌표</param>
        /// <param name="y">마우스 클릭 y좌표</param>
        /// <param name="annoOfSize">annotation의 크기</param>
        /// <returns></returns>
        private Point CaculateLocationOfAnnotation(TChart objChart, int x, int y, Size annoOfSize)
        {
            int centerX = objChart.Width / 2;
            int centerY = objChart.Height / 2;

            int[] position = new int[2];
            Point p = new Point(x, y);

            if (centerX >= x && centerY >= y) // 1영역 : 그냥 마우스 클릭 위치에..
            {
            }
            else if (centerX <= x && centerY >= y) // 2 영역 : 마우스 클릭이 anno의 왼쪽 위 모서리가 된다.
                p.X = x - annoOfSize.Width;
            else if (centerX >= x && centerY <= y) // 3 영역 : 마우스 클릭위치의 anno의 오른쪽 아래 모서리
                p.Y = y - annoOfSize.Height;
            else // 4 영역 : 마우스 클릭위치의 anno의 왼쪽 아래 모서리
            {
                p.X = x - annoOfSize.Width;
                p.Y = y - annoOfSize.Height;
            }

            int chkDiff = objChart.Height - (annoOfSize.Height + p.Y);
            if (chkDiff < 0)
                p.Y += chkDiff;

            return p;
        }

        public Bar AddVertBarSeries(TChart objChart, string p_SeriesName, VerticalAxis p_VertAxis, Color p_seriesColor, bool p_EachColor, bool p_showInLegend)
        {
            Steema.TeeChart.Styles.Bar vertBar = new Bar();
            objChart.Series.Add(vertBar);

            vertBar.VertAxis = p_VertAxis;
            vertBar.Title = p_SeriesName;
            vertBar.BarStyle = Steema.TeeChart.Styles.BarStyles.Cylinder;
            vertBar.Pen.Visible = false;
            vertBar.ColorEach = p_EachColor;
            vertBar.Marks.Visible = false;

            return vertBar;
        }

        public void ExportImage(TChart Chart)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.DefaultExt = Chart.Export.Image.GIF.FileExtension;
            saveFileDialog1.FileName = Chart.Name + "." + saveFileDialog1.DefaultExt;
            saveFileDialog1.Filter = Texts.WMFFilter;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // set some jpeg properties
                //Chart.Export.Image.GIF.ColorReduction = Steema.TeeChart.Export.GIFFormat.GIFColorReduction.None;
                //Chart.Export.Image.GIF.Save(saveFileDialog1.FileName);

                Chart.Header.Text = DateTime.Now.ToString();
                Chart.Export.Image.Metafile.Save(saveFileDialog1.FileName);
                Chart.Header.Text = DateTime.Now.ToString();
                Chart.Export.Image.PNG.Save(saveFileDialog1.FileName);
                Chart.Header.Text = DateTime.Now.ToString();
                Chart.Export.Image.TIFF.Save(saveFileDialog1.FileName);
                Chart.Header.Text = DateTime.Now.ToString();
                Chart.Export.Image.VML.Save(saveFileDialog1.FileName);
                Chart.Header.Text = DateTime.Now.ToString();
            }

            //saveFileDialog1.DefaultExt = Chart.Export.Image.JPEG.FileExtension;
            //saveFileDialog1.FileName = Chart.Name + "." + saveFileDialog1.DefaultExt;
            //saveFileDialog1.Filter = Texts.JPEGFilter;
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    // set some jpeg properties
            //    Chart.Export.Image.JPEG.GrayScale = true;
            //    Chart.Export.Image.JPEG.Quality = 75;
            //    Chart.Export.Image.JPEG.Save(saveFileDialog1.FileName);
            //}
        }

        public void ChangeLegendAlignment(TChart objChart)
        {
            if (objChart.Legend.Alignment == LegendAlignments.Right)
            {
                objChart.Legend.Alignment = LegendAlignments.Bottom;
            }
            else
            {
                objChart.Legend.Alignment = LegendAlignments.Right;
            }
        }
    }
}
