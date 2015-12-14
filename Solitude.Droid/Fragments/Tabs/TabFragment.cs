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
	/// The abstract fragment for all fragments, that are a part of tabs.
	/// TabFragment is meant to be used in viewpagers with tablayouts.
	/// </summary>
	public abstract class TabFragment : Android.Support.V4.App.Fragment
	{
		/// <summary>
		/// The position of the fragment.
		/// </summary>
		public int Position { get; set; }

		/// <summary>
		/// This method is called when this fragment is seleted.
		/// </summary>
		public abstract void OnSelected();
	}
}