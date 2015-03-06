using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;

namespace IFlixr.WebScraper.Client.Model
{

    public abstract class EntityModel
    {
        public EntityModel()
        {
            Videos = new SortableBindingList<VideoModel>();
        }
        public virtual string Title { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual short Year { get; set; }
        public SortableBindingList<VideoModel> Videos { get; set; }
    }

    public class MovieModel : EntityModel
    {
        public MovieModel()
        {
            Music = new List<MusicModel>();
            Trailor = new List<TrailorModel>();
            Images = new List<ImageModel>();
            Clip = new List<ClipModel>();
            SearchResults = new SortableBindingList<VideoModel>();
        }
        public Movie Movie { get; set; }
        public List<MusicModel> Music { get; set; }
        public List<TrailorModel> Trailor { get; set; }
        public List<ClipModel> Clip { get; set; }
        public List<ImageModel> Images { get; set; }
        public SortableBindingList<VideoModel> SearchResults { get; set; }

        public override short Year
        {
            get
            {
                return Movie.Year.Value;
            }
            set
            {
               Movie.Year = new MovieEntity<short>(MovieEntityType.Year, value);
            }
        }

        public override string Title
        {
            get
            {
                return Movie.Title.ToString();
            }
        }
    }

    public class MusicModel : EntityModel
    {
        public MusicModel(string title, string musicId, short year)
        {
            Title = title;
            Id = musicId;
            Year = year;
        }
    }

    public class TrailorModel : EntityModel
    {
        public TrailorModel(string title, string id, short year)
        {
            Title = title;
            Id = id;
            Year = year;
        }
    }

    public class ClipModel : EntityModel
    {
        public ClipModel(string title, string id, VideoType type, short year)
        {
            Title = title;
            Id = id;
            Type = type;
            Year = year;
        }

        public VideoType Type { get; set; }
    }

  

    public class ImageModel
    {
        public string FullPath { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int SupportedDimensions { get; set; }
        public bool IsDefault { get; set; }
    }
   
}
