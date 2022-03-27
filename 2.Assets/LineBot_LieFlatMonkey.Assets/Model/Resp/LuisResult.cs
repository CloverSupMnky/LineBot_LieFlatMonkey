using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.Resp
{
    /// <summary>
    /// 語意判斷結果
    /// </summary>
    public class LuisResult
    {
        public string query { get; set; }

        public Prediction prediction { get; set; }
    }

    public class Prediction
    {
        public string topIntent { get; set; }
    }
}
