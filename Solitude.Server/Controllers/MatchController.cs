using System;
using System.Web.Http;
using Newtonsoft.Json;
using DAL;
using System.Collections.Generic;

namespace Solitude.Server
{
    [RoutePrefix("api/match")]
    public class MatchController : ApiController
    {
        int uid;
        public MatchController(int uid)
        {
            this.uid = uid;
        }
        public List<Event> Get(int limit)
        {
            return DAL.DAL.MatchUser(uid, limit);
        }
    }
}