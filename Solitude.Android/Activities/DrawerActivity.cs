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
using Android.Support.V4.Widget;

namespace DineWithaDane.Android
{
	[Activity (Label = "AbstractActivity")]			
	public abstract class DrawerActivity : Activity
	{
		protected int Position { get; set; }
		protected SetupDrawer DrawerSetup { get; set; }
		protected FrameLayout Content { get; set; }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.ActivityLayout);

			Position = Intent.GetIntExtra("index", 0);

			DrawerSetup = new SetupDrawer (Position, this);

			DrawerSetup.Configure ();
			DrawerSetup.DrawerToggleSetup ();
			Content = FindViewById<FrameLayout>(Resource.Id.content_frame);
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			DrawerSetup.DrawerToggle.SyncState ();
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (DrawerSetup.DrawerToggle.OnOptionsItemSelected (item))
				return true;

			return base.OnOptionsItemSelected (item);
		}

		public override void OnBackPressed()
		{
			var dialog = new AlertDialog.Builder(this);
			dialog.SetMessage("Are you sure you want to log out?");
			dialog.SetNegativeButton("No", delegate{ /*Empty delegate hides the dialog*/ });
			dialog.SetNeutralButton("Yes", delegate
				{
					MainActivity.CIF.Logout(this);
				});
			dialog.Show();
		}

		/// <summary>
		/// Shows the spinner, indicating loading.
		/// </summary>
		/// <remarks>Must be run from the UI-thread</remarks>
		protected void ShowSpinner()
		{
			ProgressBar pb = new ProgressBar(this);
			Content.AddView(pb);
		}

		/// <summary>
		/// Removes the spinner.
		/// </summary>
		/// <remarks>Must be run in the UI-thread</remarks>
		protected void ClearLayout()
		{
			Content.RemoveAllViews();
		}

		protected void PrepareLooper()
		{
			if (Looper.MyLooper() == null)
				Looper.Prepare();
		}
	}
}

