
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
using Android.Graphics;

namespace DineWithaDane.Android
{
	public class Seperator : TextView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.Seperator"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public Seperator(Context context)
			: this(context, new Color(255, 255, 255)) { }
		
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Android.Seperator"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="color">Color.</param>
		public Seperator(Context context, Color color)
			: base(context)
		{
			SetBackgroundColor(color);
			SetMinHeight(1);
			SetMaxHeight(1);
		}
	}
}

