using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.eSPC.Condition.Controls.Interface;

namespace BISTel.eSPC.Condition.Controls.Button
{
    public partial class ButtonControl : UserControl
    {
        #region : Field

        public SearchConditionInterface _parent = null;

        #endregion

        #region : Initialization

        public ButtonControl()
        {
            InitializeComponent();
        }

        #endregion

        #region : Event handling

        private void btnAdd_Click(object sender, EventArgs e)
        {
            _parent.SendEventToParent(buttonType.ADD, ConditionType.BUTTON);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            _parent.SendEventToParent(buttonType.REMOVE, ConditionType.BUTTON);
        }

        #endregion

        #region : Public

        public void SetParent(object parent)
        {
            _parent = (SearchConditionInterface)parent;
        }

        #endregion
    }
}
