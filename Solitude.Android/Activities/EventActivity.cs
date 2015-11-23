using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;

namespace DineWithaDane.Android
{
	[Activity(Label = "Events", Icon = "@drawable/Events_Icon")]
	public class EventActivity : DrawerActivity
	{
		protected JoinedEventList Tilelist { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			// setting up and drawer
			base.OnCreate(savedInstanceState);
			ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

			var recommends = ActionBar.NewTab();
			var joined = ActionBar.NewTab();
			var hosted = ActionBar.NewTab();

			recommends.SetText("Recommended");
			joined.SetText("Joined");
			hosted.SetText("Hosted");

			recommends.TabSelected += (sender, e) => 
				{
					Content.RemoveAllViews();
					Content.AddView(LayoutInflater.Inflate(Resource.Layout.Profile, null));
				};
			joined.TabSelected += (sender, e) => 
				{
					Content.RemoveAllViews();
					Content.AddView(LayoutInflater.Inflate(Resource.Layout.Main, null));
				};
			hosted.TabSelected += (sender, e) => 
				{
					Content.RemoveAllViews();
					Content.AddView(LayoutInflater.Inflate(Resource.Layout.SignUp, null));
				};

			
			ActionBar.AddTab(recommends);
			ActionBar.AddTab(joined);
			ActionBar.AddTab(hosted);
			/*
			//Shows spinner to indicate loading
			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch offers from server
					PrepareLooper();

					var events = MainActivity.CIF.GetJoinedEvents(100);

					View view;

					if (events.Count != 0)
					{
						var adapter = new JoinedEventListAdapter(this, events);
						Tilelist = new JoinedEventList(this, adapter, (s, e) => LeaveEvent());
						view = Tilelist;
					}
					else
					{
						view = new TextView(this);
						(view as TextView).Text = Resources.GetString(Resource.String.message_event_nothing_here);
					}

					//Clear screen and show the found offers
					RunOnUiThread(() =>
						{
							ClearLayout();
							Content.AddView(view);
						});
				});
			/**/
		}

		protected void LeaveEvent()
		{
			MainActivity.CIF.CancelReg(Tilelist.PopFocus());
		}
	}
}
