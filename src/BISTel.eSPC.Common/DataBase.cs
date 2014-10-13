using System;
using System.Collections.Generic;
using System.Text;

namespace BISTel.eSPC.Common
{
    public class DataBase : BISTel.PeakPerformance.Client.SQLHandler.SQLHandler
    {
        protected string _myName = null;
        protected StringBuilder _sbQuery = null;

        public DataBase() : base(Definition.DB_NAME) { }
        public DataBase(string dbName) : base(dbName) { }
    }

    public struct MESSAGE
    {
        public static string _METHOD_ID_SET_TRACE_MODEL = "SET_TRACE_MODEL";
        public static string _METHOD_ID_SET_SUMMARY_MODEL = "SET_SUMMARY_MODEL";
        public static string _METHOD_ID_SET_SPC_MODEL = "SET_SPC_MODEL";
        public static string _METHOD_ID_REVOKEWORKFLOW = "REVOKEWORKFLOW";

        public static int _TIME_OUT = 60000;

        public static string _SOURCE = "CLIENT.SPC";
        public static string _SERVICE_ID = "SPC";
        public static string _TARGET_MODEL_GROUP = "ModelGroup";
        public static string _TARGET_MODEL = "Model";

        public static string _EES_DATA_KEY_EQP_ID = "eqp_id";
    }

    public struct SEQUENCE
    {
        public const string SEQ_CODE_MST_PP = "SEQ_CODE_MST_PP";

        public const string SEQ_MODEL_MST_SPC = "SEQ_MODEL_MST_SPC";
        public const string SEQ_MODEL_CONFIG_MST_SPC = "SEQ_MODEL_CONFIG_MST_SPC";
        public const string SEQ_MODEL_CONFIG_HST_SPC = "SEQ_MODEL_CONFIG_HST_SPC";//P2
        public const string SEQ_MODEL_CONFIG_OPT_MST_SPC = "SEQ_MODEL_CONFIG_OPT_MST_SPC";
        public const string SEQ_MODEL_CONTEXT_MST_SPC = "SEQ_MODEL_CONTEXT_MST_SPC";
        //public const string SEQ_MODEL_FILTER_MST_SPC = "SEQ_MODEL_FILTER_MST_SPC";
        public const string SEQ_MODEL_RULE_MST_SPC = "SEQ_MODEL_RULE_MST_SPC";
        public const string SEQ_MODEL_RULE_OPT_MST_SPC = "SEQ_MODEL_RULE_OPT_MST_SPC";
        public const string SEQ_MODEL_AUTOCALC_MST_SPC = "SEQ_MODEL_AUTOCALC_MST_SPC"; //P2

        public const string SEQ_DEF_CONFIG_MST_SPC = "SEQ_DEF_CONFIG_MST_SPC";
        public const string SEQ_DEF_CONFIG_OPT_MST_SPC = "SEQ_DEF_CONFIG_OPT_MST_SPC";
        public const string SEQ_DEF_CONTEXT_MST_SPC = "SEQ_DEF_CONTEXT_MST_SPC";
        public const string SEQ_DEF_RULE_MST_SPC = "SEQ_DEF_RULE_MST_SPC";
        public const string SEQ_DEF_RULE_OPT_MST_SPC = "SEQ_DEF_RULE_OPT_MST_SPC";
        public const string SEQ_DEF_AUTOCALC_MST_SPC = "SEQ_DEF_AUTOCALC_MST_SPC"; //P2

        public const string SEQ_RULE_MST_SPC = "SEQ_RULE_MST_SPC";
        public const string SEQ_RULE_OPT_MST_SPC = "SEQ_RULE_OPT_MST_SPC";
        public const string SEQ_ANALYSISCHAT_CONFIG_SPC = "SEQ_ANALYSISCHAT_CONFIG_SPC"; //추가

        public const string SEQ_MODEL_ATT_MST_SPC = "SEQ_MODEL_MST_SPC";
        public const string SEQ_MODEL_CONFIG_ATT_MST_SPC = "SEQ_MODEL_CONFIG_MST_SPC";
        public const string SEQ_MODEL_CONFIG_OPT_ATT_MST_SPC = "SEQ_MODEL_CO_ATT_MST_SPC";
        public const string SEQ_MODEL_CONTEXT_ATT_MST_SPC = "SEQ_MODEL_CONTEXT_ATT_MST_SPC";
        public const string SEQ_MODEL_RULE_ATT_MST_SPC = "SEQ_MODEL_RULE_ATT_MST_SPC";
        public const string SEQ_MODEL_RULE_OPT_ATT_MST_SPC = "SEQ_MODEL_RULE_OPT_ATT_MST_SPC";
        public const string SEQ_MODEL_AUTOCALC_ATT_MST_SPC = "SEQ_MODEL_AUTOCALC_ATT_MST_SPC"; //P2

        public const string SEQ_MODEL_GROUP_MST_SPC = "SEQ_MODEL_GROUP_MST_SPC"; 
    }

    public struct TABLE
    {
        public const string CODE_MST_PP = "CODE_MST_PP";

        public const string MODEL_MST_SPC = "MODEL_MST_SPC";
        public const string MODEL_CONFIG_MST_SPC = "MODEL_CONFIG_MST_SPC";
        public const string MODEL_CONFIG_MSTH_SPC = "MODEL_CONFIG_MSTH_SPC";
        public const string MODEL_CONFIG_HST_SPC = "MODEL_CONFIG_HST_SPC"; //P2
        public const string MODEL_CONFIG_OPT_MST_SPC = "MODEL_CONFIG_OPT_MST_SPC";
        public const string MODEL_CONTEXT_MST_SPC = "MODEL_CONTEXT_MST_SPC";
        public const string MODEL_RULE_MST_SPC = "MODEL_RULE_MST_SPC";
        public const string MODEL_RULE_OPT_MST_SPC = "MODEL_RULE_OPT_MST_SPC";
        public const string MODEL_AUTOCALC_MST_SPC = "MODEL_AUTOCALC_MST_SPC"; //P2
        //public const string MODEL_FILTER_MST_SPC = "MODEL_FILTER_MST_SPC";

        public const string DEF_CONFIG_MST_SPC = "DEF_CONFIG_MST_SPC";
        public const string DEF_CONFIG_OPT_MST_SPC = "DEF_CONFIG_OPT_MST_SPC";
        public const string DEF_CONTEXT_MST_SPC = "DEF_CONTEXT_MST_SPC";
        public const string DEF_RULE_MST_SPC = "DEF_RULE_MST_SPC";
        public const string DEF_RULE_OPT_MST_SPC = "DEF_RULE_OPT_MST_SPC";
        public const string DEF_AUTOCALC_MST_SPC = "DEF_AUTOCALC_MST_SPC"; //P2

        public const string RULE_MST_SPC = "RULE_MST_SPC";
        public const string RULE_OPT_MST_SPC = "RULE_OPT_MST_SPC";

        public const string MODEL_ATT_MST_SPC = "MODEL_ATT_MST_SPC";
        public const string MODEL_CONFIG_ATT_MST_SPC = "MODEL_CONFIG_ATT_MST_SPC";
        public const string MODEL_CONFIG_ATT_MSTH_SPC = "MODEL_CONFIG_ATT_MSTH_SPC";
        public const string MODEL_CONFIG_OPT_ATT_MST_SPC = "MODEL_CONFIG_OPT_ATT_MST_SPC";
        public const string MODEL_CONTEXT_ATT_MST_SPC = "MODEL_CONTEXT_ATT_MST_SPC";
        public const string MODEL_RULE_ATT_MST_SPC = "MODEL_RULE_ATT_MST_SPC";
        public const string MODEL_RULE_OPT_ATT_MST_SPC = "MODEL_RULE_OPT_ATT_MST_SPC";
        public const string MODEL_AUTOCALC_ATT_MST_SPC = "MODEL_AUTOCALC_ATT_MST_SPC"; //P2

        public const string DEF_CONFIG_ATT_MST_SPC = "DEF_CONFIG_ATT_MST_SPC";
        public const string DEF_CONFIG_OPT_ATT_MST_SPC = "DEF_CONFIG_OPT_ATT_MST_SPC";
        public const string DEF_CONTEXT_ATT_MST_SPC = "DEF_CONTEXT_ATT_MST_SPC";
        public const string DEF_RULE_ATT_MST_SPC = "DEF_RULE_ATT_MST_SPC";
        public const string DEF_RULE_OPT_ATT_MST_SPC = "DEF_RULE_OPT_ATT_MST_SPC";
        public const string DEF_AUTOCALC_ATT_MST_SPC = "DEF_AUTOCALC_ATT_MST_SPC"; //P2

        public const string RULE_ATT_MST_SPC = "RULE_ATT_MST_SPC";
        public const string RULE_OPT_ATT_MST_SPC = "RULE_OPT_ATT_MST_SPC";

        public const string PPK_TRX_SPC = "PPK_TRX_SPC";
        public const string ANALYSISCHAT_CONFIG_SPC = "ANALYSISCHAT_CONFIG_SPC"; //추가
        public const string SPECIAL_LOT_SPC = "SPECIAL_LOT_SPC"; //추가

        public const string CHART_VW_SPC = "CHART_VW_SPC";

        //SPC-703 OOC_Comment By Louis
        public const string OOC_CAUSE_MST_SPC = "OOC_CAUSE_MST_SPC";
        public const string OOC_COMMENT_MST_SPC = "OOC_COMMENT_MST_SPC";
        public const string OOC_PROBLEM_MST_SPC = "OOC_PROBLEM_MST_SPC";
        public const string OOC_SOLUTION_MST_SPC = "OOC_SOLUTION_MST_SPC";

        public const string MODEL_GROUP_MST_SPC = "MODEL_GROUP_MST_SPC";
        public const string DATA_SAVE_INSERT = "INSERT";
        public const string DATA_SAVE_UPDATE = "UPDATE";
        public const string DATA_SAVE_DELETE = "DELETE";

        public const string MODEL_GROUP_ATT_MST_SPC = "MODEL_GROUP_ATT_MST_SPC";



    }

    public struct COLUMN
    {
        //ATT COLUMN//
        public const string PN_UPPERSPEC = "PN_USL";
        public const string PN_LOWERSPEC = "PN_LSL";
        public const string PN_TARGET = "PN_TARGET";
        public const string PN_UCL = "PN_UCL";
        public const string PN_LCL = "PN_LCL";
        public const string PN_CENTER_LINE = "PN_CENTER_LINE";
        public const string P_UCL = "P_UCL";
        public const string P_LCL = "P_LCL";
        public const string P_CENTER_LINE = "P_CENTER_LINE";

        public const string C_UPPERSPEC = "C_USL";
        public const string C_LOWERSPEC = "C_LSL";
        public const string C_TARGET = "C_TARGET";
        public const string C_UCL = "C_UCL";
        public const string C_LCL = "C_LCL";
        public const string C_CENTER_LINE = "C_CENTER_LINE";

        public const string U_UCL = "U_UCL";
        public const string U_LCL = "U_LCL";
        public const string U_CENTER_LINE = "U_CENTER_LINE";

        public const string SPEC_PN_USL_OFFSET = "PN_USL_OFFSET";
        public const string SPEC_PN_LSL_OFFSET = "PN_LSL_OFFSET";

        public const string PN_UCL_OFFSET = "PN_UCL_OFFSET";
        public const string PN_LCL_OFFSET = "PN_LCL_OFFSET";

        public const string PN_USL_OFFSET = "PN_USL_OFFSET";
        public const string PN_LSL_OFFSET = "PN_LSL_OFFSET";

        public const string C_USL_OFFSET = "C_USL_OFFSET";
        public const string C_LSL_OFFSET = "C_LSL_OFFSET";


        public const string P_UCL_OFFSET = "P_UCL_OFFSET";
        public const string P_LCL_OFFSET = "P_LCL_OFFSET";

        public const string SPEC_C_USL_OFFSET = "C_USL_OFFSET";
        public const string SPEC_C_LSL_OFFSET = "C_LSL_OFFSET";


        public const string C_UCL_OFFSET = "C_UCL_OFFSET";
        public const string C_LCL_OFFSET = "C_LCL_OFFSET";


        public const string U_UCL_OFFSET = "U_UCL_OFFSET";
        public const string U_LCL_OFFSET = "U_LCL_OFFSET";


        public const string P_CL_YN = "P_CL_YN";
        public const string PN_CL_YN = "PN_CL_YN";
        public const string C_CL_YN = "C_CL_YN";
        public const string U_CL_YN = "U_CL_YN";

        public const string PN_CALC_COUNT = "PN_CALC_CNT";
        public const string P_CALC_COUNT = "P_CALC_CNT";
        public const string C_CALC_COUNT = "C_CALC_CNT";
        public const string U_CALC_COUNT = "U_CALC_CNT";





        //ATT end/////

        //SPC-704 by Louis
        public const string IQR_UCL = "IQR_UCL";
        public const string IQR_LCL = "IQR_LCL";
        public const string IQR_TARGET = "IQR_TARGET";
        public const string THRESHOLD_UCL = "THRESHOLD_UCL";
        public const string THRESHOLD_LCL = "THRESHOLD_LCL";
        public const string THRESHOLD_TARGET = "THRESHOLD_TARGET";
        public const string SAVE_YN = "SAVE_YN";
        public const string OUT_OF_SPEC = "OUT_OF_SPEC";
        //

        //SPC-904 by Louis
        public const string RAW_CALC_COUNT = "RAW_CALC_CNT";
        public const string MEAN_CALC_COUNT = "MEAN_CALC_CNT";
        public const string STD_CALC_COUNT = "STD_CALC_CNT";
        public const string RANGE_CALC_COUNT = "RANGE_CALC_CNT";
        public const string EWMA_MEAN_CALC_COUNT = "EWMA_M_CALC_CNT";
        public const string EWMA_RANGE_CALC_COUNT = "EWMA_R_CALC_CNT";
        public const string EWMA_STD_CALC_COUNT = "EWMA_S_CALC_CNT";
        public const string MA_CALC_COUNT = "MA_CALC_CNT";
        public const string MS_CALC_COUNT = "MS_CALC_CNT";
        public const string MR_CALC_COUNT = "MR_CALC_CNT";

        #region ::[A]
        public const string AREA_RAWID = "AREA_RAWID";
        public const string AREA = "AREA";
        public const string AUTO_TYPE_CD = "AUTO_TYPE_CD";
        public const string AUTO_SUB_YN = "AUTO_SUB_YN";
        public const string AUTO_CPK_YN = "AUTO_CPK_YN";
        public const string AUTOCALC_YN = "AUTOCALC_YN"; //P2
        public const string AUTOCALC_PERIOD = "AUTOCALC_PERIOD"; //P2
        public const string ACTIVATION_YN = "ACTIVATION_YN";
        
        #endregion

        #region ::[B]
        #endregion

        #region ::[C]
        public const string CODE = "CODE";
        public const string CREATE_BY = "CREATE_BY";
        public const string CREATE_DTTS = "CREATE_DTTS";
        public const string CONTEXT_KEY = "CONTEXT_KEY";
        public const string CONTEXT_VALUE = "CONTEXT_VALUE";
        public const string COMPLEX_YN = "COMPLEX_YN";
        public const string CENTER_LINE = "CENTER_LINE";
        public const string CONTEXT_LIST = "CONTEXT_LIST";
        public const string CONTROL_LIMIT = "CONTROL_LIMIT";//P2
        public const string CONTROL_THRESHOLD = "CONTROL_THRESHOLD";//P2
        public const string CALC_COUNT = "CALC_COUNT";

        public const string CHART_CODE = "CHART_CODE"; //추가
        public const string CHART_NAME = "CHART_NAME"; //추가
        public const string CHART_ALIAS = "CHART_ALIAS"; //추가
        public const string CHART_LIST = "CHART_LIST"; //추가
        public const string CHART_ID = "CHART_ID";
        public const string CHART_MODE_CD = "CHART_MODE_CD";

        public const string CHART_DESCRIPTON = "CHART_DESCRIPTION";//SPC-676 by Louis
        public const string CONTROL_LIMIT_OPTION = "CONTROL_LIMIT_OPTION";//SPC-1129 BY STELLA
        public const string CHANGED_ITEMS = "CHANGED_ITEMS";//SPC-779 BY STELLA

        #endregion

        #region ::[D]
        public const string DEFAULT_CHART_LIST = "DEFAULT_CHART_LIST";
        public const string DESCRIPTION = "DESCRIPTION";
        public const string DEF_CONFIG_RAWID = "DEF_CONFIG_RAWID";
        public const string DEF_RULE_RAWID = "DEF_RULE_RAWID";
        public const string DATA_LIST = "DATA_LIST";
        public const string DEFAULT_PERIOD = "DEFAULT_PERIOD";//P2
        #endregion

        #region ::[E]
        public const string EXCLUDE_LIST = "EXCLUDE_LIST";
        public const string EWMA_LAMBDA = "EWMA_LAMBDA";
        public const string EQP_MODEL = "EQP_MODEL"; //P2
        public const string EWMA_M_UCL = "EWMA_M_UCL";
        public const string EWMA_M_LCL = "EWMA_M_LCL";
        public const string EWMA_M_CENTER_LINE = "EWMA_M_CENTER_LINE";
        public const string EWMA_R_UCL = "EWMA_R_UCL";
        public const string EWMA_R_LCL = "EWMA_R_LCL";
        public const string EWMA_R_CENTER_LINE = "EWMA_R_CENTER_LINE";
        public const string EWMA_S_UCL = "EWMA_S_UCL";
        public const string EWMA_S_LCL = "EWMA_S_LCL";
        public const string EWMA_S_CENTER_LINE = "EWMA_S_CENTER_LINE";
        public const string EWMA_M_CL_YN = "EWMA_M_CL_YN";
        public const string EWMA_R_CL_YN = "EWMA_R_CL_YN";
        public const string EWMA_S_CL_YN = "EWMA_S_CL_YN";
        public const string EWMA_MEAN_UCL = "EWMA_MEAN_UCL";
        public const string EWMA_MEAN_LCL = "EWMA_MEAN_LCL";
        public const string EWMA_MEAN_CENTER_LINE = "EWMA_MEAN_CENTER_LINE";
        public const string EWMA_RANGE_UCL = "EWMA_RANGE_UCL";
        public const string EWMA_RANGE_LCL = "EWMA_RANGE_LCL";
        public const string EWMA_RANGE_CENTER_LINE = "EWMA_RANGE_CENTER_LINE";
        public const string EWMA_STD_UCL = "EWMA_STD_UCL";
        public const string EWMA_STD_LCL = "EWMA_STD_LCL";
        public const string EWMA_STD_CENTER_LINE = "EWMA_STD_CENTER_LINE";

        //chris 2010-06-18
        public const string EWMA_M_UCL_OFFSET = "EWMA_M_UCL_OFFSET";
        public const string EWMA_M_LCL_OFFSET = "EWMA_M_LCL_OFFSET";
        //public const string EWMA_M_OFFSET_YN = "EWMA_M_OFFSET_YN";
        //Chris - End


        //chris 2010-06-18
        public const string EWMA_R_UCL_OFFSET = "EWMA_R_UCL_OFFSET";
        public const string EWMA_R_LCL_OFFSET = "EWMA_R_LCL_OFFSET";
        //public const string EWMA_R_OFFSET_YN = "EWMA_R_OFFSET_YN";
        //Chris - End


        //chris 2010-06-18
        public const string EWMA_S_UCL_OFFSET = "EWMA_S_UCL_OFFSET";
        public const string EWMA_S_LCL_OFFSET = "EWMA_S_LCL_OFFSET";
        //public const string EWMA_S_OFFSET_YN = "EWMA_S_OFFSET_YN";
        //Chris - End

        //stella 2013-11-29
        public const string EWMA_M_CALC_VALUE = "EWMA_M_CALC_VALUE";
        public const string EWMA_R_CALC_VALUE = "EWMA_R_CALC_VALUE";
        public const string EWMA_S_CALC_VALUE = "EWMA_S_CALC_VALUE";
        public const string EWMA_M_CALC_OPTION = "EWMA_M_CALC_OPTION";
        public const string EWMA_R_CALC_OPTION = "EWMA_R_CALC_OPTION";
        public const string EWMA_S_CALC_OPTION = "EWMA_S_CALC_OPTION";
        public const string EWMA_M_CALC_SIDED = "EWMA_M_CALC_SIDED";
        public const string EWMA_R_CALC_SIDED = "EWMA_R_CALC_SIDED";
        public const string EWMA_S_CALC_SIDED = "EWMA_S_CALC_SIDED";
        //Stella - End

        public const string EQPMODEL = "EQPMODEL";
        #endregion

        #region ::[F]
        public const string FILE_DATA = "FILE_DATA";
        //public const string FILTER_KEY = "FILTER_KEY";
        //public const string FILTER_VALUE = "FILTER_VALUE";
        public const string FROM_NOW = "FROM_NOW"; //추가
        public const string FALSE_ALARM_YN = "FALSE_ALARM_YN";
        #endregion

        #region ::[G]
        public const string GROUP_YN = "GROUP_YN";
        public const string GROUP_NAME = "GROUP_NAME";
        public const string GROUP_RAWID = "GROUP_RAWID";

        #endregion

        #region ::[H]
        #endregion

        #region ::[I]
        public const string INTERLOCK_YN = "INTERLOCK_YN";
        public const string INHERIT_MAIN_YN = "INHERIT_MAIN_YN";
        public const string INPUT_DTTS = "INPUT_DTTS";

        //SPC-658
        public const string INITIAL_CALC_COUNT = "INITIAL_CALC_COUNT";

        #endregion

        #region ::[J]
        #endregion

        #region ::[K]
        public const string KEY_ORDER = "KEY_ORDER";
        #endregion

        #region ::[L]
        public const string LAST_UPDATE_BY = "LAST_UPDATE_BY";
        public const string LAST_UPDATE_DTTS = "LAST_UPDATE_DTTS";
        public const string LOCATION_RAWID = "LOCATION_RAWID";
        public const string LOWER_CONTROL = "LOWER_CONTROL";
        public const string LOWER_SPEC = "LOWER_SPEC";
        public const string LOT_ID = "LOT_ID"; //추가
        public const string LOT_GROUP_NAME = "LOT_GROUP_NAME"; //추가

        //chris 2010-06-22        
        public const string LOWER_FILTER = "LOWER_FILTER";
        //Chris - End
        public const string LOWER_TECHNICAL_LIMIT = "LOWER_TECHNICAL_LIMIT";

        public const string LINE_RAWID = "LINE_RAWID";
        

        #endregion

        #region ::[M]
        public const string MODEL_RAWID = "MODEL_RAWID";
        public const string MAIN_YN = "MAIN_YN";
        public const string MOVING_COUNT = "MOVING_COUNT";
        public const string MODEL_CONFIG_RAWID = "MODEL_CONFIG_RAWID";
        public const string MODEL_RULE_RAWID = "MODEL_RULE_RAWID";
        public const string MIN_SAMPLES = "MIN_SAMPLES"; //P2
        public const string MAX_PERIOD = "MAX_PERIOD"; //P2
        public const string MODIFIED_TYPE_CD = "MODIFIED_TYPE_CD"; //P2
        public const string MANAGE_TYPE_CD = "MANAGE_TYPE_CD";
        public const string MANAGE_TYPE_NAME = "MANAGE_TYPE_NAME";
        public const string MA_UCL = "MA_UCL";
        public const string MA_LCL = "MA_LCL";
        public const string MA_CENTER_LINE = "MA_CENTER_LINE";
        public const string MS_UCL = "MS_UCL";
        public const string MS_LCL = "MS_LCL";
        public const string MS_CENTER_LINE = "MS_CENTER_LINE";
        public const string MR_UCL = "MR_UCL";
        public const string MR_LCL = "MR_LCL";
        public const string MR_CENTER_LINE = "MR_CENTER_LINE";
        public const string MEAN_CL_YN = "MEAN_CL_YN";
        public const string MA_CL_YN = "MA_CL_YN";
        public const string MS_CL_YN = "MS_CL_YN";
        public const string MR_CL_YN = "MR_CL_YN";

        //Chris 2010-06-18
        public const string MEAN_UCL_OFFSET = "MEAN_UCL_OFFSET";
        public const string MEAN_LCL_OFFSET = "MEAN_LCL_OFFSET";
        //public const string MEAN_OFFSET_YN = "MEAN_OFFSET_YN";
        //Chris - End

        //chris 2010-06-18
        public const string MA_UCL_OFFSET = "MA_UCL_OFFSET";
        public const string MA_LCL_OFFSET = "MA_LCL_OFFSET";
        //public const string MA_OFFSET_YN = "MA_OFFSET_YN";
        //Chris - End

        //chris 2010-06-18
        public const string MS_UCL_OFFSET = "MS_UCL_OFFSET";
        public const string MS_LCL_OFFSET = "MS_LCL_OFFSET";
        //public const string MS_OFFSET_YN = "MS_OFFSET_YN";
        //Chris - End

        public const string MR_UCL_OFFSET = "MR_UCL_OFFSET";
        public const string MR_LCL_OFFSET = "MR_LCL_OFFSET";

        //SPC-1155 BY STELLA
        public const string MA_CALC_VALUE = "MA_CALC_VALUE";
        public const string MR_CALC_VALUE = "MR_CALC_VALUE";
        public const string MS_CALC_VALUE = "MS_CALC_VALUE";
        public const string MEAN_CALC_VALUE = "MEAN_CALC_VALUE";
        public const string MA_CALC_OPTION = "MA_CALC_OPTION";
        public const string MR_CALC_OPTION = "MR_CALC_OPTION";
        public const string MS_CALC_OPTION = "MS_CALC_OPTION";
        public const string MEAN_CALC_OPTION = "MEAN_CALC_OPTION";
        public const string MA_CALC_SIDED = "MA_CALC_SIDED";
        public const string MR_CALC_SIDED = "MR_CALC_SIDED";
        public const string MS_CALC_SIDED = "MS_CALC_SIDED";
        public const string MEAN_CALC_SIDED = "MEAN_CALC_SIDED";

        #endregion

        #region ::[N]
        public const string NAME = "NAME";
        #endregion

        #region ::[O]
        public const string OCAP_LIST = "OCAP_LIST";
        public const string OPTION_NAME = "OPTION_NAME";

        //chris 2010-06-18
        public const string OFFSET_YN = "OFFSET_YN";
        //chris end
        #endregion

        #region ::[P]
        public const string PARAM_ALIAS = "PARAM_ALIAS";
        public const string PARAM_TYPE_CD = "PARAM_TYPE_CD";
        public const string PARAM_LIST = "PARAM_LIST";
        public const string PARAM_TYPE = "PARAM_TYPE";
        public const string PUBLIC_YN = "PUBLIC_YN"; //추가

        #endregion

        #region ::[Q]
        #endregion

        #region ::[R]
        public const string RAWID = "RAWID";
        public const string REF_PARAM = "REF_PARAM";
        public const string REF_PARAM_LIST = "REF_PARAM_LIST";
        public const string RESTRICT_SAMPLE_COUNT = "RESTRICT_SAMPLE_COUNT";
        public const string RESTRICT_SAMPLE_DAYS = "RESTRICT_SAMPLE_DAYS";
        public const string RULE_RAWID = "RULE_RAWID";
        public const string RULE_OPTION_NO = "RULE_OPTION_NO";
        public const string RULE_OPTION_VALUE = "RULE_OPTION_VALUE";
        public const string RULE_OPTION = "RULE_OPTION";
        public const string RULE_OPTION_DATA = "RULE_OPTION_DATA";
        public const string RAW_UCL = "RAW_UCL";
        public const string RAW_LCL = "RAW_LCL";
        public const string RAW_CENTER_LINE = "RAW_CENTER_LINE";
        public const string RANGE_UCL = "RANGE_UCL";
        public const string RANGE_LCL = "RANGE_LCL";
        public const string RANGE_CENTER_LINE = "RANGE_CENTER_LINE";
        public const string RAW_CL_YN = "RAW_CL_YN";
        public const string RANGE_CL_YN = "RANGE_CL_YN";
        public const string RAW_DTTS = "RAW_DTTS";
        public const string RULE_ORDER = "RULE_ORDER";

        //SPC-712 by Louis
        public const string RESTRICT_SAMPLE_HOURS = "RESTRICT_SAMPLE_HOURS";


        //chris 2010-06-18
        public const string RAW_UCL_OFFSET = "RAW_UCL_OFFSET";
        public const string RAW_LCL_OFFSET = "RAW_LCL_OFFSET";
        //public const string RAW_OFFSET_YN = "RAW_OFFSET_YN";
        //Chris - End

        //chris 2010-06-18
        public const string RANGE_UCL_OFFSET = "RANGE_UCL_OFFSET";
        public const string RANGE_LCL_OFFSET = "RANGE_LCL_OFFSET";
        //public const string RANGE_OFFSET_YN = "RANGE_OFFSET_YN";
        //Chris - End        

        //SPC-1155 BY STELLA
        public const string RAW_CALC_VALUE = "RAW_CALC_VALUE";
        public const string RANGE_CALC_VALUE = "RANGE_CALC_VALUE";
        public const string RAW_CALC_OPTION = "RAW_CALC_OPTION";
        public const string RANGE_CALC_OPTION = "RANGE_CALC_OPTION";
        public const string RAW_CALC_SIDED = "RAW_CALC_SIDED";
        public const string RANGE_CALC_SIDED = "RANGE_CALC_SIDED";

        #endregion

        #region ::[S]
        public const string SELECT = "SELECT";
        public const string SAMPLE_COUNT = "SAMPLE_COUNT";
        public const string SPC_DATA_LEVEL = "SPC_DATA_LEVEL";
        public const string SPC_MODEL_NAME = "SPC_MODEL_NAME";
        public const string SPC_PARAM_CATEGORY_CD = "SPC_PARAM_CATEGORY_CD";
        public const string SPC_PRIORITY_CD = "SPC_PRIORITY_CD";
        public const string SPC_RULE = "SPC_RULE";
        public const string SPC_RULE_NO = "SPC_RULE_NO";
        public const string STD = "STD";
        public const string STD_UCL = "STD_UCL";
        public const string STD_LCL = "STD_LCL";
        public const string STD_CENTER_LINE = "STD_CENTER_LINE";
        public const string STD_YN = "STD_YN";
        public const string STD_CL_YN = "STD_CL_YN";
        public const string SUB_INTERLOCK_YN = "SUB_INTERLOCK_YN";
        public const string SHIFT_CALC_YN = "SHIFT_CALC_YN";
        public const string SEARCH_YN = "SEARCH_YN"; //추가
        public const string SUB_AUTOCALC_YN = "SUB_AUTOCALC_YN";

        //chris 2010-06-18
        public const string SPEC_USL_OFFSET = "SPEC_USL_OFFSET";
        public const string SPEC_LSL_OFFSET = "SPEC_LSL_OFFSET";
        //public const string SPEC_OFFSET_YN = "SPEC_OFFSET_YN";
        //Chris - End

        //chris 2010-06-18
        public const string STD_UCL_OFFSET = "STD_UCL_OFFSET";
        public const string STD_LCL_OFFSET = "STD_LCL_OFFSET";
        //public const string STD_OFFSET_YN = "STD_OFFSET_YN";
        //Chris - End

        //SPC-1155 BY STELLA KIM
        public const string STD_CALC_VALUE = "STD_CALC_VALUE";
        public const string STD_CALC_OPTION = "STD_CALC_OPTION";
        public const string STD_CALC_SIDED = "STD_CALC_SIDED";

        public const string SAVE_COMMENT = "SAVE_COMMENT"; //SPC-977 BY STELLA
        #endregion

        #region ::[T]
        public const string TARGET = "TARGET";
        //SPC-731 건으로 Threshold Column추가로 인한 수정 - 2012.02.06일
        public const string THRESHOLD_OFF_YN = "THRESHOLD_OFF_YN";
        #endregion

        #region ::[U]
        public const string UPPER_SPEC = "UPPER_SPEC";
        public const string UPPER_CONTROL = "UPPER_CONTROL";
        public const string USE_MAIN_SPEC = "USE_MAIN_SPEC";
        public const string USE_MAIN_SPEC_YN = "USE_MAIN_SPEC_YN";
        public const string USE_EXTERNAL_SPEC_YN = "USE_EXTERNAL_SPEC_YN";
        public const string USE_YN = "USE_YN"; //추가
        public const string USER_ID = "USER_ID"; //추가
        public const string USE_NORM_YN = "USE_NORM_YN";

        //chris 2010-06-22
        public const string UPPER_FILTER = "UPPER_FILTER";        
        //Chris - End

        public const string UPPER_TECHNICAL_LIMIT = "UPPER_TECHNICAL_LIMIT";
        public const string USE_GLOBAL_YN = "USE_GLOBAL_YN"; //SPC-1129 BY STELLA

        #endregion

        #region ::[V]
        public const string VERSION = "VERSION";
        public const string VALIDATION_SAME_MODULE_YN = "VALIDATION_SAME_MODULE_YN";
        #endregion

        #region ::[W]
        public const string WITHOUT_IQR_YN = "WITHOUT_IQR_YN";
        #endregion

        #region ::[X]
        #endregion

        #region ::[Y]
        #endregion

        #region ::[Z]
        public const string ZONE_A_UCL = "ZONE_A_UCL";
        public const string ZONE_B_UCL = "ZONE_B_UCL";
        public const string ZONE_C_UCL = "ZONE_C_UCL";
        public const string ZONE_A_LCL = "ZONE_A_LCL";
        public const string ZONE_B_LCL = "ZONE_B_LCL";
        public const string ZONE_C_LCL = "ZONE_C_LCL";
        public const string ZONE_YN = "ZONE_YN";

        //chris 2010-06-18
        public const string ZONE_A_UCL_OFFSET = "ZONE_A_UCL_OFFSET";
        public const string ZONE_A_LCL_OFFSET = "ZONE_A_LCL_OFFSET";
        //public const string ZONE_A_OFFSET_YN = "ZONE_A_OFFSET_YN";
        //Chris - End

        //chris 2010-06-18
        public const string ZONE_B_UCL_OFFSET = "ZONE_B_UCL_OFFSET";
        public const string ZONE_B_LCL_OFFSET = "ZONE_B_LCL_OFFSET";
        //public const string ZONE_B_OFFSET_YN = "ZONE_B_OFFSET_YN";
        //Chris - End

        //chris 2010-06-18
        public const string ZONE_C_UCL_OFFSET = "ZONE_C_UCL_OFFSET";
        public const string ZONE_C_LCL_OFFSET = "ZONE_C_LCL_OFFSET";
        //public const string ZONE_C_OFFSET_YN = "ZONE_C_OFFSET_YN";
        //Chris - End

        #endregion
    }


}
