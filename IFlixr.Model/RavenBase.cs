using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Imports.Newtonsoft.Json;

namespace IFlixr.Data.Model
{
    public class RavenBase
    {
        public string Id { get; set; }
        [JsonIgnore]
        public int IntId { get { return (String.IsNullOrWhiteSpace(Id) ? -1 : Convert.ToInt32(Id.Split('/')[1])); } }
    }
}
