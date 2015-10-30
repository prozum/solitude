using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using RestSharp;

namespace JsonTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var client = new RestClient("http://127.0.0.1:8080/api/");
			client.Options (new RestRequest () {RequestFormat = DataFormat.Json,  });

			var user = new 
			{	
				username = "MikkelMus10", 
				password = "hej%Hej123",
				passwordConfirm = "hej%Hej123"
			};

			var request = new RestRequest("user/register", Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddBody(user);
			Console.WriteLine (request.ToString ());

			IRestResponse response = client.Execute(request);

			Console.WriteLine (response.Content);
			Console.ReadKey ();
		}
	}
}
