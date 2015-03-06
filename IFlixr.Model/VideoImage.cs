using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Imports.Newtonsoft.Json;
using IFlixr.Base;

namespace IFlixr.Data.Model
{
    [JsonObject(Title = "VideoImage")]
    public class VideoImage
    {
        public string FileName { get; set; }
        public bool IsDefault { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ImageDimension SupportedDimensions { get; set; }
    }
}
