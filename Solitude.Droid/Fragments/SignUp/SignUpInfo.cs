using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Graphics;
using Android.Support.V7.Widget;

namespace Solitude.Droid
{
	/// <summary>
	/// The fragment containing the comparable information of the user.
	/// </summary>
	public class SignUpInfo : EditFragment
	{
		protected View Card { get; set; }
		protected LinearLayout Content { get; set; }
		protected AppCompatMultiAutoCompleteTextView AutoCompleter { get; set; }

		/// <summary>
		/// The subtitle of the card.
		/// </summary>
		protected string Subtitle { get; set; }

		/// <summary>
		/// A list representing the comparable information the user has seleted
		/// </summary>
		protected List<int> Info { get; set; }

		/// <summary>
		/// The comparable information time
		/// </summary>
		protected InfoType Type { get; set; }

		public SignUpInfo(InfoType type, string subtitle)
		{
			Type = type;
			Subtitle = subtitle;
			Info = new List<int>();
			HidesKeyboard = true;
        }

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Gets the layout, and all the relevant views it contains.
			Card = inflater.Inflate(Resource.Layout.ProfileInformationCard, null);
			Content = Card.FindViewById<LinearLayout>(Resource.Id.profile_card_entry);
			AutoCompleter = Card.FindViewById<AppCompatMultiAutoCompleteTextView>(Resource.Id.info_input);

			// Creats the card.
			CreateCard();

			//Sets the input box to visible.
			Card.FindViewById<ImageView>(Resource.Id.confirm_input).Visibility = ViewStates.Visible;
			Card.FindViewById<TextInputLayout>(Resource.Id.info_input_container).Visibility = ViewStates.Visible;

			// Adds the card to a scrollview.
			var scroll = new ScrollView(Activity);
			scroll.AddView(Card);
			return scroll;
		}

		/// <summary>
		/// Saves the comparable information contained in the card.
		/// </summary>
		public override void SaveInfo()
		{
			// This function is run, to make sure the user is not punished for forgetting 
			// to click the addbutton, before going to the next page.
			AdderClicked();

			// Put comparable information in the Intent of the Activity which contains this fragment.
			// This has to be saved as a list of java integers.
			Activity.Intent.PutIntegerArrayListExtra(Type.ToString(), Info.Select((x) => (Java.Lang.Integer)x).ToList());
		}

		/// <summary>
		/// Checks whether the card contains valid data.
		/// The card always contains valid data.
		/// </summary>
		/// <returns>true, if the information is valid, else false</returns>
		public override bool IsValidData()
		{
			return true;
		}

		/// <summary>
		/// Creates the card that this fragment contains.
		/// </summary>
		public void CreateCard()
		{
			// Get relevant views.
			var cardTitle = Card.FindViewById<TextView>(Resource.Id.profile_card_title);
			var cardSubtitle = Card.FindViewById<TextView>(Resource.Id.profile_card_subtitle);
			var adder = Card.FindViewById<ImageView>(Resource.Id.confirm_input);

			// Add listener, so that a dropdown autocomplet list appears on click.
			AutoCompleter.Click += (o, e) => AutoCompleter.ShowDropDown();
			AutoCompleter.Threshold = 0; // Set the required wordsize for autocompletion to be 0.

			// Tells the tokenizer to treat each word as its own entry.
			AutoCompleter.SetTokenizer(new Classes.SpaceTokenizer()); 

			// Get the resource id for the array which contains the right comparable information.
			var id = Resources.ObtainTypedArray(Resource.Array.info_resources).GetResourceId((int)Type, 0);
			
			// Gets possible elements to autocomplete to.
			var autocompleteElements = Resources.GetStringArray(id); 
			AutoCompleter.Adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleDropDownItem1Line, autocompleteElements);

			// Add listener, so that clicking +, adds all written information to card.
			adder.Click += (o, s) => AdderClicked();

			// Set card title and subtitle.
			cardTitle.Text = Resources.GetStringArray(Resource.Array.info_titles)[(int)Type];
			cardSubtitle.Text = Subtitle;
		}

		/// <summary>
		/// A function that is run when clicking on the +.
		/// </summary>
		private void AdderClicked()
		{
			var input = AutoCompleter.Text.ToLower();

			// Get the resource id for the array which contains the right comparable information.
			var id = Resources.ObtainTypedArray(Resource.Array.info_resources).GetResourceId((int)Type, 0);

			// Gets an array of all possible entries to compare input with.
			var compares = Resources.GetStringArray(id);

			AutoCompleter.Text = string.Empty;

			// Run AddVardEntry, if the inputfield contains elements in the compares array.
			foreach (var item in compares)
			{
				if (input.Contains(item.ToLower()))
				{
					AddCardEntry(item);
				}
			}
		}

		/// <summary>
		/// Adds a cardentry to the card.
		/// </summary>
		/// <param name="s">Name of the entry</param>
		private void AddCardEntry(string s)
		{
			// Get the resource id for the array which contains the right comparable information.
			var id = Resources.ObtainTypedArray(Resource.Array.info_resources).GetResourceId((int)Type, 0);

			// Gets the index of the entry.
			var info = Array.IndexOf(Resources.GetStringArray(id), s);

			// If card already contains the info, do nothing
			if (!Info.Contains(info))
			{
				// Get relevant views.
				var entryView = Activity.LayoutInflater.Inflate(Resource.Layout.ProfileInformationCardEntry, null);
				var remover = entryView.FindViewById<ImageView>(Resource.Id.profile_card_entry_remove);
				var entry = entryView.FindViewById<TextView>(Resource.Id.profile_card_entry_content);

				// On X click, remove entry from card.
				remover.Click += (se, ev) =>
				{
					if (Info.Remove(info))
						((ViewGroup)entryView.Parent).RemoveView(entryView);

				};

				entry.Text = s;
				remover.Visibility = ViewStates.Visible;

				// Add entry.
				Info.Add(info);
				Content.AddView(entryView);
			}
		}
	}
}