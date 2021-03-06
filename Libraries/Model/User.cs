﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System;

namespace Model
{
	public class User
	{
		[JsonIgnore]
		public Guid Id { set; get; }

		[Required]
		[Display(Name = "Name")]
		public string Name { set; get; }

		[Required]
		[Display(Name = "Location")]
		public string Location { set; get; }

		[Required]
		[Display(Name = "Birthdate")]
		[DataType(DataType.DateTime)]
		public DateTimeOffset Birthdate { set; get; }

		[Required]
		[Display(Name = "Username")]
		public string Username { set; get; }

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

