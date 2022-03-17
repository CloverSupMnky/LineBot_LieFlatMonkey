using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Model.Resp
{
    public class BaseResp<T>
    {
        /// <summary>
        /// 回傳資料
        /// </summary>
        public T Data { get; set; }
    }
}
