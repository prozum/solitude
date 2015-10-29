using System;
using Android.Widget;
using Android.Views;
using Android.Graphics;
using Android.App;

namespace DineWithaDane.Android
{
	public abstract class Notification : LinearLayout
	{
		public enum User
		{
			User, Guest, Host
		}

		protected Display display;
		protected Point displaySize = new Point ();

		protected Notification(User user, string title, string text, string time, Color headerColor, Color bodyColor, Activity activity) : base(activity)
		{
			display = activity.WindowManager.DefaultDisplay;
			display.GetSize (displaySize);

			this.LayoutParameters = new ViewGroup.LayoutParams (displaySize.X / 3 * 2, WindowManagerLayoutParams.WrapContent);

			if (user == User.Host) 
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

