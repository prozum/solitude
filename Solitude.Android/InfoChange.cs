using System;

namespace DineWithaDane.Android
{
	public class InfoChange
	{
		private string _key;
		private string _value;

		public string Key { get { return _key; } }
		public string Value { get { return _value; } }


		public InfoChange (string key, string value)
		{
			this._key = key;
			this._value = value;
		}
	}
}

