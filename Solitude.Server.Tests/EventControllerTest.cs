using System;
using NUnit.Framework;
using RestSharp;

using Solitude.Server;
using Model;

namespace Solitude.Server.Tests
{
	[TestFixture()]
	public class EventControllerTest
	{
		private RestClient _client = new RestClient("http://prozum.dk:8080");

		public EventControllerTest ()
		{
			
		}

		[Test()]
		public void TestMethod(int x)
		{
		}

		public void TestAddEvent()
		{
			Event e = new Event ();
			e.Address = "Cassiopeia";
			e.Date = "10-11-2015";
			e.Description = "Kantinen giver gratis sandwich, hcis du tager 10 sprællemænd";
			e.SlotsTaken = 0;
			e.SlotsTotal = 100;
			e.Title = "[Test] : Sandwich og sprællemænd";
			e.UserId = "";

			var request = new RestRequest ("event/add", Method.POST);
			//request.AddHeader ("Authorization", "BEARER " + userToken);
			request.RequestFormat = DataFormat.Json;
			request.AddBody (e);


		}
	}
}

