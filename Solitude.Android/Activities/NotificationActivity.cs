
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

namespace DineWithaDane.Android
{
	[Activity (Label = "Notifications", MainLauncher = true)]			
	public class NotificationActivity : AbstractActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ActivityLayout);

			drawerPosition = 0;

			drawerSetup = new SetupDrawer (
				m_DrawerTitle,
				m_Title,
				m_Drawer,
				m_DrawerList,
				drawerPosition,
				this
			);

			drawerSetup.Configure ();
			drawerSetup.DrawerToggleSetup ();

			// Create your application here
		}
	}
}

