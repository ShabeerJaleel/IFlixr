using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Cooking.ViewModel
{
    public class RecipeViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime PostedDate { get; set; }
        public string PosedDateText { get { return PostedDate.ToString("dd MMM, yyyy"); } }
        public string AuthorId { get; set; }
        public string AuthorTitle { get; set; }
        public string GalleryUrl { get; set; }
        public int ViewCount { get; set; }
        public int FavourCount { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFavourite { get; set; }
    }
}
