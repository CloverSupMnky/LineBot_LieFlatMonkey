using LineBot_LieFlatMonkey.Assets.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class QuickReplyItem
    {
        [JsonProperty("type")]
        public string Type { get; set; } = ActionType.Base;

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("action")]
        public QuickReplyAction Action { get; set; }
    }
}
