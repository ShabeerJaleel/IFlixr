using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IFlixr.Cooking.Repository;
using IFlixr.Cooking.Data;

namespace IFlixr.Cooking.Test
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void CreateUser()
        {
            var repo = RavenRepository.Create();
            repo.CreateUser(new User
            {
                Email = "shabeerjaleel@gmail.com",
                Password = "(Sui5cv9)",
                LoginEnabled = true,
                ProfileName = "admin",
                FirstName = "Shabeer",
                LastName = "Jaleel",
                 Gender="m",
                  RegisteredOn = DateTime.Now,
                   SystemCreated = true
            });
        }

        [TestMethod]
        public void CreateAuthor()
        {
            var author = new User
            {
                Id = "users/2",
               
                FirstName = "Prerna",
                LastName = "Singh",
                ProfileName = "IndianSimmer",
                Email = "prerna@malinator.com",
                Gender = "f",
                HasLocalAccount = true,
                SystemCreated = true,
                Password = "(Sui5cv9)",
                RegisteredOn = DateTime.Now,
                LoginEnabled = false,
                Sites = new List<WebSite>{ new WebSite
                {
                    Url = "http://www.indiansimmer.com/",
                    ScrappingRootUrl = "http://www.indiansimmer.com/",
                    FeedUrl = "http://feeds.feedburner.com/indiansimmer?format=xml",
                    ImageCss = ".entry-content * img",
                }}
            };
            RavenRepository.Create().CreateUser(author);
            //var data = new IndianSimmer().ScrapeRecipies();
            // var data = new AayisRecipes().ScrapeRecipies();
        }

    }
}
