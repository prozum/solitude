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

namespace Solitude.Droid
{
	[Activity(Label = "Events", Icon = "@drawable/Events_Icon")]
	public class EventActivity : DrawerActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			// setting up and drawer
			base.OnCreate(savedInstanceState);
			ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

			var recommends = ActionBar.NewTab();
			var joined = ActionBar.NewTab();
			var hosted = ActionBar.NewTab();

			recommends.SetText(Resource.String.event_menu_recommended);
			joined.SetText(Resource.String.event_menu_joined);
			hosted.SetText(Resource.String.event_menu_hosted);

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
			var list = layout.FindViewById<ListView>(Resource.Id.list);
			var adapter = new EventAdapter<Offer>(this, new List<Offer>()
				{
					new Offer()
					{
						Address = "Aalborg University",
						Date = DateTimeOffset.Now,
						Description = "It's time for a test offer. This offer is a test and should be treated as such. Therefor there is no need to join it, since nothing will happen. Don't join me plz.",
						Match = new Match()
						{
							FoodHabits = new int[]
							{
								0, 1, 2
							},
							Interests = new int[]
							{
								0, 1, 2
							},
							Languages = new int[]
							{
								0, 1, 2
							},
						},
						SlotsTaken = 2,
						SlotsTotal = 6,
						Title = "Test Offer"
					}
				});

			adapter.OnAction1 = (i) => adapter.RemoveAt(i);
			adapter.OnAction2 = (i) => adapter.RemoveAt(i);
			adapter.OnUpdatePosition = (view, evnt, exp) =>
			{
				var offer = evnt as Offer;
                string matchs = "Matched by:\n";

				for (int i = 0; i < offer.Match.Interests.Length; i++)
				{
					matchs += MainActivity.InfoNames[(int)InfoType.Interest][offer.Match.Interests[i]];

					if (i != offer.Match.Interests.Length - 1)
						matchs += ", ";
					else
						matchs += "\n";
				}


				for (int i = 0; i < offer.Match.FoodHabits.Length; i++)
				{
					matchs += MainActivity.InfoNames[(int)InfoType.FoodHabit][offer.Match.FoodHabits[i]];

					if (i != offer.Match.FoodHabits.Length - 1)
						matchs += ", ";
					else
						matchs += "\n";
				}

				for (int i = 0; i < offer.Match.FoodHabits.Length; i++)
				{
					matchs += MainActivity.InfoNames[(int)InfoType.Language][offer.Match.Languages[i]];

					if (i != offer.Match.Languages.Length - 1)
						matchs += ", ";
					else
						matchs += "\n";
				}

				view.FindViewById<TextView>(Resource.Id.expanded_content).Text = 
					string.Format("{0}\n\n{1}\n{2}/{3}\n{4}", offer.Description, offer.Address, offer.SlotsTaken, offer.SlotsTotal, matchs);

				view.FindViewById<Button>(Resource.Id.action1).Text = "Delcine";
				view.FindViewById<Button>(Resource.Id.action2).Text = "Accept";
			};

			list.Adapter = adapter;
			
			Content.RemoveAllViews();
			Content.AddView(layout);
		}

		protected void SelectJoined()
		{
			var layout = LayoutInflater.Inflate(Resource.Layout.EventList, null);
			var list = layout.FindViewById<ListView>(Resource.Id.list);
			var adapter = new EventAdapter<Event>(this, new List<Event>()
				{
					new Event()
					{
						Address = "Aalborg University",
						Date = DateTimeOffset.Now,
						Description = "It's time for a test offer. This offer is a test and should be treated as such. Therefor there is no need to join it, since nothing will happen. Don't join me plz.",
						SlotsTaken = 2,
						SlotsTotal = 6,
						Title = "Test Offer"
					}
				});

			adapter.OnAction1 = (i) => adapter.RemoveAt(i);
			adapter.OnUpdatePosition = (view, evnt, exp) =>
			{
				view.FindViewById<TextView>(Resource.Id.expanded_content).Text =
					string.Format("{0}\n\n{1}\n{2}/{3}", evnt.Description, evnt.Address, evnt.SlotsTaken, evnt.SlotsTotal);

				view.FindViewById<Button>(Resource.Id.action1).Text = "Leave";
				view.FindViewById<Button>(Resource.Id.action2).Visibility = ViewStates.Gone;
			};

			list.Adapter = adapter;

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

		protected void SelectHosted()
		{
			var layout = LayoutInflater.Inflate(Resource.Layout.EventList, null);
			var list = layout.FindViewById<ListView>(Resource.Id.list);
			var adapter = new EventAdapter<Event>(this, new List<Event>()
				{
					new Event()
					{
						Address = "Aalborg University",
						Date = DateTimeOffset.Now,
						Description = "It's time for a test offer. This offer is a test and should be treated as such. Therefor there is no need to join it, since nothing will happen. Don't join me plz.",
						SlotsTaken = 2,
						SlotsTotal = 6,
						Title = "Test Offer"
					}
				});

			adapter.OnAction1 = (i) => adapter.RemoveAt(i);
			adapter.OnUpdatePosition = (view, evnt, exp) =>
			{
				view.FindViewById<TextView>(Resource.Id.expanded_content).Text =
					string.Format("{0}\n\n{1}\n{2}/{3}", evnt.Description, evnt.Address, evnt.SlotsTaken, evnt.SlotsTotal);

				view.FindViewById<Button>(Resource.Id.action1).Text = "Cancel";
				view.FindViewById<Button>(Resource.Id.action2).Text = "Edit";
			};

			list.Adapter = adapter;

			Content.RemoveAllViews();
			Content.AddView(layout);
		}


		protected void LeaveEvent()
		{
			//MainActivity.CIF.CancelReg(Tilelist.PopFocus());
		}
	}
}
