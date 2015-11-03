using System;

namespace DineWithaDane.Android
{
	public class User
	{
		public string Name;
		public string Address;

		private string username;
		private string password;

		public int ID;

		public User (string name, string address, int id)
			: this (name, address)
		{
			this.ID = id;
		}

		public User (string name, string address)
		{
			this.Name = name;
			this.Address = address;
		}

		public User (string name, string address, string username, string password)
			: this(name, address)
		{
			this.username = username;
			this.password = password;
		}
	}
}

