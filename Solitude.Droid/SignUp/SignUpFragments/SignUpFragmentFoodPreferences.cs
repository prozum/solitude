using System;
using Android.OS;
using Android.Views;
using System.Collections.Generic;

namespace Solitude.Droid
{
	public class SignUpFragmentFoodPreferences : AbstractSignupFragment
	{
		
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			CreateCard(InfoType.FoodHabit, signUpCard, GetString(Resource.String.sign_up_foodpreferences));

			return signUpCard;
		}

		//Loads all info in the signUpInfo list into a list of InfoChanges and returns it.
		public List<InfoChange> SaveInfo()
		{
			List<InfoChange> foodPreferencesList = new List<InfoChange>();

			foreach (var info in signUpInfo)
			{
				foodPreferencesList.Add(new InfoChange(InfoType.FoodHabit, (int)Enum.Parse(typeof(FoodHabit), info), 1));
			}
			//foreach (var info in signUpInfo)
			//{
			//	foodPreferencesList.Add(new InfoChange(InfoType.FoodHabit, (int)Enum.Parse(typeof(FoodHabit), info)));
			//}
			var lang = Context.Resources.Configuration.Locale.Language;

			if (lang == "da")
				foreach (var info in signUpInfo)
					foodPreferencesList.Add(new InfoChange(InfoType.FoodHabit, (int)Enum.Parse(typeof(FoodHabitDa), info), 1)); 
			else
				foreach (var info in signUpInfo)
					foodPreferencesList.Add(new InfoChange(InfoType.FoodHabit, (int)Enum.Parse(typeof(FoodHabit), info), 1));
			
			return foodPreferencesList;
		}
	}
}

