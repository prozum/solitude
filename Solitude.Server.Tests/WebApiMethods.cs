using System;
using BBBClasses;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Configuration;

namespace Solitude.Server.Tests
{
	public class WebApiMethods : RestClient
	{
		public RestRequest Request;
		public IRestResponse Response;
		public Beer IPA = new Beer()
		{
			Name = "Test IPA",
			ABV = 5.6f,
			EBC = 35,
			IBU = 70,
		};
		public Beer Stout = new Beer()
		{
			Name = "Test Stout",
			ABV = 11.2f,
			EBC = 300,
			IBU = 25
		};
		public Recipe Recipe = new Recipe()
		{
			Quantity = 25,
			MashLiquor = 21,
			MashTime = TimeSpan.FromMinutes(60),
			MashTemperatureCelsius = 65,
			MashIngredients = new MashIngredient[3]
			{
				new MashIngredient(Malt.Pale, 8100),
				new MashIngredient(Malt.LightCrystal, 100),
				new MashIngredient(Malt.Chocolate, 80)
			},

			BoilLiquor = 27,
			BoilTime = TimeSpan.FromMinutes(70),
			BoilIngredients = new BoilIngredient[]
			{
				new BoilIngredient(Hop.Citra, 56, 70),
				new BoilIngredient(Hop.Nelson, 28, 10),
				new BoilIngredient(Hop.Citra, 100, 0)
			},

			FermentationTemperatureCelsius = 27,
			FermentationTime = TimeSpan.FromDays(12 * 7),
			DryIngredients = new DryIngredient[]
			{
				new DryIngredient(Hop.Galaxy, 50, 0),
				new DryIngredient(Hop.Galaxy, 50, 7 * 24)
			},
			Yeast = Yeast.WLP001
		};
		public RecipeTrade Trade = new RecipeTrade()
		{
			Message = "Let us trade two excelent beers!"
		};

		public User User = new User()
		{
			Name = "Dad & son brewing",
			Location = "1st brewstreet",
			Birthdate = DateTimeOffset.UtcNow,
			Password = "passw1",
			ConfirmPassword = "passw1"
		};
		public string Token;

		public WebApiMethods() : base(ConfigurationManager.ConnectionStrings["BeerBrewBuddy"].ConnectionString)
		{
		}

		public void ExecuteRequest(HttpStatusCode expected = HttpStatusCode.OK)
		{
			Response = Execute(Request);
			Assert.AreEqual(expected, Response.StatusCode, "The request failed: " + Response.Content);
		}

		public void BuildRequest(string resource, Method method, object body = null)
		{
			Request = new RestRequest (resource, method);

			Request.AddHeader ("Authorization", "bearer " + Token);
			Request.RequestFormat = DataFormat.Json;
			if (body != null)
				Request.AddBody (body);
		}

		public void RegisterUser ()
		{
			BuildRequest("user", Method.POST, User);
			ExecuteRequest();
		}

		public void Login()
		{
			Request = new RestRequest("token", Method.POST);
			Request.AddParameter("username", User.Username);
			Request.AddParameter("password", User.Password);
			Request.AddParameter("grant_type", "password");
			ExecuteRequest();

			dynamic dynObj = JsonConvert.DeserializeObject (Response.Content);
			Assert.AreEqual("bearer", dynObj.token_type.ToString());
			Assert.AreNotEqual ("", dynObj.access_token.ToString());

			Token = dynObj.access_token;
		}

		public void RegisterUserWrongPassword (string password, string correctErrorMessage)
		{
			RegisterUserWrongPassword(password, password, correctErrorMessage);
		}

		public void RegisterUserWrongPassword (string password, string confirmPassword, string correctErrorMessage)
		{
			var user = new 
			{
				name = User.Name,
				birthdate = User.Birthdate,
				location = User.Location,
				username = "user-" + Guid.NewGuid(),
				password = password,
				confirmPassword = confirmPassword
			};
			BuildRequest("user", Method.POST, user);
			ExecuteRequest(HttpStatusCode.BadRequest);

			string errorMessage = parseErrorMessage (Response);
			Assert.IsTrue (errorMessage.Contains(correctErrorMessage), errorMessage);
		}

		public void RegisterUserWrongDateTime (string correctErrorMessage)
		{
			var user = new 
			{
				name = User.Name,
				birthdate = "2015",
				address = User.Location,
				username = "user-" + Guid.NewGuid(),
				password = User.Password,
				confirmPassword = User.ConfirmPassword
			};
			BuildRequest("user", Method.POST, user);
			ExecuteRequest(HttpStatusCode.BadRequest);

			Assert.IsTrue (Response.Content.Contains(correctErrorMessage), Response.Content);
		}

		public void AddReview ()
		{
			var review = new Review()
			{
				AromaRating = 5,
				BeerId = this.IPA.Id,
				LookRating = 5,
				MouthfeelRating = 5,
				TasteRating = 5,
				Text = "Probably the best beer ever brewed!",
				UserId = User.Id
			};

			BuildRequest("review", Method.POST, review);
			ExecuteRequest();
		}

		public void AddBeer()
		{
			BuildRequest("beer", Method.POST, IPA);
			ExecuteRequest();

            IPA.Id = JsonConvert.DeserializeObject<Beer>(Response.Content).Id;
		}

		public void GetBeer()
		{
			BuildRequest ("beer", Method.GET);
			ExecuteRequest();

			var beers = JsonConvert.DeserializeObject<IEnumerable<Beer>>(Response.Content);
			var receivedBeer = beers.Where ((b) => b.Id == IPA.Id).First();
			Assert.AreEqual (IPA.Id, receivedBeer.Id, "The received event was not equal to the one created", receivedBeer);
		}
			
		public void GetOffers ()
		{
			BuildRequest("offer", Method.GET);
			ExecuteRequest();

			//Testing if the returned event has an Id.
			var trades = JsonConvert.DeserializeObject<IEnumerable<RecipeTrade>>(Response.Content);
			Trade.TradeId = trades.Last().TradeId;
			Assert.AreNotEqual("", Trade.TradeId, "An error has occured, it is likely no offers were returned: " + Response.Content);
		}

		public void AcceptOffer()
		{
			BuildRequest ("offer/" + Trade.TradeId, Method.POST, true);
			ExecuteRequest();
		}

		public void DeclineOffer()
		{
			BuildRequest ("offer/" + Trade.TradeId, Method.DELETE, false);
			ExecuteRequest();
		}

		public void UpdateBeerChangeName()
		{
			IPA.Name = "Updated IPA";
			BuildRequest ("host", Method.PUT, IPA);
			ExecuteRequest();

			//Getting the event from the server again:
			BuildRequest ("host", Method.GET);
			ExecuteRequest();

			var beers = JsonConvert.DeserializeObject<IEnumerable<Beer>>(Response.Content);
			var receivedBeer = beers.Where((b) => b.Id == IPA.Id).First();
			Assert.AreEqual(IPA.Name, receivedBeer.Name, "The recieved event did not have the updated title.");
		}

		public void DeleteBeer()
		{
			BuildRequest ("beer/" + IPA.Id, Method.DELETE);
			ExecuteRequest();

			//Now try to get event:
			BuildRequest("beer", Method.GET);
			ExecuteRequest();

			var beers = JsonConvert.DeserializeObject<IEnumerable<Beer>>(Response.Content);
			Assert.IsNull(beers.Where((b) => b.Id == IPA.Id).FirstOrDefault());
		}

		public void DeleteUser ()
		{
			BuildRequest("user", Method.DELETE);
			ExecuteRequest();

			Request = new RestRequest("token", Method.POST);
			Request.AddParameter("username", User.Username);
			Request.AddParameter("password", User.Password);
			Request.AddParameter("grant_type", "password");
			ExecuteRequest(HttpStatusCode.BadRequest);
		}

		public void GetUserData()
		{
			BuildRequest ("user", Method.GET);
			ExecuteRequest();

			dynamic receivedUserData = JsonConvert.DeserializeObject (Response.Content);
			Assert.AreEqual (User.Username, receivedUserData.UserName.ToString(), "The received username is not the same as the actual: " + Response.Content);
		}

		string parseErrorMessage(IRestResponse response)
		{
			string errorContent = response.Content;
			string[] splitErrorContent = errorContent.Split(':');
			return splitErrorContent [splitErrorContent.Length - 1]
				.Trim ('"', ':', '\\', '[', ']', '{', '}');
		}

		string parseDateTimeErrorMessage(IRestResponse response)
		{
			string errorContent = response.Content;
			string[] splitErrorContent = errorContent.Split(':');
			return splitErrorContent [splitErrorContent.Length - 3]
				.Trim ('"', ':', '\\', '[', ']', '{', '}');
		}
	}
}

