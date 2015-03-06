using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using IFlixr.Cooking.Repository;
using IFlixr.Cooking.Data;
using IFlixr.Cooking.ViewModel;

namespace IFlixr.Cooking.Web
{
    public class AuthenticationService
    {
        private RavenRepository repository = RavenRepository.Create();
        public AuthenticationViewModel Login(LoginModel model)
        {
            //TODO: validate 
            return Login(model.UserName, model.Password, model.RememberMe);
        }

        public AuthenticationViewModel Login(string userId, string password, bool rememberMe = false)
        {
            var user = repository.GetUserByEmail(userId);
            var success = user != null && user.LoginEnabled && user.Password == password;
            if (success)
            {
                SetAuthCookie(user, rememberMe);
                return new AuthenticationViewModel
                {
                    Authenticated = true,
                    DisplayName = user.FirstName,
                    UserPictureName = user.PictureName
                };
            }
            else
                return new AuthenticationViewModel();
            
        }

        public User OAuthLogin(string providerName, string providerUserId, bool rememberMe = false)
        {
            var user = repository.GetUser(providerName, providerUserId);
            if(user != null)
                SetAuthCookie(user, rememberMe);
            return user;
        }


        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        public void CreateUserAndAccount(RegisterModel model)
        {
            repository.CreateUser(new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                LoginEnabled = true,
                Password = model.Password,
                ProfileName = model.ProfileName,
                Email = model.Email,
                Gender = model.Gender,
                PictureName = model.Gender == "f" ? User.DefaultFemalePicture : User.DefaultMalePicture,
                ProfileUrl = model.ProfileName.Replace(' ','-')
            });
        }


        public User CreateOrUpdateAccount(string email, string providerName, string providerUserId,IDictionary<string, string> extraData)
        {
            var user = repository.GetUserByEmail(email);
            var ea = user.ExternalAccounts.SingleOrDefault(x => x.ProviderName == providerName && x.ProviderUserId == providerUserId);
            if (ea == null)
            {
                user.ExternalAccounts.Add(new OAuthAccount
                {
                     ProviderName = providerName,
                     ProviderUserId = providerUserId,
                     ExtraData = extraData
                });
                repository.CreateUser(user);
            }
            return user;
        }

        public bool HasLocalAccount(string userId)
        {
            return repository.GetUserByEmail(userId) != null;
        }

        public bool ChangePassword(string userId, string oldPassword, string newPassword)
        {

            var user = repository.GetUserByEmail(userId);
            if (user != null)
            {
                user.Password = newPassword;
                repository.CreateUser(user);
                return true;
            }
            return false;
        }

        public bool IsEmailValidAndNotRegistered(string email)
        {
            return  repository.GetUserByEmail(email) == null;
        }

        public bool IsProfileNameValidAndNotRegistered(string email)
        {
            return repository.GetUserByProfileName(email) == null;
        }

        private void SetAuthCookie(User user, bool rememberMe)
        {
            repository.PatchRecord(user.Id, "LastLogin", DateTime.Now);
            FormsAuthentication.SetAuthCookie(user.Email, rememberMe);
        }
    }
}