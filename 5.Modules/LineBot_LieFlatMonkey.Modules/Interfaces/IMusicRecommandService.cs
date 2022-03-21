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
        /// 推薦音樂
        /// </summary>
        Task<MusicRecommandResp> Recommand();
    }
}
