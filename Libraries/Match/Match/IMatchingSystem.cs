using System;
using System.Collections.Generic;

namespace Match
{
	public interface IMatchingSystem
	{
		List<Offer> FindMatch (Guest g);
	}
}

