using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Imports.Newtonsoft.Json;

namespace IFlixr.Data.Model
{
    [JsonObject(Title = "VideoPart")]
    public class VideoPart
    {
        public string Url { get; set; }
        public string UniqueId { get; set; }
        public int Sequence { get; set; }
        public int Duration { get; set; }
        public string Title { get; set; }
    }
}
