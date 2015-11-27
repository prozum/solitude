
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
using Android.Views.InputMethods;

namespace Solitude.Droid
{
	public class SignUpFragmentInterests : Android.Support.V4.App.Fragment
	{
		ListView interestListView;

		public List<int> Interests
		{
			get
			{
				List<int> Interests = new List<int>();

				for (int i = 0; i < interestListView.ChildCount; i++)
					if ((interestListView.GetChildAt(i) as CheckBox).Checked)
						Interests.Add(i);

				return Interests;
			}
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var userInterests = new List<int>();

			//Inflate view and find content
			View view = inflater.Inflate(Resource.Layout.signupFragLayout4, container, false);
			interestListView = view.FindViewById <ListView>(Resource.Id.interestListView);
			var desc = view.FindViewById<TextView>(Resource.Id.signupListDescription);

			//Adds the description
			desc.Text = GetString(Resource.String.profile_menu_edit_interests);

			//Populate ListView
			interestListView.Adapter = new ArrayAdapter<string>(Activity, 
				Resource.Layout.CheckedListViewItem, MainActivity.InfoNames[(int)InfoType.Interest]);
			interestListView.ChoiceMode = ChoiceMode.Multiple;

			return view;
		}

		public override void OnResume()
		{
			base.OnResume();
		}
	}
}

