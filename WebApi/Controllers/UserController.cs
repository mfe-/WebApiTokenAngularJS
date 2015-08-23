using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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
        public IHttpActionResult PostLogin([FromBody]LoginViewModel login)
        {
            if (login != null && login.Logon == "martin")
            {
                //set the time when it expires
                DateTime expires = DateTime.UtcNow.AddDays(1);

                //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
                var tokenHandler = new JwtSecurityTokenHandler();
                //get private key
                X509Certificate2 cert = new X509Certificate2(Path.Combine(AssemblyDirectory, "private.localhost.pfx"), "localhost", X509KeyStorageFlags.MachineKeySet);
                //create a identity and add claims to the user which we want to log in
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
                {
                     new Claim(ClaimTypes.Name, login.Logon),
                     new Claim("custom claim type", "custom content"),
                     new Claim(ClaimTypes.Role, "admin")
                 });
                //create the jwt 
                var token = (JwtSecurityToken)tokenHandler.CreateToken(issuer: "http://localhost", audience: "http://localhost", subject: claimsIdentity, expires: expires, signingCredentials: new X509SigningCredentials(cert));
                var tokenString = tokenHandler.WriteToken(token);
                //return the token
                return Ok<String>(tokenString);
            }
            return new BadRequestResult(this);
        }
        [HttpGet]
        [ClaimsAuthorization(ClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", ClaimValue = "admin")]
        public IHttpActionResult GetLogin()
        {
            return Ok((this.User as ClaimsPrincipal).Claims);
        }
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
