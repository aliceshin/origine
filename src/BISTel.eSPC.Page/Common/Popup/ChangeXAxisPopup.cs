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
    public partial class ChangeXAxisPopup : BasePopupFrm
    {
        BaseChart _baseChart;
        BaseCalcChart _baseCalcChart;
        BaseRawChart _baseRawChart;
        BaseRawChartView _baseRawChartView;
        BaseAnalysisChart _baseAnalysisChart;
        SPC_CHART_TYPE _spc_chart_type = SPC_CHART_TYPE.BASECHART;        
        DataSet _ds = new DataSet();
        
        CommonUtility _ComUtil;
        LinkedList _lk = new LinkedList();
        string _sColumn = null;
        private DialogResult _digResult = new DialogResult();

        public DialogResult ButtonResult
        {
            get { return this._digResult; }
            set { this._digResult = value; }
        }

        public ChangeXAxisPopup()
        {
            InitializeComponent();
            this._ComUtil = new CommonUtility();                  
            InitializeLayout();
        }


        public DataSet ds
        {
            get { return _ds; }
            set { _ds = value; }
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
            bcb_Kind.Items.Clear();
            _lk.Add("VALUE", "VALUE");
            _lk.Add("TIME", "TIME");
            bcb_Kind.Items.Add("VALUE");
            bcb_Kind.Items.Add("TIME");
            for (int i = 0; i < _ds.Tables[0].Columns.Count; i++)
            {
                string sUpper = _ds.Tables[0].Columns[i].ColumnName.ToUpper();
                if (sUpper.Contains("TIME") || sUpper.Contains("VALUE"))
                {
                    continue;
                }
                else
                {
                    _lk.Add(sUpper, sUpper);
                    bcb_Kind.Items.Add(sUpper);
                }
            }
            if (_ds.Tables.Count >= 2)
            {
                for (int i = 1; i < _ds.Tables.Count; i++)
                {
                    for (int j = 0; j < _ds.Tables[i].Columns.Count; j++)
                    {
                        string sColumnUpper = _ds.Tables[i].Columns[j].ColumnName.ToUpper();
                        try
                        {

                            if (!(_lk.Contains(sColumnUpper)))
                            {
                                _lk.Add(sColumnUpper, sColumnUpper);
                                bcb_Kind.Items.Add(sColumnUpper);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine(ex.ToString());
                            LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                        }
                    }
                }
            }
        }

        private void bcb_Kind_SelectedValueChanged(object sender, EventArgs e)
        {
            _sColumn = bcb_Kind.SelectedItem.ToString();
        }

        public string getXAxisColumn()
        {
            return _sColumn;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.ButtonResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ButtonResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
