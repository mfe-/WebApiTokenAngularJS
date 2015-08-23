using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebApi.Controllers;

namespace WebApi
{
    /// <summary>
    /// 
    /// http://www.cloudidentity.com/blog/2013/01/09/using-the-jwt-handler-for-implementing-poor-man-s-delegation-actas/
    /// </summary>
    internal class TokenValidationHandler : DelegatingHandler
    {
        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string token;

            //determine whether a jwt exists or not
            if (!TryRetrieveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                //allow requests with no token - whether a action method needs an authentication can be set with the claimsauthorization attribute
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                //X509Store store = new X509Store(StoreName.TrustedPeople, 
                //                                StoreLocation.LocalMachine);
                //store.Open(OpenFlags.ReadOnly);
                //X509Certificate2 cert = store.Certificates.Find(
                //           X509FindType.FindByThumbprint, 
                //           "C1677FBE7BDD6B131745E900E3B6764B4895A226",
                //           false)[0];
                //store.Close(); https://self-issued.info/docs/draft-ietf-oauth-json-web-token.html
                // http://blog.pluralsight.com/selfcert-create-a-self-signed-certificate-interactively-gui-or-programmatically-in-net
                //get the public key
                X509Certificate2 cert = new X509Certificate2(Path.Combine(UserController.AssemblyDirectory, "public.localhost.cer"), "localhost");

                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "http://localhost",
                    ValidIssuer = "http://localhost",
                    ValidateLifetime = true,
                    IssuerSigningToken = new X509SecurityToken(cert),
                    LifetimeValidator = this.LifetimeValidator
                };
                //extract and assign the user of the jwt
                Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out securityToken);

                //todo check here if the web.config contains a ClaimsTransformation class

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException e)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception)
            {
                statusCode = HttpStatusCode.InternalServerError;
            }
            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
        }
        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if(expires!=null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }

    }
}
