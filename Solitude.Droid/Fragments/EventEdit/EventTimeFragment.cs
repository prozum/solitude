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
	public class EventTimeFragment : Android.Support.V4.App.Fragment, IEditPage
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here

		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var layout = inflater.Inflate(Resource.Layout.editEventTimeLayout, null);
			var timepicker = layout.FindViewById<TimePicker>(Resource.Id.timepicker);
			timepicker.CurrentHour = (Java.Lang.Integer)Activity.Intent.GetIntExtra("date hour", 0);
			timepicker.CurrentMinute = (Java.Lang.Integer)Activity.Intent.GetIntExtra("date minutte", 0);
			
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