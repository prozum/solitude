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

namespace Solitude.Droid
{
	public class EventDateFragment : Android.Support.V4.App.Fragment, IEditPage
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var layout = inflater.Inflate(Resource.Layout.editEventDateLayout, null);
			var datepicker = layout.FindViewById<DatePicker>(Resource.Id.datepicker);
			datepicker.DateTime = new DateTime(Activity.Intent.GetIntExtra("date year", DateTime.Now.Year),
											   Activity.Intent.GetIntExtra("date month", DateTime.Now.Month),
											   Activity.Intent.GetIntExtra("date day", DateTime.Now.Day));
				

			return layout;
		}

		public void SaveInfo()
		{
		}

		public bool IsValidData()
		{
			return true;
		}
	}
}