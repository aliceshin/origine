using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using BISTel.eSPC.Common.ATT;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Page.ATT.Common
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
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.P, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.PN, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.C, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.U, 1);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList PARAM_CALC_ITEM
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.P, 1);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList PARAM_CALC_EXCLUED_ITEM
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.PN, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.C, 1);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.U, 1);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_P_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.P_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.P_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.P_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.P, Definition.CHART_COLUMN.P);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_PN_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.PN_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.PN_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.PN_USL, Definition.CHART_COLUMN.USL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.PN_LSL, Definition.CHART_COLUMN.LSL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.PN_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.PN, Definition.CHART_COLUMN.PN);
                return CommonChart.llstChartColumn;
            }
        }

        public static LinkedList CHART_C_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.C_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.C_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.C_USL, Definition.CHART_COLUMN.USL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.C_LSL, Definition.CHART_COLUMN.LSL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.C_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.C, Definition.CHART_COLUMN.C);
                return CommonChart.llstChartColumn;
            }
        }

        

        public static LinkedList CHART_U_COLUMN
        {
            get
            {
                CommonChart.llstChartColumn.Clear();
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.TIME, Definition.CHART_COLUMN.TIME);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.U_UCL, Definition.CHART_COLUMN.UCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.U_LCL, Definition.CHART_COLUMN.LCL);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.U_TARGET, Definition.CHART_COLUMN.TARGET);
                CommonChart.llstChartColumn.Add(Definition.CHART_COLUMN.U, Definition.CHART_COLUMN.U);
                return CommonChart.llstChartColumn;
            }
        }

        


        public static string GetChartPointSeries(string _chartName)
        {
            switch (_chartName)
            {
                case Definition.CHART_TYPE.P:
                    return Definition.CHART_COLUMN.P;
                case Definition.CHART_TYPE.PN:
                    return Definition.CHART_COLUMN.PN;
                case Definition.CHART_TYPE.C:
                    return Definition.CHART_COLUMN.C;
                case Definition.CHART_TYPE.U:
                    return Definition.CHART_COLUMN.U;
            }

            return string.Empty;
        }

        public static LinkedList GetChartSeries(string _chartName, List<string> _lstRawColumn)
        {
            LinkedList llstChartSeries = new LinkedList();
            switch (_chartName)
            {
                case Definition.CHART_TYPE.P:
                    llstChartSeries = CommonChart.CHART_P_COLUMN;
                    break;
                case Definition.CHART_TYPE.PN:
                    llstChartSeries = CommonChart.CHART_PN_COLUMN;
                    for (int j = 0; j < _lstRawColumn.Count; j++)
                    {
                        llstChartSeries.Add(_lstRawColumn[j].ToString(), _lstRawColumn[j].ToString());
                    }
                    break;
                case Definition.CHART_TYPE.C:
                    llstChartSeries = CommonChart.CHART_C_COLUMN;

                    if (_lstRawColumn != null && _lstRawColumn.Count > 0)
                    {
                        for (int j = 0; j < _lstRawColumn.Count; j++)
                        {
                            llstChartSeries.Add(_lstRawColumn[j].ToString(), _lstRawColumn[j].ToString());
                        }
                    }
                    break;
                
                case Definition.CHART_TYPE.U:
                    llstChartSeries = CommonChart.CHART_U_COLUMN;
                    break;
            }

            llstChartSeries.Add(Definition.CHART_COLUMN.DTSOURCEID, Definition.CHART_COLUMN.DTSOURCEID);
            llstChartSeries.Add(Definition.COL_TOGGLE_YN, Definition.COL_TOGGLE_YN);
            return llstChartSeries;
        }
    }
}
