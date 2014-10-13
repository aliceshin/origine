using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Page.Modeling
{
    public partial class SPCModelSavePopup : BasePopupFrm
    {
        Initialization _Initialization = null;
        MultiLanguageHandler _mlthandler = null;
        SessionData _sessionData = null;

        eSPCWebService.eSPCWebService _ws = null;

        ModifyMode _configMode = ModifyMode.SUB;
        string _messge = "";
        string _changedItems = "";
        
        bool _isShowNoButton = false;
        bool _hasSubconfig = true;

        public string COMMENT
        {
            get { return btxtComment.Text; }
        }

        public string CHANGED_ITEMS
        {
            get { return btxtChangedItems.Text; }
            set { btxtChangedItems.Text = value; }
        }

        public SPCModelSavePopup(ModifyMode configMode, string strMessage)
        {
            InitializeComponent();

            this._configMode = configMode;
            this._messge = strMessage;
        }

        public SPCModelSavePopup(ModifyMode configMode, string strMessage, bool isShowNoBtn, bool hasSubconfig)
        {
            InitializeComponent();

            this._configMode = configMode;

            if (strMessage.Contains("@@n"))
            {
                strMessage = strMessage.Replace("@@n", "\n");
            }

            this._messge = strMessage;
            this._isShowNoButton = isShowNoBtn;
            this._hasSubconfig = hasSubconfig;
        }

        public void InitializeControl()
        {
            this._Initialization = new Initialization();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization.InitializePath();
            this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            this.PageInit();
        }

        private void PageInit()
        {
            switch (_configMode)
            {
                case ModifyMode.MAIN:
                    SetPopupForMain();
                    break;
                case ModifyMode.SUB:
                    SetPopupForSub();
                    break;
                case ModifyMode.COPY:
                    SetPopupForCopy();
                    break;
                case ModifyMode.REUSE:
                    SetPopupForReuse();
                    break;
            }
        }

        private void SetPopupForMain()
        {
            this.Title = "Modify Main Model";
            this.bpnlSub.Visible = false;
            this.bpnlCopy.Visible = false;

            this.blblMain.Text = _messge;

            if (_isShowNoButton && _hasSubconfig)
                this.bbtnNo.Visible = true;
            else
                this.bbtnNo.Visible = false;
        }

        private void SetPopupForSub()
        {
            this.Title = "Modify Sub Model";

            if (!_hasSubconfig)
                this.Title = "Modify Main Model";

            this.bpnlMain.Visible = false;
            this.bpnlCopy.Visible = false;

            if (_messge != "")
                this.blblSub.Text = _messge;
            this.bbtnNo.Visible = false;
        }

        private void SetPopupForCopy()
        {
            this.Title = "Copy Model";
            this.bpnlMain.Visible = false;
            this.bpnlSub.Visible = false;

            //this.blblCopy.Text = _messge;
            this.bbtnNo.Visible = false;
        }

        private void SetPopupForReuse()
        {
            this.Title = "Rollback Main Model";
            this.bpnlSub.Visible = false;
            this.bpnlCopy.Visible = false;

            this.blblMain.Text = _messge;
            this.bbtnNo.Visible = false;
        }

        private void btxtComment_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox myTextBox = sender as TextBox;

            int CntBytes = Encoding.Default.GetByteCount(myTextBox.Text);

            if (CntBytes >= myTextBox.MaxLength)
            {
                if(e.KeyChar != '\b')
                    e.Handled = true;
            }
        }
    }
}
