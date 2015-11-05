
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
	[Activity (Label = "Hosts")]			
	public class HostActivity : AbstractActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			drawerPosition = 3;

			base.OnCreate (bundle);
		}
	}
}

