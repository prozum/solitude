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
    public class SpaceTokenizer : Java.Lang.Object, MultiAutoCompleteTextView.ITokenizer
    {
        public int FindTokenStart(ICharSequence text, int cursor)
        {
            int i = cursor;

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

        public int FindTokenEnd(ICharSequence text, int cursor)
        {
            int i = cursor;
            int len = text.Length();

            while (i < len)
            {
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

        private ICharSequence InnerToken(ICharSequence text)
        {
            int i = text.Length();

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