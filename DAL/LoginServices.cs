using ABAC.Identity;
using ABAC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ABAC.Services
{
    public interface ILoginServices
    {
        bool isAuthen();
        bool isInAdminRoles(string[] roles);
        bool isInRoles(string[] roles);
        void Login(AdUser2 user,string role, bool isPersistent, string sAMLRequest, string relayState, string actionUrl, string responseXml);
        void Logout();
        string UserRole();
        string SAMLRequest();
        string ResponseXml();
        string RelayState();
        string ActionUrl();
    }
    public class LoginServices : ILoginServices
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private HttpContext httpContext;

        public bool isAuthen()
        {
            return this.httpContext.User.Identity.IsAuthenticated;
        }
        public string AuthenName()
        {
            return this.httpContext.User.Identity.Name;
        }
        public string UserRole()
        {
            var rolename = this.httpContext.User.FindFirstValue(ClaimTypes.Role);
            return rolename;
        }
        public string SAMLRequest()
        {
            var sAMLRequest = this.httpContext.User.FindFirstValue("sAMLRequest");
            return sAMLRequest;
        }
        public string ResponseXml()
        {
            var responseXml = this.httpContext.User.FindFirstValue("responseXml");
            return responseXml;
        }
        public string RelayState()
        {
            var relayState = this.httpContext.User.FindFirstValue("relayState");
            return relayState;
        }
        public string ActionUrl()
        {
            var actionUrl = this.httpContext.User.FindFirstValue("actionUrl");
            return actionUrl;
        }
        public bool isInAdminRoles(string[] roles)
        {
            //if (UserRole() == RoleName.Admin)
            //    return true;

            //bool isRole = false;
            //for (var i = 0; i < roles.Length; i++)
            //{
            //    isRole = this.httpContext.User.IsInRole(roles[i]);
            //    if (isRole) { break; }
            //}
            //return isRole;

            return true;
        }
        public bool isInRoles(string[] roles)
        {
            bool isRole = false;
            for (var i = 0; i < roles.Length; i++)
            {
                isRole = this.httpContext.User.IsInRole(roles[i]);
                if (isRole) { break; }
            }
            return isRole;
        }

        public LoginServices(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.httpContext = this.httpContextAccessor.HttpContext;
        }
        public LoginServices(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }
        public async void Login(AdUser2 user, string role, bool isPersistent, string sAMLRequest, string relayState, string actionUrl,string responseXml)
        {
            if (string.IsNullOrEmpty(sAMLRequest))
                sAMLRequest = "";
            if (string.IsNullOrEmpty(relayState))
                relayState = "";
            if (string.IsNullOrEmpty(actionUrl))
                actionUrl = "";
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.SamAccountName));
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.SamAccountName));
            identity.AddClaim(new Claim("sAMLRequest", sAMLRequest));
            identity.AddClaim(new Claim("relayState", relayState));
            identity.AddClaim(new Claim("actionUrl", actionUrl));
            identity.AddClaim(new Claim("responseXml", responseXml));

            // Authenticate using the identity
            var principal = new ClaimsPrincipal(identity);
            await this.httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = isPersistent });
        }

        public async void Logout()
        {
            await this.httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
