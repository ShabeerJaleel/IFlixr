using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Cooking.Data;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Abstractions.Indexing;
using Raven.Client;
using IFlixr.Cooking.ViewModel;
using IFlixr.Cooking.Base;
using Raven.Client.UniqueConstraints;
using Raven.Abstractions.Data;
using Raven.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace IFlixr.Cooking.Repository
{
    public class RavenRepository
    {
        #region Fields

        private static readonly DocumentStore Store;
        private readonly string Database = "Cooking";
        private static readonly RavenRepository repository = new RavenRepository();

        #endregion

        #region Constructor

        static RavenRepository()
        {
            Store = new DocumentStore
            {
                ConnectionStringName = "RavenDB",
                DefaultDatabase = "Cooking"
            };
            Store.RegisterListener(new UniqueConstraintsStoreListener());
            Store.Conventions.SaveEnumsAsIntegers = true;
            Store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;
            Store.Initialize();
        }

        private RavenRepository()
        {

        }

        public static RavenRepository Create()
        {
            return repository;
        }

        #endregion

        #region Update

        public void PatchRecord(string id, string property, object value)
        {
            Store.DatabaseCommands.Patch(
            id, new[]
                    {
                        new PatchRequest
                            {
                                Type = PatchCommandType.Set,
                                Name = property,
                                Value = new RavenJValue(value)
                            }
                    });
        }

        public int IncrementViewCount(string id, DateTime date)
        {
            date = date.Start();
            using (var session = Store.OpenSession(Database))
            {
                var recipe = session.Load<Recipe>(id);
                var current = recipe.ViewStatistics.FirstOrDefault(x => x.Date == date);
                if (current != null)
                    current.Count++;
                else
                    recipe.ViewStatistics.Add(new ViewStat(date));
                recipe.ViewCount++;
                session.SaveChanges();
                return recipe.ViewCount;
            }
        }

        public int ToggleFavourite(string recipeId, string userId, out bool isFavourite)
        {
            isFavourite = false;
            using (var session = Store.OpenSession(Database))
            {
                var user = session.Query<User>().Single(x => x.Email == userId);
                var recipe = session.Load<Recipe>(recipeId);
                if (user.FavouredRecipes == null)
                    user.FavouredRecipes = new List<string>();

                if (!user.FavouredRecipes.Contains(recipeId))
                {
                    recipe.FavourCount++;
                    user.FavouredRecipes.Add(recipeId);
                    isFavourite = true;
                }
                else
                {
                    if (recipe.FavourCount > 0)
                        recipe.FavourCount--;
                    user.FavouredRecipes.Remove(recipeId);

                }
                session.SaveChanges();
                return recipe.FavourCount;
            }
        }

        #endregion

        #region User

        #region Update
       
        public void CreateUser(User user)
        {
            ValidateUser(user);
            using (var session = Store.OpenSession(Database))
            {
                if (session.LoadByUniqueConstraint<User>(x => x.Email, user.Email) != null)
                    throw new Exception("Email already exists");
                if (session.LoadByUniqueConstraint<User>(x => x.ProfileName, user.ProfileName) != null)
                    throw new Exception("Profile name already exists");

                session.Store(user);
                session.SaveChanges();
            }
        }

        private void ValidateUser(User user)
        {
            var validationContext = new ValidationContext(user, null, null);
            Validator.ValidateObject(user, validationContext);
        }

        public void SaveUser(User user)
        {
            using (var session = Store.OpenSession(Database))
            {
                session.Store(user);
                session.SaveChanges();
            }
        }

        #endregion

        #region Retrieve

        public User GetAuthorByUrl(string url)
        {
            using (var session = Store.OpenSession(Database))
            {
                return session.Query<User>().Where(x =>  x.Sites.Any(y => y.Url == url)).Single();
            }
        }

        public List<User> GetAllUsersWithFeedScraping()
        {
            using (var session = Store.OpenSession(Database))
            {
                return session.Query<User>().Where(x => x.Sites.Any(y => y.FeedScrapeEnabled)).ToList();
            }
        }

        public User GetUserByEmail(string email)
        {
            using (var session = Store.OpenSession(Database))
            {
                return session.Query<User>().FirstOrDefault(x => x.Email == email);
            }
        }

        public User GetUserByProfileName(string profileName)
        {
            //profileName = profileName.ToLower();
            using (var session = Store.OpenSession(Database))
            {
                return session.Query<User>().FirstOrDefault(x => x.ProfileName == profileName);
            }
        }

        public User GetUserByProfileUrl(string profileUrl)
        {
            //profileName = profileName.ToLower();
            using (var session = Store.OpenSession(Database))
            {
                return session.Query<User>().FirstOrDefault(x => x.ProfileUrl == profileUrl);
            }
        }

        public User GetUser(string providerName, string providerUserId)
        {
            using (var session = Store.OpenSession(Database))
            {
                return session.Query<User>().SingleOrDefault(x => x.ExternalAccounts.Any(y => y.ProviderName == providerName && y.ProviderUserId == providerUserId));
            }
        }

        public User GetUser(string userId)
        {
            using (var session = Store.OpenSession(Database))
            {
                return session.Load<User>(userId);
            }
        }

        public List<User> GetAllUsers()
        {
            using (var session = Store.OpenSession(Database))
            {
                return session.Query<User>().ToList();
            }
        }

        #endregion

        #endregion

        #region Recipe

        #region Update

        public void SaveRecipe(Recipe recipe)
        {
            using (var session = Store.OpenSession(Database))
            {
                session.Store(recipe);
                session.SaveChanges();
            }
        }

        public void SaveRecipeAndTags(Recipe recipe)
        {
            lock (Store)
            {
                using (var session = Store.OpenSession(Database))
                {
                    var knownTags = session.Query<KnownRecipeTagCollection>().FirstOrDefault();
                    if (knownTags == null)
                        knownTags = new KnownRecipeTagCollection();
                    foreach (var tag in recipe.ActiveTags)
                    {
                        if (!knownTags.KnowsTags.Any(x => x.Name == tag))
                            knownTags.KnowsTags.Add(new KnownRecipeTag
                            {
                                Name = tag
                            });
                    }
                    session.Store(knownTags);
                    session.Store(recipe);
                    session.SaveChanges();
                }
            }

        }

        #endregion

        #region Retrieve

        public List<Recipe> GetPendingRecipies(out int total, int count = 1)
        {
            RavenQueryStatistics stats; 
            using (var session = Store.OpenSession(Database))
            {
                var result = session.Query<Recipe>()
                    .Statistics(out stats)
                    .Where(x => x.ApprovalStatus == ApprovalStage.Pending)
                    .Take(count).ToList();
                total = stats.TotalResults;
                return result;
            }
        }

        public List<Recipe> GetAllRecipes()
        {
            using (var session = Store.OpenSession(Database))
            {
                return session.Query<Recipe>().ToList();
            }
        }

        public RecipePageViewModel GetRecipes(string sort, string duration, int count = 20)
        {
            if ("popular" == sort)
                return GetPopularApprovedRecipes(duration, 20);
            else if ("favourite" == sort)
                return GetFavouriteApprovedRecipes(duration, 20);
            else
                return GetLatestApprovedRecipes(duration, 20);
        }

        public RecipePageViewModel GetRecipesByAuthor(string authorId, string sort, string duration, int count = 20)
        {
            if ("popular" == sort)
                return GetPopularApprovedRecipesByAuthor(duration, authorId, 20);
            else if ("favourite" == sort)
                return GetFavouriteApprovedRecipesByAuthor(duration, authorId, 20);
            else
                return GetLatestApprovedRecipesByAuthor(duration, authorId, 20);

        }

        public RecipePageViewModel GetLatestApprovedRecipes(string duration, int count = 20)
        {
            var start = GetStart(duration);

            using (var session = Store.OpenSession(Database))
            {
                var recipes = session.Query<Recipe>()
                    .Customize(x => x.Include<Recipe>(o => o.PostedById))
                    .Where(x => x.ApprovalStatus == ApprovalStage.Approved && x.PostedDate >= start)
                    .Take(count)
                    .OrderByDescending(x => x.PostedDate).ToList();

                return CreateModel(recipes, session);
            }
        }

        public RecipePageViewModel GetPopularApprovedRecipes(string duration, int count = 20)
        {
            var start = GetStart(duration);

            using (var session = Store.OpenSession(Database))
            {
                var recipes = session.Query<Recipe>()
                    .Customize(x => x.Include<Recipe>(o => o.PostedById))
                    .Where(x => x.ApprovalStatus == ApprovalStage.Approved && x.PostedDate >= start)
                    .OrderByDescending(x => x.ViewCount)
                    .Take(count).ToList();

                return CreateModel(recipes, session);
            }
        }

        public RecipePageViewModel GetFavouriteApprovedRecipes(string duration, int count = 20)
        {
            var start = GetStart(duration);

            using (var session = Store.OpenSession(Database))
            {
                var recipes = session.Query<Recipe>()
                    .Customize(x => x.Include<Recipe>(o => o.PostedById))
                    .Where(x => x.ApprovalStatus == ApprovalStage.Approved && x.PostedDate >= start)
                    .OrderByDescending(x => x.FavourCount)
                    .Take(count).ToList();

                return CreateModel(recipes, session);
            }
        }

        public RecipePageViewModel GetLatestApprovedRecipesByAuthor(string duration, string authorId, int count = 20)
        {
            var start = GetStart(duration);
            using (var session = Store.OpenSession(Database))
            {
                var recipes = session.Query<Recipe>()
                    .Customize(x => x.Include<Recipe>(o => o.PostedById))
                    .Where(x => x.ApprovalStatus == ApprovalStage.Approved &&
                        x.PostedDate >= start && x.PostedById == authorId)
                    .Take(count)
                    .OrderByDescending(x => x.PostedDate).ToList();

                return CreateModel(recipes, session);
            }
        }

        public RecipePageViewModel GetPopularApprovedRecipesByAuthor(string duration, string authorId, int count = 20)
        {
            var start = GetStart(duration);

            using (var session = Store.OpenSession(Database))
            {
                var recipes = session.Query<Recipe>()
                    .Customize(x => x.Include<Recipe>(o => o.PostedById))
                    .Where(x => x.ApprovalStatus == ApprovalStage.Approved &&
                        x.PostedDate >= start && x.PostedById == authorId)
                    .OrderByDescending(x => x.ViewCount)
                    .Take(count).ToList();

                return CreateModel(recipes, session);
            }
        }

        public RecipePageViewModel GetFavouriteApprovedRecipesByAuthor(string duration, string authorId, int count = 20)
        {
            var start = GetStart(duration);
            using (var session = Store.OpenSession(Database))
            {
                var recipes = session.Query<Recipe>()
                    .Customize(x => x.Include<Recipe>(o => o.PostedById))
                    .Where(x => x.ApprovalStatus == ApprovalStage.Approved &&
                        x.PostedDate >= start && x.PostedById == authorId)
                    .OrderByDescending(x => x.FavourCount)
                    .Take(count).ToList();

                return CreateModel(recipes, session);
            }
        }

        public bool IsRecipeExists(string url)
        {
            using (var session = Store.OpenSession(Database))
            {
                return session.Query<Recipe>().Any(x => x.Url == url);
            }
        }

        private RecipePageViewModel CreateModel(List<Recipe> recipes, IDocumentSession session)
        {
            return new RecipePageViewModel
            {
                Recipes = recipes.Select(x => new RecipeViewModel
                {
                    AuthorId = x.PostedById,
                    AuthorTitle = session.Load<User>(x.PostedById).ProfileName,
                    Description = x.Description,
                    FavourCount = x.FavourCount,
                    Id = x.Id,
                    ImageUrl = LinkBuilder.GetRecipeImageLink(x.Images.First(y => y.IsDefault).ImageThumbnail),
                    PostedDate = x.PostedDate,
                    Title = x.Title,
                    Url = x.Url,
                    ViewCount = x.ViewCount,
                    GalleryUrl = LinkBuilder.GetGalleryLink(session.Load<User>(x.PostedById).ProfileUrl)
                }).ToList()
            };
        }

        public RecipeQueryViewModel Query(string query, int count = 20)
        {

            using (var session = Store.OpenSession(Database))
            {
                RavenQueryStatistics stats; ;
                var items = session.Query<Recipe>("Recipes/ByApprovalStatusAndTitle")
                     .Customize(x => x.Include<Recipe>(o => o.PostedById))
                     .Statistics(out stats)
                    .Where(x => x.ApprovalStatus == ApprovalStage.Approved && x.Title.StartsWith(query))
                    .Take(count).ToList();
                return new RecipeQueryViewModel
                {
                    IsMore = stats.TotalResults > count,
                    Recipes = items.Select(x => new RecipeViewModel
                    {
                        AuthorId = x.PostedById,
                        AuthorTitle = session.Load<User>(x.PostedById).ProfileName,
                        Description = x.Description,
                        FavourCount = x.FavourCount,
                        Id = x.Id,
                        ImageUrl = LinkBuilder.GetRecipeImageLink(x.Images.First(y => y.IsDefault).ImageThumbnail),
                        PostedDate = x.PostedDate,
                        Title = x.Title,
                        Url = x.Url,
                        ViewCount = x.ViewCount
                    }).ToList()
                };

            }
        }

        #endregion

        #endregion

        #region Tags

        public List<string> GetCuisineTags()
        {
            using (var session = Store.OpenSession(Database))
            {
                return session.Query<KnownCuisine>().Select(x => x.Tag).ToList();
            }
        }

        public void SaveCuisine(KnownCuisine cuisine)
        {
            using (var session = Store.OpenSession(Database))
            {
                if (session.LoadByUniqueConstraint<KnownCuisine>(x => x.Tag, cuisine.Tag) != null)
                    throw new Exception("Cuisine Tag already exists");

                session.Store(cuisine);
                session.SaveChanges();
            }
        }

        public void SaveKnownRecipeTags(KnownRecipeTagCollection tagsCollection)
        {
            using (var session = Store.OpenSession(Database))
            {
                session.Store(tagsCollection);
                session.SaveChanges();
            }
        }

        public KnownRecipeTagCollection GetKnownRecipeTags()
        {
            using (var session = Store.OpenSession(Database))
            {
                return session.Query<KnownRecipeTagCollection>().FirstOrDefault() ?? new KnownRecipeTagCollection();
            }
        }

        #endregion

        private DateTime GetStart(string duration)
        {

            if ("today" == duration)
                return DateTime.Now.Start();
            else if ("week" == duration)
                return DateTime.Now.AddDays(-7).Start();
            else if ("month" == duration)
                return DateTime.Now.AddMonths(-1).Start();
            else
                return new DateTime(1970, 1, 1);
        }

        public void CreateIndexes()
        {
            // Contains search
            try
            {
                var result = Store.DatabaseCommands.PutIndex("Recipes/ByApprovalStatusAndTitle",
                    new IndexDefinitionBuilder<Recipe>
                    {
                        Map = recipes => from recipe in recipes
                                         select new { recipe.Title, recipe.ApprovalStatus },
                        Indexes =
                {
                    { x => x.Title, FieldIndexing.Analyzed}
                }
                    });
            }
            catch { }

        }
    }
}
