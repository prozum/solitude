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

namespace Solitude.Droid
{
	/// <summary>
	/// The card that contains event information.
	/// Is meant to be used in a listview.
	/// </summary>
	public class EventItem<T> : FrameLayout where T : Event
	{
		/// <summary>
		/// The cards position in the listview.
		/// </summary>
		public int Position { get; private set; }

		protected View View { get; set; }
		protected EventAdapter<T> Adapter { get; set; }

		/// <summary>
		/// An action used to customize how the card is drawn.
		/// </summary>
		protected Action<View, Event, bool> OnUpdatePos { get; set; }

		public EventItem(Activity context, EventAdapter<T> adapter, 
						 Action<int> onExpand, Action<View, Event, bool> onUpdatePos,
						 Action<int> onAction1 = null, Action<int> onAction2 = null)
			: base(context)
		{
			Adapter = adapter;
			OnUpdatePos = onUpdatePos;

			// Get card from xml.
			View = context.LayoutInflater.Inflate(Resource.Layout.EventCard, null);
			
			// Set click events.
			View.FindViewById<ImageView>(Resource.Id.expander).Click += (s, e) => onExpand(Position);
			View.FindViewById<Button>(Resource.Id.action1).Click += (s, e) => onAction1(Position);
			View.FindViewById<Button>(Resource.Id.action2).Click += (s, e) => onAction2(Position);

			AddView(View);
		}

		/// <summary>
		/// A method that is run, when the eventcard is reused in the listview.
		/// </summary>
		/// <param name="pos">The eventcards new position.</param>
		/// <param name="expanded">Whether the card is expanded or not.</param>
		public virtual void UpdatePosition(int pos, bool expanded)
		{
			Position = pos;

			// Get event asosiated with this card.
			var @event = Adapter.Items[pos];

			if (expanded)
			{
				// Show content if expanded.
				View.FindViewById<ImageView>(Resource.Id.expander).SetImageResource(Resource.Drawable.ic_expand_less_black_48dp);
				View.FindViewById<TextView>(Resource.Id.expanded_content).Visibility = ViewStates.Visible;
			}
			else
			{
				// Hide content if not expanded.
				View.FindViewById<ImageView>(Resource.Id.expander).SetImageResource(Resource.Drawable.ic_expand_more_black_48dp);
				View.FindViewById<TextView>(Resource.Id.expanded_content).Visibility = ViewStates.Gone;
			}

			// Run the custom 
			OnUpdatePos(View, @event, expanded);
        }
	}


}