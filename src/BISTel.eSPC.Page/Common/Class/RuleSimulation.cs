using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Page.Common
{
    /// <summary>
    /// Rule Simulation을 위한 각종 Validation Method 모음
    /// </summary>
    public class RuleSimulation
    {
        #region : Field

        private double _dUSL;
        private double _dLSL;
        private double _dUCL;
        private double _dLCL;

        private int _iN;
        private int _iM;
        private double _dStd;
        private double _dTrendLimit;
        private double _dUTL; //Upper Technical Limit
        private double _dLTL; //Lower Technical Limit
        private double _dRawUCL;
        private double _dRawLCL;

        private string _sValueType;
        private bool _isModuleMode;
        private string _sRuleMode;

        private DataTable _dtChartData;

        private int _iRealCount;

        private eSPCWebService.eSPCWebService _wsSPC = null;

        #endregion


        #region : Property

        public double USL
        {
            set { _dUSL = value; }
        }
        public double LSL
        {
            set { _dLSL = value; }
        }
        public double UCL
        {
            set { _dUCL = value; }
        }
        public double LCL
        {
            set { _dLCL = value; }
        }

        public int N
        {
            set { _iN = value; }
        }
        public int M
        {
            set { _iM = value; }
        }
        public double STD
        {
            set { _dStd = value; }
        }
        public double UTL
        {
            set { _dUTL = value; }
        }
        public double LTL
        {
            set { _dLTL = value; }
        }
        public double TRENDLIMIT
        {
            set { _dTrendLimit = value; }
        }
        public double RAW_UCL
        {
            set { _dRawUCL = value; }
        }
        public double RAW_LCL
        {
            set { _dRawLCL = value; }
        }


        public string VALUETYPE
        {
            set { _sValueType = value; }
        }
        public bool MODULE_MODE
        {
            set { _isModuleMode = value; }
        }

        public DataTable CHART_DATA
        {
            set { _dtChartData = value; }
        }

        public int REAL_COUNT
        {
            get { return _iRealCount; }
        }

        #endregion


        #region : Constructor & Initialize

        public RuleSimulation()
        {
        }

        public void Initialize()
        {
            _wsSPC = new eSPCWebService.eSPCWebService();
        }

        #endregion


        public ArrayList Simulate(string ruleNumber)
        {
            ArrayList alReturn = new ArrayList();
            double spec = 0.0;
            double spec2 = 0.0; //소수의 rule에만 사용됨
            double lowThreshold = 0.0;
            double highThreshold = 0.0;
            _sRuleMode = "";

            #region RuleMode, Spec Definition

            if (ruleNumber.Equals("1") || ruleNumber.Equals("35"))
            {
                spec = _dUSL;
                _sRuleMode = Definition.RULE_MODE.BIGGER;
            }
            else if (ruleNumber.Equals("2") || ruleNumber.Equals("36"))
            {
                spec = _dLSL;
                _sRuleMode = Definition.RULE_MODE.SMALLER;
            }
            else if (ruleNumber.Equals("3") || ruleNumber.Equals("5") || ruleNumber.Equals("7") ||
                ruleNumber.Equals("20") || ruleNumber.Equals("22") || ruleNumber.Equals("24") ||
                ruleNumber.Equals("39"))
            {
                spec = _dUCL;
                _sRuleMode = Definition.RULE_MODE.BIGGER;
            }
            else if (ruleNumber.Equals("4") || ruleNumber.Equals("6") || ruleNumber.Equals("8") ||
                ruleNumber.Equals("21") || ruleNumber.Equals("23") || ruleNumber.Equals("25") ||
                ruleNumber.Equals("40"))
            {
                spec = _dLCL;
                _sRuleMode = Definition.RULE_MODE.SMALLER;
            }
            else if (ruleNumber.Equals("9"))
            {
                _sRuleMode = Definition.RULE_MODE.BIGGER_TREND_N;
            }
            else if (ruleNumber.Equals("10"))
            {
                _sRuleMode = Definition.RULE_MODE.SMALLER_TREND_N;
            }
            else if (ruleNumber.Equals("11"))
            {
                spec = (_dUCL + _dLCL) / 2.0;
                _sRuleMode = Definition.RULE_MODE.BIGGER_N;
            }
            else if (ruleNumber.Equals("12"))
            {
                spec = (_dUCL + _dLCL) / 2.0;
                _sRuleMode = Definition.RULE_MODE.SMALLER_N;
            }
            else if (ruleNumber.Equals("13"))
            {
                double centerLine = (_dUCL + _dLCL) / 2.0;

                lowThreshold = centerLine + (2 * _dStd);
                highThreshold = centerLine + (3 * _dStd);
                _sRuleMode = Definition.RULE_MODE.IN_ONE_ZONE_NM;
            }
            else if (ruleNumber.Equals("14"))
            {
                double centerLine = (_dUCL + _dLCL) / 2.0;

                lowThreshold = centerLine - (3 * _dStd);
                highThreshold = centerLine - (2 * _dStd);
                _sRuleMode = Definition.RULE_MODE.IN_ONE_ZONE_NM;
            }
            else if (ruleNumber.Equals("15"))
            {
                double centerLine = (_dUCL + _dLCL) / 2.0;

                lowThreshold = centerLine + (1 * _dStd);
                highThreshold = centerLine + (3 * _dStd);
                _sRuleMode = Definition.RULE_MODE.IN_ONE_ZONE_NM;
            }
            else if (ruleNumber.Equals("16"))
            {
                double centerLine = (_dUCL + _dLCL) / 2.0;

                lowThreshold = centerLine - (3 * _dStd);
                highThreshold = centerLine - (1 * _dStd);
                _sRuleMode = Definition.RULE_MODE.IN_ONE_ZONE_NM;
            }
            else if (ruleNumber.Equals("17"))
            {
                double centerLine = (_dUCL + _dLCL) / 2.0;

                lowThreshold = centerLine - (1 * _dStd);
                highThreshold = centerLine + (1 * _dStd);
                _sRuleMode = Definition.RULE_MODE.IN_ONE_ZONE_N;
            }
            else if (ruleNumber.Equals("18") || ruleNumber.Equals("27") || ruleNumber.Equals("28"))
            {
                _sRuleMode = Definition.RULE_MODE.RISING_N;
            }
            else if (ruleNumber.Equals("19"))
            {
                _sRuleMode = Definition.RULE_MODE.FALLING_N;
            }
            else if (ruleNumber.Equals("26"))
            {
                _sRuleMode = Definition.RULE_MODE.RISING_FALLING_N;
            }
            else if (ruleNumber.Equals("29"))
            {
                spec = _dUSL;
                spec2 = _dLSL;
                _sRuleMode = Definition.RULE_MODE.BIGGER_OR_SMALLER_NM;
            }
            else if (ruleNumber.Equals("30"))
            {
                spec = _dUSL;
                _sRuleMode = Definition.RULE_MODE.BIGGER_NM;
            }
            else if (ruleNumber.Equals("31"))
            {
                spec = _dLSL;
                _sRuleMode = Definition.RULE_MODE.SMALLER_NM;
            }
            else if (ruleNumber.Equals("32"))
            {
                spec = _dUCL;
                spec2 = _dLCL;
                _sRuleMode = Definition.RULE_MODE.BIGGER_OR_SMALLER_NM;
            }
            else if (ruleNumber.Equals("33") || ruleNumber.Equals("37"))
            {
                spec = _dUCL;
                _sRuleMode = Definition.RULE_MODE.BIGGER_NM;
            }
            else if (ruleNumber.Equals("34") || ruleNumber.Equals("38"))
            {
                spec = _dLCL;
                _sRuleMode = Definition.RULE_MODE.SMALLER_NM;
            }
            else if (ruleNumber.Equals("41"))
            {
                spec = _dRawUCL;
                _sRuleMode = Definition.RULE_MODE.BIGGER;
            }
            else if (ruleNumber.Equals("42"))
            {
                spec = _dRawLCL;
                _sRuleMode = Definition.RULE_MODE.SMALLER;
            }
            else if (ruleNumber.Equals("43") || ruleNumber.Equals("45"))
            {
                spec = _dUTL;
                _sRuleMode = Definition.RULE_MODE.BIGGER;
            }
            else if (ruleNumber.Equals("44") || ruleNumber.Equals("46"))
            {
                spec = _dLTL;
                _sRuleMode = Definition.RULE_MODE.SMALLER;
            }
            else if (ruleNumber.Equals("47"))
            {
                _sRuleMode = Definition.RULE_MODE.BIGGER_TREND_LR_N;
            }
            else if (ruleNumber.Equals("48"))
            {
                _sRuleMode = Definition.RULE_MODE.SMALLER_TREND_LR_N;
            }

            #endregion

            //////////////////////////////////////////////////////////////////

            #region Result Getting

            if (_sRuleMode.Equals(Definition.RULE_MODE.BIGGER) ||
                _sRuleMode.Equals(Definition.RULE_MODE.SMALLER))
            {
                alReturn = GetListOfViolator(spec, _dtChartData);
            }
            else if (_sRuleMode.Equals(Definition.RULE_MODE.BIGGER_N) ||
                     _sRuleMode.Equals(Definition.RULE_MODE.SMALLER_N))
            {
                if (_isModuleMode)
                {
                    alReturn = GetListOfModuleModeViolatorN(spec, _dtChartData);
                }
                else
                {
                    alReturn = GetListOfViolatorN(spec, _dtChartData);
                }
            }
            else if (_sRuleMode.Equals(Definition.RULE_MODE.BIGGER_NM) ||
                     _sRuleMode.Equals(Definition.RULE_MODE.SMALLER_NM) ||
                     _sRuleMode.Equals(Definition.RULE_MODE.BIGGER_OR_SMALLER_NM))
            {
                if (_isModuleMode)
                {
                    alReturn = GetListOfModuleModeViolatorNM(spec, spec2, _dtChartData);
                }
                else
                {
                    alReturn = GetListOfViolatorNM(spec, spec2, _dtChartData);
                }
            }
            else if (_sRuleMode.Equals(Definition.RULE_MODE.RISING_N) ||
                     _sRuleMode.Equals(Definition.RULE_MODE.FALLING_N) ||
                     _sRuleMode.Equals(Definition.RULE_MODE.RISING_FALLING_N))
            {
                if (_isModuleMode)
                {
                    alReturn = GetListOfModuleModeRisingAndFallingN(_dtChartData);
                }
                else
                {
                    alReturn = GetListOfRisingAndFallingN(_dtChartData);
                }
            }
            else if (_sRuleMode.Equals(Definition.RULE_MODE.IN_ONE_ZONE_N))
            {
                if (_isModuleMode)
                {
                    alReturn = GetListOfModuleModeInZoneN(lowThreshold, highThreshold, _dtChartData);
                }
                else
                {
                    alReturn = GetListInZoneN(lowThreshold, highThreshold, _dtChartData);
                }
            }
            else if (_sRuleMode.Equals(Definition.RULE_MODE.IN_ONE_ZONE_NM))
            {
                if (_isModuleMode)
                {
                    alReturn = GetListOfModuleModeInZoneNM(lowThreshold, highThreshold, _dtChartData);
                }
                else
                {
                    alReturn = GetListInZoneNM(lowThreshold, highThreshold, _dtChartData);
                }
            }
            else if (_sRuleMode.Equals(Definition.RULE_MODE.BIGGER_TREND_N) ||
                     _sRuleMode.Equals(Definition.RULE_MODE.SMALLER_TREND_N))
            {
                if (_isModuleMode)
                {
                    alReturn = GetListOfModuleModeTrendViolatorN(false, _dtChartData);
                }
                else
                {
                    alReturn = GetListOfTrendViolatorN(false, _dtChartData);
                }
            }
            else if (_sRuleMode.Equals(Definition.RULE_MODE.BIGGER_TREND_LR_N) ||
                     _sRuleMode.Equals(Definition.RULE_MODE.SMALLER_TREND_LR_N))
            {
                if (_isModuleMode)
                {
                    alReturn = GetListOfModuleModeTrendViolatorN(true, _dtChartData);
                }
                else
                {
                    alReturn = GetListOfTrendViolatorN(true, _dtChartData);
                }
            }

            #endregion

            return alReturn;
        }

        private ArrayList GetListOfViolator(double spec, DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                DataRow dr = dtData.Rows[i];
                double value = 0.0;

                if (double.TryParse(dr[_sValueType].ToString(), out value))
                {
                    if (_sRuleMode.Equals(Definition.RULE_MODE.BIGGER))
                    {
                        if (IsBig(value, spec))
                        {
                            alReturn.Add(i);
                            realCount++;
                        }
                    }
                    else
                    {
                        if (IsSmall(value, spec))
                        {
                            alReturn.Add(i);
                            realCount++;
                        }
                    }
                }
            }

            _iRealCount = realCount;

            return alReturn;
        }

        private ArrayList GetListOfViolatorN(double spec, DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;
            ArrayList alTemp = new ArrayList();

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                DataRow dr = dtData.Rows[i];
                int dataIndex = GetOriginalDataIndex(dr, i);
                double value = 0.0;

                if (double.TryParse(dr[_sValueType].ToString(), out value))
                {
                    if (_sRuleMode.Equals(Definition.RULE_MODE.BIGGER_N))
                    {
                        if (IsBig(value, spec))
                        {
                            alTemp.Add(dataIndex);
                        }
                        else
                        {
                            alTemp.Clear();
                        }
                    }
                    else if (_sRuleMode.Equals(Definition.RULE_MODE.SMALLER_N))
                    {
                        if (IsSmall(value, spec))
                        {
                            alTemp.Add(dataIndex);
                        }
                        else
                        {
                            alTemp.Clear();
                        }
                    }
                }

                if (alTemp.Count == _iN)
                {
                    //테스트용으로 이렇게 했었음.
                    //for (int j = 0; j < alTemp.Count; j++)
                    //{
                    //    alReturn.Add(alTemp[j]);
                    //}

                    alReturn.Add(alTemp[alTemp.Count - 1]);

                    alTemp.Clear();
                    realCount++;
                }
            }

            _iRealCount = realCount;

            return alReturn;
        }

        private ArrayList GetListOfViolatorNM(double spec, double spec2, DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;
            ArrayList alTemp = new ArrayList();
            ArrayList alViolatedIndex = new ArrayList();

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                DataRow dr = dtData.Rows[i];
                int dataIndex = GetOriginalDataIndex(dr, i);
                double value = 0.0;

                if (double.TryParse(dr[_sValueType].ToString(), out value))
                {
                    if (_sRuleMode.Equals(Definition.RULE_MODE.BIGGER_NM))
                    {
                        if (IsBig(value, spec))
                        {
                            alViolatedIndex.Add(i);
                        }
                    }
                    else if (_sRuleMode.Equals(Definition.RULE_MODE.SMALLER_NM))
                    {
                        if (IsSmall(value, spec))
                        {
                            alViolatedIndex.Add(i);
                        }
                    }
                    else
                    {
                        if (IsBig(value, spec) || IsSmall(value, spec2))
                        {
                            alViolatedIndex.Add(i);
                        }
                    }

                    alTemp.Add(dataIndex);
                }

                if (alViolatedIndex.Count >= _iN)
                {
                    //테스트용으로 이렇게 했었음.
                    //for (int j = 0; j < alTemp.Count; j++)
                    //{
                    //    alReturn.Add(alTemp[j]);
                    //}

                    alReturn.Add(alTemp[alTemp.Count - 1]);

                    alViolatedIndex.Clear();
                    alTemp.Clear();

                    realCount++;
                }
                else
                {
                    if (alTemp.Count == _iM)
                    {
                        if (alViolatedIndex.Count > 0)
                        {
                            if ((i - (_iM - 1)) == (int)alViolatedIndex[0])
                            {
                                alViolatedIndex.RemoveAt(0);
                            }
                        }

                        alTemp.RemoveAt(0);
                    }
                }
            }

            _iRealCount = realCount;

            return alReturn;
        }

        private ArrayList GetListOfRisingAndFallingN(DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;
            ArrayList alTemp = new ArrayList();
            double prevValue = 0.0;
            bool isFirst = true;

            bool is26First = true; //26번 rule에만 사용됨
            bool isRised = false; //26번 rule에만 사용됨
            bool isFalled = false; //26번 rule에만 사용됨

            if (_iN <= 1) //N=1일때는 첫 번째만 실행되는 코드 막기
            {
                isFirst = false;
            }

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                DataRow dr = dtData.Rows[i];
                int dataIndex = GetOriginalDataIndex(dr, i);
                double value = 0.0;

                if (double.TryParse(dr[_sValueType].ToString(), out value))
                {
                    if (isFirst)
                    {
                        prevValue = value;
                        isFirst = false;

                        alTemp.Add(dataIndex);

                        continue;
                    }

                    if (_sRuleMode.Equals(Definition.RULE_MODE.RISING_N))
                    {
                        if (!IsBig(value, prevValue))
                        {
                            alTemp.Clear();
                        }

                        alTemp.Add(dataIndex);
                    }
                    else if (_sRuleMode.Equals(Definition.RULE_MODE.FALLING_N))
                    {
                        if (!IsSmall(value, prevValue))
                        {
                            alTemp.Clear();
                        }

                        alTemp.Add(dataIndex);
                    }
                    else //26번 rule일 때
                    {
                        if (is26First )
                        {
                            if (IsBig(value, prevValue))
                            {
                                isRised = true;
                            }
                            else if (IsSmall(value, prevValue))
                            {
                                isFalled = true;
                            }

                            alTemp.Add(dataIndex);

                            if (isRised || isFalled)
                            {
                                if (alTemp.Count == _iN)
                                {
                                    alReturn.Add(alTemp[alTemp.Count - 1]);

                                    alTemp.Clear();
                                    realCount++;
                                }
                            }

                            is26First = false;
                            prevValue = value;

                            continue;
                        }

                        if (IsBig(value, prevValue))
                        {
                            if (!isRised)
                            {
                                int count = alTemp.Count;

                                for (int j = 0; j < count - 1; j++)
                                {
                                    alTemp.RemoveAt(0);
                                }

                                isRised = true;
                                isFalled = false;
                            }
                        }
                        else if (IsSmall(value, prevValue))
                        {
                            if (!isFalled)
                            {
                                int count = alTemp.Count;

                                for (int j = 0; j < count - 1; j++)
                                {
                                    alTemp.RemoveAt(0);
                                }

                                isFalled = true;
                                isRised = false;
                            }
                        }
                        else
                        {
                            int count = alTemp.Count;

                            if (count == 1)
                            {
                                alTemp.RemoveAt(0);
                            }

                            for (int j = 0; j < count - 1; j++)
                            {
                                alTemp.RemoveAt(0);
                            }

                            isRised = false;
                            isFalled = false;
                        }

                        alTemp.Add(dataIndex);
                    }
                }

                if (alTemp.Count == _iN)
                {
                    //테스트용으로 이렇게 했었음.
                    //for (int j = 0; j < alTemp.Count; j++)
                    //{
                    //    alReturn.Add(alTemp[j]);
                    //}

                    alReturn.Add(alTemp[alTemp.Count - 1]);

                    alTemp.Clear();
                    realCount++;
                }

                prevValue = value;
            }

            _iRealCount = realCount;

            return alReturn;
        }

        private ArrayList GetListInZoneN(double lowThreshold, double highThreshold, DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;
            ArrayList alTemp = new ArrayList();

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                DataRow dr = dtData.Rows[i];
                int dataIndex = GetOriginalDataIndex(dr, i);
                double value = 0.0;

                if (double.TryParse(dr[_sValueType].ToString(), out value))
                {
                    if (IsInZone(value, lowThreshold, highThreshold))
                    {
                        alTemp.Add(dataIndex);
                    }
                    else
                    {
                        alTemp.Clear();
                    }
                }

                if (alTemp.Count >= _iN)
                {
                    //테스트용으로 이렇게 했었음.
                    //for (int j = 0; j < alTemp.Count; j++)
                    //{
                    //    alReturn.Add(alTemp[j]);
                    //}

                    alReturn.Add(alTemp[alTemp.Count - 1]);

                    alTemp.Clear();
                    realCount++;
                }
            }

            _iRealCount = realCount;

            return alReturn;
        }

        private ArrayList GetListInZoneNM(double lowThreshold, double highThreshold, DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;
            ArrayList alTemp = new ArrayList();
            ArrayList alViolatedIndex = new ArrayList();

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                DataRow dr = dtData.Rows[i];
                int dataIndex = GetOriginalDataIndex(dr, i);
                double value = 0.0;

                if (double.TryParse(dr[_sValueType].ToString(), out value))
                {
                    if (IsInZone(value, lowThreshold, highThreshold))
                    {
                        alViolatedIndex.Add(i);
                    }

                    alTemp.Add(dataIndex);
                }

                if (alViolatedIndex.Count >= _iN)
                {
                    //테스트용으로 이렇게 했었음.
                    //for (int j = 0; j < alTemp.Count; j++)
                    //{
                    //    alReturn.Add(alTemp[j]);
                    //}

                    alReturn.Add(alTemp[alTemp.Count - 1]);

                    alTemp.Clear();
                    alViolatedIndex.Clear();

                    realCount++;
                }
                else
                {
                    if (alTemp.Count == _iM)
                    {
                        if (alViolatedIndex.Count > 0)
                        {
                            if ((i - (_iM - 1)) == (int)alViolatedIndex[0])
                            {
                                alViolatedIndex.RemoveAt(0);
                            }
                        }

                        alTemp.RemoveAt(0);
                    }
                }
            }

            _iRealCount = realCount;

            return alReturn;
        }

        private ArrayList GetListOfTrendViolatorN(bool isLinearRegression, DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;
            ArrayList alTemp = new ArrayList();
            ArrayList alTrend = new ArrayList();

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                DataRow dr = dtData.Rows[i];
                int dataIndex = GetOriginalDataIndex(dr, i);
                double value = 0.0;

                if (double.TryParse(dr[_sValueType].ToString(), out value))
                {
                    alTemp.Add(dataIndex);
                    alTrend.Add(value);
                }

                if (alTemp.Count == _iN)
                {
                    double calculatedTrend = GetTrendValue(alTrend, isLinearRegression);

                    if (_sRuleMode.Equals(Definition.RULE_MODE.BIGGER_TREND_N) && calculatedTrend > _dTrendLimit ||
                        _sRuleMode.Equals(Definition.RULE_MODE.SMALLER_TREND_N) && calculatedTrend < _dTrendLimit ||
                        _sRuleMode.Equals(Definition.RULE_MODE.BIGGER_TREND_LR_N) && calculatedTrend > _dTrendLimit ||
                        _sRuleMode.Equals(Definition.RULE_MODE.SMALLER_TREND_LR_N) && calculatedTrend < _dTrendLimit)
                    {
                        //테스트용으로 이렇게 했었음.
                        //for (int j = 0; j < alTemp.Count; j++)
                        //{
                        //    alReturn.Add(alTemp[j]);
                        //}

                        alReturn.Add(alTemp[alTemp.Count - 1]);

                        realCount++;

                        alTemp.Clear();
                        alTrend.Clear();
                    }
                    else
                    {
                        alTemp.RemoveAt(0);
                        alTrend.RemoveAt(0);
                    }
                }
            }

            _iRealCount = realCount;

            return alReturn;
        }


        private ArrayList GetListOfModuleModeViolatorN(double spec, DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;

            Dictionary<string, DataTable> dic = GetDataPerModule(dtData);

            foreach (string key in dic.Keys)
            {
                DataTable dtDataOneModule = dic[key];

                ArrayList alTemp = GetListOfViolatorN(spec, dtDataOneModule);

                for (int i = 0; i < alTemp.Count; i++)
                {
                    alReturn.Add(alTemp[i]);
                }

                //_iRealCount의 경우 GetListOfViolatorN이 돌 때마다 값이 변함.
                realCount += _iRealCount;
            }

            _iRealCount = realCount;

            return alReturn;
        }

        private ArrayList GetListOfModuleModeViolatorNM(double spec, double spec2, DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;

            Dictionary<string, DataTable> dic = GetDataPerModule(dtData);

            foreach (string key in dic.Keys)
            {
                DataTable dtDataOneModule = dic[key];

                ArrayList alTemp = GetListOfViolatorNM(spec, spec2, dtDataOneModule);

                for (int i = 0; i < alTemp.Count; i++)
                {
                    alReturn.Add(alTemp[i]);
                }

                //_iRealCount의 경우 GetListOfViolatorNM이 돌 때마다 값이 변함.
                realCount += _iRealCount;
            }

            _iRealCount = realCount;

            return alReturn;
        }

        private ArrayList GetListOfModuleModeRisingAndFallingN(DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;

            Dictionary<string, DataTable> dic = GetDataPerModule(dtData);

            foreach (string key in dic.Keys)
            {
                DataTable dtDataOneModule = dic[key];

                ArrayList alTemp = GetListOfRisingAndFallingN(dtDataOneModule);

                for (int i = 0; i < alTemp.Count; i++)
                {
                    alReturn.Add(alTemp[i]);
                }

                //_iRealCount의 경우 GetListOfRisingAndFallingN이 돌 때마다 값이 변함.
                realCount += _iRealCount;
            }

            _iRealCount = realCount;

            return alReturn;
        }

        private ArrayList GetListOfModuleModeInZoneN(double lowThreshold, double highThreshold, DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;

            Dictionary<string, DataTable> dic = GetDataPerModule(dtData);

            foreach (string key in dic.Keys)
            {
                DataTable dtDataOneModule = dic[key];

                ArrayList alTemp = GetListInZoneN(lowThreshold, highThreshold, dtDataOneModule);

                for (int i = 0; i < alTemp.Count; i++)
                {
                    alReturn.Add(alTemp[i]);
                }

                //_iRealCount의 경우 GetListInZoneNM이 돌 때마다 값이 변함.
                realCount += _iRealCount;
            }

            _iRealCount = realCount;

            return alReturn;
        }

        private ArrayList GetListOfModuleModeInZoneNM(double lowThreshold, double highThreshold, DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;

            Dictionary<string, DataTable> dic = GetDataPerModule(dtData);

            foreach (string key in dic.Keys)
            {
                DataTable dtDataOneModule = dic[key];

                ArrayList alTemp = GetListInZoneNM(lowThreshold, highThreshold, dtDataOneModule);

                for (int i = 0; i < alTemp.Count; i++)
                {
                    alReturn.Add(alTemp[i]);
                }

                //_iRealCount의 경우 GetListInZoneNM이 돌 때마다 값이 변함.
                realCount += _iRealCount;
            }

            _iRealCount = realCount;

            return alReturn;
        }

        private ArrayList GetListOfModuleModeTrendViolatorN(bool isLinearRegression, DataTable dtData)
        {
            ArrayList alReturn = new ArrayList();
            int realCount = 0;

            Dictionary<string, DataTable> dic = GetDataPerModule(dtData);

            foreach (string key in dic.Keys)
            {
                DataTable dtDataOneModule = dic[key];

                ArrayList alTemp = GetListOfTrendViolatorN(isLinearRegression, dtDataOneModule);

                for (int i = 0; i < alTemp.Count; i++)
                {
                    alReturn.Add(alTemp[i]);
                }

                //_iRealCount의 경우 GetListInZoneNM이 돌 때마다 값이 변함.
                realCount += _iRealCount;
            }

            _iRealCount = realCount;

            return alReturn;
        }


        private Dictionary<string, DataTable> GetDataPerModule(DataTable dtData)
        {
            Dictionary<string, DataTable> dicReturn = new Dictionary<string, DataTable>();

            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                DataRow dr = dtData.Rows[i];
                string moduleId = dr[Definition.CHART_COLUMN.MODULE_ID].ToString().Split(';')[0];
                double value = 0.0;

                if (!double.TryParse(dr[_sValueType].ToString(), out value))
                {
                    continue;
                }

                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add(Definition.DATA_INDEX);
                dtTemp.Columns.Add(_sValueType);

                if (dicReturn.ContainsKey(moduleId))
                {
                    dtTemp = dicReturn[moduleId];
                }

                DataRow drNew = dtTemp.NewRow();
                drNew[Definition.DATA_INDEX] = i;
                drNew[_sValueType] = value;

                dtTemp.Rows.Add(drNew);

                if (!dicReturn.ContainsKey(moduleId))
                {
                    dicReturn.Add(moduleId, dtTemp);
                }
            }

            return dicReturn;
        }

        /// <summary>
        /// val1이 val2보다 크면 true, 아니면 false 반환
        /// </summary>
        /// <param name="val1">val2를 기준으로 큰 지 알고 싶은 값</param>
        /// <param name="val2">기준이 되는 값</param>
        /// <returns></returns>
        private bool IsBig(double val1, double val2)
        {
            bool bReturn = false;

            if (val1 > val2)
            {
                bReturn = true;
            }

            return bReturn;
        }

        /// <summary>
        /// val1이 val2보다 작으면 true, 아니면 false 반환
        /// </summary>
        /// <param name="value">val2를 기준으로 작은 지 알고 싶은 값</param>
        /// <param name="spec">기준이 되는 값</param>
        /// <returns></returns>
        private bool IsSmall(double val1, double val2)
        {
            bool bReturn = false;

            if (val1 < val2)
            {
                bReturn = true;
            }

            return bReturn;
        }

        private bool IsInZone(double value, double lowThreshold, double highThreshold)
        {
            bool bReturn = false;

            if (value >= lowThreshold && value <= highThreshold)
            {
                bReturn = true;
            }

            return bReturn;
        }

        private double GetTrendValue(ArrayList valueList, bool isLinearRegression)
        {
            double dReturn = 0.0;

            if (valueList.Count > 1)
            {
                if (isLinearRegression)
                {
                    BISTel.PeakPerformance.Statistics.Statistics.LinearRegression lr = new PeakPerformance.Statistics.Statistics.LinearRegression();
                    string[] indexArray = new string[valueList.Count];
                    string[] valueArray = new string[valueList.Count];

                    for (int i = 0; i < valueList.Count; i++)
                    {
                        indexArray[i] = i.ToString();
                        valueArray[i] = valueList[i].ToString();
                    }

                    lr = BISTel.PeakPerformance.Statistics.Statistics.LinearLeastSquaresFitting(indexArray, valueArray);

                    dReturn = (double)lr.Coefficient;
                }
                else
                {
                    for (int i = 1; i < valueList.Count; i++)
                    {
                        double dGap = double.Parse(valueList[i].ToString()) - double.Parse(valueList[i - 1].ToString());

                        dReturn += dGap;
                    }

                    dReturn /= (valueList.Count - 1);
                }
            }

            return dReturn;
        }

        private int GetOriginalDataIndex(DataRow drData, int i)
        {
            int iReturn = 0;

            if (_isModuleMode)
            {
                iReturn = Int32.Parse(drData[Definition.DATA_INDEX].ToString());
            }
            else
            {
                iReturn = i;
            }

            return iReturn;
        }
    }
}
