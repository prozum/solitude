using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;
using System.IO;
using BBBClasses;

namespace Dal
{
	public class DatabaseAbstrationLayer : IDisposable
	{
		private readonly GraphClient _client;

		private readonly string _dataDir;
		private readonly string _userDir = "users";
		private readonly string _pictureDir = "pictures";
		private readonly string _profilePicture = "profile";

		public DatabaseAbstrationLayer(GraphClient client, string dataDir)
		{
			_client = client;
			_dataDir = dataDir;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public async Task CleanUnusedFields()
		{
			await _client.Cypher
				.OptionalMatch("()-[r]->(beer:Beer)")
				.Where((Relationship r) => r == null)
				.Delete("beer")
				.ExecuteWithoutResultsAsync();

			await _client.Cypher
				.OptionalMatch("()->[r]->(recipe:Recipe)")
				.Where((Relationship r) => r == null)
				.Delete("recipe")
				.ExecuteWithoutResultsAsync();
		}

        #region User

        public async Task<UserData> GetUserData(Guid uid)
        {
            var res = await _client.Cypher
                .Match("(user:User)")
                .Where((User user) => user.Id == uid)
                .Return(() => Return.As<UserData>("user"))
                .ResultsAsync;

            return res.First();
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="uid">The user's id</param>
        public async Task DeleteUser(Guid uid)
        {
            await _client.Cypher
                .Match("(user:User)")
                .Where((User user) => user.Id == uid)
                .Delete("user")
                .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Deletes the user's data, used as a help function for user deletion
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="uid">The user's id</param>
        public async Task DeleteUserData(Guid uid)
        {
            // Delete all beers and recipes
           await _client.Cypher
                .Match("(user:User)-[r:BREWS]->(Beer)")
                .Where((User user) => user.Id == uid)
                .Delete("r")
                .ExecuteWithoutResultsAsync();

			await _client.Cypher
				.Match("(user:User)-[r:KNOWS]->(Recipe)")
				.Where((User user) => user.Id == uid)
				.Delete("r")
				.ExecuteWithoutResultsAsync();


			// Delete the remaining releations
			await _client.Cypher
                .OptionalMatch("(user:User)-[r]->()")
                .Where((User user) => user.Id == uid)
                .Delete("r")
                .ExecuteWithoutResultsAsync();
        }

        #endregion 

        #region Notification

        public async Task AddNotification(Guid uid, Notification n)
        {
            await _client.Cypher
                .Match("(user:User)")
                .Where((User user) => user.Id == uid)
                .Create("(user)<-[:NOTIFIES]-(n:Notification {data})")
                .WithParam("data", n)
                .ExecuteWithoutResultsAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotifications(Guid uid)
        {
            var res = await _client.Cypher
                .Match("(user:User)<-[:NOTIFIES]-(n:Notification)")
                .Where((User user) => user.Id == uid)
                .Return(() => Return.As<Notification>("n"))
                .ResultsAsync;

            await ClearNotification(uid);

            return res;
        }

        public async Task ClearNotification(Guid uid)
        {
            await _client.Cypher
                .Match("(user:User)<-[:NOTIFIES]-(n:Notification)")
                .Where((User user) => user.Id == uid)
                .Delete("h, n")
                .ExecuteWithoutResultsAsync();
        }

        public void AddBirthdateNotifications(DateTimeOffset now)
        {
            _client.Cypher
                .Match("(user:User)")
                .Where((User user) => now == user.Birthdate)
                .Create("(user:User)-[HAS_NOTIFICATION]->(n:Notification {data})")
                .WithParam("data", new Notification()
                {
                    Type = NotificationType.BIRTHDATE
                })
                .ExecuteWithoutResultsAsync();
        }

		#endregion

		#region Recipe

		/// <summary>
		/// Adds a recipe to the database and setup relations with the brewer
		/// </summary>
		/// <param name="r">The new recipe to be added to the database</param>
		/// <returns></returns>
		public async Task AddRecipe(Recipe recipe)
		{
			recipe.RecipeId = Guid.NewGuid();

			await _client.Cypher
				.Match("user:User")
				.Where((User user) => user.Id == recipe.BrewerId)
				.CreateUnique("(user)-[KNOWS]->(recipe:Recipe {details})")
				.WithParam("details", recipe)
				.ExecuteWithoutResultsAsync();
		}

		public async Task DeleteRecipe(Guid rid)
		{
			await _client.Cypher
				.OptionalMatch("(r:Recipe)<-[rel]-()")
				.Where((Recipe r) => r.RecipeId == rid)
				.Delete("r, rel")
				.ExecuteWithoutResultsAsync();
		}

		public async Task UpdateRecipe(Recipe @recipe)
		{
			await _client.Cypher
				.Match("(User)-[:KNOWS]->(r:Recipe)")
				.Where((Recipe r) => r.RecipeId == @recipe.RecipeId)
				.Set("r = {newinfo}")
				.WithParam("newinfo", @recipe)
				.ExecuteWithoutResultsAsync();
		}

		public async Task<IEnumerable<Recipe>> GetAllRecipes (Guid uid)
		{
			var res = await _client.Cypher
				.Match("(user:User)-[:KNOWS]->(recipe:Recipe)")
				.Where((User user) => user.Id == uid)
				.Return(() => Return.As<Recipe>("recipe"))
				.ResultsAsync;

			return res;
		}

		public async Task<Guid> GetBrewerFromRecipe(Guid rid)
		{
			var res = await _client.Cypher
				.Match("(user:User)-[KNOWS]->(recipe:Recipe)")
				.Where((Recipe recipe) => recipe.RecipeId == rid)
				.Return(() => Return.As<UserData>("user"))
				.ResultsAsync;

			return (res as UserData).Id;
		}

		#endregion

		#region Beer

		/// <summary>
		/// Add a beer to the database
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="beer">The beer that should be added</param>
		public async Task AddBeer (Beer b)
		{
			b.Id = Guid.NewGuid();

			await _client.Cypher
				.Match ("(user:User)")
				.Where((User user) => user.Id == b.UserId)
				//creates a relation "HOSTING" between the created event 
				.Create ("(user)-[:BREWS]->(beer:Beer {data})")
				.WithParam ("data", b)
				.ExecuteWithoutResultsAsync ();
		}

        /// <summary>
        /// Updates a beer
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="beer">The beer which replaces the old event</param>
        public async Task UpdateBeer(Beer @beer, Guid uid)
        {
            await _client.Cypher
                .Match("(user:User)-[:BREWS]->(b:Beer)")
                .Where((Beer b) => b.Id == @beer.Id)
                .AndWhere((User user) => user.Id == uid)
                .Set("b = {newinfo}")
                .WithParam("newinfo", @beer)
                .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Deletes a beer
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="eid">The beer's id</param>
        public async Task DeleteBeer(Guid bid)
        {
            // Delete the Event and remaining relations
            await _client.Cypher
                .OptionalMatch("(b:Beer)<-[r]-()")
                .Where((Beer b) => b.Id == bid)
                .Delete("b, r")
                .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Gets the events that a user is hosting
        /// </summary>
        /// <returns>Task<IEnumerable<Event>> with the events the user is hosting</returns>
        /// <param name="uid">The user's id</param>
        public async Task<IEnumerable<Beer>> GetBrewedBeers(Guid uid)
        {
            var hosting = await _client.Cypher
                .Match("(user:User)-[:BREWS]->(beer:Beer)")
                .Where((User user) => user.Id == uid)
                .Return(() => Return.As<Beer>("beer"))
                .ResultsAsync;

            return hosting;
        }

		public async Task<Guid> GetBrewerFromBeer(Guid bid)
		{
			var res = await _client.Cypher
				.Match("(user:User)-[BREWS]->(beer:Beer)")
				.Where((Beer beer) => beer.Id == bid)
				.Return(() => Return.As<UserData>("user"))
				.ResultsAsync;

			return (res as UserData).Id;
		}

		public async Task<Beer> GetBeer(Guid bid)
		{
			var res = await _client.Cypher
				.Match("(beer:Beer)")
				.Where((Beer beer) => beer.Id == bid)
				.Return(() => Return.As<Beer>("beer"))
				.ResultsAsync;

			return res.First();
		}

		#endregion

		#region Review

		/// <summary>
		/// Gets the revies of a specified Beer
		/// </summary>
		/// <param name="uid">The id of the reviewed beer</param>
		/// <returns>An Enumerable with all reviews of the given beer</returns>
		public async Task<IEnumerable<Review>> GetBeerReviews(Guid bid)
		{
			var reviews = await _client.Cypher
				.Match("(review:Review)")
				.Where((Review review) => review.BeerId == bid)
				.Return(() => Return.As<Review>("review"))
				.ResultsAsync;

			return reviews;
		}

        /// <summary>
        /// Add a review to the database
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="review">The review that should be added</param>
        public async Task AddBeerReview(Review review)
		{
			await _client.Cypher
				.Match ("(b:Beer)")
				.Where((Beer b) => b.Id == review.BeerId)
				.Create ("b)-[:HAS_REVIEW]->(review:Review {data})")
				.WithParam ("data", review)
				.ExecuteWithoutResultsAsync ();
		}

		#endregion

		#region Match

		/// <summary>
		/// Matchs the user against all users that are hosting an event
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's ID</param>
		/// <param name="limit">The maximum limit of matches created</param>
		/*public async Task MatchUser(Guid uid, int limit = 5)
        {
            await CleanMatches(uid);
			*/
		/*
		 * 1: select all users (user) and users who host an event
		 * 2: filter out all users in user, except the one the uid is for
		 * 3: make sure the user isn't compared with itself (user shouldn't be in rest)
		 * 4: filter out all the events that don't have slots left
		 * 5: replace the 'uid' with the uid arg
		 * 6: add param uid
		 * 7,8,9,10,11,12: calculate weights for rest against user and send the results further along
		 * 13: order the rest by how great they are matched, descending order
		 * 14: only take the top (LIMIT) of the rest
		 * 15: create 'relationship' "MATCHED" from user to all the events that fits
		 */

		/*var now = DateTimeOffset.UtcNow.AddHours(2);

		await _client.Cypher
			.Match("(user:User), (rest:User)-[:HOSTING]->(e:Event)")
			.Where((User user) => user.Id == uid)
			.AndWhere ("NOT (user)-[]->(e)")
			.AndWhere((Event e) => e.SlotsTotal > e.SlotsTaken)
			//.AndWhere ((Event e) => e.Date > now)
			.OptionalMatch("(user)-[w1:WANTS]->(interest:Interest)<-[w2:WANTS]-(rest)")
			.With("user, rest, e, sum(w1.Weight) + sum(w2.Weight) as wt1, collect(interest.Id) as int")
			.OptionalMatch("(user)-[w3:WANTS]->(language:Language)<-[w4:WANTS]-(rest)")
			.With("user, rest, e, wt1, sum(w3.Weight) + sum(w4.Weight) as wt2, int, collect(language.Id) as lang")
			.OptionalMatch("(user)-[w5:WANTS]->(foodhabit:FoodHabit)<-[w6:WANTS]-(rest)")
			.With("user, e, wt1, wt2, sum(w5.Weight) + sum(w6.Weight) as wt3, int, lang, collect(foodhabit.Id) as food")
			.OrderBy("(wt1+wt2+wt3) DESC")
			.Limit(limit)
			.CreateUnique("(user)-[m:MATCHED { Interests:int,Languages:lang,FoodHabits:food}]->(e)")
			.ExecuteWithoutResultsAsync();

		//if (res.First().matches > 0)
		//{
		//	await AddNotification (uid, "You have new offers pending");
		//}
	}*/

		/// <summary>
		/// Cleans all matches for a given user
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">The user's id</param>
		/*async Task CleanMatches (Guid uid)
		{
			await _client.Cypher
				.Match ("(user:User)-[m:MATCHED]->(event:Event)")
				.Where((User user) => user.Id == uid)
				.Delete ("m")
				.ExecuteWithoutResultsAsync ();
		}*/

		#endregion*/

		#region Trade

		/*Proprose a new trade between two users*/
		public async Task ProposeRecipeTrade(Guid r_uid, RecipeTrade t)
		{
			t.TradeId = Guid.NewGuid();

			await _client.Cypher
				.Match("(r_user:User)")
				.Where((User r_user) => r_user.Id == r_uid)
				.Match("(s_user:User)")
				.Where((User s_user) => s_user.Id == t.SenderUser)
				.CreateUnique("(s_user)-[t:TRADE {trade}]->(r_user)")
				.WithParam("trade", t)
				.ExecuteWithoutResultsAsync();
				
		}

		/// <summary>
		/// Gets the recipe-trades a user has been offered
		/// </summary>
		/// <returns>Task<IEnumerable<RecipeTrade>> with the offers(trades)</returns>
		/// <param name="uid">The user's id</param>
		public async Task<IEnumerable<RecipeTrade>> GetPendingTrades(Guid uid)
		{
			var res = await _client.Cypher
				.Match("(s_user:User)-[t:TRADE]->(r_user:User)")
				.Where((User r_user) => r_user.Id == uid)
				.Return((t, s_user) => new
				{
					UserId = s_user.As<Guid>(),
					RecipeTrade = t.As<RecipeTrade>()
				})
				.ResultsAsync;

			// Add the senders to the RecipeTrade's field, so that receiver may see who send the offer
			var offers = new List<RecipeTrade> ();
			foreach (var pair in res) 
			{
				pair.RecipeTrade.SenderUser = pair.UserId;
				offers.Add(pair.RecipeTrade);
			}
				
			return offers;
		}

        /// <summary>
        /// Accept a trade offer
        /// </summary>
        /// <returns></returns>
        /// <param name="tid">The user's id</param>
        public async Task AcceptOffer(Guid tid)
        {
			//Find the trade that the receiver wants to accept, store in "res"
            var res = await _client.Cypher
                .Match("(s_user:User)-[t:TRADE]->(r_user:User)")
                .Where((RecipeTrade t) => t.TradeId == tid)
				.Return(() => Return.As<RecipeTrade>("t"))
                .ResultsAsync;

			//Create edge between the traded beer-recipes and the users
			// and delete the trade-edge in the process
			var rt = res as RecipeTrade;
			await _client.Cypher
				.Match("(user:User)")
				.Where((User user) => user.Id == rt.ReceiverUser)
				.Match("(recipe:Recipe)")
				.Where((Recipe recipe) => recipe.RecipeId == rt.SenderBeer)
				.CreateUnique("(user)-[KNOWS]->(recipe)")
				.ExecuteWithoutResultsAsync();
			await _client.Cypher
				.Match("(user:User)")
				.Where((User user) => user.Id == rt.SenderUser)
				.Match("(recipe:Recipe)")
				.Where((Recipe recipe) => recipe.RecipeId == rt.ReceiverBeer)
				.CreateUnique("(user)-[KNOWS]->(recipe)")
				.ExecuteWithoutResultsAsync();

			await _client.Cypher
				.Match("(User)-[t:TRADE]->(User)")
				.Where((RecipeTrade t) => t.TradeId == tid)
				.Delete("t")
				.ExecuteWithoutResultsAsync();
        }

		/// <summary>
		/// Decline the specified trade-offer
		/// </summary>
		/// <param name="tid">The trade's id</param>
		public async Task DeclineOffer(Guid tid)
		{
			await _client.Cypher
				.Match("(:User)-[t:TRADE]->(:User)")
				.Where((RecipeTrade t) => t.TradeId == tid)
				.Delete("t")
				.ExecuteWithoutResultsAsync();
		}

		public async Task<RecipeTrade> GetTrade (Guid tid)
		{
			var res = await _client.Cypher
				.Match("-[t:TRADE]->")
				.Where((RecipeTrade t) => t.TradeId == tid)
				.Return(() => Return.As<RecipeTrade>("t"))
				.ResultsAsync;

			return res.First();
		}

        #endregion

        #region Picture

        public async Task AddProfilePicture(Guid uid, byte[] picture)
		{
			var path = Path.Combine(_dataDir, _userDir, uid.ToString());
			Directory.CreateDirectory(path);

			using (var file = File.Create(Path.Combine (path, _profilePicture))) 
			{
				await file.WriteAsync(picture, 0, picture.Length);
			}
		}

		public async Task<byte[]> GetProfilePicture(Guid uid)
		{
			byte[] result;
			var path = Path.Combine(_dataDir, _userDir, uid.ToString(), _profilePicture);

			if (!File.Exists(path))
				return null;

			using (var file = File.Open(path, FileMode.Open)) 
			{
				result = new byte[file.Length];
				await file.ReadAsync(result, 0, (int)file.Length);
			}

			return result;
		}

		public async Task AddPicture(Guid uid, byte[] picture)
		{
			var path = Path.Combine(_dataDir, _userDir, uid.ToString(), _pictureDir);
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			
			using (var file = File.Create(Path.Combine(path, Guid.NewGuid().ToString()))) 
			{
				await file.WriteAsync(picture, 0, picture.Length);
			}
		}

		public async Task<IEnumerable<byte[]>> GetPictures(Guid uid)
		{
			byte[] result;
			var list = new List<byte[]>();
			var path = Path.Combine(_dataDir, _userDir, uid.ToString(), _pictureDir);

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			foreach(var filePath in Directory.GetFiles (path))
			{
				using (var file = File.Open(filePath, FileMode.Open)) 
				{
					result = new byte[file.Length];
					await file.ReadAsync(result, 0, (int)file.Length);

					list.Add (result);
				}
			}

			return list;
		}
        #endregion
    }
}