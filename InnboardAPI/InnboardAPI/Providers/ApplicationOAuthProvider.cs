using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System.Data.SqlClient;
using InnboardDomain.Models;
using InnboardDomain.Utility;

namespace InnboardAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;
        private InnboardDbContext dbContext;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
            dbContext = new InnboardDbContext();

        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string inputCompanyCode = context.Password.Remove(AppConstants.CompanyCode.Length);

            string inputPassword = context.Password.Replace(inputCompanyCode, string.Empty); 

            if (inputCompanyCode != AppConstants.CompanyCode)
            {
                context.SetError("invalid_grant", "The  password is incorrect.");
                return;
            }

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            bool IsAuthorizedUser = false;

            //SecurityUserInformation user = null;
            string query = string.Format(@"
                                        SELECT dbo.FxEnPassword('{0}')
                                    ", inputPassword);

            var encryptedtPassword = dbContext.Database.SqlQuery<string>(query).FirstOrDefault();
            
            await Task.Run(() =>
            {
                IsAuthorizedUser = encryptedtPassword == AppConstants.PassCode ? true : false;
                //user = dbContext.SecurityUserInformation.Where(s=>s.UserId == context.UserName && s.UserPassword == encryptedtPassword).FirstOrDefault();
            });

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            if (!IsAuthorizedUser)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            //identity.AddClaim(new Claim("UserName", user.UserName));

            context.Validated(identity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}