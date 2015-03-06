using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Imports.Newtonsoft.Json;
using Raven.Client.UniqueConstraints;
using System.ComponentModel.DataAnnotations;

namespace IFlixr.Cooking.Data
{
    public class WebSite
    {
      
        [Required]
        [DataType(DataType.Url)]
        [UniqueConstraint]
        public string Url { get; set; }
        public string ScrappingRootUrl { get; set; }
        public bool WebscrapeEnabled { get; set; }
        public bool FeedScrapeEnabled { get; set; }
        public string FeedUrl { get; set; }
        public string ImageCss { get; set; }
        public DateTime LastScrappedTime { get; set; }
        public DateTime LastFeededTime { get; set; }
    }

    public class User
    {
        public static readonly string DefaultMalePicture = "default_male.gif";
        public static readonly string DefaultFemalePicture = "default_female.gif";
        
        public User()
        {
            ExternalAccounts = new List<OAuthAccount>();
            Sites = new List<WebSite>();
            FavouredRecipes = new List<string>();
            Tags = new List<string>();
        }
        public string Id { get; set; }
        [UniqueConstraint]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [UniqueConstraint]
        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [RegularExpression("^[\\w ]+$", ErrorMessage = "Profile name should containt letter, number and space only")]
        public string ProfileName { get; set; }
        public string ProfileUrl { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }
        public string PictureName { get; set; }
        [Required]
        [StringLength(1, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Gender { get; set; }
        public string Description { get; set; }

        public bool LoginEnabled { get; set; }
        public bool HasLocalAccount { get; set; }
        public bool SystemCreated { get; set; }
        public DateTime RegisteredOn { get; set; }
        
        public DateTime LastLogin { get; set; }
        public List<string> FavouredRecipes { get;set;}
        public List<WebSite> Sites { get; set; }
        public List<OAuthAccount> ExternalAccounts { get; set; }
        public List<string> Tags { get; set; }

        [JsonIgnore]
        public string Name { get { return String.Format("{0} {1}", FirstName, LastName); } }
    }

    public class OAuthAccount
    {
        public OAuthAccount()
        {
            ExtraData = new Dictionary<string, string>();
        }
        public string ProviderName { get; set; }
        public string ProviderUserId { get; set; }
        public IDictionary<string, string> ExtraData { get; set; }
    }
}
