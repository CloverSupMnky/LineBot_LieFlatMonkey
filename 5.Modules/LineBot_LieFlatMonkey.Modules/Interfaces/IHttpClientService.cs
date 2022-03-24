using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// HttpClient 方法介面
    /// </summary>
    public interface IHttpClientService
    {
        /// <summary>
        /// 發送 Line Bot Push Message
        /// </summary>
        /// <param name="messages">發送訊息</param>
        /// <returns></returns>
        Task PushMessageAsync(List<ResultMessage> messages);

        /// <summary>
        /// 發送 Line Bot Reply Message
        /// </summary>
        /// <param name="messages">發送訊息</param>
        /// <param name="replyToken">回覆訊息的 replyToken </param>
        /// <returns></returns>
        Task ReplyMessageAsync(List<ResultMessage> messages,string replyToken);

        /// <summary>
        /// 依曲風類型取得音樂推薦列表
        /// </summary>
        /// <param name="musicCate">曲風類型</param>
        /// <returns></returns>
        Task<List<Song>> GetSongInfoByMusicCateType(string musicCate);

        /// <summary>
        /// 查詢地圖取得商家資料
        /// </summary>
        /// <param name="searchWord">查詢文字</param>
        /// <param name="latitude">緯度</param>
        /// <param name="longitude">經度</param>
        /// <param name="radius">查詢距離</param>
        /// <returns></returns>
        Task<SearchMapResp> SearchMapAsync(
            string searchWord, string latitude, string longitude, int radius = 1000);

        /// <summary>
        /// 依 Ptt 看板類別查詢文章
        /// </summary>
        /// <param name="type">看板類別</param>
        /// <returns></returns>
        Task<List<SearchPttResp>> SearchPttByBoardType(string type);

        /// <summary>
        /// 依 UserId 取得使用者資訊
        /// </summary>
        /// <param name="userId">使用者 Id</param>
        /// <returns></returns>
        Task<UserProfile> GetUserProfileByUserIdAsync(string userId);

        /// <summary>
        /// 依群組 Id 取得群組資訊
        /// </summary>
        /// <param name="groupId">群組 Id</param>
        /// <returns></returns>
        Task<GroupInfo> GetGroupInfoByGroupIdAsync(string groupId);
    }
}
