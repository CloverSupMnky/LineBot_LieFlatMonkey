using AngleSharp;
using AngleSharp.Dom;
using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.AppSetting;
using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// HttpClient 方法 Service
    /// </summary>
    public class HttpClientService : IHttpClientService
    {
        private readonly IOptions<LineBotSetting> lineBotSetting;
        private readonly IOptions<GoogleDriverSetting> googleDriverSetting;

        public HttpClientService(
            IOptions<LineBotSetting> lineBotSetting,
            IOptions<GoogleDriverSetting> googleDriverSetting)
        {
            this.lineBotSetting = lineBotSetting;
            this.googleDriverSetting = googleDriverSetting;
        }

        /// <summary>
        /// 發送 Line Bot Push Message
        /// </summary>
        /// <param name="messages">發送訊息</param>
        /// <returns></returns>
        public async Task PushMessageAsync(List<ResultMessage> messages)
        {
            var reqJson = this.GetReqJson(
                new
                {
                    to = this.lineBotSetting.Value.UserId,
                    messages = messages
                });

            await this.SendAsync(LineBotMessageEndpoint.Push, reqJson);
        }

        /// <summary>
        /// 發送 Line Bot Reply Message
        /// </summary>
        /// <param name="messages">發送訊息</param>
        /// <param name="replyToken">回覆訊息的 replyToken </param>
        /// <returns></returns>
        public async Task ReplyMessageAsync(List<ResultMessage> messages, string replyToken)
        {
            var reqJson = this.GetReqJson(
                new
                {
                    replyToken = replyToken,
                    messages = messages
                });

            await this.SendAsync(LineBotMessageEndpoint.Reply, reqJson);
        }

        /// <summary>
        /// 取得 Request Json
        /// </summary>
        /// <param name="req">Request Object</param>
        /// <returns></returns>
        private string GetReqJson(object req) 
        {
            return JsonConvert.SerializeObject(
                req,
                Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="reqUrl">Line Bot Message Endpoint</param>
        /// <param name="reqJson">Request Json</param>
        /// <returns></returns>
        private async Task SendAsync(string reqUrl,string reqJson)
        {
            var httpReqMsg = new HttpRequestMessage(HttpMethod.Post, reqUrl);

            // Content Type
            httpReqMsg.Content = new StringContent(reqJson, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient()) 
            {
                // Accept Type Header
                // httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                httpClient.DefaultRequestHeaders
                    .Add("Authorization", $"Bearer {this.lineBotSetting.Value.AccessToKen}");

                var resp = await httpClient.SendAsync(httpReqMsg);
            }
        }

        /// <summary>
        /// 依曲風類型取得音樂推薦列表
        /// </summary>
        /// <param name="musicCate">曲風類型</param>
        /// <returns></returns>
        public async Task<List<Song>> GetSongInfoByMusicCateType(string musicCate)
        {
            var now = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");

            var res = new List<Song>();

            try 
            {
                using (var httpClient = new HttpClient())
                {
                    // 增加 User-Agent 標頭
                    httpClient.DefaultRequestHeaders
                        .Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");

                    httpClient.DefaultRequestHeaders
                        .Add("Referer", $"https://kma.kkbox.com/charts/daily/song?cate={musicCate}&lang=tc&terr=tw");

                    string url = $"https://kma.kkbox.com/charts/api/v1/daily?category={musicCate}&date={now}&lang=tc&limit=10&terr=tw&type=song";

                    var responseResult = await httpClient.GetStringAsync(url);

                    if (!string.IsNullOrEmpty(responseResult)) 
                    {
                        var getMusicInfoResp = 
                            JsonConvert.DeserializeObject<GetMusicInfoResp>(responseResult);

                        res = getMusicInfoResp.data.charts.song;
                    }
                }

                return res;
            }
            catch 
            {
                return res;
            }
        }

        /// <summary>
        /// 查詢地圖取得商家資料
        /// </summary>
        /// <param name="searchWord">查詢文字</param>
        /// <param name="latitude">緯度</param>
        /// <param name="longitude">經度</param>
        /// <param name="radius">查詢距離(公尺)</param>
        /// <returns></returns>
        public async Task<SearchMapResp> SearchMapAsync(
            string searchWord, string latitude, string longitude,int radius = 1000)
        {
            var res = new SearchMapResp();
            try
            {
                using (var httpClient = new HttpClient())
                {

                    string url = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?keyword={searchWord}&location={latitude},{longitude}&radius={radius}&key={this.googleDriverSetting.Value.GoogleMap}&opennow=opennow&language=zh-TW";
                    
                    var responseResult = await httpClient.GetStringAsync(url);

                    if (!string.IsNullOrEmpty(responseResult))
                    {
                        res = JsonConvert.DeserializeObject<SearchMapResp>(responseResult);
                    }
                }

                return res;
            }
            catch
            {
                return res;
            }
        }

        /// <summary>
        /// 依 Ptt 看板類別查詢文章
        /// </summary>
        /// <param name="type">看板類別</param>
        /// <returns></returns>
        public async Task<List<SearchPttResp>> SearchPttByBoardType(string type)
        {
            try 
            {
                var config = Configuration.Default;
                var handler = new HttpClientHandler() { UseCookies = true };

                using (var context = BrowsingContext.New(config))
                using (var httpClient = new HttpClient(handler))
                {
                    // 增加 User-Agent 標頭
                    httpClient.DefaultRequestHeaders
                        .Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");

                    // 設定 Cookie
                    httpClient.DefaultRequestHeaders.Add("Cookie", "over18=1");

                    return await this.SearchPttAsync(context,httpClient, type, 5);
                }
            }
            catch 
            {
                return null;
            }
        }

        /// <summary>
        /// 依查詢次數查找 ptt 
        /// </summary>
        /// <param name="context">Browse 執行個體</param>
        /// <param name="httpClient">http 執行個體</param>
        /// <param name="type">看板類型</param>
        /// <param name="count">查詢次數</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<List<SearchPttResp>> SearchPttAsync(
            IBrowsingContext context, HttpClient httpClient, string type, int count)
        {
            var res = new List<SearchPttResp>();

            string baseUrl = $"https://www.ptt.cc/bbs/{type}";

            var url = $"{baseUrl}/index.html";

            IDocument document = await this.GetDocumentAsync(context, httpClient,url);

            if (document == null) return res;

            int pageCount = this.GetDomainPageCount(document);
            if(pageCount == -1) return res;

            res = this.DetectDocument(document);

            for (int i = 1;i<= count; i += 1) 
            {
                url = $"{baseUrl}/index{pageCount -= 1}.html";
                document = await this.GetDocumentAsync(context, httpClient, url);

                if (document == null) continue;

                res.AddRange(this.DetectDocument(document));
            }

            return res;
        }

        /// <summary>
        /// 取得當前頁面 Html
        /// </summary>
        /// <param name="context">Browse 執行個體</param>
        /// <param name="httpClient">http 執行個體</param>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<IDocument> GetDocumentAsync(
            IBrowsingContext context, HttpClient httpClient,string url) 
        {
            string responseResult = await httpClient.GetStringAsync(url);

            if (string.IsNullOrEmpty(responseResult)) return null;

            return await context.OpenAsync(res => res.Content(responseResult));
        }

        /// <summary>
        /// 取得主頁當前頁數
        /// </summary>
        /// <param name="document">Html 類別</param>
        /// <returns></returns>
        private int GetDomainPageCount(IDocument document)
        {
            // 找出 div class = btn-group btn-group-paging 下的第 2 個 a 元素
            var aElement = document.QuerySelector("div.btn-group.btn-group-paging > a:nth-child(2)");

            if (aElement == null) return -1;

            Regex regex = new Regex(@"[\d]+", RegexOptions.IgnoreCase);

            // 篩選網址內前一次的頁數
            var val = regex.Match(aElement.GetAttribute("href")).Value;

            if (string.IsNullOrEmpty(val) || !int.TryParse(val,out int pageNumber)) return -1;

            // 此為前一頁的頁數因此當前頁面需 + 1
            return pageNumber += 1;
        }

        /// <summary>
        /// 查找 HTML 取得需要的資料
        /// </summary>
        /// <param name="document">Html 類別</param>
        /// <returns></returns>
        private List<SearchPttResp> DetectDocument(IDocument document)
        {
            // 找出 div class = r-ent 的元素
            var devElements = document.QuerySelectorAll(".r-ent");

            List<SearchPttResp> res = new List<SearchPttResp>();
            SearchPttResp searchPttResp;
            foreach (var devElement in devElements)
            {
                // 找出 div class = nrec 下的 span 
                var spanElement = devElement.QuerySelector("div.nrec > span");

                // 若查無 span 或 內容值不是爆，且轉換數字失敗中斷執行
                if(spanElement == null || 
                  (spanElement.TextContent != "爆" && !int.TryParse(spanElement.TextContent, out int count)))
                    continue;

                // 找出 div class = title 下的 a 
                var aElement = devElement.QuerySelector("div.title > a");

                // 若查無 a 或 內容值包含 [公告] 就中斷執行
                if (aElement == null || aElement.TextContent.IndexOf("[公告]") != -1) 
                    continue;

                searchPttResp = new SearchPttResp();

                searchPttResp.Title = aElement.TextContent;
                searchPttResp.ThumbsUp = spanElement.TextContent;
                searchPttResp.PttLink = $"https://www.ptt.cc{aElement.GetAttribute("href")}";

                res.Add(searchPttResp);
            }

            return res;
        }
    }
}
