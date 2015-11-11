
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DineWithaDane.Android
{
	[Activity(Label = "SignUpInfoActivity")]			
	public class SignUpInfoActivity : Activity
	{
		protected List<Language> Languages { get; set; }
		protected List<Interest> Interests { get; set; }
		protected List<FoodHabit> FoodHabits { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.SignUpInfo);

			var name = FindViewById<EditText>(Resource.Id.editName);
			var address = FindViewById<EditText>(Resource.Id.editAddress);
			var interests = FindViewById<TextView>(Resource.Id.interests);
			var languages = FindViewById<TextView>(Resource.Id.languages);
			var foodhabits = FindViewById<TextView>(Resource.Id.foodhabits);
			var editInterests = FindViewById<Button>(Resource.Id.editInterests);
			var editLanguages = FindViewById<Button>(Resource.Id.editLanguages);
			var editFoodHabits = FindViewById<Button>(Resource.Id.editFoodHabits);
			var submit = FindViewById<Button>(Resource.Id.submit);
			var username = Intent.GetStringExtra("username");
			var password = Intent.GetStringExtra("password");

			Interests = new List<Interest>();
			Languages = new List<Language>();
			FoodHabits = new List<FoodHabit>();

			editInterests.Click += (sender, e) => 
				{
					SetupEditDialog(InfoType.Interest, Interests, interests);
				};
			editLanguages.Click += (sender, e) => 
				{
					SetupEditDialog(InfoType.Language, Languages, languages);
				};
			editFoodHabits.Click += (sender, e) => 
				{
					SetupEditDialog(InfoType.FoodHabit, FoodHabits, foodhabits);
				};

			submit.Click += (sender, e) => 
				{
					submit.Clickable = false;
					ThreadPool.QueueUserWorkItem( o => {
						var success = MainActivity.CIF.CreateUser(username, password, password);

						/*
						RunOnUiThread(() => {
							pb.Visibility = global::Android.Views.ViewStates.Invisible;
						});
						*/

						if(success)
						{
							MainActivity.CIF.Login(username, password);
							var toNotification = new Intent(this, typeof(NotificationActivity));
							StartActivity(toNotification);
						}
						else
						{
							var errorDialog = new AlertDialog.Builder(this);
							errorDialog.SetMessage(MainActivity.CIF.LatestError);
							RunOnUiThread(() => {
								errorDialog.Show();
							});

							submit.Clickable = false;
						}
					});
				};

			// Create your application here
		}

		private void SetupEditDialog(InfoType type, IList info, TextView layout)
		{
			var dialog = new InfoDialog(this, type, info);

			// adding functionallity to save button
			dialog.SaveButton.Click += (s, e) => 
				{
					string text = "";
					// adding selected info to list
					dialog.ItemsChecked(info);

					// updating ui
					if (info.Count != 0) 
					{
						for (int i = 0; i < info.Count - 1; i++) 
							text += info[i].ToString() + ", ";

						text += info[info.Count - 1];
					}

					layout.Text = text;

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

