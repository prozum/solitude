using System;
using System.Collections.Generic;

namespace Model
{
	public class Interest
	{
		public int Id { set; get; }
		public string Name { set; get; }

		public Interest(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public static List<Interest> Get()
		{
			return new List<Interest>()
			{ 
				new Interest(0, "NATURE" ),
				new Interest(1, "FITNESS"),
				new Interest(2, "MOVIES" ),
				new Interest(3, "GAMING" ),
				new Interest(4, "ELECTRONICS"),
				new Interest(5, "FOOD"),
				new Interest(6, "DRAWING")
			};
		}
	}
}

