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
		ListView foodHabitsListView;
		List<int> foodPreferencesList = new List<int>();
		ArrayAdapter<string> ChoiceAdapter;
		public List<int> FoodPreferences
		{
			get
			{
				for (int i = 0; i < foodHabitsListView.ChildCount; i++)
					if ((foodHabitsListView.GetChildAt(i) as CheckBox).Checked)
						foodPreferencesList.Add(i);

				return foodPreferencesList;
			}
		}
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			string[] choises = Resources.GetStringArray(Resource.Array.foodhabits);

			ChoiceAdapter = new ArrayAdapter<string>(this.Context,
				Resource.Layout.CheckedListViewItem, choises);
			
		}
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//Inflate and find views
			View view = inflater.Inflate(Resource.Layout.signupFragLayout4, container, false);
			foodHabitsListView = view.FindViewById <ListView>(Resource.Id.interestListView);
			var desc = view.FindViewById<TextView>(Resource.Id.signupListDescription);

			//Populate the ListView
			foodHabitsListView.Adapter = ChoiceAdapter;
			foodHabitsListView.ChoiceMode = ChoiceMode.Multiple;

			//Adds the description
			desc.Text = GetString(Resource.String.profile_menu_edit_foodhabit);



			return view;
		}
	}
}

