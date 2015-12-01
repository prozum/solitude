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
	class TabAdapter : FragmentPagerAdapter, TabLayout.IOnTabSelectedListener, ViewPager.IOnPageChangeListener
	{ 
		public List<TabFragment> Items { get; set; }

		protected ViewPager Pager { get; set; }
		protected TabLayout TabLayout { get; set; }
		protected Context Context { get; set; }

		public override int Count { get { return Items.Count; } }


		public TabAdapter(AppCompatActivity activity, ViewPager pager, TabLayout tablayout)
			: base(activity.SupportFragmentManager)
		{
			Pager = pager;
			TabLayout = tablayout;
			Context = activity;
			Items = new List<TabFragment>();
			TabLayout.RemoveAllTabs();
			TabLayout.SetOnTabSelectedListener(this);
			Pager.Adapter = this;
			Pager.AddOnPageChangeListener(this);
		}

		public void AddTab(int titleres, TabFragment frag)
		{
			var tab = TabLayout.NewTab();
			tab.SetText(titleres);
			AddTab(tab, frag);
		}
		public void AddTab(string title, TabFragment frag)
		{
			var tab = TabLayout.NewTab();
            tab.SetText(title);
			AddTab(tab, frag);
        }
		public void AddTab(TabLayout.Tab tab, TabFragment frag)
		{
			Items.Add(frag);
			TabLayout.AddTab(tab);
			frag.Position = tab.Position;
			NotifyDataSetChanged();
		}

		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return Items[position];
        }

		public void OnTabSelected(TabLayout.Tab tab)
		{
			Items[tab.Position].OnSelected();
			Pager.SetCurrentItem(tab.Position, true);
		}

		public void OnTabUnselected(TabLayout.Tab tab)
		{
		}

		public void OnTabReselected(TabLayout.Tab tab)
		{
		}

		public void OnPageSelected(int position)
		{
			Items[position].OnSelected();
			TabLayout.GetTabAt(position).Select();
		}

		public void OnPageScrollStateChanged(int state)
		{
		}

		public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
		{
		}
	}
}