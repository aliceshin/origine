using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;


namespace BISTel.eSPC.Condition.Controls.Interface
{
    public partial class SearchConditionInterface : ADynamicCondition
    {
        #region : Field

        public MultiLanguageHandler _mlthandler = null;
        private eSPCWebService.eSPCWebService _spcWebService = null;

        public string UseDefaultCondition { get; set; }

        #endregion

        #region : Initialization

        public SearchConditionInterface()
        {
            InitializeComponent();

            this._spcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            InitializeDefaultCondition();
        }

        #endregion

        #region : Virtual

        public virtual void SendEventToParent(object value, ConditionType type)
        {
        }

        #endregion

        #region : Private

        private void InitializeDefaultCondition()
        {
            //DataSet dsCodeData = new DataSet();

            //LinkedList lnkCondition = new LinkedList();
            //lnkCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_USE_DEFAULT_CONDITION);

            //byte[] baDataUseDefault = lnkCondition.GetSerialData();

            //dsCodeData = this._spcWebService.GetCodeData(baDataUseDefault);

            //if (dsCodeData != null && dsCodeData.Tables.Count > 0)
            //{
            //    this.UseDefaultCondition = dsCodeData.Tables[0].Rows[0]["CODE"].ToString();
            //}
        }

        #endregion
    }
}
