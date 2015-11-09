using NUnit.Framework;
using System;
using RestSharp;
using System.Net;

namespace Solitude.Server.Tests
{
	[TestFixture ()]
	public class Test
	{
		RestClient testClient = new RestClient("http://prozum.dk:8080/api/");
		Random r = new Random();
		string testUsername = "TestKurt" + r.Next(1, 100000);
		[Test ()]
		public void TestCase ()
		{

		}

		[Test ()]
		public void TestCaseRegisterUser ()
		{
			//Build request and user
			var request = new RestRequest ("user/register", Method.POST);
			var user = new 
			{
				username = testUsername,
				password = "Testkurt123!",
				confirmedPassword = "Testkurt123!"
			};

			//Alters request format to json and add information
			request.RequestFormat = DataFormat.Json;
			request.AddBody (user);
			var response = testClient.Execute (request);
			Assert.IsTrue (response.ResponseStatus == HttpStatusCode.OK);
		}
	}
}

