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

		public DrawerToggle (DrawerActivity currentActivity, DrawerLayout layout, int imageResource, int openDrawerDesc, int closeDrawerDesc) 
			: base (currentActivity, layout, imageResource, openDrawerDesc, closeDrawerDesc)
		{
			currentActivity = currentActivity;
		}
	}
}

