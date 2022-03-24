using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Entities.Models;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// 共用方法 Service
    /// </summary>
    public class CommonService : ICommonService
    {
        private readonly ITrackableRepository<QuickReply> quickReplyRepo;
        private readonly IHttpClientService httpClientService;

        public CommonService(
            IHttpClientService httpClientService,
            ITrackableRepository<QuickReply> quickReplyRepo)
        {
            this.httpClientService = httpClientService;
            this.quickReplyRepo = quickReplyRepo;
        }

        /// <summary>
        /// 取得亂數值
        /// </summary>
        /// <param name="maxLength">範圍最大值</param>
        /// <returns></returns>
        public int GetRandomNo(int maxLength)
        {
            // 以 Guid 的 HashCode 作為亂數種子
            Random random = new Random(Guid.NewGuid().GetHashCode());

            return random.Next(1, maxLength + 1);
        }

        /// <summary>
        /// 依模板檔案名稱取得對應模板 Json 字串
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMessageTemplateByName(string name)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, DirName.Template, name);

            if (!File.Exists(filePath)) return string.Empty;

            string res = string.Empty;
            using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                res = await streamReader.ReadToEndAsync();
            }

            return res;
        }

        /// <summary>
        /// 依類型取得 QuickReply 資料
        /// </summary>
        /// <param name="type">QuickReply 類型</param>
        /// <returns></returns>
        public List<QuickReply> GetQuickReplyByType(string type)
        {
            return this.quickReplyRepo.Queryable()
                .Where(q => q.ItemType == type)
                .OrderBy(q => q.Sort)
                .ToList();
        }

        /// <summary>
        /// 回傳錯誤訊息-Reply-聊天室
        /// </summary>
        /// <param name="text">回傳訊息</param>
        /// <returns></returns>
        public async Task ReplyErrorMessage(string text,string replyToken)
        {
            var messages = this.GetResultMessage(text);

            await this.httpClientService.ReplyMessageAsync(messages,replyToken);
        }

        /// <summary>
        /// 回傳錯誤訊息-Push 自己
        /// </summary>
        /// <param name="text">回傳訊息</param>
        /// <returns></returns>
        public async Task PushErrorMessage(string text)
        {
            var messages = this.GetResultMessage(text);

            await this.httpClientService.PushMessageAsync(messages);
        }

        /// <summary>
        /// 取得回傳模板
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<ResultMessage> GetResultMessage(string text) 
        {
            return new List<ResultMessage>()
            {
                new TextResultMessage(){ Text = text},
                new StickerResultMessage(){PackageId = "8525" , StickerId = "16581310"}
            };
        }
    }
}
