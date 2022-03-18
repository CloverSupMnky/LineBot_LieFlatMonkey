using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Modules.Interfaces.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services.Factory
{
    /// <summary>
    /// Line Bot 加入個人聊天室事件處理 Service
    /// </summary>
    public class FollowEventService : IEventFactoryService
    {
        /// <summary>
        /// 處理訊息
        /// </summary>
        /// <param name="webHookEvent">Line Bot 訊息物件</param>
        public void Invoke(WebHookEvent webHookEvent)
        {
            // TODO
        }
    }
}
