using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace LineBot_LieFlatMonkey.Entities.Models
{
    /// <summary>
    /// &#22612;&#32645;&#29260;&#36039;&#26009;&#34920;
    /// </summary>
    public partial class TarotCard : Entity
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int SeqNo { get; set; }
        /// <summary>
        /// 中文名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 英文名稱
        /// </summary>
        public string NameEn { get; set; }
        /// <summary>
        /// 意義
        /// </summary>
        public string Mean { get; set; }
        /// <summary>
        /// 正位牌意
        /// </summary>
        public string DescUp { get; set; }
        /// <summary>
        /// 逆位牌意
        /// </summary>
        public string DescRev { get; set; }
        /// <summary>
        /// 正位牌意關鍵字
        /// </summary>
        public string MeanUp { get; set; }
        /// <summary>
        /// 逆位牌意關鍵字
        /// </summary>
        public string MeanRev { get; set; }
        /// <summary>
        /// 正位牌圖片連結
        /// </summary>
        public string ImageUrlUp { get; set; }
        /// <summary>
        /// 逆位牌圖片連結
        /// </summary>
        public string ImageUrlRev { get; set; }
        /// <summary>
        /// 每日運勢牌意
        /// </summary>
        public string DescDaily { get; set; }
    }
}
