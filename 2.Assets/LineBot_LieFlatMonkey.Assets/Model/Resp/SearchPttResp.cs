using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.Resp
{
    /// <summary>
    /// Ptt 查詢結果
    /// </summary>
    public class SearchPttResp
    {
        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 推文數 1-99、爆
        /// </summary>
        public string ThumbsUp { get; set; }

        /// <summary>
        /// 文章連結
        /// </summary>
        public string PttLink { get; set; }
    }
}
