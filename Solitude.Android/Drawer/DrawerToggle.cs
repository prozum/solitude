using System;
using Android.Support.V4.App;
using Android.App;
using Android.Support.V4.Widget;
using Android.Views;

namespace DineWithaDane.Android
{
	public class DrawerToggle : ActionBarDrawerToggle
	{
		DrawerActivity currentActivity;

		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.DrawerToggle" that sets up a drawerToggle/> class.
		/// </summary>
		/// <param name="currentActivity">Current activity.</param>
		/// <param name="layout">Layout.</param>
		/// <param name="imageResource">Image resource.</param>
		/// <param name="openDrawerDesc">Open drawer desc.</param>
		/// <param name="closeDrawerDesc">Close drawer desc.</param>
		public DrawerToggle (DrawerActivity currentActivity, DrawerLayout layout, int imageResource, int openDrawerDesc, int closeDrawerDesc) 
			: base (currentActivity, layout, imageResource, openDrawerDesc, closeDrawerDesc)
		{
			currentActivity = currentActivity;
		}
	}
}

