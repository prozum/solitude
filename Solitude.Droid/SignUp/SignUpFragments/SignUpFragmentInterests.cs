using System;
using Android.OS;
using Android.Views;
using System.Collections.Generic;

namespace Solitude.Droid
{
	public class SignUpFragmentInterests : AbstractSignupFragment
	{

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			CreateCard(InfoType.Interest, signUpCard, GetString(Resource.String.sign_up_interests));

			return signUpCard;
		}

		//Loads all info in the signUpInfo list into a list of InfoChanges and returns it.
		public List<InfoChange> SaveInfo()
		{
			List<InfoChange> interestList = new List<InfoChange>();

			foreach (var info in signUpInfo)
				interestList.Add(new InfoChange(InfoType.Interest, info, 1));

			return interestList;
		}
	}
}

