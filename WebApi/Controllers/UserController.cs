using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using WebApi.Models;

namespace WebApi.Controllers
{
    [EnableCors(origins: "http://localhost:5118", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        [HttpPost]
        [HttpGet]
        public IHttpActionResult PostLogin([FromBody]LoginViewModel login)
        {
            if (login != null && login.Logon == "martin")
            {
                DateTime expires = DateTime.Now.AddDays(1);

                //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
                var tokenHandler = new JwtSecurityTokenHandler();

                X509Certificate2 cert = new X509Certificate2("filename.pfx", "password", X509KeyStorageFlags.MachineKeySet);

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
                {
                     new Claim(ClaimTypes.Name, login.Logon),
                     new Claim("custom claim type", "custom content"),
                     new Claim(ClaimTypes.Role, "admin")
                 });

                var token = (JwtSecurityToken)tokenHandler.CreateToken(issuer: "http://localhost", audience: "http://localhost", subject: claimsIdentity, expires: expires);
                var tokenString = tokenHandler.WriteToken(token);
                return Ok<String>(tokenString);
            }
            return new BadRequestResult(this);
        }
        [HttpGet]
        [ClaimsAuthorization(ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", ClaimValue = "admin")]
        public IHttpActionResult GetLogin()
        {
            return Ok(this.User.Identity as ClaimsPrincipal);
        }
    }
}
