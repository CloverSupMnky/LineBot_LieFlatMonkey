using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// HttpClient 方法介面
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// 發送 Line Bot Push Message
        /// </summary>
        /// <param name="messages">發送訊息</param>
        /// <returns></returns>
        Task PushMessageAsync(List<ResultMessage> messages);

        /// <summary>
        /// 發送 Line Bot Reply Message
        /// </summary>
        /// <param name="messages">發送訊息</param>
        /// <param name="replyToken">回覆訊息的 replyToken </param>
        /// <returns></returns>
        Task ReplyMessageAsync(List<ResultMessage> messages,string replyToken);
    }
}
