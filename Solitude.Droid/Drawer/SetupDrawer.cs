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
using Android.Support.V7.Widget;
using Android.Support.V7.App;
using Android.Support.V4.Widget;

namespace Solitude.Droid
{			
	/// <summary>
	/// A class for setting up the drawer
	/// </summary>
	public class SetupDrawer
	{
		/// <summary>
		/// The drawer to set up
		/// </summary>
		public DrawerLayout Drawer { get; set; }

		/// <summary>
		/// The listview inside the drawer
		/// </summary>
		public ListView DrawerList { get; set; }

		/// <summary>
		/// The activity the drawer is a part of
		/// </summary>
		public DrawerActivity CurrentActivity { get; set; }

		/// <summary>
		/// The currently seleted item in the drawer
		/// </summary>
		public int Position { get; set; }

		/// <summary>
		/// The button for expanding the drawer
		/// </summary>
		public ActionBarDrawerToggle DrawerToggle { get; set; }
		
		public SetupDrawer (int position, DrawerActivity currentActivity)
		{
			CurrentActivity = currentActivity;
			Position = position;
		}

		/// <summary>
		/// A method that configures the drawer
		/// </summary>
		public void Configure ()
		{
			// Find relevant views
			Drawer = CurrentActivity.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			DrawerList = CurrentActivity.FindViewById<ListView> (Resource.Id.left_drawer);

			// Set drawer adapter
			DrawerList.Adapter = new DrawerAdapter(CurrentActivity, MainActivity.DrawerActivities,
												   CurrentActivity.Resources.GetStringArray(Resource.Array.drawer_items));

			// Set the seleted item of the drawer, to be the current activity
			DrawerList.SetItemChecked (Position, true);
			
			DrawerList.ItemClick += DrawerListOnItemClick;
		}

		/// <summary>
		/// A method for setting up the drawer expanding button
		/// </summary>
		public void DrawerToggleSetup () 
		{
			DrawerToggle = new ActionBarDrawerToggle (
				CurrentActivity, 
				Drawer, 
				Resource.String.open_drawer, 
				Resource.String.close_drawer);

			// Set drawers listener to be the toggle
			Drawer.SetDrawerListener (DrawerToggle);
			CurrentActivity.SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			CurrentActivity.SupportActionBar.SetHomeButtonEnabled (true);
		}

		/// <summary>
		/// A method call when an item on the drawer is clicked
		/// </summary>
		private void DrawerListOnItemClick (object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
		{
			// If the last item is clicked, then loggout
			if (itemClickEventArgs.Position == MainActivity.DrawerActivities.Length - 1)
			{
				MainActivity.CIF.Logout(CurrentActivity);
			}
			// Do nothing if the selected item is clicked
			else if (itemClickEventArgs.Position == Position)
			{

			}
			else
			{
				// Make the intent for going to the seleted item, and start it.
				var nextIntent = new Intent (CurrentActivity, MainActivity.DrawerActivities[itemClickEventArgs.Position].Item2);
				nextIntent.PutExtra("index", itemClickEventArgs.Position);
				CurrentActivity.StartActivity (nextIntent);
			}

			// Set the seleted item to be the item seleted
			DrawerList.SetItemChecked (itemClickEventArgs.Position, true);

			// Close the drawer
			Drawer.CloseDrawer (DrawerList);
		}
	}
}
