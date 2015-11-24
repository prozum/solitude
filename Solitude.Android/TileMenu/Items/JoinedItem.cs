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

namespace DineWithaDane.Android.TileMenu.Items
{
	class JoinedItem : FrameLayout
	{
		public Event Event { get; private set; }

		protected View View { get; set; }
		protected JoinedEventListAdapter Adapter { get; set; }

		public JoinedItem(Activity context, JoinedEventListAdapter adapter, EventHandler onLeave) 
			: base(context)
		{
			View = context.LayoutInflater.Inflate(Resource.Layout.JoinedCard, null);

			var leave = View.FindViewById<Button>(Resource.Id.action1);
			leave.Click += (s, e) => { Adapter.Remove(Event); };
			leave.Click += onLeave;

			AddView(View);
        }

		public void SetEvent(Event @event)
		{
			Event = @event;

			View.FindViewById<TextView>(Resource.Id.title).Text = Event.Title;
			View.FindViewById<TextView>(Resource.Id.subtitle).Text = Event.Date + "\n";
			View.FindViewById<TextView>(Resource.Id.subtitle).Text += Event.Address;
			View.FindViewById<TextView>(Resource.Id.expanded_content).Text = Event.Description;
		}
	}


}