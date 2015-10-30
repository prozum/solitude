using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;

namespace JsonTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string username = "MikkelMus";
			string password = "hejhej123";
			string passwordConfirm = "hejhej123";

			var jsonDoc = JsonConvert.SerializeObject (new { username = username, password = password, 
				passwordConfirm = passwordConfirm });

			string url = "http://127.0.0.1:8080/api/user/register";
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "application/json";
			request.Method = "POST";

			using (var upStream = new StreamWriter (request.GetRequestStream())) {
				upStream.Write (jsonDoc);
				upStream.Flush ();
				upStream.Close ();
			}
//			using (var response = request.GetResponse()) {
//				string r = response.ToString ();
//				Console.WriteLine (r);
//			}

			Console.ReadKey ();
		}
	}
}
