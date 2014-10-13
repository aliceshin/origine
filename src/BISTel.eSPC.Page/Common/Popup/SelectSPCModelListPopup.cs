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

using BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar;
using BISTel.PeakPerformance.Client.DataAsyncHandler;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Collections;

namespace BISTel.eSPC.Page.Common
{
    public partial class SelectSPCModelListPopup : BasePopupFrm
    {
        Initialization _Initialization = null;
        MultiLanguageHandler _mlthandler = null;
        SessionData _sessionData = null;

        eSPCWebService.eSPCWebService _ws = null;

        string _configRawID = "";
        string _eqpModel = "";
        string _spcModelName = "";
        string _paramAlias = "";
        string _mainYN = "";
        string _modelRawID = "";
        string _modelName = "";

        public SelectSPCModelListPopup()
        {
            InitializeComponent();

            this.bbtnOK.Click += new EventHandler(bbtnOK_Click);
            this.bbtnCancel.Click += new EventHandler(bbtnCancel_Click);
        }




        ///////////////////////////////////////////////////////////////////////////////////
        //  PageLoad & Initialize.
        ///////////////////////////////////////////////////////////////////////////////////

        public string CONFIG_RAWID
        {
            get { return this._configRawID; }
        }

        public string MODEL_RAWID
        {
            get { return this._modelRawID; }
        }

        public string MODEL_NAME
        {
            get { return this._modelName; }
        }

        public DataSet dsSPCModelList = null;

        public string sSPCModelLevel = "";
        public string sAreaT = "";
        public string sEQPModelT = "";
        public string sAreaRawIDT = "";
        public string strParamAliasT = "";

        public bool isShowSubList = false;




        

        #region CONTEXT Info.
        #endregion
        
        #region RULE Info.
        #endregion

        #region OPTION Info.
        #endregion

        #region AUTO Calculation Info.
        #endregion
        


        ///////////////////////////////////////////////////////////////////////////////////
        //  PageLoad & Initialize.
        ///////////////////////////////////////////////////////////////////////////////////

        public void InitializeControl()
        {
            this._Initialization = new Initialization();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization.InitializePath();

            this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            this.ContentsAreaMinWidth = 660;
            this.ContentsAreaMinHeight = 725;

            if (sSPCModelLevel.Equals(Definition.CONDITION_KEY_EQP_MODEL))
            {
                this.bLabel2.Text = "EQP Model";
                lblEQPModel.Text = this.sEQPModelT;
            }
            else
            {
                this.bLabel2.Text = "Area";
                lblEQPModel.Text = this.sAreaT;
            }

            this.lblParamAlias.Text = this.strParamAliasT;

            this.bccbo_SPCModelName.DataSource = this.dsSPCModelList.Tables[0];
            this.bccbo_SPCModelName.DisplayMember = Definition.CONDITION_KEY_SPC_MODEL_NAME;

            this.InitializeBSpread();

            if (isShowSubList)
            {
                if (this.dsSPCModelList.Tables[0].Rows.Count == 1)
                {
                    this.bbtn_Search.Enabled = false;
                    this.bbtn_Search.Visible = false;
                    this.bccbo_SPCModelName.SelectedIndex = 0;
                    this.ConfigListDataBinding(((DataRowView)this.bccbo_SPCModelName.SelectedValue)["RAWID"].ToString());
                }
            }
            else
            {
                this.bbtn_Search.Enabled = false;
                this.bbtn_Search.Visible = false;
                this.btpnlSubModelList.Visible = false;
                this.Resizable = true;
                this.Height = this.Height - this.btpnlSubModelList.Height;
                this.ContentsAreaMinHeight = this.ContentsAreaMinHeight - this.btpnlSubModelList.Height; 
            }
        }

        public void InitializeBSpread()
        {

            this.bsprData.ClearHead();
            this.bsprData.AddHeadComplete();
            this.bsprData.UseSpreadEdit = false;

            //SPC-743 by Louis you
            this.bsprData.UseGeneralContextMenu = false;

        }

        private void ConfigListDataBinding(string _sSPCModelRawid)
        {
            try
            {
                LinkedList _llstSearchCondition = new LinkedList();
                DataSet _dsSPCModeData = new DataSet();

                string strParamAlias = "";
                //초기화
                _llstSearchCondition.Clear();
                _llstSearchCondition.Add(Definition.CONDITION_KEY_MODEL_RAWID, _sSPCModelRawid);


                EESProgressBar.ShowProgress(this, this._mlthandler.GetMessage(Definition.LOADING_DATA), true);

                AsyncCallHandler ach = new AsyncCallHandler(EESProgressBar.AsyncCallManager);

                object objDataSet = ach.SendWait(_ws, "GetSPCModelData", new object[] { _llstSearchCondition.GetSerialData() });

                EESProgressBar.CloseProgress(this);
                //

                if (objDataSet != null)
                {
                    _dsSPCModeData = (DataSet)objDataSet;
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("INFORMATION_NODATA"));
                    return;
                }

                if (!DSUtil.CheckRowCount(_dsSPCModeData, TABLE.MODEL_MST_SPC))
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_MODEL_ELIMINATED", null, null);
                    return;
                }

                DataTable dtConfig = _dsSPCModeData.Tables[TABLE.MODEL_CONFIG_MST_SPC];
                DataTable dtContext = _dsSPCModeData.Tables[TABLE.MODEL_CONTEXT_MST_SPC];
                DataTable dtRuleMst = _dsSPCModeData.Tables[TABLE.MODEL_RULE_MST_SPC];

                EESProgressBar.ShowProgress(this, MSGHandler.GetMessage("PROCESS_LOADING_PAGE_DATA"), false);

                DataTable dtSPCModelChartList = new DataTable();

                dtSPCModelChartList.Columns.Add(COLUMN.SELECT, typeof(Boolean));
                dtSPCModelChartList.Columns.Add(COLUMN.CHART_ID);
                dtSPCModelChartList.Columns.Add(COLUMN.PARAM_ALIAS);
                dtSPCModelChartList.Columns.Add(COLUMN.MAIN_YN);
                dtSPCModelChartList.Columns.Add(COLUMN.VERSION);
                dtSPCModelChartList.Columns.Add("MODE");

                //CONTEXT COLUMN 생성
                DataRow[] drConfigs = dtConfig.Select(COLUMN.MAIN_YN + " = 'Y'", COLUMN.RAWID);

                if (drConfigs != null && drConfigs.Length > 0)
                {
                    DataRow[] drMainContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfigs[0][COLUMN.RAWID]), COLUMN.KEY_ORDER);

                    foreach (DataRow drMainContext in drMainContexts)
                    {
                        dtSPCModelChartList.Columns.Add(drMainContext["CONTEXT_KEY_NAME"].ToString());
                    }
                }

                LinkedList llCondition = new LinkedList();
                llCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_CHART_MODE);
                DataSet _dsChartMode = this._ws.GetCodeData(llCondition.GetSerialData());

                Dictionary<string, string> modeCodeData = new Dictionary<string, string>();
                if (_dsChartMode != null && _dsChartMode.Tables.Count > 0)
                {
                    foreach (DataRow dr in _dsChartMode.Tables[0].Rows)
                    {
                        modeCodeData.Add(dr[COLUMN.CODE].ToString(), dr[COLUMN.NAME].ToString());
                    }
                }

                foreach (DataRow drConfig in dtConfig.Rows)
                {
                    DataRow drChartList = dtSPCModelChartList.NewRow();

                    drChartList[COLUMN.CHART_ID] = drConfig[COLUMN.RAWID].ToString();
                    drChartList[COLUMN.PARAM_ALIAS] = drConfig[COLUMN.PARAM_ALIAS].ToString();
                    drChartList[COLUMN.MAIN_YN] = drConfig[COLUMN.MAIN_YN].ToString();
                    //#Version이 Null 또는 Empty인 경우 대비 Check Logic 추가
                    if (!string.IsNullOrEmpty(drConfig[COLUMN.VERSION].ToString()))
                        drChartList[COLUMN.VERSION] = (1 + Convert.ToDouble(drConfig[COLUMN.VERSION].ToString()) / 100).ToString("N2");
                    string modeValue = drConfig[COLUMN.CHART_MODE_CD].ToString();
                    if (modeCodeData.ContainsKey(modeValue))
                    {
                        modeValue = modeCodeData[modeValue];
                    }
                    drChartList["MODE"] = modeValue;

                    if (strParamAlias == "")
                        strParamAlias = drConfig[COLUMN.PARAM_ALIAS].ToString();

                    DataRow[] drContexts = dtContext.Select(string.Format("{0} = '{1}'", COLUMN.MODEL_CONFIG_RAWID, drConfig[COLUMN.RAWID]));

                    foreach (DataRow drContext in drContexts)
                    {
                        //2009-11-27 bskwon 추가 : Sub Model 상속 구조가 아닌경우 예외처리
                        if (!dtSPCModelChartList.Columns.Contains(drContext["CONTEXT_KEY_NAME"].ToString()))
                            dtSPCModelChartList.Columns.Add(drContext["CONTEXT_KEY_NAME"].ToString());

                        drChartList[drContext["CONTEXT_KEY_NAME"].ToString()] = drContext[COLUMN.CONTEXT_VALUE].ToString();
                    }

                    dtSPCModelChartList.Rows.Add(drChartList);
                }

                dtSPCModelChartList.AcceptChanges();

                bsprData.ClearHead();
                bsprData.UseEdit = true;
                bsprData.UseHeadColor = true;
                bsprData.Locked = true;

                for (int i = 0; i < dtSPCModelChartList.Columns.Count; i++)
                {
                    string sColumn = dtSPCModelChartList.Columns[i].ColumnName.ToString();
                    if (i == 0)
                    {
                        this.bsprData.AddHead(i, sColumn, sColumn, 50, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.CheckBox, null, null, null, false, true);
                    }
                    else
                    {
                        this.bsprData.AddHead(i, sColumn, sColumn, 100, 20, null, null, null, ColumnAttribute.Null,
                                              ColumnType.Null, null, null, null, false, true);
                    }
                }

                this.bsprData.AddHeadComplete();
                this.bsprData.DataSet = dtSPCModelChartList;

                this.bsprData.Locked = true;
                this.bsprData.ActiveSheet.Columns[0].Locked = false;
                this.bsprData.ActiveSheet.Columns[2].Visible = false;




                this.bsprData.AllowNewRow = false;

                FarPoint.Win.Spread.CellType.TextCellType tc = new FarPoint.Win.Spread.CellType.TextCellType();
                tc.MaxLength = 1024;

                //Column Size 조절
                for (int cIdx = 0; cIdx < this.bsprData.ActiveSheet.Columns.Count; cIdx++)
                {
                    this.bsprData.ActiveSheet.Columns[cIdx].Width = this.bsprData.ActiveSheet.Columns[cIdx].GetPreferredWidth();

                    if (this.bsprData.ActiveSheet.Columns[cIdx].Width > 150)
                        this.bsprData.ActiveSheet.Columns[cIdx].Width = 150;

                    if (this.bsprData.ActiveSheet.Columns[cIdx].CellType != null
                        && this.bsprData.ActiveSheet.Columns[cIdx].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.TextCellType))
                    {
                        this.bsprData.ActiveSheet.Columns[cIdx].CellType = tc;
                    }
                }

                //MAIN은 첫번째 ROW에 배치하고 ROW HIGHLIGHT
                if (this.bsprData.GetCellText(0, 3).Equals("Y"))
                {
                    this.bsprData.ActiveSheet.Rows[0].BackColor = Color.LightGreen; //Color.LemonChiffon;
                }

                this.bsprData.LeaveCellAction();

            }
            catch (Exception ex)
            {
                EESProgressBar.CloseProgress(this);
                if (ex is OperationCanceledException || ex is TimeoutException)
                {
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
                else
                {
                    LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
                    MSGHandler.DisplayMessage(MSGType.Error, ex.Message, null, null, true);
                }
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
            finally
            {
                EESProgressBar.CloseProgress(this);
            }
        }



        ///////////////////////////////////////////////////////////////////////////////////
        //  Event Handler.
        ///////////////////////////////////////////////////////////////////////////////////
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        void bbtnOK_Click(object sender, EventArgs e)
        {
            if (isShowSubList)
            {
                if (bsprData.ActiveSheet.RowCount <= 0) return;

                int iRowIndex = -1;

                ArrayList alCheckRowIndex = this.bsprData.GetCheckedList(0);

                if (alCheckRowIndex.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_ROW", null, null);
                    return;
                }
                else if (alCheckRowIndex.Count > 1)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_SINGLE_ITEM", null, null);
                    return;
                }
                else
                {
                    iRowIndex = (int)alCheckRowIndex[0];
                    this._configRawID = this.bsprData.GetCellText(iRowIndex, 1);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

                
            }
            else
            {
                if (bccbo_SPCModelName.SelectedItem != null)
                {
                    this._modelRawID = ((DataRowView)this.bccbo_SPCModelName.SelectedValue)["RAWID"].ToString();
                    this._modelName = ((DataRowView)this.bccbo_SPCModelName.SelectedValue)["SPC_MODEL_NAME"].ToString();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                    return;
                }
            }
        }

        void bbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        ///////////////////////////////////////////////////////////////////////////////////
        //  User Defined Method.
        ///////////////////////////////////////////////////////////////////////////////////

        public void SetCopyModelInfo(string eqpModel, string spcModelName, string mainYN, string configRawID, string paramAlias)
        {
            this._configRawID = configRawID;

            this.lblEQPModel.Text = eqpModel;
            this.lblParamAlias.Text = paramAlias;

            this._configRawID = configRawID;
            this._eqpModel = eqpModel;
            this._spcModelName = spcModelName;
            this._paramAlias = paramAlias;
            this._mainYN = mainYN;
        }

        private void bbtn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                if (bccbo_SPCModelName.SelectedItem != null)
                {
                    this.ConfigListDataBinding(((DataRowView)this.bccbo_SPCModelName.SelectedValue)["RAWID"].ToString());
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                    return;
                }
            }
            catch
            {
            }
        }
    }
}
