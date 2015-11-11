using System;
using System.Collections.Generic;

namespace Model
{
	public class FoodHabit
	{
		public int Id { set; get; }
		public string Name { set; get; }

		public FoodHabit(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public static List<FoodHabit> Get()
		{
			return new List<FoodHabit>()
			{ 
				new FoodHabit(0, "HALAL" ),
				new FoodHabit(1, "KOSHER"),
				new FoodHabit(2, "VEGAN" ),
				new FoodHabit(3, "NO_LACTOSE" ),
				new FoodHabit(4, "NO_GLUTEN"),
				new FoodHabit(5, "NO_NUTS")
			};
		}
	}
}

