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
using Android.Support.V7.App;

namespace Solitude.Droid
{
	[Activity(Label = "AbstractActivity")]			
	public abstract class DrawerActivity : AppCompatActivity
	{
		protected int Position { get; set; }

		protected SetupDrawer DrawerSetup { get; set; }

		protected FrameLayout Content { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.ActivityLayout);

			Position = Intent.GetIntExtra("index", 0);

			DrawerSetup = new SetupDrawer(Position, this);

			DrawerSetup.Configure();
			DrawerSetup.DrawerToggleSetup();
			Content = FindViewById<FrameLayout>(Resource.Id.content_frame);
		}

		protected override void OnPostCreate(Bundle savedInstanceState)
		{
			base.OnPostCreate(savedInstanceState);
			DrawerSetup.DrawerToggle.SyncState();
		}
		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			menu.Add(0,0,0,"Global Menu Item");
			return true;
		}
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (DrawerSetup.DrawerToggle.OnOptionsItemSelected(item))
				return true;
			switch (item.ItemId)
			{
				case 0:
					var dialog = new Android.Support.V7.App.AlertDialog.Builder(this);
					dialog.SetMessage("This does not do anything");
					dialog.SetNeutralButton(Resource.String.ok, (s, e) => { });
					dialog.Show();
					break;
				default:
					break;
			}
			return base.OnOptionsItemSelected(item);
		}

		public override void OnBackPressed()
		{
			var dialog = new Android.Support.V7.App.AlertDialog.Builder(this);
			dialog.SetMessage(Resources.GetString(Resource.String.message_logout));
			dialog.SetNegativeButton(Resource.String.no, (s, e) => { });
			dialog.SetNeutralButton(Resource.String.yes, (s, e) => MainActivity.CIF.Logout(this));
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

