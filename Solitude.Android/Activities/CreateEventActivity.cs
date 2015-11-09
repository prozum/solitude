
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DineWithaDane.Android
{
	[Activity(Label = "CreateEventActivity")]			
	public class CreateEventActivity : DrawerActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			Position = 3;
			base.OnCreate (bundle);

			var contentFrame = FindViewById<FrameLayout>(Resource.Id.content_frame);

			var content = new LinearLayout(this);
			content.Orientation = Orientation.Vertical;

			var scroll = new ScrollView(this);
			scroll.AddView(content);
			contentFrame.AddView(scroll);

			// Create all visible elements required to create event
			var titleTitle = new TextView(this);
			titleTitle.Text = "Event name";

			var title = new EditText(this);
			title.Hint = "Title";

			var descriptionTitle = new TextView(this);
			descriptionTitle.Text = "Event Description";

			var description = new EditText(this);
			description.Hint = "Description!";

			var locationTitle = new TextView(this);
			locationTitle.Text = "Location/Address";

			var location = new EditText(this);
			location.Hint = "Location/Address";

			var guestsTitle = new TextView(this);
			guestsTitle.Text = "Guests";

			var guests = new EditText(this);
			guests.Hint = "Number of guests";
			guests.InputType = global::Android.Text.InputTypes.ClassNumber;

			var dateTitle = new TextView(this);
			dateTitle.Text = "Date";

			var date = new DatePicker(this);
			date.DateTime = DateTime.Now;

			var timeTitle = new TextView(this);
			timeTitle.Text = "Time";

			var timePicker = new TimePicker(this);

			// Buttons for creating a new event or cancel.
			var createEventButton = new Button(this);
			createEventButton.Text = "Host Event";
			createEventButton.Click += (object sender, EventArgs e) => 
				{
					// Check if all forms have been filled.
					if(String.IsNullOrEmpty(title.Text) ||
						String.IsNullOrEmpty(description.Text) ||
						String.IsNullOrEmpty(location.Text) ||
						String.IsNullOrEmpty(guests.Text))
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						AlertDialog alertDialog = builder.Create();
						alertDialog.SetTitle("Empty fields");
						alertDialog.SetMessage(String.Format("The following fields are empty:\n{0}{1}{2}{3}", 
							String.IsNullOrEmpty(title.Text) ? "- No title\n" : "",
							String.IsNullOrEmpty(description.Text) ? "- No description\n" : "",
							String.IsNullOrEmpty(location.Text) ? "- No address\n" : "",
							String.IsNullOrEmpty(guests.Text) ? "- No guest limit" : ""));
						alertDialog.SetButton("OK", (s, ev) => alertDialog.Dismiss());
						alertDialog.Show();
					}
					else
					{
						DateTime @dateTime = new DateTime(date.Year, date.Month, date.DayOfMonth, (int)timePicker.CurrentHour, (int)timePicker.CurrentMinute, 0);
						Event @event = new Event(title.Text, @dateTime, location.Text, description.Text, int.Parse(guests.Text), 0);
						MainActivity.CIF.CreateEvent(@event);
						Finish();
					}
				};
			var cancelButton = new Button(this);
			cancelButton.Text = "Cancel";
			cancelButton.Click += (object sender, EventArgs e) => Finish();

			var buttonKeeper = new LinearLayout(this);
			buttonKeeper.Orientation = Orientation.Horizontal;
			buttonKeeper.AddView(cancelButton);
			buttonKeeper.AddView(createEventButton);

			// Build Activity Content
			content.AddView(titleTitle);
			content.AddView(title);
			content.AddView(descriptionTitle);
			content.AddView(description);
			content.AddView(locationTitle);
			content.AddView(location);
			content.AddView(guestsTitle);
			content.AddView(guests);
			content.AddView(dateTitle);
			content.AddView(date);
			content.AddView(timeTitle);
			content.AddView(timePicker);
			content.AddView(buttonKeeper);

			// Create your application here
		}
	}
}

