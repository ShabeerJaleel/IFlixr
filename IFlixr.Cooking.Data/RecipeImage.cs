using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Imports.Newtonsoft.Json;

namespace IFlixr.Cooking.Data
{
    [JsonObject(Title = "VideoImage")]
    public class RecipeImage
    {
        public string OriginalUrl { get; set; }
        public string ImageThumbnail { get; set; }
        public string ImageOriginal { get; set; }
        public bool IsDefault { get; set; }
    }
}
