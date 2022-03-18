using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class QuickreplyItem
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("action")]
        public QuickreplyAction Action { get; set; }
    }
}
