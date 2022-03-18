using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class Emoji
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("emojiId")]
        public string EmojiId { get; set; }
    }
}
