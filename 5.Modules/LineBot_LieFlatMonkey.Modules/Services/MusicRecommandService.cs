using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// 音樂推薦 Service
    /// </summary>
    public class MusicRecommandService : IMusicRecommandService
    {
        private readonly IHttpClientService httpClientService;

        public MusicRecommandService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        /// <summary>
        /// 推薦音樂
        /// </summary>
        public async Task<MusicRecommandResp> Recommand()
        {
            var res = new MusicRecommandResp();

            return res;
        }
    }
}
