using System;
using System.Collections.Generic;
using System.Text;

namespace BISTel.eSPC.Common
{
    public class Definition
    {
        public Definition()
        {

        }

        #region : DB

        public static string DB_NAME = "ESPC";

        #endregion



        public const string APPName = "eSPC";
        public const string SeqTable = "SEQ_";
        public const int FAIL = 0;
        public const int SUCCESS = 1;
        public const int RESTRICT_SAMPLE_COUNT = 50;
        public const int RESTRICT_SAMPLE_DAYS = 2;        

        public const string YES = "Y";
        public const string NO = "N";
        public static string[] YNList = new string[] { "Y", "N" };

        public const string SPC_LABEL_ = "SPC_LABEL_";

        public static string[] ChartDataList = new string[] { "_USL", "_UCL", "_TARGET", "_LCL", "_LSL", "_RULE" };
        public static string INITIAL_DISPLAY_CHART = "X-BAR;STDDEV;RAW";

        public const string LOADING_DATA = "SPC_INFO_LOADING_DATA";
        public const string VALIDATING_RULE = "SPC_INFO_VALIDATING_RULE"; // SPC-930, KBLEE

        public const string NONE = "NONE"; //SPC-930, KBLEE
        public const string DESCRIPTION = "DESCRIPTION"; //SPC-930, KBLEE
        public const string RSD = "RSD"; //SPC-929, KBLEE
        /// <summary>
        /// 2009-11-12 추가 
        /// Use Page : MutiDataUC
        /// </summary>
        public struct STEP_DATAMAPPING
        {
            public const string BASE = "BASE";  //추가
            public const string TYPE = "TYPE";  //추가
            public const string OPERATION_ID = "OPERATION_ID";
            public const string OPERATION_DESCRIPTION = "OPERATION_DESCRIPTION";
            public const string INFORMATION = "INFORMATION";
            public const string ITEM = "ITEM";
            public const string SUBDATA = "SUBDATA";
            public const string PROBE = "PROBE";
        }             
            


        #region :TableName
        public struct TableName
        {
            public static string AREA_MST_PP = "AREA_MST_PP";
            public static string CODE_MST_PP = "CODE_MST_PP";

            public static string OOC_TRX_SPC = "OOC_TRX_SPC";

            public const string USERNAME_DATA = "DATA";
            public const string USERNAME_TEMPDATA = "TEMPDATA";
            public static string USERNAME_SUMTEMPDATA = "SUMTEMPDATA";

            public const string USERNAME_DATA_SHORT = "D";
            public const string USERNAME_TEMPDATA_SHORT = "T";
            public const string USERNAME_SUMTEMPDATA_SHORT = "S";

            public static string MODEL_CONFIG_MST_SPC = "MODEL_CONFIG_MST_SPC";
            public static string MODEL_CONFIG_OPT_MST_SPC = "MODEL_CONFIG_OPT_MST_SPC";
            public static string MODEL_AUTOCALC_MST_SPC = "MODEL_AUTOCALC_MST_SPC";

            public static string MODEL_CONFIG_ATT_MST_SPC = "MODEL_CONFIG_ATT_MST_SPC";
            public static string MODEL_CONFIG_OPT_ATT_MST_SPC = "MODEL_CONFIG_OPT_ATT_MST_SPC";
            public static string MODEL_AUTOCALC_ATT_MST_SPC = "MODEL_AUTOCALC_ATT_MST_SPC";

            public static string MODEL_CONTEXT_MST_SPC = "MODEL_CONTEXT_MST_SPC";
            public static string MODEL_CONTEXT_ATT_MST_SPC = "MODEL_CONTEXT_ATT_MST_SPC";

            public const string DATA_TEMP_TRX_SPC = "DATA_TEMP_TRX_SPC";
            public const string DATA_TRX_SPC = "DATA_TRX_SPC";
        }
        
        

        #region Table Column

        public const string COL_RAW_ID = "RAWID";
        public const string COL_DCP_ID = "DCP_ID";
        public const string COL_DCP_STATE_CD = "DCP_STATE_CD";
        public const string COL_METHOD_NAME = "METHOD_NAME";
        public const string COL_EXECUTION_VALUE = "EXECUTION_VALUE";
        public const string COL_EXECUTION_KEY_VALUE = "EXECUTION_KEY_VALUE";
        public const string COL_TOGGLE = "TOGGLE";
        public const string COL_TOGGLE_YN = "TOGGLE_YN";
        public const string COL_MODEL_CONFIG_RAWID = "MODEL_CONFIG_RAWID";
        public const string COL_SAMPLE_COUNT = "SAMPLE_COUNT";

        //E
        public static string COL_EQP_MODULE_ID = "EQP_MODULE_ID";
        public static string COL_EQP_RECIPE_ID = "EQP_RECIPE_ID";
        public static string COL_RECIPE_ID = "RECIPE_ID";
        public static string COL_STEP_ID = "STEP_ID";
        public static string COL_EQP_PARAM_ALIAS = "EQP_PARAM_ALIAS";

        //SPC-929, KBLEE, START
        public const string COL_EQP_ID = "EQP_ID";
        public const string COL_MODULE_ID = "MODULE_ID";
        public const string COL_MODULE_NAME = "MODULE_NAME";
        public const string COL_MODULE_ALIAS = "MODULE_ALIAS";
        public const string COL_AREA = "AREA";
        public const string COL_STATUS_CD = "STATUS_CD";
        public const string COL_LOT_ID = "LOT_ID";
        public const string COL_LOT_TYPE = "LOT_TYPE";
        public const string COL_SUBSTRATE_ID = "SUBSTRATE_ID";
        public const string COL_CASSETTE_SLOT = "CASSETTE_SLOT";
        public const string COL_OPERATION_ID = "OPERATION_ID";
        public const string COL_PRODUCT_ID = "PRODUCT_ID";
        public const string COL_CHAMBER = "CHAMBER";
        public const string COL_RECIPE = "RECIPE";
        public const string COL_STEP = "STEP";
        public const string COL_RAW = "RAW";
        public const string COL_RAW_USL = "RAW_USL";
        public const string COL_RAW_LSL = "RAW_LSL";
        public const string COL_RAW_UCL = "RAW_UCL";
        public const string COL_RAW_LCL = "RAW_LCL";
        public const string COL_RAW_TARGET = "RAW_TARGET";
        public const string COL_USL = "UPPER_SPEC";
        public const string COL_LSL = "LOWER_SPEC";
        public const string COL_TARGET = "TARGET";
        public const string COL_WAFER = "wafer";
        public const string COL_SELECT = "SELECT";

        public const string COL_SUM_VALUE = "SUM_VALUE";
        public const string COL_LOTID = "LOTID";
        public const string COL_LOTTYPE = "LOTTYPE";
        public const string COL_SUBSTRATEID = "SUBSTRATEID";
        public const string COL_OPERATION = "OPERATION";
        public const string COL_PRODUCT = "PRODUCT";
        public const string COL_SLOT = "SLOT";
        public const string COL_STATUS = "STATUS";
        //SPC-929, KBLEE, END

        #endregion

        #endregion
        
        public const string CONTEXT_MENU_REMOVE = "Remove"; //SPC-929, KBLEE


        public struct Common
        {
        
        
        
        
        }

        /// <summary>
        /// Use Page : PpkUC
        /// </summary>
        public struct PERIOD_PPK
        {
            public static string DAILY = "DAILY";
            public static string WEEKLY = "WEEKLY";
            public static string MONTHLY = "MONTHLY";
            public static string NONE = "NONE";
        }

        /// <summary>
        /// Use Page : SPCAnalysisUC
        /// </summary>
        public struct Analysis
        {
            public static string LABEL_TARGET = "SPC_LABEL_TARGET";
            public static string LABEL_SUB_DATA = "SPC_LABEL_SUB_DATA";
            public static string LABEL_CHART_LIST = "SPC_LABEL_CHART_LIST";
            public static string LABEL_SORTING_KEY = "SPC_LABEL_CONDITION_SORTING_KEY";
        }


        public struct PERIOD_TYPE
        {
            public static string CURRENT = "CURRENT";
            public static string CUSTOM = "CUSTOM";
            public static string DAY = "DAY";
            public static string MONTH = "MONTH";
            public static string SHIFT = "SHIFT";
            public static string WEEK = "WEEK";
        }        
        
        public struct CHART_BUTTON
        {
            public static string CHART_DEFAULT = "CHART_DEFAULT";
            public static string CHART_ANALYSIS = "CHART_ANALYSIS";
            public static string CHART_ANALYSIS_RUN = "CHART_ANALYSIS_RUN";
            public static string CHART_SERIES = "CHART_SERIES";
            
        }
     
        public struct CHART_COLUMN
        {            
            public const string LOT_ID = "LOT_ID";
            public const string SUBSTRATE_ID = "SUBSTRATE_ID";
            public const string CASSETTE_SLOT = "CASSETTE_SLOT";
            public const string RECIPE_ID = "RECIPE_ID";
            public const string PRODUCT_ID = "PRODUCT_ID";
            public const string OPERATION_ID = "OPERATION_ID";            
            public const string OPERATION_DESC = "OPERATION_DESC";
            public const string MEASURE_OPERATION_ID = "MEASURE_OPERATION_ID";
            public const string EQP_ID = "EQP_ID";
            public const string MEASURE_EQP_ID = "MEASURE_EQP_ID";
            public const string MODULE_NAME = "MODULE_NAME";
            public const string STEP_ID = "STEP_ID";
            public const string CONTEXT_KEY = "CONTEXT_KEY";
            public const string PARAM_ALIAS = "PARAM_ALIAS";
            public const string PARAM_VALUE = "PARAM_VALUE";
            public const string AREA = "AREA";
            public const string OCAP_RAWID = "OCAP_RAWID";
            public const string OCAP_PROBLEM = "OOC_PROBLEM";
            public const string OCAP_CAUSE = "OOC_CAUSE";
            public const string OCAP_SOLUTION = "OOC_SOLUTION";
            public const string OCAP_COMMENT = "OOC_COMMENT";
            public const string OCAP_FALSE_ALARM_YN = "FALSE_ALARM_YN";
            //added by enkim[2009.11.25]
            public const string MODULE_ID = "MODULE_ID";
            public const string MODULE_ALIAS = "MODULE_ALIAS";
            // JIRA SPC-615 [GF] Include Chart ID under "Information" column in SPC charts. - 2011.10.04 by ANDREW KO
            // Add CHART_ID Info in ChartInfomation Spread : GF Communication Sheet 20110612 V1.21  - 2011.10.04 by ANDREW KO 
            public const string CHART_ID = "CHART ID";


            public const string TIME = "TIME";
            public const string TIME2 = "TIME2";
            public const string USL = "USL";
            public const string LSL = "LSL";
            public const string UCL = "UCL";
            public const string LCL = "LCL";
            public const string CL = "CL";
            public const string TARGET = "TARGET";
            public const string MEAN = "MEAN";
            public const string RAW = "RAW";
            public const string PN = "PN";
            public const string P = "P";
            public const string U = "U";
            public const string T = "T";
            public const string L = "L";
            public const string C = "C";
            public const string R = "R";
            public const string F = "F";

            public const string UCL_LCL = "Control Limit";
            public const string USL_LSL = "SPEC";
            public const string Average = "Average";
            public const string ConterLine = "Center Line";

            public const string AVG = "AVG";
            public const string MIN = "MIN";
            public const string MAX = "MAX";
            public const string RANGE = "RANGE";
            public const string STDDEV = "STDDEV";
            public const string CP = "CP";
            public const string CPK = "CPK";
            public const string PP = "PP";
            public const string PPK = "PPK";
            public const string SAMPLE_QTY = "SAMPLE_QTY";
            public const string SPEC = "SPEC";
            public const string PERIOD = "PERIOD";
            
          
            public const string LOT_QTY = "LOT_QTY";            
            public const string SUM_SQUARED = "SUM_SQUARED";
            public const string SUM = "SUM";            
            public const string PPU = "PPU";
            public const string PPL = "PPL";
            public const string SIGMA = "SIGMA";
                                    
           
            public const string MA = "MA";
            public const string MSD = "MSD";
            public const string MR = "MR";
            public const string EWMAMEAN = "EWMAMEAN";
            public const string EWMARANGE = "EWMARANGE";
            public const string EWMASTDDEV = "EWMASTDDEV";

            public const string MEAN_USL = "MEAN_USL";
            public const string MEAN_LSL = "MEAN_LSL";
            public const string MEAN_UCL = "MEAN_UCL";
            public const string MEAN_LCL = "MEAN_LCL";
            public const string MEAN_CL = "MEAN_CL";
            public const string MEAN_TARGET = "MEAN_TARGET";

            public const string RANGE_USL = "RANGE_USL";
            public const string RANGE_LSL = "RANGE_LSL";
            public const string RANGE_UCL = "RANGE_UCL";
            public const string RANGE_LCL = "RANGE_LCL";
            public const string RANGE_CL = "RANGE_CL";
            public const string RANGE_TARGET = "RANGE_TARGET";

            public const string STDDEV_USL = "STDDEV_USL";
            public const string STDDEV_LSL = "STDDEV_LSL";
            public const string STDDEV_UCL = "STDDEV_UCL";
            public const string STDDEV_LCL = "STDDEV_LCL";
            public const string STDDEV_CL = "STDDEV_CL";
            public const string STDDEV_TARGET = "STDDEV_TARGET";

            public const string RAW_USL = "RAW_USL";
            public const string RAW_LSL = "RAW_LSL";
            public const string RAW_UCL = "RAW_UCL";
            public const string RAW_LCL = "RAW_LCL";
            public const string RAW_CL = "RAW_CL";
            public const string RAW_TARGET = "RAW_TARGET";

            public const string EWMAMEAN_USL = "EWMAMEAN_USL";
            public const string EWMAMEAN_LSL = "EWMAMEAN_LSL";
            public const string EWMAMEAN_UCL = "EWMAMEAN_UCL";
            public const string EWMAMEAN_LCL = "EWMAMEAN_LCL";
            public const string EWMAMEAN_CL = "EWMAMEAN_CL";
            public const string EWMAMEAN_TARGET = "EWMAMEAN_TARGET";

            public const string EWMARANGE_USL = "EWMARANGE_USL";
            public const string EWMARANGE_LSL = "EWMARANGE_LSL";
            public const string EWMARANGE_UCL = "EWMARANGE_UCL";
            public const string EWMARANGE_LCL = "EWMARANGE_LCL";
            public const string EWMARANGE_CL = "EWMARANGE_CL";
            public const string EWMARANGE_TARGET = "EWMARANGE_TARGET";

            public const string EWMASTDDEV_USL = "EWMASTDDEV_USL";
            public const string EWMASTDDEV_LSL = "EWMASTDDEV_LSL";
            public const string EWMASTDDEV_UCL = "EWMASTDDEV_UCL";
            public const string EWMASTDDEV_LCL = "EWMASTDDEV_LCL";
            public const string EWMASTDDEV_CL = "EWMASTDDEV_CL";
            public const string EWMASTDDEV_TARGET = "EWMASTDDEV_TARGET";

            public const string MA_USL = "MA_USL";
            public const string MA_LSL = "MA_LSL";
            public const string MA_UCL = "MA_UCL";
            public const string MA_LCL = "MA_LCL";
            public const string MA_CL = "MA_CL";
            public const string MA_TARGET = "MA_TARGET";

            public const string MSD_USL = "MSD_USL";
            public const string MSD_LSL = "MSD_LSL";
            public const string MSD_UCL = "MSD_UCL";
            public const string MSD_LCL = "MSD_LCL";
            public const string MSD_CL = "MSD_CL";
            public const string MSD_TARGET = "MSD_TARGET";

            public const string MR_USL = "MR_USL";
            public const string MR_LSL = "MR_LSL";
            public const string MR_UCL = "MR_UCL";
            public const string MR_LCL = "MR_LCL";
            public const string MR_CL = "MR_CL";
            public const string MR_TARGET = "MR_TARGET";

            //SPC-930, KBLEE, START
            public const string UTL = "UTL";
            public const string LTL = "LTL";
            public const string UPPER_TECHNICAL_LIMIT = "UPPER_TECHNICAL_LIMIT";
            public const string LOWER_TECHNICAL_LIMIT = "LOWER_TECHNICAL_LIMIT";
            public const string TREND_LIMIT = "Trend limit";
            //SPC-930, KBLEE, END

            public const string RUCL = "RUCL";
            public const string RCL = "RCL";
            public const string RLCL = "RLCL";
            public const string SUCL = "SUCL";
            public const string SCL = "SCL";
            public const string SLCL = "SLCL";

            public const string SPC_MODEL_NAME = "SPC_MODEL_NAME";
            public const string PARAM_NAME = "PARAM_NAME";

            //2009-11-03 추가 
            public const string SUBITEM = "SUBITEM";
            public const string VALUE = "VALUE";
            public const string SAMPLE_COUNT = "SAMPLE_COUNT";
            public const string SAMPLE = "SAMPLE";
            public const string MOCVDNAME = "MOCVDNAME";

            public const string SD = "SD"; //추가

            public const string PREVIEWUPPERLIMIT = "PREVIEWUPPERLIMIT";
            public const string PREVIEWLOWERLIMIT = "PREVIEWLOWERLIMIT";

            public const string RESTRICT_SAMPLE_DAYS = "RESTRICT_SAMPLE_DAYS";
            public const string RESTRICT_SAMPLE_COUNT = "RESTRICT_SAMPLE_COUNT";
            public const string RESTRICT_SAMPLE_HOURS = "RESTRICT_SAMPLE_HOURS";

            public const string DTSOURCEID = "DTSOURCEID";
            public const string ORDERINFILEDATA = "ORDERINFILEDATA";
            public const string TOTALDATA = "TOTALDATA";
            public const string TABLENAME = "TABLEDATA";

            public const string CPK_AVG = "CPK_AVG";
            public const string CPK_STDDEV = "CPK_STDDEV";
            public const string CPU = "CPU";
            public const string CPL = "CPL";
            public const string CPK_SUM = "CPK_SUM";
            public const string CPK_SUM_SQUARED = "CPK_SUM_SQUARED";
            public const string CPK_SAMPLE_QTY = "CPK_SAMPLE_QTY";

            public const string CA = "CA";
            public const string OOC_COUNT = "OOC_COUNT";
            public const string OOS_COUNT = "OOS_COUNT";
            public const string OCAP_LIST = "OCAP_LIST";

            //added by enkim 2012.08.24 for normal value
            public const string ORIGINAL_PARAM_VALUE_LIST = "ORIGINAL_PARAM_VALUE_LIST";
            public const string NORMAL_TYPE_LIST = "NORMAL_TYPE_LIST";
            public const string NORMAL_OPTION_LIST = "NORMAL_OPTION_LIST";
            public const string NORMAL_OPTION_VALUE_LIST = "NORMAL_OPTION_VALUE_LIST";
            //added end

            //added by enkim 2012.08.28 for ocap data
            public const string RAW_OCAP_LIST = "RAW_OCAP_LIST";
            public const string MEAN_OCAP_LIST = "MEAN_OCAP_LIST";
            public const string STDDEV_OCAP_LIST = "STDDEV_OCAP_LIST";
            public const string RANGE_OCAP_LIST = "RANGE_OCAP_LIST";
            public const string MA_OCAP_LIST = "MA_OCAP_LIST";
            public const string MSD_OCAP_LIST = "MSD_OCAP_LIST";
            public const string MR_OCAP_LIST = "MR_OCAP_LIST";
            public const string EWMARANGE_OCAP_LIST = "EWMARANGE_OCAP_LIST";
            public const string EWMAMEAN_OCAP_LIST = "EWMAMEAN_OCAP_LIST";
            public const string EWMASTDDEV_OCAP_LIST = "EWMASTDDEV_OCAP_LIST";
            //added end

            public const string PN_USL = "PN_USL";
            public const string PN_LSL = "PN_LSL";
            public const string C_USL = "C_USL";
            public const string C_LSL = "C_LSL";
            public const string PN_UCL = "PN_UCL";
            public const string PN_LCL = "PN_LCL";
            public const string C_UCL = "C_UCL";
            public const string C_LCL = "C_LCL";
            public const string P_UCL = "P_UCL";
            public const string P_LCL = "P_LCL";
            public const string U_UCL = "U_UCL";
            public const string U_LCL = "U_LCL";
            public const string P_USL = "P_USL";
            public const string P_LSL = "P_LSL";
            public const string U_USL = "U_USL";
            public const string U_LSL = "U_LSL";
            public const string P_TARGET = "P_TARGET";
            public const string PN_TARGET = "PN_TARGET";
            public const string C_TARGET = "C_TARGET";
            public const string U_TARGET = "U_TARGET";

            public const string P_OCAP_LIST = "P_OCAP_LIST";
            public const string PN_OCAP_LIST = "PN_OCAP_LIST";
            public const string C_OCAP_LIST = "C_OCAP_LIST";
            public const string U_OCAP_LIST = "U_OCAP_LIST";

            public static string RESTRICT_TYPE_CD = "RESTRICT_TYPE_CD";
        }


        public struct CHART_SERIES
        {                                                                    
            public const string AVG = "SPC_SERIES_AVG";
            public const string CONTROL_LIMIT = "SPC_SERIES_CONTROL_LIMIT";
            public const string MIN_MAX = "SPC_SERIES_MIN_MAX";
            public const string POINT = "SPC_SERIES_POINT";
            public const string SPEC = "SPC_SERIES_SPEC";
            public const string VAL = "SPC_SERIES_VAL";
        }
       

        public struct ChartInfomationData
        {
            public static string INFORMATION = "SPC_LABEL_INFORMATION";
            public static string CHART_CONTEXT = "SPC_LABEL_CHART_CONTEXT";
            public static string CHART_DATA = "SPC_LABEL_CHART_DATA";
            public static string DATA_RELATED = "SPC_LABEL_DATA_RELATED";
            public static string LINE = "SPC_LABEL_LINE";
            public static string PRODUCT_ID = "SPC_LABEL_PRODUCT_ID";
            public static string OPERATION_ID = "SPC_LABEL_OPERATION_ID";
            public static string OPERATION_NUMBER = "SPC_LABEL_OPERATION_NUMBER";
            public static string MEASURE_OPERATION_ID = "SPC_LABEL_MEASURE_OPERATION_ID";
            public static string AREA = "SPC_LABEL_AREA";
            public static string PARAM_NAME = "SPC_LABEL_PARAM_NAME";
            public static string PARAM_ALIAS = "SPC_LABEL_PARAM_ALIAS";
            public static string SPC_MODEL = "SPC_LABEL_MODEL";
            public static string EQP_MODEL = "SPC_LABEL_EQP_MODEL";
            public static string EQP_ID = "SPC_LABEL_EQP_ID";
            public static string MEASURE_EQP_ID = "SPC_LABEL_MEASURE_EQP_ID";
            public static string MODULE_NAME = "SPC_LABEL_MODULE_NAME";            
            public static string UPPER_SPEC = "SPC_LABEL_USL";
            public static string LOWER_SPEC = "SPC_LABEL_LSL";
            public static string UPPER_CONTROL = "SPC_LABEL_UCL";
            public static string LOWER_CONTROL = "SPC_LABEL_LCL";
            public static string RUCL = "SPC_LABEL_RUCL";
            public static string RCL = "SPC_LABEL_RCL";
            public static string RLCL = "SPC_LABEL_RLCL";
            public static string RANGE = "SPC_LABEL_RANGE";
            public static string SUCL = "SPC_LABEL_SUCL";
            public static string SCL = "SPC_LABEL_SCL";
            public static string SLCL = "SPC_LABEL_SLCL";
            public static string STDDEV = "SPC_LABEL_STDDEV";
            public static string TARGET = "SPC_LABEL_TARGET";
            public static string MIN = "SPC_LABEL_MIN";
            public static string MAX = "SPC_LABEL_MAX";
            public static string CP = "SPC_LABEL_CP";
            public static string CPK = "SPC_LABEL_CPK";
            public static string PP = "SPC_LABEL_PP";
            public static string PPK = "SPC_LABEL_PPK";
            public static string AVG = "SPC_LABEL_AVG";
            public static string TIME = "SPC_LABEL_TIME";
            public static string LOT_ID = "SPC_LABEL_LOT_ID";
            public static string SUBSTRATE_ID = "SPC_LABEL_SUBSTRATE_ID";
            public static string ANNOTATION_DATA = "SPC_LABEL_ANNOTATION_DATA";

        }

        //SPC-930, KBLEE, START
        public static string SPC_RULE_NO = "SPC_RULE_NO";
        public static string DATA_INDEX = "DATA_INDEX";
        public static string CHART_TYPE_STRING = "CHART_TYPE";
        public static string RULE_OPTION_VALUE = "RULE_OPTION_VALUE";
        public static string OPTION_NAME = "OPTION_NAME";
        //SPC-930, KBLEE, END

        public struct CHART_TYPE
        {
            public const string RAW = "RAW";
            public const string XBAR = "X-BAR";
            public const string RANGE = "RANGE";
            public const string STDDEV = "STDDEV";
            public const string EWMA_MEAN = "EWMA_MEAN";
            public const string EWMA_RANGE = "EWMA_RANGE";
            public const string EWMA_STDDEV = "EWMA_STDDEV";
            public const string MA = "MA";
            public const string MSD = "MSD";
            public const string MR = "MR";
            public const string SD = "SD"; //추가
            public const string SDRANGE = "SD_RANGE"; //추가

            //ANALYSIS
            public const string BOX = "BOX";
            public const string DOT_PLOT = "DOT_PLOT";
            public const string HISTOGRAM = "HISTOGRAM";
            public const string RUN = "RUN";
            public const string CA = "CA"; //추가

            public const string P = "P";
            public const string PN = "PN";
            public const string U = "U";
            public const string C = "C";
       
        }

        //SPC-930, KBLEE, START
        public struct CHART_TYPE_ALIAS
        {
            public const string MEAN = "MEAN";
            public const string STD = "STD";
            public const string EWMA = "EWMA";
            public const string MSTD = "MSTD";
        }
        //SPC-930, KBLEE, END

        //SPC-930, KBLEE, START
        public struct RULE_MODE
        {
            public const string BIGGER = "BIGGER";
            public const string SMALLER = "SMALLER";
            public const string BIGGER_N = "BIGGER_N";
            public const string SMALLER_N = "SMALLER_N";
            public const string BIGGER_NM = "BIGGER_NM";
            public const string SMALLER_NM = "SMALLER_NM";
            public const string BIGGER_OR_SMALLER_NM = "BIGGER_OR_SMALLER_NM";
            public const string RISING_N = "RISING_N";
            public const string FALLING_N = "FALLING_N";
            public const string RISING_FALLING_N = "RISING_FALLING_N";
            public const string IN_ONE_ZONE_N = "IN_ONE_ZONE_N";
            public const string IN_ONE_ZONE_NM = "IN_ONE_ZONE_NM";
            public const string IN_TWO_ZONE_NM = "IN_TWO_ZONE_NM";
            public const string BIGGER_TREND_N = "BIGGER_TREND_N";
            public const string SMALLER_TREND_N = "SMALLER_TREND_N";
            public const string BIGGER_TREND_LR_N = "BIGGER_TREND_LR_N";
            public const string SMALLER_TREND_LR_N = "SMALLER_TREND_LR_N";
        }
        //SPC-930, KBLEE, END


        public struct SELECT_CHART_COMBO_KEY  //추가
        {
            public const string RUN_CHART = "RUN_CHART";
            public const string C_A_CHART = "C_A_CHART";
        }


        #region : Chart Button Key
        public static string BUTTON_KEY_CHART_ZOOM_IN = "BTN_ZOOM_IN";
        public static string BUTTON_KEY_CHART_CHANGE_X_AXIS = "BTN_CHANGE_X_AXIS";
        public static string BUTTON_KEY_CHART_TOOL_TIP_COMMAND = "BTN_TOOL_TIP_COMMAND";
        public static string BUTTON_KEY_CHART_SNAP_SHOT = "BTN_SNAP_SHOT";
        public static string BUTTON_KEY_CHART_LEGEND_VISIBLITY = "BTN_LEGEND_VISIBLITY";
        public static string BUTTON_KEY_CHART_LEGEND_ALL_CHECK = "BTN_LEGEND_ALL_CHECK";
        public static string BUTTON_KEY_CHART_SERIES_SHIFT_HORI = "BTN_SERIES_SHIFT_HORI";
        public static string BUTTON_KEY_CHART_SERIES_SHIFT_VERT = "BTN_SERIES_SHIFT_VERT";
        public static string BUTTON_KEY_CHART_SAME_START_POINT = "BTN_SAME_START_POINT";
        public static string BUTTON_KEY_CHART_EXCEL_EXPORT = "BTN_EXCEL_EXPORT";
        public static string BUTTON_KEY_CHART_DELETE_BLANK = "BTN_DELETE_BLANK";
        public static string BUTTON_KEY_CHART_POINT_MARKING = "BTN_POINT_MARKING";
        public static string BUTTON_KEY_CHART_VARIATION = "BTN_VARIATION";
        public static string BUTTON_KEY_CHART_REALTIME = "BTN_REALTIME";
        public static string BUTTON_KEY_CHART_MULTI_TOOL_TIP = "BTN_MULTI_TOOL_TIP";
        public static string BUTTON_KEY_CHART_COPY_CHART = "BTN_COPY_CHART";
        public static string BUTTON_KEY_CHART_CHANGE_SERIES_STYLE = "BTN_CHANGE_SERIES_STYLE";
        public static string BUTTON_KEY_CHART_CHANGE_LEGEND_LOCATION = "BTN_CHANGE_LEGEND_LOCATION";
        #endregion

        #region : Chart KEY

        public static string Right = "Right";
        public static string Left = "Left";
        public static string Bottom = "Bottom";
        public static string Top = "Top";

        #endregion


        public struct VARIABLE_DATA_TYPE
        {
            public static string NOMINAL = "Nominal";
            public static string INTERVAL = "Interval";
        }

        public struct VARIABLE_DATA_TYPE_CD
        {
            public static string NOMINAL = "N";
            public static string INTERVAL = "I";
        }

        public struct VARIABLE_ROLE
        {
            public static string INPUT = "Input";
            public static string OUTPUT = "Output";
            public static string DATETIME = "DateTime";
            public static string UNDEFINED = "Undefined";
        }

        public struct VARIABLE_ROLE_CD
        {
            public static string INPUT = "I";
            public static string OUTPUT = "O";
            public static string UNDEFINED = "U";
        }

        public struct DATA_SET_TYPE
        {
            public static string TRAINING = "Training";
            public static string VALIDATION = "Validation";
            public static string TESTING = "Testing";
            public static string ERROR = "Error";
            public static string UNKNOWN = "Undefined";
        }

        //SPREAD HEADER COLUMN KEY        
        public struct SpreadHeaderColKey
        {
            //Common
            public static string V_INSERT = "SPC_V_INSERT";
            public static string V_SELECT = "SPC_V_SELECT";
            public static string V_MODIFY = "SPC_V_MODIFY";
            public static string V_DELETE = "SPC_V_DELETE";
            public static string ERROR_ROW = "SPC_ERROR_ROW";
            public static string RAWID = "SPC_RAWID";
            public static string ROW_KEY = "SPC_ROW_KEY";
            public static string MODEL = "SPC_MODEL";

            //SPC MODELING
            public static string MAIN_CHART = "SPC_MAIN_CHART";
            public static string PARAMETER_NAME = "SPC_LABEL_PARAMETER_NAME";
            public static string EQP_ID = "SPC_LABEL_EQP_ID";
            public static string CHAMBER_ID = "SPC_LABEL_CHAMBER_ID";
            public static string OPERATION_ID = "SPC_LABEL_OPERATION_ID";
            public static string OPERATION_NUMBER = "SPC_LABEL_OPERATION_NUMBER";
            public static string OPERATION_DESC = "SPC_DESCRIPTION";
            public static string PRODUCT_ID = "SPC_PRODUCT_ID";

            //SPC CONFIGURATION
            public static string CONTEXT_ITEM = "SPC_CONTEXT_ITEM";
            public static string CONTEXT_VALUE = "SPC_CONTEXT_VALUE";
            public static string EXCLUDE_LIST = "SPC_EXCLUDE_LIST";
            public static string EXCLUDE_YN = "EXCLUDE_YN";

            public static string RULE = "SPC_RULE";
            public static string RULE_OPTION = "SPC_RULE_OPTION";
            public static string OCAP = "SPC_OCAP";

            //OCAP LIST
            public static string NO = "SPC_NO";
            public static string STATUS = "SPC_STATUS";
            public static string PARAMETER = "SPC_PARAMETER";
            public static string PARAM_ALIAS = "SPC_PARAM_ALIAS";
            public static string DATA_TRX_RAWID = "SPC_DATA_TRX_RAWID";
            public static string MODEL_CONFIG_RAWID = "SPC_MODEL_CONFIG_RAWID";
            public static string DEFAULT_CHART_LIST = "SPC_DEFAULT_CHART_LIST";
            public static string MODULE_NAME = "SPC_MODULE_NAME";
            public static string OOC_TRX_SPC_RAWID = "SPC_OOC_TRX_SPC_RAWID";
            public static string OOC_PROBLEM = "SPC_OOC_PROBLEM";
            public static string OOC_CAUSE = "SPC_OOC_CAUSE";
            public static string OOC_SOLUTION = "SPC_OOC_SOLUTION";
            public static string OOC_COMMENT = "SPC_OOC_COMMENT";
            public static string FALSE_ALARM_YN = "SPC_FALSE_ALARM_YN";
            public static string LOT_ID = "SPC_LOT_ID";
            public static string SUBSTRATE_ID = "SPC_SUBSTRATE_ID";
            public static string AREA = "SPC_AREA";

            //공정능력     

            public static string STEP_ID = "SPC_STEP_ID";
            public static string SPEC_TYPE = "SPC_SPEC_TYPE";
            public static string SPEC = "SPC_SPEC";
            public static string LSL = "SPC_LSL";
            public static string USL = "SPC_USL";
            public static string LCL = "SPC_LCL";
            public static string UCL = "SPC_UCL";
            public static string MEAN = "SPC_MEAN";
            public static string STDEV = "SPC_STDEV";

            public static string SUM = "SPC_SUM";
            public static string PP = "SPC_PP";
            public static string PPK = "SPC_PPK";

            public static string AVG = "SPC_AVG";
            public static string STDDEV = "SPC_STDDEV";
            public static string SUM_SQUARED = "SPC_SUM_SQUARED";
            public static string PPU = "SPC_PPU";
            public static string PPL = "SPC_PPL";
            public static string MIN = "SPC_MIN";
            public static string MAX = "SPC_MAX";
            public static string LOT_QTY = "SPC_LOT_QTY";
            public static string SAMPLE_QTY = "SPC_SAMPLE_QTY";
            public static string DEFAULT_CHART = "SPC_DEFAULT_CHART";
            public static string PERIOD = "SPC_PERIOD";

            public static string CPK_AVG = "SPC_CPK_AVG";
            public static string CPK_STDDEV = "SPC_CPK_STDDEV";
            public static string CPK = "SPC_CPK";
            public static string CP = "SPC_CP";
            public static string CPU = "SPC_CPU";
            public static string CPL = "SPC_CPL";
            public static string CPK_SUM = "SPC_CPK_SUM";
            public static string CPK_SUM_SQUARED = "SPC_CPK_SUM_SQUARED";
            public static string CPK_SAMPLE_QTY = "SPC_CPK_SAMPLE_QTY";

            public static string CA = "SPC_CA";
            public static string OOC_COUNT = "SPC_OOC_COUNT";
            public static string OOS_COUNT = "SPC_OOS_COUNT";

            public static string KEY = "SPC_KEY";
            public static string VALUE = "SPC_VALUE";

            //SPC CHART

            public static string INFOMATION = "Information";
            public static string GROUP_KEY = "GROUP_KEY";
            public static string INFO_KEY = "SPC_KEY";
            public static string INFO_VALUE = "SPC_VALUE";
            public static string INFO_MAPPING = "INFO_MAPPING";

        }

        //BUTTON KEY
        public struct ButtonKey
        {
            public static string SELECT_CHART_TO_VIEW = "SPC_SELECT_CHART_TO_VIEW";
            public static string DEFAULT_CHART = "SPC_DEFAULT_CHART";
            public static string CALC = "SPC_CALC";
            public static string CHART_DATA = "SPC_CHART_DATA";
            public static string SHOW_EXCLUDED = "SPC_SHOW_EXCLUDED";
            public static string CONFIGURATION = "SPC_CONFIGURATION";
            public static string VIEW_CHART = "SPC_VIEW_CHART";
            public static string IMPORT = "IMPORT";
            public static string EXPORT = "SPC_EXPORT";
            public static string OCAP_VIEW = "OCAP_VIEW";
            public static string OCAP_MODIFY = "OCAP_MODIFY";
            public static string VIEW = "VIEW";
            public static string MODIFY = "MODIFY";
            public static string SORT = "SORT";
            public static string CONTROL_CHART = "SPC_CONTROL_CHART";
            public static string ANALYSIS_CHART = "SPC_ANALYSIS_CHART";
            public static string CUSTOM_CONTEXT = "SPC_CUSTOM_CONTEXT";

            public static string CREATE_MAINMODEL = "SPC_CREATE_MAINMODEL";
            public static string ADD_SUBCONFIG = "SPC_ADD_SUBCONFIG";
            public static string MODIFY_MODEL = "SPC_MODIFY_MODEL";
            public static string DELETE_SUBCONFIG = "SPC_DELETE_SUBCONFIG";
            public static string DELETE_MODEL = "SPC_DELETE_MODEL";
            public static string SAVE = "SAVE";
            public static string SAVEAS = "SAVEAS"; 

            public static string SEARCH_BY_AREA = "SEARCH_BY_AREA";

            public static string BTN_RIGHT = "BTN_ROUND_RIGHT"; //추가
            public static string BTN_LEFT = "BTN_ROUND_LEFT"; //추가

            public static string BTN_HISTORY = "SPC_HISTORY";

            public const string VERSION_HISTORY = "SPC_VERSION_HISTORY";
            public const string VERSION_COMPARE = "SPC_VERSION_COMPARE";
            public const string VERSION_DELETE  = "SPC_VERSION_DELETE";

            //SPC-929, KBLEE, START
            public const string SUMMARY_INSERT = "SPC_SUMMARY_INSERT";
            public const string SUMMARY_RESULT = "SPC_SUMMARY_RESULT";
            //SPC-929, KBLEE, END
        }

        public static string BUTTON_KEY_DEFAULTSETTING = "DEFAULTSETTING";
        public static string BUTTON_KEY_ADD = "ADD";
        public static string BUTTON_KEY_CLOSE = "CLOSE";
        public static string BUTTON_KEY_OK = "OK";
        public static string BUTTON_KEY_CANCEL = "CANCEL";
        public static string BUTTON_KEY_ADD_PROP = "ADD_PROP";
        public static string BUTTON_KEY_ADD_MODEL = "ADD_MODEL";
        public static string BUTTON_KEY_REMOVE = "REMOVE";
        public static string BUTTON_KEY_MODIFY = "MODIFY";
        public static string BUTTON_KEY_DELETE = "DELETE";
        public static string BUTTON_KEY_UP = "UP";
        public static string BUTTON_KEY_DOWN = "DOWN";
        public static string BUTTON_KEY_GET_VALUE = "GET_VALUE";
        public static string BUTTON_KEY_GET_EXCLUDE_VALUE = "GET_EXCLUDE_VALUE";
        public static string BUTTON_KEY_BULLET = "BULLET_SPC";
        public static string BUTTON_KEY_SAVE = "SAVE";

        #region : BUTTON LIST ITEM KEY

        public static string BUTTONLIST_KEY_MODELING = "BTNLIST_SPC_MODELING";
        public static string BUTTONLIST_KEY_OCAP_LIST = "BTNLIST_SPC_OCAP_LIST";
        public static string BUTTONLIST_KEY_CHART_DATA = "BTNLIST_SPC_CHART_DATA";
        public static string BUTTONLIST_KEY_PROCESS_CAPABILITY = "BTNLIST_PROCESS_CAPABILITY";
        public static string BUTTONLIST_KEY_MET_PROCESS_CAPABILITY = "BTNLIST_MET_PROCESS_CAPABILITY";        
        public static string BUTTONLIST_KEY_PROCESS_CAPABILITY_POPUP = "BTNLIST_PROCESS_CAPABILITY_POPUP";
        public static string BUTTONLIST_KEY_SPC_CONTROL_CHART = "BTNLIST_SPC_CONTROL_CHART";
        public static string BUTTONLIST_KEY_MULTIDATA = "BTNLIST_MULTIDATA";
        public static string BUTTONLIST_KEY_ANALYSIS = "BTNLIST_ANALYSIS"; //추가

        public static string BUTTONLIST_KEY_SPC_MODEL_HISTORY_LIST = "BTNLIST_SPC_MODEL_HISTORY_LIST";

        public static string BUTTONLIST_KEY_CHART = "BTNLIST_SPC_CHART";
        public static string BUTTONLIST_KEY_MET_CHART = "BTNLIST_SPC_MET_CHART";
        public static string BUTTONLIST_KEY_MONTHLY_CPK_CHART = "BTNLIST_MONTHLY_CPK_CHART";
        public static string BUTTONLIST_KEY_QUERY_BY_CHART = "BTNLIST_QUERY_BY_CHART";

        public static string BUTTONLIST_KEY_ATT_OCAP_LIST = "BTNLIST_SPC_ATT_OCAP_LIST";
        public static string BUTTONLIST_KEY_ATT_CHART = "BTNLIST_SPC_ATT_CHART";
        public static string BUTTONLIST_KEY_ATT_PROCESS_CAPABILITY = "BTNLIST_ATT_PROCESS_CAPABILITY";        

        #endregion


        #region : COL HEADER KEY

        //Common
        public static string COL_HEADER_KEY_V_INSERT = "SPC_V_INSERT";
        public static string COL_HEADER_KEY_V_SELECT = "SPC_V_SELECT";
        public static string COL_HEADER_KEY_V_MODIFY = "SPC_V_MODIFY";
        public static string COL_HEADER_KEY_V_DELETE = "SPC_V_DELETE";
        public static string COL_HEADER_KEY_ERROR_ROW = "SPC_ERROR_ROW";
        public static string COL_HEADER_KEY_RAWID = "SPC_RAWID";
        public static string COL_HEADER_KEY_ROW_KEY = "SPC_ROW_KEY";
        public static string COL_HEADER_KEY_MODEL = "SPC_MODEL";
        public static string COL_HEADER_KEY_VALUE = "SPC_VALUE";

        public static string COL_HEADER_KEY_CONTEXT_KEY = "SPC_CONTEXT_KEY";
        public static string COL_HEADER_KEY_CONTEXT_NAME = "SPC_CONTEXT_NAME";
        public static string COL_HEADER_KEY_CONTEXT_VALUE = "SPC_CONTEXT_VALUE";
        public static string COL_HEADER_KEY_EXCLUDE_LIST = "SPC_EXCLUDE_LIST";
        public static string COL_HEADER_KEY_ORDER = "SPC_ORDER";

        //public static string COL_HEADER_KEY_FILTER_KEY = "SPC_FILTER_KEY";
        //public static string COL_HEADER_KEY_FILTER_VALUE = "SPC_FILTER_VALUE";

        public static string COL_HEADER_KEY_RULE_NO = "SPC_RULE_NO";
        public static string COL_HEADER_KEY_RULE_OPTION = "SPC_RULE_OPTION";
        public static string COL_HEADER_KEY_RULE_OPTION_NO = "SPC_RULE_OPTION_NO";
        public static string COL_HEADER_KEY_RULE_OPTION_NAME = "SPC_OPTION_NAME";
        public static string COL_HEADER_KEY_USE_MASTER = "SPC_USE_MASTER";
        public static string COL_HEADER_KEY_USE_MASTER_YN = "SPC_USE_MASTER_YN";
        public static string COL_HEADER_KEY_OCAP = "SPC_OCAP";

        public static string COL_HEADER_KEY_CODE = "SPC_CODE";
        public static string COL_HEADER_KEY_NAME = "SPC_NAME";
        public static string COL_HEADER_KEY_DESCRIPTION = "SPC_DESCRIPTION";

        public static string COL_HEADER_KEY_PARAM_ALIAS = "SPC_PARAM_ALIAS";
        public static string COL_HEADER_KEY_SPC_GROUP = "SPC_GROUP";

        public static string COL_HEADER_KEY_SPC_AUTO_CALCULATE = "SPC_AUTO_CALCULATE";
        public static string COL_HEADER_KEY_SPC_CALCULATE_COUNT = "SPC_CALCULATE_COUNT";
        public static string COL_HEADER_KEY_SPC_CALCULATION_OPTION = "SPC_CALCULATION_OPTION";
        public static string COL_HEADER_KEY_SPC_CALCULATION_VALUE = "SPC_CALCULATION_VALUE";
        public static string COL_HEADER_KEY_SPC_CALCULATION_SIDED = "SPC_CALCULATION_SIDED";

        public static string COL_HEADER_KEY_SPC_LOCATION_RAWID = "SPC_LOCATION_RAWID";
        public static string COL_HEADER_KEY_SPC_AREA_RAWID = "SPC_AREA_RAWID";


        #endregion


        #region : FILE PATH

        public static string PATH_APPLICATION = "Application";
        public static string PATH_ESPC = "eSPC";
        public static string PATH_XML_CONFIGURATION = "/Configuration/Configuration.xml";
        public static string PATH_FIELDDEFINITION = "/RMSFieldDefinition.xml";
        public static string PATH_COMMON_FIELD = "/COMMON_FIELD.xml";
        public static string PATH_SCP_OCAP_DETAILS_POPUP = "SPC_OCAP_DETAILS_POPUP.xml";
        public static string PATH_SPC_MODELING = "SPC_MODELING.xml";
        public static string PATH_SPC_ATT_MODELING = "SPC_ATT_MODELING.xml";

        #endregion

        #region : XML VARIABLE

        public static string XML_VARIABLE_INDEX = "index";
        public static string XML_VARIABLE_KEY = "key";
        public static string XML_VARIABLE_BUTTONKEY = "ButtonKey";
        public static string XML_VARIABLE_BUTTONVISIBLE = "ButtonVisible";
        public static string XML_VARIABLE_USERCONTEXTMENU = "UserContextMenu";
        public static string XML_VARIABLE_CONTEXTKEY = "ContextKey";
        public static string XML_VARIABLE_CONTEXTVISIBLE = "ContextVisible";
        public static string XML_VARIABLE_NAME = "name";
        public static string XML_VARIABLE_MAXBYTES = "MaxBytes";
        public static string XML_VARIABLE_FIELDNAME = "FieldName";
        public static string XML_VARIABLE_TABLENAME = "TableName";

        public static string XML_VARIABLE_HEADERKEY = "HeaderKey";
        public static string XML_VARIABLE_HEADNAME = "HeadName";
        public static string XML_VARIABLE_FIELD = "Field";
        public static string XML_VARIABLE_WIDTH = "Width";
        public static string XML_VARIABLE_MAXLENGTH = "MaxLength";
        public static string XML_VARIABLE_SAVETABLE = "SaveTable";
        public static string XML_VARIABLE_SAVEFIELD = "SaveField";
        public static string XML_VARIABLE_SEQUENCETABLE = "SequenceTable";
        public static string XML_VARIABLE_COLUMNATTRIBUTE = "ColumnAttribute";
        public static string XML_VARIABLE_COLUMNTYPE = "ColumnType";
        public static string XML_VARIABLE_COLUMNDATA = "ColumnData";
        public static string XML_VARIABLE_CODEGROUPTEXT = "CodeGroupText";
        public static string XML_VARIABLE_DEFAULTVALUE = "DefaultValue";
        public static string XML_VARIABLE_COMBOVISIBLE = "ComboVisible";
        public static string XML_VARIABLE_COLUMNVISIBLE = "ColumnVisible";
        public static string XML_VARIABLE_DEFAULTVISIBLE = "DefaultVisible";
        public static string XML_VARIABLE_CONTEXTMENUNAME = "ContextMenuName";

        public static string XML_VARIABLE_TOOLTIPLABEL = "ToolTipLabel";
        public static string XML_VARIABLE_TOOLTIPKEY = "ToolTipKey";
        public static string XML_VARIABLE_TOOLTIPVISIBLE = "ToolTipVisible";

        public static string XML_VARIABLE_ADDITIONALSERIESCOLOR = "SeriesColor";
        public static string XML_VARIABLE_ADDITIONALXCOLUMN = "XColumn";
        public static string XML_VARIABLE_ADDITIONALYCOLUMN = "YColumn";
        public static string XML_VARIABLE_ADDITIONALSERIESTYPE = "SeriesType";
        public static string XML_VARIABLE_ADDITIONALLABELCOLUMN = "LabelColumn";
        public static string XML_VARIABLE_ADDITIONALEXTRACTDATAMETHOD = "ExtractDataMethod";
        public static string XML_VARIABLE_ADDITIONALDISPLAYPOSITION = "DisplayPosition";
        public static string XML_VARIABLE_ADDITIONALISSAMECONTENTSAMELINE = "IsSameContentSameLine";
        public static string XML_VARIABLE_ADDITIONALISVISIBLE = "IsVisible";
        public static string XML_VARIABLE_ADDITIONALSTYLE = "Style";
        public static string XML_VARIABLE_ADDITIONALCONSTANTVALUE = "ConstantValue";
        public static string XML_VARIABLE_ADDITIONALCONTENTNAME = "ContentName";
        public static string XML_VARIABLE_ADDITIONALADDCOLORVALUESTRING = "AddColorValueString";
        public static string XML_VARIABLE_ADDITIONALADDCOLORVALUECOLOR = "AddColorValueColor";

        public static string XML_VARIABLE_Type = "Type";
        public static string XML_VARIABLE_Value = "Value";
        public static string XML_VARIABLE_ExponentialAxis = "ExponentialAxis";
        public static string XML_VARIABLE_ChartModeGrouping = "ChartModeGrouping";
        public static string XML_VARIABLE_ShowOOCCount = "ShowOOCCount";
        public static string XML_VARIABLE_ShowCPK = "ShowCPK";
        // JIRA SPC-592 [GSMC] Disable "Use Normalization Value" - 2011.08.29 by ANDREW KO
        public static string XML_VARIABLE_ShowNormalization = "ShowNormalization";
        // JIRA No.SPC-600 Change unit of Minimum Samples to flexable unit (like Lot, Wafer...) - 2011.09.10 by ANDREW KO
        // No.5 on GF Communication Sheet 20110612 V1.21 - 2011.09.10 by ANDREW KO
        public static string XML_VARIABLE_SetUnitOfSamples = "SetUnitOfSamples";
        public static string XML_VARIABLE_OCAPSelectOfSingle = "OCAPSelectOfSingle";

        #endregion

        public struct DynamicCondition_Condition_key
        {

            public static string PERIOD_PPK = "PERIOD_PPK";
            public static string PERIOD = "PERIOD";
            
            public static string VALUEDATA = "VALUEDATA";
            public static string DISPLAYDATA = "DISPLAYDATA";
            public static string DATETIME_VALUEDATA = "DATETIMEVALUEDATA";
            public static string CATEGORY = "CATEGORY";
            public static string USE_YN = "USE_YN";
            public static string EQP = "EQP_ID";
            public static string MODULE = "MODULENAME";
            public static string TRACE_PARAM_TYPE = "TRACE_PARAM_TYPE";
            public static string PARAM = "PARAM";
            public static string EQPMODEL_VERSION_RAWID = "EQPMODEL_VERSION_RAWID";
            public static string PRODUCT_TYPE = "PRODUCT_TYPE";
            public static string PRODUCT = "PRODUCT";
            public static string RECIPE_TYPE = "RECIPE_TYPE";
            public static string RECIPE = "RECIPE";
            public static string STEP_TYPE = "STEP_TYPE";
            public static string STEP = "STEP";
            public static string STEP_ID = "STEP_ID";
            public static string OPERATION_ID = "OPERATION_ID";            
            public static string OPERATION_TYPE = "OPERATION_TYPE";
            public static string TARGET = "TARGET";
            public static string TARGET_TYPE = "TARGET_TYPE";            
            
            public static string TRACE_SUM_PARAM_TYPE = "SUM_PARAM_TYPE";
            public static string EVENT_SUM_PARAM_TYPE = "EVENT_PARAM_TYPE";
            public static string TRACE_SUM_PARAM = "TRACE_SUM_PARAM";
            public static string EVENT_SUM_PARAM = "EVENT_SUM_PARAM";
            public static string TYPE = "TYPE";
            public static string GROUP = "GROUP";
            public static string LINE_RAWID = "LINE_RAWID";
            public static string AREA = "AREA";
            public static string AREA_RAWID = "AREA_RAWID";
            public static string EQP_MODEL = "EQP_MODEL";
            public static string EQP_ID = "EQP_ID";
            public static string MAIN_YN = "MAIN_YN";            
            public static string MODULE_ID = "MODULE_ID";
            public static string MODULE_NAME = "MODULE_NAME";
            public static string MODULE_LEVEL = "MODULE_LEVEL";
            public static string ALIAS = "ALIAS";
            public static string RAWID = "RAWID";
            public static string PRODUCT_ID = "PRODUCT_ID";
            public static string LOT_ID = "LOT_ID";
            public static string RECIPE_ID = "RECIPE_ID";
            public static string SUBSTRATE_ID = "SUBSTRATE_ID";
            public static string STEP_NAME = "STEP_NAME";
            public static string PARAM_ALIAS = "PARAM_ALIAS";
            public static string EQP_RAWID = "EQP_RAWID";
            public static string PARENT = "PARENT";
            public static string GROUP_NAME = "GROUP_NAME";
            public static string EQP_PG_RAWID = "EQP_PG_RAWID";
            public static string DCP_ID = "DCP_ID";
            public static string LINE = "LINE";
            public static string SITE = "SITE";
            public static string PARAM_TYPE_CD = "PARAM_TYPE_CD";
            public static string SPC_MODEL_RAWID = "SPC_MODEL_RAWID";
            public static string METHOD_NAME = "METHOD_NAME";
            public static string FAB = "FAB";
            public static string SPC_PARAM_CATEGORY_CD = "SPC_PARAM_CATEGORY_CD";
            public static string START_DTTS = "START_DTTS";
            public static string END_DTTS = "END_DTTS";
            public static string MODEL_CONFIG_RAWID = "MODEL_CONFIG_RAWID";
            public static string GROUP_RAWID = "GROUP_RAWID";

            //SPc-704 MultiCalculation
            

            public static string MODEL_RAWID = "MODEL_RAWID";
            public static string OCAP_RAWID = "OCAP_RAWID";
            public static string OCAP_DATASET = "OCAP_DATASET";
            public static string OCAP_DATATABLE = "OCAP_DATATABLE";
            public static string USER_ID = "USER_ID";
            public static string INTERLOCK_YN = "INTERLOCK_YN";
            public static string ACTIVATION_YN = "ACTIVATION_YN";

            public static string CODE = "CODE";
            public static string CODE_NAME = "NAME";
            public static string CODE_DESCRIPTION = "DESCRIPTION";

            public static string SORTING_KEY = "SORTING_KEY";
            public static string COLUMN_LIST = "COLUMN_LIST";
            
            //2009-11-10
            public static string SUB_DATA = "SUB_DATA";
            public static string CONTEXT_KEY = "CONTEXT_KEY";            
            public static string CONTEXT_VALUE = "CONTEXT_VALUE";
            public static string CONTEXT_KEY_LIST = "CONTEXT_KEY_LIST";
            public static string RESTRICT_SAMPLE_DAYS = "RESTRICT_SAMPLE_DAYS";
            public static string DEFAULT_CHART_LIST = "DEFAULT_CHART_LIST";
            public static string MEASURE_EQP_ID = "MEASURE_EQP_ID";
            public static string MEASURE_OPERATION_ID = "MEASURE_OPERATION_ID";
            public static string EXCLUDE = "EXCLUDE";
            
            
        }

        public struct DynamicCondition_Search_key
        {
            public static string DATETIME_FROM = "ESPC_DATETIME_FROM";
            public static string DATETIME_TO = "ESPC_DATETIME_TO";
            public static string DATETIME_PERIOD = "ESPC_DATETIME_PERIOD";
            public static string RESTRICT_SAMPLE_COUNT = "ESPC_RESTRICT_SAMPLE_COUNT";
            public static string RESTRICT_SAMPLE_DAYS = "ESPC_RESTRICT_SAMPLE_DAYS";
            public static string SEARCH_COUNT = "ESPC_SEARCH_COUNT";
            public static string SPC_MODEL_CONFIG_RAWID = "ESPC_MODEL_CONFIG_RAWID";
                                                
            public static string SITE = "ESPC_SITE";            
            public static string FAB = "ESPC_FAB";
            public static string LINE = "ESPC_LINE";
            public static string AREA = "ESPC_AREA";
            public static string EQPMODEL = "ESPC_EQPMODEL";
            public static string EQPMODEL_PRODUCTS = "ESPC_EQPMODEL_PRODUCTS";
            public static string EQP_ID = "ESPC_EQP_ID";
            public static string MODULE = "ESPC_MODULENAME";
            public static string PRODUCT = "ESPC_PRODUCT";            
            public static string PRODUCTS = "ESPC_PRODUCTS";
            public static string VALUEDATA = "VALUEDATA";
            public static string DISPLAYDATA = "DISPLAYDATA";
            public static string SPC_MODEL = "ESPC_MODEL";
            public static string SPCMODEL = "ESPC_SPCMODEL";
            public static string COMBO_TREE = "ESPC_COMBO_TREE";
            public static string OPERATION = "ESPC_OPERATION";
            public static string PERIOD_PPK = "ESPC_PERIOD_PPK";
            public static string SORTING_KEY = "ESPC_SORTING_KEY";


            public static string PERIOD = "DATETIME_PERIOD";
            public static string FROMDATE = "DATETIME_FROM";
            public static string TODATE = "DATETIME_TO";

            public static string DCP_ID = "ESPC_DCP_ID";
            public static string PRODUCT_TYPE = "ESPC_PRODUCT_TYPE";
            public static string RECIPE_TYPE = "ESPC_RECIPE_TYPE";
            public static string RECIPE = "ESPC_RECIPE";
            public static string SUBSTRATE_ID = "ESPC_SUBSTRATE_ID";
            public static string TRACE_SUM_PARAM_TYPE = "ESPC_SUM_PARAM_TYPE";
            public static string EVENT_SUM_PARAM_TYPE = "ESPC_EVENT_PARAM_TYPE";
            public static string STEP_TYPE = "ESPC_STEP_TYPE";
            public static string STEP = "ESPC_STEP";
            public static string TRACE_SUM_PARAM = "ESPC_TRACE_SUM_PARAM";
            public static string EVENT_SUM_PARAM = "ESPC_EVENT_SUM_PARAM";
            public static string ADDTIONALVALUEDATA = "ADDTIONALVALUEDATA";
            public static string CATEGORY = "ESPC_CATEGORY";
            public static string METHOD_NAME = "ESPC_METHOD_NAME";
            public static string SPEC_GROUP = "ESPC_SPEC_GROUP";
            public static string TYPE = "ESPC_TYPE";
            public static string EVENT_PARAM_TYPE = "ESPC_EVENT_PARAM_TYPE";
            public static string PARAM = "ESPC_PARAM";
            public static string TRACE_PARAM_TYPE = "ESPC_TRACE_PARAM_TYPE";
            public static string SPEC_GROUP_TYPE = "ESPC_SPEC_GROUP_TYPE";
            public static string PARAM_TYPE = "ESPC_PARAM_TYPE";
            public static string PARAM_CATEGORY_CD = "ESPC_PARAM_CATEGORY_CD";

            //2009-11-10
            public static string ITEM = "ESPC_ITEM";            
            public static string SUBDATA = "ESPC_SUBDATA";            
            public static string CHART_LIST = "ESPC_CHART_LIST";
            public static string INFORMATION = "ESPC_INFORMATION";
            public static string PROBE = "ESPC_PROBE";            

            public static string TARGET = "ESPC_TARGET";
            public static string TARGET_TYPE = "ESPC_TARGET_TYPE";
            public static string TARGET_INFORMATION = "ESPC_TARGET_INFORMATION";
            public static string TARGET_ITEM = "ESPC_TARGET_ITEM";
            public static string TARGET_SUBDATA = "ESPC_TARGET_SUBDATA";
            public static string TARGET_PROBE = "ESPC_TARGET_PROBE";

            public static string BASE = "ESPC_BASE"; //추가
            public static string BASE_TYPE = "ESPC_BASE_TYPE"; //추가
            public static string BASE_INFORMATION = "ESPC_BASE_INFORMATION"; //추가
            public static string BASE_ITEM = "ESPC_BASE_ITEM"; //추가
            public static string BASE_SUBDATA = "ESPC_BASE_SUBDATA"; //추가
            public static string BASE_PROBE = "ESPC_BASE_PROBE"; //추가


            public static string CONTEXT_LIST = "ESPC_CONTEXT_LIST";
            public static string CUSTOM_CONTEXT = "ESPC_CUSTOM_CONTEXT";
            public static string CONTEXT = "ESPC_CONTEXT";
            public static string CHART_ID = "ESPC_CHART_ID";

            public static string RESTRICT_CONDITION = "ESPC_RESTRICT_CONDITION";

            public static string PARAM_ALIAS = "ESPC_PARAM_ALIAS";
        }


        public struct User_Virtual_Column
        {
            public static string PARAM = "PARAM";
            public static string CONTEXT = "CONTEXT";


        }

        public struct CONTEXT_LIST_KEY
        {
            public static string LOT_ID = "LOT_ID";
            public static string LOT_TYPE_CD = "LOT_TYPE_CD";
            public static string SUBSTRATE_ID = "SUBSTRATE_ID";
            public static string CASSETTE_SLOT = "CASSETTE_SLOT";
            public static string PRODUCT_ID = "PRODUCT_ID";
            public static string STEP_ID = "STEP_ID";
            public static string OPERATION_ID = "OPERATION_ID";
            public static string STATUS_CD = "STATUS_CD";            
            public static string RECIPE_ID = "RECIPE_ID";                                                
            public static string EQP_ID = "EQP_ID";
            public static string MODULE_NAME = "MODULE_NAME";            
            public static string CONTEXT_KEY = "CONTEXT_KEY";            
        }

        #region : CONDITION KEY

        public static string CONDITION_KEY_V_SELECT = "SPC_V_SELECT";
        public static string CONDITION_KEY_CODE_CATEGORY = "SPC_RAWID";       
        public static string CONDITION_KEY_RAWID = "RAWID";
        public static string CONDITION_KEY_LINE = "LINE";
        public static string CONDITION_KEY_LINE_RAWID = "LINE_RAWID";

        public static string CONDITION_KEY_AREA = "AREA";
        public static string CONDITION_KEY_AREA_RAWID = "AREA_RAWID";

        public static string CONDITION_KEY_MODEL_RAWID = "MODEL_RAWID";
        public static string CONDITION_KEY_MODEL_NAME = "MODEL_NAME";

        public static string CONDITION_KEY_MODEL_CONFIG_RAWID = "MODEL_CONFIG_RAWID";
        public static string CONDITION_KEY_SPC_MODEL_NAME = "SPC_MODEL_NAME";
        public static string CONDITION_KEY_OOC_TRX_SPC_RAWID = "OOC_TRX_SPC_RAWID";

        public static string CONDITION_KEY_SPC_PARAM_CATEGORY_CD = "SPC_PARAM_CATEGORY_CD";

        public static string CONDITION_KEY_SUM_CATEGORY_CD = "SUM_CATEGORY_CD"; //SPC-929, KBLEE
        public static string CONDITION_KEY_LOCATION_RAWID = "LOCATION_RAWID";
        public static string CONDITION_KEY_EQPMODEL = "EQPMODEL";
        public static string CONDITION_KEY_FILTER = "FILTER";

        public static string CONDITION_KEY_CASSETTE_SLOT = "CASSETTE_SLOT";
        public static string CONDITION_KEY_OPERATION_ID = "OPERATION_ID";
        public static string CONDITION_KEY_CONTEXT_KEY = "CONTEXT_KEY";
        public static string CONDITION_KEY_CONTEXT_LIST = "CONTEXT_LIST";
        public static string CONDITION_KEY_CONTEXT_KEY_LIST = "CONTEXT_KEY_LIST";

        public static string CONDITION_KEY_OOC_OCAP_RAWID = "OCAP_RAWID";
        public static string CONDITION_KEY_OOC_PROBLEM = "OOC_PROBLEM";
        public static string CONDITION_KEY_OOC_CAUSE = "OOC_CAUSE";
        public static string CONDITION_KEY_OOC_SOLUTION = "OOC_SOLUTION";
        public static string CONDITION_KEY_OOC_COMMENT = "OOC_COMMENT";
        public static string CONDITION_KEY_FALSE_ALARM_YN = "FALSE_ALARM_YN";

        public static string CONDITION_KEY_EQP_ID = "EQP_ID";
        public static string CONDITION_KEY_LOT_ID = "LOT_ID";
        public static string CONDITION_KEY_STEP_ID = "STEP_ID";

        public static string CONDITION_KEY_EQP_MODEL = "EQP_MODEL";

        public static string CONDITION_KEY_EQP_ID_LIST = "EQP_ID_LIST";
        public static string CONDITION_KEY_MODULE_ID = "MODULE_ID";
        public static string CONDITION_KEY_MODULE_ID_LIST = "MODULE_ID_LIST";
        public static string CONDITION_KEY_MODULE_NAME = "MODULE_NAME";
        public static string CONDITION_KEY_MODULE_ALIAS = "MODULE_ALIAS";
        public static string CONDITION_KEY_MODULE_LEVEL = "MODULE_LEVEL";
        public static string CONDITION_KEY_DCP_ID = "DCP_ID";
        public static string CONDITION_KEY_DCP_NAME = "DCP_NAME";
        public static string CONDITION_KEY_PRODUCT_ID = "PRODUCT_ID";
        public static string CONDITION_KEY_PRODUCT_ID_LIST = "PRODUCT_ID_LIST";
        public static string CONDITION_KEY_PRODUCT_RAWID_LIST = "PRODUCT_RAWID_LIST";
        public static string CONDITION_KEY_RECIPE_ID = "RECIPE_ID";
        public static string CONDITION_KEY_RECIPE_LIST = "RECIPE_LIST";
        public static string CONDITION_KEY_RECIPE_LIST_RAWID = "RECIPE_LIST_RAWID";
        public static string CONDITION_KEY_START_DATE = "START_DATE";
        public static string CONDITION_KEY_END_DATE = "END_DATE";
        public static string CONDITION_KEY_DATETIME = "DATETIME";
        public static string CONDITION_KEY_ALARM_DATE = "ALARM_DTTS";
        public static string CONDITION_KEY_FAULT_DATE = "FAULT_DTTS";
        public static string CONDITION_KEY_PERIOD_TYPE = "PERIOD_TYPE";
        public static string CONDITION_KEY_PARAMETER_LIST = "PARAMETER_LIST";
        public static string CONDITION_KEY_STEP_LIST = "STEP_LIST";
        public static string CONDITION_KEY_STEP_DATASET = "STEP_DATASET";
        public static string CONDITION_KEY_STEP_ALL_DATASET = "STEP_ALL_DATASET";

        public static string CONDITION_KEY_STEP_NAME = "STEP_NAME";

        public static string CONDITION_KEY_LOT_RAWID = "LOT_RAWID";
        public static string CONDITION_KEY_LOT_RAWID_LIST = "LOT_RAWID_LIST";
        public static string CONDITION_KEY_LOT_DATASET = "LOT_IDATASET";
        public static string CONDITION_KEY_LOT_ID_LIST = "LOT_ID_LIST";
        public static string CONDITION_KEY_SORTED_LIST = "SORTED_LIST";
        public static string CONDITION_KEY_USER_RAWID = "USER_RAWID";
        public static string CONDITION_KEY_USER_ID = "USER_ID";
        public static string CONDITION_KEY_DATA_TYPE = "DATA_TYPE";
        public static string CONDITION_KEY_CREATE_DTTS = "CREATE_DTTS";
        public static string CONDITION_KEY_LAST_UPDATE_DTTS = "LAST_UPDATE_DTTS";
        public static string CONDITION_KEY_PARENT = "PARENT";
        public static string CONDITION_KEY_SPEC_GROUP = "SPEC_GROUP";
        public static string CONDITION_KEY_SLOT = "SLOT";
        public static string CONDITION_KEY_SLOT_LIST = "SLOT_LIST";
        public static string CONDITION_KEY_SUBSTRATE_ID = "SUBSTRATE_ID";
        public static string CONDITION_KEY_SUBSTRATE_ID_LIST = "SUBSTRATE_ID_LIST";

        public static string CONDITION_KEY_LEGEND = "LEGEND";
        public static string CONDITION_KEY_TYPE = "TYPE";
        public static string CONDITION_KEY_CATEGORY = "CATEGORY";
        public static string CONDITION_KEY_USE_YN = "USE_YN";
        public static string CONDITION_KEY_DEFAULT_COL = "DEFAULT_COL";
        public static string CONDITION_KEY_FAULT_LEVEL = "FAULT_LEVEL";
        public static string CONDITION_KEY_VALUE = "VALUE";
        public static string CONDITION_KEY_TARGET = "TARGET";

        public static string CONDITION_KEY_MODULE_TO_MODULE = "SPC_MODULE_TO_MODULE";
        public static string CONDITION_KEY_RECIPE_TO_RECIPE = "SPC_RECIPE_TO_RECIPE";
        public static string CONDITION_KEY_STEP_TO_STEP = "SPC_STEP_TO_STEP";
        public static string CONDITION_KEY_PARAM_TO_PARAM = "SPC_PARAM_TO_PARAM";

        public static string CONDITION_KEY_SPEC_RAWID = "SPEC_RAWID";
        public static string CONDITION_KEY_DATA_CATEGORY_CD = "DATA_CATEGORY_CD";
        public static string CONDITION_KEY_SPEC_TYPE_CD = "SPEC_TYPE_CD";
        public static string CONDITION_KEY_SPEC_TYPE = "SPEC_TYPE";
        public static string CONDITION_KEY_SPEC_TYPE_COUNT = "SPEC_TYPE_COUNT";
        public static string CONDITION_KEY_TIME = "TIME";

        public static string CONDITION_KEY_USL = "USL";
        public static string CONDITION_KEY_UCL = "UCL";
        public static string CONDITION_KEY_LSL = "LSL";
        public static string CONDITION_KEY_LCL = "LCL";

        public static string CONDITION_KEY_CHAMBER_RAWID = "CHAMBER_RAWID";

        public static string CONDITION_KEY_SUMMARY_TYPE = "SUMMARY_TYPE";
        public static string CONDITION_KEY_PARAM_TYPE = "PARAM_TYPE";
        public static string CONDITION_KEY_PARAM_TYPE_CD = "PARAM_TYPE_CD";
        public static string CONDITION_KEY_PARAM_NAME = "PARAM_NAME";
        public static string CONDITION_KEY_PARAM_ALIAS = "PARAM_ALIAS";




        public static string CONDITION_KEY_PARAM_ALIAS_LIST = "PARAM_ALIAS_LIST";
        public static string CONDITION_KEY_PARAM_RAWID = "PARAM_RAWID";
        public static string CONDITION_KEY_PARAM_KEY = "PARAM_KEY";
        public static string CONDITION_KEY_PARAM_RAWID_LIST = "PARAM_RAWID_LIST";
        public static string CONDITION_KEY_PARAM_RAWID_NAME = "PARAM_RAWID_NAME";
        public static string CONDITION_KEY_PARAM_TRACE = "PARAM_TRACE";
        public static string CONDITION_KEY_PARAM_TRACE_DATASET = "PARAM_TRACE_DATASET";
        public static string CONDITION_KEY_PARAM_TRACE_ALL_DATASET = "PARAM_TRACE_ALL_DATASET";
        public static string CONDITION_KEY_PARAM_SUMMARY = "PARAM_SUMMARY";
        public static string CONDITION_KEY_PARAM_SUMMARY_DATASET = "PARAM_SUMMARY_DATASET";
        public static string CONDITION_KEY_PARAM_SUMMARY_ALL_DATASET = "PARAM_SUMMARY_ALL_DATASET";
        public static string CONDITION_KEY_PARAM_EVENT = "PARAM_EVENT";
        public static string CONDITION_KEY_PARAM_EVENT_DATASET = "PARAM_EVENT_DATASET";
        public static string CONDITION_KEY_PARAM_MSPC = "MSPC";
        public static string CONDITION_KEY_PARAM_MSPC_DATASET = "MSPC_DATASET";
        public static string CONDITION_KEY_DATA = "DATA";
        public static string CONDITION_KEY_COUNT = "COUNT";
        public static string CONDITION_KEY_RESULT = "RESULT";
        public static string CONDITION_KEY_RESULT_DATASET = "RESULT_DATASET";
        public static string CONDITION_KEY_BUTTON = "BUTTON";
        public static string CONDITION_KEY_GROUP_NAME = "GROUP_NAME";

        public static string CONDITION_KEY_NEW_RECIPE_ID = "NEW_RECIPE_ID";
        public static string CONDITION_KEY_RECIPE_RAWID = "RECIPE_RAWID";
        public static string CONDITION_KEY_STEP_RAWID = "STEP_RAWID";
        public static string CONDITION_KEY_BAND_TYPE = "BAND_TYPE";
        public static string CONDITION_KEY_BAND_TYPE_CD = "BAND_TYPE_CD";

        public static string CONDITION_KEY_SPEC_UNIT = "SPEC_UNIT";

        public static string CONDITION_KEY_EQP_RAWID = "EQP_RAWID";
        public static string CONDITION_KEY_EQP_MODULE_ID = "EQP_MODULE_ID";
        //SPC-679 added by enkim 2012.11.27
        public static string CONDITION_KEY_EQP_DCP_ID = "EQP_DCP_ID";
        public static string CONDITION_KEY_SUM_PARAM_ALIAS = "SUM_PARAM_ALIAS";
        //added end
        public static string CONDITION_KEY_EQP_MODULE_ID_LIST = "EQP_MODULE_ID_LIST";
        public static string CONDITION_KEY_EQP_RAWID_LIST = "EQP_RAWID_LIST";
        public static string CONDITION_KEY_EQP_PG_RAWID = "EQP_PG_RAWID";
        public static string CONDITION_KEY_SPEC_UNIT_CD = "SPEC_UNIT_CD";
        public static string CONDITION_KEY_DATASET = "DATASET";
        public static string CONDITION_KEY_TRACE_PARSE_TYPE = "TRACE_PARSE_TYPE";
        public static string CONDITION_KEY_TRACE_FAULT_COUNT = "TRACE_FAULT_COUNT";
        public static string CONDITION_KEY_SAMPLE_COUNT = "SAMPLE_COUNT";
        public static string CONDITION_KEY_MSPC_FAULT_COUNT = "MSPC_FAULT_COUNT";
        public static string CONDITION_KEY_TRACE_WARNING_COUNT = "TRACE_WARNING_COUNT";

        public static string CONDITION_KEY_SPEC_EXIST = "SPEC_EXIST";
        public static string CONDITION_KEY_CODE_LIST = "CODE_LIST";

        public static string CONDITION_KEY_MODULE_RAWID = "MODULE_RAWID";
        public static string CONDITION_KEY_PRODUCT_RAWID = "PRODUCT_RAWID";
        public static string CONDITION_KEY_DCP_RAWID = "DCP_RAWID";
        public static string CONDITION_KEY_MODULE_RAWID_LIST = "MODULE_RAWID_LIST";

        public static string CONDITION_KEY_SUMMARY_PARAM_RAWID = "SUMMARY_PARAM_RAWID";
        public static string CONDITION_KEY_SUMMARY_PARAM_RAWID_LIST = "SUMMARY_PARAM_RAWID_LIST";
        public static string CONDITION_KEY_EVENT_PARAM_RAWID = "EVENT_PARAM_RAWID";
        public static string CONDITION_KEY_EVENT_PARAM_RAWID_LIST = "EVENT_PARAM_RAWID_LIST";

        public static string CONDITION_KEY_RECIPE_ID_LIST = "RECIPE_ID_LIST";
        public static string CONDITION_KEY_RECIPE_RAWID_LIST = "RECIPE_RAWID_LIST";

        public static string CONDITION_KEY_PARAM_NAME_LIST = "PARAM_NAME_LIST";
        public static string CONDITION_KEY_PARAM_NAME_RAWID_LIST = "PARAM_NAME_RAWID_LIST";

        public static string CONDITION_KEY_TS_PARAM_NAME_LIST = "TS_PARAM_NAME_LIST";
        public static string CONDITION_KEY_TS_PARAM_NAME_RAWID_LIST = "TS_PARAM_NAME_RAWID_LIST";

        public static string CONDITION_KEY_ES_PARAM_NAME_LIST = "ES_PARAM_NAME_LIST";
        public static string CONDITION_KEY_ES_PARAM_NAME_RAWID_LIST = "ES_PARAM_NAME_RAWID_LIST";

        public static string CONDITION_KEY_STEP_ID_LIST = "STEP_ID_LIST";
        public static string CONDITION_KEY_STEP_RAWID_LIST = "STEP_RAWID_LIST";


        public static string CONDITION_KEY_EQUIPMENT = "Equipmnet";
        public static string CONDITION_KEY_CHAMBER = "Chamber";
        public static string CONDITION_KEY_LOTID = "Lot ID";
        public static string CONDITION_KEY_RECIPEID = "Recipe ID";

        public static string CONDITION_KEY_NEW_SORTED_LIST = "NEW_SORTED_LIST";
        public static string CONDITION_KEY_SELECTED_NODE_LIST = "SELECTED_NODE_LIST";

        public static string CONDITION_KEY_SPEC_DATASET = "SPEC_DATASET";
        public static string CONDITION_KEY_RULE_DATASET = "RULE_DATASET";
        public static string CONDITION_KEY_MTSPEC_RAWID = "MTSPEC_RAWID";

        public static string CONDITION_KEY_MULTIVARIATE_MODEL_RAWID = "MULTIVARIATE_MODEL_RAWID";
        public static string CONDITION_KEY_MULTIVARIATE_MODEL = "MULTIVARIATE_MODEL";

        public static string CONDITION_KEY_MULTIVARIATE_MODEL_RAWID_LIST = "MULTIVARIATE_MODEL_RAWID_LIST";
        public static string CONDITION_KEY_MULTIVARIATE_MODEL_LIST = "MULTIVARIATE_MODEL_LIST";

        public static string CONDITION_KEY_MULTIVARIATE_MODEL_DATASET = "MULTIVARIATE_MODEL_DATASET";

        public static string CONDITION_KEY_PARAMETER = "PARAMETER";
        public static string CONDITION_KEY_START_DTTS = "START_DTTS";
        public static string CONDITION_KEY_END_DTTS = "END_DTTS";
        public static string CONDITION_KEY_EVENT_DTTS = "EVENT_DTTS";

        public static string CONDITION_KEY_REAL_START_DTTS = "REAL_START_DTTS";
        public static string CONDITION_KEY_REAL_END_DTTS = "REAL_END_DTTS";
        public static string CONDITION_KEY_PPID = "PPID";

        public static string CONDITION_KEY_EQP_TRACE_PARAM_RAWID = "EQP_TRACE_PARAM_RAWID";

        public static string CONDITION_KEY_SUM_TYPE_CODE_CATEGORY = "SUM_TYPE_CODE_CATEGORY";
        public static string CONDITION_KEY_SUM_TYPE_CODE_LIST = "SUM_TYPE_CODE_LIST";
        public static string CONDITION_KEY_SUM_TYPE_CODE = "SUM_TYPE_CODE";

        public static string CONDITION_KEY_SITE = "SITE";
        public static string CONDITION_KEY_FAB = "FAB";
        public static string CONDITION_KEY_TS_VARIABLE_ID_LIST = "TS_VARIABLE_ID_LIST";
        public static string CONDITION_KEY_ES_VARIABLE_ID_LIST = "ES_VARIABLE_ID_LIST";

        public static string CONDITION_KEY_USER_NAME = "USER_NAME";

        public static string CONDITION_KEY_TARGET_SPEC_MODEL_GROUP_NAME = "TARGET_SPEC_MODEL_GROUP_NAME";
        public static string CONDITION_KEY_SPEC_MODEL_GROUP_NAME = "SPEC_MODEL_GROUP_NAME";
        public static string CONDITION_KEY_SPEC_MODEL_GROUP_RAWID = "SPEC_MODEL_GROUP_RAWID";

        public static string CONDITION_KEY_SPEC_MODEL_GROUP_NAME_LIST = "SPEC_MODEL_GROUP_NAME_LIST";
        public static string CONDITION_KEY_SPEC_MODEL_GROUP_RAWID_LIST = "SPEC_MODEL_GROUP_RAWID_LIST";

        public static string CONDITION_KEY_ID = "ID";
        public static string CONDITION_KEY_TEXT = "TEXT";
        public static string CONDITION_KEY_SAP_EQ_NR = "SAP_EQ_NR";
        public static string CONDITION_KEY_ALIAS = "ALIAS";
        public static string CONDITION_KEY_STATUS = "STATUS";
        public static string CONDITION_KEY_PRIORITY = "PRIORITY";
        public static string CONDITION_KEY_CREATE_BY = "CREATE_BY";
        public static string CONDITION_KEY_LAST_UPDATE_BY = "LAST_UPDATE_BY";
        public static string CONDITION_KEY_EQ_NR = "EQ_NR";
        public static string CONDITION_KEY_EQ_ID = "EQ_ID";
        public static string CONDITION_KEY_EL_TY = "EL_TY";
        public static string CONDITION_KEY_EL_ID = "EL_ID";
        public static string CONDITION_KEY_EL_PARENT = "EL_PARENT";
        public static string CONDITION_KEY_MAIL_GROUP_RAWID = "MAIL_GROUP_RAWID";

        public static string CONDITION_KEY_TARGET_NAME = "TARGET_NAME";

        public static string CONDITION_KEY_CODE = "CODE";
        public static string CONDITION_KEY_NAME = "NAME";
        public static string CONDITION_KEY_SUBITEM = "SUBITEM";


        public static string CONDITION_KEY_SINGLE_TAB = "TPAGESINGLE";
        public static string CONDITION_KEY_TRENDCHART_TAB = "TPAGETRENDCHART";



        public static string CONDITION_KEY_OPERATION = "OPERATION";
        public static string CONDITION_KEY_E_EVENT_TRX_RAWID = "E_EVENT_TRX_RAWID";
        public static string CONDITION_KEY_E_EVENT_PARAM_RAWID = "E_EVENT_PARAM_RAWID";
        public static string CONDITION_KEY_DATA_TYPE_CD = "DATA_TYPE_CD";

        //kal : Phase2 Metrology Result Visualization
        public static string CONDITION_KEY_RK = "RK";


        public static string CONDITION_KEY_TABLE_NAME = "TABLE_NAME";
        public static string CONDITION_KEY_CONFIG_MODE = "CONFIG_MODE";

        public static string CONDITION_KEY_MAIN_YN= "MAIN_YN";
        public static string CONDITION_KEY_HAS_SUBCONFIG= "HAS_SUBCONFIG";
        public static string CONDITION_KEY_FUNCTION = "FUNCTION";

        public static string CONDITION_KEY_SEARCH_OPERATION_ID = "SEARCH_OPERATION_ID"; //추가
        public const string CONDITION_KEY_SPC_MODEL_VERSION = "SPC_MODEL_VERSION";

        #endregion

        #region : DYNAMIC SEARCH CONDITION KEY

        public static string CONDITION_SEARCH_KEY_LINE = "ESPC_LINE";
        public static string CONDITION_SEARCH_KEY_AREA = "ESPC_AREA";
        public static string CONDITION_SEARCH_KEY_EQPMODEL = "ESPC_EQPMODEL";
        public static string CONDITION_SEARCH_KEY_EQP = "ESPC_EQP_ID";
        public static string CONDITION_SEARCH_KEY_DCP_ID = "ESPC_DCP_ID";
        public static string CONDITION_SEARCH_KEY_MODULE = "ESPC_MODULENAME";
        public static string CONDITION_SEARCH_KEY_MODEL = "ESPC_MODEL";
        public static string CONDITION_SEARCH_KEY_SPCMODEL = "ESPC_SPCMODEL";
        public static string CONDITION_SEARCH_KEY_SITE = "ESPC_SITE";
        public static string CONDITION_SEARCH_KEY_FAB = "ESPC_FAB";
        public static string CONDITION_SEARCH_KEY_GROUP_NAME = "ESPC_GROUP_NAME";
        public static string CONDITION_SEARCH_KEY_LINE_RAWID = "ESPC_LINE_RAWID";
        public static string CONDITION_SEARCH_KEY_AREA_RAWID = "ESPC_AREA_RAWID";
        public static string CONDITION_SEARCH_KEY_MODEL_CONFIG_RAWID = "ESPC_MODEL_CONFIG_RAWID";

        public static string CONDITION_SEARCH_KEY_PRODUCT_TYPE = "ESPC_PRODUCT_TYPE";
        public static string CONDITION_SEARCH_KEY_PRODUCT = "ESPC_PRODUCT";
        public static string CONDITION_SEARCH_KEY_RECIPE_TYPE = "ESPC_RECIPE_TYPE";
        public static string CONDITION_SEARCH_KEY_RECIPE = "ESPC_RECIPE";
        public static string CONDITION_SEARCH_KEY_PARAM_TYPE = "ESPC_PARAM_TYPE";
        public static string CONDITION_SEARCH_KEY_TRACE_PARAM_TYPE = "ESPC_TRACE_PARAM_TYPE";
        public static string CONDITION_SEARCH_KEY_TRACE_SUM_PARAM_TYPE = "ESPC_SUM_PARAM_TYPE";
        public static string CONDITION_SEARCH_KEY_EVENT_SUM_PARAM_TYPE = "ESPC_EVENT_PARAM_TYPE";
        public static string CONDITION_SEARCH_KEY_PARAM = "ESPC_PARAM";
        public static string CONDITION_SEARCH_KEY_TRACE_SUM_PARAM = "ESPC_TRACE_SUM_PARAM";
        public static string CONDITION_SEARCH_KEY_EVENT_SUM_PARAM = "ESPC_EVENT_SUM_PARAM";
        public static string CONDITION_SEARCH_KEY_GROUP = "ESPC_GROUP";
        public static string CONDITION_SEARCH_KEY_MULTIVARIATE_MODEL_TYPE = "ESPC_MULTIVARIATE_MODEL_TYPE";
        public static string CONDITION_SEARCH_KEY_MULTIVARIATE_MODEL = "ESPC_MULTIVARIATE_MODEL";
        public static string CONDITION_SEARCH_KEY_STEP_TYPE = "ESPC_STEP_TYPE";
        public static string CONDITION_SEARCH_KEY_STEP = "ESPC_STEP";
        public static string CONDITION_SEARCH_KEY_SPEC_GROUP_TYPE = "ESPC_SPEC_GROUP_TYPE";
        public static string CONDITION_SEARCH_KEY_SPEC_GROUP = "ESPC_SPEC_GROUP";
        public static string CONDITION_SEARCH_KEY_TYPE = "ESPC_TYPE";
        public static string CONDITION_SEARCH_KEY_LOT = "ESPC_LOT";
        public static string CONDITION_SEARCH_KEY_SUBSTRATE = "ESPC_SUBSTRATE";
        public static string CONDITION_SEARCH_KEY_SUM_CATEGORY = "ESPC_SUM_CATEGORY";

        public static string CONDITION_SEARCH_KEY_FROMDATE = "DATETIME_FROM";
        public static string CONDITION_SEARCH_KEY_TODATE = "DATETIME_TO";
        public static string CONDITION_SEARCH_KEY_DATETIME_VALUEDATA = "DATETIMEVALUEDATA";
        public static string CONDITION_SEARCH_KEY_VALUEDATA = "VALUEDATA";
        public static string CONDITION_SEARCH_KEY_DISPLAYDATA = "DISPLAYDATA";
        public static string CONDITION_SEARCH_KEY_ADDTIONALVALUEDATA = "ADDTIONALVALUEDATA";
        public static string CONDITION_SEARCH_KEY_CHECKED = "CHECKED";
        public static string CONDITION_SEARCH_KEY_SPC_MODEL_LIST = "SPC_MODEL_LIST";
        public static string CONDITION_SEARCH_KEY_FILTER = "ESPC_FILTER";

        public static string CONDITION_SEARCH_KEY_SPEC_MODEL_RAWID_LIST = "ESPC_SPEC_MODEL_RAWID_LIST";
        public static string CONDITION_SEARCH_KEY_PATTERN_RAWID_LIST = "ESPC_PATTERN_RAWID_LIST";
        public static string CONDITION_SEARCH_KEY_MTSPEC_RAWID_LIST = "ESPC_MTSPEC_RAWID_LIST";

        public static string CONDITION_SEARCH_KEY_OCAP_OOC = "ESPC_OCAP_OOC";
        #endregion

        #region : CONTEXT TYPE

        public static string CONTEXT_TYPE_LOT_ID = "LOT_ID";
        public static string CONTEXT_TYPE_SUBSTRATE_ID = "SUBSTRATE_ID";
        public static string CONTEXT_TYPE_RECIPE_ID = "RECIPE_ID";
        public static string CONTEXT_TYPE_PRODUCT_ID = "PRODUCT_ID";
        public static string CONTEXT_TYPE_OPERATION_ID = "OPERATION_ID";
        public static string CONTEXT_TYPE_EQP_ID = "EQP_ID";
        public static string CONTEXT_TYPE_MODULE_NAME = "MODULE_NAME";
        public static string CONTEXT_TYPE_STEP_ID = "STEP_ID";
        public static string CONTEXT_TYPE_CONTEXT_KEY = "CONTEXT_KEY";
        public static string CONTEXT_TYPE_NON = "NON";
        public static string CONTEXT_TYPE_STATUS = "STATUS";




        #endregion

        #region : CODE CATEGORY

        public static string CODE_CATEGORY_CHART_SERARCH_CONDITION = "SPC_CHART_SEARCH_CONDITION";
        public static string CODE_CATEGORY_CONTROL_CHART = "SPC_CONTROL_CHART";
        public static string CODE_CATEGORY_SEARCH_TYPE = "SPC_SEARCH_TYPE";
        public static string CODE_CATEGORY_DATA_TYPE = "DATA_TYPE";
        public static string CODE_CATEGORY_LOT_TYPE = "LOT_TYPE";
        public static string CODE_CATEGORY_USE_TREE = "TR";
        public static string CODE_CATEGORY_USE_USERCONTROL = "UC";
        public static string CODE_CATEGORY_CONTEXT_TYPE = "CONTEXT_TYPE";
        public static string CODE_CATEGORY_SPC_CONTEXT_TYPE = "SPC_CONTEXT_TYPE";

        public static string CODE_CATEGORY_USE_SPEC_GROUP = "SPC_USE_SPEC_GROUP";
        public static string CODE_CATEGORY_USE_DEFAULT_CONDITION = "SPC_USE_DCONDITION";
        public static string CODE_CATEGORY_USE_CONDITION_TYPE = "SPC_CONDITION_TYPE";

        public static string CODE_CATEGORY_PARAM_TYPE = "SPC_PARAM_TYPE";

        public static string CODE_CATEGORY_MET_PARAM_TYPE = "SPC_MET_PARAM_TYPE";

        public static string CODE_CATEGORY_AUTO_TYPE = "SPC_AUTO_TYPE";
        public static string CODE_CATEGORY_PARAM_CATEGORY = "SPC_PARAM_CATEGORY";
        public static string CODE_CATEGORY_PRIORITY = "SPC_PRIORITY";
        public static string CODE_CATEGORY_OCAP_TYPE = "SPC_OCAP_TYPE";
        public static string CODE_CATEGORY_PERIOD_TYPE = "PERIOD_TYPE";        
        
        //2009-10-26 추가
        public static string CODE_CATEGORY_PPK_SORTING_KEY = "SPC_PPK_SORTING_KEY";
        public static string CODE_CATEGORY_PERIOD_PPK = "SPC_PERIOD_PPK";

        public static string CODE_CATEGORY_SORTING_KEY = "SPC_SORTING_KEY";
        public static string CODE_CATEGORY_ANALYSIS_CHART = "SPC_ANALYSIS_CHART";
        public static string CODE_CATEGORY_ANALYSIS_TYPE = "SPC_ANALYSIS_TYPE";
        public static string CODE_CATEGORY_SUBITEM = "SPC_SUBITEM";
        public static string CODE_CATEGORY_PROBE_ITEM = "SPC_PROBE_ITEM";
        public static string CODE_CATEGORY_PROBE_SUBITEM = "SPC_PROBE_SUBITEM";
        public static string CODE_CATEGORY_PROBE_SORTING_KEY = "SPC_PROBE_SORTING_KEY";
        public static string CODE_CATEGORY_MULTI_SUBITEM = "SPC_MULTI_SUBITEM";

        //2009-12-04 추가
        public static string CODE_CATEGORY_MANAGE_TYPE = "SPC_MANAGE_TYPE";
        public static string CODE_CATEGORY_CHART_MODE = "SPC_CHART_MODE";

        public static string CODE_CATEGORY_ATT_CHART = "SPC_ATT_CHART";

        //2013-12-13 추가 SPC-1155 by stella
        public static string CODE_CATEGORY_AUTOCALC_OPTION = "SPC_AUTOCALC_OPTION";
                
        #endregion

        #region : PAGE KEY

        //SEARHC
        public static string PAGE_KEY_SERACH_CONDITION = "SPC_SERACH_CONDITION";
        public static string PAGE_KEY_SERACH_CONDITION_MODELING = "SPC_MODELING";

        //MAIN

        public static string PAGE_KEY_OCAP_LIST_UC = "SPC_OCAP_LIST_UC";
        public static string PAGE_KEY_SPC_MODELING = "SPC_MODELING";

        public static string PAGE_KEY_SPC_MET_MODELING = "SPC_MET_MODELING";

        public static string PAGE_KEY_SPC_CONFIGURATION = "SPC_CONFIGURATION";
        public static string PAGE_KEY_SPC_CONFIGURATION_CONTEXT = "SPC_CONFIGURATION_CONTEXT";
        //public static string PAGE_KEY_SPC_CONFIGURATION_FILTER = "SPC_CONFIGURATION_FILTER";
        public static string PAGE_KEY_SPC_CONFIGURATION_RULE = "SPC_CONFIGURATION_RULE";
        public static string PAGE_KEY_SPC_CONFIGURATION_RULE_MASTER = "SPC_CONFIGURATION_RULE_MASTER";
        public static string PAGE_KEY_SPC_CONFIGURATION_RULE_OCAP = "SPC_CONFIGURATION_RULE_OCAP";
        public static string PAGE_KEY_SPC_CONFIGURATION_DEFAULT_CHART = "SPC_CONFIGURATION_DEFAULT_CHART";
        public static string PAGE_KEY_SPC_CONFIGURATION_PARAM_LIST = "SPC_CONFIGURATION_PARAM_LIST";

        public static string PAGE_KEY_SPC_CHART_UC = "SPC_CHART_UC";
        public static string PAGE_KEY_SPC_PROCESS_CAPABILITY_UC = "SPC_PROCESS_CAPABILITY_UC";
        public static string PAGE_KEY_SPC_MET_PROCESS_CAPABILITY_UC = "SPC_MET_PROCESS_CAPABILITY_UC";
        public static string PAGE_KEY_SPC_PPK_UC = "SPC_PPK_UC";
        public static string PAGE_KEY_SPC_CONTROL_CHART_UC = "SPC_CONTROL_CHART_UC";
        public static string PAGE_KEY_SPC_MET_CONTROL_CHART_UC = "SPC_MET_CONTROL_CHART_UC";
        public static string PAGE_KEY_SPC_MULTIDATA_UC = "SPC_MULTIDATA_UC";
        public static string PAGE_KEY_SPC_MODEL_COMPARE_UC = "SPC_MODEL_COMPARE_UC";
        public static string PAGE_KEY_SPC_MET_MODEL_COMPARE_UC = "SPC_MET_MODEL_COMPARE_UC";
        public const string PAGE_KEY_SPC_DATA_EXPORT = "SPC_DATA_EXPORT";
        public const string PAGE_KEY_SPC_MODEL_HISTORY = "SPC_MODEL_HISTORY";
        public const string PAGE_KEY_SPC_MET_MODEL_HISTORY = "SPC_MET_MODEL_HISTORY";

        
        //POPUP
        public static string PAGE_KEY_CONFIGURATEION_POPUP = "SPC_CONFIGURATEION_POPUP";
        public static string PAGE_KEY_OCAP_VIEW_POPUP = "SPC_OCAP_VIEW_POPUP";
        public static string PAGE_KEY_SPC_CHART_POPUP = "SPC_CHART_POPUP";
        public static string PAGE_KEY_SPC_PROCESS_CAPABILITY_POPUP = "SPC_PROCESS_CAPABILITY_POPUP";
        public static string PAGE_KEY_SPC_OCAP_DETAILS_POPUP = "SPC_OCAP_DETAILS_POPUP";


        public static string PAGE_KEY_SERACH_CONDITION_EQP_SUM_PARAMETER = "EQP_SUM_PARAMETER";
        public static string PAGE_KEY_SERACH_CONDITION_MULTIVARIATE_MODEL = "MULTIVARIATE_MODEL";
        public static string PAGE_KEY_SERACH_CONDITION_LOT_LIST = "LOT_LIST";
        public static string PAGE_KEY_SERACH_CONDITION_RESULT_LOT_LIST = "RESULT_LOT_LIST";
        public static string PAGE_KEY_SERACH_CONDITION_RESULT_STEP_LIST = "RESULT_STEP_LIST";
        public static string PAGE_KEY_SERACH_CONDITION_RESULT_PARAM_LIST = "RESULT_PARAM_LIST";
        public static string PAGE_KEY_SERACH_CONDITION_RECIPE_LIST = "RECIPE_LIST";
        public static string PAGE_KEY_SERACH_CONDITION_STEP_LIST = "STEP_LIST";
        public static string PAGE_KEY_SERACH_CONDITION_BAND_TYPE = "BAND_TYPE";
        public static string PAGE_KEY_SEARCH_CONDITION_COPY_RECIPE_LIST = "COPY_RECIPE_LIST";
        public static string PAGE_KEY_SEARCH_CONDITION_SPEC_MODEL_GROUP_LIST = "SPEC_MODEL_GROUP_LIST";
        public static string PAGE_KEY_SEARCH_CONDITION_PRODUCT_ID_LIST = "PRODUCT_ID_LIST";

        public static string PAGE_KEY_SEARCH_CONDITION_SEARCH_BY_AREA = "SPC_SEARCH_BY_AREA";

        public static string PAGE_KEY_SPC_MODEL_HISTORY_POPUP = "SPC_MODEL_HISTORY_POPUP";

        public static string PAGE_KEY_ATT_OCAP_LIST_UC = "SPC_ATT_OCAP_LIST_UC";
        public static string PAGE_KEY_SPC_ATT_MODELING = "SPC_ATT_MODELING";

        public static string PAGE_KEY_SPC_ATT_CONTROL_CHART_UC = "SPC_ATT_CONTROL_CHART_UC";
        public static string PAGE_KEY_SPC_ATT_MODEL_COMPARE_UC = "SPC_ATT_MODEL_COMPARE_UC";
        public const string PAGE_KEY_SPC_ATT_MODEL_HISTORY = "SPC_ATT_MODEL_HISTORY";
        public static string PAGE_KEY_SPC_ATT_PROCESS_CAPABILITY_UC = "SPC_ATT_PROCESS_CAPABILITY_UC";

        public static string PAGE_KEY_SPC_CONFIGURATION_AUTO_CALCULATION = "SPC_CONFIGURATION_AUTO_CALCULATION";

        //SUMMARY
        public const string PAGE_KEY_SPC_SUMMARYDATA_OPTION_POPUP = "SPC_SUMMARYDATA_OPTION_POPUP"; //SPC-929, KBLEE
        #endregion


        #region : PAGE_HEADER
        //Main
        public static string PAGE_KEY_SPC_CHART_UC_HEADER_CHART_INFOMATION_KEY = "HEADER_CHART_INFOMATION";
        public static string PAGE_KEY_SPC_PROCESS_CAPABILITY_UC_HEADER_KEY = "HEADER_PROCESS_CAPABILITY";
        public static string PAGE_KEY_OCAP_LIST_UC_HEADER_KEY = "HEADER_OCAP_LIST";
        public static string PAGE_KEY_CONTROL_CHART_UC_CONDITION_HEADER_KEY = "HEADER_SPC_CONTROL_CHART_CONDITION";


        //Popup
        public static string PAGE_KEY_SPC_CHART_POPUP_HEADER_CHART_DATA = "HEADER_CHART_DATA";
        public static string PAGE_KEY_SPC_PROCESS_CAPABILITY_POPUP_HEADER = "HEADER_PROCESS_CAPABILITY_POPUP";
        public static string PAGE_KEY_SPC_CONTROL_CHART_UC_HEADER_KEY = "HEADER_SPC_CONTROL_CHART";
        public static string PAGE_KEY_SPC_OCAP_DETAILS_UC_HEADER_KEY = "HEADER_SPC_OCAP_DETAILS";
        public static string PAGE_KEY_SPC_OCAP_DETAILS_UC_HEADER_KEY_SELECTED_OCAP = "HEADER_SPC_SELECTED_OCAP_LIST";

        public static string HEADER_KEY_MODEL_CONTEXT_INFO = "HEADER_MODEL_CONTEXT_INFO";
        public static string HEADER_KEY_MODEL_HISTORY_INFO = "HEADER_MODEL_HISTORY_INFO";

        #endregion

        #region : FUNCTION KEY

        public const string FUNC_KEY_SPC_MODELING = "SPC_MODELING";
        public const string FUNC_KEY_SPC_CALCULATION = "SPC_CALCULATION";
        public const string FUNC_KEY_OCAP_LIST = "SPC_OCAP_LIST";
        public const string FUNC_KEY_SPC_CHART = "SPC_CHART";
        public const string FUNC_KEY_SPC_PROCESS_CAPABILITY = "SPC_PROCESS_CAPABILITY";
        public const string FUNC_KEY_SPC_CONTROL_CHART = "SPC_CONTROL_CHART";
        public const string FUNC_KEY_SPC_MODEL_COMPARE = "SPC_MODEL_COMPARE";
        public const string FUNC_KEY_SPC_DATA_EXPORT = "SPC_DATA_EXPORT";

        public const string FUNC_KEY_SPC_MET_MODELING = "SPC_MET_MODELING";
        public const string FUNC_KEY_SPC_MET_CONTROL_CHART = "SPC_MET_CONTROL_CHART";
        public const string FUNC_KEY_SPC_MET_PROCESS_CAPABILITY = "SPC_MET_PROCESS_CAPABILITY";

        public const string FUNC_KEY_SPC_ATT_MODELING = "SPC_ATT_MODELING";
        public const string FUNC_KEY_ATT_OCAP_LIST = "SPC_ATT_OCAP_LIST";
        public const string FUNC_KEY_SPC_ATT_CONTROL_CHART = "SPC_ATT_CONTROL_CHART";
        public const string FUNC_KEY_SPC_ATT_MODEL_COMPARE = "SPC_ATT_MODEL_COMPARE";
        public const string FUNC_KEY_SPC_ATT_PROCESS_CAPABILITY = "SPC_ATT_PROCESS_CAPABILITY";
        #endregion

        #region : TITLE KEY

        public static string POPUP_TITLE_KEY_SPC_CONFIGURATION = "SPC_POPUP_TITLE_SPC_CONFIGURATION";
        public static string POPUP_TITLE_KEY_OCAP_DETAILS = "SPC_POPUP_TITLE_OCAP_DETAILS";

        public static string POPUP_TITLE_KEY_CHART_VIEW = "SPC_POPUP_TITLE_CHART_VIEW";
        public static string POPUP_TITLE_KEY_CHART_DATA_VIEW = "SPC_POPUP_TITLE_CHART_DATA_VIEW";
        public static string POPUP_TITLE_KEY_SELECT_CHART_TO_VIEW = "SPC_POPUP_TITLE_SELECT_CHART_TO_VIEW";
        public static string POPUP_TITLE_KEY_PROCESS_CAPABILITY = "SPC_POPUP_TITLE_PROCESS_CAPABILITY";
        public static string POPUP_TITLE_KEY_ANALYSIS_CHART_VIEW = "SPC_POPUP_TITLE_ANALYSIS_CHART_VIEW";

        public static string POPUP_TITLE_KEY_STEP_DATAMAPPING = "SPC_POPUP_TITLE_KEY_STEP_DATAMAPPING";
        public static string POPUP_TITLE_KEY_SPCMODEL_LIST = "SPC_POPUP_TITLE_KEY_SPCMODEL_LIST";
        public static string POPUP_TITLE_KEY_SELECT_OPERATION = "SPC_POPUP_TITLE_KEY_SELECT_OPERATION"; //추가

        public static string TITLE_KEY_SPC_CONFIGURATION = "SPC_TITLE_SPC_MODELING";
        public static string TITLE_OCAP_LIST = "SPC_TITLE_OCAP_LIST";
        public static string TITLE_SPC_CHART = "SPC_TITLE_SPC_CHART";
        public static string TITLE_PROCESS_CAPABILITY = "SPC_TITLE_PROCESS_CAPABILITY";
        public static string TITLE_KEY_SPC_CONTROL_CHART = "SPC_TITLE_CONTROL_CHART";
        public static string TITLE_KEY_ANALYSIS = "SPC_TITLE_ANALYSIS";
        public static string TITLE_KEY_PROBE_DATA = "SPC_TITLE_PROBE_DATA";
        public static string TITLE_SPC_CALCULATION = "SPC_TITLE_SPC_CALCULATION";
        
        //SPC-704 multical
        public static string TITLE_SPC_MULTICALCULATION = "SPC_TITLE_SPC_MULTICALCULATION";

        public static string TITLE_KEY_SPC_ATT_CONTROL_CHART = "SPC_TITLE_ATT_CONTROL_CHART";

        #endregion

        #region : TITLE
        public static string POPUP_TITLE_SUMMARY_DATA_OPTION = "SUMMARY DATA OPTION"; //SPC-929, KBLEE
        public static string POPUP_TITLE_RULE_SIMULATION_OPTION = "Rule Simulation Option"; //SPC-930, KBLEE
        #endregion

        #region : LABEL

        public struct LABEL
        {
            public static string LOT_ID = "SPC_LABEL_LOT_ID";
            public static string EQP_ID = "SPC_LABEL_EQP_ID";
            public static string PRODUCT_ID = "SPC_PRODUCT_ID";
            public static string SUBSTRATE_ID = "SPC_LABEL_SUBSTRATE_ID";
            public static string CLASSTTE_SLOT = "SPC_LABEL_CLASSTTE_SLOT";
            public static string RECIPE_ID = "SPC_LABEL_RECIPE_ID";
            public static string OPERATION_ID = "SPC_LABEL_OPERATION_ID";
            public static string OPERATION_NUMBER = "SPC_LABEL_OPERATION_NUMBER";
            public static string MODULE_NAME = "SPC_LABEL_MODULE_NAME";
            public static string STEP_ID = "SPC_LABEL_STEP_ID";
            public static string CONTEXT_KEY = "SPC_LABEL_CONTEXT_KEY";
            public static string AREA = "SPC_LABEL_AREA";
            public static string MODEL_NAME = "SPC_LABEL_MODEL_NAME";            
            public static string PRODUCT = "SPC_LABEL_CONDITION_PRODUCT";
            public static string OPERATION = "SPC_LABEL_CONDITION_OPERATION";
            public static string PARAMETER = "SPC_LABEL_CONDITION_PARAMETER";
            public static string PERIOD = "SPC_LABEL_CONDITION_PERIOD";
            public static string SORTING_KEY = "SPC_LABEL_CONDITION_SORTING_KEY";
            public static string CHART_MODE = "CHART_MODE";

            //2009-11-10
            public static string SUB_DATA = "SPC_LABEL_CONDITION_SUB_DATA";
            public static string CHART_LIST = "SPC_LABEL_CONDITION_CHART_LIST";
            

            public static string USL = "SPC_LABEL_USL";
            public static string LSL = "SPC_LABEL_LSL";
            public static string UCL = "SPC_LABEL_UCL";
            public static string LCL = "SPC_LABEL_LCL";
            public static string RUCL = "SPC_LABEL_RUCL";
            public static string RCL = "SPC_LABEL_RCL";
            public static string RLCL = "SPC_LABEL_RLCL";
            public static string RANGE = "SPC_LABEL_RANGE";
            public static string SUCL = "SPC_LABEL_SUCL";
            public static string SCL = "SPC_LABEL_SCL";
            public static string SLCL = "SPC_LABEL_SLCL";
            public static string STDDEV = "SPC_LABEL_STDDEV";
            public static string TARGET = "SPC_LABEL_TARGET";
            public static string MIN = "SPC_LABEL_MIN";
            public static string MAX = "SPC_LABEL_MAX";
            public static string CP = "SPC_LABEL_CP";
            public static string CPK = "SPC_LABEL_CPK";
            public static string PP = "SPC_LABEL_PP";
            public static string PPK = "SPC_LABEL_PPK";
            public static string AVG = "SPC_LABEL_AVG";
            public static string MINSAMPLECNT = "SPC_LABEL_MINSAMPLECNT";
    
        }

        public static string LABEL_CONTEXT_CONFIGURATION = "SPC_LABEL_CONTEXT_CONFIGURATION";
        public static string LABEL_UPPER = "SPC_LABEL_UPPER";
        public static string LABEL_LOWER = "SPC_LABEL_LOWER";
        public static string LABEL_PRIORITY = "SPC_LABEL_PRIORITY";
        public static string LABEL_SEARCH_PERIOD = "SPC_LABEL_SEARCH_PERIOD";
        public static string LABEL_LINE = "SPC_LABEL_LINE";
        public static string LABEL_PARAMETER_CHARACTERISTIC = "SPC_LABEL_PARAMETER_CHARACTERISTIC";
        public static string LABEL_ITEM = "SPC_LABEL_ITEM";
        public static string LABEL_CPK = "SPC_LABEL_CPK";
        public static string LABEL_KEY_LINE = "LINE";
        public static string LABEL_KEY_AREA = "AREA";
        public static string LABEL_KEY_EQP_MODEL = "SPC_LABEL_EQP_MODEL";
        public static string LABEL_KEY_EQP_ID = "SPC_LABEL_EQP_ID";
        public static string LABEL_KEY_DCP_ID = "SPC_LABEL_DCP_ID";
        public static string LABEL_KEY_PRODUCT_ID = "SPC_LABEL_PRODUCT_ID";
        public static string LABEL_KEY_MODULE_ID = "SPC_LABEL_MODULE_ID";
        public static string LABEL_KEY_RECIPE_ID = "SPC_LABEL_RECIPE_ID";
        public static string LABEL_KEY_PARAM_NAME = "SPC_PARAM_NAME";
        public static string LABEL_KEY_TRACE_SUM_PARAM = "SPC_TRACE_SUM_PARAM";
        public static string LABEL_KEY_EVENT_SUM_PARAM = "SPC_EVENT_SUM_PARAM";
        public static string LABEL_KEY_STEP_ID = "SPC_STEP_ID";
        public static string LABEL_KEY_OPERATION_ID = "SPC_OPERATION_ID";
        public static string LABEL_KEY_OPERATION_NUMBER = "SPC_OPERATION_NUMBER";
        public static string LABEL_KEY_TYPE = "SPC_TYPE";
        public static string LABEL_KEY_LEGEND = "SPC_LEGEND";
        public static string LABEL_KEY_LOT = "SPC_LOT_ID";


        #endregion

        #region : TAB PAGE KEY
        public static string TAB_CONTEXT_CONFIGURATION = "SPC_TAB_CONTEXT_CONFIGURATION";
        public static string TAB_RULE_CONFIGURATION = "SPC_TAB_RULE_CONFIGURATION";
        public static string TAB_MAILING_GROUP = "SPC_TAB_MAILING_GROUP";
        public static string TAB_OPTIONAL_CONFIGURATION = "SPC_TAB_OPTIONAL_CONFIGURATION";
        public static string TAB_AUTO_CALCULATION = "SPC_TAB_AUTO_CALCULATION";
        #endregion

        #region : VARIABLE KEY

        public struct VARIABLE
        {
            public static string ALL = "ALL";
            public static string STAR = "*";
            public static string METROLOGY = "METROLOGY";

        
        }
        public static string VARIABLE_FALSE = "False";
        public static string VARIABLE_FAIL = "Fail";
        public static string VARIABLE_TRUE = "True";

        public static string VARIABLE_FIELDNAME = "FieldName";
        public static string VARIABLE_MAXBYTES = "MaxBytes";
        public static string VARIABLE_TABLENAME = "TableName";

        public static string VARIABLE_COMBO_VALUE = "VALUE";
        public static string VARIABLE_COMBO_DISPLAY = "DISPLAY";

        public static string VARIABLE_Y = "Y";
        public static string VARIABLE_N = "N";

        public static string VARIABLE_YES = "YES";
        public static string VARIABLE_NO = "NO";

        public static string VARIABLE_ZERO = "0";
        public static string VARIABLE_ONE = "1";


        public static string VARIABLE_ALL = "ALL";
        public static string VARIABLE_DEFAULT_ALL = "DEFAULT_ALL";
        public static string VARIABLE_MAIN = "MAIN";

        public static string VARIABLE_SPLIT = "|";
        public static string VARIABLE_DATAROW = "DATAROW";



        public static string VARIABLE_INT = "INT";
        public static string VARIABLE_UINT = "UINT";
        public static string VARIABLE_FLOAT = "FLOAT";
        public static string VARIABLE_TOTAL = "TOTAL";
        public static string VARIABLE_ASCII = "ASCII";
        public static string VARIABLE_BINARY = "BINARY";
        public static string VARIABLE_BOOLEAN = "BOOLEAN";


        public static string VARIABLE_GOLDEN_READY_RECIPE = "GR";
        public static string VARIABLE_GOLDEN_RECIPE = "G";
        public static string VARIABLE_EQP_RECIPE = "E";
        public static string VARIABLE_DMS_RECIPE = "D";
        public static string VARIABLE_SEARCHED_EQP_RECIPE = "SE";
        public static string VARIABLE_RECIPE_HISTORY = "RH";

        public static string VARIABLE_COPY = "COPY";
        public static string VARIABLE_DELETE = "DELETE";
        public static string VARIABLE_ENTER = "\r\n";

        public static string VARIABLE_SECTION_IN = "[";
        public static string VARIABLE_SECTION_OUT = "]";

        public static string VARIABLE_SECTION = "^";

        public static string VARIABLE_SET = "SET";
        public static string VARIABLE_CONTROL = "CONTROL";

        public static string VARIABLE_SET_CODE = "S";
        public static string VARIABLE_CONTROL_CODE = "C";

        public static string VARIABLE_EDIT_TYPE_VIEW = "VIEW";
        public static string VARIABLE_EDIT_TYPE_EQP = "EQP";

        public static string VARIABLE_MINUS_PLUS = "-/+";
        public static string VARIABLE_DIVISION = "%";

        public static string VARIABLE_LOWER_SPEC_LIMIT = "LOWNER";
        public static string VARIABLE_HIGHER_SPEC_LIMIT = "HIGHER";
        public static string VARIABLE_UPLOAD = "UPLOAD";

        public static string VARIABLE_GOLDEN = "GOLDEN";

        public static string VARIABLE_BASECOND = "BASECOND";

        public static string VARIABLE_NAME = "NAME";
        public static string VARIABLE_CODE = "CODE";
        public static string VARIABLE_CHAM = "CHAMBER";

        public static string VARIABLE_QE = "QE";
        public static string VARIABLE_PROD = "PROD";

        public static string VARIABLE_PASS = "PASS";
        public static string VARIABLE_GOLDEN_TUNNEL = "GOLDEN_TUNNEL ";

        public static string ALARM = "ALARM";
        public static string FAULT = "FAULT";
        public static string WARNING = "WARNING";

        public static string VARIABLE_DEFAULT = "DEFAULT";
        public static string VARIABLE_DEFAULT_SPEC_MODEL_GROUP = "DEFAULT";

        public static string VARIABLE_VALUE = "VALUE";
        public static string VARIABLE_SIGMA = "SIGMA";
        public static string VARIABLE_CONSTANT = "CONSTANT";

        //SPC-1284 STELLA KIM : multi calculation 화면 one side chart option 적용
        public static string VARIABLE_LOWER = "LOWER";
        public static string VARIABLE_UPPER = "UPPER";
        public static string VARIABLE_BOTH = "BOTH";

        public static string VARIABLE_SUM_TYPE_AVERAGE = "Average";
        public static string VARIABLE_SUM_TYPE_MINIMUM = "Minimum";
        public static string VARIABLE_SUM_TYPE_MAXIMUM = "Maximum";
        public static string VARIABLE_SUM_TYPE_STD_DEVIATION = "STD.Deviation";

        public static string VARIABLE_GROUP_TYPE_BY_STEP = "By Step";
        public static string VARIABLE_GROUP_TYPE_BY_STEP_AND_GLASS = "By Step and Glass";
        public static string VARIABLE_GROUP_TYPE_BY_LOT_AND_STEP = "By Lot and Step";
        //public static string VARIABLE_SPC_INITIAL_DISPLAY_CHART = "SPC_INITIAL_DISPLAY_CHART";
        public static string VARIABLE_UNASSIGNED_MODEL = "UNASSIGNED MODEL";
        public static string VARIABLE_USE_COMMA = "USE_COMMA";

        #endregion

        #region : MESSAGE KEY

        public static string GENERAL_SAVE_SUCCESS = "GENERAL_SAVE_SUCCESS";
        public static string GENERAL_SAVE_FAIL = "GENERAL_SAVE_FAIL";

        public static string MSG_KEY_NO_CHANGE_DATA = "FDC_NO_CHANGE_DATA";
        public static string MSG_KEY_SUCCESS_SAVE_CHANGE_DATA = "FDC_SUCCESS_SAVE_CHANGE_DATA";
        public static string MSG_KEY_FAIL_SAVE_CHANGE_DATA = "FDC_FAIL_SAVE_CHANGE_DATA";
        public static string MSG_KEY_ALLOW_SINGLE_ONE = "FDC_ALLOW_SINGLE_ONE";
        public static string MSG_KEY_NO_SELECTED_ROW = "FDC_NO_SELECTED_ROW";
        public static string MSG_KEY_NO_TWO_SELECTED_ROW = "FDC_NO_TWO_SELECTED_ROW";
        public static string MSG_KEY_ALLOW_SINGLE_SELECTED_ROW = "FDC_ALLOW_SINGLE_SELECTED_ROW";
        public static string MSG_KEY_NO_SPEC_DATA = "FDC_NO_SPEC_DATA";
        public static string MSG_KEY_FAIL_VALIDATE_INPUT_DATA = "FDC_FAIL_VALIDATE_INPUT_DATA";
        public static string MSG_KEY_EXISTED_INPUT_DATA = "FDC_EXISTED_INPUT_DATA";
        public static string MSG_KEY_SELECT_INPUT_DATA = "FDC_SELECT_INPUT_DATA";
        public static string MSG_KEY_DUPLICATED_INPUT_DATA = "FDC_DUPLICATED_INPUT_DATA";
        public static string MSG_KEY_EXIST_CHANGED_DATA = "FDC_EXIST_CHANGED_DATA";
        public static string MSG_KEY_NOT_PERMIT_BUTTON_BY_NEW_DATA = "FDC_NOT_PERMIT_BUTTON_BY_NEW_DATA";
        public static string MSG_KEY_NOT_ALLOW_ALL = "FDC_NOT_ALLOW_ALL";
        public static string MSG_KEY_CONFIRM_TO_DELETE = "FDC_CONFIRM_TO_DELETE";
        public static string MSG_KEY_NO_SEARCH_DATA = "FDC_NO_SEARCH_DATA";
        public static string MSG_KEY_NO_INPUT_DATA = "FDC_NO_INPUT_DATA";
        public static string MSG_KEY_NO_SEARCH_INPUT_DATA = "FDC_NO_SEARCH_INPUT_DATA";
        public static string MSG_KEY_WRONG_SAVE_DATA = "FDC_WRONG_SAVE_DATA";
        public static string MSG_KEY_WRONG_SELECTED_SEARCH_DATA = "FDC_WRONG_SELECTED_SEARCH_DATA";
        public static string MSG_KEY_SET_DEFAULT_SPEC = "FDC_SET_DEFAULT_SPEC";

        public static string MSG_KEY_SELECT_OTHER_PARAMETER = "FDC_SELECT_OTHER_PARAMETER";

        public static string MSG_KEY_INPUT_SPEC_FOR_ACTIVATE = "FDC_INPUT_SPEC_FOR_ACTIVATE";
        public static string MSG_KEY_NO_SELECTED_PARAMETER = "FDC_NO_SELECTED_PARAMETER";
        public static string MSG_KEY_NO_REGISTERED_RULE = "FDC_NO_REGISTERED_RULE";
        public static string MSG_KEY_NOTHING_HISTORY_DATA = "FDC_NOTHING_HISTORY_DATA";
        public static string MSG_KEY_CHECK_SPCRULE_INPUT_VALUE = "FDC_CHECK_SPCRULE_INPUT_VALUE";
        public static string MSG_KEY_CHECK_INPUT_VALUE = "FDC_CHECK_INPUT_VALUE";

        public static string MSG_KEY_ERROR_SELECT_SAME_DATA = "FDC_ERROR_SELECT_SAME_DATA";
        public static string MSG_KEY_ERROR_SELECT_ONE_DATA = "FDC_ERROR_SELECT_ONE_DATA";
        public static string MSG_KEY_ERROR_SAME_DATA = "FDC_ERROR_SAME_DATA";
        public static string MSG_KEY_ERROR_SELECT_DATA_COUNT = "FDC_ERROR_SELECT_DATA_COUNT";

        public static string MSG_KEY_ERROR_CONTROL_SPEC_TOL = "FDC_ERROR_CONTROL_SPEC_TOL";
        public static string MSG_KEY_ERROR_CONTROL_HARD_TOL = "FDC_ERROR_CONTROL_HARD_TOL";
        public static string MSG_KEY_ERROR_SPEC_CONTROL_TOL = "FDC_ERROR_SPEC_CONTROL_TOL";
        public static string MSG_KEY_ERROR_SPEC_HARD_TOL = "FDC_ERROR_SPEC_HARD_TOL";
        public static string MSG_KEY_ERROR_HARD_CONTROL_TOL = "FDC_ERROR_HARD_CONTROL_TOL";
        public static string MSG_KEY_ERROR_HARD_SPEC_TOL = "FDC_ERROR_HARD_SPEC_TOL";

        public static string MSG_KEY_ERROR_SPEC_CONTROL_PERCENT = "FDC_ERROR_SPEC_CONTROL_PERCENT";
        public static string MSG_KEY_ERROR_HARD_CONTROL_PERCENT = "FDC_ERROR_HARD_CONTROL_PERCENT";
        public static string MSG_KEY_ERROR_HARD_SPEC_PERCENT = "FDC_ERROR_HARD_SPEC_PERCENT";

        public static string MSG_KEY_ERROR_INPUT_TOLERENECE = "FDC_ERROR_INPUT_TOLERENECE";
        public static string MSG_KEY_ERROR_INTUT_NULL = "FDC_ERROR_INTUT_NULL";
        public static string MSG_KEY_ERROR_INTUT_STRING = "FDC_ERROR_INTUT_STRING";

        public static string MSG_KEY_ERROR_UPPER_INPUT_CALCULATION_DATA = "FDC_ERROR_UPPER_INPUT_CALCULATION_DATA";
        public static string MSG_KEY_ERROR_LOWER_INPUT_CALCULATION_DATA = "FDC_ERROR_LOWER_INPUT_CALCULATION_DATA";
        public static string MSG_KEY_ERROR_TARGET_NULL = "FDC_ERROR_TARGET_NULL";
        public static string MSG_KEY_NOT_SEARCH_LOT_LIST = "FDC_NOT_SEARCH_LOT_LIST";

        public static string MSG_KEY_ERROR_BAND_ORDER = "FDC_ERROR_BAND_ORDER";
        public static string MSG_KEY_ERROR_SETTLING_ACTIVATION_TIME = "FDC_ERROR_SETTLING_ACTIVATION_TIME";
        public static string MSG_KEY_ERROR_NULL_SETTLING_ACTIVATION_TIME = "FDC_ERROR_NULL_SETTLING_ACTIVATION_TIME";

        public static string MSG_KEY_ERROR_VALIDATE_SAVE_DATA = "FDC_ERROR_VALIDATE_SAVE_DATA";

        public static string MSG_KEY_ERROR_VALIDATE_SAVE_DATA_CASE1 = "FDC_ERROR_VALIDATE_SAVE_DATA_CASE1";
        public static string MSG_KEY_ERROR_VALIDATE_SAVE_DATA_CASE2 = "FDC_ERROR_VALIDATE_SAVE_DATA_CASE2";
        public static string MSG_KEY_ERROR_VALIDATE_SAVE_DATA_CASE3 = "FDC_ERROR_VALIDATE_SAVE_DATA_CASE3";



        public static string MSG_KEY_SUCCESS_SAVE_FAVORITE_CONDITION = "FDC_SUCCESS_SAVE_FAVORITE_CONDITION";
        public static string MSG_KEY_NO_TRX_DATA = "FDC_NO_TRX_DATA";
        public static string MSG_KEY_SELECT_CONDITION_DATA = "FDC_SELECT_CONDITION_DATA";
        public static string MSG_KEY_NO_FUNCTION_WITHOUT_DRAW_CHART = "FDC_NO_FUNCTION_WITHOUT_DRAW_CHART";


        public static string MSG_KEY_IDLE_DATA = "FDC_IDLE_DATA";
        public static string MSG_KEY_EXCEL_NOT_EXIST = "FDC_EXCEL_NOT_EXIST";
        public static string MSG_KEY_ERROR_INPUT_DATA = "FDC_ERROR_INPUT_DATA";

        public static string MSG_KEY_INFO_SEARCHING_DATA = "FDC_INFO_SEARCHING_DATA";
        public static string MSG_KEY_INFO_SAVING_DATA = "FDC_INFO_SAVING_DATA";
        public static string MSG_KEY_INFO_DRAWING_DATA = "FDC_INFO_DRAWING_DATA";
        public static string MSG_KEY_INFO_VALIDATING_DATA = "FDC_INFO_VALIDATING_DATA";


        public static string MSG_KEY_SUCCESS_SAVE_DATA = "FDC_SUCCESS_SAVE_DATA";
        public static string MSG_KEY_ERROR_SEARCH_LOT_ID = "FDC_ERROR_SEARCH_LOT_ID";
        public static string MSG_KEY_ERROR_NO_EXIST_PARAM = "FDC_ERROR_NO_EXIST_PARAM";
        public static string MSG_KEY_ERROR_NOT_CONFIRM_DATA_EIXST = "FDC_ERROR_NOT_CONFIRM_DATA_EIXST";

        public static string MSG_KEY_INFO_CHANGE_SPEC_GROUP = "FDC_INFO_CHANGE_SPEC_GROUP";
        public static string MSG_KEY_INFO_DELETE_DATA = "FDC_INFO_DELETE_DATA";
        public static string MSG_KEY_INFO_NO_DELETE_SPEC_GROUP = "FDC_INFO_NO_DELETE_SPEC_GROUP";

        public static string MSG_KEY_FDC_ERROR_SEARCH_DATE = "FDC_ERROR_SEARCH_DATE";
        public static string MSG_KEY_FDC_ERROR_NOT_EXISTED_NEW_DATA = "FDC_ERROR_NOT_EXISTED_NEW_DATA";

        public static string MSG_KEY_FDC_ERROR_SAME_SOURCE_TARGET_RECIPE_ID = "FDC_ERROR_SAME_SOURCE_TARGET_RECIPE_ID";

        public static string MSG_KEY_SUCCESS_DELETE_DATA = "FDC_SUCCESS_DELETE_DATA";
        public static string MSG_KEY_FAIL_DELETE_DATA = "FDC_FAIL_DELETE_DATA";

        public static string MSG_KEY_SUCCESS_UPDATE_DATA = "FDC_SUCCESS_UPDATE_DATA";
        public static string MSG_KEY_FAIL_UPDATE_DATA = "FDC_FAIL_UPDATE_DATA";
        public static string MSG_KEY_INFO_SPEC_DATA_EXIST_IN_SPEC_MODEL = "FDC_INFO_SPEC_DATA_EXIST_IN_SPEC_MODEL";
        public static string MSG_KEY_INFO_FINDCOLUMNDATA_SELECT_COLUMN = "FDC_INFO_FINDCOLUMNDATA_SELECT_COLUMN";

        public static string MSG_KEY_INFO_FINDCOLUMNDATA_CLICK_SEARCH = "FDC_INFO_FINDCOLUMNDATA_CLICK_SEARCH";
        public static string MSG_KEY_INFO_FINDCOLUMNDATA_NO_MATCH_DATA = "FDC_INFO_FINDCOLUMNDATA_NO_MATCH_DATA";

        public static string MSG_KEY_ALREADY_EXIST_ACTIVATE_SPEC_MODEL = "FDC_ALREADY_EXIST_ACTIVATE_SPEC_MODEL";

        public static string MSG_KEY_CHART_X_AXIS_ALIGN_ERROR = "FDC_ERROR_CHART_X_AXIS_ALIGN";

        public static string MSG_KEY_INFO_ALLOW_SINGLE_EQP_CONDITION = "FDC_INFO_ALLOW_SINGLE_EQP_CONDITION";
        public static string MSG_KEY_INFO_INFO_NEED_CONTROL_LIMIT = "FDC_INFO_NEED_CONTROL_LIMIT";

        public static string MSG_KEY_INFO_CONFIRM_LOT_SEARCH = "DCP_CONFIRM_LOT_SEARCH";

        public const string MSG_KEY_INFO_CLICK_SERIES = "SPC_INFO_CLICK_SERIES";
        public const string MSG_KEY_INFO_SELECT_RULE = "SPC_INFO_SELECT_RULE";
        public const string MSG_KEY_INFO_SEARCH_SAMPLE_DATA = "SPC_INFO_SEARCH_SAMPLE_DATA";
        public const string MSG_KEY_INFO_INPUT_OUTLIER_VALUE = "SPC_INFO_INPUT_OUTLIER_VALUE";

        public const string MSG_KEY_WARNING_IMPORT_COLUMN_VALUE_CANT_BE_MODIFIED =
            "SPC_WARNING_IMPORT_COLUMN_VALUE_CANT_BE_MODIFIED";
        public const string MSG_KEY_WARNING_IMPORT_COLUMN_IS_NOT_EXIST = "SPC_WARNING_IMPORT_COLUMN_IS_NOT_EXIST";
        public const string MSG_KEY_WARNING_IMPORT_ROW_COUNT_DIFFERENT= "SPC_WARNING_IMPORT_ROW_COUNT_DIFFERENT";
        public const string MSG_KEY_NO_PASTE_ITEM = "SPC_INFO_NO_ITEM_PASTE";
        #endregion


        #region : TYPE DISPLAY

        public static string TYPE_DISPLAY_LINE = "LINE";
        public static string TYPE_DISPLAY_AREA = "AREA";
        public static string TYPE_DISPLAY_EQP_MODEL = "EQP_MODEL";
        public static string TYPE_DISPLAY_EQP_ID = "EQP_ID";
        public static string TYPE_DISPLAY_MODULE_ID = "MODULE_ID";

        #endregion



        public static string DATETIME_FORMAT_YMD = "yyyyMMdd";        
        public static string DATETIME = "yyyy-MM-dd";        
        public static string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public static string DATETIME_FORMAT_yyyyMMdd = "yyyy/MM/dd";
        public static string DATETIME_FORMAT_yyMMdd = "yy-MM-dd";
        public static string DATETIME_FORMAT_MS = "yyyy-MM-dd HH:mm:ss.fff";
        public static string SPC_DATETIME_FORMAT_OCAP = "SPC_DATETIME_FORMAT_OCAP";
        

        public static string PARAM_DATA_TYPE_INT = "INT";
        public static string PARAM_DATA_TYPE_FLT = "FLT";
        public static string PARAM_DATA_TYPE_STR = "STR";

        public static string FCONDITION_CD_LINE = "LI";
        public static string FCONDITION_CD_AREA = "AR";
        public static string FCONDITION_CD_EQP_MODEL = "EM";
        public static string FCONDITION_CD_EQP_ID = "EQ";
        public static string FCONDITION_CD_MODULE_ID = "MI";
        public static string FCONDITION_CD_DEFAULT_PERIOD = "DP";


        #region : IMAGE PATH

        public static string IMAGE_PATH = "6_5 Element_Common Button\\images\\";
        public static string TREE_ICON_SITE = "TreeNode_Site.png";
        public static string TREE_ICON_FAB = "TreeNode_Fab.png";
        public static string TREE_ICON_LINE = "TreeNode_Line.png";
        public static string TREE_ICON_AREA = "TreeNode_Area.png";
        public static string TREE_ICON_EQP_MODEL = "TreeNode_EQPModel.png";
        public static string TREE_ICON_DCP_ACTIVE = "TreeNode_DataCollectionPlan_Activated.png";
        public static string TREE_ICON_DCP_DEACTIVE = "TreeNode_DataCollectionPlan_Deactivated.png";
        public static string TREE_ICON_EQP = "TreeNode_EQP.png";
        public static string TREE_ICON_MODULE = "TreeNode_Module.png";
        public static string TREE_ICON_PRODUCT = "TreeNode_Product.png";
        public static string TREE_ICON_RECIPE = "TreeNode_Recipe.png";
        public static string TREE_ICON_STEP = "TreeNode_Step.png";
        public static string TREE_ICON_SPECGROUP = "TreeNode_SPECGroup.png";
        public static string TREE_ICON_PARAMETER = "TreeNode_Parameter.png";
        public static string TREE_ICON_TRACE_PARAM = "TreeNode_TraceParameter.png";
        public static string TREE_ICON_TRACE_SUM_PARAM = "TreeNode_TraceSummaryParameter.png";
        public static string TREE_ICON_EVENT_SUM_PARAM = "TreeNode_EventSummaryParameter.png";
        public static string TREE_ICON_EXTERNAL_PARAM = "TreeNode_ExternalParameter.png";
        public static string TREE_ICON_VIRTUAL_PARAM = "TreeNode_VirtualParameter.png";
        public static string TREE_ICON_MULTIVARIATE_MODEL = "TreeNode_MultivariateModel.png";
        public static string TREE_ICON_PARAM_GROUP = "TreeNode_ParameterGroup.png";

        public static string EQP_STATUS_PATH = "9_1 Page_spec/9_1_7 SPC/images/";
        public static string EQP_STATUS_SAFE = "BulletStatus_Safe.png";
        public static string EQP_STATUS_WARNING = "BulletStatus_Warning.png";
        public static string EQP_STATUS_FAULT = "BulletStatus_Fault.png";
        public static string EQP_STATUS_NOMODEL = "BulletStatus_NoModel.png";
        public static string EQP_STATUS_IDEL = "BulletStatus_Idle.png";
        public static string EQP_STATUS_NODATA = "BulletStatus_NoData.png";
        public static string EQP_STATUS_OFFLINE = "BulletStatus_OffLine.png";
        public static string EQP_STATUS_DISABLE = "BulletStatus_Disable.png";
        public static string EQP_STATUS_UNKOWN = "BulletStatus_Unknown.png";
        public static string EQP_STATUS_PM = "BulletStatus_PM.png";
        public static string EQP_STATUS_BM = "BulletStatus_BM.png";

        #endregion

        #region : Log
        public static string APPLICATION_NAME = "ESPC";
        public static string TEMP_TIME = "Trace_Time";

        #endregion
        
        #region : INTERFACE TAGET INFO

        //TIB
        public static string TARGET_SEND_SERVER_TIB = "SEND_SERVER_TIB";
        public static string TARGET_SEND_CLIENT_TIB = "SEND_CLIENT_TIB";

        //ActiveMQ
        public static string TARGET_SEND_SERVER_AMQ = "SEND_SERVER_AMQ";
        public static string TARGET_SEND_CLIENT_AMQ = "SEND_CLIENT_AMQ";

        public static string TARGET_MSG_BUS_INFO = "MSG_BUS_INFO";
        public static string TARGET_MSG_BUS_INFO_AMQ = "AMQ";
        public static string TARGET_MSG_BUS_INFO_TIB = "TIB";

        public static string ACTIVEMQ_DESTINATION_TYPE_QUEUE = "QUEUE";
        public static string ACTIVEMQ_DESTINATION_TYPE_TOPIC = "TOPIC";

        public static string TARGET_CONFIG_ITEM_MSG_BUS_TYPE = "TYPE";

        public static string TARGET_CONFIG_ITEM_SUBJECT = "SUBJECT";
        public static string TARGET_CONFIG_ITEM_NETWORK = "NETWORK";
        public static string TARGET_CONFIG_ITEM_SERVICE = "SERVICE";
        public static string TARGET_CONFIG_ITEM_DAEMON = "DAEMON";
        public static string TARGET_CONFIG_ITEM_PROTOCOL_TYPE_CD = "PROTOCOL_TYPE_CD";
        public static string TARGET_CONFIG_ITEM_DATA_TYPE_CD = "DATA_TYPE_CD";

        public static string TARGET_CONFIG_ITEM_BROKER_URL = "BROKER_URL";
        public static string TARGET_CONFIG_ITEM_DESTINATION_TYPE = "DESTINATION_TYPE";


        public static string MSG_INTERFACE = "";

        public static string CLIENT_SUBJECT = "";
        public static string CLIENT_SERVICE = "";
        public static string CLIENT_NETWORK = "";
        public static string CLIENT_DAEMON = "";

        public static string SERVER_SUBJECT = "";
        public static string SERVER_SERVICE = "";
        public static string SERVER_NETWORK = "";
        public static string SERVER_DAEMON = "";

        #endregion


        #region : BLOB FIELD NAME

        public struct BLOB_FIELD_NAME
        {
            //header
            public static string HEADER_OPENING = "<header>";
            public static string HEADER_CLOSING = "</header>";
            public static string PARAM_NAME = "param_name";
            public static string LINE_INFO = "line_info";
            public static string UNIT = "unit";
            public static string SUM_TYPE_CD = "sum_type_cd";

            //line_info
            public static string TIME = "time";
            public static string LOTID = "lotid";
            public static string SUBSTRATEID = "substrateid";
            public static string SLOT = "slot";
            public static string PPID = "ppid";
            public static string RECIPE = "recipe";
            public static string PRODUCT = "product";
            public static string STEP = "step";
            public static string STATUS = "status";


            //SPC-929, KBLEE, START
            public const string EQP_ID = "eqp_id";
            public const string MODULE_ID = "module_id";
            public const string MODULE_ALIAS = "module_alias";
            public const string SUM_VALUE = "sum_value";
            public const string PARAM_INFO = "param_info";
            public const string SUBSTRATE_INFO = "substrate_info";
            public const string STEP_INFO = "step_info";
            public const string STEPNAME = "stepname";
            //SPC-929, KBLEE, END
            


            //param_name
            public static string PARAM_VALUE = "value";
            public static string TARGET = "target";
            public static string LSL = "lsl";
            public static string LCL = "lcl";
            public static string UCL = "ucl";
            public static string USL = "usl";
            public static string FAULT_RULE_LIST = "fault_rule_list";
            public static string PARAM_VALUE_LIST = "param_value_list";
            
            public static string REF_PARAM_LIST = "ref_param_list";
            public static string REF_PARAM_ALIAS = "ref_param_alias";
            public static string REF_DATA = "ref_data";
            
            public static string PARAM_LIST = "param_list";
            public static string PARAM_ALIAS = "param_alias";
                  
            //param_name list
            public static string RAW = "RAW";
            public static string MIN = "MIN";
            public static string MAX = "MAX";
            public static string MEAN = "MEAN";
            public static string STDDEV = "STDDEV";
            public static string RANGE = "RANGE";
            public static string MA = "MA";
            public static string MSD = "MSD";
            public static string MR = "MR";
            public static string EWMARANGE = "EWMARANGE";
            public static string EWMAMEAN = "EWMAMEAN";
            public static string EWMASTDDEV = "EWMASTDDEV";

            public static string P = "P";
            public static string PN = "PN";
            public static string C = "C";
            public static string U = "U";
        }

        #endregion

        public class ParameterList : System.Collections.Generic.List<ParameterInfo>
        { }

        public class ParameterInfo
        {
            string _parameterName;
            string _dataType;
            string _role;

            public ParameterInfo(string parameterName, string dataType, string role)
            {
                this._parameterName = parameterName;
                this._dataType = dataType;
                this._role = role;
            }

            public string PARAMETER_NAME
            {
                get { return this._parameterName; }
            }

            public string DATA_TYPE
            {
                get { return this._dataType; }
            }

            public string VARIABLE_ROLE
            {
                get { return this._role; }
            }
        }

        //SPC-929, KBLEE, START
        public struct DATA_CATEGORY_CODE
        {
            public static string TS = "TS";
            public static string EV = "EV";
            public static string MP = "MP";
        }
        //SPC-929, KBLEE, END

        //SPC-929, KBLEE, START
        public struct SUMMARY_DATA
        {
            public const string DATA_EXIST_MODE = "DATA_EXIST_MODE";
            public const string ALL_EXIST = "ALL_EXIST";
            public const string ONLY_RECIPE_EXIST = "ONLY_RECIPE_EXIST";
            public const string ONLY_STEP_EXIST = "ONLY_STEP_EXIST";
        }
        //SPC-929, KBLEE, END


        #region Copy SPC Model.
        public struct COPY_MODEL
        {
            public static string SOURCE_MODEL_CONFIG_RAWID = "SOURCE_MODEL_CONFIG_RAWID";
            public static string TARGET_MODEL_CONFIG_RAWID = "TARGET_MODEL_CONFIG_RAWID";

            public static string CONTEXT_INTERLOCK = "CONTEXT_INTERLOCK";
            public static string CONTEXT_USE_EXTERNAL_SPEC_LIMIT = "CONTEXT_USE_EXTERNAL_SPEC_LIMIT";
            public static string CONTEXT_AUTO_CALCULATION = "CONTEXT_AUTO_CALCULATION";
            public static string CONTEXT_AUTO_GENERATE_SUB_CHART = "CONTEXT_AUTO_GENERATE_SUB_CHART";
            public static string CONTEXT_ACTIVE = "CONTEXT_ACTIVE";
            public static string CONTEXT_MODE = "CONTEXT_MODE";
            public static string CONTEXT_SAMPLE_COUNT = "CONTEXT_SAMPLE_COUNT";
            public static string CONTEXT_MANAGE_TYPE = "CONTEXT_MANAGE_TYPE";
            public static string CONTEXT_AUTO_SETTING = "CONTEXT_AUTO_SETTING";
            public static string CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK = "CONTEXT_GENERATE_SUB_CHART_WITH_INTERLOCK";
            public static string CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION = "CONTEXT_GENERATE_SUB_CHART_WITH_AUTO_CALCULATION";
            public static string CONTEXT_INHERIT_THE_SPEC_OF_MAIN = "CONTEXT_INHERIT_THE_SPEC_OF_MAIN";
            public static string CONTEXT_USE_NORMALIZATION_VALUE = "CONTEXT_USE_NORMALIZATION_VALUE";
            public static string CONTEXT_CONTEXT_INFORMATION = "CONTEXT_CONTEXT_INFORMATION"; //SPC-1218, KBLEE

            //SPC-676 by Louis
            public static string CONTEXT_CHART_DESCRIPTION = "CONTEXT_CHART_DESCRIPTION";

            public static string RULE_MASTER_SPEC_LIMIT = "RULE_MASTER_SPEC_LIMIT";
            public static string RULE_RAW = "RULE_RAW";
            public static string RULE_MEAN = "RULE_MEAN";
            public static string RULE_STD = "RULE_STD";
            public static string RULE_RANGE = "RULE_RANGE";
            public static string RULE_EWMA_MEAN = "RULE_EWMA_MEAN";
            public static string RULE_EWMA_STD = "RULE_EWMA_STD";
            public static string RULE_EWMA_RANGE = "RULE_EWMA_RANGE";
            public static string RULE_MA = "RULE_MA";
            public static string RULE_MS = "RULE_MS";
            public static string RULE_MR = "RULE_MR";
            public static string RULE_TECHNICAL_LIMIT = "RULE_TECHNICAL_LIMIT";
            public static string RULE_MOVING_OPTIONS = "RULE_MOVING_OPTIONS";
            public static string RULE_MEAN_OPTIONS = "RULE_MEAN_OPTIONS";
            public static string RULE_ZONE_A = "RULE_ZONE_A";
            public static string RULE_ZONE_B = "RULE_ZONE_B";
            public static string RULE_ZONE_C = "RULE_ZONE_C";
            public static string RULE_RULE_SELECTION = "RULE_RULE_SELECTION";

            public static string RULE_PN_SPEC_LIMIT = "RULE_PN_SPEC_LIMIT";
            public static string RULE_C_SPEC_LIMIT = "RULE_C_SPEC_LIMIT";
            public static string RULE_PN_CONTROL = "RULE_PN_CONTROL";
            public static string RULE_C_CONTROL = "RULE_C_CONTROL";
            public static string RULE_P_CONTROL = "RULE_P_CONTROL";
            public static string RULE_U_CONTROL = "RULE_U_CONTROL";

            public static string OPTION_PARAMETER_CATEGORY = "OPTION_PARAMETER_CATEGORY";
            public static string OPTION_CALCULATE_PPK = "OPTION_CALCULATE_PPK";
            public static string OPTION_PRIORITY = "OPTION_PRIORITY";
            public static string OPTION_SAMPLE_COUNTS = "OPTION_SAMPLE_COUNTS";
            public static string OPTION_DAYS = "OPTION_DAYS";
            public static string OPTION_DEFAULT_CHART_TO_SHOW = "OPTION_DEFAULT_CHART_TO_SHOW";

            public static string AUTO_AUTO_CALCULATION_PERIOD = "AUTO_AUTO_CALCULATION_PERIOD";
            public static string AUTO_AUTO_CALCULATION_COUNT = "AUTO_AUTO_CALCULATION_COUNT";

            //SPC-658 Initial Calc Count
            public static string AUTO_AUTO_CALCULATION_INITIAL_COUNT = "AUTO_AUTO_CALCULATION_INITIAL_COUNT";

            public static string AUTO_MINIMUM_SAMPLES_TO_USE = "AUTO_MINIMUM_SAMPLES_TO_USE";
            public static string AUTO_DEFAULT_PERIOD = "AUTO_DEFAULT_PERIOD";
            public static string AUTO_MAXIMUM_PERIOD_TO_USE = "AUTO_MAXIMUM_PERIOD_TO_USE";
            public static string AUTO_CONTROL_LIMIT_TO_USE = "AUTO_CONTROL_LIMIT_TO_USE";
            public static string AUTO_CONTROL_LIMIT_THREASHOLD = "AUTO_CONTROL_LIMIT_THREASHOLD";
            public static string AUTO_RAW_CONTROL_LIMIT = "AUTO_RAW_CONTROL_LIMIT";
            public static string AUTO_MEAN_CONTROL_LIMIT = "AUTO_MEAN_CONTROL_LIMIT";
            public static string AUTO_STD_CONTROL_LIMIT = "AUTO_STD_CONTROL_LIMIT";
            public static string AUTO_RANGE_CONTROL_LIMIT = "AUTO_RANGE_CONTROL_LIMIT";
            public static string AUTO_EWMA_MEAN_CONTROL_LIMIT = "AUTO_EWMA_MEAN_CONTROL_LIMIT";
            public static string AUTO_EWMA_STD_CONTROL_LIMIT = "AUTO_EWMA_STD_CONTROL_LIMIT";
            public static string AUTO_EWMA_RANGE_CONTROL_LIMIT = "AUTO_EWMA_RANGE_CONTROL_LIMIT";
            public static string AUTO_MA_CONTROL_LIMIT = "AUTO_MA_CONTROL_LIMIT";
            public static string AUTO_MS_CONTROL_LIMIT = "AUTO_MS_CONTROL_LIMIT";
            public static string AUTO_STD_CALCULATION = "AUTO_STD_CALCULATION";
            public static string AUTO_ZONE_CALCULATION = "AUTO_ZONE_CALCULATION";
            public static string AUTO_CALCULATION_WITH_SHIFT_COMPENSATION = "AUTO_CALCULATION_WITH_SHIFT_COMPENSATION";
            public static string AUTO_CALCULATION_WITHOUT_IQR_FILTER = "AUTO_CALCULATION_WITHOUT_IQR_FILTER";

            public static string AUTO_PN_CONTROL_LIMIT = "AUTO_PN_CONTROL_LIMIT";
            public static string AUTO_P_CONTROL_LIMIT = "AUTO_P_CONTROL_LIMIT";
            public static string AUTO_C_CONTROL_LIMIT = "AUTO_C_CONTROL_LIMIT";
            public static string AUTO_U_CONTROL_LIMIT = "AUTO_U_CONTROL_LIMIT";

            //SPC-731 건으로 Threshold Column추가로 인한 수정 - 2012.02.06일
            public static string AUTO_CALCULATION_THRESHOLD_OFF_YN = "AUTO_CALCULATION_THRESHOLD_OFF_YN";

            //SPC-1155 by stella
            public static string AUTO_USE_GLOBAL_YN = "AUTO_USE_GLOBAL_YN";
            public static string AUTO_CONTROL_LIMIT_OPTION = "AUTO_CONTROL_LIMIT_OPTION";
            public static string AUTO_MR_CONTROL_LIMIT = "AUTO_MR_CONTROL_LIMIT";
            public static string AUTO_RAW_CALCULATION_VALUE = "AUTO_RAW_CALCULATION_VALUE";
            public static string AUTO_MEAN_CALCULATION_VALUE = "AUTO_MEAN_CALCULATION_VALUE";
            public static string AUTO_STD_CALCULATION_VALUE = "AUTO_STD_CALCULATION_VALUE";
            public static string AUTO_RANGE_CALCULATION_VALUE = "AUTO_RANGE_CALCULATION_VALUE";
            public static string AUTO_EWMA_MEAN_CALCULATION_VALUE = "AUTO_EWMA_MEAN_CALCULATION_VALUE";
            public static string AUTO_EWMA_STD_CALCULATION_VALUE = "AUTO_EWMA_STD_CALCULATION_VALUE";
            public static string AUTO_EWMA_RANGE_CALCULATION_VALUE = "AUTO_EWMA_RANGE_CALCULATION_VALUE";
            public static string AUTO_MA_CALCULATION_VALUE = "AUTO_MA_CALCULATION_VALUE";
            public static string AUTO_MS_CALCULATION_VALUE = "AUTO_MS_CALCULATION_VALUE";
            public static string AUTO_MR_CALCULATION_VALUE = "AUTO_MR_CALCULATION_VALUE";

            public static string AUTO_RAW_CALCULATION_OPTION = "AUTO_RAW_CALCULATION_OPTION";
            public static string AUTO_MEAN_CALCULATION_OPTION = "AUTO_MEAN_CALCULATION_OPTION";
            public static string AUTO_STD_CALCULATION_OPTION = "AUTO_STD_CALCULATION_OPTION";
            public static string AUTO_RANGE_CALCULATION_OPTION = "AUTO_RANGE_CALCULATION_OPTION";
            public static string AUTO_EWMA_MEAN_CALCULATION_OPTION = "AUTO_EWMA_MEAN_CALCULATION_OPTION";
            public static string AUTO_EWMA_STD_CALCULATION_OPTION = "AUTO_EWMA_STD_CALCULATION_OPTION";
            public static string AUTO_EWMA_RANGE_CALCULATION_OPTION = "AUTO_EWMA_RANGE_CALCULATION_OPTION";
            public static string AUTO_MA_CALCULATION_OPTION = "AUTO_MA_CALCULATION_OPTION";
            public static string AUTO_MS_CALCULATION_OPTION = "AUTO_MS_CALCULATION_OPTION";
            public static string AUTO_MR_CALCULATION_OPTION = "AUTO_MR_CALCULATION_OPTION";

            public static string AUTO_RAW_CALCULATION_SIDED = "AUTO_RAW_CALCULATION_SIDED";
            public static string AUTO_MEAN_CALCULATION_SIDED = "AUTO_MEAN_CALCULATION_SIDED";
            public static string AUTO_STD_CALCULATION_SIDED = "AUTO_STD_CALCULATION_SIDED";
            public static string AUTO_RANGE_CALCULATION_SIDED = "AUTO_RANGE_CALCULATION_SIDED";
            public static string AUTO_EWMA_MEAN_CALCULATION_SIDED = "AUTO_EWMA_MEAN_CALCULATION_SIDED";
            public static string AUTO_EWMA_STD_CALCULATION_SIDED = "AUTO_EWMA_STD_CALCULATION_SIDED";
            public static string AUTO_EWMA_RANGE_CALCULATION_SIDED = "AUTO_EWMA_RANGE_CALCULATION_SIDED";
            public static string AUTO_MA_CALCULATION_SIDED = "AUTO_MA_CALCULATION_SIDED";
            public static string AUTO_MS_CALCULATION_SIDED = "AUTO_MS_CALCULATION_SIDED";
            public static string AUTO_MR_CALCULATION_SIDED = "AUTO_MR_CALCULATION_SIDED";

            //SPC-1155 by stella
        }
        #endregion

        public struct SPC_DATA_LEVEL
        {
            public static string SPC_DATA_LEVEL_L = "SPC_DATA_LEVEL_LOT";
            public static string SPC_DATA_LEVEL_W = "SPC_DATA_LEVEL_WAFER";
        }

        #region OCAPDetailConfig

        public static string OCAP_DETAIL_PROBLEM = "Settings/Config/OCAPDetailsProblem/@Enable";
        public static string OCAP_DETAIL_CAUSE = "Settings/Config/OCAPDetailsCause/@Enable";
        public static string OCAP_DETAIL_SOLUTION = "Settings/Config/OCAPDetailsSolution/@Enable";
        public static string OCAP_DETAIL_COMMENT = "Settings/Config/OCAPDetailsComment/@Enable";
        #endregion

        public static string CONFIG_USE_COMMA = "Settings/page/UseComma";

        #region AutoCalculation Column Mapping

        public struct AUTOCALC_COLUMN
        {
            public static string EWMA_MEAN = "EWMA_M";
            public static string EWMA_RANGE = "EWMA_R";
            public static string EWMA_STDDEV = "EWMA_S";
            public static string MA = "MA";
            public static string MR = "MR";
            public static string MSD = "MS";
            public static string RANGE = "RANGE";
            public static string RAW = "RAW";
            public static string STDDEV = "STD";
            public static string XBAR = "MEAN";
        }

        public struct AUTOCALC_CHART_TYPE
        {
            public const string EWMA_MEAN = "EWMA_MEAN";
            public const string EWMA_RANGE = "EWMA_RANGE";
            public const string EWMA_STDDEV = "EWMA_STDDEV";
            public const string MA = "MA";
            public const string MR = "MR";
            public const string MSD = "MSD";
            public const string RANGE = "RANGE";
            public const string RAW = "RAW";
            public const string STDDEV = "STDDEV";
            public const string XBAR = "XBar";
        }

        public struct MULTICALC_CHART_TYPE
        {
            public const string EWMA_MEAN = "EWMA_MEAN";
            public const string EWMA_RANGE = "EWMA_RANGE";
            public const string EWMA_STDDEV = "EWMA_STDDEV";
            public const string MA = "MA";
            public const string MR = "MR";
            public const string MSD = "MSD";
            public const string RANGE = "RANGE";
            public const string RAW = "RAW";
            public const string STDDEV = "STDDEV";
            public const string XBAR = "X-BAR";
        }

        public static string[] CALCULATION_SIDED = { "BOTH", "UPPER", "LOWER" };
        public static string[] CONTROL_LIMIT_OPTION = { "SIGMA", "PERCENT", "CONSTANT" };
        #endregion

        #region SPC Rule Selection Popup

        public const string RULE_OPT_LIST_OF_RULES = "List of Rules";
        public const string RULE_OPT_SAMPLE_COUNT = "Sample count";
        public const string RULE_OPT_VIOLATION_COUNT = "Violation count";
        public const string RULE_OPT_TREND_LIMIT = "Trend limit"; //SPC-1335, KBLEE
        
        #endregion

        public static string INCLUDE = "INCLUDE";
        public static string EXCLUDE = "EXCLUDE";
        public static string DATA_FILTER = "DATA_FILTER";
    }



}
