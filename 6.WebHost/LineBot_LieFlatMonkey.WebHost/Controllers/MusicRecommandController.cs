using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicRecommandController : APIBaseController
    {
        private readonly IMusicRecommandService musicRecommandService;

        public MusicRecommandController(IMusicRecommandService musicRecommandService)
        {
            this.musicRecommandService = musicRecommandService;
        }

        [HttpPost("[action]")]
        public IActionResult GetRecommand() 
        {
            var res = this.musicRecommandService.Recommand();

            return Success(res);
        }
    }
}
