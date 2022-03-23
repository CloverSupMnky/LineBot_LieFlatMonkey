using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchPttController : APIBaseController
    {
        private readonly ISearchPttService searchPttService;

        public SearchPttController(ISearchPttService searchPttService)
        {
            this.searchPttService = searchPttService;
        }

        [HttpGet("[action]/{boardtype}")]
        public async Task<IActionResult> SearchPtt(string boardtype) 
        {
            await this.searchPttService.SearchPttByBoardType(boardtype);

            return Ok();
        }
    }
}
