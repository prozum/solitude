using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Runtime.InteropServices;
using Android.Support.Design.Widget;
using Android.Support.V4.View;

namespace Solitude.Droid
{
	class TabAdapter : FragmentPagerAdapter, TabLayout.IOnTabSelectedListener
    {
		protected ViewPager Pager { get; set; }
		protected TabLayout TabLayout { get; set; }
		protected Context Context { get; set; }
		protected List<Android.Support.V4.App.Fragment> Items { get; set; }

		public override int Count
		{
			get
			{
				return Items.Count;
            }
		}


		public TabAdapter(AppCompatActivity activity, ViewPager pager, TabLayout tablayout)
			: base(activity.SupportFragmentManager)
		{
			Pager = pager;
			TabLayout = tablayout;
			Context = activity;
			Items = new List<Android.Support.V4.App.Fragment>();
			Pager.Adapter = this;
		}

		public void AddTab(string title, Android.Support.V4.App.Fragment frag)
		{
			var tab = TabLayout.NewTab();
            tab.SetText(title);
			TabLayout.AddTab(tab);
			Items.Add(frag);
			NotifyDataSetChanged();
		}

		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return Items[position];
        }

		public void OnTabReselected(TabLayout.Tab tab)
		{
		}

		public void OnTabSelected(TabLayout.Tab tab)
		{
			Pager.SetCurrentItem(tab.Position, true);
		}

		public void OnTabUnselected(TabLayout.Tab tab)
		{
		}
	}
}