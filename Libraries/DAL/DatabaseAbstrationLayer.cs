using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Dal
{
	public class DatabaseAbstrationLayer : IDisposable
	{
		private readonly GraphClient client;

		public DatabaseAbstrationLayer(GraphClient client)
		{
			this.client = client;
		}

		public void Dispose()
		{
		}

		/// <summary>
		/// function to add an interest to the database
		/// the input is limited to an enum so it's not
		/// possible to add something out of the supported range of interests
		/// </summary>
		/// <returns><c>true</c>, if interest was added, <c>false</c> otherwise.</returns>
		/// <param name="ic">Ic.</param>
		public bool AddInterest (Interest.InterestCode ic)
		{
			string interest = Interest.GetInterest (ic);

			if (interest == null) {
				return false;
			} else {
				client.Cypher
					.Create ("(interest:Interest {info})")
					.WithParam ("info", new {Id = ic, Description = interest})
					.ExecuteWithoutResultsAsync ();
			}

			return true;
		}

		/// <summary>
		/// function to add a language to the database
		/// the input is limited to an enum so it's not
		/// possible to add something out of the supported range of languages
		/// </summary>
		/// <returns><c>true</c>, if language was added, <c>false</c> otherwise.</returns>
		/// <param name="lc">Lc.</param>
		public async Task<bool> AddLanguage (Language.LanguageCode lc)
		{
			string language = Language.GetLanguage (lc);

			if (language == null) {
				return false;
			} else {
				await client.Cypher
				.Create ("(language:Language {info})")
				.WithParam ("info", new {Id = lc, Description = language})
				.ExecuteWithoutResultsAsync ();
			}

			return true;
		}

		/// <summary>
		/// function to add a foodhabit to the database
		/// the input is limited to an enum so it's not
		/// possible to add something out of the supported range of foodhabits
		/// </summary>
		/// <returns><c>true</c>, if food habit was added, <c>false</c> otherwise.</returns>
		/// <param name="fc">Fc.</param>
		public async Task<bool> AddFoodHabit (FoodHabit.FoodHabitCode fc)
		{
			string foodhabit = FoodHabit.GetFoodHabit (fc);

			if (foodhabit == null) {
				return false;
			} else {
				await client.Cypher
					.Create ("(foodhabit:FoodHabit {info})")
					.WithParam ("info", new {Id = fc, Description = foodhabit})
					.ExecuteWithoutResultsAsync ();
			}

			return true;
		}

		public async Task SetEventIdCounter (int startVal)
		{
			await client.Cypher
				.Match ("(eid:ServerInfo)")
				.CreateUnique ("eid.value = {val}")
				.WithParam ("val", startVal)
				.ExecuteWithoutResultsAsync ();
		}

		async Task<int> GetEventIdCounter ()
		{
			var res = await client.Cypher
				.Match ("(eid:ServerInfo)")
				.Return ((eid) => new {
					eid = Return.As<int>("eid.value")
				})
				.ResultsAsync;

			return res.First ().eid;
		}

		async Task IncrementEventIdCounter ()
		{
			await client.Cypher
				.Match ("(eid:ServerInfo)")
				.Set ("eid.value = eid.value + 1")
				.ExecuteWithoutResultsAsync ();
		}

		public async Task AddEvent (string uid, string date, string description, int slotsTotal, string address)
		{
			var res = await GetEventIdCounter ();
			await IncrementEventIdCounter ();

			await client.Cypher
				.Match ("(user:User)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				//creates a relation "HOSTING" between the created event 
				.Create ("user-[:HOSTING]->(event:Event {info})")
				.WithParam ("info", new Event (date, address, description, slotsTotal, slotsTotal, uid, res + 1))
				.ExecuteWithoutResultsAsync ();
		}

		public async Task AddReview(string uid, string text, int rating)
		{
			await client.Cypher
				.Match ("(user:User)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.Create ("user-[:REVIEWER]->(review:Review {data})")
				.WithParam ("data", new Review() { ReviewText = text, Rating = rating })
				.ExecuteWithoutResultsAsync ();
		}

			
		public async Task ConnectUserInterest (string uid, Interest.InterestCode ic, int w)
		{
			await client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (interest:Interest)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.AndWhere ("interest.Id = {ic}")
				.WithParam ("ic", ic)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		public async Task ConnectUserLanguage (string uid, Language.LanguageCode lc, int w)
		{
			await client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (language:Language)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.AndWhere ("interest.Id == {lc}")
				.WithParam ("lc", lc)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		public async Task ConnectUserLanguage (string uid, FoodHabit.FoodHabitCode fc, int w)
		{
			await client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (foodhabit:FoodHabit)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.AndWhere ("interest.Id == {fc}")
				.WithParam ("fc", fc)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		async Task CleanMatches (string uid)
		{
			await client.Cypher
				.Match ("(user:User)-[m:MATCHED]->(event:Event)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.Delete ("m")
				.ExecuteWithoutResultsAsync ();
		}

		public async Task MatchUser (string uid, int LIMIT = 5)
		{
			await CleanMatches (uid);

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

			await client.Cypher
				.Match ("(user:User), (rest:User)-[:HOSTING]->(event:Event)")
				.Where ("user.Id = {uid}")
				.AndWhere ("rest.Id <> {uid}")
				.AndWhere ("event.SlotsLeft > 0")
				.WithParam ("uid", uid)
				.Match ("user-[w1:WANTS]->(interest:Interest)<-[w2:WANTS]-rest")
				.With ("user, rest, event, sum(w1.weight) + sum(w2.weight) as wt1")
				.Match ("user-[w3:WANTS]->(language:Language)<-[w4:WANTS]-rest")
				.With ("user, rest, event, wt1, sum(w3.weight) + sum(w4.weight) as wt2")
				.Match ("user-[w5:WANTS]->(foodhabit:FoodHabit)<-[w6:WANTS]-rest")
				.With ("user, event, wt1, wt2, sum(w5.weight) + sum(w6.weight) as wt3")
				.OrderBy ("(wt1+wt2+wt3) DESC")
				.Limit (LIMIT)
				.Create ("user-[:MATCHED]->event")
				.ExecuteWithoutResultsAsync();
		}

		public async Task<IEnumerable<Event>> GetOffers (string uid)
		{
			var res = await client.Cypher
				.Match ("(user:User)-[:MATCHED]->(event:Event)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.Return (() => new {
					offers = Return.As<IEnumerable<Event>> ("collect(event)")
				})
				.ResultsAsync;

			return res.First().offers;
		}

		public async Task<IEnumerable<Event>> GetEvents (string uid, bool ATTENDING = true, int LIMIT = 10)
		{
			if (ATTENDING)
			{
				var res = await client.Cypher
					.Match ("(user:User)-[:ATTENDING]->(event:Event)")
					.Where ("user.Id = {uid}")
					.WithParam ("uid", uid)
					.Return (() => new {
						attending = Return.As<IEnumerable<Event>> ("collect(event)")
					})
					.Union ()
					.Match ("user-[:HOSTING]->(event:Event)")
					.Where ("user.Id = {uid}")
					.WithParam ("uid", uid)
					.Return (() => new {
						events = Return.As<IEnumerable<Event>> ("collect(event)")
					})
					.Limit (LIMIT)
					.ResultsAsync;

				return res.First().events;
			}
			else
			{
				var res = await client.Cypher
					.Match ("(user:User)-[:HOSTING]->(event:Event)")
					.Where ("user.Id = {uid}")
					.WithParam ("uid", uid)
					.Return (() => new {
						hosting = Return.As<IEnumerable<Event>> ("collect(event)")
					})
					.Limit (LIMIT)
					.ResultsAsync;

				return res.First().hosting;
			}
		}

		public async Task UpdateEvent (string uid, Event e)
		{
			await client.Cypher
				.Match ("user-[:HOSTING]->(event:Event {info})")
				.Where ("user.Id = {uid} AND event.Id = eid AND event.uid = uid")
				.WithParam ("uid", uid)
				.WithParam ("eid", e.eid)
				.Set ("info = {newinfo}")
				.WithParam ("newinfo", e)
				.ExecuteWithoutResultsAsync();
		}

		public async void DeleteEvent (string uid, int eid)
		{
			await client.Cypher
				.Match ("(user:User)-[:HOSTING]->(event:Event)<-[r]-(rest:User)")
				.WithParam ("user.Id", uid)
				.Delete ("r, event")
				.ExecuteWithoutResultsAsync();
		}

		public async Task CancelRegistration (string uid, Event e)
		{
			await client.Cypher
				.Match ("(user:User)-[a:ATTENDS]->(event:Event)")
				.Where ("user.Id = {uid} AND event.uid = {euid} AND event.eid = {eid}")
				.WithParam ("uid", uid)
				.WithParam ("euid", e.uid)
				.WithParam ("eid", e.eid)
				.Delete ("a")
				.ExecuteWithoutResultsAsync();

			var newEvent = e;
			newEvent.SlotsLeft++;

			await UpdateEvent (uid, newEvent);
		}
			
		public async Task<bool> ReplyOffer (string uid, bool answer, Event e)
		{
			if (answer)
			{
				var res = await client.Cypher
					.Match ("(user:User), (event:Event)")
					.Where ("user.Id = {uid} AND event.eid = {eid} AND event.uid = {euid}")
					.AndWhere ("event.SlotsLeft > 0")
					.WithParam ("uid", uid)
					.WithParam ("eid", e.eid)
					.WithParam ("euid", e.uid)
					.Set ("event.SlotsLeft = event.SlotsLeft - 1")
					.Create ("user-[:ATTENDS]->event")
					.Delete ("user-[:MATCHED]->event")
					.Return (() => new {
						events = Return.As<int> ("count(event)")
					})
					.ResultsAsync;

				return res.First().events > 0;
			}
			else
			{
				await client.Cypher
					.Match ("(user:User), (event:Event)")
					.Where ("user.Id = {uid} AND event.eid = {eid} AND event.uid = {euid}")
					.WithParam ("uid", uid)
					.WithParam ("eid", e.eid)
					.WithParam ("euid", e.uid)
					.Delete ("user-[:MATCHED]->event")
					.ExecuteWithoutResultsAsync();

				return true;
			}
		}

		public async Task AddNotification (string uid, string msg)
		{
			await client.Cypher
				.Match ("(user:User)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.Create ("user-[:HAS]->(notification:Notification {msg})")
				.WithParam ("msg", msg)
				.ExecuteWithoutResultsAsync ();
		}

		public async Task ClearNotification (string uid)
		{
			await client.Cypher
				.Match ("(user:User)-[h:HAS]-(notification:Notification)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.Delete ("h, notification")
				.ExecuteWithoutResultsAsync ();
		}

		public async Task<IEnumerable<string>> GetNotification (string uid)
		{
			var res = await client.Cypher
				.Match ("(user:User)-[:HAS]-(notification:Notification)")
				.Where ("user.Id = {uid}")
				.WithParam ("uid", uid)
				.Return ((notifications) => new {
					notifications = Return.As<IEnumerable<string>> ("collect(notification)")
				})
				.ResultsAsync;

			await ClearNotification (uid);

			return res.First ().notifications;
		}
	}
}