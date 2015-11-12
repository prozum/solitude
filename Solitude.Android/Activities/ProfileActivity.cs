using System;
using System.Collections;
using System.Collections.Generic;

using Android;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;
using Android.Graphics;
using Android.Content.PM;
using Android.Provider;
using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;



namespace DineWithaDane.Android
{
	[Activity (Label = "Profile", Icon = "@drawable/Profile_Icon")]
	public class ProfileActivity : DrawerActivity
	{
		public static readonly string[] Titles = new string[]
			{
				"Languages",
				"Interests",
				"Food Habits"
			};

		public static readonly string[][] Names = new string[][]
			{
				new string[]
				{
					"Danish",
					"English",
					"German",
					"French",
					"Spanish",
					"Chinese",
					"Russian"
				},
				new string[]
				{
					"Nature",
					"Fitness",
					"Movies",
					"Gaming",
					"Electronics",
					"Food",
					"Drawing"
				},
				new string[]
				{
					"Halal",
					"Kosher",
					"Vegan",
					"Lactose Intolerance",
					"Gluten Intolerance",
					"Nut Allergy"
				}
			};


		protected User User { get; set; }
		protected List<Language> Languages { get; set; }
		protected List<Interest> Interests { get; set; }
		protected List<FoodHabit> FoodHabits { get; set; }

		protected ImageView PictureView { get; set; }
		protected TextView NameView { get; set; }
		protected TextView AddressView { get; set; }

		protected override void OnCreate (Bundle savedInstanceState)
		{
			///var profile = new ProfileView(this, new User("Jimmi", "Jimmivej 12"));

			// setting up drawer
			base.OnCreate (savedInstanceState);

			// add profile to activity
			var profile = LayoutInflater.Inflate(Resource.Layout.Profile, null);
			Content.AddView(profile);

			var languagesview = FindViewById<TextView>(Resource.Id.Languages);
			var interestsview = FindViewById<TextView>(Resource.Id.Interests);
			var foodhabitsview = FindViewById<TextView>(Resource.Id.FoodHabits);
			var editdetails = FindViewById<Button>(Resource.Id.EditDetails);
			var editlanguages = FindViewById<Button>(Resource.Id.EditLanguages);
			var editinterests = FindViewById<Button>(Resource.Id.EditInterests);
			var editfoodhabits = FindViewById<Button>(Resource.Id.EditFoodHabits);

			User = new User("Jimmi", "Jimmivej 12");
			Languages = new List<Language>();
			Interests = new List<Interest>();
			FoodHabits = new List<FoodHabit>();

			PictureView = FindViewById<ImageView>(Resource.Id.Image);
			NameView = FindViewById<TextView>(Resource.Id.Name);
			AddressView = FindViewById<TextView>(Resource.Id.Address);

			editdetails.Click += (sender, e) => EditDetails();
			editlanguages.Click += (sender, e) => SetupEditDialog(InfoType.Language, languagesview, Languages);
			editinterests.Click += (sender, e) => SetupEditDialog(InfoType.Interest, interestsview, Interests);
			editfoodhabits.Click += (sender, e) => SetupEditDialog(InfoType.FoodHabit, foodhabitsview, FoodHabits);

			UpdateDetails();
			UpdateInfo(InfoType.Language, languagesview, Languages);
			UpdateInfo(InfoType.Interest, interestsview, Interests);
			UpdateInfo(InfoType.FoodHabit, foodhabitsview, FoodHabits);

		}

		private void UpdateInfo(InfoType type, TextView layout, IList items)
		{
			var text = "";

			for (int i = 0; i < items.Count - 1; i++)
				text += items[i].ToString() + "\n";

			if (items.Count > 0)
				text += items[items.Count - 1].ToString();

			layout.Text = text;
				
		}

		private void UpdateDetails()
		{
			NameView.Text = User.Name;
			AddressView.Text = User.Address;
		}

		private void EditDetails()
		{
			var dialog = new Dialog(this);
			dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
			dialog.SetContentView(Resource.Layout.EditDetails);

			// getting relevant views
			var nameview = dialog.FindViewById<AutoCompleteTextView>(Resource.Id.NameEditor);
			var addressview = dialog.FindViewById<AutoCompleteTextView>(Resource.Id.AddressEditor);
			//var imageview = dialog.FindViewById<ImageView>(Resource.Id.Image);
			//var editimage = dialog.FindViewById<Button>(Resource.Id.EditImage);
			var savebutton = dialog.FindViewById<Button>(Resource.Id.SaveButton);
			var cancelbutton = dialog.FindViewById<Button>(Resource.Id.CancelButton);

			// setting textbox info
			nameview.Text = User.Name;
			addressview.Text = User.Address;

			/*
			editimage.Click += (s, e) => 
				{
					var file = new File("profilepic.png");
					Intent intent = new Intent(MediaStore.ActionImageCapture);

					intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(file));
					StartActivityForResult(intent, 0);

					imageview.SetImageURI(Uri.FromFile(file));
				};
				*/

			// adding functionallity to save button
			savebutton.Click += (s, e) => 
				{
					// updating user name and address
					User.Name = nameview.Text;
					User.Address = addressview.Text;

					// updating ui
					UpdateDetails();

					// closing dialog
					dialog.Dismiss();
				};

			// adding functionallity to cancelbutton
			cancelbutton.Click += (s, e) => 
				{
					// closing dialog
					dialog.Dismiss();
				};

			// show dialog
			dialog.Show();
		}

		private void SetupEditDialog(InfoType type, TextView layout, IList info)
		{
			var dialog = new InfoDialog(this, type, info);

			// adding functionallity to save button
			dialog.SaveButton.Click += (s, e) => 
				{
					// adding selected info to list
					dialog.ItemsChecked(info);

					// updating ui
					UpdateInfo(type, layout, info);

					// closing dialog
					dialog.Dismiss();
				};

			// adding functionallity to cancelbutton
			dialog.CancelButton.Click += (s, e) => 
				{
					// closing dialog
					dialog.Dismiss();
				};

			dialog.Show();
		}
	}
}


