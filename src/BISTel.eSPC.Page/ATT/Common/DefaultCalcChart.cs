using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BISTel.eSPC.Common.ATT;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using Steema.TeeChart.Styles;
using BISTel.PeakPerformance.Statistics.Application.ControlChart;

namespace BISTel.eSPC.Page.ATT.Common
{
    public partial class DefaultCalcChart : BaseCalcChart
    {
      
        string  _xBarChartType = Definition.CHART_TYPE.P;                
        string[] _parameters;
       

        public DefaultCalcChart(SourceDataManager dataManager)
        {
            InitializeComponent();            
            this.DataManager = dataManager;                                    
       
        }

        #region PageLoad & Initialize
        
        #endregion 

        #region Override Method.

        protected override void MakeDataSourceToDrawSPCChart(bool sample, int samplePeriod, int sampleCount)
        {
        
            DataRow dr = null;      
            this.dtDataSource = new DataTable();
            this.dtDataSource.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, typeof(int));   
            CommonChart.llstChartColumn = new LinkedList();
            LinkedList llstChartColumn = CommonChart.GetChartSeries(this._xBarChartType,lstRawColumn);
            if (llstChartColumn.Contains(Definition.COL_TOGGLE))
            {
                llstChartColumn.Remove(Definition.COL_TOGGLE);
            }
            if (llstChartColumn.Contains(Definition.COL_TOGGLE_YN))
            {
                llstChartColumn.Remove(Definition.COL_TOGGLE_YN);
            }
            MakeDataTable(llstChartColumn, sample, samplePeriod, sampleCount);
        }



        protected override void DrawChartWithSeriesInfo()
        {
            SeriesInfo si = null;
            ChartUtility chartUtil = new ChartUtility();
            int iCount = (int)ChartUtility.CHAERT_SERIES_COLOR.COUNT;

            for (int i = 0; i < this.dtDataSource.Columns.Count; i++)
            {
                string seriesName = this.dtDataSource.Columns[i].ColumnName;

                if (seriesName == CommonChart.COLUMN_NAME_SEQ_INDEX || seriesName == Definition.CHART_COLUMN.TIME || seriesName == Definition.CHART_COLUMN.TIME2 || 
                    seriesName == Definition.COL_TOGGLE || seriesName == Definition.COL_TOGGLE_YN || seriesName == Definition.CHART_COLUMN.DTSOURCEID || 
                    lstRawColumn.Contains(seriesName))
                    continue;
                si = new SeriesInfo(typeof(Line), seriesName, this.dtDataSource, CommonChart.COLUMN_NAME_SEQ_INDEX, seriesName);

                
                switch (seriesName)
                {
                    case Definition.CHART_COLUMN.UCL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.UCL); break;
                    case Definition.CHART_COLUMN.LCL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LCL); break;
                    case Definition.CHART_COLUMN.LSL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.LSL); break;
                    case Definition.CHART_COLUMN.USL: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.USL); break;
                    case Definition.CHART_COLUMN.TARGET: si.SeriesColor = chartUtil.GetSeriesColor((int)ChartUtility.CHAERT_SERIES_COLOR.TARGET); break;
                    default:
                        if ((seriesName == Definition.CHART_COLUMN.P)
                           || (seriesName == Definition.CHART_COLUMN.PN)
                           || (seriesName == Definition.CHART_COLUMN.C)
                           || (seriesName == Definition.CHART_COLUMN.U))
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


        #region User Defined Method.

        /// <param name="lstChartColumn"></param>
        /// <param name="sample">Whether sampling data or not</param>
        /// <param name="samplePeriod">sampling period</param>
        /// <param name="sampleCount">sample count in the sampling period</param>
        protected void MakeDataTable(LinkedList lstChartColumn, bool sample, int samplePeriod, int sampleCount)
        {
            for (int i = 0; i < lstChartColumn.Count; i++)
            {
                string strKey = lstChartColumn.GetKey(i).ToString();
                string strValue = lstChartColumn.GetValue(i).ToString();
                
                if(this.dtDataSource.Columns.Contains(strValue)) continue;

                if (strKey == Definition.CHART_COLUMN.TIME)
                {
                    this.dtDataSource.Columns.Add(strValue, typeof(DateTime));
                    this.dtDataSource.Columns.Add(Definition.CHART_COLUMN.TIME2, typeof(string));
                }
                else
                {
                    this.dtDataSource.Columns.Add(strValue, typeof(string));
                }
            }

            DateTime nextSampleStartDate =
                                    DateTime.Parse(this.DataManager.RawDataTable.Rows[0][Definition.CHART_COLUMN.TIME].ToString().Split(';')[0]);
                                nextSampleStartDate = new DateTime(nextSampleStartDate.Year, nextSampleStartDate.Month, nextSampleStartDate.Day);
            int sampleCountTemp = 0;

            DataRow dr = null;
            int seqIndex = 0;
            for (int i = 0; i < this.DataManager.DataRowCount; i++)
            {
                if(sample)
                {
                    if (DateTime.Parse(this.DataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.TIME].ToString().Split(';')[0]) > nextSampleStartDate)
                    {
                        while (DateTime.Parse(this.DataManager.RawDataTable.Rows[i][Definition.CHART_COLUMN.TIME].ToString().Split(';')[0]) > nextSampleStartDate)
                        {
                            nextSampleStartDate = nextSampleStartDate.AddDays(samplePeriod);
                        }
                        sampleCountTemp = 1;
                    }
                    else if (sampleCountTemp < sampleCount)
                    {
                        sampleCountTemp++;
                    }
                    else
                    {
                        this.DataManager.RawDataTable.Rows.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                dr = this.dtDataSource.NewRow();
                for (int j = 0; j < lstChartColumn.Count; j++)
                {
                    string strKey = lstChartColumn.GetKey(j).ToString();
                    string strValue = lstChartColumn.GetValue(j).ToString();
                    
                    if(this.DataManager.RawDataTable.Columns.Contains(strKey))
                    {
                        if (strKey == Definition.CHART_COLUMN.TIME)
                        {
                            string sTime = this.DataManager.RawDataTable.Rows[i][strKey].ToString();
                            dr[Definition.CHART_COLUMN.TIME2] = sTime.Substring(0, 10);
                            dr[strKey] = this.DataManager.RawDataTable.Rows[i][strKey];
                        }
                        else
                        {
                            dr[strValue] = this.DataManager.RawDataTable.Rows[i][strKey];
                        }
                    }
                }
                dr[CommonChart.COLUMN_NAME_SEQ_INDEX] = seqIndex.ToString();
                seqIndex++;
                this.dtDataSource.Rows.Add(dr);
            }  
        }

        public void SettingXBarInfo(string xBarChartType, string sXLabel, List<string> _lstRawColumn, LinkedList _llstChartSeriesVisibleType)
        {
            this._xBarChartType = xBarChartType;
            this.xLabel = sXLabel;
            this.lstRawColumn = _lstRawColumn; 
            this.llstChartSeriesVisibleType =_llstChartSeriesVisibleType ;     
            this.isViewButtonList = true; 
            this.IsVisibleLegendScrollBar=true;
            this.IsVisibleShadow =true;
                            
            this.InitializePage();
                            
        }


        private string ChartSeriesGroupName(string sSeriesName)
        {
        
            string sGroupName = string.Empty;
            if (sSeriesName == Definition.CHART_COLUMN.UCL || sSeriesName == Definition.CHART_COLUMN.LCL)
            {
                sGroupName = this._mlthandler.GetVariable(Definition.CHART_SERIES.CONTROL_LIMIT);
            }
            else if (sSeriesName == Definition.CHART_COLUMN.USL || sSeriesName == Definition.CHART_COLUMN.LSL)
            {
                sGroupName = this._mlthandler.GetVariable(Definition.CHART_SERIES.SPEC);
            }
            else
            {
                if (this.lstRawColumn.Contains(sSeriesName))
                {
                    sGroupName = this._mlthandler.GetVariable(Definition.CHART_SERIES.POINT);
                }
                else
                {
                    sGroupName = sSeriesName;
                }
            }
            return sGroupName;
        }
        #endregion 
    }
}
