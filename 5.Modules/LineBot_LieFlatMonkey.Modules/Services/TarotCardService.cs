using LineBot_LieFlatMonkey.Entities.Models;
using LineBot_LieFlatMonkey.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;

namespace LineBot_LieFlatMonkey.Modules.Services
{
    /// <summary>
    /// 塔羅牌相關功能 Service
    /// </summary>
    public class TarotCardService : ITarotCardService
    {
        private readonly ITrackableRepository<TarotCard> tarotCardRepo;

        public TarotCardService(ITrackableRepository<TarotCard> tarotCardRepo)
        {
            this.tarotCardRepo = tarotCardRepo;
        }
    }
}
