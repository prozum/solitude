﻿using Microsoft.AspNet.Identity;
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
            if (u.Weight == 0)
                u.Weight = 1;

            switch (u.Info)
            {
                case InfoType.LANGUAGE:
                    await DB.ConnectUserLanguage(new Guid(User.Identity.GetUserId()), u.Value, u.Weight);
                    break;
                case InfoType.INTEREST:
                    await DB.ConnectUserInterest(new Guid(User.Identity.GetUserId()), u.Value, u.Weight);
                    break;
                case InfoType.FOODHABIT:
                    await DB.ConnectUserFoodHabit(new Guid(User.Identity.GetUserId()), u.Value, u.Weight);
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
                    await DB.DisconnectUserLanguage(new Guid(User.Identity.GetUserId()), u.Value);
                    break;
                case InfoType.INTEREST: 
                    await DB.DisconnectUserInterest(new Guid(User.Identity.GetUserId()), u.Value);
                    break;
                case InfoType.FOODHABIT:
                    await DB.DisconnectUserFoodHabit(new Guid(User.Identity.GetUserId()), u.Value);
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
                    return Ok(await DB.GetUserLanguage(new Guid(User.Identity.GetUserId())));
                case InfoType.INTEREST:
                    return Ok(await DB.GetUserInterest(new Guid(User.Identity.GetUserId())));
                case InfoType.FOODHABIT:
                    return Ok(await DB.GetUserFoodHabit(new Guid(User.Identity.GetUserId())));
                default:
                    return BadRequest("Invalid information type.");
            }
        }
    }
}

