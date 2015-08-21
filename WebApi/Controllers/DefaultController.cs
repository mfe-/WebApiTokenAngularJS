using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.Models;

namespace WebApi.Controllers
{
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

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
                {
                     new Claim(ClaimTypes.Name, login.Logon),
                     new Claim("custom claim type", "custom content"),
                     new Claim(ClaimTypes.Role, "admin")
                 });

                var token = (JwtSecurityToken)tokenHandler.CreateToken(issuer: "localhost", audience: "localhost", subject: claimsIdentity, expires: expires);
                var tokenString = tokenHandler.WriteToken(token);
            }
            else
            {
                return new BadRequestResult(this);
            }

            return Ok();
        }
        [HttpGet]
        [ClaimsAuthorization(ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", ClaimValue = "admin")]
        public IHttpActionResult GetLogin()
        {
            return Ok();
        }
    }
}
