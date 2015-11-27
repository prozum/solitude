
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
	public class SignUpFragmentLanguages : Android.Support.V4.App.Fragment
	{
		ListView languagesListView;

		public List<int> Languages
		{
			get {
				for (int i = 0; i < languagesListView.ChildCount; i++)
					if ((languagesListView.GetChildAt(i) as CheckBox).Checked)
						Languages.Add(i);
				return Languages;
			}
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var userLanguages = new List<int>();

			//Inflate view and find content
			View view = inflater.Inflate(Resource.Layout.signupFragLayout4, container, false);
			var desc = view.FindViewById<TextView>(Resource.Id.signupListDescription);
			languagesListView = view.FindViewById <ListView>(Resource.Id.interestListView);
			var lay = view.FindViewById<RelativeLayout>(Resource.Id.layoutSignupLists);

			//Adds the headline
			desc.Text = GetString(Resource.String.profile_menu_edit_language);

			//Populate the ListView
			languagesListView.Adapter = new ArrayAdapter<string>(Activity, 
				Resource.Layout.CheckedListViewItem, MainActivity.InfoNames[(int)InfoType.Language]);
			languagesListView.ChoiceMode = ChoiceMode.Multiple;

			//Create and place an accept button at the bottom of the screen
			var acptBtn = new Button(view.Context);
			acptBtn.Text = GetString(Resource.String.accept_button);
			acptBtn.Click += (Activity as SignUpActivity).confirmSignup;

			//Creates an accept-button and displays it at bottom!
			RelativeLayout.LayoutParams layPar = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.WrapContent);
			acptBtn.LayoutParameters = layPar;
			layPar.AddRule(LayoutRules.AlignParentBottom);

			lay.AddView(acptBtn);
			return view;
		}
	}
}

