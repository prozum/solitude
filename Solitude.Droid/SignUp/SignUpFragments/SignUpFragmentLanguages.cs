
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

		/*public List<int> Interests
		{
			get {
				for (int i = 0; i < languagesListView.ChildCount; i++)
					if ((languagesListView.GetChildAt(i) as CheckBox).Checked)
						Interests.Add(i);
				
			}
		}*/

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			var userLanguages = new List<int>();

			View view = inflater.Inflate(Resource.Layout.signupFragLayout4, container, false);
			languagesListView = view.FindViewById <ListView>(Resource.Id.interestListView);
			languagesListView.Adapter = new ArrayAdapter<string>(Activity, 
				Resource.Layout.CheckedListViewItem, MainActivity.InfoNames[(int)InfoType.Language]);
			languagesListView.ChoiceMode = ChoiceMode.Multiple;


			return base.OnCreateView(inflater, container, savedInstanceState);
		}
	}
}

