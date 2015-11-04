using System;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace Solitude.Server
{
    [RoutePrefix("api/match")]
    public class MatchController : ApiController
    {
        [Authorize]
        public IHttpActionResult Get()
        {
            return Ok(DAL.DAL.GetOffers(User.Identity.GetUserId()));
        }
    }
}