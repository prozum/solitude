
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
using Android.Support.Design.Widget;

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
			{
				interestList.Add(new InfoChange(InfoType.Interest, (int)Enum.Parse(typeof(Interest), info)));
			}
			return interestList;
		}
	}
}

