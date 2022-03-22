using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class PostbackParams
    {
        [JsonProperty("newRichMenuAliasId")]
        public string NewRichMenuAliasId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("datetime")]
        public string Datetime { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }
    }
}
