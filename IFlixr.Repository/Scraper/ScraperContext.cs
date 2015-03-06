using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Data.Scraper
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    public class ScraperContext : DbContext
    {

        public ScraperContext()
            : base("name=IFlixr_ScraperEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public DbSet<TempData> TempDatas { get; set; }
        public DbSet<Video> Videos { get; set; }
    }
}
