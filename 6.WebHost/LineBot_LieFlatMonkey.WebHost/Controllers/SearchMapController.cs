using LineBot_LieFlatMonkey.Assets.Model.Req;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchMapController : APIBaseController
    {
        private readonly ISearchMapService searchMapService;

        public SearchMapController(ISearchMapService searchMapService)
        {
            this.searchMapService = searchMapService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SearchMap(SearchMapReq req) 
        {
            var res = 
                await this.searchMapService.SearchMap(req.SearchWord, req.Latitude, req.Longitude);

            return Success(res);
        }
    }
}
