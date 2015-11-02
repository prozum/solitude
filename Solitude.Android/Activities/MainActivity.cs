using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace DineWithaDane.Android
{
	[Activity (Label = "Solitude.Android", MainLauncher = false, Theme = "@android:style/Theme.DeviceDefault.NoActionBar")]
	public class MainActivity : Activity
	{
		private ClientCommunication.IClientCommunication CIF;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			StartService (new Intent(this, typeof(BackgroundService)));

			CIF = new ClientCommunication.CommunicationInterface ();
			List<Offer> offers;

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			Button fetchButton = FindViewById<Button> (Resource.Id.fetchOffersButton);
			TextView nameView = FindViewById<TextView> (Resource.Id.nameView);
			TextView descView = FindViewById<TextView> (Resource.Id.desciptionView);

			fetchButton.Click += async (sender, e) => {
				offers = await CIF.RequestOffers();
				nameView.Text = offers [0].offeredEvent.name;
				descView.Text = offers [0].offeredEvent.description;
			};
		}

		protected override void OnDestroy ()
		{
			base.OnDestroy ();
			StopService (new Intent (this, typeof(BackgroundService)));
		}
	}
}


