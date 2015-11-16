using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System;

namespace Model
{
	public class User
	{
		[JsonIgnore]
		public string Id { set; get; }

		[Required]
		[Display(Name = "Name")]
		public string Name { set; get; }

		[Required]
		[Display(Name = "Address")]
		public string Address { set; get; }

		[Required]
		[Display(Name = "Birthdate")]
		[DataType(DataType.DateTime)]
		public DateTime Birthdate { set; get; }

		[Required]
		[Display(Name = "User name")]
		public string UserName { set; get; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { set; get; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}
}

