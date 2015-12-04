using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient;
using Neo4jClient.Cypher;
using Model;
using System.IO;

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
            // Delete Events hosted by User
            var hosts = await _client.Cypher
                .Match("(user:User)-[:HOSTING]-(event:Event)")
                .Where((User user) => user.Id == uid)
                .Return((@event) => @event.As<Event>().Id)
                .ResultsAsync;
            foreach (var host in hosts)
                await DeleteEvent(host);

            // Cancel all attending Event
            var events = await _client.Cypher
                .Match("(user:User)-[:ATTENDS]-(event:Event)")
                .Where((User user) => user.Id == uid)
                .Return((@event) => @event.As<Event>().Id)
                .ResultsAsync;
            foreach (var eid in events)
                await CancelEventRegistration(uid, eid);

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
                .Create("user<-[:NOTIFIES]-(n:Notification {data})")
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

        #region Event

        /// <summary>
        /// Add an event to the database
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="event">The event that should be added</param>
        public async Task AddEvent (Event e)
		{
			e.Id = Guid.NewGuid();

			await _client.Cypher
				.Match ("(user:User)")
				.Where((User user) => user.Id == e.UserId)
				//creates a relation "HOSTING" between the created event 
				.Create ("user-[:HOSTING]->(event:Event {data})")
				.WithParam ("data", e)
				.ExecuteWithoutResultsAsync ();
		}

        /// <summary>
        /// Updates an event
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="event">The event which replaces the old event</param>
        public async Task UpdateEvent(Event @event, Guid uid)
        {
            await _client.Cypher
                .Match("(user:User)-[:HOSTING]->(e:Event)")
                .Where((Event e) => e.Id == @event.Id)
                .AndWhere((User user) => user.Id == uid)
                .Set("e = {newinfo}")
                .WithParam("newinfo", @event)
                .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Deletes an event
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="eid">The event's id</param>
        public async Task DeleteEvent(Guid eid)
        {
            // Delete Event Tasks
            //await DeleteEventTasks(eid);

            // Delete the Event and remaining relations
            await _client.Cypher
                .OptionalMatch("(e:Event)<-[r]-()")
                .Where((Event e) => e.Id == eid)
                .Delete("e, r")
                .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Gets the events that a user is hosting
        /// </summary>
        /// <returns>Task<IEnumerable<Event>> with the events the user is hosting</returns>
        /// <param name="uid">The user's id</param>
        public async Task<IEnumerable<Event>> GetHostingEvents(Guid uid)
        {
            var hosting = await _client.Cypher
                .Match("(user:User)-[:HOSTING]->(event:Event)")
                .Where((User user) => user.Id == uid)
                .Return(() => Return.As<Event>("event"))
                .ResultsAsync;

            return hosting;
        }

        /// <summary>
        /// Gets the events that a user is attending
        /// </summary>
        /// <returns>Task<IEnumerable<Event>> with the events the user is attending</returns>
        /// <param name="uid">The user's id</param>
        public async Task<IEnumerable<Event>> GetAttendingEvents(Guid uid)
        {
            var events = await _client.Cypher
                .Match("(user:User)-[:ATTENDS]->(event:Event)")
                .Where((User user) => user.Id == uid)
                .Return(() => Return.As<Event>("event"))
                .ResultsAsync;

            return events;
        }


        public void DeleteHeldEvents(DateTimeOffset now)
        {
            _client.Cypher
                .Match("(e:Event)<-[r:ATTENDS]-(user:User)")
                .Where((Event e) => now > e.Date)
                .Create("(user:User)<-[NOTIFIES]-(n:Notification {Type:{type}, EventId:e.Id})")
                .WithParam("type", NotificationType.REVIEW)
                .Delete("r")
                .ExecuteWithoutResultsAsync();


            _client.Cypher
                .OptionalMatch("(e:Event)-[r]-()")
                .Where((Event e) => now > e.Date)
                .Delete("r, e")
                .ExecuteWithoutResultsAsync();
        }


        /// <summary>
        /// Cancels the registration for a user to an event
        /// </summary>
        /// <param name="uid">The user's id</param>
        /// <param name="eid">The event's id</param>
        public async Task CancelEventRegistration(Guid uid, Guid eid)
        {
            await _client.Cypher
                .Match("(user:User)-[a:ATTENDS]->(e:Event)")
                .Where((User user) => user.Id == uid)
                .AndWhere((Event e) => e.Id == eid)
                .Delete("a")
                .ExecuteWithoutResultsAsync();

            await ReleaseSlot(eid);

            //await AddNotification (@event.UserID, "A person has cancelled his/her registration for your event.");
        }

        #endregion

        #region Review

		public async Task<IEnumerable<Review>> GetReviews(Guid uid)
		{
			var reviews = await _client.Cypher
				.Match("(review:Review)")
				.Where((Review review) => review.UserId == uid)
				.Return(() => Return.As<Review>("review"))
				.ResultsAsync;

			return reviews;
		}

        /// <summary>
        /// Add a review to the database
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="review">The review that should be added</param>
        public async Task AddReview(Review review)
		{
			await _client.Cypher
				.Match ("(user:User) (e:Event)")
				.Where((User user) => user.Id == review.UserId)
				.AndWhere((Event e) => e.Id == review.EventId)
				.Create ("user-[:GAVE_REVIEW]->(review:Review {data})<-[:HAS_REVIEW]-(e:Event)")
				.WithParam ("data", review)
				.ExecuteWithoutResultsAsync ();
		}

        #endregion

        #region Add InfoTypes

        /// <summary>
        /// Adds an interest to the database
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="i">The interest to add</param>
        public async Task AddInterest(Interest i)
        {
            await _client.Cypher
                .Merge("(i:Interest { Id:{id}, Name:{name}})")
                .WithParams(new
                {
                    id = i.Id,
                    name = i.Name
                })
                .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Adds a language to the database
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="lang">The language to add</param>
        public async Task AddLanguage(Language lang)
        {
            await _client.Cypher
                .Merge("(lang:Language { Id:{id}, Name:{name}})")
                .WithParams(new
                {
                    id = lang.Id,
                    name = lang.Name
                })
                .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Adds a foodhabit to the database
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="fh">The foodhabit to add</param>
        public async Task AddFoodHabit(FoodHabit fh)
        {
            await _client.Cypher
                .Merge("(fb:FoodHabit { Id:{id}, Name:{name}})")
                .WithParams(new
                {
                    id = fh.Id,
                    name = fh.Name
                })
                .ExecuteWithoutResultsAsync();
        }

        #endregion

        #region Connect InfoTypes

        /// <summary>
        /// Connects the user to an interest
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="uid">The user's id</param>
        /// <param name="ic">The interest which the user should be connected to</param>
        /// <param name="w">The weight of the relationship between the user and interest</param>
        public async Task ConnectUserInterest (Guid uid, int ic, int weight)
		{
			await _client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User), (interest:Interest)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("interest.Id = {ic}")
				.WithParam ("ic", ic)
				//create a unique relation "WANTS" with the weight 'w'
				.CreateUnique ("user-[w:WANTS]->interest")
				.Set("w.Weight = {weight}")
				.WithParam("weight", weight)
				.ExecuteWithoutResultsAsync ();
		}

        /// <summary>
        /// Connects the user to a language
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="uid">The user's id</param>
        /// <param name="lc">The language which the user should be connected to</param>
        /// <param name="w">The weight of the relationship between the user and language</param>
        public async Task ConnectUserLanguage(Guid uid, int lc, int weight)
        {
            await _client.Cypher
                //make sure that the interest is related with the right user
                .Match("(user:User), (language:Language)")
                .Where((User user) => user.Id == uid)
                .AndWhere("language.Id = {lc}")
                .WithParam("lc", lc)
                //create a unique relation "WANTS" with the weight 'w'
                .CreateUnique(("user-[w:WANTS]->language"))
				.Set("w.Weight = {weight}")
				.WithParam("weight", weight)
                .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Connects the user to a foodhabit
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="uid">The user's id</param>
        /// <param name="fh">The foodhabit which the user should be connected to</param>
        /// <param name="w">The weight of the relationship between the user and foodhabit</param>
        public async Task ConnectUserFoodHabit(Guid uid, int fh, int weight)
        {
            await _client.Cypher
                //make sure that the interest is related with the right user
                .Match("(user:User), (foodhabit:FoodHabit)")
                .Where((User user) => user.Id == uid)
                .AndWhere("foodhabit.Id = {fh}")
                .WithParam("fh", fh)
                //create a unique relation "WANTS" with the weight 'w'
                .CreateUnique(("user-[w:WANTS]->foodhabit"))
				.Set("w.Weight = {weight}")
				.WithParam("weight", weight)
                .ExecuteWithoutResultsAsync();
        }

        #endregion

        #region Disconnect InfoTypes

        /// <summary>
        /// Disconnects the user from an interest
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="uid">The user's id</param>
        /// <param name="ic">The interest that the user should be disconnected from</param>
        public async Task DisconnectUserInterest (Guid uid, int ic)
		{
			await _client.Cypher
				//make sure that the interest is related with the right user
				.Match ("(user:User)-[w:WANTS]->(interest:Interest)")
				.Where((User user) => user.Id == uid)
				.AndWhere ("interest.Id = {ic}")
				.WithParam ("ic", ic)
				.Delete("w")
				.ExecuteWithoutResultsAsync ();
		}

        /// <summary>
        /// Disconnects the user from a language
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="uid">The user's id</param>
        /// <param name="lc">The language that the user should be disconnected from</param>
        public async Task DisconnectUserLanguage(Guid uid, int lc)
        {
            await _client.Cypher
                //make sure that the interest is related with the right user
                .Match("(user:User)-[w:WANTS]->(language:Language)")
                .Where((User user) => user.Id == uid)
                .AndWhere("language.Id = {lc}")
                .WithParam("lc", lc)
                .Delete("w")
                .ExecuteWithoutResultsAsync();
        }

        /// <summary>
        /// Disconnects the user from a foodhabit
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="uid">The user's id</param>
        /// <param name="fh">The foodhabit that the user should be disconnected from</param>
        public async Task DisconnectUserFoodHabit(Guid uid, int fh)
        {
            await _client.Cypher
                //make sure that the interest is related with the right user
                .Match("(user:User)-[w:WANTS]->(foodhabit:FoodHabit)")
                .Where((User user) => user.Id == uid)
                .AndWhere("foodhabit.Id = {fh}")
                .WithParam("fh", fh)
                .Delete("w")
                .ExecuteWithoutResultsAsync();
        }

        #endregion

        #region Get InfoTypes

        /// <summary>
        /// Gets the user's interests
        /// </summary>
        /// <returns>Task<IEnumerable<int>> with the interest values the user has </returns>
        /// <param name="uid">The user's id</param>
        public async Task<IEnumerable<int>> GetUserInterest (Guid uid)
		{
			var res = await _client.Cypher
				.Match ("(user:User)-[:WANTS]->(interest:Interest)")
				.Where ((User user) => user.Id == uid)
				.Return (() => Return.As<int> ("interest.Id"))
				.ResultsAsync;

			return res;
		}

        /// <summary>
        /// Gets the user's languages
        /// </summary>
        /// <returns>Task<IEnumerable<int>> with the language values the user has </returns>
        /// <param name="uid">The user's id</param>
        public async Task<IEnumerable<int>> GetUserLanguage(Guid uid)
        {
            var res = await _client.Cypher
                .Match("(user:User)-[:WANTS]->(language:Language)")
                .Where((User user) => user.Id == uid)
                .Return(() => Return.As<int>("language.Id"))
                .ResultsAsync;

            return res;
        }

        /// <summary>
        /// Gets the user's foodhabits
        /// </summary>
        /// <returns>Task<IEnumerable<int>> with the foodhabit values the user has </returns>
        /// <param name="uid">The user's id</param>
        public async Task<IEnumerable<int>> GetUserFoodHabit(Guid uid)
        {
            var res = await _client.Cypher
                .Match("(user:User)-[:WANTS]->(foodhabit:FoodHabit)")
                .Where((User user) => user.Id == uid)
                .Return(() => Return.As<int>("foodhabit.Id"))
                .ResultsAsync;

            return res;
        }

        #endregion

        #region Match

        /// <summary>
        /// Matchs the user against all users that are hosting an event
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="uid">The user's ID</param>
        /// <param name="limit">The maximum limit of matches created</param>
        public async Task MatchUser(Guid uid, int limit = 5)
        {
            await CleanMatches(uid);

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

            var now = DateTimeOffset.UtcNow.AddHours(2);

            await _client.Cypher
                .Match("(user:User), (rest:User)-[:HOSTING]->(e:Event)")
                .Where((User user) => user.Id == uid)
                .AndWhere ("NOT user-[]->e")
                .AndWhere((Event e) => e.SlotsTotal > e.SlotsTaken)
                //.AndWhere ((Event e) => e.Date > now)
                .OptionalMatch("user-[w1:WANTS]->(interest:Interest)<-[w2:WANTS]-rest")
                .With("user, rest, e, sum(w1.Weight) + sum(w2.Weight) as wt1, collect(interest.Id) as int")
                .OptionalMatch("user-[w3:WANTS]->(language:Language)<-[w4:WANTS]-rest")
                .With("user, rest, e, wt1, sum(w3.Weight) + sum(w4.Weight) as wt2, int, collect(language.Id) as lang")
                .OptionalMatch("user-[w5:WANTS]->(foodhabit:FoodHabit)<-[w6:WANTS]-rest")
                .With("user, e, wt1, wt2, sum(w5.Weight) + sum(w6.Weight) as wt3, int, lang, collect(foodhabit.Id) as food")
                .OrderBy("(wt1+wt2+wt3) DESC")
                .Limit(limit)
                .CreateUnique("user-[m:MATCHED { Interests:int,Languages:lang,FoodHabits:food}]->e")
                .ExecuteWithoutResultsAsync();

			//if (res.First().matches > 0)
			//{
			//	await AddNotification (uid, "You have new offers pending");
			//}
        }

        /// <summary>
        /// Cleans all matches for a given user
        /// </summary>
        /// <returns>Task</returns>
        /// <param name="uid">The user's id</param>
        async Task CleanMatches (Guid uid)
		{
			await _client.Cypher
				.Match ("(user:User)-[m:MATCHED]->(event:Event)")
				.Where((User user) => user.Id == uid)
				.Delete ("m")
				.ExecuteWithoutResultsAsync ();
		}

        #endregion

        #region Offer

        /// <summary>
        /// Gets the events a user has been offered
        /// </summary>
        /// <returns>Task<IEnumerable<Event>> with the offers(events)</returns>
        /// <param name="uid">The user's id</param>
        public async Task<IEnumerable<Offer>> GetOffers(Guid uid)
		{
			var res = await _client.Cypher
				.Match ("(user:User)-[m:MATCHED]->(e:Event)")
				.Where((User user) => user.Id == uid)
				.Return((e, m) => new
					{
						Offer = e.As<Offer>(),
						Match = m.As<Match>()
					})
				.ResultsAsync;

			// Combine Match and Offer
			var offers = new List<Offer> ();
			foreach (var pair in res) 
			{
				pair.Offer.Match = pair.Match;
				offers.Add(pair.Offer);
			}
				
			return offers;
		}

        /// <summary>
        /// Reply to an offer
        /// </summary>
        /// <returns>Returns a Task<bool> for whether it succeeds or not</returns>
        /// <param name="uid">The user's id</param>
        /// <param name="answer">true if the user wants to attend the event and false if vice versa</param>
        /// <param name="eid">The event's id</param>
        public async Task<bool> ReplyOffer(Guid uid, Guid eid, bool answer)
        {
            if (answer)
            {
                var freeSlots = await TakeSlot(eid);

                if (!freeSlots)
                    return false;

                await _client.Cypher
                    .Match("(user:User)-[m:MATCHED]->(e:Event)")
                    .Where((User user) => user.Id == uid)
                    .AndWhere((Event e) => e.Id == eid)
                    .Delete("m")
                    .CreateUnique("user-[:ATTENDS]->e")
                    .ExecuteWithoutResultsAsync();

                return true;
            }
            else
            {
                await _client.Cypher
                    .Match("(user:User)-[m:MATCHED]->(e:Event)")
                    .Where((User user) => user.Id == uid)
                    .AndWhere((Event e) => e.Id == eid)
                    .Delete("m")
                    .ExecuteWithoutResultsAsync();

                return true;
            }
        }

        #endregion

        #region Slot

        /// <summary>
        /// Take a slot in an event
        /// </summary>
        /// <returns>Task<bool> whether or not it's possible to take the slot </returns>
        /// <param name="eid">The event's id</param>
        public async Task<bool> TakeSlot(Guid eid)
		{
			var res = await _client.Cypher
				.Match("(e:Event)")
				.Where((Event e) => e.Id == eid)
				.AndWhere ((Event e) => e.SlotsTotal > e.SlotsTaken)
				.Set ("e.SlotsTaken = e.SlotsTaken + 1")
				.Return ((e) => e.As<Event> ())
				.ResultsAsync;

			return res.Any();
		}

		/// <summary>
		/// Release a slot in an event
		/// </summary>
		/// <returns>Task</returns>
		/// <param name="eid">The event's id</param>
		public async Task ReleaseSlot(Guid eid)
		{
			await _client.Cypher
				.Match("(e:Event)")
				.Where((Event e) => e.Id == eid)
				.AndWhere((Event e) => e.SlotsTaken > 0)
				.Set("e.SlotsTaken = e.SlotsTaken - 1")
				.ExecuteWithoutResultsAsync();
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