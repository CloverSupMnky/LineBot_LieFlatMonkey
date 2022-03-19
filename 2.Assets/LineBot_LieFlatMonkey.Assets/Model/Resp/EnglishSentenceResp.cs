using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.Resp
{
    /// <summary>
    /// 英文句子回傳結果
    /// </summary>
    public class EnglishSentenceResp
    {
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
