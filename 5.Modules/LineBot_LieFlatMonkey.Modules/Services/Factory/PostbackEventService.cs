using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using LineBot_LieFlatMonkey.Modules.Interfaces.Factory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace LineBot_LieFlatMonkey.Modules.Services.Factory
{
    /// <summary>
    /// Line Bot Postback 事件處理 Service
    /// </summary>
    public class PostbackEventService : IEventFactoryService
    {
        private readonly IHttpClientService httpClientService;
        private readonly ISearchMapService searchMapService; 
        private readonly ICommonService commonService;

        public PostbackEventService(
            IHttpClientService httpClientService, 
            ISearchMapService searchMapService,
            ICommonService commonService)
        {
            this.httpClientService = httpClientService;
            this.searchMapService = searchMapService;
            this.commonService = commonService;
        }

        public async Task Invoke(Event eventInfo)
        {
            var messages = await this.GetPostbackMessages(eventInfo.Postback.Data);

            if(messages == null) 
            {
                // TODO 處理
                return;
            }

            await this.httpClientService.ReplyMessageAsync(messages,eventInfo.ReplyToken);
        }

        /// <summary>
        /// 取得對應 Postback 類型模板訊息
        /// </summary>
        /// <param name="queryString">查詢字串</param>
        /// <returns></returns>
        private async Task<List<ResultMessage>> GetPostbackMessages(string queryString) 
        {
            var query = HttpUtility.ParseQueryString(queryString);

            var type = query[QueryStringPropertyType.Type];

            switch (type)
            {
                case QuickReplyType.SearchMap:
                    return await this.GetSearchMapMessages(query);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 取得地圖探索模板訊息
        /// </summary>
        /// <param name="query">查詢物件</param>
        /// <returns></returns>
        private async Task<List<ResultMessage>> GetSearchMapMessages(
            NameValueCollection query)
        {
            try 
            {
                var mapInfos = await this.searchMapService.SearchMap(
                        query[QueryStringPropertyType.Word],
                        query[QueryStringPropertyType.Latitude],
                        query[QueryStringPropertyType.Longitude]);

                var carouselResultMessage = new CarouselResultMessage();
                carouselResultMessage.Contents = await this.GetSearchMapResContentMessage(mapInfos);

                if (carouselResultMessage.Contents.Count == 0)
                {
                    return null;
                }

                return new List<ResultMessage>()
                {
                    new FlexResultMessage(){ Contents = carouselResultMessage ,AltText = "地圖探索結果"}
                };
            }
            catch 
            {
                return null;
            }
        }

        /// <summary>
        /// 取得探索地圖查詢結果內容模板訊息
        /// </summary>
        /// <param name="searchRes">探索地圖結果</param>
        /// <returns></returns>
        private async Task<List<object>> GetSearchMapResContentMessage(List<MapInfoResp> mapInfos)
        {
            string jsonString =
                await this.commonService.GetMessageTemplateByName("SearchMapTemplate.json");

            if (string.IsNullOrEmpty(jsonString)) return null;

            var tempString = string.Empty;
            var res = new List<object>();
            object obj = null;
            Regex regex = new Regex("{#MapUrl}");
            foreach (var mapInfo in mapInfos)
            {
                tempString = jsonString;

                tempString = tempString.Replace("{#ImageUrl}", mapInfo.ImageUrl);
                tempString = tempString.Replace("{#StoreName}", mapInfo.StoreName);
                tempString = tempString.Replace("{#Rating}", mapInfo.Rating);
                tempString = tempString.Replace("{#Address}", mapInfo.Address);
                tempString = regex.Replace(tempString, mapInfo.MapUrl);

                obj = JsonConvert.DeserializeObject<object>(tempString);

                res.Add(obj);
            }

            return res;
        }
    }
}
