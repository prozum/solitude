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
	/// The fragment containing the time of the event, that is being edited.
	/// </summary>
	public class EventTimeFragment : EditFragment
	{
		protected TimePicker Picker { get; set; }
		protected View Layout { get; set; }

		public EventTimeFragment()
		{
			// This fragment should hide the keyboard.
			HidesKeyboard = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Gets the layout, and all the relevant views it contains.
			Layout = inflater.Inflate(Resource.Layout.editEventTimeLayout, null);
			Picker = Layout.FindViewById<TimePicker>(Resource.Id.timepicker);

			// Sets the selected time of the timepicker to be either, the time contained in 
			// the activities Intent or DateTime.Now.
			Picker.CurrentHour = (Java.Lang.Integer)Activity.Intent.GetIntExtra("date hour", DateTime.Now.Hour);
			Picker.CurrentMinute = (Java.Lang.Integer)Activity.Intent.GetIntExtra("date minutte", DateTime.Now.Minute);
			
			return Layout;
		}

		/// <summary>
		/// Saves the time contained in the timepicker.
		/// </summary>
		public override void SaveInfo()
		{
			// Put hour and minutte in the Intent of the Activity which contains this fragment.
			Activity.Intent.PutExtra("date hour", (int)Picker.CurrentHour);
			Activity.Intent.PutExtra("date minutte", (int)Picker.CurrentMinute);
		}

		/// <summary>
		/// Checks whether the timepicker is on a valid time.
		/// The time for an event must never be before now.
		/// </summary>
		/// <returns>true, if the information is valid, else false</returns>
		public override bool IsValidData()
		{
			// Gets the year, month and day of the event that is being edited/created.
			var final = new DateTime(Activity.Intent.GetIntExtra("date year", DateTime.Now.Year),
									 Activity.Intent.GetIntExtra("date month", DateTime.Now.Month),
									 Activity.Intent.GetIntExtra("date day", DateTime.Now.Day),
									 (int)Picker.CurrentHour,
									 (int)Picker.CurrentMinute, 0);
			
			var isvalid = DateTime.Now <= final;

			// If date is not valid, show error snackbar.
			if (!isvalid)
				AccentSnackBar.Make(Layout, Activity, Resource.String.event_error_invalid_date, 2000).Show();

			return isvalid;
		}
	}
}