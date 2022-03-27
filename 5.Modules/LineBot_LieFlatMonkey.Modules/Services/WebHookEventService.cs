using LineBot_LieFlatMonkey.Assets.Constant;
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
        private readonly ICommonService commonService;

        public WebHookEventService(EventFactory eventFactory, ICommonService commonService)
        {
            this.eventFactory = eventFactory;
            this.commonService = commonService;
        }

        /// <summary>
        /// 處理 Line Bot 訊息
        /// </summary>
        /// <param name="webHookEvent">Line Bot 訊息物件</param>
        public async Task EventHandler(WebHookEvent webHookEvent)
        {
            if(webHookEvent == null || webHookEvent.Events == null) 
            {
                await this.commonService.PushErrorMessage(
                    $"{SystemMessageType.SysError}-無 WebHookEvent 資料");

                return;
            }

            try 
            {
                foreach (Event e in webHookEvent.Events)
                {
                    var eventService = this.eventFactory.GetEventService(e.Type);

                    if (eventService == null) continue;

                    await eventService.Invoke(e);
                }
            }
            catch(Exception ex) 
            {
                await this.commonService.PushErrorMessage(
                    $"{SystemMessageType.SysError}-{ex.Message}");

                return;
            }
        }
    }
}
