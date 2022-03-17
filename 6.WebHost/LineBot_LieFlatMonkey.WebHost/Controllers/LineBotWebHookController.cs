using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.WebHost.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotWebHookController : ControllerBase
    {
        [HttpPost]
        [ServiceFilter(typeof(VerifySignatureFilter))]
        public IActionResult Post(WebHookEvent webHookEvent) 
        {
            return Ok();
        }
    }
}
