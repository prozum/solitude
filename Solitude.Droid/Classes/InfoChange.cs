using System;

namespace Solitude.Droid
{
	public enum InfoType
	{
		Language    = 0,
		Interest	= 1,
		FoodHabit   = 2
	}
	public enum Language
	{
		Danish		= 0,
		English		= 1,
		German		= 2,
		French		= 3,
		Spanish		= 4,
		Chinese		= 5,
		Russian		= 6
	}
	public enum FoodHabit
	{
		Halal		= 0,
		Kosher		= 1,
		Vegan		= 2,
		NoLactose	= 3,
		NoGluten	= 4,
		NoNuts		= 5
	}
	public enum Interest
	{
		Nature		= 0,
		Fitness		= 1,
		Movies		= 2,
		Gaming		= 3,
		Electronics	= 4,
		Cooking		= 5,
		Drawing		= 6
	}

	public enum LanguageDa
	{
		Dansk		= 0,
		Engelsk		= 1,
		Tysk		= 2,
		Fransk		= 3,
		Spansk		= 4,
		Kinesisk	= 5,
		Russisk		= 6
	}

	public enum FoodHabitDa
	{
		Halal				= 0,
		Kosher				= 1,
		Vegetar				= 2,
		LaktoseIntolerance	= 3,
		GlutenIntolerance	= 4,
		NøddeAllergi		= 5
	}

	public enum InterestDa
	{
		Natur		= 0,
		Fitness		= 1,
		Film		= 2,
		Gaming		= 3,
		Elektronik	= 4,
		Madlavning	= 5,
		Tegning		= 6
	}

	public class InfoChange
	{
		public InfoType Info { get; private set; }
		public int Value { get; private set; }
		public int Weight { get; private set; }

		public InfoChange (InfoType key, int value, int weight)
		{
			Info = key;
			Value = value;
			Weight = weight;
		}
	}
}