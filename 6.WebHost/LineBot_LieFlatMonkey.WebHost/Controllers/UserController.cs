using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : APIBaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetUserProfileByUserId(string userId) 
        {
            var res = await this.userService.GetUserProfileByUserId(userId);

            return Success(res);
        }
    }
}
