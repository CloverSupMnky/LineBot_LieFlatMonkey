using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LUISController : APIBaseController
    {
        private readonly ILUISService luisService;

        public LUISController(ILUISService luisService)
        {
            this.luisService = luisService;
        }

        [HttpGet("[action]/{text}")]
        public async Task<IActionResult> GetIntent(string text) 
        {
            var res = await luisService.GetIntent(text);

            return Success(res);
        }
    }
}
