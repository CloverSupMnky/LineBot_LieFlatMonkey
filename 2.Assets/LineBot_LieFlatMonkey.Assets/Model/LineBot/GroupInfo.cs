using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.LineBot
{
    public class GroupInfo
    {
        [JsonProperty("groupId")]
        public string GroupId { get; set; }

        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        [JsonProperty("pictureUrl")]
        public string PictureUrl { get; set; }
    }
}
