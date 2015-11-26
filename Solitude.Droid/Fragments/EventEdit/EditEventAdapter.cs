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
		protected AppCompatActivity Activity { get; set; }
		protected ViewPager Pager { get; set; }
		protected Button Next { get; set; }
		protected Button Previous { get; set; }
		protected Context Context { get; set; }
		protected List<Android.Support.V4.App.Fragment> Items { get; set; }

		public override int Count { get { return Items.Count; } }


		public EditEventAdapter(AppCompatActivity activity, ViewPager pager, Button next, Button prev)
			: base(activity.SupportFragmentManager)
		{
			Activity = activity;
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
			if (Pager.CurrentItem >= Items.Count - 1)
			{
				var type = Activity.Intent.GetStringExtra("type");
				var title = Activity.Intent.GetStringExtra("title");
				var description = Activity.Intent.GetStringExtra("description");
				var year = Activity.Intent.GetIntExtra("date year", 0);
				var month = Activity.Intent.GetIntExtra("date month", 0);
				var day = Activity.Intent.GetIntExtra("date day", 0);
				var hour = Activity.Intent.GetIntExtra("date hour", 0);
				var minut = Activity.Intent.GetIntExtra("date minutte", 0);
				var place = Activity.Intent.GetStringExtra("place");
				var max = Activity.Intent.GetIntExtra("maxslots", 0);
				var taken = Activity.Intent.GetIntExtra("slotstaken", 0);
				var id = Activity.Intent.GetIntExtra("id", 0);

				var @event = new Event()
				{
					Address = place,
					Date = new DateTimeOffset(year, month, day, hour, minut, 0, new TimeSpan(0)),
                    Description = description,
					Id = id,
					SlotsTaken = taken,
					SlotsTotal = max,
					Title = title
				};

				bool completed = false;

				if (type == "edit")
					completed = MainActivity.CIF.UpdateEvent(@event);
				else if (type == "new")
					completed = MainActivity.CIF.CreateEvent(@event);
				else
					throw new ArgumentException("type was either edit or new");

				if (!completed)
				{
					var dialog = new Android.Support.V7.App.AlertDialog.Builder(Activity);
					dialog.SetMessage(Activity.Resources.GetString(Resource.String.message_error_event_update_event) + "\n" + MainActivity.CIF.LatestError);
					dialog.SetNegativeButton(Resource.String.ok, (s, earg) => { });
					dialog.Show();
				}

				Activity.Finish();
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