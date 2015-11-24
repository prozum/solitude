using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using Android.Support.V7.Widget;

namespace DineWithaDane.Droid
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

			recommends.TabSelected += (sender, e) => SelectRecommends();
			joined.TabSelected += (sender, e) => SelectJoined();
			hosted.TabSelected += (sender, e) => SelectHosted();

			
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

		protected void SelectRecommends()
		{
			var layout = LayoutInflater.Inflate(Resource.Layout.EventList, null);
			var list = layout.FindViewById<ListView>(Resource.Id.List);

			list.Adapter = new EventAdapter(this, new List<Event>()
				{
					new Event("sdfsdf", DateTimeOffset.Now, "Thing", "Thign", 10, 0),
					new Event("sdfsdf", DateTimeOffset.Now, "Thing", "Thign", 10, 0),
					new Event("afaff", DateTimeOffset.Now, "Thing", "Thign", 10, 0),
					new Event("afcvxcvxtet", DateTimeOffset.Now, "Thing", "Thign", 10, 0),
					new Event("teaaffat", DateTimeOffset.Now, "Thing", "Thign", 10, 0),
					new Event("texvcvcxt", DateTimeOffset.Now, "Thing", "Thign", 10, 0)
				});

			/*
			//Shows spinner to indicate loading
			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch offers from server
					PrepareLooper();
					//Clear screen and show the found offers
					RunOnUiThread(() =>
						{
							ClearLayout();
							Content.AddView(view);
						});
				});
				*/
			
			Content.RemoveAllViews();
			Content.AddView(layout);
		}

		protected void SelectJoined()
		{
			Content.RemoveAllViews();
			Content.AddView(LayoutInflater.Inflate(Resource.Layout.Main, null));
		}

		protected void SelectHosted()
		{
			Content.RemoveAllViews();
			Content.AddView(LayoutInflater.Inflate(Resource.Layout.SignUp, null));
		}


		protected void LeaveEvent()
		{
			MainActivity.CIF.CancelReg(Tilelist.PopFocus());
		}
	}
}
