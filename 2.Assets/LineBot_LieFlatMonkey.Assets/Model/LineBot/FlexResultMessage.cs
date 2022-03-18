using LineBot_LieFlatMonkey.Assets.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class FlexResultMessage : ResultMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; } = MessageType.Flex;

        [JsonProperty("altText")]
        public string AltText { get; set; } = "This is Flex Message";


        [JsonProperty("contents")]
        public object Contents { get; set; }
    }
}
