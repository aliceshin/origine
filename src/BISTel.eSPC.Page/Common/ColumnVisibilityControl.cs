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

namespace BISTel.eSPC.Page.Common
{
    public partial class ColumnVisibilityControl : UserControl
    {
        public ColumnVisibilityControl()
        {
            InitializeComponent();
        }

        BISTel.eSPC.Common.CommonUtility _ComUtil;

        BISTel.PeakPerformance.Client.BISTelControl.BSpread _bsprData;

        SortedList _slColumnList;

        [Editor(typeof(string), typeof(string))]
        public string sLabel
        {
            get { return this.blblLabel.Text; }
            set { this.blblLabel.Text = value; }
        }

        [Editor(typeof(SortedList), typeof(SortedList))]
        public SortedList slColumnList
        {
            get { return this._slColumnList; }
            set { this._slColumnList = value; }
        }

        public string SelectedText
        {
            get { return this.bCheckCombo.Text; }
        }

        public string SelectedValue
        {

            get { return this.bCheckCombo.SelectedValue.ToString(); }
        }

        public BSpread bsprParentData
        {
            set { this._bsprData = value; }

            get { return this._bsprData; }
        }

        public void InitializeControl()
        {
            this._ComUtil = new BISTel.eSPC.Common.CommonUtility();

            this._bsprData = new BSpread();

            this._slColumnList = new SortedList();

            InitializeLayout();
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
        public void DataBinding(ArrayList alVisibleColumnList, SortedList slDefaultVisibleList)
        {
            this.bCheckCombo.Items.Clear();
            string[] saVisibleColumnList = (string[])alVisibleColumnList.ToArray(typeof(string));
            this.bCheckCombo.AddItems(saVisibleColumnList);

            for (int i = 0; i < this.bCheckCombo.chkBox.Items.Count; i++)
            {
                string sItem = this.bCheckCombo.chkBox.Items[i].ToString();
                bool bChecked = Convert.ToBoolean(slDefaultVisibleList[sItem]);

                this.bCheckCombo.chkBox.SetItemChecked(i, bChecked);

                //if (!bChecked)
                //{
                //    this._bsprData.ActiveSheet.Columns[i].Visible = bChecked;
                //}
            }
            //_ComUtil.SetBComboBoxData(this.bCheckCombo, llKeyValueData, "", true);
        }

        public void DataBinding(DataSet dataSource, string displaymember, string valuemember)
        {
            //_ComUtil.SetBComboBoxData(this.bCheckCombo, dataSource, displaymember, valuemember, "", true);
        }

        public bool bEnable
        {
            get { return this.bCheckCombo.Enabled; }
            set { this.bCheckCombo.Enabled = value; }
        }

        public string sName
        {
            get { return this.Name; }
            set { this.Name = value; }
        }

        private void bCheckCombo_Item_Check(object sender, ItemCheckEventArgs e)
        {
            string sItem = this.bCheckCombo.chkBox.Items[e.Index].ToString();
            bool bChecked = false;


            int iColIndex = Convert.ToInt32(this._slColumnList[sItem]);

            if (CheckState.Checked == e.NewValue)
                bChecked = true;

            this._bsprData.ActiveSheet.Columns[iColIndex].Visible = bChecked;
        }

    }
}
