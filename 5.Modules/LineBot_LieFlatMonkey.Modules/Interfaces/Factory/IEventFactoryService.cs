using LineBot_LieFlatMonkey.Assets.Model.LineBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Interfaces.Factory
{
    /// <summary>
    /// Line Bot Event 工廠
    /// </summary>
    public interface IEventFactoryService
    {
        /// <summary>
        /// 處理事件
        /// </summary>
        /// <param name="eventInfo">Line Bot Event 物件</param>
        Task Invoke(Event eventInfo);
    }
}
