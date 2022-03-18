using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class Quickreply
    {
        [JsonProperty("items")]
        public QuickreplyItem[] Items { get; set; }
    }
}
