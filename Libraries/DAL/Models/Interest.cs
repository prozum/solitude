using System;

namespace DAL
{
	public static class Interest
	{
		public enum InterestCode
		{
			NATURE 		= 0,
			FITNESS		= 1,
			MOVIES		= 2,
			GAMING		= 3,
			ELECTRONICS	= 4,
			FOOD		= 5,
			DRAWING		= 6
		}

		static string Nature = "Nature";
		static string Fitness = "Fitness";
		static string Movies = "Movies";
		static string Gaming = "Gaming";
		static string Electronics = "Electronics";
		static string Food = "Food";
		static string Drawing = "Drawing";

		public static string GetInterest (InterestCode ic)
		{
			switch (ic) {
			case InterestCode.NATURE:
				return Nature;
			case InterestCode.FITNESS:
				return Fitness;
			case InterestCode.MOVIES:
				return Movies;
			case InterestCode.GAMING:
				return Gaming;
			case InterestCode.ELECTRONICS:
				return Electronics;
			case InterestCode.FOOD:
				return Food;
			case InterestCode.DRAWING:
				return Drawing;
			default:
				return null;
			}
		}
	}
}

