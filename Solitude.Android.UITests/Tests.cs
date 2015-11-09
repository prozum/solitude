using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;
using System.Diagnostics;

namespace DineWithaDane.Android.UITests
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

			testUsername = "UI_test_user_@+ID/" + rand.Next();
			testPassword = "UI_test_password_@+id/" + rand.Next();

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
		public void GotoProfile()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Profile"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Profile"));

			Assert.IsTrue(result.Any(), "Profile");
		}

		[Test]
		public void GotoOffer()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Offer"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Offer"));

			Assert.IsTrue(result.Any(), "Offer");
		}

		[Test]
		public void GotoHost()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Host"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Host"));

			Assert.IsTrue(result.Any(), "Host");
		}

		[Test]
		public void GotoEvents()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Events"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Events"));

			Assert.IsTrue(result.Any(), "Events");
		}

		[Test]
		public void GotoSettings()
		{
			_Login();

			app.Tap(c => c.Id("action_bar_container"));

			app.Tap(c => c.Id("text1").Text("Settings"));

			System.Threading.Thread.Sleep(5000);

			AppResult[] result = app.Query(c => c.Id("action_bar_title").Text("Settings"));

			Assert.IsTrue(result.Any(), "Settings");
		}
	}
}

