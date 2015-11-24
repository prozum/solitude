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

namespace Solitude.Droid
{
	[Activity (Label = "Notifications", Icon = "@drawable/Notification_Icon")]			
	public class NotificationActivity : DrawerActivity
	{
		ObservableCollection<Notification> notificationList = new ObservableCollection<Notification>();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			var content = new FrameLayout(this);
			var scroll = new ScrollView(this);
			scroll.AddView(content);
			Content.AddView(scroll);

			LinearLayout notificationLayout = new LinearLayout (this);
			notificationLayout.Orientation = Orientation.Vertical;


			notificationList.CollectionChanged += (sender, e) =>
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

			Event flan = new Event("Flan", new DateTime(2015, 10, 27), "Cassiopeia", "Computer gaming and fapping", 20, 20);
			Event iDag = new Event("I-dag", new DateTime(2015, 05, 11), "DE-klubben", "Alle brækker sig og er trælse", 50, 35);
			Event fo4 = new Event("Fallout4 playing", new DateTime(2015, 11, 11), "Lee's Shithole", "The keyboard must be hacked!", 1, 1);
			Event julefrokos = new Event("Julefrokost", new DateTime(2015, 11, 27), "Tobias' Crib", "Lots of food and schnaps!", 7, 7);

			notificationList.Add (new ReviewNotification (flan, this, notificationList));
			notificationList.Add (new ReviewNotification (iDag, this, notificationList));
			notificationList.Add (new EventReminderNotification (fo4, this, notificationList));
			notificationList.Add (new OfferNotification (julefrokos, this, notificationList));

			content.AddView (notificationLayout);

		}
	}
}