using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using IFlixr.Cooking.Base;

namespace IFlixr.Cooking.Data
{

    public interface IPrincipalEx : IPrincipal
    {
        string  UserId { get;  }
        string FirstName { get;  }
        string LastName { get;  }
        string PicUrl { get; }
        string UserPictureName { get;  }
        User User { get; }
    }

    public class PrincipalEx : IPrincipalEx
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }
        public User User { get; private set; }

        public PrincipalEx(User user)
        {
            this.Identity = new GenericIdentity(user.Email);
            User = user;
        }

        //public PrincipalEx(IPrincipal principal)
        //{
        //    this.Identity = principal.Identity;
        //}

        public string UserId { get { return User.Email; } }
        public string FirstName { get { return User.FirstName; } }
        public string LastName { get { return User.LastName; } }
        public string PicUrl { get { return LinkBuilder.GetProfileImageLink(UserPictureName); } }
        public string UserPictureName { get { return User.PictureName; } }
    }
}
