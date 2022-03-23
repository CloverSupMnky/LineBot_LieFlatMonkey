using LineBot_LieFlatMonkey.Assets.Model.Resp;
using LineBot_LieFlatMonkey.Modules.Comparer;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// 查詢 Ptt Service
    /// </summary>
    public class SearchPttService : ISearchPttService
    {
        private readonly IHttpClientService httpClientService;

        public SearchPttService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        /// <summary>
        /// 依看板類別查詢文章
        /// </summary>
        /// <param name="type">看板類別</param>
        /// <returns></returns>
        public async Task<List<SearchPttResp>> SearchPttByBoardType(string type)
        {

            var searchPttResps = await this.httpClientService.SearchPttByBoardType(type);

            searchPttResps.Sort(new SearchPttResComparer());

            searchPttResps = searchPttResps.Take(10).ToList();

            return searchPttResps;
        }
    }
}
