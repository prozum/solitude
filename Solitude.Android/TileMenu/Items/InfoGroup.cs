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

namespace DineWithaDane.Droid
{
	public class InfoGroup : TileListGroup
	{
		#region Field
		/// <summary>
		/// Sets the title of the view.
		/// </summary>
		public string Title { set { TitleView.Text = value; } }

		protected TextView TitleView { get; set; }
		protected ImageView Arrow { get; set; }
		#endregion


		#region Contructors
		/// <summary>
		/// Initializes a new instance of the <see cref="DineWithaDane.Droid.EventGroup"/> class.
		/// </summary>
		/// <param name="context">Context.</param>
		public InfoGroup(Context context)
			: base(context)
		{
			TitleView = new TextView(context);
			Arrow = new ImageView(context);

			Initialize();
		}
		#endregion

		#region Public Methods
		public void SetArrowDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.Down:
					Arrow.SetImageResource(Resource.Drawable.ArrowDown);
					break;
				case Direction.Up:
					Arrow.SetImageResource(Resource.Drawable.ArrowUp);
					break;
				default:
					throw new NotImplementedException();
			}
		}
		#endregion

		#region Private Methods
		protected override void Initialize()
		{
			var layout = new RelativeLayout(Context);
			var titleparams = new RelativeLayout.LayoutParams(-2, -2);
			var arrowparmas = new RelativeLayout.LayoutParams(80, -2);

			layout.AddView(TitleView);
			layout.AddView(Arrow);
			AddView(layout);
			AddView(SeperatorView);

			TitleView.TextSize = 20;
			TitleView.SetPadding(0, 8, 0, 8);

			Arrow.Id = 22;
			TitleView.Id = 23;
			SeperatorView.LayoutParameters.Width = -1;
			titleparams.AddRule(LayoutRules.LeftOf, Arrow.Id);
			titleparams.AddRule(LayoutRules.AlignParentLeft);
			arrowparmas.AddRule(LayoutRules.AlignParentRight);
			arrowparmas.AddRule(LayoutRules.AlignTop, TitleView.Id);
			arrowparmas.AddRule(LayoutRules.AlignBottom, TitleView.Id);
			Arrow.SetPadding(0, 20, 0, 20);

			TitleView.LayoutParameters = titleparams;
			Arrow.LayoutParameters = arrowparmas;
		}
		#endregion
	}
}