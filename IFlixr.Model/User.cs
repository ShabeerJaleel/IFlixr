using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Imports.Newtonsoft.Json;

namespace IFlixr.Data.Model
{
    [JsonObject(Title = "User")]
    public class User : RavenBase
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
