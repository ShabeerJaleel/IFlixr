using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IFlixr.ViewModel.Base;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using IFlixr.Base;

namespace IFlixr.ViewModel
{
    public class CarouselModel : BaseModel, IJsTemplate
    {
        public CarouselModel()
        {
            IsNavigationEnabled = true;
        }
        public string Id { get; set; }

        public string Title { get; set; }

        public int MinItems { get; set; }

        public int MaxItems { get; set; }

        public bool IsNavigationEnabled { get; set; }

        public bool IsPaginationEnabled { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CarouselSize Size { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CarouselType Type { get; set; }

        public IEnumerable<CarouselItemModel> Items { get; set; }

        public string TemplateName
        {
            get { return Size.ToString().ToLower() + "-carousel"; }
        }

        public string SizeCSSClass { get; set; }
    }
}
