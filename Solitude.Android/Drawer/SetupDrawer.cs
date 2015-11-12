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


		private static readonly Tuple<string, int, Type>[] Sections = new[] 
			{
				new Tuple<string, int, Type>("Profile", Resource.Drawable.Profile_Icon, typeof(ProfileActivity)),
				//new Tuple<string, int, Type>("Notifications", Resource.Drawable.Notification_Icon, typeof(NotificationActivity)),
				new Tuple<string, int, Type>("Offer", Resource.Drawable.Offer_Icon, typeof(OfferActivity)),
				new Tuple<string, int, Type>("Events", Resource.Drawable.Events_Icon, typeof(EventActivity)),
				new Tuple<string, int, Type>("Host", Resource.Drawable.Host_Icon, typeof(HostActivity)),
				new Tuple<string, int, Type>("Settings", Resource.Drawable.Settings_Icon, typeof(SettingsActivitiy)),
				new Tuple<string, int, Type>("Logout", Resource.Drawable.Logout_Icon, typeof(MainActivity))
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

			if (itemClickEventArgs.Position == Sections.Length - 1)
			{
				MainActivity.CIF.Logout(CurrentActivity);
			}
			else
			{
				var nextIntent = new Intent (CurrentActivity, Sections[itemClickEventArgs.Position].Item3);
				nextIntent.PutExtra("index", itemClickEventArgs.Position);
				CurrentActivity.StartActivity (nextIntent);
			}

			DrawerList.SetItemChecked (itemClickEventArgs.Position, true);
			Drawer.CloseDrawer (DrawerList);
		}
	}
}
