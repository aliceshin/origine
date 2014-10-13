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
    public partial class SelectContextValuePopup : BasePopupFrm
    {
        #region : Constructor
        public SelectContextValuePopup()
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

        private ArrayList _sSelectedContextKeys;
        private ArrayList _sSelectedContextValues;

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

            this.InitializeBSpread();}


        #endregion

        private void InitializeBSpread()
        {
            //#01. RULE OPTION
            this.bsprContextValue.ClearHead();
            this.bsprContextValue.AddHeadComplete();
            this.bsprContextValue.UseFilter = false;
            this.bsprContextValue.FilterVisible = true;
            this.bsprContextValue.UseSpreadEdit = false;
            this.bsprContextValue.AutoGenerateColumns = false;

            //context key값에 해당되는 value list가 있는지 Table에서 select한다.
            DataSet _dsContextKey = this._wsSPC.GetSPCContextKeyTableAndColumns();
            FarPoint.Win.Spread.CellType.CheckBoxCellType ct = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

            DataSet dsContextValues = new DataSet();
            DataTable dtContextValues = new DataTable();

            if (_dsContextKey != null && _dsContextKey.Tables != null && _dsContextKey.Tables.Count > 0)
            {
                for (int i = 0; i<_dsContextKey.Tables[0].Rows.Count; i++)
                {
                    if (this._sSelectedContextKeys.Contains(_dsContextKey.Tables[0].Rows[i]["CONTEXT_KEY"].ToString()))
                    {
                        this.bsprContextValue.ActiveSheet.ColumnCount += 2;

                        this.bsprContextValue.ActiveSheet.Columns[this.bsprContextValue.ActiveSheet.ColumnCount - 2, this.bsprContextValue.ActiveSheet.ColumnCount - 1].Locked = true;

                        string strTempTableName = _dsContextKey.Tables[0].Rows[i]["TABLE_NAME"].ToString();
                        string strTempColumnName = _dsContextKey.Tables[0].Rows[i]["COLUMN_NAME"].ToString();

                        //DataSet _dsTemp = this._wsSPC.GetSPCContextValueList(strTempTableName, strTempColumnName);
                        DataSet _dsTemp = new DataSet();
                        DataRow _drTemp = _dsTemp.Tables[0].NewRow();
                        _drTemp[strTempColumnName] = "ALL(*)";
                        _dsTemp.Tables[0].Rows.InsertAt(_drTemp, 0);

                        if (this.bsprContextValue.ActiveSheet.RowCount < _dsTemp.Tables[0].Rows.Count)
                        {
                            this.bsprContextValue.ActiveSheet.RowCount = _dsTemp.Tables[0].Rows.Count;
                        }

                        DataColumn dc = new DataColumn(_dsTemp.Tables[0].Columns[0].ColumnName, System.Type.GetType("System.String"));
                        dtContextValues.Columns.Add(dc);
                        for (int k = 0; k < _dsTemp.Tables[0].Rows.Count; k++)
                        {
                            if (dtContextValues.Rows.Count < k + 1)
                            {
                                DataRow dr = dtContextValues.NewRow();
                                dr[_dsTemp.Tables[0].Columns[0].ColumnName] = _dsTemp.Tables[0].Rows[k][0].ToString();
                                dtContextValues.Rows.Add(dr);
                            }
                            else
                            {
                                dtContextValues.Rows[k][_dsTemp.Tables[0].Columns[0].ColumnName] = _dsTemp.Tables[0].Rows[k][0].ToString();
                            }
                        }

                        this.bsprContextValue.ActiveSheet.ColumnHeader.Cells[0, this.bsprContextValue.ActiveSheet.ColumnCount - 1].Text = _dsContextKey.Tables[0].Rows[i]["CONTEXT_KEY"].ToString();
                        this.bsprContextValue.ActiveSheet.ColumnHeader.Cells[0, this.bsprContextValue.ActiveSheet.ColumnCount - 2].Text = "SELECT";

                        this.bsprContextValue.ActiveSheet.Columns[this.bsprContextValue.ActiveSheet.ColumnCount - 1].DataField = _dsTemp.Tables[0].Columns[0].ColumnName;
                        this.bsprContextValue.ActiveSheet.Cells[0, this.bsprContextValue.ActiveSheet.ColumnCount - 2, _dsTemp.Tables[0].Rows.Count - 1, this.bsprContextValue.ActiveSheet.ColumnCount - 2].CellType = ct;
                        this.bsprContextValue.ActiveSheet.Cells[0, this.bsprContextValue.ActiveSheet.ColumnCount - 2, _dsTemp.Tables[0].Rows.Count - 1, this.bsprContextValue.ActiveSheet.ColumnCount - 2].Locked = false;
                    }
                }
            }

            dsContextValues.Tables.Add(dtContextValues);
            this.bsprContextValue.ActiveSheet.DataSource = dsContextValues;

            ////기존에 선택되어 있던 value list가 있을 경우 binding한다. (* 일 경우 전체 선택한다.)
            //int idxTemp = this._sSelectedContextKeys.IndexOf(_dsContextKey.Tables[0].Rows[i]["CONTEXT_KEY"].ToString());

            //string strTempContextValue = this._sSelectedContextValues[idxTemp].ToString();
            //if (strTempContextValue == "*")
            //{
            //    this.bsprContextValue.ActiveSheet.Cells[0, this.bsprContextValue.ActiveSheet.ColumnCount - 2].Value = true;
            //}
            //else if (strTempContextValue.Length > 0)
            //{
            //    string[] strArrTempContextValue = strTempContextValue.Split(';');
            //    ArrayList alTempContextValue = new ArrayList(strArrTempContextValue);
            //    for (int k = 1; k < this.bsprContextValue.ActiveSheet.RowCount; k++)
            //    {
            //        if (alTempContextValue.Contains(this.bsprContextValue.ActiveSheet.Cells[k, this.bsprContextValue.ActiveSheet.ColumnCount - 1].Text)
            //            && this.bsprContextValue.ActiveSheet.Cells[k, this.bsprContextValue.ActiveSheet.ColumnCount - 2].CellType != null
            //            && this.bsprContextValue.ActiveSheet.Cells[k, this.bsprContextValue.ActiveSheet.ColumnCount - 2].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.CheckBoxCellType))
            //        {
            //            this.bsprContextValue.ActiveSheet.Cells[k, this.bsprContextValue.ActiveSheet.ColumnCount - 2].Value = true;
            //        }
            //    }
            //}

            this.bsprContextValue.ActiveSheet.ColumnHeader.Columns[0, this.bsprContextValue.ActiveSheet.ColumnCount - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            for (int cIdx = 0; cIdx < this.bsprContextValue.ActiveSheet.Columns.Count; cIdx++)
            {
                if (cIdx % 2 == 0)
                    this.bsprContextValue.ActiveSheet.Columns[cIdx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                else
                    this.bsprContextValue.ActiveSheet.Columns[cIdx].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            }
            this.bsprContextValue.ActiveSheet.Columns[0, this.bsprContextValue.ActiveSheet.ColumnCount - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            int iWidth = 0;

            for (int cIdx = 0; cIdx < this.bsprContextValue.ActiveSheet.Columns.Count; cIdx++)
            {
                this.bsprContextValue.ActiveSheet.Columns[cIdx].Width = this.bsprContextValue.ActiveSheet.Columns[cIdx].GetPreferredWidth();
                iWidth += Convert.ToInt32(this.bsprContextValue.ActiveSheet.Columns[cIdx].GetPreferredWidth());
            }

            if (this.ContentsAreaMinWidth < iWidth)
            {
                this.ContentsAreaMinWidth = iWidth + 100;
            }            
        }

        private void bsprContextValue_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column % 2 == 0)
            {
                if (e.Row == 0)
                {
                    if (Convert.ToBoolean(this.bsprContextValue.ActiveSheet.Cells[e.Row, e.Column].Value))
                    {
                        for (int i = 1; i < this.bsprContextValue.ActiveSheet.RowCount; i++)
                        {
                            if (this.bsprContextValue.ActiveSheet.Cells[i, e.Column].CellType != null && this.bsprContextValue.ActiveSheet.Cells[i, e.Column].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.CheckBoxCellType))
                            {
                                this.bsprContextValue.ActiveSheet.Cells[i, e.Column].Value = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (Convert.ToBoolean(this.bsprContextValue.ActiveSheet.Cells[e.Row, e.Column].Value))
                        this.bsprContextValue.ActiveSheet.Cells[0, e.Column].Value = false;
                }
            }

        }

        private void bbtnOK_Click(object sender, EventArgs e)
        {
            ArrayList alTempContextKeys = new ArrayList();
            ArrayList alTempContextValues = new ArrayList();
            for (int i = 0; i < this.bsprContextValue.ActiveSheet.ColumnCount; i = i + 2)
            {
                alTempContextKeys.Add(this.bsprContextValue.ActiveSheet.ColumnHeader.Cells[0, i+1].Text);
                string strTemp = "";
                for (int k = 0; k < this.bsprContextValue.ActiveSheet.RowCount; k++)
                {
                    if (this.bsprContextValue.ActiveSheet.Cells[k, i].CellType != null
                        && this.bsprContextValue.ActiveSheet.Cells[k, i].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.CheckBoxCellType))
                    {
                        if (Convert.ToBoolean(this.bsprContextValue.ActiveSheet.Cells[k, i].Value))
                        {
                            if (k == 0)
                            {
                                strTemp = ";*";
                                break;
                            }
                            else
                            {
                                strTemp += ";" + this.bsprContextValue.ActiveSheet.Cells[k, i + 1].Text;
                            }
                        }
                    }
                }
                if (strTemp.Length > 0)
                    strTemp = strTemp.Substring(1);

                alTempContextValues.Add(strTemp);
            }

            this._sSelectedContextKeys = alTempContextKeys;
            this._sSelectedContextValues = alTempContextValues;

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
    }
}
