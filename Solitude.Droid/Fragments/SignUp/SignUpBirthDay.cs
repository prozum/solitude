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
	class SignUpBirthDay : EditFragment
	{
		protected DatePicker Picker { get; set; }
		protected View Layout { get; set; }

		public SignUpBirthDay()
		{
			HidesKeyboard = true;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Layout = inflater.Inflate(Resource.Layout.signupFragLayout2, null);
			Picker = Layout.FindViewById<DatePicker>(Resource.Id.datepicker);
			Picker.DateTime = new DateTime(DateTime.Now.Year - 20, DateTime.Now.Month, DateTime.Now.Day);

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
			var now = DateTime.Today;
			var isvalid = now >= Picker.DateTime;

			if (!isvalid)
				AccentSnackBar.Make(Layout, Activity, Resource.String.event_error_invalid_date, 2000).Show();

			return isvalid;
		}
	}
}