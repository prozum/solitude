
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
		protected FrameLayout Content
		{
			get;
			set;
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.ActivityLayout);

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
			Content = FindViewById<FrameLayout>(Resource.Id.content_frame);
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

