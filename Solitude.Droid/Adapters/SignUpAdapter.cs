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
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using System.Threading;

namespace Solitude.Droid
{
	public class SignUpAdapter : EditDataAdapter
	{
		public SignUpAdapter(AppCompatActivity activity, ViewPager pager, FloatingActionButton finish, ProgressBar progress)
			: base(activity, pager, finish, progress) { }

		protected override void UpdateData()
		{
			var username = Activity.Intent.GetStringExtra("username");
			var password = Activity.Intent.GetStringExtra("password");
			var confirm = Activity.Intent.GetStringExtra("confirm");
			var name = Activity.Intent.GetStringExtra("name");
			var address = Activity.Intent.GetStringExtra("address");
			var year = Activity.Intent.GetIntExtra("date year", DateTime.Now.Year);
			var month = Activity.Intent.GetIntExtra("date month", DateTime.Now.Year);
			var day = Activity.Intent.GetIntExtra("date day", DateTime.Now.Year);
			var interests = Activity.Intent.GetIntegerArrayListExtra(InfoType.Interest.ToString());
			var foodhabits = Activity.Intent.GetIntegerArrayListExtra(InfoType.FoodHabit.ToString());
			var languages = Activity.Intent.GetIntegerArrayListExtra(InfoType.Language.ToString());
			
			//Generates a dialog showing a spinner
			var pb = new Android.Support.V7.App.AlertDialog.Builder(Activity).Create();
			pb.SetView(new ProgressBar(pb.Context));
			pb.SetCancelable(false);
			pb.Show();

			ThreadPool.QueueUserWorkItem(o =>
			{
				//Tries to create the user on the server
				if (MainActivity.CIF.CreateUser(name, address, new DateTimeOffset(year, month, day, 0, 0, 0, new TimeSpan(0)), username, password, confirm))
				{
					MainActivity.CIF.Login(username, password);
					foreach (var interest in interests)
					{
						MainActivity.CIF.AddInformation(new InfoChange(InfoType.Interest, (int)interest, 1));
					}
					foreach (var foodhabit in foodhabits)
					{
						MainActivity.CIF.AddInformation(new InfoChange(InfoType.FoodHabit, (int)foodhabit, 1));
					}
					foreach (var language in languages)
					{
						MainActivity.CIF.AddInformation(new InfoChange(InfoType.Language, (int)language, 1));
					}

					//Removes the spinner again
					Activity.RunOnUiThread(() => pb.Dismiss());

					var profileIntent = new Intent(Activity, typeof(ProfileActivity));
					Activity.StartActivity(profileIntent);

				}
				else
				{
					//Removes the spinner again
					Activity.RunOnUiThread(() =>
					{
						pb.Dismiss();

						//Generates a dialog showing an errormessage
						var errorDialog = new Android.Support.V7.App.AlertDialog.Builder(Activity);
						errorDialog.SetMessage(MainActivity.CIF.LatestError);
						errorDialog.SetNegativeButton(Resource.String.ok, (s, earg) => { });
						errorDialog.Show();
					});
                }

			});
		}

		/// <summary>
		/// A method for going to the profile activity.
		/// </summary>
		protected override void Back()
		{
			var intent = new Intent(Activity, typeof(ProfileActivity));
			Activity.StartActivity(intent);
		}

		/// <summary>
		/// A method for going to the activty that this ViewPager should lead to.
		/// </summary>
		protected override void BackWarning()
		{
			var intent = new Intent(Activity, typeof(MainActivity));
			Activity.StartActivity(intent);
		}
	}
}