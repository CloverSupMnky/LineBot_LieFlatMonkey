using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.AppSetting
{
    /// <summary>
    /// 智能語音相關設定
    /// </summary>
    public class SpeechSetting
    {
        /// <summary>
        /// 訂用帳戶金鑰
        /// </summary>
        public string Subkey { get; set; }

        /// <summary>
        /// 訂用帳戶服務的所在區域
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 要辨識的語言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 服務端點
        /// </summary>
        public string Endpoint { get; set; }
    }
}
