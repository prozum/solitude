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
using Android.Support.Design.Widget;
using Android.Graphics;
using Android.Views.InputMethods;

namespace Solitude.Droid
{
	public class SignUpFragmentNameAddress : Android.Support.V4.App.Fragment
	{
		EditText address, name, day, month, year;

		/// <summary>
		/// Gets the birthdate if correctly filled else get today.
		/// </summary>
		/// <value>The birthdate.</value>
		public DateTime Birthdate 
		{
			get
			{
				//int iYear, iMonth, iDay;
				//if (int.TryParse(year.Text, out iYear) && int.TryParse(month.Text, out iMonth) && int.TryParse(day.Text, out iDay))
				//	return new DateTime(iYear, iMonth, iDay);
				//else
					return DateTime.Today;
			}
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return name.Text; }
		}

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
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

			//var imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
			//imm.HideSoftInputFromWindow(address.WindowToken, 0);
			//imm.HideSoftInputFromWindow(name.WindowToken, 0);
			//imm.HideSoftInputFromWindow(day.WindowToken, 0);
			//imm.HideSoftInputFromWindow(month.WindowToken, 0);
			//imm.HideSoftInputFromWindow(year.WindowToken, 0);

			return view;
		}
	}
}

