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
	/// <summary>
	/// A static helperclass, for making snakebars with the right textcolor.
	/// </summary>
	public static class AccentSnackBar
	{
		/// <summary>
		/// Makes a snackbar.
		/// </summary>
		/// <param name="view">The view that should contain the snackbar.</param>
		/// <param name="context">The context for getting resources.</param>
		/// <param name="resource">The textresource that should be used in the 
		/// snackbar</param>
		/// <param name="time">How long the snackbar should be shown (ms).</param>
		/// <returns>The finished snackbar.</returns>
		public static Snackbar Make(View view, Context context, int resource, int time)
		{
			return Make(Snackbar.Make(view, resource, time), context);
		}
		/// <summary>
		/// Makes a snackbar.
		/// </summary>
		/// <param name="view">The view that should contain the snackbar.</param>
		/// <param name="context">The context for getting resources.</param>
		/// <param name="resource">The string should be used in the snackbar</param>
		/// <param name="time">How long the snackbar should be shown (ms).</param>
		/// <returns>The finished snackbar.</returns>
		public static Snackbar Make(View view, Context context, string text, int time)
		{
			return Make(Snackbar.Make(view, text, time), context);
		}
		/// <summary>
		/// A overloaded helpermethod, for setting the color of a snackbars text.
		/// </summary>
		/// <param name="snack">The snackbar which textcolor should be set.</param>
		/// <param name="context">The context for getting resources.</param>
		/// <returns>The finished snackbar.</returns>
		private static Snackbar Make(Snackbar snack, Context context)
		{
			// Finds the textview in the snackbar, and the sets the color.
			snack.View.FindViewById<TextView>(Resource.Id.snackbar_text)
				 .SetTextColor(context.Resources.GetColor(Resource.Color.accent_text));

			return snack;
		}
	}
}