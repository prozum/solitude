
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
	public class CreateEventActivity : AbstractActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			drawerPosition = 3;
			base.OnCreate (bundle);

			var content = FindViewById<FrameLayout>(Resource.Id.content_frame);
			LinearLayout internalContent = new LinearLayout(this);
			internalContent.Orientation = Orientation.Vertical;
			ScrollView scroll = new ScrollView(this);
			scroll.AddView(internalContent);
			content.AddView(scroll);

			TextView descriptionTitle = new TextView(this);
			descriptionTitle.Text = "Event Description";

			EditText description = new EditText(this);
			description.Hint = "Description!";

			TextView locationTitle = new TextView(this);
			locationTitle.Text = "Location/Address";

			EditText location = new EditText(this);
			location.Hint = "Location/Address";

			TextView guestsTitle = new TextView(this);
			guestsTitle.Text = "Guests";

			EditText guests = new EditText(this);
			guests.Hint = "Number of guests";
			guests.InputType = global::Android.Text.InputTypes.ClassNumber;

			TextView dateTitle = new TextView(this);
			dateTitle.Text = "Date";

			DatePicker date = new DatePicker(this);
			date.DateTime = DateTime.Now;

			TextView timeTitle = new TextView(this);
			timeTitle.Text = "Time";

			TimePicker time = new TimePicker(this);

			Button createEventButton = new Button(this);
			createEventButton.Text = "Host Event";
			Button cancelButton = new Button(this);
			cancelButton.Text = "Cancel";

			LinearLayout buttonKeeper = new LinearLayout(this);
			buttonKeeper.Orientation = Orientation.Horizontal;
			buttonKeeper.AddView(cancelButton);
			buttonKeeper.AddView(createEventButton);

			internalContent.AddView(descriptionTitle);
			internalContent.AddView(description);
			internalContent.AddView(locationTitle);
			internalContent.AddView(location);
			internalContent.AddView(guestsTitle);
			internalContent.AddView(guests);
			internalContent.AddView(dateTitle);
			internalContent.AddView(date);
			internalContent.AddView(timeTitle);
			internalContent.AddView(time);
			internalContent.AddView(buttonKeeper);

			// Create your application here
		}
	}
}

