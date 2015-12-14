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

namespace Solitude.Droid
{
	/// <summary>
	/// The abstract fragment for all fragments, that are a part of an 
	/// information editing process, such as sign up.
	/// EditFragment is meant to be used in viewpagers.
	/// </summary>
	public abstract class EditFragment : Android.Support.V4.App.Fragment
	{
		/// <summary>
		/// The bool representing whether the keyboard should be hidden when the 
		/// viewpager switches to this fragment.
		/// </summary>
		public bool HidesKeyboard { get; set; }

		/// <summary>
		/// When overridden, this function should save all the information in the
		/// fragment, that is needed for later use.
		/// The information should be accessible for the viewpager adapter.
		/// </summary>
		public abstract void SaveInfo();

		/// <summary>
		/// When overriden, this method should check, whether the information
		/// in the fragment can be saved.
		/// </summary>
		/// <returns>true, if the information can be saved, else false.</returns>
		public abstract bool IsValidData();
	}
}