using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces
{
    /// <summary>
    /// 共用方法介面
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// 取得亂數值
        /// </summary>
        /// <param name="maxLength">範圍最大值</param>
        /// <returns></returns>
        int GetRandomNo(int maxLength);

        /// <summary>
        /// 依模板檔案名稱取得對應模板 Json 字串
        /// </summary>
        /// <returns></returns>
        Task<string> GetMessageTemplateByName(string name);
    }
}
