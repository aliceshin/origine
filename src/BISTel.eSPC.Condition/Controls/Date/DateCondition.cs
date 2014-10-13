using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.eSPC.Condition.Controls.Interface;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;

namespace BISTel.eSPC.Condition.Controls.Date
{
    public partial class DateCondition : UserControl
    {

        #region : Field
        private bool _bShow = false;
        double _dParentPeriod = 1;
        CommonUtility _ComUtil;
        MultiLanguageHandler _mlthandler;
        eSPCWebService.eSPCWebService _wsSPC;
        string _sDateType = string.Empty;
        LinkedList llstDateType = new LinkedList();
        int restrict_sample_count = 50;

        private string _baseDayTime = "00:00:00";
        private List<ShiftTime> _lstShift = new List<ShiftTime>();
        #endregion

        #region : Initialization

        public DateCondition()
        {
            InitializeComponent();

            this._ComUtil = new CommonUtility();
            this._mlthandler = MultiLanguageHandler.getInstance();
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            InitializeCode();
            InitializeLayout();

            ShowButton(_bShow);
            cboType.SelectedIndex = 0;
            cboType_SelectedIndexChanged(null, null);
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            dtpStart.Value = DateTime.Now;
            dtpEnd.Value = DateTime.Now;

            DataSet dsTimeInfo = this._wsSPC.getTimeData();

            DataRow[] drBaseDayTime = dsTimeInfo.Tables[0].Select("CUTOFF_CYCLE='D'");

            if (drBaseDayTime != null && drBaseDayTime.Length > 0)
            {
                this._baseDayTime = drBaseDayTime[0]["FROM_TIME"].ToString();
            }

            DataRow[] drShift = dsTimeInfo.Tables[0].Select("CUTOFF_CYCLE='S'");

            if (drShift != null && drShift.Length > 0)
            {
                for (int i = 0; i < drShift.Length; i++)
                {
                    this.AddShiftInfo(
                        drShift[i]["CUTOFF_KIND"].ToString(),
                        drShift[i]["CUTOFF_CODE"].ToString(),
                        drShift[i]["FROM_TIME"].ToString(),
                        drShift[i]["TO_TIME"].ToString());
                }
            }
        }

        private void AddShiftInfo(string name, string code, string fromTime, string toTime)
        {
            ShiftTime shiftTime = new ShiftTime();
            shiftTime.Name = name;
            shiftTime.Code = code;
            shiftTime.From = fromTime;
            shiftTime.To = toTime;

            _lstShift.Add(shiftTime);
        }

        public void InitializeCode()
        {
            LinkedList llstData = new LinkedList();
            llstData.Add(Definition.DynamicCondition_Condition_key.CATEGORY, Definition.CODE_CATEGORY_PERIOD_TYPE);
            llstData.Add(Definition.DynamicCondition_Condition_key.USE_YN, "Y");
            DataSet ds = this._wsSPC.GetCodeData(llstData.GetSerialData());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    this.cboType.Items.Add(dr["NAME"].ToString());
                    this.llstDateType.Add(dr["CODE"].ToString(), dr["NAME"].ToString());
                }
            }
        }


        public void InitializeLayout()
        {
            try
            {
                //Image bulletImg = this._ComUtil.GetImage(Definition.BUTTON_KEY_BULLET);
                //this.blblbullet01.Image = bulletImg;
                //this.blblbullet02.Image = bulletImg;
                //this.blblbullet03.Image = bulletImg;
            }
            catch (Exception ex)
            {
                BISTel.PeakPerformance.Client.CommonLibrary.LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
        }

        public void ReSetSartDate()
        {
            dtpStart.Value = DateTime.Now.Subtract(TimeSpan.FromDays(this._dParentPeriod));
        }

        #endregion

        #region : Event Handling

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboType.SelectedIndex < 0) return;
            int nIndex = cboType.SelectedIndex;

            dtpStart.Enabled = false;
            dtpEnd.Enabled = false;
            switch (this.DateType)
            {
                case "CURRENT"://Today and Current
                    {
                        DateTime dtFrom = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " " + _baseDayTime, "yyyy-MM-dd HH:mm:ss", null);
                        DateTime dtTo = dtFrom.AddDays(1);

                        if ((dtFrom.CompareTo(DateTime.Now) > 0)) // && dtTo.CompareTo(dt) > 0)
                        {
                            dtTo = dtTo.AddDays(-1);
                            dtFrom = dtFrom.AddDays(-1);
                        }

                        dtpStart.Value = dtFrom;
                        dtpEnd.Value = DateTime.Now;

                        //SPC-867 by Louis
                        //dtpStart.Value = DateTime.Today;
                        //dtpEnd.Value = DateTime.Now;
                    }
                    break;
                case "DAY"://Yesterday
                    {
                        TimeSpan timeGap = TimeSpan.Parse(_baseDayTime);
                        string baseDayTime = _baseDayTime;

                        DateTime dtFrom = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " " + baseDayTime, "yyyy-MM-dd HH:mm:ss", null);
                        DateTime dtTo = dtFrom.AddDays(1);

                        int corrInterval = 0;
                        if ((dtFrom.CompareTo(DateTime.Now) <= 0) && dtTo.CompareTo(DateTime.Now) > 0)
                            corrInterval = -1;
                        else if ((dtFrom.CompareTo(DateTime.Now) > 0) && dtTo.CompareTo(DateTime.Now) > 0)
                            corrInterval = -2;
                        else
                            corrInterval = 0;

                        dtpEnd.Value = dtTo.AddDays(corrInterval);
                        dtpStart.Value = dtFrom.AddDays(corrInterval);

                        //dtpStart.Value = DateTime.Now.AddDays(-1);
                        //dtpEnd.Value = DateTime.Now;
                    }
                    break;
                case "WEEK"://Weekly 
                    {

                        dtpStart.Value = DateTime.Now.AddDays(-7);
                        dtpEnd.Value = DateTime.Now;
                    }
                    break;
                case "SHIFT"://Previous shift
                    {
                        TimeSpan timeGap = TimeSpan.Parse(_baseDayTime);
                        DateTime dtBase = DateTime.Now;

                        int i = 0;
                        int currentIndex = 0;
                        for (; i < _lstShift.Count; i++)
                        {
                            _lstShift[i].BaseDay = dtBase;
                            if (_lstShift[i].IsBetween(dtBase))
                                currentIndex = i;
                        }

                        int preIndex = 0;
                        if (currentIndex == 0)
                            preIndex = _lstShift.Count - 1;
                        if (!_lstShift[preIndex].BaseDay.Equals(dtBase))
                            _lstShift[preIndex].BaseDay = dtBase;

                        if (currentIndex == 0)
                        {
                            if (_lstShift[preIndex].ToDateTime.AddDays(-1) > (dtBase) && preIndex > 0)
                            {
                                dtpEnd.Value = _lstShift[preIndex - 1].ToDateTime.AddDays(-1);
                                dtpStart.Value = _lstShift[preIndex - 1].FromDateTime.AddDays(-1);

                                //_bcbShiftOrHour.SelectedIndex = preIndex - 1;
                            }
                            else
                            {
                                dtpEnd.Value = _lstShift[preIndex].ToDateTime.AddDays(-1);
                                dtpStart.Value = _lstShift[preIndex].FromDateTime.AddDays(-1);

                                //_bcbShiftOrHour.SelectedIndex = preIndex;
                            }
                        }
                        else
                        {
                            if (_lstShift[currentIndex].ToDateTime > (dtBase))
                            {
                                dtpEnd.Value = _lstShift[currentIndex - 1].ToDateTime;
                                dtpStart.Value = _lstShift[currentIndex - 1].FromDateTime;

                                //_bcbShiftOrHour.SelectedIndex = currentIndex - 1;
                            }
                            else
                            {
                                dtpEnd.Value = _lstShift[preIndex].ToDateTime;
                                dtpStart.Value = _lstShift[preIndex].FromDateTime;

                                //_bcbShiftOrHour.SelectedIndex = preIndex;
                            }
                        }

                        //dtpStart.Value = DateTime.Now.Subtract(TimeSpan.FromHours(8));
                        //dtpEnd.Value = dtpStart.Value.Subtract(TimeSpan.FromHours(-8));
                    }
                    break;
                case "MONTH"://Monthly
                    {
                        dtpStart.Value = DateTime.Now.AddMonths(-1);
                        dtpEnd.Value = DateTime.Now;
                    }
                    break;

                case "CUSTOM"://User Define
                    {
                        dtpStart.Enabled = true;
                        dtpEnd.Enabled = true;
                        dtpStart.Value = this.DateTimeStart;
                        dtpEnd.Value = this.DateTimeEnd;
                    }
                    break;
            }
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            //if (!_bShow)
            //    SendEventToParent();
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            //if (!_bShow)
            //    SendEventToParent();
        }



        #endregion

        #region : Public
        public DataTable GetDateTimeSelectedValue(string _sDateType)
        {
            string strDateTime = string.Empty;

            if (_sDateType.Equals("F"))
                strDateTime = dtpStart.Value.ToString(Definition.DATETIME_FORMAT_MS);
            else
                strDateTime = dtpEnd.Value.ToString(Definition.DATETIME_FORMAT_MS);

            DataTable dtValue = DCUtil.MakeDataTableForDCValue(strDateTime, strDateTime);
            return dtValue;
        }

        public void ShowButton(bool bShow)
        {
            _bShow = bShow;
            if (bShow)
            {
                //this.pnlRight.Show();
            }
            else
            {
                //this.pnlRight.Hide();
            }
        }


        public string DateType
        {
            set
            {
                for (int i = 0; i < this.llstDateType.Count; i++)
                {
                    if (value == this.llstDateType.GetKey(i).ToString())
                    {
                        this.cboType.SelectedIndex = i;
                        break;
                    }
                }
            }
            get
            {
                return this.llstDateType.GetKey(this.cboType.SelectedIndex).ToString();
            }
        }


        public DateTime DateTimeStart
        {
            set
            {
                dtpStart.Value = value;
            }
            get
            {
                return dtpStart.Value;
            }
        }


        public DateTime DateTimeEnd
        {
            set
            {
                dtpEnd.Value = value;
            }
            get
            {
                return dtpEnd.Value;
            }
        }





        #endregion

        internal class ShiftTime
        {
            public string Name;
            public string Code;
            public string From;
            public string To;

            private DateTime dtBaseDay;
            private DateTime dtFrom;
            private DateTime dtTo;

            public DateTime BaseDay
            {
                get { return dtBaseDay; }
                set
                {
                    dtBaseDay = value;
                    TimeSpan tsFrom = TimeSpan.Parse(From);
                    TimeSpan tsTo = TimeSpan.Parse(To);

                    string strFrom = dtBaseDay.ToString("yyyy-MM-dd") + " " + From;
                    string strTo = dtBaseDay.ToString("yyyy-MM-dd") + " " + To;

                    dtFrom = DateTime.ParseExact(strFrom, "yyyy-MM-dd HH:mm:ss", null);
                    dtTo = DateTime.ParseExact(strTo, "yyyy-MM-dd HH:mm:ss", null);

                    if (tsFrom.CompareTo(tsTo) > 0)
                        //dtFrom = dtFrom.AddDays(-1);
                        dtTo = dtTo.AddDays(1);
                    else
                    {
                        if (dtFrom.CompareTo(dtTo) > 0)
                            dtTo = dtTo.AddDays(1);
                    }
                }
            }

            /// <summary>
            /// FromDateTime을 가져옵니다.
            /// </summary>
            public DateTime FromDateTime
            {
                get
                {
                    return dtFrom;
                }
            }

            /// <summary>
            /// ToDateTime을 가져옵니다.
            /// </summary>
            public DateTime ToDateTime
            {
                get
                {
                    return dtTo;
                }
            }

            /// <summary>
            /// 인자로 받는 DateTime이 FromDate와 ToDate 사이에 있으면 True를 반환하고, 아니면 false를 반환합니다.
            /// </summary>
            /// <param name="dt">비교할 DateTime 입니다.</param>
            /// <returns></returns>
            public bool IsBetween(DateTime dt)
            {
                if ((dtFrom.CompareTo(dt) <= 0)
                    && dtTo.CompareTo(dt) > 0)
                    return true;
                else
                    return false;
            }


        }



    }
}
