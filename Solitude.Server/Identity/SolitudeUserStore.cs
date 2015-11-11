using Neo4j.AspNet.Identity;
using Neo4jClient;
using System.Threading.Tasks;
using Dal;

namespace Solitude.Server
{
    public class SolitudeUserStore : Neo4jUserStore<ApplicationUser>
    {
        private readonly DatabaseAbstrationLayer _dal;

        public SolitudeUserStore(IGraphClient graphClient, DatabaseAbstrationLayer DAL) : base(graphClient)
        {
            _dal = DAL;
        }

        public async Task DeleteAsyncFixed(ApplicationUser user)
        {
            Throw.ArgumentException.IfNull(user, "user");

            await _dal.DeleteUserData(user.Id);

            await _dal.DeleteUser(user.Id);
        }
    }
}