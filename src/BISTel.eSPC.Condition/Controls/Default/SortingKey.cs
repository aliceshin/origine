using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.BaseUserCtrl.DynamicCondition;
using BISTel.PeakPerformance.Client.DataHandler;

using BISTel.eSPC.Common;

namespace BISTel.eSPC.Condition.Controls.Default
{
    public partial class SortingKey : UserControl
    {

        private eSPCWebService.eSPCWebService _wsSPC = null;
        private MultiLanguageHandler _mlthandler;
        private Initialization _Initialization;

        public SortingKey()
        {
            this._wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();
            this._mlthandler = MultiLanguageHandler.getInstance();
            InitializeComponent();

        }


        #region Properties
        public ListBox listBoxSKeySelected
        {
            get
            {
                return this.lstSKeyUnSelected;
            }
        }

        public ListBox listBoxSKeyUnSelected
        {
            get
            {
                return this.lstSKeySelected;
            }
        }

        public string[] SelectedItems
        {
            get
            {
                string[] arr = new string[this.lstSKeySelected.Items.Count];
                for (int i = 0; i < this.lstSKeySelected.Items.Count; i++)
                {
                    arr[i] = this.lstSKeySelected.Items[i].ToString();
                }
                return arr;
            }
        }


        #endregion



        #region User Define



        public void AddItemsSelected(DataTable dt, string colName)
        {
            if (!DataUtil.IsNullOrEmptyDataTable(dt))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    this.lstSKeyUnSelected.Items.Remove(dr[colName].ToString());
                }
                AddItems(dt, this.lstSKeySelected, this.lstSKeyUnSelected, colName);
            }
        }

        public void AddItemsUnSelected(DataTable dt, string colName)
        {
            AddItems(dt, this.lstSKeyUnSelected, this.lstSKeySelected, colName);
        }

        public void AddItems(DataTable dt, ListBox _lst, ListBox _lstTarget, string colName)
        {
            _lst.Items.Clear();
            if (!DataUtil.IsNullOrEmptyDataTable(dt))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sKey = dt.Rows[i][colName].ToString();
                    if (!_lstTarget.Items.Contains(sKey))
                        _lst.Items.Add(sKey);
                }
            }
        }

        #endregion

        #region Event
        #region Sorting Key Event

        private void lstSKeySelected_DoubleClick(object sender, System.EventArgs e)
        {
            ListBox lst = sender as ListBox;

            for (int i = lst.SelectedItems.Count - 1; i >= 0; i--)
            {
                if (!this.lstSKeyUnSelected.Items.Contains(lst.SelectedItems[i].ToString()))
                {
                    this.lstSKeyUnSelected.Items.Add(lst.SelectedItems[i].ToString());
                    lst.Items.Remove(lst.SelectedItems[i].ToString());
                }
            }



        }

        private void lstSKeyUnSelected_DoubleClick(object sender, System.EventArgs e)
        {
            ListBox lst = sender as ListBox;
            for (int i = lst.SelectedItems.Count - 1; i >= 0; i--)
            {
                if (!this.lstSKeySelected.Items.Contains(lst.SelectedItems[i].ToString()))
                {
                    this.lstSKeySelected.Items.Add(lst.SelectedItems[i].ToString());
                    lst.Items.Remove(lst.SelectedItems[i].ToString());
                }
            }
        }


        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lstSKeyUnSelected.SelectedItems == null || this.lstSKeyUnSelected.SelectedItems.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("DCP_NO_SELECT"));
                    return;
                }

                for (int i = 0; i < this.lstSKeyUnSelected.SelectedItems.Count; i++)
                {
                    if (!this.lstSKeySelected.Items.Contains(this.lstSKeyUnSelected.SelectedItems[i].ToString()))
                        this.lstSKeySelected.Items.Add(this.lstSKeyUnSelected.SelectedItems[i].ToString());
                }

                for (int i = this.lstSKeyUnSelected.SelectedItems.Count - 1; i >= 0; i--)
                    this.lstSKeyUnSelected.Items.Remove(this.lstSKeyUnSelected.SelectedItems[i].ToString());

            }
            catch
            {
            }
            finally
            {

            }
        }




        private void btnUnSelect_Click(object sender, EventArgs e)
        {


            try
            {

                if (this.lstSKeySelected.SelectedItems == null || this.lstSKeySelected.SelectedItems.Count == 0)
                {
                    MSGHandler.DisplayMessage(MSGType.Warning, MSGHandler.GetMessage("DCP_NO_SELECT"));
                    return;
                }

                for (int i = 0; i < this.lstSKeySelected.SelectedItems.Count; i++)
                {
                    if (!this.lstSKeyUnSelected.Items.Contains(this.lstSKeySelected.SelectedItems[i].ToString()))
                        this.lstSKeyUnSelected.Items.Add(this.lstSKeySelected.SelectedItems[i].ToString());
                }

                for (int i = this.lstSKeySelected.SelectedItems.Count - 1; i >= 0; i--)
                {
                    this.lstSKeySelected.Items.Remove(this.lstSKeySelected.SelectedItems[i].ToString());
                }
            }
            catch
            {
            }
            finally
            {

            }

        }
        #endregion




        private void btnPlus_Click(object sender, EventArgs e)
        {
            ListBox lstCopy = new ListBox();
            try
            {
                lstCopy = this.lstSKeySelected;
                int iSelectIndex = this.lstSKeySelected.SelectedIndex;

                for (int i = 0; i < lstCopy.Items.Count; i++)
                {
                    string sValue = lstCopy.Items[i].ToString();
                    if (this.lstSKeySelected.SelectedItems.Contains(sValue))
                    {
                        for (int k = 0; k < this.lstSKeySelected.SelectedIndices.Count; k++)
                        {
                            int idx = this.lstSKeySelected.SelectedIndices[k] + 1;
                            if (this.lstSKeySelected.SelectedIndices.Contains(idx))
                                continue;
                            if (lstCopy.Items.Count > idx)
                            {
                                this.lstSKeySelected.Items.Insert(i, this.lstSKeySelected.Items[idx].ToString());

                                i = idx + 1;
                                this.lstSKeySelected.Items.RemoveAt(i);
                            }
                        }
                    }

                }
                

            }
            catch (System.Web.Services.Protocols.SoapException sex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, sex.Message);
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            ListBox lstCopy = new ListBox();
            try
            {
                lstCopy = this.lstSKeySelected;
                for (int i = lstCopy.Items.Count - 1; i >= 0; i--)
                {
                    string sValue = lstCopy.Items[i].ToString();
                    if (this.lstSKeySelected.SelectedItems.Contains(sValue))
                    {
                        for (int k = this.lstSKeySelected.SelectedIndices.Count - 1; k >= 0; k--)
                        {
                            int idx = this.lstSKeySelected.SelectedIndices[k] - 1;
                            if (this.lstSKeySelected.SelectedIndices.Contains(idx))
                                continue;
                            if (0 <= idx)
                            {
                                this.lstSKeySelected.Items.Insert(i + 1, this.lstSKeySelected.Items[idx].ToString());
                                this.lstSKeySelected.Items.RemoveAt(idx);
                                i = idx;
                            }
                        }
                    }

                }

            }
            catch (System.Web.Services.Protocols.SoapException sex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, sex.Message);
            }
            catch (Exception ex)
            {
                MSGHandler.DisplayMessage(MSGType.Error, ex.Message);
            }
        }

        #endregion




    }
}
