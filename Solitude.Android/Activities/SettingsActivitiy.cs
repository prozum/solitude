
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
	[Activity(Label = "SettingsActivitiy")]			
	public class SettingsActivitiy : DrawerActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			// Layout setup
			LinearLayout settingsLayout = new LinearLayout(this);
			settingsLayout.Orientation = Orientation.Vertical;

			// Button setup
			Button testBtn = new Button(this);
			testBtn.Text = "Delete Account";
			testBtn.Click += (object sender, EventArgs e) => {
				deletionDialog();
			};

			settingsLayout.AddView(testBtn);



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

			cancelButton.Click += (object sender, EventArgs e) => {
				dialog.Dismiss();
			};

			acceptButton.Click += (object sender, EventArgs e) => {
				MainActivity.CIF.DeleteUser();
				dialog.Dismiss ();
			};

			dialog.Show();
		}
	}
}

