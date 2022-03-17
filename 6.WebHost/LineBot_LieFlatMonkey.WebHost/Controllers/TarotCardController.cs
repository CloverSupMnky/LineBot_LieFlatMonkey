using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    /// <summary>
    /// 塔羅牌 API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TarotCardController : APIBaseController
    {
        private readonly ITarotCardService tarotCardService;

        public TarotCardController(ITarotCardService tarotCardService)
        {
            this.tarotCardService = tarotCardService;
        }
    }
}
