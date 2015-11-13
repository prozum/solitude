using System;

namespace DineWithaDane.Android
{
	public class User
	{
		public int ID;
		public string Name { get; set; }
		public string Address { get; set; }
		public DateTime Birthday { get; set; }

		private string username;
		private string password;

		public User (string name, string address, int id)
			: this (name, address)
		{
			ID = id;
		}

		public User (string name, string address)
		{
			Name = name;
			Address = address;
		}

		public User (string name, string address, string username, string password)
			: this(name, address)
		{
			this.username = username;
			this.password = password;
		}

		public User (string name, string address, DateTime birthday)
			: this (name, address)
		{
			this.Birthday = birthday;
		}
	}
}

