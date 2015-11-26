
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
using Android.Support.V4.View;

namespace Solitude.Droid
{
	[Activity(Label = "Host Event")]			
	public class HostEventActivity : DrawerActivity
	{
		protected EditEventAdapter Adapter { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			var layout = LayoutInflater.Inflate(Resource.Layout.SignUp, null);
			var next = layout.FindViewById<Button>(Resource.Id.signUpNextBtn);
			var back = layout.FindViewById<Button>(Resource.Id.signUpPreviousBtn);
			var viewpager = layout.FindViewById<ViewPager>(Resource.Id.signUpViewPager);
			Adapter = new EditEventAdapter(this, viewpager, next, back);

			viewpager.Adapter = Adapter;

			Adapter.AddPager(new EventInfoFragment());
			Adapter.AddPager(new EventDateFragment());
			Adapter.AddPager(new EventTimeFragment());

			Content.AddView(layout);
		}
		
		public override void OnBackPressed()
		{
			Adapter.PreviousPage();
        }
	}
}

