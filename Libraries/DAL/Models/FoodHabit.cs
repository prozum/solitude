using System;

namespace Dal
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

		static string Halal = "Halal";
		static string Kosher = "Kosher";
		static string Vegan = "Vegan";
		static string LactoseIntolerant = "Lactose intolerance";
		static string GlutenIntolerance = "Gluten intolerance";
		static string NutAllergy = "Nut allergy";

		public static string GetFoodHabit (FoodHabitCode fc)
		{
			switch (fc) {
			case FoodHabitCode.HALAL:
				return Halal;
			case FoodHabitCode.KOSHER:
				return Kosher;
			case FoodHabitCode.VEGAN:
				return Vegan;
			case FoodHabitCode.NO_LACTOSE:
				return LactoseIntolerant;
			case FoodHabitCode.NO_GLUTEN:
				return GlutenIntolerance;
			case FoodHabitCode.NO_NUTS:
				return NutAllergy;
			default:
				return null;
			}
		}
	}
}

