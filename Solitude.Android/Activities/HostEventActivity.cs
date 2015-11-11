
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
	[Activity(Label = "Host Event")]			
	public class HostEventActivity : DrawerActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			Position = 3;
			base.OnCreate(bundle);

			// There is allready a Content property that does this
			//var contentFrame = FindViewById<FrameLayout>(Resource.Id.content_frame);

			var content = new LinearLayout(this);
			content.Orientation = Orientation.Vertical;

			var scroll = new ScrollView(this);
			scroll.AddView(content);
			Content.AddView(scroll);

			// Create all visible elements required to create event
			var titleTitle = new TextView(this);
			titleTitle.Text = "Event name";

			var title = new EditText(this);
			title.Hint = "Title";
			title.Id = 0x0001;

			var descriptionTitle = new TextView(this);
			descriptionTitle.Text = "Event Description";

			var description = new EditText(this);
			description.Hint = "Description!";
			description.Id = 0x0002;

			var locationTitle = new TextView(this);
			locationTitle.Text = "Location/Address";

			var location = new EditText(this);
			location.Hint = "Location/Address";
			location.Id = 0x0003;

			var guestsTitle = new TextView(this);
			guestsTitle.Text = "Guests";

			var guests = new EditText(this);
			guests.Hint = "Number of guests";
			guests.InputType = global::Android.Text.InputTypes.ClassNumber;
			guests.Id = 0x0004;

			var dateTitle = new TextView(this);
			dateTitle.Text = "Date";

			var date = new DatePicker(this);
			date.DateTime = DateTime.Now;
			date.Id = 0x0005;

			var dateCurrent = new TextView(this);
			dateCurrent.Gravity = GravityFlags.Center;
			dateCurrent.TextSize = 24;
			string day = date.DateTime.Day.ToString();
			dateCurrent.Text = String.Format("{0}. {1} - {2}", day.Length == 1 ? "0" + day : day, MonthConverter(date.DateTime.Month), date.DateTime.Year);

			var dateBuilder = new AlertDialog.Builder(this);
			var dateDialog = dateBuilder.Create();
			dateDialog.SetTitle("Date");
			dateDialog.SetView(date);
			dateDialog.SetButton("Close", (s, ev) =>
				{
					day = date.DateTime.Day.ToString();
					dateCurrent.Text = String.Format("{0}. {1} - {2}", day.Length == 1 ? "0" + day : day, MonthConverter(date.DateTime.Month), date.DateTime.Year);
					dateDialog.Dismiss();
				});

			var buttonDate = new Button(this);
			buttonDate.Text = "Date";
			buttonDate.Click += (object sender, EventArgs e) => dateDialog.Show();

			var timeTitle = new TextView(this);
			timeTitle.Text = "Time";

			var timePicker = new TimePicker(this);
			timePicker.Id = 0x0006;

			var timeCurrent = new TextView(this);
			timeCurrent.Gravity = GravityFlags.Center;
			timeCurrent.TextSize = 24;
			string hour = (string)timePicker.CurrentHour;
			string minute = (string)timePicker.CurrentMinute;
			timeCurrent.Text = String.Format("{0} : {1}", hour.Length == 1 ? "0" + hour : hour, minute.Length == 1 ? "0" + minute : minute);

			var timeBuilder = new AlertDialog.Builder(this);
			var timeDialog = timeBuilder.Create();
			timeDialog.SetTitle("Title");
			timeDialog.SetView(timePicker);
			timeDialog.SetButton("Close", (s, ev) =>
				{
					hour = (string)timePicker.CurrentHour;
					minute = (string)timePicker.CurrentMinute;
					timeCurrent.Text = String.Format("{0} : {1}", hour.Length == 1 ? "0" + hour : hour, minute.Length == 1 ? "0" + minute : minute);
					dateDialog.Dismiss();
				});

			var buttonTime = new Button(this);
			buttonTime.Text = "Time";
			buttonTime.Click += (object sender, EventArgs e) => timeDialog.Show();

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
			content.AddView(dateCurrent);
			content.AddView(buttonDate);
			content.AddView(timeTitle);
			content.AddView(timeCurrent);
			content.AddView(buttonTime);

			var typeString = Intent.GetStringExtra("type");

			if (typeString.Equals("new"))
			{
				// Buttons for creating a new event or cancel.
				var createEventButton = new Button(this);
				createEventButton.Text = "Host Event";
				createEventButton.Id = 0x0007;
				createEventButton.Click += (object sender, EventArgs e) =>
				{
					int numberOfGuestsMax;
					bool boolGuestCount = int.TryParse(guests.Text, out numberOfGuestsMax);
					bool boolTitle = String.IsNullOrEmpty(title.Text);
					bool boolDescription = String.IsNullOrEmpty(description.Text);
					bool boolLocation = String.IsNullOrEmpty(location.Text);
					bool boolGuest = String.IsNullOrEmpty(guests.Text);
					// Check if all forms have been filled.
					if (boolTitle
					    || boolDescription
					    || boolLocation
					    || boolGuest
					    || !boolGuestCount)
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						AlertDialog alertDialog = builder.Create();
						alertDialog.SetTitle("Empty fields");
						alertDialog.SetMessage(String.Format("There are errors in your event:\n{0}{1}{2}{3}{4}", 
								boolTitle ? "- No title\n" : "",
								boolDescription ? "- No description\n" : "",
								boolLocation ? "- No address\n" : "",
								boolGuest ? "- No guest limit\n" : "",
								(!boolGuestCount && !boolGuest) ? "- Too many event guests." : ""));
						alertDialog.SetButton("OK", (s, ev) => alertDialog.Dismiss());
						alertDialog.Show();
					}
					else
					{
						DateTime @dateTime = new DateTime(date.DateTime.Year, date.DateTime.Month, date.DateTime.Day, (int)timePicker.CurrentHour, (int)timePicker.CurrentMinute, 0);
						Event @event = new Event(title.Text, @dateTime, location.Text, description.Text, numberOfGuestsMax, 0);
						bool completed = MainActivity.CIF.CreateEvent(@event);
						if (completed)
							Finish();
						else
						{
							var dialog = new AlertDialog.Builder(this);
							dialog.SetMessage("Sorry, could not create event:\n" + MainActivity.CIF.LatestError);
							dialog.Show();
						}
					}
				};
				var cancelButton = new Button(this);
				cancelButton.Text = "Cancel";
				cancelButton.Id = 0x0008;
				cancelButton.Click += (object sender, EventArgs e) => Finish();

				var buttonKeeper = new LinearLayout(this);
				buttonKeeper.Orientation = Orientation.Horizontal;
				buttonKeeper.AddView(cancelButton);
				buttonKeeper.AddView(createEventButton);

				content.AddView(buttonKeeper);
			}
			else if (typeString.Equals("edit"))
			{
				title.Text = Intent.GetStringExtra("title");
				description.Text = Intent.GetStringExtra("description");
				DateTime dateTime = new DateTime(Intent.GetIntExtra("date year", 0), Intent.GetIntExtra("date month", 0), Intent.GetIntExtra("date day", 0), Intent.GetIntExtra("date hour", 0), Intent.GetIntExtra("date minutte", 0), 0);
				location.Text = Intent.GetStringExtra("place");
				guests.Text = Intent.GetIntExtra("maxslots", 0).ToString();
				date.DateTime = dateTime;
				day = (string)date.DateTime.Day.ToString();
				dateCurrent.Text = String.Format("{0}. {1} - {2}", day.Length == 1 ? "0" + day : day, MonthConverter(date.DateTime.Month), date.DateTime.Year);
				timePicker.CurrentHour = (Java.Lang.Integer)dateTime.Hour;
				timePicker.CurrentMinute = (Java.Lang.Integer)dateTime.Minute;
				hour = (string)timePicker.CurrentHour;
				minute = (string)timePicker.CurrentMinute;
				timeCurrent.Text = String.Format("{0} : {1}", hour.Length == 1 ? "0" + hour : hour, minute.Length == 1 ? "0" + minute : minute);


				var buttonConfirm = new Button(this);
				buttonConfirm.Id = 0x0007;
				buttonConfirm.Text = "Save changes";
				buttonConfirm.Click += (object sender, EventArgs e) =>
				{
					int numberOfGuestsMax;
					bool boolGuestCount = int.TryParse(guests.Text, out numberOfGuestsMax);
					bool boolTitle = String.IsNullOrEmpty(title.Text);
					bool boolDescription = String.IsNullOrEmpty(description.Text);
					bool boolLocation = String.IsNullOrEmpty(location.Text);
					bool boolGuest = String.IsNullOrEmpty(guests.Text);
					bool bool32BitGuest = numberOfGuestsMax < Intent.GetIntExtra("leftslots", Int32.MaxValue);

					if (boolTitle
					    || boolDescription
					    || boolLocation
					    || boolGuest
					    || !boolGuestCount
					    || bool32BitGuest)
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						AlertDialog alertDialog = builder.Create();
						alertDialog.SetTitle("Empty fields");
						alertDialog.SetMessage(String.Format("There are errors in your event:\n{0}{1}{2}{3}{4}{5}", 
								boolTitle ? "- No title\n" : "",
								boolDescription ? "- No description\n" : "",
								boolLocation ? "- No address\n" : "",
								boolGuest ? "- No guest limit\n" : "",
								(!boolGuestCount && !boolGuest) ? "- Too many event guests." : "",
								bool32BitGuest ? "- Your new guest limit is below current number of participants" : ""));
						alertDialog.SetButton("OK", (s, ev) => alertDialog.Dismiss());
						alertDialog.Show();
					}
					Event @event = new Event(title.Text, new DateTime(date.DateTime.Year, date.DateTime.Month, date.DateTime.Day, (int)timePicker.CurrentHour, (int)timePicker.CurrentMinute, 0), location.Text, description.Text, numberOfGuestsMax, Intent.GetIntExtra("leftslots", 0), Intent.GetIntExtra("id", 0));
					MainActivity.CIF.UpdateEvent(@event);
				};
				
				var buttonCancel = new Button(this);
				buttonCancel.Id = 0x0008;
				buttonCancel.Text = "Back";
				buttonCancel.Click += (object sender, EventArgs e) => Finish();

				var buttonKeeper = new LinearLayout(this);
				buttonKeeper.Orientation = Orientation.Horizontal;

				buttonKeeper.AddView(buttonCancel);
				buttonKeeper.AddView(buttonConfirm);

				content.AddView(buttonKeeper);
			}
			else
			{
				throw new ArgumentOutOfRangeException();
			}
		}

		private string MonthConverter(int month)
		{
			string ret = "";
			switch (month)
			{
				case 1:
					ret = "Jan";
					break;
				case 2:
					ret = "Feb";
					break;
				case 3:
					ret = "Mar";
					break;
				case 4:
					ret = "Apr";
					break;
				case 5:
					ret = "May";
					break;
				case 6:
					ret = "Jun";
					break;
				case 7:
					ret = "Jul";
					break;
				case 8:
					ret = "Aug";
					break;
				case 9:
					ret = "Sep";
					break;
				case 10:
					ret = "Oct";
					break;
				case 11:
					ret = "Nov";
					break;
				case 12:
					ret = "Dec";
					break;
			}
			return ret;
		}
	}
}

