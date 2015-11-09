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
	[Activity (Label = "Notifications")]			
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

			Event test1 = new Event("Flan", new DateTime(2015, 10, 27), "Cassiopeia", "Computer gaming and fapping", 20, 20);
			Event test2 = new Event("I-dag", new DateTime(2015, 05, 11), "DE-klubben", "Alle brækker sig og er trælse", 50, 35);
			Event test3 = new Event("Fallout4 release", new DateTime(2015, 11, 10), "Whole world", "Tobias sover ikke hele natten og hans liv går i stå", 1000000, 100000);

			notificationList.Add (new ReviewNotification (Notification.NotificationPosition.Right, test1,  test1.Title, test1.Description, test1.Date.ToString(), this, notificationList));
			notificationList.Add (new ReviewNotification (Notification.NotificationPosition.Right, test2, test2.Title, test2.Description, test2.Date.ToString(), this, notificationList));
			notificationList.Add (new ReviewNotification (Notification.NotificationPosition.Right, test3, test3.Title, test3.Description, test3.Date.ToString(), this, notificationList));

			content.AddView (notificationLayout);

		}
	}
}