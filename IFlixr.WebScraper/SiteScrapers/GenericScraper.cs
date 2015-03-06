using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsQuery;
using IFlixr.Base;
using System.Diagnostics;

namespace IFlixr.WebScraper
{
    public class GenericScraper
    {
        public void Crawl()
        {
            var movies = new List<ScrappedMovie>();
            var dom = CQ.CreateFromUrl("http://www.hindilinks4u.net/tag/2013");
            var a = dom.Select("html body div#container div#entries ul.result li div.results_content a.archive-permalink");
            var elems = a.Elements.ToList();
            foreach (var elem in elems)
            {
                dom = CQ.CreateFromUrl(elem.Attributes["href"]);

                var sm = new ScrappedMovie()
                {
                    Title = dom.Select("html body div#container div#entries h2").Text(),
                    ImageUrl = dom.Select("html body div#container div#entries div.post p a img.alignnone").Attr("src")
                };
                movies.Add(sm);
                
                
                var paras = dom.Select("html body div#container div#entries div.post p");
                foreach(var p in paras)
                
                {
                    
                    var links = p.ChildElements.Where(x => x.HasClass("external") && x.NodeName == "A")
                        .Select(x => x.Attributes["href"]);
                    if(links.Count() == 0)
                        continue;
                    var sources = new List<string>();
                    bool ignoreHost = false;
                    foreach (var link in links)
                    {
                        var host = VideoHostParser.FindSource(link);
                        if (host != VideoSource.Unknown)
                        {
                            sources.Add(link);
                            continue;
                        }
                        else if (VideoHostParser.IgnoreHost(link))
                        {
                            ignoreHost = true;
                            break;
                        }

                        dom = CQ.CreateFromUrl(link);
                        var srcs = dom.Select("iFrame").Select(x => x.Attributes["src"]).ToList();
                        foreach (var src in srcs)
                        {
                            host = VideoHostParser.FindSource(src);
                            if (host != VideoSource.Unknown)
                            {
                                sources.Add(src);
                                break;
                            }
                        }
                        if (sources.Count == 0)
                        {
                            srcs = dom.Select("object param").Select(x => x.Attributes["value"]).ToList();
                            foreach (var src in srcs)
                            {
                                host = VideoHostParser.FindSource(src);
                                if (host != VideoSource.Unknown)
                                {
                                    sources.Add(src);
                                    break;
                                }
                            }
                        }
                    }
                    if( sources.Count > 0)
                    {
                        var sv = new ScrappedVideo{ Source = VideoHostParser.FindSource(sources[0])};
                        foreach (var s in sources)
                            sv.Parsers.Add(new VideoHostParser(s));
                        sm.Videos.Add(sv);
                    }
                }

                Debug.WriteLine("Movie: {0}, Count: {1}", sm.Title, sm.Videos.Count);
            }
        }

        public class ScrappedMovie
        {
            public ScrappedMovie()
            {
                Videos = new List<ScrappedVideo>();
            }
            public string Title { get; set; }
            public string ImageUrl { get; set; }
            public List<ScrappedVideo> Videos { get; set; }
        }

        public class ScrappedVideo
        {
            public ScrappedVideo ()
	        {
               Parsers = new List<VideoHostParser>();
            }
            public VideoSource Source { get; set; }
            public List<VideoHostParser> Parsers { get; set; }
        }
    }
}
