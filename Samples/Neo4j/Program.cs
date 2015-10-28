using System;
using Neo4jClient;

namespace Neo4j
{
	class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	class MainClass
	{
		private static GraphClient client;

		public static void Main (string[] args)
		{
			client = new GraphClient(new Uri("http://prozum.dk:7474/db/data"),"neo4j","password");
			client.Connect ();

			var newUser = new User { Id = 1, Name = "Test Testesen" };

			client.Cypher
				.Create("(user:User {newUser})")
				.WithParam("newUser", newUser)
				.ExecuteWithoutResults();

			var users = client.Cypher
				.Match ("(user:User)")
				.Where ((User user) => user.Id == 1)
				.Return (user => user.As<User>())
				.Results;

			foreach (var user in users) {
				Console.WriteLine (user.Name);
			}
		}
	}
}
