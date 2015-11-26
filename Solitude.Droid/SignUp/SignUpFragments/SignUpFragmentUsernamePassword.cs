
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
using Android.Graphics;
using System.Text.RegularExpressions;

namespace Solitude.Droid
{
	public class SignUpFragmentUsernamePassword : Android.Support.V4.App.Fragment
	{
		EditText username, password, confirmed;
		TextView lengthText, charText;
		public string Username
		{
			get { return username.Text; }
		}
		public string Password
		{
			get { return password.Text; }
		}
		public string ConfirmedPassword
		{
			get { return confirmed.Text; }
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		private void checkCorrectPassword(object sender, EventArgs e)
		{
			if (Password.Length >= 6)
			{
				lengthText.Text = GetString(Resource.String.password_req_length_accepted);
				lengthText.SetTextColor(Color.Green);
			}
			else
			{
				lengthText.Text = GetString(Resource.String.password_req_length_unaccepted);
				lengthText.SetTextColor(Color.Red);
			}

			//Some Regex to check for symbols
			Regex reg = new Regex(@"\d");

			if (reg.IsMatch(Password))
			{
				charText.Text = GetString(Resource.String.password_req_char_accepted);
				charText.SetTextColor(Color.Green);
			}
			else
			{
				charText.Text = GetString(Resource.String.password_req_char_unaccepted);
				charText.SetTextColor(Color.Red);
			}
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.signupFragLayout3, container, false);

			//Find and store views
			username = view.FindViewById<EditText>(Resource.Id.editUsername);
			password = view.FindViewById<EditText>(Resource.Id.editPassword);
			confirmed = view.FindViewById<EditText>(Resource.Id.editConfirm);
			lengthText = view.FindViewById<TextView>(Resource.Id.password_length);
			charText = view.FindViewById<TextView>(Resource.Id.password_symbol);

			//Adds textCheckers to password fields
			password.TextChanged += checkCorrectPassword;
			confirmed.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => {
				if(confirmed.Text != "" && confirmed.Text == password.Text)
					confirmed.Background.SetColorFilter(Color.Green, PorterDuff.Mode.SrcAtop);
			};

			return view;
		}
	}
}

