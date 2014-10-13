using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BISTel.PeakPerformance.Client.CommonLibrary;
using BISTel.eSPC.Common;
using BISTel.PeakPerformance.Client.BISTelControl;
using BISTel.PeakPerformance.Client.DataHandler;

using System.Windows.Forms;

namespace BISTel.eSPC.Condition.Controls.Tree
{
    public class TreeInterface
    {
        #region : Field

        public eSPCWebService.eSPCWebService _wsSPC = null;

        private string _sUseDefaultCondition = "Y";

        private BTreeView _btvTreeView = null;
        private string _sLineRawid = string.Empty;
        private string _sAreaRawid = string.Empty;
        private string _sEqpModelRawid = string.Empty;
        private string _sEqpModelName = string.Empty;
        private string _sEqpVerRawid = string.Empty;
        private string _sEqpID = string.Empty;
        private string _sDcpID = string.Empty;
        private string _sModuleID = string.Empty;
        private string _sProductID = string.Empty;
        private string _sOperationID = string.Empty;
        private string _sEqpRawid = string.Empty;
        private string _sDcpRawid = string.Empty;
        private string _sCategory = string.Empty;
        private string _sMethodName = string.Empty;
        private string _sSearchTypeCode = string.Empty;
        private string _sParamType = string.Empty;
        private string _sParamTypeCD = string.Empty;
        private string _sModelRawID = string.Empty;
        private string _sFilterValue = string.Empty;

        private RecipeType _recipeType = RecipeType.NONE;
        private ParameterType _paramType = ParameterType.NONE;

        private bool _bIsShowCheck = false;
        private bool _bIsShowCheckModule = false;
        private bool _bIsShowCheckProduct = false;
        private bool _bIsShowCheckEQP = false;
        private bool _bIsShowCheckRecipe = false;
        private bool _bIsShowMainModule = true;
        private bool _bIsLastNode = false;
        private bool _bIsLastNodeModule = false;
        private bool _bIsCheckParamType = true;
        private bool _bIsShowRecipeWildcard = false;
        private bool _bIsShowCheckEQPModel = false;





        public string UseDefaultCondition
        {
            get
            {
                return _sUseDefaultCondition;
            }
            set
            {
                _sUseDefaultCondition = value;
            }
        }

        public BTreeView XTreeView
        {
            get
            {
                return _btvTreeView;
            }
            set
            {
                _btvTreeView = value;
            }
        }

        public string LineRawid
        {
            get
            {
                return _sLineRawid;
            }
            set
            {
                _sLineRawid = value;
            }
        }

        public string AreaRawid
        {
            get
            {
                return _sAreaRawid;
            }
            set
            {
                _sAreaRawid = value;
            }
        }

        public string EqpModelName
        {
            get
            {
                return _sEqpModelName;
            }
            set
            {
                _sEqpModelName = value;
            }
        }

        public string EqpModelRawid
        {
            get
            {
                return _sEqpModelRawid;
            }
            set
            {
                _sEqpModelRawid = value;
            }
        }

        public string EqpVerRawid
        {
            get
            {
                return _sEqpVerRawid;
            }
            set
            {
                _sEqpVerRawid = value;
            }
        }

        public string EqpRawid
        {
            get
            {
                return _sEqpRawid;
            }
            set
            {
                _sEqpRawid = value;
            }
        }

        public string EqpID
        {
            get
            {
                return _sEqpID;
            }
            set
            {
                _sEqpID = value;
            }
        }

        public string DcpRawid
        {
            get
            {
                return _sDcpRawid;
            }
            set
            {
                _sDcpRawid = value;
            }
        }

        public string DcpID
        {
            get
            {
                return _sDcpID;
            }
            set
            {
                _sDcpID = value;
            }
        }

        public string ModuleID
        {
            get
            {
                return _sModuleID;
            }
            set
            {
                _sModuleID = value;
            }
        }

        public string ProductID
        {
            get
            {
                return _sProductID;
            }
            set
            {
                _sProductID = value;
            }
        }


        public string OperationID
        {
            get
            {
                return _sOperationID;
            }
            set
            {
                _sOperationID = value;
            }
        }


        public string ParamType
        {
            get
            {
                return _sParamType;
            }
            set
            {
                _sParamType = value;
            }
        }



        public string Category
        {
            get
            {
                return _sCategory;
            }
            set
            {
                _sCategory = value;
            }
        }

        public string MethodName
        {
            get
            {
                return _sMethodName;
            }
            set
            {
                _sMethodName = value;
            }
        }

        public RecipeType RecipeTypeCode
        {
            get
            {
                return _recipeType;
            }
            set
            {
                _recipeType = value;
            }
        }

        public bool IsShowRecipeWildcard
        {
            get
            {
                return _bIsShowRecipeWildcard;
            }
            set
            {
                _bIsShowRecipeWildcard = value;
            }
        }

        public ParameterType ParamTypeCode
        {
            get
            {
                return _paramType;
            }
            set
            {
                _paramType = value;
            }
        }


        public string ParamTypeCD
        {
            get
            {
                return _sParamTypeCD;
            }
            set
            {
                _sParamTypeCD = value;
            }
        }




        public string ModelRawID
        {
            get
            {
                return _sModelRawID;
            }
            set
            {
                _sModelRawID = value;
            }
        }




        public string SearchTypeCode
        {
            get
            {
                return _sSearchTypeCode;
            }
            set
            {
                _sSearchTypeCode = value;
            }
        }




        public bool IsShowCheck
        {
            get
            {
                return _bIsShowCheck;
            }
            set
            {
                _bIsShowCheck = value;
            }
        }

        public bool IsShowCheckModule
        {
            get
            {
                return _bIsShowCheckModule;
            }
            set
            {
                _bIsShowCheckModule = value;
            }
        }

        public bool IsShowCheckEQP
        {
            get
            {
                return _bIsShowCheckEQP;
            }
            set
            {
                _bIsShowCheckEQP = value;
            }
        }


        public bool IsShowCheckEQPModel
        {
            get
            {
                return _bIsShowCheckEQPModel;
            }
            set
            {
                _bIsShowCheckEQPModel = value;
            }
        }

        public bool IsShowCheckProduct
        {
            get
            {
                return _bIsShowCheckProduct;
            }
            set
            {
                _bIsShowCheckProduct = value;
            }
        }

        public bool IsShowCheckRecipe
        {
            get
            {
                return _bIsShowCheckRecipe;
            }
            set
            {
                _bIsShowCheckRecipe = value;
            }
        }

        public bool IsShowMainModule
        {
            get
            {
                return _bIsShowMainModule;
            }
            set
            {
                _bIsShowMainModule = value;
            }
        }

        public bool IsLastNode
        {
            get
            {
                return _bIsLastNode;
            }
            set
            {
                _bIsLastNode = value;
            }
        }

        public bool IsLastNodeModule
        {
            get
            {
                return _bIsLastNodeModule;
            }
            set
            {
                _bIsLastNodeModule = value;
            }
        }

        public bool IsCheckParamType
        {
            get
            {
                return _bIsCheckParamType;
            }
            set
            {
                _bIsCheckParamType = value;
            }
        }

        public string FilterValue
        {
            get
            {
                return _sFilterValue;
            }
            set
            {
                _sFilterValue = value;
            }
        }


        #endregion

        #region : Initialization

        public TreeInterface()
        {
            _wsSPC = new WebServiceController<eSPCWebService.eSPCWebService>().Create();

            InitializeDefaultCondition();
        }

        #endregion

        #region : Private

        private void InitializeDefaultCondition()
        {
            //DataSet dsCodeData = new DataSet();

            //LinkedList lnkCondition = new LinkedList();
            //lnkCondition.Add(Definition.CONDITION_KEY_CATEGORY, Definition.CODE_CATEGORY_USE_DEFAULT_CONDITION);

            //byte[] baDataUseDefault = lnkCondition.GetSerialData();

            //dsCodeData = this._wsSPC.GetCodeData(baDataUseDefault);

            //if (dsCodeData.Tables.Count > 0)
            //{
            //    this._sUseDefaultCondition = dsCodeData.Tables[0].Rows[0]["CODE"].ToString();
            //}
        }

        #endregion
    }
}
