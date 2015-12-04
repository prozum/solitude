using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Neo4j.AspNet.Identity;
using Neo4jClient;
using Model;
using System;

namespace Solitude.Server
{
    public class InfoController : SolitudeController
    {
        public async Task<IHttpActionResult> Get(InfoType id)
        {
            switch (id)
            {
                case InfoType.LANGUAGE:
                    return Ok(await DB.GetUserLanguage(UserId));
                case InfoType.INTEREST:
                    return Ok(await DB.GetUserInterest(UserId));
                case InfoType.FOODHABIT:
                    return Ok(await DB.GetUserFoodHabit(UserId));
                default:
                    return BadRequest("Invalid information type.");
            }
        }

        public async Task<IHttpActionResult> Post(InfoUpdate u)
        {
            switch (u.Info)
            {
                case InfoType.LANGUAGE:
                    await DB.ConnectUserLanguage(UserId, u.Value, u.Weight);
                    break;
                case InfoType.INTEREST:
                    await DB.ConnectUserInterest(UserId, u.Value, u.Weight);
                    break;
                case InfoType.FOODHABIT:
                    await DB.ConnectUserFoodHabit(UserId, u.Value, u.Weight);
                    break;
                default:
                    return BadRequest("Invalid information type.");
            }

            return Ok();
        }
            
        public async Task<IHttpActionResult> Delete(InfoUpdate u)
        {
            switch (u.Info)
            {
                case InfoType.LANGUAGE:
                    await DB.DisconnectUserLanguage(UserId, u.Value);
                    break;
                case InfoType.INTEREST: 
                    await DB.DisconnectUserInterest(UserId, u.Value);
                    break;
                case InfoType.FOODHABIT:
                    await DB.DisconnectUserFoodHabit(UserId, u.Value);
                    break;
                default:
                    return BadRequest("Invalid information type.");
            }

            return Ok();
        }
    }
}

