using Neo4j.AspNet.Identity;
using Neo4jClient;
using System.Threading.Tasks;

namespace Solitude.Server
{
    public class SolitudeUserStore : Neo4jUserStore<ApplicationUser>
    {
        private readonly IGraphClient _graphClient;

        public SolitudeUserStore(IGraphClient graphClient) : base(graphClient)
        {
            _graphClient = graphClient;
        }

        public async Task DeleteAsyncFixed(ApplicationUser user)
        {
            Throw.ArgumentException.IfNull(user, "user");

            await _graphClient.Cypher
                .Match("(u:User)-[r]-() DETACH")
                .Where((ApplicationUser u) => u.Id == user.Id)
                .Delete("r, u")
                .ExecuteWithoutResultsAsync();
        }
    }
}

