using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineBot_LieFlatMonkey.Assets.Constant;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class TextResultMessage : BaseResultMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; } = MessageType.Text;

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("emojis")]
        public Emoji[] Emojis { get; set; }
    }
}
