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

            if (!TryRetrieveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                //we allow requests with no token
                //return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
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

                X509Certificate2 cert = new X509Certificate2(Path.Combine(UserController.AssemblyDirectory, "public.localhost.cer"), "localhost");

                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "http://localhost",
                    ValidIssuer = "http://localhost",
                    IssuerSigningToken = new X509SecurityToken(cert)
                };

                Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out securityToken);

                //check here if the web.config contains a set ClaimsTransformation

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

    }
}
