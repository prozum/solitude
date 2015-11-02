using System;
using Android.Support.V4.App;
using Android.App;
using Android.Support.V4.Widget;
using Android.Views;

namespace DineWithaDane.Android
{
	public class DrawerToggle : ActionBarDrawerToggle
	{
		AbstractActivity _currentActivity;

		public DrawerToggle (AbstractActivity currentActivity, DrawerLayout layout, int imageResource, int openDrawerDesc, int closeDrawerDesc) 
			: base (currentActivity, layout, imageResource, openDrawerDesc, closeDrawerDesc)
		{
			_currentActivity = currentActivity;
		}

		public override void OnDrawerOpened (View drawerView)
		{

			base.OnDrawerOpened (drawerView);
		}

		public override void OnDrawerClosed (View drawerView)
		{
			base.OnDrawerClosed (drawerView);
		}

		public override void OnDrawerSlide (View drawerView, float slideOffset)
		{
			base.OnDrawerSlide (drawerView, slideOffset);
		}
	}
}

