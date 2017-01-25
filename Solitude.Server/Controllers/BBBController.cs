using System.Web.Http;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using Dal;
using System;

namespace Solitude.Server
{
    public abstract class BBBController : ApiController 
    {
        public DatabaseAbstrationLayer DB
        {
            get
            {
                return Request.GetOwinContext().Get<DatabaseAbstrationLayer>();
            }
        }

        public Guid UserId
        {
            get
            {
                return new Guid(User.Identity.GetUserId());
            }
        }


        internal IHttpActionResult ErrorResult(IdentityResult result)
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

