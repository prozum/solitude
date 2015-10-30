using System;

namespace DAL
{
	public static class Language
	{
		public enum LanguageCode
		{
			DANISH 		= 0,
			ENGLISH		= 1,
			GERMAN		= 2,
			FRENCH		= 3,
			SPANISH		= 4,
			CHINESE		= 5,
			RUSSIAN		= 6
		}

		public static string Danish = "Danish";
		public static string English = "English";
		public static string German = "German";
		public static string French = "French";
		public static string Spanish = "Spanish";
		public static string Chinese = "Chinese";
		public static string Russian = "Russian";

		public static string GetLanguage (LanguageCode ic)
		{
			switch (ic) {
			case LanguageCode.DANISH:
				return Danish;
			case LanguageCode.ENGLISH:
				return English;
			case LanguageCode.GERMAN:
				return German;
			case LanguageCode.FRENCH:
				return French;
			case LanguageCode.SPANISH:
				return Spanish;
			case LanguageCode.CHINESE:
				return Chinese;
			case LanguageCode.RUSSIAN:
				return Russian;
			default:
				return null;
			}
		}
	}
}

