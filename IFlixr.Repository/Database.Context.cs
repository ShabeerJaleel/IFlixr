﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IFlixr.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class IFlixrEntities : DbContext
    {
        public IFlixrEntities()
            : base("name=IFlixrEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Show> Shows { get; set; }
        public DbSet<VideoLink> VideoLinks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<CaroselItem> CaroselItems { get; set; }
        public DbSet<Carousel> Carousels { get; set; }
    }
}
