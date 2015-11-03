using System;
using System.Web.Http;
using Newtonsoft.Json;
using DAL;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace Solitude.Server
{
    [RoutePrefix("api/match")]
    public class MatchController : ApiController
    {
        [Authorize]
        public List<Event> Get(int limit)
        {
            return DAL.DAL.MatchUser(User.Identity.GetUserId(), limit);
        }
    }
}