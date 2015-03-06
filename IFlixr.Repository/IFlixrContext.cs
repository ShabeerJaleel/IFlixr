namespace IFlixr.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class IFlixrContext : DbContext, IDisposable
    {
        public IFlixrContext()
            : base("name=IFlixrEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<VideoLink> VideoLinks { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Carousel> Carousels { get; set; }
    }
}
