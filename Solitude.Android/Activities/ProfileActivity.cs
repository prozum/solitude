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


			/*
			var dialog = new Dialog(this);
			dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
			dialog.SetContentView(Resource.Layout.popup);

			Button btnRetry = dialog.FindViewById<Button>(Resource.Id.btnFirst);
			btnRetry.Typeface = Typeface.CreateFromAsset (this.context.Assets, "Fonts/arial.ttf");
			btnRetry.Text = this.context.Resources.GetString(Resource.String.WifiInactiveDialogPositive);
			btnRetry.Click += (object sender, EventArgs e) =>
				{
					dialog.Dismiss();
					Check ();
				};
			

			dialog.Show();
			*/
  
		}
	}
}


