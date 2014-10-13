using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Condition.Controls.Interface;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;

namespace BISTel.eSPC.Condition.Controls.Date
{
    public partial class PpkDateCondition : UserControl
    {

        private eSPCWebService.eSPCWebService _wsSPC = null;
        private MultiLanguageHandler _mlthandler;
        private Initialization _Initialization;
        private string _sType = string.Empty;
        private string sStartTime = string.Empty;
        private string sEndTime = string.Empty;

        public PpkDateCondition()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            InitializeComponent();

        }

        public void InitializePage()
        {
            this.dtpStart.Value = DateTime.Now.AddDays(-31);
            this.dtpEnd.Value = DateTime.Now.AddDays(-1);
            this._sType = Definition.PERIOD_PPK.DAILY;
            this.cboType.Items.Clear();
        }

        public BComboBox ComboType
        {
            get
            {
                return this.cboType;
            }
        }

        public string PeriodType
        {
            get
            {
                return this._sType;
            }
            set
            {
                _sType = value;
                for (int i = 0; i < this.cboType.Items.Count; i++)
                {
                    if (value == this.cboType.Items[i].ToString())
                        this.cboType.SelectedIndex = i;
                }
            }
        }

        public void AddItems(DataSet ds)
        {
            this.cboType.Items.Clear();
            if (!DataUtil.IsNullOrEmptyDataSet(ds))
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sKey = ds.Tables[0].Rows[i]["CODE"].ToString();
                    this.cboType.Items.Add(sKey);

                    if (sKey == PeriodType)
                        this.cboType.SelectedIndex = i;
                }
            }
        }

        public DataTable GetDateTimeSelectedValue(string _sDateType)
        {
            string strDateTime = string.Empty;

            if (_sDateType.Equals("F"))
                strDateTime = this.dtpStart.Value.ToString(Definition.DATETIME_FORMAT_MS);
            else
                strDateTime = this.dtpEnd.Value.ToString(Definition.DATETIME_FORMAT_MS);

            DataTable dtValue = DCUtil.MakeDataTableForDCValue(strDateTime, strDateTime);
            return dtValue;
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                ComboBox cbo = sender as ComboBox;
                if (cbo == null) return;
                if (cbo.SelectedItem == null) return;
                this._sType = cbo.SelectedItem.ToString();

            }
            catch (System.Web.Services.Protocols.SoapException sex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, sex.Message);
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }

        }



        public DateTime DateTimeStart
        {
            set
            {
                dtpStart.Value = value;
            }
            get
            {
                return dtpStart.Value;
            }
        }


        public DateTime DateTimeEnd
        {
            set
            {
                dtpEnd.Value = value;
            }
            get
            {
                return dtpEnd.Value;
            }
        }



    }
}
