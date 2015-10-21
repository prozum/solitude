using System;
using System.Collections.Generic;

namespace Solitude.Server
{
	public interface IMatchingSystem
	{
		List<Offer> FindMatch (Guest g);
	}
}

