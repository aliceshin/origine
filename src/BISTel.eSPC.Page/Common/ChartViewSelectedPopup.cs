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

namespace BISTel.eSPC.Page.Common.Popup
{
    public partial class ChartViewSelectedPopup : BasePopupFrm
    {
        #region : Constructor
        public ChartViewSelectedPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region : Field

        protected Boolean _IsResult = true;
        protected Initialization _Initialization = null;
        protected MultiLanguageHandler _mlthandler = null;
        protected SessionData _SessionData;
        protected SortedList _slDefaultSettingData;
        protected LinkedList _llstResultChart = null;
        protected LinkedList _llstDefaultChart = null;
        CheckComboControl checkCombo = null;
        List<string> lstValue = new List<string>();
        List<string> lstDisplay = new List<string>();

        DataSet _ds = null;

        #endregion



        #region : Initialization

        public void InitializePopup()
        {
            _llstResultChart = new LinkedList();
            _Initialization = new Initialization();
            _mlthandler = MultiLanguageHandler.getInstance();


            this.InitializeVariable();
            this.InitializeLayout();
            this.InitializeCondition();
        }

        public void InitializeVariable()
        {
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._slDefaultSettingData = new SortedList();
            SetValueList();

        }

        public void InitializeLayout()
        {
            this.Title = this._mlthandler.GetVariable(Definition.POPUP_TITLE_KEY_CHART_VIEW);
        }

        public void InitializeCondition()
        {
            checkCombo = new CheckComboControl();
            checkCombo.TitleText = this._mlthandler.GetVariable(Definition.POPUP_TITLE_KEY_SELECT_CHART_TO_VIEW);
            checkCombo.ResetValue();
            checkCombo.llstDefault = llstDefaultChart;
            checkCombo.SetComboValue(lstValue.ToArray(), lstValue.ToArray());
            this.bArrangePanel1.Controls.Add(checkCombo);
            this.bArrangePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        }

        #endregion

        #region : Popup Logic

        #region :: Data Condition


        #region ::: Function

        private void SetValueList()
        {
            foreach (DataRow dr in DataSetChart.Tables[0].Rows)
            {
                lstValue.Add(dr["CODE"].ToString());
                // lstDisplay.Add(dr["DESCRIPTION"].ToString());
            }
        }
        #endregion

        #region ::: Event
        #endregion

        #endregion

        #region :: Button

        #region ::: Function

        private void ClickSaveButton()
        {
            LinkedList _llstResult = this.checkCombo.llsResult;
            if (_llstResult.Count > 0)
            {
                _llstResultChart.Clear();
                foreach (DataRow dr in DataSetChart.Tables[0].Rows)
                {
                    string strCode = dr["CODE"].ToString();
                    string strDesc = dr["DESCRIPTION"].ToString();

                    for (int i = 0; i < _llstResult.Count; i++)
                    {
                        if (strCode.Equals(_llstResult.GetKey(i).ToString()))
                        {
                            _llstResultChart.Add(strCode, strDesc);
                        }
                    }
                }
                //MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_SUCCESS_SAVE_CHANGE_DATA));
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                //선택된 Chart가 없습니다.
                MSGHandler.DisplayMessage(MSGType.Information, string.Format(MSGHandler.GetMessage("GENERAL_SELECT_OBJECT"), "Chart"));
            }
        }
        #endregion

        #region ::: Event

        private void bbtnSave_Click(object sender, EventArgs e)
        {
            this.ClickSaveButton();
        }

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #endregion

        #endregion

        #region : Public
        public bool Result
        {
            get
            {
                return this._IsResult;
            }
            set
            {
                this._IsResult = value;
            }
        }

        public SessionData SessionData
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        public DataSet DataSetChart
        {
            get { return _ds; }
            set { _ds = value; }
        }

        public LinkedList llstResultListChart
        {
            get { return _llstResultChart; }
            set { _llstResultChart = value; }
        }


        public LinkedList llstDefaultChart
        {
            get { return _llstDefaultChart; }
            set { _llstDefaultChart = value; }
        }




        #endregion
    }
}
