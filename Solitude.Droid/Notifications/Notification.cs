using System;
using Android.Widget;
using Android.Views;
using Android.Graphics;
using Android.App;
using System.Collections.ObjectModel;
using Android.Graphics.Drawables;

namespace Solitude.Droid
{
	public abstract class Notification : LinearLayout
	{
		protected Display display;
		protected Point displaySize = new Point ();
		protected ObservableCollection<Notification> notificationList;
		protected Color BodyColor { get; private set; }
		protected Color HeaderColor { get; private set;}
		protected LinearLayout ButtonKeeper { get; set; }
		protected Button LeftButton { private set; get; }
		protected Button RightButton { private set; get; }
		private ImageView Icon;

		protected Notification(string title, string text, string time, Activity act, ObservableCollection<Notification> notiList)
			: this (title, text, time, Color.DarkOrange, Color.Orange, act, notiList)
		{
		}

		protected Notification(string title, string text, string time, Color headerColor, Color bodyColor, Activity activity, ObservableCollection<Notification> notificationList) : base(activity)
		{
			//Store colors
			this.BodyColor = bodyColor;
			this.HeaderColor = headerColor;

			//Gets display size and stores it
			display = activity.WindowManager.DefaultDisplay;
			display.GetSize (displaySize);
			this.notificationList = notificationList;

			Orientation = Orientation.Vertical;
			LayoutParameters = new ViewGroup.LayoutParams (displaySize.X, WindowManagerLayoutParams.WrapContent);
			SetGravity (GravityFlags.Left);

			var textViewTitle = new TextView (this.Context);
			textViewTitle.Text = title;
			textViewTitle.TextSize = 20;
			textViewTitle.SetTypeface(null, TypefaceStyle.Bold);
			textViewTitle.SetBackgroundColor (headerColor);

			var textViewTime = new TextView (this.Context);
			textViewTime.SetTypeface(null, TypefaceStyle.Italic);
			textViewTime.Text = time;
			textViewTime.SetBackgroundColor (bodyColor);

			var textViewText = new TextView (this.Context);
			textViewText.Text = text;
			textViewText.SetBackgroundColor (bodyColor);

			AddView (textViewTitle);
			AddView (textViewTime);
			AddView (textViewText);

			placeButtons(activity);

			initializeImageView ();

			this.SetX(0);
		}

		/// <summary>
		/// Places the buttons on notifications.
		/// </summary>
		/// <param name="activity">Activity.</param>
		private void placeButtons(Activity activity)
		{
			//Initialize the buttonkeeper
			ButtonKeeper = new LinearLayout (activity);
			ButtonKeeper.SetBackgroundColor(BodyColor);
			ButtonKeeper.Orientation = Orientation.Horizontal;
			//And the buttons
			LeftButton = new Button(activity);
			LeftButton.Gravity = GravityFlags.Center;
			LeftButton.SetWidth(displaySize.X / 3);
			RightButton = new Button(activity);
			RightButton.Gravity = GravityFlags.Center;
			RightButton.SetWidth (displaySize.X / 3);
			ButtonKeeper.AddView (LeftButton);
			ButtonKeeper.AddView (RightButton);

			AddView (ButtonKeeper);
		}

		private void initializeImageView()
		{
			this.Icon = new ImageView(this.Context);

			Icon.SetMaxHeight(base.LayoutParameters.Height);
			Icon.SetAdjustViewBounds(true);

			//LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
			//lp.Gravity = GravityFlags.Right;
			//Icon.LayoutParameters = lp;

			Icon.SetX(displaySize.X - Icon.MaxWidth - 5);

			Icon.Visibility = ViewStates.Invisible;
			AddView(Icon);
		}

		public void PlaceNotificationImage(int resourceID)
		{
			Icon.SetImageResource(resourceID);
			Icon.Visibility = ViewStates.Visible;
		}
	}
}

