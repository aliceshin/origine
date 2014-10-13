using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace BISTel.eSPC.Common
{
    public class EnumTypes
    {
    }


    #region : Common

    /// <summary>
    /// Sort Type
    /// </summary>
    public enum SortType
    {
        asc = 0,
        desc
    }

    #endregion


    #region : Interface
    public enum MessageInterface
    {
        TIB = 0,
        BMS = 1
    }
    #endregion

    public enum DefaultSettingOptionCode
    {
        UHT,
        LHT,
        UST,
        UCT,
        LCT,
        LST,
        ACYN,
        SYN,
        DYN,
        ST,
        AT,
        FLCD,
        FRX,
        FRY,
        ACPN,
        ACPU,
        ACLT,
        ACMLC,
        SUOYN,
        SREYN,
        SLSC,
        DCSN,
        DCSU
    }

    #region : CONDITION TYPE
    //public enum LotListColumn
    //{
    //    SELECT = 0,
    //    RAWID,
    //    EQP_RAWID,
    //    EQP_ID,
    //    MODULE_NAME,
    //    LOT_ID,
    //    PPID,
    //    PRODUCT_ID,
    //    RECIPE_ID,
    //    START_DTTS,
    //    END_DTTS,
    //    STATUS,
    //    TRACE_FAULT_COUNT,
    //    SAMPLE_COUNT,
    //    MSPC_FAULT_COUNT,
    //    TRACE_WARNING_COUNT,
    //    FILE_DATA,
    //    REAL_START_DTTS,
    //    REAL_END_DTTS
    //}

    //public enum ParamColumn
    //{
    //    SELECT = 0,
    //    RAWID = 1,
    //    PARAM_NAME = 2,
    //    SPEC_EXIST = 3,
    //    RIGHT = 4
    //}
    //public enum MultivariateMoelColumn
    //{
    //    SELECT = 0,
    //    RAWID = 1,
    //    MVA_NAME = 2
    //}
    //public enum TraceParamColumn
    //{
    //    SELECT = 0,
    //    RAWID = 1,
    //    PARAM_KEY = 2,
    //    PARAM_NAME = 3,
    //    LOT_ID = 4,
    //    RECIPE_ID = 5,
    //    SPEC_EXIST = 6,
    //    RIGHT = 7
    //}
    //public enum StepColumn
    //{
    //    SELECT = 0,
    //    RAWID = 1,
    //    STEP_NAME = 2,
    //    SPEC_EXIST = 3,
    //    STEP_ID = 4
    //}
    //public enum ResultLotConditionColumn
    //{
    //    SELECT = 0,
    //    RAWID = 1,
    //    EQP_RAWID = 2,
    //    LOT_RAWID = 3,
    //    LOT_ID = 4,
    //    PARAM_RAWID = 5,
    //    PARAMETER = 6,
    //    PPID = 7,
    //    MODULE_ID = 8,
    //    RECIPE_ID = 9,
    //    PRODUCT_ID = 10,
    //    START_DTTS = 11,
    //    END_DTTS = 12,
    //    EQP_ID = 13,
    //    REAL_START_DTTS,
    //    REAL_END_DTTS
    //}

    //public enum ResultStepConditionColumn
    //{
    //    SELECT = 0,
    //    RAWID = 1,
    //    EQP_RAWID = 2,
    //    RECIPE_ID = 3,
    //    PARAM_RAWID = 4,
    //    PARAMETER = 5,
    //    STEP_ID = 6,
    //    PARAM_TYPE = 7,
    //    EQP_ID = 8,
    //    MODULE_ID = 9
    //}

    //public enum ResultParamConditionColumn
    //{
    //    SELECT = 0,
    //    RAWID = 1,
    //    EQP_RAWID = 2,
    //    MODULE_ID = 3,
    //    PARAM_RAWID = 4,
    //    PARAMETER = 5,
    //    EQP_ID = 6
    //}
    public enum ConditionType
    {
        LINE = 0,
        AREA = 1,
        EQPMODEL = 2,
        EQPID = 3,
        DCPID = 4,
        MODULEID = 5,
        PRODUCTID = 6,
        RECIPEID = 7,
        TYPE = 8,
        PARAM = 9,
        STEP = 10,
        DATE = 11,
        LOTCHECK = 12,
        LOTLIST = 13,
        NONE = 14,
        LEGEND = 15,
        BUTTON = 16,
        MULTITYPE = 17,
        MULTIVARIATE_MODEL = 18,
        SPECMODELID = 19
    };
    
    
    
    public enum ParameterType
    {
        TRACE = 0,
        EVENT = 1,
        TRACE_SUMMARY = 2,
        EVENT_SUMMARY = 3,
        LOT_PARAM = 4,
        NONE = 5
    }

    public enum ResultType
    {
        PARAM_MODULE = 1,
        PARAM_LOT = 2,
        PARAM_STEP = 3
    }

    public enum RecipeType
    {
        STEP = 0,
        SPEC_GROUP = 1,
        MVA_MODEL = 2,
        ALL = 3,
        RECIPE = 4,
        NONE = 5
    }

    #endregion

    #region : ButtonType
    public enum buttonType
    {
        SEARCH,
        COMPARE,
        EXCUTE,
        COPY,
        ADD,
        REMOVE,
        SAVE,
        START,
        STOP,
        VIEWSTART,
        VIEWSTOP,
        TIMECHANGE
    }
    #endregion

    public enum TreeNodeImage
    {
        Uncheck = 0,
        Check = 1,
        CloseFolder = 2,
        OpenFolder = 3
    }

    #region : FAB Overview

   
    #endregion


    #region : Favorite Condition
    public enum FavoriteConditionType
    {
        LINE = 0,
        AREA = 1,
        EQPMODEL = 2,
        EQPID = 3,
        MODULEID = 4,
        DEFAULTPERIOD = 5
    };

    #endregion


    public enum NumericValidation_DataType
    {
        Double,
        Integer
    }

    public enum NumericValidation_RangeOption
    {
        NotInUse,
        IncludeValue,
        ExcludeValue
    }

    public enum NumericValidation_Result
    {
        OK,
        Empty,
        NonNumeric,
        InvalidDataType,
        OutOfRange,
        Etc
    }


    #region Chart

    public enum PCA_TYPE
    {
        PCA_SCORE,
        EIGEN_VALUE,
        EIGEN_RATE,
        EIGEN_VECTOR
    }
  
 
    public enum enum_ChartInfomationData
    {

        LINE = 0    
        ,AREA
        ,SPC_MODEL_NAME
        ,PARAM_ALIAS        
        ,CHART_CONTEXT        
    }
    #endregion 
    
        
    public enum enum_OCAPLIST
    {
        SPC_V_SELECT = 0,
        TIME,
        MODEL_CONFIG_RAWID,
        EQP_ID,
        FALSE_ALARM,
        STATUS,
        SPC_MODEL_NAME,
        PARAM_ALIAS,
        OPERATION_ID,
        OPERATION_NUMBER,
        PRODUCT_ID,
        LOT_ID,
        SUBSTRATE_ID,
        RECIPE_ID,
        RULE_NO,
        OOC_VALUE,
        OOC_PROBLEM,
        OOC_CAUSE,
        OOC_SOLUTION,
        OOC_COMMENT,
        AREA,
        MAIN_MODULE_ID,
        MODULE_ID,
        MODULE_ALIAS,
        CASSETTE_SLOT,
        STEP_ID,
        DEFAULT_CHART_LIST,
        COMPLEX_YN,
        CONTEXT_LIST,
        MAIN_YN,
        OCAP_RAWID,
        PARAM_TYPE_CD,
        OCAP_RAWID_LIST,
        LINE,
        AREA_RAWID,
        ColumnCount = 35
    }


    public enum enum_OCAPList_Defails
    {
        V_SELECT = 0,
        V_MODIFY,
        OCAP_RAWID,
        STATUS,
        RULE_NO,
        RULE_DESC,
        OOC_VALUE,
        OOC_PROBLEM,
        OOC_CAUSE,
        OOC_SOLUTION,
        CONTEXT_LIST,
        OOC_COMMENT,
        FALSE_ALARM_YN,
        ColumnCount = 12
    }


    public enum enum_ProcessCapability
    {
        SPC_V_SELECT = 0,
        MODEL_CONFIG_RAWID,
        COMPLEX_YN,
        MAIN_YN,
        DEFAULT_CHART_LIST,
        AREA,
        PARAM_ALIAS,
        PARAM_TYPE_CD
    }
    
    public enum enum_SPCControlChart
    {
        V_SELECT = 0,    
        NO,    
        AREA,
        SPC_MODEL_NAME ,
        PARAM_ALIAS,
        OPERATION_ID,
        PRODUCT_ID,        
        LOT_ID ,        
        EQP_ID,                                
        DEFAULT_CHART_LIST,
        COMPLEX_YN ,  
        OOC_RULE,  
        ColumnCount = 12
    }


    public enum enum_PopupType
    {       
       Modify,
       View       
    }


    public enum ConfigMode
    {
        CREATE_MAIN,
        CREATE_SUB,
        MODIFY,
        DEFAULT,
        VIEW,
        SAVE_AS,
        ROLLBACK,
        CREATE_MAIN_FROM
    }


    public enum CHART_PARENT_MODE
    {
        MODELING,
        OCAP,
        PPK_REPORT,
        SPC_CONTROL_CHART,
        //Louis SPC-834 [SPC Compare] Chart 
        COMPARE 
        //
    }


    //2009-10-29
    public enum SPC_CHART_TYPE
    {
        BASECHART,
        ANALYSISCHART,
        LEDBASECHART,
        BASECALCCHART,//추가
        //SPC-805
        BASERAWCHART
    }


    public enum AnalysisWindowType
    {
        SPCChart,
        Analysis

    }

    public enum enumMultiDataConditionPopup
    {
        SELECT = 0,
        BASE,
        OPERATION_ID,
        OPERATION_DESCRIPTION,
        INFORMATION,
        ITEM,
        SUBDATA,
        PROBE,
        TYPE
    }

    public enum SelectedItems
    {
        SELECT = 0,
        OPERATION_DESC,
        PARAM_ITEM,
        PRODUCT_ID,
        MODULE_NAME,
        OPERATION_ID,
        PARAM_TYPE,
        EQP_ID
    }


    public enum ChartInfoDataType
    {
        TITLE = 0,
        FIX,
        CONTEXT,
        RELATED,
        DATA,
        OCAP
    }

    public enum SPCMODEL_TYPE
    {
        CONDITION = 0,
        SPC_CHART
    }
    
    public enum enumChartDataList
    {
        _USL = 0,
        _UCL,
        _TARGET,
        _LCL,
        _LSL,
        _RULE        
    }      
    
    #region Modeling - AutoCalculation Tab  

    public enum AutoCalcColumnIndex
    {
        NAME = 0,
        CL_YN,
        CALC_CNT,
        CALC_VALUE,
        CALC_OPTION,
        CALC_SIDED
    }

    public enum LimitOption
    {
        SIGMA = 0,
        PERCENT,
        CONSTANT
    }

    public enum AutoCalcSideOption
    {
        UPPER = 0,
        LOWER,
        BOTH
    }

    public enum ModifyMode
    {
        MAIN,
        SUB,
        COPY,
        REUSE
    }

    #endregion
}

