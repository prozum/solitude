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

namespace DineWithaDane.Android
{
	[Activity (Label = "Notifications", MainLauncher = true)]			
	public class NotificationActivity : AbstractActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			drawerPosition = 0;

			base.OnCreate (bundle);
		}
	}
}
