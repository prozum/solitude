using System;

namespace DineWithaDane.Android
{
	public class User
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public DateTimeOffset Birthdate { get; set; }

		public string UserName { get; set; }
		private string password;

		public User (string name, string address, string id)
			: this (name, address)
		{
			Id = id;
		}

		public User()
		{
			
		}

		public User (string name, string address)
		{
			Name = name;
			Address = address;
		}

		public User (string name, string address, string username, string password)
			: this(name, address)
		{
			this.UserName = username;
			this.password = password;
		}

		public User (string name, string address, DateTimeOffset birthday)
			: this (name, address)
		{
			this.Birthdate = birthday;
		}
	}
}

