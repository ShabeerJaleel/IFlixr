using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScrapySharp.Extensions;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Web;
using System.Net;
using System.Text.RegularExpressions;
using IFlixr.Base;


namespace IFlixr.WebScraper
{
    public class WikiScraper
    {
        #region Fields

        private static readonly List<string> FilmColumnTitles = new List<string>() {"film", "title","movie"};
        private static readonly string WikiRootUrl = "http://en.wikipedia.org";
        private static readonly Regex RunningTimeRegex = new Regex(@"([^\d]|^)\d{1,3}([^\d]|$)");
        private static readonly Regex NumberFinderRegex = new Regex(@"[0-9]{1,3}");
        private static readonly Regex SubscriptRegex = new Regex(@"\[\w\]");

        
        #endregion

        #region Constructor

       

        #endregion

        #region Public

        public List<MovieName> GetMovieNames(IEnumerable<HtmlNode> nodes, short year)
        {
            var movieNames = new List<MovieName>();

            foreach (var table in nodes)
            {
                var colHeads = table.CssSelect("tr th").ToList();

                var index = FindMovieTitleColumnIndex(colHeads);
                if (index == -1)
                    continue;
                var headerCount = GetColumnHeaderCount(colHeads);
                
                var rows = table.CssSelect("tr").ToList();
                var lastIndex = 0;
                for (int i = 1; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var tdIndex = FindMovieCell(row.SelectNodes("td").ToList());
                    if (tdIndex == -1)
                        continue;
                    lastIndex = tdIndex;
                    var firstChild = row.SelectSingleNode(String.Format("td[{0}]", tdIndex + 1)).FirstChild;
                    var anchorNode = firstChild.CssSelect("a").FirstOrDefault();

                    string wikiTitle = null;
                    string href = null;
                    if (anchorNode != null)
                    {
                        if (anchorNode.Attributes.Contains("title"))
                        {
                            wikiTitle = HttpUtility.HtmlDecode(anchorNode.Attributes["title"].Value);
                            href = anchorNode.Attributes["href"].Value;
                        }
                    }
                    else
                    {
                        var titleLink = row.SelectSingleNode(String.Format("td[{0}]", tdIndex + 1)).FirstChild;
                        wikiTitle = titleLink.InnerHtml;
                    }

                    if (!String.IsNullOrWhiteSpace(wikiTitle) && !movieNames.Where(x => x.WikiTitle == wikiTitle).Any())
                        movieNames.Add(CreateMovieName(wikiTitle, href, year));
                }
            }

            return movieNames.OrderBy(x => x.Name).ToList();
        }

        public Movie GetMovie(MovieName mn)
        {
            var movie = Creator.CreateMovie(mn);

            if (!mn.IsValidUrl)
                 return movie;

            var rootNode = CreateWeb().Load(mn.WikiUrl).DocumentNode;
            var nodes = rootNode.CssSelect("table.infobox").ToList();
            if (nodes.Count == 0)
                return movie;
            movie.Description = new MovieEntity<string>(MovieEntityType.Description,
                SubscriptRegex.Replace(rootNode.CssSelect("#mw-content-text p").First().InnerText, ""));
            
            var rows = nodes[0].CssSelect("tr").ToList();
            if (rows.Count == 0)
                return movie;

           
            foreach (var row in rows)
            {
                var heading = row.SelectSingleNode("th");
                if (heading != null)
                {
                    var content = row.SelectSingleNode("td");

                    if (content != null)
                       AddEntity(movie, content, heading.InnerText.ToLower().Trim());
                    
                }
            }
            return movie;
        }


        private void AddEntity(Movie movie, HtmlNode node, string keyword)
        {
            var enumFields = System.Enum.GetValues(typeof(MovieEntityType));
               
            foreach(var field in enumFields)
            {
                bool matches = (typeof(MovieEntityType).GetMember(field.ToString())[0]
                    .GetCustomAttributes(typeof(KeyWordAttribute), true)
                    .First() as KeyWordAttribute).Matches(keyword);
              
               if(matches)
               {
                   MovieEntityType type = (MovieEntityType)field;
                   switch(type)
                   {
                       case MovieEntityType.DirectedBy:
                           movie.DirectedBy = new MovieEntity<string>(type, CreateMovieEntities<string>(node));
                           break;
                        case MovieEntityType.ProducedBy:
                            movie.ProducedBy = new MovieEntity<string>(type, CreateMovieEntities<string>(node));
                            break;
                        case MovieEntityType.WrittenBy:
                            movie.WrittenBy = new MovieEntity<string>(type, CreateMovieEntities<string>(node));
                            break;
                        case MovieEntityType.Starring:
                            movie.Starring = new MovieEntity<string>(type, CreateMovieEntities<string>(node));
                            break;
                        case MovieEntityType.MusicBy:
                            movie.MusicBy = new MovieEntity<string>(type, CreateMovieEntities<string>(node));
                            break;
                        case MovieEntityType.Cinematography:
                            movie.Cinematography = new MovieEntity<string>(type, CreateMovieEntities<string>(node));
                            break;
                        case MovieEntityType.EditingBy:
                            movie.EditingBy = new MovieEntity<string>(type, CreateMovieEntities<string>(node));
                            break;
                        case MovieEntityType.Studio:
                            movie.Studio = new MovieEntity<string>(type, CreateMovieEntities<string>(node));
                            break;
                        case MovieEntityType.DistributedBy:
                            movie.DistributedBy = new MovieEntity<string>(type, CreateMovieEntities<string>(node));
                            break;
                        case MovieEntityType.ScreenplayBy:
                            movie.ScreenplayBy = new MovieEntity<string>(type, CreateMovieEntities<string>(node));
                            break;
                        case MovieEntityType.Language:
                            {
                                movie.Language = new MovieEntity<Language>(type, Language.Malayalam /* CreateMovieEntities<string>(node)*/);
                            }
                            break;
                        case MovieEntityType.Country:
                            movie.Country = new MovieEntity<Country>(type, Country.India /*CreateMovieEntities<string>(node)*/);
                            break;
                        case MovieEntityType.Duration:
                            {
                                var results = RunningTimeRegex.Matches(node.InnerText);
                                if (results.Count > 0)
                                {
                                    var runningTime = 0;
                                    if (results.Count == 1)
                                    {
                                        runningTime = Int32.Parse(new Regex("[0-9]{1,3}").Match(results[0].Value).Value);
                                    }
                                    else if (results.Count >= 2)
                                    {
                                        runningTime = Int32.Parse(new Regex("[0-9]{1,3}").Match(results[0].Value).Value) * 60;
                                        runningTime += Int32.Parse(new Regex("[0-9]{1,3}").Match(results[1].Value).Value);
                                    }
                                    else
                                    {
                                        runningTime += Int32.Parse(new Regex("[0-9]{1,3}").Match(results[0].Value).Value);
                                    }
                                    movie.RunningTime = new MovieEntity<int>(MovieEntityType.Duration, runningTime);
                                }
                                else
                                    Debug.Assert(false);
                            }
                            break;
                        case MovieEntityType.ReleaseDate:
                            {
                                var date = HttpUtility.HtmlDecode(node.InnerText).Replace("\n",String.Empty);
                                var start = date.IndexOf('(');
                                if (start != -1)
                                    date = date.Remove(start);
                                date = date.Trim();
                                if (date.Length == 4)
                                {
                                    try
                                    {
                                        movie.ReleaseDate = new MovieEntity<DateTime>(MovieEntityType.ReleaseDate,
                                            new DateTime(Int32.Parse(date), 1, 1));
                                    }
                                    catch (Exception ex)
                                    {
                                        movie.ReleaseDate = new MovieEntity<DateTime>(MovieEntityType.ReleaseDate,
                                            new DateTime(movie.Year.Value, 1, 1));
                                    }
                                }
                                else
                                {
                                    DateTime res;
                                    if (DateTime.TryParse(date, out res))
                                        movie.ReleaseDate = new MovieEntity<DateTime>(MovieEntityType.ReleaseDate,res);
                                    else
                                        movie.ReleaseDate = new MovieEntity<DateTime>(MovieEntityType.ReleaseDate,
                                            new DateTime(movie.Year.Value, 1, 1));
                                }

                            }
                            break;
                       case MovieEntityType.NarratedBy:
                       case MovieEntityType.BoxOffice:
                       case MovieEntityType.BasedOn:
                            break;
                   }
                   return;
               }
            }
        }

        public List<Movie> GetMovies(List<MovieName> movieNames)
        {
            var movies = new List<Movie>();
            movieNames.ForEach(x => movies.Add(GetMovie(x)));
            return movies;

        }

        public IEnumerable<HtmlNode> GetNodesUsingCss(string url, string selector)
        {

            var rootNode = CreateWeb().Load(url).DocumentNode;
            return rootNode.CssSelect(selector);
        }

        public IEnumerable<HtmlNode> GetNodesUsingXPath(string url, string selector)
        {
            var rootNode = CreateWeb().Load(url).DocumentNode;
            return rootNode.SelectNodes(selector);
        }

        #endregion

        #region Private

        private HtmlWeb CreateWeb()
        {
            var web = new HtmlWeb();
            web.PreRequest = delegate(HttpWebRequest webRequest)
            {
                webRequest.Timeout = 30000;
                return true;
            };

            return web;
        }

        private MovieName CreateMovieName(string title, string wikiUrl, short year)
        {
            var name = title;
            var fullUrl = Uri.IsWellFormedUriString(wikiUrl, UriKind.Absolute);
            var url = fullUrl ?  wikiUrl : WikiRootUrl + wikiUrl ;
            var start = title.IndexOf("(");
            if (start != -1)
            {
                var end = title.IndexOf(")");
                if(end != -1)
                    name = title.Remove(start, end - start + 1);
            }

            return new MovieName()
            {
                WikiTitle = title,
                Name = name,
                IsValidUrl = !fullUrl,
                WikiUrl = url,
                Year = year
            };

        }

        private List<MovieEntityItem<T>> CreateMovieEntities<T>(HtmlNode node) 
        {
            var anchors = node.CssSelect("a");
            var tokens = node.InnerText.Replace("\n", ",").Split(',').Where(x => !String.IsNullOrWhiteSpace(x));
            var entities = new List<MovieEntityItem<T>>();
            foreach (var token in tokens)
            {
                var anchor = GetAnchorNode(token, anchors);
                if (anchor != null)
                {
                    entities.Add(Creator.CreateMovieEntityItem<T>(
                        Helper.ChangeType<T>(HttpUtility.HtmlDecode(anchor.InnerText.Trim())),
                        anchor.Attributes["href"].Value, LinkType.Wiki));
                }
                else
                {
                    entities.Add(Creator.CreateMovieEntityItem<T>(
                        Helper.ChangeType<T>(HttpUtility.HtmlDecode(token.Trim()))));
                }
            }

            foreach (var anchor in anchors)
            {
                if (entities.Where(x => x.Text.Trim() == anchor.InnerText.Trim()).Count() == 0)
                {
                    entities.Add(Creator.CreateMovieEntityItem<T>(
                       Helper.ChangeType<T>(anchor.InnerText.Trim()),
                       anchor.Attributes["href"].Value, LinkType.Wiki));
                }
            }
            
            return entities;
        }
        
        private HtmlNode GetAnchorNode(string text, IEnumerable<HtmlNode> nodes)
        {
            return nodes.Where(x => x.InnerText.Trim() == text.Trim()).FirstOrDefault();
        }

        private int FindMovieCell(List<HtmlNode> rowCells)
        {
            for (var i = 0; i < rowCells.Count; i++)
            {
                var cell = rowCells[i];
                var child = cell.FirstChild;
                if (child != null && child.Name == "i")
                    return i;
            }
            return -1;
        }

        private int FindMovieTitleColumnIndex(List<HtmlNode> colHeads)
        {
           
           var index = 0;
           for (var i = 0; i < colHeads.Count; i++)
           {
               var head = colHeads[i];
               if (head.Attributes.Contains("colspan"))
                   index += Int32.Parse(head.Attributes["colspan"].Value);
               else
                   index++;

               string header = head.LastChild.InnerHtml.ToLower();
               if (FilmColumnTitles.Contains(header))
                   return index;
           }

            return -1;
        }

        private int GetColumnHeaderCount(List<HtmlNode> colHeads)
        {
            var index = 0;
            for (var i = 0; i < colHeads.Count; i++)
            {
                var head = colHeads[i];
                if (head.Attributes.Contains("colspan"))
                    index += Int32.Parse(head.Attributes["colspan"].Value);
                else
                    index++;
            }

            return index;
        }

        private void ThrowMovieInfoNotFoundException(MovieName mn)
        {
            throw new Exception(String.Format("Movie details not found. Title: {0}, Url: {1}",
                   mn.WikiTitle, mn.WikiUrl));
           
        }

        #endregion

    }
}
