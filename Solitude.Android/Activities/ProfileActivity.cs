
﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using Android;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;
using Android.Graphics;
using Android.Content.PM;
using Android.Provider;
using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;



namespace DineWithaDane.Droid
{
	[Activity(Label = "Profile", Icon = "@drawable/Profile_Icon")]
	public class ProfileActivity : DrawerActivity
	{
		protected User User { get; set; }

		protected List<int>[] Info { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			///var profile = new ProfileView(this, new User("Jimmi", "Jimmivej 12"));

			// setting up drawer
			base.OnCreate(savedInstanceState);

			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch offers from server
					PrepareLooper();

					Info = MainActivity.CIF.GetInformation();
					User = MainActivity.CIF.GetUserData();

					//Clear screen and show the found offers
					RunOnUiThread(() =>
						{
							ClearLayout();
							SetupUI();
						});
				});

		}

		private void SetupUI()
		{
			// add profile to activity
			var profile = LayoutInflater.Inflate(Resource.Layout.Profile, null);
			Content.AddView(profile);

			var picture = FindViewById<ImageView>(Resource.Id.Image);
			var name = FindViewById<TextView>(Resource.Id.Name);
			var address = FindViewById<TextView>(Resource.Id.Address);
			var age = FindViewById<TextView>(Resource.Id.Age);
			var layout = FindViewById<LinearLayout>(Resource.Id.Layout);
			
			//var adapter = new InfoAdapter(this, Info);
			//var tilemenu = new InfoList(this, adapter);

			//layout.AddView(tilemenu);

			name.SetTypeface(null, TypefaceStyle.Bold);
			name.TextSize = 20;
			name.Text = User.Name;

			address.Text = User.Address;
			DateTime today = DateTime.Today;
			int iAge = today.Year - User.Birthdate.Year;
			if (User.Birthdate > today.AddYears(-iAge))
				iAge--;
			age.Text = iAge + Resources.GetString(Resource.String.year_old);
		}
	}
}