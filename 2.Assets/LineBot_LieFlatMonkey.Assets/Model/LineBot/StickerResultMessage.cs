using LineBot_LieFlatMonkey.Assets.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class StickerResultMessage : ResultMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; } = MessageType.Sticker;

        [JsonProperty("packageId")]
        public string PackageId { get; set; }

        [JsonProperty("stickerId")]
        public string StickerId { get; set; }
    }
}
