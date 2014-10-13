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

using System.Threading;
using System.Collections;

namespace BISTel.eSPC.Page.ATT.Modeling
{
    public partial class SearchModelByArea : BasePopupFrm
    {
        delegate void SetGridCallBack(int row, int column, string text);
        delegate void SetProgressCallBack(string displayText);
        delegate void SetPnlSPCModelVisibleCallBack(bool iVisible);
    
        
        
        private const string COL_NAME_EQP_ID_CONTEXT = "EQP ID";
        private const string COL_NAME_MODULE_ID_CONTEXT = "MODULE ID";
        private const string COL_NAME_RECIPE_ID_CONTEXT = "RECIPE ID";
        private const string COL_NAME_STEP_ID_CONTEXT = "STEP ID";
        private const string COL_NAME_MODEL_CONFIG_RAWID = "MODEL CONFIG RAWID";
        private const string PROGRESS_STRING = "Context Info is binding";

        
        Initialization _Initialization = null;
        MultiLanguageHandler _mlthandler = null;
        SessionData _sessionData = null;

        eSPCWebService.eSPCWebService _ws = null;

        SortedList _slColumnIndex = new SortedList();

        int _indexColEQP = -1;
        int _indexColModule = -1;
        int _indexColRecipe = -1;
        int _indexColStep = -1;
        int _indexColModelConfigRawID = -1;

        protected LinkedList _llstChartSearchCondition = new LinkedList();

        Thread _threadContextInfo = null;
        DataTable _dtContextInfo = null;

        DataSet _dsAreaAndSPCModelName = null;        
        
        
        public SearchModelByArea()
        {
            InitializeComponent();

            this.bblstSearch.ButtonClick += new BISTel.PeakPerformance.Client.BISTelControl.ButtonList.ImageDelegate(bblstSearch_ButtonClick);
            this.bbtnOK.Click += new EventHandler(bbtnOK_Click);
        }



        public SessionData SESSIONDATA
        {
            get { return this._sessionData; }
            set { this._sessionData = value; }
        }




        ///////////////////////////////////////////////////////////////////////////////////
        //  PageLoad & Initialize.
        ///////////////////////////////////////////////////////////////////////////////////

        public void InitializeControl()
        {
            this._Initialization = new Initialization();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization.InitializePath();

            this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            this.ContentsAreaMinWidth = 1000;
            this.ContentsAreaMinHeight = 600;

            InitSpread();

            GetSearchCondition_Area();

            this.bcboArea_SelectedIndexChanged(null, null);
        }

        private void InitSpread()
        {
            this._Initialization.InitializeButtonList(this.bblstSearch, Definition.PAGE_KEY_SEARCH_CONDITION_SEARCH_BY_AREA, "", this._sessionData);

            this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref this.bsprdSPCModel, Definition.PAGE_KEY_SEARCH_CONDITION_SEARCH_BY_AREA, true, "");

            this._indexColEQP = (int)this._slColumnIndex[COL_NAME_EQP_ID_CONTEXT];
            this._indexColModule = (int)this._slColumnIndex[COL_NAME_MODULE_ID_CONTEXT];
            this._indexColRecipe = (int)this._slColumnIndex[COL_NAME_RECIPE_ID_CONTEXT];
            this._indexColStep = (int)this._slColumnIndex[COL_NAME_STEP_ID_CONTEXT];
            this._indexColModelConfigRawID = (int)this._slColumnIndex[COL_NAME_MODEL_CONFIG_RAWID];

            this.bsprdSPCModel.UseAutoSort = false;
        }




        ///////////////////////////////////////////////////////////////////////////////////
        //  Event Handler.
        ///////////////////////////////////////////////////////////////////////////////////

        void bblstSearch_ButtonClick(string name)
        {
            
            if (name.Equals(Definition.ButtonKey.EXPORT))
            {
                if (this._threadContextInfo != null && this._threadContextInfo.IsAlive)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_BINDING_JOB_WORKING", null, null);
                    return;
                }

                this.bsprdSPCModel.Export(false);
            }
            else if (name.ToUpper().Equals("SEARCH"))
            {
                this.KillThread();

                //this.pnl_SPCModel.Visible = false;
                this.bccbo_SPCModelName.Items.Clear();
                this.bccbo_SPCModelName.Text = "";

                this.GetSearchModel();
            }
        }

        void bbtnOK_Click(object sender, EventArgs e)
        {
            this.KillThread();
            
            this.Close();
        }


 

        ///////////////////////////////////////////////////////////////////////////////////
        //  User Defined Method.
        ///////////////////////////////////////////////////////////////////////////////////

        private void GetSearchCondition_Area()
        {
            _llstChartSearchCondition.Clear();
            _llstChartSearchCondition.Add(Definition.DynamicCondition_Condition_key.LINE, this._sessionData.Line);

            DataSet dsConditionData_Area = this._ws.GetArea(_llstChartSearchCondition.GetSerialData());

            DataTable dsCoData_Area = dsConditionData_Area.Tables[0];
            
            //SPC-797 Search By Area Duplicated Info Meassage
            if (dsCoData_Area != null)
            {
                List<string> lstAreas = new List<string>();
                foreach (DataRow dr in dsCoData_Area.Rows)
                {
                    string sArea = dr["AREA"].ToString();
                    if (lstAreas.Contains(sArea))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_DUPLICATE_AREA", null, null);
                        break;
                    }
                    lstAreas.Add(sArea);
                }
            }


            this.bcboArea.DataSource = dsConditionData_Area.Tables[0];
            this.bcboArea.DisplayMember = "AREA";
            this.bcboArea.ValueMember = "AREA_RAWID";
        }

        private void GetSearchModel()
        {
            try
            {
                this.MsgShow(this._mlthandler.GetMessage(Definition.LOADING_DATA));

                this.KillThread();
                this._dtContextInfo = null;                

                _llstChartSearchCondition.Clear();
                _llstChartSearchCondition.Add(Definition.DynamicCondition_Condition_key.AREA, this.bcboArea.SelectedValue.ToString());

                DataSet dsModel = this._ws.SearchByArea(_llstChartSearchCondition.GetSerialData());

                //Chris 추가 부분
                if (dsModel != null)
                {
                    if (dsModel.Tables.Count > 0)
                    {
                        this._dsAreaAndSPCModelName = dsModel.Copy();

                        this.bccbo_SPCModelName.IsIncludeAll = true;

                        LinkedList llstItem = this.GetDistinctColumnItemToDataTable(this._dsAreaAndSPCModelName.Tables[0], Definition.CONDITION_KEY_SPC_MODEL_NAME);

                        if (llstItem != null)
                        {
                            int listcount = llstItem.Count;

                            for (int i = 0; i < listcount; i++)
                            {
                                bccbo_SPCModelName.Items.Add(llstItem.GetKey(i).ToString());
                            }
                        }
                    }
                }
                //여기까지

                this.bsprdSPCModel.DataSource = dsModel.Tables[0];

                this._dtContextInfo = dsModel.Tables[1];
            }
            catch (Exception ex) { }
            finally
            {
                this.MsgClose();
            }

            this._threadContextInfo = new Thread(new ThreadStart(WriteContextInfo));
            this._threadContextInfo.Start();
        }

        //Chris :: Column Distinct List 반환
        private LinkedList GetDistinctColumnItemToDataTable(DataTable dt, string sColumnName)
        {
            LinkedList llstData = new LinkedList();

            try
            {
                int RowCount = dt.Rows.Count;

                string sColumn = "";

                for (int i = 0; i < RowCount; i++)
                {
                    sColumn = dt.Rows[i][sColumnName].ToString();

                    if (!llstData.Contains(sColumn))
                    {
                        llstData.Add(sColumn, sColumn);
                    }
                }

                return llstData;
            }
            catch
            {
                return null;
            }            
        }

        private void SetGridText(int row, int column, string text)
        {
            if (this.bsprdSPCModel.InvokeRequired)
            {
                SetGridCallBack dele = new SetGridCallBack(SetGridText);
                this.Invoke(dele, new object[] { row, column, text });
            }
            else
            {
                this.bsprdSPCModel.ActiveSheet.Cells[row, column].Text = text;
            }
        }

        private void SetProgressText(string displayText)
        {
            if (this.bsprdSPCModel.InvokeRequired)
            {
                SetProgressCallBack dele = new SetProgressCallBack(SetProgressText);
                this.Invoke(dele, new object[] { displayText });
            }
            else
            {
                this.lblProgress.Text = displayText;
            }
        }

        //Chris
        private void SetPnlSPCModelVisible(bool iVisible)
        {
            if (this.bsprdSPCModel.InvokeRequired)
            {
                SetPnlSPCModelVisibleCallBack dele = new SetPnlSPCModelVisibleCallBack(SetPnlSPCModelVisible);
                this.Invoke(dele, new object[] { iVisible });
            }
            else
            {
                this.pnl_SPCModel.Visible = iVisible;
            }
        }

        private void WriteContextInfo()
        {
            if (this._dtContextInfo != null && this._dtContextInfo.Rows.Count > 0)
            {
                int clearCount = 0;

                SetProgressText(PROGRESS_STRING);

                for (int i = 0; i < this.bsprdSPCModel.ActiveSheet.RowCount; i++)
                {
                    if (i % 50 == 0)
                    {
                        clearCount++;

                        if (clearCount > 100)
                        {
                            clearCount = 0;
                            SetProgressText(PROGRESS_STRING);
                        }
                        else
                        {
                            string msg = this.lblProgress.Text + ".";
                            SetProgressText(msg);
                        }
                    }

                    string modelConfigRawID = this.bsprdSPCModel.ActiveSheet.Cells[i, _indexColModelConfigRawID].Text;

                    DataRow[] selectedRow = this._dtContextInfo.Select("MODEL_CONFIG_RAWID = '" + modelConfigRawID + "'");

                    foreach (DataRow row in selectedRow)
                    { 
                        string key = row["CONTEXT_KEY"].ToString();
                        string value = row["CONTEXT_VALUE"].ToString();

                        switch(key)
                        {
                            case "EQP_ID":
                                SetGridText(i, _indexColEQP, value);
                                break;

                            case "MODULE_ID":
                                SetGridText(i, _indexColModule, value);
                                break;

                            case "RECIPE_ID":
                                SetGridText(i, _indexColRecipe, value);
                                break;

                            case "STEP_ID":
                                SetGridText(i, _indexColStep, value);
                                break;
                        }
                    }
                }
                
                string finishedMsg = "Binding of context info was finished.";                
                SetProgressText(finishedMsg);
                //SetPnlSPCModelVisible(true);
            }
        }

        private void KillThread()
        {
            if (this._threadContextInfo != null)
            {
                if (this._threadContextInfo.IsAlive)
                {
                    this._threadContextInfo.Abort();
                    this._threadContextInfo.Join();
                }

                this._threadContextInfo = null;
            }
        }

        private void bbtn_Search_Click(object sender, EventArgs e)
        {
            int SelectItem = bccbo_SPCModelName.chkBox.CheckedItems.Count;

            if (bccbo_SPCModelName.chkBox.CheckedItems.Contains("ALL"))
            {
                try
                {
                    this.MsgShow(this._mlthandler.GetMessage(Definition.LOADING_DATA));

                    this.KillThread();
                    this._dtContextInfo = null;

                    _llstChartSearchCondition.Clear();
                    _llstChartSearchCondition.Add(Definition.DynamicCondition_Condition_key.AREA, this.bcboArea.SelectedValue.ToString());

                    DataSet dsModel = this._ws.SearchByArea(_llstChartSearchCondition.GetSerialData());

                    //SPC-920 Search 시 팝업 메시지 추가 요청 By Louis
                    if (dsModel.Tables[0].Rows.Count == 0)
                    {
                        this.MsgClose();
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_DATA", null, null);
                        return;
                    }

                    this.bsprdSPCModel.DataSource = dsModel.Tables[0];

                    this._dtContextInfo = dsModel.Tables[1];
                }
                catch(Exception exec)
                {
                    LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, exec);
                }
                finally
                {
                    this.MsgClose();
                }
                this._threadContextInfo = new Thread(new ThreadStart(WriteContextInfo));
                this._threadContextInfo.Start();
            }


            else if (SelectItem > 0)
            {
                try
                {
                    this.MsgShow(this._mlthandler.GetMessage(Definition.LOADING_DATA));

                    this.KillThread();
                    this._dtContextInfo = null;

                    this.bsprdSPCModel.DataSource = new DataSet();

                    _llstChartSearchCondition.Clear();
                    _llstChartSearchCondition.Add(Definition.DynamicCondition_Condition_key.AREA, this.bcboArea.SelectedValue.ToString());


                    string SelectSQL = "";

                    for (int i = 0; i < SelectItem; i++)
                    {
                        SelectSQL += ",'" + bccbo_SPCModelName.chkBox.CheckedItems[i].ToString() + "'";
                    }

                    SelectSQL = SelectSQL.Remove(0, 1);

                    //이거 조건으로 보낼 것
                    _llstChartSearchCondition.Add(Definition.CONDITION_KEY_SPC_MODEL_NAME, SelectSQL);

                    DataSet dsModel = this._ws.SearchByArea(_llstChartSearchCondition.GetSerialData());

                    //SPC-920 Search 시 팝업 메시지 추가 요청 By Louis
                    if (dsModel.Tables[0].Rows.Count == 0)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NO_DATA", null, null);
                    }


                    this.bsprdSPCModel.DataSource = dsModel.Tables[0];

                    this._dtContextInfo = dsModel.Tables[1];


                    //DataRow[] dr = this._dsAreaAndSPCModelName.Tables[0].Select("SPC_MODEL_NAME IN (" + SelectSQL + ")");

                  
                    //DataSet dsSPCModel = this._dsAreaAndSPCModelName.Clone();
                    //dsSPCModel.Merge(dr);

                    //this.bsprdSPCModel.DataSource = dsSPCModel.Tables[0];

                    //this._dtContextInfo = this._dsAreaAndSPCModelName.Tables[1];
                }
                catch (Exception ex) { }
                finally
                {
                    this.MsgClose();
                }

                this._threadContextInfo = new Thread(new ThreadStart(WriteContextInfo));
                this._threadContextInfo.Start();

            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Warning, "SPC_INFO_SELECT_ITEM", null, null);
            }

        }

        //Chris
        private void bcboArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.MsgShow(this._mlthandler.GetMessage(Definition.LOADING_DATA));
                
                int AreaRawID;
                if (int.TryParse(this.bcboArea.SelectedValue.ToString(), out AreaRawID))
                {
                    _llstChartSearchCondition.Clear();
                    _llstChartSearchCondition.Add(Definition.DynamicCondition_Condition_key.AREA, this.bcboArea.SelectedValue.ToString());

                    DataSet dsModel = this._ws.SearchByAreaSPCModelName(_llstChartSearchCondition.GetSerialData());

                    if (dsModel != null)
                    {
                        if (dsModel.Tables.Count > 0)
                        {

                            this.bccbo_SPCModelName.DataSource = dsModel.Tables[0];
                            this.bccbo_SPCModelName.DisplayMember = Definition.CONDITION_KEY_SPC_MODEL_NAME;

                            this.bccbo_SPCModelName.IsIncludeAll = true;


                        }
                    }
                }
            }
            catch (Exception ex) { }
            finally
            {
                this.MsgClose();
            }
        }
    }
}
