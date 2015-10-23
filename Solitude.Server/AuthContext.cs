using System;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Solitude.Server
{
	public class AuthContext : IdentityDbContext<IdentityUser>
	{
		public AuthContext()
			: base("AuthContext")
		{

		}
	}
}

