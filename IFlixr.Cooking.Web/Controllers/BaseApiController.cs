using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using IFlixr.Cooking.Repository;

namespace IFlixr.Cooking.Web.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
        {
            Repository = RavenRepository.Create();
        }

        protected RavenRepository Repository { get; private set; }

    }
}