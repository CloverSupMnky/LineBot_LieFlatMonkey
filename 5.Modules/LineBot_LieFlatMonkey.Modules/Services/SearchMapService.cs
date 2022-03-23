using LineBot_LieFlatMonkey.Assets.Model.AppSetting;
using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// 探索地圖功能 Service
    /// </summary>
    public class SearchMapService : ISearchMapService
    {
        private readonly IHttpClientService httpClientService;
        private readonly IOptions<GoogleDriverSetting> googleDriverSetting;

        public SearchMapService(
            IHttpClientService httpClientService, 
            IOptions<GoogleDriverSetting> googleDriverSetting)
        {
            this.httpClientService = httpClientService;
            this.googleDriverSetting = googleDriverSetting;
        }

        /// <summary>
        /// 查詢地圖取得商家資料
        /// </summary>
        /// <param name="searchWord">查詢文字</param>
        /// <param name="latitude">緯度</param>
        /// <param name="longitude">經度</param>
        public async Task<List<MapInfoResp>> SearchMap(
            string searchWord, string latitude, string longitude)
        {
            var searchRes = await this.httpClientService.SearchMapAsync(searchWord, latitude, longitude);

            // 取得 5 筆查詢結果
            var res = searchRes.results
                .OrderByDescending(r => r.rating)
                .Where(r => r.photos != null && r.photos.Length > 0)
                .Take(5)
                .Select(r => this.ParseMapInfo(r))
                .ToList();

            return res;
        }


        /// <summary>
        /// 轉換回傳用地圖資訊
        /// </summary>
        /// <param name="r">Google Map 查詢結果</param>
        /// <returns></returns>
        private MapInfoResp ParseMapInfo(Result searchRes)
        {
            var res = new MapInfoResp();

            res.StoreName = searchRes.name;
            res.Address = searchRes.vicinity;
            res.Rating = searchRes.rating.ToString();

            res.MapUrl =$"https://www.google.com/maps/search/?api=1&query={HttpUtility.UrlEncode(searchRes.vicinity)}&query_palce_id={searchRes.place_id}";

            if (searchRes.photos != null && searchRes.photos.Length > 0) 
            {
                res.ImageUrl = $"https://maps.googleapis.com/maps/api/place/photo?photo_reference={searchRes.photos[0].photo_reference}&maxwidth=400&key={this.googleDriverSetting.Value.GoogleMap}";
            }

            return res;
        }
    }
}
