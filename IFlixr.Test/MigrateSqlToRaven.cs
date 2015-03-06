using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IFlixr.Data;
using IFlixr.Document;
using System.Threading;


namespace IFlixr.Test
{
    [TestClass]
    public class MigrateSqlToRaven
    {
       
        [TestMethod]
        public void MigrateMovies()
        {
            
            using (var db = new IFlixrContext())
            {
                new SqlExportService().Add(db.Movies);

            }
        }

        [TestMethod]
        public void MigrateVideos()
        {

            using (var db = new IFlixrContext())
            {
                new SqlExportService().AddVideos(db.Shows.Where(x => x.Type != 0));

            }
        }

        [TestMethod]
        public void CreateUser()
        {

            using (var db = new IFlixrContext())
            {
                new DocumentService().CreateUser(new IFlixr.Data.Model.User { Email="admin@iflixr.com", Name = "Admin" });

            }
           
        }
    }
}
