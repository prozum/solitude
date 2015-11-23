using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;

namespace DineWithaDane.Android
{
	public class EventAdapter : BaseCardAdapter<Event>
	{
		public EventAdapter(Activity context, List<Event> items)
			: base(context, items) { }

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView;

			if (view == null)
				view = Context.LayoutInflater.Inflate(Resource.Layout.EventCard, null);

			view.FindViewById<TextView>(Resource.Id.title).Text = Items[position].Title;

			return view;
		}
	}
}

