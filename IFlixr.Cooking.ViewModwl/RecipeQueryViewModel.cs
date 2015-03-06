using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Cooking.ViewModel
{
    public class RecipeQueryViewModel : BaseViewModel
    {
        public RecipeQueryViewModel()
        {
            Recipes = new List<RecipeViewModel>();
        }
        public List<RecipeViewModel> Recipes { get; set; }
        public bool IsMore { get; set; }
    }
}
