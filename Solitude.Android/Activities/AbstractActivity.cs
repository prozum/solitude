
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

namespace DineWithaDane.Android
{
	[Activity (Label = "AbstractActivity")]			
	public class AbstractActivity : Activity
	{
		protected string m_DrawerTitle, m_Title;
		protected DrawerLayout m_Drawer;
		protected ListView m_DrawerList;
		protected int drawerPosition;
		protected SetupDrawer drawerSetup;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			drawerSetup.DrawerToggle.SyncState ();
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (drawerSetup.DrawerToggle.OnOptionsItemSelected (item))
				return true;

			return base.OnOptionsItemSelected (item);
		}
	}
}

