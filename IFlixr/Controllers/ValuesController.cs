using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IFlixr.Service;
using IFlixr.ViewModel;
using IFlixr.Base;

namespace IFlixr.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly FlixrService flixrService;
        
        public ValuesController()
        {
            this.flixrService = new FlixrService();
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        public DynamicLayout Get(string id)
        {
            var layout = this.flixrService.GetDynamicLayout(Language.Malayalam, EntertainmentCategory.Movie);
            return layout;
        }


        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}