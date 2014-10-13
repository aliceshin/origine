using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Interface;
using System.Collections;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;

namespace BISTel.eSPC.Condition.Controls.Default
{
    public partial class ComboControl : ComboInterface
    {
        #region : Field

        #endregion

        #region : Initialization

        //public ComboControl()
        //{
        //    InitializeComponent();
        //}

        public ComboControl(object parent)
        {
            InitializeComponent();
            _parent = (DefaultCondition)parent;
        }

        #endregion

        #region : Editor

        [Editor(typeof(string), typeof(string))]
        public string TitleText
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        #endregion

        #region : Override

        public override void ResetValue()
        {
            cboValue.DataSource = null;
            cboValue.Items.Clear();
            cboValue.Text = string.Empty;
        }

        public override void SetComboValue(object[] list)
        {
            if (cboValue.SelectedText != string.Empty) return;

            cboValue.DataSource = null;
            cboValue.Items.Clear();
            //cboValue.Items.AddRange(list);

            if (ComboValueTable == null)
            {
                ComboValueTable = DCUtil.MakeDataTableForDCValue((string[])list, (string[])list);
            }

            cboValue.DataSource = ComboValueTable;
            cboValue.DisplayMember = Definition.DynamicCondition_Search_key.DISPLAYDATA;
            cboValue.ValueMember = Definition.DynamicCondition_Search_key.VALUEDATA;

        }

        public override void SelectLastIndex()
        {
            int count = cboValue.Items.Count;
            if (count > 0)
                cboValue.SelectedIndex = count - 1;
        }

        public override void SetSelectedIndex(int index)
        {
            int count = cboValue.Items.Count;
            if (count > 0)
                cboValue.SelectedIndex = 0;

            //cboValue.Focus();
        }

        public override void EnableCombo(bool enable)
        {
            cboValue.Enabled = enable;
        }

        public override DataTable GetSelectedValue()
        {
            ArrayList alSelValue = new ArrayList();
            alSelValue.Add(cboValue.SelectedItem);

            DataTable dt = ComboValueTable.Clone();
            for (int i = 0; i < ComboValueTable.Rows.Count; i++)
            {
                string rawid = ComboValueTable.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                string select = ((DataRowView)cboValue.SelectedItem).Row[Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                if (select == rawid)
                    dt.ImportRow(ComboValueTable.Rows[i]);
            }

            return dt;
            //return alSelValue;
        }

        public override void SetTitleText(string text)
        {
            this.lblTitle.Text = text;
        }

        public override void SetComboText(string text)
        {
            this.cboValue.Text = text;
        }

        #endregion

        #region : Event handling

        private void cboValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboValue.SelectedIndex < 0) return;
            //string str = cboValue.Items[cboValue.SelectedIndex].ToString();
            DataRowView drv = (DataRowView)cboValue.SelectedItem;
            if (drv == null) return;
            //if (str == "") return;

            _parent.EventSelectValue(GetSelectedValue(), _type);
        }

        private void cboValue_DropDown(object sender, EventArgs e)
        {
            base.SetComboList(_type, this);
        }

        private void cboValue_TextUpdate(object sender, EventArgs e)
        {
            string str = cboValue.Text;

            _parent.EventSelectValue(GetSelectedValue(), _type);
        }

        #endregion

        #region : Public


        #endregion
    }
}
