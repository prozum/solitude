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
using Android.Support.V4.App;

namespace DineWithaDane.Android
{			
	public class SetupDrawer
	{
		
		public DrawerLayout Drawer { get; set; }
		public ListView DrawerList { get; set; }
		public DrawerActivity CurrentActivity { get; set; }
		public int Position { get; set; }
		public ActionBarDrawerToggle DrawerToggle { get; set; }


		private static readonly string[] Sections = new[] 
		{
			"Notifications", "Offer", "Events", "Host", "Profile", "Settings", "Logout"
		};

		public SetupDrawer (
			//DrawerLayout drawer,
			//ListView drawerlist,
			int position,
			DrawerActivity currentActivity
		)
		{

			CurrentActivity = currentActivity;
			Position = position;
		}

		public void Configure ()
		{
			Drawer = CurrentActivity.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			DrawerList = CurrentActivity.FindViewById<ListView> (Resource.Id.left_drawer);

			DrawerList.Adapter = new ArrayAdapter<string> (CurrentActivity, Resource.Layout.item_menu, Sections);
			DrawerList.SetItemChecked (0, true);
			DrawerList.SetItemChecked (Position, true);

			DrawerList.ItemClick += DrawerListOnItemClick;
		}

		public void DrawerToggleSetup () {
			DrawerToggle = new ActionBarDrawerToggle (
				CurrentActivity, 
				Drawer, 
				Resource.Drawable.ic_navigation_drawer, 
				Resource.String.open_drawer, 
				Resource.String.close_drawer);

			Drawer.SetDrawerListener (DrawerToggle);
			CurrentActivity.ActionBar.SetDisplayHomeAsUpEnabled (true);
			CurrentActivity.ActionBar.SetHomeButtonEnabled (true);
		}

		/// <summary>
		/// Drawer item click cases
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="itemClickEventArgs">The ${ParameterType} instance containing the event data.</param>
		private void DrawerListOnItemClick (object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
		{
			switch (itemClickEventArgs.Position) 
			{
			case 0:
				Intent NotificationIntent = new Intent (CurrentActivity, typeof(NotificationActivity));
				CurrentActivity.StartActivity (NotificationIntent);
				break;
			case 1:
				Intent OfferIntent = new Intent (CurrentActivity, typeof(OfferActivity));
				CurrentActivity.StartActivity (OfferIntent);
				break;
			case 2:
				Intent EventIntent = new Intent (CurrentActivity, typeof(EventActivity));
				CurrentActivity.StartActivity (EventIntent);
				break;
			case 3:
				Intent HostIntent = new Intent (CurrentActivity, typeof(HostActivity));
				CurrentActivity.StartActivity (HostIntent);
				break;
			case 4:
				Intent ProfileIntent = new Intent (CurrentActivity, typeof(ProfileActivity));
				CurrentActivity.StartActivity (ProfileIntent);
				break;
			case 5:
				Intent SettingsIntent = new Intent (CurrentActivity, typeof(SettingsActivitiy));
				CurrentActivity.StartActivity (SettingsIntent);
				break;
			case 6:
				MainActivity.CIF.Logout(CurrentActivity);
				break;
			default:
				break;
			}

			DrawerList.SetItemChecked (itemClickEventArgs.Position, true);
			Drawer.CloseDrawer (DrawerList);
		}
	}
}
