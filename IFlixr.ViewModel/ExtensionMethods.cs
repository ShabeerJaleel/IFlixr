using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.Base;
using IFlixr.ViewModel;
using IFlixr.Data.Model;

namespace IFlixr.ViewModel
{
    public static class ExtensionMethods
    {
        // /Movie/Malayalam/2005/1/xgsox/casanova
        private static readonly string movieUrlTemplate = "/Movie/{0}/{1}/{2}/{3}/{4}";

        // GET: /Video/1/xvrgs/malayalam comedy
        private static readonly string videoUrlTemplate = "/Video/{0}/{1}/{2}";


        public static string GenerateVideoUrl(this Movie movie, string videoId)
        {
            return DoGenerateMovieUrl(movie.IntId, videoId, movie.Title, movie.Languages, movie.ReleaseDate);
        }

        public static string GenerateVideoUrl(this ThumbnailMovie movie)
        {
            return DoGenerateMovieUrl(movie.IntId, movie.UniqueId, movie.Title, movie.Languages, movie.ReleasedDate);
        }

        public static string GenerateVideoUrl(this Video video)
        {
            return String.Format(videoUrlTemplate, video.IntId, video.Links[0].Parts[0].UniqueId,
                Uri.EscapeUriString(video.Title.Trim().Replace(' ', '-')));
        }


        public static string GenerateVideoUrl(this ThumbnailVideo video)
        {
            if (String.IsNullOrWhiteSpace(video.UniqueId))
                return String.Empty;
            return String.Format(videoUrlTemplate, video.IntId, video.UniqueId,
                Uri.EscapeUriString(video.Title.Trim().Replace(' ', '-')));
        }


      

        public static string GenerateImageUrl(this Video video)
        {
            var img = video.Images
                   .OrderBy(x => x.IsDefault)
                   .FirstOrDefault();

            if (img != null)
                return "/Data/img/" + img.FileName;

            var link = video.Links.FirstOrDefault();
            if (link != null && link.Source == VideoSource.Youtube)
                return String.Format("http://img.youtube.com/vi/{0}/hqdefault.jpg", link.Parts.First().UniqueId);
            return string.Empty;
        }

        private static string DoGenerateMovieUrl(int id, string videoId,  string title, List<string> languages, DateTime releasedDate)
        {
            if (String.IsNullOrWhiteSpace(videoId))
                return String.Empty;

            return String.Format(movieUrlTemplate,
              languages.Count > 0 ? languages[0] : KnownLanguages.English,
              releasedDate.Year,
               id,
               videoId,
               Uri.EscapeUriString(title.Trim().Replace(' ', '-')));
       }
    }
}
