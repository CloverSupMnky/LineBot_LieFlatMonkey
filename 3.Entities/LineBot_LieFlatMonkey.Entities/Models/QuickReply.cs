using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace LineBot_LieFlatMonkey.Entities.Models
{
    /// <summary>
    /// QuickReply &#20351;&#29992;&#38917;&#30446;
    /// </summary>
    public partial class QuickReply : Entity
    {
        /// <summary>
        /// 項目類別
        /// </summary>
        public string ItemType { get; set; }
        /// <summary>
        /// 項目值
        /// </summary>
        public string ItemValue { get; set; }
        public string ImageUrl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
