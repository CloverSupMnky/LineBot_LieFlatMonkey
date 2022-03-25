using LineBot_LieFlatMonkey.Assets.Model.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// 音樂推薦介面
    /// </summary>
    public interface IMusicRecommandService
    {
        /// <summary>
        /// 依曲風推薦音樂
        /// </summary>
        /// <param name="musicCateType">曲風</param>
        /// <returns></returns>
        Task<List<MusicRecommandResp>> RecommandByMusicCateType(string musicCateType);
    }
}
