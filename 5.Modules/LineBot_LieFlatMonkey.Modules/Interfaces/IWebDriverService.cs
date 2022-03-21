using LineBot_LieFlatMonkey.Assets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// WebDriver 相關介面
    /// </summary>
    public interface IWebDriverService
    {
        /// <summary>
        /// 依曲風類型取得音樂推薦列表
        /// </summary>
        /// <param name="musicCate">曲風類型</param>
        /// <returns></returns>
        List<MusicRecommandMusicInfo> GetMusicListByMusicCateType(string musicCate);

        /// <summary>
        /// 依音樂資訊取得影片資訊
        /// </summary>
        /// <param name="info">音樂資訊</param>
        MusicRecommandVideoInfo GetVideoInfoByMusicInfo(MusicRecommandMusicInfo info);
    }
}
