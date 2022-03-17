using LineBot_LieFlatMonkey.Assets.Model.Resp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIBaseController : ControllerBase
    {
        private protected virtual IActionResult Success<T>(T content)
        {
            var resp = new BaseResp<T>()
            {
                Data = content
            };

            return Ok(resp);
        }
    }
}
