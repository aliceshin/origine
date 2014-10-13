using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using BISTel.eSPC.Common;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Page.Common
{
    public partial class StepToStepPopup : BasePopupFrm
    {
        #region : Field

        SortedList _slColumnIndex = new SortedList();
        CommonUtility _ComUtil;
        Initialization _Initialization;
        MultiLanguageHandler _mlthandler;
        DataTable _dt = new DataTable();

        int _iSelectColIndex = 0;
        int _iSeriesNameColIndex = 0;
        int _iStepIDColIndex = 0;

        ArrayList TableNameList = new ArrayList();
        public ArrayList TableNameArrayList
        {
            get
            {
                return TableNameList;
            }
        }

        #endregion

        #region : Constructor

        public StepToStepPopup()
        {
            InitializeComponent();
            this.Init();
        }

        #endregion

        #region : Initialize

        private void Init()
        {
            this.InitializeVariable();
            this.InitializeBSpread();
            this.InitializeLayout();
        }

        public void InitializeVariable()
        {
            this._Initialization = new Initialization();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._ComUtil = new CommonUtility();
            this._Initialization.InitializePath();
        }

        private void InitializeBSpread()
        {
            this._slColumnIndex = new SortedList();
            this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref bspr_StepData, Definition.PAGE_KEY_STEP_TO_STEP_POPUP, false, "");
            this.bspr_StepData.UseHeadColor = true;
            this._Initialization.SetCheckColumnHeader(this.bspr_StepData, this._iSelectColIndex);

            string sSeriesNameColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_SERIES_NAME);
            this._iSeriesNameColIndex = (int)this._slColumnIndex[sSeriesNameColName];

            string sStepIDColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_STEP_ID);
            this._iStepIDColIndex = (int)this._slColumnIndex[sStepIDColName];
        }

        public void InitializeLayout()
        {
            try
            {
                Image bulletImg = this._ComUtil.GetImage(Definition.BUTTON_KEY_BULLET);
                this.blbullet.Image = bulletImg;
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
        }

        #endregion

        #region : Publish

        public void SetSpread(DataTable ds)
        {
            if (bspr_StepData.DataSet != null)
            {
                DataSet ds_Temp = new DataSet();
                bspr_StepData.DataSet = ds_Temp;
            }
            bspr_StepData.DataSet = ds;
            _dt = ds.Clone();
            _dt = ds.Copy();
            bspr_StepData.ActiveSheet.SortRows(_iSeriesNameColIndex, true, false);
        }

        public void SetComboList(string[] sList)
        {
            bcComboStep.AddItems(sList);
        }

        #endregion

        #region : Event

        private void bbtn_OK_Click(object sender, EventArgs e)
        {
            this.TableNameList.Clear();
            
            for (int i = 0; i < bspr_StepData.ActiveSheet.RowCount; i++)
            {
                string sBool = this.bspr_StepData.ActiveSheet.Cells[i, 0].Text;
                if(sBool == "True")
                //if (bool.Parse(this.bspr_StepData.ActiveSheet.Cells[i, 0].Text))
                {
                    this.TableNameList.Add(this.bspr_StepData.ActiveSheet.Cells[i, _iSeriesNameColIndex].Text);
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void bbtn_Cancel_Click(object sender, EventArgs e)
        {
            this.TableNameList.Clear();
            this.Close();

        }       

        private void bspr_StepData_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //int Count = e.Row;

            //if (bool.Parse(this.bspr_StepData.ActiveSheet.Cells[Count, 0].Text))
            //{
            //    this.TableNameList.Add(this.bspr_StepData.ActiveSheet.Cells[Count, _iSeriesNameColIndex].Text);
            //}
            //else
            //{
            //    this.TableNameList.Remove(this.bspr_StepData.ActiveSheet.Cells[Count, _iSeriesNameColIndex].Text);
            //}
            
        }

        #endregion

        private void bspr_StepData_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (this.bspr_StepData.ActiveSheet.RowCount > 0)
            {
                if (e.ColumnHeader)
                {
                    if (e.Column == this._iSelectColIndex)
                    {
                        FarPoint.Win.Spread.CellType.ICellType ct = this.bspr_StepData.ActiveSheet.ColumnHeader.Cells[e.Row, e.Column].CellType;
                        if (ct is FarPoint.Win.Spread.CellType.CheckBoxCellType)
                        {
                            this._Initialization.SetCheckColumnHeaderStatus(this.bspr_StepData, this._iSelectColIndex);
                        }
                    }
                }
            }
        }

        private void bcComboStep_Item_Check(object sender, ItemCheckEventArgs e)
        {
            string sComboList = this.bcComboStep.Text;
            string[] sFilterList = sComboList.Split(',');
            DataTable dtnew = new DataTable();
            dtnew = _dt.Clone();
            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                string sStepList = _dt.Rows[i][0].ToString();
                string[] sStepListFilter = sStepList.Split('^');
                if (sStepListFilter.Length == 5)
                {
                    for (int j = 0; j < sFilterList.GetLength(0); j++)
                    {
                        if (sStepListFilter[4].Equals(sFilterList[j]))
                        {
                            dtnew.Rows.Add(sStepList);
                        }
                    }
                }
            }
            this.bspr_StepData.DataSet = dtnew;
        }
    }
}
