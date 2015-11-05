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
		private string m_DrawerTitle, m_Title;
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

		private static readonly string[] Sections = new[] 
		{
			"Notifications", "Offer", "Events", "Host"
		};

		public SetupDrawer (
			string DrawerTitle,
			string Title,
			DrawerLayout Drawer,
			ListView DrawerList,
			int drawerPosition,
			AbstractActivity currentActivity
		)
		{
			m_DrawerTitle = DrawerTitle;
			m_Title = m_DrawerTitle = Title;
			m_Drawer = Drawer;
			m_DrawerList = DrawerList;
			_currentActivity = currentActivity;
			_drawerPosition = drawerPosition;
		}

		public void Configure ()
		{
			m_Drawer = _currentActivity.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			m_DrawerList = _currentActivity.FindViewById<ListView> (Resource.Id.left_drawer);

			m_DrawerList.Adapter = new ArrayAdapter<string> (_currentActivity, Resource.Layout.item_menu, Sections);
			this.m_DrawerList.SetItemChecked (0, true);
			this.m_DrawerList.SetItemChecked (_drawerPosition, true);

			m_DrawerList.ItemClick += DrawerListOnItemClick;
		}

		public void DrawerToggleSetup () {
			drawerToggle = new DrawerToggle (
				_currentActivity, 
				m_Drawer, 
				Resource.Drawable.ic_navigation_drawer, 
				Resource.String.open_drawer, 
				Resource.String.close_drawer);

			m_Drawer.SetDrawerListener (drawerToggle);
			_currentActivity.ActionBar.SetDisplayHomeAsUpEnabled (true);
			_currentActivity.ActionBar.SetHomeButtonEnabled (true);
		}

		private void DrawerListOnItemClick (object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
		{
			switch (itemClickEventArgs.Position) 
			{
			case 0:
				Intent FrameMain = new Intent (_currentActivity, typeof(NotificationActivity));
				_currentActivity.StartActivity (FrameMain);
				break;
			case 1:
				Intent SecondActivity = new Intent (_currentActivity, typeof(OfferActivity));
				_currentActivity.StartActivity (SecondActivity);
				break;
			case 2:
				Intent ThirdActivity = new Intent (_currentActivity, typeof(EventsActivity));
				_currentActivity.StartActivity (ThirdActivity);
				break;
			case 3:
				Intent FourthActivity = new Intent (_currentActivity, typeof(HostActivity));
				_currentActivity.StartActivity (FourthActivity);
				break;
			case 4:
				Intent FifthActivity = new Intent (_currentActivity, typeof(ProfileActivity));
				_currentActivity.StartActivity (FifthActivity);
				break;
			default:
				break;
			}

			this.m_DrawerList.SetItemChecked (itemClickEventArgs.Position, true);
			this.m_Drawer.CloseDrawer (m_DrawerList);
		}
	}
}

