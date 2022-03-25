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
        private readonly ISearchPttService searchPttService;
        private readonly ICommonService commonService;
        private readonly ITarotCardService tarotCardService;
        private readonly IMusicRecommandService musicRecommandService;

        public PostbackEventService(
            IHttpClientService httpClientService, 
            ISearchMapService searchMapService,
            ISearchPttService searchPttService,
            ICommonService commonService,
            ITarotCardService tarotCardService,
            IMusicRecommandService musicRecommandService)
        {
            this.httpClientService = httpClientService;
            this.searchMapService = searchMapService;
            this.commonService = commonService;
            this.searchPttService = searchPttService;
            this.tarotCardService = tarotCardService;
            this.musicRecommandService = musicRecommandService;
        }

        public async Task Invoke(Event eventInfo)
        {
            var messages = await this.GetPostbackMessages(eventInfo.Postback.Data);

            if(messages == null) 
            {
                await this.commonService.ReplyErrorMessage(SystemMessageType.NoData, eventInfo.ReplyToken);

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
                case QuickReplyType.SearchPTT:
                    return await this.GetSearchPttMessages(query);
                case QuickReplyType.TarotCard:
                    return await this.GetTarotCardMessages();
                case QuickReplyType.MusicRecommand:
                    return await this.GetMusicRecommandMessages(query);
                default:
                    return this.commonService.GetResultMessage(SystemMessageType.NoData);
            }
        }

        /// <summary>
        /// 取得音樂推薦模板訊息
        /// </summary>
        /// <param name="query">查詢物件</param>
        /// <returns></returns>
        private async Task<List<ResultMessage>> GetMusicRecommandMessages(NameValueCollection query)
        {
            var musicCateType = query[QueryStringPropertyType.Word];

            var musicRecommands = await this.musicRecommandService.RecommandByMusicCateType(musicCateType);

            string jsonString =
                await this.commonService.GetMessageTemplateByName("MusicRecommandTemplate.json");

            if (string.IsNullOrEmpty(jsonString)) 
                return this.commonService.GetResultMessage(SystemMessageType.NoData);

            string tempString = string.Empty;
            List<object> objs = new List<object>();
            object obj;
            foreach (var musicRecommand in musicRecommands) 
            {
                tempString = jsonString;

                tempString = tempString.Replace("{#ImageUrl}", musicRecommand.ImageUrl);
                tempString = tempString.Replace("{#Artist}", musicRecommand.Artist);
                tempString = tempString.Replace("{#SongType}", musicRecommand.SongType);
                tempString = tempString.Replace("{#Song}", musicRecommand.Song);
                tempString = tempString.Replace("{#VideoUrl}", musicRecommand.VideoUrl);

                obj = JsonConvert.DeserializeObject<object>(tempString);

                objs.Add(obj);
            }

            if (objs.Count == 0)
            {
                return this.commonService.GetResultMessage(SystemMessageType.NoData);
            }

            var carouselResultMessage = new CarouselResultMessage();
            carouselResultMessage.Contents = objs;

            return new List<ResultMessage>()
                {
                    new FlexResultMessage(){ Contents = carouselResultMessage ,AltText = "音樂推薦"}
                };
        }

        /// <summary>
        /// 取得塔羅牌直覺運勢占卜結果訊息
        /// </summary>
        /// <returns></returns>
        private async Task<List<ResultMessage>> GetTarotCardMessages()
        {
            var tarotCard =
                this.tarotCardService.FortuneTellingByType(FortuneTellingType.Normal);

            string jsonString =
                await this.commonService.GetMessageTemplateByName("TarotCardTemplate.json");

            if (string.IsNullOrEmpty(jsonString)) 
                return this.commonService.GetResultMessage(SystemMessageType.NoData);

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

        /// <summary>
        /// 取得 Ptt 熱門文章模板訊息
        /// </summary>
        /// <param name="query">查詢物件</param>
        /// <returns></returns>
        private async Task<List<ResultMessage>> GetSearchPttMessages(
            NameValueCollection query)
        {
            try 
            {
                var pttInfos =
                    await this.searchPttService.SearchPttByBoardType(query[QueryStringPropertyType.Word]);

                var carouselResultMessage = new CarouselResultMessage();
                carouselResultMessage.Contents = await this.GetSearchPttResContentMessage(pttInfos);

                if (carouselResultMessage.Contents.Count == 0)
                {
                    return this.commonService.GetResultMessage(SystemMessageType.NoData);
                }

                return new List<ResultMessage>()
                {
                    new FlexResultMessage(){ Contents = carouselResultMessage ,AltText = "熱門文章推薦"}
                };
            }
            catch 
            {
                return this.commonService.GetResultMessage(SystemMessageType.NoData);
            }
        }

        /// <summary>
        /// 取得熱門文章查詢結果內容模板訊息
        /// </summary>
        /// <param name="searchRes">熱門文章推薦結果</param>
        /// <returns></returns>
        private async Task<List<object>> GetSearchPttResContentMessage(List<SearchPttResp> pttInfos)
        {
            string jsonString =
                await this.commonService.GetMessageTemplateByName("SearchPttTemplate.json");

            if (string.IsNullOrEmpty(jsonString)) return null;

            var tempString = string.Empty;
            var res = new List<object>();
            object obj = null;
            foreach (var pttInfo in pttInfos)
            {
                tempString = jsonString;

                tempString = tempString.Replace("{#Title}", pttInfo.Title);
                tempString = tempString.Replace("{#ThumbsUp}", pttInfo.ThumbsUp);
                tempString = tempString.Replace("{#ArticleUrl}", pttInfo.ArticleUrl);

                obj = JsonConvert.DeserializeObject<object>(tempString);

                res.Add(obj);
            }

            return res;
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
                    return this.commonService.GetResultMessage(SystemMessageType.NoData);
                }

                return new List<ResultMessage>()
                {
                    new FlexResultMessage(){ Contents = carouselResultMessage ,AltText = "地圖探索結果"}
                };
            }
            catch 
            {
                return this.commonService.GetResultMessage(SystemMessageType.NoData);
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
