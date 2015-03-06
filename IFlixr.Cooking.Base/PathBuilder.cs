using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Cooking.Base
{
    public static class PathBuilder
    {
        public static string GetRecipeImageFolder(string imageName)
        {
            return GlobalSettings.RecipeImageFolder + imageName.Substring(0, 3) + "/";
        }

        public static string GetRecipeImagePath(string imageName)
        {
            return GetRecipeImageFolder(imageName) + imageName;
        }

      
    }
}
