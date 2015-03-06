using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Imports.Newtonsoft.Json;
using IFlixr.Base;

namespace IFlixr.Data.Model
{
    [JsonObject(Title = "Movie")]
    public class Movie : Video
    {
        public DateTime ReleaseDate { get; set; }
        public string DirectedBy { get; set; }
        public string ProducedBy { get; set; }
        public string WrittenBy { get; set; }
        public string Starring { get; set; }
        public string MusicBy { get; set; }
        public string SongsBy { get; set; }
        public string CinematographyBy { get; set; }
        public string EditingBy { get; set; }
        public string Studio { get; set; }
        public string DistributedBy { get; set; }
        public string ScreenplayBy { get; set; }

        [JsonIgnore]
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
    }
}
