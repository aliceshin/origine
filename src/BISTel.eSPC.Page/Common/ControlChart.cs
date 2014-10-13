using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using Steema.TeeChart.Styles;
using BISTel.PeakPerformance.Statistics.Application.ControlChart;

namespace BISTel.eSPC.Page.Common
{
    public partial class ControlChart : BaseSPCChart
    {
      
        string  _xBarChartType = Definition.CHART_TYPE.XBAR;                
        string[] _parameters;
       

        public ControlChart(SourceDataManager dataManager)
        {
            InitializeComponent();            

            this.DataManager = dataManager;          
        }

        


        #region PageLoad & Initialize
        
        #endregion 

        #region Override Method.
        protected override void MakeDataSourceToDrawSPCChart()
        {
        
            DataRow dr = null;      
            this._dtDataSource = new DataTable();
            this._dtDataSource.Columns.Add(CommonChart.COLUMN_NAME_SEQ_INDEX, typeof(string));                                                                             
            switch(this._xBarChartType)    
            {
                case Definition.CHART_TYPE.XBAR:                    
                    MakeDataTable(CommonChart.CHART_XBAR_COLUMN);                                            
                    break;
                case Definition.CHART_TYPE.RANGE:
                    MakeDataTable(CommonChart.CHART_RANGE_COLUMN);                    
                    break;
                case Definition.CHART_TYPE.STDDEV:
                    MakeDataTable(CommonChart.CHART_STDDEV_COLUMN);
                    break;     
                case Definition.CHART_TYPE.RAW:
                
                    LinkedList llstRawColumn = new LinkedList();
                    for(int i=0; i<CommonChart.CHART_RAW_COLUMN.Count; i++)
                    {
                        string strKey = CommonChart.CHART_RAW_COLUMN.GetKey(i).ToString();
                        string strValue = CommonChart.CHART_RAW_COLUMN.GetValue(i).ToString();
                        if(strKey==Definition.CHART_COLUMN.RAW)
                        {
                            for(int j=0; j<lstRawColumn.Count; j++)
                            {
                                llstRawColumn.Add(lstRawColumn[j].ToString(), lstRawColumn[j].ToString());
                            }
                            
                        }else
                        {
                            llstRawColumn.Add(strKey, strValue);
                        }
                    }
                    MakeDataTable(llstRawColumn);
                    break;     
                case Definition.CHART_TYPE.MA:
                    MakeDataTable(CommonChart.CHART_MA_COLUMN);
                    break;     
                case Definition.CHART_TYPE.MSD:
                    MakeDataTable(CommonChart.CHART_MSD_COLUMN);
                    break;     
                case Definition.CHART_TYPE.EWMA_MEAN:
                    MakeDataTable(CommonChart.CHART_EWMA_MEAN_COLUMN);
                    break;      
                case Definition.CHART_TYPE.EWMA_RANGE:
                    MakeDataTable(CommonChart.CHART_EWMA_RANGE_COLUMN);
                    break;      
                case Definition.CHART_TYPE.EWMA_STDDEV:
                    MakeDataTable(CommonChart.CHART_EWMA_STDDEV_COLUMN);                                                                                
                    break;      
            }

           
                                                                   
        }

        public override string[] UsedParameters(Steema.TeeChart.Styles.Series s)
        {
            return _parameters;
        }

        public override void DrawCrossLine_WithParametersSEQIndex(string[] parameters, string seqIndex)
        {
           
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
                if(strKey==Definition.CHART_COLUMN.TIME)
                    this._dtDataSource.Columns.Add(strValue, typeof(DateTime));
                else
                {
                    this._dtDataSource.Columns.Add(strValue, typeof(string));
                }
            }

            DataRow dr = null;
            for (int i = 0; i < this.DataManager.DataRowCount; i++)
            {
                dr = this._dtDataSource.NewRow();
                for (int j = 0; j < lstChartColumn.Count; j++)
                {
                    string strKey = lstChartColumn.GetKey(j).ToString();
                    string strValue = lstChartColumn.GetValue(j).ToString();
                    dr[strValue] = this.DataManager.RawDataTable.Rows[i][strKey];
                }
                dr[CommonChart.COLUMN_NAME_SEQ_INDEX] = i.ToString();
                this._dtDataSource.Rows.Add(dr);
            }  
            
        }

        public void SettingXBarInfo(string xBarChartType, string sXLabel, List<string> _lstRawColumn, LinkedList _llstChartSeriesVisibleType)
        {
            this._xBarChartType = xBarChartType;                        
            this._xLabel = sXLabel;
            this.lstRawColumn = _lstRawColumn; 
            this.llstChartSeriesVisibleType =_llstChartSeriesVisibleType ;                                   
        }
        
        #endregion 
    }
}
