using LineBot_LieFlatMonkey.Assets.Constant;
using LineBot_LieFlatMonkey.Modules.Interfaces.Factory;
using LineBot_LieFlatMonkey.Modules.Services.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Modules.Factory
{
    /// <summary>
    /// Line Bot 事件工廠
    /// </summary>
    public class EventFactory
    {
        private readonly IEventFactoryService messageEventService;
        private readonly IEventFactoryService followEventService;
        private readonly IEventFactoryService joinEventService;
        private readonly IEventFactoryService postbackEventService;

        public EventFactory(Func<string, IEventFactoryService> serviceProvider)
        {
            messageEventService = serviceProvider(EventType.Message);
            followEventService = serviceProvider(EventType.Follow);
            joinEventService = serviceProvider(EventType.Join);
            postbackEventService = serviceProvider(EventType.Postback);
        }

        /// <summary>
        /// 依事件類別取得對應的處理 Service
        /// </summary>
        /// <param name="type">事件類別</param>
        public IEventFactoryService GetEventService(string type) 
        {
            switch (type)
            {
                case EventType.Message:
                    return this.messageEventService;
                case EventType.Follow: 
                    return this.followEventService;
                case EventType.Join: 
                    return this.joinEventService;
                case EventType.Postback:
                    return this.postbackEventService;
                default:
                    throw new Exception("EventFactory 未處理的 Line Bot 事件類型");
            }
        }
    }
}
