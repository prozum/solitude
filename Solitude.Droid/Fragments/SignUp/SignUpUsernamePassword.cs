using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Graphics;
using System.Text.RegularExpressions;

namespace Solitude.Droid
{
	public class SignUpUsernamePassword : EditFragment
	{
		protected View Layout { get; set; }
		protected EditText Username { get; set; }
		protected EditText Password { get; set; }
		protected EditText Confirm { get; set; }
		protected TextInputLayout UsernameLayout { get; set; }
		protected bool PassWordIsValid { get; set; }
		protected bool ConfirmIsValid { get; set; }

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Layout = inflater.Inflate(Resource.Layout.signupFragLayout3, container, false);

			//Find and store views
			Username = Layout.FindViewById<EditText>(Resource.Id.editUsername);
			UsernameLayout = Layout.FindViewById<TextInputLayout>(Resource.Id.signupUsernameText);
			Password = Layout.FindViewById<EditText>(Resource.Id.editPassword);
			Confirm = Layout.FindViewById<EditText>(Resource.Id.editConfirm);

			//Adds textCheckers to password fields
			Password.TextChanged += checkCorrectPassword;
			Confirm.TextChanged += (s, e) => 
			{
				if (Confirm.Text != "" && Confirm.Text == Password.Text)
				{
					ConfirmIsValid = true;
                    Confirm.Background.SetColorFilter(Color.Green, PorterDuff.Mode.SrcAtop);
				}
				else
				{
					ConfirmIsValid = false;
                    Confirm.Background.SetColorFilter(Color.Red, PorterDuff.Mode.SrcAtop);
				}
			};

			return Layout;
		}

		public override void SaveInfo()
		{
			Activity.Intent.PutExtra("username", Username.Text);
			Activity.Intent.PutExtra("password", Password.Text);
			Activity.Intent.PutExtra("confirm", Confirm.Text);
		}

		public override bool IsValidData()
		{
			var usernameisvalid = !string.IsNullOrEmpty(Username.Text);

			UpdateLayout(UsernameLayout, Username, Resource.String.sign_up_error_no_username, usernameisvalid);

			return usernameisvalid && PassWordIsValid && ConfirmIsValid;
		}

		private void UpdateLayout(TextInputLayout layout, EditText text, int message, bool noerror)
		{
			layout.ErrorEnabled = !noerror;

			if (noerror)
			{
                layout.Error = string.Empty;
				text.Background.SetColorFilter(Color.Green, PorterDuff.Mode.SrcAtop);
			}
			else
			{
                layout.Error = Resources.GetString(message);
				text.Background.SetColorFilter(Color.Red, PorterDuff.Mode.SrcAtop);
			}
		}


		private void checkCorrectPassword(object sender, EventArgs e)
		{
			var lengthText = Layout.FindViewById<TextView>(Resource.Id.password_length);
			var charText = Layout.FindViewById<TextView>(Resource.Id.password_symbol);

			if (Password.Text.Length >= 6)
			{
				PassWordIsValid = true;
				lengthText.Text = GetString(Resource.String.password_req_length_accepted);
				lengthText.SetTextColor(Color.Green);
			}
			else
			{
				PassWordIsValid = false;
				lengthText.Text = GetString(Resource.String.password_req_length_unaccepted);
				lengthText.SetTextColor(Color.Red);
			}

			//Some Regex to check for symbols
			var reg = new Regex(@"\d");

			if (reg.IsMatch(Password.Text))
			{
				PassWordIsValid = true;
				charText.Text = GetString(Resource.String.password_req_char_accepted);
				charText.SetTextColor(Color.Green);
			}
			else
			{
				PassWordIsValid = false;
				charText.Text = GetString(Resource.String.password_req_char_unaccepted);
				charText.SetTextColor(Color.Red);
			}
		}
	}
}