using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace IFlixr.Base
{
    public static class GlobalSettings
    {
        public static string ImageRootUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageRootUrl"];
            }
        }

        public static string MovieRootUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["MovieRootUrl"];
            }
        }

        public static string MovieImageUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["MovieImageUrl"];
            }
        }

        public static string PlaceHolderMoviePoster
        {
            get
            {
                return ConfigurationManager.AppSettings["PlaceHolderMoviePoster"];
            }
        }

        public static int PageSizeInBrowseMovie
        {
            get
            {
                return 2;
            }
        }

        public static int InitialYearInBrowseMovie
        {
            get
            {
                return 2012;
            }
        }
        
    }


}
