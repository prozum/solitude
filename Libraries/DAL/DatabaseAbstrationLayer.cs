using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;
using Model;

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
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// function to add an interest to the database
		/// the input is limited to an enum so it's not
		/// possible to add something out of the supported range of interests
		/// </summary>
		/// <returns><c>true</c>, if interest was added, <c>false</c> otherwise.</returns>
		/// <param name="ic">Ic.</param>
		public async Task AddInterest (Interest ic)
		{
				await client.Cypher
					.Create ("(interest:Interest {info})")
					.WithParam ("info", ic)
					.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// function to add a language to the database
		/// the input is limited to an enum so it's not
		/// possible to add something out of the supported range of languages
		/// </summary>
		/// <returns><c>true</c>, if language was added, <c>false</c> otherwise.</returns>
		/// <param name="lc">Lc.</param>
		public async Task AddLanguage (Language lc)
		{
			await client.Cypher
				.Create ("(language:Language {info})")
				.WithParam ("info", lc)
				.ExecuteWithoutResultsAsync ();
		}

		/// <summary>
		/// function to add a foodhabit to the database
		/// the input is limited to an enum so it's not
		/// possible to add something out of the supported range of foodhabits
		/// </summary>
		/// <returns><c>true</c>, if food habit was added, <c>false</c> otherwise.</returns>
		/// <param name="fh">Fc.</param>
		public async Task AddFoodHabit (FoodHabit fh)
		{
			await client.Cypher
				.Create ("(foodhabit:FoodHabit {info})")
				.WithParam ("info", fh)
				.ExecuteWithoutResultsAsync ();
		}

		public async Task SetEventIdCounter (int startVal)
		{
			await client.Cypher
				.Merge("(sinfo:ServerInfo)")
				.OnCreate()
				.Set("sinfo.eid = {val}")
				.WithParam("val", startVal)
				.ExecuteWithoutResultsAsync();
		}

		async Task<int> GetEventIdCounter ()
		{
			var res = await client.Cypher
				.Match ("(sinfo:ServerInfo)")
				.Return (() => Return.As<int>("sinfo.eid"))
				.ResultsAsync;

			return res.First ();
		}

		async Task IncrementEventIdCounter ()
		{
			await client.Cypher
				.Match ("(sinfo:ServerInfo)")
				.Set ("sinfo.eid = sinfo.eid + 1")
				.ExecuteWithoutResultsAsync ();
		}

		public async Task AddEvent (Event @event)
		{
			@event.Id = await GetEventIdCounter ();
			await IncrementEventIdCounter ();

			await client.Cypher
				.Match ("(user:User)")
				.Where((User user) => user.Id == @event.UserId)
				//creates a relation "HOSTING" between the created event 
				.Create ("user-[:HOSTING]->(event:Event {info})")
				.WithParam ("info", @event)
				.ExecuteWithoutResultsAsync ();
		}

		public async Task AddReview(Review review)
		{
			await client.Cypher
				.Match ("(user:User)")
				.Where((User user) => user.Id == review.UserID)
				.Create ("user-[:GAVE_REVIEW]->(review:Review {data})")
				.WithParam ("data", review)
				.ExecuteWithoutResultsAsync ();
		}

			
		public async Task ConnectUserInterest (string uid, Interest ic, int w)
		{
			await client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (interest:Interest)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("interest.Id = {ic}")
				.WithParam ("ic", ic)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		public async Task DisconnectUserInterest (string uid, Interest ic)
		{
			await client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User)-[w:WANTS]->(interest:Interest)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("interest.Id = {ic}")
				.WithParam ("ic", ic)
				.Delete("w")
				.ExecuteWithoutResultsAsync ();
		}

		public async Task ConnectUserLanguage (string uid, Language lc, int w)
		{
			await client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (language:Language)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("interest.Id == {lc}")
				.WithParam ("lc", lc)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		public async Task DisconnectUserLanguage (string uid, Language lc)
		{
			await client.Cypher
			//make sure that the interest is related with the right user
				.Match ("(user:User)-[w:WANTS]->(language:Language)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("language.Id = {lc}")
				.WithParam ("lc", lc)
				.Delete("w")
				.ExecuteWithoutResultsAsync ();
		}

		public async Task ConnectUserFoodHabit (string uid, FoodHabit fh, int w)
		{
			await client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (foodhabit:FoodHabit)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("interest.Id == {fh}")
				.WithParam ("fh", fh)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique (("user-[:WANTS {weight}]->interest"))
				.WithParam ("weight", new {weight = w})
				.ExecuteWithoutResultsAsync ();
		}

		public async Task DisconnectUserInterest (string uid, FoodHabit fh)
		{
			await client.Cypher
			//make sure that the interest is related with the right user
				.Match ("(user:User)-[w:WANTS]->(foodhabit:FoodHabit)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("foodhabit.Id = {fh}")
				.WithParam ("fh", fh)
				.Delete("w")
				.ExecuteWithoutResultsAsync ();
		}

		async Task CleanMatches (string uid)
		{
			await client.Cypher
				.Match ("(user:User)-[m:MATCHED]->(event:Event)")
				.Where((User user) => user.Id == uid)
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

			var res = await client.Cypher
				.Match ("(user:User), (rest:User)-[:HOSTING]->(event:Event)")
				.Where ("user.Id = {uid}")
				.AndWhere ("rest.Id <> {uid}")
				.AndWhere ("event.SlotsTotal > event.SlotsTaken")
				.WithParam ("uid", uid)
				.Match ("user-[w1:WANTS]->(interest:Interest)<-[w2:WANTS]-rest")
				.With ("user, rest, event, sum(w1.weight) + sum(w2.weight) as wt1")
				.Match ("user-[w3:WANTS]->(language:Language)<-[w4:WANTS]-rest")
				.With ("user, rest, event, wt1, sum(w3.weight) + sum(w4.weight) as wt2")
				.Match ("user-[w5:WANTS]->(foodhabit:FoodHabit)<-[w6:WANTS]-rest")
				.With ("user, event, wt1, wt2, sum(w5.weight) + sum(w6.weight) as wt3")
				.OrderBy ("(wt1+wt2+wt3) DESC")
				.Limit (LIMIT)
				.Create ("user-[m:MATCHED]->event")
				.Return ((matches) => Return.As<int> ("count(m)")).ResultsAsync;

			/*
			if (res.First().matches > 0)
			{
				await AddNotification (uid, "You have new offers pending");
			}
			*/
		}

		public async Task<IEnumerable<Event>> GetOffers (string uid)
		{
			var res = await client.Cypher
				.Match ("(user:User)-[:MATCHED]->(event:Event)")
				.Where((User user) => user.Id == uid)
				.Return (() => Return.As<IEnumerable<Event>> ("collect(event)"))
				.ResultsAsync;

			return res.First();
		}

		public async Task<IEnumerable<Event>> GetEvents (string uid, bool ATTENDING = true, int LIMIT = 10)
		{
			if (ATTENDING)
			{
				var res = await client.Cypher
					.Match ("(user:User)-[:ATTENDING]->(event:Event)")
					.Where((User user) => user.Id == uid)
					.Return (() =>  Return.As<IEnumerable<Event>> ("collect(event)"))
					.Union ()
					.Match ("user-[:HOSTING]->(event:Event)")
					.Where((User user) => user.Id == uid)
					.Return (() => Return.As<IEnumerable<Event>> ("collect(event)"))
					.Limit (LIMIT)
					.ResultsAsync;

				return res.First();
			}
			else
			{
				var res = await client.Cypher
					.Match ("(user:User)-[:HOSTING]->(event:Event)")
					.Where((User user) => user.Id == uid)
					.Return (() => Return.As<IEnumerable<Event>> ("collect(event)"))
					.Limit (LIMIT)
					.ResultsAsync;

				return res.First();
			}
		}

		public async Task UpdateEvent (Event @event)
		{
			await client.Cypher
				.Match ("user-[:HOSTING]->(event:Event {info})")
				.Where ("event.Id = {eid}")
				.WithParam ("eid", @event.Id)
				.Set ("info = {newinfo}")
				.WithParam ("newinfo", @event)
				.ExecuteWithoutResultsAsync();
		}

		public async Task DeleteEvent (int eid)
		{
			await client.Cypher
				.Match ("(user:User)-[:HOSTING]->(event:Event)<-[r]-(rest:User)")
				.Where ("event.eid = {eid}")
				.WithParam ("event.eid", eid)
				.Delete ("r, event")
				.ExecuteWithoutResultsAsync();
		}

		public async Task<bool> TakeSlot(int eid)
		{
			var res = await client.Cypher
				.Match ("(e:Event)")
				.Where ((Event e) => e.Id == eid)
				.AndWhere ((Event e) => e.SlotsTaken > e.SlotsTotal)
				.Set ("event.SlotsTaken = event.SlotsTaken + 1")
				.Return ((@event) => @event.As<Event> ())
				.ResultsAsync;

			return res.Count() > 0;
		}

		public async Task ReleaseSlot(int eid)
		{
			await client.Cypher
				.Match("(e:Event)")
				.Where((Event e) => e.Id == eid)
				.AndWhere((Event e) => e.SlotsTaken > 0)
				.Set("event.SlotsTaken = event.SlotsTaken - 1")
				.ExecuteWithoutResultsAsync();
		}

		public async Task CancelRegistration (string uid, int eid)
		{
			await client.Cypher
				.Match ("(user:User)-[a:ATTENDS]->(e:Event)")
				.Where((User user) => user.Id == uid)
				.AndWhere((Event e) => e.Id == eid)
				.Delete ("a")
				.ExecuteWithoutResultsAsync();

			await ReleaseSlot(eid);

			//await AddNotification (@event.UserID, "A person has cancelled his/her registration for your event.");
		}
			
		public async Task<bool> ReplyOffer (string uid, bool answer, int eid)
		{
			if (answer)
			{
				var freeSlots = await TakeSlot(eid);

				if (!freeSlots)
					return false;

				await client.Cypher
					.Match ("(user:User), (e:Event)")
					.Where ((User user) => user.Id == uid)
					.AndWhere ((Event e) => e.Id == eid)
					.Create ("user-[:ATTENDS]->e")
					.Delete ("user-[:MATCHED]->e")
					.ExecuteWithoutResultsAsync();

				return true;
			}
			else
			{
				await client.Cypher
					.Match ("(user:User), (e:Event)")
					.Where((User user) => user.Id == uid)
					.Where((Event e) => e.Id == eid)
					.Delete ("user-[:MATCHED]->e")
					.ExecuteWithoutResultsAsync();

				return true;
			}
		}

		public async Task AddNotification (string uid, string msg)
		{
			await client.Cypher
				.Match ("(user:User)")
				.Where((User user) => user.Id == uid)
				.Create ("user-[:HAS]->(notification:Notification {msg})")
				.WithParam ("msg", msg)
				.ExecuteWithoutResultsAsync ();
		}

		public async Task ClearNotification (string uid)
		{
			await client.Cypher
				.Match ("(user:User)-[h:HAS]-(notification:Notification)")
				.Where((User user) => user.Id == uid)
				.Delete ("h, notification")
				.ExecuteWithoutResultsAsync ();
		}

		public async Task<IEnumerable<string>> GetNotification (string uid)
		{
			var res = await client.Cypher
				.Match ("(user:User)-[:HAS]-(notification:Notification)")
				.Where((User user) => user.Id == uid)
				.Return ((notifications) => Return.As<IEnumerable<string>> ("collect(notification)"))
				.ResultsAsync;

			await ClearNotification (uid);

			return res.First();
		}

		/// <summary>
		/// Deletes the user's data, used as a help function for user deletion
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="uid">Uid.</param>
		public async Task DeleteUserData(string uid)
		{
			var hostIds = await client.Cypher
				.Match("(user:User)-[:HOSTS]-(event:Event)")
				.Where((User user) => user.Id == uid)
				.Return((@event) => @event.As<Event>().Id)
				.ResultsAsync;

			foreach (var id in hostIds)
			{
				await DeleteEvent(id);
			}

			var attendingIds = await client.Cypher
				.Match("(user:User)-[:ATTENDS]-(event:Event)")
				.Where((User user) => user.Id == uid)
				.Return((@event) => @event.As<Event>().Id)
				.ResultsAsync;

			foreach (var id in attendingIds)
			{
				await CancelRegistration (uid, id);
			}

			await client.Cypher
				.OptionalMatch ("(user:User)-[r]->()")
				.Where((User user) => user.Id == uid)
				.Delete ("r")
				.ExecuteWithoutResultsAsync ();
		}

		public async Task DeleteUser(string uid)
		{
			await client.Cypher
				.Match("(user:User)")
				.Where((User user) => user.Id == uid)
				.Delete("user")
				.ExecuteWithoutResultsAsync();
		}
	}
}