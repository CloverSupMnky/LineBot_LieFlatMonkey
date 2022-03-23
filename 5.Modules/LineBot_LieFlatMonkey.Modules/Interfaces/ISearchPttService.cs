using LineBot_LieFlatMonkey.Assets.Model.Resp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// 查詢 Ptt 介面
    /// </summary>
    public interface ISearchPttService
    {
        /// <summary>
        /// 依看板類別查詢文章
        /// </summary>
        /// <param name="type">看板類別</param>
        /// <returns></returns>
        Task<List<SearchPttResp>> SearchPttByBoardType(string type);
    }
}
