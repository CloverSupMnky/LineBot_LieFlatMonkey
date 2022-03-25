using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Constant
{
    /// <summary>
    /// 音頻檔檔名類別
    /// </summary>
    public class EnglishSenteceFileNameType
    {
        /// <summary>
        /// 正常取得音頻檔檔名
        /// </summary>
        public const string Normal = "englishsentence.wav";

        /// <summary>
        /// 無法取得使用者對應音頻檔檔名
        /// </summary>
        public const string NotFound = "filenotfound.wav";

        /// <summary>
        /// 轉檔結果暫存檔名
        /// </summary>
        public const string TempM4A = "temp.m4a";

        /// <summary>
        /// 正常取得音頻檔檔名 M4A 檔案
        /// </summary>
        public const string NormalM4A = "englishsentence.m4a";

        /// <summary>
        /// 無法取得使用者對應音頻檔檔名 M4A 檔案
        /// </summary>
        public const string NotFoundM4A = "filenotfound.m4a";
    }
}
