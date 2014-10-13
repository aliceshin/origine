using System;
using System.Collections.Generic;
using System.Text;

namespace BISTel.eSPC.Common
{
    public class Constants
    {
        public class ActivityNames
        {
            public const string START = "VMSStart";
            public const string END = "VMSEnd";
            public const string PLS = "PLS";
            public const string REGRESSION = "Regression";
            public const string NEURAL_NETWORK = "NeuralNetwork";
            public const string DISTRIBUTION = "Distribution";
            public const string CORRELATION = "Correlation";
            public const string IMPORTANCE = "Importance";
            public const string PCA = "PCA";
            public const string REPLACE = "Replace";
            public const string REMOVE_OUTLIERS = "RemoveOutliers";
            public const string TRANSFORM_VARIABLES = "TransformVariables";
            public const string COMPAREPREDICTOR = "ComparePredictor";
            public const string BASICSTATISTIC = "BasicStatistic";
            //public const string VARIABLE_NAME           = "VARIABLE_NAME";
            //public const string VARIABLE_ROLE_INPUT     = "I";
            //public const string VARIABLE_ROLE_OUTPUT    = "O";

            //DataSource
            public const string CONTROLLER = "Controller";
            public const string DEADBAND = "Deadband";
            public const string PREDICTOR = "Predictor";
            public const string MODEL = "Model";

            public const string VARIABLE = "Variable";


        }

        /// <summary>
        /// Reserved Variables for VMS Activity Properties
        /// </summary>
        public class RESERVE_VARIABLES
        {
            public const string CONFIDENCE_INTERVAL = "$Confidence_Interval";
            public const string CUMULATIVE_PERCENTAGE = "$Cumulative_Percentage";
            public const string INPUT = "$Input";
            public const string OUTPUT = "$Output";
            public const string SELECTED_VARIABLES = "$Selected_Variables";
            public const string SELECTED_STATISTICS = "$Selected_Statistics";
            public const string X_AXIS = "$X_Axis";
            public const string Y_AXIS = "$Y_Axis";
            public const string METHOD = "$Method";
            public const string UCL = "$UCL";
            public const string LCL = "$LCL";
            public const string SIGMA = "$Sigma";
            public const string LAMBDA = "$Lambda";
            public const string HIDDENLAYER = "$Hidden_Layer";
            public const string LENGTH = "$Length";
            public const string ALPHA = "$Alpha";
            public const string BETA = "$Beta";
            public const string GAMMA = "$Gamma";
            public const string DELTA = "$Delta";
            public const string SCALE = "$Scale";
            public const string SCALETOL = "$Scale_tol";
            public const string TOL = "$Tol";
            public const string INPUTRANGE = "$Input_Range";
            public const string OUTPUTRANGE = "$Output_Range";
            public const string RANDOM_SEEDNUMBER = "$Random_SeedNumber";
            public const string VALIDATION_STOPNUMBER = "$Validation_StopNumber";
            public const string TRAINING_COUNT = "$Training_Count";
            public const string VARIABLE_NAMES = "$Variable_Names";
            public const string ROLE = "$Role";
            public const string FORMULA = "$Formula";
            public const string DESCRIPTION = "$Description";
            public const string PARAMETER_VALUE = "$Parameter_Value";
            public const string PARAMETERS = "$Parameters";
            public const string APPLY = "$Apply";
            public const string ACTIVITY = "$Activity";
            public const string TRAINING = "$Training";
            public const string SIMULATION = "$Simulation";
            public const string PREDICTOR_NAME = "$PredictorName";
            public const string PREDICTOR_TYPE = "$PredictorType";
            public const string DO_SAVE = "$DoSave";
            public const string LOAD_ONLY = "$LoadOnly";
            public const string STATE_NAME = "$StateName";
        }

        /// <summary>
        /// Parameter List Type for ParameterAllSelectUC
        /// </summary>
        public class PARAMETERLIST_TYPE
        {
            public const string ALL = "ALL";
            public const string INPUT = "I";
            public const string OUTPUT = "O";
            public const string UNDEFINED = "U";
        }

        public class PREDICTOR_TYPE
        {
            public const string PL = "PL";
            public const string RG = "RG";
            public const string NN = "NN";

        }

        public class GLOBAL_VARIABLE_NAME
        {
            public const string VARIABLE_COLLECTION = "VariableCollection";
            public const string SOURCE_DATA = "SourceData";
            public const string MODEL_RAWID = "ModelRawID";
            public const string TRAINING_SOURCE_DATA = "TrainingSourceData";
            public const string TESTING_SOURCE_DATA = "TestingSourceData";
            public const string VALIDATION_SOURCE_DATA = "ValidationSourceData";
            public const string RESULT = "Result_";
            public const string VARIABLE_COLLECTION_ORIGINAL = "VariableCollectionOriginal";
            public const string SOURCE_DATA_ORIGINAL = "SourceDataOriginal";
            public const string RESULT_1 = "Result_1_";
            public const string RESULT_2 = "Result_2_";
            public const string COMMON = "COMMON";
            public const string FAIL = "Fail_";


        }

        // General Chart Variable
        public class ChartVariable
        {
            public const double cdChart_LowerSpec = 0;
            public const double cdChart_UpperSpec = 100;
        }

        public class GridVariable
        {
            public const float ciGrid_SheetZoomFactor = (float)0.85;
        }

        //Used in Data Source
        public class DataTypeCode
        {
            public static string MESSAGE = "MG";
            public static string SPEC_DATA = "SD";
            public static string VARIABLE = "VR";
            public static string VALUE = "VL";
            public static string CALCULATED_OUTPUT = "CO";
            public static string CURRENT_RUN = "CR";
        };

        public class DataTypeName
        {
            public static string MESSAGE = "MESSAGE";
            public static string SPEC_DATA = "SPEC_DATA";
            public static string VARIABLE = "VARIABLE";
            public static string VALUE = "VALUE";
            public static string CALCULATED_OUTPUT = "CALCULATED_OUTPUT";
            public static string CURRENT_RUN = "CURRENT_RUN";
        };

        public class ActivityScope
        {
            public const string GLOBAL = "GLOBAL";
            public const string LOCAL = "LOCAL";
        }

        public class StateTypeCode
        {
            public static string CONTROLLER = "CS";
            public static string DEADBAND = "DS";
            public static string PREDICTOR = "PS";
            public static string MODEL = "MS";
        };

        public class ParamTypeName
        {
            public static string LOT = "LOT";
            public static string INPUT = "INPUT";
            public static string OUTPUT = "OUTPUT";
            public static string INQUIRY = "INQUIRY";
            public static string PREDICT = "PREDICT";
            public static string FF_OUTPUT = "FF_OUTPUT";
            public static string FB_OUTPUT = "FB_OUTPUT";
        };

        public class ParamTypeCode
        {
            public static string LOT = "LT";
            public static string INPUT = "IP";
            public static string OUTPUT = "OP";
            public static string INQUIRY = "IQ";
            public static string PREDICT = "PD";
            public static string IN_VIRTUAL = "IV";
            public static string OUT_VIRTUAL = "OV";
            public static string FF_OUTPUT = "OPFF";
            public static string FB_OUTPUT = "OPFB";

            public static string FF_OUTPUT_VIRTUAL = "OVFF";
            public static string FB_OUTPUT_VIRTUAL = "OVFB";

            public static string INPUT_REF = "IR";
            public static string FF_OUTPUT_REF = "ORFF";
            public static string FB_OUTPUT_REF = "ORFB";

            public static string INPUT_SIBLING = "IS";
            public static string FF_OUTPUT_SIBLING = "OSFF";
            public static string FB_OUTPUT_SIBLING = "OSFB";

        };

        public class VMS_DATASOURCE_COLUMNSIZE
        {
            public const int DEFAULT = 100;
            public const int DATA_SOURCE_TYPE = 75;
            public const int PATH_0 = 70;
            public const int PATH_1 = 70;
            public const int PATH_2 = 80;
            public const int PATH_3 = 30;
            public const int PATH_4 = 30;
            public const int DATA_SOURCE_ATTRIBUTE = 80;

        }

        public class VMSCodeMst
        {

            public const string PARAMETER_TYPE_INPUT = "INPUT";
            public const string PARAMETER_TYPE_OUTPUT = "OUTPUT";
            public const string PARAMETER_TYPE_PREDICT = "PREDICT";
            public const string PARAMETER_TYPE_INQUIRY = "INQUIRY";

            public const string PREDICTOR_TYPE_REGRESSION = "RG";
            public const string PREDICTOR_TYPE_NEURALNETWROK = "NN";
            public const string PREDICTOR_TYPE_PLS = "PL";

            public const string CATEGORY_VALUE = "CATEGORY_VALUE";
            public const string MODEL_RAWID = "RAWID";

            public const string CURRENT_ITEM = "CURRENTITEM";

            public const string MAXTIME = "MAXTIME";
            public const string MINTIME = "MINTIME";

            public const string CURRENT_LOT = "CURRENTLOT";
        }

        public class PREDICTOR_METHOD
        {
            public const string REGRESSION_PREDICTOR = "LINEAR_REGRESSION";
            public const string NEURALNETWROK_PREDICTOR = "NEURAL_NETWORK";
            public const string PLS_PREDICTOR = "PLS";
        }


    }
}
