using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Cooking.ViewModel
{
    public class RecipePageViewModel : RecipeQueryViewModel
    {
        public RecipePageViewModel()
        {
            IsMore = true;
        }

        public string Title { get; set; }
    }


   
}
