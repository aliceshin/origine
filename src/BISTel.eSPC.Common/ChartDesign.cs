/*
 * Name     : ChartDesign 
 * Create   : 2006.01
 * Author   : Hanson Jang
 * Modify   : 2007.01.23 (Revised)
 * Version  : 2.1
 * Contents : Chart design class
 * 
 * */

using System;
using System.Drawing;
using System.Windows.Forms;

using Steema.TeeChart;
using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;
using BISTel.PeakPerformance.Client.BISTelControl;

namespace BISTel.eSPC.Common
{
    /// <summary>
    /// Chart design class
    /// </summary>
    public class ChartDesign
    {
        public ChartDesign()
        {
        }

        #region 일반적인 Chart Color를 정의합니다.
        public static readonly string[] CHARTCOLOR = new string[]{
            "#A05C37", "#538053", "#5C518B", "#CD2E57", "#FF9614", "#6E9FED", "#975A97", "#CD904A", "#DB7A9D", "#69A8AA", "#646496", 
            "#D2954F", "#FFBE0A", "#14A0A0", "#6E6EFF", "#73B2B4", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
			"#CD904A", "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
            "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#A05C37", "#538053", "#5C518B", "#CD2E57", "#FF9614", "#6E9FED", "#975A97", "#CD904A", "#DB7A9D", "#69A8AA", "#646496", 
            "#D2954F", "#FFBE0A", "#14A0A0", "#6E6EFF", "#73B2B4", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
			"#CD904A", "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
            "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#A05C37", "#538053", "#5C518B", "#CD2E57", "#FF9614", "#6E9FED", "#975A97", "#CD904A", "#DB7A9D", "#69A8AA", "#646496", 
            "#D2954F", "#FFBE0A", "#14A0A0", "#6E6EFF", "#73B2B4", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
			"#CD904A", "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
            "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#A05C37", "#538053", "#5C518B", "#CD2E57", "#FF9614", "#6E9FED", "#975A97", "#CD904A", "#DB7A9D", "#69A8AA", "#646496", 
            "#D2954F", "#FFBE0A", "#14A0A0", "#6E6EFF", "#73B2B4", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
			"#CD904A", "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
            "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#A05C37", "#538053", "#5C518B", "#CD2E57", "#FF9614", "#6E9FED", "#975A97", "#CD904A", "#DB7A9D", "#69A8AA", "#646496", 
            "#D2954F", "#FFBE0A", "#14A0A0", "#6E6EFF", "#73B2B4", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
			"#CD904A", "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
            "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#A05C37", "#538053", "#5C518B", "#CD2E57", "#FF9614", "#6E9FED", "#975A97", "#CD904A", "#DB7A9D", "#69A8AA", "#646496", 
            "#D2954F", "#FFBE0A", "#14A0A0", "#6E6EFF", "#73B2B4", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
			"#CD904A", "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
            "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#A05C37", "#538053", "#5C518B", "#CD2E57", "#FF9614", "#6E9FED", "#975A97", "#CD904A", "#DB7A9D", "#69A8AA", "#646496", 
            "#D2954F", "#FFBE0A", "#14A0A0", "#6E6EFF", "#73B2B4", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
			"#CD904A", "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
            "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#A05C37", "#538053", "#5C518B", "#CD2E57", "#FF9614", "#6E9FED", "#975A97", "#CD904A", "#DB7A9D", "#69A8AA", "#646496", 
            "#D2954F", "#FFBE0A", "#14A0A0", "#6E6EFF", "#73B2B4", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
			"#CD904A", "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
            "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#A05C37", "#538053", "#5C518B", "#CD2E57", "#FF9614", "#6E9FED", "#975A97", "#CD904A", "#DB7A9D", "#69A8AA", "#646496", 
            "#D2954F", "#FFBE0A", "#14A0A0", "#6E6EFF", "#73B2B4", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
			"#CD904A", "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513",
            "#33CC00", "#FF9999", "#EFBEF5", "#E50F9F", "#3366FF", "#FFFF00", "#FFFFE0", "#00C7FF", "#CCECFF", "#53A513", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513",
            "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF", "#EFD6C6", "#53A513"
        };



        //		  
        //		  , "#8BD26B", "#D7BA74", "#82545D", "#FFB064", "#00CCFF", "#CC33FF", "#FF6600", "#CCCCFF", "#FFCCFF"	, "#EFD6C6"
        //        , "#CCFFCC", "#660099", "#CFBEFF", "#EFBEF5", "#FF54CD", "#EEEEA6", "#99A2DB", "#CEB3AD", "#F4C8F1", "#C2C5CF"
        //        , "#BDD158"	, "#7CBDD9", "#CBDD9", "#D37482", "#528789", "#A5B8C9", "#00759F", "#FF3F7E", "#5BDD45", "#72B8B4"
        //        , "#797372", "#FF8E70", "#545B72", "#32BEC0", "#FBE5A8"	, "#C8DBD8", "#D7A9EC", "#B4A681", "#B4A681", "#988F8B"
        //        , "#BCC967", "#00648F", "#A13762", "#E2D7D4", "#B8C4C5", "#C7B9C2", "#D7B9C2", "#6AC17A", "#E5C8B9"	, "#654479"
        //        , "#4A9DAC", "#4E2D973", "#00638D", "#EAACBA", "#315B69", "#958B84", "#B580CB", "#C3A8BD", "#F44877", "#292BAF"
        //        , "#B7AB58"	, "#77C7BF", "#AFC1C9", "#D3E173"	
        #endregion


        /// <summary>
        /// MCC Chart 에서 쓰이는 Color 입니다.
        /// </summary>
        /// <param name="mccType"></param>
        /// <returns></returns>
        public static Color GetMCC_Color(int index)
        {
            Color color = Color.Empty;

            switch (index)
            {
                case 0:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
                case 1:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
                case 2:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
                case 3:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
                case 4:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
                case 5:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
                case 6:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
                case 7:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
                case 8:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
                case 9:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
                case 10:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
                case 11:
                    color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
                    break;
            }

            return color;
        }

        /// <summary>
        /// chart의 series의 색을 지정합니다.
        /// </summary>
        /// <param name="index">series Index</param>
        /// <returns></returns>
        public static Color GetColor(int index)
        {
            Color color = Color.Empty;

            if (CHARTCOLOR.Length > index)
                color = ColorTranslator.FromHtml(CHARTCOLOR[index]);
            else
                color = ColorTranslator.FromHtml(CHARTCOLOR[(CHARTCOLOR.Length - 1) % index]);

            return color;
        }

        /// <summary>
        /// Chart Image(jpeg)를 clipboard에 복사를 합니다.
        /// </summary>
        /// <param name="chart">복사할 Chart</param>
        public static void CopyToClipboard(BTChart chart)
        {
            chart.Export.Image.JPEG.CopyToClipboard();
        }

        /// <summary>
        /// Chart Image를 2개를 Clipboard에 복사를 합니다.
        /// </summary>
        /// <param name="bm1"></param>
        /// <param name="bm2"></param>
        public static void MultipleCopyToClipboard(Bitmap bm1, Bitmap bm2)
        {
            Bitmap b = new Bitmap(bm1.Width, bm1.Height + bm2.Height);
            Graphics g = Graphics.FromImage(b);
            g.DrawImage(bm1, 0, 0, bm1.Width, bm1.Height);
            g.DrawImage(bm2, 0, bm1.Height, bm2.Width, bm2.Height);

            IDataObject ido = new DataObject();
            ido.SetData(DataFormats.Bitmap, true, b);
            Clipboard.SetDataObject(ido, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bm1_w"></param>
        /// <param name="bm2"></param>
        /// <param name="bm3"></param>
        public static void TripleCopyToClipboard(Bitmap bm1_w, Bitmap bm2, Bitmap bm3)
        {
            Bitmap b = new Bitmap(bm1_w.Width, bm1_w.Height + bm2.Height);
            Graphics g = Graphics.FromImage(b);

            g.DrawImage(bm1_w, 0, 0, bm1_w.Width, bm1_w.Height);
            g.DrawImage(bm2, 0, bm1_w.Height, bm2.Width, bm2.Height);
            g.DrawImage(bm3, bm2.Width, bm1_w.Height, bm3.Width, bm3.Height);

            IDataObject ido = new DataObject();
            ido.SetData(DataFormats.Bitmap, true, b);
            Clipboard.SetDataObject(ido, true);
        }

        /// <summary>
        /// Chart를 초기화 합니다.
        /// </summary>
        /// <param name="chart"></param>
        public static void InitChart(BTChart chart, bool view3D)
        {
            chart.Tools.Clear();
            chart.Series.Clear();

            chart.Aspect.View3D = view3D;

            //chart.Aspect.ColorPaletteIndex = 1; //excel type
            chart.Aspect.ColorPaletteIndex = 9; //win xp type
            chart.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            chart.Aspect.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            chart.Aspect.ThemeIndex = 4;

            chart.Panel.Brush.Color = Color.FromArgb(226, 236, 221);

            chart.Header.Visible = false;
            chart.Legend.Visible = false;

            chart.Axes.Bottom.Labels.Style = AxisLabelStyle.None;
            chart.Axes.Bottom.Title.Visible = false;

            //InitPanel(chart);
            InitAxes(chart);

            if (view3D)
            {
                InitWall(chart);
            }
        }

        /// <summary>
        ///  Gantt Zoom Chart만 적용됩니다.
        /// </summary>
        /// <param name="chart"></param>
        public static void InitZoomChart(BTChart chart)
        {
            chart.Aspect.View3D = false;

            chart.Tools.Clear();
            chart.Series.Clear();

            chart.Header.Visible = false;
            chart.Legend.Visible = false;

            chart.Aspect.ColorPaletteIndex = 9; //win xp type
            chart.Aspect.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
            chart.Aspect.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            chart.Aspect.ThemeIndex = 4;

            chart.Axes.Depth.AxisPen.Visible = false;
            chart.Axes.Depth.Title.Visible = false;

            chart.Axes.Bottom.AxisPen.Visible = false;
            chart.Axes.Bottom.MinorTicks.Visible = false;
            chart.Axes.Bottom.Title.Visible = false;

            chart.Axes.Right.AxisPen.Visible = false;
            chart.Axes.Right.MinorTicks.Visible = false;
            chart.Axes.Right.Title.Visible = false;

            chart.Panel.Brush.Color = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(255)));

            chart.Axes.Bottom.Labels.Style = AxisLabelStyle.None;
            chart.Axes.Bottom.Title.Visible = false;

            chart.Panel.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            chart.Panel.MarginBottom = 0;
            chart.Panel.MarginLeft = 2.6;
            chart.Panel.MarginRight = 2.6;
            chart.Panel.MarginTop = 0;

            //zoom scroll mode 막기
            chart.Panning.Allow = Steema.TeeChart.ScrollModes.None;
            chart.Panning.Active = false;

            chart.Zoom.Allow = true;
            chart.Zoom.Direction = Steema.TeeChart.ZoomDirections.Horizontal;

            chart.Tools.Clear();
            chart.Series.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chart"></param>
        public static void InitPieChart(BTChart chart)
        {
            chart.Aspect.Chart3DPercent = 25;
            chart.Aspect.Elevation = 315;
            chart.Aspect.Orthogonal = false;
            chart.Aspect.Perspective = 0;
            chart.Aspect.Rotation = 360;

            chart.Tools.Clear();
            chart.Series.Clear();

            chart.Aspect.View3D = true;
            InitChart(chart, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chart"></param>
        public static void InitAxes(BTChart chart)
        {
            chart.Axes.Bottom.Grid.Color = System.Drawing.Color.Gray;
            chart.Axes.Bottom.Ticks.Length = 2;
            chart.Axes.Depth.Grid.Color = System.Drawing.Color.Gray;
            chart.Axes.Depth.Ticks.Length = 2;
            chart.Axes.Left.Grid.Color = System.Drawing.Color.Gray;
            chart.Axes.Left.Ticks.Length = 2;
            chart.Axes.Right.Grid.Color = System.Drawing.Color.Gray;
            chart.Axes.Right.Ticks.Length = 2;
            chart.Axes.Top.Grid.Color = System.Drawing.Color.Gray;
            chart.Axes.Top.Ticks.Length = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chart"></param>
        public static void InitPanel(BTChart chart)
        {
            //chart.Panel.Bevel.Outer = Steema.TeeChart.Drawing.BevelStyles.None;
            //chart.Panel.BorderRound = 10;

            chart.Panel.Brush.Color = System.Drawing.Color.FromArgb(((System.Byte)(226)), ((System.Byte)(236)), ((System.Byte)(221)), ((System.Byte)(192)));

            //chart.Panel.Brush.Gradient.EndColor = System.Drawing.Color.DarkGray;
            //chart.Panel.Brush.Gradient.MiddleColor = System.Drawing.Color.Empty;
            //chart.Panel.Brush.Gradient.StartColor = System.Drawing.Color.White;

            //chart.Panel.Gradient.EndColor = System.Drawing.Color.DarkGray;
            //chart.Panel.Gradient.MiddleColor = System.Drawing.Color.Empty;
            //chart.Panel.Gradient.StartColor = System.Drawing.Color.White;

            //chart.Panel.Pen.Color = System.Drawing.Color.Navy;

            chart.Panel.Pen.Visible = false;
            //chart.Panel.Pen.Width = 3;
            chart.Panel.Shadow.Height = 0;
            chart.Panel.Shadow.Width = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chart"></param>
        public static void InitWall(BTChart chart)
        {
            chart.Walls.Bottom.Pen.Color = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(224)), ((System.Byte)(224)), ((System.Byte)(224)));
            chart.Walls.Bottom.Pen.Visible = false;
            chart.Walls.Bottom.Size = 5;
            chart.Walls.Left.Brush.Color = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(224)), ((System.Byte)(224)), ((System.Byte)(224)));
            chart.Walls.Left.Size = 5;
            chart.Walls.Right.Brush.Color = System.Drawing.Color.Silver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="pTitle"></param>
        public static void InitHeader(BTChart chart, string pTitle)
        {
            chart.Header.Font.Brush.Color = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(128)));
            chart.Header.Font.Name = "Tahoma";
            chart.Header.Font.Shadow.Brush.Color = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(224)), ((System.Byte)(224)), ((System.Byte)(224)));
            chart.Header.Font.Shadow.Visible = true;
            chart.Header.Text = pTitle;
            chart.Header.Visible = true;
        }

        /// <summary>
        /// clear chart
        /// </summary>
        /// <param name="objChart"></param>
        public static void ChartClear(BTChart chart)
        {
            chart.Panel.Color = Color.White;
            chart.Tools.Clear();
            chart.Series.RemoveAllSeries();
            chart.Aspect.View3D = false;
            chart.Header.Visible = false;
            chart.Axes.Bottom.Labels.Style = AxisLabelStyle.None;
            chart.Axes.Bottom.Title.Visible = false;
        }

        /// <summary>
        /// 선택한 축의 title을 정의합니다.
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="axisTitle"></param>
        public static void SetAxisTitle(Axis axis, string axisTitle)
        {
            axis.Title.Visible = true;
            axis.Title.Font.Size = 7;
            axis.Title.Font.Name = "Tahoma";
            axis.Title.Text = axisTitle;
        }

        /// <summary>
        /// Legend를 사용합니다.
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="location"></param>
        /// <param name="visibleCheckBox"></param>
        public static void InitLegend(BTChart chart, LegendAlignments location, LegendStyles style, bool visibleCheckBox)
        {
            chart.Legend.Visible = true;
            chart.Legend.Alignment = location;
            chart.Legend.LegendStyle = style;
            chart.Legend.TextStyle = LegendTextStyles.Plain;
            chart.Legend.Font.Name = "Tahoma";
            chart.Legend.Font.Size = 7;
            chart.Legend.Symbol.Pen.Visible = false;
            chart.Legend.Symbol.Width = 10;
            chart.Legend.Symbol.WidthUnits = LegendSymbolSize.Pixels;
            chart.Legend.TopLeftPos = 0;
            chart.Legend.CheckBoxes = visibleCheckBox;
            chart.Legend.Shadow.Visible = false;
            chart.Legend.Brush.Color = chart.Panel.Color;
            chart.Legend.Pen.Visible = false;
            chart.Legend.Font.Size = 7;
        }

        /// <summary>
        /// 선택한 축의 Min/Max를 설정합니다.
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="OffsetMin"></param>
        /// <param name="OffsetMax"></param>
        public static void SetAxisMinMax(Axis axis, int OffsetMin, int OffsetMax)
        {
            axis.Automatic = true;
            axis.MinimumOffset = OffsetMin;
            axis.MaximumOffset = OffsetMax;
        }

        public static void SetAxisMinMax(Axis axis, DateTime FromDateTime, DateTime ToDateTime)
        {
            axis.Automatic = false;
            axis.SetMinMax(FromDateTime, ToDateTime);
        }

        public static void InitAxis(Axis axis, AxisLabelStyle axisLabelStyle)
        {
            axis.Labels.Visible = true;
            axis.Labels.Style = axisLabelStyle;
            axis.Labels.Font.Size = 8;
            axis.Labels.Font.Name = "Tahoma";
        }



        #region : Initialize Series :

        /// <summary>
        /// line series 초기화를 합니다.
        /// </summary>
        /// <param name="lineSeries"></param>
        /// <param name="IsViewPoint"></param>
        public static void InitLineSeries(Line lineSeries, VerticalAxis Yaxis, string seriesTitle, bool boolTime, bool IsViewPoint)
        {
            lineSeries.Pointer.Brush.Color = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
            lineSeries.Pointer.HorizSize = 3;
            lineSeries.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Rectangle;
            lineSeries.Pointer.VertSize = 3;
            lineSeries.Pointer.Visible = IsViewPoint;

            lineSeries.Title = seriesTitle;
            lineSeries.VertAxis = Yaxis;
            lineSeries.XValues.DateTime = boolTime;
        }

        /// <summary>
        /// Gantt series를 초기화 합니다.
        /// </summary>
        /// <param name="ganttSeries"></param>
        /// <param name="IsViewPen"></param>
        public static void InitGanttSeries(Gantt ganttSeries, VerticalAxis Yaxis, string seriesTitle, bool boolTime, bool IsViewPen)
        {
            ganttSeries.Marks.Visible = false;
            ganttSeries.Pointer.Pen.Visible = IsViewPen;
            ganttSeries.ColorEach = false;

            ganttSeries.Title = seriesTitle;
            ganttSeries.VertAxis = Yaxis;
            ganttSeries.XValues.DateTime = boolTime;
        }

        /// <summary>
        /// Bar series를 초기화 합니다.
        /// </summary>
        /// <param name="barSeries"></param>
        /// <param name="Yaxis"></param>
        /// <param name="style"></param>
        /// <param name="boolTime"></param>
        /// <param name="colorEach"></param>
        /// <param name="markVisible"></param>
        public static void InitBarSeries(Bar barSeries, VerticalAxis Yaxis, BarStyles style, bool boolTime,
            bool colorEach, bool markVisible)
        {
            barSeries.BarStyle = style;
            barSeries.VertAxis = Yaxis;
            barSeries.XValues.DateTime = boolTime;
            barSeries.Marks.Visible = false;

            barSeries.ColorEach = colorEach;
            MarkView(barSeries, markVisible);

            barSeries.Brush.Color = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(153)), ((System.Byte)(153)));
            barSeries.Pen.Color = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(128)), ((System.Byte)(128)), ((System.Byte)(128)));
        }

        public static void InitBarSeries(Bar barSeries, VerticalAxis Yaxis, BarStyles style, bool boolTime,
            bool colorEach, bool markVisible, bool rectBoarder)
        {
            barSeries.BarStyle = style;
            barSeries.VertAxis = Yaxis;
            barSeries.XValues.DateTime = boolTime;
            barSeries.Marks.Visible = false;

            barSeries.ColorEach = colorEach;
            MarkView(barSeries, markVisible);

            if (rectBoarder)
            {
                barSeries.Brush.Color = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(153)), ((System.Byte)(153)));
                barSeries.Pen.Color = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(128)), ((System.Byte)(128)), ((System.Byte)(128)));
            }
        }

        /// <summary>
        /// PIE series를 초기화합니다.
        /// </summary>
        /// <param name="pieSeries"></param>
        /// <param name="Yaxis"></param>
        /// <param name="seriesName"></param>
        public static void InitPieSeries(Pie pieSeries, VerticalAxis Yaxis, string seriesName)
        {
            pieSeries.VertAxis = Yaxis;
            pieSeries.XValues.DateTime = false;
            pieSeries.Title = seriesName;
            pieSeries.Marks.Visible = false;
            pieSeries.ColorEach = false;

            //pieSeries.Brush.Color = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(153)), ((System.Byte)(153)));
            //pieSeries.Pen.Color = System.Drawing.Color.FromArgb(((System.Byte)(254)), ((System.Byte)(128)), ((System.Byte)(128)), ((System.Byte)(128)));
        }

        /// <summary>
        /// Points series를 초기화합니다.
        /// </summary>
        /// <param name="pointSeries"></param>
        /// <param name="Yaxis"></param>
        /// <param name="seriesName"></param>
        public static void InitPointsSeries(Points pointSeries, VerticalAxis Yaxis, string seriesName)
        {
            pointSeries.Pointer.Style = Steema.TeeChart.Styles.PointerStyles.Sphere;
            pointSeries.Pointer.HorizSize = 1;
            pointSeries.Pointer.VertSize = 4;
            //pointSeries.Color = color;
            pointSeries.Pointer.Visible = true;
            pointSeries.Pointer.Pen.Color = Color.Red;
            pointSeries.Pointer.Pen.Width = 3;
            pointSeries.Title = seriesName;
            pointSeries.VertAxis = Yaxis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="horizBarSeries"></param>
        /// <param name="Yaxis"></param>
        /// <param name="seriesName"></param>
        public static void InitHorizBarSeries(HorizBar horizBarSeries, VerticalAxis Yaxis, string seriesName)
        {
            // Axis 관련 항목
            horizBarSeries.BarStyle = Steema.TeeChart.Styles.BarStyles.Cylinder;
            horizBarSeries.Pen.Visible = false;
            horizBarSeries.ColorEach = true;

            horizBarSeries.VertAxis = Yaxis;
            horizBarSeries.XValues.DateTime = false;
            horizBarSeries.Title = seriesName;
            horizBarSeries.Marks.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="histoSeries"></param>
        /// <param name="p_SeriesName"></param>
        /// <param name="p_VertAxis"></param>
        /// <param name="p_seriesColor"></param>
        /// <param name="p_IsDateTime"></param>
        /// <param name="p_showInLegend"></param>
        public static void InitHistogramSeries(Histogram histoSeries, string p_SeriesName, VerticalAxis p_VertAxis,
            Color p_seriesColor, bool p_IsDateTime, bool p_showInLegend)
        {
            histoSeries.VertAxis = p_VertAxis;
            histoSeries.XValues.DateTime = p_IsDateTime;
            histoSeries.Title = p_SeriesName;
            histoSeries.Marks.Visible = false;
            histoSeries.ShowInLegend = p_showInLegend;
            histoSeries.Color = p_seriesColor;
            histoSeries.LinePen.Visible = false;
        }

        #endregion



        public static void MarkView(Steema.TeeChart.Styles.Series series, bool pMarkVisible)
        {
            series.Marks.Visible = pMarkVisible;
            series.Marks.Style = Steema.TeeChart.Styles.MarksStyles.Value;

            series.Marks.Arrow.Visible = true;
            series.Marks.Arrow.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            series.Marks.Font.Name = "Tahoma";
            series.Marks.Arrow.Color = System.Drawing.Color.Black;
            series.Marks.Font.Size = 7;

            series.Marks.Symbol.Shadow.Height = 1;
            series.Marks.Symbol.Shadow.Visible = true;
            series.Marks.Symbol.Shadow.Width = 1;
            series.Marks.Transparent = false;

        }

        /// <summary>
        /// 차트의 header를 설정합니다.
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="title"></param>
        public static void SetHeader(BTChart chart, string title)
        {
            chart.Header.Visible = true;
            chart.Header.Text = title;
            chart.Header.Font.Bold = true;
        }

        public static void SetChartHeaderTitle(BTChart chart, string titleText, System.Drawing.StringAlignment alignment, int titleFontSize)
        {
            chart.Header.Alignment = alignment;
            chart.Header.Font.Bold = false;
            chart.Header.Font.Name = "Tahoma";
            chart.Header.Font.Size = titleFontSize;
            chart.Header.Font.Color = Color.Black;
            chart.Header.Text = titleText;
            chart.Header.Visible = true;
        }

        public static void DrawVerticalLine(BTChart objChart, Line lineSeries, float Value)
        {
            float[] HorizontalX = new float[2];
            float[] HorizontalY = new float[2];

            HorizontalX[0] = HorizontalX[1] = Value;
            HorizontalY[0] = (float)objChart.Axes.Left.Minimum;
            HorizontalY[1] = (float)objChart.Axes.Left.Maximum;
            lineSeries.Add(HorizontalX, HorizontalY);
        }

        #region : Tool-Tip (annotation) :

        /// <summary>
        /// 차트의 ToolTip을 설정하고 Annotation을 반환합니다.
        /// </summary>
        /// <param name="chart"></param>
        public static Annotation AddToolTip(BTChart chart)
        {
            Steema.TeeChart.Tools.Annotation ann = new Steema.TeeChart.Tools.Annotation();

            ann.Active = false;
            ann.Shape.CustomPosition = true;
            ann.Shape.Shadow.Visible = false;
            ann.Shape.Color = Color.Yellow;
            //ann.Shape.Font.Name = "Tahoma";
            ann.Shape.Font.Name = "arial";
            //anno.Shape.Shadow.Visible = false;
            ann.Shape.Transparency = 50;
            ann.Shape.Pen.Color = ChartDesign.GetColor(0);
            ann.Shape.ShapeStyle = Steema.TeeChart.Drawing.TextShapeStyle.RoundRectangle;

            chart.Tools.Add(ann);
            return ann;
        }

        public static Size CalculateSizeOfAnnotation(string strAnnotationText)
        {
            string[] tips = strAnnotationText.Replace("\n", "^").Split('^');
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

            return new Size(maxLength * 6 - 10, tips.Length * 11 + 5);
        }

        public static Point CaculateLocationOfAnnotation(int x, int y, Size annoOfSize, BTChart tchart)
        {
            int centerX = tchart.Width / 2;
            int centerY = tchart.Height / 2;

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

            int chkDiff = tchart.Height - (annoOfSize.Height + p.Y);
            if (chkDiff < 0)
                p.Y += chkDiff;

            return p;
        }

        #endregion

    }
}
