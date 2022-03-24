using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientService httpClientService;

        public UserService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        /// <summary>
        /// 依使用者 Id 取得使用者資訊
        /// </summary>
        /// <param name="userId">使用者 Id</param>
        /// <returns></returns>
        public async Task<UserProfile> GetUserProfileByUserId(string userId)
        {
            return await this.httpClientService.GetUserProfileByUserIdAsync(userId);
        }
    }
}
