using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.Resp
{
    /// <summary>
    /// 地圖查詢結果
    /// </summary>
    public class MapInfoResp
    {
        /// <summary>
        /// 圖片連結
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 地圖連結
        /// </summary>
        public string MapUrl { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 商家名稱
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// Google 評分
        /// </summary>
        public string Rating { get; set; }
    }
}
