using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using Steema.TeeChart.Styles;
using BISTel.PeakPerformance.Statistics.Application.ControlChart;

//modified by enkim 2012.05.21 SPC-851
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.Styles;
//modified end SPC-851

namespace BISTel.eSPC.Page.Common
{
    public partial class DefaultChart : BaseChart
    {

        string _xBarChartType = Definition.CHART_TYPE.XBAR;
        string[] _parameters;


        public DefaultChart(SourceDataManager dataManager)
        {
            InitializeComponent();
            this.DataManager = dataManager;

        }

        #region PageLoad & Initialize

        #endregion

        #region Override Method.
        protected override void MakeDataSourceToDrawSPCChart()
        {
            //2012-03-23 added by rachel -->
            //[SPC-659]
            //DataRow dr = null;
            ReleaseData();
            //<----            

            this.dtDataSource = new DataTable();
            this.dtDataSource.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, typeof(int));
            CommonChart.llstChartColumn = new LinkedList();
            LinkedList llstChartColumn = CommonChart.GetChartSeries(this._xBarChartType, lstRawColumn);
            MakeDataTable(llstChartColumn);
        }


        //modified by enkim 2012.05.25 SPC-851
        protected override void DrawChartWithSeriesInfo()
        {
            SeriesInfo si = null;
            ChartUtility chartUtil = new ChartUtility();
            int iCount = (int)ChartUtility.CHAERT_SERIES_COLOR.COUNT;

            for (int i = 0; i < this.dtDataSource.Columns.Count; i++)
            {
                string seriesName = this.dtDataSource.Columns[i].ColumnName;

                if (seriesName == CommonChart.COLUMN_NAME_SEQ_INDEX || 
                    seriesName == Definition.CHART_COLUMN.TIME || 
                    seriesName == Definition.CHART_COLUMN.TIME2 || 
                    seriesName == Definition.CHART_COLUMN.DTSOURCEID ||
                    seriesName == Definition.COL_TOGGLE_YN ||
                    lstRawColumn.Contains(seriesName))
                    continue;
                //si = new SeriesInfo(typeof(Line), seriesName, this.dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName);
                si = new SeriesInfo(typeof(ExtremeBFastLine), seriesName, this.dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName);
                si.TreatNulls = TreatNullsStyle.DoNotPaint;


                switch (seriesName)
                {
                    case Definition.CHART_COLUMN.UCL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.UCL); break;
                    case Definition.CHART_COLUMN.LCL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LCL); break;
                    case Definition.CHART_COLUMN.LSL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LSL); break;
                    case Definition.CHART_COLUMN.USL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.USL); break;
                    case Definition.CHART_COLUMN.MIN: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.MIN); break;
                    case Definition.CHART_COLUMN.MAX: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.MAX); break;
                    case Definition.CHART_COLUMN.TARGET: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.TARGET); break;
                    default:
                        if ((seriesName == Definition.CHART_COLUMN.RAW)
                           || (seriesName == Definition.CHART_COLUMN.STDDEV)
                           || (seriesName == Definition.CHART_COLUMN.AVG)
                           || (seriesName == Definition.CHART_COLUMN.RANGE)
                           || (seriesName == Definition.CHART_COLUMN.MA)
                           || (seriesName == Definition.CHART_COLUMN.MSD)
                           || (seriesName == Definition.CHART_COLUMN.MR)
                           || (seriesName == Definition.CHART_COLUMN.MEAN)
                           || (seriesName == Definition.CHART_COLUMN.EWMAMEAN)
                           || (seriesName == Definition.CHART_COLUMN.EWMARANGE)
                           || (seriesName == Definition.CHART_COLUMN.EWMASTDDEV))
                        {
                            si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.AVG);
                        }
                        else
                            si.SeriesColor = chartUtil.GetSeriesColor(iCount++);

                        break;
                }
                this.DrawChart(si);
            }
        }
        //modified end SPC-851

        //modified by enkim 2012.05.25 SPC-851
        protected override void DrawChartWithRawSeriesInfo()
        {
            SeriesInfo si = null;
            ChartUtility chartUtil = new ChartUtility();
            DataTable dtTemp = new DataTable();
            ArrayList arrTemp = new ArrayList();
            Type tempType = null;
            LinkedList lnkList = new LinkedList();

            if (this.dtDataSource.Rows.Count == 0)
                return;

            for (int i = 0; i < this.dtDataSource.Columns.Count; i++)
            {
                string seriesName = this.dtDataSource.Columns[i].ColumnName;
                string seriesNameTemp = this.dtDataSource.Columns[i].ColumnName.Split('^')[0];

                if (seriesName == CommonChart.COLUMN_NAME_SEQ_INDEX)
                {
                    tempType = this.dtDataSource.Rows[0][i].GetType();
                }
                else if (lstRawColumn.Contains(seriesName))
                {
                    if (dtTemp.Columns.Contains(seriesNameTemp))
                        continue;
                    else if (!lnkList.Contains(seriesNameTemp))
                    {
                        dtTemp = new DataTable();
                        dtTemp.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, tempType);
                        if (!dtTemp.Columns.Contains("TIME"))
                        {
                            dtTemp.Columns.Add("TIME", typeof(DateTime));
                        }
                        if (!dtTemp.Columns.Contains(Definition.CHART_COLUMN.TIME2))
                        {
                            dtTemp.Columns.Add(Definition.CHART_COLUMN.TIME2, typeof(string));
                        }
                        for (int k = 0; k < this.dtDataSource.Rows.Count; k++)
                        {
                            if (this.dtDataSource.Rows[k][i] != null && this.dtDataSource.Rows[k][i].ToString().Length > 0)
                            {
                                dtTemp.Columns.Add(seriesNameTemp, this.dtDataSource.Rows[k][i].GetType());
                                break;
                            }
                        }
                        lnkList.Add(seriesNameTemp, dtTemp.Clone());
                        arrTemp.Add(seriesNameTemp);
                    }
                }
            }

            if (lnkList.Count > 0)
            {
                for (int i = 0; i < arrTemp.Count; i++)
                {
                    DataRow dr = null;
                    DataTable dt = new DataTable();
                    dt = ((DataTable)lnkList[arrTemp[i].ToString()]).Clone();
                    if (dt.Columns.Count > 3)
                    {
                        string sTempColumnName = dt.Columns[3].ColumnName;

                        for (int j = 0; j < this.dtDataSource.Rows.Count; j++)
                        {
                            for (int k = 0; k < this.dtDataSource.Columns.Count; k++)
                            {
                                if (this.dtDataSource.Columns[k].ColumnName.Split('^')[0] == sTempColumnName)
                                {
                                    if (this.dtDataSource.Rows[j][k] != null && this.dtDataSource.Rows[j][k].ToString().Length > 0)
                                    {
                                        dr = dt.NewRow();
                                        dr[0] = dtDataSource.Rows[j][CommonChart.COLUMN_NAME_SEQ_INDEX]; ;
                                        dr[1] = dtDataSource.Rows[j]["TIME"];
                                        dr[2] = dtDataSource.Rows[j][Definition.CHART_COLUMN.TIME2];
                                        dr[3] = dtDataSource.Rows[j][k];
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }

                        lnkList.Remove(arrTemp[i].ToString());
                        lnkList.Add(arrTemp[i].ToString(), dt);
                    }
                }

                int jcolor = 0;

                for (int i = 0; i < arrTemp.Count; i++)
                {
                    //si = new SeriesInfo(typeof(Line), arrTemp[i].ToString(), ((DataTable)lnkList[arrTemp[i].ToString()]), CommonChart.COLUMN_NAME_SEQ_INDEX, arrTemp[i].ToString());
                    si = new SeriesInfo(typeof(ExtremeBFastLine), arrTemp[i].ToString(), ((DataTable)lnkList[arrTemp[i].ToString()]), CommonChart.COLUMN_NAME_SEQ_INDEX, arrTemp[i].ToString());
                    si.TreatNulls = TreatNullsStyle.DoNotPaint;

                    si.SeriesColor = chartUtil.GetSeriesColor(jcolor);
                    if (si.SeriesColor == Color.Red || si.SeriesColor == Color.Black)
                    {
                        jcolor++;
                        si.SeriesColor = chartUtil.GetSeriesColor(jcolor);
                    }
                    this.DrawChart(si);
                    jcolor++;
                }
            }
            //si = new SeriesInfo(typeof(Line), seriesName, this.dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName);


            //switch (seriesName)
            //{
            //    case Definition.CHART_COLUMN.UCL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.UCL); break;
            //    case Definition.CHART_COLUMN.LCL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LCL); break;
            //    case Definition.CHART_COLUMN.LSL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LSL); break;
            //    case Definition.CHART_COLUMN.USL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.USL); break;
            //    case Definition.CHART_COLUMN.MIN: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.MIN); break;
            //    case Definition.CHART_COLUMN.MAX: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.MAX); break;
            //    case Definition.CHART_COLUMN.TARGET: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.TARGET); break;
            //    default:
            //        if ((seriesName == Definition.CHART_COLUMN.RAW)
            //           || (seriesName == Definition.CHART_COLUMN.STDDEV)
            //           || (seriesName == Definition.CHART_COLUMN.AVG)
            //           || (seriesName == Definition.CHART_COLUMN.RANGE)
            //           || (seriesName == Definition.CHART_COLUMN.MA)
            //           || (seriesName == Definition.CHART_COLUMN.MSD)
            //           || (seriesName == Definition.CHART_COLUMN.MEAN)
            //           || (seriesName == Definition.CHART_COLUMN.EWMAMEAN)
            //           || (seriesName == Definition.CHART_COLUMN.EWMARANGE)
            //           || (seriesName == Definition.CHART_COLUMN.EWMASTDDEV))
            //        {
            //            si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.AVG);
            //        }
            //        else
            //            si.SeriesColor = chartUtil.GetSeriesColor(iCount++);

            //        break;
            //}
            //this.DrawChart(si);
            //}
        }
        //modified end SPC-851


        //modified by enkim 2012.05.21 SPC-851
        protected override void DrawChartWithSeriesInfoForSumChart()
        {
            SeriesInfo si = null;
            ChartUtility chartUtil = new ChartUtility();
            int iCount = (int)ChartUtility.CHAERT_SERIES_COLOR.COUNT;

            for (int i = 0; i < this.dtDataSource.Columns.Count; i++)
            {
                string seriesName = this.dtDataSource.Columns[i].ColumnName;

                if (seriesName == CommonChart.COLUMN_NAME_SEQ_INDEX ||
                    seriesName == Definition.CHART_COLUMN.TIME ||
                    seriesName == Definition.CHART_COLUMN.TIME2 ||
                    seriesName == Definition.CHART_COLUMN.DTSOURCEID ||
                    seriesName == Definition.COL_TOGGLE_YN ||
                    lstRawColumn.Contains(seriesName))
                    continue;

                
                //if ((seriesName == Definition.CHART_COLUMN.RAW)
                //           || (seriesName == Definition.CHART_COLUMN.STDDEV)
                //           || (seriesName == Definition.CHART_COLUMN.AVG)
                //           || (seriesName == Definition.CHART_COLUMN.RANGE)
                //           || (seriesName == Definition.CHART_COLUMN.MA)
                //           || (seriesName == Definition.CHART_COLUMN.MSD)
                //           || (seriesName == Definition.CHART_COLUMN.MEAN)
                //           || (seriesName == Definition.CHART_COLUMN.EWMAMEAN)
                //           || (seriesName == Definition.CHART_COLUMN.EWMARANGE)
                //           || (seriesName == Definition.CHART_COLUMN.EWMASTDDEV))
                //{
                //    si = new SeriesInfo(typeof(ExtremeBFastLine), seriesName, this.dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName);
                //    si.TreatNulls = TreatNullsStyle.DoNotPaint;
                //}
                //else
                //{
                //    si = new SeriesInfo(typeof(ExtremeBFastLine), seriesName, this.dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName);
                //    si.TreatNulls = TreatNullsStyle.DoNotPaint;
                //}

                si = new SeriesInfo(typeof(ExtremeBFastLine), seriesName, this.dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName);
                si.TreatNulls = TreatNullsStyle.DoNotPaint;

                switch (seriesName)
                {
                    case Definition.CHART_COLUMN.UCL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.UCL); break;
                    case Definition.CHART_COLUMN.LCL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LCL); break;
                    case Definition.CHART_COLUMN.LSL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LSL); break;
                    case Definition.CHART_COLUMN.USL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.USL); break;
                    case Definition.CHART_COLUMN.MIN: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.MIN); break;
                    case Definition.CHART_COLUMN.MAX: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.MAX); break;
                    case Definition.CHART_COLUMN.TARGET: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.TARGET); break;
                    default:
                        if ((seriesName == Definition.CHART_COLUMN.RAW)
                           || (seriesName == Definition.CHART_COLUMN.STDDEV)
                           || (seriesName == Definition.CHART_COLUMN.AVG)
                           || (seriesName == Definition.CHART_COLUMN.RANGE)
                           || (seriesName == Definition.CHART_COLUMN.MA)
                           || (seriesName == Definition.CHART_COLUMN.MSD)
                           || (seriesName == Definition.CHART_COLUMN.MR)
                           || (seriesName == Definition.CHART_COLUMN.MEAN)
                           || (seriesName == Definition.CHART_COLUMN.EWMAMEAN)
                           || (seriesName == Definition.CHART_COLUMN.EWMARANGE)
                           || (seriesName == Definition.CHART_COLUMN.EWMASTDDEV))
                        {
                            si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.AVG);
                        }
                        else
                            si.SeriesColor = chartUtil.GetSeriesColor(iCount++);

                        break;
                }
                this.DrawChart(si);
            }
        }
        //modified end SPC-851

        //modified by enkim 2012.05.21 SPC-851
        protected override void DrawChartWithRawSeriesInfoForSumChart()
        {
            SeriesInfo si = null;
            ChartUtility chartUtil = new ChartUtility();
            DataTable dtTemp = new DataTable();
            ArrayList arrTemp = new ArrayList();
            Type tempType = null;
            LinkedList lnkList = new LinkedList();

            if (this.dtDataSource.Rows.Count == 0)
                return;

            for (int i = 0; i < this.dtDataSource.Columns.Count; i++)
            {
                string seriesName = this.dtDataSource.Columns[i].ColumnName;
                string seriesNameTemp = this.dtDataSource.Columns[i].ColumnName.Split('^')[0];

                if (seriesName == CommonChart.COLUMN_NAME_SEQ_INDEX)
                {
                    tempType = this.dtDataSource.Rows[0][i].GetType();
                }
                else if (lstRawColumn.Contains(seriesName))
                {
                    if (dtTemp.Columns.Contains(seriesNameTemp))
                        continue;
                    else if (!lnkList.Contains(seriesNameTemp))
                    {
                        dtTemp = new DataTable();
                        dtTemp.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, tempType);
                        if (!dtTemp.Columns.Contains("TIME"))
                        {
                            dtTemp.Columns.Add("TIME", typeof(DateTime));
                        }
                        if (!dtTemp.Columns.Contains(Definition.CHART_COLUMN.TIME2))
                        {
                            dtTemp.Columns.Add(Definition.CHART_COLUMN.TIME2, typeof(string));
                        }

                        for (int k = 0; k < this.dtDataSource.Rows.Count; k++)
                        {
                            if (this.dtDataSource.Rows[k][i] != null && this.dtDataSource.Rows[k][i].ToString().Length > 0)
                            {
                                dtTemp.Columns.Add(seriesNameTemp, this.dtDataSource.Rows[k][i].GetType());
                                break;
                            }
                        }
                        lnkList.Add(seriesNameTemp, dtTemp.Clone());
                        arrTemp.Add(seriesNameTemp);
                    }
                }
            }

            if (lnkList.Count > 0)
            {
                for (int i = 0; i < arrTemp.Count; i++)
                {
                    DataRow dr = null;
                    DataTable dt = new DataTable();
                    dt = ((DataTable)lnkList[arrTemp[i].ToString()]).Clone();
                    if (dt.Columns.Count > 3)
                    {
                        string sTempColumnName = dt.Columns[3].ColumnName;
                        for (int j = 0; j < this.dtDataSource.Rows.Count; j++)
                        {
                            for (int k = 0; k < this.dtDataSource.Columns.Count; k++)
                            {
                                if (this.dtDataSource.Columns[k].ColumnName.Split('^')[0] == sTempColumnName)
                                {
                                    if (this.dtDataSource.Rows[j][k] != null && this.dtDataSource.Rows[j][k].ToString().Length > 0)
                                    {
                                        dr = dt.NewRow();
                                        dr[0] = dtDataSource.Rows[j][CommonChart.COLUMN_NAME_SEQ_INDEX];
                                        dr[1] = dtDataSource.Rows[j]["TIME"];
                                        dr[2] = dtDataSource.Rows[j][Definition.CHART_COLUMN.TIME2];
                                        dr[3] = dtDataSource.Rows[j][k];
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }

                        lnkList.Remove(arrTemp[i].ToString());
                        lnkList.Add(arrTemp[i].ToString(), dt);
                    }
                }

                int jcolor = 0;

                for (int i = 0; i < arrTemp.Count; i++)
                {
                    si = new SeriesInfo(typeof(ExtremeBFastLine), arrTemp[i].ToString(), ((DataTable)lnkList[arrTemp[i].ToString()]), CommonChart.COLUMN_NAME_SEQ_INDEX, arrTemp[i].ToString());
                    si.TreatNulls = TreatNullsStyle.DoNotPaint;
                    
                    si.SeriesColor = chartUtil.GetSeriesColor(jcolor);
                    if (si.SeriesColor == Color.Red || si.SeriesColor == Color.Black)
                    {
                        jcolor++;
                        si.SeriesColor = chartUtil.GetSeriesColor(jcolor);
                    }
                    this.DrawChart(si);
                    jcolor++;
                }
            }
        }
        //modified end SPC-851

        public override string[] UsedParameters(Steema.TeeChart.Styles.Series s)
        {
            return _parameters;
        }

        public override void DrawCrossLine_WithParametersSEQIndex(string seqIndex)
        {

            foreach (Series s in this.SPCChart.Chart.Series)
            {
                if (s.Visible)
                {
                    SeriesInfo seriesInfo = this.SPCChart.GetSeriesInfo(s);

                    string filter = CommonChart.COLUMN_NAME_SEQ_INDEX + "='" + seqIndex + "'";
                    DataRow[] selectedRow = seriesInfo.SeriesData.Select(filter);

                    if (selectedRow.Length == 1)
                    {
                        string yValue = selectedRow[0][seriesInfo.SeriesName].ToString();

                        DrawCrossLine(Convert.ToDouble(seqIndex), Convert.ToDouble(yValue));
                    }

                }
            }

        }

        #endregion


        #region EventHandler



        #endregion


        #region User Defined Method.
        protected void MakeDataTable(LinkedList lstChartColumn)
        {
            for (int i = 0; i < lstChartColumn.Count; i++)
            {
                string strKey = lstChartColumn.GetKey(i).ToString();
                string strValue = lstChartColumn.GetValue(i).ToString();

                if (this.dtDataSource.Columns.Contains(strValue)) continue;

                if (strKey == Definition.CHART_COLUMN.TIME)
                {
                    this.dtDataSource.Columns.Add(strValue, typeof(DateTime));
                    this.dtDataSource.Columns.Add(Definition.CHART_COLUMN.TIME2, typeof(string));
                }
                else if (strKey == Definition.CHART_COLUMN.RAW)
                {
                    if (lstRawColumn.Count == 0 || lstRawColumn.Count == 1)
                        this.dtDataSource.Columns.Add(strValue, typeof(float));
                }
                else if (strKey == Definition.COL_TOGGLE_YN)
                {
                    this.dtDataSource.Columns.Add(strValue, typeof(string));
                }
                else
                {
                    this.dtDataSource.Columns.Add(strValue, typeof(string));
                }
            }

            DataRow dr = null;
            for (int i = 0; i < this.DataManager.DataRowCount; i++)
            {
                dr = this.dtDataSource.NewRow();
                for (int j = 0; j < lstChartColumn.Count; j++)
                {
                    string strKey = lstChartColumn.GetKey(j).ToString();
                    string strValue = lstChartColumn.GetValue(j).ToString();

                     if (this.DataManager.RawDataTable.Columns.Contains(strKey))
                    {
                        if (strKey == Definition.CHART_COLUMN.TIME)
                        {
                            string sTime = this.DataManager.RawDataTable.Rows[i][strKey].ToString();
                            dr[Definition.CHART_COLUMN.TIME2] = sTime.Substring(0, 16);
                            dr[strKey] = this.DataManager.RawDataTable.Rows[i][strKey];
                        }
                        else if (strKey == Definition.CHART_COLUMN.RAW)
                        {
                            if (lstRawColumn.Count == 0)
                            {
                                dr[strValue] = this.DataManager.RawDataTable.Rows[i][strKey];
                            }
                        }
                        else
                            dr[strValue] = this.DataManager.RawDataTable.Rows[i][strKey];
                    }
                }
                dr[CommonChart.COLUMN_NAME_SEQ_INDEX] = i.ToString();
                if (this.Name != Definition.CHART_TYPE.RAW)
                {
                    if (this.Name != Definition.CHART_TYPE.XBAR && this.Name != Definition.CHART_TYPE.EWMA_MEAN)
                    {
                        if (this.Name.Contains("EWMA"))
                        {
                            if (dr[this.Name.Replace("EWMA_", "")].ToString() == "")
                                continue;
                            else
                                this.dtDataSource.Rows.Add(dr);
                        }
                        else
                        {
                            if (dr[this.Name].ToString() == "")
                                continue;
                            else
                                this.dtDataSource.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        if (dr["AVG"].ToString() == "")
                            continue;
                        else
                            this.dtDataSource.Rows.Add(dr);
                    }
                }
                else
                {
                    this.dtDataSource.Rows.Add(dr);
                }
            }
        }

        public void SettingXBarInfo(string xBarChartType, string sXLabel, List<string> _lstRawColumn, LinkedList _llstChartSeriesVisibleType)
        {
            this._xBarChartType = xBarChartType;
            this.xLabel = sXLabel;
            this.lstRawColumn = _lstRawColumn;
            this.llstChartSeriesVisibleType = _llstChartSeriesVisibleType;
            this.isViewButtonList = true;
            this.IsVisibleLegendScrollBar = true;
            this.IsVisibleShadow = true;

            this.InitializePage();

        }


        private string ChartSeriesGroupName(string sSeriesName)
        {

            string sGroupName = string.Empty;
            if (sSeriesName == Definition.CHART_COLUMN.UCL || sSeriesName == Definition.CHART_COLUMN.LCL)
                sGroupName = this._mlthandler.GetVariable(Definition.CHART_SERIES.CONTROL_LIMIT);
            else if (sSeriesName == Definition.CHART_COLUMN.USL || sSeriesName == Definition.CHART_COLUMN.LSL)
                sGroupName = this._mlthandler.GetVariable(Definition.CHART_SERIES.SPEC);
            else if (sSeriesName == Definition.CHART_COLUMN.MIN || sSeriesName == Definition.CHART_COLUMN.MAX)
                sGroupName = this._mlthandler.GetVariable(Definition.CHART_SERIES.MIN_MAX);
            else
            {
                if (this.lstRawColumn.Contains(sSeriesName))
                    sGroupName = this._mlthandler.GetVariable(Definition.CHART_SERIES.POINT);
                else
                    sGroupName = sSeriesName;



            }

            return sGroupName;
        }
        #endregion
    }
}
