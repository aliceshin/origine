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
using BISTel.PeakPerformance.Client.BISTelControl;

namespace BISTel.eSPC.Condition.Controls.Interface
{
    public partial class ControlInterface : UserControl//BasePageUCtrl
    {
        #region : Field

        public int _height;
        public bool _hide = false;
        public SearchConditionInterface _parent = null;
        public string _sTitleKey = "";
        public MultiLanguageHandler _mlthandler = null;

        #endregion

        #region : Virtual

        public virtual void AlertEvent(object condition, ConditionType type)
        {
            ((SearchConditionInterface)Parent).SendEventToParent(condition, type);
        }

        public virtual string[] DataSetToArray(DataSet ds, string name)
        {
            string[] datas = new string[0];

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        datas = new string[ds.Tables[0].Rows.Count];
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            datas[row] = (string)ds.Tables[0].Rows[row][name];
                        }
                    }
                }
            }

            return datas;
        }

        public virtual void SetParent(object parent)
        {
            _parent = (SearchConditionInterface)parent;
        }

        #endregion

        #region : Initialization

        public ControlInterface()
        {
            InitializeComponent();
        }

        public ControlInterface(object parent)
        {
            InitializeComponent();
            _parent = (SearchConditionInterface)parent;
        }

        #endregion


        #region : Editor

        [Editor(typeof(string), typeof(string))]
        public string TitleKey
        {
            get { return _sTitleKey; }
            set
            {
                _sTitleKey = value;
                //string sTitle = _mlthandler.GetVariable((string)value);
                //btpnlTitle.Title = sTitle;
            }
        }

        [Editor(typeof(string), typeof(string))]
        public string MaxControl
        {
            get { return this.btpnlTitle.MaxControlName; }
            set { this.btpnlTitle.MaxControlName = value; }
        }

        [Editor(typeof(bool), typeof(bool))]
        public bool MaxResize
        {
            get { return this.btpnlTitle.MaxResizable; }
            set { this.btpnlTitle.MaxResizable = value; }
        }

        #endregion

        #region : Event handling

        private void btpnlTitle_Click(object sender, EventArgs e)
        {
            //if (_hide)
            //{
            //    this.Height = _height;
            //}
            //else
            //{
            //    _height = Size.Height;
            //    this.Height = 27;
            //}


            //_hide = !_hide;
        }

        #endregion

        #region :Public

        public SessionData GetSession()
        {
            SessionData sessionData = new SessionData();
            sessionData.UserId = "bistel";
            sessionData.Level = "0";
            sessionData.UserRawID = "3";

            return sessionData;
        }

        #endregion

        private void ControlInterface_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                btpnlTitle.Title = MultiLanguageHandler.getInstance().GetVariable(_sTitleKey);
            }
        }
    }
}
