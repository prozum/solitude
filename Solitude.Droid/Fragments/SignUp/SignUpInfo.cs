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
	public class SignUpInfo : EditFragment
	{
		protected View Card { get; set; }
		protected LinearLayout Content { get; set; }
		protected AppCompatMultiAutoCompleteTextView AutoCompleter { get; set; }
		protected List<int> Info { get; set; }
		protected InfoType Type { get; set; }
		protected string Subtitle { get; set; }

		public SignUpInfo(InfoType type, string subtitle)
		{
			Type = type;
			Subtitle = subtitle;
			Info = new List<int>();
			HidesKeyboard = true;
        }

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			Card = inflater.Inflate(Resource.Layout.ProfileInformationCard, null);
			Content = Card.FindViewById<LinearLayout>(Resource.Id.profile_card_entry);
			AutoCompleter = Card.FindViewById<AppCompatMultiAutoCompleteTextView>(Resource.Id.info_input);

			CreateCard();

			//Sets the input box to visible
			Card.FindViewById<ImageView>(Resource.Id.confirm_input).Visibility = ViewStates.Visible;
			Card.FindViewById<TextInputLayout>(Resource.Id.info_input_container).Visibility = ViewStates.Visible;

			var scroll = new ScrollView(Activity);
			scroll.AddView(Card);
			return scroll;
		}

		public override void SaveInfo()
		{
			AdderClicked();
            Activity.Intent.PutIntegerArrayListExtra(Type.ToString(), Info.Select((x) => (Java.Lang.Integer)x).ToList());
		}

		public override bool IsValidData()
		{
			return true;
		}

		public void CreateCard()
		{
			var cardTitle = Card.FindViewById<TextView>(Resource.Id.profile_card_title);
			var cardSubtitle = Card.FindViewById<TextView>(Resource.Id.profile_card_subtitle);

			AutoCompleter.Click += (o, e) => AutoCompleter.ShowDropDown();
			AutoCompleter.Threshold = 0;
			AutoCompleter.SetTokenizer(new Classes.SpaceTokenizer()); // Tells the tokenizer to treat each word as its own entry
			var autocompleteElements = MainActivity.InfoNames[(int)Type]; // Gets possible elements to autocomplete to
			var adapter = new ArrayAdapter(Context, Android.Resource.Layout.SimpleDropDownItem1Line, autocompleteElements);
			AutoCompleter.Adapter = adapter;

			var adder = Card.FindViewById<ImageView>(Resource.Id.confirm_input);
			adder.Click += (o, s) => AdderClicked();
			cardTitle.Text = MainActivity.InfoTitles[(int)Type];
			cardSubtitle.Text = Subtitle;
		}

		private void AdderClicked()
		{
			var input = AutoCompleter.Text.ToLower();
			AutoCompleter.Text = string.Empty;
			var compares = MainActivity.InfoNames[(int)Type].ToArray(); // Gets an array of all possible entries to compare input with
			foreach (var item in compares)
			{
				if (input.Contains(item.ToLower()))
				{
					AddCardEntry(item);
				}
			}
		}

		void AddCardEntry(string s)
		{
			var info = Array.IndexOf(MainActivity.InfoNames[(int)Type], s);

			if (!Info.Contains(info))
			{
				var contentCard = Activity.LayoutInflater.Inflate(Resource.Layout.ProfileInformationCardEntry, null);

				var remover = contentCard.FindViewById<ImageView>(Resource.Id.profile_card_entry_remove);
				var entry = contentCard.FindViewById<TextView>(Resource.Id.profile_card_entry_content);

				remover.Click += (se, ev) =>
				{
					var inf = Array.IndexOf(MainActivity.InfoNames[(int)Type], s);

					if (Info.Remove(inf))
						((ViewGroup)contentCard.Parent).RemoveView(contentCard);

				};

				entry.Text = s;
				Info.Add(info);
				remover.Visibility = ViewStates.Visible;
				Content.AddView(contentCard);
			}
		}
	}
}