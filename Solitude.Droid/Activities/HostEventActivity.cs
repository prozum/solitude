
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
using Android.Support.V4.View;

namespace Solitude.Droid
{
	[Activity(Label = "Host Event")]			
	public class HostEventActivity : DrawerActivity
	{
		#region Var

		EditText title;
		EditText description;
		EditText location;
		EditText guests;
		DatePicker date;
		TimePicker timePicker;

		TextView timeCurrent;
		TextView dateCurrent;

		LinearLayout content;

		#endregion

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			var layout = LayoutInflater.Inflate(Resource.Layout.SignUp, null);
			var next = layout.FindViewById<Button>(Resource.Id.signUpNextBtn);
			var back = layout.FindViewById<Button>(Resource.Id.signUpPreviousBtn);
			var viewpager = layout.FindViewById<ViewPager>(Resource.Id.signUpViewPager);
			var adapter = new EditEventAdapter(this, viewpager, next, back);

			viewpager.Adapter = adapter;

			adapter.AddPager(new EventInfoFragment());
			adapter.AddPager(new EventDateFragment());
			adapter.AddPager(new EventTimeFragment());

			Content.AddView(layout);

			var typeString = Intent.GetStringExtra("type");




			/*
			Build();


			if (typeString.Equals("new"))
				New();
			else if (typeString.Equals("edit"))
				Edit();
			else
				throw new ArgumentOutOfRangeException();
			*/
		}

		private void Build()
		{

			content = new LinearLayout(this);
			content.Orientation = Orientation.Vertical;

			var scroll = new ScrollView(this);
			scroll.AddView(content);
			Content.AddView(scroll);

			// Create all visible elements required to create event
			var titleTitle = new TextView(this);
			titleTitle.Text = Resources.GetString(Resource.String.event_name);

			title = new EditText(this);
			title.Hint = Resources.GetString(Resource.String.event_name_hint);
			title.Id = 0x0001;

			var descriptionTitle = new TextView(this);
			descriptionTitle.Text = Resources.GetString(Resource.String.event_description);

			description = new EditText(this);
			description.Hint = Resources.GetString(Resource.String.event_description_hint);
			description.Id = 0x0002;

			var locationTitle = new TextView(this);
			locationTitle.Text = Resources.GetString(Resource.String.event_place);

			location = new EditText(this);
			location.Hint = Resources.GetString(Resource.String.event_place_hint);
			location.Id = 0x0003;

			var guestsTitle = new TextView(this);
			guestsTitle.Text = Resources.GetString(Resource.String.event_guest);

			guests = new EditText(this);
			guests.Hint = Resources.GetString(Resource.String.event_guest_hint);
			guests.InputType = global::Android.Text.InputTypes.ClassNumber;
			guests.Id = 0x0004;

			var dateTitle = new TextView(this);
			dateTitle.Text = Resources.GetString(Resource.String.event_date);

			date = new DatePicker(this);
			date.DateTime = DateTime.Now;
			date.CalendarViewShown = false;
			date.Id = 0x0005;

			dateCurrent = new TextView(this);
			dateCurrent.Gravity = GravityFlags.Center;
			dateCurrent.TextSize = 24;
			dateCurrent.Text = FormatDate(date.DateTime);
			var dateBuilder = new AlertDialog.Builder(this);
			var dateDialog = dateBuilder.Create();
			dateDialog.SetTitle(Resources.GetString(Resource.String.event_date));
			dateDialog.SetView(date);
			dateDialog.SetButton(Resources.GetString(Resource.String.cancel_button), (s, ev) =>
				{
					dateCurrent.Text = FormatDate(date.DateTime);
					dateDialog.Dismiss();
				});

			var buttonDate = new Button(this);
			buttonDate.Text = Resources.GetString(Resource.String.event_date);
			buttonDate.Click += (object sender, EventArgs e) => dateDialog.Show();

			var timeTitle = new TextView(this);
			timeTitle.Text = Resources.GetString(Resource.String.event_time);

			timePicker = new TimePicker(this);
			timePicker.Id = 0x0006;
			timePicker.SetIs24HourView(Java.Lang.Boolean.True);

			timeCurrent = new TextView(this);
			timeCurrent.Gravity = GravityFlags.Center;
			timeCurrent.TextSize = 24;
			timeCurrent.Text = FormatTime(timePicker);

			var timeBuilder = new AlertDialog.Builder(this);
			var timeDialog = timeBuilder.Create();
			timeDialog.SetTitle(Resources.GetString(Resource.String.event_time));
			timeDialog.SetView(timePicker);
			timeDialog.SetButton(Resources.GetString(Resource.String.event_time), (s, ev) =>
				{
					timeCurrent.Text = FormatTime(timePicker);
					dateDialog.Dismiss();
				});

			var buttonTime = new Button(this);
			buttonTime.Text = Resources.GetString(Resource.String.event_time);
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
		}

		private string FormatTime(TimePicker tp)
		{
			string h = (string)tp.CurrentHour;
			string m = (string)tp.CurrentMinute;
			return String.Format("{0} : {1}", h.Length == 1 ? "0" + h : h, m.Length == 1 ? "0" + m : m);
		}

		private string FormatDate(DateTime dt)
		{
			string d = dt.Day.ToString();
			return String.Format("{0}. {1} - {2}", d.Length == 1 ? "0" + d : d, MonthConverter(dt.Month), dt.Year);

		}

		private void New()
		{
			// Buttons for creating a new event or cancel.
			var createEventButton = new Button(this);
			createEventButton.Text = Resources.GetString(Resource.String.event_host_event);
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
					alertDialog.SetTitle(Resources.GetString(Resource.String.event_invalid_info));
					alertDialog.SetMessage(String.Format(Resources.GetString(Resource.String.event_error_in_info) + "\n{0}{1}{2}{3}{4}", 
							boolTitle ? Resources.GetString(Resource.String.event_error_no_title) + "\n" : "",
							boolDescription ? Resources.GetString(Resource.String.event_error_no_description) + "\n" : "",
							boolLocation ? Resources.GetString(Resource.String.event_error_no_place) + "\n" : "",
							boolGuest ? Resources.GetString(Resource.String.event_error_no_guest) + "\n" : "",
							(!boolGuestCount && !boolGuest) ? Resources.GetString(Resource.String.event_error_many_guest) : ""));
					alertDialog.SetButton(Resources.GetString(Resource.String.ok), (s, ev) => alertDialog.Dismiss());
					alertDialog.Show();
				}
				else
				{
					var @dateTime = new DateTimeOffset(date.DateTime.Year, 
															date.DateTime.Month, 
															date.DateTime.Day, 
															(int)timePicker.CurrentHour, 
															(int)timePicker.CurrentMinute, 
															0, 
															new TimeSpan(0));
					Event @event = new Event(title.Text, @dateTime, location.Text, description.Text, numberOfGuestsMax, 0);
					bool completed = MainActivity.CIF.CreateEvent(@event);
					if (completed)
						Finish();
					else
					{
						var dialog = new AlertDialog.Builder(this);
						dialog.SetMessage(Resources.GetString(Resource.String.event_error_could_not_create) + "\n" + MainActivity.CIF.LatestError);
						dialog.Show();
					}
				}
			};
			var cancelButton = new Button(this);
			cancelButton.Text = Resources.GetString(Resource.String.cancel_button);
			cancelButton.Id = 0x0008;
			cancelButton.Click += (object sender, EventArgs e) => Finish();

			var buttonKeeper = new LinearLayout(this);
			buttonKeeper.Orientation = Orientation.Horizontal;
			buttonKeeper.AddView(cancelButton);
			buttonKeeper.AddView(createEventButton);

			content.AddView(buttonKeeper);
		}

		private void Edit()
		{
			title.Text = Intent.GetStringExtra("title");
			description.Text = Intent.GetStringExtra("description");
			date.DateTime = new DateTime(Intent.GetIntExtra("date year", 0), Intent.GetIntExtra("date month", 0), Intent.GetIntExtra("date day", 0));
			location.Text = Intent.GetStringExtra("place");
			guests.Text = Intent.GetIntExtra("maxslots", 0).ToString();
			dateCurrent.Text = FormatDate(date.DateTime);
			timePicker.CurrentHour = (Java.Lang.Integer)Intent.GetIntExtra("date hour", 0);
			timePicker.CurrentMinute = (Java.Lang.Integer)Intent.GetIntExtra("date minutte", 0);
			timeCurrent.Text = FormatTime(timePicker);

			var buttonConfirm = new Button(this);
			buttonConfirm.Id = 0x0007;
			buttonConfirm.Text = Resources.GetString(Resource.String.host_save_changes);
			buttonConfirm.Click += (object sender, EventArgs e) =>
				{
					int numberOfGuestsMax;
					bool boolGuestCount = int.TryParse(guests.Text, out numberOfGuestsMax);
					bool boolTitle = String.IsNullOrEmpty(title.Text);
					bool boolDescription = String.IsNullOrEmpty(description.Text);
					bool boolLocation = String.IsNullOrEmpty(location.Text);
					bool boolGuest = String.IsNullOrEmpty(guests.Text);
					bool bool32BitGuest = numberOfGuestsMax < Intent.GetIntExtra("slotstaken", Int32.MaxValue);

					if (boolTitle
					    || boolDescription
					    || boolLocation
					    || boolGuest
					    || !boolGuestCount
					    || bool32BitGuest)
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						AlertDialog alertDialog = builder.Create();
						alertDialog.SetTitle(Resources.GetString(Resource.String.event_invalid_info));
						alertDialog.SetMessage(String.Format(Resources.GetString(Resource.String.event_error_in_info) + "\n{0}{1}{2}{3}{4}{5}", 
								boolTitle ? Resources.GetString(Resource.String.event_error_no_title) + "\n" : "",
								boolDescription ? Resources.GetString(Resource.String.event_error_no_description) + "\n" : "",
								boolLocation ? Resources.GetString(Resource.String.event_error_no_place) + "\n" : "",
								boolGuest ? Resources.GetString(Resource.String.event_error_no_guest) + "\n" : "",
								(!boolGuestCount && !boolGuest) ? Resources.GetString(Resource.String.event_error_many_guest) : "",
								bool32BitGuest ? Resources.GetString(Resource.String.event_error_guest_limit) : ""));
						alertDialog.SetButton(Resources.GetString(Resource.String.ok), (s, ev) => alertDialog.Dismiss());
						alertDialog.Show();
					}
					else
					{
						var @datetime = new DateTimeOffset(	date.DateTime.Year, 
															date.DateTime.Month, 
															date.DateTime.Day, 
															(int)timePicker.CurrentHour, 
															(int)timePicker.CurrentMinute, 
															0, 
															new TimeSpan(0));
						
						Event @event = new Event(	title.Text, 
													@datetime, 
							               			location.Text, 
							               			description.Text, 
							               			numberOfGuestsMax, 
							               			Intent.GetIntExtra("slotstaken", 0), 
							               			Intent.GetIntExtra("id", 0));
								
						bool completed = MainActivity.CIF.UpdateEvent(@event);
						if (completed)
							Finish();
						else
						{
							var dialog = new AlertDialog.Builder(this);
							dialog.SetMessage(Resources.GetString(Resource.String.message_error_event_update_event) + "\n" + MainActivity.CIF.LatestError);
							dialog.SetNegativeButton(Resource.String.ok, (s, earg) => {});
							dialog.Show();
						}
						Finish();
					}
					
				};

			var buttonCancel = new Button(this);
			buttonCancel.Id = 0x0008;
			buttonCancel.Text = Resources.GetString(Resource.String.cancel_button);
			buttonCancel.Click += (object sender, EventArgs e) => Finish();

			var buttonKeeper = new LinearLayout(this);
			buttonKeeper.Orientation = Orientation.Horizontal;

			buttonKeeper.AddView(buttonCancel);
			buttonKeeper.AddView(buttonConfirm);

			content.AddView(buttonKeeper);
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

