using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Statistics.Algorithm.Stat;
using BISTel.PeakPerformance.Statistics.Application.Common;

using BISTel.eSPC.Common;

namespace BISTel.eSPC.Page.Common
{
    
    public class CommonSPCStat
    {
        private double _sum2 = 0.0;
        private double _sum = 0.0;
        private double _min = 0.0;
        private double _max = 0.0;
        private double _range = 0.0;
        private double _mean = 0.0;
        private double _sigma = 0.0;
        private double _ppk = 0.0;
        private double _cpk = 0.0;        
        private double _stddev = 0.0;
        private double _pp = 0.0;
        private double _ppu = 0.0;
        private double _ppl = 0.0;
        private double _upper = 0.0;
        private double _lower = 0.0;
        private double _cpkstddev = 0.0;
        private double _cp = 0.0;
        private double _cpu = 0.0;
        private double _cpl = 0.0;
        private double _ca = 0.0;


        public List<double> AddDataList(DataTable dt)
        {
            List<double> listRawData = new List<double>();            
            if(!DataUtil.IsNullOrEmptyDataTable(dt)
                && dt.Columns.Contains(Definition.CHART_COLUMN.RAW))
            {            
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string strRaw = dt.Rows[j][Definition.CHART_COLUMN.RAW].ToString();
                    string[] arr = strRaw.Split('@');
                    string[] arrRaw = arr[0].Split(';');
                    for (int k = 0; k < arrRaw.Length; k++)
                    {
                        if (string.IsNullOrEmpty(arrRaw[k].Trim())) continue;
                        listRawData.Add(double.Parse(arrRaw[k]));
                    }
                }
            }            
            return listRawData;
        }
        
        public void CalcPpk(double[] data, double usl, double lsl)
        {
            int count = data.Length;
            _sum2 = Math.Round(Stat.squaredSum(data), 5);
            _sum = Math.Round(Stat.sum(data), 5);
            _min = Math.Round(Stat.min(data), 5);
            _max = Math.Round(Stat.max(data), 5);
            _mean = Math.Round(_sum / count, 5);

            _range = Math.Round(_max - _min, 5);
            _stddev = Math.Round(Stat.std(data), 5);
            _sigma = Math.Round((_sum2 / count) - (_mean * _mean), 5);
            _upper = Math.Round((usl - _mean) / _sigma, 5);
            _lower = Math.Round((_mean - lsl) / _sigma, 5);
            _cpk = Math.Round(Math.Min(_upper, _lower) / 3, 5);

            /*------------------------------------
             * PP 계산
             *-------------------------------------*/
            _pp = _stddev == 0 ? 0 : Math.Round((usl - lsl) / (6 * _stddev), 2);
            _ppu = _stddev == 0 ? 0 : Math.Round((usl - _mean) / (3 * _stddev), 2);
            _ppl = _stddev == 0 ? 0 : Math.Round((_mean - lsl) / (3 * _stddev), 2);
            //modified by enkim 2010.08.04 Gemini P3-3007
            if (double.IsNaN(_ppl))
                _ppk = _ppu;
            else if (double.IsNaN(_ppu))
                _ppk = _ppl;
            else
                _ppk = Math.Min(_ppu, _ppl);
            //modified end
        }

        public void CalcPpk(double usl, double lsl)
        {           
            /*------------------------------------
             * PP 계산
             *-------------------------------------*/
            if (!Double.IsNaN(usl) && !Double.IsNaN(lsl))
            {
                _pp = _stddev == 0 ? 0 : Math.Round((usl - lsl) / (6 * _stddev), 2);
                _ppu = _stddev == 0 ? 0 : Math.Round((usl - _mean) / (3 * _stddev), 2);
                _ppl = _stddev == 0 ? 0 : Math.Round((_mean - lsl) / (3 * _stddev), 2);
                _ppk = Math.Min(_ppu, _ppl);
            }
            else if (Double.IsNaN(usl) && !Double.IsNaN(lsl))
            {
                _ppl = _stddev == 0 ? 0 : Math.Round((_mean - lsl) / (3 * _stddev), 2);
                _ppk = _ppl;
            }
            else if (!Double.IsNaN(usl) && Double.IsNaN(lsl))
            {
                _ppu = _stddev == 0 ? 0 : Math.Round((usl - _mean) / (3 * _stddev), 2);
                _ppk = _ppu;
            }
            else
            {
                _pp = Double.NaN;
                _ppu = Double.NaN;
                _ppl = Double.NaN;
                _ppk = Double.NaN;
            }
        }

        public void CalcCpk(double usl, double lsl)
        {
            if (!Double.IsNaN(usl) && !Double.IsNaN(lsl))
            {
                _cp = _cpkstddev == 0 ? 0 : Math.Round((usl - lsl) / (6 * _cpkstddev), 2);
                _cpu = _cpkstddev == 0 ? 0 : Math.Round((usl - _mean) / (3 * _cpkstddev), 2);
                _cpl = _cpkstddev == 0 ? 0 : Math.Round((_mean - lsl) / (3 * _cpkstddev), 2);
                _cpk = Math.Min(_cpu, _cpl);
            }
            else if (Double.IsNaN(usl) && !Double.IsNaN(lsl))
            {
                _cpl = _cpkstddev == 0 ? 0 : Math.Round((_mean - lsl) / (3 * _cpkstddev), 2);
                _cpk = _cpl;
            }
            else if (!Double.IsNaN(usl) && Double.IsNaN(lsl))
            {
                _cpu = _cpkstddev == 0 ? 0 : Math.Round((usl - _mean) / (3 * _cpkstddev), 2);
                _cpk = _cpu;
            }
            else
            {
                _cp = Double.NaN;
                _cpu = Double.NaN;
                _cpl = Double.NaN;
                _cpk = Double.NaN;
            }
        }

        public void CalcCa(double usl, double lsl)
        {
            /*------------------------------------
             * Ca 계산
             *-------------------------------------*/
            if (!Double.IsNaN(usl) && !Double.IsNaN(lsl))
            {
                double target = ((usl + lsl) / 2);

                if (_mean >= target)
                {
                    _ca = (_mean - target) / (usl - target);
                }
                else
                {
                    _ca = (target - _mean) / (target - lsl);
                }
            }
        }


        public void CalcMulti(double[] data)
        {
            int count = data.Length;
            _sum2 = Math.Round(Stat.squaredSum(data), 5);
            _sum = Math.Round(Stat.sum(data), 5);
            _min = Math.Round(Stat.min(data), 5);
            _max = Math.Round(Stat.max(data), 5);
            _mean = Math.Round(_sum / count, 5);
            _range = Math.Round(_max - _min, 5);
            _stddev = Math.Round(Stat.std(data), 5);
        }


        public double sum2
        {
            get { return this._sum2; }
            set { this._sum2 = value; }
        }

        public double sum
        {
            get { return this._sum; }
            set { this._sum = value; }
        }


        public double min
        {
            get { return this._min; }
            set { this._min = value; }
        }

        public double max
        {
            get { return this._max; }
            set { this._max = value; }
        }

        public double range
        {
            get { return this._range; }
            set { this._range = value; }
        }

        public double mean
        {
            get { return this._mean; }
            set { this._mean = value; }
        }

        public double ppk
        {
            get { return this._ppk; }
            set { this._ppk = value; }
        }

        public double stddev
        {
            get { return this._stddev; }
            set { this._stddev = value; }
        }

        public double pp
        {
            get { return this._pp; }
            set { this._pp = value; }
        }
        public double ppu
        {
            get { return this._ppu; }
            set { this._ppu = value; }
        }
        public double ppl
        {
            get { return this._ppl; }
            set { this._ppl = value; }
        }

        public double lower
        {
            get { return this._lower; }
            set { this._lower = value; }
        }
        public double upper
        {
            get { return this._upper; }
            set { this._upper = value; }
        }

        public double sigma
        {
            get { return this._sigma; }
            set { this._sigma = value; }
        }

        public double cpkstddev
        {
            get { return this._cpkstddev; }
            set { this._cpkstddev = value; }
        }

        public double cpk
        {
            get { return this._cpk; }
            set { this._cpk = value; }
        }

        public double cp
        {
            get { return this._cp; }
            set { this._cp = value; }
        }
        public double cpu
        {
            get { return this._cpu; }
            set { this._cpu = value; }
        }
        public double cpl
        {
            get { return this._cpl; }
            set { this._cpl = value; }
        }
        public double ca
        {
            get { return this._ca; }
            set { this._ca = value; }
        }
        
        
    }
}
