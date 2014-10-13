using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Common
{
    public class SPCStruct
    {      
        public class ChartConditionContextList : System.Collections.Generic.List<ChartContextInfo> { }
        public class ChartContextInfo
        {            
            string mModel_config_rawid;
            string mParamAlias;
            string mLine;
            string mArea;
            string mMainYN;
            string mSPCModel;
            string mDefaultChartList;
            int iRestrictSampleDays;
            string mSPCModelName;
            string mAreaRawid= string.Empty;
            string mEQPModel = string.Empty;
                        
            LinkedList mllstContext;
            LinkedList mllstCustomContext;                        

            DataTable mDTModelContext = new DataTable();
            DataTable mDTResult = new DataTable();                                 
            SPCMODEL_TYPE mSPCModelType = SPCMODEL_TYPE.CONDITION;
                        
            public ChartContextInfo()
            {              
            }

            public ChartContextInfo(string argModel_config_rawid, string argMainYN, 
            string argParamAlias,
            string argLine,
            string argArea,
            string argSPCModelName, 
            LinkedList argllstContext, 
            LinkedList argllstCustomContext)
            {
                this.mModel_config_rawid = argModel_config_rawid;                
                this.mParamAlias = argParamAlias;                
                this.mLine = argLine;
                this.mArea = argArea;
                this.mMainYN = argMainYN;                
                this.mSPCModelName = argSPCModelName;                                
                this.mllstContext = argllstContext;
                this.mllstCustomContext = argllstCustomContext;
            }

            public string SPC_MODEL
            {
                set { this.mSPCModel = value; }
                get { return this.mSPCModel; }
            }


            public string EQP_MODEL
            {
                set { this.mEQPModel = value; }
                get { return this.mEQPModel; }
            }


            public string SPC_MODEL_NAME
            {
                set { this.mSPCModelName = value; }
                get { return this.mSPCModelName; }
            }

            public string PARAM_ALIAS
            {
                set { this.mParamAlias = value; }
                get { return this.mParamAlias; }
            }

            public string LINE
            {
                set { this.mLine = value; }
                get { return this.mLine; }
            }

            public string AREA
            {
                set { this.mArea = value; }
                get { return this.mArea; }
            }


            public string AREA_RAWID
            {
                set { this.mAreaRawid = value; }
                get { return this.mAreaRawid; }
            }


            public string MAIN_YN
            {
                set { this.mMainYN = value; }
                get { return this.mMainYN; }
            }


            public int RESTRICT_SAMPLE_DAYS
            {
                set { this.iRestrictSampleDays = value; }
                get { return this.iRestrictSampleDays; }
            }

            public string DEFAULT_CHART_LIST
            {
                set { this.mDefaultChartList = value; }
                get { return this.mDefaultChartList; }
            }

            public string MODEL_CONFIG_RAWID
            {
                set { this.mModel_config_rawid = value; }
                get { return this.mModel_config_rawid; }
            }
            

            public SPCMODEL_TYPE SPCModelType
            {
                set { this.mSPCModelType = value; }
                get { return this.mSPCModelType; }
            }

            public LinkedList llstContext
            {
                set { this.mllstContext = value; }
                get { return this.mllstContext; }
            }

            public LinkedList llstCustomContext
            {
                set { this.mllstCustomContext = value; }
                get { return this.mllstCustomContext; }
            }

            public DataTable DTResult
            {
                set { this.mDTResult = value; }
                get { return this.mDTResult; }
            }

            public DataTable DTModelContext
            {
                set { this.mDTModelContext = value; }
                get { return this.mDTModelContext; }
            }            

        }    
        
        public class ChartInfoList : System.Collections.Generic.List<ChartInfo> { }
        public class ChartInfo
        {
            string _code;
            string _name;
            string _value;
            string _desc;
            ChartInfoDataType _dataType;

            public ChartInfo(string code, string name, string desc, ChartInfoDataType dataType)
            {
                this._code = code;
                this._name = name;
                this._desc = desc;
                this._dataType = dataType;
            }

            public string CODE
            {
                get { return this._code; }
                set { this._code = value; }
            }

            public string NAME
            {
                get { return this._name; }
            }

            public string DESC
            {
                get { return this._desc; }
            }

            public string VALUE
            {
                get { return this._value; }
                set { this._value = value; }
            }

            public ChartInfoDataType DATA_TYPE
            {
                get { return this._dataType; }
            }
        }

        public class ContextTypeList : System.Collections.Generic.List<ContextTypeInfo> { }
        public class ContextTypeInfo
        {
            string mCode;
            string mName;
            public ContextTypeInfo(string argCode, string argName)
            {
                this.mCode = argCode;
                this.mName = argName;               
            }

            public string CODE
            {
                set { this.mCode = value; }
                get { return this.mCode; }
            }

            public string NAME
            {
                set { this.mName = value; }
                get { return this.mName; }
            }

            public string GetCodeName(string argValue)
            {
                if (argValue.Equals(this.CODE))
                    return this.NAME;
                else
                    return argValue;
            }           
        }

        public class FilterTypeList : System.Collections.Generic.List<FilterTypeInfo> { }
        public class FilterTypeInfo
        {
            string mCode;
            string mName;
            public FilterTypeInfo(string argCode, string argName)
            {
                this.mCode = argCode;
                this.mName = argName;
            }

            public string CODE
            {
                set { this.mCode = value; }
                get { return this.mCode; }
            }

            public string NAME
            {
                set { this.mName = value; }
                get { return this.mName; }
            }

            public string GetCodeName(string argValue)
            {
                if (argValue.Equals(this.CODE))
                    return this.NAME;
                else
                    return argValue;
            }
        }    
        
       
  } 
}
