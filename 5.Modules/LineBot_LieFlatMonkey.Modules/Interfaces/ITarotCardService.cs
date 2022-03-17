using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// 塔羅牌相關功能介面
    /// </summary>
    public interface ITarotCardService
    {
        /// <summary>
        /// 依占卜類別進行占卜
        /// </summary>
        /// <param name="fortuneTellingType">塔羅牌占卜方式</param>
        /// <returns>FortuneTellingResp 塔羅牌占卜結果</returns>
        FortuneTellingResp FortuneTellingByType(string fortuneTellingType);
    }
}
