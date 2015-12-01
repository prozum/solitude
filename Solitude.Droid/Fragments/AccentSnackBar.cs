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
using Android.Support.Design.Widget;

namespace Solitude.Droid
{
	public static class AccentSnackBar
	{
		public static Snackbar Make(View view, Context context, int resource, int time)
		{
			return Make(Snackbar.Make(view, resource, time), context);
		}
		public static Snackbar Make(View view, Context context, string text, int time)
		{
			return Make(Snackbar.Make(view, text, time), context);
		}
		private static Snackbar Make(Snackbar snack, Context context)
		{
			snack.View.FindViewById<TextView>(Resource.Id.snackbar_text)
				 .SetTextColor(context.Resources.GetColor(Resource.Color.accent_text));
			return snack;
		}
	}
}