using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Runtime.InteropServices;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views.InputMethods;

namespace Solitude.Droid
{
	public class EditEventAdapter : EditDataAdapter
	{
		public EditEventAdapter(AppCompatActivity activity, ViewPager pager, FloatingActionButton finish, ProgressBar progress)
			: base(activity, pager, finish, progress) { }

		/// <summary>
		/// A method for updating data when the finish button is pressed.
		/// </summary>
		protected override void UpdateData()
		{
			// Get all the info saved in the activity.
			var type = Activity.Intent.GetStringExtra("type");
			var title = Activity.Intent.GetStringExtra("title");
			var description = Activity.Intent.GetStringExtra("description");
			var year = Activity.Intent.GetIntExtra("date year", 0);
			var month = Activity.Intent.GetIntExtra("date month", 0);
			var day = Activity.Intent.GetIntExtra("date day", 0);
			var hour = Activity.Intent.GetIntExtra("date hour", 0);
			var minut = Activity.Intent.GetIntExtra("date minutte", 0);
			var place = Activity.Intent.GetStringExtra("place");
			var max = Activity.Intent.GetIntExtra("maxslots", 0);
			var taken = Activity.Intent.GetIntExtra("slotstaken", 0);
			var id = Activity.Intent.GetStringExtra("id");

			// Creat the event based on the information.
			var @event = new Event()
			{
				Location = place,
				Date = new DateTimeOffset(year, month, day, hour, minut, 0, new TimeSpan(0)),
				Description = description,
				Id = id,
				SlotsTaken = taken,
				SlotsTotal = max,
				Title = title
			};

			bool completed = false;

			// Check whether this is editing an allready existing event, or making a new one.
			if (type == "edit")
				completed = MainActivity.CIF.UpdateEvent(@event);
			else if (type == "new")
				completed = MainActivity.CIF.CreateEvent(@event);
			else
				throw new ArgumentException("type has to be either edit or new");


			if (completed)
			{
				Back();
			}
			else
			{
				// If an error occured, show message.
				var dialog = new Android.Support.V7.App.AlertDialog.Builder(Activity);
				dialog.SetMessage(Activity.Resources.GetString(Resource.String.message_error_event_update_event) + "\n" + MainActivity.CIF.LatestError);
				dialog.SetNegativeButton(Resource.String.ok, (s, earg) => { });
				dialog.Show();
			}
        }

		/// <summary>
		/// A method for going back to the event activity
		/// </summary>
		protected override void Back()
		{
			var intent = new Intent(Activity, typeof(EventActivity));
			intent.PutExtra("index", Activity.Intent.GetIntExtra("index", 0));
			intent.PutExtra("tab", Activity.Intent.GetIntExtra("tab", 0));
			Activity.StartActivity(intent);
		}

		/// <summary>
		/// A method for going to the activty that this ViewPager should lead to.
		/// </summary>
		protected override void BackWarning()
		{
			var dialog = new Android.Support.V7.App.AlertDialog.Builder(Activity);
			dialog.SetMessage(Resource.String.event_warning_edit);
			dialog.SetPositiveButton(Resource.String.yes, (s, earg) => Back());
			dialog.SetNegativeButton(Resource.String.no, (s, earg) => { });
			dialog.Show();
		}
	}
}