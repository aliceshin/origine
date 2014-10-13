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
    public partial class SelectContextValuePopup_New : BasePopupFrm
    {
        #region : Constructor
        public SelectContextValuePopup_New()
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

        private string _sLineRawID;
        private string _sAreaRawID;
        private string _sEQPModel;

        private ArrayList _sSelectedContextKeys;
        private ArrayList _sSelectedContextValues;
        private ArrayList _sSelectedExcludeValues;
        private ArrayList _arrLstDBContextKeys;
        private bool _bContext = true;

        private int _iSelectedRowIdx = -1;

        DataSet _dsContextKey = new DataSet();

        FarPoint.Win.Spread.CellType.CheckBoxCellType ct = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

        private bool _bUseComma;

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

            if (this._sSelectedContextValues != null)
            {
                this.Title = "Set Context Value List";
                _bContext = true;
            }
            else if (this._sSelectedExcludeValues != null)
            {
                this.Title = "Set Exclude Value List";
                _bContext = false;
            }

            _dsContextKey = this._wsSPC.GetSPCContextKeyTableAndColumns();

            _arrLstDBContextKeys = new ArrayList();

            for (int i = 0; i < _dsContextKey.Tables[0].Rows.Count; i++)
            {
                this._arrLstDBContextKeys.Add(_dsContextKey.Tables[0].Rows[i]["CONTEXT_KEY"].ToString());
            }

            this.InitializeBTabControl();
        }


        #endregion

        private void InitializeBTabControl()
        {
            for (int i = 0; i < this._sSelectedContextKeys.Count; i++)
            {
                TabPage tabPage = new TabPage(this._sSelectedContextKeys[i].ToString());
                BSpread bSpread = new BSpread();
                FarPoint.Win.Spread.SheetView bSpread_Sheet1 = new FarPoint.Win.Spread.SheetView();

                bSpread_Sheet1.Reset();
                bSpread_Sheet1.SheetName = "Sheet1";

                bSpread.About = "3.0.2005.2005";
                bSpread.AccessibleDescription = "";
                bSpread.AllowNewRow = true;
                bSpread.AutoClipboard = false;
                bSpread.AutoGenerateColumns = false;
                bSpread.BssClass = "";
                bSpread.ClickPos = new System.Drawing.Point(0, 0);
                bSpread.ColFronzen = 0;
                bSpread.ComboEnable = true;
                bSpread.DataAutoHeadings = false;
                bSpread.DataSet = null;
                bSpread.DateToDateTimeFormat = false;
                bSpread.DefaultDeleteValue = true;
                bSpread.DisplayColumnHeader = true;
                bSpread.DisplayRowHeader = true;
                bSpread.Dock = System.Windows.Forms.DockStyle.Fill;
                bSpread.EditModeReplace = true;
                bSpread.FilterVisible = false;
                bSpread.HeadHeight = 20F;
                bSpread.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                bSpread.IsCellCopy = false;
                bSpread.IsMultiLanguage = false;
                bSpread.IsReport = false;
                bSpread.Key = "";
                bSpread.Location = new System.Drawing.Point(3, 3);
                bSpread.Name = "bSpread";
                bSpread.RowFronzen = 0;
                bSpread.RowInsertType = BISTel.PeakPerformance.Client.BISTelControl.InsertType.Current;
                bSpread.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {bSpread_Sheet1});
                bSpread.Size = new System.Drawing.Size(550, 283);
                bSpread.StyleID = null;
                bSpread.TabIndex = 0;
                bSpread.UseAutoSort = false;
                bSpread.UseCheckAll = false;
                bSpread.UseCommandIcon = false;
                bSpread.UseFilter = false;
                bSpread.UseGeneralContextMenu = false;
                bSpread.UseHeadColor = false;
                bSpread.UseOriginalEvent = false;
                bSpread.UseSpreadEdit = true;
                bSpread.UseWidthMemory = true;
                bSpread.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
                bSpread.WhenDeleteUseModify = false;
                
                bSpread.ClearHead();
                bSpread.AddHeadComplete();
                bSpread.UseFilter = false;
                bSpread.FilterVisible = true;
                bSpread.UseSpreadEdit = false;
                bSpread.AutoGenerateColumns = false;
                bSpread.IsMultiLanguage = false;
                this.InitializeBSpread(ref bSpread, this._sSelectedContextKeys[i].ToString());
                bSpread.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(bSpread_ButtonClicked);
                bSpread.Dock = DockStyle.Fill;
                
                
                tabPage.Controls.Add(bSpread);

                this.bTabControl1.Controls.Add(tabPage);                
            }
        }

        private void bSpread_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            BSpread bSpread = (BSpread)(sender);
            if (e.Column == 0)
            {
                if (e.Row == 0)
                {
                    if (Convert.ToBoolean(bSpread.ActiveSheet.Cells[e.Row, e.Column].Value))
                    {
                        for (int i = 1; i < bSpread.ActiveSheet.RowCount; i++)
                        {
                            if (bSpread.ActiveSheet.Cells[i, e.Column].CellType != null && bSpread.ActiveSheet.Cells[i, e.Column].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.CheckBoxCellType))
                            {
                                bSpread.ActiveSheet.Cells[i, e.Column].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (Convert.ToBoolean(bSpread.ActiveSheet.Cells[e.Row, e.Column].Value))
                        bSpread.ActiveSheet.Cells[0, e.Column].Value = false;
                }
            }
        }

        private void InitializeBSpread(ref BSpread bSpread, string strContextKey)
        {
            if (this._arrLstDBContextKeys.Contains(strContextKey))
            {
                int idxTemp = this._arrLstDBContextKeys.IndexOf(strContextKey);

                string strTempTableName = _dsContextKey.Tables[0].Rows[idxTemp]["TABLE_NAME"].ToString();
                string strTempColumnName = _dsContextKey.Tables[0].Rows[idxTemp]["COLUMN_NAME"].ToString();
                string strTempDisplayColumnName = _dsContextKey.Tables[0].Rows[idxTemp]["DISPLAY_COLUMNS"].ToString();
                string strTempConditionName = _dsContextKey.Tables[0].Rows[idxTemp]["WHERE_CONDITION"].ToString();
                string strTempConditionValue = this._sLineRawID + "," + this._sAreaRawID + "," + this._sEQPModel;

                if (strTempDisplayColumnName.Trim().Length > 0)
                {
                    bSpread.ActiveSheet.ColumnCount = 2 + strTempDisplayColumnName.Split(',').Length;
                }
                else
                {
                    bSpread.ActiveSheet.ColumnCount = 2;
                }

                
                bSpread.ActiveSheet.Columns[0, bSpread.ActiveSheet.ColumnCount - 1].Locked = true;

                DataSet _dsTemp = this._wsSPC.GetSPCContextValueList(strTempTableName, strTempColumnName, strTempDisplayColumnName, strTempConditionName, strTempConditionValue);
                DataRow _drTemp = _dsTemp.Tables[0].NewRow();
                _drTemp[strTempColumnName] = "ALL(*)";
                _dsTemp.Tables[0].Rows.InsertAt(_drTemp, 0);

                if (bSpread.ActiveSheet.RowCount < _dsTemp.Tables[0].Rows.Count)
                {
                    bSpread.ActiveSheet.RowCount = _dsTemp.Tables[0].Rows.Count;
                }

                bSpread.ActiveSheet.ColumnHeader.Cells[0, 1].Text = strContextKey;
                bSpread.ActiveSheet.ColumnHeader.Cells[0, 0].Text = "SELECT";

                bSpread.ActiveSheet.Columns[1].DataField = _dsTemp.Tables[0].Columns[0].ColumnName;
                if (_dsTemp.Tables[0].Columns.Count > 1)
                {
                    for (int j = 1; j < _dsTemp.Tables[0].Columns.Count;j++)
                    {
                        bSpread.ActiveSheet.Columns[j + 1].DataField = _dsTemp.Tables[0].Columns[j].ColumnName;
                        bSpread.ActiveSheet.ColumnHeader.Cells[0, j + 1].Text = _dsTemp.Tables[0].Columns[j].ColumnName;
                    }
                }
                bSpread.ActiveSheet.Cells[0, 0, _dsTemp.Tables[0].Rows.Count - 1, 0].CellType = ct;
                bSpread.ActiveSheet.Cells[0, 0, _dsTemp.Tables[0].Rows.Count - 1, 0].Locked = false;

                bSpread.ActiveSheet.DataSource = _dsTemp;

                //기존에 선택되어 있던 value list가 있을 경우 binding한다. (* 일 경우 전체 선택한다.)
                int idxTempKey = this._sSelectedContextKeys.IndexOf(strContextKey);
                string strTempContextValue = "";
                if(this._bContext)
                    strTempContextValue = this._sSelectedContextValues[idxTempKey].ToString();
                else
                    strTempContextValue = this._sSelectedExcludeValues[idxTempKey].ToString();

                if (strTempContextValue == "*")
                {
                    bSpread.ActiveSheet.Cells[0, 0].Value = true;
                }
                else if (strTempContextValue.Length > 0)
                {
                    string[] strArrTempContextValue = strTempContextValue.Split(';');
                    ArrayList alTempContextValue = new ArrayList(strArrTempContextValue);
                    for (int k = 1; k < bSpread.ActiveSheet.RowCount; k++)
                    {
                        if (alTempContextValue.Contains(bSpread.ActiveSheet.Cells[k, 1].Text)
                            && bSpread.ActiveSheet.Cells[k, 0].CellType != null
                            && bSpread.ActiveSheet.Cells[k, 0].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.CheckBoxCellType))
                        {
                            bSpread.ActiveSheet.Cells[k, 0].Value = true;
                        }
                    }
                }

                for (int cIdx = 0; cIdx < bSpread.ActiveSheet.Columns.Count; cIdx++)
                {
                    if (cIdx == 0)
                        bSpread.ActiveSheet.Columns[cIdx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    else
                        bSpread.ActiveSheet.Columns[cIdx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                }

                bSpread.ActiveSheet.Columns[0, bSpread.ActiveSheet.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                //int iWidth = 0;

                //for (int cIdx = 0; cIdx < bSpread.ActiveSheet.Columns.Count; cIdx++)
                //{
                //    bSpread.ActiveSheet.Columns[cIdx].Width = bSpread.ActiveSheet.Columns[cIdx].GetPreferredWidth();
                //    iWidth += Convert.ToInt32(bSpread.ActiveSheet.Columns[cIdx].GetPreferredWidth());
                //}

                //if (this.ContentsAreaMinWidth < iWidth)
                //{
                //    this.ContentsAreaMinWidth = iWidth + 100;
                //}

                //if (this.ContentsAreaMinWidth > 950)
                //{
                //    this.ContentsAreaMinWidth = 950;
                //}
            }
            else
            {
                bSpread.ActiveSheet.ColumnCount = 2;
                bSpread.ActiveSheet.Columns[0, 1].Locked = true;
                bSpread.ActiveSheet.RowCount = 1;

                bSpread.ActiveSheet.ColumnHeader.Cells[0, 1].Text = strContextKey;
                bSpread.ActiveSheet.ColumnHeader.Cells[0, 0].Text = "SELECT";

                bSpread.ActiveSheet.Cells[0, 0].CellType = ct;
                bSpread.ActiveSheet.Cells[0, 0].Locked = false;

                bSpread.ActiveSheet.Cells[0, 1].Text = "ALL(*)";

                //기존에 선택되어 있던 value list가 있을 경우 binding한다. (* 일 경우 전체 선택한다.)
                int idxTempKey = this._sSelectedContextKeys.IndexOf(strContextKey);
                string strTempContextValue = "";
                if (this._bContext)
                    strTempContextValue = this._sSelectedContextValues[idxTempKey].ToString();
                else
                    strTempContextValue = this._sSelectedExcludeValues[idxTempKey].ToString();

                if (strTempContextValue == "*")
                {
                    bSpread.ActiveSheet.Cells[0, 0].Value = true;
                }
                else if (strTempContextValue.Length > 0)
                {
                    string[] strArrTempContextValue = strTempContextValue.Split(';');
                    ArrayList alTempContextValue = new ArrayList(strArrTempContextValue);
                    for (int k = 1; k < bSpread.ActiveSheet.RowCount; k++)
                    {
                        if (alTempContextValue.Contains(bSpread.ActiveSheet.Cells[k, 1].Text)
                            && bSpread.ActiveSheet.Cells[k, 0].CellType != null
                            && bSpread.ActiveSheet.Cells[k, 0].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.CheckBoxCellType))
                        {
                            bSpread.ActiveSheet.Cells[k, 0].Value = true;
                        }
                    }
                }

                for (int cIdx = 0; cIdx < bSpread.ActiveSheet.Columns.Count; cIdx++)
                {
                    if (cIdx == 0)
                        bSpread.ActiveSheet.Columns[cIdx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                    else
                        bSpread.ActiveSheet.Columns[cIdx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                }

                bSpread.ActiveSheet.Columns[0, bSpread.ActiveSheet.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                //int iWidth = 0;

                //for (int cIdx = 0; cIdx < bSpread.ActiveSheet.Columns.Count; cIdx++)
                //{
                //    bSpread.ActiveSheet.Columns[cIdx].Width = bSpread.ActiveSheet.Columns[cIdx].GetPreferredWidth();
                //    iWidth += Convert.ToInt32(bSpread.ActiveSheet.Columns[cIdx].GetPreferredWidth());
                //}

                //if (this.ContentsAreaMinWidth < iWidth)
                //{
                //    this.ContentsAreaMinWidth = iWidth + 100;
                //}

                //if (this.ContentsAreaMinWidth > 950)
                //{
                //    this.ContentsAreaMinWidth = 950;
                //}

            }
        }

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            ArrayList alTempContextKeys = new ArrayList();
            ArrayList alTempContextValues = new ArrayList();
            ArrayList alTempCheckContextValues = new ArrayList();

            int idx = this.bTabControl1.Controls.Count;
            for (int i = 0; i < this.bTabControl1.Controls.Count; i++)
            {
                TabPage tabPage = (TabPage)this.bTabControl1.Controls[i];
                BSpread bSpread = null;
                if (tabPage.Controls.Count > 1)
                {
                    bSpread = (BSpread)tabPage.Controls[tabPage.Controls.Count - 1];
                }
                else
                {
                    bSpread = (BSpread)tabPage.Controls[0];
                }

                

                alTempContextKeys.Add(bSpread.ActiveSheet.ColumnHeader.Cells[0, 1].Text);
                string strTemp = "";

                for (int k = 0; k < bSpread.ActiveSheet.RowCount; k++)
                {
                    if (bSpread.ActiveSheet.Cells[k, 0].CellType != null
                        && bSpread.ActiveSheet.Cells[k, 0].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.CheckBoxCellType))
                    {
                        if (Convert.ToBoolean(bSpread.ActiveSheet.Cells[k, 0].Value))
                        {
                            if (k == 0)
                            {
                                strTemp = ";*";
                                break;
                            }
                            else
                            {
                                if (alTempCheckContextValues.Contains(bSpread.ActiveSheet.Cells[k, 1].Text))
                                {
                                    continue;
                                }
                                else
                                {
                                    strTemp += ";" + bSpread.ActiveSheet.Cells[k, 1].Text;
                                    alTempCheckContextValues.Add(bSpread.ActiveSheet.Cells[k, 1].Text);
                                }
                            }
                        }
                    }
                }
                if (strTemp.Length > 0)
                    strTemp = strTemp.Substring(1);

                alTempContextValues.Add(strTemp);
            }

            this._sSelectedContextKeys = alTempContextKeys;
            if(this._bContext)
                this._sSelectedContextValues = alTempContextValues;
            else
                this._sSelectedExcludeValues = alTempContextValues;

            this.DialogResult = DialogResult.OK;
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

        public ArrayList SELECTED_CONTEXT_KEYS
        {
            get { return _sSelectedContextKeys; }
            set { _sSelectedContextKeys = value; }
        }

        public ArrayList SELECTED_CONTEXT_VALUES
        {
            get { return _sSelectedContextValues; }
            set { _sSelectedContextValues = value; }
        }

        public ArrayList SELECTED_EXCLUDE_VALUES
        {
            get { return _sSelectedExcludeValues; }
            set { _sSelectedExcludeValues = value; }
        }
        //spc1281
        public bool USE_COMMA
        {
            get { return _bUseComma; }
            set { _bUseComma = value; }
        }
    }
}
