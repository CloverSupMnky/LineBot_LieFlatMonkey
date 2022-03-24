using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// 使用者功能介面
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 依使用者 Id 取得使用者資訊
        /// </summary>
        /// <param name="userId">使用者 Id</param>
        /// <returns></returns>
        Task<UserProfile> GetUserProfileByUserId(string userId);

        /// <summary>
        /// 依群組 Id 取得群組資訊
        /// </summary>
        /// <param name="groupId">群組 Id</param>
        /// <returns></returns>
        Task<GroupInfo> GetGroupInfoByGroupId(string groupId);
    }
}
