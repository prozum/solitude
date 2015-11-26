using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Solitude.Droid
{
	public class HostingFragment : Android.Support.V4.App.Fragment
	{
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
			
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var layout = inflater.Inflate(Resource.Layout.HostList, null);
			var list = layout.FindViewById<ListView>(Resource.Id.list);
			var adapter = new EventAdapter<Event>(Activity, new List<Event>()
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

			return layout;
		}
	}
}