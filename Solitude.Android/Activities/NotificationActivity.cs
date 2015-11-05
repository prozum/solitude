using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DineWithaDane.Android
{
	[Activity (Label = "Notifications", MainLauncher = false)]			
	public class NotificationActivity : AbstractActivity
	{
		ObservableCollection<Notification> notificationList = new ObservableCollection<Notification>();

		protected override void OnCreate (Bundle bundle)
		{
			drawerPosition = 0;
			base.OnCreate (bundle);

			// Create your application here

			var content = FindViewById<FrameLayout> (Resource.Id.content_frame);

			LinearLayout notificationLayout = new LinearLayout (this);
			notificationLayout.Orientation = Orientation.Vertical;

			notificationList.CollectionChanged += (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) =>
			{
				notificationLayout.RemoveAllViewsInLayout();
				if(notificationList.Count > 0)
				{	
					foreach (var item in notificationList) {
						notificationLayout.AddView(item);
					}

				}
				else 
				{
					TextView nothingHere = new TextView(this);
					nothingHere.Text = "Nothing Here";
					notificationLayout.AddView(nothingHere);
				}
			};

			notificationList.Add (new ReviewNotification (Notification.User.Host, "Review Title #1", "Review Text", DateTime.Now.ToString(), this, notificationList));
			notificationList.Add (new ReviewNotification (Notification.User.Host, "Review Title #2", "Review Text", DateTime.Now.ToString(), this, notificationList));
			notificationList.Add (new ReviewNotification (Notification.User.Host, "Review Title #3", "Review Text", DateTime.Now.ToString(), this, notificationList));

			content.AddView (notificationLayout);

		}
	}
}