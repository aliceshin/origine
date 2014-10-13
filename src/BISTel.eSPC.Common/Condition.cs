
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using System.Data;

using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;

using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Common
{
    public class Condition
    {
        #region : Condition


        public Hashtable GetComboMember(string sDisplayField, string sValueField)
        {
            Hashtable ht = new Hashtable();

            string sDisplayMember = "";
            string sValueMember = "";
            if (sDisplayField != "")
            {
                Hashtable htdisplayAttributes = (Hashtable)Initialization._htFieldName[sDisplayField];
                sDisplayMember = htdisplayAttributes[Definition.VARIABLE_FIELDNAME].ToString();
            }
            else
                sDisplayMember = sDisplayField;

            if (sValueField != "")
            {
                Hashtable htValueAttributes = (Hashtable)Initialization._htFieldName[sValueField];
                sValueMember = htValueAttributes[Definition.VARIABLE_FIELDNAME].ToString();
            }
            else
                sValueMember = sDisplayMember;

            ht.Add(Definition.VARIABLE_COMBO_DISPLAY, sDisplayMember);
            ht.Add(Definition.VARIABLE_COMBO_VALUE, sValueMember);

            return ht;
        }


        public void BindToControl(Control control, DataSet ds, string sDisplayMember, string sValueMumber, bool bAll)
        {
            try
            {
                if (ds.Tables.Count > 0)
                {
                    if (control is BISTel.PeakPerformance.Client.BISTelControl.BComboBox)
                    {
                        BISTel.PeakPerformance.Client.BISTelControl.BComboBox bComboBox = (BISTel.PeakPerformance.Client.BISTelControl.BComboBox)control;

                        bComboBox.Items.Clear();

                        //if (bAll)
                        //    combo.Items.Add(Definition.VARIABLE_ALL); //ALL
                        //else
                        //    combo.Items.Remove(Definition.VARIABLE_ALL);                   

                        bComboBox.BeginUpdate();

                        if (sDisplayMember != "")
                            bComboBox.DisplayMember = sDisplayMember;

                        if (sValueMumber != "")
                            bComboBox.ValueMember = sValueMumber;


                        if (DSUtil.GetResultSucceed(ds) != 0)
                        {
                            bComboBox.DataSource = ds.Tables[0];

                            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            //{
                            //    bComboBox.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                            //}
                            bComboBox.EndUpdate();

                            if (bComboBox.Items.Count > 0)
                            {
                                bComboBox.SelectedIndex = 0;
                            }
                        }
                        else
                            bComboBox.EndUpdate();
                    }
                    else if (control is BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)
                    {
                        BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo bCheckComboBox = (BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)control;

                        bCheckComboBox.chkBox.Items.Clear();

                        bCheckComboBox.chkBox.BeginUpdate();

                        if (DSUtil.GetResultSucceed(ds) != 0)
                        {
                            ArrayList alItemList = new ArrayList();
                            string[] saItemList = new string[ds.Tables[0].Rows.Count];
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                alItemList.Add(ds.Tables[0].Rows[i][sDisplayMember].ToString());
                                saItemList[i] = ds.Tables[0].Rows[i][sDisplayMember].ToString();
                            }

                            if (sDisplayMember != "")
                                bCheckComboBox.DisplayMember = sDisplayMember;

                            if (sValueMumber != "")
                                bCheckComboBox.ValueMember = sValueMumber;

                            bCheckComboBox.DataSource = ds.Tables[0];

                            //string[] saItemList = (string[])alItemList.ToArray(typeof(string[]));

                            //bCheckComboBox.AddItems(saItemList);

                            bCheckComboBox.chkBox.EndUpdate();
                            bCheckComboBox.chkBox.SelectedIndex = 0;
                        }
                        else
                            bCheckComboBox.chkBox.EndUpdate();
                    }
                    else if (control is System.Windows.Forms.ListBox)
                    {
                        System.Windows.Forms.ListBox lstbox = (System.Windows.Forms.ListBox)control;

                        lstbox.Items.Clear();

                        lstbox.BeginUpdate();

                        if (DSUtil.GetResultSucceed(ds) != 0)
                        {
                            ArrayList alItemList = new ArrayList();
                            string[] saItemList = new string[ds.Tables[0].Rows.Count];
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                alItemList.Add(ds.Tables[0].Rows[i][sDisplayMember].ToString());
                                saItemList[i] = ds.Tables[0].Rows[i][sDisplayMember].ToString();
                            }

                            ////string[] saItemList = (string[])alItemList.ToArray(typeof(string[]));

                            //lstbox.Items.AddRange(saItemList);
                            ////lstbox.DataSource = ds;


                            if (sDisplayMember != "")
                                lstbox.DisplayMember = sDisplayMember;

                            if (sValueMumber != "")
                                lstbox.ValueMember = sValueMumber;

                            lstbox.DataSource = ds.Tables[0];
                            lstbox.EndUpdate();
                        }
                        else
                            lstbox.EndUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RefreshConditionItems(ArrayList asControlTypes, Control control)
        {
            int iIndex = asControlTypes.IndexOf(control);

            for (int i = iIndex + 1; i < asControlTypes.Count; i++)
            {
                if (asControlTypes[i] is BISTel.PeakPerformance.Client.BISTelControl.BComboBox)
                {
                    ((BISTel.PeakPerformance.Client.BISTelControl.BComboBox)asControlTypes[i]).DataSource = null;//.Items.Clear();

                    if (((BISTel.PeakPerformance.Client.BISTelControl.BComboBox)asControlTypes[i]).Items.Count > 0)
                        ((BISTel.PeakPerformance.Client.BISTelControl.BComboBox)asControlTypes[i]).Items.Clear();

                }
                else if (asControlTypes[i] is BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)
                {
                    ((BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)asControlTypes[i]).chkBox.Items.Clear();
                }
                else if (asControlTypes[i] is System.Windows.Forms.ListBox)
                {
                    ((System.Windows.Forms.ListBox)asControlTypes[i]).Items.Clear();
                }
            }
        }

        public void RefreshConditionText(ArrayList asControlTypes, Control control, bool bAll)
        {
            int iIndex = asControlTypes.IndexOf(control);

            for (int i = iIndex + 1; i < asControlTypes.Count; i++)
            {
                if (asControlTypes[i] is BISTel.PeakPerformance.Client.BISTelControl.BComboBox)
                {
                    if (((BISTel.PeakPerformance.Client.BISTelControl.BComboBox)asControlTypes[i]).Items.Count == 0)
                    {
                        if (bAll)
                        {
                            ((BISTel.PeakPerformance.Client.BISTelControl.BComboBox)asControlTypes[i]).Items.Add(Definition.VARIABLE_ALL);
                            ((BISTel.PeakPerformance.Client.BISTelControl.BComboBox)asControlTypes[i]).Text = Definition.VARIABLE_ALL;
                        }
                        else
                        {
                            ((BISTel.PeakPerformance.Client.BISTelControl.BComboBox)asControlTypes[i]).Text = "";
                        }
                    }
                }
                else if (asControlTypes[i] is BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)
                {
                    if (((BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)asControlTypes[i]).chkBox.Items.Count == 0)
                    {
                        if (bAll)
                        {
                            ((BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)asControlTypes[i]).chkBox.Items.Add(Definition.VARIABLE_ALL);
                            ((BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)asControlTypes[i]).chkBox.Items[0].Equals(Definition.VARIABLE_ALL);
                            ((BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)asControlTypes[i]).txtBox.Text = "";

                            //if (((BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)asControlTypes[i]).chkBox.Items.Count > 0)
                            //{
                            //    ((BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)asControlTypes[i]).chkBox.SetItemCheckState(0, CheckState.Checked);
                            //}
                        }
                        else
                        {
                            ((BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)asControlTypes[i]).txtBox.Text = "";
                        }
                    }
                }
            }
        }

        public bool IsEmptyComboData(Control control)
        {
            if (control is BISTel.PeakPerformance.Client.BISTelControl.BComboBox)
            {
                if (((BISTel.PeakPerformance.Client.BISTelControl.BComboBox)control).Items.Count == 0)
                    return true;
                else if (((BISTel.PeakPerformance.Client.BISTelControl.BComboBox)control).Items.Count == 1)
                {
                    if (((BISTel.PeakPerformance.Client.BISTelControl.BComboBox)control).Items[0].ToString() == Definition.VARIABLE_ALL)
                        return true;
                }
            }
            else if (control is BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)
            {
                if (((BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)control).chkBox.Items.Count == 0)
                    return true;
                else if (((BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)control).chkBox.Items.Count == 1)
                {
                    if (((BISTel.PeakPerformance.Client.BISTelControl.BCheckCombo)control).chkBox.Items[0].ToString() == Definition.VARIABLE_ALL)
                        return true;
                }
            }
            else if (control is System.Windows.Forms.ListBox)
            {
                if (((System.Windows.Forms.ListBox)control).Items.Count == 0)
                    return true;
                else if (((System.Windows.Forms.ListBox)control).Items.Count == 1)
                {
                    if (((System.Windows.Forms.ListBox)control).Items[0].ToString() == Definition.VARIABLE_ALL)
                        return true;
                }
            }

            return false;
        }

        #endregion
    }
}
