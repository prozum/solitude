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

namespace Solitude.Droid
{
	/// <summary>
	/// The fragment that contains the name and address editing textfields.
	/// </summary>
	public class SignUpNameAddress : EditFragment
	{
		public TextInputLayout NameLayout { get; set; }
		public TextInputLayout AddressLayout { get; set; }
		public EditText Name { get; set; }
		public EditText Address { get; set; }

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Gets the layout, and all the relevant views it contains.
			var layout = inflater.Inflate(Resource.Layout.signupFragLayout1, null);
			NameLayout = layout.FindViewById<TextInputLayout>(Resource.Id.signupName);
			AddressLayout = layout.FindViewById<TextInputLayout>(Resource.Id.signupAddress);
			Name = layout.FindViewById<EditText>(Resource.Id.editSignUpName);
			Address = layout.FindViewById<EditText>(Resource.Id.editAddress);

			return layout;
		}

		/// <summary>
		/// Saves the data contained in the textfields.
		/// </summary>
		public override void SaveInfo()
		{
			// Put name and address in the Intent of the activity which contains this fragment.
			Activity.Intent.PutExtra("name", Name.Text);
			Activity.Intent.PutExtra("address", Address.Text);
		}

		/// <summary>
		/// Checks whether the textfields contains valid data.
		/// The textfields just have to contains some information.
		/// </summary>
		/// <returns>true, if the information is valid, else false</returns>
		public override bool IsValidData()
		{
			// Validdating info.
			var nameisvalid = !string.IsNullOrEmpty(Name.Text);
			var addressisvalid = !string.IsNullOrEmpty(Address.Text);

			// Updating all the TextInputLayouts with all the nonevalid infomation errors.
			UpdateLayout(NameLayout, Name, Resource.String.sign_up_error_no_name, nameisvalid);
			UpdateLayout(AddressLayout, Address, Resource.String.sign_up_error_no_address, addressisvalid);

			return nameisvalid && addressisvalid;
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
	}
}