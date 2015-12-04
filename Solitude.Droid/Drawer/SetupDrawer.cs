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
	public class SetupDrawer
	{
		
		public DrawerLayout Drawer { get; set; }
		public ListView DrawerList { get; set; }
		public DrawerActivity CurrentActivity { get; set; }
		public int Position { get; set; }
		public ActionBarDrawerToggle DrawerToggle { get; set; }
		
		public SetupDrawer (int position, DrawerActivity currentActivity)
		{
			CurrentActivity = currentActivity;
			Position = position;
			
		}

		public void Configure ()
		{
			Drawer = CurrentActivity.FindViewById<DrawerLayout> (Resource.Id.drawer_layout);
			DrawerList = CurrentActivity.FindViewById<ListView> (Resource.Id.left_drawer);

			DrawerList.Adapter = new DrawerAdapter(CurrentActivity, MainActivity.DrawerActivities,
												   CurrentActivity.Resources.GetStringArray(Resource.Array.drawer_items));

			DrawerList.SetItemChecked (Position, true);

			DrawerList.ItemClick += DrawerListOnItemClick;
		}

		public void DrawerToggleSetup () 
		{
			DrawerToggle = new ActionBarDrawerToggle (
				CurrentActivity, 
				Drawer, 
				Resource.String.open_drawer, 
				Resource.String.close_drawer);

			Drawer.SetDrawerListener (DrawerToggle);
			CurrentActivity.SupportActionBar.SetDisplayHomeAsUpEnabled (true);
			CurrentActivity.SupportActionBar.SetHomeButtonEnabled (true);
		}

		/// <summary>
		/// Drawer item click cases
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="itemClickEventArgs">The ${ParameterType} instance containing the event data.</param>
		private void DrawerListOnItemClick (object sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
		{

			if (itemClickEventArgs.Position == MainActivity.DrawerActivities.Length - 1)
			{
				MainActivity.CIF.Logout(CurrentActivity);
			}
			else if (itemClickEventArgs.Position == Position)
			{

			}
			else
			{
				var nextIntent = new Intent (CurrentActivity, MainActivity.DrawerActivities[itemClickEventArgs.Position].Item2);
				nextIntent.PutExtra("index", itemClickEventArgs.Position);
				CurrentActivity.StartActivity (nextIntent);
			}

			DrawerList.SetItemChecked (itemClickEventArgs.Position, true);
			Drawer.CloseDrawer (DrawerList);
		}
	}
}
