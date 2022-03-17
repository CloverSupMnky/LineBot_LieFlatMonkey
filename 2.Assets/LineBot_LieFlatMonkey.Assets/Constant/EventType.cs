using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineBot_LieFlatMonkey.Assets.Constant
{
    public class EventType
    {
        /// <summary>
        /// Webhook event object which contains the sent message. The message property contains 
        /// a message object which corresponds with the message type. You can reply to message 
        /// events.
        /// </summary>
        public const string Message = "message";

        /// <summary>
        /// Event object for when your LINE Official Account is added as a friend (or unblocked).
        /// You can reply to follow events.
        /// </summary>
        public const string Follow = "follow";

        /// <summary>
        /// Event object for when your LINE Official Account joins a group chat or multi-person 
        /// chat. You can reply to join events.
        /// </summary>
        public const string Join = "join";
    }
}
