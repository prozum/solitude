using System;
using Android.App;
using System.Threading.Tasks;
using Java.Lang;
using Android.OS;
using Android.Content;

namespace DineWithaDane.Droid
{
	[Service]
	public class BackgroundService : Service
	{
		public override IBinder OnBind (Intent intent)
		{
			throw new NotImplementedException ();
		}

		public BackgroundService () : base()
		{
			new Task (() => {
				while (true) // The service should be running for the entirety of the Apps lifetime. Created and destroyed with MainActivity.
				{
					Thread.Sleep(10000); // Run background routines every 10 seconds.
				}
			}).Start();
		}
	}
}

