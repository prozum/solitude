using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Neo4j.AspNet.Identity;

namespace Solitude.Server
{
	public class AuthRepository : IDisposable
    {
        private UserManager<IdentityUser> userManager;

		public AuthRepository()
		{
//			ctx = new AuthContext();
//			userManager = new UserManager<IdentityUser>(new Neo4jUserStore<IdentityUser>(ctx));
		}

		public async Task<IdentityResult> RegisterUser(UserModel userModel)
		{
			IdentityUser user = new IdentityUser
			{
				UserName = userModel.UserName
			};

			var result = await userManager.CreateAsync(user, userModel.Password);

			return result;
		}

		public async Task<IdentityUser> FindUser(string userName, string password)
		{
			IdentityUser user = await userManager.FindAsync(userName, password);

			return user;
		}

		public void Dispose()
		{
			userManager.Dispose();
		}
	}
}

