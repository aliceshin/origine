using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using BISTel.eSPC.Common;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using BISTel.PeakPerformance.Client.CommonLibrary;

namespace BISTel.eSPC.Condition.Controls
{
    public class ImageLoader
    {
        private LinkedList _imageList = new LinkedList();
        private Image[] _arImage = null;

        public LinkedList TreeImageList
        {
            get
            {
                return _imageList;
            }
            set
            {
                _imageList = value;
            }
        }

        public Image[] TreeArrayImage
        {
            get
            {
                return _arImage;
            }
            set
            {
                _arImage = value;
            }
        }

        public enum TREE_IMAGE_INDEX
        {
            SITE = 0,
            FAB = 1,
            LINE = 2,
            AREA = 3,
            EQP_MODEL = 4,
            EQP = 5,
            DCP_ACTIVE = 6,
            DCP_DEACTIVE = 7,
            MODULE = 8,
            PRODUCT = 9,
            RECIPE = 10,
            STEP = 11,
            SPECGROUP = 12,
            PARAMETER = 13,
            TRACE_PARAM = 14,
            TRACE_SUM_PARAM = 15,
            EVENT_SUM_PARAM = 16,
            EXTERNAL_PARAM = 17,
            VIRTUAL_PARAM = 18,
            MULTIVATIATE_MODEL = 19,
            PARAM_GROUP = 20
        }

        public ImageLoader()
        {
            TreeLoadImage();
        }

        public void TreeLoadImage()
        {
            try
            {
                Image imgSite = LoadImage(Definition.TREE_ICON_SITE);
                Image imgFab = LoadImage(Definition.TREE_ICON_FAB);
                Image imgLine = LoadImage(Definition.TREE_ICON_LINE);
                Image imgArea = LoadImage(Definition.TREE_ICON_AREA);
                Image imgEqpModel = LoadImage(Definition.TREE_ICON_EQP_MODEL);
                Image imgEqp = LoadImage(Definition.TREE_ICON_EQP);
                Image imgDcpActive = LoadImage(Definition.TREE_ICON_DCP_ACTIVE);
                Image imgDcpDeactive = LoadImage(Definition.TREE_ICON_DCP_DEACTIVE);
                Image imgModule = LoadImage(Definition.TREE_ICON_MODULE);
                Image imgProduct = LoadImage(Definition.TREE_ICON_PRODUCT);
                Image imgRecipe = LoadImage(Definition.TREE_ICON_RECIPE);
                Image imgStep = LoadImage(Definition.TREE_ICON_STEP);
                Image imgSpec = LoadImage(Definition.TREE_ICON_SPECGROUP);
                Image imgParam = LoadImage(Definition.TREE_ICON_PARAMETER);
                Image imgTraceParam = LoadImage(Definition.TREE_ICON_TRACE_PARAM);
                Image imgTraceSumParam = LoadImage(Definition.TREE_ICON_TRACE_SUM_PARAM);
                Image imgEventSumParam = LoadImage(Definition.TREE_ICON_EVENT_SUM_PARAM);
                Image imgExternalParam = LoadImage(Definition.TREE_ICON_EXTERNAL_PARAM);
                Image imgVirtualParam = LoadImage(Definition.TREE_ICON_VIRTUAL_PARAM);
                Image imgMvaModel = LoadImage(Definition.TREE_ICON_MULTIVARIATE_MODEL);
                Image imgParamGroup = LoadImage(Definition.TREE_ICON_PARAM_GROUP);

                _imageList.Add(TREE_IMAGE_INDEX.SITE, imgSite);
                _imageList.Add(TREE_IMAGE_INDEX.FAB, imgFab);
                _imageList.Add(TREE_IMAGE_INDEX.LINE, imgLine);
                _imageList.Add(TREE_IMAGE_INDEX.AREA, imgArea);
                _imageList.Add(TREE_IMAGE_INDEX.EQP_MODEL, imgEqpModel);
                _imageList.Add(TREE_IMAGE_INDEX.EQP, imgEqp);
                _imageList.Add(TREE_IMAGE_INDEX.DCP_ACTIVE, imgDcpActive);
                _imageList.Add(TREE_IMAGE_INDEX.DCP_DEACTIVE, imgDcpDeactive);
                _imageList.Add(TREE_IMAGE_INDEX.MODULE, imgModule);
                _imageList.Add(TREE_IMAGE_INDEX.PRODUCT, imgProduct);
                _imageList.Add(TREE_IMAGE_INDEX.RECIPE, imgRecipe);
                _imageList.Add(TREE_IMAGE_INDEX.STEP, imgStep);
                _imageList.Add(TREE_IMAGE_INDEX.SPECGROUP, imgSpec);
                _imageList.Add(TREE_IMAGE_INDEX.PARAMETER, imgParam);
                _imageList.Add(TREE_IMAGE_INDEX.TRACE_PARAM, imgTraceParam);
                _imageList.Add(TREE_IMAGE_INDEX.TRACE_SUM_PARAM, imgTraceSumParam);
                _imageList.Add(TREE_IMAGE_INDEX.EVENT_SUM_PARAM, imgEventSumParam);
                _imageList.Add(TREE_IMAGE_INDEX.EXTERNAL_PARAM, imgExternalParam);
                _imageList.Add(TREE_IMAGE_INDEX.VIRTUAL_PARAM, imgVirtualParam);
                _imageList.Add(TREE_IMAGE_INDEX.MULTIVATIATE_MODEL, imgMvaModel);
                _imageList.Add(TREE_IMAGE_INDEX.PARAM_GROUP, imgParamGroup);

                _arImage = new Image[_imageList.Count];

                for (int i = 0; i < _imageList.Count; i++)
                {
                    _arImage[i] = (Image)_imageList.GetValue(i);
                }
            }
            catch (Exception ex)
            {
                LogHandler.ExceptionLogWrite(Definition.APPLICATION_NAME, ex);
            }
        }

        public Image LoadImage(string name)
        {
            if (File.Exists(Application.StartupPath + "\\" + name) == false)
            {
                WebClient wc = new WebClient();
                string sPath = BISTel.PeakPerformance.Client.CommonLibrary.Configuration.Instance.SkinPath + Definition.IMAGE_PATH + name;
                wc.DownloadFile(sPath, Application.StartupPath + "\\" + name);
            }

            Image image = Image.FromFile(Application.StartupPath + "\\" + name);
            return image;
        }
    }
}
