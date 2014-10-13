using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Condition.Controls.Default;
using System.Collections;

namespace BISTel.eSPC.Condition.Controls.Interface
{
    public partial class ComboInterface : UserControl
    {
        #region : Field

        LinkedList comboList = new LinkedList();
        public string _title = "";
        public ConditionType _type = 0;
        public DefaultCondition _parent = null;
        private DataTable _dtComboValue = null;

        public DataTable ComboValueTable
        {
            set
            {
                _dtComboValue = value;
            }
            get
            {
                return _dtComboValue;
            }
        }

        #endregion

        #region : Initialization

        public ComboInterface()
        {
            InitializeComponent();
        }

        public ComboInterface(object parent)
        {
            InitializeComponent();
            _parent = (DefaultCondition)parent;
        }

        #endregion

        #region : Editor

        [Editor(typeof(ConditionType), typeof(ConditionType))]
        public ConditionType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion

        #region : Virtaul

        public virtual void ResetValue() { }
        public virtual void EnableCombo(bool enable) { }
        public virtual void SetComboValue(object[] list) { }
        public virtual void SetSelectedIndex(int index) { }
        public virtual void SelectLastIndex() { }
        public virtual void SetTitleText(string text) { }
        public virtual void SetComboText(string text) { }
        public virtual DataTable GetSelectedValue()
        {
            return new DataTable();
        }

        #endregion

        #region : public

        public virtual void SetComboList(ConditionType type, object child)
        {
            _parent.SetDropDownCombo(_type, this);
        }

        #endregion
    }
}
