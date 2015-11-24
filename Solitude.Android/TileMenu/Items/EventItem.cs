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

namespace DineWithaDane.Android
{
	public class EventItem<T> : FrameLayout where T : Event
	{
		public int Position { get; private set; }

		protected View View { get; set; }
		protected EventAdapter<T> Adapter { get; set; }
		protected Action<View, Event, bool> OnUpdatePos { get; set; }

		public EventItem(Activity context, EventAdapter<T> adapter, 
						Action<int> onExpand, Action<View, Event, bool> onUpdatePos,
						Action<int> onAction1 = null, Action<int> onAction2 = null)
			: base(context)
		{
			Adapter = adapter;
			OnUpdatePos = onUpdatePos;

			View = context.LayoutInflater.Inflate(Resource.Layout.EventCard, null);
			
			View.FindViewById<ImageView>(Resource.Id.expander).Click += (s, e) => onExpand(Position);
			View.FindViewById<Button>(Resource.Id.action1).Click += (s, e) => onAction1(Position);
			View.FindViewById<Button>(Resource.Id.action2).Click += (s, e) => onAction2(Position);

			AddView(View);
		}

		public virtual void UpdatePosition(int pos, bool expanded)
		{
			Position = pos;
			var @event = Adapter.Items[pos];

			View.FindViewById<TextView>(Resource.Id.title).Text = @event.Title;
			View.FindViewById<TextView>(Resource.Id.subtitle).Text = @event.Date.ToString();

			if (expanded)
			{
				View.FindViewById<ImageView>(Resource.Id.expander).SetImageResource(Resource.Drawable.ic_expand_less_black_48dp);
				View.FindViewById<TextView>(Resource.Id.expanded_content).Visibility = ViewStates.Visible;
			}
			else
			{
				View.FindViewById<ImageView>(Resource.Id.expander).SetImageResource(Resource.Drawable.ic_expand_more_black_48dp);
				View.FindViewById<TextView>(Resource.Id.expanded_content).Visibility = ViewStates.Gone;
			}

			OnUpdatePos(View, @event, expanded);
        }
	}


}