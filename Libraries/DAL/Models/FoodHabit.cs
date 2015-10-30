using System;

namespace DAL
{
	public static class FoodHabit
	{
		public enum FoodHabitCode
		{
			HALAL		= 0,
			KOSHER		= 1,
			VEGAN		= 2,
			NO_LACTOSE	= 3,
			NO_GLUTEN	= 4,
			NO_NUTS		= 5
		}

		public static string Halal = "Halal";
		public static string Kosher = "Kosher";
		public static string Vegan = "Vegan";
		public static string LactoseIntolerant = "Lactose intolerance";
		public static string GlutenIntolerance = "Gluten intolerance";
		public static string NutAllergy = "Nut allergy";
	}
}

