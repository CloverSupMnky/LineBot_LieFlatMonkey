using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.Resp
{
    /// <summary>
    /// 音樂推薦資訊
    /// </summary>
    public class MusicRecommandResp
    {
        /// <summary>
        /// 曲風類型
        /// </summary>
        public string SongType { get; set; }

        /// <summary>
        /// 歌名
        /// </summary>
        public string Song { get; set; }

        /// <summary>
        /// 演唱者
        /// </summary>
        public string Artist { get; set; }

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
