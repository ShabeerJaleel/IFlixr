using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.UniqueConstraints;
using System.ComponentModel.DataAnnotations;

namespace IFlixr.Cooking.Data
{
    public class KnownRecipeTagCollection
    {
        public KnownRecipeTagCollection()
        {
            KnowsTags = new List<KnownRecipeTag>();
        }
        public List<KnownRecipeTag> KnowsTags { get; set; }
    }

    public class KnownRecipeTag
    {
        [UniqueConstraint]
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
