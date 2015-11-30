
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
using Android.Views.InputMethods;
using Android.Support.V7.Widget;

namespace Solitude.Droid
{
	public class SignUpFragmentInterests : Android.Support.V4.App.Fragment
	{
		//ListView interestListView;
		LayoutInflater inflators;
		List<bool> interestList = new List<bool>();
		protected View cardInterest { get; set; }
		protected bool isEditing { get; set; }
		public List<int> Interests
		{
			get
			{
				for (int i = 0; i < 2; i++)
					//if ((interestListView.GetChildAt(i) as CheckBox).Checked)
						Interests.Add(i);

				return Interests;
			}
		}
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			#region Checkbox
			var userInterests = new List<int>();
			//Inflate view and find content
			//View view = inflater.Inflate(Resource.Layout.signupFragLayout4, container, false);
			/*
			interestListView = view.FindViewById <ListView>(Resource.Id.interestListView);
			var desc = view.FindViewById<TextView>(Resource.Id.signupListDescription);

			//Adds the description
			desc.Text = GetString(Resource.String.profile_menu_edit_interests);

			//Populate ListView
			interestListView.Adapter = new ArrayAdapter<string>(Activity, 
				Resource.Layout.CheckedListViewItem, MainActivity.InfoNames[(int)InfoType.Interest]);
			interestListView.ChoiceMode = ChoiceMode.Multiple; */
			#endregion




			inflators = inflater;
			cardInterest = inflater.Inflate(Resource.Layout.ProfileInformationCard, null);
			CreateCard(InfoType.Interest, cardInterest, "Fisk");
			return cardInterest;
		}
		public void CreateCard(InfoType type, View card, string subtitle)
		{

			var cardTitle = card.FindViewById<TextView>(Resource.Id.profile_card_title);
			var cardSubtitle = card.FindViewById<TextView>(Resource.Id.profile_card_subtitle);
			var content = card.FindViewById<LinearLayout>(Resource.Id.profile_card_entry);

			foreach (var item in new List<string>() { "something", "nothing", "everything" })
			{
				AddCardEntry(content, item);
			}
			cardTitle.Text = MainActivity.InfoTitles[(int)type];
			cardSubtitle.Text = subtitle;
		}
		private void AddCardEntry(LinearLayout content, string s)
		{
			var contentCard = inflators.Inflate(Resource.Layout.ProfileInformationCardEntry, null);

			var remover = contentCard.FindViewById<ImageView>(Resource.Id.profile_card_entry_remove);
			var entry = contentCard.FindViewById<TextView>(Resource.Id.profile_card_entry_content);

			remover.Click += (se, ev) =>
				{
					((ViewGroup)contentCard.Parent).RemoveView(contentCard); // Removes entry from card when clicked
				};

			entry.Text = s;
			remover.Visibility = isEditing ? ViewStates.Visible : ViewStates.Invisible; // Checks if the remove burron should be shown when added.

			content.AddView(contentCard);
		}
	}
}

