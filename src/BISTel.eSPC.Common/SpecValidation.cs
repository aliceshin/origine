using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.UserAuthorityHandler;

namespace BISTel.eSPC.Common
{
    public class SpecValidation
    {
        public bool ValidateData(string sMinor, string sMajor)
        {
            bool bSuccess = true;

            bool bMinorResult = true;
            bool bMajorResult = true;

            Decimal dcMinor = 0;
            Decimal dcMajor = 0;

            try
            {
                if (sMinor.Length > 0)
                {
                    string[] saMinor = sMinor.ToUpper().Split('E');

                    try
                    {
                        if (saMinor.Length >= 2)
                        {
                            double dobMinor = Convert.ToDouble(sMinor);
                            dcMinor = Convert.ToDecimal(dobMinor);
                        }
                        else
                        {
                            dcMinor = Convert.ToDecimal(sMinor);
                        }
                    }
                    catch
                    {
                        bMinorResult = false;
                    }
                }

                if (sMajor.Length > 0)
                {
                    string[] saMajor = sMajor.ToUpper().Split('E');

                    try
                    {
                        if (saMajor.Length >= 2)
                        {
                            double dobMajor = Convert.ToDouble(sMajor);
                            dcMajor = Convert.ToDecimal(dobMajor);
                        }
                        else
                        {
                            dcMajor = Convert.ToDecimal(sMajor);
                        }
                    }
                    catch
                    {
                        bMajorResult = false;
                    }
                }

                if (bMinorResult & bMajorResult)
                {
                    int i = Decimal.Compare(dcMinor, dcMajor);
                    if (i > 0)
                        bSuccess = false;
                }
                else
                {
                    if (sMinor != sMajor)
                        bSuccess = false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception();
            }

            return bSuccess;
        }
    }
}
