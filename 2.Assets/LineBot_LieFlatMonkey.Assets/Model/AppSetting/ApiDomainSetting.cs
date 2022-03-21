using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.AppSetting
{
    /// <summary>
    /// API 路由相關設定
    /// </summary>
    public class ApiDomainSetting
    {
        /// <summary>
        /// 發佈位置
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// 開發位置
        /// </summary>
        public string Develop { get; set; }
    }
}
