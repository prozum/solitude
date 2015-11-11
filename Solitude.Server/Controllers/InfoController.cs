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
        public async Task<IHttpActionResult> Post(InfoUpdate u)
        {
            switch (u.Info)
            {
                case InfoType.LANGUAGE:
                    await DB.ConnectUserLanguage(User.Identity.GetUserId(), u.Value, 1);
                    break;
                case InfoType.INTEREST:
                    await DB.ConnectUserInterest(User.Identity.GetUserId(), u.Value, 1);
                    break;
                case InfoType.FOODHABIT:
                    await DB.ConnectUserFoodHabit(User.Identity.GetUserId(), u.Value, 1);
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
                    await DB.DisconnectUserLanguage(User.Identity.GetUserId(), u.Value);
                    break;
                case InfoType.INTEREST: 
                    await DB.DisconnectUserInterest(User.Identity.GetUserId(), u.Value);
                    break;
                case InfoType.FOODHABIT:
                    await DB.DisconnectUserFoodHabit(User.Identity.GetUserId(), u.Value);
                    break;
                default:
                    return BadRequest("Invalid information type.");
            }

            return Ok();
        }
            
        public async Task<IHttpActionResult> Get(InfoType id)
        {
            switch (id)
            {
                case InfoType.LANGUAGE:
                    return Ok(await DB.GetUserLanguage(User.Identity.GetUserId()));
                case InfoType.INTEREST:
                    return Ok(await DB.GetUserInterest(User.Identity.GetUserId()));
                case InfoType.FOODHABIT:
                    return Ok(await DB.GetUserFoodHabit(User.Identity.GetUserId()));
                default:
                    return BadRequest("Invalid information type.");
            }
        }
    }
}

