using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// LUIS 意圖判斷介面
    /// </summary>
    public interface ILUISService
    {
        /// <summary>
        /// 取得意圖判斷結果
        /// </summary>
        /// <param name="text">需要意圖判斷的文字</param>
        /// <returns></returns>
        Task<string> GetIntent(string text);
    }
}
