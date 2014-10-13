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
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Modeling
{
    public partial class SelectParamPopup : BasePopupFrm
    {
        #region : Constructor
        public SelectParamPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region : Field

        SessionData _SessionData;
        string _sPort;

        Initialization _Initialization;
        MultiLanguageHandler _mlthandler;
        LinkedList _llstSearchCondition = new LinkedList();

        eSPCWebService.eSPCWebService _wsSPC = null;


        BISTel.eSPC.Common.BSpreadUtility _SpreadUtil;
        BISTel.eSPC.Common.CommonUtility _ComUtil;

        private SortedList _slColumnIndex = new SortedList();
        private SortedList _slBtnListIndex = new SortedList();

        private int _ColIdx_SELECT;
        private int _ColIdx_PARAM_ALIAS;

        private string _sLineRawID;
        private string _sAreaRawID;
        private string _sEQPModel;
        private string _sParamTypeCD;
        private List<string> _alExcludeParam = new List<string>();

        private string _sSelectedParamAlias;

        private int _iSelectedRowIdx = -1;

        #endregion



        #region : Initialization

        public void InitializePopup()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._SpreadUtil = new BSpreadUtility();
            this._ComUtil = new CommonUtility();

            this.InitializeBSpread();

            this.InitializeData();
        }


        #endregion

        private void InitializeBSpread()
        {
            //#01. RULE OPTION
            this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprParam, Definition.PAGE_KEY_SPC_CONFIGURATION, true, Definition.PAGE_KEY_SPC_CONFIGURATION_PARAM_LIST);
            this.bsprParam.UseHeadColor = true;

            //SPC-
            this.bsprParam.UseFilter = false;
            this.bsprParam.FilterVisible = false;
            
            this._ColIdx_SELECT = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT)];
            this._ColIdx_PARAM_ALIAS = (int)this._slColumnIndex[this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_PARAM_ALIAS)];

            this.bsprParam.SelectionBlockOptions = FarPoint.Win.Spread.SelectionBlockOptions.None;
        }

        private void InitializeData()
        {
            _llstSearchCondition.Clear();
            _llstSearchCondition.Add(Definition.CONDITION_KEY_LINE_RAWID, _sLineRawID);
            _llstSearchCondition.Add(Definition.CONDITION_KEY_AREA_RAWID, _sAreaRawID);
            _llstSearchCondition.Add(Definition.CONDITION_KEY_EQP_MODEL, _sEQPModel);
            _llstSearchCondition.Add(Definition.CONDITION_KEY_PARAM_TYPE_CD, _sParamTypeCD);

            DataSet ds = _wsSPC.GetSPCParamList(_llstSearchCondition.GetSerialData());

            foreach (string excludeParam in _alExcludeParam)
            { 
                DataRow[] drExcludeParams = ds.Tables[0].Select(string.Format("{0} = '{1}'", COLUMN.PARAM_ALIAS, excludeParam));

                if (drExcludeParams != null && drExcludeParams.Length > 0)
                {
                    for (int i = 0; i < drExcludeParams.Length; i++)
                    {
                        drExcludeParams[i].Delete();
                    }
                }
            }

            ds.AcceptChanges();

            this.bsprParam.DataSet = ds;
        }

        private void InitializeLayout()
        {
           
        }

        private void bsprParam_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != _ColIdx_SELECT)
                return;

            if ((bool)this.bsprParam.GetCellValue(e.Row, _ColIdx_SELECT) == true)
            {
                for (int i = 0; i < this.bsprParam.ActiveSheet.RowCount; i++)
                {
                    if (e.Row == i)
                    {
                        if (e.Column != _ColIdx_SELECT)
                            this.bsprParam.ActiveSheet.Cells[i, _ColIdx_SELECT].Value = 1;

                        this._iSelectedRowIdx = e.Row;

                        continue;
                    }

                    this.bsprParam.ActiveSheet.Cells[i, _ColIdx_SELECT].Value = 0;
                }
            }
            else
                this._iSelectedRowIdx = -1;
        }


        private void bsprParam_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader) return;

            _sSelectedParamAlias = this.bsprParam.GetCellText(e.Row, _ColIdx_PARAM_ALIAS);

            this.DialogResult = DialogResult.OK;
        }


        private void bbtnOK_Click(object sender, EventArgs e)
        {
            if (_iSelectedRowIdx > -1)
            {
                _sSelectedParamAlias = this.bsprParam.GetCellText(_iSelectedRowIdx, _ColIdx_PARAM_ALIAS);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_PARAM", null, null);   
            }
        }

        private void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public SessionData SESSIONDATA
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        public string PORT
        {
            get { return _sPort; }
            set { _sPort = value; }
        }

        public string LINE_RAWID
        {
            get { return _sLineRawID; }
            set { _sLineRawID = value; }
        }

        public string AREA_RAWID
        {
            get { return _sAreaRawID; }
            set { _sAreaRawID = value; }
        }

        public string EQP_MODEL
        {
            get { return _sEQPModel; }
            set { _sEQPModel = value; }
        }

        public string PARAM_TYPE_CD
        {
            get { return _sParamTypeCD; }
            set { _sParamTypeCD = value; }
        }

        public List<string> EXCLUDE_PARAM_LIST
        {
            get { return _alExcludeParam; }
            set { _alExcludeParam = value; }
        }

        public string SELECTED_PARAM_ALIAS
        {
            get { return _sSelectedParamAlias; }
            set { _sSelectedParamAlias = value; }
        }

      

       

    }
}
