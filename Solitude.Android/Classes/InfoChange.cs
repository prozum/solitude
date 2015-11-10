using System;

namespace DineWithaDane.Android
{
	public class InfoChange
	{
		public enum InfoType
		{
			LANGUAGE    = 0,
			INTEREST    = 1,
			FOODHABIT   = 2
		}
		public enum Language
		{
			DANISH 		= 0,
			ENGLISH		= 1,
			GERMAN		= 2,
			FRENCH		= 3,
			SPANISH		= 4,
			CHINESE		= 5,
			RUSSIAN		= 6
		}
		public enum FoodHabit
		{
			HALAL		= 0,
			KOSHER		= 1,
			VEGAN		= 2,
			NO_LACTOSE	= 3,
			NO_GLUTEN	= 4,
			NO_NUTS		= 5
		}
		public enum Interest
		{
			NATURE 		= 0,
			FITNESS		= 1,
			MOVIES		= 2,
			GAMING		= 3,
			ELECTRONICS	= 4,
			FOOD		= 5,
			DRAWING		= 6
		}

		private InfoType _key;
		private int _value;

		public InfoType t { get { return _key; } }
		public int val { get { return _value; } }


		public InfoChange (InfoType key, int value)
		{
			this._key = key;
			this._value = value;
		}
	}
}

