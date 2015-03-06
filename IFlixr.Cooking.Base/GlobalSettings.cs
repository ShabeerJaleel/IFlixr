using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace IFlixr.Cooking.Base
{
    public static class GlobalSettings
    {
        public static string ImageDownloadFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageDownloadFolder"];
            }
        }

        public static string ImageFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageFolder"];
            }
        }

        public static string ProfileImageFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["ProfileImageFolder"];
            }
        }

        public static string RecipeImageFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["RecipeImageFolder"];
            }
        }

        public static string GalleryUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["GalleryUrl"];
            }
        }
    }


}
