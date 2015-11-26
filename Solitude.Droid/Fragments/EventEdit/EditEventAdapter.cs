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
	class EditEventAdapter : FragmentPagerAdapter
	{
		protected ViewPager Pager { get; set; }
		protected Button Next { get; set; }
		protected Button Previous { get; set; }
		protected Context Context { get; set; }
		protected List<Android.Support.V4.App.Fragment> Items { get; set; }

		public override int Count { get { return Items.Count; } }


		public EditEventAdapter(AppCompatActivity activity, ViewPager pager, Button next, Button prev)
			: base(activity.SupportFragmentManager)
		{
			Pager = pager;
			Context = activity;
			Next = next;
			Previous = prev;
			Items = new List<Android.Support.V4.App.Fragment>();
			Pager.Adapter = this;
			Pager.HorizontalScrollBarEnabled = false;

			Next.Click += (s, e) => NextPage();
			Previous.Click += (s, e) => PreviousPage();

			Previous.Visibility = ViewStates.Invisible;
        }
		
		public void AddPager(Android.Support.V4.App.Fragment frag)
		{
			if (!(frag is IEditPage))
				throw new ArgumentException("Fragments need to implement ISaveable");

			Items.Add(frag);
			NotifyDataSetChanged();
		}

		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return Items[position];
		}
		
		public void OnTabSelected(TabLayout.Tab tab)
		{
			Pager.SetCurrentItem(tab.Position, true);
		}

		protected void NextPage()
		{
			if (Pager.CurrentItem >= Items.Count)
			{
			}
			else
			{
				if ((Items[Pager.CurrentItem] as IEditPage).IsValidData())
				{
					(Items[Pager.CurrentItem] as IEditPage).SaveInfo();
                    Pager.SetCurrentItem(Pager.CurrentItem + 1, true);

					if (Pager.CurrentItem >= Items.Count - 1)
						Next.Text = "Finish";
				}
			}
		}

		protected void PreviousPage()
		{
			if (Pager.CurrentItem <= 0)
			{
			}
			else
			{
				(Items[Pager.CurrentItem] as IEditPage).SaveInfo();
				Pager.SetCurrentItem(Pager.CurrentItem - 1, true);
				Next.Text = "Next";
            }
		}

	}
}