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
			drawerPosition = 4;

			base.OnCreate (savedInstanceState);

			var profile = FindViewById<RelativeLayout>(Resource.Layout.Profile);
			var adapter = new ProfileInfoListAdapter(this, TestMaterial.Infos);
			var tilelist = new InfoList(this, adapter);
			var tilelistparams = new RelativeLayout.LayoutParams(-1, -1);
			var layout = profile.FindViewById<RelativeLayout>(Resource.Id.ProfileLayout);

			tilelistparams.AddRule(LayoutRules.Below, Resource.Id.ProfileDataLayout);
			tilelistparams.AddRule(LayoutRules.Above, Resource.Id.EditProfileButton);
			tilelist.LayoutParameters = tilelistparams;

			layout.AddView(tilelist);

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

		[Activity (Label = "Profile")]			
		public class ProfileActivityOld : AbstractActivity
		{
			protected override void OnCreate (Bundle bundle)
			{
			}
		}
	}
}


