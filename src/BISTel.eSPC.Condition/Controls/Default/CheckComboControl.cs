using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Interface;
using BISTel.PeakPerformance.Client.CommonLibrary;
using System.Collections;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;

namespace BISTel.eSPC.Condition.Controls.Default
{
    public partial class CheckComboControl : ComboInterface
    {
        #region : Field

        #endregion

        #region : Initialization

        //public MultiComboControl()
        //{
        //    InitializeComponent();
        //}

        public CheckComboControl(object parent)
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
            if (cboValue.SelectedValue != string.Empty) return;
            //if (cboValue.Text != string.Empty) return;
            ResetValue();
            //foreach (string text in list)
            //    cboValue.Items.Add(text);

            if (ComboValueTable == null)
            {
                ComboValueTable = DCUtil.MakeDataTableForDCValue((string[])list, (string[])list);
            }

            cboValue.DataSource = ComboValueTable;
            cboValue.DisplayMember = Definition.DynamicCondition_Search_key.DISPLAYDATA;
            cboValue.ValueMember = Definition.DynamicCondition_Search_key.VALUEDATA;
        }

        public override void EnableCombo(bool enable)
        {
            cboValue.Enabled = enable;
        }

        public override DataTable GetSelectedValue()
        {
            ArrayList valueList = new ArrayList();

            string value = cboValue.SelectedValue;
            int start = 0;
            int found = 0;
            do
            {
                found = value.IndexOf(",", start);
                if (found > 0)
                    valueList.Add(value.Substring(start, found - start));
                else
                    valueList.Add(value.Substring(start));
                start = found + 1;
            }
            while (found > 0);

            if (valueList.Count == 0) return null;

            DataTable dt = null;
            if (ComboValueTable != null)
            {
                dt = ComboValueTable.Clone();
                for (int i = 0; i < ComboValueTable.Rows.Count; i++)
                {
                    string rawid = ComboValueTable.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    if (valueList.Contains(rawid))
                        dt.ImportRow(ComboValueTable.Rows[i]);
                }
            }

            //return valueList;
            return dt;
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

        private void cboValue_DropDown(object sender, EventArgs e)
        {
            base.SetComboList(_type, this);
        }

        private void cboValue_CheckCombo_Select(object sender, EventArgs e)
        {
            string str = cboValue.Text;

            _parent.EventSelectValue(GetSelectedValue(), _type);
        }

        private void cboValue_TextChanged(object sender, EventArgs e)
        {
            string str = cboValue.Text;

            _parent.EventSelectValue(GetSelectedValue(), _type);
        }

        #endregion



        #region : Public

        #endregion
    }
}
