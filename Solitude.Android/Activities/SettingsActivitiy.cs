
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
using Android.Graphics;

namespace DineWithaDane.Android
{
	[Activity(Label = "Settings", Icon = "@drawable/Settings_Icon")]			
	public class SettingsActivitiy : DrawerActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			// Layout setup
			var settingsLayout = new LinearLayout(this);
			settingsLayout.Orientation = Orientation.Vertical;

			// Button setup
			var deletbutton = new Button(this);
			deletbutton.Text = "Delete Account";
			deletbutton.Click += (sender, e) => deletionDialog();

			settingsLayout.AddView(deletbutton);

			base.OnCreate(bundle);

			Content.AddView(settingsLayout);
		}

		private void deletionDialog ()
		{
			var dialog = new Dialog(this);
			Button cancelButton, acceptButton;
	
			dialog.SetContentView(Resource.Layout.deletionDialog);
			dialog.SetTitle("Are you sure?");

			cancelButton = (Button)dialog.FindViewById <Button>(Resource.Id.CancelDeletionBtn);
			acceptButton = (Button)dialog.FindViewById <Button>(Resource.Id.deleteAccountBtn);

			cancelButton.Click += (sender, e) => dialog.Dismiss();

			acceptButton.Click += (sender, e) => 
				{
					MainActivity.CIF.DeleteUser();
					MainActivity.CIF.Logout(this);
				};

			dialog.Show();
		}
	}
}

