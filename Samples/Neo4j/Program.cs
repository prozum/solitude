using System;
using Neo4jClient;

namespace Neo4j
{
	class MainClass
	{
		private static GraphClient _graphClient;

		class User
		{
			public int Id;
			public string Name;
		}

		public static void Main (string[] args)
		{
			_graphClient = new GraphClient(new Uri("http://prozum.dk:7474/db/data"),"neo4j","password");
			_graphClient.Connect ();

			var newUser = new User { Id = 1, Name = "Test Testesen" };
			_graphClient.Cypher
				.Create("(user:User {newUser})")
				.WithParam("newUser", newUser)
				.ExecuteWithoutResults();

			var res =_graphClient.Cypher
				.Match ("(user:User)")
				.Where ((User user) => user.Id == 1)
				.Return (user => user.Labels ())
				.Results;

			Console.Write (res);
		}
	}
}
