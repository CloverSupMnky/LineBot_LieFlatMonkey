using LineBot_LieFlatMonkey.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// 共用方法介面
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// 取得亂數值
        /// </summary>
        /// <param name="maxLength">範圍最大值</param>
        /// <returns></returns>
        int GetRandomNo(int maxLength);

        /// <summary>
        /// 依模板檔案名稱取得對應模板 Json 字串
        /// </summary>
        /// <returns></returns>
        Task<string> GetMessageTemplateByName(string name);

        /// <summary>
        /// 依類型取得 QuickReply 資料
        /// </summary>
        /// <param name="type">QuickReply 類型</param>
        /// <returns></returns>
        List<QuickReply> GetQuickReplyByType(string type);
    }
}
