using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Condition.eSPCWebService;
using System.Collections;
using BISTel.eSPC.Condition.Controls.Interface;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Condition.Controls.Default
{
    public partial class DefaultCondition : ControlInterface
    {
        #region : Field

        private eSPCWebService.eSPCWebService _fdcWebService = null;

        public DefaultCondition _bindControl = null;
        public LinkedList controlList = null;

        public DefaultInfo _defaultInfo = new DefaultInfo();

        private string _sUseSpecModelGroup = "";
        private string _sUseDefaultCondition = "";

        private int nHideControl = 0;
        private int _nHeight = 0;

        public int DefaultHeight
        {
            get
            {
                return _nHeight;
            }
            set
            {
                _nHeight = value;
            }
        }

        public LinkedList _llstCondition = new LinkedList();

        #endregion

        #region : Initialization

        public DefaultCondition()
        {
            InitializeComponent();
            _nHeight = 30;
        }

        public void SetParent(object parent)
        {
            _parent = (SearchConditionInterface)parent;
            this._fdcWebService = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            controlList = new LinkedList();
        }

        public void InitializeModelGroup()
        {
            DataSet dsCodeData = new DataSet();

            LinkedList lnkCondition = new LinkedList();
            lnkCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_USE_SPEC_GROUP);

            byte[] baDataUseSpecGroup = lnkCondition.GetSerialData();

            dsCodeData = this._fdcWebService.GetCodeData(baDataUseSpecGroup);

            if (dsCodeData.Tables.Count > 0)
            {
                this._sUseSpecModelGroup = dsCodeData.Tables[0].Rows[0]["CODE"].ToString();
            }
        }

        public void InitializeDefaultCondition()
        {
            DataSet dsCodeData = new DataSet();

            LinkedList lnkCondition = new LinkedList();
            lnkCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_USE_DEFAULT_CONDITION);

            byte[] baDataUseDefault = lnkCondition.GetSerialData();

            dsCodeData = this._fdcWebService.GetCodeData(baDataUseDefault);

            if (dsCodeData.Tables.Count > 0)
            {
                this._sUseDefaultCondition = dsCodeData.Tables[0].Rows[0]["CODE"].ToString();
            }
        }

        public void InitializeLine()
        {

            DataSet ds = null;
            _defaultInfo._htLineRawid = new Hashtable();

            LinkedList llstData = new LinkedList();
            llstData.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CONDITION_KEY_SITE);
            DataSet dsCodeData = _fdcWebService.GetCodeData(llstData.GetSerialData());
            if (dsCodeData != null && dsCodeData.Tables.Count > 0 && dsCodeData.Tables[0].Rows.Count > 0)
            {
                string sSite = dsCodeData.Tables[0].Rows[0][Definition.VARIABLE_NAME].ToString();
                llstData.Clear();
                llstData.Add(Definition.CONDITION_KEY_SITE, sSite);
            }

            ds = _fdcWebService.GetLine(llstData.GetSerialData());

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    //LinkedList llstLine = new LinkedList();
                    //string[] list = new string[ds.Tables[0].Rows.Count];
                    //for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    //{
                    //    string rawid = Convert.ToString(ds.Tables[0].Rows[row][Definition.CONDITION_KEY_RAWID]);
                    //    string line = Convert.ToString(ds.Tables[0].Rows[row][Definition.CONDITION_KEY_LINE]);
                    //    list[row] = line;
                    //    _defaultInfo._htLineRawid.Add(line, rawid);
                    //    llstLine.Add(rawid, line);
                    //}
                    //((ComboInterface)controlList[ConditionType.LINE]).SetComboValue(list);
                    //((ComboInterface)controlList[ConditionType.LINE]).SetSelectedIndex(0);

                    DataTable dtLine = ds.Tables[0].Copy();
                    dtLine.Columns[Definition.CONDITION_KEY_RAWID].ColumnName = Definition.DynamicCondition_Search_key.VALUEDATA;
                    dtLine.Columns[Definition.CONDITION_KEY_LINE].ColumnName = Definition.DynamicCondition_Search_key.DISPLAYDATA;
                    ((ComboInterface)controlList[ConditionType.LINE]).ComboValueTable = dtLine;

                    ((ComboInterface)controlList[ConditionType.LINE]).SetComboValue(null);
                    ((ComboInterface)controlList[ConditionType.LINE]).SetSelectedIndex(0);
                }
            }
        }

        #endregion

        #region : Public

        public void AddControl(ConditionType type, string title, bool multi)
        {
            ComboInterface control = null;

            if (multi)
            {
                control = new CheckComboControl(this);
                ((CheckComboControl)control).TitleText = title;
            }
            else
            {
                control = new ComboControl(this);
                ((ComboControl)control).TitleText = title;
            }

            control._type = type;
            control._title = title;

            controlList.Add(type, control);
            _nHeight += 30;
        }

        public void AddControlAndValue(ConditionType type, string title, bool multi, string[] value)
        {
            ComboInterface control = null;

            if (multi)
            {
                control = new CheckComboControl(this);
                ((CheckComboControl)control).TitleText = title;
            }
            else
            {
                control = new ComboControl(this);
                ((ComboControl)control).TitleText = title;
            }

            control._type = type;
            control._title = title;
            control.SetComboValue(value);

            controlList.Add(type, control);
            _nHeight += 30;
        }

        public void AttachControl()
        {
            this.InitializeModelGroup();
            this.InitializeDefaultCondition();

            for (int i = 0; i < controlList.Count; i++)
            {
                ComboInterface control = (ComboInterface)controlList.GetValue(i);
                pnlBody.Controls.Add(control);
                control.BringToFront();
                control.Dock = DockStyle.Top;
                control.TabIndex = i;

                if (control._type == ConditionType.SPECMODELID && !this._sUseSpecModelGroup.Equals(Definition.VARIABLE_Y))
                {
                    nHideControl++;
                    control.Visible = false;
                }

                if (control._type == ConditionType.DCPID)
                {
                    nHideControl++;
                    control.Visible = false;
                }

                if (!this._sUseDefaultCondition.Equals(Definition.VARIABLE_Y))
                {
                    if (control._type == ConditionType.PRODUCTID || control._type == ConditionType.EQPMODEL)
                    {
                        nHideControl++;
                        control.Visible = false;
                    }
                }
            }

            //Line을 불려온다.
            InitializeLine();
            //this.SetFavoriteCondition();

            if (nHideControl > 0)
            {
                _nHeight -= (nHideControl * 30);
            }
        }

        public void ClearControls()
        {
            for (int i = 0; i < controlList.Count; i++)
            {
                this.pnlBody.Controls.Remove((ComboInterface)controlList.GetValue(i));
            }
        }

        public void ChangeMultiControl(ConditionType key, bool multi)
        {
            if (!controlList.Contains(key))
                return;

            Type csType = controlList[key].GetType();

            ComboInterface control = null;
            if (multi)
            {
                if (csType == typeof(ComboControl))
                    control = new CheckComboControl(this);
                else
                    return;
            }
            else if (!multi)
            {
                if (csType == typeof(CheckComboControl))
                    control = new ComboControl(this);
                else
                    return;
            }

            control._title = ((ComboInterface)controlList[key])._title;
            control.SetTitleText(control._title);
            control._type = key;
            (controlList[key]) = control;

            SetChildControlFromControls(key, control);

        }

        //public void SetFavoriteCondition()
        //{
        //    LinkedList llstData = new LinkedList();

        //    try
        //    {
        //        string sUserName = GlobalDefinition.SessionData.UserName;
        //        string sUserRawID = GlobalDefinition.SessionData.UserRawID;
        //        llstData.Add(Definition.CONDITION_KEY_USER_RAWID, GlobalDefinition.SessionData.UserRawID);
        //        byte[] btdata = llstData.GetSerialData();


        //        DataSet dsData = this._fdcWebService.GetFavoriteCondition(btdata);

        //        string sLine = "";
        //        string sArea = "";
        //        string sEQPModel = "";
        //        string sEQPID = "";
        //        string sModuleID = "";

        //        if (dsData.Tables.Count > 0)
        //        {
        //            for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
        //            {
        //                string sConditionCD = dsData.Tables[0].Rows[i]["CONDITION_CD"].ToString();
        //                string sConditionValue = dsData.Tables[0].Rows[i]["CONDITION_VALUE"].ToString();

        //                if (sConditionCD.Equals(Definition.FCONDITION_CD_LINE))
        //                {
        //                    sLine = sConditionValue;
        //                }
        //                else if (sConditionCD.Equals(Definition.FCONDITION_CD_AREA))
        //                {
        //                    sArea = sConditionValue;
        //                }
        //                else if (sConditionCD.Equals(Definition.FCONDITION_CD_EQP_MODEL))
        //                {
        //                    sEQPModel = sConditionValue;
        //                }
        //                else if (sConditionCD.Equals(Definition.FCONDITION_CD_EQP_ID))
        //                {
        //                    sEQPID = sConditionValue;
        //                }
        //                else if (sConditionCD.Equals(Definition.FCONDITION_CD_MODULE_ID))
        //                {
        //                    sModuleID = sConditionValue;
        //                }
        //            }
        //        }

        //        if (sLine.Length > 0)
        //        {
        //            ((ComboInterface)controlList[ConditionType.LINE]).SetComboText(sLine);
        //        }

        //        if (sArea.Length > 0)
        //        {
        //            this.SetDropDownCombo(ConditionType.AREA, null);
        //            ((ComboInterface)controlList[ConditionType.AREA]).SetComboText(sArea);
        //        }

        //        if (sEQPModel.Length > 0)
        //        {
        //            this.SetDropDownCombo(ConditionType.EQPMODEL, null);
        //            ((ComboInterface)controlList[ConditionType.EQPMODEL]).SetComboText(sEQPModel);
        //        }

        //        if (sEQPID.Length > 0)
        //        {
        //            this.SetDropDownCombo(ConditionType.EQPID, null);
        //            ((ComboInterface)controlList[ConditionType.EQPID]).SetComboText(sEQPID);
        //        }

        //        if (sModuleID.Length > 0)
        //        {
        //            this.SetDropDownCombo(ConditionType.MODULEID, null);
        //            ((ComboInterface)controlList[ConditionType.MODULEID]).SetComboText(sModuleID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        #endregion

        #region : Private

        public ComboInterface GetChildControlFromControls(ConditionType type)
        {
            ComboInterface control = null;
            int count = this.pnlBody.Controls.Count;
            for (int i = 0; i < count; i++)
            {
                control = (ComboInterface)pnlBody.Controls[i];
                if (control._type == type)
                    return (ComboInterface)pnlBody.Controls[i];

            }
            return null;
        }

        private void SetChildControlFromControls(ConditionType type, ComboInterface control)
        {
            int count = this.pnlBody.Controls.Count;
            for (int i = 0; i < count; i++)
            {
                if (((ComboInterface)pnlBody.Controls[i])._type == type)
                {
                    pnlBody.Controls.RemoveAt(i);
                    control.BringToFront();
                    control.Dock = DockStyle.Top;
                    pnlBody.Controls.Add(control);
                    pnlBody.Controls.SetChildIndex(control, i);
                    return;
                }
            }
        }

        private void ResetComboValue(ConditionType type)
        {
            ComboInterface control = GetChildControlFromControls(type);

            if (control == null) return;

            ((ComboInterface)control).ResetValue();
        }



        private void ResetComboList(ConditionType type)
        {
            _defaultInfo.Reset(type);
            switch (type)
            {
                case ConditionType.MULTITYPE:
                    {
                        ResetComboValue(ConditionType.AREA);
                        ResetComboValue(ConditionType.EQPMODEL);
                        ResetComboValue(ConditionType.EQPID);
                        ResetComboValue(ConditionType.DCPID);
                        ResetComboValue(ConditionType.MODULEID);
                        ResetComboValue(ConditionType.PRODUCTID);
                        ResetComboValue(ConditionType.RECIPEID);
                        //InitializeLine();
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.AREA);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.EQPMODEL);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.EQP_ID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.DCP_ID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.MODULE);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.PRODUCT);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.RECIPE);
                        //CONDITION_SEARCH_KEY_RECIPE
                    }
                    break;
                case ConditionType.LINE:
                    {
                        ResetComboValue(ConditionType.AREA);
                        ResetComboValue(ConditionType.EQPMODEL);
                        ResetComboValue(ConditionType.EQPID);
                        ResetComboValue(ConditionType.DCPID);
                        ResetComboValue(ConditionType.MODULEID);
                        ResetComboValue(ConditionType.PRODUCTID);
                        ResetComboValue(ConditionType.RECIPEID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.AREA);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.EQPMODEL);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.EQP_ID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.DCP_ID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.MODULE);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.PRODUCT);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.RECIPE);
                    }
                    break;
                case ConditionType.AREA:
                    {
                        ResetComboValue(ConditionType.EQPMODEL);
                        ResetComboValue(ConditionType.EQPID);
                        ResetComboValue(ConditionType.DCPID);
                        ResetComboValue(ConditionType.MODULEID);
                        ResetComboValue(ConditionType.PRODUCTID);
                        ResetComboValue(ConditionType.RECIPEID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.EQPMODEL);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.EQP_ID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.DCP_ID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.MODULE);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.PRODUCT);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.RECIPE);
                    }
                    break;
                case ConditionType.EQPMODEL:
                    {
                        ResetComboValue(ConditionType.EQPID);
                        ResetComboValue(ConditionType.DCPID);
                        ResetComboValue(ConditionType.MODULEID);
                        ResetComboValue(ConditionType.PRODUCTID);
                        ResetComboValue(ConditionType.RECIPEID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.EQP_ID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.DCP_ID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.MODULE);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.PRODUCT);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.RECIPE);
                    }
                    break;
                case ConditionType.EQPID:
                    {
                        ResetComboValue(ConditionType.DCPID);
                        ResetComboValue(ConditionType.MODULEID);
                        ResetComboValue(ConditionType.PRODUCTID);
                        ResetComboValue(ConditionType.RECIPEID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.DCP_ID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.MODULE);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.PRODUCT);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.RECIPE);
                    }
                    break;
                case ConditionType.DCPID:
                    {
                        ResetComboValue(ConditionType.MODULEID);
                        ResetComboValue(ConditionType.PRODUCTID);
                        ResetComboValue(ConditionType.RECIPEID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.MODULE);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.PRODUCT);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.RECIPE);
                    }
                    break;
                case ConditionType.MODULEID:
                    {
                        ResetComboValue(ConditionType.PRODUCTID);
                        ResetComboValue(ConditionType.RECIPEID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.PRODUCT);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.RECIPE);
                    }
                    break;
                case ConditionType.PRODUCTID:
                    {
                        ResetComboValue(ConditionType.RECIPEID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.RECIPE);
                    }
                    break;
                case ConditionType.RECIPEID:
                    {
                        ResetComboValue(ConditionType.SPECMODELID);
                        _llstCondition.Remove(Definition.DynamicCondition_Search_key.SPEC_GROUP);
                    }
                    break;
                case ConditionType.SPECMODELID:
                    {

                    }
                    break;
            }
        }

        #endregion

        #region : Event Handling

        public void SetDropDownCombo(ConditionType type, object child)
        {
            switch (type)
            {
                case ConditionType.LINE:
                    {
                    }
                    break;
                case ConditionType.AREA:
                    {
                        DataTable dt = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.LINE];
                        string rawid = dt.Rows[0][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                        LinkedList line = new LinkedList();
                        line.Add(Definition.CONDITION_KEY_LINE_RAWID, rawid);
                        byte[] btdata = line.GetSerialData();
                        DataSet ds = _fdcWebService.GetArea(btdata);
                        //_defaultInfo._htAreaRawid = new Hashtable();
                        //LinkedList line = new LinkedList();
                        //line.Add(Definition.CONDITION_KEY_LINE_RAWID, _defaultInfo._htLineRawid[_defaultInfo._sLine]);
                        //byte[] btdata =line.GetSerialData();
                        //DataSet ds = _fdcWebService.GetArea(btdata);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                //string[] list = new string[ds.Tables[0].Rows.Count];
                                //for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                //{
                                //    string rawid = Convert.ToString(ds.Tables[0].Rows[row][Definition.CONDITION_KEY_RAWID]);
                                //    string area = Convert.ToString(ds.Tables[0].Rows[row][Definition.CONDITION_KEY_AREA]);
                                //    list[row] = area;
                                //    _defaultInfo._htAreaRawid.Add(area, rawid);
                                //}

                                // add
                                DataTable dtArea = ds.Tables[0].Copy();
                                dtArea.Columns[Definition.CONDITION_KEY_LINE_RAWID].ColumnName = Definition.DynamicCondition_Search_key.LINE;
                                dtArea.Columns[Definition.CONDITION_KEY_RAWID].ColumnName = Definition.DynamicCondition_Search_key.VALUEDATA;
                                dtArea.Columns[Definition.CONDITION_KEY_AREA].ColumnName = Definition.DynamicCondition_Search_key.DISPLAYDATA;
                                ((ComboInterface)controlList[type]).ComboValueTable = dtArea;

                                //((ComboInterface)controlList[type]).SetComboValue(list);
                                ((ComboInterface)controlList[type]).SetComboValue(null);
                            }
                        }
                    }
                    break;
                case ConditionType.EQPMODEL:
                    {
                        string lineRawid = string.Empty;
                        string areaRawid = string.Empty;
                        LinkedList modelList = new LinkedList();

                        DataTable dtLine = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.LINE];
                        if (dtLine != null)
                            lineRawid = dtLine.Rows[0][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                        DataTable dtArea = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.AREA];
                        if (dtArea != null)
                            areaRawid = dtArea.Rows[0][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();

                        modelList.Add(Definition.CONDITION_KEY_LINE_RAWID, lineRawid);
                        modelList.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawid);
                        byte[] btdata = modelList.GetSerialData();
                        DataSet ds = _fdcWebService.GetEqpModel(btdata);

                        //string[] list = DataSetToArray(ds, Definition.CONDITION_KEY_EQP_MODEL);
                        //modelList.Add(Definition.CONDITION_KEY_LINE_RAWID, _defaultInfo._htLineRawid[_defaultInfo._sLine]);
                        //modelList.Add(Definition.CONDITION_KEY_AREA_RAWID, _defaultInfo._htAreaRawid[_defaultInfo._sArea]);
                        //byte[] btdata = modelList.GetSerialData();
                        //DataSet ds = _fdcWebService.GetEqpModel(btdata);
                        //string[] list = DataSetToArray(ds, Definition.CONDITION_KEY_EQP_MODEL);

                        // add
                        DataTable dtEqpModel = ds.Tables[0].Copy();
                        dtEqpModel.Columns[Definition.CONDITION_KEY_LINE_RAWID].ColumnName = Definition.DynamicCondition_Search_key.LINE;
                        dtEqpModel.Columns[Definition.CONDITION_KEY_AREA_RAWID].ColumnName = Definition.DynamicCondition_Search_key.AREA;
                        dtEqpModel.Columns[Definition.CONDITION_KEY_EQP_MODEL].ColumnName = Definition.DynamicCondition_Search_key.VALUEDATA;
                        ((ComboInterface)controlList[type]).ComboValueTable = dtEqpModel;

                        //((ComboInterface)controlList[type]).SetComboValue(list);
                        ((ComboInterface)controlList[type]).SetComboValue(null);
                    }
                    break;
                case ConditionType.EQPID:
                    {
                        string lineRawid = string.Empty;
                        string areaRawid = string.Empty;
                        string eqpModel = string.Empty;
                        LinkedList list = new LinkedList();

                        DataTable dtLine = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.LINE];
                        if (dtLine != null)
                            lineRawid = dtLine.Rows[0][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                        DataTable dtArea = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.AREA];
                        if (dtArea != null)
                            areaRawid = dtArea.Rows[0][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                        DataTable dtEqpModel = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.EQPMODEL];
                        if (dtEqpModel != null)
                            eqpModel = dtEqpModel.Rows[0][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();

                        list.Add(Definition.CONDITION_KEY_LINE_RAWID, lineRawid);
                        list.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawid);
                        list.Add(Definition.CONDITION_KEY_EQP_MODEL, eqpModel);
                        byte[] btdata = list.GetSerialData();
                        DataSet ds = _fdcWebService.GetEQP(btdata);


                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {

                                // add
                                DataTable dtEqp = ds.Tables[0].Copy();
                                dtEqp.Columns[Definition.CONDITION_KEY_LINE_RAWID].ColumnName = Definition.DynamicCondition_Search_key.LINE;
                                dtEqp.Columns[Definition.CONDITION_KEY_AREA_RAWID].ColumnName = Definition.DynamicCondition_Search_key.AREA;
                                dtEqp.Columns[Definition.CONDITION_KEY_EQP_MODEL].ColumnName = Definition.DynamicCondition_Search_key.EQPMODEL;
                                dtEqp.Columns[Definition.CONDITION_KEY_RAWID].ColumnName = Definition.DynamicCondition_Search_key.VALUEDATA;
                                dtEqp.Columns[Definition.CONDITION_KEY_EQP_ID].ColumnName = Definition.DynamicCondition_Search_key.DISPLAYDATA;
                                ((ComboInterface)controlList[type]).ComboValueTable = dtEqp;

                                //((ComboInterface)controlList[type]).SetComboValue(list);
                                ((ComboInterface)controlList[type]).SetComboValue(null);
                            }
                        }
                    }
                    break;
                case ConditionType.DCPID:
                    {
                        //if (_defaultInfo._sEQPID == string.Empty && _defaultInfo._alEqpIDList == null) return;

                        //_defaultInfo._htDCPRawid = new Hashtable();
                        //LinkedList cmbrList = new LinkedList();
                        //if (_defaultInfo._alEqpIDList == null)
                        //{
                        //    string eqpRawID = (string)_defaultInfo._htEQPRawid[_defaultInfo._sEQPID];
                        //    cmbrList.Add(Definition.CONDITION_KEY_EQP_RAWID, eqpRawID);
                        //}
                        //else
                        //{
                        //    ArrayList alEqpRawid = new ArrayList();
                        //    for (int i = 0; i < _defaultInfo._alEqpIDList.Count; i++)
                        //        alEqpRawid.Add((string)_defaultInfo._htEQPRawid[_defaultInfo._alEqpIDList[i].ToString()]);
                        //    cmbrList.Add(Definition.CONDITION_KEY_EQP_RAWID_LIST, alEqpRawid);
                        //}
                        //byte[] btdata = cmbrList.GetSerialData();
                        //DataSet ds = _fdcWebService.GetDCP(btdata);

                        //if (ds != null)
                        //{
                        //    if (ds.Tables.Count > 0)
                        //    {
                        //        ArrayList alList = new ArrayList();
                        //        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        //        {
                        //            string rawid = Convert.ToString(ds.Tables[0].Rows[row][Definition.CONDITION_KEY_RAWID]);
                        //            string module = Convert.ToString(ds.Tables[0].Rows[row][Definition.CONDITION_KEY_DCP_NAME]);

                        //            if (!_defaultInfo._htDCPRawid.Contains(module))
                        //            {
                        //                alList.Add(module);
                        //                _defaultInfo._htDCPRawid.Add(module, rawid);
                        //            }
                        //        }
                        //        if (((ComboInterface)controlList[type]) != null)
                        //        {
                        //            ((ComboInterface)controlList[type]).SetComboValue(alList.ToArray());
                        //        }
                        //    }
                        //}                        
                    }
                    break;
                case ConditionType.MODULEID:
                    {
                        GetModuleList();
                    }
                    break;
                case ConditionType.PRODUCTID:
                    {
                        LinkedList list = new LinkedList();
                        DataTable dtModule = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.MODULE];
                        if (dtModule != null && dtModule.Rows.Count > 0)
                        {
                            ArrayList alRawid = new ArrayList();
                            ArrayList alID = new ArrayList();
                            for (int i = 0; i < dtModule.Rows.Count; i++)
                            {
                                string rawid = dtModule.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                                string module = dtModule.Rows[i][Definition.DynamicCondition_Search_key.DISPLAYDATA].ToString();
                                alRawid.Add(rawid);
                                alID.Add(module);
                            }
                            list.Add(Definition.CONDITION_KEY_MODULE_RAWID_LIST, alRawid);
                            list.Add(Definition.CONDITION_KEY_MODULE_ID_LIST, alID);
                        }
                        byte[] btdata = list.GetSerialData();
                        DataSet ds = _fdcWebService.GetProduct(btdata);



                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {

                                // add
                                DataTable dtProduct = ds.Tables[0].Copy();
                                dtProduct.Columns[Definition.CONDITION_KEY_EQP_RAWID].ColumnName = Definition.DynamicCondition_Search_key.MODULE;
                                dtProduct.Columns[Definition.CONDITION_KEY_RAWID].ColumnName = Definition.DynamicCondition_Search_key.VALUEDATA;
                                dtProduct.Columns[Definition.CONDITION_KEY_PRODUCT_ID].ColumnName = Definition.DynamicCondition_Search_key.DISPLAYDATA;
                                ((ComboInterface)controlList[type]).ComboValueTable = dtProduct;

                                //((ComboInterface)controlList[type]).SetComboValue(list);
                                ((ComboInterface)controlList[type]).SetComboValue(null);
                            }
                        }
                    }
                    break;
                case ConditionType.RECIPEID:
                    {
                        GetRecipeList();
                    }
                    break;
                case ConditionType.SPECMODELID:
                    {
                    }
                    break;
                case ConditionType.TYPE:
                    {

                    }
                    break;
            }
        }

        public void EventSelectValue(DataTable str, ConditionType type)
        {
            SelectChangedEvent(str, type);
            _parent.SendEventToParent(str, type);
        }

        public void SelectChangedEvent(DataTable value, ConditionType type)
        {
            ResetComboList(type);
            switch (type)
            {
                case ConditionType.LINE:
                    //_defaultInfo._sLine = value[0].ToString();
                    if (_llstCondition.Contains(Definition.DynamicCondition_Search_key.LINE))
                        _llstCondition[Definition.DynamicCondition_Search_key.LINE] = value;
                    else
                        _llstCondition.Add(Definition.DynamicCondition_Search_key.LINE, value);
                    break;
                case ConditionType.AREA:
                    //_defaultInfo._sArea = value[0].ToString();
                    if (_llstCondition.Contains(Definition.DynamicCondition_Search_key.AREA))
                        _llstCondition[Definition.DynamicCondition_Search_key.AREA] = value;
                    else
                        _llstCondition.Add(Definition.DynamicCondition_Search_key.AREA, value);
                    break;
                case ConditionType.EQPMODEL:
                    //_defaultInfo._sEQPModel = value[0].ToString();
                    if (_llstCondition.Contains(Definition.DynamicCondition_Search_key.EQPMODEL))
                        _llstCondition[Definition.DynamicCondition_Search_key.EQPMODEL] = value;
                    else
                        _llstCondition.Add(Definition.DynamicCondition_Search_key.EQPMODEL, value);
                    break;
                case ConditionType.EQPID:
                    {


                        if (_llstCondition.Contains(Definition.DynamicCondition_Search_key.EQP_ID))
                            _llstCondition[Definition.DynamicCondition_Search_key.EQP_ID] = value;
                        else
                            _llstCondition.Add(Definition.DynamicCondition_Search_key.EQP_ID, value);
                    }
                    break;
                case ConditionType.DCPID:
                    //_defaultInfo._sDCPID = value[0].ToString();
                    break;
                case ConditionType.MODULEID:

                    if (_llstCondition.Contains(Definition.DynamicCondition_Search_key.MODULE))
                        _llstCondition[Definition.DynamicCondition_Search_key.MODULE] = value;
                    else
                        _llstCondition.Add(Definition.DynamicCondition_Search_key.MODULE, value);
                    break;
                case ConditionType.PRODUCTID:
                    //_defaultInfo._sProductID = value[0].ToString();
                    if (_llstCondition.Contains(Definition.DynamicCondition_Search_key.PRODUCT))
                        _llstCondition[Definition.DynamicCondition_Search_key.PRODUCT] = value;
                    else
                        _llstCondition.Add(Definition.DynamicCondition_Search_key.PRODUCT, value);
                    break;
                case ConditionType.RECIPEID:
                    {
                        if (_llstCondition.Contains(Definition.DynamicCondition_Search_key.RECIPE))
                            _llstCondition[Definition.DynamicCondition_Search_key.RECIPE] = value;
                        else
                            _llstCondition.Add(Definition.DynamicCondition_Search_key.RECIPE, value);
                    }
                    break;
                case ConditionType.TYPE:
                    {
                    }
                    break;
                case ConditionType.LEGEND:
                    //_defaultInfo._sLegend = value;
                    if (_llstCondition.Contains("LEGEND"))
                        _llstCondition["LEGEND"] = value;
                    else
                        _llstCondition.Add("LEGEND", value);
                    break;
                case ConditionType.MULTITYPE:
                    //_defaultInfo._sType = value[0].ToString();
                    if (_llstCondition.Contains(Definition.DynamicCondition_Search_key.TYPE))
                        _llstCondition[Definition.DynamicCondition_Search_key.TYPE] = value;
                    else
                        _llstCondition.Add(Definition.DynamicCondition_Search_key.TYPE, value);
                    break;
                case ConditionType.SPECMODELID:
                    //_defaultInfo._sSpecModelGroupID = value[0].ToString();
                    if (_llstCondition.Contains(Definition.DynamicCondition_Search_key.SPEC_GROUP))
                        _llstCondition[Definition.DynamicCondition_Search_key.SPEC_GROUP] = value;
                    else
                        _llstCondition.Add(Definition.DynamicCondition_Search_key.SPEC_GROUP, value);
                    break;
            }
        }

        public void SetComboValue(ConditionType type, string[] list)
        {
            ((ComboInterface)GetChildControlFromControls(type)).SetComboValue(list);
            ((ComboInterface)controlList[type]).SetComboValue(list);
        }

        public void EnableCombo(ConditionType type, bool enable)
        {
            ComboInterface control = GetChildControlFromControls(type);

            if (control == null) return;

            ((ComboInterface)control).EnableCombo(enable);
        }

        public void SetEnableStatus(bool enable)
        {
            EnableCombo(ConditionType.LINE, enable);
            EnableCombo(ConditionType.AREA, enable);
            EnableCombo(ConditionType.EQPMODEL, enable);
            EnableCombo(ConditionType.EQPID, enable);
            EnableCombo(ConditionType.DCPID, enable);
            EnableCombo(ConditionType.MODULEID, enable);
            EnableCombo(ConditionType.PRODUCTID, enable);
            EnableCombo(ConditionType.RECIPEID, enable);
            EnableCombo(ConditionType.LEGEND, enable);
            EnableCombo(ConditionType.SPECMODELID, enable);
        }

        #endregion

        #region : Load Data

        private void GetModuleList()
        {
            string lineRawid = string.Empty;
            string areaRawid = string.Empty;
            string eqpModel = string.Empty;
            LinkedList list = new LinkedList();

            DataTable dtLine = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.LINE];
            if (dtLine != null)
                lineRawid = dtLine.Rows[0][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();

            DataTable dtArea = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.AREA];
            if (dtArea != null)
                areaRawid = dtArea.Rows[0][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();

            DataTable dtEqpModel = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.EQPMODEL];
            if (dtEqpModel != null)
                eqpModel = dtEqpModel.Rows[0][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();


            list.Add(Definition.CONDITION_KEY_LINE_RAWID, lineRawid);
            list.Add(Definition.CONDITION_KEY_AREA_RAWID, areaRawid);
            list.Add(Definition.CONDITION_KEY_EQP_MODEL, eqpModel);

            DataTable dtEqpID = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.EQP_ID];
            if (dtEqpID != null && dtEqpID.Rows.Count > 0)
            {
                ArrayList alModuleRawid = new ArrayList();
                ArrayList alModuleID = new ArrayList();
                for (int i = 0; i < dtEqpID.Rows.Count; i++)
                {
                    string rawid = dtEqpID.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    string module = dtEqpID.Rows[i][Definition.DynamicCondition_Search_key.DISPLAYDATA].ToString();
                    alModuleRawid.Add(rawid);
                    alModuleID.Add(module);
                }
                list.Add(Definition.CONDITION_KEY_EQP_RAWID_LIST, alModuleRawid);
                list.Add(Definition.CONDITION_KEY_EQP_ID_LIST, alModuleID);
            }
            byte[] btdata = list.GetSerialData();
            DataSet ds = _fdcWebService.GetModule(btdata);

            //if (_defaultInfo._sEQPID == string.Empty && _defaultInfo._alEqpIDList == null)
            //    return;

            //_defaultInfo._htModuleRawid = new Hashtable();
            //LinkedList cmbrList = new LinkedList();
            //cmbrList.Add(Definition.CONDITION_KEY_LINE_RAWID, _defaultInfo._htLineRawid[_defaultInfo._sLine]);
            //cmbrList.Add(Definition.CONDITION_KEY_AREA_RAWID, _defaultInfo._htAreaRawid[_defaultInfo._sArea]);
            //cmbrList.Add(Definition.CONDITION_KEY_EQP_MODEL, _defaultInfo._sEQPModel);
            //if (_defaultInfo._alEqpIDList == null)
            //{
            //    string eqpRawID = (string)_defaultInfo._htEQPRawid[_defaultInfo._sEQPID];
            //    cmbrList.Add(Definition.CONDITION_KEY_EQP_RAWID, eqpRawID);
            //    cmbrList.Add(Definition.CONDITION_KEY_EQP_ID, _defaultInfo._sEQPID);
            //}
            //else
            //{
            //    ArrayList alEqpRawid = new ArrayList();
            //    ArrayList alEqpID = new ArrayList();
            //    for (int i = 0; i < _defaultInfo._alEqpIDList.Count; i++)
            //    {
            //        string sEqpRawid = (string)_defaultInfo._htEQPRawid[_defaultInfo._alEqpIDList[i].ToString()];
            //        string sEqpID = _defaultInfo._alEqpIDList[i].ToString();
            //        if (!alEqpRawid.Contains(sEqpRawid))
            //        {
            //            alEqpRawid.Add(sEqpRawid);
            //            alEqpID.Add(sEqpID);
            //        }
            //    }
            //    cmbrList.Add(Definition.CONDITION_KEY_EQP_RAWID_LIST, alEqpRawid);
            //    cmbrList.Add(Definition.CONDITION_KEY_EQP_ID_LIST, alEqpID);
            //}
            ////if (_defaultInfo._htDCPRawid[_defaultInfo._sDCPID] == null)
            ////    return;

            ////cmbrList.Add(Definition.CONDITION_KEY_DCP_RAWID, _defaultInfo._htDCPRawid[_defaultInfo._sDCPID]);
            //byte[] btdata = cmbrList.GetSerialData();
            //DataSet ds = _fdcWebService.GetModule(btdata);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    //ArrayList alList = new ArrayList();

                    //if (_defaultInfo._alEqpIDList != null && _defaultInfo._alEqpIDList.Count > 0)
                    //{
                    //    for (int i = 0; i < _defaultInfo._alEqpIDList.Count; i++)
                    //    {
                    //        string eqpid = _defaultInfo._alEqpIDList[i].ToString();
                    //        string rawid = _defaultInfo._htEQPRawid[eqpid].ToString();
                    //        alList.Add(eqpid);
                    //        _defaultInfo._htModuleRawid.Add(eqpid, rawid);
                    //    }
                    //}
                    //else
                    //{
                    //    alList.Add(_defaultInfo._sEQPID);
                    //    _defaultInfo._htModuleRawid.Add(_defaultInfo._sEQPID, _defaultInfo._htEQPRawid[_defaultInfo._sEQPID].ToString());
                    //}

                    //for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    //{
                    //    string rawid = Convert.ToString(ds.Tables[0].Rows[row][Definition.CONDITION_KEY_RAWID]);
                    //    string module = Convert.ToString(ds.Tables[0].Rows[row][Definition.CONDITION_KEY_MODULE_NAME]);
                    //    //list[row + 1] = module;
                    //    if (!alList.Contains(module))
                    //    {
                    //        alList.Add(module);
                    //        _defaultInfo._htModuleRawid.Add(module, rawid);
                    //    }
                    //}

                    // add
                    DataTable dtModule = ds.Tables[0].Copy();
                    dtModule.Columns[Definition.CONDITION_KEY_LINE_RAWID].ColumnName = Definition.DynamicCondition_Search_key.LINE;
                    dtModule.Columns[Definition.CONDITION_KEY_AREA_RAWID].ColumnName = Definition.DynamicCondition_Search_key.AREA;
                    dtModule.Columns[Definition.CONDITION_KEY_EQP_MODEL].ColumnName = Definition.DynamicCondition_Search_key.EQPMODEL;
                    dtModule.Columns[Definition.CONDITION_KEY_EQP_RAWID].ColumnName = Definition.DynamicCondition_Search_key.EQP_ID;
                    dtModule.Columns[Definition.CONDITION_KEY_RAWID].ColumnName = Definition.DynamicCondition_Search_key.VALUEDATA;
                    dtModule.Columns[Definition.CONDITION_KEY_MODULE_NAME].ColumnName = Definition.DynamicCondition_Search_key.DISPLAYDATA;
                    ((ComboInterface)controlList[ConditionType.MODULEID]).ComboValueTable = dtModule;

                    //((ComboInterface)controlList[ConditionType.MODULEID]).SetComboValue(alList.ToArray());
                    ((ComboInterface)controlList[ConditionType.MODULEID]).SetComboValue(null);
                }
            }
        }

        private void GetRecipeList()
        {
            LinkedList list = new LinkedList();

            DataTable dtModuleID = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.MODULE];
            if (dtModuleID != null && dtModuleID.Rows.Count > 0)
            {
                ArrayList alModuleRawid = new ArrayList();
                ArrayList alModuleID = new ArrayList();
                for (int i = 0; i < dtModuleID.Rows.Count; i++)
                {
                    string rawid = dtModuleID.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    string module = dtModuleID.Rows[i][Definition.DynamicCondition_Search_key.DISPLAYDATA].ToString();
                    alModuleRawid.Add(rawid);
                    alModuleID.Add(module);
                }
                list.Add(Definition.CONDITION_KEY_MODULE_RAWID_LIST, alModuleRawid);
                list.Add(Definition.CONDITION_KEY_MODULE_ID_LIST, alModuleID);
            }

            DataTable dtProductID = (DataTable)_llstCondition[Definition.DynamicCondition_Search_key.PRODUCT];
            if (dtProductID != null && dtProductID.Rows.Count > 0)
            {
                ArrayList alProductRawid = new ArrayList();
                ArrayList alProductID = new ArrayList();
                for (int i = 0; i < dtProductID.Rows.Count; i++)
                {
                    string rawid = dtProductID.Rows[i][Definition.DynamicCondition_Search_key.VALUEDATA].ToString();
                    string module = dtProductID.Rows[i][Definition.DynamicCondition_Search_key.DISPLAYDATA].ToString();
                    alProductRawid.Add(rawid);
                    alProductID.Add(module);
                }
                list.Add(Definition.CONDITION_KEY_PRODUCT_RAWID_LIST, alProductRawid);
                list.Add(Definition.CONDITION_KEY_PRODUCT_ID_LIST, alProductID);
            }

            byte[] btdata = list.GetSerialData();
            DataSet ds = _fdcWebService.GetRecipe(btdata);

            //_defaultInfo._htRecipeRawid = new Hashtable();
            //if (_defaultInfo._sEQPID == string.Empty && _defaultInfo._alEqpIDList == null) return;

            //LinkedList recipeList = new LinkedList();

            //if (_defaultInfo._alModuleIDList == null)
            //{
            //    string module = (string)_defaultInfo._htModuleRawid[_defaultInfo._sModuleID];
            //    recipeList.Add(Definition.CONDITION_KEY_MODULE_RAWID, module);
            //}
            //else
            //{
            //    ArrayList alModuleRawid = new ArrayList();
            //    for (int i = 0; i < _defaultInfo._alModuleIDList.Count; i++)
            //        alModuleRawid.Add((string)_defaultInfo._htModuleRawid[_defaultInfo._alModuleIDList[i].ToString()]);
            //    recipeList.Add(Definition.CONDITION_KEY_MODULE_RAWID_LIST, alModuleRawid);
            //}

            //string product = (string)_defaultInfo._htProductRawid[_defaultInfo._sProductID];
            //recipeList.Add(Definition.CONDITION_KEY_PRODUCT_RAWID, product);

            //byte[] btdata = recipeList.GetSerialData();
            //DataSet ds = _fdcWebService.GetRecipe(btdata);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    //_defaultInfo._htRecipeRawid.Clear();

                    //ArrayList alValue = new ArrayList();
                    //for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    //{
                    //    string rawid = Convert.ToString(ds.Tables[0].Rows[row][Definition.CONDITION_KEY_RAWID]);
                    //    string recipe = Convert.ToString(ds.Tables[0].Rows[row][Definition.CONDITION_KEY_RECIPE_ID]);

                    //    if (!_defaultInfo._htRecipeRawid.ContainsKey(recipe))
                    //    {
                    //        _defaultInfo._htRecipeRawid.Add(recipe, rawid);
                    //        alValue.Add(recipe);
                    //    }
                    //}

                    // add
                    DataTable dtRecipe = ds.Tables[0].Copy();
                    dtRecipe.Columns[Definition.CONDITION_KEY_EQP_RAWID].ColumnName = Definition.DynamicCondition_Search_key.MODULE;
                    dtRecipe.Columns[Definition.CONDITION_KEY_RAWID].ColumnName = Definition.DynamicCondition_Search_key.VALUEDATA;
                    dtRecipe.Columns[Definition.CONDITION_KEY_RECIPE_ID].ColumnName = Definition.DynamicCondition_Search_key.DISPLAYDATA;
                    ((ComboInterface)controlList[ConditionType.RECIPEID]).ComboValueTable = dtRecipe;

                    //((ComboInterface)controlList[ConditionType.RECIPEID]).SetComboValue(alValue.ToArray());
                    ((ComboInterface)controlList[ConditionType.RECIPEID]).SetComboValue(null);
                }
            }
        }


        #endregion



    }
}
