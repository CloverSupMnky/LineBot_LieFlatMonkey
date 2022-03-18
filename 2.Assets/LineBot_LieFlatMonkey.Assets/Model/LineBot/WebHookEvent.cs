using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    /// <summary>
    /// Line Bot 訊息物件
    /// </summary>
    public class WebHookEvent
    {
        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("events")]
        public Event[] Events { get; set; }
    }
}
