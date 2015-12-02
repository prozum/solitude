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
	public class EventDateFragment : EditFragment
	{
		protected DatePicker Picker { get; set; }
		protected View Layout { get; set; }

		public EventDateFragment()
		{
			HidesKeyboard = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Layout = inflater.Inflate(Resource.Layout.editEventDateLayout, null);
			Picker = Layout.FindViewById<DatePicker>(Resource.Id.datepicker);
			Picker.DateTime = new DateTime(Activity.Intent.GetIntExtra("date year", DateTime.Now.Year),
											   Activity.Intent.GetIntExtra("date month", DateTime.Now.Month),
											   Activity.Intent.GetIntExtra("date day", DateTime.Now.Day + 1));
				
			return Layout;
		}
		

		public override void SaveInfo()
		{
			Activity.Intent.PutExtra("date year", Picker.DateTime.Year);
			Activity.Intent.PutExtra("date month", Picker.DateTime.Month);
            Activity.Intent.PutExtra("date day", Picker.DateTime.Day);
		}

		public override bool IsValidData()
		{
			var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			var isvalid = now <= Picker.DateTime;

			if (!isvalid)
				AccentSnackBar.Make(Layout, Activity, Resource.String.event_error_invalid_date, 2000).Show();

			return isvalid;
        }
	}
}