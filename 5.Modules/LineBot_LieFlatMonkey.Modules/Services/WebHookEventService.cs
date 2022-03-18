using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Modules.Factory;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// 處理 Line Bot 傳來訊息 Service
    /// </summary>
    public class WebHookEventService : IWebHookEventService
    {
        private readonly EventFactory eventFactory;
        public WebHookEventService(EventFactory eventFactory)
        {
            this.eventFactory = eventFactory;
        }

        /// <summary>
        /// 處理 Line Bot 訊息
        /// </summary>
        /// <param name="webHookEvent">Line Bot 訊息物件</param>
        public void EventHandler(WebHookEvent webHookEvent)
        {
            if(webHookEvent == null || webHookEvent.Events == null) 
            {
                // TODO 處理錯誤

                return;
            }

            try 
            {
                foreach (Event e in webHookEvent.Events)
                {
                    var eventService = this.eventFactory.GetEventService(e.Type);

                    eventService.Invoke(e);
                }
            }
            catch(Exception ex) 
            {
                // TODO 處理錯誤

                return;
            }
        }
    }
}
