
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
using Android.Support.V7.App;
using Android.Support.Design.Widget;

namespace Solitude.Droid
{
	[Activity(Label = "Host Event")]			
	public class HostEventActivity : AppCompatActivity
	{
		protected EditEventAdapter Adapter { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.SignUp);
			var finish = FindViewById<FloatingActionButton>(Resource.Id.finish);
			var viewpager = FindViewById<ViewPager>(Resource.Id.signUpViewPager);
			var progress = FindViewById<ProgressBar>(Resource.Id.signupProgress);
			Adapter = new EditEventAdapter(this, viewpager, finish,  progress);

			viewpager.Adapter = Adapter;
			viewpager.AddOnPageChangeListener(Adapter);

			Adapter.AddPager(new EventInfoFragment());
			Adapter.AddPager(new EventDateFragment());
			Adapter.AddPager(new EventTimeFragment());
		}
		
		public override void OnBackPressed()
		{
			Adapter.PreviousPage();
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			var prev = menu.Add(0, 0, 0, "Prev").SetIcon(Resource.Drawable.ic_arrow_back_black_24dp);
			var next = menu.Add(0, 1, 0, "Next").SetIcon(Resource.Drawable.ic_arrow_forward_black_24dp);
			next.SetShowAsAction(ShowAsAction.IfRoom);
			prev.SetShowAsAction(ShowAsAction.IfRoom);

			Adapter.SetNextButton(next);
			Adapter.SetPreviousButton(prev);

			return true;
		}
	}
}

