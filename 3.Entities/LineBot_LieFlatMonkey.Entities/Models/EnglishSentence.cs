using System;
using System.Collections.Generic;
using URF.Core.EF.Trackable;

namespace LineBot_LieFlatMonkey.Entities.Models
{
    /// <summary>
    /// &#33521;&#25991;&#30701;&#21477;&#36039;&#26009;&#34920;
    /// </summary>
    public partial class EnglishSentence : Entity
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int SeqNo { get; set; }
        /// <summary>
        /// 英文句子
        /// </summary>
        public string Sentence { get; set; }
        /// <summary>
        /// 英文翻譯
        /// </summary>
        public string Translation { get; set; }
        /// <summary>
        /// 句子出處
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 句子出處類型
        /// </summary>
        public string SourceType { get; set; }
    }
}
