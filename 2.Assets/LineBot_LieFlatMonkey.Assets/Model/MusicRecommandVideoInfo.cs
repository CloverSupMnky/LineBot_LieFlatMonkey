using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model
{
    /// <summary>
    /// 音樂推薦的 YT 影片資訊
    /// </summary>
    public class MusicRecommandVideoInfo
    {
        /// <summary>
        /// 影片路由
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// 影片瀏覽圖片
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
