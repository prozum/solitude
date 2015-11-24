using System;
using Android.App;
using Android.Graphics;
using Android.Widget;
using Android.Views;
using System.Collections.ObjectModel;

namespace Solitude.Droid
{
	public class OfferNotification : Notification
	{
		public OfferNotification (Event e, Activity activity, ObservableCollection<Notification> notificationList)
			: base(e.Title, string.Format("{0}/{1} participants", e.SlotsTotal - e.SlotsTaken, e.SlotsTotal), e.Date.ToString(), activity, notificationList)
		{
			LeftButton.Text = "Decline";


			RightButton.Text = "Accept";

		}
	}
}

