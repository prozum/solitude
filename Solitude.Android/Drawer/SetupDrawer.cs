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


		private static readonly Tuple<string, int>[] Sections = new[] 
		{
			new Tuple<string, int>("Notifications", Resource.Drawable.Notification_Icon),
			new Tuple<string, int>("Offer", Resource.Drawable.Offer_Icon),
			new Tuple<string, int>("Events", Resource.Drawable.Events_Icon),
			new Tuple<string, int>("Host", Resource.Drawable.Host_Icon),
			new Tuple<string, int>("Profile", Resource.Drawable.Profile_Icon),
			new Tuple<string, int>("Settings", Resource.Drawable.Settings_Icon),
			new Tuple<string, int>("Logout", Resource.Drawable.Logout_Icon)
		};

		public SetupDrawer (int position, DrawerActivity currentActivity)
		{
			
			CurrentActivity = currentActivity;
			Position = position;
		}

		public void Configure ()
		{
			Drawer = CurrentActivity.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			DrawerList = CurrentActivity.FindViewById<ListView> (Resource.Id.left_drawer);
			//DrawerList.SetSelector(Resource.Drawable.orange);

			DrawerList.Adapter = new DrawerAdapter(CurrentActivity, Sections);

			//DrawerList.Adapter = new ArrayAdapter<string> (CurrentActivity, Resource.Layout.item_menu, Sections);
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
					var notificationIntent = new Intent (CurrentActivity, typeof(NotificationActivity));
					CurrentActivity.StartActivity (notificationIntent);
					break;
				case 1:
					var offerIntent = new Intent (CurrentActivity, typeof(OfferActivity));
					CurrentActivity.StartActivity (offerIntent);
					break;
				case 2:
					var eventIntent = new Intent (CurrentActivity, typeof(EventActivity));
					CurrentActivity.StartActivity (eventIntent);
					break;
				case 3:
					var hostIntent = new Intent (CurrentActivity, typeof(HostActivity));
					CurrentActivity.StartActivity (hostIntent);
					break;
				case 4:
					var profileIntent = new Intent (CurrentActivity, typeof(ProfileActivity));
					CurrentActivity.StartActivity (profileIntent);
					break;
				case 5:
					var settingsIntent = new Intent (CurrentActivity, typeof(SettingsActivitiy));
					CurrentActivity.StartActivity (settingsIntent);
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
