using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BISTelControl;

using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    public partial class ComboBoxControl : UserControl
    {
        CommonUtility _ComUtil = new CommonUtility();

        public delegate void SelectedIndexChanged(string ControlName, BComboBox sender);

        public event SelectedIndexChanged selectedIndexChanged;

        public ComboBoxControl()
        {
            InitializeComponent();
            InitializeLayout();
        }

        [Editor(typeof(string), typeof(string))]
        public string sLabel
        {
            get { return this.blblLabel.Text; }
            set { this.blblLabel.Text = value; }
        }

        public string SelectedText
        {
            get { return this.bcboData.Text; }
        }

        public string SelectedValue
        {
            get
            {
                string sValue = "";

                if (this.bcboData.SelectedValue != null)
                    sValue = this.bcboData.SelectedValue.ToString();
                return sValue;
            }
        }

        public void DataBinding(LinkedList llKeyValueData)
        {
            _ComUtil.SetBComboBoxData(this.bcboData, llKeyValueData, "", true);
        }

        public void DataBinding(DataSet dataSource, string displaymember, string valuemember)
        {
            _ComUtil.SetBComboBoxData(this.bcboData, dataSource, displaymember, valuemember, "", true);
        }

        public bool bEnable
        {
            get { return this.bcboData.Enabled; }
            set { this.bcboData.Enabled = value; }
        }

        public string sName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        public ComboBoxStyle DropDownStyle
        {
            get { return this.bcboData.DropDownStyle; }
            set { this.bcboData.DropDownStyle = value; }
        }

        public void InitializeLayout()
        {
            try
            {
                Image bulletImg = this._ComUtil.GetImage(BISTel.eSPC.Common.Definition.BUTTON_KEY_BULLET);
                this.blblbullet01.Image = bulletImg;
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(BISTel.eSPC.Common.Definition.APPLICATION_NAME, ex);
            }
        }

        private void bcboData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedIndexChanged != null)
            {
                selectedIndexChanged(this.Name, bcboData);
            }
        }

    }
}
