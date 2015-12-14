using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace Solitude.Droid
{
    [Activity(Label = "AbstractActivity")]			
	public abstract class DrawerActivity : AppCompatActivity
	{
        /// <summary>
        /// The position of the element currently selected in the drawer.
        /// </summary>
		public int Position { get; set; }

        /// <summary>
        /// The setup of the drawer, filling it with content.
        /// </summary>
		protected SetupDrawer DrawerSetup { get; set; }

        /// <summary>
        /// Content of the drawer. All activities that inherit from this should add their layout to this layout.
        /// </summary>
		protected FrameLayout Content { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);


			SetContentView(Resource.Layout.DrawerLayout);

			Position = Intent.GetIntExtra("index", 0); // Gets the current position

			DrawerSetup = new SetupDrawer(Position, this);

			DrawerSetup.Configure();
			DrawerSetup.DrawerToggleSetup();
			Content = FindViewById<FrameLayout>(Resource.Id.content_frame);
		}

		protected override void OnPostCreate(Bundle savedInstanceState)
		{
			base.OnPostCreate(savedInstanceState);
			DrawerSetup.DrawerToggle.SyncState();
		}

        /// <summary>
        /// Shows the user an alert of they try to log out by pressing the hardware back button.   
        /// </summary>
		public override void OnBackPressed()
		{
			var dialog = new Android.Support.V7.App.AlertDialog.Builder(this);
			dialog.SetMessage(Resources.GetString(Resource.String.message_logout));
			dialog.SetNegativeButton(Resource.String.no, (s, e) => { });
			dialog.SetPositiveButton(Resource.String.yes, (s, e) => MainActivity.CIF.Logout(this));
			dialog.Show();
		}
		
        /// <summary>
        /// Handles pressing an item on the drawer.
        /// </summary>
        /// <param name="item">The item selected in the drawer.</param>
        /// <returns></returns>
		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (DrawerSetup.DrawerToggle.OnOptionsItemSelected(item))
				return true;

			return base.OnOptionsItemSelected(item);
		}

		/// <summary>
		/// Shows the spinner, indicating loading.
		/// </summary>
		/// <remarks>Must be run from the UI-thread</remarks>
		protected void ShowSpinner()
		{
			ProgressBar pb = new ProgressBar(this);
			Content.AddView(pb);
		}

		/// <summary>
		/// Removes the spinner.
		/// </summary>
		/// <remarks>Must be run in the UI-thread</remarks>
		protected void ClearLayout()
		{
			Content.RemoveAllViews();
		}
	}
}

