using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// 處理 Line Bot 傳來訊息介面
    /// </summary>
    public interface IWebHookEventService
    {
        /// <summary>
        /// 處理 Line Bot 訊息
        /// </summary>
        /// <param name="webHookEvent">Line Bot 訊息物件</param>
        Task EventHandler(WebHookEvent webHookEvent);
    }
}
