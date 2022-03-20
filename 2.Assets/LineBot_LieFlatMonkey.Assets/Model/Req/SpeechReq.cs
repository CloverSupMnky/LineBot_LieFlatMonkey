using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.Req
{
    /// <summary>
    /// 智能語音 Request
    /// </summary>
    public class SpeechReq
    {
        /// <summary>
        /// 要轉換音檔的文字
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 使用者 Id
        /// </summary>
        public string UserId { get; set; }
    }
}
