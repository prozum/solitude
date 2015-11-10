using NUnit.Framework;
using System;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

namespace Solitude.Server.Tests
{
	[TestFixture ()]
	public class Test
	{
		public RestClient testClient = new RestClient("http://prozum.dk:8080/api/");
		public Random r = new Random();
		public string testUsername, testToken = "", token_type = "";

		public Test ()
		{
			testUsername = "ServerTestUser" + r.Next(1, 1000000);
		}
		[Test ()]
		public void TestCase ()
		{

		}

		[Test ()]
		public void TestCaseRegisterUser ()
		{
			var request = new RestRequest ("user/register", Method.POST);
			var user = new 
			{
				username = testUsername,
				password = "Testkurt123!",
				confirmedPassword = "Testkurt123!"
			};

			request.RequestFormat = DataFormat.Json;
			request.AddBody (user);
			var response = testClient.Execute (request);
			Assert.IsTrue (response.StatusCode == HttpStatusCode.OK);
		}

		public void Login()
		{
			//Generates the token-request
			var request = new RestRequest("token", Method.POST);

			//Adds headers to notify server that it's a token-request
			request.AddHeader("content-type", "x-www-form-urlencoded");
			request.AddHeader ("postman-token", "a4e85886-daf2-5856-b530-12ed21af5867");
			request.AddHeader("cache-control", "no_cache");

			//Adds body including username and password and specify, that a grant_type as password is desired
			request.AddParameter("x-www-form-urlencoded", String.Format("username={0}&password={1}&grant_type=password", testUsername, "Testkurt123!"), ParameterType.RequestBody);

			//Execute and await response, parse afterwards
			var tokenResponse = testClient.Execute (request);

			Assert.IsTrue (tokenResponse.StatusCode != 0, "[Login failed] Likely there's no internet connection.");

			dynamic dynObj = JsonConvert.DeserializeObject (tokenResponse.Content);

			Assert.IsTrue (tokenResponse.StatusCode == HttpStatusCode.OK);

			testToken = dynObj.access_token;
			token_type = dynObj.token_type;

			Assert.AreNotEqual (testToken, "");
			Assert.AreNotEqual (token_type, "");
		}
	}
}

