using LineBot_LieFlatMonkey.Assets.Constant;
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

        [HttpGet("[action]/{replyToken}")]
        public IActionResult GetAudioByReplyToken(string replyToken) 
        {
            var res = this.englishSentenceService.GetAudioByReplyToken(replyToken);

            return File(res, "audio/m4a", EnglishSenteceFileNameType.Normal);
        }

        [HttpGet("[action]")]
        public IActionResult GetNotFoundAudio()
        {
            var res = this.englishSentenceService.GetNotFoundAudio();

            return File(res, "audio/m4a", EnglishSenteceFileNameType.NotFound);
        }
    }
}
