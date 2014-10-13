using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using System.Collections;

namespace BISTel.eSPC.Page.Report.Popup
{
    public partial class SeriesGroupingAndFilteringPopup : BasePopupFrm
    {
        List<ContextGroupingAndFiltering> contexts = new List<ContextGroupingAndFiltering>();
        private string[] contextNames = null;
        ArrayList _arrPreGroupingSelected;
        private ArrayList _arrPreSeletedValues;

        public string[] ContextNames
        {
            get { return contextNames; }
        }

        public Dictionary<string, string[]> GetGroupingAndFilteringValues
        {
            get 
            {
                var result = new Dictionary<string, string[]>();
                foreach(var context in contexts)
                {
                    if(context.IsGroupingSelected)
                    {
                        result.Add(context.ContextName, context.SelectedValues);
                    }
                }

                return result;
            }
        }

        public SeriesGroupingAndFilteringPopup()
        {
            InitializeComponent();
        }

        public SeriesGroupingAndFilteringPopup(DataTable dataTable, string[] contextNames)
        {
            InitializeComponent();

            this._arrPreGroupingSelected = new ArrayList();
            this._arrPreSeletedValues = new ArrayList();

            for (int i = 0; i < contextNames.Length; i++)
            {
                var context = new ContextGroupingAndFiltering(dataTable, contextNames[i]);
                if (contextNames[i] == BISTel.eSPC.Common.Definition.CHART_COLUMN.MODULE_ID)
                {
                    context.Width = 450;
                }
                contexts.Add(context);
                

                this.flplColumns.Controls.Add(context);
            }

            this.contextNames = contextNames;
            this.SetFilterPreviousInfo();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.SetFilterPreviousInfo();
            this.Close();
        }

        private void SetFilterPreviousInfo()
        {
            this._arrPreGroupingSelected = new ArrayList();
            this._arrPreSeletedValues = new ArrayList();

            for (int i = 0; i < this.contextNames.Length; i++)
            {
                _arrPreGroupingSelected.Add(((ContextGroupingAndFiltering)this.flplColumns.Controls[i]).IsGroupingSelected);
                _arrPreSeletedValues.Add(((ContextGroupingAndFiltering)this.flplColumns.Controls[i]).SelectedValues);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            for (int i = 0; i < this.contextNames.Length; i++)
            {
                ((ContextGroupingAndFiltering)this.flplColumns.Controls[i]).IsGroupingSelected = (bool)this._arrPreGroupingSelected[i];
                ((ContextGroupingAndFiltering)this.flplColumns.Controls[i]).SelectedValues = this._arrPreSeletedValues[i] as string[];
            }

            this.Close();
        }
    }
}
