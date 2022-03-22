using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class Postback
    {
        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("params")]
        public PostbackParams Params { get; set; }
    }
}
