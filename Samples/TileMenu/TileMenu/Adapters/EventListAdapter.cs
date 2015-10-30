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
		#region Fields
		EventHandler onCancel;
		#endregion

		#region Constructors
		public EventListAdapter(Activity context, List<Event> items, EventHandler onCancel) : base(context, items)
		{
			this.onCancel = onCancel;
		}
		#endregion

		#region Public Methods
		public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
		{
			View view = base.GetGroupView(groupPosition, isExpanded, convertView, parent);

			// add new infomation to view
			view.FindViewById<TextView>(Resource.Id.Title).Text = "Title: " + Items[groupPosition].Title; 
			view.FindViewById<TextView>(Resource.Id.Info1).Text = "Place: " + Items[groupPosition].Place; 
			view.FindViewById<TextView>(Resource.Id.Info2).Text = "Date: " + Items[groupPosition].Date.ToString(@"dd\/MM\/yyyy HH:mm"); 

			return view;
		}

		public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available

			if (view == null)// otherwise create a new one
			{
				view = Context.LayoutInflater.Inflate(Resource.Layout.ListItem, null);

				var button1 = view.FindViewById<Button>(Resource.Id.Button1);

				// adding accept and decline button functionality
				button1.Click += onCancel;
				button1.Text = "Cancel Event";

				view.FindViewById<Button>(Resource.Id.Button2).Visibility = ViewStates.Gone;
			}

			// add new infomation to view
			view.FindViewById<TextView>(Resource.Id.Info).Text = "Description: " + Items[groupPosition].Description; 

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

