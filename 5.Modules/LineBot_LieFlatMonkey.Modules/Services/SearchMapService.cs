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
    /// 探索地圖功能 Service
    /// </summary>
    public class SearchMapService : ISearchMapService
    {
        private readonly IHttpClientService httpClientService;

        public SearchMapService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        /// <summary>
        /// 查詢地圖取得商家資料
        /// </summary>
        /// <param name="searchWord">查詢文字</param>
        /// <param name="latitude">緯度</param>
        /// <param name="longitude">經度</param>
        public async Task<SearchMapResp> SearchMap(string searchWord, string latitude, string longitude)
        {
            return await this.httpClientService.SearchMapAsync(searchWord, latitude, longitude);
        }
    }
}
