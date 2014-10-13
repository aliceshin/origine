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
using BISTel.eSPC.Common.ATT;

namespace BISTel.eSPC.Page.ATT.Modeling
{
    public partial class SPCModelHistoryPopup : BasePopupFrm
    {
        #region : Constructor
        public SPCModelHistoryPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region : Field

        Initialization _Initialization;

        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _ws;

        SortedList _slContextColumnIndex = new SortedList();
        SortedList _slHistoryColumnIndex = new SortedList();
        SortedList _slButtonIndex = new SortedList();

        private const string COL_NAME_EQP_ID_CONTEXT = "EQP ID";
        private const string COL_NAME_MODULE_ID_CONTEXT = "MODULE ID";
        private const string COL_NAME_RECIPE_ID_CONTEXT = "RECIPE ID";
        private const string COL_NAME_STEP_ID_CONTEXT = "STEP ID";

        int _indexColEQP = -1;
        int _indexColModule = -1;
        int _indexColRecipe = -1;
        int _indexColStep = -1;

        DataSet _dsSPCModelData = new DataSet();

        private string _sModelConfigRawID_Private = "";
        public string _sModelConfigRawID_Public
        {
            get
            {
                return this._sModelConfigRawID_Private;
            }
            set
            {
                this._sModelConfigRawID_Private = value;
            }
        }

        private bool _IsHistoryData_Pr = true;
        public bool _IsHistoryData_Pb
        {
            get
            {
                return this._IsHistoryData_Pr;
            }
            set
            {
                this._IsHistoryData_Pr = value;
            }
        }

        #endregion



        #region : Initialization
        
        public void InitializePopup()
        {
            this.InitializeLayout();
            this.InitializeBspread();
            this.InitializeDataButton();
            this.InitData();
        }

        public void InitializeLayout()
        {
            this._ws = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();
        }

        public void InitializeBspread()
        {
            this._slContextColumnIndex = this._Initialization.InitializeColumnHeader(ref this.bsprData_ContextInfo, Definition.PAGE_KEY_SPC_MODEL_HISTORY_POPUP, true, Definition.HEADER_KEY_MODEL_CONTEXT_INFO);

            this._indexColEQP = (int)this._slContextColumnIndex[COL_NAME_EQP_ID_CONTEXT];
            this._indexColModule = (int)this._slContextColumnIndex[COL_NAME_MODULE_ID_CONTEXT];
            this._indexColRecipe = (int)this._slContextColumnIndex[COL_NAME_RECIPE_ID_CONTEXT];
            this._indexColStep = (int)this._slContextColumnIndex[COL_NAME_STEP_ID_CONTEXT];            

            this._slHistoryColumnIndex = this._Initialization.InitializeColumnHeader(ref this.bsprData_HistoryInfo, Definition.PAGE_KEY_SPC_MODEL_HISTORY_POPUP, true, Definition.HEADER_KEY_MODEL_HISTORY_INFO);
        }

        public void InitializeDataButton()
        {
            this._slButtonIndex = this._Initialization.InitializeButtonList(this.bbtnlist_History, Definition.PAGE_KEY_SPC_MODEL_HISTORY_POPUP, Definition.BUTTONLIST_KEY_SPC_MODEL_HISTORY_LIST, null);
        }

        public void InitData()
        {
            try
            {                
                this.SetContextSpreadData();

                this.SetHistorySpreadData();
            }
            catch (Exception exec)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, exec);
            }
            finally
            {
            }
        }

        #endregion

        #region : PageLogic

        private void SetContextSpreadData()
        {
            //RawID를 가지고 온다
            string sModelConfigRawID = this._sModelConfigRawID_Private;

            //LinkedList를 만든다
            LinkedList llstData = new LinkedList();
            llstData.Add(BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, sModelConfigRawID);

            byte[] baData = llstData.GetSerialData();

            //웹서비스로 보내 DataSet을 받는다            
            DataSet ds = this._ws.GetATTSPCModelContextInfo(baData);

            //받은 데이터를 칼럼에 넣는다.
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.bsprData_ContextInfo.ActiveSheet.Rows.Add(0, 1);

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string key = ds.Tables[0].Rows[i]["CONTEXT_KEY"].ToString();
                            string value = ds.Tables[0].Rows[i]["CONTEXT_VALUE"].ToString();                            

                            switch (key)
                            {
                                case "EQP_ID":
                                    this.bsprData_ContextInfo.ActiveSheet.Cells[0, this._indexColEQP].Text = value;
                                    break;

                                case "MODULE_ID":
                                    this.bsprData_ContextInfo.ActiveSheet.Cells[0, this._indexColModule].Text = value;
                                    break;

                                case "RECIPE_ID":
                                    this.bsprData_ContextInfo.ActiveSheet.Cells[0, this._indexColRecipe].Text = value;
                                    break;

                                case "STEP_ID":
                                    this.bsprData_ContextInfo.ActiveSheet.Cells[0, this._indexColStep].Text = value;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_CONTEXT_INFO_NOT_EXIST", null, null);
                        this._IsHistoryData_Pr = false;
                    }
                }                
            }
        }

        private void SetHistorySpreadData()
        {
            //RawID를 가지고 온다
            string sModelConfigRawID = this._sModelConfigRawID_Private;

            //LinkedList를 만든다
            LinkedList llstData = new LinkedList();
            llstData.Add(BISTel.eSPC.Common.COLUMN.MODEL_CONFIG_RAWID, sModelConfigRawID);

            byte[] baData = llstData.GetSerialData();

            //웹서비스로 보내 DataSet을 받는다            
            DataSet ds = this._ws.GetATTSPCModelHistoryInfo(baData);

            //스프레드 바인딩

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.bsprData_HistoryInfo.DataSet = ds;
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_HISTORY_INFO_NOT_EXIST", null, null);
                        this._IsHistoryData_Pr = false;
                    }
                }
                
            }
        }

        

        #endregion

        #region : Event
        
        private void bbtnlist_History_ButtonClick(string name)
        {
            if (name.Equals(Definition.ButtonKey.EXPORT))
            {
                this.bsprData_HistoryInfo.Export(false);
            }
        }

        #endregion
    }
}