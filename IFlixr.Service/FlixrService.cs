using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.ViewModel;
using IFlixr.ViewModel.Base;
using System.IO;
using System.Web;
using IFlixr.Data;
using IFlixr.Base;
using System.Data.Objects.SqlClient;
using System.Web.Mvc;
using System.Diagnostics;

namespace IFlixr.Service
{
    public class FlixrService //: IFlixrService
    {
        public FlixrService()
        {

        }

        #region Public

        public BannerModel GetHomeScreenBanner(Language language, EntertainmentCategory category)
        {
            using (var context = new IFlixrContext())
            {
                return new BannerModel()
                {
                    Items = context.Banners
                                   .Where(x => (language == Language.Any || x.Language == (byte) language) && 
                                          (category == EntertainmentCategory.Any || x.Category == (byte)category))
                                   .Select(x => new BannerItem(){
                                            ImageUrl = "Images/Banner/" +  x.ImageName,
                                            Title = x.Title,
                                            Description = x.Text,
                                            LinkUrl = x.Link
                                        }).ToList()
                                         
                                   
                };
            }

        }


        public HomePageOld GetHomePage()
        {
            var page = new HomePageOld()
            {
                TopBanner = GetHomeScreenBanner(Language.Malayalam, EntertainmentCategory.Movie),
                DynamicPart = GetDynamicLayout(Language.Malayalam, EntertainmentCategory.Movie)
            };
            return page;
        }

        public HoverBoxModel GetHoverBox(int showId)
        {
            using (var ctx = new IFlixrContext())
            {
                var model = new HoverBoxModel();
                var movie = ctx.Movies.FirstOrDefault(x => x.Id == showId);
                if (movie == null)
                    model.Title = "No Informaton Available";
                else
                {
                    model.Title = movie.Title;
                    model.ReleaseDate = movie.ReleaseData;
                    model.Language = (Language)movie.Language;
                    model.Rating = "Family";
                    model.Duration = movie.RunningTime;
                    model.Footer = String.Format("{0}{1}", 
                        movie.Starring.Substring(0, movie.Starring.Length > 60 ? 60 : 
                        movie.Starring.Length), movie.Starring.Length > 60 ? "..." : String.Empty)
                        .Replace(",",", ");
                    model.Description = new MvcHtmlString(String.Format("{0}<a href=\"/movies/{1}\"> ...more</a>",
                        (!String.IsNullOrWhiteSpace(movie.Description) ?
                        movie.Description.Substring(0, (movie.Description.Length > 240 ? 240 : movie.Description.Length)) :
                        "Not description available"), showId));
                }

                return model;
            }
        }


        public DynamicLayout GetDynamicLayout(Language language, EntertainmentCategory category)
        {
            using(var ctx = new IFlixrContext())
            {
                var layout = new DynamicLayout()
                {
                    Carousels = ctx.Carousels
                                    .Where(x => (language == Language.Any || x.Language == (byte) language) && 
                                          (category == EntertainmentCategory.Any || x.Category == (byte)category)
                                          && x.Active)
                                    .Select(x => new CarouselModel()
                                    {
                                        Id =  SqlFunctions.StringConvert((double)x.Id,1),
                                        Title = x.Title,
                                        Size = (CarouselSize) x.Size,
                                        Type = (CarouselType) x.Type,
                                        SizeCSSClass = "medium",
                                        Items = x.CaroselItems
                                                .Where(y => y.Active)                                                  
                                                .Select(y => new CarouselItemModel()
                                                {
                                                    Id = y.ShowId,
                                                    ImageUrl =  y.Show.Images.Where(z => z.IsDefault).FirstOrDefault().FullPath,
                                                    Title = y.Show.Title,
                                                    Year = y.Show.Year
                                                })
                                    }).ToList()
                };

                layout.Carousels.ForEach(x => 
                {
                    x.Items.ToList()
                    .ForEach(y => 
                    {
                        y.ModuleSizeCSSClass = "medium";
                        y.DisplayHoverBox = true;
                        y.NavigationLink = CreateShowUrl(y, language);
                        if (!String.IsNullOrWhiteSpace(y.ImageUrl))
                            y.ImageUrl = GlobalSettings.MovieImageUrl + y.ImageUrl;
                        else
                            y.ImageUrl = GlobalSettings.PlaceHolderMoviePoster;
                    });
                });
                return layout;
            }
 
        }

        public MoviePageOld GetMoviePage(int id)
        {
            using (var ctx = new IFlixrContext())
            {
                Data.Movie movie = ctx.Movies.FirstOrDefault(x => x.Id == id);
                Debug.Assert(movie != null);

                var children = movie.Children.ToList();
                var yVideo = children
                    .Where(x => x.Type == (byte)VideoType.Trailer)
                    .Select(x => x.VideoLinks.FirstOrDefault())
                    .FirstOrDefault();
                   
                var moviePage = new MoviePageOld()
                {
                    Title = movie.Title,
                    Country = (Country)movie.Country,
                    Duration = movie.RunningTime,
                    Language = (Language)movie.Language,
                    Description = String.IsNullOrWhiteSpace(movie.Description) ? "No description available" : movie.Description,
                    Rating = "Family",
                    ReleaseDate = movie.ReleaseData,
                    Starring = movie.Starring,
                    ProducedBy = movie.ProducedBy,
                    DirectedBy = movie.DirectedBy,
                    CinematographyBy = movie.CinematographyBy,
                    DistributedBy = movie.DistributedBy,
                    EditingBy = movie.EditingBy,
                    MusicBy = movie.MusicBy,
                    ScreenplayBy = movie.ScreenplayBy,
                    SongsBy = movie.SongsBy,
                    Studio = movie.Studio,
                    WrittenBy = movie.WrittenBy,
                    MovieLink = movie.UniqueToken,
                    TrailerLink = yVideo != null ? yVideo.Url : null,
                    TrailerImageLink = yVideo != null ? String.Format("http://img.youtube.com/vi/{0}/0.jpg", yVideo.UniqueToken) : null,
                    MovieLinkText = "Wikipedia",

                    ImageLink = GlobalSettings.MovieImageUrl + movie.Images.FirstOrDefault(x => x.IsDefault).FullPath,
                };


                var otherVideos = new CarouselModel()
                {
                    Id = "0",
                    Title = "Other Videos",
                    Size = CarouselSize.Youtube,
                    Type =  CarouselType.Image,
                    SizeCSSClass = "small youtube",
                    Items = children
                            .Where(x => x.Type != (byte)VideoType.Movie)
                            .Select(y => new CarouselItemModel()
                            {
                                Id = y.Id,
                                ImageUrl = String.Format("http://img.youtube.com/vi/{0}/1.jpg", y.VideoLinks.FirstOrDefault().UniqueToken),
                                Title = y.Title,
                                Year = y.Year,
                                ModuleSizeCSSClass = "small youtube",
                                NavigationLink = y.VideoLinks.FirstOrDefault().Url
                            }).ToList()
                };
                moviePage.DynamicPart.Carousels.Add(otherVideos);
                moviePage.DynamicPart.Carousels.Add(GetOne(1, "You might also like..", "small"));
                moviePage.DynamicPart.Carousels.Add( GetOne(2, "Popular Movies", "small"));
                moviePage.DynamicPart.Carousels.Add( GetOne(3, "Popular Malayalam Movies","small"));
                return moviePage;
            }
        }

        public MovieBrowsePageOld GetMovieBrowsePage(string id)
        {
            var imageSizeClass = "thumbnail";
            var files = Directory.EnumerateFiles(HttpContext.Current.Server.MapPath("~/Images/Posters"),
                "*", SearchOption.AllDirectories).Select(Path.GetFileName).ToList();
            return new MovieBrowsePageOld()
            {
                Content = new SearchResult()
                {
                    Items = new List<CarouselItemModel>{
                          GetImageModule(files[0], imageSizeClass),
                            GetImageModule(files[1], imageSizeClass),
                            GetImageModule(files[2], imageSizeClass),
                            GetImageModule(files[3], imageSizeClass),
                            GetImageModule(files[4], imageSizeClass),
                            GetImageModule(files[5], imageSizeClass),
                            GetImageModule(files[6], imageSizeClass),
                            GetImageModule(files[7], imageSizeClass),
                            GetImageModule(files[8], imageSizeClass),
                            GetImageModule(files[9], imageSizeClass),
                    }

                }

            };
        }

        #endregion

        #region Private

        private string CreateShowUrl(CarouselItemModel show, Language language)
        {
            return String.Format("{0}{1}/{2}/{3}/{4}",
                GlobalSettings.MovieRootUrl,
                ((Language)language).ToString().ToLower(),
                show.Year,show.Id, Uri.EscapeDataString(show.Title.Trim())).Trim();
        }

        private CarouselModel GetOne(int index, string title, string imageSizeClass)
        {
            var files = Directory.EnumerateFiles(HttpContext.Current.Server.MapPath("~/Images/Posters"),
                "*", SearchOption.AllDirectories).Select(Path.GetFileName).ToList();


            return
                 new CarouselModel()
                 {
                     Id = "JustRandom_" + index.ToString(),
                     MinItems = 1,
                     MaxItems = 10,
                     Title = title,
                     Size = CarouselSize.Large,
                      SizeCSSClass = imageSizeClass,
                     Items = new List<CarouselItemModel>(){
                            GetImageModule(files[0], imageSizeClass),
                            GetImageModule(files[1], imageSizeClass),
                            GetImageModule(files[2], imageSizeClass),
                            GetImageModule(files[3], imageSizeClass),
                            GetImageModule(files[4], imageSizeClass),
                            GetImageModule(files[5], imageSizeClass),
                            GetImageModule(files[6], imageSizeClass),
                            GetImageModule(files[7], imageSizeClass),
                            GetImageModule(files[8], imageSizeClass),
                            GetImageModule(files[9], imageSizeClass),
                            
                         }
                 };
        }
        private Random rand = new Random();
        private CarouselItemModel  GetImageModule(string file, string sizeClassName)
        {
            return new CarouselItemModel()
            {
                ImageUrl = "/images/posters/" + file,
                NavigationLink = "/movie/" + Path.GetFileNameWithoutExtension(file),
                DisplayLanguage = sizeClassName != "medium",
                DisplayYear = sizeClassName != "medium",
                ModuleSizeCSSClass = sizeClassName
            };
        }

        #endregion


       
    }
}
