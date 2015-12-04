using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System;

namespace Solitude.Server 
{
    public class PictureController : SolitudeController
    {
        public async Task<IHttpActionResult> Get()
        {
            var pictures = await DB.GetPictures(UserId);

            return Ok(pictures);
        }
            
        public async Task<IHttpActionResult> Post()
        {
            var picture = await Request.Content.ReadAsByteArrayAsync();

            await DB.AddPicture(UserId, picture);

            return Ok();
        }
            
        [Route("api/picture/profile")]
        public async Task<IHttpActionResult> GetProfile()
        {
            var picture = await DB.GetProfilePicture(UserId);

            return Ok(picture);
        }

        [Route("api/picture/profile")]
        public async Task<IHttpActionResult> PostProfile()
        {
            var picture = await Request.Content.ReadAsByteArrayAsync();

            await DB.AddProfilePicture(UserId, picture);

            return Ok();
        }
    }
}

