﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class BaseResultMessage
    {
        [JsonProperty("quickReply")]
        public Quickreply QuickReply { get; set; }
    }
}
