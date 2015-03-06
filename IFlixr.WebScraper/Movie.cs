using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;

namespace IFlixr.WebScraper
{
    public class Movie
    {
        public Movie(MovieEntity<string> name, MovieEntity<short> year, string wikiUrl)
        {
            Year = year;
            Title = name;
            WikiUrl = wikiUrl;

            //default
            DirectedBy = new MovieEntity<string>(MovieEntityType.DirectedBy);
            ProducedBy = new MovieEntity<string>(MovieEntityType.ProducedBy);
            WrittenBy = new MovieEntity<string>(MovieEntityType.WrittenBy);
            Starring = new MovieEntity<string>(MovieEntityType.Starring);
            MusicBy = new MovieEntity<string>(MovieEntityType.MusicBy);
            SongsBy = new MovieEntity<string>(MovieEntityType.SongsBy);
            Cinematography = new MovieEntity<string>(MovieEntityType.Cinematography);
            EditingBy = new MovieEntity<string>(MovieEntityType.EditingBy);
            Studio = new MovieEntity<string>(MovieEntityType.Studio);
            DistributedBy = new MovieEntity<string>(MovieEntityType.DistributedBy);
            ScreenplayBy = new MovieEntity<string>(MovieEntityType.ScreenplayBy);
            ReleaseDate = new MovieEntity<DateTime>(MovieEntityType.ReleaseDate);
            Country = new MovieEntity<Country>(MovieEntityType.Country);
            Language = new MovieEntity<Language>(MovieEntityType.Language);
            RunningTime = new MovieEntity<int>(MovieEntityType.Duration);
            Description = new MovieEntity<string>(MovieEntityType.Description);

        }
       
        public MovieEntity<short> Year { get; set; }
        public MovieEntity<string> Title { get; set; }
        public MovieEntity<string> DirectedBy { get; set; }
        public MovieEntity<string> ProducedBy { get; set; }
        public MovieEntity<string> WrittenBy { get; set; }
        public MovieEntity<string> Starring { get; set; }
        public MovieEntity<string> MusicBy { get; set; }
        public MovieEntity<string> SongsBy { get; set; }
        public MovieEntity<string> Cinematography { get; set; }
        public MovieEntity<string> EditingBy { get; set; }
        public MovieEntity<string> Studio { get; set; }
        public MovieEntity<string> DistributedBy { get; set; }
        public MovieEntity<string> ScreenplayBy { get; set; }

        public MovieEntity<DateTime> ReleaseDate { get; set; }
        public MovieEntity<int> RunningTime { get; set; }
        public MovieEntity<Country> Country { get; set; }
        public MovieEntity<Language> Language { get; set; }
        public string WikiUrl { get; set; }
        public MovieEntity<string> Description { get; set; }

        public override string ToString()
        {
            return "Title: " + Title + Environment.NewLine;
                //"Year: " + Year.ToString() + Environment.NewLine +
                //FlattenEntity("Directed By: ",  DirectedBy) + 
                //FlattenEntity("Produced By: ",  ProducedBy) + 
                //FlattenEntity("Written By: ",  WrittenBy) + 
                //FlattenEntity("Starring By: ",  Starring) + 
                //FlattenEntity("Songs By: ",  SongsBy) + 
                //FlattenEntity("Music By: ",  MusicBy) + 
                //FlattenEntity("Cinematography: ",  Cinematography) + 
                //FlattenEntity("Editing: ",  EditingBy) + 
                //FlattenEntity("Studio: ",  Studio) + 
                //FlattenEntity("Distributed By: ",  DistributedBy) ;

        }

        private string FlattenEntity(string heading, List<MovieEntityItem<object>> entities)
        {
            var text = String.Empty;
            foreach (var entity in entities)
                text += entity.Text + ",";
            return heading + " : " + text + Environment.NewLine;
        }
       
    }

    public class MovieEntityItem<T>
    {
        public MovieEntityItem()
        {
            Links = new List<MovieEntityLink>();
        }

        public MovieEntityItem(T data, List<MovieEntityLink> links = null)
        {
            Data = data;
            Links = links ?? new List<MovieEntityLink>();
        }

        public MovieEntityItem(T data, MovieEntityLink link)
        {
            Data = data;
            Links = new List<MovieEntityLink>() { link };
        }


        public string Text { get { return Data.ToString(); } }
        public T Data { get; set; }
        public MovieEntityLink WikiLink
        {
            get
            {
                return GetLink(LinkType.Wiki);
            }
        }
        public MovieEntityLink FirstLink { get { return Links.FirstOrDefault(); } }
        public List<MovieEntityLink> Links { get; set; }
        public MovieEntityLink GetLink(LinkType type)
        {
            return Links.Where(x => x.Type == type).FirstOrDefault();
        }
        public List<MovieEntityLink> GetLinks(LinkType type)
        {
            return Links.Where(x => x.Type == type).ToList();
        }
    }

    public class MovieEntityLink
    {
        public MovieEntityLink(LinkType type, string url)
        {
            Url = url;
            Type = type;

        }
        public string Url { get; set; }
        public LinkType Type { get; set; }
    }

    public enum LinkType
    {
        Wiki,
        Youtube,
        Vimeo,
        Generic
    }

    public class MovieEntity<T>
    {
        public MovieEntity(MovieEntityType type)
        {
            Type = type;
            Items = new List<MovieEntityItem<T>>();
        }


        public MovieEntity(MovieEntityType type, T[] collection)
        {
            Type = type;
            Items = new List<MovieEntityItem<T>>();
            foreach (var data in collection)
            {
                Items.Add(new MovieEntityItem<T>(data));
            }
        }

        public MovieEntity(MovieEntityType type, T data)
        {
            Type = type;
            Items = new List<MovieEntityItem<T>>();
            Items.Add(new MovieEntityItem<T>(data));
        }

        public MovieEntity(MovieEntityType type, List<MovieEntityItem<T>> items)
        {
            Type = type;
            Items = items ?? new List<MovieEntityItem<T>>();
        }

        public MovieEntity(MovieEntityType type, MovieEntityItem<T> item)
        {
            Type = type;
            Items = new List<MovieEntityItem<T>>() { item };
        }

        public MovieEntityType Type { get; set; }
        public List<MovieEntityItem<T>> Items { get; set; }
        public MovieEntityItem<T> First { get { return Items.FirstOrDefault(); } }
        public T Value
        { 
            get 
            { 
                var first = First;
                if (first != null)
                    return first.Data;
                return default(T);
            } 
        }
        public override string ToString()
        {
            string text = String.Empty;
            Items.ForEach(x => text += x.Text + ",");
            var index = text.LastIndexOf(",");
            if(index != -1)
                text = text.Remove(index);

            return text;
        }
    }

    public enum MovieEntityType
    {
        [KeyWord("title")]
        Title,
        [KeyWord("year")]
        Year,
        [KeyWord("language")]
        Language,
        [KeyWord("country")]
        Country,
        [KeyWord("running time")]
        Duration,
        [KeyWord("directed by")]
        DirectedBy,
        [KeyWord("produced by")]
        ProducedBy,
        [KeyWord("written by", "story by")]
        WrittenBy,
        [KeyWord("starring")]
        Starring,
        [KeyWord("music by")]
        MusicBy,
        [KeyWord("songs by")]
        SongsBy,
        [KeyWord("cinematography")]
        Cinematography,
        [KeyWord("editing by")]
        EditingBy,
        [KeyWord("studio")]
        Studio,
        [KeyWord("distributed by")]
        DistributedBy,
        [KeyWord("screenplay by")]
        ScreenplayBy,
        [KeyWord("release date(s)")]
        ReleaseDate,
        [KeyWord("narrated by")]
        NarratedBy,
        [KeyWord("budget")]
        Budget,
        [KeyWord("box office")]
        BoxOffice,
        [KeyWord("based on")]
        BasedOn,
        [KeyWord("description")]
        Description,
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class KeyWordAttribute : Attribute
    {
        private List<string> keywords = new List<string>();
        public KeyWordAttribute(params string[] kws)
        {
            foreach (var kw in kws)
                this.keywords.Add(kw);
        }

        public bool Matches(string kw)
        {
            return keywords.Contains(kw.ToLower());
        }
    }
}
