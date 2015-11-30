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
	public class EventTimeFragment : EditFragment
	{
		protected TimePicker Picker { get; set; }
		protected View Layout { get; set; }

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Layout = inflater.Inflate(Resource.Layout.editEventTimeLayout, null);
			Picker = Layout.FindViewById<TimePicker>(Resource.Id.timepicker);
			Picker.CurrentHour = (Java.Lang.Integer)Activity.Intent.GetIntExtra("date hour", 0);
			Picker.CurrentMinute = (Java.Lang.Integer)Activity.Intent.GetIntExtra("date minutte", 0);
			
			return Layout;
		}

		public override void SaveInfo()
		{
			Activity.Intent.PutExtra("date hour", (int)Picker.CurrentHour);
			Activity.Intent.PutExtra("date minutte", (int)Picker.CurrentMinute);
		}

		public override bool IsValidData()
		{
			var final = new DateTime(Activity.Intent.GetIntExtra("date year", DateTime.Now.Year),
									 Activity.Intent.GetIntExtra("date month", DateTime.Now.Month),
									 Activity.Intent.GetIntExtra("date day", DateTime.Now.Day),
									 (int)Picker.CurrentHour,
									 (int)Picker.CurrentMinute, 0);
			var isvalid = DateTime.Now <= final;

			if (!isvalid)
				Snackbar.Make(Layout, Resources.GetString(Resource.String.event_error_invalid_date), 2000).Show();

			return isvalid;
		}
	}
}