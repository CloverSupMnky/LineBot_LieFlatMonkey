using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using LineBot_LieFlatMonkey.Modules.Interfaces.Factory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public PostbackEventService(
            IHttpClientService httpClientService, 
            ISearchMapService searchMapService)
        {
            this.httpClientService = httpClientService;
            this.searchMapService = searchMapService;
        }

        public async Task Invoke(Event eventInfo)
        {
            var messages = await this.SearchMapRes(eventInfo.Postback.Data);

            await this.httpClientService.ReplyMessageAsync(messages,eventInfo.ReplyToken);
        }

        /// <summary>
        /// 取得探索地圖後回傳模板訊息
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        private async Task<List<ResultMessage>> SearchMapRes(string queryString) 
        {
            try 
            {
                var query = HttpUtility.ParseQueryString(queryString);

                var searchRes = await this.searchMapService.SearchMap(
                    query[QueryStringType.Word], 
                    query[QueryStringType.Latitude], 
                    query[QueryStringType.Longitude]);

                return null;
            }
            catch 
            {
                return null;
            }
        }
    }
}
