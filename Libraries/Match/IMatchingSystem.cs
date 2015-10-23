using System;
using System.Collections.Generic;
using GraphDB;

namespace Match
{
	public interface IMatchingSystem
	{
		List<Offer> FindMatch (Guest g);
	}
}

