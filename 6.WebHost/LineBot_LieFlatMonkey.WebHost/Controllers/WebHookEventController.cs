using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using LineBot_LieFlatMonkey.WebHost.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookEventController : ControllerBase
    {
        private readonly IWebHookEventService webHookEventService;

        public WebHookEventController(IWebHookEventService webHookEventService)
        {
            this.webHookEventService = webHookEventService;
        }

        [HttpPost]
        [ServiceFilter(typeof(VerifySignatureFilter))]
        public IActionResult Post(WebHookEvent webHookEvent) 
        {
            this.webHookEventService.EventHandler(webHookEvent);

            return Ok();
        }
    }
}
