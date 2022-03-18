using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Constant
{
    /// <summary>
    /// Line Bot Message 各發送 Endpoint
    /// </summary>
    public class LineBotMessageEndpoint
    {
        public const string Push = @"https://api.line.me/v2/bot/message/push";

        public const string Reply = @"https://api.line.me/v2/bot/message/reply";
    }
}
