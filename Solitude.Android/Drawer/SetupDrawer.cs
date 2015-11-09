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
		public DrawerActivity _currentActivity { get; set; }
		public int Position { get; set; }
		public ActionBarDrawerToggle DrawerToggle { get; set; }


		private static readonly string[] Sections = new[] 
		{
			"Notifications", "Offer", "Events", "Host", "Profile", "Settings"
		};

		public SetupDrawer (
			//DrawerLayout drawer,
			//ListView drawerlist,
			int position,
			DrawerActivity currentActivity
		)
		{

			_currentActivity = currentActivity;
			Position = position;
		}

		public void Configure ()
		{
			Drawer = _currentActivity.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			DrawerList = _currentActivity.FindViewById<ListView> (Resource.Id.left_drawer);

			DrawerList.Adapter = new ArrayAdapter<string> (_currentActivity, Resource.Layout.item_menu, Sections);
			DrawerList.SetItemChecked (0, true);
			DrawerList.SetItemChecked (Position, true);

			DrawerList.ItemClick += DrawerListOnItemClick;
		}

		public void DrawerToggleSetup () {
			DrawerToggle = new ActionBarDrawerToggle (
				_currentActivity, 
				Drawer, 
				Resource.Drawable.ic_navigation_drawer, 
				Resource.String.open_drawer, 
				Resource.String.close_drawer);

			Drawer.SetDrawerListener (DrawerToggle);
			_currentActivity.ActionBar.SetDisplayHomeAsUpEnabled (true);
			_currentActivity.ActionBar.SetHomeButtonEnabled (true);
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
				Intent NotificationIntent = new Intent (_currentActivity, typeof(NotificationActivity));
				_currentActivity.StartActivity (NotificationIntent);
				break;
			case 1:
				Intent OfferIntent = new Intent (_currentActivity, typeof(OfferActivity));
				_currentActivity.StartActivity (OfferIntent);
				break;
			case 2:
				Intent EventIntent = new Intent (_currentActivity, typeof(EventActivity));
				_currentActivity.StartActivity (EventIntent);
				break;
			case 3:
				Intent HostIntent = new Intent (_currentActivity, typeof(HostActivity));
				_currentActivity.StartActivity (HostIntent);
				break;
			case 4:
				Intent ProfileIntent = new Intent (_currentActivity, typeof(ProfileActivity));
				_currentActivity.StartActivity (ProfileIntent);
				break;
			case 5:
				Intent SettingsIntent = new Intent (_currentActivity, typeof(SettingsActivitiy));
				_currentActivity.StartActivity (SettingsIntent);
				break;
			default:
				break;
			}

			DrawerList.SetItemChecked (itemClickEventArgs.Position, true);
			Drawer.CloseDrawer (DrawerList);
		}
	}
}
