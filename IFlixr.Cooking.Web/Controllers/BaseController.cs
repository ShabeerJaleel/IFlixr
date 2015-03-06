using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.Cooking.Repository;
using IFlixr.Cooking.ViewModel;
using IFlixr.Cooking.Data;

namespace IFlixr.Cooking.Web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            Repository = RavenRepository.Create();
            Authentication = new AuthenticationService();
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var json = filterContext.RequestContext.HttpContext.Request.QueryString["jsSupport"];
            filterContext.RequestContext.HttpContext.Session["jsSupport"] = !string.IsNullOrEmpty(json) ? false : true;
            AjaxRequest = filterContext.HttpContext.Request.IsAjaxRequest();
            base.OnActionExecuting(filterContext);
        }

        protected RavenRepository Repository { get; private set; }
        protected bool AjaxRequest { get; private set; }
        protected AuthenticationService Authentication { get; private set; }
        //public virtual new IPrincipalEx User { get { return (IPrincipalEx)User; } }
        public IPrincipalEx AuthenticatedUser { get { return (PrincipalEx)User; } }
    }
}
