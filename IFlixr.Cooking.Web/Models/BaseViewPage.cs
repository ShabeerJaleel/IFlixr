using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IFlixr.Cooking.ViewModel;
using IFlixr.Cooking.Data;


namespace IFlixr.Cooking.Web
{
    public abstract class BaseViewPage : WebViewPage
    {
        public BaseViewPage()
        {
            Menu = new MenuModel();
            Menu.ActiveMenu = ActiveMenu;
            Menu.ActiveSubMenu = ActiveSubMenu;
        }

        public AuthenticationViewModel Authentication
        {
            get
            {
                var user = base.User as IPrincipalEx;
                if (user != null)
                {
                    return new AuthenticationViewModel
                    {
                        Authenticated = user.Identity.IsAuthenticated,
                        DisplayName = user.FirstName,
                        UserId = user.UserId,
                        UserPictureName = user.UserPictureName
                    };
                }
                else
                {
                    return new AuthenticationViewModel();
                }
            }
        }

        public MenuModel Menu { get; set; }

        protected string ActiveMenu
        {
            get
            {
                return Request.QueryString["sort"] != null ? (string)Request.QueryString["sort"] : "latest";
            }
        }

        protected string ActiveSubMenu
        {
            get
            {
                var menu = ActiveMenu;
                if (!String.IsNullOrWhiteSpace(menu))
                {
                    var sub = Request.QueryString["duration"] != null ? (string)Request.QueryString["duration"] : "all";
                    switch (menu)
                    {
                        case "latest":
                            return "l" + sub;
                        case "popular":
                            return "p" + sub;
                        case "favourite":
                            return "f" + sub;
                    }
                }
                return String.Empty;
            }
        }
        
    }

    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
       

        public AuthenticationViewModel Authentication
        {
            get
            {
                var user = base.User as IPrincipalEx;
                if (user != null)
                {
                    return new AuthenticationViewModel
                    {
                        Authenticated = user.Identity.IsAuthenticated,
                        DisplayName = user.FirstName,
                        UserId = user.UserId,
                        UserPictureName = user.UserPictureName
                    };
                }
                else
                {
                    return new AuthenticationViewModel();
                }
            }
        }

        private MenuModel model;
        public MenuModel Menu
        {
            get
            {
                if (model == null)
                {
                    model = new MenuModel();
                    Menu.ActiveMenu = ActiveMenu;
                    Menu.ActiveSubMenu = ActiveSubMenu;
                }
                return model;
            }
        }

        protected string ActiveMenu
        {
            get
            {
                return Request.QueryString["sort"] != null ? (string)Request.QueryString["sort"] : "latest";
            }
        }

        protected string ActiveSubMenu
        {
            get
            {
                var menu = ActiveMenu;
                if (!String.IsNullOrWhiteSpace(menu))
                {
                    var sub = Request.QueryString["duration"] != null ? (string)Request.QueryString["duration"] : "all";
                    switch (menu)
                    {
                        case "latest":
                            return "l" + sub;
                        case "popular":
                            return "p" + sub;
                        case "favourite":
                            return "f" + sub;
                    }
                }
                return String.Empty;
            }
        }
        
    }
}