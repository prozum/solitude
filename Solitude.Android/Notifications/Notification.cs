using System;
using Android.Widget;
using Android.Views;
using Android.Graphics;
using Android.App;
using System.Collections.ObjectModel;

namespace DineWithaDane.Android
{
	public abstract class Notification : LinearLayout
	{
		public enum NotificationPosition
		{
			Left, Right
		}

		protected Display display;
		protected Point displaySize = new Point ();
		protected ObservableCollection<Notification> notificationList;

		protected Notification(NotificationPosition position, string title, string text, string time, Color headerColor, Color bodyColor, Activity activity, ObservableCollection<Notification> notificationList) : base(activity)
		{
			display = activity.WindowManager.DefaultDisplay;
			display.GetSize (displaySize);
			this.notificationList = notificationList;

			LayoutParameters = new ViewGroup.LayoutParams (displaySize.X / 3 * 2, WindowManagerLayoutParams.WrapContent);

			if (position == NotificationPosition.Right) 
				this.SetX (displaySize.X / 3);

			SetGravity (GravityFlags.Left);
			Orientation = Orientation.Vertical;

			var textViewTitle = new TextView (activity);
			textViewTitle.Text = title;
			textViewTitle.TextSize = 20;
			textViewTitle.SetBackgroundColor (headerColor);

			var textViewTime = new TextView (activity);
			textViewTime.Text = time;
			textViewTime.SetBackgroundColor (bodyColor);

			var textViewText = new TextView (activity);
			textViewText.Text = text;
			textViewText.SetBackgroundColor (bodyColor);

			AddView (textViewTitle);
			AddView (textViewTime);
			AddView (textViewText);
		}
	}
}

