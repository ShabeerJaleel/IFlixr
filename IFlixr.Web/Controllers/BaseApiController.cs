using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using IFlixr.Document;

namespace IFlixr.Web.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
        {
            DocumentService = new DocumentService();
        }

        protected IDocumentService DocumentService { get; private set; }

    }
}