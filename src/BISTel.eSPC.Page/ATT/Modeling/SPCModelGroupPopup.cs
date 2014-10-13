using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.DataHandler;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common.ATT;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.BISTelControl.BConditionTree;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition.DCControl;
using BISTel.PeakPerformance.Client.BaseUserCtrl.ProgressBar;

namespace BISTel.eSPC.Page.ATT.Modeling
{
    public partial class SPCModelGroupPopup : BasePopupFrm
    {
        private eSPCWebService.eSPCWebService _wsSPC;
        private MultiLanguageHandler _mlthandler;
        private Initialization _Initialization;
        private BSpreadUtility _SpreadUtil;
        private CommonUtility _ComUtil;
        private System.Collections.SortedList _slGroupColumnIndex;
        private SessionData _SessionData;
        private DataSet _dsGroupList;
        private DataTable _dtGroup;
        //private bool isMET = false;
        private ADynamicCondition _dynamicEQP;
        private TreeNode _tnCheckedComboNode;
        private string sLevelValue;
        private string sEQPModel;
        private string _sSPCModelLevel;
        private string sAreaRawID;
        private string sLocationRawID;
        private string sGroupRawID;
        private BTreeCombo _btreeComboEQP;
        //private bool _isMET;

        enum GroupColumnIndex : int
        {
            Insert = 0,
            Modify,
            Delete,
            Select,
            Rawid,
            GroupModel
        };

        enum SPCModelColumnIndex : int
        {
            Select = 0,
            Rawid,
            SPCModel
        };

        enum ModelMappingColumnIndex : int
        {
            Insert = 0,
            Select,
            Delete,
            LocationRawid,
            AreaRawid,
            Rawid,
            SPCModel
        };

        public SessionData SESSIONDATA
        {
            get { return _SessionData; }
            set { _SessionData = value; }
        }

        //public bool IS_MET
        //{
        //    get { return _isMET; }
        //    set { _isMET = value; }
        //}

        public SPCModelGroupPopup()
        {
            InitializeComponent();
        }

        public void InitializePopup()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._Initialization = new Initialization();
            this._Initialization.InitializePath();

            this._SpreadUtil = new BSpreadUtility();
            this._ComUtil = new CommonUtility();
            this._SessionData = new SessionData();
            this._dsGroupList = new DataSet();

            this.Title = "Model Group Mapping";

            InitializeButtonList();
            InitializeSpread();
        }

        private void InitializeButtonList()
        {
            this._Initialization.InitializeButtonList(this.bbtnButtonList, "SPC_MODEL_GROUP_POPUP", "BTNLIST_MODEL_GROUP", _SessionData);
        }

        private void SetData()
        {
            this.bComboGroupList.Items.Clear();

            DataTable dtGroup = new DataTable();
            DataRow[] dr = _dsGroupList.Tables["MODEL_GROUP_SPC"].Select(string.Format("GROUP_LEVEL ='{0}'", sLevelValue), "GROUP_NAME ASC");

            ((DataSet)bsprGroupList.DataSource).Clear();
            dtGroup = ((DataSet)bsprGroupList.DataSource).Tables[0].Copy();

            if (dr.Length > 0)
            {
                DataRow drGroup;
                foreach (DataRow row in dr)
                {
                    drGroup = dtGroup.NewRow();
                    drGroup[(int)GroupColumnIndex.Insert] = Definition.VARIABLE_FALSE;
                    drGroup[(int)GroupColumnIndex.Select] = Definition.VARIABLE_FALSE;
                    drGroup[(int)GroupColumnIndex.Modify] = Definition.VARIABLE_FALSE;
                    drGroup[(int)GroupColumnIndex.Delete] = Definition.VARIABLE_FALSE;
                    drGroup[BISTel.eSPC.Common.COLUMN.RAWID] = row[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    drGroup[BISTel.eSPC.Common.COLUMN.LOCATION_RAWID] = row[BISTel.eSPC.Common.COLUMN.LOCATION_RAWID].ToString();
                    drGroup[BISTel.eSPC.Common.COLUMN.AREA_RAWID] = row[BISTel.eSPC.Common.COLUMN.AREA_RAWID].ToString();
                    drGroup[Definition.CONDITION_KEY_GROUP_NAME] = row[Definition.CONDITION_KEY_GROUP_NAME].ToString();
                    dtGroup.Rows.Add(drGroup);
                }
            }
            else
            {
                ((DataSet)bsprSPCModel.DataSource).Clear();
                bsprSPCModel.DataSet = this.bsprSPCModel.DataSet;
                ((DataSet)bsprModelMapping.DataSource).Clear();
                bsprModelMapping.DataSet = this.bsprModelMapping.DataSet;
            }
            
            this._dtGroup = dtGroup.Copy();
            this.bsprGroupList.DataSet = dtGroup;

            //dr = _dsGroupList.Tables["MODEL_GROUP_SPC"].Select(string.Format("GROUP_LEVEL ='{0}'", sLevelValue), "GROUP_LEVEL ASC");

            //bComboGroupList.Items.Clear();

            //if(this._tnCheckedComboNode.Checked)
            //    bComboGroupList.Items.Add("UNASSINGED MODEL");

            //if (dr.Length > 0)
            //{
            //    foreach (DataRow row in dr)
            //    {
            //        bComboGroupList.Items.Add(row[Definition.CONDITION_KEY_GROUP_NAME].ToString());
            //    }
            //}
            ((DataSet)bsprSPCModel.DataSource).Clear();
            bsprSPCModel.DataSet = this.bsprSPCModel.DataSet;
            ((DataSet)bsprModelMapping.DataSource).Clear();
            bsprModelMapping.DataSet = this.bsprModelMapping.DataSet;
        }

        private void Refresh()
        {
            SetData();
            this.bsprGroupList.ClearData();
            this.bsprSPCModel.ClearData();
            this.bsprModelMapping.ClearData();
        }

        public void InitializeSpread()
        {
            this.bsprGroupList.ClearHead();
            
            this.bsprGroupList.UseFilter = false;
            this.bsprGroupList.FilterVisible = false;
            this.bsprGroupList.UseSpreadEdit = false;
            this.bsprGroupList.AutoGenerateColumns = false;
            this.bsprGroupList.UseGeneralContextMenu = false;
            this._slGroupColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprGroupList, "SPC_MODEL_GROUP_POPUP", true, "HEADER_MODEL_GROUP");
            this.bsprGroupList.ActiveSheet.Columns[(int)GroupColumnIndex.Modify].DataField = "_MODIFY";

            this.bsprGroupList.AddHeadComplete();

            this.bsprSPCModel.ClearHead();
            this.bsprSPCModel.UseFilter = false;
            this.bsprSPCModel.FilterVisible = false;
            this.bsprSPCModel.UseSpreadEdit = false;
            this.bsprSPCModel.AutoGenerateColumns = false;
            this.bsprSPCModel.UseGeneralContextMenu = false;
            this._slGroupColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprSPCModel, "SPC_MODEL_GROUP_POPUP", true, "HEADER_SPC_MODEL_LIST");
            this.bsprSPCModel.AddHeadComplete();

            this.bsprModelMapping.ClearHead();
            this.bsprModelMapping.UseFilter = false;
            this.bsprModelMapping.FilterVisible = false;
            this.bsprModelMapping.UseSpreadEdit = false;
            this.bsprModelMapping.AutoGenerateColumns = false;
            this.bsprModelMapping.UseGeneralContextMenu = false;
            this._slGroupColumnIndex = this._Initialization.InitializeColumnHeader(ref bsprModelMapping, "SPC_MODEL_GROUP_POPUP", true, "HEADER_SPC_MODEL_MAPPING");
            this.bsprModelMapping.AddHeadComplete();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bbtnButtonList_ButtonClick(string name)
        {
            if (this._tnCheckedComboNode!=null && this._tnCheckedComboNode.Checked)
            {
                if (name == Definition.BUTTON_KEY_ADD)
                {
                    bsprGroupList.ContextMenuAction(name);
                    bsprGroupList.ActiveSheet.Cells[bsprGroupList.ActiveSheet.ActiveRowIndex, 3].Locked = true;

                    this.bsprGroupList_LeaveCell(this.bsprGroupList, new FarPoint.Win.Spread.LeaveCellEventArgs(this.bsprGroupList.ActiveSheet.ContainingViews[0], this.bsprGroupList.ActiveSheet.ParentRowIndex,
                        this.bsprGroupList.ActiveSheet.ActiveColumnIndex, this.bsprGroupList.ActiveSheet.ActiveRowIndex, this.bsprGroupList.ActiveSheet.ActiveColumnIndex));
                    
                    //DataRow row = this._dtGroup.NewRow();
                    //this._dtGroup.Rows.Add(row);
                }

                else if (name == Definition.BUTTON_KEY_SAVE)
                {
                    if (bsprGroupList.ActiveSheet.Rows.Count > 0)
                    {
                        bool bGroupName = ValidationGroupName();
                        
                        if (!bGroupName)
                        {
                            return;
                        }

                        DataSet dsSave = bsprGroupList.GetSaveData();
                        DataTable dtModify = FindModifyGroupList();

                        if (dtModify != null && dtModify.Rows.Count>0)
                        {
                            dsSave.Tables.Add(dtModify);
                        }

                        if (dsSave.Tables.Count > 0)
                        {
                            bool bcheckDuplicate = CheckGroupName(dsSave);

                            if (bcheckDuplicate)
                            {
                                LinkedList llParam = new LinkedList();
                                DataRow[] dr = _dsGroupList.Tables["SPC_MODEL_LEVEL"].Select(string.Format("GROUP_LEVEL = '{0}'", sLevelValue));

                                if (dr.Length > 0)
                                {
                                    llParam.Add(BISTel.eSPC.Common.COLUMN.LOCATION_RAWID, sLocationRawID);
                                    llParam.Add(BISTel.eSPC.Common.COLUMN.AREA_RAWID, sAreaRawID);
                                    llParam.Add(BISTel.eSPC.Common.COLUMN.EQP_MODEL, sEQPModel);
                                    llParam.Add(BISTel.eSPC.Common.COLUMN.USER_ID, _SessionData.UserId);
                                }

                                EESProgressBar.ShowProgress(this, this._mlthandler.GetMessage(Definition.MSG_KEY_INFO_SAVING_DATA), true);
                                
                                bool bResult = _wsSPC.SaveATTSPCGroupList(dsSave, llParam.GetSerialData());

                                EESProgressBar.CloseProgress(this);

                                if (!bResult)
                                {
                                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.GENERAL_SAVE_FAIL));
                                    return;
                                }
                                else
                                {

                                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.GENERAL_SAVE_SUCCESS));

                                    EESProgressBar.ShowProgress(this, this._mlthandler.GetMessage(Definition.LOADING_DATA), true);
                                    
                                    this._dsGroupList = _wsSPC.GetATTSPCModelGroupList();
                                    
                                    EESProgressBar.CloseProgress(this);
                                    this.SetData();
                                    this.bComboGroupList_SelectedIndexChanged(bComboGroupList, new EventArgs());
                                }
                            }
                        }
                        else
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.MSG_KEY_NO_CHANGE_DATA));
                            this.bsprGroupList.DataSource = this._dtGroup;
                            return;
                        }
                    }
                }
            }
        }

        private bool ValidationGroupName()
        {
            bool bResult = true;

            string GroupName = string.Empty;

            //파일명 유효성 검사
            for (int i = 0; i < this.bsprGroupList.ActiveSheet.Rows.Count; i++)
            {
                if (this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.GroupModel].Value == null ||
                        String.IsNullOrEmpty(this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.GroupModel].Value.ToString()))
                {
                    if ((this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.Delete].Value != null &&
                        this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.Delete].Value.ToString() == Definition.VARIABLE_TRUE))
                    {

                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_VAILD_GROUP_NAME", null, null);
                        return false;
                    }
                }
                else if (this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.GroupModel].Value.ToString() == Definition.VARIABLE_UNASSIGNED_MODEL)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NOT_USE_UNASSIGNED_MODEL", null, null);
                    return false;
                }
                else
                {
                    GroupName = this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.GroupModel].Value.ToString();
                }

                foreach (char ch in System.IO.Path.GetInvalidFileNameChars())
                {
                    if (GroupName.Contains(ch.ToString()))
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_NOT_USE_SPECIAL_CHAR", null, null);
                        return false;
                    }
                }
            }
            return bResult;
        }

        private bool CheckGroupName(DataSet ds)
        {
            LinkedList llParam = new LinkedList();
            llParam.Add(BISTel.eSPC.Common.COLUMN.LOCATION_RAWID, this.sLocationRawID);
            llParam.Add(BISTel.eSPC.Common.COLUMN.AREA_RAWID, this.sAreaRawID);
            llParam.Add(BISTel.eSPC.Common.COLUMN.SPC_DATA_LEVEL, this._sSPCModelLevel);
            
            if(this._sSPCModelLevel == Definition.CONDITION_KEY_EQP_MODEL)
            {
                llParam.Add(BISTel.eSPC.Common.COLUMN.EQP_MODEL, this.sEQPModel);
            }

            DataTable dt = this.bsprGroupList.ActiveSheet.GetDataView(true).ToTable();

            foreach (DataRow row in dt.Rows)
            {
                int checkCount = 0;
                string name = row[BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString().ToUpper();

                if (ds.Tables.Contains(BISTel.eSPC.Common.TABLE.DATA_SAVE_INSERT))
                {
                    DataRow[] chRows = ds.Tables[BISTel.eSPC.Common.TABLE.DATA_SAVE_INSERT].Select(string.Format("GROUP_NAME = '{0}'", name));

                    if (chRows.Length > 1)
                    {
                        checkCount++;
                    }
                    else
                    {
                        DataRow[] rows = dt.Select(string.Format("GROUP_NAME NOT IN ('{0}')", name));
                        foreach (DataRow dr in rows)
                        {
                            if (dr[BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString().ToUpper() == row[BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString().ToUpper() && dr[(int)GroupColumnIndex.Delete].ToString() != Definition.VARIABLE_TRUE)
                            {
                                checkCount++;
                                //MSGHandler.DisplayMessage(MSGType.Information, "GROUP NAME is Duplicated");
                                //return false;
                            }
                            else if (String.IsNullOrEmpty(dr[BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString()))
                            {
                                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_GROUP_NAME", null, null);
                                return false;
                            }
                        }
                    }
                }

                if (ds.Tables.Contains(BISTel.eSPC.Common.TABLE.DATA_SAVE_UPDATE))
                {
                    foreach (DataRow dr in ds.Tables[BISTel.eSPC.Common.TABLE.DATA_SAVE_UPDATE].Rows)
                    {
                        if (dr[BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString().ToUpper() == row[BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString().ToUpper() && dr[(int)GroupColumnIndex.Delete].ToString() != Definition.VARIABLE_TRUE
                            && row[(int)GroupColumnIndex.Rawid].ToString() != dr[BISTel.eSPC.Common.COLUMN.RAWID].ToString())
                        {
                            checkCount++;
                            //MSGHandler.DisplayMessage(MSGType.Information, "GROUP NAME is Duplicated");
                            //return false;
                        }
                        else if (String.IsNullOrEmpty(dr[BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString()))
                        {
                            MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_INPUT_GROUP_NAME", null, null);
                            return false;
                        }
                    }
                }

                if (checkCount > 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_DUPLICATE_GROUP_NAME", null, null);
                    return false;
                }
            }


            bool bResult = this._wsSPC.CheckDuplicateATTGroupName(ds, llParam.GetSerialData());

            if (!bResult)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_DUPLICATE_GROUP_NAME", null, null);
            }

            return bResult;
        }

        private DataTable FindModifyGroupList()
        {
            DataTable dtCurrentData = ((DataSet)this.bsprGroupList.DataSet).Tables[0].Copy();
            DataTable dt = dtCurrentData.Clone();

            for (int i = 0; i < this._dtGroup.Rows.Count; i++)
            {
                if (this.bsprGroupList.ActiveSheet.Cells[i,(int)GroupColumnIndex.Modify].Value.ToString() == Definition.VARIABLE_TRUE)
                {
                    dt.ImportRow(dtCurrentData.Rows[i]);
                }
            }
            dt.TableName = BISTel.eSPC.Common.TABLE.DATA_SAVE_UPDATE;
            return dt;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool bResult = false;
            DataSet dsMapping = this.bsprModelMapping.GetSaveData();//GetModelGroupMappingData();
            LinkedList llParam = new LinkedList();
            llParam.Add(Definition.CONDITION_KEY_USER_ID, this._SessionData.UserId);
            llParam.Add(BISTel.eSPC.Common.COLUMN.GROUP_RAWID, sGroupRawID);

            if (bsprModelMapping.ActiveSheet.Rows.Count > 0)
            {
                if (dsMapping.Tables.Contains(BISTel.eSPC.Common.TABLE.DATA_SAVE_INSERT) || dsMapping.Tables.Contains(BISTel.eSPC.Common.TABLE.DATA_SAVE_DELETE))
                {
                    EESProgressBar.ShowProgress(this, this._mlthandler.GetMessage(Definition.MSG_KEY_INFO_SAVING_DATA), true);

                    bResult = _wsSPC.SaveATTSPCModelMapping(dsMapping, llParam.GetSerialData());

                    EESProgressBar.CloseProgress(this);

                    if (!bResult)
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.GENERAL_SAVE_FAIL));
                        return;
                    }
                    else
                    {
                        MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage(Definition.GENERAL_SAVE_SUCCESS));

                        EESProgressBar.ShowProgress(this, this._mlthandler.GetMessage(Definition.LOADING_DATA), true);
                       
                        this._dsGroupList = _wsSPC.GetATTSPCModelGroupList();
                        
                        EESProgressBar.CloseProgress(this);

                        SetData();
                    }
                }
                else
                {
                    MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("GENERAL_NO_CHANGE"));
                    return;
                }
            }
            else
            {
                MSGHandler.DisplayMessage(MSGType.Information, MSGHandler.GetMessage("GENERAL_NO_SAVE_DATA"));
                return;
            }
        }

        private DataSet GetModelGroupMappingData()
        {
            DataSet ds = this.bsprModelMapping.DataSet as DataSet;
            DataSet dsSave = new DataSet();

            if (this.bsprModelMapping.GetSelectedCheckRows((int)ModelMappingColumnIndex.Insert).Count > 0)
            {
                DataTable dt = ds.Tables[0].Clone();
                dt.TableName = BISTel.eSPC.Common.TABLE.DATA_SAVE_INSERT;
                dsSave.Tables.Add(dt);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[(int)ModelMappingColumnIndex.Select].ToString() == Definition.VARIABLE_TRUE)
                    {
                        dsSave.Tables[BISTel.eSPC.Common.TABLE.DATA_SAVE_INSERT].ImportRow(dr);
                    }
                }
            }

            if (this.bsprModelMapping.GetSelectedCheckRows((int)ModelMappingColumnIndex.Delete).Count > 0)
            {
                DataTable dt = ds.Tables[0].Clone();
                dt.TableName = BISTel.eSPC.Common.TABLE.DATA_SAVE_DELETE;
                dsSave.Tables.Add(dt);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[(int)ModelMappingColumnIndex.Delete].ToString() == Definition.VARIABLE_TRUE)
                    {
                        dsSave.Tables[BISTel.eSPC.Common.TABLE.DATA_SAVE_DELETE].ImportRow(dr);
                    }
                }
            }

            return dsSave;
        }

        private void bsprGroupList_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (bsprGroupList.ActiveSheet.Columns[e.Column].Label == BISTel.eSPC.Common.COLUMN.SELECT)
            {
                //Unchecked.
                if (this.bsprGroupList.ActiveSheet.Cells[e.Row, e.Column].Value.ToString() == Definition.VARIABLE_FALSE)
                {
                    sGroupRawID = string.Empty;

                    ((DataSet)bsprModelMapping.DataSource).Clear();
                    this.bsprModelMapping.DataSet = this.bsprModelMapping.DataSet;

                    bComboGroupList.Items.Clear();

                    ((DataSet)bsprSPCModel.DataSource).Clear();
                    this.bsprSPCModel.DataSet = this.bsprSPCModel.DataSet;
                }
                else        //check.
                {
                    sGroupRawID = this.bsprGroupList.ActiveSheet.Cells[e.Row, (int)GroupColumnIndex.Rawid].Value.ToString();

                    DataTable dtGroup = new DataTable();
                    DataRow[] dr = _dsGroupList.Tables[BISTel.eSPC.Common.TABLE.MODEL_MST_SPC].Select(
                        string.Format("GROUP_NAME ='{0}' AND LOCATION_RAWID = '{1}' AND AREA_RAWID = '{2}'", bsprGroupList.ActiveSheet.Cells[e.Row, (int)GroupColumnIndex.GroupModel].Value, this.sLocationRawID, this.sAreaRawID), "SPC_MODEL_NAME ASC");

                    if (dr.Length > 0)              //group에 model이 존재하는 경우
                    {
                        ((DataSet)bsprModelMapping.DataSource).Clear();

                        dtGroup = ((DataSet)bsprModelMapping.DataSource).Tables[0].Copy();
                        DataRow drGroup;

                        foreach (DataRow row in dr)
                        {
                            drGroup = dtGroup.NewRow();
                            drGroup[BISTel.eSPC.Common.COLUMN.RAWID] = row[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                            drGroup[BISTel.eSPC.Common.COLUMN.LOCATION_RAWID] = row[BISTel.eSPC.Common.COLUMN.LOCATION_RAWID].ToString();
                            drGroup[BISTel.eSPC.Common.COLUMN.AREA_RAWID] = row[BISTel.eSPC.Common.COLUMN.AREA_RAWID].ToString();
                            drGroup[Definition.CONDITION_KEY_MODEL_NAME] = row[BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString();
                            dtGroup.Rows.Add(drGroup);
                        }

                        bsprModelMapping.DataSource = dtGroup;

                        for (int i = 0; i < bsprModelMapping.ActiveSheet.Rows.Count; i++)
                        {
                            bsprModelMapping.ActiveSheet.Cells[i, (int)ModelMappingColumnIndex.Select].Locked = true;
                            //bsprModelMapping.ActiveSheet.Cells[i, (int)ModelMappingColumnIndex.SPCModel].Locked = true;
                        }
                    }
                    else                //group에 model이 없는 경우
                    {
                        ((DataSet)bsprModelMapping.DataSource).Clear();
                        bsprModelMapping.DataSet = this.bsprModelMapping.DataSet;
                    }

                    //check 한 row이외에 모든 row에 대해 select column uncheck.
                    for (int i = 0; i < this.bsprGroupList.ActiveSheet.Rows.Count; i++)
                    {
                        if (i != e.Row)
                        {
                            this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.Select].Value = Definition.VARIABLE_FALSE;
                        }
                    }
                    
                    dr = _dsGroupList.Tables["MODEL_GROUP_SPC"].Select(string.Format("GROUP_LEVEL ='{0}'", sLevelValue), "GROUP_LEVEL ASC");
                    
                    bComboGroupList.Items.Clear();
                    
                    //Default로 Unssigned Model 추가.
                    if (this._tnCheckedComboNode.Checked)
                        bComboGroupList.Items.Add(Definition.VARIABLE_UNASSIGNED_MODEL);

                    //선택한 group을 제외한 group list를 추가.
                    if (dr.Length > 0)
                    {
                        foreach (DataRow row in dr)
                        {
                            if (row[BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString() != bsprGroupList.ActiveSheet.Cells[e.Row, (int)GroupColumnIndex.GroupModel].Value.ToString())
                                bComboGroupList.Items.Add(row[Definition.CONDITION_KEY_GROUP_NAME].ToString());
                        }
                    }

                    this.bsprSPCModel.GetDataSource().Clear();
                    this.bsprSPCModel.DataSet = this.bsprSPCModel.DataSet;

                }
            }
        }

        private void bComboGroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtGroup = new DataTable();
            DataRow[] dr = _dsGroupList.Tables["MODEL_MST_SPC"].Select(string.Format("GROUP_NAME ='{0}' AND LOCATION_RAWID ='{1}' AND AREA_RAWID ='{2}'", bComboGroupList.SelectedItem, sLocationRawID, sAreaRawID),"SPC_MODEL_NAME ASC");

            if (dr.Length > 0)
            {
                ((DataSet)bsprSPCModel.DataSource).Clear();

                dtGroup = ((DataSet)bsprSPCModel.DataSource).Tables[0].Copy();
                DataRow drGroup;

                foreach (DataRow row in dr)
                {
                    drGroup = dtGroup.NewRow();
                    drGroup[BISTel.eSPC.Common.COLUMN.RAWID] = row[BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                    drGroup[Definition.CONDITION_KEY_MODEL_NAME] = row[BISTel.eSPC.Common.COLUMN.SPC_MODEL_NAME].ToString();
                    dtGroup.Rows.Add(drGroup);
                }

                bsprSPCModel.DataSource = dtGroup;
            }
            else
            {
                bsprSPCModel.ClearData();
            }
        }


        private void bsprGroupList_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            if (this.bsprGroupList.DataSet != null)
            {
                DataTable dt = ((DataSet)this.bsprGroupList.DataSet).Tables[0];

                //Insert된 Row에 대해서 Select column Cell Lock 설정
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.Insert].Value.ToString() == Definition.VARIABLE_TRUE)
                    {
                        this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.Select].Locked = true;
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.GroupModel].Value != null)
                        this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.GroupModel].Value = this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.GroupModel].Value.ToString().ToUpper();
                }

                for (int i = 0; i < _dtGroup.Rows.Count; i++)
                {
                    if (dt.Rows[i][BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString() == _dtGroup.Rows[i][BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString())
                    {
                        this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.Modify].Value = Definition.VARIABLE_FALSE;
                    }
                    else
                    {
                        this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.Modify].Value = Definition.VARIABLE_TRUE;
                    }
                }

                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                //    for (int j = 0; j < dt.Rows.Count; j++)
                //    {
                //        //Group Name이 변경된 경우만 Modify Column에 check.
                //        if (dt.Rows.Count == this._dtGroup.Rows.Count)
                //        {
                //            if (dt.Rows[j][i].ToString() != this._dtGroup.Rows[j][i].ToString() && (i != (int)GroupColumnIndex.Insert && i != (int)GroupColumnIndex.Modify
                //                && i != (int)GroupColumnIndex.Delete && i != (int)GroupColumnIndex.Select) && dt.Rows[j][(int)GroupColumnIndex.Insert].ToString() != Definition.VARIABLE_TRUE)
                //            {
                //                this.bsprGroupList.ActiveSheet.Cells[j, (int)GroupColumnIndex.Modify].Value = Definition.VARIABLE_TRUE;
                //                //return;
                //            }
                //            //else
                //            //{
                //            //    this.bsprGroupList.ActiveSheet.Cells[j, (int)GroupColumnIndex.Modify].Value = Definition.VARIABLE_FALSE;
                //            //}
                //        }
                //    }
                //}
            }
        }

        private void bsprGroupList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            DataTable dt = this.bsprGroupList.ActiveSheet.GetDataView(true).ToTable();

            if (e.Column == (int)GroupColumnIndex.Select)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i != e.Row)
                    {
                        dt.Rows[i][(int)GroupColumnIndex.Select] = Definition.VARIABLE_FALSE;
                    }
                }

                //Modify Column에 Check된 항목 변경되지 않도록 다시 Check함.
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][(int)GroupColumnIndex.Insert].ToString() == Definition.VARIABLE_FALSE)
                    {
                        if (dt.Rows[i][(int)GroupColumnIndex.Modify].ToString() == Definition.VARIABLE_TRUE)
                        {
                            dt.Rows[i][(int)GroupColumnIndex.Modify] = Definition.VARIABLE_TRUE;
                        }
                    }
                }
                this.bsprGroupList.DataSet = this.bsprGroupList.DataSet;

                //Insert된 Row에 대한 Select Column Cell Lock 설정.
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.Insert].Value.ToString() == Definition.VARIABLE_TRUE)
                    {
                        this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.Select].Locked = true;
                    }
                }
            }
        }

        private void btn_Right_Click(object sender, EventArgs e)
        {
            if (this._dtGroup == null)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_GROUP_NAME", null, null);
                return;
            }

            DataSet dsSave = bsprGroupList.GetSaveData();
            DataTable dtModify = FindModifyGroupList();

            if (dtModify != null && dtModify.Rows.Count > 0)
            {
                dsSave.Tables.Add(dtModify);
            }

            if (dsSave.Tables.Count != 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SAVE_GROUP_LIST_FIRST", null, null);
                return;
            }

            DataTable dtCheckSPCModel = this.bsprSPCModel.ActiveSheet.GetDataView(true).ToTable();
            dtCheckSPCModel.Columns.Add(BISTel.eSPC.Common.COLUMN.GROUP_RAWID);

            bool isEmptyGroupRawid = true;

            if (bComboGroupList.SelectedItem == null || bComboGroupList.Items.Count < 1)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_GROUP_NAME", null, null);
                return;
            }

            if(bComboGroupList.SelectedItem.ToString() == Definition.VARIABLE_UNASSIGNED_MODEL)
                isEmptyGroupRawid = true;
            else
                isEmptyGroupRawid = false;

            DataRow[] datarow = this._dtGroup.Select(string.Format("GROUP_NAME = '{0}' ", bComboGroupList.SelectedItem.ToString()));
           
            if (datarow.Length > 0)
            {
                foreach (DataRow model in dtCheckSPCModel.Rows)
                {
                    if(model[(int)SPCModelColumnIndex.Select].ToString() == Definition.VARIABLE_TRUE)
                        model[BISTel.eSPC.Common.COLUMN.GROUP_RAWID] = datarow[0][BISTel.eSPC.Common.COLUMN.RAWID].ToString();
                }
            }

            string strCheck = this._wsSPC.CheckDuplicateATTMapping(dtCheckSPCModel, isEmptyGroupRawid);

            if (!string.IsNullOrEmpty(strCheck))
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_RELOAD_GROUP_LIST", new string[] { strCheck, this.bComboGroupList.SelectedItem.ToString() }, null);
                return;
            }
            
            LinkedList llCheckParam = new LinkedList();
            llCheckParam.Add(BISTel.eSPC.Common.COLUMN.LOCATION_RAWID, this.sLocationRawID);
            llCheckParam.Add(BISTel.eSPC.Common.COLUMN.AREA_RAWID, this.sAreaRawID);
            llCheckParam.Add(BISTel.eSPC.Common.COLUMN.GROUP_RAWID, this.sGroupRawID);

            int checkSPCModelCnt = 0;

            for (int i = 0; i < this.bsprSPCModel.ActiveSheet.Rows.Count; i++)
            {
                if (this.bsprSPCModel.ActiveSheet.Cells[i, (int)SPCModelColumnIndex.Select].Value != null &&
                    (this.bsprSPCModel.ActiveSheet.Cells[i, (int)SPCModelColumnIndex.Select].Value.ToString() == Definition.VARIABLE_TRUE))
                {
                    checkSPCModelCnt++;
                }
            }

            if (checkSPCModelCnt == 0)
            {
                MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_SELECT_MODEL", null, null);
                return;
            }

            DataTable dt = this.bsprModelMapping.ActiveSheet.GetDataView(true).ToTable();
            
            if (bsprGroupList.GetSelectedCheckRows((int)GroupColumnIndex.Select).Count > 0)
            {
                int chkCNT = 0;
                string duplicateModel = string.Empty;

                for (int i = 0; i < this.bsprSPCModel.ActiveSheet.Rows.Count; i++)
                {
                    if (this.bsprSPCModel.ActiveSheet.Cells[i, (int)SPCModelColumnIndex.Select].Value != null &&
                        (this.bsprSPCModel.ActiveSheet.Cells[i, (int)SPCModelColumnIndex.Select].Value.ToString() == Definition.VARIABLE_TRUE))
                    {
                        DataRow[] dr = dt.Select(string.Format("RAWID = '{0}'", this.bsprSPCModel.ActiveSheet.Cells[i, (int)SPCModelColumnIndex.Rawid].Value));
                        if (dr.Length > 0)
                        {
                            chkCNT++;
                            duplicateModel += this.bsprSPCModel.ActiveSheet.Cells[i, (int)SPCModelColumnIndex.SPCModel].Value.ToString() + ", ";
                        }
                    }
                }

                if (chkCNT == bsprSPCModel.GetCheckedList((int)SPCModelColumnIndex.Select).Count)
                {
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_DUPLICATE_ALL_MODELS", null, null);
                }
                else if(chkCNT>0)
                {
                    //MSGHandler.DisplayMessage(MSGType.Information, string.Format("{0} is duplicated. Not duplicated Models are add the group mapping list. ",duplicateModel.Remove(duplicateModel.Length-2,2)));
                    MSGHandler.DisplayMessage(MSGType.Information, "SPC_INFO_DUPLICATE_MODEL_EXIST", null, null);
                }

                for (int i = 0; i < this.bsprSPCModel.ActiveSheet.Rows.Count; i++)
                {
                    if (this.bsprSPCModel.ActiveSheet.Cells[i, (int)SPCModelColumnIndex.Select].Value != null &&
                        (this.bsprSPCModel.ActiveSheet.Cells[i, (int)SPCModelColumnIndex.Select].Value.ToString() == Definition.VARIABLE_TRUE))
                    {
                        DataRow[] dr = dt.Select(string.Format("RAWID = '{0}'", this.bsprSPCModel.ActiveSheet.Cells[i, (int)SPCModelColumnIndex.Rawid].Value));
                        if (dr.Length == 0)
                        {
                            this.bsprModelMapping.ActiveSheet.AddRows(this.bsprModelMapping.ActiveSheet.Rows.Count, 1);
                            this.bsprModelMapping.ActiveSheet.Cells[this.bsprModelMapping.ActiveSheet.Rows.Count - 1, (int)ModelMappingColumnIndex.Insert].Value = Definition.VARIABLE_TRUE;
                            this.bsprModelMapping.ActiveSheet.Cells[this.bsprModelMapping.ActiveSheet.Rows.Count - 1, (int)ModelMappingColumnIndex.Delete].Locked = true;
                            //this.bsprModelMapping.ActiveSheet.Cells[this.bsprModelMapping.ActiveSheet.Rows.Count - 1, (int)ModelMappingColumnIndex.LocationRawid].Value = sLocationRawID;
                            //this.bsprModelMapping.ActiveSheet.Cells[this.bsprModelMapping.ActiveSheet.Rows.Count - 1, (int)ModelMappingColumnIndex.AreaRawid].Value = sAreaRawID;
                            this.bsprModelMapping.ActiveSheet.Cells[this.bsprModelMapping.ActiveSheet.Rows.Count - 1, (int)ModelMappingColumnIndex.Rawid].Value = this.bsprSPCModel.ActiveSheet.Cells[i, (int)SPCModelColumnIndex.Rawid].Value;
                            this.bsprModelMapping.ActiveSheet.Cells[this.bsprModelMapping.ActiveSheet.Rows.Count - 1, (int)ModelMappingColumnIndex.SPCModel].Value = this.bsprSPCModel.ActiveSheet.Cells[i, (int)SPCModelColumnIndex.SPCModel].Value;
                        }
                    }
                }
            }
        }

        

        private void btn_Left_Click(object sender, EventArgs e)
        {
            for (int i = this.bsprModelMapping.ActiveSheet.Rows.Count-1; i >= 0; i--)
            {
                if (this.bsprModelMapping.ActiveSheet.Cells[i, (int)ModelMappingColumnIndex.Select].Value != null &&
                    (this.bsprModelMapping.ActiveSheet.Cells[i, (int)ModelMappingColumnIndex.Select].Value.ToString() == Definition.VARIABLE_TRUE))
                {
                    this.bsprModelMapping.ActiveSheet.RemoveRows(i, 1);
                }
            }
        }

        private void SPCModelGroupPopup_Load(object sender, EventArgs e)
        {

            DynamicConditionFactory dcf = new DynamicConditionFactory();
            _dynamicEQP = dcf.GetDCOfPageByKey("BISTel.eSPC.Page.ATT.Modeling.SPCATTModelingUC");
            Control ctrlEQP = ((DCBar)_dynamicEQP).GetControlByContextID("ESPC_ATT_COMBO_TREE");
            this._btreeComboEQP = (BTreeCombo)ctrlEQP;
            this._btreeComboEQP.SelectionCommitted += new EventHandler(_btreeComboEQP_SelectionCommitted);
            this.bplEQPModel.Controls.Add(ctrlEQP);
            ctrlEQP.Dock = DockStyle.Fill;

            //SPC MODEL LEVEL을 가져옴
            LinkedList llstCondtion = new LinkedList();
            llstCondtion.Add(BISTel.eSPC.Common.Definition.CONDITION_KEY_CATEGORY, "SPC_ATT_MODEL_LEVEL");
            llstCondtion.Add(BISTel.eSPC.Common.Definition.CONDITION_KEY_USE_YN, "Y");

            DataSet dsModelLevel = _wsSPC.GetCodeData(llstCondtion.GetSerialData());
            if (DSUtil.CheckRowCount(dsModelLevel))
            {
                _sSPCModelLevel = dsModelLevel.Tables[0].Rows[0][BISTel.eSPC.Common.COLUMN.CODE].ToString();
            }
            

            EESProgressBar.ShowProgress(this, this._mlthandler.GetMessage(Definition.LOADING_DATA), true);

            this._dsGroupList = _wsSPC.GetATTSPCModelGroupList();

            EESProgressBar.CloseProgress(this);

            this._btreeComboEQP_SelectionCommitted(_btreeComboEQP, new EventArgs());
        }

        private void FindCheckedNode(TreeNodeCollection nodes)
        {
            int i = 0;
            int count = 0;

            count = nodes.Count;

            for (i = 0; i < count; i++)
            {
                if (nodes[i].Checked.Equals(true))
                {
                    this._tnCheckedComboNode = nodes[i];
                }
                else
                {
                    FindCheckedNode(nodes[i].Nodes);
                }
            }
        }

        private void _btreeComboEQP_SelectionCommitted(object sender, EventArgs e)
        {
            TreeNodeCollection tnc = this._btreeComboEQP.TreeView.Nodes;
            this.FindCheckedNode(tnc);

            DCValueOfTree dvtModel = TreeDCUtil.GetDCValue(this._tnCheckedComboNode);
            List<DCValueOfTree> lstSelectedValue = TreeDCUtil.GetDCValueOfAllParent(this._tnCheckedComboNode);

            if (this._tnCheckedComboNode != null && dvtModel != null)
            {
                if (_sSPCModelLevel.Equals(BISTel.eSPC.Common.Definition.CONDITION_KEY_EQP_MODEL))
                {
                    sEQPModel = dvtModel.Value;

                    if (this._tnCheckedComboNode.Checked)
                        sLevelValue = sEQPModel;
                    else
                        sLevelValue = string.Empty;

                    if (lstSelectedValue != null)
                    {
                        string contextID = "";

                        for (int i = 0; i < lstSelectedValue.Count; i++)
                        {
                            contextID = lstSelectedValue[i].ContextId;

                            switch (contextID)
                            {
                                case "ESPC_AREA":
                                    sAreaRawID = lstSelectedValue[i].Value;
                                    break;
                                case "ESPC_LINE":
                                    sLocationRawID = lstSelectedValue[i].Value;
                                    break;
                                default: break;
                            }
                        }
                    }
                }
                else
                {
                    sAreaRawID = dvtModel.Value;

                    if (this._tnCheckedComboNode.Checked)
                        sLevelValue = dvtModel.Disp;
                    else
                        sLevelValue = string.Empty;

                    if (lstSelectedValue != null)
                    {
                        string contextID = "";

                        for (int i = 0; i < lstSelectedValue.Count; i++)
                        {
                            contextID = lstSelectedValue[i].ContextId;

                            switch (contextID)
                            {
                                case "ESPC_LINE":
                                    sLocationRawID = lstSelectedValue[i].Value;
                                    break;
                                default: break;
                            }
                        }
                    }
                }

                this._dsGroupList = _wsSPC.GetATTSPCModelGroupList();
                SetData();
            }

        }

        private void bsprGroupList_EditModeOff(object sender, EventArgs e)
        {
            DataTable dt = ((DataSet)this.bsprGroupList.DataSet).Tables[0];

            for (int i = 0; i < _dtGroup.Rows.Count; i++)
            {
                if (dt.Rows[i][BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString() == _dtGroup.Rows[i][BISTel.eSPC.Common.COLUMN.GROUP_NAME].ToString())
                {
                    this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.Modify].Value = Definition.VARIABLE_FALSE;
                }
                else
                {
                    this.bsprGroupList.ActiveSheet.Cells[i, (int)GroupColumnIndex.Modify].Value = Definition.VARIABLE_TRUE;
                }
            }
        }

    }
}
