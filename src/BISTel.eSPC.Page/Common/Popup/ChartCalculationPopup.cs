using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Web.Services;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;

using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;

namespace BISTel.eSPC.Page.Common
{
    public partial class ChartCalculationPopup : BasePopupFrm
    {
        #region : Field

        SessionData _SessionData;
        MultiLanguageHandler _mlthandler;

        private string _sModelConfigRawID;
        private string _sParamAlias;
        //SPC-812
        public bool _sCalculation = true;

        #endregion

        #region : Constructor

        public ChartCalculationPopup()
        {
            InitializeComponent();
            this._mlthandler = MultiLanguageHandler.getInstance();
        }

        #endregion

        #region : Initialization

        public void InitializePopup()
        {
            this.InitializeLayout();
            this.InitializeSPCChart();
        }

        public void InitializeCondition()
        {
        }

        public void InitializeLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.TITLE_SPC_CALCULATION);
        }

        public void InitializeSPCChart()
        {
            this.spcCalcUC.SModelConfigRawID = this._sModelConfigRawID;
            this.spcCalcUC.SessionData = this._SessionData;
            this.spcCalcUC.URL = this.URL;
            //SPC-812
            this.spcCalcUC._sCalculation = false;
            //
            this.spcCalcUC.InitializePopup();
        }

        #endregion



        #region : Popup Logic
        #endregion

        #region :: Button Event & Method

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion



        #region : Public

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        public string SModelConfigRawID
        {
            get { return this._sModelConfigRawID; }
            set { this._sModelConfigRawID = value; }
        }

        public string SParamAlias
        {
            get { return this._sParamAlias; }
            set { this._sParamAlias = value; }
        }
        #endregion



    }
}
