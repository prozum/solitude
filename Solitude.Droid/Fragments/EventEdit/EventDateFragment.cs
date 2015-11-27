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

namespace Solitude.Droid
{
	public class EventDateFragment : Android.Support.V4.App.Fragment, IEditPage
	{
		protected DatePicker Picker { get; set; }
		protected View Layout { get; set; }

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Layout = inflater.Inflate(Resource.Layout.editEventDateLayout, null);
			Picker = Layout.FindViewById<DatePicker>(Resource.Id.datepicker);
			Picker.DateTime = new DateTime(Activity.Intent.GetIntExtra("date year", DateTime.Now.Year),
											   Activity.Intent.GetIntExtra("date month", DateTime.Now.Month),
											   Activity.Intent.GetIntExtra("date day", DateTime.Now.Day));
				
			return Layout;
		}
		

		public void SaveInfo()
		{
			Activity.Intent.PutExtra("date year", Picker.DateTime.Year);
			Activity.Intent.PutExtra("date month", Picker.DateTime.Month);
            Activity.Intent.PutExtra("date day", Picker.DateTime.Day);
		}

		public bool IsValidData()
		{
			var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			var isvalid = now <= Picker.DateTime;

			if (!isvalid)
				Snackbar.Make(Layout, Resources.GetString(Resource.String.event_error_invalid_date), 2000).Show();

			return isvalid;
        }
	}
}