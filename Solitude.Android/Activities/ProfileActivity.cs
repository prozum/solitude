using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

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
		protected User User { get; set; }
		protected List<List<int>> Info { get; set; }

		protected override void OnCreate (Bundle savedInstanceState)
		{
			///var profile = new ProfileView(this, new User("Jimmi", "Jimmivej 12"));

			// setting up drawer
			base.OnCreate (savedInstanceState);

			ShowSpinner();

			ThreadPool.QueueUserWorkItem(o =>
				{
					//Fetch offers from server
					PrepareLooper();

					User = new User("Jimmi", "Jimmivej 12");
					Info = MainActivity.CIF.GetInformation();

					//Clear screen and show the found offers
					RunOnUiThread( () => 
					{
						ClearLayout();
						SetupUI();
					});
				});

		}

		private void SetupUI()
		{
			// add profile to activity
			var profile = LayoutInflater.Inflate(Resource.Layout.Profile, null);
			Content.AddView(profile);

			var picture  = FindViewById<ImageView>(Resource.Id.Image);
			var name = FindViewById<TextView>(Resource.Id.Name);
			var address = FindViewById<TextView>(Resource.Id.Address);
			var layout = FindViewById<LinearLayout>(Resource.Id.Layout);
			
			var adapter = new InfoAdapter(this, Info);
			var tilemenu = new InfoList(this, adapter);

			layout.AddView(tilemenu);

			name.Text = User.Name;
			address.Text = User.Address;
		}

		private void UpdateInfo(InfoType type, TextView layout, List<int> changes)
		{
			/*
			foreach (var item in Info)
			{
				if (item.Item1 == type)
				{
					if (!changes.Contains(item.Item2))
						MainActivity.CIF.DeleteInformation(new InfoChange(type, item.Item2));

					if (true)
					{
						
					}
				}
				
			}
			*/
			/*
			var text = "";

			for (int i = 0; i < items.Count - 1; i++)
				text += items[i].ToString() + "\n";

			if (items.Count > 0)
				text += items[items.Count - 1].ToString();

			layout.Text = text;
			*/
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
					//UpdateDetails();

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

		private void SetupEditDialog(InfoType type, TextView layout)
		{
			var dialog = new InfoDialog(this, type, Info);

			// adding functionallity to save button
			dialog.SaveButton.Click += (s, e) => 
				{
					// updating ui
					UpdateInfo(type, layout, dialog.ItemsChecked());

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


