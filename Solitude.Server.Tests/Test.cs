using NUnit.Framework;
using System;
using RestSharp;
using System.Net;
using Newtonsoft.Json;

namespace Solitude.Server.Tests
{
	[TestFixture ()]
	public class aTest
	{
		public aTest ()
		{
			Values.testUsername = "ServerTestUser" + Values.r.Next(1, 1000000);
		}
		/*[Test ()]
		public void TestCase ()
		{

		}*/

		[Test()]
		public void _1TestCaseRegisterUser ()
		{
			var request = new RestRequest ("user/register", Method.POST);
			var user = new 
			{
				username = Values.testUsername,
				password = Values.password,
				confirmPassword = Values.password
			};

			request.RequestFormat = DataFormat.Json;
			request.AddBody (user);
			var response = Values.testClient.Execute (request);
			Assert.IsTrue (response.StatusCode == HttpStatusCode.OK, response.Content); //Test something else
		}

		[Test()]
		public void _2Login()
		{
			var request = new RestRequest("token", Method.POST);

			request.AddHeader("content-type", "x-www-form-urlencoded");
			request.AddHeader ("postman-token", "a4e85886-daf2-5856-b530-12ed21af5867");
			request.AddHeader("cache-control", "no_cache");

			request.AddParameter("x-www-form-urlencoded", String.Format("username={0}&password={1}&grant_type=password", Values.testUsername, Values.password), ParameterType.RequestBody);

			var tokenResponse = Values.testClient.Execute (request);

			Assert.IsFalse (tokenResponse.StatusCode == 0, "[Login failed] Likely there's no internet connection.");

			dynamic dynObj = JsonConvert.DeserializeObject (tokenResponse.Content);

			Assert.IsTrue (tokenResponse.StatusCode == HttpStatusCode.OK, tokenResponse.Content);

			Values.testToken = dynObj.access_token;
			Values.token_type = dynObj.token_type;

			Assert.AreNotEqual (Values.testToken, "");
			Assert.AreNotEqual (Values.token_type, "");
		}
		[Test ()]
		public void TestCaseAddReview ()
		{
			var request = new RestRequest ("review", Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader ("Authorization", "bearer " + Values.testToken);

			var review = new {
				rating = 3,
				reviewTect = "Koldt"//, eventID = 1234
			};
			request.AddBody(review);

			var response = Values.testClient.Execute (request);
			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request failed " + response.Content);
		}
		[Test ()]
		public void TestCaseAddInterest ()
		{
			var request = new RestRequest ("info", Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddHeader ("Authorization", "bearer " + Values.testToken);

			var InfoUpdate = new {
				Info = 1,
				val = 5
			};
			request.AddBody (InfoUpdate);
			var response = Values.testClient.Execute (request);
			Assert.AreEqual (HttpStatusCode.OK, response.StatusCode, "The request failed " + response.Content);
		}
	}
}

