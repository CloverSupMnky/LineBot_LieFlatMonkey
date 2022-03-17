using LineBot_LieFlatMonkey.Assets.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class Event
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("Timestamp")]
        public long timestamp { get; set; }

        [JsonProperty("source")]
        public Source Source { get; set; }

        [JsonProperty("replyToken")]
        public string ReplyToken { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }
    }
}
