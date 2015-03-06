using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.UniqueConstraints;
using System.ComponentModel.DataAnnotations;

namespace IFlixr.Cooking.Data
{
    public class KnownCuisine
    {
        public string Id { get; set; }
        [UniqueConstraint]
        [Required]
        public string Tag { get; set; }

        public string Description { get; set; }

    }
}
