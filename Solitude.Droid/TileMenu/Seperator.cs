
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

namespace Solitude.Droid
{
	public class Seperator : TextView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Solitude.Droid.Seperator"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="heigth">Heigth.</param>
		/// <param name="width">Width.</param>
		public Seperator(Context context, int heigth = 2, int width = 2)
			: this(context, Color.Gray, heigth, width) { } 

		/// <summary>
		/// Initializes a new instance of the <see cref="Solitude.Droid.Seperator"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <param name="color">Color.</param>
		/// <param name="heigth">Heigth.</param>
		/// <param name="witdh">Witdh.</param>
		public Seperator(Context context, Color color, int heigth = 2, int width = 2)
			: base(context)
		{
			SetBackgroundColor(color);
			SetMinHeight(heigth);
			SetMaxHeight(heigth);
			SetMaxWidth(width);
			SetMinWidth(width);
		}
	}
}

