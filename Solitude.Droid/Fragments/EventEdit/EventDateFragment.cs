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
	/// The fragment containing the date of the event, that is being edited.
	/// </summary>
	public class EventDateFragment : EditFragment
	{
		protected DatePicker Picker { get; set; }
		protected View Layout { get; set; }

		public EventDateFragment()
		{
			// This fragment should hide the keyboard.
			HidesKeyboard = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Gets the layout, and all the relevant views it contains.
			Layout = inflater.Inflate(Resource.Layout.editEventDateLayout, null);
			Picker = Layout.FindViewById<DatePicker>(Resource.Id.datepicker);

			// Sets the selected time of the datepicker to be either, the time contained in 
			// the activities Intent or a day after DateTime.Now.
			Picker.DateTime = new DateTime(Activity.Intent.GetIntExtra("date year", DateTime.Now.Year),
											   Activity.Intent.GetIntExtra("date month", DateTime.Now.Month),
											   Activity.Intent.GetIntExtra("date day", DateTime.Now.Day + 1));
				
			return Layout;
		}
		
		/// <summary>
		/// Saves the data contained in the datepicker.
		/// </summary>
		public override void SaveInfo()
		{
			// Put year, month and day in the Intent of the activity which contains this fragment.
			Activity.Intent.PutExtra("date year", Picker.DateTime.Year);
			Activity.Intent.PutExtra("date month", Picker.DateTime.Month);
            Activity.Intent.PutExtra("date day", Picker.DateTime.Day);
		}

		/// <summary>
		/// Checks whether the datepicker is on a valid date.
		/// The date for an event must never be before now.
		/// </summary>
		/// <returns>true, if the information is valid, else false</returns>
		public override bool IsValidData()
		{
			// Gets now, without the hours, minuts and seconds.
			var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			var isvalid = now <= Picker.DateTime;

			// If date is not valid, show error snackbar.
			if (!isvalid)
				AccentSnackBar.Make(Layout, Activity, Resource.String.event_error_invalid_date, 2000).Show();

			return isvalid;
        }
	}
}