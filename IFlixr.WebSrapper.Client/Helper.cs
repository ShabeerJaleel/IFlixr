using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using IFlixr.Data.Scraper;
using System.Windows.Forms;

namespace IFlixr.WebScraper.Client
{
    public static class Helper
    {
        public static readonly string RootImageTempPath;
        public static readonly string RootImagePath;

        static Helper()
        {
            RootImagePath =@"Images\Movies\";
            RootImageTempPath = @"Images\Temp\";
        }


        public static SortableBindingList<MovieDetailSummaryGridItem> CreateMovieSummaryItems(Movie movie)
        {
            var items = new SortableBindingList<MovieDetailSummaryGridItem>();
            items.Add(CreateMovieSummaryItem( movie.Title));
            items.Add(CreateMovieSummaryItem(movie.Year));
            items.Add(CreateMovieSummaryItem(movie.Language));
            items.Add(CreateMovieSummaryItem(movie.Country));
            items.Add(CreateMovieSummaryItem(movie.ProducedBy));
            items.Add(CreateMovieSummaryItem(movie.DirectedBy));
            items.Add(CreateMovieSummaryItem(movie.WrittenBy));
            items.Add(CreateMovieSummaryItem(movie.Starring));
            items.Add(CreateMovieSummaryItem(movie.Cinematography));
            items.Add(CreateMovieSummaryItem(movie.EditingBy));
            items.Add(CreateMovieSummaryItem(movie.MusicBy));
            items.Add(CreateMovieSummaryItem(movie.DistributedBy));
            items.Add(CreateMovieSummaryItem(movie.ScreenplayBy));
            items.Add(CreateMovieSummaryItem(movie.SongsBy));
            items.Add(CreateMovieSummaryItem(movie.Studio));
            items.Add(CreateMovieSummaryItem(movie.ReleaseDate));
            items.Add(CreateMovieSummaryItem(movie.RunningTime));
            items.Add(CreateMovieSummaryItem(movie.Description));
            return items;
        }

        private static MovieDetailSummaryGridItem CreateMovieSummaryItem<T>(MovieEntity<T> data)
        {
            return new MovieDetailSummaryGridItem()
            {
                Type = data.Type,
                Text = data.ToString(),
                Data = data
            };
        }

    
    }

    
}
