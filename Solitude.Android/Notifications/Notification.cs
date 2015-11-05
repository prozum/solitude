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
		public enum Direction
		{
			User, Guest, Host
		}

		protected Display display;
		protected Point displaySize = new Point ();
		protected ObservableCollection<Notification> notificationList;

		protected Notification(Direction user, string title, string text, string time, Color headerColor, Color bodyColor, Activity activity, ObservableCollection<Notification> notificationList) : base(activity)
		{
			display = activity.WindowManager.DefaultDisplay;
			display.GetSize (displaySize);
			this.notificationList = notificationList;

			this.LayoutParameters = new ViewGroup.LayoutParams (displaySize.X / 3 * 2, WindowManagerLayoutParams.WrapContent);

			if (user == Direction.Host) 
			{
				this.SetX (displaySize.X / 3);
			}

			this.SetGravity (GravityFlags.Left);
			this.Orientation = Orientation.Vertical;

			TextView textViewTitle = new TextView (activity);
			textViewTitle.Text = title;
			textViewTitle.TextSize = 20;
			textViewTitle.SetBackgroundColor (headerColor);

			TextView textViewTime = new TextView (activity);
			textViewTime.Text = time;
			textViewTime.SetBackgroundColor (bodyColor);

			TextView textViewText = new TextView (activity);
			textViewText.Text = text;
			textViewText.SetBackgroundColor (bodyColor);

			this.AddView (textViewTitle);
			this.AddView (textViewTime);
			this.AddView (textViewText);
		}
	}
}

