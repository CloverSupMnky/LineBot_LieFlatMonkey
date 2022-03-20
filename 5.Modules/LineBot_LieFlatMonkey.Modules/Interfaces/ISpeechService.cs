using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// 智能語音使用介面
    /// </summary>
    public interface ISpeechService
    {
        /// <summary>
        /// 產生對應文字音檔並儲存對應使用者檔案路徑
        /// </summary>
        /// <param name="text">輸入文字</param>
        /// <param name="userId">使用者 Id</param>
        /// <returns></returns>
        Task<bool> GenAudioAndSave(string text,string userId);

        /// <summary>
        /// 取得音頻檔長度
        /// </summary>
        /// <param name="fileName">檔案路徑</param>
        /// <returns></returns>
        int GetAudioLength(string path);
    }
}
