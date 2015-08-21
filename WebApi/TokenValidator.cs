using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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
                return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
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
                //store.Close();

                SecurityToken securityToken;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = "http://localhost",
                    ValidIssuer = "http://localhost"
                    //SigningToken = new X509SecurityToken(cert)
                };

                Thread.CurrentPrincipal = handler.ValidateToken(token, validationParameters, out securityToken);
                HttpContext.Current.User = handler.ValidateToken(token, validationParameters, out securityToken);

                //check here if the web.config contains a set ClaimsTransformation

                return base.SendAsync(request, cancellationToken);
            }
            catch (ArgumentException e)
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
