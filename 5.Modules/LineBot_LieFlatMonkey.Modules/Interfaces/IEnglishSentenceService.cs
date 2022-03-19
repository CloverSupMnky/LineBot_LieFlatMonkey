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
    }
}
