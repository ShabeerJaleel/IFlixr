using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IFlixr.Base
{
    public class QueryParam
    {
        public string FullSearhText{get;set;}
        public string Keyword{get;set;}
        public List<string> KeywordTokens
        {
            get
            {
                var keyword = Keyword.ToLowerInvariant();
                var tokens = keyword.Split(' ')
                    .Where(x => !String.IsNullOrWhiteSpace(x) && x.Length > 2 &&
                   !x.Equals("the")).ToList();

                return tokens;
            }
        }
        public string Uploader{get;set;}
        public VideoSource Source { get; set; }
        public VideoDuration Duration { get; set; }
        public bool FilterByUploader { get { return !String.IsNullOrWhiteSpace(Uploader); } }
    }
}
