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
	public class SignUpNameAddress : EditFragment
	{
		public TextInputLayout NameLayout { get; set; }
		public TextInputLayout AddressLayout { get; set; }
		public EditText Name { get; set; }
		public EditText Address { get; set; }

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var layout = inflater.Inflate(Resource.Layout.signupFragLayout1, null);
			NameLayout = layout.FindViewById<TextInputLayout>(Resource.Id.signupName);
            AddressLayout = layout.FindViewById<TextInputLayout>(Resource.Id.signupAddress);
			Name = layout.FindViewById<EditText>(Resource.Id.editSignUpName);
			Address = layout.FindViewById<EditText>(Resource.Id.editAddress);

			return layout;
		}

		public override void SaveInfo()
		{
			Activity.Intent.PutExtra("name", Name.Text);
			Activity.Intent.PutExtra("address", Address.Text);
		}

		public override bool IsValidData()
		{
			var nameisvalid = !string.IsNullOrEmpty(Name.Text);
			var addressisvalid = !string.IsNullOrEmpty(Address.Text);

			UpdateLayout(NameLayout, Name, Resource.String.sign_up_error_no_name, nameisvalid);
			UpdateLayout(AddressLayout, Address, Resource.String.sign_up_error_no_address, addressisvalid);

			return nameisvalid && addressisvalid;
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
	}
}