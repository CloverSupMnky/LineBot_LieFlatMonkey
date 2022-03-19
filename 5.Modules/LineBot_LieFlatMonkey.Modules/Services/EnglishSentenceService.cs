using LineBot_LieFlatMonkey.Assets.Model.Resp;
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
    /// 英文句子產出 Service
    /// </summary>
    public class EnglishSentenceService : IEnglishSentenceService
    {
        private readonly ITrackableRepository<EnglishSentence> englishSentenceRepo;

        public EnglishSentenceService(ITrackableRepository<EnglishSentence> englishSentenceRepo)
        {
            this.englishSentenceRepo = englishSentenceRepo;
        }

        /// <summary>
        /// 取得英文句子
        /// </summary>
        /// <returns></returns>
        public EnglishSentenceResp GetSentence()
        {
            var sentenceNo = this.GetSentenceNo();

            var englishSentence = this.englishSentenceRepo.Queryable()
                .FirstOrDefault(e => e.SeqNo == sentenceNo);

            var res = new EnglishSentenceResp();

            if(englishSentence != null)
            {
                res.Sentence = englishSentence.Sentence;
                res.Translation = englishSentence.Translation;
                res.Source = englishSentence.Source;
                res.SourceType = englishSentence.SourceType;
            }

            return res;
        }

        /// <summary>
        /// 隨機取得一個英文句子的編號
        /// </summary>
        /// <returns></returns>
        private int GetSentenceNo()
        {
           var totalCount = this.englishSentenceRepo.Queryable().Count();

            Random random = new Random(Guid.NewGuid().GetHashCode());

            return random.Next(1,totalCount + 1);
        }
    }
}
