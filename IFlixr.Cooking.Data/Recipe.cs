using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Imports.Newtonsoft.Json;
using Raven.Client.UniqueConstraints;
using IFlixr.Cooking.Base;
using System.ComponentModel.DataAnnotations;

namespace IFlixr.Cooking.Data
{
    public class Recipe
    {
        public Recipe()
        {
            Images = new List<RecipeImage>();
            ActiveTags = new List<string>();
            ViewStatistics = new List<ViewStat>();
            Tags = new List<string>();
        }
        public string Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Title { get; set; }
        public string Description { get; set; }
        [UniqueConstraint]
        public string Url { get; set; }
        [Required]
        public DateTime PostedDate { get; set; }
        [Required]
        public string PostedById { get; set; }
        public int ViewCount { get; set; }
        public List<ViewStat> ViewStatistics { get; set; }
        public int FavourCount { get; set; }
        public List<string> ActiveTags { get; set; }
        public List<string> Tags { get; set; }
        public List<RecipeImage> Images { get; set; }
        public ApprovalStage ApprovalStatus { get; set; }
        public bool IsUserPosted { get; set; }

        [JsonIgnore]
        public string PostedByName { get; set; }
    }

    public enum ApprovalStage
    {
        Pending = 0,
        Approved,
        Declined
    }

    public class ViewStat
    {
        public ViewStat(DateTime date)
        {
            Date = date.Start();
            Count = 1;
        }
        public DateTime Date { get; private set; }
        public int Count { get; set; }
    }
}
