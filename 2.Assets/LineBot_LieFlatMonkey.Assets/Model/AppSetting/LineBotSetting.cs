using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.AppSetting
{
    public class LineBotSetting
    {
        /// <summary>
        /// Channel access token
        /// </summary>
        public string AccesstoKen { get; set; }

        /// <summary>
        /// A unique secret key
        /// </summary>
        public string ChannelSecret { get; set; }

        /// <summary>
        /// LINE account's user ID
        /// </summary>
        public string UserId { get; set; }
    }
}
