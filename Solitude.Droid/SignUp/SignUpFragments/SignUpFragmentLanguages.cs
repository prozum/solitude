
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

namespace Solitude.Droid
{
	public class SignUpFragmentLanguages : AbstractSignupFragment
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			CreateCard(InfoType.Language, signUpCard, GetString(Resource.String.sign_up_languages));
			return signUpCard;
		}

		//Loads all info in the signUpInfo list into a list of InfoChanges and returns it.
		public List<InfoChange> SaveInfo()
		{
			List<InfoChange> languageList = new List<InfoChange>();
			foreach (var info in signUpInfo)
			{
				languageList.Add(new InfoChange(InfoType.Language, (int)Enum.Parse(typeof(Language), info), 1));
			}
			return languageList;
		}
	}
}

