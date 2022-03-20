using LineBot_LieFlatMonkey.Assets.Model.Req;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeechController : APIBaseController
    {
        private readonly ISpeechService speechService;

        public SpeechController(ISpeechService speechService)
        {
            this.speechService = speechService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GenAudioAndSave(SpeechReq req) 
        {
           var res = await this.speechService.GenAudioAndSave(req.Text,req.UserId);

            return Success(res);
        }
    }
}
