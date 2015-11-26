
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
	public class SignUpFragmentNameAddress : Android.Support.V4.App.Fragment
	{
		EditText address, name, day, month, year;
		public DateTime Birthdate 
		{
			get { return new DateTime(int.Parse(year.Text), int.Parse(month.Text), int.Parse(day.Text)); }
		}
		public string Name
		{
			get { return name.Text; }
		}
		public string Address
		{
			get { return address.Text; }
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);
			View view = inflater.Inflate(Resource.Layout.signupFragLayout1, container, false);

			// Find all views on the fragment
			address = view.FindViewById<EditText>(Resource.Id.editAddress);
			name = view.FindViewById<EditText>(Resource.Id.editSignUpName);
			day = view.FindViewById<EditText>(Resource.Id.signupBirthday);
			month = view.FindViewById<EditText>(Resource.Id.signupBirthMonth);
			year = view.FindViewById<EditText>(Resource.Id.signupBirthYear);

			return view;
		}
	}
}

