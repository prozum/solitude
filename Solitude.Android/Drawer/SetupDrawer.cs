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
		// Private variables
		private string m_DrawerTitle, m_Title;
		private DrawerLayout m_Drawer;
		private ListView m_DrawerList;
		private AbstractActivity _currentActivity;
		private int _drawerPosition;
		private ActionBarDrawerToggle drawerToggle;

		/// <summary>
		/// Gets the drawer toggle.
		/// </summary>
		/// <value>The drawer toggle.</value>
		public ActionBarDrawerToggle DrawerToggle {
			get {
				return drawerToggle;
			}
		}

		// Strings displayed in the drawer, in order
		private static readonly string[] Sections = new[] 
		{
			"Notifications", "Offer", "Events", "Host", "Profile", "Settings"
		};

		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.SetupDrawer"/> class.
		/// </summary>
		/// <param name="DrawerTitle">Drawer title.</param>
		/// <param name="Title">Title.</param>
		/// <param name="Drawer">Drawer.</param>
		/// <param name="DrawerList">Drawer list.</param>
		/// <param name="drawerPosition">Drawer position.</param>
		/// <param name="currentActivity">Current activity.</param>
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

		// Configures the drawer, sets up an adapter and adds click functionality
		public void Configure ()
		{
			m_Drawer = _currentActivity.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			m_DrawerList = _currentActivity.FindViewById<ListView> (Resource.Id.left_drawer);

			m_DrawerList.Adapter = new ArrayAdapter<string> (_currentActivity, Resource.Layout.item_menu, Sections);
			this.m_DrawerList.SetItemChecked (0, true);
			this.m_DrawerList.SetItemChecked (_drawerPosition, true);

			m_DrawerList.ItemClick += DrawerListOnItemClick;
		}

		// sets up the drawer toggle button in the current activity
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

			this.m_DrawerList.SetItemChecked (itemClickEventArgs.Position, true);
			this.m_Drawer.CloseDrawer (m_DrawerList);
		}
	}
}

