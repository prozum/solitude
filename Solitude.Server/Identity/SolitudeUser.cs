using Microsoft.AspNet.Identity;
using Neo4j.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace Solitude.Server
{
    public class SolitudeUser : ApplicationUser
    {
        public string Name { set; get; }
        public string Location { set; get; }

        DateTimeOffset _birthdate;
        public DateTimeOffset Birthdate 
        { 
            set
            {
                _birthdate = value.Date;
            }
            get
            {
                return _birthdate;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsyncFixed(UserManager<SolitudeUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}

