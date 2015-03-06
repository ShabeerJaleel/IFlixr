using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.ViewModel.Base;
using IFlixr.Base;

namespace IFlixr.ViewModel
{
    public class MoviePageOld : BaseModel
    {
        public MoviePageOld()
        {
            DynamicPart = new DynamicLayout();
        }
        public string Title { get; set; }
        public Language Language { get; set; }
        public int Duration { get; set; }
        public string DurationText
        {
            get
            {
                if (Duration > 0)
                {
                    var ts = new TimeSpan(0, Duration, 0);
                    return string.Format("{0} hrs {1} min", ts.Hours,
                                ts.Minutes);
                }

                return String.Empty;
            }
        }
        public string Rating { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ReleaseDateText
        {
            get
            {
                if (ReleaseDate.Day != 1 && ReleaseDate.Month != 1)
                    return ReleaseDate.ToShortDateString();
                else
                    return ReleaseDate.Year.ToString();
            }
        }
        public Country Country { get; set; }
        public string ImageLink { get; set; }
        public string Description { get; set; }
        public string MovieLink { get; set; }
        public string MovieLinkText { get; set; }
        public bool IsTrailerAvailable { get { return !String.IsNullOrWhiteSpace(TrailerLink); } }
        public string TrailerLink { get; set; }
        public string TrailerImageLink { get; set; }

        public string Starring { get; set; }
        public string ProducedBy { get; set; }
        public string DirectedBy { get; set; }
        public string WrittenBy { get; set; }
        public string MusicBy { get; set; }
        public string SongsBy { get; set; }
        public string CinematographyBy { get; set; }
        public string EditingBy { get; set; }
        public string Studio { get; set; }
        public string DistributedBy { get; set; }
        public string ScreenplayBy { get; set; }

        //TODO: add required fields
        public DynamicLayout DynamicPart { get; set; }
    }
}
