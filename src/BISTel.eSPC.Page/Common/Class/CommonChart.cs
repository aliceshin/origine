using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Page.Common
{

    public class CommonChart
    {
        public static List<string> lstChartColumn = new List<string>();
        public static LinkedList llstChartColumn = new LinkedList();

        public const string COLUMN_NAME_SEQ_INDEX = "$SEQ_INDEX$";
        public const int START_RAW_DATA_INDEX = 0;

        public static LinkedList PARAM_ITEM
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RAW, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MEAN, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.STDDEV, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RANGE, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MAX, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MIN, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MSD, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MA, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MR, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMAMEAN, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMARANGE, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMASTDDEV, 1);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList PARAM_CALC_ITEM
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RAW, 1);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList PARAM_CALC_EXCLUED_ITEM
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MEAN, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.STDDEV, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RANGE, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MAX, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MIN, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MSD, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MA, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MR, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMAMEAN, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMARANGE, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMASTDDEV, 1);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_XBAR_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MEAN_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MEAN_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MEAN_USL, Definition.CHART_COLUMN.USL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MEAN_LSL, Definition.CHART_COLUMN.LSL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MEAN_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MEAN, Definition.CHART_COLUMN.AVG);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MIN, Definition.CHART_COLUMN.MIN);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MAX, Definition.CHART_COLUMN.MAX);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_RANGE_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RANGE_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RANGE_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RANGE_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RANGE, Definition.CHART_COLUMN.RANGE);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_STDDEV_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.STDDEV_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.STDDEV_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.STDDEV_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.STDDEV, Definition.CHART_COLUMN.STDDEV);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_EWMA_MEAN_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMAMEAN_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMAMEAN_LCL, Definition.CHART_COLUMN.LCL);
                //CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMAMEAN_USL, Definition.CHART_COLUMN.USL);
                //CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMAMEAN_LSL, Definition.CHART_COLUMN.LSL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMAMEAN_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMAMEAN, Definition.CHART_COLUMN.AVG);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_EWMA_RANGE_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMARANGE_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMARANGE_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMARANGE_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMARANGE, Definition.CHART_COLUMN.RANGE);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_EWMA_STDDEV_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMASTDDEV_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMASTDDEV_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMASTDDEV_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.EWMASTDDEV, Definition.CHART_COLUMN.STDDEV);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_RAW_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RAW_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RAW_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RAW_USL, Definition.CHART_COLUMN.USL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RAW_LSL, Definition.CHART_COLUMN.LSL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RAW_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.RAW, Definition.CHART_COLUMN.RAW);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_MA_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MA_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MA_LCL, Definition.CHART_COLUMN.LCL);
                //CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MA_USL, Definition.CHART_COLUMN.USL);
                //CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MA_LSL, Definition.CHART_COLUMN.LSL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MA_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MA, Definition.CHART_COLUMN.MA);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_MSD_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MSD_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MSD_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MSD_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MSD, Definition.CHART_COLUMN.MSD);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_MR_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MR_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MR_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MR_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.MR, Definition.CHART_COLUMN.MR);
                return CommonChart.llstChartColumn;
            }
        }


        public static string GetChartPointSeries(string _chartName)
        {
            switch (_chartName)
            {
                case Definition.CHART_TYPE.XBAR:
                    return Definition.CHART_COLUMN.AVG;
                case Definition.CHART_TYPE.RANGE:
                    return Definition.CHART_COLUMN.RANGE;
                case Definition.CHART_TYPE.STDDEV:
                    return Definition.CHART_COLUMN.STDDEV;
                case Definition.CHART_TYPE.RAW:
                    return Definition.CHART_COLUMN.RAW;
                case Definition.CHART_TYPE.MA:
                    return Definition.CHART_COLUMN.MA;
                case Definition.CHART_TYPE.MSD:
                    return Definition.CHART_COLUMN.MSD;
                case Definition.CHART_TYPE.MR:
                    return Definition.CHART_COLUMN.MR;
                case Definition.CHART_TYPE.EWMA_MEAN:
                    return Definition.CHART_COLUMN.AVG;
                case Definition.CHART_TYPE.EWMA_RANGE:
                    return Definition.CHART_COLUMN.RANGE;
                case Definition.CHART_TYPE.EWMA_STDDEV:
                    return Definition.CHART_COLUMN.STDDEV;
                case Definition.CHART_TYPE.BOX:
                    return Definition.CHART_COLUMN.STDDEV;
                case Definition.CHART_TYPE.DOT_PLOT:
                    return Definition.CHART_COLUMN.STDDEV;
                case Definition.CHART_TYPE.HISTOGRAM:
                    return Definition.CHART_COLUMN.STDDEV;
                case Definition.CHART_TYPE.RUN:
                    return Definition.CHART_COLUMN.STDDEV;
            }

            return string.Empty;
        }

        public static LinkedList GetChartSeries(string _chartName, List<string> _lstRawColumn)
        {
            LinkedList llstChartSeries = new LinkedList();
            switch (_chartName)
            {
                case Definition.CHART_TYPE.XBAR:
                    llstChartSeries = CommonChart.CHART_XBAR_COLUMN;
                    for (int j = 0; j < _lstRawColumn.Count; j++)
                    {
                        llstChartSeries.Add(_lstRawColumn[j].ToString(), _lstRawColumn[j].ToString());
                    }
                    break;
                case Definition.CHART_TYPE.RANGE:
                    llstChartSeries = CommonChart.CHART_RANGE_COLUMN;
                    break;
                case Definition.CHART_TYPE.STDDEV:
                    llstChartSeries = CommonChart.CHART_STDDEV_COLUMN;
                    break;
                case Definition.CHART_TYPE.RAW:
                    llstChartSeries = CommonChart.CHART_RAW_COLUMN;

                    if (_lstRawColumn != null && _lstRawColumn.Count > 0)
                    {
                        for (int j = 0; j < _lstRawColumn.Count; j++)
                        {
                            llstChartSeries.Add(_lstRawColumn[j].ToString(), _lstRawColumn[j].ToString());
                        }
                    }
                    break;
                case Definition.CHART_TYPE.MA:
                    llstChartSeries = CommonChart.CHART_MA_COLUMN;
                    for (int j = 0; j < _lstRawColumn.Count; j++)
                    {
                        llstChartSeries.Add(_lstRawColumn[j].ToString(), _lstRawColumn[j].ToString());
                    }
                    break;
                case Definition.CHART_TYPE.MSD:
                    llstChartSeries = CommonChart.CHART_MSD_COLUMN;
                    break;
                case Definition.CHART_TYPE.MR:
                    llstChartSeries = CommonChart.CHART_MR_COLUMN;
                    break;
                case Definition.CHART_TYPE.EWMA_MEAN:
                    llstChartSeries = CommonChart.CHART_EWMA_MEAN_COLUMN;
                    break;
                case Definition.CHART_TYPE.EWMA_RANGE:
                    llstChartSeries = CommonChart.CHART_EWMA_RANGE_COLUMN;
                    break;
                case Definition.CHART_TYPE.EWMA_STDDEV:
                    llstChartSeries = CommonChart.CHART_EWMA_STDDEV_COLUMN;
                    break;

                case Definition.CHART_TYPE.BOX:
                    llstChartSeries = CommonChart.CHART_EWMA_STDDEV_COLUMN;
                    break;

                case Definition.CHART_TYPE.DOT_PLOT:
                    llstChartSeries = CommonChart.CHART_EWMA_STDDEV_COLUMN;
                    break;

                case Definition.CHART_TYPE.HISTOGRAM:
                    llstChartSeries = CommonChart.CHART_EWMA_STDDEV_COLUMN;
                    break;

                case Definition.CHART_TYPE.RUN:
                    llstChartSeries = CommonChart.CHART_EWMA_STDDEV_COLUMN;
                    break;
            }

            llstChartSeries.Add(Definition.CHART_COLUMN.DTSOURCEID, Definition.CHART_COLUMN.DTSOURCEID);
            llstChartSeries.Add(Definition.COL_TOGGLE_YN, Definition.COL_TOGGLE_YN);
            return llstChartSeries;
        }
    }
}
