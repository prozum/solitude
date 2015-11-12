﻿using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DineWithaDane.Android
{
	[Activity (Label = "Profile", Icon = "@drawable/Profile_Icon")]
	public class ProfileActivity : DrawerActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			var profile = new ProfileView(this, new User("Jimmi", "Jimmivej 12"));

			// setting up drawer
			base.OnCreate (savedInstanceState);

			// add profile to activity
			Content.AddView(profile);
		}
	}
}


