using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace LineBot_LieFlatMonkey.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnglishSentenceController : APIBaseController
    {
        private readonly IEnglishSentenceService englishSentenceService;

        public EnglishSentenceController(IEnglishSentenceService englishSentenceService)
        {
            this.englishSentenceService = englishSentenceService;
        }

        [HttpPost("[action]")]
        public IActionResult GetSentence() 
        {
            var res = this.englishSentenceService.GetSentence();

            return Success(res);
        }

        [HttpGet("[action]/{userId}")]
        public IActionResult GetAudio(string userId) 
        {
            return Ok();
        }
    }
}
