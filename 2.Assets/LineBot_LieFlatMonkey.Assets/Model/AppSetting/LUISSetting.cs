using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.AppSetting
{
    /// <summary>
    /// 意圖判斷設定
    /// </summary>
    public class LUISSetting
    {
        /// <summary>
        /// App ID
        /// </summary>
        public string AppID { get; set; }

        /// <summary>
        /// Endpoint URL
        /// </summary>
        public string EndpointURL { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Primary Key
        /// </summary>
        public string PrimaryKey { get; set; }
    }
}
