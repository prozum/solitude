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

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Gets the layout, and all the relevant views it contains.
			Layout = inflater.Inflate(Resource.Layout.signupFragLayout3, container, false);
			Username = Layout.FindViewById<EditText>(Resource.Id.editUsername);
			UsernameLayout = Layout.FindViewById<TextInputLayout>(Resource.Id.signupUsernameText);
			Password = Layout.FindViewById<EditText>(Resource.Id.editPassword);
			Confirm = Layout.FindViewById<EditText>(Resource.Id.editConfirm);

			// Adds textCheckers to password fields
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

		/// <summary>
		/// Saves the data contained in the textfields.
		/// </summary>
		public override void SaveInfo()
		{
			// Put username, password and confirm in the Intent of the activity which contains this fragment.
			Activity.Intent.PutExtra("username", Username.Text);
			Activity.Intent.PutExtra("password", Password.Text);
			Activity.Intent.PutExtra("confirm", Confirm.Text);
		}

		/// <summary>
		/// Checks whether the textfields contains valid data.
		/// The textfields just have to contains some information.
		/// </summary>
		/// <returns>true, if the information is valid, else false</returns>
		public override bool IsValidData()
		{
			// Validdating info.
			var usernameisvalid = !string.IsNullOrEmpty(Username.Text);

			// Updating all the TextInputLayouts with all the nonevalid infomation errors.
			UpdateLayout(UsernameLayout, Username, Resource.String.sign_up_error_no_username, usernameisvalid);

			return usernameisvalid && PassWordIsValid && ConfirmIsValid;
		}

		/// <summary>
		/// A helpermethod for updating the TextInputLayouts error messages and color.
		/// </summary>
		/// <param name="layout">The layout to update.</param>
		/// <param name="text">The textfield the layout contains.</param>
		/// <param name="message">The error message.</param>
		/// <param name="noerror">The validation bool. true means, the information is valid, false means not.</param>
		private void UpdateLayout(TextInputLayout layout, EditText text, int message, bool noerror)
		{
			layout.ErrorEnabled = !noerror;

			if (noerror)
			{
				// There should be no message, when no error occured, and the color should be green.
				layout.Error = string.Empty;
				text.Background.SetColorFilter(Color.Green, PorterDuff.Mode.SrcAtop);
			}
			else
			{
				// There should be a message, when an error occured, and the color should be red.
				layout.Error = Resources.GetString(message);
				text.Background.SetColorFilter(Color.Red, PorterDuff.Mode.SrcAtop);
			}
		}

		/// <summary>
		/// The function for handeling password validdation.
		/// </summary>
		private void checkCorrectPassword(object sender, EventArgs e)
		{
			// Get relevant views.
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