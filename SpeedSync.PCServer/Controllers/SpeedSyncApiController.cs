using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace SpeedSync.PCServer.Controllers
{
    [ApiController]
    [Route("SpeedSync/[controller]/[action]")]
    public class SpeedSyncApiController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Auth(string Password)
        {

        }
    }
}