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
	/// <summary>
	/// The adapter for controlling a ViewPager with a TabLayout
	/// </summary>
	class TabAdapter : FragmentPagerAdapter, TabLayout.IOnTabSelectedListener, ViewPager.IOnPageChangeListener
	{ 
		/// <summary>
		/// Items in the adapter
		/// </summary>
		public List<TabFragment> Items { get; set; }

		/// <summary>
		/// The ViewPager the TabLayout should controll
		/// </summary>
		protected ViewPager Pager { get; set; }
		protected TabLayout TabLayout { get; set; }

		/// <summary>
		/// The context the TabLayout is a part of
		/// </summary>
		protected Context Context { get; set; }

		/// <summary>
		/// Total number of Items
		/// </summary>
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

		/// <summary>
		/// Add Tab to the TabLayout
		/// </summary>
		public void AddTab(int titleres, TabFragment frag)
		{
			var tab = TabLayout.NewTab();
			tab.SetText(titleres);
			AddTab(tab, frag);
		}
		/// <summary>
		/// Add Tab to the TabLayout
		/// </summary>
		public void AddTab(string title, TabFragment frag)
		{
			var tab = TabLayout.NewTab();
            tab.SetText(title);
			AddTab(tab, frag);
		}
		/// <summary>
		/// Add Tab to the TabLayout
		/// </summary>
		public void AddTab(TabLayout.Tab tab, TabFragment frag)
		{
			Items.Add(frag);
			TabLayout.AddTab(tab);
			frag.Position = tab.Position;
			NotifyDataSetChanged();
		}

		/// <summary>
		/// Get item at a postion
		/// </summary>
		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return Items[position];
        }

		/// <summary>
		/// A method called, when a tab is selected
		/// </summary>
		/// <param name="tab"></param>
		public void OnTabSelected(TabLayout.Tab tab)
		{
			Items[tab.Position].OnSelected();

			// Change the ViewPagers current view to the one corresponing
			// to the tab position
			Pager.SetCurrentItem(tab.Position, true);
		}

		public void OnTabUnselected(TabLayout.Tab tab)
		{
		}

		public void OnTabReselected(TabLayout.Tab tab)
		{
		}

		/// <summary>
		/// A method called, when a page is selected in the ViewPager
		/// </summary>
		public void OnPageSelected(int position)
		{
			Items[position].OnSelected();

			// Select the tab corresponding to the selected page
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