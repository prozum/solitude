using Neo4j.AspNet.Identity;
using Neo4jClient;
using System.Threading.Tasks;
using Dal;
using System;

namespace Solitude.Server
{
    public class SolitudeUserStore : Neo4jUserStore<SolitudeUser>
    {
        private readonly DatabaseAbstrationLayer _dal;

        public SolitudeUserStore(IGraphClient graphClient, DatabaseAbstrationLayer dal) : base(graphClient)
        {
            _dal = dal;
        }

        public async Task DeleteAsyncFixed(SolitudeUser user)
        {
            Throw.ArgumentException.IfNull(user, "user");

            await _dal.DeleteUserData(new Guid(user.Id));

            await _dal.DeleteUser(new Guid(user.Id));
        }
    }
}