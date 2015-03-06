using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.Document;

namespace IFlixr.Web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            DocumentService = new DocumentService();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var json = filterContext.RequestContext.HttpContext.Request.QueryString["jsSupport"];
            filterContext.RequestContext.HttpContext.Session["jsSupport"] = !string.IsNullOrEmpty(json) ? false : true;
            base.OnActionExecuting(filterContext);
        }

        protected IDocumentService DocumentService { get; private set; }
        
    }
}
