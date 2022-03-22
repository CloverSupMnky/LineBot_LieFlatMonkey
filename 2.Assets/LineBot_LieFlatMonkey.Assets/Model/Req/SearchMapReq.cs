using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.Req
{
    /// <summary>
    /// 探索地圖 Request
    /// </summary>
    public class SearchMapReq
    {
        /// <summary>
        /// 查詢文字
        /// </summary>
        public string SearchWord { get; set; }

        /// <summary>
        /// 緯度
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// 經度
        /// </summary>
        public string Longitude { get; set; }
    }
}
