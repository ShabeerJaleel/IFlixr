using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IFlixr.Cooking.Base;

namespace IFlixr.Cooking.Web
{
    public class AuthenticationViewModel
    {
        public bool Authenticated { get; set; }
        public string UserPictureName { get; set; }
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string UserPicUrl
        {
            get
            {
                return LinkBuilder.GetProfileImageLink(UserPictureName);
            }
        }

        public string Token { get; set; }
    }

    public class MenuModel
    {
        public string ActiveMenu { get; set; }
        public string ActiveSubMenu { get; set; }
    }
}