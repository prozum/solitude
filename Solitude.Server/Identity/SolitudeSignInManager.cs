using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Neo4j.AspNet.Identity;

namespace Solitude.Server
{
    // Configure the application sign-in manager which is used in this application.
    public class SolitudeSignInManager : SignInManager<ApplicationUser, string>
    {
        public SolitudeSignInManager(SolitudeUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((SolitudeUserManager)UserManager);
        }

        public static SolitudeSignInManager Create(IdentityFactoryOptions<SolitudeSignInManager> options, IOwinContext context)
        {
            return new SolitudeSignInManager(context.GetUserManager<SolitudeUserManager>(), context.Authentication);
        }
    }
}

