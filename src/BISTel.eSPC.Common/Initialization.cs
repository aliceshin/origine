using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

using BISTel.PeakPerformance.Client.BaseUserCtrl;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.PeakPerformance.Client.UserAuthorityHandler;

using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos;
using BISTel.PeakPerformance.Client.BISTelControl.BTeeCharts.SeriesInfos.Additional;

namespace BISTel.eSPC.Common
{
    public enum ButtonListType
    {
        Spread = 0
        , Chart = 1
    }
    public class Initialization
    {
        #region : FIELD

        private static string _sWebPath;
        private static string _sConfigurationPath;

        private static XmlDocument _xdocFieldDefinition;
        private static XmlDocument _xdocConfiguration;

        public static Hashtable _htFieldName;

        MultiLanguageHandler _mlthandler;

        private SortedList _slDefaultVisiableList;

        BSpreadUtility _bspreadutility;


        private const String xdocSelectNodes_Buttons = "/Settings/page/Buttons";
        private const String xdocSelectNodes_ChartButtons = "/Settings/page/ChartButtons";
        private const String xdocSelectNodes_ChartSeries = "/Settings/page/ChartSeries";
        private const String xdocSelectNodes_ContextMenu = "/Settings/page/ContextMenu";
        private const String xdocSelectNodes_SearchConfig = "/Settings/page/searchconfig";
        private const String xdocSelectNodes_OcapVisible = "/Settings/page/ocapvisible";
        private const String xdocSelectNodes_PpkConfig = "/Settings/page/ppkconfig";
        // Set for Displaying Check Box of Normalization in Configuration UI - 2011.08.29 by ANDREW KO
        private const String xdocSelectNodes_ContextConfig = "/Settings/page/ContextConfig";

        #endregion

        #region : Initialize

        public String GetApplicationXmlFile(String PageKey)
        {
            String path = String.Format("{0}/{1}/{2}/{3}.xml", _sWebPath, Definition.PATH_APPLICATION, Definition.PATH_ESPC, PageKey);

            return path;
        }

        public void InitializePath()
        {
            try
            {
                this._mlthandler = MultiLanguageHandler.getInstance();

                if (_sWebPath == "" || _sWebPath == null)
                    _sWebPath = Configuration.getInstance().URL;

                if (_sConfigurationPath == "" || _sConfigurationPath == null)
                    _sConfigurationPath = _sWebPath + Definition.PATH_XML_CONFIGURATION;

                if (_xdocConfiguration == null)
                {
                    _xdocConfiguration = new XmlDocument();
                    _xdocConfiguration.Load(_sConfigurationPath);
                }

                if (_xdocFieldDefinition == null)
                {
                    _xdocFieldDefinition = new XmlDocument();

                    if (_htFieldName == null)
                    {
                        _htFieldName = new Hashtable();
                    }
                }

                this._bspreadutility = new BSpreadUtility();
            }
            catch
            {
            }
            finally
            {
            }
        }

        public ArrayList GetSearchPeriod(string PageKey)
        {
            ArrayList arrSearchPeriod = new ArrayList();
            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey);
                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);

                XmlNode xmlNodeConfig = null;
                xmlNodeConfig = xdocPageStyle.SelectSingleNode(xdocSelectNodes_SearchConfig);

                string sType = xmlNodeConfig[Definition.XML_VARIABLE_Type].InnerText;
                string sValue = xmlNodeConfig[Definition.XML_VARIABLE_Value].InnerText;

                if (sType.Length > 0)
                {
                    arrSearchPeriod.Add(sType);
                }
                if (sValue.Length > 0)
                {
                    arrSearchPeriod.Add(sValue);
                }
            }
            catch
            {
            }
            return arrSearchPeriod;
        }

        public string GetSearchConfig(string PageKey)
        {
            string strReturn = string.Empty;
            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey);
                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);

                XmlNode xmlNodeConfig = null;
                xmlNodeConfig = xdocPageStyle.SelectSingleNode(xdocSelectNodes_OcapVisible);

                string sValue = xmlNodeConfig[Definition.XML_VARIABLE_Value].InnerText;

                if (sValue.Length > 0)
                {
                    strReturn = sValue;
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
            return strReturn;
        }

        public bool GetExponentialAxis(string PageKey)
        {
            bool bExpResult = false;
            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey);
                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);

                XmlNode xmlNodeConfig = null;
                xmlNodeConfig = xdocPageStyle.SelectSingleNode(xdocSelectNodes_SearchConfig);

                string sExponentialAxis = xmlNodeConfig[Definition.XML_VARIABLE_ExponentialAxis].InnerText;

                bExpResult = Convert.ToBoolean(sExponentialAxis);
            }
            catch
            {
                return false;
            }
            return bExpResult;
        }

        public bool GetChartModeGrouping(string PageKey)
        {
            bool bExpResult = false;
            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey);
                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);

                XmlNode xmlNodeConfig = null;
                xmlNodeConfig = xdocPageStyle.SelectSingleNode(xdocSelectNodes_SearchConfig);

                string sExponentialAxis = xmlNodeConfig[Definition.XML_VARIABLE_ChartModeGrouping].InnerText;

                bExpResult = Convert.ToBoolean(sExponentialAxis);
            }
            catch
            {
                return false;
            }
            return bExpResult;
        }

        public bool GetPpkShowOOC(string PageKey)
        {
            bool bExpResult = false;
            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey);
                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);

                XmlNode xmlNodeConfig = null;
                xmlNodeConfig = xdocPageStyle.SelectSingleNode(xdocSelectNodes_PpkConfig);

                string sShowOOCCount = xmlNodeConfig[Definition.XML_VARIABLE_ShowOOCCount].InnerText;

                bExpResult = Convert.ToBoolean(sShowOOCCount);
            }
            catch
            {
                return false;
            }
            return bExpResult;
        }

        public bool GetPpkShowCpk(string PageKey)
        {
            bool bExpResult = false;
            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey);
                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);

                XmlNode xmlNodeConfig = null;
                xmlNodeConfig = xdocPageStyle.SelectSingleNode(xdocSelectNodes_PpkConfig);

                string sShowOOCCount = xmlNodeConfig[Definition.XML_VARIABLE_ShowCPK].InnerText;

                bExpResult = Convert.ToBoolean(sShowOOCCount);
            }
            catch
            {
                return false;
            }
            return bExpResult;
        }
        // JIRA SPC-592 [GSMC] Disable "Use Normalization Value" - 2011.08.29 by ANDREW KO
        public bool GetContextShowNormalization(string PageKey)
        {
            bool bExpResult = false;
            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey);
                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);

                XmlNode xmlNodeConfig = null;
                xmlNodeConfig = xdocPageStyle.SelectSingleNode(xdocSelectNodes_ContextConfig);

                string sShowNormalization = xmlNodeConfig[Definition.XML_VARIABLE_ShowNormalization].InnerText;

                bExpResult = Convert.ToBoolean(sShowNormalization);
            }
            catch
            {
                return false;
            }
            return bExpResult;
        }
        // JIRA No.SPC-600 Change unit of Minimum Samples to flexable unit (like Lot, Wafer...) - 2011.09.10 by ANDREW KO
        // No.5 on GF Communication Sheet 20110612 V1.21 - 2011.09.10 by ANDREW KO
        public string GetUnitOfSamples(string PageKey)
        {
            string strReturn = string.Empty;
            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey);
                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);

                XmlNode xmlNodeConfig = null;
                xmlNodeConfig = xdocPageStyle.SelectSingleNode(xdocSelectNodes_ContextConfig);

                string sValue = xmlNodeConfig[Definition.XML_VARIABLE_SetUnitOfSamples].InnerText;

                if (sValue.Length > 0)
                {
                    strReturn = sValue;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
            return strReturn;
        }

        //SPC-736 OCAP action(option) - 2012.02.10 by louis you
        public string GetOCAPOfSingle(string PageKey)
        {
            string strReturn = string.Empty;
            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey);
                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);

                XmlNode xmlNodeConfig = null;
                xmlNodeConfig = xdocPageStyle.SelectSingleNode(xdocSelectNodes_ContextConfig);

                string sValue = xmlNodeConfig[Definition.XML_VARIABLE_OCAPSelectOfSingle].InnerText;

                if (sValue.Length > 0)
                {
                    strReturn = sValue;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
            return strReturn;
        }

        #endregion



        #region : BUTTON :: InitializeButtonList

        public void AddSortedListButtonList(SortedList slButtonListIndex, BButtonList bbtnlist, String strXMLPath, string ItemKey, ButtonListType btnListType)
        {
            XmlDocument xdocPageStyle = new XmlDocument();
            xdocPageStyle.Load(strXMLPath);

            XmlNodeList xnListButtons = null;

            if (btnListType == ButtonListType.Spread)
                xnListButtons = xdocPageStyle.SelectNodes(xdocSelectNodes_Buttons);
            else
                xnListButtons = xdocPageStyle.SelectNodes(xdocSelectNodes_ChartButtons);

            Hashtable htButtonList = new Hashtable();
            ItemKey = ItemKey.Trim();
            foreach (XmlNode NodeButtons in xnListButtons)
            {
                string sKey = "";
                if (NodeButtons.Attributes[Definition.XML_VARIABLE_KEY] != null)
                {
                    sKey = NodeButtons.Attributes[Definition.XML_VARIABLE_KEY].Value;
                }

                if (sKey == ItemKey)
                {
                    foreach (XmlNode NodeButton in NodeButtons)
                    {
                        string sIndex = NodeButton.Attributes[Definition.XML_VARIABLE_INDEX].Value;
                        Hashtable htButtonAttributes = new Hashtable();

                        string sButtonKey = NodeButton[Definition.XML_VARIABLE_BUTTONKEY].InnerText;
                        string sButtonVisible = NodeButton[Definition.XML_VARIABLE_BUTTONVISIBLE].InnerText;
                        string sUserContextMenu = NodeButton[Definition.XML_VARIABLE_USERCONTEXTMENU].InnerText;

                        htButtonAttributes.Add(Definition.XML_VARIABLE_BUTTONKEY, sButtonKey);
                        htButtonAttributes.Add(Definition.XML_VARIABLE_BUTTONVISIBLE, sButtonVisible);
                        htButtonAttributes.Add(Definition.XML_VARIABLE_USERCONTEXTMENU, sUserContextMenu);

                        htButtonList.Add(sIndex, htButtonAttributes);

                        slButtonListIndex.Add(sIndex, sButtonKey);
                    }
                }
            }


            bbtnlist.Items.Clear();
            for (int i = 0; i < htButtonList.Count; i++)
            {
                string sIndex = i.ToString();
                Hashtable htButtonAttributes = (Hashtable)htButtonList[sIndex];
                string sButtonName = htButtonAttributes[Definition.XML_VARIABLE_BUTTONKEY].ToString();
                string Visible = htButtonAttributes[Definition.XML_VARIABLE_BUTTONVISIBLE].ToString().ToUpper();

                bool IsContextMenu = Convert.ToBoolean(htButtonAttributes[Definition.XML_VARIABLE_USERCONTEXTMENU]);

                if (Visible == Definition.VARIABLE_TRUE.ToUpper())
                    bbtnlist.Items.Add(sButtonName, IsContextMenu);

            }
            bbtnlist.Show();
            //return slButtonListIndex;                                             
        }

        public SortedList InitializeButtonList(BButtonList bbtnlist, ref BSpread bsprData, string PageKey, string ItemKey, SessionData sessionData)
        {
            SortedList slButtonListIndex = null;
            try
            {
                slButtonListIndex = new SortedList();
                string strXMLPath = GetApplicationXmlFile(PageKey); ;
                AddSortedListButtonList(slButtonListIndex, bbtnlist, strXMLPath, ItemKey, ButtonListType.Spread);

                bsprData.ContextMenu = bbtnlist.ContextMenu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return slButtonListIndex;
        }

        public SortedList InitializeButtonList(BButtonList bbtnlist, ref BSpread bsprData, string PageKey, string ItemKey, SessionData sessionData, string sPath)
        {
            SortedList slButtonListIndex = null;
            try
            {
                slButtonListIndex = new SortedList();
                AddSortedListButtonList(slButtonListIndex, bbtnlist, sPath, ItemKey, ButtonListType.Spread);
                bsprData.ContextMenu = bbtnlist.ContextMenu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return slButtonListIndex;
        }

        public SortedList InitializeButtonList(BButtonList bbtnlist, string PageKey, string ItemKey, SessionData sessionData)
        {
            SortedList slButtonListIndex = null;
            try
            {
                slButtonListIndex = new SortedList();
                string strXMLPath = GetApplicationXmlFile(PageKey);
                AddSortedListButtonList(slButtonListIndex, bbtnlist, strXMLPath, ItemKey, ButtonListType.Spread);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return slButtonListIndex;
        }

        public SortedList InitializeChartButtonList(BButtonList bbtnlist, string PageKey, string ItemKey, SessionData sessionData)
        {
            SortedList slButtonListIndex = null;
            try
            {
                slButtonListIndex = new SortedList();
                string strXMLPath = GetApplicationXmlFile(PageKey); ;
                AddSortedListButtonList(slButtonListIndex, bbtnlist, strXMLPath, ItemKey, ButtonListType.Chart);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return slButtonListIndex;
        }



        public Hashtable InitializeChartSeriesList(string PageKey, string ItemKey, SessionData sessionData)
        {
            Hashtable htButtonList = null;
            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey); ;

                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);

                XmlNodeList xnListButtons = xdocPageStyle.SelectNodes(xdocSelectNodes_ChartSeries);

                htButtonList = new Hashtable();
                ItemKey = ItemKey.Trim();
                foreach (XmlNode NodeButtons in xnListButtons)
                {
                    string sKey = "";
                    if (NodeButtons.Attributes[Definition.XML_VARIABLE_KEY] != null)
                    {
                        sKey = NodeButtons.Attributes[Definition.XML_VARIABLE_KEY].Value;
                    }

                    if (sKey == ItemKey)
                    {
                        foreach (XmlNode NodeButton in NodeButtons)
                        {
                            string sIndex = NodeButton.Attributes[Definition.XML_VARIABLE_INDEX].Value;
                            Hashtable htButtonAttributes = new Hashtable();

                            string sButtonKey = NodeButton[Definition.XML_VARIABLE_BUTTONKEY].InnerText;
                            string sButtonVisible = NodeButton[Definition.XML_VARIABLE_BUTTONVISIBLE].InnerText;
                            string sDefaultValue = NodeButton[Definition.XML_VARIABLE_DEFAULTVALUE].InnerText;

                            htButtonAttributes.Add(Definition.XML_VARIABLE_BUTTONKEY, sButtonKey);
                            htButtonAttributes.Add(Definition.XML_VARIABLE_BUTTONVISIBLE, sButtonVisible);
                            htButtonAttributes.Add(Definition.XML_VARIABLE_DEFAULTVALUE, sDefaultValue);
                            htButtonList.Add(sIndex, htButtonAttributes);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return htButtonList;
        }



        #endregion

        #region : CONTEXT MENU

        public SortedList InitializeContextMenuList(string PageKey)
        {
            SortedList slButtonListIndex = null;
            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey);
                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);
                XmlNodeList xnListButtons = xdocPageStyle.SelectNodes(xdocSelectNodes_ContextMenu);
                Hashtable htButtonList = new Hashtable();
                int iIndex = 0;
                foreach (XmlNode NodeButtons in xnListButtons)
                {
                    foreach (XmlNode NodeButton in NodeButtons)
                    {

                        string sIndex = NodeButton.Attributes[Definition.XML_VARIABLE_INDEX].Value;
                        Hashtable htButtonAttributes = new Hashtable();

                        string sContextKey = NodeButton[Definition.XML_VARIABLE_CONTEXTKEY].InnerText;
                        string sContextVisible = NodeButton[Definition.XML_VARIABLE_CONTEXTVISIBLE].InnerText;

                        if (sContextVisible.ToUpper() == Definition.VARIABLE_TRUE)
                        {
                            string sContextMenu = this._mlthandler.GetVariable(sContextKey);
                            slButtonListIndex.Add(iIndex.ToString(), sContextMenu);
                            iIndex++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }

            return slButtonListIndex;
        }

        public SortedList InitializeContextMenuList(string PageKey, string sItemKey)
        {
            SortedList slButtonListIndex = new SortedList();

            try
            {

                string strXMLPath = GetApplicationXmlFile(PageKey);

                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);
                XmlNodeList xnListButtons = xdocPageStyle.SelectNodes(xdocSelectNodes_ContextMenu);
                sItemKey = sItemKey.Trim();


                Hashtable htButtonList = new Hashtable();
                int iIndex = 0;
                foreach (XmlNode NodeButtons in xnListButtons)
                {
                    string sKey = "";
                    if (NodeButtons.Attributes[Definition.XML_VARIABLE_KEY] != null)
                    {
                        sKey = NodeButtons.Attributes[Definition.XML_VARIABLE_KEY].Value;
                    }

                    if (sKey == sItemKey)
                    {
                        foreach (XmlNode NodeButton in NodeButtons)
                        {

                            string sIndex = NodeButton.Attributes[Definition.XML_VARIABLE_INDEX].Value;
                            Hashtable htButtonAttributes = new Hashtable();

                            string sContextKey = NodeButton[Definition.XML_VARIABLE_CONTEXTKEY].InnerText;
                            string sContextVisible = NodeButton[Definition.XML_VARIABLE_CONTEXTVISIBLE].InnerText;

                            if (sContextVisible.ToUpper() == Definition.VARIABLE_TRUE)
                            {
                                string sContextMenu = this._mlthandler.GetVariable(sContextKey);
                                slButtonListIndex.Add(iIndex.ToString(), sContextMenu);
                                iIndex++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }

            return slButtonListIndex;
        }

        #endregion

        #region : SPREAD


        ///2009-10-21 bskwon 추가
        ///Ppk Spread Header
        public void InitializePpkColumnHeader(ref BSpread bsprData, SortedList _sortHeader, SortedList _sortHeaderLabel)
        {
            if (_sortHeader == null || _sortHeader.Count == 0) return;
            if (_sortHeaderLabel == null || _sortHeaderLabel.Count == 0) return;

            //bsprData.ActiveSheet.DefaultStyle.ResetLocked();
            //bsprData.ClearHead();
            //bsprData.UseEdit = true;
            ////bsprData.UseHeadColor = true;
            //bsprData.Locked = true;

            int iWidth = 100;
            int iMaxLength = 20;

            bsprData.ActiveSheet.RowCount = 0;
            bsprData.ActiveSheet.Columns.Count = _sortHeader.Count;

            for (int i = 0; i < _sortHeader.Count; i++)
            {
                string sDataField = _sortHeader[i].ToString();
                string sHeaderAlias = sDataField;
                bool bColumnVisible = true;
                if (_sortHeaderLabel.ContainsKey(sDataField))
                    sHeaderAlias = _mlthandler.GetVariable(_sortHeaderLabel[_sortHeader[i].ToString()].ToString());
                else
                    bColumnVisible = false;

                if (i == 0)
                {
                    iWidth = 60;
                    bsprData.AddHead(i, sHeaderAlias, sDataField, iWidth, iMaxLength, null, null, null, ColumnAttribute.Null, ColumnType.CheckBox, null, null, null, false, bColumnVisible);
                }
                else
                {
                    iWidth = 100;
                    if (i > 9) iWidth = 80;

                    bsprData.AddHead(i, sHeaderAlias, sDataField, iWidth, iMaxLength, null, null, null, ColumnAttribute.Null, ColumnType.Null, null, null, null, false, bColumnVisible);
                }
            }
            bsprData.AddHeadComplete();
        }

        public SortedList InitializeColumnHeader(ref BSpread bsprData, string PageKey, bool bUseHeadCommand, string ItemKey)
        {
            SortedList slColumnHeaderIndex = new SortedList();
            this._slDefaultVisiableList = new SortedList();
            int iErrorIndex = 0;
            try
            {

                string sXMLColumnListPath = GetApplicationXmlFile(PageKey);

                XmlDocument xdocColumnHeader = new XmlDocument();

                xdocColumnHeader.Load(sXMLColumnListPath);

                XmlNodeList xnListColumnHeader = xdocColumnHeader.SelectNodes("/Settings/page/Header");

                Hashtable htColumnAttributes = new Hashtable();

                ItemKey = ItemKey.Trim();

                foreach (XmlNode xNodecolumns in xnListColumnHeader)
                {
                    string sKey = "";
                    if (xNodecolumns.Attributes[Definition.XML_VARIABLE_KEY] != null)
                    {
                        sKey = xNodecolumns.Attributes[Definition.XML_VARIABLE_KEY].Value;
                    }



                    if (sKey == ItemKey)
                    {
                        foreach (XmlNode xNodecolumn in xNodecolumns)
                        {
                            Hashtable htColumnAttribute = new Hashtable();

                            string sIndex = xNodecolumn.Attributes[Definition.XML_VARIABLE_INDEX].Value;

                            string strValue = xNodecolumn[Definition.XML_VARIABLE_HEADERKEY].InnerText;


                            string sHeaderKeyText = xNodecolumn[Definition.XML_VARIABLE_HEADERKEY].InnerText;
                            string sHeadNameText = xNodecolumn[Definition.XML_VARIABLE_HEADNAME].InnerText;
                            string sFieldText = xNodecolumn[Definition.XML_VARIABLE_FIELD].InnerText;
                            string sWidthText = xNodecolumn[Definition.XML_VARIABLE_WIDTH].InnerText;
                            string sMaxLengthText = xNodecolumn[Definition.XML_VARIABLE_MAXLENGTH].InnerText;
                            string sSaveTableText = xNodecolumn[Definition.XML_VARIABLE_SAVETABLE].InnerText;
                            string sSaveFieldText = xNodecolumn[Definition.XML_VARIABLE_SAVEFIELD].InnerText;
                            string sSequenceTableText = xNodecolumn[Definition.XML_VARIABLE_SEQUENCETABLE].InnerText;
                            string sColumnAttributeText = xNodecolumn[Definition.XML_VARIABLE_COLUMNATTRIBUTE].InnerText;
                            string sColumnTypeText = xNodecolumn[Definition.XML_VARIABLE_COLUMNTYPE].InnerText;
                            string sColumnDataText = xNodecolumn[Definition.XML_VARIABLE_COLUMNDATA].InnerText;
                            string sCodeGroupTextText = xNodecolumn[Definition.XML_VARIABLE_CODEGROUPTEXT].InnerText;
                            string sDefaultValueText = xNodecolumn[Definition.XML_VARIABLE_DEFAULTVALUE].InnerText;
                            string sComboVisibleText = xNodecolumn[Definition.XML_VARIABLE_COMBOVISIBLE].InnerText;
                            string sColumnVisibleText = xNodecolumn[Definition.XML_VARIABLE_COLUMNVISIBLE].InnerText;
                            string sDefaultVisibleText = "";
                            string sContextMenuName = "";

                            if (xNodecolumn[Definition.XML_VARIABLE_DEFAULTVISIBLE] != null)
                                sDefaultVisibleText = xNodecolumn[Definition.XML_VARIABLE_DEFAULTVISIBLE].InnerText;

                            if (xNodecolumn[Definition.XML_VARIABLE_CONTEXTMENUNAME] != null)
                                sContextMenuName = xNodecolumn[Definition.XML_VARIABLE_CONTEXTMENUNAME].InnerText;

                            htColumnAttribute.Add(Definition.XML_VARIABLE_HEADERKEY, sHeaderKeyText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_HEADNAME, sHeadNameText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_FIELD, sFieldText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_WIDTH, sWidthText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_MAXLENGTH, sMaxLengthText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_SAVETABLE, sSaveTableText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_SAVEFIELD, sSaveFieldText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_SEQUENCETABLE, sSequenceTableText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_COLUMNATTRIBUTE, sColumnAttributeText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_COLUMNTYPE, sColumnTypeText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_COLUMNDATA, sColumnDataText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_CODEGROUPTEXT, sCodeGroupTextText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_DEFAULTVALUE, sDefaultValueText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_COMBOVISIBLE, sComboVisibleText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_COLUMNVISIBLE, sColumnVisibleText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_DEFAULTVISIBLE, sDefaultVisibleText);
                            htColumnAttribute.Add(Definition.XML_VARIABLE_CONTEXTMENUNAME, sContextMenuName);
                            htColumnAttributes.Add(sIndex, htColumnAttribute);

                        }
                    }
                }

                bsprData.ActiveSheet.DefaultStyle.ResetLocked();
                bsprData.ClearHead();
                bsprData.UseEdit = true;
                //bsprData.UseHeadColor = true;
                bsprData.Locked = true;

                for (int i = 0; i < htColumnAttributes.Count; i++)
                {
                    Hashtable htColumns = (Hashtable)htColumnAttributes[i.ToString()];
                    iErrorIndex = i;

                    string sHeaderKey = htColumns[Definition.XML_VARIABLE_HEADERKEY].ToString();
                    string sHeaderAlias = this._mlthandler.GetVariable(sHeaderKey);

                    string sHeadName = htColumns[Definition.XML_VARIABLE_HEADNAME].ToString();
                    string sField = htColumns[Definition.XML_VARIABLE_FIELD].ToString();

                    //Hashtable htFieldAttributes = (Hashtable)_htFieldName[sHeaderKey];

                    //if (sField == "" && htFieldAttributes != null)
                    //    sField = htFieldAttributes[Definition.XML_VARIABLE_FIELDNAME].ToString();

                    string sWidth = htColumns[Definition.XML_VARIABLE_WIDTH].ToString();
                    int iWidth = 80;
                    if (sWidth != "")
                        iWidth = Convert.ToInt32(sWidth);

                    string sMaxLength = htColumns[Definition.XML_VARIABLE_MAXLENGTH].ToString();
                    //if (sMaxLength == "" && htFieldAttributes != null)
                    //    sMaxLength = htFieldAttributes[Definition.XML_VARIABLE_MAXBYTES].ToString();
                    int iMaxLength = 10;
                    if (sMaxLength != "")
                        iMaxLength = Convert.ToInt32(sMaxLength);

                    string sSaveTable = htColumns[Definition.XML_VARIABLE_SAVETABLE].ToString();
                    string sSaveField = htColumns[Definition.XML_VARIABLE_SAVEFIELD].ToString();
                    string sSequenceTable = htColumns[Definition.XML_VARIABLE_SEQUENCETABLE].ToString();
                    string sColumnAttribute = htColumns[Definition.XML_VARIABLE_COLUMNATTRIBUTE].ToString();
                    ColumnAttribute caColumnAttribute = ColumnAttribute.Null;
                    if (sColumnAttribute != "")
                        caColumnAttribute = this.SetColumnAttribute(sColumnAttribute);

                    string sColumnType = htColumns[Definition.XML_VARIABLE_COLUMNTYPE].ToString();
                    ColumnType ctColumnType = ColumnType.Null;
                    if (sColumnType != "")
                        ctColumnType = this.SetColumnType(sColumnType);

                    string sColumnData = htColumns[Definition.XML_VARIABLE_COLUMNDATA].ToString();
                    string[] saColumnData = null;
                    if (sColumnData != "")
                        saColumnData = sColumnData.Split('^');

                    string sCodeGroupText = htColumns[Definition.XML_VARIABLE_CODEGROUPTEXT].ToString();
                    string sDefaultValue = htColumns[Definition.XML_VARIABLE_DEFAULTVALUE].ToString();
                    string sComboVisible = htColumns[Definition.XML_VARIABLE_COMBOVISIBLE].ToString();
                    bool bComboVisible = false;
                    if (sComboVisible != "")
                        bComboVisible = Convert.ToBoolean(sComboVisible);

                    string sColumnVisible = htColumns[Definition.XML_VARIABLE_COLUMNVISIBLE].ToString();
                    bool bColumnVisible = false;
                    if (sColumnVisible != "")
                        bColumnVisible = Convert.ToBoolean(sColumnVisible);

                    string sDefaultVisible = htColumns[Definition.XML_VARIABLE_DEFAULTVISIBLE].ToString();

                    string sContextMenuName = htColumns[Definition.XML_VARIABLE_CONTEXTMENUNAME].ToString();


                    Hashtable htFieldAttributes = new Hashtable();

                    htFieldAttributes.Add(Definition.XML_VARIABLE_HEADERKEY, sHeaderKey);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_HEADNAME, sHeadName);
                    //htFieldAttributes.Add(Definition.XML_VARIABLE_FIELD, sField);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_WIDTH, sWidth);

                    htFieldAttributes.Add(Definition.XML_VARIABLE_FIELDNAME, sField);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_MAXLENGTH, sMaxLength);

                    htFieldAttributes.Add(Definition.XML_VARIABLE_SAVETABLE, sSaveTable);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_SAVEFIELD, sSaveField);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_SEQUENCETABLE, sSequenceTable);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_COLUMNATTRIBUTE, sColumnAttribute);

                    htFieldAttributes.Add(Definition.XML_VARIABLE_COLUMNTYPE, sColumnType);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_COLUMNDATA, sColumnData);

                    htFieldAttributes.Add(Definition.XML_VARIABLE_CODEGROUPTEXT, sCodeGroupText);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_DEFAULTVALUE, sDefaultValue);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_COMBOVISIBLE, sComboVisible);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_COLUMNVISIBLE, sColumnVisible);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_DEFAULTVISIBLE, sDefaultVisible);
                    htFieldAttributes.Add(Definition.XML_VARIABLE_CONTEXTMENUNAME, sContextMenuName);

                    //htFieldAttributes.Add(Definition.XML_VARIABLE_MAXBYTES, iMaxLength);

                    //if (_htFieldName[sHeaderKey] == null)
                    //    _htFieldName.Add(sHeaderKey, htFieldAttributes);

                    if (_htFieldName[sHeaderKey] == null)
                        _htFieldName.Add(sHeaderKey, htFieldAttributes);
                    else
                    {
                        _htFieldName.Remove(sHeaderKey);
                        _htFieldName.Add(sHeaderKey, htFieldAttributes);
                    }

                    slColumnHeaderIndex.Add(sHeaderAlias, i);
                    this._slDefaultVisiableList.Add(sHeaderAlias, sDefaultVisible);

                    if (sHeaderKey == Definition.COL_HEADER_KEY_V_INSERT ||
                        sHeaderKey == Definition.COL_HEADER_KEY_V_MODIFY ||
                        sHeaderKey == Definition.COL_HEADER_KEY_V_DELETE)
                    {
                        if (bUseHeadCommand)
                        {
                            if (sContextMenuName.Length == 0)
                                sContextMenuName = sHeaderAlias;

                            bsprData.AddHeadCommand(i, sHeaderAlias, iWidth, ctColumnType, bColumnVisible, sContextMenuName);
                        }
                        else
                            bsprData.AddHead(i, sHeaderAlias, sField, iWidth, iMaxLength, sSaveTable, sSaveField, sSequenceTable,
                            caColumnAttribute, ctColumnType, saColumnData, sCodeGroupText, sDefaultValue, bComboVisible, bColumnVisible);
                    }
                    else
                    {
                        bsprData.AddHead(i, sHeaderAlias, sField, iWidth, iMaxLength, sSaveTable, sSaveField, sSequenceTable,
                            caColumnAttribute, ctColumnType, saColumnData, sCodeGroupText, sDefaultValue, bComboVisible, bColumnVisible);
                    }
                }

                bsprData.AddHeadComplete();

            }
            catch (Exception ex)
            {
                MessageBox.Show(iErrorIndex.ToString());
            }
            finally
            {
            }

            return slColumnHeaderIndex;
        }

        public SortedList slDefaultVisibleList
        {
            set
            {
                this._slDefaultVisiableList = value;
            }
            get
            {
                return this._slDefaultVisiableList;
            }
        }


        /// <summary>
        /// Spread Row Header의 Column Name, Caption, Tag, Width, Visible를 설정합니다.
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="spreadIdx"></param>
        public void SetRowHeader(ref BSpread spread, int spreadIdx)
        {
            //spread.ClearHead();
            //XmlNodeList nodeList = this.mStyleDoc.SelectNodes("/Settings/Spread");

            //int rowIdx = 0;
            //foreach (XmlNode node in nodeList)
            //{
            //    if (node.Attributes["index"].Value.Equals(spreadIdx.ToString()))
            //    {
            //        spread.ActiveSheet.RowHeader.Columns[0].ResetWidth();
            //        spread.ActiveSheet.RowHeader.Columns[0].Width = int.Parse(node.Attributes["rowwidth"].Value.ToString());

            //        if (node.HasChildNodes)
            //        {
            //            foreach (XmlNode headerNode in node.ChildNodes)
            //            {
            //                if (headerNode.HasChildNodes)
            //                {
            //                    spread.ActiveSheet.RowCount = headerNode.ChildNodes.Count;

            //                    foreach (XmlNode colNode in headerNode.ChildNodes)
            //                    {
            //                        rowIdx = int.Parse(colNode.Attributes["index"].Value);
            //                        if (colNode.HasChildNodes)
            //                        {
            //                            XmlNode keyNode = colNode.FirstChild;
            //                            spread.ActiveSheet.RowHeader.Rows[rowIdx].Tag = keyNode.InnerText;

            //                            XmlNode capNode = keyNode.NextSibling;
            //                            spread.ActiveSheet.RowHeader.Rows[rowIdx].Label = capNode.InnerText;

            //                            XmlNode visibleNode = colNode.LastChild;
            //                            spread.ActiveSheet.RowHeader.Rows[rowIdx].Visible = bool.Parse(visibleNode.InnerText);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}//end root node

            //spread.UseAutoSort = false;
            //spread.DataAutoHeadings = true;
            //spread.AutoGenerateColumns = true;
        }

        public ColumnType SetColumnType(string key)
        {
            try
            {
                switch (key)
                {
                    case "NULL":
                        return ColumnType.Null;
                        break;

                    case "BUTTON":
                        return ColumnType.Button;
                        break;

                    case "CHECKBOX":
                        return ColumnType.CheckBox;
                        break;

                    case "COMBOBOX":
                        return ColumnType.ComboBox;
                        break;

                    case "COMBOBOXCODE":
                        return ColumnType.ComboBoxCode;
                        break;

                    case "DELETE":
                        return ColumnType.Delete;
                        break;

                    case "INSERT":
                        return ColumnType.Insert;
                        break;

                    case "MODIFY":
                        return ColumnType.Modify;
                        break;

                    case "SEQUENCE":
                        return ColumnType.Sequence;
                        break;

                    case "DateTime":
                        return ColumnType.DateTime;
                        break;

                    case "Date":
                        return ColumnType.Date;
                        break;

                    case "Time":
                        return ColumnType.Time;
                        break;

                    case "INT":
                        return ColumnType.Int;
                        break;

                    case "DOUBLE":
                        return ColumnType.Double;
                        break;

                    default:
                        return ColumnType.Null;
                        break;
                }

            }
            catch (Exception ex)
            {
                return ColumnType.Null;
            }
            return ColumnType.Null;
        }

        public ColumnAttribute SetColumnAttribute(string key)
        {
            try
            {
                switch (key)
                {
                    case "KEY":
                        return ColumnAttribute.Key; //
                        break;

                    case "MANDATORY":
                        return ColumnAttribute.Mandatory;
                        break;

                    case "NULL":
                        return ColumnAttribute.Null;
                        break;

                    case "OPTIONAL":
                        return ColumnAttribute.Optional;
                        break;

                    case "READONLY":
                        return ColumnAttribute.ReadOnly;
                        break;

                    case "UNIQUE":
                        return ColumnAttribute.Unique;
                        break;

                    default:
                        return ColumnAttribute.Null;
                        break;
                }
            }
            catch (Exception ex)
            {
                return ColumnAttribute.Null;
            }
            return ColumnAttribute.Null;
        }

        public LinkedList InitializeToolTipList(string PageKey, string ItemKey, SessionData sessionData)
        {
            LinkedList slToolTipListIndex = new LinkedList();

            try
            {


                string strXMLPath = GetApplicationXmlFile(PageKey);

                XmlDocument xdocPageStyle = new XmlDocument();

                xdocPageStyle.Load(strXMLPath);

                XmlNodeList xnListToolTips = xdocPageStyle.SelectNodes("/Settings/page/ToolTips");

                Hashtable htToolTipList = new Hashtable();

                ItemKey = ItemKey.Trim();

                foreach (XmlNode NodeToolTips in xnListToolTips)
                {
                    string sKey = "";
                    if (NodeToolTips.Attributes[Definition.XML_VARIABLE_KEY] != null)
                    {
                        sKey = NodeToolTips.Attributes[Definition.XML_VARIABLE_KEY].Value;
                    }

                    if (sKey == ItemKey)
                    {
                        foreach (XmlNode NodeToolTip in NodeToolTips)
                        {
                            string sIndex = NodeToolTip.Attributes[Definition.XML_VARIABLE_INDEX].Value;
                            Hashtable htToolTipAttributes = new Hashtable();

                            string sToolTipLabel = NodeToolTip[Definition.XML_VARIABLE_TOOLTIPLABEL].InnerText;
                            string sToolTipKey = NodeToolTip[Definition.XML_VARIABLE_TOOLTIPKEY].InnerText;
                            string sToolTipVisible = NodeToolTip[Definition.XML_VARIABLE_TOOLTIPVISIBLE].InnerText;
                            bool bToolTipVisible = bool.Parse(sToolTipVisible);

                            string sTitle = "";

                            if (sToolTipLabel == "")
                            {
                                sTitle = this._mlthandler.GetVariable(sToolTipKey);
                            }
                            else
                            {
                                sTitle = this._mlthandler.GetVariable(sToolTipLabel);
                            }

                            string sColumn = "";

                            if (sToolTipKey == "")
                            {

                            }
                            else
                            {
                                sColumn = this._mlthandler.GetVariable(sToolTipKey);
                            }

                            if (bToolTipVisible)
                            {
                                slToolTipListIndex.Add(sTitle, sColumn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return slToolTipListIndex;
        }

        public LinkedList InitializeAdditionalList(string PageKey, string ItemKey, SessionData sessionData)
        {
            LinkedList slAdditionalListIndex = new LinkedList();

            try
            {
                string strXMLPath = GetApplicationXmlFile(PageKey);
                XmlDocument xdocPageStyle = new XmlDocument();
                xdocPageStyle.Load(strXMLPath);

                XmlNodeList xnListAdditionals = xdocPageStyle.SelectNodes("/Settings/page/Additionals");

                Hashtable htAdditionalList = new Hashtable();

                ItemKey = ItemKey.Trim();

                foreach (XmlNode NodeAdditionals in xnListAdditionals)
                {
                    string sKey = "";
                    if (NodeAdditionals.Attributes[Definition.XML_VARIABLE_KEY] != null)
                    {
                        sKey = NodeAdditionals.Attributes[Definition.XML_VARIABLE_KEY].Value;
                    }

                    if (sKey == ItemKey)
                    {
                        int iCount = 0;

                        foreach (XmlNode NodeAdditional in NodeAdditionals)
                        {
                            string sIndex = NodeAdditional.Attributes[Definition.XML_VARIABLE_INDEX].Value;
                            Hashtable htAdditionalAttributes = new Hashtable();
                            AdditionalSeriesInfo additional = new AdditionalSeriesInfo();

                            string sSeriesColor = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALSERIESCOLOR].InnerText;
                            if (!sSeriesColor.Equals(string.Empty))
                            {
                                additional.SeriesColor = Color.FromName(sSeriesColor);
                            }

                            string sXColumn = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALXCOLUMN].InnerText;
                            if (!sXColumn.Equals(string.Empty))
                            {
                                additional.XColumn = sXColumn;
                            }

                            string sYColumn = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALYCOLUMN].InnerText;
                            if (!sYColumn.Equals(string.Empty))
                            {
                                additional.YColumn = sYColumn;
                            }

                            string sSeriesType = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALSERIESTYPE].InnerText;
                            if (sSeriesType.Equals("Points"))
                            {
                                additional.SeriesType = typeof(Steema.TeeChart.Styles.Points);
                            }
                            else if (sSeriesType.Equals("Line"))
                            {
                                additional.SeriesType = typeof(Steema.TeeChart.Styles.Line);
                            }

                            string sLabelColumn = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALLABELCOLUMN].InnerText;
                            if (!sLabelColumn.Equals(string.Empty))
                            {
                                additional.LabelColumn = sLabelColumn;
                            }

                            string sExtractDataMethod = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALEXTRACTDATAMETHOD].InnerText;
                            string[] sSplitExtract = sExtractDataMethod.Split('|');
                            int iArrayCount = sSplitExtract.Length;
                            ExtractDataMethods[] eDataMethods = new ExtractDataMethods[iArrayCount];
                            ExtractDataMethods eDataMethodsClone = new ExtractDataMethods();
                            for (int i = 0; i < sSplitExtract.Length; i++)
                            {
                                sSplitExtract[i] = sSplitExtract[i].Trim();
                                switch (sSplitExtract[i])
                                {
                                    case "BeforeDistinct":
                                        eDataMethods[i] = ExtractDataMethods.BeforeDistinct;
                                        break;

                                    case "Constant":
                                        eDataMethods[i] = ExtractDataMethods.Constant;
                                        break;

                                    case "Custom":
                                        eDataMethods[i] = ExtractDataMethods.Custom;
                                        break;

                                    case "IsExist":
                                        eDataMethods[i] = ExtractDataMethods.IsExist;
                                        break;

                                    case "NullRestart":
                                        eDataMethods[i] = ExtractDataMethods.NullRestart;
                                        break;

                                    default:
                                        eDataMethods[i] = ExtractDataMethods.All;
                                        break;
                                }
                            }
                            int iExtractCount = 0;
                            eDataMethodsClone = eDataMethods[0];
                            for (int i = 0; i < sSplitExtract.Length; i++)
                            {
                                if (iExtractCount == sSplitExtract.Length - 1)
                                {
                                    break;
                                }
                                else
                                {
                                    eDataMethodsClone = eDataMethodsClone | eDataMethods[i + 1];
                                }
                                iExtractCount++;
                            }
                            additional.ExtractDataMethod = eDataMethodsClone;

                            string sDisplayPosition = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALDISPLAYPOSITION].InnerText;
                            switch (sDisplayPosition)
                            {
                                case "Top":
                                    additional.DisplayPosition = DisplayPositions.Top;
                                    break;

                                case "Bottom":
                                    additional.DisplayPosition = DisplayPositions.Bottom;
                                    break;

                                case "Content":
                                    additional.DisplayPosition = DisplayPositions.Content;
                                    break;

                                default:
                                    break;
                            }

                            string sIsSameContentSameLine = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALISSAMECONTENTSAMELINE].InnerText;
                            if (!sIsSameContentSameLine.Equals(string.Empty))
                            {
                                bool bIsSameContentSameLine = bool.Parse(sIsSameContentSameLine);
                                additional.IsSameContentSameLine = bIsSameContentSameLine;
                            }

                            string sIsVisible = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALISVISIBLE].InnerText;
                            if (!sIsVisible.Equals(string.Empty))
                            {
                                bool bIsVisible = bool.Parse(sIsVisible);
                                additional.IsVisible = bIsVisible;
                            }

                            string sStyle = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALSTYLE].InnerText;
                            switch (sStyle)
                            {
                                case "ColorLine":
                                    additional.Style = AdditonalStyle.ColorLine;
                                    break;

                                case "ColorBar":
                                    additional.Style = AdditonalStyle.ColorBar;
                                    break;

                                case "CrossSymbol":
                                    additional.Style = AdditonalStyle.CrossSymbol;
                                    break;

                                case "DiamondSymbol":
                                    additional.Style = AdditonalStyle.DiamondSymbol;
                                    break;

                                case "DownTriangle":
                                    additional.Style = AdditonalStyle.DownTriangle;
                                    break;

                                case "RectangleFrame":
                                    additional.Style = AdditonalStyle.RectangleFrame;
                                    break;

                                case "RectangleSymbol":
                                    additional.Style = AdditonalStyle.RectangleSymbol;
                                    break;

                                case "RotateFrame90":
                                    additional.Style = AdditonalStyle.RotateFrame90;
                                    break;

                                case "RoundRectangleFrame":
                                    additional.Style = AdditonalStyle.RoundRectangleFrame;
                                    break;

                                default:
                                    additional.Style = AdditonalStyle.None;
                                    break;
                            }

                            string sContentName = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALCONTENTNAME].InnerText;
                            if (!sContentName.Equals(string.Empty))
                            {
                                additional.ContentName = sContentName;
                            }

                            string sConstantValue = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALCONSTANTVALUE].InnerText;
                            if (!sConstantValue.Equals(string.Empty))
                            {
                                additional.ConstantValue = sConstantValue;
                            }

                            string sAddColorValueString = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALADDCOLORVALUESTRING].InnerText;
                            string sAddColorValueColor = NodeAdditional[Definition.XML_VARIABLE_ADDITIONALADDCOLORVALUECOLOR].InnerText;
                            if (!(sAddColorValueColor.Equals(string.Empty)) && !(sAddColorValueString.Equals(string.Empty)))
                            {
                                string[] sSplitString = sAddColorValueString.Split(';');
                                string[] sSplitColor = sAddColorValueColor.Split(';');
                                for (int i = 0; i < sSplitColor.Length; i++)
                                {
                                    sSplitString[i] = sSplitString[i].Trim();
                                    sSplitColor[i] = sSplitColor[i].Trim();
                                    additional.AddColorValue(sSplitString[i], Color.FromName(sSplitColor[i]));
                                }
                            }


                            slAdditionalListIndex.Add("Additional" + iCount, additional);
                            iCount++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return slAdditionalListIndex;
        }



        public void SetCheckColumnHeader(Control control, int iColIndex)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread spread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            FarPoint.Win.Spread.CellType.CheckBoxCellType checkbox = new FarPoint.Win.Spread.CellType.CheckBoxCellType();

            string sColText = spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Text;

            if (sColText.Length == 0)
            {
                sColText = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT);
            }

            spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].CellType = checkbox;
            checkbox.Caption = sColText;
            spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Text = sColText;

            //spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Value = false;
        }

        //public void SetTextColumnHeader(Control control, int iColIndex)
        //{
        //    BISTel.PeakPerformance.Client.BISTelControl.BSpread spread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

        //    FarPoint.Win.Spread.CellType.TextCellType textCell = new FarPoint.Win.Spread.CellType.TextCellType();

        //    string sColText = spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Text;

        //    if (sColText.Length == 0)
        //    {
        //        sColText = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT);
        //    }

        //    spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].CellType = textCell;
        //    spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Text = sColText;
        //}

        public void SetTextColumnHeader(Control control, int iColIndex)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread spread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;

            FarPoint.Win.Spread.CellType.TextCellType textCell = new FarPoint.Win.Spread.CellType.TextCellType();

            string sColText = string.Empty;
            if (spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].CellType.GetType() == typeof(FarPoint.Win.Spread.CellType.CheckBoxCellType))
            {
                FarPoint.Win.Spread.CellType.CheckBoxCellType checkbox = (FarPoint.Win.Spread.CellType.CheckBoxCellType)spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].CellType;
                sColText = checkbox.Caption;
            }
            else
                sColText = spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Text;

            if (sColText.Length == 0)
            {
                sColText = this._mlthandler.GetVariable(Definition.COL_HEADER_KEY_V_SELECT);
            }

            spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].CellType = textCell;
            spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Text = sColText;
        }

        public void SetCheckColumnHeaderStatus(BSpread spread, int iColIndex)
        {
            int[] iVisibleRow = this._bspreadutility.GetVisibleRowIndex(spread);

            if (iVisibleRow == null || iVisibleRow.Length.Equals(0))
                return;

            string sValue = spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Value.ToString();

            if (sValue.ToUpper().Equals(Definition.VARIABLE_TRUE.ToUpper()))
            {
                if (!spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Locked)
                    spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Value = false;

                for (int i = 0; i < iVisibleRow.Length; i++)
                {
                    if (!spread.ActiveSheet.Cells[iVisibleRow[i], iColIndex].Locked)
                        spread.ActiveSheet.Cells[iVisibleRow[i], iColIndex].Value = "False";
                }
            }
            else if (!sValue.ToUpper().Equals(Definition.VARIABLE_TRUE.ToUpper()))
            {
                if (!spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Locked)
                    spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Value = true;

                for (int i = 0; i < iVisibleRow.Length; i++)
                {
                    if (!spread.ActiveSheet.Cells[iVisibleRow[i], iColIndex].Locked)
                        spread.ActiveSheet.Cells[iVisibleRow[i], iColIndex].Value = "True";
                }
            }
            else if (spread.ActiveSheet.Rows.Count > 0)
            {
                if (!spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Locked)
                    spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Value = true;

                for (int i = 0; i < iVisibleRow.Length; i++)
                {
                    if (!spread.ActiveSheet.Cells[iVisibleRow[i], iColIndex].Locked)
                        spread.ActiveSheet.Cells[iVisibleRow[i], iColIndex].Value = "True";
                }
            }
        }

        public void SetCheckColumnHeaderStatus(Control control, int iColIndex, bool bCheck)
        {
            BISTel.PeakPerformance.Client.BISTelControl.BSpread spread = (BISTel.PeakPerformance.Client.BISTelControl.BSpread)control;
            spread.ActiveSheet.ColumnHeader.Cells[0, iColIndex].Value = bCheck;
        }

        #endregion




    }
}
