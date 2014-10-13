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
using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Page.ATT.Common
{
    public partial class ZoomPopup : BasePopupFrm
    {
        BaseChart _baseChart;
        BaseCalcChart _baseCalcChart;
        BISTel.eSPC.Common.SPC_CHART_TYPE _spc_chart_type = BISTel.eSPC.Common.SPC_CHART_TYPE.BASECHART;
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

        public BISTel.eSPC.Common.SPC_CHART_TYPE SPC_CHART_TYPE 
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

        private void bcb_Kind_SelectedValueChanged(object sender, EventArgs e)
        {
           
            if(SPC_CHART_TYPE == BISTel.eSPC.Common.SPC_CHART_TYPE.BASECHART)
            {      
                if (bcb_Kind.Text == "Vertical")
                    BaseChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Vertical;                       
                else if(bcb_Kind.Text == "Horizontal")
                    BaseChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Horizontal;                            
                else if (bcb_Kind.Text == "Both")
                    BaseChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Both;                
            }
            else if (SPC_CHART_TYPE == BISTel.eSPC.Common.SPC_CHART_TYPE.BASECALCCHART)
            {
                if (bcb_Kind.Text == "Vertical")
                    BaseCalcChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Vertical;
                else if (bcb_Kind.Text == "Horizontal")
                    BaseCalcChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Horizontal;
                else if (bcb_Kind.Text == "Both")
                    BaseCalcChart.Get_Chart().ZoomDirection = Steema.TeeChart.ZoomDirections.Both;
            }
            
            this.Close();
        }
    }
}
