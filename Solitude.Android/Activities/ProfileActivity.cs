using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace DineWithaDane.Android
{
	[Activity (Label = "TileMenu", MainLauncher = true)]
	public class ProfileActivity : AbstractActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			
			var adapter = new ProfileInfoListAdapter(this, TestMaterial.Infos);
			var tilelist = new InfoList(this, adapter);
			var profile = new ProfileView(this, "Sven", 22, "Svenvej 3", tilelist);

			// setting up drawer
			drawerPosition = 4;
			base.OnCreate (savedInstanceState);

			// add profile to activity
			Content.AddView(profile);



  
		}
	}
}


