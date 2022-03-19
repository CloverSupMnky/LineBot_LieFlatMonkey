using LineBot_LieFlatMonkey.Assets.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class AudioResultMessage : ResultMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; } = MessageType.Audio;

        [JsonProperty("originalContentUrl")]
        public string OriginalContentUrl { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }
    }
}
