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
using Java.Lang;
using Android.Text;

namespace Solitude.Droid.Classes
{
	/// <summary>
	/// A calls for autocompleting word in a MultiAutoCompleteTextView
	/// </summary>
	public class SpaceTokenizer : Java.Lang.Object, MultiAutoCompleteTextView.ITokenizer
    {
		/// <summary>
		/// Find the start of the Token
		/// </summary>
        public int FindTokenStart(ICharSequence text, int cursor)
        {
            int i = cursor;

			// Moved i, untill it hits a space, first going back, the going forward
            while (i > 0 && text.CharAt(i - 1) != ' ')
            {
                i--;
            }
            while (i < cursor && text.CharAt(i) == ' ')
            {
                i++;
            }

            return i;
        }

		/// <summary>
		/// Find the end of the Token
		/// </summary>
		public int FindTokenEnd(ICharSequence text, int cursor)
        {
            int i = cursor;
            int len = text.Length();

            while (i < len)
            {
				// If a space is hit, then the token has ended
                if (text.CharAt(i) == ' ')
                {
                    return i;
                }
                else
                {
                    i++;
                }
            }

            return len;
        }
		
        public ICharSequence TerminateToken(ICharSequence text)
        {
            return InnerToken(text);
        }

        public ICharSequence TerminateTokenFormatted(ICharSequence text)
        {
            return InnerToken(text);
        }

		/// <summary>
		/// A method for getting tokens containing mutible words
		/// </summary>
        private ICharSequence InnerToken(ICharSequence text)
        {
            int i = text.Length();

			// Find the first space in the Token, going backwards
            while (i > 0 && text.CharAt(i - 1) == ' ')
            {
                i--;
            }
			
            if (i > 0 && text.CharAt(i - 1) == ' ')
            {
                return text;
            }
            else
            {
                if (text.GetType().IsInstanceOfType(typeof(ISpanned)))
                {
                    SpannableString sp = new SpannableString(text + " ");
                    TextUtils.CopySpansFrom((ISpanned)text, 0, text.Length(), Java.Lang.Class.FromType(typeof(Java.Lang.Object)), sp, 0);
                    return sp;
                }
                else
                {
                    return new Java.Lang.String(text + " ");
                }
            }
        }
    }
}