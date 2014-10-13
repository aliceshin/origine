using System;
using System.Collections.Generic;
using System.Text;

namespace BISTel.eSPC.Condition.ATT.Controls
{
    public static class ImageLoaderHandler
    {
        static readonly ImageLoader instance = new ImageLoader();

        public static ImageLoader Instance
        {
            get
            {
                return instance;
            }
        }

        static ImageLoaderHandler()
        {
            instance = (ImageLoader)Activator.CreateInstance(typeof(ImageLoader), true);
        }
    }
}
