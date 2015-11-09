﻿using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Dal;

namespace Solitude.Server
{
    [RoutePrefix("api/offer")]
    public class OfferController : ApiController
    {
        public DatabaseAbstrationLayer DB
        {
            get
            {
                return Request.GetOwinContext().Get<DatabaseAbstrationLayer>();
            }
        }

        [Authorize]
        public async Task<IHttpActionResult> Get()
        {
            var offers = await DB.GetOffers(User.Identity.GetUserId());
            return Ok(offers);
        }
    }
}