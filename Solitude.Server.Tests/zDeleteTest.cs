using System;
using RestSharp;
using NUnit.Framework;
using System.Net;

namespace Solitude.Server.Tests
{/*
	[TestFixture ()]
	public class zDeleteTest
	{
		[Test ()]
		public void zzzDeleteTest ()
		{
			var deleteRequest = new RestRequest ("user", Method.DELETE);
			deleteRequest.RequestFormat = DataFormat.Json;

			deleteRequest.AddHeader("Authorization", "bearer " + Values.testToken);

			//Adds body to the request
			var body = new {
				userToken = Values.testToken
			};
			deleteRequest.AddBody(body);
			var response = Values.testClient.Execute (deleteRequest);

			var request = new RestRequest("token", Method.POST);

			request.AddHeader("content-type", "x-www-form-urlencoded");
			request.AddHeader ("postman-token", "a4e85886-daf2-5856-b530-12ed21af5867");
			request.AddHeader("cache-control", "no_cache");

			request.AddParameter("x-www-form-urlencoded", String.Format("username={0}&password={1}&grant_type=password", Values.testUsername, Values.password), ParameterType.RequestBody);

			var tokenResponse = Values.testClient.Execute (request);

			Assert.IsFalse (tokenResponse.StatusCode == HttpStatusCode.OK, "Login succeeded unexpectedly " + response.StatusCode.ToString());
		}
	}*/
}

