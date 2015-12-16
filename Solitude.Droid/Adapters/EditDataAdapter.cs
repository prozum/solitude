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
	/// <summary>
	/// The adapter for getting the fragments for the edit panels on signup and edit/new event.
	/// </summary>
	public abstract class EditDataAdapter : FragmentPagerAdapter, ViewPager.IOnPageChangeListener, IMenuItemOnMenuItemClickListener
	{
		/// <summary>
		/// A MenuItem for going to the next fragment.
		/// </summary>
		protected IMenuItem Next { get; set; }

		/// <summary>
		/// A MenuItem for going to the previous fragment.
		/// </summary>
		protected IMenuItem Previous { get; set; }

		/// <summary>
		/// The activity the adapter is a part of.
		/// </summary>
		protected AppCompatActivity Activity { get; set; }

		/// <summary>
		/// The viewpager this adapter is contained in
		/// </summary>
		protected ViewPager Pager { get; set; }

		/// <summary>
		/// The button for finishing the information edition process.
		/// </summary>
		protected FloatingActionButton Finish { get; set; }

		/// <summary>
		/// The ProgressBar showing how far the user is through the information editing.
		/// </summary>
		protected ProgressBar Progress { get; set; }

		/// <summary>
		/// All the fragments in the adapter.
		/// </summary>
		protected List<EditFragment> Items { get; set; }

		/// <summary>
		/// The item currently on the screen.
		/// </summary>
		public int Selected { get; set; }

		/// <summary>
		/// The total number of Items.
		/// </summary>
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

		/// <summary>
		/// Add a page to the ViewPager.
		/// </summary>
		public virtual void AddPager(EditFragment frag)
		{
			Items.Add(frag);
			Progress.Max = Items.Count;
			NotifyDataSetChanged();
		}

		/// <summary>
		/// Get the item at a position.
		/// </summary>
		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return Items[position];
		}

		/// <summary>
		/// Go to the next page in the ViewPager.
		/// </summary>
		public virtual void NextPage()
		{
			if (Items[Pager.CurrentItem].IsValidData())
			{
				Items[Pager.CurrentItem].SaveInfo();

				// If current page is not the last.
				if (Pager.CurrentItem >= Items.Count - 1)
				{
					UpdateData();
				}
				else
				{
					// Hide keyboard if the currentitems HidesKeyboard poperty is true.
					if (Items[Pager.CurrentItem + 1].HidesKeyboard)
					{
						InputMethodManager imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
						imm.HideSoftInputFromWindow(Pager.WindowToken, 0);
					}

					// Set the current item to be the next item.
					Pager.SetCurrentItem(Pager.CurrentItem + 1, true);
					Progress.Progress++;

					// Update finish button is visible only on the last page
					if (Pager.CurrentItem >= Items.Count - 1)
						Finish.Visibility = ViewStates.Visible;
				}
			}
		}

		/// <summary>
		/// Go to the previous page in the ViewPager.
		/// </summary>
		public virtual void PreviousPage()
		{
			// If current page is the first
			if (Pager.CurrentItem <= 0)
			{
				BackWarning();
			}
			else
			{
				Items[Pager.CurrentItem].SaveInfo();

				// Set the current item to be the previous item.
				Pager.SetCurrentItem(Pager.CurrentItem - 1, true);
				Progress.Progress--;
				Finish.Visibility = ViewStates.Gone;
			}
		}

		/// <summary>
		/// A method for updating data when the finish button is pressed.
		/// </summary>
		protected abstract void UpdateData();

		/// <summary>
		/// A method called, to giv the user a warning, when the user is about to leave
		/// the process, without finishing.
		/// </summary>
		protected abstract void BackWarning();

		/// <summary>
		/// A method for going to the activty that this ViewPager should lead to.
		/// </summary>
		protected abstract void Back();

		public virtual void OnPageScrollStateChanged(int state)
		{
		}

		public virtual void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
		{
		}

		/// <summary>
		/// A method called when the ViewPager selects a new page
		/// </summary>
		public virtual void OnPageSelected(int position)
		{
			// If the postion changed, to something above the current
			if (position > Selected)
			{
				if (Items[Selected].IsValidData())
				{
					// Hide keyboard if the currentitems HidesKeyboard poperty is true.
					if (Items[position].HidesKeyboard)
					{
						InputMethodManager imm = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
						imm.HideSoftInputFromWindow(Pager.WindowToken, 0);
					}

					Items[Selected].SaveInfo();
					Selected = position;
					Progress.Progress = position + 1;

					// Update finish button is visible only on the last page
					if (Selected >= Items.Count - 1)
						Finish.Visibility = ViewStates.Visible;

					//Update the navigation buttons visibility
					if (Selected >= Count - 1 && Next != null)
						Next.SetVisible(false);
					else if (!(Selected <= 0 && Previous != null))
						Previous.SetVisible(true);
				}
				else
				{
					// If the page couldn't save the data, revert the ViewPager back to the page
					// that is missing data
					Pager.SetCurrentItem(Selected, true);
				}
			}
			else
			{
				Selected = position;
				Progress.Progress = position + 1;
				Finish.Visibility = ViewStates.Gone;

				//Update the navigation buttons visibility
				if (Selected <= 0 && Previous != null)
					Previous.SetVisible(false);
				else if (!(Selected >= Count - 1) && Next != null)
					Next.SetVisible(true);
			}
			
			
		}

		/// <summary>
		/// A method called when one of the navigation buttons are clicked.
		/// </summary>
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

		/// <summary>
		/// A method for setting the Next Button
		/// </summary>
		public virtual void SetNextButton(IMenuItem next)
		{
			Next = next;

			if (Next != null)
			{
				// Update visibility base on position
				if (Selected >= Count - 1)
					Next.SetVisible(false);

				// Set the buttons listner to this
				Next.SetOnMenuItemClickListener(this);
			}
		}

		/// <summary>
		/// A method for setting the Next Button
		/// </summary>
		public virtual void SetPreviousButton(IMenuItem prev)
		{
			Previous = prev;

			if (Previous != null)
			{
				// Update visibility base on position
				if (Selected <= 0)
					Previous.SetVisible(false);

				// Set the buttons listner to this
				Previous.SetOnMenuItemClickListener(this);
			}
		}
	}
}