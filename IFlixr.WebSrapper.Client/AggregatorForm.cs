using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IFlixr.WebScraper;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using AutoMapper;
using IFlixr.WebScraper.Client.Model;
using IFlixr.Data;
using System.Data.Entity.Infrastructure;
using IFlixr.Data.Scraper;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using IFlixr.Base;
using System.Threading;


namespace IFlixr.WebScraper.Client
{
    public partial class AggregatorForm : Form
    {
        #region Fields

        private MovieModel selectedMovieModel;
        private List<MovieModel> movieCollection = new List<MovieModel>();
        private delegate void AddImageToSearchGridDelegate(string filePath);
        private static readonly Regex MoviePartIdentifier = new Regex(@"([^\d]|^)\d{1,2}([^\d]|$)");

       
        
        #endregion

        #region Constructor

        public AggregatorForm()
        {
            InitializeComponent();
            Init();
        }

        #endregion

        #region Private

        private void Init()
        {
            this.comboBoxYear.Items.AddRange(new string[] { "2012","2011","2010","2009", "2008", "2007", "2006", "2005" });
            this.comboBoxLanguage.Items.AddRange(new string[] { "Malayalam" });
            this.comboBoxYear.SelectedIndex = this.comboBoxLanguage.SelectedIndex = 0;


            MethodInfo getSyntax = typeof(UriParser).GetMethod("GetSyntax", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            FieldInfo flagsField = typeof(UriParser).GetField("m_Flags", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (getSyntax != null && flagsField != null)
            {
                foreach (string scheme in new[] { "http", "https" })
                {
                    UriParser parser = (UriParser)getSyntax.Invoke(null, new object[] { scheme });
                    if (parser != null)
                    {
                        int flagsValue = (int)flagsField.GetValue(parser);
                        // Clear the CanonicalizeAsFilePath attribute
                        if ((flagsValue & 0x1000000) != 0)
                            flagsField.SetValue(parser, flagsValue & ~0x1000000);
                    }
                }
            }

            this.selectedMovieIndexColumn.DataSource = IndexChoice.Choices;
            this.selectedMovieSequenceColumn.DataSource = IndexChoice.Choices;
            this.selectedMusicIndexColumn.DataSource = IndexChoice.Choices;
            this.selectedMusicSequenceColumn.DataSource = IndexChoice.Choices;
            this.selectedTrailorIndexColumn.DataSource = IndexChoice.Choices;
            this.selectedTrailorSequenceColumn.DataSource = IndexChoice.Choices;
            this.selectedClipIndexColumn.DataSource = IndexChoice.Choices;
            this.selectedClipSequenceColumn.DataSource = IndexChoice.Choices;
            this.selectedClipTypeColumn.DataSource = Enum.GetValues(typeof(VideoType));
            this.dgvMovieVideoType.DataSource = Enum.GetValues(typeof(VideoType));
            this.dgvMusicVideoType.DataSource = Enum.GetValues(typeof(VideoType));
            this.dgvTrailerVideoType.DataSource = Enum.GetValues(typeof(VideoType));
            this.dgvClipVideoType.DataSource = Enum.GetValues(typeof(VideoType));
            Scraper.LastFetchTime = DateTime.Now;
            
        }

        private MovieGridItem GetSelectedItem()
        {
            var row = this.dataGridViewMovies.SelectedRows[0];
           return this.dataGridViewMovies.Rows[row.Index].DataBoundItem as MovieGridItem;
        }


        private SortableBindingList<VideoModel> Scrap(string searchText, string keyword,
            VideoType searchType,
            string uploader = "",
            VideoSource source = VideoSource.Any)
        {
            List<VideoInfo> result = null;
            SortableBindingList<VideoModel> items = null;

            var crawler = new YouTubeScraper();
            switch (searchType)
            {
                case VideoType.Movie:
                    result = Scraper.ScrapMovies(new QueryParam()
                    {
                        FullSearhText = searchText,
                        Keyword = keyword,
                        Uploader = uploader,
                        Source = source
                    });
                    items = Mapper.Map<List<VideoInfo>, SortableBindingList<VideoModel>>(result);
                    items.Each(x => x.Type = VideoType.Movie);
                    break;
                case VideoType.Music:
                    result = crawler.GetMusic(searchText, keyword, uploader);
                    items = Mapper.Map<List<VideoInfo>, SortableBindingList<VideoModel>>(result);
                    items.Each(x => x.Type = VideoType.Music);
                    break;
                case VideoType.Trailer:
                    result = crawler.GetTrailors(searchText, keyword, uploader);
                    items = Mapper.Map<List<VideoInfo>, SortableBindingList<VideoModel>>(result);
                    items.Each(x => x.Type = VideoType.Trailer);
                    break;
                case VideoType.Clip:
                    result = crawler.GetTrailors(searchText, keyword, uploader);
                    items = Mapper.Map<List<VideoInfo>, SortableBindingList<VideoModel>>(result);
                    items.Each(x => x.Type = VideoType.Clip);
                    break;
            }

            return items;
        }
        private void SearchVideo(string searchText, string keyword, 
            DataGridView grid,VideoType searchType, 
            SortableBindingList<VideoModel> currentItems, 
            string uploader = "",
            VideoSource source = VideoSource.Any)
        {
            SetProgressStatus(true, searchType.ToString() +  " video");
            var items = Scrap(searchText,keyword,searchType,uploader,source); 
            if(currentItems != null)
                currentItems.Each(x => 
                { 
                    var item = items.FirstOrDefault(y => y.VideoId == x.VideoId);
                    if (item != null)
                        item.Selected = true;
                    else
                    {
                        var newItem = x.Clone();
                        newItem.Selected = true;
                        items.Add(newItem);
                    }
                });

            var existingItems = grid.DataSource as SortableBindingList<VideoModel>;
            if (existingItems != null)
            {
                existingItems.Each(x => 
                { 
                    if(!items.Any(y => y.VideoId == x.VideoId))
                        items.Add(x);
                });
            }

            grid.DataSource = new SortableBindingList<VideoModel>(items.OrderByDescending(x => x.PlaylistId)
                .ThenBy(x => x.PlaylistSequence)
                .ThenBy(x => x.Uploader)
                .ThenByDescending(x => x.DurationInMinutes));
                
           
            SetProgressStatus(false, "video");
        }

        private void SearchImage()
        {
            var item = GetSelectedItem();
            if (item != null)
            {
                SetProgressStatus(true, "images");
                this.flowLayoutPanelImages.Controls.Clear();
                DoSearchImage(item.Name,this.textBoxImageSearch.Text, true, (int)this.numericUpDownImage.Value);
                SetProgressStatus(false, "images");
            }
        }

        private void DoSearchImage(string name, string searchText, bool updateUI, int count = 20)
        {
            DirectoryInfo di = new DirectoryInfo(Path.Combine(Application.StartupPath, Helper.RootImagePath));
            if (!di.Exists)
                di.CreateDirectory();
            di = new DirectoryInfo(Path.Combine(Application.StartupPath, Helper.RootImageTempPath));
            if (!di.Exists)
                di.CreateDirectory();

            ImageScraper scraper = new ImageScraper();
            var imgLinks = scraper.GetImageLinks(searchText);

            var relativePath = name.Replace(":", "_").Replace(" ", "_");
            di = new DirectoryInfo(Path.Combine(di.FullName, relativePath));
            if (!di.Exists)
                di.CreateDirectory();
            else if (di.GetFiles().Count() > 15)
                return;

            using (var context = new ScraperContext())
            {
                Dictionary<string, string> imageKeyValues = new Dictionary<string, string>();

                for (var i = 0; i < imgLinks.Count; i++)
                {
                    var link = imgLinks[i];
                    var data = context.TempDatas.SingleOrDefault(x => x.Key.Equals(link,
                        StringComparison.InvariantCultureIgnoreCase));
                    var imagePath = di.FullName;
                    if (data != null)
                    {
                        if (data.Skip)
                            continue;
                        imagePath = Path.Combine(di.FullName, data.Value);
                    }


                    if (!File.Exists(imagePath))
                    {
                        var startIndex = link.LastIndexOf(".") + 1;
                        var extension = link.Substring(startIndex, link.Length - startIndex);
                        var imageName = GetUniqueImageFileName(name, extension, di);
                        imagePath = Path.Combine(di.FullName, imageName);
                    }
                    else if(updateUI)
                    {
                        AddImageToSearchGrid(imagePath);
                    }

                    imageKeyValues.Add(link, imagePath);

                }
                var downlodList = imageKeyValues
                    .Where(x => x.Key.Length < 512 && !File.Exists(x.Value))
                    .Take(count)
                    .ToList();
                Parallel.ForEach(downlodList, x =>
                {
                    try
                    {
                        bool success = scraper.DownloadImage(x.Key, x.Value);
                        if (success)
                        {
                            if (File.Exists(x.Value) && updateUI)
                                AddImageToSearchGrid(x.Value);
                        }
                        else
                            Debug.WriteLine("Could not download : " + x.Key);

                        lock (context)
                        {
                            var dbItem = context.TempDatas.FirstOrDefault(y => x.Key == y.Key);
                            if (dbItem == null)
                                context.TempDatas.Add(new TempData() { Key = x.Key, Value = Path.GetFileName(x.Value), Skip = !success });
                            else
                            {
                                dbItem.Value = Path.GetFileName(x.Value);
                                dbItem.Skip = !success;
                            }

                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error while saving. Exception: " + ex.Message);
                    }

                });

            }
        }

        private void AddImageToSearchGrid(string filePath)
        {
            if (!InvokeRequired)
            {
                var box = new SelectablePictureBox(new ImageModel() { FullPath = filePath }, false);
                this.flowLayoutPanelImages.Controls.Add(box);
                Application.DoEvents();
            }
            else
            {
                var del = new AddImageToSearchGridDelegate(AddImageToSearchGrid);
                BeginInvoke(del, filePath);
            }
        }

        private string GetUniqueImageFileName(string prefix, string extension, DirectoryInfo dir)
        {
            while (true)
            {
               var fileName = prefix.Replace(":", "_").Replace(" ", "_") + "_" + Guid.NewGuid() + "." + extension;
                if(!File.Exists(Path.Combine(dir.FullName, fileName)))
                    return fileName;
            }
           
        }

        private void SetProgressStatus(bool start, string message)
        {
            if (start)
            {
                this.labelProgress.Text = "Loading " + message;
                this.labelProgress.Font = new Font(FontFamily.GenericSansSerif, 15, FontStyle.Bold);
                this.labelProgress.ForeColor = Color.Red;
                Application.DoEvents();
            }
            else
            {
                this.labelProgress.Font = new Font(SystemFonts.DefaultFont, FontStyle.Regular);
                this.labelProgress.Text = "Loading Completed";
                this.labelProgress.ForeColor = Color.Black;
            }
        }

        private void SetSearchFields(MovieGridItem item = null)
        {
            if (item == null)
            {
                var row = this.dataGridViewMovies.SelectedRows[0];
                item = (MovieGridItem)row.DataBoundItem;
            }

            var name = item.Name.Replace(".", " ");
            this.textBoxImageSearch.Text = name + " malayalam movie";
            this.textBoxMovieSeach.Text = name;
            this.textBoxMoviePostfix.Text = "malayalam movie";
            this.textBoxMusicSearch.Text = name;
            this.textBoxMusicPostfix.Text = "malayalam movie song";
            this.textBoxOtherSearch.Text = name;
            this.textBoxTrailerPostfix.Text = "malayalam movie trailer";
        }

        private void AddMovies(SortableBindingList<VideoModel> movies)
        {
            var videoList = CreateVideoList( VideoType.Movie, movies, 
                this.dataGridViewSelectedMovies.DataSource as SortableBindingList<VideoModel>, true);
            this.dataGridViewSelectedMovies.DataSource = videoList;
        }

        private void LoadMovies(SortableBindingList<VideoModel> movies)
        {
            var videoList = CreateVideoList(VideoType.Movie, movies,
                this.dataGridViewSelectedMovies.DataSource as SortableBindingList<VideoModel>, false);
            this.dataGridViewSelectedMovies.DataSource = videoList;
            var search = this.dataGridViewMovieVideos.DataSource as SortableBindingList<VideoModel>;
            if (search != null)
                search.AddRange(videoList.Where(x => !search.Any(y => x.VideoId == y.VideoId)));
            this.dataGridViewMovieVideos.Refresh();
        }

        private void AddMusic(SortableBindingList<VideoModel> music)
        {
            var videoList = CreateVideoList( VideoType.Music, music,
                this.dataGridViewSelectedMusic.DataSource as SortableBindingList<VideoModel>, true);
            this.dataGridViewSelectedMusic.DataSource = videoList;
        }

        private void LoadMusic(SortableBindingList<VideoModel> music)
        {
            var videoList = CreateVideoList(VideoType.Music, music,
                this.dataGridViewSelectedMusic.DataSource as SortableBindingList<VideoModel>, false);
            this.dataGridViewSelectedMusic.DataSource = videoList;
        }

        private void AddTrailers(SortableBindingList<VideoModel> trailors)
        {
            var videoList = CreateVideoList( VideoType.Trailer,trailors, 
                this.dataGridViewSelectedTrailors.DataSource as SortableBindingList<VideoModel>, true);
            this.dataGridViewSelectedTrailors.DataSource = videoList;
        }

        private void LoadTrailers(SortableBindingList<VideoModel> trailers)
        {
            var videoList = CreateVideoList(VideoType.Trailer, trailers,
                this.dataGridViewSelectedTrailors.DataSource as SortableBindingList<VideoModel>, false);
            this.dataGridViewSelectedTrailors.DataSource = videoList;
        }

        private void AddClips(SortableBindingList<VideoModel> clips)
        {
            var videoList = CreateVideoList(VideoType.Clip, clips,
                this.dataGridViewSelectedClips.DataSource as SortableBindingList<VideoModel>, true);
            videoList.Each(x => { if (((byte)x.Type) < (byte)VideoType.Clip)x.Type = VideoType.Clip; });
            this.dataGridViewSelectedClips.DataSource = videoList;
        }

        private void LoadClips(SortableBindingList<VideoModel> clips)
        {
            var videoList = CreateVideoList(VideoType.Clip, clips,
                this.dataGridViewSelectedClips.DataSource as SortableBindingList<VideoModel>, false);
            this.dataGridViewSelectedClips.DataSource = videoList;
        }

        private void AddImages(IEnumerable<string> imagesFiles)
        {
            foreach (var imgPath in imagesFiles)
            {
                var model = new ImageModel() { FullPath = imgPath, SupportedDimensions = ( (int)ImageDimension.Medium_W220XH320 
                    | (int)ImageDimension.Small_W120XH174 | (int) ImageDimension.Thumb_W120XH174) };
                var box = new SelectablePictureBox( model, true);
                this.flowLayoutPanelSelectedImages.Controls.Add(box);
            }
        }

        private void LoadImages(IEnumerable<ImageModel> images)
        {
            foreach (var img in images)
            {
                if (File.Exists(img.FullPath))
                {
                    var box = new SelectablePictureBox(img, true);
                    this.flowLayoutPanelSelectedImages.Controls.Add(box);
                }
            }
        }

        private SortableBindingList<VideoModel> CreateVideoList(VideoType type, SortableBindingList<VideoModel> source, 
            SortableBindingList<VideoModel> dest, bool createIndex)
        {
            if (source == null)
                return new SortableBindingList<VideoModel>(); 
            if (dest == null)
                dest = new SortableBindingList<VideoModel>();

            //var index = 1;
            //var last = dest.LastOrDefault();
            //if (last != null)
            //    index = last.Index + 1;
           
            var newItems = Mapper.Map<IEnumerable<VideoModel>,
                SortableBindingList<VideoModel>>(source.Where(x => (x.Selected || !createIndex) &&
                !dest.Any(y => y.Url.Equals(x.Url, StringComparison.InvariantCultureIgnoreCase)))
                .Select(x => new VideoModel()
                {
                    Duration = x.Duration,
                    Url = x.Url,
                    Title = x.Title,
                    Uploader = x.Uploader,
                    PublishedDate = x.PublishedDate,
                    VideoId = x.VideoId,
                    Index = x.Index,
                    PlaylistSequence = x.PlaylistSequence,
                    Source = x.Source,
                    Type = x.Type,
                    PlaylistId = x.PlaylistId
                
                }));

            if (createIndex && type == VideoType.Movie)
            {
                var index = 1;
                var last = dest.LastOrDefault();
                if (last != null)
                    index = last.Index + 1;

               
                var plGroups = newItems.GroupBy(x => x.PlaylistId)
                    .Where(x =>!String.IsNullOrWhiteSpace(x.Key));
                foreach (var group in plGroups)
                {
                   var items = group.Select(x => x); 
                   items.Each(x => x.Index = index);
                   dest.AddRange(items);
                   newItems.RemoveRange(items);
                   index++;
                }

                var groups = newItems.GroupBy(x => x.Uploader);
                foreach (var group in groups)
                {                   
                    var items = group.Select(x => x); 
                    foreach (var item in items)
                    {
                        item.Index = index;
                        item.PlaylistSequence = group.Count() == 1 ? 0 : FindFirstNumber(item.Title);
                    }
                    index++;
                }
            }

           dest.AddRange(newItems);
           return new SortableBindingList<VideoModel>(dest.OrderBy(x => x.Index).ThenBy(x => x.PlaylistSequence));
        }

        private IEnumerable<VideoModel> OrderMovieParts(IEnumerable<VideoModel> videos)
        {
            if (videos.Count() < 2)
                return videos;

            var orderedList = new  VideoModel[videos.Count()];
            foreach (var video in videos)
            {
                int part = FindFirstNumber(video.Title);
                if (part == -1 || orderedList.Length <= part - 1 || orderedList[part - 1] != null)
                    return videos.OrderBy(x => x.PublishedDate);
                orderedList[part - 1] = video;
            }

            return orderedList;

        }

        private int FindFirstNumber(string text)
        {
            text = text.Remove(0, 4);
            var match = MoviePartIdentifier.Match(text);
            if (match.Success)
                return Int32.Parse(new Regex("[0-9]{1,2}").Match(match.Value).Value);

            return 0;


            //var numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            //text = text.Remove(0, 3);
            //var index = text.IndexOfAny(numbers);
            //if( index != -1)
            //{
            //    var number = "" + text[index];
            //    var prevChar = text[index - 1];
            //    if (text.Length > index + 1 )
            //    {
            //        if (Char.IsNumber(text[index + 1]))
            //        {
            //            number += text[index + 1];
            //            index++;
            //        }
            //    }
            //    else
            //    {
            //        if ((!Char.IsNumber(prevChar) || prevChar == '0'))
            //            return Int32.Parse(number);
            //    }

            //    var nextChar = index < text.Length - 1 ? text[index + 1] : 'a';

            //    if ((!Char.IsNumber(prevChar) || prevChar == '0') && !Char.IsNumber(nextChar))
            //        return Int32.Parse(number);
            //}
           

            //return -1;
        }

        private MovieModel FindMovieModel(string wikiUrl)
        {
            return this.movieCollection.FirstOrDefault(x => wikiUrl.Equals(x.Movie.WikiUrl,
                StringComparison.InvariantCultureIgnoreCase));
        }

        private void SaveMovieInMemory(MovieModel previous)
        {
            if (previous != null)
            {
                var movieModel = FindMovieModel(previous.Movie.WikiUrl);
                
                //Movie videos
                movieModel.Videos.Clear();
                var videoList = this.dataGridViewSelectedMovies.DataSource as SortableBindingList<VideoModel>;
                if (videoList != null && videoList.Count > 0)
                {
                    videoList.Each(x => x.Type = VideoType.Movie);
                    int duration = videoList.Where(x => x.Index == 1).Sum(x => x.DurationInMinutes);
                    if (movieModel.Movie.RunningTime.Value == 0)
                        movieModel.Movie.RunningTime = new MovieEntity<int>(MovieEntityType.Duration, duration);
                    
                    movieModel.Videos.AddRange(videoList);
                }

                //Music Videos 
                videoList = this.dataGridViewSelectedMusic.DataSource as SortableBindingList<VideoModel>;
                movieModel.Music.Clear();
                if (videoList != null)
                {
                    videoList.Each(x => x.Type = VideoType.Music);
                    foreach (var video in videoList)
                    {
                        var item = movieModel.Music.Where(x => x.Videos.Any(y => y.Index == video.Index)).SingleOrDefault();
                        if (item != null)
                            item.Videos.Add(video);
                        else
                            movieModel.Music.Add(new MusicModel(video.Title, video.VideoId, movieModel.Year) 
                            { Videos = new SortableBindingList<VideoModel>() { video } });
                    }
                }

                //trailer
                videoList = this.dataGridViewSelectedTrailors.DataSource as SortableBindingList<VideoModel>;
                movieModel.Trailor.Clear();
                if (videoList != null)
                {
                    videoList.Each(x => x.Type = VideoType.Trailer);
                    foreach (var video in videoList)
                    {
                        var item = movieModel.Trailor.Where(x => x.Videos.Any(y => y.Index == video.Index)).SingleOrDefault();
                        if (item != null)
                            item.Videos.Add(video);
                        else
                            movieModel.Trailor.Add(new TrailorModel(video.Title, video.VideoId, movieModel.Year) 
                            { Videos = new SortableBindingList<VideoModel>() { video } });
                    }
                }

                //clips
                videoList = this.dataGridViewSelectedClips.DataSource as SortableBindingList<VideoModel>;
                movieModel.Clip.Clear();
                if (videoList != null)
                {
                    //videoList.Each(x => x.Type = Model.VideoType.Trailer);
                    foreach (var video in videoList)
                    {
                        var item = movieModel.Clip.Where(x => x.Videos.Any(y => y.Index == video.Index)).SingleOrDefault();
                        if (item != null)
                            item.Videos.Add(video);
                        else
                            movieModel.Clip.Add(new ClipModel(video.Title, video.VideoId, video.Type, movieModel.Year) 
                            { Videos = new SortableBindingList<VideoModel>() { video } });
                    }
                }

                //Images
                movieModel.Images.Clear();
                foreach (SelectablePictureBox box in this.flowLayoutPanelSelectedImages.Controls)
                {
                    movieModel.Images.Add(new ImageModel()
                    {
                        Height = box.ImageHeight,
                        Width = box.ImageWidth,
                        SupportedDimensions = GetSupportedImageDimensions(box),
                        FullPath = box.FileName,
                        IsDefault = box.Default
                    });
                }

                SaveResults();
            }
        }

        private int GetSupportedImageDimensions(SelectablePictureBox pBox)
        {
            int dimensions = pBox.Medium ? (int)ImageDimension.Medium_W220XH320 : 0;
            dimensions = pBox.Small ? dimensions | (int)ImageDimension.Small_W120XH174 : dimensions;
            dimensions = pBox.Thumbnail ? dimensions | (int)ImageDimension.Thumb_W120XH174 : dimensions;
            return dimensions == 0 ? (int)ImageDimension.Medium_W220XH320 : dimensions;
        }

        private void RemoveItems(DataGridView view)
        {
            var videos = view.DataSource as SortableBindingList<VideoModel>;
            if(videos != null)
                videos.Where(x => x.Selected).ToList().Each(x => videos.Remove(x));
        }

        private void ClearUI()
        {
            this.movieGridItemBindingSource.DataSource = null;

        }

        private void RefreshMovieGrid()
        {
            var movies = this.movieGridItemBindingSource.DataSource as List<MovieGridItem>;
            var context = new IFlixrContext();
            foreach (var mv in movies)
            {
                var dbMovie = context.Movies.FirstOrDefault(x => x.UniqueToken == mv.Link);

                if (dbMovie != null)
                {
                    mv.IsInDB = true;
                    mv.IsMovieVideoAvailable = dbMovie.VideoLinks.Count() > 0;
                    mv.IsMusicVideoAvailable = dbMovie.Children.Where(x => x.Type == (byte)VideoType.Music).Count() > 0;
                    mv.IsTrailerVideoAvailable = dbMovie.Children.Where(x => x.Type == (byte)VideoType.Trailer).Count() > 0;
                    mv.IsClipAvailable = dbMovie.Children.Where(x => x.Type >= (byte)VideoType.Clip).Count() > 0;
                    mv.IsImageAvailable = dbMovie.Images.Count > 0;
                }
                else
                {
                    mv.IsInDB = false;
                    mv.IsMovieVideoAvailable = false;
                    mv.IsMusicVideoAvailable = false;
                    mv.IsTrailerVideoAvailable = false;
                    mv.IsClipAvailable = false;
                    mv.IsImageAvailable = false;
                }
                
            }
            this.movieGridItemBindingSource.ResetBindings(false);
          
        }

        private string GetSelectedUploader(DataGridView view)
        {
            var data = view.DataSource as SortableBindingList<VideoModel>;
         
            if(data != null)
            {
                var item = data.FirstOrDefault(x => x.Selected);
                if(item != null)
                    return item.Uploader;
            }

            return String.Empty;

        }

        private VideoSource GetSelectedUploaderSource(DataGridView view)
        {
            var data = view.DataSource as SortableBindingList<VideoModel>;
            if(data != null)
            {
                var item = data.FirstOrDefault(x => x.Selected);
                if(item != null)
                    return item.Source;
            }

            return VideoSource.Any;

        }

        private void SaveResults()
        {
            SaveSearchResults(this.selectedMovieModel, this.dataGridViewMovieVideos.DataSource as SortableBindingList<VideoModel>);
            SaveSearchResults(this.selectedMovieModel, this.dataGridViewMusic.DataSource as SortableBindingList<VideoModel>);
            SaveSearchResults(this.selectedMovieModel, this.dataGridViewTrailer.DataSource as SortableBindingList<VideoModel>);
            SaveSearchResults(this.selectedMovieModel, this.dataGridViewClip.DataSource as SortableBindingList<VideoModel>);
        }
        
        private void SaveSearchResults(MovieModel movieModel, SortableBindingList<VideoModel> model)
        {
            if (model == null)
                return;

            model.Each(x =>
            {
                if (!movieModel.SearchResults.Any(y => y.VideoId == x.VideoId
                     && y.Type == x.Type))
                    movieModel.SearchResults.Add(x);
            });
        }
        
        #endregion

        #region Events

        private void buttonReloadWiki_Click(object sender, EventArgs e)
         {
            var item = this.movieGridItemBindingSource.Current as MovieGridItem;
            if (item == null)
                return;

            if (!String.IsNullOrWhiteSpace(item.Link))
            {
                var movieModel = this.movieCollection
                    .Where(x => x.Movie.WikiUrl == item.Entity.WikiUrl)
                    .SingleOrDefault();

                WikiScraper scraper = new WikiScraper();
                var movie = new WikiScraper().GetMovie(item.Entity);
                movieModel.Movie.Map(movie);
                this.dataGridViewMovieDetailSummary.DataSource = null;
                this.dataGridViewMovieDetailSummary.DataSource = Helper.CreateMovieSummaryItems(movieModel.Movie);
            }   

         }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView view = (DataGridView)sender;
            if (e.RowIndex != -1 )
            {

                // Open the link in the default browser
                var data = view.DataSource as SortableBindingList<VideoModel>;

                if (view.Columns[e.ColumnIndex].CellType == typeof(DataGridViewLinkCell))
                {
                    if (data != null)
                    {
                        var vm = data.ElementAt(e.RowIndex);
                        if (Control.ModifierKeys == Keys.Shift) 
                        {
                            if (vm.Type == VideoType.Movie)
                            {
                                SearchVideo(this.textBoxMovieSeach.Text + " " + this.textBoxMoviePostfix.Text,
                                             this.textBoxMovieSeach.Text,
                                             this.dataGridViewMovieVideos,
                                             VideoType.Movie,
                                             this.dataGridViewSelectedMovies.DataSource as SortableBindingList<VideoModel>,
                                             vm.Uploader, vm.Source);
                            }
                        }  
                        else
                            Process.Start(vm.Url);
                    }
                    else
                    {
                        var wiki = this.movieGridItemBindingSource.Current as MovieGridItem;
                        if (wiki != null)
                            Process.Start(wiki.Link);
                    }
                }

            }
            
        }

        private void buttonFetch_Click(object sender, EventArgs e)
        {
            try
            {
                ClearUI();
                string wikiLink = String.Format("http://en.wikipedia.org/wiki/Malayalam_films_of_{0}", this.comboBoxYear.SelectedItem);
                var scrapper = new WikiScraper();
                var movieNames = scrapper.GetMovieNames(
                    scrapper.GetNodesUsingCss(wikiLink, "table.wikitable"),
                    Int16.Parse(this.comboBoxYear.SelectedItem.ToString()));
                var movies = new List<MovieGridItem>();
                int index = 1;

                var context = new IFlixrContext();

                foreach (var mn in movieNames)
                {
                    var movie = context.Movies.FirstOrDefault(x => x.UniqueToken == mn.WikiUrl);
                    movies.Add(new MovieGridItem()
                    {
                        Index = index++,
                        Language = "Mal",
                        Name = mn.Name,
                        Year = Int32.Parse(this.comboBoxYear.SelectedItem.ToString()),
                        Link = mn.WikiUrl,
                        Entity = mn,
                        IsInDB = movie != null,
                        IsMovieVideoAvailable = movie != null && movie.VideoLinks.Count > 0,
                        IsMusicVideoAvailable = movie != null && movie.Children.Where(x => x.Type == (byte)VideoType.Music).Count() > 0,
                        IsTrailerVideoAvailable = movie != null && movie.Children.Where(x => x.Type == (byte)VideoType.Trailer).Count() > 0,
                        IsClipAvailable  = movie != null && movie.Children.Where(x => x.Type >= (byte)VideoType.Clip).Count() > 0,
                        IsImageAvailable = movie != null && movie.Images.Count > 0
                    });

                }
                
                this.movieGridItemBindingSource.DataSource = movies;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "iFlixr");
            }
        }

       
        private void movieGridItemBindingSource_PositionChanged(object sender, EventArgs e)
        {
            SaveMovieInMemory(this.selectedMovieModel); //save the previous
            
            var item = this.movieGridItemBindingSource.Current as MovieGridItem;
            if (item == null)
                return;
            this.dataGridViewMovieVideos.DataSource = null;
            this.dataGridViewMusic.DataSource = null;
            this.dataGridViewTrailer.DataSource = null;
            this.dataGridViewSelectedMovies.DataSource = null;
            this.dataGridViewSelectedMusic.DataSource = null;
            this.dataGridViewSelectedTrailors.DataSource = null;
            this.dataGridViewClip.DataSource = null;
            this.dataGridViewSelectedClips.DataSource = null;
            this.dataGridViewMovieDetailSummary.DataSource = null;

            this.flowLayoutPanelImages.Controls.Clear();
            this.flowLayoutPanelSelectedImages.Controls.Clear();

            if (!String.IsNullOrWhiteSpace(item.Link))
            {
                var movieModel = this.movieCollection
                    .Where(x => x.Movie.WikiUrl == item.Entity.WikiUrl)
                    .SingleOrDefault();

                //check db
                if (movieModel == null)
                {
                    using (var context = new IFlixrContext())
                    {
                        var movie = context.Movies.FirstOrDefault(x => x.UniqueToken == item.Link);
                        if(movie != null)
                            movieModel = movie.Map();
                    }
                }

                if (movieModel == null)
                {
                    WikiScraper scraper = new WikiScraper();
                    movieModel = new MovieModel() { Movie = scraper.GetMovie(item.Entity) };
                }
                else
                {
                    LoadImages(movieModel.Images);

                    var relativePath = item.Name.Replace(":", "_").Replace(" ", "_");
                    var di = new DirectoryInfo(Path.Combine(Application.StartupPath, Helper.RootImageTempPath, relativePath));
                    if (di.Exists)
                    {
                        this.flowLayoutPanelImages.Controls.Clear();
                        var files = di.GetFiles();
                        foreach (var file in files)
                        {
                            var box = new SelectablePictureBox(new ImageModel() { FullPath = file.FullName },false);
                            this.flowLayoutPanelImages.Controls.Add(box);
                        }

                    }

                    var list = new SortableBindingList<VideoModel>();
                    movieModel.Music.Each(y => list.AddRange(y.Videos));
                    LoadMusic(list);

                    list.Clear();
                    movieModel.Trailor.Each(y => list.AddRange(y.Videos));
                    LoadTrailers(list);

                    list.Clear();
                    movieModel.Clip.Each(y => list.AddRange(y.Videos));
                    LoadClips(list);
     
                }

                using (var scraperContext = new ScraperContext())
                {
                    var videos = scraperContext.Videos
                        .Where(x => x.MovieUrl == item.Entity.WikiUrl)
                        .ToList();
                    movieModel.SearchResults = videos.Map();
                }

                if (movieModel.SearchResults.Count > 0)
                {
                    //load search results
                    this.dataGridViewMovieVideos.DataSource = new SortableBindingList<VideoModel>(
                        movieModel.SearchResults.Where(x => (VideoType)x.Type == VideoType.Movie).ToList());
                    this.dataGridViewMusic.DataSource = new SortableBindingList<VideoModel>(
                        movieModel.SearchResults.Where(x => (VideoType)x.Type == VideoType.Music).ToList());
                    this.dataGridViewTrailer.DataSource = new SortableBindingList<VideoModel>(
                            movieModel.SearchResults.Where(x => (VideoType)x.Type == VideoType.Trailer).ToList());
                    this.dataGridViewClip.DataSource = new SortableBindingList<VideoModel>(
                           movieModel.SearchResults.Where(x => (VideoType)x.Type == VideoType.Clip).ToList());
                }
           
                if (!this.movieCollection.Any(x => x.Movie.WikiUrl == item.Entity.WikiUrl))
                    this.movieCollection.Add(movieModel);

                LoadMovies(movieModel.Videos);

                this.selectedMovieModel = movieModel;
                this.dataGridViewMovieDetailSummary.DataSource = Helper.CreateMovieSummaryItems(movieModel.Movie);
            }
            
            SetSearchFields(item);
        }

        private void buttonImageSearch_Click(object sender, EventArgs e)
        {
            SearchImage();
        }

        private void buttonSearchVideo_Click(object sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (item != null)
            {
                SearchVideo(this.textBoxMovieSeach.Text + " " + this.textBoxMoviePostfix.Text,
                    this.textBoxMovieSeach.Text,
                    this.dataGridViewMovieVideos,
                    VideoType.Movie, 
                    this.dataGridViewSelectedMovies.DataSource as SortableBindingList<VideoModel>,
                    String.Empty, VideoSource.Any);
                SaveSearchResults(this.selectedMovieModel, 
                    this.dataGridViewMovies.DataSource as SortableBindingList<VideoModel>);
            }
        }

        private void buttonSearchMusic_Click(object sender, EventArgs e)
        {

            var item = GetSelectedItem();
            if (item != null)
            {
                SearchVideo(this.textBoxMusicSearch.Text + " " + this.textBoxMusicPostfix.Text,
                    this.textBoxMusicSearch.Text,
                    this.dataGridViewMusic,
                    VideoType.Music,
                    this.dataGridViewSelectedMusic.DataSource as SortableBindingList<VideoModel>,
                    String.Empty, VideoSource.Any);

                SaveSearchResults(this.selectedMovieModel,
                   this.dataGridViewMusic.DataSource as SortableBindingList<VideoModel>);
            }

        }

        private void buttonSearchTrailor_Click(object sender, EventArgs e)
        {
            
            var item = GetSelectedItem();
            if (item != null)
            {
               SearchVideo(this.textBoxOtherSearch.Text + " " + this.textBoxTrailerPostfix.Text, 
                   this.textBoxOtherSearch.Text,
                   this.dataGridViewTrailer,
                   VideoType.Trailer,
                   this.dataGridViewSelectedTrailors.DataSource as SortableBindingList<VideoModel>,
                  String.Empty);

               SaveSearchResults(this.selectedMovieModel,
                this.dataGridViewTrailer.DataSource as SortableBindingList<VideoModel>);
            }
        }

        private void buttonSearchClip_Click(object sender, EventArgs e)
        {
            var item = GetSelectedItem();
            if (item != null)
            {
                SearchVideo(this.textBoxImageSearch.Text,
                    this.textBoxImageSearch.Text,
                    this.dataGridViewClip,
                    VideoType.Clip,
                    this.dataGridViewSelectedClips.DataSource as SortableBindingList<VideoModel>,
                    String.Empty, VideoSource.Any);

                SaveSearchResults(this.selectedMovieModel,
                 this.dataGridViewClip.DataSource as SortableBindingList<VideoModel>);
            }
        }

        private void buttonSearchAll_Click(object sender, EventArgs e)
        {
            buttonSearchAllVideos_Click(sender, e);
            buttonImageSearch_Click(sender, e);
        }

        private void buttonSearchAllVideos_Click(object sender, EventArgs e)
        {
            buttonSearchVideo_Click(sender,e);
            buttonSearchMusic_Click(sender, e);
            buttonSearchTrailor_Click(sender, e);
            buttonSearchClip_Click(sender, e);
        }

        private void buttonAddAssets_Click(object sender, EventArgs e)
        {
            var movieGridSource = this.dataGridViewMovieVideos.DataSource as SortableBindingList<VideoModel>;
            var musicGridSource = this.dataGridViewMusic.DataSource as SortableBindingList<VideoModel>;
            var trailerGridSource = this.dataGridViewTrailer.DataSource as SortableBindingList<VideoModel>;
            var clipGridSource = this.dataGridViewClip.DataSource as SortableBindingList<VideoModel>;
            var complete = new SortableBindingList<VideoModel>();
            if (movieGridSource != null)
            {
                movieGridSource.Where(z => z.Selected).Each(x =>
                {
                    if (!complete.Any(y => y.VideoId == x.VideoId))
                        complete.Add(x);
                });
            }
            if (musicGridSource != null)
            {
                musicGridSource.Where(z => z.Selected).Each(x =>
                {
                    if (!complete.Any(y => y.VideoId == x.VideoId))
                        complete.Add(x);
                });
            }
            if (trailerGridSource != null)
            {
                trailerGridSource.Where(z => z.Selected).Each(x =>
                {
                    if (!complete.Any(y => y.VideoId == x.VideoId))
                        complete.Add(x);
                });
            }
            if (clipGridSource != null)
            {
                clipGridSource.Where(z => z.Selected).Each(x =>
                {
                    if (!complete.Any(y => y.VideoId == x.VideoId))
                        complete.Add(x);
                });
            }


            AddMovies(new SortableBindingList<VideoModel>(complete.Where(x => x.Type == VideoType.Movie).ToList()));
            AddMusic(new SortableBindingList<VideoModel>(complete.Where(x => x.Type == VideoType.Music).ToList()));
            AddTrailers(new SortableBindingList<VideoModel>(complete.Where(x => x.Type == VideoType.Trailer).ToList()));
            AddClips(new SortableBindingList<VideoModel>(complete.Where(x => (byte)x.Type >= (byte)VideoType.Clip).ToList()));
            
            var imageFiles = new List<string>();
            foreach(SelectablePictureBox spb in this.flowLayoutPanelImages.Controls)
            {
                if(spb != null && spb.Selected)
                    imageFiles.Add(spb.FileName);
            }
            AddImages(imageFiles);
        }

        private void buttonRemoveAssets_Click(object sender, EventArgs e)
        {
            RemoveItems(this.dataGridViewSelectedMovies);
            RemoveItems(this.dataGridViewSelectedMusic);
            RemoveItems(this.dataGridViewSelectedTrailors);
            RemoveItems(this.dataGridViewSelectedClips);

            Control [] controls = new Control[this.flowLayoutPanelSelectedImages.Controls.Count];
            this.flowLayoutPanelSelectedImages.Controls.CopyTo(controls, 0);
            foreach (SelectablePictureBox box in controls)
            {
                if (box.Selected)
                    this.flowLayoutPanelSelectedImages.Controls.Remove(box);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.selectedMovieModel != null)
                {
                    SaveMovieInMemory(this.selectedMovieModel);
                    this.selectedMovieModel.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "iFlixr");
            }
            RefreshMovieGrid();
        }

        private void buttonSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                SaveMovieInMemory(this.selectedMovieModel); //save the selected one
                foreach (var movie in this.movieCollection)
                    movie.Validate();

                foreach (var movie in this.movieCollection)
                    movie.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "iFlixr");
            }
            RefreshMovieGrid();
        }

        private void buttonClearMovies_Click(object sender, EventArgs e)
        {
            this.dataGridViewMovieVideos.DataSource = new SortableBindingList<VideoModel>();
        }

        private void dataGridViewMovieVideos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView view = (DataGridView)sender;
            if (e.RowIndex != -1)
            {

                // Open the link in the default browser
                var data = view.DataSource as SortableBindingList<VideoModel>;
                if (Control.ModifierKeys == Keys.Shift)
                {
                    var elem = data.ElementAt(e.RowIndex);
                    if (!String.IsNullOrWhiteSpace(elem.PlaylistId))
                        data.Where(x => x.PlaylistId == elem.PlaylistId).Each(x => x.Selected = elem.Selected);
                    else
                        data.Where(x => x.Uploader == elem.Uploader).Each(x => x.Selected = elem.Selected);
                    view.Refresh();
                }

            }
        }

        private void buttonBatch_Click(object sender, EventArgs e)
        {
            for (var i = ((int)this.numericUpDownStart.Value) - 1 ; i < this.dataGridViewMovies.Rows.Count; i++)
                BatchProcess(this.dataGridViewMovies.Rows[i].DataBoundItem as MovieGridItem);
        }

        private void BatchProcess(MovieGridItem mgItem)
        {
            var keyword = mgItem.Name.Replace(".", " ");
            labelBatchProgress.Text = keyword;
            Application.DoEvents();
            var searchText = keyword + " malayalam movie";

            var allVideos = new SortableBindingList<VideoModel>();
            allVideos.AddRange( Scrap(keyword + " malayalam movie", keyword, VideoType.Movie));
            allVideos.AddRange( Scrap(keyword + " malayalam movie song", keyword, VideoType.Music));
            allVideos.AddRange(  Scrap(keyword + " malayalam movie trailer", keyword, VideoType.Trailer));
            allVideos.AddRange( Scrap(keyword + " malayalam movie", keyword, VideoType.Clip));
            allVideos.Save(mgItem.Link);

            DoSearchImage(keyword, keyword + " malayalam movie",false);
            Thread.Sleep(10000);
            
        }

        #endregion

    }
  
  

}

