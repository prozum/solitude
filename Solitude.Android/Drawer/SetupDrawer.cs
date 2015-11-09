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
		//private string m_DrawerTitle, m_Title;
		public DrawerLayout Drawer { get; set; }
		public ListView DrawerList { get; set; }
		public DrawerActivity CurrentActivity { get; set; }
		public int Position { get; set; }
		public ActionBarDrawerToggle DrawerToggle { get; set; }

		/*
		private DrawerLayout m_Drawer;
		private ListView m_DrawerList;
		private AbstractActivity _currentActivity;
		private int _drawerPosition;
		private ActionBarDrawerToggle drawerToggle;

		public ActionBarDrawerToggle DrawerToggle {
			get {
				return drawerToggle;
			}
		}
		*/

		private static readonly string[] Sections = new[] 
		{
			"Notifications", "Offer", "Events", "Host", "Profile"
		};

		public SetupDrawer (
			//DrawerLayout drawer,
			//ListView drawerlist,
			int position,
			DrawerActivity currentActivity
		)
		{
			//m_DrawerTitle = DrawerTitle;
			//m_Title = m_DrawerTitle = Title;
			//Drawer = drawer;
			//DrawerList = drawerlist;
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

		private void DrawerListOnItemClick (object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
		{
			switch (itemClickEventArgs.Position) 
			{
			case 0:
				Intent FrameMain = new Intent (CurrentActivity, typeof(NotificationActivity));
				CurrentActivity.StartActivity (FrameMain);
				break;
			case 1:
				Intent SecondActivity = new Intent (CurrentActivity, typeof(OfferActivity));
				CurrentActivity.StartActivity (SecondActivity);
				break;
			case 2:
				Intent ThirdActivity = new Intent (CurrentActivity, typeof(EventActivity));
				CurrentActivity.StartActivity (ThirdActivity);
				break;
			case 3:
				Intent FourthActivity = new Intent (CurrentActivity, typeof(HostActivity));
				CurrentActivity.StartActivity (FourthActivity);
				break;
			case 4:
				Intent FifthActivity = new Intent (CurrentActivity, typeof(ProfileActivity));
				CurrentActivity.StartActivity (FifthActivity);
				break;
			default:
				break;
			}

			DrawerList.SetItemChecked (itemClickEventArgs.Position, true);
			Drawer.CloseDrawer (DrawerList);
		}
	}
}

