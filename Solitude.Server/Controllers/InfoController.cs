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
        public InfoController() : base() {}

        [Authorize]
        public async Task<IHttpActionResult> Post(InfoType t, int val)
        {
            switch (t)
            {
                case InfoType.LANGUAGE:
                    if (Enum.IsDefined(typeof(Model.Language), val))
                    {
                        await DB.ConnectUserLanguage(User.Identity.GetUserId(), (Model.Language)val, 1);
                    }
                    else
                    {
                         return BadRequest("Invalid information.");
                    }
                    break;
                case InfoType.INTEREST:
                    if (Enum.IsDefined(typeof(Model.Interest), val))
                    {
                        await DB.ConnectUserInterest(User.Identity.GetUserId(), (Model.Interest)val, 1);
                    }
                    else
                    {
                        return BadRequest("Invalid information.");
                    }
                    break;
                case InfoType.FOODHABIT:
                    if (Enum.IsDefined(typeof(Model.FoodHabit), val))
                    {
                        await DB.ConnectUserFoodHabit(User.Identity.GetUserId(), (Model.FoodHabit)val, 1);
                    }
                    else
                    {
                        return BadRequest("Invalid information.");
                    }
                    break;
                default:
                    return BadRequest("Invalid information type.");
            }

            return Ok();
        }
            
        [Authorize]
        public async Task<IHttpActionResult> Delete(InfoType t, int val)
        {
            switch (t)
            {
                case InfoType.LANGUAGE:
                    if (Enum.IsDefined(typeof(Model.Language), val))
                    {
                        await DB.DisconnectUserLanguage(User.Identity.GetUserId(), (Model.Language)val);
                    }
                    else
                    {
                        return BadRequest("Invalid information.");
                    }
                    break;
                case InfoType.INTEREST:
                    if (Enum.IsDefined(typeof(Model.Interest), val))
                    {
                        await DB.DisconnectUserInterest(User.Identity.GetUserId(), (Model.Interest)val);
                    }
                    else
                    {
                        return BadRequest("Invalid information.");
                    }
                    break;
                case InfoType.FOODHABIT:
                    if (Enum.IsDefined(typeof(Model.FoodHabit), val))
                    {
                        await DB.DisconnectUserFoodHabit(User.Identity.GetUserId(), (Model.FoodHabit)val);
                    }
                    else
                    {
                        return BadRequest("Invalid information.");
                    }
                    break;
                default:
                    return BadRequest("Invalid information type.");
            }

            return Ok();
        }

        public async Task<IHttpActionResult> Get(InfoType t)
        {
            switch (t)
            {
                case InfoType.INTEREST:
                    return Ok(await DB.GetUserLanguage(User.Identity.GetUserId()));
                case InfoType.LANGUAGE:
                    return Ok(await DB.GetUserInterest(User.Identity.GetUserId()));
                case InfoType.FOODHABIT:
                    return Ok(await DB.GetUserFoodHabit(User.Identity.GetUserId()));
                default:
                    return BadRequest("Invalid information type.");
            }
        }
    }
}

