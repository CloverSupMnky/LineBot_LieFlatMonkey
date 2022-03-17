using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.Resp
{
    /// <summary>
    /// 塔羅牌占卜結果
    /// </summary>
    public class FortuneTellingResp
    {
        /// <summary>
        /// 正位或逆位牌
        /// </summary>
        public string FaceType { get; set; }

        /// <summary>
        /// 中文名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 英文名稱
        /// </summary>
        public string NameEn { get; set; }

        /// <summary>
        /// 塔羅牌意義
        /// </summary>
        public string Mean { get; set; }

        /// <summary>
        /// 牌意(正、逆位)
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 牌意關鍵字(正、逆位)
        /// </summary>
        public string MeanWord { get; set; }

        /// <summary>
        /// 圖片連結(正、逆位)
        /// </summary>
        public string ImageUrl { get; set; }
    }
}
