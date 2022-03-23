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
        public async Task SearchPttByBoardType(string type)
        {
           
        }
    }
}
