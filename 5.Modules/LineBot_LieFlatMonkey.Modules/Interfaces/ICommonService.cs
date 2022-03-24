using LineBot_LieFlatMonkey.Assets.Model.LineBot;
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

        /// <summary>
        /// 回傳錯誤訊息-Reply-聊天室
        /// </summary>
        /// <param name="text">回傳訊息</param>
        /// <returns></returns>
        Task ReplyErrorMessage(string text, string replyToken);

        /// <summary>
        /// 回傳錯誤訊息-Push 自己
        /// </summary>
        /// <param name="text">回傳訊息</param>
        /// <returns></returns>
        Task PushErrorMessage(string text);

        /// <summary>
        /// 取得回傳模板
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        List<ResultMessage> GetResultMessage(string text);
    }
}
