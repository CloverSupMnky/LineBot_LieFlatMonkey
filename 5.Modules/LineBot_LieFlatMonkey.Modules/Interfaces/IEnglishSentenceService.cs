using LineBot_LieFlatMonkey.Assets.Model.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// 英文句子產出介面
    /// </summary>
    public interface IEnglishSentenceService
    {
        /// <summary>
        /// 取得英文句子
        /// </summary>
        /// <returns></returns>
        EnglishSentenceResp GetSentence();

        /// <summary>
        /// 依使用者 Id 取得英文句子英頻檔
        /// </summary>
        /// <param name="replyToken">使用者 replyToken</param>
        /// <returns></returns>
        byte[] GetAudioByReplyToken(string replyToken);

        /// <summary>
        /// 取得無法正常取得使用者音頻檔提示音檔
        /// </summary>
        /// <returns></returns>
        byte[] GetNotFoundAudio();
    }
}
