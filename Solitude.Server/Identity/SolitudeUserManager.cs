using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Neo4j.AspNet.Identity;
using Dal;

namespace Solitude.Server
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class SolitudeUserManager : UserManager<ApplicationUser>
    {
        private SolitudeUserStore _store;

        public SolitudeUserManager(SolitudeUserStore store)
            : base(store)
        {
            _store = store;
        }

        public static SolitudeUserManager Create(IdentityFactoryOptions<SolitudeUserManager> options, IOwinContext context) 
        {
            var manager = new SolitudeUserManager(new SolitudeUserStore(context.Get<GraphClientWrapper>().GraphClient, context.Get<DatabaseAbstrationLayer>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = true,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public override async Task<IdentityResult> DeleteAsync(ApplicationUser user)
        {
            if (user == null)
                return IdentityResult.Failed("User does not exist!");
            
            await _store.DeleteAsyncFixed(user);
            return IdentityResult.Success;
        }
    }
}

