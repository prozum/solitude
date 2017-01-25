using Newtonsoft.Json;
using System;

namespace BBBClasses
{
	public class User
	{
		[JsonIgnore]
		public Guid Id { set; get; }
		
		public string Name { set; get; }
		
		public string Location { set; get; }
		
		public DateTimeOffset Birthdate { set; get; }
		
		public string Username { set; get; }

		public string Password { set; get; }

		public string ConfirmPassword { get; set; }
	}
}

