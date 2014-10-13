using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    public partial class ZoomPopup : BasePopupFrm
    {
        BaseChart _baseChart;
        BaseCalcChart _baseCalcChart;
        BaseRawChart _baseRawChart;
        BaseRawChartView _baseRawChartView;
        BaseAnalysisChart _baseAnalysisChart;
        SPC_CHART_TYPE _spc_chart_type = SPC_CHART_TYPE.BASECHART;
        CommonUtility _ComUtil;        
        
        public ZoomPopup()
        {
            InitializeComponent();
            this._ComUtil = new CommonUtility();              
            InitializeLayout();
        }

        public void InitializeLayout()
        {
            try
            {
                Image bulletImg = this._ComUtil.GetImage(Definition.BUTTON_KEY_BULLET);
                this.blblbullet01.Image = bulletImg;
                
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
        }
    
        public SPC_CHART_TYPE SPC_CHART_TYPE 
        {
            get { return _spc_chart_type; }
            set { _spc_chart_type = value; }
        }

        public BaseChart BaseChart
        {
            get { return _baseChart; }
            set { _baseChart = value; }
        }

        public BaseCalcChart BaseCalcChart
        {
            get { return _baseCalcChart; }
            set { _baseCalcChart = value; }
        }

        public BaseRawChart BaseRawChart
        {
            get { return _baseRawChart; }
            set { _baseRawChart = value; }
        }

        public BaseRawChartView BaseRawChartView
        {
            get { return _baseRawChartView; }
            set { _baseRawChartView = value; }
        }

        public BaseAnalysisChart BaseAnalysisChart
        {
            get { return _baseAnalysisChart; }
            set { _baseAnalysisChart = value; }
        }
       
        private void bcb_Kind_SelectedValueChanged(object sender, EventArgs e)
        {
           
            if(SPC_CHART_TYPE == SPC_CHART_TYPE.BASECHART)
            {      
                if (bcb_Kind.Text == "Vertical")
                    BaseChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Vertical;                       
                else if(bcb_Kind.Text == "Horizontal")
                    BaseChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Horizontal;                            
                else if (bcb_Kind.Text == "Both")
                    BaseChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Both;                
            }
            //SPC-805 - BaseChart -> BaseRawChart 변경 Louis  
            else if (SPC_CHART_TYPE == SPC_CHART_TYPE.BASERAWCHART)
            {
                if (bcb_Kind.Text == "Vertical")
                    BaseRawChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Vertical;
                else if (bcb_Kind.Text == "Horizontal")
                    BaseRawChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Horizontal;
                else if (bcb_Kind.Text == "Both")
                    BaseRawChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Both;        
            }

            else if (SPC_CHART_TYPE == SPC_CHART_TYPE.BASECALCCHART)
            {
                if (bcb_Kind.Text == "Vertical")
                    BaseCalcChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Vertical;
                else if (bcb_Kind.Text == "Horizontal")
                    BaseCalcChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Horizontal;
                else if (bcb_Kind.Text == "Both")
                    BaseCalcChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Both;
            }
            else
            {
                if (bcb_Kind.Text == "Vertical")
                    BaseAnalysisChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Vertical;
                else if (bcb_Kind.Text == "Horizontal")
                    BaseAnalysisChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Horizontal;
                else if (bcb_Kind.Text == "Both")
                    BaseAnalysisChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Both;
            }
            
            this.Close();
        }
    }
}
