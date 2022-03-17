using LineBot_LieFlatMonkey.Assets.Constant;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class Source
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
