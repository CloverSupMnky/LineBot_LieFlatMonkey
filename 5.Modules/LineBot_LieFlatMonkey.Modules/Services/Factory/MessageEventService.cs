using LineBot_LieFlatMonkey.Assets.Constant;
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
    /// Line Bot 訊息事件處理 Service
    /// </summary>
    public class MessageEventService : IEventFactoryService
    {
        /// <summary>
        /// 處理訊息
        /// </summary>
        /// <param name="eventInfo">Line Bot Event 物件</param>
        public void Invoke(Event eventInfo)
        {
            switch (eventInfo.Message.Type) 
            {
                case MessageType.Text:
                    this.TextMessage(eventInfo.Message);
                    break;
            }
        }

        /// <summary>
        /// 文字訊息
        /// </summary>
        /// <param name="message">Line Bot Message 物件</param>
        private void TextMessage(Message message)
        {
            switch (message.Text)
            {
                case TextMessageType.TarotCardDaily:
                    break;
                case TextMessageType.TarotCardNormal:
                    break;
            }
        }
    }
}
