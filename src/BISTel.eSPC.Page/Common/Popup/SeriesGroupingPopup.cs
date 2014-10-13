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
using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    public partial class SeriesGroupingPopup : BasePopupFrm
    {
        #region : Field

        SortedList _slColumnIndex = new SortedList();

        Initialization _Initialization;
        MultiLanguageHandler _mlthandler;

        int _iSeriesNameColIndex = 0;
        int _iEQPIDColIndex = 0;
        int _iEQPAliasColIndex = 0;
        int _iModuleIDColIndex = 0;
        int _iModuleAliasColIndex = 0;
        int _iRecipeIDColIndex = 0;
        int _iLotIDColIndex = 0;
        int _iSlotIDColIndex = 0;
        int _iSubStrateIDColIndex = 0;
        int _iStepIDColIndex = 0;
        int _iSeriesGroupColIndex = 0;
        int _iColorColIndex = 0;
        int _iSymbolColIndex = 0;
        int _iParamColIndex = 0;
        int _iParamAliasColIndex = 0;

        //int _iSelectColIndex = 0;
        //int _iSeriesNameColIndex = 0;
        //int _iStepIDColIndex = 0;    

        private DataTable dt_BsprDataTable = new DataTable();

        private BaseChart ParentUnitedChart;       

        #endregion

        #region : Constructor

        public SeriesGroupingPopup()
        {
            InitializeComponent();
            Init();
        }

        public SeriesGroupingPopup(DataTable dt_Data, BaseChart uChart)
        {
            InitializeComponent();
            this.dt_BsprDataTable = dt_Data.Copy();
            this.ParentUnitedChart = uChart;
            Init();           
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
            this._Initialization.InitializePath();
        }

        private void InitializeBSpread()
        {
            this._slColumnIndex = new SortedList();
            this._slColumnIndex = this._Initialization.InitializeColumnHeader(ref bspr_SeriesGrouping, Definition.PAGE_KEY_SERIESGROUPING_POPUP, true, "");
            this.bspr_SeriesGrouping.UseHeadColor = true;

            string sSeriesNameColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_SERIES_NAME);
            this._iSeriesNameColIndex = (int)this._slColumnIndex[sSeriesNameColName];

            string sEQPIDColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_EQP_ID);
            this._iEQPIDColIndex = (int)this._slColumnIndex[sEQPIDColName];

            string sEQPAliasColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_EQP_ALIAS);
            this._iEQPAliasColIndex = (int)this._slColumnIndex[sEQPAliasColName];

            string sModuleIDColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_MODULE_ID);
            this._iModuleIDColIndex = (int)this._slColumnIndex[sModuleIDColName];

            string sModuleAliasColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_MODULE_ALIAS);
            this._iModuleAliasColIndex = (int)this._slColumnIndex[sModuleAliasColName];

            string sRecipeIDColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_RECIPE_ID);
            this._iRecipeIDColIndex = (int)this._slColumnIndex[sRecipeIDColName];

            string sLotIDColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_LOT_ID);
            this._iLotIDColIndex = (int)this._slColumnIndex[sLotIDColName];

            string sSlotIDColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_SLOT_ID);
            this._iSlotIDColIndex = (int)this._slColumnIndex[sSlotIDColName];

            string sSubStrateIDColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_SUBSTRATE_ID);
            this._iSubStrateIDColIndex = (int)this._slColumnIndex[sSubStrateIDColName];

            string sParamColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_PARAM_NAME);
            this._iParamColIndex = (int)this._slColumnIndex[sParamColName];

            string sStepIDColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_STEP_ID);
            this._iStepIDColIndex = (int)this._slColumnIndex[sStepIDColName];

            string sSeriesGroupColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_SERIES_GROUP);
            this._iSeriesGroupColIndex = (int)this._slColumnIndex[sSeriesGroupColName];

            string sColorColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_COLOR);
            this._iColorColIndex = (int)this._slColumnIndex[sColorColName];

            string sSymbolColName = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_SYMBOL);
            this._iSymbolColIndex = (int)this._slColumnIndex[sSymbolColName];            
        }

        private void InitializeLayout()
        {            
            List<Color> ChartColorList = this.ParentUnitedChart.Get_Chart().SeriesColor.ListColors;

            this.bspr_SeriesGrouping.DataSet = this.dt_BsprDataTable;

            BSpreadUtility bUtil = new BSpreadUtility();            
            
            string[] saColor = new string[ChartColorList.Count];
            for(int i = 0;i<ChartColorList.Count;i++)
            {
                saColor.SetValue(ChartColorList[i].ToString(),i);
            }  

            string[] saSymbol = new string[]{
                "Rectangle",
                "Circle",
                "Triangle",
                "DownTriangle",
                "Cross",
                "DiagCross",
                "Star",
                "Diamond",
                "SmallDot",
                "Nothing",
                "LeftTriangle",
                "RightTriangle",
                "Sphere",
                "PolishedSphere",
                "Hexagon"};
            //for (int i = 0; i < Steema.TeeChart.Styles.PointerStyles; i++)
            //{
                
            //}
            
            this.bspr_SeriesGrouping.DataSet = this.dt_BsprDataTable;

            for (int i = 0; i < dt_BsprDataTable.Rows.Count; i++)
            {
                bUtil.SetComboBoxCellType(this.bspr_SeriesGrouping.ActiveSheet.Cells[i, this._iColorColIndex], saColor, false);
                this.bspr_SeriesGrouping.ActiveSheet.Cells[i, this._iColorColIndex].Value = this.dt_BsprDataTable.Rows[i][Definition.COL_HEADER_KEY_COLOR].ToString();
                bUtil.SetComboBoxCellType(this.bspr_SeriesGrouping.ActiveSheet.Cells[i, this._iSymbolColIndex], saSymbol, false);
                this.bspr_SeriesGrouping.ActiveSheet.Cells[i, this._iSymbolColIndex].Value = this.dt_BsprDataTable.Rows[i][Definition.COL_HEADER_KEY_SYMBOL].ToString();
            }            
        }

        #endregion

        private void bnt_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Draw_Click(object sender, EventArgs e)
        {
            BSpreadUtility bUtil = new BSpreadUtility();
            
            DataTable dt_BsprData = new DataTable();            
            dt_BsprData = ((DataSet)this.bspr_SeriesGrouping.ActiveSheet.DataSource).Tables[0];

            this.ParentUnitedChart.dt_SeriesGroup = dt_BsprData;

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        //private void cb_EQP_ID_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.ClickAction();
        //}

        private void cb_MODULE_ID_CheckedChanged(object sender, EventArgs e)
        {
            this.ClickAction();
        }

        private void cb_RECIPE_ID_CheckedChanged(object sender, EventArgs e)
        {
            this.ClickAction();
        }

        private void cb_LOT_ID_CheckedChanged(object sender, EventArgs e)
        {
            this.ClickAction();
        }

        private void cb_SLOT_ID_CheckedChanged(object sender, EventArgs e)
        {
            this.ClickAction();
        }

        private void cb_STEP_ID_CheckedChanged(object sender, EventArgs e)
        {
            this.ClickAction();
        }

        private void cb_Param_Name_CheckedChanged(object sender, EventArgs e)
        {
            this.ClickAction();
        }

        private void ClickAction()
        {
            DataSet ds_Spread = (DataSet)this.bspr_SeriesGrouping.DataSet;
            DataTable dt_Spread = ds_Spread.Tables[0];

            string GroupName = "";

            ///Check별 Sorting
            //if (cb_EQP_ID.Checked)
            //{
            //    DataView dv = dt_Spread.DefaultView;
            //    dv.Sort = Definition.COL_HEADER_KEY_EQP_ID;
            //    dt_Spread = dv.ToTable();
            //}
            if (cb_MODULE_ID.Checked)
            {
                DataView dv = dt_Spread.DefaultView;
                dv.Sort = Definition.COL_HEADER_KEY_MODULE_ID;
                dt_Spread = dv.ToTable();
            }
            if (cb_RECIPE_ID.Checked)
            {
                DataView dv = dt_Spread.DefaultView;
                dv.Sort = Definition.COL_HEADER_KEY_RECIPE_ID;
                dt_Spread = dv.ToTable();
            }
            if (cb_LOT_ID.Checked)
            {
                DataView dv = dt_Spread.DefaultView;
                dv.Sort = Definition.COL_HEADER_KEY_LOT_ID;
                dt_Spread = dv.ToTable();
            }
            if (cb_SLOT_ID.Checked)
            {
                DataView dv = dt_Spread.DefaultView;
                dv.Sort = Definition.COL_HEADER_KEY_SLOT_ID;
                dt_Spread = dv.ToTable();
            }
            if (cb_Param_Name.Checked)
            {
                DataView dv = dt_Spread.DefaultView;
                dv.Sort = Definition.COL_HEADER_KEY_PARAM_NAME;
                dt_Spread = dv.ToTable();
            }
            if (cb_STEP_ID.Checked)
            {
                DataView dv = dt_Spread.DefaultView;
                dv.Sort = Definition.COL_HEADER_KEY_STEP_ID;
                dt_Spread = dv.ToTable();
            }            

            for (int i = 0; i < dt_Spread.Rows.Count; i++)
            {
                bool isStart = false;
                //if (cb_EQP_ID.Checked)
                //{
                //    string sEqpID = dt_Spread.Rows[i][Definition.COL_HEADER_KEY_EQP_ID].ToString();

                //    if (sEqpID == "")
                //    {
                //        sEqpID = "NULL";
                //    }

                //    if (isStart)
                //    {
                //        GroupName += "^" + sEqpID;
                //    }
                //    else
                //    {
                //        GroupName = sEqpID;
                //        isStart = true;
                //    }

                //}
                if (cb_MODULE_ID.Checked)
                {
                    string sModuleID = dt_Spread.Rows[i][Definition.COL_HEADER_KEY_MODULE_ID].ToString();

                    if (sModuleID == "")
                    {
                        sModuleID = "NULL";
                    }

                    if (isStart)
                    {
                        GroupName += "^" + sModuleID;
                    }
                    else
                    {
                        GroupName = sModuleID;
                        isStart = true;
                    }
                }
                if (cb_RECIPE_ID.Checked)
                {
                    string sRecipeID = dt_Spread.Rows[i][Definition.COL_HEADER_KEY_RECIPE_ID].ToString();

                    if (sRecipeID == "")
                    {
                        sRecipeID = "NULL";
                    }

                    if (isStart)
                    {
                        GroupName += "^" + sRecipeID;
                    }
                    else
                    {
                        GroupName = sRecipeID;
                        isStart = true;
                    }
                }
                if (cb_LOT_ID.Checked)
                {
                    string sLotID = dt_Spread.Rows[i][Definition.COL_HEADER_KEY_LOT_ID].ToString();

                    if (sLotID == "")
                    {
                        sLotID = "NULL";
                    }

                    if (isStart)
                    {
                        GroupName += "^" + sLotID;
                    }
                    else
                    {
                        GroupName = sLotID;
                        isStart = true;
                    }
                }
                if (cb_SLOT_ID.Checked)
                {
                    string sSlotID = dt_Spread.Rows[i][Definition.COL_HEADER_KEY_SLOT_ID].ToString();

                    if (sSlotID == "")
                    {
                        sSlotID = "NULL";
                    }

                    if (isStart)
                    {
                        GroupName += "^" + sSlotID;
                    }
                    else
                    {
                        GroupName = sSlotID;
                        isStart = true;
                    }
                }
                if (cb_Param_Name.Checked)
                {
                    string sParamName = dt_Spread.Rows[i][Definition.COL_HEADER_KEY_PARAM_NAME].ToString();

                    if (sParamName == "")
                    {
                        sParamName = "NULL";
                    }

                    if (isStart)
                    {
                        GroupName += "^" + sParamName;
                    }
                    else
                    {
                        GroupName = sParamName;
                        isStart = true;
                    }
                }
                if (cb_STEP_ID.Checked)
                {
                    string sStepID = dt_Spread.Rows[i][Definition.COL_HEADER_KEY_STEP_ID].ToString();

                    if (sStepID == "")
                    {
                        sStepID = "NULL";
                    }

                    if (isStart)
                    {
                        GroupName += "^" + sStepID;
                    }
                    else
                    {
                        GroupName = sStepID;
                        isStart = true;
                    }
                }

                dt_Spread.Rows[i][Definition.COL_HEADER_KEY_SERIES_GROUP] = GroupName;
            }
            
            /// 동일칼라 지정
            /// 

            List<string> lGroupName = new List<string>();
            List<Color> ChartColorList = this.ParentUnitedChart.Get_Chart().SeriesColor.ListColors;
            int ColorIndex = -1;

            for (int i = 0; i < dt_Spread.Rows.Count; i++)
            {
                string sGroupName = dt_Spread.Rows[i][Definition.COL_HEADER_KEY_SERIES_GROUP].ToString();
                if (!lGroupName.Contains(sGroupName))
                {
                    lGroupName.Add(sGroupName);
                    ColorIndex++;
                    dt_Spread.Rows[i][Definition.COL_HEADER_KEY_COLOR] = ChartColorList[ColorIndex].ToString();                    
                }
                else
                {
                    dt_Spread.Rows[i][Definition.COL_HEADER_KEY_COLOR] = ChartColorList[ColorIndex].ToString();
                }

            }

            bspr_SeriesGrouping.DataSet = dt_Spread;
        }        
    }
}
