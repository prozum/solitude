using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Neo4jClient;
using System.Net.Http;
using Neo4j.AspNet.Identity;
using Dal;

namespace Solitude.Server
{
    public class SolitudeController : ApiController 
    {
        public SolitudeController()
        {
        }

        public DatabaseAbstrationLayer DB
        {
            get
            {
                return Request.GetOwinContext().Get<DatabaseAbstrationLayer>();
            }
        }

        public IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}

