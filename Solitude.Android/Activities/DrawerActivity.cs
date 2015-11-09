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
	public abstract class DrawerActivity : Activity
	{
		//protected string m_DrawerTitle, m_Title;
		//protected DrawerLayout m_Drawer;
		//protected ListView m_DrawerList;
		protected int Position { get; set; }
		protected SetupDrawer DrawerSetup { get; set; }
		protected FrameLayout Content { get; set; }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.ActivityLayout);

			DrawerSetup = new SetupDrawer (
				//m_DrawerTitle,
				//m_Title,
				//m_Drawer,
				//m_DrawerList,
				Position,
				this
			);

			DrawerSetup.Configure ();
			DrawerSetup.DrawerToggleSetup ();
			Content = FindViewById<FrameLayout>(Resource.Id.content_frame);
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			DrawerSetup.DrawerToggle.SyncState ();
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			if (DrawerSetup.DrawerToggle.OnOptionsItemSelected (item))
				return true;

			return base.OnOptionsItemSelected (item);
		}
	}
}

