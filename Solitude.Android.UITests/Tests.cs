using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;
using System.Diagnostics;

namespace Solitude.Droid.UITests
{
	[TestFixture]
	public class Tests
	{
		IApp app;
		string testUsername;
		string testPassword;

		[SetUp]
		public void BeforeEachTest ()
		{
			app = ConfigureApp.Android.PreferIdeSettings().StartApp();
		}

		[Test]
		public void _CreateUser()
		{
			app.Tap(c => c.Id("buttonSignUp"));

			System.Threading.Thread.Sleep(5000);

			Random rand = new Random();

			testUsername = "test" + rand.Next();
			testPassword = "T3$t" + rand.Next();

			app.Tap(c => c.Id("editName"));

			app.EnterText("Test User " + rand.Next());

			app.Tap(c => c.Id("editUsername"));

			app.EnterText(testUsername);

			app.Tap(c => c.Id("editPassword"));

			app.EnterText(testPassword);

			app.Tap(c => c.Id("editConfirm"));

			app.EnterText(testPassword);

			app.Tap(c => c.Id("buttonSubmit"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Login"));

			Assert.IsTrue(result.Any(), "Create user");
		}

		[Test]
		public void _Login()
		{
			app.Tap(c => c.Id("editUsername"));
			app.EnterText(testUsername);

			app.Tap(c => c.Id("editPassword"));
			app.EnterText(testPassword);

			app.Tap(c => c.Id("buttonLogin"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Notifications"));

			Assert.IsTrue(result.Any(), "Notifications");
		}

		[Test]
		public void _Logout()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Logout"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Login"));

			Assert.IsTrue(result.Any(), "Logout");
		}

		[Test]
		public void DrawerProfile()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Profile"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Profile"));

			Assert.IsTrue(result.Any(), "Profile");
		}

		[Test]
		public void DrawerOffer()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Offer"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Offer"));

			Assert.IsTrue(result.Any(), "Offer");
		}

		[Test]
		public void DrawerHost()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Host"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Host"));

			Assert.IsTrue(result.Any(), "Host");
		}

		[Test]
		public void DrawerEvents()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Events"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Events"));

			Assert.IsTrue(result.Any(), "Events");
		}

		[Test]
		public void DrawerSettings()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Settings"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Settings"));

			Assert.IsTrue(result.Any(), "Settings");
		}

		[Test]
		public void HostEvent()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Host"));

			app.Tap(c => c.Text("Host New Event"));

			app.Tap(c => c.Id("NoResourceEntry-1"));

			app.EnterText("Test title");

			app.Tap(c => c.Id("NoResourceEntry-2"));

			app.EnterText("Test description");

			app.Tap(c => c.Id("NoResourceEntry-3"));

			app.EnterText("Test address");

			app.Tap(c => c.Id("NoResourceEntry-4"));

			app.EnterText("3");

			// Can't interact with date-picker

			// Can't interact with time-picker

			app.ScrollDownTo(c => c.Id("NoResourceEntry-7"));

			app.Tap(c => c.Id("NoResourceEntry-7"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Host"));

			Assert.IsTrue(result.Any(), "Host");
		}
	}
}

