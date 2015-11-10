using System;
using RestSharp;

namespace Solitude.Server.Tests
{
	public static class Values
	{
		public static RestClient testClient = new RestClient("http://prozum.dk:8080/api/");
		public static Random r = new Random();
		public static string testUsername, testToken = "", token_type = "";
		public static string password = "Testkurt123!";
	}
}

