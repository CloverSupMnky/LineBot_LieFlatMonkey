using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using LineBot_LieFlatMonkey.Modules.Interfaces.Factory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services.Factory
{
    /// <summary>
    /// Line Bot Postback 事件處理 Service
    /// </summary>
    public class PostbackEventService : IEventFactoryService
    {
        private readonly IHttpClientService httpClientService;

        public PostbackEventService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task Invoke(Event eventInfo)
        {
            var messages = new List<ResultMessage>()
            {
                new TextResultMessage(){ Text = JsonConvert.SerializeObject(eventInfo.Postback)}
            };

            await this.httpClientService.ReplyMessageAsync(messages,eventInfo.ReplyToken);
        }
    }
}
