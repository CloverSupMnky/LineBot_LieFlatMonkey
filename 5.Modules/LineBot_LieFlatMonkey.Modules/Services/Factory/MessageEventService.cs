using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using LineBot_LieFlatMonkey.Modules.Interfaces.Factory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IHttpClientService httpClientService;
        private readonly ITarotCardService tarotCardService;

        public MessageEventService(
            IHttpClientService httpClientService,
            ITarotCardService tarotCardService)
        {
            this.httpClientService = httpClientService;
            this.tarotCardService = tarotCardService;
        }

        /// <summary>
        /// 處理訊息
        /// </summary>
        /// <param name="eventInfo">Line Bot Event 物件</param>
        public async Task Invoke(Event eventInfo)
        {
            switch (eventInfo.Message.Type) 
            {
                case MessageType.Text:
                    await this.TextMessage(eventInfo.Message,eventInfo.ReplyToken);
                    break;
            }
        }

        /// <summary>
        /// 文字訊息
        /// </summary>
        /// <param name="message">Line Bot Message 物件</param>
        private async Task TextMessage(Message message,string replyToken)
        {
            List<ResultMessage> messages = null;

            switch (message.Text)
            {
                case TextMessageType.TarotCardDaily:
                    messages = await this.TarotCardMessage(FortuneTellingType.Daily);
                    break;
                case TextMessageType.TarotCardNormal:
                    messages = await this.TarotCardMessage(FortuneTellingType.Normal);
                    break;
            }

            if(messages == null) 
            {
                // TODO 錯誤處理
                return;
            }

            await this.httpClientService.ReplyMessageAsync(messages, replyToken);
        }

        /// <summary>
        /// 塔羅牌訊息處理
        /// </summary>
        /// <param name="fortuneTellingType">塔羅牌占卜方式</param>
        /// <returns></returns>
        private async Task<List<ResultMessage>> TarotCardMessage(string fortuneTellingType) 
        {
            var filePath = Path.Combine(
                Environment.CurrentDirectory, "Template", "TarotCardTemplate.json");

            if (!File.Exists(filePath)) return null;

            var tarotCard =
                this.tarotCardService.FortuneTellingByType(fortuneTellingType);

            string jsonString = String.Empty;
            using (var streamReader = new StreamReader(filePath,Encoding.UTF8))
            {
                jsonString = await streamReader.ReadToEndAsync();
            }

            jsonString = jsonString.Replace("{#ImageUrl}", tarotCard.ImageUrl);
            jsonString = jsonString.Replace("{#Name}", tarotCard.Name);
            jsonString = jsonString.Replace("{#NameEN}", tarotCard.NameEn);
            jsonString = jsonString.Replace("{#FaceType}", tarotCard.FaceType);
            jsonString = jsonString.Replace("{#Mean}", tarotCard.Mean);
            jsonString = jsonString.Replace("{#MeanWord}", tarotCard.MeanWord);
            jsonString = jsonString.Replace("{#Desc}", tarotCard.Desc);

            var obj = JsonConvert.DeserializeObject<object>(jsonString);

            return new List<ResultMessage>()
            {
                new FlexResultMessage(){ Contents = obj ,AltText = "運勢占卜結果"}
            };
        }
    }
}
