using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using FarPoint.Win.Spread;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Common.ATT
{
    public delegate void DelegateClosePopup(System.Windows.Forms.DialogResult popupResult);

    public interface IPopupContents
    {
        event DelegateClosePopup ClosePopup;
    }

    public enum VariableDataType
    {
        Interval,
        Nominal
    }


    public class SourceDataManager : BISTel.eSPC.Common.SourceDataManager
    {
    }
}
