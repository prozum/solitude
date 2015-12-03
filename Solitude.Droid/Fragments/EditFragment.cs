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
	public abstract class EditFragment : Android.Support.V4.App.Fragment
	{
		public bool HidesKeyboard { get; set; }

		public abstract void SaveInfo();
		public abstract bool IsValidData();
	}
}