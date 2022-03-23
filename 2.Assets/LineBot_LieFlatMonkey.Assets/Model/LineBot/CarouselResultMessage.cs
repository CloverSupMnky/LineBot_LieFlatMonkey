using LineBot_LieFlatMonkey.Assets.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class CarouselResultMessage
    {
        [JsonProperty("type")]
        public string Type { get; set; } = MessageType.Carousel;

        [JsonProperty("contents")]
        public List<object> Contents { get; set; }
    }
}
