using System.Web.Http;
using System.Threading.Tasks;

namespace Solitude.Server
{
    [RoutePrefix("api/review")]
    public class ReviewController : ApiController
    {
        [Route("add")]
        public async Task<IHttpActionResult> Add(ReviewModel model)
        {
            return Ok(model);
        }
    }
}

