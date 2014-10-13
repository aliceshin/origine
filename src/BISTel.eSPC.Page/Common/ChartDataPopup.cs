using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;

using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    public partial class ChartDataPopup : BasePopupFrm
    {

        #region : Constructor
        public ChartDataPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region : Field
        Initialization _Initialization;
        BSpreadUtility _bspreadutility;
        CommonUtility _ComUtil;
        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _ws;
        DataTable _dtParam;
        SessionData _SessionData;

        #endregion


        #region : Initialization

        public void InitializePopup()
        {
            this.InitializeVariable();
            this.InitializeLayout();
            this.InitializeDataButton();
            this.InitializeBSpread();
        }

        public void InitializeVariable()
        {
            this._ws = new BISTel.eSPC.Page.eSPCWebService.eSPCWebService();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
            this._bspreadutility = new BSpreadUtility();
            this._ComUtil = new CommonUtility();

        }

        public void InitializeLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.POPUP_TITLE_KEY_CHART_DATA_VIEW);
        }


        public void InitializeDataButton()
        {
            this._Initialization.InitializeButtonList(this.bbtnList, ref bsprData, Definition.PAGE_KEY_SPC_CHART_POPUP, Definition.BUTTONLIST_KEY_CHART_DATA, this.SessionData);
        }

        public void InitializeBSpread()
        {
            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
            this.bsprData.UseSpreadEdit = false;
            this.bsprData.AutoGenerateColumns = true;
            this.bsprData.DataSource = DataTableParam;

        }

        #endregion




        #region : Popup Logic

        #region :: Data Condition

        #region ::: Function
        #endregion

        #region ::: Event
        #endregion

        #endregion

        #region :: Button

        #region ::: Function

        #endregion

        #region ::: Event

        private void bbtnList_ButtonClick(string name)
        {
            try
            {
                if (name.ToUpper() == Definition.ButtonKey.EXPORT)
                {
                    this.bsprData.Export(true);
                }
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }



        private void bbtnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #endregion

        #endregion

        #region : Public

        public DataTable DataTableParam
        {
            get { return _dtParam; }
            set { _dtParam = value; }
        }

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        #endregion

    }
}
