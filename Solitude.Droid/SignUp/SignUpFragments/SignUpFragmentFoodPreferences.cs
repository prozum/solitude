
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Solitude.Droid
{
	public class SignUpFragmentFoodPreferences : Android.Support.V4.App.Fragment
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			var userFoodHabits = new List<int>();

			View view = inflater.Inflate(Resource.Layout.signupFragLayout4, container, false);
			var foodHabitsListView = view.FindViewById <ListView>(Resource.Id.interestListView);
			foodHabitsListView.Adapter = new ArrayAdapter<string>(Activity, 
				Resource.Layout.CheckedListViewItem, MainActivity.InfoNames[(int)InfoType.FoodHabit]);
			foodHabitsListView.ChoiceMode = ChoiceMode.Multiple;

			return view;
		}
	}
}

