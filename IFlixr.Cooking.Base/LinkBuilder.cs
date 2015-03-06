using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace IFlixr.Cooking.Base
{
    public static class LinkBuilder
    {
        public static string GetProfileImageLink(string imgName)
        {
            if (String.IsNullOrWhiteSpace(imgName))
                return String.Empty;
            return GlobalSettings.ProfileImageFolder + imgName;
        }

        public static string GetRecipeImageLink(string imgName)
        {
            if (String.IsNullOrWhiteSpace(imgName))
                return String.Empty;
            return GlobalSettings.RecipeImageFolder + imgName;
        }

        public static string GetGalleryLink(string profileUrl)
        {
            if (String.IsNullOrWhiteSpace(profileUrl))
                return String.Empty;

            return GlobalSettings.GalleryUrl + profileUrl;
        }

      
    }
}
