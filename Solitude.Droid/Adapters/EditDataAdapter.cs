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
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Views.InputMethods;

namespace Solitude.Droid
{
	public abstract class EditDataAdapter : FragmentPagerAdapter, ViewPager.IOnPageChangeListener, IMenuItemOnMenuItemClickListener
	{
		protected IMenuItem Next { get; set; }
		protected IMenuItem Previous { get; set; }
		protected AppCompatActivity Activity { get; set; }
		protected ViewPager Pager { get; set; }
		protected FloatingActionButton Finish { get; set; }
		protected ProgressBar Progress { get; set; }
		protected List<EditFragment> Items { get; set; }

		public int Selected { get; set; }
		public override int Count { get { return Items.Count; } }

		public EditDataAdapter(AppCompatActivity activity, ViewPager pager, FloatingActionButton finish, ProgressBar progress, 
							   IMenuItem prev = null, IMenuItem next = null)
			: base(activity.SupportFragmentManager)
		{
			Activity = activity;
			Pager = pager;
			Finish = finish;
			Progress = progress;
			Items = new List<EditFragment>();

			Finish.Click += (s, e) => NextPage();

			Pager.Adapter = this;
			Progress.Progress = 1;

			SetNextButton(next);
			SetPreviousButton(prev);
		}

		public virtual void AddPager(EditFragment frag)
		{
			Items.Add(frag);
			Progress.Max = Items.Count;
			NotifyDataSetChanged();
		}

		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return Items[position];
		}

		public virtual void NextPage()
		{
			if (Items[Pager.CurrentItem].IsValidData())
			{
				Items[Pager.CurrentItem].SaveInfo();

				if (Pager.CurrentItem >= Items.Count - 1)
				{
					UpdateData();
				}
				else
				{
					if (Items[Pager.CurrentItem + 1].HidesKeyboard)
					{
						InputMethodManager imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
						imm.HideSoftInputFromWindow(Pager.WindowToken, 0);
					}

					Pager.SetCurrentItem(Pager.CurrentItem + 1, true);
					Progress.Progress++;

					if (Pager.CurrentItem >= Items.Count - 1)
						Finish.Visibility = ViewStates.Visible;
				}
			}
		}

		public virtual void PreviousPage()
		{
			if (Pager.CurrentItem <= 0)
			{
				BackWarning();
			}
			else
			{

				Items[Pager.CurrentItem].SaveInfo();
				Pager.SetCurrentItem(Pager.CurrentItem - 1, true);
				Progress.Progress--;
				Finish.Visibility = ViewStates.Gone;
			}
		}

		protected abstract void UpdateData();

		protected abstract void BackWarning();

		protected abstract void Back();

		public virtual void OnPageScrollStateChanged(int state)
		{
		}

		public virtual void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
		{
		}

		public virtual void OnPageSelected(int position)
		{
			if (position > Selected)
			{
				if (Items[Selected].IsValidData())
				{
					if (Items[position].HidesKeyboard)
					{
						InputMethodManager imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
						imm.HideSoftInputFromWindow(Pager.WindowToken, 0);
					}

					Items[Selected].SaveInfo();
					Selected = position;
					Progress.Progress = position + 1;

					if (Selected >= Items.Count - 1)
						Finish.Visibility = ViewStates.Visible;

					if (Selected >= Count - 1 && Next != null)
						Next.SetVisible(false);
					else if (!(Selected <= 0 && Previous != null))
						Previous.SetVisible(true);
				}
				else
				{
					Pager.SetCurrentItem(Selected, true);
				}
			}
			else
			{
				Selected = position;
				Progress.Progress = position + 1;
				Finish.Visibility = ViewStates.Gone;

				if (Selected <= 0 && Previous != null)
					Previous.SetVisible(false);
				else if (!(Selected >= Count - 1) && Next != null)
					Next.SetVisible(true);
			}
			
			
		}

		public virtual bool OnMenuItemClick(IMenuItem item)
		{
			if (item.ItemId == Previous.ItemId)
				PreviousPage();
			else if (item.ItemId == Next.ItemId)
				NextPage();
			else
				return false;

			return true;
		}

		public virtual void SetNextButton(IMenuItem next)
		{
			Next = next;

			if (Next != null)
			{
				if (Selected >= Count - 1)
					Next.SetVisible(false);

				Next.SetOnMenuItemClickListener(this);
			}
		}

		public virtual void SetPreviousButton(IMenuItem prev)
		{
			Previous = prev;

			if (Previous != null)
			{
				if (Selected <= 0)
					Previous.SetVisible(false);

				Previous.SetOnMenuItemClickListener(this);
			}
		}
	}
}