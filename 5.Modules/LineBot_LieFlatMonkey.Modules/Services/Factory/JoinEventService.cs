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

namespace LineBot_LieFlatMonkey.Modules.Services.Factory
{
    /// <summary>
    /// Line Bot 加入群組聊天室事件處理 Service
    /// </summary>
    public class JoinEventService : IEventFactoryService
    {
        private readonly IUserService userService;
        private readonly ICommonService commonService;
        private readonly IHttpClientService httpClientService;

        public JoinEventService(
            IUserService userService, 
            ICommonService commonService,
            IHttpClientService httpClientService)
        {
            this.userService = userService;
            this.commonService = commonService;
            this.httpClientService = httpClientService;
        }

        /// <summary>
        /// 處理訊息
        /// </summary>
        /// <param name="eventInfo">Line Bot Event 物件</param>
        public async Task Invoke(Event eventInfo)
        {
            var text = string.Empty;

            switch (eventInfo.Source.Type) 
            {
                case SourceType.User:
                    text = await this.GetUserName(eventInfo.Source.UserId);
                    break;
                case SourceType.Room:
                case SourceType.Group:
                    text = await this.GetGroupName(eventInfo.Source.GroupId);
                    break;
            }

            await this.SendMessage(text, eventInfo.ReplyToken);
        }

        /// <summary>
        /// 取得群組名稱
        /// </summary>
        /// <param name="groupId">群組 Id</param>
        /// <returns></returns>
        private async Task<string> GetGroupName(string groupId)
        {
            var groupInfo = await this.userService.GetGroupInfoByGroupId(groupId);

            var groupName = string.Empty;

            if (groupInfo != null)
            {
                groupName = groupInfo.GroupName;
            }

            return $"{groupName} 的大家好呀!!";
        }

        /// <summary>
        /// 取得使用者名稱
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetUserName(string userId) 
        {
            var userProfile = await this.userService.GetUserProfileByUserId(userId);

            var userName = string.Empty;
            if (userProfile != null) 
            {
                userName = userProfile.DisplayName;
            }

            return $"{userName} 您好呀!!";
        }

        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="name">群組/使用者名稱</param>
        /// <returns></returns>
        private async Task SendMessage(string name, string replyToken) 
        {
            string jsonString =
                await this.commonService.GetMessageTemplateByName("JoinTemplate.json");

            if (string.IsNullOrEmpty(jsonString)) return;

            jsonString = jsonString.Replace("{#UserName}", name);

            var obj = JsonConvert.DeserializeObject<object>(jsonString);

            var messages = new List<ResultMessage>()
            {
                new FlexResultMessage(){ Contents = obj ,AltText = "歡迎加入 『猴子の日常』"},
                new StickerResultMessage(){ StickerId = "16581296", PackageId = "8525"}
            };

            await this.httpClientService.ReplyMessageAsync(messages, replyToken);
        }
    }
}
