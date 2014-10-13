using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.eSPC.Common;
using BISTel.eSPC.Page.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using System.Collections;
using BISTel.eSPC.Page.Modeling;

namespace BISTel.eSPC.Page.Common.Popup
{
    public partial class MultiCalculationPopup : BasePopupFrm
    {
        #region : Field

        SessionData _SessionData;
        MultiLanguageHandler _mlthandler;

        private ArrayList _sModelConfigRawID = new ArrayList();
        private ArrayList _sParamAlias = new ArrayList();

        #endregion

        #region :::Constructor

        public MultiCalculationPopup()
        {
            InitializeComponent();
            this._mlthandler = MultiLanguageHandler.getInstance();
        }

        #endregion

        #region :: Initialization

        public void InitializeMultiPopup()
        {
            this.InitializeLayout();
            this.InitializeSPCChart();
        }
        //SPC-704
        public void InitializeLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.TITLE_SPC_MULTICALCULATION);
            this.spcMultiCalcUC.ClickCloseEvent += new SPCMultiCalculationUC.ClickCloseEventDelegate(spcMultiCalcUC_ClickCloseEvent);
        }

        void spcMultiCalcUC_ClickCloseEvent()
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public void InitializeSPCChart()
        {
            this.spcMultiCalcUC.SMulModelConfigRawID = this._sModelConfigRawID;
            this.spcMultiCalcUC.SessionData = this._SessionData;
            this.spcMultiCalcUC.URL = this.URL;
            this.spcMultiCalcUC.InitializePopup();
        }

        #endregion
        #region : Public

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        public ArrayList SModelConfigRawID
        {
            get { return this._sModelConfigRawID; }
            set { this._sModelConfigRawID = value; }
        }

        public ArrayList SParamAlias
        {
            get { return this._sParamAlias; }
            set { this._sParamAlias = value; }
        }
        #endregion
       

    }
}
