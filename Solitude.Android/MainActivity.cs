using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Solitude.Android
{
	[Activity (Label = "Solitude.Android", MainLauncher = true, Theme = "@android:style/Theme.DeviceDefault.NoActionBar")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			int count = 0;

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			var menuButton = FindViewById (Resource.Id.buttonMenu);
			var dispText = FindViewById <TextView> (Resource.Id.textView1);
			dispText.Text = count.ToString ();
			menuButton.Click += (sender, e) => {
				count++;
				dispText.Text = count.ToString();
			};
		}
	}
}


