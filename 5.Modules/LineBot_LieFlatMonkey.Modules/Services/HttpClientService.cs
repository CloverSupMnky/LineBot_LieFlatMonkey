using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.AppSetting;
using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
        private readonly HttpClient httpClient;
        private readonly IOptions<LineBotSetting> lineBotSetting;

        public HttpClientService(IOptions<LineBotSetting> lineBotSetting)
        {
            httpClient = new HttpClient();

            this.lineBotSetting = lineBotSetting;

            // Accept Type Header
            // httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            httpClient.DefaultRequestHeaders
                .Add("Authorization", $"Bearer {this.lineBotSetting.Value.AccesstoKen}");
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

            var resp = await this.httpClient.SendAsync(httpReqMsg);
        }
    }
}
