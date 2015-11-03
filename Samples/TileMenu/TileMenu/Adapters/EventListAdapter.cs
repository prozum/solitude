using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TileMenu
{
	public class EventListAdapter : BaseTileListAdapter<Event>
	{
		#region Constructors
		public EventListAdapter(Activity context, List<Event> items) : base(context, items)
		{
			
		}
		#endregion

		#region Public Methods
		public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			EventGroup view = (convertView as EventGroup); // re-use an existing view, if one is available

			if (view == null) // otherwise create a new one
				view = new EventGroup(Context);

			view.Title = Items[groupPosition].Title;
			view.Place = Items[groupPosition].Place;
			view.Date = Items[groupPosition].Date;

			if (isExpanded)
				view.SeperatorVisibility(ViewStates.Gone);
			else
				view.SeperatorVisibility(ViewStates.Visible);

			return view;
		}

		public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			EventItem view = (convertView as EventItem); // re-use an existing view, if one is available

			if (view == null)// otherwise create a new one
				view = new EventItem(Context, (s, e) => 
					{
						return;
					});

			view.Descrition = Items[groupPosition].Description;

			return view;
		}

		public override void Sort(string context)
		{
			switch (context)
			{
				case"Title (A-Z)":
					Items.Sort(new TitleComparer());
					break;
				case"Title (Z-A)":
					Items.Sort(new TitleComparer());
					Items.Reverse();
					break;
				case"Date (Soonest)":
					Items.Sort(new DateComparer());
					break;
				case"Date (Lastest)":
					Items.Sort(new DateComparer());
					Items.Reverse();
					break;
				case"Distance (Closest)":
					Items.Sort(new DistanceComparer());
					break;
				case"Distance (Farthest)":
					Items.Sort(new DistanceComparer());
					Items.Reverse();
					break;
				default:
					throw new NotImplementedException();
			}

			NotifyDataSetChanged();
		}
		#endregion

		#region Private Methods
		#endregion
	}
}

