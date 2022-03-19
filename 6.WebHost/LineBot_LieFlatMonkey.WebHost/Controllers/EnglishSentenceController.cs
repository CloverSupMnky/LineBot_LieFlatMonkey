using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnglishSentenceController : ControllerBase
    {
        [HttpPost("[action]")]
        public IActionResult GetSentence() 
        {
            return Ok();
        }
    }
}
