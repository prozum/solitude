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

namespace Solitude.Droid
{
	/// <summary>
	/// The fragment containing the birthday of the signup process.
	/// </summary>
	public class SignUpBirthDay : EditFragment
	{
		protected DatePicker Picker { get; set; }
		protected View Layout { get; set; }

		public SignUpBirthDay()
		{
			// This fragment should hide the keyboard.
			HidesKeyboard = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Gets the layout, and all the relevant views it contains.
			Layout = inflater.Inflate(Resource.Layout.signupFragLayout2, null);
			Picker = Layout.FindViewById<DatePicker>(Resource.Id.datepicker);

			// Sets the selected time of the datepicker to be 20 years before DateTime.Now.
			Picker.DateTime = new DateTime(DateTime.Now.Year - 20, DateTime.Now.Month, DateTime.Now.Day);

			return Layout;
		}

		/// <summary>
		/// Saves the data contained in the datepicker.
		/// </summary>
		public override void SaveInfo()
		{
			Activity.Intent.PutExtra("date year", Picker.DateTime.Year);
			Activity.Intent.PutExtra("date month", Picker.DateTime.Month);
			Activity.Intent.PutExtra("date day", Picker.DateTime.Day);
		}

		/// <summary>
		/// Checks whether the datepicker is on a valid date.
		/// The date for making a profile must be before now.
		/// </summary>
		/// <returns>true, if the information is valid, else false</returns>
		public override bool IsValidData()
		{
			var now = DateTime.Today;
			var isvalid = now >= Picker.DateTime;

			// If date is not valid, show error snackbar.
			if (!isvalid)
				AccentSnackBar.Make(Layout, Activity, Resource.String.event_error_invalid_date, 2000).Show();

			return isvalid;
		}
	}
}