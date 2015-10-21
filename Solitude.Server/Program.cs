using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitude.Server
{
	class Program
	{
		static void Main (string[] args)
		{
			string baseUri = "http://localhost:8080";

			Console.WriteLine ("Starting web Server...");
			WebApp.Start<Startup> (baseUri);
			Console.WriteLine ("Server running at {0} - press Enter to quit. ", baseUri);
			Console.WriteLine ("I'm running on {0} directly from assembly {1}", Environment.OSVersion, System.Reflection.Assembly.GetEntryAssembly ().FullName);
			Console.ReadLine ();
		}
	}
}