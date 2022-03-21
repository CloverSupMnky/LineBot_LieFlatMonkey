using LineBot_LieFlatMonkey.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// 共用方法 Service
    /// </summary>
    public class CommonService : ICommonService
    {
        /// <summary>
        /// 取得亂數值
        /// </summary>
        /// <param name="maxLength">範圍最大值</param>
        /// <returns></returns>
        public int GetRandomNo(int maxLength)
        {
            // 以 Guid 的 HashCode 作為亂數種子
            Random random = new Random(Guid.NewGuid().GetHashCode());

            return random.Next(1, maxLength + 1);
        }
    }
}
